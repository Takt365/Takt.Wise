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
DECLARE @ddl NVARCHAR(500);

-- 源/目标列与实体 TaktMaterialDocument / Item 一致
-- plant_code 取自各源表本列（主表 R.plant_code、明细 R.plant_code）；空 plant 丢弃，不回退 @plant_code
-- tenant/company/plant/culture 取自各源表本列；空值丢弃，不回退任务参数
-- 主表唯一键：Tenant+Company+material_document_year+material_document_code
-- 明细唯一键：material_document_id+line_number
-- 源明细不回写 material_document_id：回写源库失败会中断整批，目标子表 MERGE 不会执行
-- 装入按凭证码关联源主表取年；#item 的 material_document_id 再对齐目标主表雪花 id
-- 源侧同凭证码跨年时一行会挂到各年主表；当前源库同码无多年（已核对）
-- posted_by：主表单据级；源明细无此列，装入恒为 NULL

-- 目标库主表缺 posted_by 时补列（实体已有）
IF COL_LENGTH(N'dbo.takt_logistics_materials_material_document', N'posted_by') IS NULL
  ALTER TABLE [dbo].[takt_logistics_materials_material_document] ADD [posted_by] NVARCHAR(6) NULL;
IF COL_LENGTH(N'dbo.takt_logistics_materials_material_document_item', N'posted_by') IS NULL
  ALTER TABLE [dbo].[takt_logistics_materials_material_document_item] ADD [posted_by] NVARCHAR(6) NULL;

-- 源库主表缺 posted_by 时补列（旧暂存表无此列）
IF COL_LENGTH(N'{{SourceDatabase}}.dbo.takt_logistics_materials_material_document', N'posted_by') IS NULL
BEGIN
  SET @ddl = N'ALTER TABLE [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document] ADD [posted_by] NVARCHAR(6) NULL';
  EXEC sys.sp_executesql @ddl;
END

IF OBJECT_ID('tempdb..#hdr') IS NOT NULL DROP TABLE #hdr;
IF OBJECT_ID('tempdb..#item') IS NOT NULL DROP TABLE #item;
IF OBJECT_ID('tempdb..#hdr_delta') IS NOT NULL DROP TABLE #hdr_delta;
IF OBJECT_ID('tempdb..#item_delta') IS NOT NULL DROP TABLE #item_delta;
IF OBJECT_ID('tempdb..#hdr_soft') IS NOT NULL DROP TABLE #hdr_soft;
IF OBJECT_ID('tempdb..#item_soft') IS NOT NULL DROP TABLE #item_soft;

CREATE TABLE #hdr (
  [rn] INT, [id] BIGINT, [plant_code] NVARCHAR(4),
  [material_document_code] NVARCHAR(10), [material_document_year] NVARCHAR(4),
  [transaction_event_type] NVARCHAR(2), [document_type] NVARCHAR(2), [revaluation_type] NVARCHAR(2),
  [document_date] DATETIME, [posting_date] DATETIME,
  [reference_code] NVARCHAR(16), [header_text] NVARCHAR(25),
  [bill_of_lading_code] NVARCHAR(16), [delivery_code] NVARCHAR(10),
  [transaction_code] NVARCHAR(40), [posted_by] NVARCHAR(6),
  [tenant_code] NVARCHAR(3), [company_code] NVARCHAR(4), [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

CREATE TABLE #item (
  [rn] INT, [id] BIGINT, [material_document_id] BIGINT,
  [material_document_year] NVARCHAR(4), [material_document_code] NVARCHAR(10), [line_number] INT,
  [plant_code] NVARCHAR(4),
  [line_id] NVARCHAR(6), [parent_line_id] NVARCHAR(6), [line_depth] NVARCHAR(2),
  [movement_type] NVARCHAR(3), [auto_created_flag] NVARCHAR(1),
  [material_code] NVARCHAR(20), [warehouse_code] NVARCHAR(4), [batch_code] NVARCHAR(10),
  [stock_type] NVARCHAR(1), [restricted_stock_flag] NVARCHAR(1), [special_stock] NVARCHAR(1),
  [supplier_code] NVARCHAR(10), [customer_code] NVARCHAR(10),
  [debit_credit_indicator] NVARCHAR(1), [currency_code] NVARCHAR(3),
  [local_currency_amount] DECIMAL(13,2), [alternative_amount] DECIMAL(13,2),
  [quantity] DECIMAL(13,3), [base_unit] NVARCHAR(3),
  [entry_quantity] DECIMAL(13,3), [entry_unit] NVARCHAR(3),
  [po_price_quantity] DECIMAL(13,3), [po_price_unit] NVARCHAR(3),
  [purchase_order_code] NVARCHAR(20), [purchase_order_item] INT,
  [reference_document_year] NVARCHAR(4), [reference_document_code] NVARCHAR(10), [reference_document_item] INT,
  [original_material_document_year] NVARCHAR(4), [original_material_document_code] NVARCHAR(10), [original_line_number] INT,
  [delivery_completed_flag] NVARCHAR(1), [item_text] NVARCHAR(50),
  [equipment_code] NVARCHAR(18), [goods_recipient] NVARCHAR(12), [unloading_point] NVARCHAR(25),
  [business_area_code] NVARCHAR(4), [controlling_area_code] NVARCHAR(4), [trading_partner_business_area] NVARCHAR(4),
  [production_order_code] NVARCHAR(12), [asset_code] NVARCHAR(12), [asset_sub_code] NVARCHAR(4),
  [fiscal_year] NVARCHAR(4), [post_to_previous_period_flag] NVARCHAR(1), [post_to_previous_year_flag] NVARCHAR(1),
  [accounting_document_code] NVARCHAR(10), [accounting_document_item] INT,
  [revaluation_document_code] NVARCHAR(10), [revaluation_document_item] NVARCHAR(3),
  [reservation_code] NVARCHAR(10), [reservation_item] INT,
  [final_issue_flag] NVARCHAR(1), [reservation_quantity] DECIMAL(13,3),
  [receiving_material_code] NVARCHAR(20), [receiving_plant_code] NVARCHAR(4), [receiving_warehouse_code] NVARCHAR(4),
  [profit_center_code] NVARCHAR(10),
  [valuated_stock_quantity] DECIMAL(13,3), [total_valuated_stock_value] DECIMAL(13,2),
  [price_control] NVARCHAR(1), [manufacturer_part_material_code] NVARCHAR(40),
  [mkpf_reference_code] NVARCHAR(32), [im_delivery_code] NVARCHAR(20), [im_delivery_item] INT,
  [posted_by] NVARCHAR(6), [is_obsolete] INT,
  [tenant_code] NVARCHAR(3), [company_code] NVARCHAR(4), [culture_code] NVARCHAR(5),
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

CREATE TABLE #hdr_delta (rn INT, oper_type NVARCHAR(10), id BIGINT, [material_document_year] NVARCHAR(4), [material_document_code] NVARCHAR(10));
CREATE TABLE #item_delta (rn INT, oper_type NVARCHAR(10), id BIGINT, [material_document_code] NVARCHAR(10), [line_number] INT);
CREATE TABLE #hdr_soft ([id] BIGINT, [material_document_year] NVARCHAR(4), [material_document_code] NVARCHAR(10));
CREATE TABLE #item_soft ([id] BIGINT, [material_document_code] NVARCHAR(10), [line_number] INT);

INSERT INTO #hdr
SELECT S.rn, @base_id + S.rn, S.[plant_code],
  S.[material_document_code], S.[material_document_year],
  S.[transaction_event_type], S.[document_type], S.[revaluation_type],
  S.[document_date], S.[posting_date],
  S.[reference_code], S.[header_text], S.[bill_of_lading_code], S.[delivery_code],
  S.[transaction_code], S.[posted_by],
  S.[tenant_code], S.[company_code], S.[culture_code], S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT N.*, ROW_NUMBER() OVER (ORDER BY N.[material_document_year], N.[material_document_code]) AS rn
  FROM (
    SELECT
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_document_code])), 10), N''), N'') AS [material_document_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_document_year])), 4), N''), N'') AS [material_document_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[transaction_event_type])), 2), N'') AS [transaction_event_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_type])), 2), N'') AS [document_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[revaluation_type])), 2), N'') AS [revaluation_type],
      ISNULL(TRY_CAST(R.[document_date] AS DATETIME), CAST('1900-01-01' AS DATETIME)) AS [document_date],
      ISNULL(TRY_CAST(R.[posting_date] AS DATETIME), CAST('1900-01-01' AS DATETIME)) AS [posting_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_code])), 16), N'') AS [reference_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[header_text])), 25), N'') AS [header_text],
      NULLIF(LEFT(LTRIM(RTRIM(R.[bill_of_lading_code])), 16), N'') AS [bill_of_lading_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[delivery_code])), 10), N'') AS [delivery_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[transaction_code])), 40), N'') AS [transaction_code],
      -- 源主表旧暂存可能无 posted_by；同批 ALTER 不可见，装入恒 NULL（MERGE 不回写覆盖）
      CAST(NULL AS NVARCHAR(6)) AS [posted_by],
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
        PARTITION BY LTRIM(RTRIM(R.[material_document_year])), LTRIM(RTRIM(R.[material_document_code]))
        ORDER BY LTRIM(RTRIM(R.[material_document_year])), LTRIM(RTRIM(R.[material_document_code]))
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_document_year], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[material_document_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @hdr_source INT = (SELECT COUNT(*) FROM #hdr);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @hdr_source AS [to_rn],
  @hdr_source AS [max_rn],
  @hdr_source AS [batch_rows],
  N'main' AS [scope];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@hdr_source) AS NVARCHAR(20)), N'|',
  CAST((@hdr_source) AS NVARCHAR(20)), N'|',
  CAST((@hdr_source) AS NVARCHAR(20)), N'|',
  N'main');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @hdr_sap_keys INT = (
  SELECT COUNT(*) FROM (
    SELECT LTRIM(RTRIM(R.[material_document_year])) AS [material_document_year], LTRIM(RTRIM(R.[material_document_code])) AS [material_document_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_document_year], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[material_document_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
    GROUP BY LTRIM(RTRIM(R.[material_document_year])), LTRIM(RTRIM(R.[material_document_code]))
  ) K
);
IF @hdr_source <> @hdr_sap_keys
BEGIN
  DECLARE @hdr_src_msg NVARCHAR(200) = CONCAT(N'主表业务键装入不一致: keys=', @hdr_sap_keys, N', loaded=', @hdr_source);
  THROW 50003, @hdr_src_msg, 1;
END;

DECLARE @hdr_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_materials_material_document] T
  WHERE T.[is_deleted]=0
    AND EXISTS (
      SELECT 1 FROM #hdr S
      WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code]
    )
);

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #hdr), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE [takt_logistics_materials_material_document] AS T
USING (SELECT * FROM #hdr WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[material_document_year]))=S.[material_document_year]
 AND LTRIM(RTRIM(T.[material_document_code]))=S.[material_document_code]
WHEN MATCHED AND (
  ISNULL(T.[transaction_event_type],N'')<>ISNULL(S.[transaction_event_type],N'')
  OR ISNULL(T.[document_type],N'')<>ISNULL(S.[document_type],N'')
  OR ISNULL(T.[revaluation_type],N'')<>ISNULL(S.[revaluation_type],N'')
  OR ISNULL(T.[document_date],CAST('1900-01-01' AS DATETIME))<>ISNULL(S.[document_date],CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[posting_date],CAST('1900-01-01' AS DATETIME))<>ISNULL(S.[posting_date],CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[reference_code],N'')<>ISNULL(S.[reference_code],N'')
  OR ISNULL(T.[header_text],N'')<>ISNULL(S.[header_text],N'')
  OR ISNULL(T.[bill_of_lading_code],N'')<>ISNULL(S.[bill_of_lading_code],N'')
  OR ISNULL(T.[delivery_code],N'')<>ISNULL(S.[delivery_code],N'')
  OR ISNULL(T.[transaction_code],N'')<>ISNULL(S.[transaction_code],N'')
  OR ISNULL(T.[plant_code],N'')<>ISNULL(S.[plant_code],N'')
  OR ISNULL(T.[culture_code],N'')<>ISNULL(S.[culture_code],N'')
  OR T.[is_deleted]<>S.[is_deleted]

  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN UPDATE SET
  T.[transaction_event_type]=S.[transaction_event_type],
  T.[document_type]=S.[document_type],
  T.[revaluation_type]=S.[revaluation_type],
  T.[document_date]=S.[document_date],
  T.[posting_date]=S.[posting_date],
  T.[reference_code]=S.[reference_code],
  T.[header_text]=S.[header_text],
  T.[bill_of_lading_code]=S.[bill_of_lading_code],
  T.[delivery_code]=S.[delivery_code],
  T.[transaction_code]=S.[transaction_code],
  T.[plant_code]=S.[plant_code],
  T.[culture_code]=S.[culture_code],
  T.[ext_field]=S.[ext_field],
  T.[remark]=S.[remark],
  T.[created_by]=S.[created_by],
  T.[created_at]=S.[created_at],
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=S.[updated_at],
  T.[is_deleted]=S.[is_deleted],
  T.[deleted_by]=S.[deleted_by],
  T.[deleted_at]=S.[deleted_at]
WHEN NOT MATCHED THEN
  INSERT ([id],[plant_code],[material_document_code],[material_document_year],[transaction_event_type],[document_type],[revaluation_type],[document_date],[posting_date],[reference_code],[header_text],[bill_of_lading_code],[delivery_code],[transaction_code],[tenant_code],[company_code],[culture_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[plant_code],S.[material_document_code],S.[material_document_year],S.[transaction_event_type],S.[document_type],S.[revaluation_type],S.[document_date],S.[posting_date],S.[reference_code],S.[header_text],S.[bill_of_lading_code],S.[delivery_code],S.[transaction_code],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at])
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[material_document_year], INSERTED.[material_document_code]
INTO #hdr_delta (rn, oper_type, id, [material_document_year], [material_document_code]);
  SET @dml_n = @@ROWCOUNT;
  SELECT
    N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
    N'merge' AS [phase],
    @merge_from_rn AS [from_rn],
    CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END AS [to_rn],
    @merge_max_rn AS [max_rn],
    @dml_n AS [batch_rows],
    N'main' AS [scope];
  SET @progress_msg = CONCAT(
    N'QUARTZ_SYNC_PROGRESS|',
    N'merge', N'|',
    CAST((@merge_from_rn) AS NVARCHAR(20)), N'|',
    CAST((CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END) AS NVARCHAR(20)), N'|',
    CAST((@merge_max_rn) AS NVARCHAR(20)), N'|',
    CAST((@dml_n) AS NVARCHAR(20)), N'|',
    N'main');
  RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
  SET @merge_from_rn = @merge_to_rn + 1;
END

UPDATE S SET S.[id]=T.[id]
FROM #hdr S
INNER JOIN [takt_logistics_materials_material_document] T
  ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[material_document_year]))=S.[material_document_year]
 AND LTRIM(RTRIM(T.[material_document_code]))=S.[material_document_code];

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
OUTPUT INSERTED.[id], INSERTED.[material_document_year], INSERTED.[material_document_code]
INTO #hdr_soft ([id], [material_document_year], [material_document_code])
FROM [takt_logistics_materials_material_document] T
WHERE T.[is_deleted]=0
  AND EXISTS (
    SELECT 1 FROM #hdr S0
    WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code]
  )
  AND NOT EXISTS (SELECT 1 FROM #hdr S WHERE S.[id]=T.[id]);
DECLARE @hdr_delete INT = @@ROWCOUNT;

-- 子表开始：不更新源库。按凭证码装入 #item，再把 material_document_id 对齐目标主表
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'loadstart' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  CAST(0 AS INT) AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  CAST(0 AS INT) AS [batch_rows],
  N'detail' AS [scope];
SET @progress_msg = N'QUARTZ_SYNC_PROGRESS|loadstart|0|0|0|0|detail';
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;

INSERT INTO #item
SELECT S.rn, @base_id+1000000000+S.rn, 0,
  S.[material_document_year], S.[material_document_code], S.[line_number], S.[plant_code],
  S.[line_id], S.[parent_line_id], S.[line_depth],
  S.[movement_type], S.[auto_created_flag],
  S.[material_code], S.[warehouse_code], S.[batch_code],
  S.[stock_type], S.[restricted_stock_flag], S.[special_stock],
  S.[supplier_code], S.[customer_code],
  S.[debit_credit_indicator], S.[currency_code],
  S.[local_currency_amount], S.[alternative_amount],
  S.[quantity], S.[base_unit], S.[entry_quantity], S.[entry_unit],
  S.[po_price_quantity], S.[po_price_unit],
  S.[purchase_order_code], S.[purchase_order_item],
  S.[reference_document_year], S.[reference_document_code], S.[reference_document_item],
  S.[original_material_document_year], S.[original_material_document_code], S.[original_line_number],
  S.[delivery_completed_flag], S.[item_text],
  S.[equipment_code], S.[goods_recipient], S.[unloading_point],
  S.[business_area_code], S.[controlling_area_code], S.[trading_partner_business_area],
  S.[production_order_code], S.[asset_code], S.[asset_sub_code],
  S.[fiscal_year], S.[post_to_previous_period_flag], S.[post_to_previous_year_flag],
  S.[accounting_document_code], S.[accounting_document_item],
  S.[revaluation_document_code], S.[revaluation_document_item],
  S.[reservation_code], S.[reservation_item],
  S.[final_issue_flag], S.[reservation_quantity],
  S.[receiving_material_code], S.[receiving_plant_code], S.[receiving_warehouse_code],
  S.[profit_center_code],
  S.[valuated_stock_quantity], S.[total_valuated_stock_value],
  S.[price_control], S.[manufacturer_part_material_code],
  S.[mkpf_reference_code], S.[im_delivery_code], S.[im_delivery_item],
  S.[posted_by], S.[is_obsolete],
  S.[tenant_code], S.[company_code], S.[culture_code], S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT N.*, ROW_NUMBER() OVER (ORDER BY N.[material_document_year], N.[material_document_code], N.[line_number]) AS rn
  FROM (
    SELECT
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(SH.[material_document_year])), 4), N''), N'') AS [material_document_year],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_document_code])), 10), N''), N'') AS [material_document_code],
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0) AS [line_number],
      LTRIM(RTRIM(R.[plant_code])) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[line_id])), 6), N'') AS [line_id],
      NULLIF(LEFT(LTRIM(RTRIM(R.[parent_line_id])), 6), N'') AS [parent_line_id],
      NULLIF(LEFT(LTRIM(RTRIM(R.[line_depth])), 2), N'') AS [line_depth],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[movement_type])), 3), N''), N'') AS [movement_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[auto_created_flag])), 1), N'') AS [auto_created_flag],
      CASE
        WHEN LEN(LTRIM(RTRIM(ISNULL(R.[material_code], N''))))=18 AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_code])), 20), N''), N'')
      END AS [material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[warehouse_code])), 4), N'') AS [warehouse_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[batch_code])), 10), N'') AS [batch_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[stock_type])), 1), N'') AS [stock_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[restricted_stock_flag])), 1), N'') AS [restricted_stock_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[special_stock])), 1), N'') AS [special_stock],
      NULLIF(LEFT(LTRIM(RTRIM(R.[supplier_code])), 10), N'') AS [supplier_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[customer_code])), 10), N'') AS [customer_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[debit_credit_indicator])), 1), N'') AS [debit_credit_indicator],
      NULLIF(LEFT(LTRIM(RTRIM(R.[currency_code])), 3), N'') AS [currency_code],
      ISNULL(ROUND(TRY_CAST(R.[local_currency_amount] AS DECIMAL(13,2)), 2), 0) AS [local_currency_amount],
      ROUND(TRY_CAST(R.[alternative_amount] AS DECIMAL(13,2)), 2) AS [alternative_amount],
      ISNULL(ROUND(TRY_CAST(R.[quantity] AS DECIMAL(13,3)), 3), 0) AS [quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[base_unit])), 3), N'') AS [base_unit],
      ROUND(TRY_CAST(R.[entry_quantity] AS DECIMAL(13,3)), 3) AS [entry_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[entry_unit])), 3), N'') AS [entry_unit],
      ROUND(TRY_CAST(R.[po_price_quantity] AS DECIMAL(13,3)), 3) AS [po_price_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[po_price_unit])), 3), N'') AS [po_price_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_order_code])), 20), N'') AS [purchase_order_code],
      TRY_CAST(R.[purchase_order_item] AS INT) AS [purchase_order_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_document_year])), 4), N'') AS [reference_document_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reference_document_code])), 10), N'') AS [reference_document_code],
      TRY_CAST(R.[reference_document_item] AS INT) AS [reference_document_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[original_material_document_year])), 4), N'') AS [original_material_document_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[original_material_document_code])), 10), N'') AS [original_material_document_code],
      TRY_CAST(R.[original_line_number] AS INT) AS [original_line_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[delivery_completed_flag])), 1), N'') AS [delivery_completed_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[item_text])), 50), N'') AS [item_text],
      NULLIF(LEFT(LTRIM(RTRIM(R.[equipment_code])), 18), N'') AS [equipment_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[goods_recipient])), 12), N'') AS [goods_recipient],
      NULLIF(LEFT(LTRIM(RTRIM(R.[unloading_point])), 25), N'') AS [unloading_point],
      NULLIF(LEFT(LTRIM(RTRIM(R.[business_area_code])), 4), N'') AS [business_area_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[controlling_area_code])), 4), N'') AS [controlling_area_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[trading_partner_business_area])), 4), N'') AS [trading_partner_business_area],
      NULLIF(LEFT(LTRIM(RTRIM(R.[production_order_code])), 12), N'') AS [production_order_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[asset_code])), 12), N'') AS [asset_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[asset_sub_code])), 4), N'') AS [asset_sub_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fiscal_year])), 4), N'') AS [fiscal_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[post_to_previous_period_flag])), 1), N'') AS [post_to_previous_period_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[post_to_previous_year_flag])), 1), N'') AS [post_to_previous_year_flag],
      NULLIF(LEFT(LTRIM(RTRIM(R.[accounting_document_code])), 10), N'') AS [accounting_document_code],
      TRY_CAST(R.[accounting_document_item] AS INT) AS [accounting_document_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[revaluation_document_code])), 10), N'') AS [revaluation_document_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[revaluation_document_item])), 3), N'') AS [revaluation_document_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[reservation_code])), 10), N'') AS [reservation_code],
      TRY_CAST(R.[reservation_item] AS INT) AS [reservation_item],
      NULLIF(LEFT(LTRIM(RTRIM(R.[final_issue_flag])), 1), N'') AS [final_issue_flag],
      ROUND(TRY_CAST(R.[reservation_quantity] AS DECIMAL(13,3)), 3) AS [reservation_quantity],
      CASE
        WHEN LEN(LTRIM(RTRIM(ISNULL(R.[receiving_material_code], N''))))=18 AND LTRIM(RTRIM(R.[receiving_material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[receiving_material_code])), 10)
        ELSE NULLIF(LEFT(LTRIM(RTRIM(R.[receiving_material_code])), 20), N'')
      END AS [receiving_material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[receiving_plant_code])), 4), N'') AS [receiving_plant_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[receiving_warehouse_code])), 4), N'') AS [receiving_warehouse_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[profit_center_code])), 10), N'') AS [profit_center_code],
      ROUND(TRY_CAST(R.[valuated_stock_quantity] AS DECIMAL(13,3)), 3) AS [valuated_stock_quantity],
      ROUND(TRY_CAST(R.[total_valuated_stock_value] AS DECIMAL(13,2)), 2) AS [total_valuated_stock_value],
      NULLIF(LEFT(LTRIM(RTRIM(R.[price_control])), 1), N'') AS [price_control],
      NULLIF(LEFT(LTRIM(RTRIM(R.[manufacturer_part_material_code])), 40), N'') AS [manufacturer_part_material_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[mkpf_reference_code])), 32), N'') AS [mkpf_reference_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[im_delivery_code])), 20), N'') AS [im_delivery_code],
      TRY_CAST(R.[im_delivery_item] AS INT) AS [im_delivery_item],
      -- 源明细暂存表无 posted_by（过账人仅主表）；恒 NULL
      CAST(NULL AS NVARCHAR(6)) AS [posted_by],
      ISNULL(TRY_CAST(R.[is_obsolete] AS INT), 0) AS [is_obsolete],
      ISNULL(R.[ext_field], N'{}') AS [ext_field],
      ISNULL(R.[remark], N'') AS [remark],
      COALESCE(TRY_CAST(R.[created_by] AS BIGINT), 0) AS [created_by],
      R.[created_at] AS [created_at],
      TRY_CAST(R.[updated_by] AS BIGINT) AS [updated_by],
      R.[updated_at] AS [updated_at],
      TRY_CAST(R.[deleted_by] AS BIGINT) AS [deleted_by],
      R.[deleted_at] AS [deleted_at],
      CASE WHEN ISNULL(R.[is_deleted], 0)=0 THEN 0 ELSE 1 END AS [is_deleted],
            ROW_NUMBER() OVER (
        PARTITION BY LTRIM(RTRIM(SH.[material_document_year])), LTRIM(RTRIM(R.[material_document_code])), COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
        ORDER BY COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document] SH
      ON LTRIM(RTRIM(SH.[material_document_code])) = LTRIM(RTRIM(R.[material_document_code]))
    WHERE LTRIM(RTRIM(ISNULL(SH.[material_document_year], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[material_document_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N'')))<>N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT), 0)>0
  ) N
  WHERE N.dup_rn=1
) S
WHERE @batch_size=0 OR S.rn<=@batch_size;

UPDATE I SET I.[material_document_id]=H.[id], I.[id]=COALESCE(T.[id], I.[id])
FROM #item I
INNER JOIN #hdr H ON H.[material_document_year]=I.[material_document_year] AND H.[material_document_code]=I.[material_document_code]
LEFT JOIN [takt_logistics_materials_material_document_item] T
  ON T.[tenant_code]=I.[tenant_code] AND T.[company_code]=I.[company_code]
 AND T.[material_document_id]=H.[id] AND T.[line_number]=I.[line_number];

DECLARE @item_source INT = (SELECT COUNT(*) FROM #item WHERE [material_document_id]<>0);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @item_source AS [to_rn],
  @item_source AS [max_rn],
  @item_source AS [batch_rows],
  N'detail' AS [scope];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@item_source) AS NVARCHAR(20)), N'|',
  CAST((@item_source) AS NVARCHAR(20)), N'|',
  CAST((@item_source) AS NVARCHAR(20)), N'|',
  N'detail');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @item_sap_keys INT = (
  SELECT COUNT(*) FROM (
    SELECT LTRIM(RTRIM(SH.[material_document_year])) AS [material_document_year], LTRIM(RTRIM(R.[material_document_code])) AS [material_document_code], COALESCE(TRY_CAST(R.[line_number] AS INT),0) AS [line_number]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document] SH
      ON LTRIM(RTRIM(SH.[material_document_code])) = LTRIM(RTRIM(R.[material_document_code]))
    WHERE LTRIM(RTRIM(ISNULL(SH.[material_document_year], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[material_document_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N'')))<>N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT),0)>0
    GROUP BY LTRIM(RTRIM(SH.[material_document_year])), LTRIM(RTRIM(R.[material_document_code])), COALESCE(TRY_CAST(R.[line_number] AS INT),0)
  ) K
);
IF @item_source <> @item_sap_keys
BEGIN
  DECLARE @item_src_msg NVARCHAR(200) = CONCAT(N'明细业务键装入不一致: keys=', @item_sap_keys, N', loaded=', @item_source);
  THROW 50003, @item_src_msg, 1;
END;
DELETE FROM #item WHERE [material_document_id]=0;

DECLARE @item_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_materials_material_document_item] T
  WHERE T.[is_deleted]=0
    AND EXISTS (
      SELECT 1 FROM #item S
      WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code]
    )
);

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #item), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE [takt_logistics_materials_material_document_item] AS T
USING (SELECT * FROM #item WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND T.[material_document_id]=S.[material_document_id] AND T.[line_number]=S.[line_number]
WHEN MATCHED AND (
  ISNULL(T.[plant_code],N'')<>ISNULL(S.[plant_code],N'')
  OR ISNULL(T.[material_document_code],N'')<>ISNULL(S.[material_document_code],N'')
  OR ISNULL(T.[line_id],N'')<>ISNULL(S.[line_id],N'')
  OR ISNULL(T.[parent_line_id],N'')<>ISNULL(S.[parent_line_id],N'')
  OR ISNULL(T.[line_depth],N'')<>ISNULL(S.[line_depth],N'')
  OR ISNULL(T.[movement_type],N'')<>ISNULL(S.[movement_type],N'')
  OR ISNULL(T.[auto_created_flag],N'')<>ISNULL(S.[auto_created_flag],N'')
  OR ISNULL(T.[material_code],N'')<>ISNULL(S.[material_code],N'')
  OR ISNULL(T.[warehouse_code],N'')<>ISNULL(S.[warehouse_code],N'')
  OR ISNULL(T.[batch_code],N'')<>ISNULL(S.[batch_code],N'')
  OR ISNULL(T.[stock_type],N'')<>ISNULL(S.[stock_type],N'')
  OR ISNULL(T.[restricted_stock_flag],N'')<>ISNULL(S.[restricted_stock_flag],N'')
  OR ISNULL(T.[special_stock],N'')<>ISNULL(S.[special_stock],N'')
  OR ISNULL(T.[supplier_code],N'')<>ISNULL(S.[supplier_code],N'')
  OR ISNULL(T.[customer_code],N'')<>ISNULL(S.[customer_code],N'')
  OR ISNULL(T.[debit_credit_indicator],N'')<>ISNULL(S.[debit_credit_indicator],N'')
  OR ISNULL(T.[currency_code],N'')<>ISNULL(S.[currency_code],N'')
  OR ISNULL(T.[local_currency_amount],-1)<>ISNULL(S.[local_currency_amount],-1)
  OR ISNULL(T.[alternative_amount],-1)<>ISNULL(S.[alternative_amount],-1)
  OR ISNULL(T.[quantity],-1)<>ISNULL(S.[quantity],-1)
  OR ISNULL(T.[base_unit],N'')<>ISNULL(S.[base_unit],N'')
  OR ISNULL(T.[entry_quantity],-1)<>ISNULL(S.[entry_quantity],-1)
  OR ISNULL(T.[entry_unit],N'')<>ISNULL(S.[entry_unit],N'')
  OR ISNULL(T.[po_price_quantity],-1)<>ISNULL(S.[po_price_quantity],-1)
  OR ISNULL(T.[po_price_unit],N'')<>ISNULL(S.[po_price_unit],N'')
  OR ISNULL(T.[purchase_order_code],N'')<>ISNULL(S.[purchase_order_code],N'')
  OR ISNULL(T.[purchase_order_item],-1)<>ISNULL(S.[purchase_order_item],-1)
  OR ISNULL(T.[reference_document_year],N'')<>ISNULL(S.[reference_document_year],N'')
  OR ISNULL(T.[reference_document_code],N'')<>ISNULL(S.[reference_document_code],N'')
  OR ISNULL(T.[reference_document_item],-1)<>ISNULL(S.[reference_document_item],-1)
  OR ISNULL(T.[original_material_document_year],N'')<>ISNULL(S.[original_material_document_year],N'')
  OR ISNULL(T.[original_material_document_code],N'')<>ISNULL(S.[original_material_document_code],N'')
  OR ISNULL(T.[original_line_number],-1)<>ISNULL(S.[original_line_number],-1)
  OR ISNULL(T.[delivery_completed_flag],N'')<>ISNULL(S.[delivery_completed_flag],N'')
  OR ISNULL(T.[item_text],N'')<>ISNULL(S.[item_text],N'')
  OR ISNULL(T.[equipment_code],N'')<>ISNULL(S.[equipment_code],N'')
  OR ISNULL(T.[goods_recipient],N'')<>ISNULL(S.[goods_recipient],N'')
  OR ISNULL(T.[unloading_point],N'')<>ISNULL(S.[unloading_point],N'')
  OR ISNULL(T.[business_area_code],N'')<>ISNULL(S.[business_area_code],N'')
  OR ISNULL(T.[controlling_area_code],N'')<>ISNULL(S.[controlling_area_code],N'')
  OR ISNULL(T.[trading_partner_business_area],N'')<>ISNULL(S.[trading_partner_business_area],N'')
  OR ISNULL(T.[production_order_code],N'')<>ISNULL(S.[production_order_code],N'')
  OR ISNULL(T.[asset_code],N'')<>ISNULL(S.[asset_code],N'')
  OR ISNULL(T.[asset_sub_code],N'')<>ISNULL(S.[asset_sub_code],N'')
  OR ISNULL(T.[fiscal_year],N'')<>ISNULL(S.[fiscal_year],N'')
  OR ISNULL(T.[post_to_previous_period_flag],N'')<>ISNULL(S.[post_to_previous_period_flag],N'')
  OR ISNULL(T.[post_to_previous_year_flag],N'')<>ISNULL(S.[post_to_previous_year_flag],N'')
  OR ISNULL(T.[accounting_document_code],N'')<>ISNULL(S.[accounting_document_code],N'')
  OR ISNULL(T.[accounting_document_item],-1)<>ISNULL(S.[accounting_document_item],-1)
  OR ISNULL(T.[revaluation_document_code],N'')<>ISNULL(S.[revaluation_document_code],N'')
  OR ISNULL(T.[revaluation_document_item],N'')<>ISNULL(S.[revaluation_document_item],N'')
  OR ISNULL(T.[reservation_code],N'')<>ISNULL(S.[reservation_code],N'')
  OR ISNULL(T.[reservation_item],-1)<>ISNULL(S.[reservation_item],-1)
  OR ISNULL(T.[final_issue_flag],N'')<>ISNULL(S.[final_issue_flag],N'')
  OR ISNULL(T.[reservation_quantity],-1)<>ISNULL(S.[reservation_quantity],-1)
  OR ISNULL(T.[receiving_material_code],N'')<>ISNULL(S.[receiving_material_code],N'')
  OR ISNULL(T.[receiving_plant_code],N'')<>ISNULL(S.[receiving_plant_code],N'')
  OR ISNULL(T.[receiving_warehouse_code],N'')<>ISNULL(S.[receiving_warehouse_code],N'')
  OR ISNULL(T.[profit_center_code],N'')<>ISNULL(S.[profit_center_code],N'')
  OR ISNULL(T.[valuated_stock_quantity],-1)<>ISNULL(S.[valuated_stock_quantity],-1)
  OR ISNULL(T.[total_valuated_stock_value],-1)<>ISNULL(S.[total_valuated_stock_value],-1)
  OR ISNULL(T.[price_control],N'')<>ISNULL(S.[price_control],N'')
  OR ISNULL(T.[manufacturer_part_material_code],N'')<>ISNULL(S.[manufacturer_part_material_code],N'')
  OR ISNULL(T.[mkpf_reference_code],N'')<>ISNULL(S.[mkpf_reference_code],N'')
  OR ISNULL(T.[im_delivery_code],N'')<>ISNULL(S.[im_delivery_code],N'')
  OR ISNULL(T.[im_delivery_item],-1)<>ISNULL(S.[im_delivery_item],-1)
  OR ISNULL(T.[is_obsolete],-1)<>ISNULL(S.[is_obsolete],-1)
  OR ISNULL(T.[culture_code],N'')<>ISNULL(S.[culture_code],N'')
  OR T.[is_deleted]<>S.[is_deleted]

  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN UPDATE SET
  T.[plant_code]=S.[plant_code],
  T.[material_document_code]=S.[material_document_code],
  T.[line_id]=S.[line_id],
  T.[parent_line_id]=S.[parent_line_id],
  T.[line_depth]=S.[line_depth],
  T.[movement_type]=S.[movement_type],
  T.[auto_created_flag]=S.[auto_created_flag],
  T.[material_code]=S.[material_code],
  T.[warehouse_code]=S.[warehouse_code],
  T.[batch_code]=S.[batch_code],
  T.[stock_type]=S.[stock_type],
  T.[restricted_stock_flag]=S.[restricted_stock_flag],
  T.[special_stock]=S.[special_stock],
  T.[supplier_code]=S.[supplier_code],
  T.[customer_code]=S.[customer_code],
  T.[debit_credit_indicator]=S.[debit_credit_indicator],
  T.[currency_code]=S.[currency_code],
  T.[local_currency_amount]=S.[local_currency_amount],
  T.[alternative_amount]=S.[alternative_amount],
  T.[quantity]=S.[quantity],
  T.[base_unit]=S.[base_unit],
  T.[entry_quantity]=S.[entry_quantity],
  T.[entry_unit]=S.[entry_unit],
  T.[po_price_quantity]=S.[po_price_quantity],
  T.[po_price_unit]=S.[po_price_unit],
  T.[purchase_order_code]=S.[purchase_order_code],
  T.[purchase_order_item]=S.[purchase_order_item],
  T.[reference_document_year]=S.[reference_document_year],
  T.[reference_document_code]=S.[reference_document_code],
  T.[reference_document_item]=S.[reference_document_item],
  T.[original_material_document_year]=S.[original_material_document_year],
  T.[original_material_document_code]=S.[original_material_document_code],
  T.[original_line_number]=S.[original_line_number],
  T.[delivery_completed_flag]=S.[delivery_completed_flag],
  T.[item_text]=S.[item_text],
  T.[equipment_code]=S.[equipment_code],
  T.[goods_recipient]=S.[goods_recipient],
  T.[unloading_point]=S.[unloading_point],
  T.[business_area_code]=S.[business_area_code],
  T.[controlling_area_code]=S.[controlling_area_code],
  T.[trading_partner_business_area]=S.[trading_partner_business_area],
  T.[production_order_code]=S.[production_order_code],
  T.[asset_code]=S.[asset_code],
  T.[asset_sub_code]=S.[asset_sub_code],
  T.[fiscal_year]=S.[fiscal_year],
  T.[post_to_previous_period_flag]=S.[post_to_previous_period_flag],
  T.[post_to_previous_year_flag]=S.[post_to_previous_year_flag],
  T.[accounting_document_code]=S.[accounting_document_code],
  T.[accounting_document_item]=S.[accounting_document_item],
  T.[revaluation_document_code]=S.[revaluation_document_code],
  T.[revaluation_document_item]=S.[revaluation_document_item],
  T.[reservation_code]=S.[reservation_code],
  T.[reservation_item]=S.[reservation_item],
  T.[final_issue_flag]=S.[final_issue_flag],
  T.[reservation_quantity]=S.[reservation_quantity],
  T.[receiving_material_code]=S.[receiving_material_code],
  T.[receiving_plant_code]=S.[receiving_plant_code],
  T.[receiving_warehouse_code]=S.[receiving_warehouse_code],
  T.[profit_center_code]=S.[profit_center_code],
  T.[valuated_stock_quantity]=S.[valuated_stock_quantity],
  T.[total_valuated_stock_value]=S.[total_valuated_stock_value],
  T.[price_control]=S.[price_control],
  T.[manufacturer_part_material_code]=S.[manufacturer_part_material_code],
  T.[mkpf_reference_code]=S.[mkpf_reference_code],
  T.[im_delivery_code]=S.[im_delivery_code],
  T.[im_delivery_item]=S.[im_delivery_item],
  T.[is_obsolete]=S.[is_obsolete],
  T.[culture_code]=S.[culture_code],
  T.[ext_field]=S.[ext_field],
  T.[remark]=S.[remark],
  T.[created_by]=S.[created_by],
  T.[created_at]=S.[created_at],
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=S.[updated_at],
  T.[is_deleted]=S.[is_deleted],
  T.[deleted_by]=S.[deleted_by],
  T.[deleted_at]=S.[deleted_at]
WHEN NOT MATCHED THEN
  INSERT ([id],[material_document_id],[plant_code],[material_document_code],[line_number],[line_id],[parent_line_id],[line_depth],[movement_type],[auto_created_flag],[material_code],[warehouse_code],[batch_code],[stock_type],[restricted_stock_flag],[special_stock],[supplier_code],[customer_code],[debit_credit_indicator],[currency_code],[local_currency_amount],[alternative_amount],[quantity],[base_unit],[entry_quantity],[entry_unit],[po_price_quantity],[po_price_unit],[purchase_order_code],[purchase_order_item],[reference_document_year],[reference_document_code],[reference_document_item],[original_material_document_year],[original_material_document_code],[original_line_number],[delivery_completed_flag],[item_text],[equipment_code],[goods_recipient],[unloading_point],[business_area_code],[controlling_area_code],[trading_partner_business_area],[production_order_code],[asset_code],[asset_sub_code],[fiscal_year],[post_to_previous_period_flag],[post_to_previous_year_flag],[accounting_document_code],[accounting_document_item],[revaluation_document_code],[revaluation_document_item],[reservation_code],[reservation_item],[final_issue_flag],[reservation_quantity],[receiving_material_code],[receiving_plant_code],[receiving_warehouse_code],[profit_center_code],[valuated_stock_quantity],[total_valuated_stock_value],[price_control],[manufacturer_part_material_code],[mkpf_reference_code],[im_delivery_code],[im_delivery_item],[is_obsolete],[tenant_code],[company_code],[culture_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[material_document_id],S.[plant_code],S.[material_document_code],S.[line_number],S.[line_id],S.[parent_line_id],S.[line_depth],S.[movement_type],S.[auto_created_flag],S.[material_code],S.[warehouse_code],S.[batch_code],S.[stock_type],S.[restricted_stock_flag],S.[special_stock],S.[supplier_code],S.[customer_code],S.[debit_credit_indicator],S.[currency_code],S.[local_currency_amount],S.[alternative_amount],S.[quantity],S.[base_unit],S.[entry_quantity],S.[entry_unit],S.[po_price_quantity],S.[po_price_unit],S.[purchase_order_code],S.[purchase_order_item],S.[reference_document_year],S.[reference_document_code],S.[reference_document_item],S.[original_material_document_year],S.[original_material_document_code],S.[original_line_number],S.[delivery_completed_flag],S.[item_text],S.[equipment_code],S.[goods_recipient],S.[unloading_point],S.[business_area_code],S.[controlling_area_code],S.[trading_partner_business_area],S.[production_order_code],S.[asset_code],S.[asset_sub_code],S.[fiscal_year],S.[post_to_previous_period_flag],S.[post_to_previous_year_flag],S.[accounting_document_code],S.[accounting_document_item],S.[revaluation_document_code],S.[revaluation_document_item],S.[reservation_code],S.[reservation_item],S.[final_issue_flag],S.[reservation_quantity],S.[receiving_material_code],S.[receiving_plant_code],S.[receiving_warehouse_code],S.[profit_center_code],S.[valuated_stock_quantity],S.[total_valuated_stock_value],S.[price_control],S.[manufacturer_part_material_code],S.[mkpf_reference_code],S.[im_delivery_code],S.[im_delivery_item],S.[is_obsolete],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at])
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[material_document_code], INSERTED.[line_number]
INTO #item_delta (rn, oper_type, id, [material_document_code], [line_number]);
  SET @dml_n = @@ROWCOUNT;
  SELECT
    N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
    N'merge' AS [phase],
    @merge_from_rn AS [from_rn],
    CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END AS [to_rn],
    @merge_max_rn AS [max_rn],
    @dml_n AS [batch_rows],
    N'detail' AS [scope];
  SET @progress_msg = CONCAT(
    N'QUARTZ_SYNC_PROGRESS|',
    N'merge', N'|',
    CAST((@merge_from_rn) AS NVARCHAR(20)), N'|',
    CAST((CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END) AS NVARCHAR(20)), N'|',
    CAST((@merge_max_rn) AS NVARCHAR(20)), N'|',
    CAST((@dml_n) AS NVARCHAR(20)), N'|',
    N'detail');
  RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
  SET @merge_from_rn = @merge_to_rn + 1;
END

UPDATE I SET I.[id]=T.[id]
FROM #item I
INNER JOIN [takt_logistics_materials_material_document_item] T
  ON T.[tenant_code]=I.[tenant_code] AND T.[company_code]=I.[company_code]
 AND T.[material_document_id]=I.[material_document_id] AND T.[line_number]=I.[line_number];

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
OUTPUT INSERTED.[id], INSERTED.[material_document_code], INSERTED.[line_number]
INTO #item_soft ([id], [material_document_code], [line_number])
FROM [takt_logistics_materials_material_document_item] T
WHERE T.[is_deleted]=0
  AND EXISTS (
    SELECT 1 FROM #item S0
    WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code]
  )
  AND NOT EXISTS (SELECT 1 FROM #item S WHERE S.[id]=T.[id]);
DECLARE @item_delete INT = @@ROWCOUNT;

DECLARE @hdr_source_active INT = (SELECT COUNT(*) FROM #hdr WHERE [is_deleted]=0);
DECLARE @item_source_active INT = (SELECT COUNT(*) FROM #item WHERE [is_deleted]=0);
DECLARE @hdr_after INT = (
  SELECT COUNT(*)
  FROM #hdr S
  INNER JOIN [takt_logistics_materials_material_document] T
    ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
   AND LTRIM(RTRIM(T.[material_document_year]))=S.[material_document_year]
   AND LTRIM(RTRIM(T.[material_document_code]))=S.[material_document_code]
   AND T.[is_deleted]=0
  WHERE S.[is_deleted]=0
);
DECLARE @item_after INT = (
  SELECT COUNT(*)
  FROM #item S
  INNER JOIN [takt_logistics_materials_material_document_item] T
    ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
   AND T.[material_document_id]=S.[material_document_id] AND T.[line_number]=S.[line_number]
   AND T.[is_deleted]=0
  WHERE S.[is_deleted]=0
);
IF @hdr_after <> @hdr_source_active BEGIN DECLARE @hdr_cnt NVARCHAR(200)=CONCAT(N'主表有效行数不一致: source=',@hdr_source_active,N', active=',@hdr_after); THROW 50002,@hdr_cnt,1; END;
IF @item_after <> @item_source_active BEGIN DECLARE @item_cnt NVARCHAR(200)=CONCAT(N'明细有效行数不一致: source=',@item_source_active,N', active=',@item_after); THROW 50002,@item_cnt,1; END;

DECLARE @hdr_ins INT=(SELECT COUNT(*) FROM #hdr_delta WHERE oper_type=N'INSERT');
DECLARE @hdr_upd INT=(SELECT COUNT(*) FROM #hdr_delta WHERE oper_type=N'UPDATE');
DECLARE @hdr_unchanged INT=@hdr_source-@hdr_ins-@hdr_upd;
DECLARE @item_ins INT=(SELECT COUNT(*) FROM #item_delta WHERE oper_type=N'INSERT');
DECLARE @item_upd INT=(SELECT COUNT(*) FROM #item_delta WHERE oper_type=N'UPDATE');
DECLARE @item_unchanged INT=@item_source-@item_ins-@item_upd;

DECLARE @hdr_physical INT=(
  SELECT COUNT(*) FROM [takt_logistics_materials_material_document] T
  WHERE EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @item_physical INT=(
  SELECT COUNT(*) FROM [takt_logistics_materials_material_document_item] T
  WHERE EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @hdr_soft_total INT=(
  SELECT COUNT(*) FROM [takt_logistics_materials_material_document] T
  WHERE T.[is_deleted]=1
    AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @item_soft_total INT=(
  SELECT COUNT(*) FROM [takt_logistics_materials_material_document_item] T
  WHERE T.[is_deleted]=1
    AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @hdr_sap_raw INT=(SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document]);
DECLARE @item_sap_raw INT=(SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_document_item]);

DECLARE @hdr_soft_keys NVARCHAR(MAX)=N'';
SELECT @hdr_soft_keys=STRING_AGG(CAST(CONCAT(CAST([id] AS NVARCHAR(30)),N'|',ISNULL([material_document_year],N''),N'/',ISNULL([material_document_code],N'')) AS NVARCHAR(MAX)), N'; ')
FROM (SELECT TOP (100) * FROM #hdr_soft ORDER BY [id]) SoftSample;
SET @hdr_soft_keys=ISNULL(@hdr_soft_keys,N'');
IF @hdr_delete>100 SET @hdr_soft_keys=CONCAT(@hdr_soft_keys,N'; ...(+',CAST(@hdr_delete-100 AS NVARCHAR(20)),N')');

DECLARE @item_soft_keys NVARCHAR(MAX)=N'';
SELECT @item_soft_keys=STRING_AGG(CAST(CONCAT(CAST([id] AS NVARCHAR(30)),N'|',ISNULL([material_document_code],N''),N'/',CAST([line_number] AS NVARCHAR(20))) AS NVARCHAR(MAX)), N'; ')
FROM (SELECT TOP (100) * FROM #item_soft ORDER BY [id]) SoftSample;
SET @item_soft_keys=ISNULL(@item_soft_keys,N'');
IF @item_delete>100 SET @item_soft_keys=CONCAT(@item_soft_keys,N'; ...(+',CAST(@item_delete-100 AS NVARCHAR(20)),N')');

SELECT N'QUARTZ_SYNC_SUMMARY' AS [summary_tag], CAST(N'main' AS NVARCHAR(40)) AS [scope],
  @hdr_sap_raw AS [source_raw_count], @hdr_source AS [source_count],
  @hdr_before AS [target_before], @hdr_after AS [target_after],
  @hdr_physical AS [target_physical], @hdr_soft_total AS [soft_deleted],
  @hdr_ins AS [insert_count], @hdr_upd AS [update_count], @hdr_unchanged AS [unchanged_count],
  @hdr_delete AS [delete_count], @hdr_soft_keys AS [soft_deleted_keys]
UNION ALL
SELECT N'QUARTZ_SYNC_SUMMARY', CAST(N'detail' AS NVARCHAR(40)),
  @item_sap_raw, @item_source,
  @item_before, @item_after,
  @item_physical, @item_soft_total,
  @item_ins, @item_upd, @item_unchanged,
  @item_delete, @item_soft_keys;
