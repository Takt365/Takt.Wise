// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.Logging
// 文件名称：TaktOperLog.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：操作日志实体，记录用户操作信息与当前请求/响应快照（统计日志域）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Constants;

namespace Takt.Domain.Entities.Statistics.Logging;

/// <summary>
/// 操作日志实体
/// </summary>
/// <remarks>
/// 记录业务操作上下文及当前操作值（RequestParam、JsonResult 等 JSON 快照）。
/// 与 TaktDeltaLog 区分：操作日志不记录库表字段级变更前后对比。
/// 数据隔离：租户 + 公司（TaktCompanyEntityBase），与 TaktLoginLog 一致。
/// 业务字段均非空：无数据用空串；UserName 无法解析时为 unknown；Browser/Os/DeviceType 默认 unknown；ErrorMsg 成功时为空串。
/// </remarks>
[SugarTable("takt_statistics_logging_oper_log", "操作日志表")]
[SugarIndex("ix_oper_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_oper_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_oper_log_oper_module", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OperModule), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_oper_log_oper_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OperStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_oper_log_oper_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OperTime), OrderByType.Desc, false)]
[SugarIndex("ix_takt_statistics_logging_oper_log_oper_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OperType), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_oper_log_tenant_company_oper_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OperTime), OrderByType.Desc, false)]
[SugarIndex("ix_takt_statistics_logging_oper_log_user_name", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserName), OrderByType.Asc, false)]
public class TaktOperLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false, DefaultValue = TaktConstants.AuditUserName.Unknown)]
    public string UserName { get; set; } = TaktConstants.AuditUserName.Unknown;

    /// <summary>
    /// 操作模块（如：用户管理、部门管理）
    /// </summary>
    [SugarColumn(ColumnName = "oper_module", ColumnDescription = "操作模块", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string OperModule { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型（TaktConstants.OperType；如 create=新增、update=修改、delete=删除、query=查询）
    /// </summary>
    [SugarColumn(ColumnName = "oper_type", ColumnDescription = "操作类型", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = TaktConstants.OperType.Unknown)]
    public string OperType { get; set; } = TaktConstants.OperType.Unknown;

    /// <summary>
    /// 操作方法（如：TaktUserService.CreateUserAsync）
    /// </summary>
    [SugarColumn(ColumnName = "oper_method", ColumnDescription = "操作方法", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string OperMethod { get; set; } = string.Empty;

    /// <summary>
    /// 请求方式（GET、POST、PUT、DELETE 等）
    /// </summary>
    [SugarColumn(ColumnName = "request_method", ColumnDescription = "请求方式", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// 操作 URL（含查询字符串）
    /// </summary>
    [SugarColumn(ColumnName = "oper_url", ColumnDescription = "操作URL", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string OperUrl { get; set; } = string.Empty;

    /// <summary>
    /// 请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）
    /// </summary>
    [SugarColumn(ColumnName = "request_param", ColumnDescription = "请求参数", ColumnDataType = "nvarchar", Length = -1, IsNullable = false)]
    public string RequestParam { get; set; } = string.Empty;

    /// <summary>
    /// 返回结果 JSON（当前操作出参/响应摘要）
    /// </summary>
    [SugarColumn(ColumnName = "json_result", ColumnDescription = "返回结果", ColumnDataType = "nvarchar", Length = -1, IsNullable = false)]
    public string JsonResult { get; set; } = string.Empty;

    /// <summary>
    /// 错误消息（失败时；成功为空串）
    /// </summary>
    [SugarColumn(ColumnName = "error_msg", ColumnDescription = "错误消息", ColumnDataType = "nvarchar", Length = -1, IsNullable = false)]
    public string ErrorMsg { get; set; } = string.Empty;

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
    /// 操作时间（业务操作发生时刻）
    /// </summary>
    [SugarColumn(ColumnName = "oper_time", ColumnDescription = "操作时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime OperTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    [SugarColumn(ColumnName = "elapsed_time", ColumnDescription = "执行耗时毫秒", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ElapsedTime { get; set; }

    /// <summary>
    /// 操作状态（字典 sys_success_fail_status；0=失败 1=成功）
    /// </summary>
    [SugarColumn(ColumnName = "oper_status", ColumnDescription = "操作状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OperStatus { get; set; } = 1;
}
