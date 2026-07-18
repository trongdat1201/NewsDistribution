-- ================================================================
-- MIGRATION: Triggers_lock_dieu_phoi_sau_hoa_don.sql
-- Purpose:  Chan UPDATE/DELETE/INSERT (voi soHD cu) tren tabDieuPhoi
--           va tabChiTietDieuPhoi neu soHD da co trong tabHOADON
-- Safety:   Chi tao TRIGGER — khong ALTER TABLE, khong them cot moi
-- Run on:   SQL Server 2016+
-- Rollback: xem script "Rollback_Triggers_lock_dieu_phoi.sql" o duoi
-- ================================================================
--
-- NGUYEN TAC KHOA:
--   Mot khi soHD da xuat hien trong tabHOADON (= da lap hoa don),
--   KHONG DUOC phep:
--     1. UPDATE bat ky cot nao tren tabDieuPhoi voi soHD do
--     2. UPDATE bat ky cot nao tren tabChiTietDieuPhoi voi soHD do
--     3. DELETE ban ghi nao tren 2 bang voi soHD do
--     4. INSERT ban ghi moi tren 2 bang voi soHD da ton tai trong tabHOADON
--        (tranh bypass bang cach them dong moi cho phieu cu)
--
-- INSERT phieu moi (soHD chua co trong tabHOADON) van cho phep binh thuong.
-- ================================================================

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

-- ================================================================
-- 1. TRIGGER tren tabChiTietDieuPhoi (bang con — kiem tra truoc)
-- ================================================================
IF OBJECT_ID('dbo.trg_ChiTietDieuPhoi_LockAfterHoaDon', 'TR') IS NOT NULL
BEGIN
    DROP TRIGGER dbo.trg_ChiTietDieuPhoi_LockAfterHoaDon;
    PRINT 'Dropped existing trigger: trg_ChiTietDieuPhoi_LockAfterHoaDon';
END
GO

CREATE TRIGGER dbo.trg_ChiTietDieuPhoi_LockAfterHoaDon
ON dbo.tabChiTietDieuPhoi
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @soHDList NVARCHAR(MAX) = N'';

    -- Lay danh sach soHD vua bi anh huong (ca INSERTED va DELETED de cover UPDATE)
    SELECT @soHDList = @soHDList + NCHAR(10) + N'  - ' + CAST(i.sohd AS NVARCHAR(50))
    FROM (SELECT DISTINCT sohd FROM inserted) i
    WHERE EXISTS (SELECT 1 FROM dbo.tabHOADON h WITH (NOLOCK) WHERE h.sohd = i.sohd);

    SELECT @soHDList = @soHDList + NCHAR(10) + N'  - ' + CAST(d.sohd AS NVARCHAR(50))
    FROM (SELECT DISTINCT sohd FROM deleted) d
    WHERE EXISTS (SELECT 1 FROM dbo.tabHOADON h WITH (NOLOCK) WHERE h.sohd = d.sohd);

    IF LEN(@soHDList) > 0
    BEGIN
        DECLARE @action NVARCHAR(20) =
            CASE
                WHEN EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted) THEN N'UPDATE'
                WHEN EXISTS (SELECT 1 FROM inserted)                                  THEN N'INSERT'
                ELSE N'DELETE'
            END;

        DECLARE @msg NVARCHAR(2000) =
            N'KHONG THE ' + @action + N' tabChiTietDieuPhoi — cac soHD sau da lap hoa don:' +
            @soHDList + NCHAR(10) + N'Huy hoa don lien quan truoc khi sua/xoa.';

        ;THROW 50001, @msg, 1;
        RETURN;
    END
END
GO

PRINT 'Created trigger: trg_ChiTietDieuPhoi_LockAfterHoaDon';
GO

-- ================================================================
-- 2. TRIGGER tren tabDieuPhoi (bang cha — bao ve metadata phieu)
-- ================================================================
IF OBJECT_ID('dbo.trg_DieuPhoi_LockAfterHoaDon', 'TR') IS NOT NULL
BEGIN
    DROP TRIGGER dbo.trg_DieuPhoi_LockAfterHoaDon;
    PRINT 'Dropped existing trigger: trg_DieuPhoi_LockAfterHoaDon';
END
GO

CREATE TRIGGER dbo.trg_DieuPhoi_LockAfterHoaDon
ON dbo.tabDieuPhoi
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @soHDList NVARCHAR(MAX) = N'';

    SELECT @soHDList = @soHDList + NCHAR(10) + N'  - ' + CAST(i.soHD AS NVARCHAR(50))
    FROM (SELECT DISTINCT soHD FROM inserted) i
    WHERE EXISTS (SELECT 1 FROM dbo.tabHOADON h WITH (NOLOCK) WHERE h.sohd = i.soHD);

    SELECT @soHDList = @soHDList + NCHAR(10) + N'  - ' + CAST(d.soHD AS NVARCHAR(50))
    FROM (SELECT DISTINCT soHD FROM deleted) d
    WHERE EXISTS (SELECT 1 FROM dbo.tabHOADON h WITH (NOLOCK) WHERE h.sohd = d.soHD);

    IF LEN(@soHDList) > 0
    BEGIN
        DECLARE @action NVARCHAR(20) =
            CASE
                WHEN EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted) THEN N'UPDATE'
                WHEN EXISTS (SELECT 1 FROM inserted)                                  THEN N'INSERT'
                ELSE N'DELETE'
            END;

        DECLARE @msg NVARCHAR(2000) =
            N'KHONG THE ' + @action + N' tabDieuPhoi — cac soHD sau da lap hoa don:' +
            @soHDList + NCHAR(10) + N'Huy hoa don lien quan truoc khi sua/xoa.';

        ;THROW 50001, @msg, 1;
        RETURN;
    END
END
GO

PRINT 'Created trigger: trg_DieuPhoi_LockAfterHoaDon';
GO

-- ================================================================
-- SUMMARY
-- ================================================================
PRINT '';
PRINT '=== Migration Complete ===';
PRINT '2 triggers created. tabDieuPhoi & tabChiTietDieuPhoi are now LOCKED';
PRINT 'once their soHD exists in tabHOADON.';
PRINT 'INSERT new soHD (not yet invoiced) still works normally.';
GO

-- ================================================================
-- ROLLBACK (chay thu cong neu can go trigger)
-- ================================================================
-- DROP TRIGGER dbo.trg_ChiTietDieuPhoi_LockAfterHoaDon;
-- DROP TRIGGER dbo.trg_DieuPhoi_LockAfterHoaDon;
-- GO