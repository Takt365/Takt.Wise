// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.Logging
// 文件名称：TaktBackupLogDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：BackupLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBackupLog 生成，请按需审阅）
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
// BackupLog 响应 DTO
// ========================================

/// <summary>
/// 备份日志（完整审计）
/// 对应前端 TaktBackupLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBackupLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BackupLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BackupLogId { get; set; }

    /// <summary>
    /// 备份种类（小写，如 database / file / config）
    /// </summary>
    public string BackupKind { get; set; } = string.Empty;

    /// <summary>
    /// 来源业务键（备份配置 Id、任务号等，统一字符串）
    /// </summary>
    public string SourceId { get; set; } = string.Empty;

    /// <summary>
    /// 来源业务键（备份配置 Id、任务号等，统一字符串）
    /// </summary>
    public string? SourceName { get; set; }

    /// <summary>
    /// 来源编码快照（配置编码、任务编码等）
    /// </summary>
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标名称（库展示名、目标标签等）
    /// </summary>
    public string TargetName { get; set; } = string.Empty;

    /// <summary>
    /// 目标范围（可选；如租户码、公司码、路径根等）
    /// </summary>
    public string TargetScope { get; set; } = string.Empty;

    /// <summary>
    /// 同步模式快照（1=完整 2=增量；其它场景可按业务约定）
    /// </summary>
    public int SyncMode { get; set; } = 0;

    /// <summary>
    /// 执行方式快照（1=立即 2=后台）
    /// </summary>
    public int ExecuteMode { get; set; } = 0;

    /// <summary>
    /// 路径类型快照（1=本地 2=网络 3=FTP；无路径场景为 0）
    /// </summary>
    public int PathType { get; set; } = 0;

    /// <summary>
    /// 执行后结果路径
    /// </summary>
    public string? ResultPath { get; set; } = string.Empty;

    /// <summary>
    /// 结果大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// 运行状态（0=进行中 1=成功 2=失败）
    /// </summary>
    public int RunStatus { get; set; } = 0;

    /// <summary>
    /// 失败错误信息
    /// </summary>
    public string? ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? FinishedAt { get; set; }

}

// ========================================
// BackupLog 查询 DTO
// ========================================

/// <summary>
/// BackupLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBackupLogQueryDto : TaktPagedQuery
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
    /// 备份种类（小写，如 database / file / config）
    /// </summary>
    public string? BackupKind { get; set; } = string.Empty;

    /// <summary>
    /// 来源业务键（备份配置 Id、任务号等，统一字符串）
    /// </summary>
    public string? SourceId { get; set; } = string.Empty;

    /// <summary>
    /// 来源编码快照（配置编码、任务编码等）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标名称（库展示名、目标标签等）
    /// </summary>
    public string? TargetName { get; set; } = string.Empty;

    /// <summary>
    /// 目标范围（可选；如租户码、公司码、路径根等）
    /// </summary>
    public string? TargetScope { get; set; } = string.Empty;

    /// <summary>
    /// 同步模式快照（1=完整 2=增量；其它场景可按业务约定）
    /// </summary>
    public int? SyncMode { get; set; }

    /// <summary>
    /// 执行方式快照（1=立即 2=后台）
    /// </summary>
    public int? ExecuteMode { get; set; }

    /// <summary>
    /// 路径类型快照（1=本地 2=网络 3=FTP；无路径场景为 0）
    /// </summary>
    public int? PathType { get; set; }

    /// <summary>
    /// 执行后结果路径
    /// </summary>
    public string? ResultPath { get; set; } = string.Empty;

    /// <summary>
    /// 结果大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileSizeBytes { get; set; }

    /// <summary>
    /// 运行状态（0=进行中 1=成功 2=失败）
    /// </summary>
    public int? RunStatus { get; set; }

    /// <summary>
    /// 失败错误信息
    /// </summary>
    public string? ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间（范围查询-开始）
    /// </summary>
    public DateTime? StartedAtStart { get; set; }

    /// <summary>
    /// 开始时间（范围查询-结束）
    /// </summary>
    public DateTime? StartedAtEnd { get; set; }

    /// <summary>
    /// 结束时间（范围查询-开始）
    /// </summary>
    public DateTime? FinishedAtStart { get; set; }

    /// <summary>
    /// 结束时间（范围查询-结束）
    /// </summary>
    public DateTime? FinishedAtEnd { get; set; }

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
// 创建BackupLog DTO
// ========================================

/// <summary>
/// 创建BackupLog DTO
/// </summary>
public class TaktBackupLogCreateDto
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
    /// 备份种类（小写，如 database / file / config）
    /// </summary>
    [Required(ErrorMessage = "备份种类（小写，如 database / file / config）不能为空")]
    public string BackupKind { get; set; } = string.Empty;

    /// <summary>
    /// 来源业务键（备份配置 Id、任务号等，统一字符串）
    /// </summary>
    [Required(ErrorMessage = "来源业务键（备份配置 Id、任务号等，统一字符串）不能为空")]
    public string SourceId { get; set; } = string.Empty;

    /// <summary>
    /// 来源编码快照（配置编码、任务编码等）
    /// </summary>
    [Required(ErrorMessage = "来源编码快照（配置编码、任务编码等）不能为空")]
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标名称（库展示名、目标标签等）
    /// </summary>
    [Required(ErrorMessage = "目标名称（库展示名、目标标签等）不能为空")]
    public string TargetName { get; set; } = string.Empty;

    /// <summary>
    /// 目标范围（可选；如租户码、公司码、路径根等）
    /// </summary>
    [Required(ErrorMessage = "目标范围（可选；如租户码、公司码、路径根等）不能为空")]
    public string TargetScope { get; set; } = string.Empty;

    /// <summary>
    /// 同步模式快照（1=完整 2=增量；其它场景可按业务约定）
    /// </summary>
    public int SyncMode { get; set; } = 0;

    /// <summary>
    /// 执行方式快照（1=立即 2=后台）
    /// </summary>
    public int ExecuteMode { get; set; } = 0;

    /// <summary>
    /// 路径类型快照（1=本地 2=网络 3=FTP；无路径场景为 0）
    /// </summary>
    public int PathType { get; set; } = 0;

    /// <summary>
    /// 执行后结果路径
    /// </summary>
    public string? ResultPath { get; set; } = string.Empty;

    /// <summary>
    /// 结果大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// 运行状态（0=进行中 1=成功 2=失败）
    /// </summary>
    public int RunStatus { get; set; } = 0;

    /// <summary>
    /// 失败错误信息
    /// </summary>
    public string? ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? FinishedAt { get; set; }

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
// 更新BackupLog DTO
// ========================================

/// <summary>
/// 更新BackupLog DTO
/// 继承 TaktBackupLogCreateDto，添加 BackupLogId 字段
/// </summary>
public class TaktBackupLogUpdateDto : TaktBackupLogCreateDto
{
    /// <summary>
    /// BackupLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BackupLogId { get; set; }

}

// ========================================
// BackupLog 状态 DTO
// ========================================

/// <summary>
/// BackupLog 状态更新 DTO
/// </summary>
public class TaktBackupLogStatusDto
{
    /// <summary>
    /// BackupLogID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BackupLogId { get; set; }

    /// <summary>
    /// 运行状态（0=进行中 1=成功 2=失败）
    /// </summary>
    [Required(ErrorMessage = "运行状态（0=进行中 1=成功 2=失败）不能为空")]
    public int RunStatus { get; set; } = 0;
}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// BackupLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBackupLogExportDto
{
    /// <summary>
    /// BackupLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BackupLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 备份种类（小写，如 database / file / config）
    /// </summary>
    public string BackupKind { get; set; } = string.Empty;

    /// <summary>
    /// 来源业务键（备份配置 Id、任务号等，统一字符串）
    /// </summary>
    public string SourceId { get; set; } = string.Empty;

    /// <summary>
    /// 来源编码快照（配置编码、任务编码等）
    /// </summary>
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标名称（库展示名、目标标签等）
    /// </summary>
    public string TargetName { get; set; } = string.Empty;

    /// <summary>
    /// 目标范围（可选；如租户码、公司码、路径根等）
    /// </summary>
    public string TargetScope { get; set; } = string.Empty;

    /// <summary>
    /// 同步模式快照（1=完整 2=增量；其它场景可按业务约定）
    /// </summary>
    public int SyncMode { get; set; } = 0;

    /// <summary>
    /// 执行方式快照（1=立即 2=后台）
    /// </summary>
    public int ExecuteMode { get; set; } = 0;

    /// <summary>
    /// 路径类型快照（1=本地 2=网络 3=FTP；无路径场景为 0）
    /// </summary>
    public int PathType { get; set; } = 0;

    /// <summary>
    /// 执行后结果路径
    /// </summary>
    public string? ResultPath { get; set; } = string.Empty;

    /// <summary>
    /// 结果大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// 运行状态（0=进行中 1=成功 2=失败）
    /// </summary>
    public int RunStatus { get; set; } = 0;

    /// <summary>
    /// 失败错误信息
    /// </summary>
    public string? ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? FinishedAt { get; set; }

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
