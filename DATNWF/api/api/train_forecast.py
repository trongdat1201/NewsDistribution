"""train_forecast.py - Batch du bao toan bo 1400 cap (MaKhachHang, MaBao) cho
CA 2 truong: TongSoLuongBanThucTe va SoLuongPhatHanhTrongThucTe.

Nam trong api/. Duong dan output lay tu paths.py (configurable qua env FORECAST_DIR).

Su dung:
  python train_forecast.py --model standard                 # ca 2 truong (default)
  python train_forecast.py --model crosslearn               # Cross-Learning ML
  python train_forecast.py --model crosslearn --target TongSoLuongBanThucTe
"""
import argparse
from pathlib import Path

import forecast_lib
from paths import OUT_CSV, CROSSLEARN_CSV


if __name__ == "__main__":
    ap = argparse.ArgumentParser(description="Batch forecast ca 2 truong")
    ap.add_argument("--model", default="standard", choices=["standard", "crosslearn"])
    ap.add_argument("--target", default=None,
                    choices=["TongSoLuongBanThucTe", "SoLuongPhatHanhTrongThucTe"],
                    help="chi chay 1 truong (mac dinh: ca 2)")
    args = ap.parse_args()
    out = OUT_CSV if args.model == "standard" else CROSSLEARN_CSV
    m = forecast_lib.predict_all(target=args.target, out_csv=out, model=args.model)
    print(f"\nDa ghi {out}")
    print("gate:", "PASS" if (len(m) == 1400 and m.dropna().shape[0] == len(m)) else "FAIL",
          f"(rows={len(m)})")
    print(m.head(10).to_string(index=False))
