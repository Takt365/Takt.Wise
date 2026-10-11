// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktOnlineDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：Online 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktOnline 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// Online 响应 DTO
// ========================================

/// <summary>
/// 在线用户实体 公司级实体：在线用户按租户+公司双重隔离
/// 对应前端 TaktOnlineDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktOnlineDto : TaktCompanyDtoBase
{
    /// <summary>
    /// OnlineID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OnlineId { get; set; }

    /// <summary>
    /// SignalR 连接 ID（唯一索引：租户+公司内唯一，见 ix_online_connection_id_unique）
    /// </summary>
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// SignalR 连接 名称（填充字段）
    /// </summary>
    public string? ConnectionName { get; set; }

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 连接 IP 地址
    /// </summary>
    public string ConnectIp { get; set; } = string.Empty;

    /// <summary>
    /// 连接地点
    /// </summary>
    public string ConnectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType）
    /// </summary>
    public string DeviceType { get; set; } = TaktConstants.DeviceType.Unknown;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType）
    /// </summary>
    public string BrowserType { get; set; } = TaktConstants.BrowserType.Unknown;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem）
    /// </summary>
    public string OperatingSystem { get; set; } = TaktConstants.OperatingSystem.Unknown;

    /// <summary>
    /// 连接时间
    /// </summary>
    public DateTime ConnectTime { get; set; }

    /// <summary>
    /// 最后活动时间
    /// </summary>
    public DateTime LastActiveTime { get; set; }

    /// <summary>
    /// 断开时间（未断开时为 null）
    /// </summary>
    public DateTime? DisconnectTime { get; set; }

    /// <summary>
    /// 连接时长（秒）
    /// </summary>
    public int ConnectionDuration { get; set; }

    /// <summary>
    /// 在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）
    /// </summary>
    public int OnlineStatus { get; set; } = 0;

}

// ========================================
// Online 查询 DTO
// ========================================

/// <summary>
/// Online 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktOnlineQueryDto : TaktPagedQuery
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
    /// SignalR 连接 ID（唯一索引：租户+公司内唯一，见 ix_online_connection_id_unique）
    /// </summary>
    public string? ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 连接 IP 地址
    /// </summary>
    public string? ConnectIp { get; set; } = string.Empty;

    /// <summary>
    /// 连接地点
    /// </summary>
    public string? ConnectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string? UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType）
    /// </summary>
    public string? DeviceType { get; set; }

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType）
    /// </summary>
    public string? BrowserType { get; set; }

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem）
    /// </summary>
    public string? OperatingSystem { get; set; }

    /// <summary>
    /// 连接时间（范围查询-开始）
    /// </summary>
    public DateTime? ConnectTimeStart { get; set; }

    /// <summary>
    /// 连接时间（范围查询-结束）
    /// </summary>
    public DateTime? ConnectTimeEnd { get; set; }

    /// <summary>
    /// 最后活动时间（范围查询-开始）
    /// </summary>
    public DateTime? LastActiveTimeStart { get; set; }

    /// <summary>
    /// 最后活动时间（范围查询-结束）
    /// </summary>
    public DateTime? LastActiveTimeEnd { get; set; }

    /// <summary>
    /// 断开时间（范围查询-开始）
    /// </summary>
    public DateTime? DisconnectTimeStart { get; set; }

    /// <summary>
    /// 断开时间（范围查询-结束）
    /// </summary>
    public DateTime? DisconnectTimeEnd { get; set; }

    /// <summary>
    /// 连接时长（秒）
    /// </summary>
    public int? ConnectionDuration { get; set; }

    /// <summary>
    /// 在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）
    /// </summary>
    public int? OnlineStatus { get; set; }

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
// 创建Online DTO
// ========================================

/// <summary>
/// 创建Online DTO
/// </summary>
public class TaktOnlineCreateDto
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
    /// SignalR 连接 ID（唯一索引：租户+公司内唯一，见 ix_online_connection_id_unique）
    /// </summary>
    [Required(ErrorMessage = "SignalR 连接 ID（唯一索引：租户+公司内唯一，见 ix_online_connection_id_unique）不能为空")]
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [Required(ErrorMessage = "用户 ID不能为空")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "用户名不能为空")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 连接 IP 地址
    /// </summary>
    public string ConnectIp { get; set; } = string.Empty;

    /// <summary>
    /// 连接地点
    /// </summary>
    public string ConnectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType）
    /// </summary>
    public string DeviceType { get; set; } = TaktConstants.DeviceType.Unknown;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType）
    /// </summary>
    public string BrowserType { get; set; } = TaktConstants.BrowserType.Unknown;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem）
    /// </summary>
    public string OperatingSystem { get; set; } = TaktConstants.OperatingSystem.Unknown;

    /// <summary>
    /// 连接时间
    /// </summary>
    public DateTime ConnectTime { get; set; }

    /// <summary>
    /// 最后活动时间
    /// </summary>
    public DateTime LastActiveTime { get; set; }

    /// <summary>
    /// 断开时间（未断开时为 null）
    /// </summary>
    public DateTime? DisconnectTime { get; set; }

    /// <summary>
    /// 连接时长（秒）
    /// </summary>
    public int ConnectionDuration { get; set; }

    /// <summary>
    /// 在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）
    /// </summary>
    public int OnlineStatus { get; set; } = 0;

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
// 更新Online DTO
// ========================================

/// <summary>
/// 更新Online DTO
/// 继承 TaktOnlineCreateDto，添加 OnlineId 字段
/// </summary>
public class TaktOnlineUpdateDto : TaktOnlineCreateDto
{
    /// <summary>
    /// OnlineID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OnlineId { get; set; }

}

// ========================================
// Online 状态 DTO
// ========================================

/// <summary>
/// Online 状态更新 DTO
/// </summary>
public class TaktOnlineStatusDto
{
    /// <summary>
    /// OnlineID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OnlineId { get; set; }

    /// <summary>
    /// 在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）
    /// </summary>
    [Required(ErrorMessage = "在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）不能为空")]
    public int OnlineStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Online 导入模板行 DTO
/// </summary>
public class TaktOnlineTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
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
    /// SignalR 连接 ID（唯一索引：租户+公司内唯一，见 ix_online_connection_id_unique）
    /// </summary>
    public string? ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 连接 IP 地址
    /// </summary>
    public string? ConnectIp { get; set; } = string.Empty;

    /// <summary>
    /// 连接地点
    /// </summary>
    public string? ConnectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string? UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType）
    /// </summary>
    public string? DeviceType { get; set; }

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType）
    /// </summary>
    public string? BrowserType { get; set; }

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem）
    /// </summary>
    public string? OperatingSystem { get; set; }

    /// <summary>
    /// 连接时间
    /// </summary>
    public DateTime? ConnectTime { get; set; }

    /// <summary>
    /// 最后活动时间
    /// </summary>
    public DateTime? LastActiveTime { get; set; }

    /// <summary>
    /// 断开时间
    /// </summary>
    public DateTime? DisconnectTime { get; set; }

    /// <summary>
    /// 连接时长（秒）
    /// </summary>
    public int? ConnectionDuration { get; set; }

    /// <summary>
    /// 在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）
    /// </summary>
    public int? OnlineStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// Online 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktOnlineImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
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
    /// SignalR 连接 ID（唯一索引：租户+公司内唯一，见 ix_online_connection_id_unique）
    /// </summary>
    public string? ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 连接 IP 地址
    /// </summary>
    public string? ConnectIp { get; set; } = string.Empty;

    /// <summary>
    /// 连接地点
    /// </summary>
    public string? ConnectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string? UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType）
    /// </summary>
    public string? DeviceType { get; set; }

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType）
    /// </summary>
    public string? BrowserType { get; set; }

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem）
    /// </summary>
    public string? OperatingSystem { get; set; }

    /// <summary>
    /// 连接时间
    /// </summary>
    public DateTime? ConnectTime { get; set; }

    /// <summary>
    /// 最后活动时间
    /// </summary>
    public DateTime? LastActiveTime { get; set; }

    /// <summary>
    /// 断开时间
    /// </summary>
    public DateTime? DisconnectTime { get; set; }

    /// <summary>
    /// 连接时长（秒）
    /// </summary>
    public int? ConnectionDuration { get; set; }

    /// <summary>
    /// 在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）
    /// </summary>
    public int? OnlineStatus { get; set; }

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
// 导出 DTO
// ========================================

/// <summary>
/// Online 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktOnlineExportDto
{
    /// <summary>
    /// OnlineID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long OnlineId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// SignalR 连接 ID（唯一索引：租户+公司内唯一，见 ix_online_connection_id_unique）
    /// </summary>
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 连接 IP 地址
    /// </summary>
    public string ConnectIp { get; set; } = string.Empty;

    /// <summary>
    /// 连接地点
    /// </summary>
    public string ConnectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 用户代理（User-Agent）
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 登录设备（TaktConstants.DeviceType）
    /// </summary>
    public string DeviceType { get; set; } = TaktConstants.DeviceType.Unknown;

    /// <summary>
    /// 浏览器（TaktConstants.BrowserType）
    /// </summary>
    public string BrowserType { get; set; } = TaktConstants.BrowserType.Unknown;

    /// <summary>
    /// 操作系统（TaktConstants.OperatingSystem）
    /// </summary>
    public string OperatingSystem { get; set; } = TaktConstants.OperatingSystem.Unknown;

    /// <summary>
    /// 连接时间
    /// </summary>
    public DateTime ConnectTime { get; set; }

    /// <summary>
    /// 最后活动时间
    /// </summary>
    public DateTime LastActiveTime { get; set; }

    /// <summary>
    /// 断开时间（未断开时为 null）
    /// </summary>
    public DateTime? DisconnectTime { get; set; }

    /// <summary>
    /// 连接时长（秒）
    /// </summary>
    public int ConnectionDuration { get; set; }

    /// <summary>
    /// 在线状态（字典 sys_online_status；0=在线 1=离线 2=离开）
    /// </summary>
    public int OnlineStatus { get; set; } = 0;

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
// 在线统计 DTO
// ========================================

/// <summary>
/// 在线时长统计查询 DTO
/// </summary>
public class TaktOnlineStatisticsQueryDto
{
    /// <summary>
    /// 用户名（为空时取当前登录用户）
    /// </summary>
    public string? UserName { get; set; }
}

/// <summary>
/// 在线时长统计 DTO（唯一实现：ITaktOnlineService.GetOnlineStatisticsAsync）
/// </summary>
public class TaktOnlineStatisticsDto
{
    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 当前活跃在线连接数
    /// </summary>
    public int OnlineCount { get; set; }

    /// <summary>
    /// 当前在线总时长（秒）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CurrentDurationSeconds { get; set; }

    /// <summary>
    /// 当天累计在线时长（秒）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TodayDurationSeconds { get; set; }

    /// <summary>
    /// 本周累计在线时长（秒，周一至今日）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WeekTotalDurationSeconds { get; set; }

    /// <summary>
    /// 本周日均在线时长（秒）= 本周累计 / 本周已过去自然日天数
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WeekAverageDurationSeconds { get; set; }

    /// <summary>
    /// 当月累计在线时长（秒）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MonthDurationSeconds { get; set; }

    /// <summary>
    /// 本月日均在线时长（秒）= 本月累计 / 本月已过去自然日天数
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MonthAverageDurationSeconds { get; set; }
}

/// <summary>
/// 在线看板统计 DTO（公司维度：在线人数、当日总访问量）
/// </summary>
public class TaktOnlineDashboardStatisticsDto
{
    /// <summary>
    /// 当前在线用户数（OnlineStatus=0）
    /// </summary>
    public int OnlineUserCount { get; set; }

    /// <summary>
    /// 当日总访问量（TaktVisitLog 当日 VisitCount 之和，与在线时长无关）
    /// </summary>
    public int TodayVisitCount { get; set; }
}

// ========================================
// SignalR 强退 / 统计推送 DTO
// ========================================

/// <summary>
/// 在线用户强退 DTO
/// </summary>
public class TaktOnlineForceKickDto
{
    /// <summary>
    /// SignalR 连接 ID（可选，与在线记录 ID 二选一校验）
    /// </summary>
    public string? ConnectionId { get; set; }

    /// <summary>
    /// 强退原因
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// 延迟强退秒数（0 或未传表示立即强退；见 TaktOnlineConstants.DelayedKickSeconds）
    /// </summary>
    public int DelaySeconds { get; set; }
}

/// <summary>
/// 在线用户批量强退 DTO
/// </summary>
public class TaktOnlineForceKickBatchDto
{
    /// <summary>
    /// 在线用户记录 ID 列表
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public List<long> OnlineIds { get; set; } = [];

    /// <summary>
    /// 强退原因
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// 延迟强退秒数（0 或未传表示立即强退）
    /// </summary>
    public int DelaySeconds { get; set; }
}

/// <summary>
/// SignalR 向指定用户推送统计请求 DTO
/// </summary>
public class TaktSignalRPushStatisticsRequestDto
{
    /// <summary>
    /// 目标用户 ID（可选，前端 string 雪花 ID）
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// 目标用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;
}
