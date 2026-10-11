// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.Logging
// 文件名称：TaktDeltaLog.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：差异日志实体，记录 SqlSugar AOP 数据变更差异（变更前/后/差异 JSON）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Constants;

namespace Takt.Domain.Entities.Statistics.Logging;

/// <summary>
/// 差异日志实体（AOP 审计）
/// </summary>
/// <remarks>
/// 记录库表数据变更的旧值与新值：BeforeData、AfterData，以及字段级差异 DiffData（在 OnDiffLogEvent 内对比 BeforeData/AfterData 的 Columns 得到）。
/// 与 TaktOperLog 区分：差异日志面向持久化层变更，不替代 HTTP 操作入参日志。
/// 数据隔离：租户 + 公司（TaktCompanyEntityBase），与 TaktLoginLog 一致。
/// </remarks>
[SugarTable("takt_statistics_logging_delta_log", "差异日志表")]
[SugarIndex("ix_delta_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_delta_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_delta_log_oper_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OperTime), OrderByType.Desc, false)]
[SugarIndex("ix_takt_statistics_logging_delta_log_oper_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OperType), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_delta_log_primary_key_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PrimaryKeyId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_delta_log_table_name", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TableName), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_delta_log_tenant_company_oper_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OperTime), OrderByType.Desc, false)]
[SugarIndex("ix_takt_statistics_logging_delta_log_user_name", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserName), OrderByType.Asc, false)]
public class TaktDeltaLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false, DefaultValue = TaktConstants.AuditUserName.Unknown)]
    public string UserName { get; set; } = TaktConstants.AuditUserName.Unknown;

    /// <summary>
    /// 操作类型（TaktConstants.OperType；如 create=新增、update=修改、delete=删除、query=查询）
    /// </summary>
    [SugarColumn(ColumnName = "oper_type", ColumnDescription = "操作类型", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = TaktConstants.OperType.Unknown)]
    public string OperType { get; set; } = TaktConstants.OperType.Unknown;

    /// <summary>
    /// 数据库表名（SugarTable 物理表名）
    /// </summary>
    [SugarColumn(ColumnName = "table_name", ColumnDescription = "表名", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 业务主键 ID
    /// </summary>
    [SugarColumn(ColumnName = "primary_key_id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PrimaryKeyId { get; set; }

    /// <summary>
    /// 修改前数据 JSON（旧值快照；无数据为空串）
    /// </summary>
    [SugarColumn(ColumnName = "before_data", ColumnDescription = "修改前数据", ColumnDataType = "nvarchar", Length = -1, IsNullable = false)]
    public string BeforeData { get; set; } = string.Empty;

    /// <summary>
    /// 修改后数据 JSON（新值快照；无数据为空串）
    /// </summary>
    [SugarColumn(ColumnName = "after_data", ColumnDescription = "修改后数据", ColumnDataType = "nvarchar", Length = -1, IsNullable = false)]
    public string AfterData { get; set; } = string.Empty;

    /// <summary>
    /// 差异内容 JSON（变更字段及旧/新值明细；无差异为空串）
    /// </summary>
    [SugarColumn(ColumnName = "diff_data", ColumnDescription = "差异内容", ColumnDataType = "nvarchar", Length = -1, IsNullable = false)]
    public string DiffData { get; set; } = string.Empty;

    /// <summary>
    /// 执行的 SQL 语句（AOP 捕获；无 SQL 为空串）
    /// </summary>
    [SugarColumn(ColumnName = "sql_statement", ColumnDescription = "SQL语句", ColumnDataType = "nvarchar", Length = -1, IsNullable = false)]
    public string SqlStatement { get; set; } = string.Empty;

    /// <summary>
    /// 操作 IP
    /// </summary>
    [SugarColumn(ColumnName = "oper_ip", ColumnDescription = "操作IP", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string OperIp { get; set; } = string.Empty;

    /// <summary>
    /// 操作地点（由 OperIp 解析，如：中国-广东省-深圳市）
    /// </summary>
    [SugarColumn(ColumnName = "oper_location", ColumnDescription = "操作地点", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string OperLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    [SugarColumn(ColumnName = "user_agent", ColumnDescription = "用户代理", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）
    /// </summary>
    [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = TaktConstants.BrowserType.Unknown)]
    public string Browser { get; set; } = TaktConstants.BrowserType.Unknown;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）
    /// </summary>
    [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = TaktConstants.OperatingSystem.Unknown)]
    public string Os { get; set; } = TaktConstants.OperatingSystem.Unknown;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType；如 unknown、pc、mobile、tablet）
    /// </summary>
    [SugarColumn(ColumnName = "device_type", ColumnDescription = "登录设备", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = TaktConstants.DeviceType.Unknown)]
    public string DeviceType { get; set; } = TaktConstants.DeviceType.Unknown;

    /// <summary>
    /// 操作时间（数据变更发生时刻）
    /// </summary>
    [SugarColumn(ColumnName = "oper_time", ColumnDescription = "操作时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime OperTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    [SugarColumn(ColumnName = "elapsed_time", ColumnDescription = "执行耗时毫秒", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ElapsedTime { get; set; }
}
