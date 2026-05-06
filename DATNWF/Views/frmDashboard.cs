    using Guna.Charts.WinForms;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATNWF.Views
{
    public partial class frmDashboard : Form
    {

        public frmDashboard()
        {
            InitializeComponent();
            LoadPieChartRevenue();
            LoadLineChartRevenueTimeline();
            LoadGroupedBarChartInventory();
            LoadHorizontalBarChartTopCustomers();
            LoadMetricCards();
        }
        private DataTable GetRevenueData()
        {
            DataTable dt = new DataTable();
            string connString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";

            string query = "SELECT b.ten, SUM(ct.thanhTien) as Tong FROM tabCHITIETHOADON ct JOIN tabBAO b ON ct.maBao = b.maBao GROUP BY b.ten";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message);
            }

            return dt;
        }
        public void LoadPieChartRevenue()
        {
            var pieDataset = new GunaPieDataset();

            pieDataset.FillColors.AddRange(new Color[]
            {
                Color.FromArgb(0, 255, 255),   
                Color.FromArgb(255, 0, 255),   
                Color.FromArgb(100, 149, 237), 
                Color.FromArgb(127, 255, 212), 
                Color.FromArgb(240, 248, 255)  
            });

            DataTable dt = GetRevenueData();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string tenBao = row["ten"].ToString();
                    double tongDoanhThu = Convert.ToDouble(row["Tong"]);

                    pieDataset.DataPoints.Add(tenBao, tongDoanhThu);
                }

                chartRevenueShare.Datasets.Clear();
                chartRevenueShare.Datasets.Add(pieDataset);
                chartRevenueShare.Update();
            }
        }
        private DataTable GetRevenueTimelineData()
        {
            DataTable dt = new DataTable();
            string connString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";

            string query = @"
                SELECT * FROM (
                    SELECT TOP 30
                        CAST(ngayNhan AS DATE) AS Ngay, 
                        SUM(thanhTien) / 1000000 AS TongDoanhThu
                    FROM tabCHITIETHOADON
                    GROUP BY CAST(ngayNhan AS DATE)
                    ORDER BY Ngay DESC
                ) AS SubQuery
                ORDER BY Ngay ASC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi CSDL: " + ex.Message);
            }
            return dt;
        }
        public void LoadLineChartRevenueTimeline()
        {
            chartRevenueTimeline.Datasets.Clear();

            var lineDataset = new GunaLineDataset();
            lineDataset.Label = "Doanh thu (Triệu VNĐ)";

            lineDataset.BorderColor = Color.Cyan;
            lineDataset.FillColor = Color.FromArgb(50, 0, 255, 255);
            lineDataset.PointFillColors.Add(Color.Magenta);
            lineDataset.PointBorderColors.Add(Color.White);
            lineDataset.BorderWidth = 2;

            //lineDataset.PointRadius = 3;
            //lineDataset.PointHoverRadius = 5;
            //lineDataset.Tension = 0.3;

            DataTable dt = GetRevenueTimelineData();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DateTime ngay = Convert.ToDateTime(row["Ngay"]);
                    string nhanTrucX = ngay.ToString("dd/MM");
                    double doanhThu = Convert.ToDouble(row["TongDoanhThu"]);

                    lineDataset.DataPoints.Add(nhanTrucX, doanhThu);
                }

                chartRevenueTimeline.Datasets.Add(lineDataset);
            }

            chartRevenueTimeline.YAxes.GridLines.Display = true;
            chartRevenueTimeline.YAxes.GridLines.Color = Color.FromArgb(40, 255, 255, 255);
            chartRevenueTimeline.XAxes.GridLines.Display = false;
            chartRevenueTimeline.Update();
        }
        private DataTable GetInventoryData()
        {
            DataTable dt = new DataTable();
            string connString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";

            string query = @"
                SELECT * FROM (
                    SELECT TOP 7
                        CAST(ngay AS DATE) AS Ngay,
                        ISNULL(SUM(slPhatHanh), 0) AS TongPhatHanh,
                        ISNULL(SUM(banthuc + banLe + dieuPhoi), 0) AS TongTieuThu,
                        ISNULL(SUM(ton), 0) AS TongTonKho
                    FROM tabTon
                    GROUP BY CAST(ngay AS DATE)
                    ORDER BY Ngay DESC
                ) AS SubQuery
                ORDER BY Ngay ASC";
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi CSDL: " + ex.Message);
            }
            return dt;
        }
        public void LoadGroupedBarChartInventory()
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

            DataTable dt = GetInventoryData();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string ngay = Convert.ToDateTime(row["Ngay"]).ToString("dd/MM");

                    dsPhatHanh.DataPoints.Add(ngay, Convert.ToDouble(row["TongPhatHanh"]));
                    dsTieuThu.DataPoints.Add(ngay, Convert.ToDouble(row["TongTieuThu"]));
                    dsTonKho.DataPoints.Add(ngay, Convert.ToDouble(row["TongTonKho"]));
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
        private DataTable GetTopCustomersData()
        {
            DataTable dt = new DataTable();
            string connString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";

            string query = @"
                SELECT TOP 10 
                    kh.TEN, 
                    ISNULL(SUM(cthd.soLuongThuc + cthd.soLuongPhatSinh), 0) AS TongSoLuong
                FROM tabCHITIETHOADON cthd
                JOIN tabHOADON hd ON cthd.sohd = hd.sohd
                JOIN tabKHACHHANG kh ON hd.makh = kh.MAKH
                GROUP BY kh.TEN
                ORDER BY TongSoLuong ASC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi CSDL: " + ex.Message);
            }
            return dt;
        }
        public void LoadHorizontalBarChartTopCustomers()
        {
            chartTopCustomers.Datasets.Clear();

            var hBarDataset = new Guna.Charts.WinForms.GunaHorizontalBarDataset();
            hBarDataset.Label = "Tổng lượng mua (Tờ)";

            hBarDataset.FillColors.Add(Color.FromArgb(0, 204, 255));
            hBarDataset.CornerRadius = 4;
            //hBarDataset.BarPercentage = 0.6;

            DataTable dt = GetTopCustomersData();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string tenKhachHang = row["TEN"].ToString();
                    double tongSoLuong = Convert.ToDouble(row["TongSoLuong"]);

                    hBarDataset.DataPoints.Add(tenKhachHang, tongSoLuong);
                }
            }

            chartTopCustomers.Datasets.Add(hBarDataset);

            chartTopCustomers.XAxes.GridLines.Display = true;
            chartTopCustomers.XAxes.GridLines.Color = Color.FromArgb(40, 255, 255, 255);
            chartTopCustomers.YAxes.GridLines.Display = false;

            chartTopCustomers.Update();
        }
        public void LoadMetricCards()
        {
            string connString = @"Data Source=DESKTOP-IKRN14J\SQLEXPRESS;Initial Catalog=Thanhnien;Integrated Security=True";

            // Các câu truy vấn lấy con số tổng quát[cite: 2]
            string queryDoanhThu = "SELECT ISNULL(SUM(thanhTien), 0) FROM tabCHITIETHOADON";
            string querySoBao = "SELECT ISNULL(SUM(soLuongThuc + soLuongPhatSinh), 0) FROM tabCHITIETHOADON";
            string queryKhachHang = "SELECT COUNT(MAKH) FROM tabKHACHHANG";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(queryDoanhThu, conn))
                    {
                        double tongDoanhThu = Convert.ToDouble(cmd.ExecuteScalar());
                        lblTotalRevenue.Text = tongDoanhThu.ToString("N0") + " đ";
                    }

                    using (SqlCommand cmd = new SqlCommand(querySoBao, conn))
                    {
                        int tongSoBao = Convert.ToInt32(cmd.ExecuteScalar());
                        lblTotalNewspapers.Text = tongSoBao.ToString("N0") + " tờ";
                    }

                    using (SqlCommand cmd = new SqlCommand(queryKhachHang, conn))
                    {
                        int tongKhach = Convert.ToInt32(cmd.ExecuteScalar());
                        lblTotalCustomer.Text = tongKhach.ToString("N0");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải Metric Cards: " + ex.Message);
            }
        }
    }
}
