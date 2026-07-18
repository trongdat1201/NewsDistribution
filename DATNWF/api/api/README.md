# Forecast API — deploy tu api/

Thu muc nay tu cach phan lập: chua toan bo code API + model, khong phu thuoc vao
cac file o thu muc cha. Copy nguyen thu muc `api/` sang may khac la chay duoc.

## Cau truc
```
api/
  api_server.py      # HTTP API (stdlib, khong can framework them)
  forecast_lib.py    # model standard (ARIMA(1,1,1) vs GB lag-5)
  features.py        # feature engineering (clip 5/95, lag, rolling, momentum)
  crosslearn.py      # model Cross-Learning (HistGradientBoosting)
  paths.py           # tap trung duong dan data + output CSV
  train_forecast.py  # batch tao lai CSV (chay lai model)
  requirements.txt   # pin version da test
  README.md
```

## Cai dat (tren may moi)
```bash
python -m venv .venv
.venv/bin/pip install -r api/requirements.txt
```

## Chuan bi du lieu
Dat file `NewData.xlsx` vao mot thu muc, roi chi dinh qua env `FORECAST_DIR`:
```bash
export FORECAST_DIR=/duong/dan/den/thu_muc_chua_NewData.xlsx
```
Mac dinh (khong dat env): `FORECAST_DIR` = thu muc cha cua `api/`
(vi du neu `api/` nam trong `model/`, thi `model/NewData.xlsx` duoc dung).

Cac file CSV output (forecast_next_period.csv, forecast_next_period_crosslearn.csv)
cung duoc ghi vao `FORECAST_DIR`.

## Chay API
```bash
# tu thu muc chua api/ (de import tuong doi hoat dong)
.venv/bin/python api/api_server.py                 # 127.0.0.1:8011
.venv/bin/python api/api_server.py --port 9000 --host 0.0.0.0
```

Endpoints:
- `GET /health`
- `GET /predict?kh=<MaKhachHang>&bao=<MaBao>&target=<ten|both>&model=standard|crosslearn`
- `GET /predict_all?target=<ten|both>&model=standard|crosslearn&format=csv|json`
- `GET /predict_ky?kh=<MaKhachHang>&bao=<MaBao>&ky=<KyBao>&target=<ten|both>&model=crosslearn`
- `GET /predict_ky_batch?kh=<MaKhachHang>&bao=<MaBao>&ky_list=<ky1,ky2>&date_list=<YYYY-MM-DD,...>&target=<ten|both>&model=crosslearn`

`date_list` la tuy chon nhung, neu co, phai cung so phan tu va cung thu tu voi
`ky_list`. Batch prediction chay cuon chieu: ket qua ky truoc duoc dung lam lag
cho ky tiep theo. Neu bo qua `date_list`, ngay tuong lai duoc suy ra bang trung
vi khoang cach ngay duong trong lich su cua cap khach hang/bao.

Vi du:
```bash
curl "http://127.0.0.1:8011/predict?kh=KH-0000000196&bao=BD&target=both&model=crosslearn"
curl "http://127.0.0.1:8011/predict_ky_batch?kh=KH-0000000196&bao=BD&ky_list=101,102&date_list=2026-07-20,2026-07-24&target=both&model=crosslearn"
curl -s "http://127.0.0.1:8011/predict_all?target=both&model=crosslearn&format=csv" > out.csv
```

## Tao lai CSV (neu doi du lieu / muon chay lai model)
```bash
.venv/bin/python api/train_forecast.py --model standard
.venv/bin/python api/train_forecast.py --model crosslearn
```
(Luu y: model `standard` chay ARIMA tren tung cap nen kha lau ~15-20 phut cho 1400 cap.)

## Ket qua MAE (honest holdout 15 ky cuoi, n=13947)
| Truong | standard (~) | crosslearn |
|--------|--------------|------------|
| TongSoLuongBanThucTe | 105 | 77.69 |
| SoLuongPhatHanhTrongThucTe | 105 | 60.06 |
