SET NOCOUNT ON;
DECLARE @progress_msg NVARCHAR(400);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'start' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  CAST(0 AS INT) AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  CAST(0 AS INT) AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'start', N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @culture_code NVARCHAR(5) = N'{{CultureCode}}';
DECLARE @plant_code NVARCHAR(4) = N'{{PlantCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @apply_chunk INT = 20000;
DECLARE @merge_from_rn INT;
DECLARE @merge_to_rn INT;
DECLARE @merge_max_rn INT;
DECLARE @dml_n INT;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源表 / 目标表：同名 + 列与实体 TaktSupplier 一致
-- {{SourceDatabase}}.dbo.takt_logistics_procurement_supplier → 当前租户库同名表
-- 业务唯一键：TenantCode + CompanyCode + PurchasingOrganization + SupplierCode
-- tenant/company/plant/culture 取自源表本列；空值丢弃，不回退任务参数

IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'supplier_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [supplier_code] NVARCHAR(10) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'supplier_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [supplier_code] NVARCHAR(10) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'account_group') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [account_group] VARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'account_group') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [account_group] VARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'supplier_title') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [supplier_title] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'supplier_title') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [supplier_title] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'supplier_name1') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [supplier_name1] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'supplier_name1') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [supplier_name1] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'supplier_name2') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [supplier_name2] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'supplier_name2') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [supplier_name2] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'supplier_name3') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [supplier_name3] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'supplier_name3') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [supplier_name3] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'supplier_name4') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [supplier_name4] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'supplier_name4') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [supplier_name4] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'supplier_short_name') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [supplier_short_name] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'supplier_short_name') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [supplier_short_name] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'country_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [country_code] NVARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'country_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [country_code] NVARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'region_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [region_code] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'region_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [region_code] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'city_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [city_code] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'city_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [city_code] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'addr1') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [addr1] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'addr1') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [addr1] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'addr2') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [addr2] NVARCHAR(140) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'addr2') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [addr2] NVARCHAR(140) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'postal_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [postal_code] NVARCHAR(10) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'postal_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [postal_code] NVARCHAR(10) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'phone1') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [phone1] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'phone1') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [phone1] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'phone2') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [phone2] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'phone2') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [phone2] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'fax_number') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [fax_number] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'fax_number') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [fax_number] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'customer_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [customer_code] NVARCHAR(10) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'customer_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [customer_code] NVARCHAR(10) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'trading_partner') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [trading_partner] VARCHAR(6) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'trading_partner') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [trading_partner] VARCHAR(6) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'authorization_group') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [authorization_group] VARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'authorization_group') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [authorization_group] VARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'group_key') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [group_key] VARCHAR(10) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'group_key') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [group_key] VARCHAR(10) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'supplier_type') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [supplier_type] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'supplier_type') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [supplier_type] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'enterprise_nature') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [enterprise_nature] VARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'enterprise_nature') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [enterprise_nature] VARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'industry_attribute') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [industry_attribute] VARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'industry_attribute') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [industry_attribute] VARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'credit_info_number') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [credit_info_number] NVARCHAR(11) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'credit_info_number') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [credit_info_number] NVARCHAR(11) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'bank_country_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [bank_country_code] NVARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'bank_country_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [bank_country_code] NVARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'bank_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [bank_code] NVARCHAR(15) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'bank_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [bank_code] NVARCHAR(15) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'bank_account') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [bank_account] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'bank_account') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [bank_account] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'account_holder') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [account_holder] NVARCHAR(80) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'account_holder') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [account_holder] NVARCHAR(80) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'bank_charge_bearer_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [bank_charge_bearer_code] VARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'bank_charge_bearer_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [bank_charge_bearer_code] VARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'reconciliation_account') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [reconciliation_account] VARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'reconciliation_account') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [reconciliation_account] VARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'head_office_account_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [head_office_account_code] NVARCHAR(10) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'head_office_account_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [head_office_account_code] NVARCHAR(10) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'planning_group') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [planning_group] VARCHAR(10) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'planning_group') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [planning_group] VARCHAR(10) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'payment_terms') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [payment_terms] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'payment_terms') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [payment_terms] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'double_invoice_check') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [double_invoice_check] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'double_invoice_check') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [double_invoice_check] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'payment_method') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [payment_method] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'payment_method') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [payment_method] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'payment_block') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [payment_block] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'payment_block') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [payment_block] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'house_bank') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [house_bank] NVARCHAR(15) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'house_bank') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [house_bank] NVARCHAR(15) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'clearing_with_customer') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [clearing_with_customer] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'clearing_with_customer') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [clearing_with_customer] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'accounting_clerk') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [accounting_clerk] VARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'accounting_clerk') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [accounting_clerk] VARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'our_account_at_supplier') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [our_account_at_supplier] NVARCHAR(12) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'our_account_at_supplier') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [our_account_at_supplier] NVARCHAR(12) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'contact_person') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [contact_person] NVARCHAR(80) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'contact_person') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [contact_person] NVARCHAR(80) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'contact_phone') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [contact_phone] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'contact_phone') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [contact_phone] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'contact_email') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [contact_email] NVARCHAR(100) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'contact_email') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [contact_email] NVARCHAR(100) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'withholding_tax_country') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [withholding_tax_country] NVARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'withholding_tax_country') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [withholding_tax_country] NVARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'withholding_tax_type') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [withholding_tax_type] VARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'withholding_tax_type') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [withholding_tax_type] VARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'withholding_tax_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [withholding_tax_code] VARCHAR(2) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'withholding_tax_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [withholding_tax_code] VARCHAR(2) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'purchasing_organization') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [purchasing_organization] VARCHAR(4) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'purchasing_organization') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [purchasing_organization] VARCHAR(4) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'order_currency_code') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [order_currency_code] NVARCHAR(3) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'order_currency_code') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [order_currency_code] NVARCHAR(3) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'incoterms1') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [incoterms1] VARCHAR(3) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'incoterms1') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [incoterms1] VARCHAR(3) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'incoterms2') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [incoterms2] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'incoterms2') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [incoterms2] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'pricing_date_control') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [pricing_date_control] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'pricing_date_control') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [pricing_date_control] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'gr_based_invoice_inspection') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [gr_based_invoice_inspection] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'gr_based_invoice_inspection') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [gr_based_invoice_inspection] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'evaluated_receipt_settlement') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [evaluated_receipt_settlement] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'evaluated_receipt_settlement') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [evaluated_receipt_settlement] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'return_ers_flag') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [return_ers_flag] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'return_ers_flag') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [return_ers_flag] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'automatic_purchase_order') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [automatic_purchase_order] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'automatic_purchase_order') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [automatic_purchase_order] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'purchase_group') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [purchase_group] VARCHAR(6) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'purchase_group') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [purchase_group] VARCHAR(6) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'tax_number1') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [tax_number1] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'tax_number1') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [tax_number1] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'tax_number2') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [tax_number2] NVARCHAR(40) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'tax_number2') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [tax_number2] NVARCHAR(40) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'tax_number3') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [tax_number3] NVARCHAR(60) NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'tax_number3') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [tax_number3] NVARCHAR(60) NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'credit_level') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [credit_level] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'credit_level') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [credit_level] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'supplier_level') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [supplier_level] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'supplier_level') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [supplier_level] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'sort_order') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [sort_order] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'sort_order') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [sort_order] INT NULL;
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'supplier_status') IS NULL
  EXEC(N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] ADD [supplier_status] INT NULL');
IF COL_LENGTH(N'dbo.takt_logistics_procurement_supplier', N'supplier_status') IS NULL
  ALTER TABLE [dbo].[takt_logistics_procurement_supplier] ADD [supplier_status] INT NULL;

IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'supplier_short_name') IS NOT NULL
  AND COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'search_term') IS NOT NULL
  EXEC(N'UPDATE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier]
    SET [supplier_short_name] = LEFT(LTRIM(RTRIM([search_term])), 40)
    WHERE NULLIF(LTRIM(RTRIM([supplier_short_name])), N'''') IS NULL
      AND NULLIF(LTRIM(RTRIM([search_term])), N'''') IS NOT NULL');
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'trading_partner') IS NOT NULL
  AND COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'trading_partner_company_id') IS NOT NULL
  EXEC(N'UPDATE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier]
    SET [trading_partner] = LEFT(LTRIM(RTRIM([trading_partner_company_id])), 6)
    WHERE NULLIF(LTRIM(RTRIM([trading_partner])), N'''') IS NULL
      AND NULLIF(LTRIM(RTRIM([trading_partner_company_id])), N'''') IS NOT NULL');
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_procurement_supplier', N'purchasing_organization') IS NOT NULL
  EXEC(N'UPDATE [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier]
    SET [purchasing_organization] = LEFT(LTRIM(RTRIM([company_code])), 4)
    WHERE NULLIF(LTRIM(RTRIM([purchasing_organization])), N'''') IS NULL
      AND NULLIF(LTRIM(RTRIM([company_code])), N'''') IS NOT NULL');
IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(4),
  [supplier_code] NVARCHAR(10),
  [account_group] VARCHAR(4),
  [supplier_title] INT,
  [supplier_name1] NVARCHAR(140),
  [supplier_name2] NVARCHAR(140),
  [supplier_name3] NVARCHAR(140),
  [supplier_name4] NVARCHAR(140),
  [supplier_short_name] NVARCHAR(40),
  [country_code] NVARCHAR(2),
  [region_code] NVARCHAR(40),
  [city_code] NVARCHAR(40),
  [addr1] NVARCHAR(40),
  [addr2] NVARCHAR(140),
  [postal_code] NVARCHAR(10),
  [phone1] NVARCHAR(40),
  [phone2] NVARCHAR(40),
  [fax_number] NVARCHAR(40),
  [customer_code] NVARCHAR(10),
  [trading_partner] VARCHAR(6),
  [authorization_group] VARCHAR(4),
  [group_key] VARCHAR(10),
  [supplier_type] INT,
  [enterprise_nature] VARCHAR(4),
  [industry_attribute] VARCHAR(4),
  [credit_info_number] NVARCHAR(11),
  [bank_country_code] NVARCHAR(2),
  [bank_code] NVARCHAR(15),
  [bank_account] NVARCHAR(40),
  [account_holder] NVARCHAR(80),
  [bank_charge_bearer_code] VARCHAR(2),
  [reconciliation_account] VARCHAR(40),
  [head_office_account_code] NVARCHAR(10),
  [planning_group] VARCHAR(10),
  [payment_terms] NVARCHAR(40),
  [double_invoice_check] INT,
  [payment_method] INT,
  [payment_block] INT,
  [house_bank] NVARCHAR(15),
  [clearing_with_customer] INT,
  [accounting_clerk] VARCHAR(2),
  [our_account_at_supplier] NVARCHAR(12),
  [contact_person] NVARCHAR(80),
  [contact_phone] NVARCHAR(40),
  [contact_email] NVARCHAR(100),
  [withholding_tax_country] NVARCHAR(2),
  [withholding_tax_type] VARCHAR(2),
  [withholding_tax_code] VARCHAR(2),
  [purchasing_organization] VARCHAR(4),
  [order_currency_code] NVARCHAR(3),
  [incoterms1] VARCHAR(3),
  [incoterms2] NVARCHAR(40),
  [pricing_date_control] INT,
  [gr_based_invoice_inspection] INT,
  [evaluated_receipt_settlement] INT,
  [return_ers_flag] INT,
  [automatic_purchase_order] INT,
  [purchase_group] VARCHAR(6),
  [tax_number1] NVARCHAR(40),
  [tax_number2] NVARCHAR(40),
  [tax_number3] NVARCHAR(60),
  [credit_level] INT,
  [supplier_level] INT,
  [sort_order] INT,
  [supplier_status] INT,
  [tenant_code] NVARCHAR(3),
  [company_code] NVARCHAR(4),
  [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  S.[plant_code],
  S.[supplier_code],
  S.[account_group],
  S.[supplier_title],
  S.[supplier_name1],
  S.[supplier_name2],
  S.[supplier_name3],
  S.[supplier_name4],
  S.[supplier_short_name],
  S.[country_code],
  S.[region_code],
  S.[city_code],
  S.[addr1],
  S.[addr2],
  S.[postal_code],
  S.[phone1],
  S.[phone2],
  S.[fax_number],
  S.[customer_code],
  S.[trading_partner],
  S.[authorization_group],
  S.[group_key],
  S.[supplier_type],
  S.[enterprise_nature],
  S.[industry_attribute],
  S.[credit_info_number],
  S.[bank_country_code],
  S.[bank_code],
  S.[bank_account],
  S.[account_holder],
  S.[bank_charge_bearer_code],
  S.[reconciliation_account],
  S.[head_office_account_code],
  S.[planning_group],
  S.[payment_terms],
  S.[double_invoice_check],
  S.[payment_method],
  S.[payment_block],
  S.[house_bank],
  S.[clearing_with_customer],
  S.[accounting_clerk],
  S.[our_account_at_supplier],
  S.[contact_person],
  S.[contact_phone],
  S.[contact_email],
  S.[withholding_tax_country],
  S.[withholding_tax_type],
  S.[withholding_tax_code],
  S.[purchasing_organization],
  S.[order_currency_code],
  S.[incoterms1],
  S.[incoterms2],
  S.[pricing_date_control],
  S.[gr_based_invoice_inspection],
  S.[evaluated_receipt_settlement],
  S.[return_ers_flag],
  S.[automatic_purchase_order],
  S.[purchase_group],
  S.[tax_number1],
  S.[tax_number2],
  S.[tax_number3],
  S.[credit_level],
  S.[supplier_level],
  S.[sort_order],
  S.[supplier_status],
  S.[tenant_code],
  S.[company_code],
  S.[culture_code],
  S.[ext_field],
  S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[company_code], N.[purchasing_organization], N.[supplier_code]) AS rn
  FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[supplier_code], N''))), 10), N''), N'') AS [supplier_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[account_group], N''))), 4), N''), N'') AS [account_group],
      COALESCE(TRY_CAST(R.[supplier_title] AS INT), 1) AS [supplier_title],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[supplier_name1], N''))), 140), N''), N'') AS [supplier_name1],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[supplier_name2], N''))), 140), N'') AS [supplier_name2],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[supplier_name3], N''))), 140), N'') AS [supplier_name3],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[supplier_name4], N''))), 140), N'') AS [supplier_name4],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[supplier_short_name], N''))), 40), N'') AS [supplier_short_name],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[country_code], N''))), 2), N'') AS [country_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[region_code], N''))), 40), N'') AS [region_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[city_code], N''))), 40), N'') AS [city_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[addr1], N''))), 40), N'') AS [addr1],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[addr2], N''))), 140), N'') AS [addr2],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[postal_code], N''))), 10), N'') AS [postal_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[phone1], N''))), 40), N'') AS [phone1],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[phone2], N''))), 40), N'') AS [phone2],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[fax_number], N''))), 40), N'') AS [fax_number],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[customer_code], N''))), 10), N'') AS [customer_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[trading_partner], N''))), 6), N'') AS [trading_partner],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[authorization_group], N''))), 4), N'') AS [authorization_group],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[group_key], N''))), 10), N'') AS [group_key],
      COALESCE(TRY_CAST(R.[supplier_type] AS INT), 0) AS [supplier_type],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[enterprise_nature], N''))), 4), N''), N'150') AS [enterprise_nature],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[industry_attribute], N''))), 4), N''), N'C') AS [industry_attribute],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[credit_info_number], N''))), 11), N'') AS [credit_info_number],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[bank_country_code], N''))), 2), N'') AS [bank_country_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[bank_code], N''))), 15), N'') AS [bank_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[bank_account], N''))), 40), N'') AS [bank_account],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[account_holder], N''))), 80), N'') AS [account_holder],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[bank_charge_bearer_code], N''))), 2), N'') AS [bank_charge_bearer_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[reconciliation_account], N''))), 40), N''), N'') AS [reconciliation_account],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[head_office_account_code], N''))), 10), N'') AS [head_office_account_code],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[planning_group], N''))), 10), N'') AS [planning_group],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[payment_terms], N''))), 40), N''), N'COD') AS [payment_terms],
      COALESCE(TRY_CAST(R.[double_invoice_check] AS INT), 1) AS [double_invoice_check],
      COALESCE(TRY_CAST(R.[payment_method] AS INT), 0) AS [payment_method],
      COALESCE(TRY_CAST(R.[payment_block] AS INT), 0) AS [payment_block],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[house_bank], N''))), 15), N'') AS [house_bank],
      COALESCE(TRY_CAST(R.[clearing_with_customer] AS INT), 0) AS [clearing_with_customer],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[accounting_clerk], N''))), 2), N'') AS [accounting_clerk],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[our_account_at_supplier], N''))), 12), N'') AS [our_account_at_supplier],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[contact_person], N''))), 80), N'') AS [contact_person],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[contact_phone], N''))), 40), N'') AS [contact_phone],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[contact_email], N''))), 100), N'') AS [contact_email],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[withholding_tax_country], N''))), 2), N'') AS [withholding_tax_country],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[withholding_tax_type], N''))), 2), N'') AS [withholding_tax_type],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[withholding_tax_code], N''))), 2), N'') AS [withholding_tax_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[purchasing_organization], N''))), 4), N''), N'') AS [purchasing_organization],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[order_currency_code], N''))), 3), N'') AS [order_currency_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[incoterms1], N''))), 3), N''), N'FOB') AS [incoterms1],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[incoterms2], N''))), 40), N''), N'') AS [incoterms2],
      COALESCE(TRY_CAST(R.[pricing_date_control] AS INT), 1) AS [pricing_date_control],
      COALESCE(TRY_CAST(R.[gr_based_invoice_inspection] AS INT), 0) AS [gr_based_invoice_inspection],
      COALESCE(TRY_CAST(R.[evaluated_receipt_settlement] AS INT), 0) AS [evaluated_receipt_settlement],
      COALESCE(TRY_CAST(R.[return_ers_flag] AS INT), 0) AS [return_ers_flag],
      COALESCE(TRY_CAST(R.[automatic_purchase_order] AS INT), 0) AS [automatic_purchase_order],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[purchase_group], N''))), 6), N''), N'') AS [purchase_group],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tax_number1], N''))), 40), N'') AS [tax_number1],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tax_number2], N''))), 40), N'') AS [tax_number2],
      NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[tax_number3], N''))), 60), N'') AS [tax_number3],
      COALESCE(TRY_CAST(R.[credit_level] AS INT), 0) AS [credit_level],
      COALESCE(TRY_CAST(R.[supplier_level] AS INT), 0) AS [supplier_level],
      COALESCE(TRY_CAST(R.[sort_order] AS INT), 0) AS [sort_order],
      COALESCE(TRY_CAST(R.[supplier_status] AS INT), 1) AS [supplier_status],
      ISNULL(R.[ext_field], N'{}') AS [ext_field],
      ISNULL(R.[remark], N'') AS [remark],
      COALESCE(TRY_CAST(R.[created_by] AS BIGINT), 0) AS [created_by],
      R.[created_at] AS [created_at],
      TRY_CAST(R.[updated_by] AS BIGINT) AS [updated_by],
      R.[updated_at] AS [updated_at],
      TRY_CAST(R.[deleted_by] AS BIGINT) AS [deleted_by],
      R.[deleted_at] AS [deleted_at],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
      ROW_NUMBER() OVER (
        PARTITION BY
          LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3),
          LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4),
          LTRIM(RTRIM(ISNULL(R.[purchasing_organization], N''))),
          LTRIM(RTRIM(ISNULL(R.[supplier_code], N'')))
        ORDER BY ISNULL(R.[is_deleted], 0), ISNULL(R.[updated_at], CAST('1900-01-01' AS DATETIME)) DESC
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_supplier] R
    WHERE LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[purchasing_organization], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[supplier_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @source_count INT = (SELECT COUNT(*) FROM #st_source);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @source_count AS [to_rn],
  @source_count AS [max_rn],
  @source_count AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@source_count) AS NVARCHAR(20)), N'|',
  CAST((@source_count) AS NVARCHAR(20)), N'|',
  CAST((@source_count) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;

IF EXISTS (
  SELECT 1 FROM #st_source
  GROUP BY [tenant_code], [company_code], [purchasing_organization], [supplier_code]
  HAVING COUNT(*) > 1
)
BEGIN
  THROW 50001, N'业务键重复', 1;
END;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT
);

DECLARE @target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_procurement_supplier] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

UPDATE S
SET S.[id] = COALESCE(T.[id], S.[id])
FROM #st_source S
LEFT JOIN [takt_logistics_procurement_supplier] T
  ON T.[tenant_code] = S.[tenant_code]
 AND T.[company_code] = S.[company_code]
 AND LTRIM(RTRIM(T.[purchasing_organization]))=S.[purchasing_organization]
  AND LTRIM(RTRIM(T.[supplier_code]))=S.[supplier_code];

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #st_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_procurement_supplier] AS T
USING (SELECT * FROM #st_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = S.[tenant_code]
AND T.[company_code] = S.[company_code]
AND LTRIM(RTRIM(T.[purchasing_organization]))=S.[purchasing_organization]
 AND LTRIM(RTRIM(T.[supplier_code]))=S.[supplier_code]
WHEN MATCHED AND (
  LTRIM(RTRIM(ISNULL(T.[plant_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[plant_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[supplier_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[supplier_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[account_group], N'')))<>LTRIM(RTRIM(ISNULL(S.[account_group], N'')))
  OR T.[supplier_title]<>S.[supplier_title]
  OR LTRIM(RTRIM(ISNULL(T.[supplier_name1], N'')))<>LTRIM(RTRIM(ISNULL(S.[supplier_name1], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[supplier_name2], N'')))<>LTRIM(RTRIM(ISNULL(S.[supplier_name2], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[supplier_name3], N'')))<>LTRIM(RTRIM(ISNULL(S.[supplier_name3], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[supplier_name4], N'')))<>LTRIM(RTRIM(ISNULL(S.[supplier_name4], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[supplier_short_name], N'')))<>LTRIM(RTRIM(ISNULL(S.[supplier_short_name], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[country_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[country_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[region_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[region_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[city_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[city_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[addr1], N'')))<>LTRIM(RTRIM(ISNULL(S.[addr1], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[addr2], N'')))<>LTRIM(RTRIM(ISNULL(S.[addr2], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[postal_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[postal_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[phone1], N'')))<>LTRIM(RTRIM(ISNULL(S.[phone1], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[phone2], N'')))<>LTRIM(RTRIM(ISNULL(S.[phone2], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[fax_number], N'')))<>LTRIM(RTRIM(ISNULL(S.[fax_number], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[customer_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[customer_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[trading_partner], N'')))<>LTRIM(RTRIM(ISNULL(S.[trading_partner], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[authorization_group], N'')))<>LTRIM(RTRIM(ISNULL(S.[authorization_group], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[group_key], N'')))<>LTRIM(RTRIM(ISNULL(S.[group_key], N'')))
  OR T.[supplier_type]<>S.[supplier_type]
  OR LTRIM(RTRIM(ISNULL(T.[enterprise_nature], N'')))<>LTRIM(RTRIM(ISNULL(S.[enterprise_nature], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[industry_attribute], N'')))<>LTRIM(RTRIM(ISNULL(S.[industry_attribute], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[credit_info_number], N'')))<>LTRIM(RTRIM(ISNULL(S.[credit_info_number], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[bank_country_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[bank_country_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[bank_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[bank_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[bank_account], N'')))<>LTRIM(RTRIM(ISNULL(S.[bank_account], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[account_holder], N'')))<>LTRIM(RTRIM(ISNULL(S.[account_holder], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[bank_charge_bearer_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[bank_charge_bearer_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[reconciliation_account], N'')))<>LTRIM(RTRIM(ISNULL(S.[reconciliation_account], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[head_office_account_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[head_office_account_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[planning_group], N'')))<>LTRIM(RTRIM(ISNULL(S.[planning_group], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[payment_terms], N'')))<>LTRIM(RTRIM(ISNULL(S.[payment_terms], N'')))
  OR T.[double_invoice_check]<>S.[double_invoice_check]
  OR T.[payment_method]<>S.[payment_method]
  OR T.[payment_block]<>S.[payment_block]
  OR LTRIM(RTRIM(ISNULL(T.[house_bank], N'')))<>LTRIM(RTRIM(ISNULL(S.[house_bank], N'')))
  OR T.[clearing_with_customer]<>S.[clearing_with_customer]
  OR LTRIM(RTRIM(ISNULL(T.[accounting_clerk], N'')))<>LTRIM(RTRIM(ISNULL(S.[accounting_clerk], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[our_account_at_supplier], N'')))<>LTRIM(RTRIM(ISNULL(S.[our_account_at_supplier], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[contact_person], N'')))<>LTRIM(RTRIM(ISNULL(S.[contact_person], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[contact_phone], N'')))<>LTRIM(RTRIM(ISNULL(S.[contact_phone], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[contact_email], N'')))<>LTRIM(RTRIM(ISNULL(S.[contact_email], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[withholding_tax_country], N'')))<>LTRIM(RTRIM(ISNULL(S.[withholding_tax_country], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[withholding_tax_type], N'')))<>LTRIM(RTRIM(ISNULL(S.[withholding_tax_type], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[withholding_tax_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[withholding_tax_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[purchasing_organization], N'')))<>LTRIM(RTRIM(ISNULL(S.[purchasing_organization], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[order_currency_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[order_currency_code], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[incoterms1], N'')))<>LTRIM(RTRIM(ISNULL(S.[incoterms1], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[incoterms2], N'')))<>LTRIM(RTRIM(ISNULL(S.[incoterms2], N'')))
  OR T.[pricing_date_control]<>S.[pricing_date_control]
  OR T.[gr_based_invoice_inspection]<>S.[gr_based_invoice_inspection]
  OR T.[evaluated_receipt_settlement]<>S.[evaluated_receipt_settlement]
  OR T.[return_ers_flag]<>S.[return_ers_flag]
  OR T.[automatic_purchase_order]<>S.[automatic_purchase_order]
  OR LTRIM(RTRIM(ISNULL(T.[purchase_group], N'')))<>LTRIM(RTRIM(ISNULL(S.[purchase_group], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[tax_number1], N'')))<>LTRIM(RTRIM(ISNULL(S.[tax_number1], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[tax_number2], N'')))<>LTRIM(RTRIM(ISNULL(S.[tax_number2], N'')))
  OR LTRIM(RTRIM(ISNULL(T.[tax_number3], N'')))<>LTRIM(RTRIM(ISNULL(S.[tax_number3], N'')))
  OR T.[credit_level]<>S.[credit_level]
  OR T.[supplier_level]<>S.[supplier_level]
  OR T.[sort_order]<>S.[sort_order]
  OR T.[supplier_status]<>S.[supplier_status]
  OR LTRIM(RTRIM(ISNULL(T.[culture_code], N'')))<>LTRIM(RTRIM(ISNULL(S.[culture_code], N'')))
  OR ISNULL(T.[is_deleted], 0)<>S.[is_deleted]
  OR ISNULL(T.[ext_field], N'')<>ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'')<>ISNULL(S.[remark], N'')
) THEN
  UPDATE SET
  T.[plant_code]=S.[plant_code],
  T.[supplier_code]=S.[supplier_code],
  T.[account_group]=S.[account_group],
  T.[supplier_title]=S.[supplier_title],
  T.[supplier_name1]=S.[supplier_name1],
  T.[supplier_name2]=S.[supplier_name2],
  T.[supplier_name3]=S.[supplier_name3],
  T.[supplier_name4]=S.[supplier_name4],
  T.[supplier_short_name]=S.[supplier_short_name],
  T.[country_code]=S.[country_code],
  T.[region_code]=S.[region_code],
  T.[city_code]=S.[city_code],
  T.[addr1]=S.[addr1],
  T.[addr2]=S.[addr2],
  T.[postal_code]=S.[postal_code],
  T.[phone1]=S.[phone1],
  T.[phone2]=S.[phone2],
  T.[fax_number]=S.[fax_number],
  T.[customer_code]=S.[customer_code],
  T.[trading_partner]=S.[trading_partner],
  T.[authorization_group]=S.[authorization_group],
  T.[group_key]=S.[group_key],
  T.[supplier_type]=S.[supplier_type],
  T.[enterprise_nature]=S.[enterprise_nature],
  T.[industry_attribute]=S.[industry_attribute],
  T.[credit_info_number]=S.[credit_info_number],
  T.[bank_country_code]=S.[bank_country_code],
  T.[bank_code]=S.[bank_code],
  T.[bank_account]=S.[bank_account],
  T.[account_holder]=S.[account_holder],
  T.[bank_charge_bearer_code]=S.[bank_charge_bearer_code],
  T.[reconciliation_account]=S.[reconciliation_account],
  T.[head_office_account_code]=S.[head_office_account_code],
  T.[planning_group]=S.[planning_group],
  T.[payment_terms]=S.[payment_terms],
  T.[double_invoice_check]=S.[double_invoice_check],
  T.[payment_method]=S.[payment_method],
  T.[payment_block]=S.[payment_block],
  T.[house_bank]=S.[house_bank],
  T.[clearing_with_customer]=S.[clearing_with_customer],
  T.[accounting_clerk]=S.[accounting_clerk],
  T.[our_account_at_supplier]=S.[our_account_at_supplier],
  T.[contact_person]=S.[contact_person],
  T.[contact_phone]=S.[contact_phone],
  T.[contact_email]=S.[contact_email],
  T.[withholding_tax_country]=S.[withholding_tax_country],
  T.[withholding_tax_type]=S.[withholding_tax_type],
  T.[withholding_tax_code]=S.[withholding_tax_code],
  T.[purchasing_organization]=S.[purchasing_organization],
  T.[order_currency_code]=S.[order_currency_code],
  T.[incoterms1]=S.[incoterms1],
  T.[incoterms2]=S.[incoterms2],
  T.[pricing_date_control]=S.[pricing_date_control],
  T.[gr_based_invoice_inspection]=S.[gr_based_invoice_inspection],
  T.[evaluated_receipt_settlement]=S.[evaluated_receipt_settlement],
  T.[return_ers_flag]=S.[return_ers_flag],
  T.[automatic_purchase_order]=S.[automatic_purchase_order],
  T.[purchase_group]=S.[purchase_group],
  T.[tax_number1]=S.[tax_number1],
  T.[tax_number2]=S.[tax_number2],
  T.[tax_number3]=S.[tax_number3],
  T.[credit_level]=S.[credit_level],
  T.[supplier_level]=S.[supplier_level],
  T.[sort_order]=S.[sort_order],
  T.[supplier_status]=S.[supplier_status],
  T.[culture_code]=S.[culture_code],
  T.[ext_field]=S.[ext_field],
  T.[remark]=S.[remark],
  T.[created_by]=S.[created_by],
  T.[created_at]=COALESCE(S.[created_at], T.[created_at], @now),
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=S.[updated_at],
  T.[is_deleted]=S.[is_deleted],
  T.[deleted_by]=S.[deleted_by],
  T.[deleted_at]=S.[deleted_at]
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[supplier_code],[account_group],[supplier_title],[supplier_name1],[supplier_name2],[supplier_name3],[supplier_name4],[supplier_short_name],[country_code],[region_code],[city_code],[addr1],[addr2],[postal_code],[phone1],[phone2],[fax_number],[customer_code],[trading_partner],[authorization_group],[group_key],[supplier_type],[enterprise_nature],[industry_attribute],[credit_info_number],[bank_country_code],[bank_code],[bank_account],[account_holder],[bank_charge_bearer_code],[reconciliation_account],[head_office_account_code],[planning_group],[payment_terms],[double_invoice_check],[payment_method],[payment_block],[house_bank],[clearing_with_customer],[accounting_clerk],[our_account_at_supplier],[contact_person],[contact_phone],[contact_email],[withholding_tax_country],[withholding_tax_type],[withholding_tax_code],[purchasing_organization],[order_currency_code],[incoterms1],[incoterms2],[pricing_date_control],[gr_based_invoice_inspection],[evaluated_receipt_settlement],[return_ers_flag],[automatic_purchase_order],[purchase_group],[tax_number1],[tax_number2],[tax_number3],[credit_level],[supplier_level],[sort_order],[supplier_status],[tenant_code],[company_code],[culture_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[supplier_code],S.[account_group],S.[supplier_title],S.[supplier_name1],S.[supplier_name2],S.[supplier_name3],S.[supplier_name4],S.[supplier_short_name],S.[country_code],S.[region_code],S.[city_code],S.[addr1],S.[addr2],S.[postal_code],S.[phone1],S.[phone2],S.[fax_number],S.[customer_code],S.[trading_partner],S.[authorization_group],S.[group_key],S.[supplier_type],S.[enterprise_nature],S.[industry_attribute],S.[credit_info_number],S.[bank_country_code],S.[bank_code],S.[bank_account],S.[account_holder],S.[bank_charge_bearer_code],S.[reconciliation_account],S.[head_office_account_code],S.[planning_group],S.[payment_terms],S.[double_invoice_check],S.[payment_method],S.[payment_block],S.[house_bank],S.[clearing_with_customer],S.[accounting_clerk],S.[our_account_at_supplier],S.[contact_person],S.[contact_phone],S.[contact_email],S.[withholding_tax_country],S.[withholding_tax_type],S.[withholding_tax_code],S.[purchasing_organization],S.[order_currency_code],S.[incoterms1],S.[incoterms2],S.[pricing_date_control],S.[gr_based_invoice_inspection],S.[evaluated_receipt_settlement],S.[return_ers_flag],S.[automatic_purchase_order],S.[purchase_group],S.[tax_number1],S.[tax_number2],S.[tax_number3],S.[credit_level],S.[supplier_level],S.[sort_order],S.[supplier_status],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],
    COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],
    S.[is_deleted],S.[deleted_by],S.[deleted_at]
  )
OUTPUT S.rn, $action, INSERTED.[id]
INTO #delta(rn, oper_type, id);
  SET @dml_n = @@ROWCOUNT;
  SELECT
    N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
    N'merge' AS [phase],
    @merge_from_rn AS [from_rn],
    CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END AS [to_rn],
    @merge_max_rn AS [max_rn],
    @dml_n AS [batch_rows];
  SET @progress_msg = CONCAT(
    N'QUARTZ_SYNC_PROGRESS|',
    N'merge', N'|',
    CAST((@merge_from_rn) AS NVARCHAR(20)), N'|',
    CAST((CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END) AS NVARCHAR(20)), N'|',
    CAST((@merge_max_rn) AS NVARCHAR(20)), N'|',
    CAST((@dml_n) AS NVARCHAR(20)), N'|',
    N'');
  RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
  SET @merge_from_rn = @merge_to_rn + 1;
END

DECLARE @delete_count INT = 0;
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
  UPDATE TOP (@apply_chunk) T
  SET
    T.[is_deleted] = 1,
    T.[deleted_by] = @sync_user_id,
    T.[deleted_at] = @now,
    T.[updated_by] = @sync_user_id,
    T.[updated_at] = @now
  FROM [takt_logistics_procurement_supplier] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
    AND NOT EXISTS (
      SELECT 1 FROM #st_source S
      WHERE S.[tenant_code] = T.[tenant_code]
        AND S.[company_code] = T.[company_code]
        AND S.[purchasing_organization] = LTRIM(RTRIM(T.[purchasing_organization]))
        AND S.[supplier_code] = LTRIM(RTRIM(T.[supplier_code]))
    );
  SET @dml_n = @@ROWCOUNT;
  SET @delete_count = @delete_count + @dml_n;
END

DECLARE @target_count INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_procurement_supplier] T
  WHERE T.[is_deleted] = 0
    AND EXISTS (SELECT 1 FROM #st_source S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @source_active_count INT = (SELECT COUNT(*) FROM #st_source WHERE [is_deleted] = 0);
IF @target_count <> @source_active_count
BEGIN
  DECLARE @count_msg NVARCHAR(200) = CONCAT(N'有效行数不一致: source=', @source_active_count, N', active=', @target_count);
  THROW 50002, @count_msg, 1;
END;

DECLARE @insert_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'UPDATE');
DECLARE @unchanged_count INT = @source_count - @insert_count - @update_count;
DECLARE @json_result NVARCHAR(MAX) =
  N'{"source":' + CAST(@source_count AS NVARCHAR)
  + N',"target_before":' + CAST(@target_before AS NVARCHAR)
  + N',"target_after":' + CAST(@target_count AS NVARCHAR)
  + N',"insert":' + CAST(@insert_count AS NVARCHAR)
  + N',"update":' + CAST(@update_count AS NVARCHAR)
  + N',"unchanged":' + CAST(@unchanged_count AS NVARCHAR)
  + N',"soft_delete_this_run":' + CAST(@delete_count AS NVARCHAR) + N'}';

INSERT INTO [takt_statistics_logging_delta_log] (
  [id],[oper_type],[table_name],[primary_key_id],
  [before_data],[after_data],[diff_data],[sql_statement],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[tenant_code],[company_code],[plant_code],[culture_code],
  [ext_field],[remark],[created_by],[created_at]
)
VALUES (
  @base_id,
  N'SYNC',
  N'takt_logistics_procurement_supplier',
  NULL,
  N'{}',
  ISNULL(@json_result, N'{}'),
  N'{}',
  N'MERGE TaktSupplier Sync (summary)',
  N'127.0.0.1',N'Server',N'SQLCMD',N'Server',N'Windows',N'Server',
  @now,0,
  @tenant_code,@company_code,@plant_code,@culture_code,
  N'{}',N'SYNC batch summary',@sync_user_id,@now
);

INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[plant_code],[culture_code],[created_by],[created_at]
)
VALUES (
  @base_id + 1,
  N'SYSTEM_SYNC',
  N'SYNC',
  N'供货商信息',
  N'exec_sql_merge',
  'SQL',
  N'/sync/supplier',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@plant_code,@culture_code,@sync_user_id,@now
);

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  @source_count AS [source_count],
  @target_before AS [target_before],
  @target_count AS [target_after],
  @insert_count AS [insert_count],
  @update_count AS [update_count],
  @unchanged_count AS [unchanged_count],
  @delete_count AS [delete_count];
