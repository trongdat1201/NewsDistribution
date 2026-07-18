"""paths.py - Cac duong dan du lieu / output, tap trung o mot cho de deploy.

BASE_DIR la noi chua NewData.xlsx va cac file CSV output
(forecast_next_period.csv, forecast_next_period_crosslearn.csv).

- Mac dinh: thu muc cha cua api/ (khi chay tai may hien tai: model/).
- Deploy tren may khac: dat env FORECAST_DIR tro den thu muc chua NewData.xlsx,
  hoac dat NewData.xlsx cung cap voi api/ roi dat FORECAST_DIR=duong_dan_do.
"""
import os
from pathlib import Path

API_DIR = Path(__file__).resolve().parent
BASE_DIR = Path(os.environ.get("FORECAST_DIR", r"C:\Users\Acer\OneDrive\Documents\ExcelDATN").strip())

DATA_PATH = BASE_DIR / "NewData.xlsx"
OUT_CSV = BASE_DIR / "forecast_next_period.csv"
CROSSLEARN_CSV = BASE_DIR / "forecast_next_period_crosslearn.csv"
