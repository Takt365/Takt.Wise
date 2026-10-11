// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.Logging
// 文件名称：TaktArchiveLogDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：ArchiveLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktArchiveLog 生成，请按需审阅）
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
// ArchiveLog 响应 DTO
// ========================================

/// <summary>
/// 归档日志（完整审计）
/// 对应前端 TaktArchiveLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktArchiveLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ArchiveLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ArchiveLogId { get; set; }

    /// <summary>
    /// 归档种类（小写点号分段，如 table.year / file / attachment）
    /// </summary>
    public string ArchiveKind { get; set; } = string.Empty;

    /// <summary>
    /// 来源业务键（策略 Id、单据号等，统一字符串）
    /// </summary>
    public string SourceId { get; set; } = string.Empty;

    /// <summary>
    /// 来源名称（表名、路径、资源名等）
    /// </summary>
    public string SourceName { get; set; } = string.Empty;

    /// <summary>
    /// 归档目标名称（年分表名、归档路径等）
    /// </summary>
    public string TargetName { get; set; } = string.Empty;

    /// <summary>
    /// 归档年份（按年归档时填写；其它场景可空）
    /// </summary>
    public int? ArchiveYear { get; set; }

    /// <summary>
    /// 归档前匹配数量（行/文件/对象）
    /// </summary>
    public int SourceCount { get; set; } = 0;

    /// <summary>
    /// 实际归档数量
    /// </summary>
    public int ArchivedCount { get; set; } = 0;

    /// <summary>
    /// 源侧删除数量（热区清理等；无删除则为 0）
    /// </summary>
    public int DeletedCount { get; set; } = 0;

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
// ArchiveLog 查询 DTO
// ========================================

/// <summary>
/// ArchiveLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktArchiveLogQueryDto : TaktPagedQuery
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
    /// 归档种类（小写点号分段，如 table.year / file / attachment）
    /// </summary>
    public string? ArchiveKind { get; set; } = string.Empty;

    /// <summary>
    /// 来源业务键（策略 Id、单据号等，统一字符串）
    /// </summary>
    public string? SourceId { get; set; } = string.Empty;

    /// <summary>
    /// 来源名称（表名、路径、资源名等）
    /// </summary>
    public string? SourceName { get; set; } = string.Empty;

    /// <summary>
    /// 归档目标名称（年分表名、归档路径等）
    /// </summary>
    public string? TargetName { get; set; } = string.Empty;

    /// <summary>
    /// 归档年份（按年归档时填写；其它场景可空）
    /// </summary>
    public int? ArchiveYear { get; set; }

    /// <summary>
    /// 归档前匹配数量（行/文件/对象）
    /// </summary>
    public int? SourceCount { get; set; }

    /// <summary>
    /// 实际归档数量
    /// </summary>
    public int? ArchivedCount { get; set; }

    /// <summary>
    /// 源侧删除数量（热区清理等；无删除则为 0）
    /// </summary>
    public int? DeletedCount { get; set; }

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
// 创建ArchiveLog DTO
// ========================================

/// <summary>
/// 创建ArchiveLog DTO
/// </summary>
public class TaktArchiveLogCreateDto
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
    /// 归档种类（小写点号分段，如 table.year / file / attachment）
    /// </summary>
    [Required(ErrorMessage = "归档种类（小写点号分段，如 table.year / file / attachment）不能为空")]
    public string ArchiveKind { get; set; } = string.Empty;

    /// <summary>
    /// 来源业务键（策略 Id、单据号等，统一字符串）
    /// </summary>
    [Required(ErrorMessage = "来源业务键（策略 Id、单据号等，统一字符串）不能为空")]
    public string SourceId { get; set; } = string.Empty;

    /// <summary>
    /// 来源名称（表名、路径、资源名等）
    /// </summary>
    [Required(ErrorMessage = "来源名称（表名、路径、资源名等）不能为空")]
    public string SourceName { get; set; } = string.Empty;

    /// <summary>
    /// 归档目标名称（年分表名、归档路径等）
    /// </summary>
    [Required(ErrorMessage = "归档目标名称（年分表名、归档路径等）不能为空")]
    public string TargetName { get; set; } = string.Empty;

    /// <summary>
    /// 归档年份（按年归档时填写；其它场景可空）
    /// </summary>
    public int? ArchiveYear { get; set; }

    /// <summary>
    /// 归档前匹配数量（行/文件/对象）
    /// </summary>
    public int SourceCount { get; set; } = 0;

    /// <summary>
    /// 实际归档数量
    /// </summary>
    public int ArchivedCount { get; set; } = 0;

    /// <summary>
    /// 源侧删除数量（热区清理等；无删除则为 0）
    /// </summary>
    public int DeletedCount { get; set; } = 0;

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
// 更新ArchiveLog DTO
// ========================================

/// <summary>
/// 更新ArchiveLog DTO
/// 继承 TaktArchiveLogCreateDto，添加 ArchiveLogId 字段
/// </summary>
public class TaktArchiveLogUpdateDto : TaktArchiveLogCreateDto
{
    /// <summary>
    /// ArchiveLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ArchiveLogId { get; set; }

}

// ========================================
// ArchiveLog 状态 DTO
// ========================================

/// <summary>
/// ArchiveLog 状态更新 DTO
/// </summary>
public class TaktArchiveLogStatusDto
{
    /// <summary>
    /// ArchiveLogID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ArchiveLogId { get; set; }

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
/// ArchiveLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktArchiveLogExportDto
{
    /// <summary>
    /// ArchiveLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ArchiveLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 归档种类（小写点号分段，如 table.year / file / attachment）
    /// </summary>
    public string ArchiveKind { get; set; } = string.Empty;

    /// <summary>
    /// 来源业务键（策略 Id、单据号等，统一字符串）
    /// </summary>
    public string SourceId { get; set; } = string.Empty;

    /// <summary>
    /// 来源名称（表名、路径、资源名等）
    /// </summary>
    public string SourceName { get; set; } = string.Empty;

    /// <summary>
    /// 归档目标名称（年分表名、归档路径等）
    /// </summary>
    public string TargetName { get; set; } = string.Empty;

    /// <summary>
    /// 归档年份（按年归档时填写；其它场景可空）
    /// </summary>
    public int? ArchiveYear { get; set; }

    /// <summary>
    /// 归档前匹配数量（行/文件/对象）
    /// </summary>
    public int SourceCount { get; set; } = 0;

    /// <summary>
    /// 实际归档数量
    /// </summary>
    public int ArchivedCount { get; set; } = 0;

    /// <summary>
    /// 源侧删除数量（热区清理等；无删除则为 0）
    /// </summary>
    public int DeletedCount { get; set; } = 0;

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
