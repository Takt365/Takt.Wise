// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.Logging
// 文件名称：TaktTrackingLog.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：前端交互日志实体（Long Task API 主线程卡顿等客户端性能事件）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Constants;

namespace Takt.Domain.Entities.Statistics.Logging;

/// <summary>
/// 前端交互日志实体
/// </summary>
/// <remarks>
/// 记录浏览器 Performance Long Task 等客户端性能事件；由前端批量上报写入。
/// 数据隔离：租户 + 公司（TaktCompanyEntityBase）。
/// </remarks>
[SugarTable("takt_statistics_logging_tracking_log", "交互日志表")]
[SugarIndex("ix_tracking_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_tracking_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_tracking_log_user_name", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserName), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_tracking_log_event_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EventTime), OrderByType.Desc, false)]
[SugarIndex("ix_takt_statistics_logging_tracking_log_duration_ms", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DurationMs), OrderByType.Desc, false)]
[SugarIndex("ix_takt_statistics_logging_tracking_log_tracking_level", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TrackingLevel), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_tracking_log_event_tracking_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EventTrackingType), OrderByType.Asc, false)]
public class TaktTrackingLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false, DefaultValue = TaktConstants.AuditUserName.Unknown)]
    public string UserName { get; set; } = TaktConstants.AuditUserName.Unknown;

    /// <summary>
    /// 事件类型（如 longtask）
    /// </summary>
    [SugarColumn(ColumnName = "event_tracking_type", ColumnDescription = "事件类型", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string EventTrackingType { get; set; } = string.Empty;

    /// <summary>
    /// 事件分类（如 performance）
    /// </summary>
    [SugarColumn(ColumnName = "event_tracking_category", ColumnDescription = "事件分类", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string EventTrackingCategory { get; set; } = string.Empty;

    /// <summary>
    /// 事件发生时间（客户端 UTC）
    /// </summary>
    [SugarColumn(ColumnName = "event_time", ColumnDescription = "事件发生时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime EventTime { get; set; }

    /// <summary>
    /// 长任务阻塞时长（毫秒）
    /// </summary>
    [SugarColumn(ColumnName = "duration_ms", ColumnDescription = "阻塞时长毫秒", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DurationMs { get; set; }

    /// <summary>
    /// PerformanceEntry.startTime（毫秒，相对页面导航起点）
    /// </summary>
    [SugarColumn(ColumnName = "performance_start_ms", ColumnDescription = "Performance开始毫秒", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal PerformanceStartMs { get; set; }

    /// <summary>
    /// PerformanceEntry.name
    /// </summary>
    [SugarColumn(ColumnName = "entry_name", ColumnDescription = "Performance条目名", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string EntryName { get; set; } = string.Empty;

    /// <summary>
    /// 追踪级别（字典 sys_tracking_level；1=warn 2=error）
    /// </summary>
    [SugarColumn(ColumnName = "tracking_level", ColumnDescription = "追踪级别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int TrackingLevel { get; set; } = 1;

    /// <summary>
    /// SPA 路由路径
    /// </summary>
    [SugarColumn(ColumnName = "route_path", ColumnDescription = "路由路径", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 页面完整 URL
    /// </summary>
    [SugarColumn(ColumnName = "page_url", ColumnDescription = "页面URL", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string PageUrl { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerType
    /// </summary>
    [SugarColumn(ColumnName = "container_type", ColumnDescription = "归因容器类型", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string ContainerType { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerName
    /// </summary>
    [SugarColumn(ColumnName = "container_name", ColumnDescription = "归因容器名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ContainerName { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerSrc
    /// </summary>
    [SugarColumn(ColumnName = "container_src", ColumnDescription = "归因脚本来源", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string ContainerSrc { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerId
    /// </summary>
    [SugarColumn(ColumnName = "container_id", ColumnDescription = "归因容器ID", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string ContainerId { get; set; } = string.Empty;

    /// <summary>
    /// 完整 attribution JSON 数组
    /// </summary>
    [SugarColumn(ColumnName = "attribution_json", ColumnDescription = "归因JSON", ColumnDataType = "nvarchar", Length = -1, IsNullable = false)]
    public string AttributionJson { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    [SugarColumn(ColumnName = "user_agent", ColumnDescription = "用户代理", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 客户端 IP
    /// </summary>
    [SugarColumn(ColumnName = "client_ip", ColumnDescription = "客户端IP", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string ClientIp { get; set; } = string.Empty;
}
