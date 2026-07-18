using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace DATNWF.Models
{
    public class PrintInvoiceService : PrintDocument
    {
        private readonly PrintPreviewDialog _preview = new PrintPreviewDialog();
        private readonly PrintDialog _printDialog = new PrintDialog();

        private string _soHD;
        private Dictionary<string, object> _hdInfo;
        private DataTable _dtChiTiet;

        private readonly Font _fontHeader = new Font("Segoe UI", 14, FontStyle.Bold);
        private readonly Font _fontSub = new Font("Segoe UI", 11, FontStyle.Bold);
        private readonly Font _fontNormal = new Font("Segoe UI", 10);
        private readonly Font _fontSmall = new Font("Segoe UI", 9);

        private readonly StringFormat _sfCenter = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        private readonly StringFormat _sfLeft = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center
        };
        private readonly StringFormat _sfRight = new StringFormat
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Center
        };

        private int _rowIndex;
        private int _totalPages;
        private int _currentPage;
        private int _rowsPerPage;

        public void Print(string soHD, Dictionary<string, object> hdInfo, DataTable dtChiTiet)
        {
            _soHD = soHD;
            _hdInfo = hdInfo;
            _dtChiTiet = dtChiTiet;

            _rowIndex = 0;
            _currentPage = 0;
            _rowsPerPage = 12;

            CalculateTotalPages();

            PrinterSettings = new PrinterSettings();
            PrintPage += OnPrintPage;
            PrintController = new StandardPrintController();

            _preview.Document = this;
            _preview.StartPosition = FormStartPosition.CenterScreen;
            _preview.Width = 900;
            _preview.Height = 700;

            var result = MessageBox.Show(
                "Bạn muốn xem trước hay in trực tiếp?\n\n[Yes] = Xem trước\n[No] = In\n[Cancel] = Hủy",
                "In hóa đơn",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _preview.ShowDialog();
            }
            else if (result == DialogResult.No)
            {
                _printDialog.Document = this;
                if (_printDialog.ShowDialog() == DialogResult.OK)
                    Print();
            }
        }

        private void CalculateTotalPages()
        {
            int totalRows = _dtChiTiet?.Rows.Count ?? 0;
            _totalPages = (int)Math.Ceiling(totalRows / (double)_rowsPerPage);
            if (_totalPages == 0) _totalPages = 1;
        }

        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            if (_dtChiTiet == null) return;

            Graphics g = e.Graphics;
            Rectangle r = e.MarginBounds;
            int marginL = r.Left;
            int marginT = r.Top;
            int pageW = r.Width;
            int y = marginT;

            // ── HEADER ──────────────────────────────────────────────
            using (var brush = new SolidBrush(Color.FromArgb(240, 165, 0)))
            {
                g.FillRectangle(brush, marginL, y, pageW, 36);
            }

            g.DrawString("HÓA ĐƠN BÁO", _fontHeader, Brushes.White,
                new RectangleF(marginL, y, pageW, 36), _sfCenter);
            y += 40;

            // ── Info khách hàng ────────────────────────────────────
            string tenKH = _hdInfo != null && _hdInfo.TryGetValue("tenKhachHang", out var vTen) ? vTen?.ToString() ?? "" : "";
            string sdt = _hdInfo != null && _hdInfo.TryGetValue("sdtKhachHang", out var vSdt) ? vSdt?.ToString() ?? "" : "";
            var dtTuNgay = _hdInfo != null && _hdInfo.TryGetValue("tuNgay", out var vTu) && vTu is DateTime d1 ? d1 : DateTime.MinValue;
            var dtDenNgay = _hdInfo != null && _hdInfo.TryGetValue("denNgay", out var vDen) && vDen is DateTime d2 ? d2 : DateTime.MinValue;
            decimal ck = 0;
            if (_hdInfo != null && _hdInfo.TryGetValue("chietKhau", out var vCk) && vCk != null && vCk != DBNull.Value)
                ck = Convert.ToDecimal(vCk);

            g.DrawString($"Khách hàng: {tenKH}  |  ĐT: {sdt}", _fontSub, Brushes.Black,
                new RectangleF(marginL, y, pageW, 22), _sfLeft);
            y += 22;

            g.DrawString($"Kỳ: {dtTuNgay:dd/MM/yyyy} – {dtDenNgay:dd/MM/yyyy}  |  CK: {(ck > 0 ? $"{ck:N0}%" : "Không")}  |  Số HĐ: {_soHD}",
                _fontSmall, Brushes.DarkGray,
                new RectangleF(marginL, y, pageW, 18), _sfLeft);
            y += 26;

            var cols = new[] { "Ngày nhận", "Mã báo", "Tên báo", "Số báo", "SL thực", "Dư", "Đơn giá", "Thành tiền" };
            int[] colW = { 80, 70, 130, 60, 60, 50, 80, 90 };
            int[] colX = new int[cols.Length];
            colX[0] = marginL;
            for (int i = 1; i < cols.Length; i++)
                colX[i] = colX[i - 1] + colW[i - 1];

            using (var brush = new SolidBrush(Color.FromArgb(240, 165, 0)))
            {
                g.FillRectangle(brush, marginL, y, pageW, 22);
            }
            g.DrawString("Ngày nhận", _fontSmall, Brushes.White,
                new RectangleF(colX[0], y, colW[0], 22), _sfCenter);
            g.DrawString("Mã báo", _fontSmall, Brushes.White,
                new RectangleF(colX[1], y, colW[1], 22), _sfCenter);
            g.DrawString("Tên báo", _fontSmall, Brushes.White,
                new RectangleF(colX[2], y, colW[2], 22), _sfCenter);
            g.DrawString("Số báo", _fontSmall, Brushes.White,
                new RectangleF(colX[3], y, colW[3], 22), _sfCenter);
            g.DrawString("SL thực", _fontSmall, Brushes.White,
                new RectangleF(colX[4], y, colW[4], 22), _sfCenter);
            g.DrawString("Dư", _fontSmall, Brushes.White,
                new RectangleF(colX[5], y, colW[5], 22), _sfCenter);
            g.DrawString("Đơn giá", _fontSmall, Brushes.White,
                new RectangleF(colX[6], y, colW[6], 22), _sfCenter);
            g.DrawString("Thành tiền", _fontSmall, Brushes.White,
                new RectangleF(colX[7], y, colW[7], 22), _sfCenter);
            y += 22;

            int endIndex = Math.Min(_rowIndex + _rowsPerPage, _dtChiTiet.Rows.Count);
            decimal tongTruocCK = 0;

            for (int i = _rowIndex; i < endIndex; i++)
            {
                var dr = _dtChiTiet.Rows[i];
                bool alt = (i - _rowIndex) % 2 == 1;
                int slThuc = Convert.ToInt32(dr["soLuongThuc"]);
                int slDu = Convert.ToInt32(dr["soLuongDu"]);
                decimal dg = Convert.ToDecimal(dr["donGia"]);
                decimal tt = Convert.ToDecimal(dr["thanhTien"]);
                tongTruocCK += tt;

                Color rowBg = alt ? Color.FromArgb(245, 245, 245) : Color.White;
                if (slDu < 0) rowBg = Color.FromArgb(255, 235, 156); // thừa
                else if (slDu > 0) rowBg = Color.FromArgb(255, 199, 206); // thiếu

                using (var brush = new SolidBrush(rowBg))
                    g.FillRectangle(brush, marginL, y, pageW, 20);

                g.DrawString(dr["ngayNhan"] != DBNull.Value
                    ? ((DateTime)dr["ngayNhan"]).ToString("dd/MM/yy") : "-",
                    _fontSmall, Brushes.Black,
                    new RectangleF(colX[0], y, colW[0], 20), _sfCenter);
                g.DrawString(dr["maBao"]?.ToString() ?? "", _fontSmall, Brushes.Black,
                    new RectangleF(colX[1], y, colW[1], 20), _sfCenter);
                g.DrawString(Truncate(dr["tenBao"]?.ToString() ?? "", 18), _fontSmall, Brushes.Black,
                    new RectangleF(colX[2], y, colW[2], 20), _sfLeft);
                g.DrawString(dr["soBao"]?.ToString() ?? "", _fontSmall, Brushes.Black,
                    new RectangleF(colX[3], y, colW[3], 20), _sfCenter);
                g.DrawString($"{slThuc:N0}", _fontSmall, Brushes.Black,
                    new RectangleF(colX[4], y, colW[4], 20), _sfCenter);
                g.DrawString($"{slDu:N0}", _fontSmall,
                    slDu < 0 ? Brushes.Red : slDu > 0 ? Brushes.OrangeRed : Brushes.Black,
                    new RectangleF(colX[5], y, colW[5], 20), _sfCenter);
                g.DrawString($"{dg:N0}", _fontSmall, Brushes.Black,
                    new RectangleF(colX[6], y, colW[6], 20), _sfCenter);
                g.DrawString($"{tt:N0}", _fontSmall, Brushes.Black,
                    new RectangleF(colX[7], y, colW[7], 20), _sfCenter);

                g.DrawLine(Pens.LightGray, marginL, y + 19, marginL + pageW, y + 19);
                y += 20;
            }

            y += 4;
            g.DrawLine(Pens.Black, colX[6], y, colX[7] + colW[7], y);
            y += 4;

            g.DrawString("Tổng trước CK:", _fontSmall, Brushes.Black,
                new RectangleF(colX[6], y, 80, 20), _sfRight);
            g.DrawString($"{tongTruocCK:N0}", _fontSub, Brushes.Black,
                new RectangleF(colX[7], y, colW[7], 20), _sfCenter);
            y += 22;

            if (ck > 0)
            {
                decimal tienCK = tongTruocCK * ck / 100;
                decimal tongSauCK = tongTruocCK - tienCK;

                g.DrawString($"Chiết khấu ({ck:N0}%):", _fontSmall, Brushes.Black,
                    new RectangleF(colX[6], y, 80, 20), _sfRight);
                g.DrawString($"-{tienCK:N0}", _fontSmall, Brushes.Red,
                    new RectangleF(colX[7], y, colW[7], 20), _sfCenter);
                y += 20;

                g.DrawString("TỔNG CỘNG:", _fontSub, Brushes.Black,
                    new RectangleF(colX[6], y, 80, 24), _sfRight);
                g.DrawString($"{tongSauCK:N0}", _fontSub, Brushes.Black,
                    new RectangleF(colX[7], y, colW[7], 24), _sfCenter);
            }
            else
            {
                g.DrawString("TỔNG CỘNG:", _fontSub, Brushes.Black,
                    new RectangleF(colX[6], y, 80, 24), _sfRight);
                g.DrawString($"{tongTruocCK:N0}", _fontSub, Brushes.Black,
                    new RectangleF(colX[7], y, colW[7], 24), _sfCenter);
            }

            y = e.PageBounds.Bottom - 40;
            g.DrawLine(Pens.Gray, marginL, y, marginL + pageW, y);
            y += 6;
            g.DrawString($"Trang {_currentPage + 1} / {_totalPages}",
                _fontSmall, Brushes.Gray,
                new RectangleF(marginL, y, pageW, 16), _sfCenter);

            _rowIndex += _rowsPerPage;
            _currentPage++;
            e.HasMorePages = _rowIndex < _dtChiTiet.Rows.Count;
            if (!e.HasMorePages)
            {
                _rowIndex = 0;
                _currentPage = 0;
            }
        }

        private static string Truncate(string s, int maxLen)
            => s?.Length > maxLen ? s.Substring(0, maxLen - 1) + "…" : s ?? "";

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fontHeader?.Dispose();
                _fontSub?.Dispose();
                _fontNormal?.Dispose();
                _fontSmall?.Dispose();
                _sfCenter?.Dispose();
                _sfLeft?.Dispose();
                _preview?.Dispose();
                _printDialog?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
