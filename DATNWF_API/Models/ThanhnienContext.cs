using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DATNWF_API.Models;

public partial class ThanhnienContext : DbContext
{
    public ThanhnienContext()
    {
    }

    public ThanhnienContext(DbContextOptions<ThanhnienContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TabBao> TabBaos { get; set; }

    public virtual DbSet<TabBaoNgoaiLe> TabBaoNgoaiLes { get; set; }

    public virtual DbSet<TabChiTietDieuPhoi> TabChiTietDieuPhois { get; set; }

    public virtual DbSet<TabChitiethoadon> TabChitiethoadons { get; set; }

    public virtual DbSet<TabDieuPhoi> TabDieuPhois { get; set; }

    public virtual DbSet<TabHoadon> TabHoadons { get; set; }

    public virtual DbSet<TabKhachhang> TabKhachhangs { get; set; }

    public virtual DbSet<TabKhachhang1> TabKhachhang1s { get; set; }

    public virtual DbSet<TabLogin> TabLogins { get; set; }

    public virtual DbSet<TabTon> TabTons { get; set; }

    public virtual DbSet<TabTonOld> TabTonOlds { get; set; }

    public virtual DbSet<TabUser> TabUsers { get; set; }

    public virtual DbSet<Tam> Tams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Thanhnien;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Vietnamese_CI_AS");

        modelBuilder.Entity<TabBao>(entity =>
        {
            entity.HasKey(e => e.MaBao).IsClustered(false);

            entity.ToTable("tabBAO", tb =>
                {
                    tb.HasTrigger("triBAODelete");
                    tb.HasTrigger("triBAOupdate");
                });

            entity.Property(e => e.MaBao)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("maBao");
            entity.Property(e => e.DonGia).HasColumnName("donGia");
            entity.Property(e => e.Dvt)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("DVT");
            entity.Property(e => e.NgayBatDau)
                .HasColumnType("datetime")
                .HasColumnName("ngayBatDau");
            entity.Property(e => e.SoLanPhtrongTuan).HasColumnName("soLanPHtrongTuan");
            entity.Property(e => e.Sogoc).HasColumnName("sogoc");
            entity.Property(e => e.Ten)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ten");
            entity.Property(e => e.Thu1).HasColumnName("thu1");
            entity.Property(e => e.Thu2).HasColumnName("thu2");
            entity.Property(e => e.Thu3).HasColumnName("thu3");
            entity.Property(e => e.Thu4).HasColumnName("thu4");
            entity.Property(e => e.Thu5).HasColumnName("thu5");
            entity.Property(e => e.Thu6).HasColumnName("thu6");
            entity.Property(e => e.Thu7).HasColumnName("thu7");
        });

        modelBuilder.Entity<TabBaoNgoaiLe>(entity =>
        {
            entity.HasKey(e => new { e.MaBao, e.NgayPhatHanh })
                .HasName("PK_tabBao(ngoaiLe)")
                .IsClustered(false);

            entity.ToTable("tabBao_ngoaiLe");

            entity.Property(e => e.MaBao)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("maBao");
            entity.Property(e => e.NgayPhatHanh)
                .HasColumnType("datetime")
                .HasColumnName("ngayPhatHanh");
            entity.Property(e => e.SoLanTrongNam).HasColumnName("soLanTrongNam");

            entity.HasOne(d => d.MaBaoNavigation).WithMany(p => p.TabBaoNgoaiLes)
                .HasForeignKey(d => d.MaBao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BAONGOAILE_BAO");
        });

        modelBuilder.Entity<TabChiTietDieuPhoi>(entity =>
        {
            entity.HasKey(e => new { e.Sohd, e.NgayNhan, e.MaBao })
                .HasName("PK_tabDieuPhoi")
                .IsClustered(false);

            entity.ToTable("tabChiTietDieuPhoi");

            entity.Property(e => e.Sohd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("sohd");
            entity.Property(e => e.NgayNhan)
                .HasColumnType("datetime")
                .HasColumnName("ngayNhan");
            entity.Property(e => e.MaBao)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("maBao");
            entity.Property(e => e.DonGia)
                .HasColumnType("money")
                .HasColumnName("donGia");
            entity.Property(e => e.Sobao).HasColumnName("sobao");
            entity.Property(e => e.SoluongBan).HasColumnName("soluongBan");
            entity.Property(e => e.SoluongDieuPhoi).HasColumnName("soluongDieuPhoi");
            entity.Property(e => e.Tenbao)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("tenbao");
            entity.Property(e => e.ThanhTien)
                .HasColumnType("money")
                .HasColumnName("thanhTien");

            entity.HasOne(d => d.MaBaoNavigation).WithMany(p => p.TabChiTietDieuPhois)
                .HasForeignKey(d => d.MaBao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDP_BAO");

            entity.HasOne(d => d.SohdNavigation).WithMany(p => p.TabChiTietDieuPhois)
                .HasForeignKey(d => d.Sohd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTDP_DIEUPHOI");
        });

        modelBuilder.Entity<TabChitiethoadon>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tabCHITIETHOADON");

            entity.Property(e => e.DieuPhoi).HasColumnName("dieuPhoi");
            entity.Property(e => e.DonGia).HasColumnName("donGia");
            entity.Property(e => e.MaBao)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("maBao");
            entity.Property(e => e.NgayNhan)
                .HasColumnType("datetime")
                .HasColumnName("ngayNhan");
            entity.Property(e => e.SoBao).HasColumnName("soBao");
            entity.Property(e => e.SoLuongDu).HasColumnName("soLuongDu");
            entity.Property(e => e.SoLuongThuc).HasColumnName("soLuongThuc");
            entity.Property(e => e.Sohd)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("sohd");
            entity.Property(e => e.Soluongphatsinh1).HasColumnName("soluongphatsinh1");
            entity.Property(e => e.TenBao)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("tenBao");
            entity.Property(e => e.ThanhTien).HasColumnName("thanhTien");

            entity.HasOne(d => d.MaBaoNavigation).WithMany()
                .HasForeignKey(d => d.MaBao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHD_BAO");

            entity.HasOne(d => d.SohdNavigation).WithMany()
                .HasForeignKey(d => d.Sohd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHD_HOADON");
        });

        modelBuilder.Entity<TabDieuPhoi>(entity =>
        {
            entity.HasKey(e => e.SoHd).HasName("PK_tabDieuPhoi_1");

            entity.ToTable("tabDieuPhoi");

            entity.Property(e => e.SoHd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("soHD");
            entity.Property(e => e.Denngay)
                .HasColumnType("datetime")
                .HasColumnName("denngay");
            entity.Property(e => e.GhiChu)
                .HasMaxLength(200)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ghiChu");
            entity.Property(e => e.Makh)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("makh");
            entity.Property(e => e.Ngay)
                .HasColumnType("datetime")
                .HasColumnName("ngay");
            entity.Property(e => e.Tungay)
                .HasColumnType("datetime")
                .HasColumnName("tungay");

            entity.HasOne(d => d.MakhNavigation).WithMany(p => p.TabDieuPhois)
                .HasForeignKey(d => d.Makh)
                .HasConstraintName("FK_DIEUPHOI_KHACHHANG");
        });

        modelBuilder.Entity<TabHoadon>(entity =>
        {
            entity.HasKey(e => e.Sohd).IsClustered(false);

            entity.ToTable("tabHOADON", tb => tb.HasTrigger("triDeleteTabHOADON"));

            entity.Property(e => e.Sohd)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("sohd");
            entity.Property(e => e.DenNgay)
                .HasColumnType("datetime")
                .HasColumnName("denNgay");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(200)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ghichu");
            entity.Property(e => e.Makh)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("makh");
            entity.Property(e => e.NgayLapPhieu)
                .HasColumnType("datetime")
                .HasColumnName("ngayLapPhieu");
            entity.Property(e => e.ThanhToan).HasColumnName("thanhToan");
            entity.Property(e => e.TuNgay)
                .HasColumnType("datetime")
                .HasColumnName("tuNgay");

            entity.HasOne(d => d.MakhNavigation).WithMany(p => p.TabHoadons)
                .HasForeignKey(d => d.Makh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HOADON_KHACHHANG");
        });

        modelBuilder.Entity<TabKhachhang>(entity =>
        {
            entity.HasKey(e => e.Makh)
                .HasName("PK_tabKHACHHANG_1")
                .IsClustered(false);

            entity.ToTable("tabKHACHHANG");

            entity.Property(e => e.Makh)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("MAKH");
            entity.Property(e => e.Chietkhau).HasColumnName("CHIETKHAU");
            entity.Property(e => e.Diachi)
                .HasMaxLength(300)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("DIACHI");
            entity.Property(e => e.Dienthoai)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("DIENTHOAI");
            entity.Property(e => e.PKt).HasColumnName("P_KT");
            entity.Property(e => e.PPh).HasColumnName("P_PH");
            entity.Property(e => e.Ten)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("TEN");
            entity.Property(e => e.Uutien)
                .HasMaxLength(10)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("UUTIEN");
        });

        modelBuilder.Entity<TabKhachhang1>(entity =>
        {
            entity.HasKey(e => e.Makh)
                .HasName("PK_tabKHACHHANG")
                .IsClustered(false);

            entity.ToTable("tabKHACHHANG1", tb =>
                {
                    tb.HasTrigger("triDelete");
                    tb.HasTrigger("triKHACHHANGupdate");
                });

            entity.Property(e => e.Makh)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("makh");
            entity.Property(e => e.ChietKhau).HasColumnName("chietKhau");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(300)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("diaChi");
            entity.Property(e => e.DienThoai)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("dienThoai");
            entity.Property(e => e.PKt).HasColumnName("P_KT");
            entity.Property(e => e.PPh).HasColumnName("P_PH");
            entity.Property(e => e.Ten)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("ten");
            entity.Property(e => e.UuTien)
                .HasMaxLength(10)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("uuTien");
        });

        modelBuilder.Entity<TabLogin>(entity =>
        {
            entity.HasKey(e => e.TenDangNhap).HasName("PK__tabLogin__59267D4BC6873CBE");

            entity.ToTable("tabLogin");

            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .HasColumnName("tenDangNhap");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("Role");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(100)
                .HasColumnName("matKhau");
        });

        modelBuilder.Entity<TabTon>(entity =>
        {
            entity.HasKey(e => new { e.Ngay, e.MaBao }).IsClustered(false);

            entity.ToTable("tabTon");

            entity.Property(e => e.Ngay)
                .HasColumnType("datetime")
                .HasColumnName("ngay");
            entity.Property(e => e.MaBao)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("maBao");
            entity.Property(e => e.BanLe).HasColumnName("banLe");
            entity.Property(e => e.Banthuc).HasColumnName("banthuc");
            entity.Property(e => e.DieuPhoi).HasColumnName("dieuPhoi");
            entity.Property(e => e.SlPhatHanh).HasColumnName("slPhatHanh");
            entity.Property(e => e.SoBao).HasColumnName("soBao");
            entity.Property(e => e.TenBao)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("tenBao");
            entity.Property(e => e.Ton).HasColumnName("ton");

            entity.HasOne(d => d.MaBaoNavigation).WithMany(p => p.TabTons)
                .HasForeignKey(d => d.MaBao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TON_BAO");
        });

        modelBuilder.Entity<TabTonOld>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tabTon_old");

            entity.Property(e => e.BanLe).HasColumnName("banLe");
            entity.Property(e => e.Banthuc).HasColumnName("banthuc");
            entity.Property(e => e.DieuPhoi).HasColumnName("dieuPhoi");
            entity.Property(e => e.MaBao)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("maBao");
            entity.Property(e => e.Ngay)
                .HasColumnType("datetime")
                .HasColumnName("ngay");
            entity.Property(e => e.SlPhatHanh).HasColumnName("slPhatHanh");
            entity.Property(e => e.SoBao).HasColumnName("soBao");
            entity.Property(e => e.TenBao)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("tenBao");
            entity.Property(e => e.Ton).HasColumnName("ton");
        });

        modelBuilder.Entity<TabUser>(entity =>
        {
            entity.HasKey(e => e.TenDangNhap).IsClustered(false);

            entity.ToTable("tabUser");

            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(10)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("tenDangNhap");
            entity.Property(e => e.Bc1).HasColumnName("BC1");
            entity.Property(e => e.Bc2).HasColumnName("BC2");
            entity.Property(e => e.Bc3).HasColumnName("BC3");
            entity.Property(e => e.HoTen)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("hoTen");
            entity.Property(e => e.Ht1).HasColumnName("HT1");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(10)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("matKhau");
            entity.Property(e => e.Nv1).HasColumnName("NV1");
            entity.Property(e => e.Nv2).HasColumnName("NV2");
            entity.Property(e => e.Nv3).HasColumnName("NV3");
            entity.Property(e => e.Nv4).HasColumnName("NV4");
            entity.Property(e => e.Nv5).HasColumnName("NV5");
            entity.Property(e => e.St1).HasColumnName("ST1");
            entity.Property(e => e.St2).HasColumnName("ST2");
        });

        modelBuilder.Entity<Tam>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tam");

            entity.Property(e => e.Makh)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("makh");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
