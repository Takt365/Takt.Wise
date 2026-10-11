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

-- 源/目标列与实体 TaktPurchaseOrder / TaktPurchaseOrderItem 一致
-- {{SourceDatabase}}.dbo.takt_logistics_procurement_purchase_order[_item] → 当前租户库同名表
-- 主表唯一键：Tenant+Company+Plant+purchase_order_code+supplier_code+order_date(日)
-- 明细唯一键：purchase_order_id+line_number+material_code
-- 源明细装入：SH.id=R.purchase_order_id，并携带 SH.supplier_code+order_date
-- 目标 FK 回填：#hdr 按 plant+code+supplier+order_date(日) 对齐（与主表唯一键一致；禁止仅 plant+code）
-- tenant/company/plant/culture 取自各源表本列；任一层为空则丢弃，不回退任务参数

IF OBJECT_ID('tempdb..#hdr') IS NOT NULL DROP TABLE #hdr;
IF OBJECT_ID('tempdb..#item') IS NOT NULL DROP TABLE #item;
IF OBJECT_ID('tempdb..#hdr_delta') IS NOT NULL DROP TABLE #hdr_delta;
IF OBJECT_ID('tempdb..#item_delta') IS NOT NULL DROP TABLE #item_delta;
IF OBJECT_ID('tempdb..#hdr_soft') IS NOT NULL DROP TABLE #hdr_soft;
IF OBJECT_ID('tempdb..#item_soft') IS NOT NULL DROP TABLE #item_soft;

CREATE TABLE #hdr (
  [rn] INT, [id] BIGINT,
  [company_code] NVARCHAR(4), [plant_code] NVARCHAR(4), [tenant_code] NVARCHAR(3), [culture_code] NVARCHAR(5),
  [purchase_order_code] NVARCHAR(20),
  [purchase_request_id] BIGINT, [purchase_request_code] NVARCHAR(20),
  [supplier_code] NVARCHAR(10), [supplier_name1] NVARCHAR(140),
  [order_date] DATETIME, [required_arrival_date] DATETIME, [actual_arrival_date] DATETIME,
  [purchase_group] NVARCHAR(3),
  [total_quantity] DECIMAL(18,4), [total_amount] DECIMAL(18,2), [discount_amount] DECIMAL(18,2),
  [currency_code] NVARCHAR(3), [exchange_rate] DECIMAL(18,5),
  [tax_code] NVARCHAR(4), [tax_rate] INT, [tax_amount] DECIMAL(18,2), [actual_amount] DECIMAL(18,2),
  [received_quantity] DECIMAL(18,4), [received_amount] DECIMAL(18,2), [paid_amount] DECIMAL(18,2),
  [payment_method] INT, [delivery_method] INT, [delivery_address] NVARCHAR(500),
  [order_status] INT, [delivery_status] INT,
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

CREATE TABLE #item (
  [rn] INT, [id] BIGINT, [purchase_order_id] BIGINT,
  [company_code] NVARCHAR(4), [plant_code] NVARCHAR(4), [tenant_code] NVARCHAR(3), [culture_code] NVARCHAR(5),
  [purchase_order_code] NVARCHAR(20), [supplier_code] NVARCHAR(10), [order_date] DATETIME, [line_number] INT,
  [request_code] NVARCHAR(20), [request_line_number] INT,
  [material_code] NVARCHAR(20), [material_description] NVARCHAR(40), [material_specification] NVARCHAR(70),
  [purchase_unit] NVARCHAR(20),
  [order_quantity] DECIMAL(18,5), [received_quantity] DECIMAL(18,5),
  [purchase_per_unit] INT, [purchase_unit_price] DECIMAL(18,5),
  [discount_rate] DECIMAL(5,2), [discount_amount] DECIMAL(18,5),
  [tax_included_amount] DECIMAL(18,5), [untaxed_amount] DECIMAL(18,5), [tax_amount] DECIMAL(18,5),
  [purchase_amount] DECIMAL(18,5),
  [delivery_status] INT, [is_obsolete] INT,
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

CREATE TABLE #hdr_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  [plant_code] NVARCHAR(4), [purchase_order_code] NVARCHAR(20), [supplier_code] NVARCHAR(10)
);
CREATE TABLE #item_delta (
  rn INT, oper_type NVARCHAR(10), id BIGINT,
  [purchase_order_code] NVARCHAR(20), [line_number] INT
);
CREATE TABLE #hdr_soft (
  [id] BIGINT, [plant_code] NVARCHAR(4), [purchase_order_code] NVARCHAR(20), [supplier_code] NVARCHAR(10)
);
CREATE TABLE #item_soft (
  [id] BIGINT, [purchase_order_code] NVARCHAR(20), [line_number] INT
);

INSERT INTO #hdr
SELECT S.rn, @base_id + S.rn,
  S.[company_code], S.[plant_code], S.[tenant_code], S.[culture_code], S.[purchase_order_code],
  S.[purchase_request_id], S.[purchase_request_code],
  S.[supplier_code], S.[supplier_name1],
  S.[order_date], S.[required_arrival_date], S.[actual_arrival_date],
  S.[purchase_group],
  S.[total_quantity], S.[total_amount], S.[discount_amount],
  S.[currency_code], S.[exchange_rate],
  S.[tax_code], S.[tax_rate], S.[tax_amount], S.[actual_amount],
  S.[received_quantity], S.[received_amount], S.[paid_amount],
  S.[payment_method], S.[delivery_method], S.[delivery_address],
  S.[order_status], S.[delivery_status],
  S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT N.*, ROW_NUMBER() OVER (
    ORDER BY N.[company_code], N.[plant_code], N.[purchase_order_code], N.[supplier_code], N.[order_date]
  ) AS rn
  FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_order_code])), 20), N''), N'') AS [purchase_order_code],
      TRY_CAST(R.[purchase_request_id] AS BIGINT) AS [purchase_request_id],
      NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_request_code])), 20), N'') AS [purchase_request_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[supplier_code])), 10), N''), N'') AS [supplier_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[supplier_name1])), 140), N''), N'') AS [supplier_name1],
      ISNULL(TRY_CAST(R.[order_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS [order_date],
      TRY_CAST(R.[required_arrival_date] AS DATETIME) AS [required_arrival_date],
      TRY_CAST(R.[actual_arrival_date] AS DATETIME) AS [actual_arrival_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_group])), 3), N'') AS [purchase_group],
      ROUND(COALESCE(TRY_CAST(R.[total_quantity] AS DECIMAL(18,8)), 0), 4) AS [total_quantity],
      ROUND(COALESCE(TRY_CAST(R.[total_amount] AS DECIMAL(18,8)), 0), 2) AS [total_amount],
      ROUND(COALESCE(TRY_CAST(R.[discount_amount] AS DECIMAL(18,8)), 0), 2) AS [discount_amount],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[currency_code])), 3), N''), N'') AS [currency_code],
      ROUND(COALESCE(TRY_CAST(R.[exchange_rate] AS DECIMAL(18,8)), 1), 5) AS [exchange_rate],
      NULLIF(LEFT(LTRIM(RTRIM(R.[tax_code])), 4), N'') AS [tax_code],
      COALESCE(TRY_CAST(R.[tax_rate] AS INT), 0) AS [tax_rate],
      ROUND(COALESCE(TRY_CAST(R.[tax_amount] AS DECIMAL(18,8)), 0), 2) AS [tax_amount],
      ROUND(COALESCE(TRY_CAST(R.[actual_amount] AS DECIMAL(18,8)), 0), 2) AS [actual_amount],
      ROUND(COALESCE(TRY_CAST(R.[received_quantity] AS DECIMAL(18,8)), 0), 4) AS [received_quantity],
      ROUND(COALESCE(TRY_CAST(R.[received_amount] AS DECIMAL(18,8)), 0), 2) AS [received_amount],
      ROUND(COALESCE(TRY_CAST(R.[paid_amount] AS DECIMAL(18,8)), 0), 2) AS [paid_amount],
      COALESCE(TRY_CAST(R.[payment_method] AS INT), 0) AS [payment_method],
      COALESCE(TRY_CAST(R.[delivery_method] AS INT), 0) AS [delivery_method],
      NULLIF(LEFT(LTRIM(RTRIM(R.[delivery_address])), 500), N'') AS [delivery_address],
      COALESCE(TRY_CAST(R.[order_status] AS INT), 1) AS [order_status],
      COALESCE(TRY_CAST(R.[delivery_status] AS INT), 0) AS [delivery_status],
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
          LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4),
          LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4),
          LTRIM(RTRIM(R.[purchase_order_code])),
          LTRIM(RTRIM(R.[supplier_code])),
          CAST(ISNULL(TRY_CAST(R.[order_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE)
        ORDER BY R.[id]
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_order] R
    WHERE LTRIM(RTRIM(ISNULL(R.[purchase_order_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[supplier_code], N''))) <> N''
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
  @hdr_source AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@hdr_source) AS NVARCHAR(20)), N'|',
  CAST((@hdr_source) AS NVARCHAR(20)), N'|',
  CAST((@hdr_source) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @hdr_sap_keys INT = (
  SELECT COUNT(*) FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4) AS [plant_code],
      LTRIM(RTRIM(R.[purchase_order_code])) AS [purchase_order_code],
      LTRIM(RTRIM(R.[supplier_code])) AS [supplier_code],
      CAST(ISNULL(TRY_CAST(R.[order_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE) AS [order_day]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_order] R
    WHERE LTRIM(RTRIM(ISNULL(R.[purchase_order_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[supplier_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
    GROUP BY
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4),
      LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4),
      LTRIM(RTRIM(R.[purchase_order_code])),
      LTRIM(RTRIM(R.[supplier_code])),
      CAST(ISNULL(TRY_CAST(R.[order_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE)
  ) K
);
IF @hdr_source <> @hdr_sap_keys
BEGIN
  DECLARE @hdr_src_msg NVARCHAR(200) = CONCAT(N'主表业务键装入不一致: keys=', @hdr_sap_keys, N', loaded=', @hdr_source);
  THROW 50003, @hdr_src_msg, 1;
END;

DECLARE @hdr_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_order] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #hdr), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE [takt_logistics_procurement_purchase_order] AS T
USING (SELECT * FROM #hdr WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[plant_code]))=S.[plant_code]
 AND LTRIM(RTRIM(T.[purchase_order_code]))=S.[purchase_order_code]
 AND LTRIM(RTRIM(T.[supplier_code]))=S.[supplier_code]
 AND CAST(T.[order_date] AS DATE)=CAST(S.[order_date] AS DATE)
WHEN MATCHED AND (
  ISNULL(T.[purchase_request_id],0)<>ISNULL(S.[purchase_request_id],0)
  OR ISNULL(T.[purchase_request_code],N'')<>ISNULL(S.[purchase_request_code],N'')
  OR ISNULL(T.[supplier_name1],N'')<>S.[supplier_name1]
  OR ISNULL(T.[required_arrival_date],CAST('1900-01-01' AS DATETIME))<>ISNULL(S.[required_arrival_date],CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[actual_arrival_date],CAST('1900-01-01' AS DATETIME))<>ISNULL(S.[actual_arrival_date],CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[purchase_group],N'')<>ISNULL(S.[purchase_group],N'')
  OR T.[total_quantity]<>S.[total_quantity] OR T.[total_amount]<>S.[total_amount] OR T.[discount_amount]<>S.[discount_amount]
  OR T.[currency_code]<>S.[currency_code] OR T.[exchange_rate]<>S.[exchange_rate]
  OR ISNULL(T.[tax_code],N'')<>ISNULL(S.[tax_code],N'') OR T.[tax_rate]<>S.[tax_rate]
  OR T.[tax_amount]<>S.[tax_amount] OR T.[actual_amount]<>S.[actual_amount]
  OR T.[received_quantity]<>S.[received_quantity] OR T.[received_amount]<>S.[received_amount] OR T.[paid_amount]<>S.[paid_amount]
  OR T.[payment_method]<>S.[payment_method] OR T.[delivery_method]<>S.[delivery_method]
  OR ISNULL(T.[delivery_address],N'')<>ISNULL(S.[delivery_address],N'')
  OR T.[order_status]<>S.[order_status] OR T.[delivery_status]<>S.[delivery_status]
  OR ISNULL(T.[culture_code],N'')<>ISNULL(S.[culture_code],N'') OR T.[is_deleted]<>S.[is_deleted]

  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN UPDATE SET
  T.[purchase_request_id]=S.[purchase_request_id],
  T.[purchase_request_code]=S.[purchase_request_code],
  T.[supplier_name1]=S.[supplier_name1],
  T.[required_arrival_date]=S.[required_arrival_date],
  T.[actual_arrival_date]=S.[actual_arrival_date],
  T.[purchase_group]=S.[purchase_group],
  T.[total_quantity]=S.[total_quantity],
  T.[total_amount]=S.[total_amount],
  T.[discount_amount]=S.[discount_amount],
  T.[currency_code]=S.[currency_code],
  T.[exchange_rate]=S.[exchange_rate],
  T.[tax_code]=S.[tax_code],
  T.[tax_rate]=S.[tax_rate],
  T.[tax_amount]=S.[tax_amount],
  T.[actual_amount]=S.[actual_amount],
  T.[received_quantity]=S.[received_quantity],
  T.[received_amount]=S.[received_amount],
  T.[paid_amount]=S.[paid_amount],
  T.[payment_method]=S.[payment_method],
  T.[delivery_method]=S.[delivery_method],
  T.[delivery_address]=S.[delivery_address],
  T.[order_status]=S.[order_status],
  T.[delivery_status]=S.[delivery_status],
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
  INSERT ([id],[plant_code],[purchase_order_code],[purchase_request_id],[purchase_request_code],[supplier_code],[supplier_name1],[order_date],[required_arrival_date],[actual_arrival_date],[purchase_group],[total_quantity],[total_amount],[discount_amount],[currency_code],[exchange_rate],[tax_code],[tax_rate],[tax_amount],[actual_amount],[received_quantity],[received_amount],[paid_amount],[payment_method],[delivery_method],[delivery_address],[order_status],[delivery_status],[tenant_code],[company_code],[culture_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[plant_code],S.[purchase_order_code],S.[purchase_request_id],S.[purchase_request_code],S.[supplier_code],S.[supplier_name1],S.[order_date],S.[required_arrival_date],S.[actual_arrival_date],S.[purchase_group],S.[total_quantity],S.[total_amount],S.[discount_amount],S.[currency_code],S.[exchange_rate],S.[tax_code],S.[tax_rate],S.[tax_amount],S.[actual_amount],S.[received_quantity],S.[received_amount],S.[paid_amount],S.[payment_method],S.[delivery_method],S.[delivery_address],S.[order_status],S.[delivery_status],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at])
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[plant_code], INSERTED.[purchase_order_code], INSERTED.[supplier_code]
INTO #hdr_delta (rn, oper_type, id, [plant_code], [purchase_order_code], [supplier_code]);
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

UPDATE S SET S.[id]=T.[id]
FROM #hdr S
INNER JOIN [takt_logistics_procurement_purchase_order] T
  ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND LTRIM(RTRIM(T.[plant_code]))=S.[plant_code]
 AND LTRIM(RTRIM(T.[purchase_order_code]))=S.[purchase_order_code]
 AND LTRIM(RTRIM(T.[supplier_code]))=S.[supplier_code]
 AND CAST(T.[order_date] AS DATE)=CAST(S.[order_date] AS DATE);

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
OUTPUT INSERTED.[id], INSERTED.[plant_code], INSERTED.[purchase_order_code], INSERTED.[supplier_code]
INTO #hdr_soft ([id], [plant_code], [purchase_order_code], [supplier_code])
FROM [takt_logistics_procurement_purchase_order] T
WHERE T.[is_deleted]=0
  AND EXISTS (SELECT 1 FROM #hdr S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
  AND NOT EXISTS (SELECT 1 FROM #hdr S WHERE S.[id]=T.[id]);
DECLARE @hdr_delete INT = @@ROWCOUNT;

-- 源库回填：仅当 plant+code 在源主表唯一时才写 purchase_order_id（避免同码多供应商/多日期误绑）
UPDATE I
SET I.[purchase_order_id] = U.[id]
FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_order_item] AS I
INNER JOIN (
  SELECT
    LEFT(LTRIM(RTRIM(ISNULL(H.[company_code], N''))), 4) AS [company_code],
    LEFT(LTRIM(RTRIM(ISNULL(H.[plant_code], N''))), 4) AS [plant_code],
    LTRIM(RTRIM(H.[purchase_order_code])) AS [purchase_order_code],
    MIN(H.[id]) AS [id]
  FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_order] AS H
  WHERE LTRIM(RTRIM(ISNULL(H.[purchase_order_code], N''))) <> N''
    AND LTRIM(RTRIM(ISNULL(H.[plant_code], N''))) <> N''
  GROUP BY
    LEFT(LTRIM(RTRIM(ISNULL(H.[company_code], N''))), 4),
    LEFT(LTRIM(RTRIM(ISNULL(H.[plant_code], N''))), 4),
    LTRIM(RTRIM(H.[purchase_order_code]))
  HAVING COUNT(*) = 1
) AS U
  ON U.[company_code] = LEFT(LTRIM(RTRIM(ISNULL(I.[company_code], N''))), 4)
 AND U.[plant_code] = LEFT(LTRIM(RTRIM(ISNULL(I.[plant_code], N''))), 4)
 AND U.[purchase_order_code] = LTRIM(RTRIM(I.[purchase_order_code]))
WHERE I.[purchase_order_id] IS NULL OR I.[purchase_order_id] <> U.[id];

INSERT INTO #item
SELECT S.rn, @base_id+1000000000+S.rn, 0,
  S.[company_code], S.[plant_code], S.[tenant_code], S.[culture_code],
  S.[purchase_order_code], S.[supplier_code], S.[order_date], S.[line_number],
  S.[request_code], S.[request_line_number],
  S.[material_code], S.[material_description], S.[material_specification],
  S.[purchase_unit], S.[order_quantity], S.[received_quantity],
  S.[purchase_per_unit], S.[purchase_unit_price],
  S.[discount_rate], S.[discount_amount],
  S.[tax_included_amount], S.[untaxed_amount], S.[tax_amount], S.[purchase_amount],
  S.[delivery_status], S.[is_obsolete],
  S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT N.*, ROW_NUMBER() OVER (
    ORDER BY N.[company_code], N.[plant_code], N.[purchase_order_code], N.[supplier_code], N.[order_date], N.[line_number], N.[material_code]
  ) AS rn
  FROM (
    SELECT
      LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4) AS [company_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4) AS [plant_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) AS [culture_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_order_code])), 20), N''), N'') AS [purchase_order_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(SH.[supplier_code])), 10), N''), N'') AS [supplier_code],
      ISNULL(TRY_CAST(SH.[order_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS [order_date],
      COALESCE(TRY_CAST(R.[line_number] AS INT), 0) AS [line_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[request_code])), 20), N'') AS [request_code],
      TRY_CAST(R.[request_line_number] AS INT) AS [request_line_number],
      CASE
        WHEN LEN(LTRIM(RTRIM(ISNULL(R.[material_code], N''))))=18 AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE NULLIF(LEFT(LTRIM(RTRIM(R.[material_code])), 20), N'')
      END AS [material_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[material_description], N''))), 40) AS [material_description],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_specification])), 70), N'') AS [material_specification],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[purchase_unit])), 20), N''), N'') AS [purchase_unit],
      ROUND(COALESCE(TRY_CAST(R.[order_quantity] AS DECIMAL(18,8)), 0), 5) AS [order_quantity],
      ROUND(COALESCE(TRY_CAST(R.[received_quantity] AS DECIMAL(18,8)), 0), 5) AS [received_quantity],
      COALESCE(TRY_CAST(R.[purchase_per_unit] AS INT), 0) AS [purchase_per_unit],
      ROUND(COALESCE(TRY_CAST(R.[purchase_unit_price] AS DECIMAL(18,8)), 0), 5) AS [purchase_unit_price],
      ROUND(COALESCE(TRY_CAST(R.[discount_rate] AS DECIMAL(18,8)), 0), 2) AS [discount_rate],
      ROUND(COALESCE(TRY_CAST(R.[discount_amount] AS DECIMAL(18,8)), 0), 5) AS [discount_amount],
      ROUND(COALESCE(TRY_CAST(R.[tax_included_amount] AS DECIMAL(18,8)), 0), 5) AS [tax_included_amount],
      ROUND(COALESCE(TRY_CAST(R.[untaxed_amount] AS DECIMAL(18,8)), 0), 5) AS [untaxed_amount],
      ROUND(COALESCE(TRY_CAST(R.[tax_amount] AS DECIMAL(18,8)), 0), 5) AS [tax_amount],
      ROUND(COALESCE(TRY_CAST(R.[purchase_amount] AS DECIMAL(18,8)), 0), 5) AS [purchase_amount],
      COALESCE(TRY_CAST(R.[delivery_status] AS INT), 0) AS [delivery_status],
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
        PARTITION BY
          LEFT(LTRIM(RTRIM(ISNULL(R.[company_code], N''))), 4),
          LEFT(LTRIM(RTRIM(ISNULL(R.[plant_code], N''))), 4),
          LTRIM(RTRIM(R.[purchase_order_code])),
          LTRIM(RTRIM(SH.[supplier_code])),
          CAST(ISNULL(TRY_CAST(SH.[order_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE),
          COALESCE(TRY_CAST(R.[line_number] AS INT), 0),
          CASE
            WHEN LEN(LTRIM(RTRIM(ISNULL(R.[material_code], N''))))=18 AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
            THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
            ELSE ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[material_code], N''))), 20), N''), N'')
          END
        ORDER BY COALESCE(TRY_CAST(R.[line_number] AS INT), 0)
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_order_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_order] SH
      ON SH.[id]=R.[purchase_order_id]
    WHERE LTRIM(RTRIM(ISNULL(R.[purchase_order_code], N'')))<>N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT), 0)>0
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(SH.[supplier_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N'')))<>N''
  ) N
  WHERE N.dup_rn=1
) S
WHERE @batch_size=0 OR S.rn<=@batch_size;

UPDATE I SET I.[purchase_order_id]=H.[id], I.[id]=COALESCE(T.[id], I.[id]), I.[plant_code]=H.[plant_code]
FROM #item I
INNER JOIN #hdr H
  ON H.[company_code]=I.[company_code]
 AND H.[plant_code]=I.[plant_code]
 AND H.[purchase_order_code]=I.[purchase_order_code]
 AND H.[supplier_code]=I.[supplier_code]
 AND CAST(H.[order_date] AS DATE)=CAST(I.[order_date] AS DATE)
LEFT JOIN [takt_logistics_procurement_purchase_order_item] T
  ON T.[tenant_code]=I.[tenant_code] AND T.[company_code]=I.[company_code]
 AND T.[purchase_order_id]=H.[id] AND T.[line_number]=I.[line_number]
 AND ISNULL(T.[material_code],N'')=ISNULL(I.[material_code],N'');

DECLARE @item_source INT = (SELECT COUNT(*) FROM #item WHERE [purchase_order_id]<>0);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @item_source AS [to_rn],
  @item_source AS [max_rn],
  @item_source AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@item_source) AS NVARCHAR(20)), N'|',
  CAST((@item_source) AS NVARCHAR(20)), N'|',
  CAST((@item_source) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @item_sap_keys INT = (
  SELECT COUNT(*) FROM (
    SELECT
      LTRIM(RTRIM(R.[purchase_order_code])) AS [purchase_order_code],
      LTRIM(RTRIM(SH.[supplier_code])) AS [supplier_code],
      CAST(ISNULL(TRY_CAST(SH.[order_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE) AS [order_day],
      COALESCE(TRY_CAST(R.[line_number] AS INT),0) AS [line_number],
      CASE
        WHEN LEN(LTRIM(RTRIM(ISNULL(R.[material_code], N''))))=18 AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[material_code], N''))), 20), N''), N'')
      END AS [material_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_order_item] R
    INNER JOIN [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_order] SH ON SH.[id]=R.[purchase_order_id]
    WHERE LTRIM(RTRIM(ISNULL(R.[purchase_order_code], N'')))<>N''
      AND COALESCE(TRY_CAST(R.[line_number] AS INT),0)>0
      AND LTRIM(RTRIM(ISNULL(R.[plant_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(SH.[supplier_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[company_code], N'')))<>N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N'')))<>N''
    GROUP BY
      LTRIM(RTRIM(R.[purchase_order_code])),
      LTRIM(RTRIM(SH.[supplier_code])),
      CAST(ISNULL(TRY_CAST(SH.[order_date] AS DATETIME), CAST(CONVERT(DATE, @now) AS DATETIME)) AS DATE),
      COALESCE(TRY_CAST(R.[line_number] AS INT),0),
      CASE
        WHEN LEN(LTRIM(RTRIM(ISNULL(R.[material_code], N''))))=18 AND LTRIM(RTRIM(R.[material_code])) NOT LIKE '%[^0-9]%'
        THEN RIGHT(LTRIM(RTRIM(R.[material_code])), 10)
        ELSE ISNULL(NULLIF(LEFT(LTRIM(RTRIM(ISNULL(R.[material_code], N''))), 20), N''), N'')
      END
  ) K
);
IF @item_source <> @item_sap_keys
BEGIN
  DECLARE @item_src_msg NVARCHAR(200) = CONCAT(N'明细业务键装入不一致: keys=', @item_sap_keys, N', loaded=', @item_source);
  THROW 50003, @item_src_msg, 1;
END;
DELETE FROM #item WHERE [purchase_order_id]=0;

DECLARE @item_before INT = (
  SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_order_item] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #item), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE [takt_logistics_procurement_purchase_order_item] AS T
USING (SELECT * FROM #item WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code]=S.[tenant_code] AND T.[company_code]=S.[company_code]
 AND T.[purchase_order_id]=S.[purchase_order_id] AND T.[line_number]=S.[line_number]
 AND ISNULL(T.[material_code],N'')=ISNULL(S.[material_code],N'')
WHEN MATCHED AND (
  ISNULL(T.[request_code],N'')<>ISNULL(S.[request_code],N'')
  OR ISNULL(T.[request_line_number],-1)<>ISNULL(S.[request_line_number],-1)
  OR ISNULL(T.[material_description],N'')<>S.[material_description]
  OR ISNULL(T.[material_specification],N'')<>ISNULL(S.[material_specification],N'')
  OR T.[purchase_unit]<>S.[purchase_unit]
  OR T.[order_quantity]<>S.[order_quantity] OR T.[received_quantity]<>S.[received_quantity]
  OR T.[purchase_per_unit]<>S.[purchase_per_unit] OR T.[purchase_unit_price]<>S.[purchase_unit_price]
  OR T.[discount_rate]<>S.[discount_rate] OR T.[discount_amount]<>S.[discount_amount]
  OR T.[tax_included_amount]<>S.[tax_included_amount] OR T.[untaxed_amount]<>S.[untaxed_amount]
  OR T.[tax_amount]<>S.[tax_amount] OR T.[purchase_amount]<>S.[purchase_amount]
  OR T.[delivery_status]<>S.[delivery_status] OR T.[is_obsolete]<>S.[is_obsolete]
  OR ISNULL(T.[plant_code],N'')<>S.[plant_code] OR ISNULL(T.[culture_code],N'')<>ISNULL(S.[culture_code],N'')
  OR T.[is_deleted]<>S.[is_deleted]

  OR ISNULL(T.[ext_field], N'') <> ISNULL(S.[ext_field], N'')
  OR ISNULL(T.[remark], N'') <> ISNULL(S.[remark], N'')

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN UPDATE SET
  T.[purchase_order_code]=S.[purchase_order_code],
  T.[request_code]=S.[request_code],
  T.[request_line_number]=S.[request_line_number],
  T.[material_code]=S.[material_code],
  T.[material_description]=S.[material_description],
  T.[material_specification]=S.[material_specification],
  T.[purchase_unit]=S.[purchase_unit],
  T.[order_quantity]=S.[order_quantity],
  T.[received_quantity]=S.[received_quantity],
  T.[purchase_per_unit]=S.[purchase_per_unit],
  T.[purchase_unit_price]=S.[purchase_unit_price],
  T.[discount_rate]=S.[discount_rate],
  T.[discount_amount]=S.[discount_amount],
  T.[tax_included_amount]=S.[tax_included_amount],
  T.[untaxed_amount]=S.[untaxed_amount],
  T.[tax_amount]=S.[tax_amount],
  T.[purchase_amount]=S.[purchase_amount],
  T.[delivery_status]=S.[delivery_status],
  T.[is_obsolete]=S.[is_obsolete],
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
  INSERT ([id],[purchase_order_id],[plant_code],[purchase_order_code],[line_number],[request_code],[request_line_number],[material_code],[material_description],[material_specification],[purchase_unit],[order_quantity],[received_quantity],[purchase_per_unit],[purchase_unit_price],[discount_rate],[discount_amount],[tax_included_amount],[untaxed_amount],[tax_amount],[purchase_amount],[delivery_status],[is_obsolete],[tenant_code],[company_code],[culture_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[purchase_order_id],S.[plant_code],S.[purchase_order_code],S.[line_number],S.[request_code],S.[request_line_number],S.[material_code],S.[material_description],S.[material_specification],S.[purchase_unit],S.[order_quantity],S.[received_quantity],S.[purchase_per_unit],S.[purchase_unit_price],S.[discount_rate],S.[discount_amount],S.[tax_included_amount],S.[untaxed_amount],S.[tax_amount],S.[purchase_amount],S.[delivery_status],S.[is_obsolete],S.[tenant_code],S.[company_code],S.[culture_code],S.[ext_field],S.[remark],COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at])
OUTPUT S.rn, $action, INSERTED.[id], INSERTED.[purchase_order_code], INSERTED.[line_number]
INTO #item_delta (rn, oper_type, id, [purchase_order_code], [line_number]);
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

UPDATE I SET I.[id]=T.[id]
FROM #item I
INNER JOIN [takt_logistics_procurement_purchase_order_item] T
  ON T.[tenant_code]=I.[tenant_code] AND T.[company_code]=I.[company_code]
 AND T.[purchase_order_id]=I.[purchase_order_id] AND T.[line_number]=I.[line_number]
 AND ISNULL(T.[material_code],N'')=ISNULL(I.[material_code],N'');

UPDATE T SET T.[is_deleted]=1, T.[deleted_by]=@sync_user_id, T.[deleted_at]=@now, T.[updated_by]=@sync_user_id, T.[updated_at]=@now
OUTPUT INSERTED.[id], INSERTED.[purchase_order_code], INSERTED.[line_number]
INTO #item_soft ([id], [purchase_order_code], [line_number])
FROM [takt_logistics_procurement_purchase_order_item] T
WHERE T.[is_deleted]=0
  AND EXISTS (SELECT 1 FROM #item S0 WHERE S0.[tenant_code]=T.[tenant_code] AND S0.[company_code]=T.[company_code])
  AND NOT EXISTS (SELECT 1 FROM #item S WHERE S.[id]=T.[id]);
DECLARE @item_delete INT = @@ROWCOUNT;

DECLARE @hdr_source_active INT = (SELECT COUNT(*) FROM #hdr WHERE [is_deleted]=0);
DECLARE @item_source_active INT = (SELECT COUNT(*) FROM #item WHERE [is_deleted]=0);
DECLARE @hdr_after INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_procurement_purchase_order] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
DECLARE @item_after INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_procurement_purchase_order_item] T
  WHERE T.[is_deleted]=0
    AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code])
);
IF @hdr_after <> @hdr_source_active BEGIN DECLARE @hdr_cnt NVARCHAR(200)=CONCAT(N'主表有效行数不一致: source=',@hdr_source_active,N', active=',@hdr_after); THROW 50002,@hdr_cnt,1; END;
IF @item_after <> @item_source_active BEGIN DECLARE @item_cnt NVARCHAR(200)=CONCAT(N'明细有效行数不一致: source=',@item_source_active,N', active=',@item_after); THROW 50002,@item_cnt,1; END;

DECLARE @hdr_ins INT=(SELECT COUNT(*) FROM #hdr_delta WHERE oper_type=N'INSERT');
DECLARE @hdr_upd INT=(SELECT COUNT(*) FROM #hdr_delta WHERE oper_type=N'UPDATE');
DECLARE @hdr_unchanged INT=@hdr_source-@hdr_ins-@hdr_upd;
DECLARE @item_ins INT=(SELECT COUNT(*) FROM #item_delta WHERE oper_type=N'INSERT');
DECLARE @item_upd INT=(SELECT COUNT(*) FROM #item_delta WHERE oper_type=N'UPDATE');
DECLARE @item_unchanged INT=@item_source-@item_ins-@item_upd;

DECLARE @hdr_soft_keys NVARCHAR(MAX)=N'';
SELECT @hdr_soft_keys=STRING_AGG(CAST(CONCAT(CAST([id] AS NVARCHAR(30)),N'|',ISNULL([plant_code],N''),N'/',ISNULL([purchase_order_code],N''),N'/',ISNULL([supplier_code],N'')) AS NVARCHAR(MAX)), N'; ')
FROM (SELECT TOP (100) * FROM #hdr_soft ORDER BY [id]) SoftSample;
SET @hdr_soft_keys=ISNULL(@hdr_soft_keys,N'');
IF @hdr_delete>100 SET @hdr_soft_keys=CONCAT(@hdr_soft_keys,N'; ...(+',CAST(@hdr_delete-100 AS NVARCHAR(20)),N')');

DECLARE @item_soft_keys NVARCHAR(MAX)=N'';
SELECT @item_soft_keys=STRING_AGG(CAST(CONCAT(CAST([id] AS NVARCHAR(30)),N'|',ISNULL([purchase_order_code],N''),N'/',CAST([line_number] AS NVARCHAR(20))) AS NVARCHAR(MAX)), N'; ')
FROM (SELECT TOP (100) * FROM #item_soft ORDER BY [id]) SoftSample;
SET @item_soft_keys=ISNULL(@item_soft_keys,N'');
IF @item_delete>100 SET @item_soft_keys=CONCAT(@item_soft_keys,N'; ...(+',CAST(@item_delete-100 AS NVARCHAR(20)),N')');

DECLARE @hdr_sap_raw INT = (SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_order]);
DECLARE @item_sap_raw INT = (SELECT COUNT(*) FROM [{{SourceDatabase}}].[dbo].[takt_logistics_procurement_purchase_order_item]);
DECLARE @hdr_physical INT = (SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_order] T WHERE EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code]));
DECLARE @item_physical INT = (SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_order_item] T WHERE EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code]));
DECLARE @hdr_soft_total INT = (SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_order] T WHERE T.[is_deleted]=1 AND EXISTS (SELECT 1 FROM #hdr S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code]));
DECLARE @item_soft_total INT = (SELECT COUNT(*) FROM [takt_logistics_procurement_purchase_order_item] T WHERE T.[is_deleted]=1 AND EXISTS (SELECT 1 FROM #item S WHERE S.[tenant_code]=T.[tenant_code] AND S.[company_code]=T.[company_code]));

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
