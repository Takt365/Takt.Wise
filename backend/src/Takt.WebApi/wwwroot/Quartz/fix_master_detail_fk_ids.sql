-- ========================================
-- 项目名称：节拍智控·TaktWise
-- 文件名称：fix_master_detail_fk_ids.sql
-- 功能描述：按业务编码回填主子表外键 Id（目标租户库内执行）
--   1) SalesConditionRecord → Item → ScaleQuantity / ScaleValue
--   2) SalesInvoice → Item
--   3) SalesOrder → Item
--   4) PurchaseInfoRecord → Item → ScaleQuantity / ScaleValue
--   5) PurchaseInvoice → Item
--   6) PurchaseOrder → Item
--   7) MaterialDocument → Item
-- 匹配键对齐 sync_sp / sync_pup / sync_billing / sync_so / sync_miro / sync_po / sync_matdoc
-- 版权信息：Copyright (c) 2026 Takt  All rights reserved.
-- ========================================

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @n INT;

BEGIN TRAN;

-- ----------------------------------------
-- 1. 销售条件记录：Item.sales_condition_record_id
--    键：tenant + company + plant + sales_condition_record_code
-- ----------------------------------------
UPDATE I
SET I.[sales_condition_record_id] = H.[id]
FROM [dbo].[takt_logistics_sales_condition_record_item] AS I
INNER JOIN [dbo].[takt_logistics_sales_condition_record] AS H
  ON H.[tenant_code] = I.[tenant_code]
 AND H.[company_code] = I.[company_code]
 AND LTRIM(RTRIM(H.[plant_code])) = LTRIM(RTRIM(I.[plant_code]))
 AND LTRIM(RTRIM(H.[sales_condition_record_code])) = LTRIM(RTRIM(I.[sales_condition_record_code]))
 AND H.[is_deleted] = 0
WHERE I.[is_deleted] = 0
  AND (I.[sales_condition_record_id] IS NULL OR I.[sales_condition_record_id] <> H.[id]);
SET @n = @@ROWCOUNT;
PRINT CONCAT(N'[SCR] item.sales_condition_record_id updated rows = ', @n);

-- ScaleQuantity.sales_condition_record_item_id
-- 键：tenant + company + plant + code + seq
UPDATE Q
SET Q.[sales_condition_record_item_id] = I.[id]
FROM [dbo].[takt_logistics_sales_condition_record_scale_quantity] AS Q
INNER JOIN [dbo].[takt_logistics_sales_condition_record_item] AS I
  ON I.[tenant_code] = Q.[tenant_code]
 AND I.[company_code] = Q.[company_code]
 AND LTRIM(RTRIM(I.[plant_code])) = LTRIM(RTRIM(Q.[plant_code]))
 AND LTRIM(RTRIM(I.[sales_condition_record_code])) = LTRIM(RTRIM(Q.[sales_condition_record_code]))
 AND I.[sales_condition_record_seq] = Q.[sales_condition_record_seq]
 AND I.[is_deleted] = 0
WHERE Q.[is_deleted] = 0
  AND (Q.[sales_condition_record_item_id] IS NULL OR Q.[sales_condition_record_item_id] <> I.[id]);
SET @n = @@ROWCOUNT;
PRINT CONCAT(N'[SCR] scale_quantity.sales_condition_record_item_id updated rows = ', @n);

-- ScaleValue.sales_condition_record_item_id
UPDATE V
SET V.[sales_condition_record_item_id] = I.[id]
FROM [dbo].[takt_logistics_sales_condition_record_scale_value] AS V
INNER JOIN [dbo].[takt_logistics_sales_condition_record_item] AS I
  ON I.[tenant_code] = V.[tenant_code]
 AND I.[company_code] = V.[company_code]
 AND LTRIM(RTRIM(I.[plant_code])) = LTRIM(RTRIM(V.[plant_code]))
 AND LTRIM(RTRIM(I.[sales_condition_record_code])) = LTRIM(RTRIM(V.[sales_condition_record_code]))
 AND I.[sales_condition_record_seq] = V.[sales_condition_record_seq]
 AND I.[is_deleted] = 0
WHERE V.[is_deleted] = 0
  AND (V.[sales_condition_record_item_id] IS NULL OR V.[sales_condition_record_item_id] <> I.[id]);
SET @n = @@ROWCOUNT;
PRINT CONCAT(N'[SCR] scale_value.sales_condition_record_item_id updated rows = ', @n);

-- ----------------------------------------
-- 2. 销售发票：Item.sales_invoice_id
--    键：tenant + company + billing_document_code
-- ----------------------------------------
UPDATE I
SET I.[sales_invoice_id] = H.[id]
FROM [dbo].[takt_logistics_sales_invoice_item] AS I
INNER JOIN [dbo].[takt_logistics_sales_invoice] AS H
  ON H.[tenant_code] = I.[tenant_code]
 AND H.[company_code] = I.[company_code]
 AND LTRIM(RTRIM(H.[billing_document_code])) = LTRIM(RTRIM(I.[billing_document_code]))
 AND H.[is_deleted] = 0
WHERE I.[is_deleted] = 0
  AND (I.[sales_invoice_id] IS NULL OR I.[sales_invoice_id] <> H.[id]);
SET @n = @@ROWCOUNT;
PRINT CONCAT(N'[SI] item.sales_invoice_id updated rows = ', @n);

-- ----------------------------------------
-- 3. 销售订单：Item.sales_order_id
--    键：tenant + company + plant + sales_order_code
-- ----------------------------------------
UPDATE I
SET I.[sales_order_id] = H.[id]
FROM [dbo].[takt_logistics_sales_order_item] AS I
INNER JOIN [dbo].[takt_logistics_sales_order] AS H
  ON H.[tenant_code] = I.[tenant_code]
 AND H.[company_code] = I.[company_code]
 AND LTRIM(RTRIM(H.[plant_code])) = LTRIM(RTRIM(I.[plant_code]))
 AND LTRIM(RTRIM(H.[sales_order_code])) = LTRIM(RTRIM(I.[sales_order_code]))
 AND H.[is_deleted] = 0
WHERE I.[is_deleted] = 0
  AND (I.[sales_order_id] IS NULL OR I.[sales_order_id] <> H.[id]);
SET @n = @@ROWCOUNT;
PRINT CONCAT(N'[SO] item.sales_order_id updated rows = ', @n);

-- ----------------------------------------
-- 4. 采购信息记录：Item.purchase_info_record_id
--    键：tenant + company + plant + purchase_info_record_code
-- ----------------------------------------
UPDATE I
SET I.[purchase_info_record_id] = H.[id]
FROM [dbo].[takt_logistics_procurement_purchase_info_record_item] AS I
INNER JOIN [dbo].[takt_logistics_procurement_purchase_info_record] AS H
  ON H.[tenant_code] = I.[tenant_code]
 AND H.[company_code] = I.[company_code]
 AND LTRIM(RTRIM(H.[plant_code])) = LTRIM(RTRIM(I.[plant_code]))
 AND LTRIM(RTRIM(H.[purchase_info_record_code])) = LTRIM(RTRIM(I.[purchase_info_record_code]))
 AND H.[is_deleted] = 0
WHERE I.[is_deleted] = 0
  AND (I.[purchase_info_record_id] IS NULL OR I.[purchase_info_record_id] <> H.[id]);
SET @n = @@ROWCOUNT;
PRINT CONCAT(N'[PIR] item.purchase_info_record_id updated rows = ', @n);

UPDATE Q
SET Q.[purchase_info_record_item_id] = I.[id]
FROM [dbo].[takt_logistics_procurement_purchase_info_record_scale_quantity] AS Q
INNER JOIN [dbo].[takt_logistics_procurement_purchase_info_record_item] AS I
  ON I.[tenant_code] = Q.[tenant_code]
 AND I.[company_code] = Q.[company_code]
 AND LTRIM(RTRIM(I.[plant_code])) = LTRIM(RTRIM(Q.[plant_code]))
 AND LTRIM(RTRIM(I.[purchase_info_record_code])) = LTRIM(RTRIM(Q.[purchase_info_record_code]))
 AND I.[purchase_info_record_seq] = Q.[purchase_info_record_seq]
 AND I.[is_deleted] = 0
WHERE Q.[is_deleted] = 0
  AND (Q.[purchase_info_record_item_id] IS NULL OR Q.[purchase_info_record_item_id] <> I.[id]);
SET @n = @@ROWCOUNT;
PRINT CONCAT(N'[PIR] scale_quantity.purchase_info_record_item_id updated rows = ', @n);

UPDATE V
SET V.[purchase_info_record_item_id] = I.[id]
FROM [dbo].[takt_logistics_procurement_purchase_info_record_scale_value] AS V
INNER JOIN [dbo].[takt_logistics_procurement_purchase_info_record_item] AS I
  ON I.[tenant_code] = V.[tenant_code]
 AND I.[company_code] = V.[company_code]
 AND LTRIM(RTRIM(I.[plant_code])) = LTRIM(RTRIM(V.[plant_code]))
 AND LTRIM(RTRIM(I.[purchase_info_record_code])) = LTRIM(RTRIM(V.[purchase_info_record_code]))
 AND I.[purchase_info_record_seq] = V.[purchase_info_record_seq]
 AND I.[is_deleted] = 0
WHERE V.[is_deleted] = 0
  AND (V.[purchase_info_record_item_id] IS NULL OR V.[purchase_info_record_item_id] <> I.[id]);
SET @n = @@ROWCOUNT;
PRINT CONCAT(N'[PIR] scale_value.purchase_info_record_item_id updated rows = ', @n);

-- ----------------------------------------
-- 5. 采购发票：Item.purchase_invoice_id
--    键：tenant + company + purchase_invoice_code
--    说明：明细无 fiscal_year；同码跨年时可能歧义（与 sync_miro 一致）
-- ----------------------------------------
UPDATE I
SET I.[purchase_invoice_id] = H.[id]
FROM [dbo].[takt_logistics_procurement_purchase_invoice_item] AS I
INNER JOIN [dbo].[takt_logistics_procurement_purchase_invoice] AS H
  ON H.[tenant_code] = I.[tenant_code]
 AND H.[company_code] = I.[company_code]
 AND LTRIM(RTRIM(H.[purchase_invoice_code])) = LTRIM(RTRIM(I.[purchase_invoice_code]))
 AND H.[is_deleted] = 0
WHERE I.[is_deleted] = 0
  AND (I.[purchase_invoice_id] IS NULL OR I.[purchase_invoice_id] <> H.[id]);
SET @n = @@ROWCOUNT;
PRINT CONCAT(N'[PI] item.purchase_invoice_id updated rows = ', @n);

-- ----------------------------------------
-- 6. 采购订单：Item.purchase_order_id
--    键：tenant + company + plant + purchase_order_code
-- ----------------------------------------
UPDATE I
SET I.[purchase_order_id] = H.[id]
FROM [dbo].[takt_logistics_procurement_purchase_order_item] AS I
INNER JOIN [dbo].[takt_logistics_procurement_purchase_order] AS H
  ON H.[tenant_code] = I.[tenant_code]
 AND H.[company_code] = I.[company_code]
 AND LTRIM(RTRIM(H.[plant_code])) = LTRIM(RTRIM(I.[plant_code]))
 AND LTRIM(RTRIM(H.[purchase_order_code])) = LTRIM(RTRIM(I.[purchase_order_code]))
 AND H.[is_deleted] = 0
WHERE I.[is_deleted] = 0
  AND (I.[purchase_order_id] IS NULL OR I.[purchase_order_id] <> H.[id]);
SET @n = @@ROWCOUNT;
PRINT CONCAT(N'[PO] item.purchase_order_id updated rows = ', @n);

-- ----------------------------------------
-- 7. 物料凭证：Item.material_document_id
--    键：tenant + company + material_document_code
--      + 明细 fiscal_year = 主表 material_document_year（明细年为空时仅按码匹配）
-- ----------------------------------------
UPDATE I
SET I.[material_document_id] = H.[id]
FROM [dbo].[takt_logistics_materials_material_document_item] AS I
INNER JOIN [dbo].[takt_logistics_materials_material_document] AS H
  ON H.[tenant_code] = I.[tenant_code]
 AND H.[company_code] = I.[company_code]
 AND LTRIM(RTRIM(H.[material_document_code])) = LTRIM(RTRIM(I.[material_document_code]))
 AND (
       NULLIF(LTRIM(RTRIM(ISNULL(I.[fiscal_year], N''))), N'') IS NULL
       OR LTRIM(RTRIM(I.[fiscal_year])) = LTRIM(RTRIM(H.[material_document_year]))
     )
 AND H.[is_deleted] = 0
WHERE I.[is_deleted] = 0
  AND (I.[material_document_id] IS NULL OR I.[material_document_id] <> H.[id]);
SET @n = @@ROWCOUNT;
PRINT CONCAT(N'[MD] item.material_document_id updated rows = ', @n);

COMMIT TRAN;
PRINT N'fix_master_detail_fk_ids.sql completed.';
