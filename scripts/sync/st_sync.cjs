// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts/sync
// 文件名称：st_sync.cjs
// 创建时间：2026-07-07
// 创建人：Takt365(Cursor AI)
// 功能描述：标准工时同步（源库工时表 → takt_logistics_manufacturing_bom_standard_operation_time；自动审批；SMT=0.02800/自插=0.04500；effective_date=当前-10天）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const {
  BATCH_SIZE,
  execSQL,
  execSQLValue,
  formatBatchSizeLabel,
  parseInsUpdSummaryCounts,
  resolveBatchSizeSqlValue,
} = require('./common_sync.cjs');

/** 点数转分钟汇率：工序描述含 SMT → 0.02800；含自插 → 0.04500；其余 → 1 */
const POINTS_TO_MINUTES_RATE_SMT = '0.02800';
const POINTS_TO_MINUTES_RATE_ZICHA = '0.04500';
const POINTS_TO_MINUTES_RATE_DEFAULT = '1.0000';

/**
 * 生成标准工时 MERGE SQL（approval_status=created 自动审批；含 delta/oper 日志）
 * @returns {string}
 */
function makeSyncSql() {
  const batchSizeValue = resolveBatchSizeSqlValue();
  return `
SET NOCOUNT ON;
DECLARE @batch_size INT = ${batchSizeValue};
DECLARE @now DATETIME = GETDATE();
-- 生效日期：当前-10天（与组立日报默认生产日期对齐，保证 by-material 有效期命中）
DECLARE @effective_date DATE = DATEADD(DAY, -10, CAST(@now AS DATE));
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

IF OBJECT_ID('tempdb..#st_source') IS NOT NULL DROP TABLE #st_source;
CREATE TABLE #st_source (
  [rn] INT,
  [id] BIGINT,
  [plant_code] NVARCHAR(100),
  [material_code] NVARCHAR(100),
  [work_center] NVARCHAR(100),
  [operation_desc] NVARCHAR(200),
  [standard_minutes] DECIMAL(18,2),
  [time_unit] NVARCHAR(20),
  [standard_shorts] INT,
  [points_unit] NVARCHAR(20),
  [points_to_minutes_rate] DECIMAL(18,4),
  [converted_minutes] DECIMAL(18,4),
  [tenant_code] NVARCHAR(20),
  [company_code] NVARCHAR(20),
  [ext_field] NVARCHAR(MAX),
  [remark] NVARCHAR(MAX),
  [updated_by] BIGINT
);

INSERT INTO #st_source
SELECT
  S.rn,
  @base_id + S.rn,
  LTRIM(RTRIM(S.[D_SAP_ZPBLD_Z001])),
  LTRIM(RTRIM(S.[D_SAP_ZPBLD_Z002])),
  LTRIM(RTRIM(S.[D_SAP_ZPBLD_Z003])),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[D_SAP_ZPBLD_Z004])), ''), ''),
  COALESCE(TRY_CAST(S.[D_SAP_ZPBLD_Z007] AS DECIMAL(18,2)), 0),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[D_SAP_ZPBLD_Z008])), ''), ''),
  COALESCE(TRY_CAST(S.[D_SAP_ZPBLD_Z005] AS INT), 0),
  ISNULL(NULLIF(LTRIM(RTRIM(S.[D_SAP_ZPBLD_Z006])), ''), ''),
  CASE
    WHEN S.[D_SAP_ZPBLD_Z004] LIKE '%SMT%' THEN CAST(${POINTS_TO_MINUTES_RATE_SMT} AS DECIMAL(18,4))
    WHEN S.[D_SAP_ZPBLD_Z004] LIKE N'%自插%' THEN CAST(${POINTS_TO_MINUTES_RATE_ZICHA} AS DECIMAL(18,4))
    ELSE CAST(${POINTS_TO_MINUTES_RATE_DEFAULT} AS DECIMAL(18,4))
  END AS [points_to_minutes_rate],
  ROUND(
    COALESCE(TRY_CAST(S.[D_SAP_ZPBLD_Z005] AS DECIMAL(18,4)), 0) *
    CASE
      WHEN S.[D_SAP_ZPBLD_Z004] LIKE '%SMT%' THEN CAST(${POINTS_TO_MINUTES_RATE_SMT} AS DECIMAL(18,4))
      WHEN S.[D_SAP_ZPBLD_Z004] LIKE N'%自插%' THEN CAST(${POINTS_TO_MINUTES_RATE_ZICHA} AS DECIMAL(18,4))
      ELSE CAST(${POINTS_TO_MINUTES_RATE_DEFAULT} AS DECIMAL(18,4))
    END
  , 4) AS [converted_minutes],
  '000',
  '2300',
  '{}',
  '',
  900001
FROM (
  SELECT *,
    ROW_NUMBER() OVER (
      ORDER BY [D_SAP_ZPBLD_Z001],[D_SAP_ZPBLD_Z002],[D_SAP_ZPBLD_Z003],[D_SAP_ZPBLD_Z004]
    ) AS rn
  FROM [Sap_Data].[dbo].[PP_SapManhour]
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

IF OBJECT_ID('tempdb..#delta') IS NOT NULL DROP TABLE #delta;
CREATE TABLE #delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  material_code NVARCHAR(100),
  tenant_code NVARCHAR(20),
  company_code NVARCHAR(20),
  change_by BIGINT,
  standard_minutes_old DECIMAL(18,2),
  standard_minutes_new DECIMAL(18,2),
  time_unit_old NVARCHAR(20),
  time_unit_new NVARCHAR(20),
  standard_shorts_old INT,
  standard_shorts_new INT,
  points_unit_old NVARCHAR(20),
  points_unit_new NVARCHAR(20),
  points_to_minutes_rate_old DECIMAL(18,4),
  points_to_minutes_rate_new DECIMAL(18,4),
  converted_minutes_old DECIMAL(18,4),
  converted_minutes_new DECIMAL(18,4),
  ext_field_old NVARCHAR(MAX),
  ext_field_new NVARCHAR(MAX),
  remark_old NVARCHAR(MAX),
  remark_new NVARCHAR(MAX)
);

DECLARE @merge_actions TABLE ([action] NVARCHAR(10));

MERGE INTO [takt_logistics_manufacturing_bom_standard_operation_time] AS T
USING #st_source AS S
ON T.[plant_code] = S.[plant_code]
AND T.[material_code] = S.[material_code]
AND T.[work_center] = S.[work_center]
AND T.[operation_desc] = S.[operation_desc]
WHEN MATCHED THEN
  UPDATE SET
    T.[standard_minutes] = S.[standard_minutes],
    T.[time_unit] = S.[time_unit],
    T.[standard_shorts] = S.[standard_shorts],
    T.[points_unit] = S.[points_unit],
    T.[points_to_minutes_rate] = S.[points_to_minutes_rate],
    T.[converted_minutes] = S.[converted_minutes],
    T.[effective_date] = @effective_date,
    T.[updated_by] = S.[updated_by],
    T.[updated_at] = @now,
    T.[approved_by] = T.[created_by],
    T.[approved_at] = T.[created_at],
    T.[approval_status] = 2
WHEN NOT MATCHED THEN
  INSERT (
    [id],[plant_code],[material_code],[work_center],[operation_desc],
    [standard_minutes],[time_unit],[standard_shorts],[points_unit],
    [points_to_minutes_rate],[converted_minutes],
    [effective_date],[expiry_date],
    [tenant_code],[company_code],[ext_field],[remark],
    [created_by],[created_at],[updated_by],[updated_at],
    [approved_by],[approved_at],[approval_status],
    [is_deleted]
  )
  VALUES (
    S.[id],S.[plant_code],S.[material_code],S.[work_center],S.[operation_desc],
    S.[standard_minutes],S.[time_unit],S.[standard_shorts],S.[points_unit],
    S.[points_to_minutes_rate],S.[converted_minutes],
    @effective_date,'9999-12-31',
    S.[tenant_code],S.[company_code],S.[ext_field],S.[remark],
    S.[updated_by],@now,S.[updated_by],@now,
    S.[updated_by],@now,2,
    0
  )
OUTPUT
  S.rn,
  $action,
  INSERTED.[id],
  INSERTED.[material_code],
  INSERTED.[tenant_code],
  INSERTED.[company_code],
  INSERTED.[updated_by],
  DELETED.[standard_minutes], INSERTED.[standard_minutes],
  DELETED.[time_unit], INSERTED.[time_unit],
  DELETED.[standard_shorts], INSERTED.[standard_shorts],
  DELETED.[points_unit], INSERTED.[points_unit],
  DELETED.[points_to_minutes_rate], INSERTED.[points_to_minutes_rate],
  DELETED.[converted_minutes], INSERTED.[converted_minutes],
  DELETED.[ext_field], INSERTED.[ext_field],
  DELETED.[remark], INSERTED.[remark]
INTO #delta(
  rn, oper_type, id, material_code, tenant_code, company_code, change_by,
  standard_minutes_old, standard_minutes_new,
  time_unit_old, time_unit_new,
  standard_shorts_old, standard_shorts_new,
  points_unit_old, points_unit_new,
  points_to_minutes_rate_old, points_to_minutes_rate_new,
  converted_minutes_old, converted_minutes_new,
  ext_field_old, ext_field_new,
  remark_old, remark_new
);

INSERT INTO [takt_statistics_logging_delta_log] (
  [id],[oper_type],[table_name],[primary_key_id],
  [before_data],[after_data],[diff_data],[sql_statement],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[tenant_code],[company_code],
  [ext_field],[remark],[created_by],[created_at]
)
SELECT
  @base_id + d.rn,
  d.oper_type,
  N'takt_logistics_manufacturing_bom_standard_operation_time',
  d.id,
  ISNULL((
    SELECT
      d.standard_minutes_old AS [standard_minutes],
      d.time_unit_old AS [time_unit],
      d.standard_shorts_old AS [standard_shorts],
      d.points_unit_old AS [points_unit],
      d.points_to_minutes_rate_old AS [points_to_minutes_rate],
      d.converted_minutes_old AS [converted_minutes]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  (
    SELECT
      d.standard_minutes_new AS [standard_minutes],
      d.time_unit_new AS [time_unit],
      d.standard_shorts_new AS [standard_shorts],
      d.points_unit_new AS [points_unit],
      d.points_to_minutes_rate_new AS [points_to_minutes_rate],
      d.converted_minutes_new AS [converted_minutes]
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ),
  ISNULL((
    SELECT
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.standard_minutes_old AS NVARCHAR), 'null') END AS [standard_minutes.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.standard_minutes_new AS NVARCHAR), 'null') END AS [standard_minutes.new],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.converted_minutes_old AS NVARCHAR), 'null') END AS [converted_minutes.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.converted_minutes_new AS NVARCHAR), 'null') END AS [converted_minutes.new],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.points_to_minutes_rate_old AS NVARCHAR), 'null') END AS [points_to_minutes_rate.old],
      CASE WHEN d.oper_type = 'UPDATE' THEN ISNULL(CAST(d.points_to_minutes_rate_new AS NVARCHAR), 'null') END AS [points_to_minutes_rate.new]
    WHERE d.oper_type = 'UPDATE'
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  ), '{}'),
  N'MERGE Manhour Sync',
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,0,
  d.tenant_code,d.company_code,'{}',N'SAP_SYNC',d.change_by,@now
FROM #delta d;

DECLARE @insert_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'INSERT');
DECLARE @update_count INT = (SELECT COUNT(*) FROM #delta WHERE oper_type = 'UPDATE');
DECLARE @json_result NVARCHAR(MAX) = N'{"insert":' + CAST(@insert_count AS NVARCHAR) + N',"update":' + CAST(@update_count AS NVARCHAR) + N'}';

INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[created_by],[created_at]
)
SELECT TOP 1
  @base_id + 1,
  N'SYSTEM_SAP_SYNC',
  N'SAP_SYNC',
  N'工时管理',
  N'exec_sql_merge',
  'SQL',
  N'/sync/sap/manhour',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  d.tenant_code,d.company_code,900001,@now
FROM #delta d;

INSERT INTO @merge_actions SELECT oper_type FROM #delta;

SELECT 'INS', COUNT(*) FROM @merge_actions WHERE [action] = 'INSERT';
SELECT 'UPD', COUNT(*) FROM @merge_actions WHERE [action] = 'UPDATE';
`;
}

// ========================================
// 主流程
// ========================================

(async () => {
  const effectiveDate = new Date();
  effectiveDate.setDate(effectiveDate.getDate() - 10);
  const effectiveDateYmd = [
    effectiveDate.getFullYear(),
    String(effectiveDate.getMonth() + 1).padStart(2, '0'),
    String(effectiveDate.getDate()).padStart(2, '0'),
  ].join('-');
  console.log('==========================================');
  console.log('  工时同步（自动审批 approved=created）');
  console.log(`  effective_date: ${effectiveDateYmd}（当前-10天）`);
  console.log(`  BATCH_SIZE: ${formatBatchSizeLabel()}`);
  console.log('==========================================');

  const totalSourceRows = Number(execSQLValue(
    `SELECT COUNT(*) FROM [Sap_Data].[dbo].[PP_SapManhour]`,
    { filePrefix: 'st_val' }
  ));
  console.log(`\n发现源表数据 ${totalSourceRows} 条，${BATCH_SIZE === 0 ? '执行全量同步' : '仅处理前 ' + BATCH_SIZE + ' 条'}`);

  const syncSql = makeSyncSql();
  const syncResult = await execSQL(syncSql, '工时同步（含自动审批）', { filePrefix: 'st_sync' });
  const summaryCounts = parseInsUpdSummaryCounts(syncResult);

  console.log('\n==========================================');
  console.log('最终日志:');
  console.log('  插入：' + summaryCounts.mainInsert);
  console.log('  更新：' + summaryCounts.mainUpdate);
  console.log('  目标表总记录数：' + execSQLValue(
    'SELECT COUNT(*) FROM [takt_logistics_manufacturing_bom_standard_operation_time]',
    { filePrefix: 'st_val' }
  ));
  console.log('==========================================');
})();