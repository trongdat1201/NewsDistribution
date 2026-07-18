using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DATNWF.Models
{
    public class DbHelper
    {
        private static DbHelper _instance;
        public static DbHelper Instance => _instance ?? (_instance = new DbHelper());

        private readonly string _connectionString;

        private DbHelper()
        {
            _connectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["DATNWF.Properties.Settings.ThanhnienConnectionString"]
                .ConnectionString;
        }

        public string ConnectionString => _connectionString;

        public SqlConnection CreateConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

        public int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand(sql, conn) { CommandTimeout = 60 })
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, CancellationToken ct = default, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync(ct);

                using (var cmd = new SqlCommand(sql, conn) { CommandTimeout = 60 })
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    return await cmd.ExecuteNonQueryAsync(ct);
                }
            }
        }

        public SqlDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            var conn = CreateConnection();
            var cmd = new SqlCommand(sql, conn) { CommandTimeout = 60 };
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        public object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand(sql, conn) { CommandTimeout = 60 })
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                var result = cmd.ExecuteScalar();
                return result ?? DBNull.Value;
            }
        }

        public T ExecuteScalar<T>(string sql, params SqlParameter[] parameters)
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand(sql, conn) { CommandTimeout = 60 })
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return default(T);
                return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, CancellationToken ct = default, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync(ct);

                using (var cmd = new SqlCommand(sql, conn) { CommandTimeout = 60 })
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    using (ct.Register(() => { try { cmd.Cancel(); } catch { } }))
                    {
                        var result = await cmd.ExecuteScalarAsync(ct);
                        if (result == null || result == DBNull.Value)
                            return default(T);
                        return (T)Convert.ChangeType(result, typeof(T));
                    }
                }
            }
        }

        public DataTable FillDataTable(string sql, params SqlParameter[] parameters)
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand(sql, conn) { CommandTimeout = 60 })
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public async Task<DataTable> FillDataTableAsync(string sql, CancellationToken cancellationToken = default, params SqlParameter[] parameters)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException(cancellationToken);

            var dt = new DataTable();

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync(cancellationToken);

                using (var cmd = new SqlCommand(sql, conn) { CommandTimeout = 60 })
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    using (cancellationToken.Register(() =>
                    {
                        try { cmd.Cancel(); } catch { /* command may already have completed */ }
                    }))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
            return dt;
        }

        /// <summary>
        /// Lay N ky gan nhat cua (kh, bao) tu database.
        /// </summary>
        public System.Collections.Generic.List<ForecastKyPrediction> GetRecentHistory(
            string maKH, string maBao, int count = 5)
        {
            string sql = @"
                SELECT TOP (@count) cthd.soBao, cthd.soLuongThuc, cthd.dieuPhoi, cthd.soLuongPhatSinh, cthd.ngayNhan
                FROM tabCHITIETHOADON cthd WITH (NOLOCK)
                INNER JOIN tabHOADON hd WITH (NOLOCK) ON hd.sohd = cthd.sohd
                WHERE hd.makh = @maKH AND cthd.maBao = @maBao
                ORDER BY cthd.soBao DESC";

            var result = new System.Collections.Generic.List<ForecastKyPrediction>();
            var dt = FillDataTable(sql,
                new SqlParameter("@maKH", maKH),
                new SqlParameter("@maBao", maBao),
                new SqlParameter("@count", count));

            foreach (DataRow row in dt.Rows)
            {
                result.Add(new ForecastKyPrediction
                {
                    KyBao = row["soBao"] != DBNull.Value ? Convert.ToInt32(row["soBao"]) : 0,
                    PredSLBan = row["soLuongThuc"] != DBNull.Value ? Convert.ToDouble(row["soLuongThuc"]) : 0,
                    PredSLPhatHanh = row["dieuPhoi"] != DBNull.Value ? Convert.ToDouble(row["dieuPhoi"]) : 0,
                    NgayNhan = row["ngayNhan"] != DBNull.Value ? Convert.ToDateTime(row["ngayNhan"]) : DateTime.MinValue,
                    IsActual = true,
                });
            }
            return result;
        }
    }

    public class ForecastKyPrediction
    {
        public int KyBao { get; set; }
        public double PredSLBan { get; set; }
        public double PredSLPhatHanh { get; set; }
        public DateTime NgayNhan { get; set; }
        public bool IsActual { get; set; }
    }
}
