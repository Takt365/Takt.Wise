-- ========================================
-- 暂存表 DDL（手工执行，非 Quartz 任务）
-- 表名与租户库目标表相同（仅库不同：暂存库 vs 租户业务库）
-- 隔离列必须按目标实体继承，禁止四列一套乱加：
--   TaktTenantCoreEntityBase    → tenant_code
--   TaktTenantCultureEntityBase → tenant_code + culture_code
--   TaktTenantPlantEntityBase   → tenant_code + related_plant
--   TaktTenantEntityBase        → tenant_code + culture_code + related_plant
--   TaktCompanyEntityBase       → tenant_code + company_code + culture_code + plant_code
--   TaktApprovalEntityBase      → 同上四列
-- 本文件表：
--   sync_ad      / TaktAdminDivision          → Core（仅 tenant_code）
--   sync_mfrmat  / TaktManufacturerMaterial   → Core（仅 tenant_code）
--   sync_distmat / TaktSellerMaterial         → Core（仅 tenant_code）
--   sync_matpkg  / TaktPackagingMaterial      → Company
--   sync_bc/bv   / TaktBomMaterialCostItem/Cost → Company
--   sync_pup/sp  / 采购信息记录 / 销售条件记录四级 → Company
--   sync_sup/cus / TaktSupplier / TaktCustomer → Company
--   sync_po      / TaktPurchaseOrder(+Item)    → Company
--   sync_so      / TaktSalesOrder(+Item)       → Company
--   sync_matdoc  / TaktMaterialDocument(+Item) → Company
--   sync_miro    / TaktPurchaseInvoice(+Item)  → Company
--   sync_billing / TaktSalesInvoice(+Item)     → Company
-- 源表列与目标实体隔离列一致；sync 从源表读 tenant/company/culture/plant，禁止用任务占位符回填
-- ========================================
-- 手工执行前请将 USE 改为实际暂存库名（如 zTakt_900_Dev / zTakt_900_Prod）
USE [zTakt_900_Dev];
GO

-- ----------------------------------------
-- 行政区划（sync_ad）
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_foundation_admin_division', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_foundation_admin_division] (
    [id] BIGINT NOT NULL,
    [tenant_code] VARCHAR(3) NOT NULL,
    [country_code] NVARCHAR(2) NOT NULL,
    [division_code] NVARCHAR(40) NOT NULL,
    [division_name] NVARCHAR(200) NOT NULL,
    [parent_id] BIGINT NOT NULL CONSTRAINT [df_sap_admin_division_parent_id] DEFAULT (0),
    [level] INT NOT NULL CONSTRAINT [df_sap_admin_division_level] DEFAULT (1),
    [division_path] NVARCHAR(500) NOT NULL CONSTRAINT [df_sap_admin_division_path] DEFAULT (N''),
    [is_leaf] INT NOT NULL CONSTRAINT [df_sap_admin_division_is_leaf] DEFAULT (0),
    [postal_code] NVARCHAR(20) NULL,
    [currency_code] VARCHAR(3) NOT NULL CONSTRAINT [df_sap_admin_division_currency] DEFAULT (''),
    [phone_code] VARCHAR(16) NOT NULL CONSTRAINT [df_sap_admin_division_phone] DEFAULT (''),
    [is_built_in] INT NOT NULL CONSTRAINT [df_sap_admin_division_built_in] DEFAULT (0),
    [sort_order] INT NOT NULL CONSTRAINT [df_sap_admin_division_sort] DEFAULT (0),
    [division_status] INT NOT NULL CONSTRAINT [df_sap_admin_division_status] DEFAULT (1),
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_admin_division_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL,
    CONSTRAINT [pk_sap_admin_division] PRIMARY KEY CLUSTERED ([id])
  );
  CREATE UNIQUE INDEX [ux_sap_admin_division_code]
    ON [dbo].[takt_foundation_admin_division] ([tenant_code], [division_code]);
  CREATE INDEX [ix_sap_admin_division_parent]
    ON [dbo].[takt_foundation_admin_division] ([parent_id]);
  CREATE INDEX [ix_sap_admin_division_country_level]
    ON [dbo].[takt_foundation_admin_division] ([country_code], [level]);
END
GO

-- ----------------------------------------
-- 制造商物料 / 销售商物料（sync_mfrmat / sync_distmat）
-- TaktManufacturerMaterial / TaktSellerMaterial：Core，仅 tenant_code
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_logistics_procurement_manufacturer_material', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_manufacturer_material] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [vendor_code] NVARCHAR(20) NULL,
    [vendor_short_name] NVARCHAR(40) NULL,
    [supplier_code] NVARCHAR(10) NULL,
    [supplier_short_name] NVARCHAR(40) NULL,
    [material_type] NVARCHAR(4) NOT NULL CONSTRAINT [df_sap_manufacturer_material_type] DEFAULT (N'HERS'),
    [material_group] NVARCHAR(9) NOT NULL CONSTRAINT [df_sap_manufacturer_material_group] DEFAULT (N''),
    [internal_material_code] NVARCHAR(20) NOT NULL,
    [material_code] NVARCHAR(20) NOT NULL,
    [material_description] NVARCHAR(40) NOT NULL CONSTRAINT [df_sap_manufacturer_material_desc] DEFAULT (N''),
    [manufacturer_material_code] NVARCHAR(40) NOT NULL,
    [manufacturer_material_description] NVARCHAR(40) NOT NULL CONSTRAINT [df_sap_manufacturer_mfr_desc] DEFAULT (N''),
    [manufacturer_material_specification] NVARCHAR(70) NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_manufacturer_material_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_manufacturer_material]
    ON [dbo].[takt_logistics_procurement_manufacturer_material]
    ([tenant_code], [internal_material_code], [material_code]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_seller_material', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_seller_material] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [customer_code] NVARCHAR(10) NULL,
    [customer_short_name] NVARCHAR(40) NULL,
    [client_code] NVARCHAR(20) NULL,
    [client_short_name] NVARCHAR(40) NULL,
    [material_type] NVARCHAR(4) NOT NULL CONSTRAINT [df_sap_seller_material_type] DEFAULT (N'HERS'),
    [material_group] NVARCHAR(9) NOT NULL CONSTRAINT [df_sap_seller_material_group] DEFAULT (N''),
    [internal_material_code] NVARCHAR(20) NOT NULL,
    [material_code] NVARCHAR(20) NOT NULL,
    [material_description] NVARCHAR(40) NOT NULL CONSTRAINT [df_sap_seller_material_desc] DEFAULT (N''),
    [seller_material_code] NVARCHAR(40) NOT NULL,
    [seller_material_description] NVARCHAR(40) NOT NULL CONSTRAINT [df_sap_seller_seller_desc] DEFAULT (N''),
    [seller_material_specification] NVARCHAR(70) NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_seller_material_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_seller_material]
    ON [dbo].[takt_logistics_sales_seller_material]
    ([tenant_code], [internal_material_code], [material_code]);
END
GO

-- ----------------------------------------
-- 包装物料（sync_matpkg）
-- TaktPackagingMaterial：Company
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_logistics_materials_packaging_material', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_materials_packaging_material] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [packaging_material_code] NVARCHAR(20) NOT NULL,
    [material_code] NVARCHAR(20) NOT NULL,
    [material_description] NVARCHAR(40) NOT NULL CONSTRAINT [df_sap_packaging_material_desc] DEFAULT (N''),
    [hs_code] NVARCHAR(20) NULL,
    [hs_name] NVARCHAR(500) NULL,
    [additional_code] NVARCHAR(20) NULL,
    [origin_country_region_code] NVARCHAR(2) NULL,
    [origin_country_region_name] NVARCHAR(100) NULL,
    [destination_country_region_code] NVARCHAR(2) NULL,
    [destination_country_region_name] NVARCHAR(100) NULL,
    [regulatory_condition_code] NVARCHAR(40) NULL,
    [tariff_rate_type] NVARCHAR(40) NULL,
    [gross_weight] DECIMAL(18,10) NULL,
    [net_weight] DECIMAL(18,10) NULL,
    [weight_unit] NVARCHAR(10) NOT NULL CONSTRAINT [df_sap_packaging_weight_unit] DEFAULT (N'KG'),
    [business_volume] DECIMAL(18,6) NULL,
    [volume_unit] NVARCHAR(10) NOT NULL CONSTRAINT [df_sap_packaging_volume_unit] DEFAULT (N'M3'),
    [size_dimension] NVARCHAR(40) NULL,
    [packaging_type] NVARCHAR(40) NOT NULL CONSTRAINT [df_sap_packaging_type] DEFAULT (N'VERP'),
    [packing_unit] NVARCHAR(20) NOT NULL CONSTRAINT [df_sap_packaging_packing_unit] DEFAULT (N'CAR'),
    [quantity_per_packing] DECIMAL(18,2) NULL,
    [packaging_spec] NVARCHAR(200) NULL,
    [packaging_description] NVARCHAR(500) NULL,
    [sort_order] INT NOT NULL CONSTRAINT [df_sap_packaging_sort] DEFAULT (0),
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_packaging_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_packaging_material]
    ON [dbo].[takt_logistics_materials_packaging_material]
    ([tenant_code], [company_code], [plant_code], [packaging_material_code]);
END
GO

-- ----------------------------------------
-- BOM 物料成本明细 / 汇总（sync_bc / sync_bv）
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_logistics_manufacturing_bom_material_cost_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_manufacturing_bom_material_cost_item] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [bom_level] NVARCHAR(20) NULL,
    [bom_item_code] NVARCHAR(4) NOT NULL,
    [product_code] NVARCHAR(20) NOT NULL,
    [line_number] INT NOT NULL CONSTRAINT [df_sap_bom_material_cost_item_line_number] DEFAULT (10),
    [product_description] NVARCHAR(40) NULL,
    [component_code] NVARCHAR(20) NOT NULL,
    [component_description] NVARCHAR(40) NULL,
    [component_quantity] DECIMAL(18,2) NULL,
    [batch_indicator] NVARCHAR(1) NULL,
    [production_related] NVARCHAR(1) NULL,
    [pcb_sect_indicator] NVARCHAR(1) NULL,
    [purchase_type] NVARCHAR(1) NULL,
    [special_procurement_type] NVARCHAR(50) NULL,
    [profit_center_code] NVARCHAR(4) NULL,
    [moving_average_price] DECIMAL(18,5) NULL,
    [moving_price_unit] INT NULL,
    [moving_price_currency_code] NVARCHAR(3) NULL,
    [purchase_organization] NVARCHAR(4) NULL,
    [purchase_group] NVARCHAR(3) NULL,
    [supplier_code] NVARCHAR(10) NULL,
    [net_purchase_price] DECIMAL(18,5) NULL,
    [purchase_price_unit] INT NULL,
    [purchase_currency_code] NVARCHAR(3) NULL,
    [costing_date] DATETIME NULL,
    [ext_field] NVARCHAR(MAX) NULL,
    [remark] NVARCHAR(MAX) NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_bom_material_cost_item_is_deleted] DEFAULT (0),
    [created_by] BIGINT NULL,
    [created_at] DATETIME NULL,
    [updated_by] BIGINT NULL,
    [updated_at] DATETIME NULL,
    [deleted_by] BIGINT NULL,
    [deleted_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_bom_material_cost_item]
    ON [dbo].[takt_logistics_manufacturing_bom_material_cost_item]
    ([tenant_code], [company_code], [plant_code], [bom_level], [bom_item_code], [product_code], [line_number], [component_code],
     [component_quantity], [batch_indicator], [production_related], [purchase_type],
     [special_procurement_type], [costing_date]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_manufacturing_bom_material_cost', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_manufacturing_bom_material_cost] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [model_code] NVARCHAR(40) NOT NULL,
    [model_monthly_average_cost] DECIMAL(18,5) NULL,
    [material_type] NVARCHAR(4) NOT NULL CONSTRAINT [df_sap_bom_material_cost_material_type] DEFAULT (N'FERT'),
    [product_code] NVARCHAR(20) NOT NULL,
    [product_description] NVARCHAR(40) NULL,
    [product_monthly_cost] DECIMAL(18,5) NULL,
    [product_monthly_calculation] DECIMAL(18,5) NULL,
    [latest_purchase_cost] DECIMAL(18,5) NULL,
    [currency_code] NVARCHAR(3) NULL,
    [costing_period] NVARCHAR(7) NULL,
    [costing_date] DATETIME NULL,
    [ext_field] NVARCHAR(MAX) NULL,
    [remark] NVARCHAR(MAX) NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_bom_material_cost_is_deleted] DEFAULT (0),
    [created_by] BIGINT NULL,
    [created_at] DATETIME NULL,
    [updated_by] BIGINT NULL,
    [updated_at] DATETIME NULL,
    [deleted_by] BIGINT NULL,
    [deleted_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_bom_material_cost]
    ON [dbo].[takt_logistics_manufacturing_bom_material_cost]
    ([tenant_code], [company_code], [plant_code], [model_code], [product_code], [costing_period]);
END
GO

-- ----------------------------------------
-- 采购信息记录四级（sync_pup）
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_info_record', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_info_record] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [purchase_info_record_code] NVARCHAR(20) NOT NULL,
    [price_type] NVARCHAR(4) NULL,
    [supplier_code] NVARCHAR(10) NULL,
    [material_code] NVARCHAR(20) NULL,
    [material_description] NVARCHAR(40) NULL,
    [purchase_group] NVARCHAR(3) NULL,
    [tax_code] NVARCHAR(4) NULL,
    [gr_based_invoice_inspection] INT NULL,
    [pricing_date_control] INT NULL,
    [valid_from] DATETIME NULL,
    [valid_to] DATETIME NULL,
    [purchase_inquiry_id] BIGINT NULL,
    [purchase_inquiry_code] VARCHAR(20) NULL,
    [variable_key] NVARCHAR(40) NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_purchase_price_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_price]
    ON [dbo].[takt_logistics_procurement_purchase_info_record] ([tenant_code], [company_code], [plant_code], [purchase_info_record_code]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_info_record_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_info_record_item] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [purchase_info_record_code] NVARCHAR(20) NOT NULL,
    [purchase_info_record_seq] INT NOT NULL,
    [price_type] NVARCHAR(4) NULL,
    [scale_type] NVARCHAR(1) NULL,
    [scale_basis] NVARCHAR(1) NULL,
    [scale_quantity] DECIMAL(18,4) NULL,
    [scale_unit] NVARCHAR(5) NULL,
    [scale_value] DECIMAL(18,5) NULL,
    [scale_currency_code] NVARCHAR(3) NULL,
    [calculation_type] NVARCHAR(1) NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [condition_currency_code] NVARCHAR(3) NULL,
    [price_unit] INT NULL,
    [unit_of_measure] NVARCHAR(5) NULL,
    [min_order_quantity] INT NULL,
    [rounding_value] INT NULL,
    [planned_delivery_time_days] INT NULL,
    [is_obsolete] INT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_purchase_info_record_item_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_info_record_item]
    ON [dbo].[takt_logistics_procurement_purchase_info_record_item] ([tenant_code], [company_code], [plant_code], [purchase_info_record_code], [purchase_info_record_seq]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_info_record_scale_quantity', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_info_record_scale_quantity] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [purchase_info_record_code] NVARCHAR(20) NOT NULL,
    [purchase_info_record_seq] INT NOT NULL,
    [purchase_scale_seq] INT NOT NULL,
    [scale_quantity] DECIMAL(18,4) NOT NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [is_obsolete] INT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_purchase_price_scale_qty_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_price_scale_qty]
    ON [dbo].[takt_logistics_procurement_purchase_info_record_scale_quantity]
    ([tenant_code], [company_code], [plant_code], [purchase_info_record_code], [purchase_info_record_seq], [purchase_scale_seq], [scale_quantity]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_info_record_scale_value', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_info_record_scale_value] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [purchase_info_record_code] NVARCHAR(20) NOT NULL,
    [purchase_info_record_seq] INT NOT NULL,
    [purchase_scale_seq] INT NOT NULL,
    [scale_value] DECIMAL(18,5) NOT NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [is_obsolete] INT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_purchase_price_scale_val_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_price_scale_val]
    ON [dbo].[takt_logistics_procurement_purchase_info_record_scale_value]
    ([tenant_code], [company_code], [plant_code], [purchase_info_record_code], [purchase_info_record_seq], [purchase_scale_seq], [scale_value]);
END
GO

-- ----------------------------------------
-- 销售条件记录四级（sync_sp）
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_logistics_sales_condition_record', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_condition_record] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [sales_condition_record_code] NVARCHAR(20) NOT NULL,
    [price_type] NVARCHAR(4) NULL,
    [customer_code] NVARCHAR(10) NULL,
    [material_code] NVARCHAR(20) NULL,
    [material_description] NVARCHAR(40) NULL,
    [sales_group] NVARCHAR(3) NULL,
    [tax_code] NVARCHAR(4) NULL,
    [gr_based_invoice_inspection] INT NULL,
    [pricing_date_control] INT NULL,
    [valid_from] DATETIME NULL,
    [valid_to] DATETIME NULL,
    [sales_quotation_id] BIGINT NULL,
    [sales_quotation_code] VARCHAR(20) NULL,
    [variable_key] NVARCHAR(40) NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_sales_price_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_price]
    ON [dbo].[takt_logistics_sales_condition_record] ([tenant_code], [company_code], [plant_code], [sales_condition_record_code]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_condition_record_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_condition_record_item] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [sales_condition_record_code] NVARCHAR(20) NOT NULL,
    [sales_condition_record_seq] INT NOT NULL,
    [price_type] NVARCHAR(4) NULL,
    [scale_type] NVARCHAR(1) NULL,
    [scale_basis] NVARCHAR(1) NULL,
    [scale_quantity] DECIMAL(18,4) NULL,
    [scale_unit] NVARCHAR(5) NULL,
    [scale_value] DECIMAL(18,5) NULL,
    [scale_currency_code] NVARCHAR(3) NULL,
    [calculation_type] NVARCHAR(1) NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [condition_currency_code] NVARCHAR(3) NULL,
    [price_unit] INT NULL,
    [unit_of_measure] NVARCHAR(5) NULL,
    [min_order_quantity] INT NULL,
    [rounding_value] INT NULL,
    [planned_delivery_time_days] INT NULL,
    [is_obsolete] INT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_sales_price_item_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_price_item]
    ON [dbo].[takt_logistics_sales_condition_record_item] ([tenant_code], [company_code], [plant_code], [sales_condition_record_code], [sales_condition_record_seq]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_condition_record_scale_quantity', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_condition_record_scale_quantity] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [sales_condition_record_code] NVARCHAR(20) NOT NULL,
    [sales_condition_record_seq] INT NOT NULL,
    [sales_scale_seq] INT NOT NULL,
    [scale_quantity] DECIMAL(18,4) NOT NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [is_obsolete] INT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_sales_price_scale_qty_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_price_scale_qty]
    ON [dbo].[takt_logistics_sales_condition_record_scale_quantity]
    ([tenant_code], [company_code], [plant_code], [sales_condition_record_code], [sales_condition_record_seq], [sales_scale_seq], [scale_quantity]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_condition_record_scale_value', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_condition_record_scale_value] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [sales_condition_record_code] NVARCHAR(20) NOT NULL,
    [sales_condition_record_seq] INT NOT NULL,
    [sales_scale_seq] INT NOT NULL,
    [scale_value] DECIMAL(18,5) NOT NULL,
    [price] DECIMAL(18,5) NULL,
    [untaxed_price] DECIMAL(18,5) NULL,
    [tax_included_price] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [is_obsolete] INT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_sales_price_scale_val_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_price_scale_val]
    ON [dbo].[takt_logistics_sales_condition_record_scale_value]
    ([tenant_code], [company_code], [plant_code], [sales_condition_record_code], [sales_condition_record_seq], [sales_scale_seq], [scale_value]);
END
GO

-- 已有价格表补列 tax_amount（幂等）
IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_info_record_item', N'tax_amount') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_info_record_item] ADD [tax_amount] DECIMAL(18,5) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_info_record_scale_quantity', N'tax_amount') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_info_record_scale_quantity] ADD [tax_amount] DECIMAL(18,5) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_info_record_scale_value', N'tax_amount') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_info_record_scale_value] ADD [tax_amount] DECIMAL(18,5) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_condition_record_item', N'tax_amount') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_condition_record_item] ADD [tax_amount] DECIMAL(18,5) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_condition_record_scale_quantity', N'tax_amount') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_condition_record_scale_quantity] ADD [tax_amount] DECIMAL(18,5) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_condition_record_scale_value', N'tax_amount') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_condition_record_scale_value] ADD [tax_amount] DECIMAL(18,5) NULL;
GO

-- ----------------------------------------
-- 供货商 / 客户信息（sync_sup / sync_cus）
-- 与实体 TaktSupplier 同名：takt_logistics_procurement_supplier
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_logistics_materials_supplier', N'U') IS NOT NULL
  AND OBJECT_ID(N'dbo.takt_logistics_procurement_supplier', N'U') IS NULL
BEGIN
  EXEC sp_rename N'dbo.takt_logistics_materials_supplier', N'takt_logistics_procurement_supplier';
  IF EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.takt_logistics_procurement_supplier')
      AND name = N'ux_sap_materials_supplier'
  )
    EXEC sp_rename N'dbo.takt_logistics_procurement_supplier.ux_sap_materials_supplier', N'ux_sap_procurement_supplier', N'INDEX';
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_procurement_supplier', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_supplier] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [supplier_code] NVARCHAR(10) NOT NULL,
    [supplier_name1] NVARCHAR(140) NOT NULL,
    [supplier_name2] NVARCHAR(140) NULL,
    [supplier_short_name] NVARCHAR(40) NULL,
    [supplier_type] INT NULL,
    [enterprise_nature] VARCHAR(4) NULL,
    [industry_attribute] VARCHAR(4) NULL,
    [supplier_tax_number] NVARCHAR(50) NULL,
    [tax_code] NVARCHAR(4) NULL,
    [tax_rate] INT NULL,
    [registration_country] NVARCHAR(2) NULL,
    [registration_province] NVARCHAR(70) NULL,
    [registration_city] NVARCHAR(70) NULL,
    [registration_address1] NVARCHAR(140) NULL,
    [registration_address2] NVARCHAR(140) NULL,
    [supplier_phone] NVARCHAR(50) NULL,
    [supplier_fax] NVARCHAR(50) NULL,
    [supplier_email] NVARCHAR(100) NULL,
    [supplier_website] NVARCHAR(200) NULL,
    [contact_person] NVARCHAR(50) NULL,
    [contact_phone] NVARCHAR(50) NULL,
    [contact_email] NVARCHAR(100) NULL,
    [currency_code] NVARCHAR(3) NULL,
    [reconciliation_account] VARCHAR(40) NULL,
    [customer_code] NVARCHAR(10) NULL,
    [clearing_with_customer] INT NULL,
    [payment_method] INT NULL,
    [payment_terms] NVARCHAR(40) NULL,
    [bank_code] NVARCHAR(15) NULL,
    [bank_account] NVARCHAR(40) NULL,
    [account_holder] NVARCHAR(100) NULL,
    [gr_based_invoice_inspection] INT NULL,
    [incoterms1] VARCHAR(3) NULL,
    [incoterms2] NVARCHAR(40) NULL,
    [automatic_purchase_order] INT NULL,
    [pricing_date_control] INT NULL,
    [purchase_group] NVARCHAR(3) NULL,
    [planned_delivery_time_days] INT NULL,
    [evaluated_receipt_settlement] INT NULL,
    [purchasing_organization] VARCHAR(4) NULL,
    [supplier_level] INT NULL,
    [evaluation_score] DECIMAL(5,2) NULL,
    [sort_order] INT NULL,
    [supplier_status] INT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_procurement_supplier_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_procurement_supplier]
    ON [dbo].[takt_logistics_procurement_supplier] ([tenant_code], [company_code], [supplier_code]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_customer', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_customer] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [customer_code] NVARCHAR(10) NOT NULL,
    [customer_name1] NVARCHAR(140) NOT NULL,
    [customer_name2] NVARCHAR(140) NULL,
    [customer_short_name] NVARCHAR(40) NULL,
    [customer_type] INT NULL,
    [enterprise_nature] VARCHAR(4) NULL,
    [industry_attribute] VARCHAR(4) NULL,
    [customer_tax_number] NVARCHAR(50) NULL,
    [tax_code] NVARCHAR(4) NULL,
    [tax_rate] INT NULL,
    [registration_country] NVARCHAR(2) NULL,
    [registration_province] NVARCHAR(70) NULL,
    [registration_city] NVARCHAR(70) NULL,
    [registration_address1] NVARCHAR(140) NULL,
    [registration_address2] NVARCHAR(140) NULL,
    [customer_phone] NVARCHAR(50) NULL,
    [customer_fax] NVARCHAR(50) NULL,
    [customer_email] NVARCHAR(100) NULL,
    [customer_website] NVARCHAR(200) NULL,
    [contact_person] NVARCHAR(50) NULL,
    [contact_phone] NVARCHAR(50) NULL,
    [contact_email] NVARCHAR(100) NULL,
    [currency_code] NVARCHAR(3) NULL,
    [sales_organization] VARCHAR(4) NULL,
    [distribution_channel] VARCHAR(2) NULL,
    [product_group] VARCHAR(2) NULL,
    [customer_group] VARCHAR(2) NULL,
    [trading_partner] VARCHAR(4) NULL,
    [account_assignment_group] VARCHAR(2) NULL,
    [supplier_code] NVARCHAR(10) NULL,
    [nielsen_indicator] VARCHAR(2) NULL,
    [central_posting_block] INT NULL,
    [reconciliation_account] VARCHAR(40) NULL,
    [headquarters] NVARCHAR(20) NULL,
    [clearing_with_vendor] INT NULL,
    [payment_terms] NVARCHAR(40) NULL,
    [payment_method] INT NULL,
    [delivering_plant] VARCHAR(4) NULL,
    [incoterms1] VARCHAR(3) NULL,
    [incoterms2] NVARCHAR(40) NULL,
    [shipping_conditions] VARCHAR(2) NULL,
    [customer_pricing_procedure] VARCHAR(2) NULL,
    [credit_level] INT NULL,
    [credit_amount] DECIMAL(18,2) NULL,
    [discount_rate] DECIMAL(5,2) NULL,
    [customer_level] INT NULL,
    [evaluation_score] DECIMAL(5,2) NULL,
    [sort_order] INT NULL,
    [customer_status] INT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_sales_customer_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_customer]
    ON [dbo].[takt_logistics_sales_customer] ([tenant_code], [company_code], [plant_code], [customer_code]);
END
GO

-- ----------------------------------------
-- 采购订单 / 销售订单（sync_po / sync_so）
-- TaktPurchaseOrder(+Item) / TaktSalesOrder(+Item)：Company
-- ----------------------------------------
IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_order', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_order] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [purchase_order_code] NVARCHAR(20) NOT NULL,
    [purchase_request_id] BIGINT NULL,
    [purchase_request_code] NVARCHAR(20) NULL,
    [supplier_code] NVARCHAR(10) NOT NULL,
    [supplier_name1] NVARCHAR(140) NOT NULL,
    [order_date] DATETIME NOT NULL,
    [required_arrival_date] DATETIME NULL,
    [actual_arrival_date] DATETIME NULL,
    [purchase_group] NVARCHAR(3) NULL,
    [total_quantity] DECIMAL(18,4) NULL,
    [total_amount] DECIMAL(18,2) NULL,
    [discount_amount] DECIMAL(18,2) NULL,
    [currency_code] NVARCHAR(3) NOT NULL CONSTRAINT [df_sap_purchase_order_currency] DEFAULT (N'CNY'),
    [exchange_rate] DECIMAL(18,5) NULL,
    [tax_code] NVARCHAR(4) NULL,
    [tax_rate] INT NULL,
    [tax_amount] DECIMAL(18,2) NULL,
    [actual_amount] DECIMAL(18,2) NULL,
    [received_quantity] DECIMAL(18,4) NULL,
    [received_amount] DECIMAL(18,2) NULL,
    [paid_amount] DECIMAL(18,2) NULL,
    [payment_method] INT NULL,
    [delivery_method] INT NULL,
    [delivery_address] NVARCHAR(500) NULL,
    [order_status] INT NULL,
    [delivery_status] INT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_purchase_order_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_order]
    ON [dbo].[takt_logistics_procurement_purchase_order]
    ([tenant_code], [company_code], [plant_code], [purchase_order_code], [supplier_code], [order_date]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_order_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_order_item] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [purchase_order_id] BIGINT NOT NULL,
    [purchase_order_code] NVARCHAR(20) NOT NULL,
    [line_number] INT NOT NULL,
    [request_code] NVARCHAR(20) NULL,
    [request_line_number] INT NULL,
    [material_code] NVARCHAR(20) NULL,
    [material_description] NVARCHAR(40) NOT NULL CONSTRAINT [df_sap_purchase_order_item_desc] DEFAULT (N''),
    [material_specification] NVARCHAR(70) NULL,
    [purchase_unit] NVARCHAR(20) NOT NULL CONSTRAINT [df_sap_purchase_order_item_unit] DEFAULT (N'PC'),
    [order_quantity] DECIMAL(18,5) NULL,
    [received_quantity] DECIMAL(18,5) NULL,
    [purchase_per_unit] INT NULL,
    [purchase_unit_price] DECIMAL(18,5) NULL,
    [discount_rate] DECIMAL(5,2) NULL,
    [discount_amount] DECIMAL(18,5) NULL,
    [tax_included_amount] DECIMAL(18,5) NULL,
    [untaxed_amount] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [purchase_amount] DECIMAL(18,5) NULL,
    [delivery_status] INT NULL,
    [is_obsolete] INT NOT NULL CONSTRAINT [df_sap_purchase_order_item_obsolete] DEFAULT (0),
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_purchase_order_item_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_order_item]
    ON [dbo].[takt_logistics_procurement_purchase_order_item]
    ([tenant_code], [company_code], [purchase_order_id], [line_number], [material_code]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_order', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_order] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [sales_order_code] NVARCHAR(20) NOT NULL,
    [customer_code] NVARCHAR(10) NOT NULL,
    [customer_name1] NVARCHAR(140) NOT NULL,
    [order_date] DATETIME NOT NULL,
    [required_delivery_date] DATETIME NULL,
    [actual_delivery_date] DATETIME NULL,
    [sales_group] NVARCHAR(3) NULL,
    [total_quantity] DECIMAL(18,4) NULL,
    [total_amount] DECIMAL(18,2) NULL,
    [discount_amount] DECIMAL(18,2) NULL,
    [currency_code] NVARCHAR(3) NOT NULL CONSTRAINT [df_sap_sales_order_currency] DEFAULT (N'CNY'),
    [exchange_rate] DECIMAL(18,5) NULL,
    [tax_code] NVARCHAR(4) NULL,
    [tax_rate] INT NULL,
    [tax_amount] DECIMAL(18,2) NULL,
    [actual_amount] DECIMAL(18,2) NULL,
    [shipped_quantity] DECIMAL(18,4) NULL,
    [shipped_amount] DECIMAL(18,2) NULL,
    [received_amount] DECIMAL(18,2) NULL,
    [delivery_method] INT NULL,
    [payment_method] INT NULL,
    [delivery_address] NVARCHAR(500) NULL,
    [order_status] INT NULL,
    [delivery_status] INT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_sales_order_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_order]
    ON [dbo].[takt_logistics_sales_order]
    ([tenant_code], [company_code], [plant_code], [sales_order_code]);
END
GO

IF OBJECT_ID(N'dbo.takt_logistics_sales_order_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_order_item] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [sales_order_id] BIGINT NOT NULL,
    [sales_order_code] NVARCHAR(20) NOT NULL,
    [line_number] INT NOT NULL,
    [material_code] NVARCHAR(20) NOT NULL,
    [material_description] NVARCHAR(40) NOT NULL CONSTRAINT [df_sap_sales_order_item_desc] DEFAULT (N''),
    [material_specification] NVARCHAR(70) NULL,
    [sales_unit] NVARCHAR(5) NOT NULL CONSTRAINT [df_sap_sales_order_item_unit] DEFAULT (N'PC'),
    [order_quantity] DECIMAL(18,5) NULL,
    [shipped_quantity] DECIMAL(18,5) NULL,
    [sales_per_unit] INT NULL,
    [sales_unit_price] DECIMAL(18,5) NULL,
    [discount_rate] DECIMAL(5,2) NULL,
    [discount_amount] DECIMAL(18,5) NULL,
    [tax_included_amount] DECIMAL(18,5) NULL,
    [untaxed_amount] DECIMAL(18,5) NULL,
    [tax_amount] DECIMAL(18,5) NULL,
    [sales_amount] DECIMAL(18,5) NULL,
    [delivery_status] INT NULL,
    [is_obsolete] INT NOT NULL CONSTRAINT [df_sap_sales_order_item_obsolete] DEFAULT (0),
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_sales_order_item_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_order_item]
    ON [dbo].[takt_logistics_sales_order_item]
    ([tenant_code], [company_code], [sales_order_id], [line_number]);
END
GO

-- ----------------------------------------
-- 物料凭证 / 采购发票 / 销售发票暂存表
-- 规则：表名与租户库相同；业务列与实体一致；隔离列按 TaktCompanyEntityBase
-- ----------------------------------------
-- 物料凭证主（sync_matdoc；业务列=TaktMaterialDocument）
IF OBJECT_ID(N'dbo.takt_logistics_materials_material_document', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_materials_material_document] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [material_document_code] NVARCHAR(10) NOT NULL,
    [material_document_year] NVARCHAR(4) NOT NULL,
    [transaction_event_type] NVARCHAR(2) NULL,
    [document_type] NVARCHAR(2) NULL,
    [revaluation_type] NVARCHAR(2) NULL,
    [document_date] DATETIME NOT NULL,
    [posting_date] DATETIME NOT NULL,
    [reference_code] NVARCHAR(16) NULL,
    [header_text] NVARCHAR(25) NULL,
    [transaction_code] NVARCHAR(4) NULL,
    [delivery_code] NVARCHAR(10) NULL,
    [posted_by] NVARCHAR(6) NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_material_document_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_material_document]
    ON [dbo].[takt_logistics_materials_material_document]
    ([tenant_code], [company_code], [material_document_year], [material_document_code]);
END
GO

-- 物料凭证明细（sync_matdoc；业务列=TaktMaterialDocumentItem，无 year；关联靠 material_document_id）
IF OBJECT_ID(N'dbo.takt_logistics_materials_material_document_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_materials_material_document_item] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [material_document_id] BIGINT NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [material_document_code] NVARCHAR(10) NOT NULL,
    [line_number] INT NOT NULL,
    [line_id] NVARCHAR(6) NULL,
    [parent_line_id] NVARCHAR(6) NULL,
    [line_depth] NVARCHAR(2) NULL,
    [account_assignment_original_line] INT NULL,
    [movement_type] NVARCHAR(3) NOT NULL,
    [auto_created_flag] NVARCHAR(1) NULL,
    [material_code] NVARCHAR(20) NOT NULL,
    [warehouse_code] NVARCHAR(4) NULL,
    [batch_code] NVARCHAR(10) NULL,
    [stock_type] NVARCHAR(1) NULL,
    [batch_status_key] NVARCHAR(1) NULL,
    [restricted_stock_flag] NVARCHAR(1) NULL,
    [special_stock] NVARCHAR(1) NULL,
    [supplier_code] NVARCHAR(10) NULL,
    [customer_code] NVARCHAR(10) NULL,
    [sales_order_code] NVARCHAR(20) NULL,
    [sales_order_item] INT NULL,
    [sales_order_schedule] INT NULL,
    [distribution_code] NVARCHAR(10) NULL,
    [debit_credit_indicator] NVARCHAR(1) NULL,
    [currency_code] NVARCHAR(3) NULL,
    [local_currency_amount] DECIMAL(13,2) NOT NULL,
    [delivery_cost_amount] DECIMAL(13,2) NULL,
    [alternative_amount] DECIMAL(13,2) NULL,
    [revaluation_debit_credit] NVARCHAR(1) NULL,
    [revaluation_amount] DECIMAL(13,2) NULL,
    [valuation_type] NVARCHAR(10) NULL,
    [quantity] DECIMAL(13,3) NOT NULL,
    [base_unit] NVARCHAR(3) NULL,
    [entry_quantity] DECIMAL(13,3) NULL,
    [entry_unit] NVARCHAR(3) NULL,
    [po_price_quantity] DECIMAL(13,3) NULL,
    [po_price_unit] NVARCHAR(3) NULL,
    [purchase_order_code] NVARCHAR(20) NULL,
    [purchase_order_item] INT NULL,
    [reference_document_year] NVARCHAR(4) NULL,
    [reference_document_code] NVARCHAR(10) NULL,
    [reference_document_item] INT NULL,
    [original_material_document_year] NVARCHAR(4) NULL,
    [original_material_document_code] NVARCHAR(10) NULL,
    [original_line_number] INT NULL,
    [delivery_completed_flag] NVARCHAR(1) NULL,
    [item_text] NVARCHAR(50) NULL,
    [equipment_code] NVARCHAR(18) NULL,
    [goods_recipient] NVARCHAR(12) NULL,
    [unloading_point] NVARCHAR(25) NULL,
    [business_area_code] NVARCHAR(4) NULL,
    [controlling_area_code] NVARCHAR(4) NULL,
    [trading_partner_business_area] NVARCHAR(4) NULL,
    [clearing_company_code] NVARCHAR(4) NULL,
    [cost_center_code] NVARCHAR(10) NULL,
    [legacy_project_code] NVARCHAR(16) NULL,
    [production_order_code] NVARCHAR(12) NULL,
    [asset_code] NVARCHAR(12) NULL,
    [asset_sub_code] NVARCHAR(4) NULL,
    [cost_center_stat_flag] NVARCHAR(1) NULL,
    [order_stat_flag] NVARCHAR(1) NULL,
    [project_stat_flag] NVARCHAR(1) NULL,
    [profitability_stat_flag] NVARCHAR(1) NULL,
    [fiscal_year] NVARCHAR(4) NULL,
    [post_to_previous_period_flag] NVARCHAR(1) NULL,
    [post_to_previous_year_flag] NVARCHAR(1) NULL,
    [accounting_document_code] NVARCHAR(10) NULL,
    [accounting_document_item] INT NULL,
    [revaluation_document_code] NVARCHAR(10) NULL,
    [revaluation_document_item] NVARCHAR(3) NULL,
    [reservation_code] NVARCHAR(10) NULL,
    [reservation_item] INT NULL,
    [final_issue_flag] NVARCHAR(1) NULL,
    [reservation_quantity] DECIMAL(13,3) NULL,
    [statistics_relevant_flag] NVARCHAR(1) NULL,
    [receiving_material_code] NVARCHAR(20) NULL,
    [receiving_plant_code] NVARCHAR(4) NULL,
    [receiving_warehouse_code] NVARCHAR(4) NULL,
    [goods_receipt_slip_count] INT NULL,
    [profit_center_code] NVARCHAR(10) NULL,
    [network_code] NVARCHAR(12) NULL,
    [routing_number] NVARCHAR(10) NULL,
    [routing_counter] NVARCHAR(8) NULL,
    [order_item_number] INT NULL,
    [gl_account_code] NVARCHAR(10) NULL,
    [order_unit_quantity] DECIMAL(13,3) NULL,
    [order_unit] NVARCHAR(3) NULL,
    [supplying_vendor_code] NVARCHAR(10) NULL,
    [partner_profit_center_code] NVARCHAR(10) NULL,
    [stock_managed_material_code] NVARCHAR(20) NULL,
    [receiving_stock_material_code] NVARCHAR(20) NULL,
    [quantity_string] NVARCHAR(4) NULL,
    [value_string] NVARCHAR(4) NULL,
    [quantity_update_flag] NVARCHAR(1) NULL,
    [value_update_flag] NVARCHAR(1) NULL,
    [valuated_stock_quantity] DECIMAL(13,3) NULL,
    [total_valuated_stock_value] DECIMAL(13,2) NULL,
    [price_control] NVARCHAR(1) NULL,
    [original_item_line] INT NULL,
    [manufacturer_part_material_code] NVARCHAR(40) NULL,
    [stock_type_modification] NVARCHAR(1) NULL,
    [transaction_event_type] NVARCHAR(2) NULL,
    [mkpf_reference_code] NVARCHAR(32) NULL,
    [mkpf_transaction_code2] NVARCHAR(40) NULL,
    [im_delivery_code] NVARCHAR(20) NULL,
    [im_delivery_item] INT NULL,
    [is_obsolete] INT NOT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_material_document_item_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_material_document_item]
    ON [dbo].[takt_logistics_materials_material_document_item]
    ([tenant_code], [company_code], [material_document_id], [line_number]);
END
GO

-- 采购发票主（sync_miro；业务列=TaktPurchaseInvoice）
IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_invoice', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_invoice] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [purchase_invoice_code] NVARCHAR(10) NOT NULL,
    [fiscal_year] NVARCHAR(4) NOT NULL,
    [document_type] NVARCHAR(2) NULL,
    [document_date] DATETIME NOT NULL,
    [posting_date] DATETIME NOT NULL,
    [posted_by] NVARCHAR(6) NULL,
    [transaction_code] NVARCHAR(20) NULL,
    [transaction_event_type] NVARCHAR(2) NULL,
    [reference_code] NVARCHAR(16) NULL,
    [supplier_code] NVARCHAR(10) NOT NULL,
    [currency_code] NVARCHAR(3) NOT NULL,
    [exchange_rate] DECIMAL(9,5) NULL,
    [gross_amount] DECIMAL(13,2) NOT NULL,
    [vat_amount] DECIMAL(13,2) NULL,
    [tax_code] NVARCHAR(2) NULL,
    [payment_terms] NVARCHAR(4) NULL,
    [invoice_flag] NVARCHAR(1) NULL,
    [header_text] NVARCHAR(25) NULL,
    [calculate_tax_flag] NVARCHAR(1) NULL,
    [reversal_document_code] NVARCHAR(10) NULL,
    [reversal_fiscal_year] NVARCHAR(4) NULL,
    [invoice_verification_category] NVARCHAR(1) NULL,
    [invoice_verification_type] NVARCHAR(1) NULL,
    [invoice_status] NVARCHAR(1) NULL,
    [supplying_country] NVARCHAR(3) NULL,
    [scb_indicator] NVARCHAR(3) NULL,
    [tax_exchange_rate] DECIMAL(9,5) NULL,
    [payment_method] NVARCHAR(1) NULL,
    [baseline_date] DATETIME NULL,
    [branch_account] NVARCHAR(10) NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_purchase_invoice_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_invoice]
    ON [dbo].[takt_logistics_procurement_purchase_invoice]
    ([tenant_code], [company_code], [fiscal_year], [purchase_invoice_code]);
END
GO

-- 采购发票明细（sync_miro；业务列=TaktPurchaseInvoiceItem + fiscal_year 关联键）
IF OBJECT_ID(N'dbo.takt_logistics_procurement_purchase_invoice_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_procurement_purchase_invoice_item] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [fiscal_year] NVARCHAR(4) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [purchase_invoice_code] NVARCHAR(10) NOT NULL,
    [line_number] INT NOT NULL,
    [purchase_order_code] NVARCHAR(20) NULL,
    [purchase_order_item] INT NULL,
    [account_assignment_seq] NVARCHAR(2) NULL,
    [material_code] NVARCHAR(20) NULL,
    [valuation_area] NVARCHAR(4) NULL,
    [amount] DECIMAL(13,2) NULL,
    [debit_credit_indicator] NVARCHAR(1) NULL,
    [tax_code] NVARCHAR(2) NULL,
    [quantity] DECIMAL(13,3) NULL,
    [order_unit] NVARCHAR(3) NULL,
    [po_price_quantity] DECIMAL(13,3) NULL,
    [po_price_unit] NVARCHAR(3) NULL,
    [valuated_stock_quantity] DECIMAL(13,3) NULL,
    [previous_period_stock] DECIMAL(13,3) NULL,
    [base_unit] NVARCHAR(3) NULL,
    [item_category] NVARCHAR(1) NULL,
    [account_assignment_category] NVARCHAR(1) NULL,
    [valuation_class] NVARCHAR(4) NULL,
    [final_invoice_flag] NVARCHAR(1) NULL,
    [update_po_history_flag] NVARCHAR(1) NULL,
    [subsequent_debit_credit] NVARCHAR(1) NULL,
    [block_reason_quantity] NVARCHAR(1) NULL,
    [value_string] NVARCHAR(4) NULL,
    [reference_code] NVARCHAR(16) NULL,
    [return_posting_flag] NVARCHAR(1) NULL,
    [delivery_cost_share] DECIMAL(13,2) NULL,
    [total_valuated_stock_value] DECIMAL(13,2) NULL,
    [previous_period_value] DECIMAL(13,2) NULL,
    [reference_document_code] NVARCHAR(10) NULL,
    [reference_document_year] NVARCHAR(4) NULL,
    [reference_document_item] INT NULL,
    [stock_managed_material_code] NVARCHAR(20) NULL,
    [material_document_item] INT NULL,
    [is_obsolete] INT NOT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_purchase_invoice_item_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_purchase_invoice_item]
    ON [dbo].[takt_logistics_procurement_purchase_invoice_item]
    ([tenant_code], [company_code], [fiscal_year], [purchase_invoice_code], [line_number]);
END
GO

-- 销售发票主（sync_billing；业务列=TaktSalesInvoice）
IF OBJECT_ID(N'dbo.takt_logistics_sales_invoice', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_invoice] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [billing_document_code] NVARCHAR(10) NOT NULL,
    [billing_type] NVARCHAR(4) NULL,
    [billing_category] NVARCHAR(1) NULL,
    [document_category] NVARCHAR(1) NULL,
    [currency_code] NVARCHAR(3) NOT NULL,
    [sales_organization] NVARCHAR(4) NULL,
    [distribution_channel] NVARCHAR(2) NULL,
    [pricing_procedure] NVARCHAR(6) NULL,
    [condition_code] NVARCHAR(10) NULL,
    [shipping_conditions] NVARCHAR(2) NULL,
    [billing_date] DATETIME NOT NULL,
    [customer_group] NVARCHAR(2) NULL,
    [price_list_type] NVARCHAR(2) NULL,
    [incoterms1] NVARCHAR(3) NULL,
    [export_flag] NVARCHAR(1) NULL,
    [posting_status] NVARCHAR(1) NULL,
    [accounting_exchange_rate] DECIMAL(9,5) NULL,
    [fixed_exchange_rate_flag] NVARCHAR(1) NULL,
    [payment_terms] NVARCHAR(4) NULL,
    [payment_method] NVARCHAR(1) NULL,
    [account_assignment_group] NVARCHAR(2) NULL,
    [country_code] NVARCHAR(3) NULL,
    [region] NVARCHAR(3) NULL,
    [customer_tax_class1] NVARCHAR(1) NULL,
    [net_amount] DECIMAL(15,2) NOT NULL,
    [combination_criteria] NVARCHAR(40) NULL,
    [posted_by] NVARCHAR(6) NULL,
    [payer_code] NVARCHAR(10) NULL,
    [customer_code] NVARCHAR(10) NOT NULL,
    [dunning_area] NVARCHAR(2) NULL,
    [statistics_currency_code] NVARCHAR(3) NULL,
    [foreign_trade_code] NVARCHAR(10) NULL,
    [cancelled_billing_document] NVARCHAR(10) NULL,
    [agreement_code] NVARCHAR(10) NULL,
    [invoice_list_type] NVARCHAR(4) NULL,
    [invoice_list_date] DATETIME NULL,
    [exchange_rate_type] NVARCHAR(4) NULL,
    [dunning_key] NVARCHAR(1) NULL,
    [dunning_block] NVARCHAR(1) NULL,
    [division] NVARCHAR(2) NULL,
    [credit_control_area] NVARCHAR(4) NULL,
    [credit_account] NVARCHAR(10) NULL,
    [credit_currency_code] NVARCHAR(3) NULL,
    [credit_exchange_rate] DECIMAL(9,5) NULL,
    [hierarchy_type_pricing] NVARCHAR(1) NULL,
    [customer_purchase_order] NVARCHAR(35) NULL,
    [trading_partner] NVARCHAR(6) NULL,
    [tax_departure_country] NVARCHAR(3) NULL,
    [organization_sales_tax_number] NVARCHAR(20) NULL,
    [country_sales_tax_number] NVARCHAR(20) NULL,
    [reference_code] NVARCHAR(16) NULL,
    [assignment] NVARCHAR(18) NULL,
    [tax_amount] DECIMAL(13,2) NULL,
    [logical_system] NVARCHAR(10) NULL,
    [cancelled_flag] NVARCHAR(1) NULL,
    [exchange_rate_date] DATETIME NULL,
    [payment_reference] NVARCHAR(30) NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_sales_invoice_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_invoice]
    ON [dbo].[takt_logistics_sales_invoice]
    ([tenant_code], [company_code], [billing_document_code]);
END
GO

-- 销售发票明细（sync_billing；业务列=TaktSalesInvoiceItem）
IF OBJECT_ID(N'dbo.takt_logistics_sales_invoice_item', N'U') IS NULL
BEGIN
  CREATE TABLE [dbo].[takt_logistics_sales_invoice_item] (
    [tenant_code] VARCHAR(3) NOT NULL,
    [company_code] VARCHAR(4) NOT NULL,
    [culture_code] VARCHAR(5) NOT NULL,
    [plant_code] NVARCHAR(4) NOT NULL,
    [billing_document_code] NVARCHAR(10) NOT NULL,
    [line_number] INT NOT NULL,
    [billing_quantity] DECIMAL(13,3) NULL,
    [sales_unit] NVARCHAR(3) NULL,
    [numerator] DECIMAL(5,0) NULL,
    [denominator] DECIMAL(5,0) NULL,
    [base_unit] NVARCHAR(3) NULL,
    [scale_quantity] DECIMAL(13,3) NULL,
    [billing_quantity_sku] DECIMAL(13,3) NULL,
    [required_quantity] DECIMAL(13,3) NULL,
    [net_weight] DECIMAL(15,3) NULL,
    [gross_weight] DECIMAL(15,3) NULL,
    [weight_unit] NVARCHAR(3) NULL,
    [volume] DECIMAL(15,3) NULL,
    [volume_unit] NVARCHAR(3) NULL,
    [business_area_code] NVARCHAR(4) NULL,
    [pricing_date] DATETIME NULL,
    [service_rendered_date] DATETIME NULL,
    [pricing_exchange_rate] DECIMAL(9,5) NULL,
    [net_amount] DECIMAL(15,2) NULL,
    [origin_document_code] NVARCHAR(10) NULL,
    [origin_document_item] INT NULL,
    [reference_document_code] NVARCHAR(10) NULL,
    [reference_document_item] INT NULL,
    [reference_document_category] NVARCHAR(1) NULL,
    [sales_document_code] NVARCHAR(20) NULL,
    [sales_document_item] INT NULL,
    [sales_document_reference_flag] NVARCHAR(1) NULL,
    [material_code] NVARCHAR(20) NOT NULL,
    [material_description] NVARCHAR(40) NULL,
    [material_group] NVARCHAR(9) NULL,
    [sales_item_category] NVARCHAR(4) NULL,
    [item_type] NVARCHAR(1) NULL,
    [product_hierarchy] NVARCHAR(18) NULL,
    [shipping_point] NVARCHAR(4) NULL,
    [replacement_part_flag] NVARCHAR(1) NULL,
    [division] NVARCHAR(2) NULL,
    [partner_item] INT NULL,
    [departure_country] NVARCHAR(3) NULL,
    [plant_region] NVARCHAR(3) NULL,
    [statistical_value_flag] NVARCHAR(1) NULL,
    [pricing_flag] NVARCHAR(1) NULL,
    [cash_discount_flag] NVARCHAR(1) NULL,
    [cash_discount_base] DECIMAL(13,2) NULL,
    [cost_center_code] NVARCHAR(10) NULL,
    [sales_office] NVARCHAR(4) NULL,
    [division_for_order] NVARCHAR(2) NULL,
    [debit_credit_indicator] NVARCHAR(1) NULL,
    [valuation_type] NVARCHAR(10) NULL,
    [warehouse_code] NVARCHAR(4) NULL,
    [cost_amount] DECIMAL(13,2) NULL,
    [subtotal1] DECIMAL(13,2) NULL,
    [subtotal2] DECIMAL(13,2) NULL,
    [subtotal3] DECIMAL(13,2) NULL,
    [subtotal4] DECIMAL(13,2) NULL,
    [subtotal5] DECIMAL(13,2) NULL,
    [subtotal6] DECIMAL(13,2) NULL,
    [statistics_exchange_rate] DECIMAL(9,5) NULL,
    [international_article_number] NVARCHAR(18) NULL,
    [profit_center_code] NVARCHAR(10) NULL,
    [material_group4] NVARCHAR(3) NULL,
    [entered_material_code] NVARCHAR(20) NULL,
    [controlling_area_code] NVARCHAR(4) NULL,
    [profitability_segment] NVARCHAR(10) NULL,
    [credit_price] DECIMAL(11,2) NULL,
    [credit_active_flag] NVARCHAR(1) NULL,
    [customer_group_sales_order] NVARCHAR(2) NULL,
    [destination_country_order] NVARCHAR(3) NULL,
    [manual_pricing_flag] NVARCHAR(1) NULL,
    [price_list_order] NVARCHAR(2) NULL,
    [region_order] NVARCHAR(3) NULL,
    [sales_organization_order] NVARCHAR(4) NULL,
    [distribution_channel_order] NVARCHAR(2) NULL,
    [document_category] NVARCHAR(1) NULL,
    [tax_amount] DECIMAL(13,2) NULL,
    [order_reason] NVARCHAR(3) NULL,
    [payment_guarantee_form] NVARCHAR(2) NULL,
    [gross_amount] DECIMAL(15,2) NULL,
    [exchange_rate_date] DATETIME NULL,
    [is_obsolete] INT NOT NULL,
    [is_deleted] INT NOT NULL CONSTRAINT [df_sap_sales_invoice_item_is_deleted] DEFAULT (0),
    [created_at] DATETIME NULL
  );
  CREATE UNIQUE INDEX [ux_sap_sales_invoice_item]
    ON [dbo].[takt_logistics_sales_invoice_item]
    ([tenant_code], [company_code], [billing_document_code], [line_number]);
END
GO

-- ----------------------------------------
-- 已有表幂等补齐（ALTER 一律 NULL，避免存量失败）
-- ----------------------------------------
-- Core：仅 tenant_code；禁止 company/plant/culture
DECLARE @core_table SYSNAME;
DECLARE @core_ddl NVARCHAR(400);
DECLARE core_tbl CURSOR LOCAL FAST_FORWARD FOR
SELECT v.n FROM (VALUES
  (N'takt_foundation_admin_division'),
  (N'takt_logistics_procurement_manufacturer_material'),
  (N'takt_logistics_sales_seller_material')
) v(n);
OPEN core_tbl;
FETCH NEXT FROM core_tbl INTO @core_table;
WHILE @@FETCH_STATUS = 0
BEGIN
  IF OBJECT_ID(N'dbo.' + @core_table, N'U') IS NOT NULL
  BEGIN
    IF COL_LENGTH(N'dbo.' + @core_table, N'tenant_code') IS NULL
    BEGIN
      SET @core_ddl = N'ALTER TABLE [dbo].[' + @core_table + N'] ADD [tenant_code] VARCHAR(3) NULL;';
      EXEC sys.sp_executesql @core_ddl;
    END
    IF COL_LENGTH(N'dbo.' + @core_table, N'is_deleted') IS NULL
    BEGIN
      SET @core_ddl = N'ALTER TABLE [dbo].[' + @core_table + N'] ADD [is_deleted] INT NULL;';
      EXEC sys.sp_executesql @core_ddl;
    END
    IF COL_LENGTH(N'dbo.' + @core_table, N'created_at') IS NULL
    BEGIN
      SET @core_ddl = N'ALTER TABLE [dbo].[' + @core_table + N'] ADD [created_at] DATETIME NULL;';
      EXEC sys.sp_executesql @core_ddl;
    END
    IF COL_LENGTH(N'dbo.' + @core_table, N'company_code') IS NOT NULL
    BEGIN
      SET @core_ddl = N'ALTER TABLE [dbo].[' + @core_table + N'] DROP COLUMN [company_code];';
      EXEC sys.sp_executesql @core_ddl;
    END
    IF COL_LENGTH(N'dbo.' + @core_table, N'plant_code') IS NOT NULL
    BEGIN
      SET @core_ddl = N'ALTER TABLE [dbo].[' + @core_table + N'] DROP COLUMN [plant_code];';
      EXEC sys.sp_executesql @core_ddl;
    END
    IF COL_LENGTH(N'dbo.' + @core_table, N'culture_code') IS NOT NULL
    BEGIN
      SET @core_ddl = N'ALTER TABLE [dbo].[' + @core_table + N'] DROP COLUMN [culture_code];';
      EXEC sys.sp_executesql @core_ddl;
    END
  END
  FETCH NEXT FROM core_tbl INTO @core_table;
END
CLOSE core_tbl;
DEALLOCATE core_tbl;
GO

-- Company：TaktCompanyEntityBase → tenant_code + company_code + culture_code + plant_code
DECLARE @company_table SYSNAME;
DECLARE @ddl NVARCHAR(400);
DECLARE company_tbl CURSOR LOCAL FAST_FORWARD FOR
SELECT v.n FROM (VALUES
  (N'takt_logistics_manufacturing_bom_material_cost_item'),
  (N'takt_logistics_manufacturing_bom_material_cost'),
  (N'takt_logistics_procurement_purchase_info_record'),
  (N'takt_logistics_procurement_purchase_info_record_item'),
  (N'takt_logistics_procurement_purchase_info_record_scale_quantity'),
  (N'takt_logistics_procurement_purchase_info_record_scale_value'),
  (N'takt_logistics_sales_condition_record'),
  (N'takt_logistics_sales_condition_record_item'),
  (N'takt_logistics_sales_condition_record_scale_quantity'),
  (N'takt_logistics_sales_condition_record_scale_value'),
  (N'takt_logistics_procurement_supplier'),
  (N'takt_logistics_sales_customer'),
  (N'takt_logistics_procurement_purchase_order'),
  (N'takt_logistics_procurement_purchase_order_item'),
  (N'takt_logistics_sales_order'),
  (N'takt_logistics_sales_order_item'),
  (N'takt_logistics_materials_packaging_material'),
  (N'takt_logistics_materials_material_document'),
  (N'takt_logistics_materials_material_document_item'),
  (N'takt_logistics_procurement_purchase_invoice'),
  (N'takt_logistics_procurement_purchase_invoice_item'),
  (N'takt_logistics_sales_invoice'),
  (N'takt_logistics_sales_invoice_item')
) v(n);
OPEN company_tbl;
FETCH NEXT FROM company_tbl INTO @company_table;
WHILE @@FETCH_STATUS = 0
BEGIN
  IF OBJECT_ID(N'dbo.' + @company_table, N'U') IS NOT NULL
  BEGIN
    IF COL_LENGTH(N'dbo.' + @company_table, N'tenant_code') IS NULL
    BEGIN
      SET @ddl = N'ALTER TABLE [dbo].[' + @company_table + N'] ADD [tenant_code] VARCHAR(3) NULL;';
      EXEC sys.sp_executesql @ddl;
    END
    IF COL_LENGTH(N'dbo.' + @company_table, N'company_code') IS NULL
    BEGIN
      SET @ddl = N'ALTER TABLE [dbo].[' + @company_table + N'] ADD [company_code] VARCHAR(4) NULL;';
      EXEC sys.sp_executesql @ddl;
    END
    IF COL_LENGTH(N'dbo.' + @company_table, N'culture_code') IS NULL
    BEGIN
      SET @ddl = N'ALTER TABLE [dbo].[' + @company_table + N'] ADD [culture_code] VARCHAR(5) NULL;';
      EXEC sys.sp_executesql @ddl;
    END
    IF COL_LENGTH(N'dbo.' + @company_table, N'plant_code') IS NULL
    BEGIN
      SET @ddl = N'ALTER TABLE [dbo].[' + @company_table + N'] ADD [plant_code] NVARCHAR(4) NULL;';
      EXEC sys.sp_executesql @ddl;
    END
    IF COL_LENGTH(N'dbo.' + @company_table, N'is_deleted') IS NULL
    BEGIN
      SET @ddl = N'ALTER TABLE [dbo].[' + @company_table + N'] ADD [is_deleted] INT NULL;';
      EXEC sys.sp_executesql @ddl;
    END
    IF COL_LENGTH(N'dbo.' + @company_table, N'created_at') IS NULL
    BEGIN
      SET @ddl = N'ALTER TABLE [dbo].[' + @company_table + N'] ADD [created_at] DATETIME NULL;';
      EXEC sys.sp_executesql @ddl;
    END
  END
  FETCH NEXT FROM company_tbl INTO @company_table;
END
CLOSE company_tbl;
DEALLOCATE company_tbl;
GO

-- 实体有、旧 CREATE 缺的业务列
IF COL_LENGTH(N'dbo.takt_logistics_manufacturing_bom_material_cost', N'material_type') IS NULL
  ALTER TABLE [dbo].[takt_logistics_manufacturing_bom_material_cost] ADD [material_type] NVARCHAR(4) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_manufacturing_bom_material_cost', N'latest_purchase_cost') IS NULL
  ALTER TABLE [dbo].[takt_logistics_manufacturing_bom_material_cost] ADD [latest_purchase_cost] DECIMAL(18,5) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_info_record', N'material_description') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_info_record] ADD [material_description] NVARCHAR(40) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_condition_record', N'material_description') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_condition_record] ADD [material_description] NVARCHAR(40) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'tax_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [tax_code] NVARCHAR(4) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'tax_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] ADD [tax_code] NVARCHAR(4) NULL;
GO

-- posted_by：旧暂存表缺列时补齐（物料凭证主 / 采购发票主 / 销售发票主）
IF COL_LENGTH(N'dbo.takt_logistics_materials_material_document', N'posted_by') IS NULL
  ALTER TABLE [dbo].[takt_logistics_materials_material_document] ADD [posted_by] NVARCHAR(6) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_procurement_purchase_invoice', N'posted_by') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_purchase_invoice] ADD [posted_by] NVARCHAR(6) NULL;
GO
IF COL_LENGTH(N'dbo.takt_logistics_sales_invoice', N'posted_by') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_invoice] ADD [posted_by] NVARCHAR(6) NULL;
GO

-- sales_by / sales_employee_* → sales_group（对齐 TaktSalesOrder.SalesGroup）
IF COL_LENGTH(N'dbo.takt_logistics_sales_order', N'sales_group') IS NULL
  ALTER TABLE [dbo].[takt_logistics_sales_order] ADD [sales_group] NVARCHAR(3) NULL;
GO
IF OBJECT_ID(N'dbo.takt_logistics_sales_order', N'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.takt_logistics_sales_order', N'sales_by') IS NOT NULL
  AND COL_LENGTH(N'dbo.takt_logistics_sales_order', N'sales_group') IS NOT NULL
  UPDATE [dbo].[takt_logistics_sales_order]
  SET [sales_group] = LEFT(LTRIM(RTRIM([sales_by])), 3)
  WHERE [sales_group] IS NULL AND NULLIF(LTRIM(RTRIM([sales_by])), N'') IS NOT NULL;
GO
IF OBJECT_ID(N'dbo.takt_logistics_sales_order', N'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.takt_logistics_sales_order', N'sales_employee_name') IS NOT NULL
  AND COL_LENGTH(N'dbo.takt_logistics_sales_order', N'sales_group') IS NOT NULL
  UPDATE [dbo].[takt_logistics_sales_order]
  SET [sales_group] = LEFT(LTRIM(RTRIM([sales_employee_name])), 3)
  WHERE [sales_group] IS NULL AND NULLIF(LTRIM(RTRIM([sales_employee_name])), N'') IS NOT NULL;
GO

-- default_culture 不是实体列；有值则迁到 culture_code 再删
IF OBJECT_ID(N'dbo.takt_logistics_procurement_supplier', N'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'default_culture') IS NOT NULL
  AND COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'culture_code') IS NOT NULL
  UPDATE [dbo].[takt_logistics_procurement_supplier]
  SET [culture_code] = [default_culture]
  WHERE [culture_code] IS NULL AND [default_culture] IS NOT NULL;
GO
IF OBJECT_ID(N'dbo.takt_logistics_procurement_supplier', N'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'default_culture') IS NOT NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] DROP COLUMN [default_culture];
GO
IF OBJECT_ID(N'dbo.takt_logistics_sales_customer', N'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'default_culture') IS NOT NULL
  AND COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'culture_code') IS NOT NULL
  UPDATE [dbo].[takt_logistics_sales_customer]
  SET [culture_code] = [default_culture]
  WHERE [culture_code] IS NULL AND [default_culture] IS NOT NULL;
GO
IF OBJECT_ID(N'dbo.takt_logistics_sales_customer', N'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.takt_logistics_sales_customer', N'default_culture') IS NOT NULL
  ALTER TABLE [dbo].[takt_logistics_sales_customer] DROP COLUMN [default_culture];
GO


