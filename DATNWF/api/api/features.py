"""features.py - Feature engineering khong ro ri (chi dung gia tri qua khu)
cho mo hinh Cross-Learning. Clip ngoai le theo phan vi 5/95 cua tung cap.

Nam trong api/. Duong dan du lieu lay tu paths.py (configurable qua env FORECAST_DIR).
"""
import warnings
warnings.filterwarnings("ignore")
import numpy as np
import pandas as pd

from paths import DATA_PATH

CLIP_LO = 0.05
CLIP_HI = 0.95
LAGS = [1, 2, 3, 4, 5]


def load_df(path=DATA_PATH):
    df = pd.read_excel(path)
    df["NgayNhan"] = pd.to_datetime(df["NgayNhan"])
    return df


def clip_pairwise(df, target, lo=CLIP_LO, hi=CLIP_HI):
    """Clip outlier theo phan vi cua tung cap (MaKhachHang, MaBao).
    Gia tri thay the = bound (khong sua df goc)."""
    up = df.groupby(["MaKhachHang", "MaBao"])[target].transform("quantile", hi)
    low = df.groupby(["MaKhachHang", "MaBao"])[target].transform("quantile", lo)
    return np.clip(df[target], low, up)


def add_features(df, target):
    """Them feature: Lag 1-5, Rolling-Mean 3/5, Momentum, GapDays, DayOfWeek.
    Chi su dung shift qua khu -> khong ro ri khi holdout theo thoi gian."""
    df = df.sort_values(["MaKhachHang", "MaBao", "NgayNhan"]).reset_index(drop=True)
    y = clip_pairwise(df, target)
    out = df[["MaKhachHang", "MaBao", "KyBao", "NgayNhan"]].copy()
    out["y"] = y.values
    g = out.groupby(["MaKhachHang", "MaBao"])
    for L in LAGS:
        out[f"Lag_{L}"] = g["y"].shift(L)
    out["Rolling_Mean_3"] = g["Lag_1"].transform(lambda s: s.rolling(3).mean())
    out["Rolling_Mean_5"] = g["Lag_1"].transform(lambda s: s.rolling(5).mean())
    out["Momentum"] = g["y"].shift(1).diff() / g["y"].shift(1)
    out["GapDays"] = g["NgayNhan"].diff().dt.days
    out["DayOfWeek"] = out["NgayNhan"].dt.dayofweek
    out = out.dropna().reset_index(drop=True)
    return out


FEATURE_COLS = [f"Lag_{L}" for L in LAGS] + [
    "Rolling_Mean_3",
    "Rolling_Mean_5",
    "Momentum",
    "GapDays",
    "DayOfWeek",
]


def build_pooled(target, data_path=DATA_PATH):
    """Tra bang pooled (toan bo cap) co FEATURE_COLS + y + meta."""
    df = load_df(data_path)
    return add_features(df, target)
