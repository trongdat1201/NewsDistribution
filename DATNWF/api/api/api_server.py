"""api_server.py - HTTP API (stdlib http.server, khong can cai them) de request
du bao de dang. Load cache tu forecast_next_period*.csv (neu co), neu chua co
thi tu dong chay tao CSV o tien trinh nen (khong block server).

Nam trong thu muc api/; cac module cung cap (forecast_lib, features, crosslearn)
deu nam trong api/ nen deploy chi can copy nguyen thu muc api/.

Endpoints:
  GET /health
        -> {"status":"ok","pairs":1400,"targets":[...],"models":[...]}
  GET /predict?kh=<MaKhachHang>&bao=<MaBao>&target=<ten|both>&model=<standard|crosslearn>
        -> JSON 1 cap.
  GET /predict_all?target=<ten|both>&model=<standard|crosslearn>&format=csv|json
        -> JSON list hoac file CSV (text/csv) toan bo.

Chay:
  cd <dir_chua_api> && python api/api_server.py            # 127.0.0.1:8011
  python api/api_server.py --port 9000 --host 0.0.0.0
"""
import sys
import argparse
import csv
import io
import json
import os
import threading
from datetime import datetime
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from urllib.parse import urlparse, parse_qs
from pathlib import Path

import pandas as pd

from paths import OUT_CSV, CROSSLEARN_CSV
from forecast_lib import predict_all, TARGETS
from crosslearn import predict_for_ky, predict_batches_for_ky

# Cache ca 2 model (lazy). Neu CSV chua ton tai, se tu dong tao o background.
CACHE = {"standard": None, "crosslearn": None}
_BUILDING = {"standard": False, "crosslearn": False}
_BUILD_LOCK = threading.Lock()


def _ensure_csv(model):
    """Dam bao CSV ton tai. Neu chua co: tao o tien trinh nen (khong block)."""
    csv_path = CROSSLEARN_CSV if model == "crosslearn" else OUT_CSV
    if csv_path.exists():
        return True
    with _BUILD_LOCK:
        if _BUILDING[model]:
            return False  # dang tao o background
        _BUILDING[model] = True

    def _build():
        try:
            predict_all(model=model, out_csv=csv_path)
        finally:
            with _BUILD_LOCK:
                _BUILDING[model] = False

    t = threading.Thread(target=_build, daemon=True)
    t.start()
    return False


def load_cache(model="standard"):
    if model not in CACHE:
        raise ValueError("model phai la 'standard' hoac 'crosslearn'")
    if CACHE[model] is None:
        _ensure_csv(model)
        # CSV co the chua duoc tao (lan dau). Doc neu co.
        csv_path = CROSSLEARN_CSV if model == "crosslearn" else OUT_CSV
        if csv_path.exists():
            CACHE[model] = pd.read_csv(csv_path)
    return CACHE[model]


def _row_to_dict(row):
    return {
        "MaKhachHang": str(row["MaKhachHang"]),
        "MaBao": str(row["MaBao"]),
        "NextKyBao": int(row["NextKyBao"]),
    }


def predict_one(kh, bao, target, model="standard"):
    """Tra dict du bao tu cache CSV. Neu target='both' tra ca 2 truong."""
    C = load_cache(model)
    if C is None:
        return {"error": f"dang tao cache '{model}' lan dau, thu lai sau ~2-3 phut "
                         f"(hoac chay truoc: python api/train_forecast.py --model {model})"}
    sub = C[(C["MaKhachHang"] == kh) & (C["MaBao"] == bao)]
    if sub.empty:
        return None
    r = sub.iloc[0]
    base = _row_to_dict(r)
    if target == "both" or target is None:
        out = dict(base)
        for t in TARGETS:
            out[t] = {
                "Model": str(r[f"Model_{t}"]),
                "Pred": float(r[f"Pred_{t}"]),
            }
        return out
    if target not in TARGETS:
        return {"error": f"target phai thuoc {TARGETS} hoac 'both'"}
    return {
        **base,
        "Target": target,
        "Model": str(r[f"Model_{target}"]),
        "Pred": float(r[f"Pred_{target}"]),
    }


def predict_ky(ma_kh, ma_bao, ky_bao, target, model="crosslearn"):
    """Predict cho cap (kh, bao) tai KyBao cu the.
    
    Args:
        ma_kh: MaKhachHang
        ma_bao: MaBao
        ky_bao: KyBao can predict
        target: "TongSoLuongBanThucTe" hoac "SoLuongPhatHanhTrongThucTe" hoac "both"
        model: chi ho tro "crosslearn"
    Returns:
        dict voi KyBao, Pred_<target>, is_actual
    """
    if model != "crosslearn":
        return {"error": "predict_ky chi ho tro model='crosslearn'"}
    
    results = {}
    if target == "both" or target is None:
        for t in TARGETS:
            r = predict_for_ky(ma_kh, ma_bao, ky_bao, t)
            if r is None:
                return {"error": f"khong tim thay du lieu cho ({ma_kh},{ma_bao})"}
            results[t] = {
                "Model": r[f"Model_{t}"],
                "Pred": r[f"Pred_{t}"],
                "is_actual": r.get("is_actual", False)
            }
        return {
            "MaKhachHang": ma_kh,
            "MaBao": ma_bao,
            "KyBao": ky_bao,
            **results
        }
    else:
        if target not in TARGETS:
            return {"error": f"target phai thuoc {TARGETS} hoac 'both'"}
        r = predict_for_ky(ma_kh, ma_bao, ky_bao, target)
        if r is None:
            return {"error": f"khong tim thay du lieu cho ({ma_kh},{ma_bao})"}
        return {
            "MaKhachHang": ma_kh,
            "MaBao": ma_bao,
            "KyBao": ky_bao,
            "Target": target,
            "Model": r[f"Model_{target}"],
            "Pred": r[f"Pred_{target}"],
            "is_actual": r.get("is_actual", False)
        }


def _parse_ky_list(ky_list):
    if isinstance(ky_list, str):
        raw_values = [value.strip() for value in ky_list.split(",")]
    else:
        raw_values = list(ky_list)
    if not raw_values or any(str(value).strip() == "" for value in raw_values):
        raise ValueError("ky_list phai la list int hoac chuoi '1,2,3'")
    try:
        parsed = [int(value) for value in raw_values]
    except (TypeError, ValueError) as exc:
        raise ValueError("ky_list phai la list int hoac chuoi '1,2,3'") from exc
    if len(set(parsed)) != len(parsed):
        raise ValueError("ky_list khong duoc trung lap")
    return parsed


def _parse_date_list(date_list, expected_count):
    if date_list is None or date_list == "":
        return None
    raw_values = (
        [value.strip() for value in date_list.split(",")]
        if isinstance(date_list, str)
        else list(date_list)
    )
    if len(raw_values) != expected_count:
        raise ValueError("date_list phai co cung so phan tu voi ky_list")

    parsed = []
    for value in raw_values:
        try:
            parsed.append(pd.Timestamp(datetime.strptime(str(value), "%Y-%m-%d").date()))
        except (TypeError, ValueError) as exc:
            raise ValueError("date_list phai dung dinh dang YYYY-MM-DD") from exc
    return parsed


def predict_ky_batch(ma_kh, ma_bao, ky_list, target, model="crosslearn",
                     date_list=None):
    """Predict nhieu KyBao theo thu tu cuon chieu cho mot cap (kh, bao)."""
    if model != "crosslearn":
        return {"error": "predict_ky_batch chi ho tro model='crosslearn'"}

    try:
        parsed_kys = _parse_ky_list(ky_list)
        parsed_dates = _parse_date_list(date_list, len(parsed_kys))
    except ValueError as exc:
        return {"error": str(exc)}

    results = {}
    try:
        if target == "both" or target is None:
            for target_name in TARGETS:
                predictions = predict_batches_for_ky(
                    ma_kh, ma_bao, parsed_kys, target_name, parsed_dates
                )
                results[target_name] = [{
                    "KyBao": prediction["KyBao"],
                    "NgayNhan": prediction.get("NgayNhan"),
                    "Pred": prediction[f"Pred_{target_name}"],
                    "is_actual": prediction.get("is_actual", False),
                } for prediction in predictions]
        else:
            if target not in TARGETS:
                return {"error": f"target phai thuoc {TARGETS} hoac 'both'"}
            predictions = predict_batches_for_ky(
                ma_kh, ma_bao, parsed_kys, target, parsed_dates
            )
            results[target] = [{
                "KyBao": prediction["KyBao"],
                "NgayNhan": prediction.get("NgayNhan"),
                "Pred": prediction[f"Pred_{target}"],
                "is_actual": prediction.get("is_actual", False),
            } for prediction in predictions]
    except ValueError as exc:
        return {"error": str(exc)}
    except Exception as exc:  # noqa: BLE001 - surface unexpected model failures
        return {"error": f"du bao loi: {exc}"}

    return {
        "MaKhachHang": ma_kh,
        "MaBao": ma_bao,
        "KyBaoList": parsed_kys,
        "DateList": [date.strftime("%Y-%m-%d") for date in parsed_dates]
                    if parsed_dates is not None else None,
        "predictions": results,
    }


def to_csv_bytes(df_subset, target):
    cols = ["MaKhachHang", "MaBao", "NextKyBao"]
    if target == "both" or target is None:
        for t in TARGETS:
            cols += [f"Model_{t}", f"Pred_{t}"]
    else:
        cols += [f"Model_{target}", f"Pred_{target}"]
    buf = io.StringIO()
    df_subset[cols].to_csv(buf, index=False)
    return buf.getvalue().encode("utf-8")


class Handler(BaseHTTPRequestHandler):
    def _send(self, code, body, ctype="application/json"):
        if isinstance(body, (dict, list)):
            body = json.dumps(body, ensure_ascii=False)
        if isinstance(body, str):
            body = body.encode("utf-8")
        self.send_response(code)
        self.send_header("Content-Type", ctype)
        self.send_header("Content-Length", str(len(body)))
        self.end_headers()
        self.wfile.write(body)

    def do_GET(self):
        u = urlparse(self.path)
        q = parse_qs(u.query)
        path = u.path

        if path == "/health":
            ready = {
                "standard": OUT_CSV.exists() or CACHE["standard"] is not None,
                "crosslearn": CROSSLEARN_CSV.exists() or CACHE["crosslearn"] is not None,
            }
            return self._send(200, {
                "status": "ok",
                "targets": TARGETS,
                "models": ["standard", "crosslearn"],
                "ready": ready,
                "csv_standard": str(OUT_CSV),
                "csv_crosslearn": str(CROSSLEARN_CSV),
            })

        if path == "/predict":
            kh = (q.get("kh") or [None])[0]
            bao = (q.get("bao") or [None])[0]
            target = (q.get("target") or ["TongSoLuongBanThucTe"])[0]
            model = (q.get("model") or ["standard"])[0]
            if model not in ("standard", "crosslearn"):
                return self._send(400, {"error": "model phai la 'standard' hoac 'crosslearn'"})
            if not kh or not bao:
                return self._send(400, {"error": "thieu kh hoac bao"})
            res = predict_one(kh, bao, target, model)
            if res is None:
                return self._send(404, {"error": f"khong tim thay ({kh},{bao})"})
            if "error" in res:
                return self._send(503, res)
            return self._send(200, res)

        if path == "/predict_all":
            target = (q.get("target") or ["both"])[0]
            fmt = (q.get("format") or ["json"])[0].lower()
            model = (q.get("model") or ["standard"])[0]
            if model not in ("standard", "crosslearn"):
                return self._send(400, {"error": "model phai la 'standard' hoac 'crosslearn'"})
            C = load_cache(model)
            if C is None:
                return self._send(503, {"error": f"dang tao cache '{model}' lan dau, thu lai sau"})
            if target == "both" or target is None:
                sub = C
            elif target in TARGETS:
                sub = C
            else:
                return self._send(400, {"error": f"target phai thuoc {TARGETS} hoac 'both'"})
            if fmt == "csv":
                return self._send(200, to_csv_bytes(sub, target), "text/csv; charset=utf-8")
            return self._send(200, json.loads(sub.to_json(orient="records")))

        # Endpoint moi: predict cho KyBao cu the
        if path == "/predict_ky":
            kh = (q.get("kh") or [None])[0]
            bao = (q.get("bao") or [None])[0]
            ky = (q.get("ky") or [None])[0]
            target = (q.get("target") or ["both"])[0]
            model = (q.get("model") or ["crosslearn"])[0]
            if not kh or not bao or not ky:
                return self._send(400, {"error": "thieu kh, bao, hoac ky"})
            try:
                ky = int(ky)
            except:
                return self._send(400, {"error": "ky phai la so nguyen"})
            res = predict_ky(kh, bao, ky, target, model)
            if "error" in res:
                return self._send(400, res)
            return self._send(200, res)

        # Endpoint moi: predict nhieu KyBao cung luc
        if path == "/predict_ky_batch":
            kh = (q.get("kh") or [None])[0]
            bao = (q.get("bao") or [None])[0]
            ky_list = (q.get("ky_list") or [None])[0]
            date_list = (q.get("date_list") or [None])[0]
            target = (q.get("target") or ["both"])[0]
            model = (q.get("model") or ["crosslearn"])[0]
            if not kh or not bao or not ky_list:
                return self._send(400, {"error": "thieu kh, bao, hoac ky_list"})
            res = predict_ky_batch(
                kh, bao, ky_list, target, model, date_list=date_list
            )
            if "error" in res:
                return self._send(400, res)
            return self._send(200, res)

        return self._send(404, {"error": "unknown endpoint", "paths": ["/health", "/predict", "/predict_all", "/predict_ky", "/predict_ky_batch"]})

    def log_message(self, *args):
        pass  # im lang log


def main():
    ap = argparse.ArgumentParser(description="Forecast HTTP API (stdlib)")
    ap.add_argument("--host", default="127.0.0.1")
    ap.add_argument("--port", type=int, default=8011)
    ap.add_argument("--no-warmup", action="store_true", help="skip cache warmup at startup")
    args = ap.parse_args()
    print(f"API tai http://{args.host}:{args.port}")
    print(f"  standard CSV:   {OUT_CSV}  (ton tai: {OUT_CSV.exists()})")
    print(f"  crosslearn CSV: {CROSSLEARN_CSV}  (ton tai: {CROSSLEARN_CSV.exists()})")
    if not args.no_warmup:
        from crosslearn import _get_pool_and_model
        from forecast_lib import TARGETS
        print("  Warming up crosslearn cache...")
        for t in TARGETS:
            print(f"    Building {t}...", end=" ", flush=True)
            p, m = _get_pool_and_model(t)
            print(f"OK ({len(p)} rows, {m.n_iter_} iters)")
        print("  Cache warmup xong! Requests se nhanh (<0.1s).")
    else:
        if not OUT_CSV.exists() or not CROSSLEARN_CSV.exists():
            print("  [thong bao] lan dau chay se tu dong tao CSV o tien trinh nen "
                  "(standard ~15-20p, crosslearn ~3p). Hoac chay truoc: "
                  "python api/train_forecast.py --model <standard|crosslearn>")
    srv = ThreadingHTTPServer((args.host, args.port), Handler)
    try:
        srv.serve_forever()
    except KeyboardInterrupt:
        print("\nstopped")


if __name__ == "__main__":
    main()
