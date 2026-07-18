"""crosslearn.py - Cross-Learning (1 model HistGradientBoosting tren toan bo cap).
Do MAE honest bang holdout thoi gian: train tren qua khu, predict cac ky cuoi.

Nam trong api/. Duong dan du lieu/output lay tu paths.py (configurable qua env FORECAST_DIR).
"""
import warnings
warnings.filterwarnings("ignore")
import numpy as np
import pandas as pd
from sklearn.ensemble import HistGradientBoostingRegressor

from paths import DATA_PATH, CROSSLEARN_CSV
from features import build_pooled, FEATURE_COLS

HOLDOUT = 15
TARGETS = ["TongSoLuongBanThucTe", "SoLuongPhatHanhTrongThucTe"]

# Cache trained model + pool theo target, de predict nhanh (tranh train lai moi request)
_MODEL_CACHE = {}
_POOL_CACHE = {}


def _get_pool_and_model(target, data_path=DATA_PATH):
    """Lazy-train: tra ve (pool, model). Cache theo target."""
    if target in _POOL_CACHE:
        return _POOL_CACHE[target], _MODEL_CACHE[target]
    pool = build_pooled(target, data_path)
    m = make_model().fit(pool[FEATURE_COLS], pool["y"])
    _POOL_CACHE[target] = pool
    _MODEL_CACHE[target] = m
    return pool, m


def make_model():
    return HistGradientBoostingRegressor(max_iter=200, learning_rate=0.05,
                                          max_depth=5, random_state=42)


def holdout_eval(target, holdout=HOLDOUT, data_path=DATA_PATH):
    """Train tren KyBao <= (max-H), predict H ky cuoi cua moi cap. MAE honest."""
    pool = build_pooled(target, data_path)
    maxkb = pool["KyBao"].max()
    test_mask = pool["KyBao"] > (maxkb - holdout)
    train, test = pool[~test_mask], pool[test_mask]
    m = make_model().fit(train[FEATURE_COLS], train["y"])
    pred = m.predict(test[FEATURE_COLS])
    err = np.abs(pred - test["y"].values)
    mae = float(np.mean(err))
    rmse = float(np.sqrt(np.mean((pred - test["y"].values) ** 2)))
    mape = float(np.mean(np.abs(err / test["y"].values)) * 100)
    per = pd.DataFrame({
        "MaKhachHang": test["MaKhachHang"].values,
        "MaBao": test["MaBao"].values,
        "y_true": test["y"].values,
        "y_pred": pred,
        "ae": err,
    })
    per_pair = per.groupby(["MaKhachHang", "MaBao"])["ae"].mean().reset_index()
    return {"mae": mae, "rmse": rmse, "mape": mape, "per_pair": per_pair,
            "n_test": len(test)}


def _normalise_timestamp(value):
    timestamp = pd.Timestamp(value)
    if pd.isna(timestamp):
        raise ValueError("ngay du bao khong hop le")
    return timestamp.normalize()


def _median_gap_days(history):
    dates = pd.to_datetime(history["NgayNhan"], errors="coerce").dropna().sort_values()
    gaps = dates.diff().dt.total_seconds().div(86400)
    positive_gaps = gaps[gaps > 0]
    if not positive_gaps.empty:
        return float(positive_gaps.median())

    if "GapDays" in history:
        stored_gaps = pd.to_numeric(history["GapDays"], errors="coerce")
        stored_gaps = stored_gaps[stored_gaps > 0]
        if not stored_gaps.empty:
            return float(stored_gaps.median())
    return 4.0


def _build_future_features(history, forecast_date):
    ordered = history.sort_values(["KyBao", "NgayNhan"]).reset_index(drop=True)
    if ordered.empty:
        raise ValueError("khong co lich su de tao feature")

    values = ordered["y"].astype(float).tolist()

    def lag(position):
        if position <= len(values):
            return values[-position]
        return values[0]

    previous_date = _normalise_timestamp(ordered.iloc[-1]["NgayNhan"])
    current_date = _normalise_timestamp(forecast_date)
    gap_days = (current_date - previous_date).total_seconds() / 86400
    if gap_days <= 0:
        raise ValueError("ngay du bao phai tang dan va sau ngay lich su gan nhat")

    previous_value = lag(2)
    momentum = (lag(1) - previous_value) / previous_value if previous_value != 0 else 0.0
    return pd.DataFrame([{
        "Lag_1": lag(1),
        "Lag_2": lag(2),
        "Lag_3": lag(3),
        "Lag_4": lag(4),
        "Lag_5": lag(5),
        "Rolling_Mean_3": float(np.mean(values[-3:])),
        "Rolling_Mean_5": float(np.mean(values[-5:])),
        "Momentum": momentum,
        "GapDays": gap_days,
        "DayOfWeek": current_date.dayofweek,
    }])


def _prediction_result(ma_kh, ma_bao, ky_bao, target, prediction, is_actual, date):
    return {
        "MaKhachHang": ma_kh,
        "MaBao": ma_bao,
        "KyBao": int(ky_bao),
        f"Model_{target}": "CrossLearn-HistGBR",
        f"Pred_{target}": round(max(0, float(prediction)), 0),
        "is_actual": is_actual,
        "NgayNhan": _normalise_timestamp(date).strftime("%Y-%m-%d"),
    }


def predict_for_ky(ma_kh, ma_bao, ky_bao, target, data_path=DATA_PATH, forecast_date=None):
    """Predict mot KyBao; forecast_date la ngay ISO tuy chon cho ky tuong lai."""
    date_list = [forecast_date] if forecast_date is not None else None
    results = predict_batches_for_ky(
        ma_kh, ma_bao, [ky_bao], target, data_path=data_path, date_list=date_list
    )
    return results[0] if results else None


def predict_batches_for_ky(ma_kh, ma_bao, ky_list, target, date_list=None,
                           data_path=DATA_PATH):
    """Predict nhieu ky theo thu tu cuon chieu cho mot cap khach hang/bao.

    Moi du bao tuong lai duoc noi vao lich su tam thoi truoc khi tao feature cho
    ky tiep theo. Neu khong co date_list, ngay moi duoc suy ra bang median gap
    duong cua lich su cap do.
    """
    requested_kys = [int(ky) for ky in ky_list]
    if not requested_kys:
        return []
    if len(set(requested_kys)) != len(requested_kys):
        raise ValueError("ky_list khong duoc trung lap")

    requested_dates = None
    if date_list is not None:
        if len(date_list) != len(requested_kys):
            raise ValueError("date_list phai co cung so phan tu voi ky_list")
        requested_dates = [_normalise_timestamp(value) for value in date_list]

    pool, model = _get_pool_and_model(target, data_path)
    pair_history = pool[
        (pool["MaKhachHang"] == ma_kh) & (pool["MaBao"] == ma_bao)
    ].copy()
    if pair_history.empty:
        return []

    pair_history["NgayNhan"] = pd.to_datetime(pair_history["NgayNhan"])
    pair_history = pair_history.sort_values(["KyBao", "NgayNhan"]).reset_index(drop=True)
    median_gap = _median_gap_days(pair_history)
    timeline = pair_history[["KyBao", "NgayNhan", "y"]].copy()
    actual_by_ky = {
        int(row.KyBao): row
        for row in pair_history[["KyBao", "NgayNhan", "y"]].itertuples(index=False)
    }

    indexed_requests = list(enumerate(zip(requested_kys, requested_dates or [None] * len(requested_kys))))
    indexed_requests.sort(key=lambda item: item[1][0])
    results_by_index = {}

    for original_index, (ky_bao, explicit_date) in indexed_requests:
        actual = actual_by_ky.get(ky_bao)
        if actual is not None:
            results_by_index[original_index] = _prediction_result(
                ma_kh, ma_bao, ky_bao, target, actual.y, True, actual.NgayNhan
            )
            continue

        history_before_ky = timeline[timeline["KyBao"] < ky_bao].copy()
        if history_before_ky.empty:
            continue

        previous_date = _normalise_timestamp(history_before_ky.iloc[-1]["NgayNhan"])
        forecast_date = explicit_date or (
            previous_date + pd.to_timedelta(median_gap, unit="D")
        )
        feature_frame = _build_future_features(history_before_ky, forecast_date)
        raw_prediction = max(0.0, float(model.predict(feature_frame[FEATURE_COLS])[0]))
        timeline = pd.concat([
            timeline,
            pd.DataFrame([{
                "KyBao": ky_bao,
                "NgayNhan": forecast_date,
                "y": raw_prediction,
            }]),
        ], ignore_index=True)
        timeline = timeline.sort_values(["KyBao", "NgayNhan"]).reset_index(drop=True)
        results_by_index[original_index] = _prediction_result(
            ma_kh, ma_bao, ky_bao, target, raw_prediction, False, forecast_date
        )

    return [results_by_index[index] for index in range(len(requested_kys))
            if index in results_by_index]


def fit_and_predict_all(target, data_path=DATA_PATH):
    """Retrain toan bo va predict mot buoc voi ngay suy ra bang median gap."""
    pool = build_pooled(target, data_path)
    model = make_model().fit(pool[FEATURE_COLS], pool["y"])
    rows = []
    for (kh, bao), history in pool.groupby(["MaKhachHang", "MaBao"]):
        history = history.sort_values(["KyBao", "NgayNhan"]).reset_index(drop=True)
        last = history.iloc[-1]
        next_ky = int(last["KyBao"]) + 1
        next_date = _normalise_timestamp(last["NgayNhan"]) + pd.to_timedelta(
            _median_gap_days(history), unit="D"
        )
        feature_frame = _build_future_features(history, next_date)
        prediction = max(0.0, float(model.predict(feature_frame[FEATURE_COLS])[0]))
        rows.append({
            "MaKhachHang": kh,
            "MaBao": bao,
            "NextKyBao": next_ky,
            f"Model_{target}": "CrossLearn-HistGBR",
            f"Pred_{target}": round(prediction, 0),
        })
    return pd.DataFrame(rows)
