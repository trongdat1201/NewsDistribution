import json
import threading
import unittest
from datetime import datetime
from unittest.mock import patch
from urllib.error import HTTPError
from urllib.request import urlopen

import numpy as np
import pandas as pd

import api_server
import crosslearn
import features


TARGET = "TongSoLuongBanThucTe"


class RecordingModel:
    def __init__(self):
        self.frames = []

    def predict(self, frame):
        snapshot = frame.copy()
        self.frames.append(snapshot)
        return np.array([float(snapshot.iloc[0]["Lag_1"]) + 1.0])


def make_pool():
    dates = pd.to_datetime(
        [
            "2026-05-30",
            "2026-06-01",
            "2026-06-05",
            "2026-06-08",
            "2026-06-13",
            "2026-06-15",
        ]
    )
    return pd.DataFrame(
        {
            "MaKhachHang": ["KH01"] * 6,
            "MaBao": ["BD"] * 6,
            "KyBao": [95, 96, 97, 98, 99, 100],
            "NgayNhan": dates,
            "y": [50.0, 52.0, 54.0, 56.0, 58.0, 60.0],
            "Lag_1": [np.nan, 50.0, 52.0, 54.0, 56.0, 58.0],
            "Lag_2": [np.nan, np.nan, 50.0, 52.0, 54.0, 56.0],
            "Lag_3": [np.nan, np.nan, np.nan, 50.0, 52.0, 54.0],
            "Lag_4": [np.nan, np.nan, np.nan, np.nan, 50.0, 52.0],
            "Lag_5": [np.nan, np.nan, np.nan, np.nan, np.nan, 50.0],
            "Rolling_Mean_3": [np.nan, np.nan, 50.0, 52.0, 54.0, 56.0],
            "Rolling_Mean_5": [np.nan, np.nan, np.nan, np.nan, np.nan, 50.0],
            "Momentum": [np.nan, 0.04, 0.038, 0.037, 0.036, 0.034],
            "GapDays": [2.0, 2.0, 4.0, 3.0, 5.0, 2.0],
            "DayOfWeek": [5, 0, 4, 1, 5, 0],
        }
    )


class FeatureEngineeringTests(unittest.TestCase):
    def test_add_features_exposes_day_of_week(self):
        dates = pd.date_range("2026-07-13", periods=9, freq="D")
        source = pd.DataFrame(
            {
                "MaKhachHang": ["KH01"] * len(dates),
                "MaBao": ["BD"] * len(dates),
                "KyBao": list(range(1, len(dates) + 1)),
                "NgayNhan": dates,
                TARGET: np.arange(10.0, 10.0 + len(dates)),
            }
        )

        result = features.add_features(source, TARGET)

        self.assertIn("DayOfWeek", features.FEATURE_COLS)
        self.assertIn("DayOfWeek", result.columns)
        self.assertListEqual(
            result["DayOfWeek"].tolist(),
            result["NgayNhan"].dt.dayofweek.tolist(),
        )


class RecursiveForecastTests(unittest.TestCase):
    def test_batch_rolls_each_prediction_into_the_next_period(self):
        model = RecordingModel()
        dates = [pd.Timestamp("2026-06-17"), pd.Timestamp("2026-06-21")]

        with patch.object(
            crosslearn, "_get_pool_and_model", return_value=(make_pool(), model)
        ):
            result = crosslearn.predict_batches_for_ky(
                "KH01", "BD", [101, 102], TARGET, date_list=dates
            )

        self.assertEqual([61.0, 62.0], [item[f"Pred_{TARGET}"] for item in result])
        self.assertEqual(61.0, model.frames[1].iloc[0]["Lag_1"])
        self.assertEqual(2.0, model.frames[0].iloc[0]["GapDays"])
        self.assertEqual(4.0, model.frames[1].iloc[0]["GapDays"])
        self.assertEqual(2, model.frames[0].iloc[0]["DayOfWeek"])
        self.assertEqual(6, model.frames[1].iloc[0]["DayOfWeek"])

    def test_batch_falls_back_to_historical_median_gap(self):
        model = RecordingModel()

        with patch.object(
            crosslearn, "_get_pool_and_model", return_value=(make_pool(), model)
        ):
            crosslearn.predict_batches_for_ky("KH01", "BD", [101], TARGET)

        feature_row = model.frames[0].iloc[0]
        self.assertEqual(3.0, feature_row["GapDays"])
        self.assertEqual(3, feature_row["DayOfWeek"])


class BatchApiTests(unittest.TestCase):
    def test_service_parses_and_forwards_iso_date_list(self):
        returned = [
            {
                "KyBao": 101,
                f"Pred_{TARGET}": 61.0,
                "is_actual": False,
            },
            {
                "KyBao": 102,
                f"Pred_{TARGET}": 62.0,
                "is_actual": False,
            },
        ]

        with patch.object(api_server, "predict_batches_for_ky", return_value=returned) as call:
            response = api_server.predict_ky_batch(
                "KH01",
                "BD",
                "101,102",
                TARGET,
                date_list="2026-06-17,2026-06-21",
            )

        self.assertNotIn("error", response)
        forwarded_dates = call.call_args.args[4]
        self.assertEqual(
            [pd.Timestamp("2026-06-17"), pd.Timestamp("2026-06-21")],
            forwarded_dates,
        )
        self.assertEqual(["2026-06-17", "2026-06-21"], response["DateList"])

    def test_service_rejects_mismatched_date_count(self):
        response = api_server.predict_ky_batch(
            "KH01",
            "BD",
            "101,102",
            TARGET,
            date_list="2026-06-17",
        )

        self.assertIn("error", response)
        self.assertIn("cung so phan tu", response["error"])

    def test_service_rejects_invalid_iso_date(self):
        response = api_server.predict_ky_batch(
            "KH01",
            "BD",
            "101",
            TARGET,
            date_list="17/06/2026",
        )

        self.assertIn("error", response)
        self.assertIn("YYYY-MM-DD", response["error"])

    def test_http_endpoint_accepts_date_list(self):
        captured = {}

        def fake_predict(kh, bao, ky_list, target, model, date_list=None):
            captured["date_list"] = date_list
            return {
                "MaKhachHang": kh,
                "MaBao": bao,
                "KyBaoList": [101, 102],
                "DateList": ["2026-06-17", "2026-06-21"],
                "predictions": {},
            }

        server = api_server.ThreadingHTTPServer(("127.0.0.1", 0), api_server.Handler)
        worker = threading.Thread(target=server.serve_forever, daemon=True)
        worker.start()
        try:
            query = (
                "/predict_ky_batch?kh=KH01&bao=BD&ky_list=101,102"
                "&date_list=2026-06-17,2026-06-21"
            )
            with patch.object(api_server, "predict_ky_batch", side_effect=fake_predict):
                with urlopen(
                    f"http://127.0.0.1:{server.server_port}{query}", timeout=5
                ) as response:
                    payload = json.loads(response.read().decode("utf-8"))
        finally:
            server.shutdown()
            server.server_close()
            worker.join(timeout=5)

        self.assertEqual("2026-06-17,2026-06-21", captured["date_list"])
        self.assertEqual(["2026-06-17", "2026-06-21"], payload["DateList"])


class BatchApiValidationTests(unittest.TestCase):
    def _run(self, **overrides):
        kwargs = {
            "ma_kh": "KH01",
            "ma_bao": "BD",
            "ky_list": "101,102",
            "target": TARGET,
            "date_list": "2026-06-17,2026-06-21",
        }
        kwargs.update(overrides)
        return api_server.predict_ky_batch(**kwargs)

    def test_blank_inputs_are_rejected(self):
        # predict_ky_batch requires ky_list to be parseable, but allows blank kh/bao
        # (delegating to model). Make sure that the empty ky_list path returns an error.
        response = self._run(ky_list="")
        self.assertIn("error", response)
        self.assertIn("ky_list", response["error"])

    def test_unknown_target_is_rejected(self):
        response = self._run(target="unknown_target")
        self.assertIn("error", response)
        self.assertIn("target", response["error"])

    def test_duplicate_ky_values_are_rejected(self):
        response = self._run(ky_list="101,101")
        self.assertIn("error", response)
        self.assertIn("trung lap", response["error"])

    def test_empty_date_list_is_treated_as_no_date(self):
        response = self._run(date_list="")
        self.assertNotIn("error", response)
        self.assertIsNone(response["DateList"])

    def test_malformed_ky_value_is_rejected(self):
        response = self._run(ky_list="101,abc")
        self.assertIn("error", response)
        self.assertIn("ky_list", response["error"])

    def test_invalid_date_calendar_value_is_rejected(self):
        response = self._run(ky_list="101", date_list="2026-13-40")
        self.assertIn("error", response)
        self.assertIn("YYYY-MM-DD", response["error"])

    def test_forecast_combines_both_targets_when_requested(self):
        returned_first = [
            {
                "KyBao": 101,
                "NgayNhan": "2026-06-17",
                f"Model_{TARGET}": "CrossLearn-HistGBR",
                f"Pred_{TARGET}": 61.0,
                "is_actual": False,
            },
            {
                "KyBao": 102,
                "NgayNhan": "2026-06-21",
                f"Model_{TARGET}": "CrossLearn-HistGBR",
                f"Pred_{TARGET}": 62.0,
                "is_actual": False,
            },
        ]
        returned_second = [
            {
                "KyBao": 101,
                "NgayNhan": "2026-06-17",
                "Model_SoLuongPhatHanhTrongThucTe": "CrossLearn-HistGBR",
                "Pred_SoLuongPhatHanhTrongThucTe": 70.0,
                "is_actual": False,
            },
            {
                "KyBao": 102,
                "NgayNhan": "2026-06-21",
                "Model_SoLuongPhatHanhTrongThucTe": "CrossLearn-HistGBR",
                "Pred_SoLuongPhatHanhTrongThucTe": 71.0,
                "is_actual": False,
            },
        ]

        with patch.object(
            api_server,
            "predict_batches_for_ky",
            side_effect=[returned_first, returned_second],
        ) as delegate:
            response = api_server.predict_ky_batch(
                "KH01",
                "BD",
                "101,102",
                "both",
                date_list="2026-06-17,2026-06-21",
            )

        self.assertNotIn("error", response)
        self.assertEqual(2, delegate.call_count)
        first_target_list = response["predictions"][TARGET]
        self.assertEqual(101, first_target_list[0]["KyBao"])
        self.assertEqual(62.0, first_target_list[1]["Pred"])
        self.assertEqual(
            71.0,
            response["predictions"]["SoLuongPhatHanhTrongThucTe"][1]["Pred"],
        )
        self.assertEqual("2026-06-17", first_target_list[0]["NgayNhan"])

    def test_forecast_wraps_validation_exception_in_error_response(self):
        def boom(*args, **kwargs):
            raise ValueError("model crashed")

        with patch.object(api_server, "predict_batches_for_ky", side_effect=boom):
            response = api_server.predict_ky_batch(
                "KH01", "BD", "101", TARGET, date_list="2026-06-17"
            )

        self.assertIn("error", response)
        self.assertIn("model crashed", response["error"])

    def test_forecast_wraps_unexpected_exception_in_error_response(self):
        def boom(*args, **kwargs):
            raise RuntimeError("unexpected model failure")

        with patch.object(api_server, "predict_batches_for_ky", side_effect=boom):
            response = api_server.predict_ky_batch(
                "KH01", "BD", "101", TARGET, date_list="2026-06-17"
            )

        self.assertIn("error", response)
        self.assertIn("unexpected model failure", response["error"])

    def test_forecast_rejects_non_crosslearn_model(self):
        response = api_server.predict_ky_batch(
            "KH01", "BD", "101", TARGET, model="standard",
            date_list="2026-06-17",
        )
        self.assertIn("error", response)
        self.assertIn("crosslearn", response["error"])

    def test_parse_ky_list_rejects_non_string_non_list_input(self):
        # None input is not iterable -> TypeError. List containing blanks is rejected explicitly.
        with self.assertRaises((TypeError, ValueError)):
            api_server._parse_ky_list(None)
        with self.assertRaises(ValueError):
            api_server._parse_ky_list([None, "101"])

    def test_parse_date_list_accepts_iterable_of_dates(self):
        parsed = api_server._parse_date_list(
            ["2026-06-17", "2026-06-21"], expected_count=2
        )
        self.assertEqual(
            [pd.Timestamp("2026-06-17"), pd.Timestamp("2026-06-21")],
            parsed,
        )

    def test_parse_date_list_rejects_unsupported_value(self):
        with self.assertRaises(ValueError):
            api_server._parse_date_list([123], expected_count=1)


class HttpEndpointTests(unittest.TestCase):
    def _start_server(self, fake_predict):
        server = api_server.ThreadingHTTPServer(("127.0.0.1", 0), api_server.Handler)
        worker = threading.Thread(target=server.serve_forever, daemon=True)
        worker.start()
        self.addCleanup(server.shutdown)
        self.addCleanup(server.server_close)
        self.addCleanup(lambda: worker.join(timeout=5))
        return server

    def test_endpoint_returns_404_for_unknown_route(self):
        server = self._start_server(lambda *args, **kwargs: {})

        try:
            urlopen(
                f"http://127.0.0.1:{server.server_port}/no-such-route", timeout=5
            )
        except HTTPError as exc:
            self.assertEqual(404, exc.code)
        else:
            self.fail("expected HTTP 404")

    def test_endpoint_returns_400_for_missing_kh(self):
        server = self._start_server(lambda *args, **kwargs: {})

        try:
            urlopen(
                f"http://127.0.0.1:{server.server_port}/predict_ky_batch?bao=BD&ky_list=101",
                timeout=5,
            )
        except HTTPError as exc:
            self.assertEqual(400, exc.code)
        else:
            self.fail("expected HTTP 400")

    def test_endpoint_returns_400_when_dates_mismatch(self):
        server = self._start_server(lambda *args, **kwargs: {})

        try:
            urlopen(
                f"http://127.0.0.1:{server.server_port}/predict_ky_batch?"
                "kh=KH01&bao=BD&ky_list=101,102&date_list=2026-06-17",
                timeout=5,
            )
        except HTTPError as exc:
            self.assertEqual(400, exc.code)
        else:
            self.fail("expected HTTP 400 for mismatched date_list")

    def test_endpoint_returns_400_when_ky_list_invalid(self):
        server = self._start_server(lambda *args, **kwargs: {})

        try:
            urlopen(
                f"http://127.0.0.1:{server.server_port}/predict_ky_batch?"
                "kh=KH01&bao=BD&ky_list=foo&date_list=2026-06-17",
                timeout=5,
            )
        except HTTPError as exc:
            self.assertEqual(400, exc.code)
        else:
            self.fail("expected HTTP 400 for invalid ky_list")

    def test_endpoint_returns_400_when_date_list_invalid(self):
        server = self._start_server(lambda *args, **kwargs: {})

        try:
            urlopen(
                f"http://127.0.0.1:{server.server_port}/predict_ky_batch?"
                "kh=KH01&bao=BD&ky_list=101&date_list=2026/06/17",
                timeout=5,
            )
        except HTTPError as exc:
            self.assertEqual(400, exc.code)
        else:
            self.fail("expected HTTP 400 for invalid date_list")

    def test_endpoint_returns_400_when_target_unknown(self):
        server = self._start_server(lambda *args, **kwargs: {})

        try:
            urlopen(
                f"http://127.0.0.1:{server.server_port}/predict_ky_batch?"
                "kh=KH01&bao=BD&ky_list=101&date_list=2026-06-17&target=bogus",
                timeout=5,
            )
        except HTTPError as exc:
            self.assertEqual(400, exc.code)
        else:
            self.fail("expected HTTP 400 for unknown target")


class CrossLearnHelpersTests(unittest.TestCase):
    def test_normalise_timestamp_accepts_various_inputs(self):
        cases = [
            "2026-06-17",
            pd.Timestamp("2026-06-18"),
            datetime(2026, 6, 19),
            np.datetime64("2026-06-20"),
        ]
        normalised = [crosslearn._normalise_timestamp(value) for value in cases]
        self.assertTrue(all(isinstance(value, pd.Timestamp) for value in normalised))
        self.assertEqual(
            [pd.Timestamp(value) for value in cases],
            normalised,
        )

    def test_median_gap_days_ignores_zero_or_negative_gaps(self):
        pool = pd.DataFrame(
            {
                "NgayNhan": pd.to_datetime(
                    [
                        "2026-06-01",
                        "2026-06-01",  # zero gap, drop
                        "2026-06-05",
                        "2026-06-05",  # zero gap, drop
                        "2026-06-08",
                        "2026-06-09",
                        "2026-06-12",
                    ]
                ),
            }
        )
        # Positive gaps: 1, 3, 4 -> median = 3
        self.assertEqual(3.0, crosslearn._median_gap_days(pool))

    def test_median_gap_days_falls_back_to_stored_gap_column(self):
        pool = pd.DataFrame(
            {
                "NgayNhan": pd.to_datetime(
                    ["2026-06-01", "2026-06-01"]  # both equal -> zero gap, fall back to stored
                ),
                "GapDays": [2.0, 4.0],
            }
        )
        # Positive stored gaps: 2, 4 -> median = 3
        self.assertEqual(3.0, crosslearn._median_gap_days(pool))

    def test_median_gap_days_defaults_when_no_history(self):
        empty = pd.DataFrame({"NgayNhan": pd.to_datetime([])})
        self.assertEqual(4.0, crosslearn._median_gap_days(empty))

    def test_predict_for_ky_delegates_to_batches(self):
        with patch.object(
            crosslearn,
            "predict_batches_for_ky",
            return_value=[
                {
                    "KyBao": 101,
                    "Model_TongSoLuongBanThucTe": "CrossLearn-HistGBR",
                    "Pred_TongSoLuongBanThucTe": 61.0,
                    "is_actual": False,
                    "NgayNhan": "2026-06-17",
                }
            ],
        ) as delegate:
            result = crosslearn.predict_for_ky("KH01", "BD", 101, TARGET)

        self.assertEqual(1, delegate.call_count)
        self.assertIsInstance(result, dict)
        self.assertEqual(61.0, result[f"Pred_{TARGET}"])

    def test_predict_batches_marks_actual_periods_when_known(self):
        pool = make_pool()
        with patch.object(
            crosslearn,
            "_get_pool_and_model",
            return_value=(pool, RecordingModel()),
        ):
            result = crosslearn.predict_batches_for_ky(
                "KH01",
                "BD",
                [100, 101],
                TARGET,
                date_list=[
                    pd.Timestamp("2026-06-15"),
                    pd.Timestamp("2026-06-17"),
                ],
            )

        self.assertTrue(result[0]["is_actual"])
        self.assertFalse(result[1]["is_actual"])

    def test_predict_batches_skips_kys_with_no_history(self):
        # If user requests a ky below all known ky for the pair, the function should
        # skip it gracefully and still emit the rest.
        model = RecordingModel()
        with patch.object(
            crosslearn, "_get_pool_and_model", return_value=(make_pool(), model)
        ):
            result = crosslearn.predict_batches_for_ky(
                "KH01", "BD", [10, 101], TARGET,
                date_list=[pd.Timestamp("2026-05-01"), pd.Timestamp("2026-06-17")],
            )

        self.assertEqual(1, len(result))
        self.assertEqual(101, result[0]["KyBao"])

    def test_predict_batches_rejects_duplicate_requested_kys(self):
        with self.assertRaises(ValueError):
            crosslearn.predict_batches_for_ky(
                "KH01", "BD", [101, 101], TARGET,
                date_list=[pd.Timestamp("2026-06-17"), pd.Timestamp("2026-06-21")],
            )

    def test_predict_batches_raises_when_date_list_mismatched(self):
        with self.assertRaises(ValueError):
            crosslearn.predict_batches_for_ky(
                "KH01", "BD", [101, 102], TARGET,
                date_list=[pd.Timestamp("2026-06-17")],
            )

    def test_normalise_timestamp_rejects_nan(self):
        with self.assertRaises(ValueError):
            crosslearn._normalise_timestamp(pd.NaT)

    def test_build_future_features_rejects_zero_or_negative_gap(self):
        history = make_pool().copy()
        with self.assertRaises(ValueError):
            crosslearn._build_future_features(history, pd.Timestamp("2026-06-15"))

    def test_build_future_features_handles_short_history_with_lag_fallback(self):
        # Construct a minimal history that needs lag fallback.
        history = pd.DataFrame(
            {
                "KyBao": [1, 2],
                "NgayNhan": pd.to_datetime(["2026-06-01", "2026-06-05"]),
                "y": [10.0, 20.0],
            }
        )
        # 2026-06-10 is a Wednesday (dayofweek=2)
        frame = crosslearn._build_future_features(history, pd.Timestamp("2026-06-10"))
        # Lag_3..Lag_5 fall back to first value
        self.assertEqual(10.0, frame.iloc[0]["Lag_3"])
        self.assertEqual(10.0, frame.iloc[0]["Lag_5"])
        self.assertEqual(20.0, frame.iloc[0]["Lag_1"])
        self.assertEqual(5.0, frame.iloc[0]["GapDays"])
        self.assertEqual(2, frame.iloc[0]["DayOfWeek"])

    def test_predict_batches_returns_empty_for_unknown_pair(self):
        with patch.object(crosslearn, "_get_pool_and_model", return_value=(make_pool(), RecordingModel())):
            result = crosslearn.predict_batches_for_ky(
                "UNKNOWN", "BD", [101], TARGET,
                date_list=[pd.Timestamp("2026-06-17")],
            )
        self.assertEqual([], result)

    def test_predict_batches_uses_median_gap_when_date_not_provided(self):
        # When date_list is None, the function must infer forecast_date from median gap.
        model = RecordingModel()
        with patch.object(crosslearn, "_get_pool_and_model", return_value=(make_pool(), model)):
            crosslearn.predict_batches_for_ky("KH01", "BD", [101], TARGET)

        self.assertEqual(3.0, model.frames[0].iloc[0]["GapDays"])

    def test_fit_and_predict_all_emits_one_row_per_pair(self):
        feature_pool = pd.DataFrame(
            {
                "MaKhachHang": ["KH01", "KH01", "KH02", "KH02"],
                "MaBao": ["BD", "BD", "BD", "BD"],
                "KyBao": [1, 2, 1, 2],
                "NgayNhan": pd.to_datetime(
                    [
                        "2026-06-01",
                        "2026-06-05",
                        "2026-06-01",
                        "2026-06-05",
                    ]
                ),
                "GapDays": [4.0, 4.0, 4.0, 4.0],
                "Lag_1": [10.0, 12.0, 20.0, 22.0],
                "Lag_2": [9.0, 11.0, 19.0, 21.0],
                "Lag_3": [8.0, 10.0, 18.0, 20.0],
                "Lag_4": [7.0, 9.0, 17.0, 19.0],
                "Lag_5": [6.0, 8.0, 16.0, 18.0],
                "Rolling_Mean_3": [9.0, 11.0, 19.0, 21.0],
                "Rolling_Mean_5": [8.0, 10.0, 18.0, 20.0],
                "Momentum": [0.1, 0.1, 0.1, 0.1],
                "DayOfWeek": [0, 4, 0, 4],
                "y": [10.0, 12.0, 20.0, 24.0],
            }
        )
        with patch.object(crosslearn, "build_pooled", return_value=feature_pool):
            result = crosslearn.fit_and_predict_all(TARGET)

        self.assertEqual(2, len(result))
        self.assertEqual({"KH01", "KH02"}, set(result["MaKhachHang"]))
        self.assertEqual({"BD"}, set(result["MaBao"]))
        self.assertEqual(3, result["NextKyBao"].iloc[0])

    def test_get_pool_and_model_uses_cache(self):
        sentinel_pool = make_pool()
        sentinel_model = RecordingModel()
        crosslearn._POOL_CACHE.clear()
        crosslearn._MODEL_CACHE.clear()

        with patch.object(
            crosslearn, "build_pooled", return_value=sentinel_pool
        ) as build_mock:
            pool1, model1 = crosslearn._get_pool_and_model(TARGET)
            pool2, model2 = crosslearn._get_pool_and_model(TARGET)

        self.assertIs(pool1, pool2)
        self.assertIs(model1, model2)
        self.assertEqual(1, build_mock.call_count)


if __name__ == "__main__":
    unittest.main()
