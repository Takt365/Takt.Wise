// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.Logging
// 文件名称：TaktTrackingLogDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TrackingLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTrackingLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Statistics.Logging;

// ========================================
// TrackingLog 响应 DTO
// ========================================

/// <summary>
/// 前端交互日志实体
/// 对应前端 TaktTrackingLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTrackingLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TrackingLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrackingLogId { get; set; }

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 事件类型（如 longtask）
    /// </summary>
    public string EventTrackingType { get; set; } = string.Empty;

    /// <summary>
    /// 事件分类（如 performance）
    /// </summary>
    public string EventTrackingCategory { get; set; } = string.Empty;

    /// <summary>
    /// 事件发生时间（客户端 UTC）
    /// </summary>
    public DateTime EventTime { get; set; }

    /// <summary>
    /// 长任务阻塞时长（毫秒）
    /// </summary>
    public int DurationMs { get; set; } = 0;

    /// <summary>
    /// PerformanceEntry.startTime（毫秒，相对页面导航起点）
    /// </summary>
    public decimal PerformanceStartMs { get; set; }

    /// <summary>
    /// PerformanceEntry.name
    /// </summary>
    public string EntryName { get; set; } = string.Empty;

    /// <summary>
    /// 追踪级别（1=warn 2=error，前端阈值映射）
    /// </summary>
    public int TrackingLevel { get; set; } = 0;

    /// <summary>
    /// SPA 路由路径
    /// </summary>
    public string RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 页面完整 URL
    /// </summary>
    public string PageUrl { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerType
    /// </summary>
    public string ContainerType { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerName
    /// </summary>
    public string ContainerName { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerSrc
    /// </summary>
    public string ContainerSrc { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerId
    /// </summary>
    public string ContainerId { get; set; } = string.Empty;

    /// <summary>
    /// 完整 attribution JSON 数组
    /// </summary>
    public string AttributionJson { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 客户端 IP
    /// </summary>
    public string ClientIp { get; set; } = string.Empty;

}

// ========================================
// TrackingLog 查询 DTO
// ========================================

/// <summary>
/// TrackingLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTrackingLogQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 事件类型（如 longtask）
    /// </summary>
    public string? EventTrackingType { get; set; } = string.Empty;

    /// <summary>
    /// 事件分类（如 performance）
    /// </summary>
    public string? EventTrackingCategory { get; set; } = string.Empty;

    /// <summary>
    /// 事件发生时间（客户端 UTC）（范围查询-开始）
    /// </summary>
    public DateTime? EventTimeStart { get; set; }

    /// <summary>
    /// 事件发生时间（客户端 UTC）（范围查询-结束）
    /// </summary>
    public DateTime? EventTimeEnd { get; set; }

    /// <summary>
    /// 长任务阻塞时长（毫秒）
    /// </summary>
    public int? DurationMs { get; set; }

    /// <summary>
    /// PerformanceEntry.startTime（毫秒，相对页面导航起点）
    /// </summary>
    public decimal? PerformanceStartMs { get; set; }

    /// <summary>
    /// PerformanceEntry.name
    /// </summary>
    public string? EntryName { get; set; } = string.Empty;

    /// <summary>
    /// 追踪级别（1=warn 2=error，前端阈值映射）
    /// </summary>
    public int? TrackingLevel { get; set; }

    /// <summary>
    /// SPA 路由路径
    /// </summary>
    public string? RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 页面完整 URL
    /// </summary>
    public string? PageUrl { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerType
    /// </summary>
    public string? ContainerType { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerName
    /// </summary>
    public string? ContainerName { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerSrc
    /// </summary>
    public string? ContainerSrc { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerId
    /// </summary>
    public string? ContainerId { get; set; } = string.Empty;

    /// <summary>
    /// 完整 attribution JSON 数组
    /// </summary>
    public string? AttributionJson { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string? UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 客户端 IP
    /// </summary>
    public string? ClientIp { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建TrackingLog DTO
// ========================================

/// <summary>
/// 创建TrackingLog DTO
/// </summary>
public class TaktTrackingLogCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    [Required(ErrorMessage = "用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）不能为空")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 事件类型（如 longtask）
    /// </summary>
    [Required(ErrorMessage = "事件类型（如 longtask）不能为空")]
    public string EventTrackingType { get; set; } = string.Empty;

    /// <summary>
    /// 事件分类（如 performance）
    /// </summary>
    [Required(ErrorMessage = "事件分类（如 performance）不能为空")]
    public string EventTrackingCategory { get; set; } = string.Empty;

    /// <summary>
    /// 事件发生时间（客户端 UTC）
    /// </summary>
    public DateTime EventTime { get; set; }

    /// <summary>
    /// 长任务阻塞时长（毫秒）
    /// </summary>
    public int DurationMs { get; set; } = 0;

    /// <summary>
    /// PerformanceEntry.startTime（毫秒，相对页面导航起点）
    /// </summary>
    public decimal PerformanceStartMs { get; set; }

    /// <summary>
    /// PerformanceEntry.name
    /// </summary>
    [Required(ErrorMessage = "PerformanceEntry.name不能为空")]
    public string EntryName { get; set; } = string.Empty;

    /// <summary>
    /// 追踪级别（1=warn 2=error，前端阈值映射）
    /// </summary>
    public int TrackingLevel { get; set; } = 0;

    /// <summary>
    /// SPA 路由路径
    /// </summary>
    [Required(ErrorMessage = "SPA 路由路径不能为空")]
    public string RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 页面完整 URL
    /// </summary>
    [Required(ErrorMessage = "页面完整 URL不能为空")]
    public string PageUrl { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerType
    /// </summary>
    [Required(ErrorMessage = "TaskAttribution.containerType不能为空")]
    public string ContainerType { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerName
    /// </summary>
    [Required(ErrorMessage = "TaskAttribution.containerName不能为空")]
    public string ContainerName { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerSrc
    /// </summary>
    [Required(ErrorMessage = "TaskAttribution.containerSrc不能为空")]
    public string ContainerSrc { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerId
    /// </summary>
    [Required(ErrorMessage = "TaskAttribution.containerId不能为空")]
    public string ContainerId { get; set; } = string.Empty;

    /// <summary>
    /// 完整 attribution JSON 数组
    /// </summary>
    [Required(ErrorMessage = "完整 attribution JSON 数组不能为空")]
    public string AttributionJson { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    [Required(ErrorMessage = "用户代理（User-Agent）不能为空")]
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 客户端 IP
    /// </summary>
    [Required(ErrorMessage = "客户端 IP不能为空")]
    public string ClientIp { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新TrackingLog DTO
// ========================================

/// <summary>
/// 更新TrackingLog DTO
/// 继承 TaktTrackingLogCreateDto，添加 TrackingLogId 字段
/// </summary>
public class TaktTrackingLogUpdateDto : TaktTrackingLogCreateDto
{
    /// <summary>
    /// TrackingLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrackingLogId { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// TrackingLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTrackingLogExportDto
{
    /// <summary>
    /// TrackingLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrackingLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 事件类型（如 longtask）
    /// </summary>
    public string EventTrackingType { get; set; } = string.Empty;

    /// <summary>
    /// 事件分类（如 performance）
    /// </summary>
    public string EventTrackingCategory { get; set; } = string.Empty;

    /// <summary>
    /// 事件发生时间（客户端 UTC）
    /// </summary>
    public DateTime EventTime { get; set; }

    /// <summary>
    /// 长任务阻塞时长（毫秒）
    /// </summary>
    public int DurationMs { get; set; } = 0;

    /// <summary>
    /// PerformanceEntry.startTime（毫秒，相对页面导航起点）
    /// </summary>
    public decimal PerformanceStartMs { get; set; }

    /// <summary>
    /// PerformanceEntry.name
    /// </summary>
    public string EntryName { get; set; } = string.Empty;

    /// <summary>
    /// 追踪级别（1=warn 2=error，前端阈值映射）
    /// </summary>
    public int TrackingLevel { get; set; } = 0;

    /// <summary>
    /// SPA 路由路径
    /// </summary>
    public string RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 页面完整 URL
    /// </summary>
    public string PageUrl { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerType
    /// </summary>
    public string ContainerType { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerName
    /// </summary>
    public string ContainerName { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerSrc
    /// </summary>
    public string ContainerSrc { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerId
    /// </summary>
    public string ContainerId { get; set; } = string.Empty;

    /// <summary>
    /// 完整 attribution JSON 数组
    /// </summary>
    public string AttributionJson { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 客户端 IP
    /// </summary>
    public string ClientIp { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

// ========================================
// Long Task 客户端上报 DTO（非实体 CRUD）
// ========================================

/// <summary>
/// Long Task 单条上报 DTO
/// </summary>
public class TaktTrackingLogTrackDto
{
    /// <summary>
    /// 事件类型（默认 longtask）
    /// </summary>
    public string EventTrackingType { get; set; } = string.Empty;

    /// <summary>
    /// 事件分类（默认 performance）
    /// </summary>
    public string EventTrackingCategory { get; set; } = string.Empty;

    /// <summary>
    /// 客户端事件时间
    /// </summary>
    public DateTime EventTime { get; set; }

    /// <summary>
    /// 阻塞时长（毫秒）
    /// </summary>
    public int DurationMs { get; set; }

    /// <summary>
    /// Performance 开始毫秒
    /// </summary>
    public decimal PerformanceStartMs { get; set; }

    /// <summary>
    /// PerformanceEntry.name
    /// </summary>
    public string EntryName { get; set; } = string.Empty;

    /// <summary>
    /// 追踪级别（0~2）
    /// </summary>
    public int TrackingLevel { get; set; }

    /// <summary>
    /// SPA 路由路径
    /// </summary>
    public string RoutePath { get; set; } = string.Empty;

    /// <summary>
    /// 页面完整 URL
    /// </summary>
    public string PageUrl { get; set; } = string.Empty;

    /// <summary>
    /// TaskAttribution.containerType
    /// </summary>
    public string? ContainerType { get; set; }

    /// <summary>
    /// TaskAttribution.containerName
    /// </summary>
    public string? ContainerName { get; set; }

    /// <summary>
    /// TaskAttribution.containerSrc
    /// </summary>
    public string? ContainerSrc { get; set; }

    /// <summary>
    /// TaskAttribution.containerId
    /// </summary>
    public string? ContainerId { get; set; }

    /// <summary>
    /// 归因 JSON（空则落库为 []）
    /// </summary>
    public string? AttributionJson { get; set; }

    /// <summary>
    /// 用户代理
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;
}

/// <summary>
/// Long Task 批量上报 DTO
/// </summary>
public class TaktTrackingLogBatchTrackDto
{
    /// <summary>
    /// 上报条目（单次上限由验证器约束）
    /// </summary>
    public List<TaktTrackingLogTrackDto> Items { get; set; } = new();
}
