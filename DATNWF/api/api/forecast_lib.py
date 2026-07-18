"""forecast_lib.py - Mo hinh du bao chuan (ARIMA(1,1,1) vs GradientBoosting lag-5,
clip 99th, rolling-CV chon tot nhat) cho du lieu (MaKhachHang, MaBao) per-period.

Chuyen tu train_forecast_run2.ipynb (Phase 1-4). Du doan CA 2 truong:
  - TongSoLuongBanThucTe
  - SoLuongPhatHanhTrongThucTe
Moi cap (kh, bao) va moi truong duoc huan luyen doc lap.
Khong dung auto_ts (can shim, statsmodels broken) -> code sach, deterministic, chay duoc khong shim.

Nam trong api/. Duong dan du lieu/output lay tu paths.py (configurable qua env FORECAST_DIR).
"""
import warnings
warnings.filterwarnings("ignore")
import numpy as np
from scipy.optimize import minimize
import pandas as pd
from sklearn.ensemble import GradientBoostingRegressor

from paths import DATA_PATH, OUT_CSV

K_ROLLING = 10
CLIP_Q = 0.99
TARGETS = ["TongSoLuongBanThucTe", "SoLuongPhatHanhTrongThucTe"]


# ----------------------------------------------------------------------------
# ARIMA(1,1,1) 1-step forecast (numpy-only, deterministic)
# ----------------------------------------------------------------------------
def arima111_forecast(y, h=1, maxiter=300):
    """ARIMA(1,1,1) 1-step forecast. Fit ARMA(1,1) tren serie da sai phan."""
    y = np.asarray(y, float)
    z = np.diff(y)
    n = len(z)

    def css(params):
        phi, theta = params
        if abs(phi) >= 0.999 or abs(theta) >= 0.999:
            return 1e12
        e = np.zeros(n)
        zp = np.zeros(n)
        for t in range(1, n):
            zp[t] = phi * z[t - 1] + theta * e[t - 1]
            e[t] = z[t] - zp[t]
        return float(np.sum(e[1:] ** 2))

    res = minimize(css, [0.1, 0.1], method="Nelder-Mead",
                   options={"maxiter": maxiter, "xatol": 1e-4, "fatol": 1e-1})
    phi, theta = res.x
    e = np.zeros(n)
    zp = np.zeros(n)
    for t in range(1, n):
        zp[t] = phi * z[t - 1] + theta * e[t - 1]
        e[t] = z[t] - zp[t]
    zf = phi * z[-1] + theta * e[-1]
    return float(y[-1] + zf)


# ----------------------------------------------------------------------------
# Preprocessing helpers
# ----------------------------------------------------------------------------
def clip_outliers(y, q=CLIP_Q):
    y = np.asarray(y, float)
    lo, hi = np.quantile(y, 1 - q), np.quantile(y, q)
    return np.clip(y, lo, hi)


def add_lag_features(d, lags=5):
    d = d.copy()
    for L in range(1, lags + 1):
        d[f"Lag_{L}"] = d["y"].shift(L)
    d["RM3"] = d["y"].shift(1).rolling(3).mean()
    d["RM5"] = d["y"].shift(1).rolling(5).mean()
    return d.dropna().reset_index(drop=True)


# ----------------------------------------------------------------------------
# Rolling-CV MAE (danh gia, khong bat buoc cho predict)
# ----------------------------------------------------------------------------
def rolling_mae_arima(y, k=K_ROLLING):
    y = np.asarray(y, float)
    errs = []
    for i in range(k, 0, -1):
        train, actual = (y[:-1], y[-1]) if i == 1 else (y[:-i], y[-i])
        if len(train) < 5:
            continue
        try:
            errs.append(abs(arima111_forecast(train) - actual))
        except Exception:
            continue
    return float(np.mean(errs)) if errs else np.nan


def gb_rolling_mae(d, k=K_ROLLING):
    feats = [c for c in d.columns if c.startswith(("Lag_", "RM"))]
    errs = []
    for i in range(k, 0, -1):
        tr, te = (d.iloc[:-1], d.iloc[-1:]) if i == 1 else (d.iloc[:-i], d.iloc[-i:-i + 1])
        if len(tr) < 10:
            continue
        gb = GradientBoostingRegressor(n_estimators=100, max_depth=3,
                                       learning_rate=0.1, random_state=42)
        gb.fit(tr[feats], tr["y"])
        errs.append(abs(gb.predict(te[feats])[0] - te["y"].values[0]))
    return float(np.mean(errs)) if errs else np.nan


# ----------------------------------------------------------------------------
# Chon mo hinh tot nhat + du bao 1 ky tiep theo
# ----------------------------------------------------------------------------
def fit_best_and_predict(y_raw, lags=5):
    """Clip outlier, holdout 1-step tren y[:-1] -> y[-1] de chon ARIMA vs GB.
    Tra (pred_ky_tiep, model_name)."""
    y = clip_outliers(y_raw)
    try:
        ar_hold = abs(arima111_forecast(y[:-1]) - y[-1])
        ar_pred = arima111_forecast(y)
    except Exception:
        ar_hold, ar_pred = np.inf, float(y[-1])
    d = add_lag_features(pd.DataFrame({"y": np.asarray(y, float)}))
    if len(d) >= 15:
        try:
            feats = [c for c in d.columns if c.startswith(("Lag_", "RM"))]
            tr, te = d.iloc[:-1], d.iloc[-1:]
            gb = GradientBoostingRegressor(n_estimators=100, max_depth=3,
                                           learning_rate=0.1, random_state=42)
            gb.fit(tr[feats], tr["y"])
            gb_hold = abs(gb.predict(te[feats])[0] - te["y"].values[0])
            gb_pred = float(gb.predict(d[feats].iloc[[-1]])[0])
        except Exception:
            gb_hold, gb_pred = np.inf, float(y[-1])
    else:
        gb_hold, gb_pred = np.inf, float(y[-1])
    return (ar_pred if ar_hold <= gb_hold else gb_pred), ("ARIMA" if ar_hold <= gb_hold else "GB")


# ----------------------------------------------------------------------------
# Data loading + series cache (lazy, 1 lan / target)
# ----------------------------------------------------------------------------
_df = None
_series_cache = {}


def get_df():
    global _df
    if _df is None:
        _df = pd.read_excel(DATA_PATH)
        _df["NgayNhan"] = pd.to_datetime(_df["NgayNhan"])
    return _df


def build_series(target):
    if target not in TARGETS:
        raise ValueError(f"target phai thuoc {TARGETS}, nhan '{target}'")
    if target in _series_cache:
        return _series_cache[target]
    df = get_df()
    series = {}
    for (kh, bao), g in df.groupby(["MaKhachHang", "MaBao"]):
        g = g.sort_values("NgayNhan").reset_index(drop=True)
        s = g[["NgayNhan", "KyBao", target]].rename(columns={target: "y"})
        s["y"] = s["y"].astype(float)
        series[(kh, bao)] = s
    _series_cache[target] = series
    return series


# ----------------------------------------------------------------------------
# Public predict API
# ----------------------------------------------------------------------------
def predict_pair(kh, bao, target="TongSoLuongBanThucTe", model="standard"):
    """Du bao 1 cap (kh, bao) cho 1 truong. model='standard' hoac 'crosslearn'."""
    if model == "crosslearn":
        from crosslearn import fit_and_predict_all
        df = fit_and_predict_all(target)
        row = df[(df["MaKhachHang"] == kh) & (df["MaBao"] == bao)]
        if row.empty:
            raise KeyError(f"cap ({kh}, {bao}) khong ton tai")
        r = row.iloc[0]
        return {
            "MaKhachHang": kh, "MaBao": bao,
            "NextKyBao": int(r["NextKyBao"]),
            "Target": target, "Model": str(r[f"Model_{target}"]),
            "Pred": float(r[f"Pred_{target}"]),
        }
    series = build_series(target)
    key = (kh, bao)
    if key not in series:
        raise KeyError(f"cap ({kh}, {bao}) khong ton tai trong du lieu")
    d0 = series[key]
    pred, mdl = fit_best_and_predict(d0["y"].values)
    return {
        "MaKhachHang": kh, "MaBao": bao,
        "NextKyBao": int(d0["KyBao"].values[-1]) + 1,
        "Target": target, "Model": mdl,
        "Pred": round(float(pred), 0),
    }


def predict_all(target=None, out_csv=OUT_CSV, model="standard"):
    """Chay toan bo cap cho 1 hoac ca 2 truong. model='standard' hoac 'crosslearn'."""
    if model == "crosslearn":
        from crosslearn import fit_and_predict_all
        targets = [target] if target else TARGETS
        merged = None
        for t in targets:
            tdf = fit_and_predict_all(t)
            merged = tdf if merged is None else merged.merge(
                tdf, on=["MaKhachHang", "MaBao", "NextKyBao"], how="outer")
        merged.to_csv(out_csv, index=False)
        return merged
    targets = [target] if target else TARGETS
    merged = None
    for t in targets:
        series = build_series(t)
        rows = []
        for (kh, bao), d0 in series.items():
            pred, mdl = fit_best_and_predict(d0["y"].values)
            rows.append({
                "MaKhachHang": kh, "MaBao": bao,
                "NextKyBao": int(d0["KyBao"].values[-1]) + 1,
                f"Model_{t}": mdl,
                f"Pred_{t}": round(float(pred), 0),
            })
        tdf = pd.DataFrame(rows)
        merged = tdf if merged is None else merged.merge(
            tdf, on=["MaKhachHang", "MaBao", "NextKyBao"], how="outer")
    merged.to_csv(out_csv, index=False)
    return merged
