// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.Logging
// 文件名称：TaktLoginLog.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：登录日志实体，记录用户登录行为和结果（统计日志域）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Constants;

namespace Takt.Domain.Entities.Statistics.Logging;

/// <summary>
/// 登录日志实体
/// </summary>
/// <remarks>
/// 记录用户登录行为和结果（统计日志域）。
/// 数据隔离：租户 + 公司（TaktCompanyEntityBase）。
/// </remarks>
[SugarTable("takt_statistics_logging_login_log", "登录日志表")]
[SugarIndex("ix_login_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_login_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_login_log_created_at", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CreatedAt), OrderByType.Desc, false)]
[SugarIndex("ix_takt_statistics_logging_login_log_login_ip", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(LoginIp), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_login_log_login_result", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(LoginResult), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_login_log_UserName", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserName), OrderByType.Asc, false)]
public class TaktLoginLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 用户名（登录账号）
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 登录方式（TaktConstants.LoginType；如 password=账号密码、refreshtoken=刷新令牌）
    /// </summary>
    [SugarColumn(ColumnName = "login_type", ColumnDescription = "登录方式", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = TaktConstants.LoginType.Unknown)]
    public string LoginType { get; set; } = TaktConstants.LoginType.Unknown;

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
    /// 用户代理（User-Agent）
    /// </summary>
    [SugarColumn(ColumnName = "user_agent", ColumnDescription = "用户代理", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 登录结果（TaktConstants.LoginResult；如 success=成功、passworderror=密码错误）
    /// </summary>
    [SugarColumn(ColumnName = "login_result", ColumnDescription = "登录结果", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = TaktConstants.LoginResult.Success)]
    public string LoginResult { get; set; } = TaktConstants.LoginResult.Success;

    /// <summary>
    /// 登录结果消息
    /// </summary>
    [SugarColumn(ColumnName = "login_message", ColumnDescription = "登录结果消息", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string LoginMessage { get; set; } = string.Empty;

    /// <summary>
    /// 登录IP地址
    /// </summary>
    [SugarColumn(ColumnName = "login_ip", ColumnDescription = "登录IP地址", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string LoginIp { get; set; } = string.Empty;

    /// <summary>
    /// 登录地点（IP解析，如：中国-广东省-深圳市）
    /// </summary>
    [SugarColumn(ColumnName = "login_location", ColumnDescription = "登录地点", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string LoginLocation { get; set; } = string.Empty;
    /// <summary>
    /// 登出时间（未登出时为 null；登出成功时由 CloseOpenLoginSessionAsync 回填，对齐 TaktOnline.DisconnectTime）
    /// </summary>
    [SugarColumn(ColumnName = "logout_at", ColumnDescription = "登出时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LogoutAt { get; set; }
}
