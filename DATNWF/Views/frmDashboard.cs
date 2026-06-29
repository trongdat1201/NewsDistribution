using Guna.Charts.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DATNWF.Models;
using DATNWF.Models.DTO;

namespace DATNWF.Views
{
    public partial class frmDashboard : Form
    {
        public frmDashboard()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Load += frmDashboard_Load;
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            this.SuspendLayout();
            try
            {
                DashboardSummaryDto data = ApiClient.Instance.GetAsync<DashboardSummaryDto>("Dashboard").GetAwaiter().GetResult();
                if (data != null)
                {
                    // 1. Load Metric Cards
                    lblTotalRevenue.Text = data.TongDoanhThu.ToString("N0") + " đ";
                    lblTotalNewspapers.Text = data.TongSoBao.ToString("N0") + " tờ";
                    lblTotalCustomer.Text = data.TongKhachHang.ToString("N0");

                    // 2. Load Pie Chart
                    LoadPieChart(data.TyTrongDoanhThu);

                    // 3. Load Line Chart
                    LoadLineChart(data.BienDongDoanhThu);

                    // 4. Load Grouped Bar Chart
                    LoadGroupedBarChart(data.ThongKeTonKho);

                    // 5. Load Horizontal Bar Chart
                    LoadHorizontalBarChart(data.KhachHangTiemNang);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu Dashboard từ API: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.ResumeLayout();
        }

        private void LoadPieChart(List<ThongKeTronDto> list)
        {
            chartRevenueShare.Datasets.Clear();
            var pieDataset = new GunaPieDataset();

            pieDataset.FillColors.AddRange(new Color[]
            {
                Color.FromArgb(0, 255, 255),   
                Color.FromArgb(255, 0, 255),   
                Color.FromArgb(100, 149, 237), 
                Color.FromArgb(127, 255, 212), 
                Color.FromArgb(240, 248, 255)  
            });

            if (list != null)
            {
                foreach (var item in list)
                {
                    pieDataset.DataPoints.Add(item.TenBao, (double)item.DoanhThu);
                }
            }

            chartRevenueShare.Datasets.Add(pieDataset);
            chartRevenueShare.Update();
        }

        private void LoadLineChart(List<ThongKeDuongDto> list)
        {
            chartRevenueTimeline.Datasets.Clear();

            var lineDataset = new GunaLineDataset();
            lineDataset.Label = "Doanh thu (Triệu VNĐ)";
            lineDataset.BorderColor = Color.Cyan;
            lineDataset.FillColor = Color.FromArgb(50, 0, 255, 255);
            lineDataset.PointFillColors.Add(Color.Magenta);
            lineDataset.PointBorderColors.Add(Color.White);
            lineDataset.BorderWidth = 2;

            if (list != null)
            {
                foreach (var item in list)
                {
                    lineDataset.DataPoints.Add(item.Ngay, item.DoanhThu);
                }
            }

            chartRevenueTimeline.Datasets.Add(lineDataset);
            chartRevenueTimeline.YAxes.GridLines.Display = true;
            chartRevenueTimeline.YAxes.GridLines.Color = Color.FromArgb(40, 255, 255, 255);
            chartRevenueTimeline.XAxes.GridLines.Display = false;
            chartRevenueTimeline.Update();
        }

        private void LoadGroupedBarChart(List<ThongKeCotDto> list)
        {
            chartInventory.Datasets.Clear();

            var dsPhatHanh = new GunaBarDataset { Label = "Phát hành" };
            var dsTieuThu = new GunaBarDataset { Label = "Tiêu thụ" };
            var dsTonKho = new GunaBarDataset { Label = "Tồn kho" };

            dsPhatHanh.FillColors.Add(Color.FromArgb(100, 149, 237)); 
            dsTieuThu.FillColors.Add(Color.FromArgb(0, 255, 255));   
            dsTonKho.FillColors.Add(Color.FromArgb(255, 0, 102));    

            dsPhatHanh.CornerRadius = 3;
            dsTieuThu.CornerRadius = 3;
            dsTonKho.CornerRadius = 3;

            if (list != null)
            {
                foreach (var item in list)
                {
                    dsPhatHanh.DataPoints.Add(item.Ngay, item.PhatHanh);
                    dsTieuThu.DataPoints.Add(item.Ngay, item.TieuThu);
                    dsTonKho.DataPoints.Add(item.Ngay, item.TonKho);
                }
            }

            chartInventory.Datasets.Add(dsPhatHanh);
            chartInventory.Datasets.Add(dsTieuThu);
            chartInventory.Datasets.Add(dsTonKho);

            chartInventory.YAxes.GridLines.Display = true;
            chartInventory.YAxes.GridLines.Color = Color.FromArgb(40, 255, 255, 255);
            chartInventory.XAxes.GridLines.Display = false;
            chartInventory.Legend.Position = Guna.Charts.WinForms.LegendPosition.Top;
            chartInventory.Update();
        }

        private void LoadHorizontalBarChart(List<TopKhachHangDto> list)
        {
            chartTopCustomers.Datasets.Clear();

            var hBarDataset = new Guna.Charts.WinForms.GunaHorizontalBarDataset();
            hBarDataset.Label = "Tổng lượng mua (Tờ)";
            hBarDataset.FillColors.Add(Color.FromArgb(0, 204, 255));
            hBarDataset.CornerRadius = 4;

            if (list != null)
            {
                foreach (var item in list)
                {
                    hBarDataset.DataPoints.Add(item.TenKhachHang, item.SoLuongMua);
                }
            }

            chartTopCustomers.Datasets.Add(hBarDataset);
            chartTopCustomers.XAxes.GridLines.Display = true;
            chartTopCustomers.XAxes.GridLines.Color = Color.FromArgb(40, 255, 255, 255);
            chartTopCustomers.YAxes.GridLines.Display = false;
            chartTopCustomers.Update();
        }
    }
}
