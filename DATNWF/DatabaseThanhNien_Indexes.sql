-- ================================================================
-- MIGRATION: Add_indexes_for_performance.sql
-- Purpose: Tạo index TRÊN CỘT ĐÃ CO SẴN (không thay đổi cấu trúc bảng)
-- Safety:   Chi tao CREATE INDEX — khong ALTER TABLE, khong tao cot moi
-- Run on:   SQL Server 2016+
-- ================================================================
-- LUU Y: Neu db chi duoc doc (khong co quyen tao index),
-- bo qua script nay va chi can chay code da fix CommandTimeout la du.

-- ================================================================
-- 1. tabCHITIETHOADON — index tren sohd (JOIN voi tabHOADON, WHERE)
-- ================================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_tabCHITIETHOADON_sohd'
      AND object_id = OBJECT_ID('dbo.tabCHITIETHOADON')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_tabCHITIETHOADON_sohd
    ON dbo.tabCHITIETHOADON (sohd)
    INCLUDE (thanhTien, soLuongThuc, soLuongDu, maBao, ngayNhan);

    PRINT 'Created: IX_tabCHITIETHOADON_sohd';
END
ELSE
    PRINT 'Already exists: IX_tabCHITIETHOADON_sohd';
GO

-- ================================================================
-- 2. tabCHITIETHOADON — index tren ngayNhan (GROUP BY theo ngay)
-- ================================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_tabCHITIETHOADON_ngayNhan'
      AND object_id = OBJECT_ID('dbo.tabCHITIETHOADON')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_tabCHITIETHOADON_ngayNhan
    ON dbo.tabCHITIETHOADON (ngayNhan)
    INCLUDE (thanhTien);

    PRINT 'Created: IX_tabCHITIETHOADON_ngayNhan';
END
ELSE
    PRINT 'Already exists: IX_tabCHITIETHOADON_ngayNhan';
GO

-- ================================================================
-- 3. tabTon — index tren ngay (GROUP BY theo ngay cho inventory)
-- ================================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_tabTon_ngay'
      AND object_id = OBJECT_ID('dbo.tabTon')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_tabTon_ngay
    ON dbo.tabTon (ngay)
    INCLUDE (slPhatHanh, banthuc, banLe, dieuPhoi, ton);

    PRINT 'Created: IX_tabTon_ngay';
END
ELSE
    PRINT 'Already exists: IX_tabTon_ngay';
GO

-- ================================================================
-- 4. tabHOADON — index tren makh (JOIN voi tabCHITIETHOADON)
-- ================================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_tabHOADON_makh'
      AND object_id = OBJECT_ID('dbo.tabHOADON')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_tabHOADON_makh
    ON dbo.tabHOADON (makh)
    INCLUDE (ngayLapPhieu, thanhToan, sohd);

    PRINT 'Created: IX_tabHOADON_makh';
END
ELSE
    PRINT 'Already exists: IX_tabHOADON_makh';
GO

-- ================================================================
-- 5. tabDieuPhoi — index tren denngay (WHERE denngay <= GETDATE())
-- ================================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_tabDieuPhoi_denngay'
      AND object_id = OBJECT_ID('dbo.tabDieuPhoi')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_tabDieuPhoi_denngay
    ON dbo.tabDieuPhoi (denngay)
    INCLUDE (soHD, makh, ngay, tungay, ghiChu);

    PRINT 'Created: IX_tabDieuPhoi_denngay';
END
ELSE
    PRINT 'Already exists: IX_tabDieuPhoi_denngay';
GO

-- ================================================================
-- 6. tabChiTietDieuPhoi — index tren sohd (WHERE sohd = @soHD)
-- ================================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_tabChiTietDieuPhoi_sohd'
      AND object_id = OBJECT_ID('dbo.tabChiTietDieuPhoi')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_tabChiTietDieuPhoi_sohd
    ON dbo.tabChiTietDieuPhoi (sohd)
    INCLUDE (ngayNhan, maBao, tenbao, donGia, soluongBan, soluongDieuPhoi, thanhTien);

    PRINT 'Created: IX_tabChiTietDieuPhoi_sohd';
END
ELSE
    PRINT 'Already exists: IX_tabChiTietDieuPhoi_sohd';
GO

-- ================================================================
-- 7. tabKHACHHANG — index tren MAKH (JOIN voi tabHOADON)
-- ================================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_tabKHACHHANG_MAKH'
      AND object_id = OBJECT_ID('dbo.tabKHACHHANG')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_tabKHACHHANG_MAKH
    ON dbo.tabKHACHHANG (MAKH)
    INCLUDE (TEN, P_PH, P_KT);

    PRINT 'Created: IX_tabKHACHHANG_MAKH';
END
ELSE
    PRINT 'Already exists: IX_tabKHACHHANG_MAKH';
GO

-- ================================================================
-- 8. tabBAO — index tren maBao (JOIN voi tabCHITIETHOADON)
-- ================================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_tabBAO_maBao'
      AND object_id = OBJECT_ID('dbo.tabBAO')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_tabBAO_maBao
    ON dbo.tabBAO (maBao)
    INCLUDE (ten, donGia, ngayBatDau);

    PRINT 'Created: IX_tabBAO_maBao';
END
ELSE
    PRINT 'Already exists: IX_tabBAO_maBao';
GO

-- ================================================================
-- SUMMARY
-- ================================================================
PRINT '';
PRINT '=== Migration Complete ===';
PRINT 'All indexes are CREATE INDEX only — no table structure changes.';
PRINT 'If you lack permission to create indexes, SKIP this script.';
PRINT 'The CommandTimeout=60 fix in code alone resolves the timeout.';
GO
