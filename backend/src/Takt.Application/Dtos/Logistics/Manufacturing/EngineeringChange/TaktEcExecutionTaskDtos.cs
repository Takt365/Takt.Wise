// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecutionTaskDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：EcExecutionTask 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcExecutionTask 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

// ========================================
// EcExecutionTask 响应 DTO
// ========================================

/// <summary>
/// 完成时间
/// 对应前端 TaktEcExecutionTaskDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcExecutionTaskDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcExecutionTaskID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcExecutionTaskId { get; set; }

    /// <summary>
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

    /// <summary>
    /// 通知单 名称（填充字段）
    /// </summary>
    public string? EcNotificationName { get; set; }

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcTeId { get; set; }

    /// <summary>
    /// 设变 名称（填充字段）
    /// </summary>
    public string? EcName { get; set; }

    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联设变部门行 ID（TaktEcPc/Mp 等 8 张部门执行表主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcExecId { get; set; }

    /// <summary>
    /// 关联设变部门行 名称（填充字段）
    /// </summary>
    public string? EcExecName { get; set; }

    /// <summary>
    /// 设变明细 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }

    /// <summary>
    /// 设变明细 名称（填充字段）
    /// </summary>
    public string? EcDetailName { get; set; }

    /// <summary>
    /// 责任部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务标题
    /// </summary>
    public string TaskTitle { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）
    /// </summary>
    public int TaskStatus { get; set; } = 0;

    /// <summary>
    /// 进度百分比 0-100
    /// </summary>
    public int ProgressPercent { get; set; } = 0;

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 最近进度说明
    /// </summary>
    public string? LastProgressRemark { get; set; } = string.Empty;

    /// <summary>
    /// 完成时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

}

// ========================================
// EcExecutionTask 查询 DTO
// ========================================

/// <summary>
/// EcExecutionTask 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcExecutionTaskQueryDto : TaktPagedQuery
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
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationId { get; set; }

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcTeId { get; set; }

    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联设变部门行 ID（TaktEcPc/Mp 等 8 张部门执行表主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcExecId { get; set; }

    /// <summary>
    /// 设变明细 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }

    /// <summary>
    /// 责任部门编码
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务标题
    /// </summary>
    public string? TaskTitle { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）
    /// </summary>
    public int? TaskStatus { get; set; }

    /// <summary>
    /// 进度百分比 0-100
    /// </summary>
    public int? ProgressPercent { get; set; }

    /// <summary>
    /// 截止日期（范围查询-开始）
    /// </summary>
    public DateTime? DueDateStart { get; set; }

    /// <summary>
    /// 截止日期（范围查询-结束）
    /// </summary>
    public DateTime? DueDateEnd { get; set; }

    /// <summary>
    /// 最近进度说明
    /// </summary>
    public string? LastProgressRemark { get; set; } = string.Empty;

    /// <summary>
    /// 完成时间（范围查询-开始）
    /// </summary>
    public DateTime? CompletedAtStart { get; set; }

    /// <summary>
    /// 完成时间（范围查询-结束）
    /// </summary>
    public DateTime? CompletedAtEnd { get; set; }

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
// 创建EcExecutionTask DTO
// ========================================

/// <summary>
/// 创建EcExecutionTask DTO
/// </summary>
public class TaktEcExecutionTaskCreateDto
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
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcTeId { get; set; }

    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    [Required(ErrorMessage = "设变单号（冗余）不能为空")]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联设变部门行 ID（TaktEcPc/Mp 等 8 张部门执行表主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcExecId { get; set; }

    /// <summary>
    /// 设变明细 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }

    /// <summary>
    /// 责任部门编码
    /// </summary>
    [Required(ErrorMessage = "责任部门编码不能为空")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务标题
    /// </summary>
    [Required(ErrorMessage = "任务标题不能为空")]
    public string TaskTitle { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）
    /// </summary>
    public int TaskStatus { get; set; } = 0;

    /// <summary>
    /// 进度百分比 0-100
    /// </summary>
    public int ProgressPercent { get; set; } = 0;

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 最近进度说明
    /// </summary>
    public string? LastProgressRemark { get; set; } = string.Empty;

    /// <summary>
    /// 完成时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

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
// 更新EcExecutionTask DTO
// ========================================

/// <summary>
/// 更新EcExecutionTask DTO
/// 继承 TaktEcExecutionTaskCreateDto，添加 EcExecutionTaskId 字段
/// </summary>
public class TaktEcExecutionTaskUpdateDto : TaktEcExecutionTaskCreateDto
{
    /// <summary>
    /// EcExecutionTaskID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcExecutionTaskId { get; set; }

}

// ========================================
// EcExecutionTask 状态 DTO
// ========================================

/// <summary>
/// EcExecutionTask 状态更新 DTO
/// </summary>
public class TaktEcExecutionTaskStatusDto
{
    /// <summary>
    /// EcExecutionTaskID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcExecutionTaskId { get; set; }

    /// <summary>
    /// 任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）
    /// </summary>
    [Required(ErrorMessage = "任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）不能为空")]
    public int TaskStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcExecutionTask 导入模板行 DTO
/// </summary>
public class TaktEcExecutionTaskTemplateDto
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
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationId { get; set; }

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcTeId { get; set; }

    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联设变部门行 ID（TaktEcPc/Mp 等 8 张部门执行表主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcExecId { get; set; }

    /// <summary>
    /// 设变明细 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }

    /// <summary>
    /// 责任部门编码
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务标题
    /// </summary>
    public string? TaskTitle { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）
    /// </summary>
    public int? TaskStatus { get; set; }

    /// <summary>
    /// 进度百分比 0-100
    /// </summary>
    public int? ProgressPercent { get; set; }

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 最近进度说明
    /// </summary>
    public string? LastProgressRemark { get; set; } = string.Empty;

    /// <summary>
    /// 完成时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

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
/// EcExecutionTask 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcExecutionTaskImportDto
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
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcNotificationId { get; set; }

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcTeId { get; set; }

    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    public string? EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联设变部门行 ID（TaktEcPc/Mp 等 8 张部门执行表主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcExecId { get; set; }

    /// <summary>
    /// 设变明细 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }

    /// <summary>
    /// 责任部门编码
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务标题
    /// </summary>
    public string? TaskTitle { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）
    /// </summary>
    public int? TaskStatus { get; set; }

    /// <summary>
    /// 进度百分比 0-100
    /// </summary>
    public int? ProgressPercent { get; set; }

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 最近进度说明
    /// </summary>
    public string? LastProgressRemark { get; set; } = string.Empty;

    /// <summary>
    /// 完成时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

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
/// EcExecutionTask 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcExecutionTaskExportDto
{
    /// <summary>
    /// EcExecutionTaskID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcExecutionTaskId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 通知单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcNotificationId { get; set; }

    /// <summary>
    /// 设变 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcTeId { get; set; }

    /// <summary>
    /// 设变单号（冗余）
    /// </summary>
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联设变部门行 ID（TaktEcPc/Mp 等 8 张部门执行表主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcExecId { get; set; }

    /// <summary>
    /// 设变明细 ID（可选）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcDetailId { get; set; }

    /// <summary>
    /// 责任部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 任务标题
    /// </summary>
    public string TaskTitle { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（0待执行 1执行中 2已完成 3阻塞 4超时）
    /// </summary>
    public int TaskStatus { get; set; } = 0;

    /// <summary>
    /// 进度百分比 0-100
    /// </summary>
    public int ProgressPercent { get; set; } = 0;

    /// <summary>
    /// 截止日期
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 最近进度说明
    /// </summary>
    public string? LastProgressRemark { get; set; } = string.Empty;

    /// <summary>
    /// 完成时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

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
