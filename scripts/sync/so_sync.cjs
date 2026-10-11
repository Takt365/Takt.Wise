// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts/sync
// 文件名称：so_sync.cjs
// 创建时间：2026-07-07
// 创建人：Takt365(Cursor AI)
// 功能描述：生产工单同步（PP_SapOrders + 工时/序列号回填 → takt_logistics_manufacturing_aps_production_order）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const {
  BATCH_SIZE,
  execSQL,
  execSQLValue,
  formatBatchSizeLabel,
  resolveBatchSizeSqlValue,
} = require('./common_sync.cjs');

/**
 * 生成生产工单 MERGE SQL（工单 + work_center + serial_no 回填）
 * @returns {string}
 */
function makeSyncSql() {
  const bv = resolveBatchSizeSqlValue();
  return `
SET NOCOUNT ON;
DECLARE @batch_size INT = ${bv};
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

IF OBJECT_ID('tempdb..#order_source') IS NOT NULL DROP TABLE #order_source;
CREATE TABLE #order_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(100),
  [prod_order_code] NVARCHAR(100),
  [material_code] NVARCHAR(100),
  [prod_batch] NVARCHAR(100),
  [prod_order_qty] DECIMAL(18,4),
  [produced_qty] DECIMAL(18,4),
  [unit_of_measure] NVARCHAR(20),
  [actual_start_date] DATE,
  [priority] INT,
  [routing_code] NVARCHAR(100),
  [prod_order_type] NVARCHAR(100)
);

INSERT INTO #order_source
SELECT
  S.rn,
  @base_id + S.rn,
  LTRIM(RTRIM(S.[D_SAP_COOIS_C001])),
  LTRIM(RTRIM(S.[D_SAP_COOIS_C002])),
  LTRIM(RTRIM(S.[D_SAP_COOIS_C003])),
  LTRIM(RTRIM(S.[D_SAP_COOIS_C004])),
  COALESCE(TRY_CAST(S.[D_SAP_COOIS_C005] AS DECIMAL(18,4)), 0),
  COALESCE(TRY_CAST(S.[D_SAP_COOIS_C006] AS DECIMAL(18,4)), 0),
  CASE WHEN ISNULL(S.[D_SAP_COOIS_C004], '') LIKE '%||%' THEN 'EA' ELSE 'PC' END,
  TRY_CAST(S.[D_SAP_COOIS_C007] AS DATE),
  3,
  LTRIM(RTRIM(S.[D_SAP_COOIS_C008])),
  LTRIM(RTRIM(S.[D_SAP_COOIS_C009]))
FROM (
  SELECT *, ROW_NUMBER() OVER (ORDER BY [D_SAP_COOIS_C002]) AS rn
  FROM [Sap_Data].[dbo].[PP_SapOrders]
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

IF OBJECT_ID('tempdb..#order_delta') IS NOT NULL DROP TABLE #order_delta;
CREATE TABLE #order_delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  plant_code NVARCHAR(100),
  prod_order_code NVARCHAR(100),
  material_code_old NVARCHAR(100),
  material_code_new NVARCHAR(100),
  prod_order_qty_old DECIMAL(18,4),
  prod_order_qty_new DECIMAL(18,4),
  produced_qty_old DECIMAL(18,4),
  produced_qty_new DECIMAL(18,4),
  unit_of_measure_old NVARCHAR(20),
  unit_of_measure_new NVARCHAR(20),
  actual_start_date_old DATE,
  actual_start_date_new DATE,
  routing_code_old NVARCHAR(100),
  routing_code_new NVARCHAR(100),
  prod_order_type_old NVARCHAR(100),
  prod_order_type_new NVARCHAR(100)
);

MERGE INTO [takt_logistics_manufacturing_aps_production_order] AS T
USING #order_source AS S
ON T.[plant_code] = S.[plant_code]
AND T.[prod_order_code] = S.[prod_order_code]
WHEN MATCHED THEN
  UPDATE SET
    T.[material_code] = S.[material_code],
    T.[prod_batch] = S.[prod_batch],
    T.[prod_order_qty] = S.[prod_order_qty],
    T.[produced_qty] = S.[produced_qty],
    T.[unit_of_measure] = S.[unit_of_measure],
    T.[actual_start_date] = S.[actual_start_date],
    T.[routing_code] = S.[routing_code],
    T.[prod_order_type] = S.[prod_order_type],
    T.[updated_by] = 900001,
    T.[updated_at] = @now
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[prod_order_code],[material_code],[prod_batch],
    [prod_order_qty],[produced_qty],[unit_of_measure],
    [actual_start_date],[actual_end_date],[priority],[work_center],
    [routing_code],[serial_no],[prod_order_type],
    [planned_order_id],[aps_order_id],
    [planned_start_time],[planned_end_time],[order_status],
    [tenant_code],[company_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],[is_deleted],
    [deleted_by],[deleted_at]
  )
  VALUES (
    S.[id],S.[plant_code],S.[prod_order_code],S.[material_code],S.[prod_batch],
    S.[prod_order_qty],S.[produced_qty],S.[unit_of_measure],
    S.[actual_start_date],NULL,3,'',
    S.[routing_code],'',S.[prod_order_type],
    NULL,NULL,NULL,NULL,1,
    '000','2300','{}','mismatch update',
    900001,@now,900001,@now,0,
    NULL,NULL
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[plant_code],
  INSERTED.[prod_order_code],
  DELETED.[material_code], INSERTED.[material_code],
  DELETED.[prod_order_qty], INSERTED.[prod_order_qty],
  DELETED.[produced_qty], INSERTED.[produced_qty],
  DELETED.[unit_of_measure], INSERTED.[unit_of_measure],
  DELETED.[actual_start_date], INSERTED.[actual_start_date],
  DELETED.[routing_code], INSERTED.[routing_code],
  DELETED.[prod_order_type], INSERTED.[prod_order_type]
INTO #order_delta(
  rn, oper_type, id, plant_code, prod_order_code,
  material_code_old, material_code_new,
  prod_order_qty_old, prod_order_qty_new,
  produced_qty_old, produced_qty_new,
  unit_of_measure_old, unit_of_measure_new,
  actual_start_date_old, actual_start_date_new,
  routing_code_old, routing_code_new,
  prod_order_type_old, prod_order_type_new
);

-- update work_center
;WITH wc_src AS (
  SELECT
    LTRIM(RTRIM([D_SAP_ZPBLD_Z002])) AS material_code,
    LTRIM(RTRIM([D_SAP_ZPBLD_Z003])) AS z003,
    LTRIM(RTRIM([D_SAP_ZPBLD_Z004])) AS z004
  FROM [Sap_Data].[dbo].[PP_SapManhour]
),
wc_agg AS (
  SELECT
    material_code,
    STRING_AGG(z003 + '||' + z004, ';') AS work_center
  FROM wc_src
  GROUP BY material_code
)
UPDATE T
SET
  T.[work_center] = W.work_center,
  T.[updated_at]  = @now
FROM [takt_logistics_manufacturing_aps_production_order] T
JOIN wc_agg W
  ON LTRIM(RTRIM(T.[material_code])) = W.material_code
WHERE ISNULL(T.[work_center], '') <> ISNULL(W.work_center, '');

-- update serial_no
;WITH ser_agg AS (
  SELECT
    LTRIM(RTRIM([D_SAP_SER05_C002])) AS prod_order_code,
    MIN([D_SAP_SER05_C004]) + '~' + MAX([D_SAP_SER05_C004]) AS serial_no
  FROM [Sap_Data].[dbo].[PP_SapOrderSerial]
  WHERE isDelete = 0
  GROUP BY [D_SAP_SER05_C002]
)
UPDATE T
SET
  T.[serial_no] = S.serial_no,
  T.[updated_at] = @now
FROM [takt_logistics_manufacturing_aps_production_order] T
JOIN ser_agg S
  ON LTRIM(RTRIM(T.[prod_order_code])) = S.prod_order_code
WHERE ISNULL(T.[serial_no], '') <> ISNULL(S.serial_no, '');

-- delta_log
INSERT INTO [takt_statistics_logging_delta_log] (
  [id],
  [oper_type],
  [table_name],
  [primary_key_id],
  [before_data],
  [after_data],
  [diff_data],
  [sql_statement],
  [oper_ip],
  [oper_location],
  [user_agent],
  [browser],
  [os],
  [device_type],
  [oper_time],
  [elapsed_time],
  [tenant_code],
  [company_code],
  [ext_field],
  [remark],
  [created_by],
  [created_at]
)
SELECT
  @base_id + d.rn,
  d.oper_type,
  N'takt_logistics_manufacturing_aps_production_order',
  d.id,
  CASE WHEN d.oper_type = 'UPDATE' THEN
    (
      SELECT
        d.material_code_old AS material_code,
        d.prod_order_qty_old AS prod_order_qty,
        d.produced_qty_old AS produced_qty,
        d.unit_of_measure_old AS unit_of_measure,
        d.actual_start_date_old AS actual_start_date,
        d.routing_code_old AS routing_code,
        d.prod_order_type_old AS prod_order_type
      FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    )
  ELSE '{}' END,
  (
    SELECT
      d.material_code_new AS material_code,
      d.prod_order_qty_new AS prod_order_qty,
      d.produced_qty_new AS produced_qty,
      d.unit_of_measure_new AS unit_of_measure,
      d.actual_start_date_new AS actual_start_date,
      d.routing_code_new AS routing_code,
      d.prod_order_type_new AS prod_order_type
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ),
  CASE WHEN d.oper_type = 'UPDATE' THEN
    (
      SELECT
        ISNULL(d.material_code_old, 'null') AS [material_code.old],
        ISNULL(d.material_code_new, 'null') AS [material_code.new],
        ISNULL(CAST(d.prod_order_qty_old AS NVARCHAR), 'null') AS [prod_order_qty.old],
        ISNULL(CAST(d.prod_order_qty_new AS NVARCHAR), 'null') AS [prod_order_qty.new],
        ISNULL(CAST(d.produced_qty_old AS NVARCHAR), 'null') AS [produced_qty.old],
        ISNULL(CAST(d.produced_qty_new AS NVARCHAR), 'null') AS [produced_qty.new]
      FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    )
  ELSE '{}' END,
  N'MERGE Order Sync',
  '127.0.0.1',
  'Server',
  'SQLCMD',
  'Server',
  'Windows',
  'Server',
  @now,
  0,
  '000',
  '2300',
  '{}',
  N'SAP_SYNC',
  900001,
  @now
FROM #order_delta d;

-- oper_log
DECLARE @order_ins INT = (SELECT COUNT(*) FROM #order_delta WHERE oper_type = 'INSERT');
DECLARE @order_upd INT = (SELECT COUNT(*) FROM #order_delta WHERE oper_type = 'UPDATE');
DECLARE @wc_count  INT = (SELECT COUNT(*) FROM [takt_logistics_manufacturing_aps_production_order] WHERE ISNULL([work_center], '') <> '');
DECLARE @ser_count INT = (SELECT COUNT(*) FROM [takt_logistics_manufacturing_aps_production_order] WHERE ISNULL([serial_no], '') <> '');
DECLARE @json_result NVARCHAR(MAX) = N'{"insert":' + ISNULL(CAST(@order_ins AS NVARCHAR),'0') + N',"update":' + ISNULL(CAST(@order_upd AS NVARCHAR),'0') + N',"work_center":' + ISNULL(CAST(@wc_count AS NVARCHAR),'0') + N',"serial_no":' + ISNULL(CAST(@ser_count AS NVARCHAR),'0') + N'}';

INSERT INTO [takt_statistics_logging_oper_log] (
  [id],
  [user_name],
  [oper_type],
  [oper_module],
  [oper_method],
  [request_method],
  [oper_url],
  [request_param],
  [json_result],
  [oper_ip],
  [oper_location],
  [user_agent],
  [browser],
  [os],
  [device_type],
  [oper_time],
  [elapsed_time],
  [oper_status],
  [error_msg],
  [tenant_code],
  [company_code],
  [created_by],
  [created_at]
)
SELECT
  @base_id + 1,
  N'SYSTEM_SAP_SYNC',
  N'SAP_SYNC',
  N'order management',
  N'exec_sql_merge',
  'SQL',
  N'/sync/sap/order',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1',
  'Server',
  'SQLCMD',
  'Server',
  'Windows',
  'Server',
  @now,
  DATEDIFF(MILLISECOND, @now, GETDATE()),
  1,
  '',
  '000',
  '2300',
  900001,
  @now;

-- summary
SELECT 'ORDER_INS', @order_ins;
SELECT 'ORDER_UPD', @order_upd;
SELECT 'WC_CNT', @wc_count;
SELECT 'SER_CNT', @ser_count;
`;
}

/**
 * 从 sqlcmd 输出解析工单/工作中心/序列号汇总行
 * @param {string} text sqlcmd stdout
 * @returns {{ orderIns: number, orderUpd: number, wcCnt: number, serCnt: number }}
 */
function parseAllCounts(text) {
  const result = { orderIns: 0, orderUpd: 0, wcCnt: 0, serCnt: 0 };
  const ins = text.match(/ORDER_INS\s+(\d+)/i);
  const upd = text.match(/ORDER_UPD\s+(\d+)/i);
  const wc  = text.match(/WC_CNT\s+(\d+)/i);
  const ser = text.match(/SER_CNT\s+(\d+)/i);
  result.orderIns = ins ? Number(ins[1]) : 0;
  result.orderUpd = upd ? Number(upd[1]) : 0;
  result.wcCnt  = wc  ? Number(wc[1])  : 0;
  result.serCnt = ser ? Number(ser[1]) : 0;
  return result;
}

// ========================================
// 主流程
// ========================================

(async () => {
  console.log('==========================================');
  console.log('  order sync with work_center + serial_no');
  console.log('  BATCH_SIZE: ' + formatBatchSizeLabel());

  const valOpts = { filePrefix: 'so_val' };
  const orderRows = Number(execSQLValue('SELECT COUNT(*) FROM [Sap_Data].[dbo].[PP_SapOrders]', valOpts));
  const wcRows = Number(execSQLValue(
    "SELECT COUNT(DISTINCT LTRIM(RTRIM(D_SAP_ZPBLD_Z002))) FROM [Sap_Data].[dbo].[PP_SapManhour]",
    valOpts
  ));
  const serRows = Number(execSQLValue(
    'SELECT COUNT(DISTINCT LTRIM(RTRIM(D_SAP_SER05_C002))) FROM [Sap_Data].[dbo].[PP_SapOrderSerial] WHERE isDelete = 0',
    valOpts
  ));

  console.log('');
  console.log('Order source rows: ' + orderRows);
  console.log('Work center materials: ' + wcRows);
  console.log('Serial order codes: ' + serRows);
  console.log('==========================================');

  const syncSql = makeSyncSql();
  const syncResult = await execSQL(syncSql, 'order sync', { filePrefix: 'so_sync' });
  const counts = parseAllCounts(syncResult || '');

  console.log('');
  console.log('==========================================');
  console.log('Result:');
  console.log('  Order: insert ' + counts.orderIns + ', update ' + counts.orderUpd);
  console.log('  Work center: ' + counts.wcCnt);
  console.log('  Serial no: ' + counts.serCnt);
  console.log('==========================================');
})().catch((e) => {
  console.error('FATAL: ' + (e.message || e));
  process.exit(1);
});