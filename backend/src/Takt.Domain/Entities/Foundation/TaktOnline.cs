// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktOnline.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线用户实体，用于通过 SignalR 管理在线用户
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Shared.Constants;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 在线用户实体
/// 公司级实体：租户+公司+UserId 唯一一行（当前/历史在线状态与 ConnectionDuration 均写同一行）
/// </summary>
[SugarTable("takt_foundation_online", "在线用户表")]
[SugarIndex("ix_online_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_online_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_online_connection_id_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConnectionId), OrderByType.Asc, true)]
[SugarIndex("ix_online_connect_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConnectTime), OrderByType.Desc, false)]
[SugarIndex("ix_online_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OnlineStatus), OrderByType.Asc, false)]
[SugarIndex("ix_online_user_id_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, true)]
[SugarIndex("ix_online_user_name", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserName), OrderByType.Asc, false)]
public class TaktOnline : TaktCompanyEntityBase
{
    /// <summary>
    /// SignalR 连接 ID（当前连接；租户+公司内唯一，见 ix_online_connection_id_unique）
    /// </summary>
    [SugarColumn(ColumnName = "connection_id", ColumnDescription = "SignalR连接ID", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 连接 IP 地址
    /// </summary>
    [SugarColumn(ColumnName = "connect_ip", ColumnDescription = "连接IP地址", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ConnectIp { get; set; } = string.Empty;

    /// <summary>
    /// 连接地点
    /// </summary>
    [SugarColumn(ColumnName = "connect_location", ColumnDescription = "连接地点", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ConnectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    [SugarColumn(ColumnName = "user_agent", ColumnDescription = "用户代理", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType，如 unknown、pc、mobile、tablet）
    /// </summary>
    [SugarColumn(ColumnName = "device_type", ColumnDescription = "登录设备", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = TaktConstants.DeviceType.Unknown)]
    public string DeviceType { get; set; } = TaktConstants.DeviceType.Unknown;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType，如 unknown、chrome、firefox、safari、edge）
    /// </summary>
    [SugarColumn(ColumnName = "browser_type", ColumnDescription = "浏览器", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = TaktConstants.BrowserType.Unknown)]
    public string BrowserType { get; set; } = TaktConstants.BrowserType.Unknown;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem，如 unknown、windows、macos、linux、android、ios）
    /// </summary>
    [SugarColumn(ColumnName = "operating_system", ColumnDescription = "操作系统", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = TaktConstants.OperatingSystem.Unknown)]
    public string OperatingSystem { get; set; } = TaktConstants.OperatingSystem.Unknown;

    /// <summary>
    /// 连接时间
    /// </summary>
    [SugarColumn(ColumnName = "connect_time", ColumnDescription = "连接时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ConnectTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 最后活动时间（Heartbeat 刷新；登出/断开时写为 DisconnectTime，对齐 TaktLoginLog.LogoutAt）
    /// </summary>
    [SugarColumn(ColumnName = "last_active_time", ColumnDescription = "最后活动时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime LastActiveTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 断开时间（未断开时为 null，对齐登录日志 LogoutAt）
    /// </summary>
    [SugarColumn(ColumnName = "disconnect_time", ColumnDescription = "断开时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? DisconnectTime { get; set; }

    /// <summary>
    /// SignalR Heartbeat 累计 +ReportingIntervalSeconds；写入与统计见 TaktOnlineService
    /// </summary>
    [SugarColumn(ColumnName = "connection_duration", ColumnDescription = "连接时长", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ConnectionDuration { get; set; }

    /// <summary>
    /// 当前会话开始时 TaktDurationLog 当日已累计秒数（ConnectTime 自然日）
    /// </summary>
    [SugarColumn(ColumnName = "session_duration_baseline_seconds", ColumnDescription = "会话开始时当日时长基线秒数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SessionDurationBaselineSeconds { get; set; }

    /// <summary>
    /// 日汇总基线对应的自然日（跨天会话时随 Heartbeat 刷新）
    /// </summary>
    [SugarColumn(ColumnName = "daily_duration_baseline_date", ColumnDescription = "日汇总基线日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime DailyDurationBaselineDate { get; set; } = DateTime.Today;

    /// <summary>
    /// 当前自然日 TaktDurationLog 已累计秒数基线（DailyDurationBaselineDate 当天）
    /// </summary>
    [SugarColumn(ColumnName = "daily_duration_baseline_seconds", ColumnDescription = "日汇总基线秒数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DailyDurationBaselineSeconds { get; set; }

    /// <summary>
    /// 在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）
    /// </summary>
    [SugarColumn(ColumnName = "online_status", ColumnDescription = "在线状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OnlineStatus { get; set; } = 0;
}
