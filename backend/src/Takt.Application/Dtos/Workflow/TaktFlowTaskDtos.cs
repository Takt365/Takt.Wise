// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Workflow
// 文件名称：TaktFlowTaskDtos.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：FlowTask 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFlowTask 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Workflow;

// ========================================
// FlowTask 响应 DTO
// ========================================

/// <summary>
/// 流程用户任务实体
/// 对应前端 TaktFlowTaskDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktFlowTaskDto : TaktCompanyDtoBase
{
    /// <summary>
    /// FlowTaskID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTaskId { get; set; }

    /// <summary>
    /// 流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InstanceId { get; set; }

    /// <summary>
    /// 流程实例 名称（填充字段）
    /// </summary>
    public string? InstanceName { get; set; }

    /// <summary>
    /// 任务定义键（设计器 nodeId；与实例 CurrentActivityId 一致）
    /// </summary>
    public string TaskDefinitionKey { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称（冗余字段，便于查询）
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 办理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssigneeUserId { get; set; }

    /// <summary>
    /// 办理人姓名（冗余：按 AssigneeUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? AssigneeUserName { get; set; } = string.Empty;

    /// <summary>
    /// 任务所有者 ID（选项 TaktUsers/options；DictValue=Id；转办前原办理人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 任务所有者姓名（冗余：按 OwnerUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? OwnerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 会签类型（字典 sys_flow_sign_type；1=或签 2=会签）
    /// </summary>
    public int SignType { get; set; } = 0;

    /// <summary>
    /// 优先级
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 认领时间
    /// </summary>
    public DateTime? ClaimTime { get; set; }

    /// <summary>
    /// 办结时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 是否加签任务（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsAddSign { get; set; } = 0;

    /// <summary>
    /// 加签记录 ID（选项 TaktFlowAddSigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AddSignId { get; set; }

    /// <summary>
    /// 加签记录 名称（填充字段）
    /// </summary>
    public string? AddSignName { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 多实例序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）
    /// </summary>
    public int TaskStatus { get; set; } = 0;

    /// <summary>
    /// 所属流程实例
    /// （主表：TaktFlowInstance）
    /// </summary>
    public TaktFlowInstanceDto? Instance { get; set; }

}

// ========================================
// FlowTask 查询 DTO
// ========================================

/// <summary>
/// FlowTask 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFlowTaskQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InstanceId { get; set; }

    /// <summary>
    /// 任务定义键（设计器 nodeId；与实例 CurrentActivityId 一致）
    /// </summary>
    public string? TaskDefinitionKey { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称（冗余字段，便于查询）
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 办理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeUserId { get; set; }

    /// <summary>
    /// 办理人姓名（冗余：按 AssigneeUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? AssigneeUserName { get; set; } = string.Empty;

    /// <summary>
    /// 任务所有者 ID（选项 TaktUsers/options；DictValue=Id；转办前原办理人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 任务所有者姓名（冗余：按 OwnerUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? OwnerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 会签类型（字典 sys_flow_sign_type；1=或签 2=会签）
    /// </summary>
    public int? SignType { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 到期时间（范围查询-开始）
    /// </summary>
    public DateTime? DueDateStart { get; set; }

    /// <summary>
    /// 到期时间（范围查询-结束）
    /// </summary>
    public DateTime? DueDateEnd { get; set; }

    /// <summary>
    /// 认领时间（范围查询-开始）
    /// </summary>
    public DateTime? ClaimTimeStart { get; set; }

    /// <summary>
    /// 认领时间（范围查询-结束）
    /// </summary>
    public DateTime? ClaimTimeEnd { get; set; }

    /// <summary>
    /// 办结时间（范围查询-开始）
    /// </summary>
    public DateTime? CompletedAtStart { get; set; }

    /// <summary>
    /// 办结时间（范围查询-结束）
    /// </summary>
    public DateTime? CompletedAtEnd { get; set; }

    /// <summary>
    /// 是否加签任务（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsAddSign { get; set; }

    /// <summary>
    /// 加签记录 ID（选项 TaktFlowAddSigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AddSignId { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 多实例序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）
    /// </summary>
    public int? TaskStatus { get; set; }

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
// 创建FlowTask DTO
// ========================================

/// <summary>
/// 创建FlowTask DTO
/// </summary>
public class TaktFlowTaskCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InstanceId { get; set; }

    /// <summary>
    /// 任务定义键（设计器 nodeId；与实例 CurrentActivityId 一致）
    /// </summary>
    [Required(ErrorMessage = "任务定义键（设计器 nodeId；与实例 CurrentActivityId 一致）不能为空")]
    public string TaskDefinitionKey { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称（冗余字段，便于查询）
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 办理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssigneeUserId { get; set; }

    /// <summary>
    /// 办理人姓名（冗余：按 AssigneeUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? AssigneeUserName { get; set; } = string.Empty;

    /// <summary>
    /// 任务所有者 ID（选项 TaktUsers/options；DictValue=Id；转办前原办理人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 任务所有者姓名（冗余：按 OwnerUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? OwnerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 会签类型（字典 sys_flow_sign_type；1=或签 2=会签）
    /// </summary>
    public int SignType { get; set; } = 0;

    /// <summary>
    /// 优先级
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 认领时间
    /// </summary>
    public DateTime? ClaimTime { get; set; }

    /// <summary>
    /// 办结时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 是否加签任务（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsAddSign { get; set; } = 0;

    /// <summary>
    /// 加签记录 ID（选项 TaktFlowAddSigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AddSignId { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）
    /// </summary>
    public int TaskStatus { get; set; } = 0;

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
// 更新FlowTask DTO
// ========================================

/// <summary>
/// 更新FlowTask DTO
/// 继承 TaktFlowTaskCreateDto，添加 FlowTaskId 字段
/// </summary>
public class TaktFlowTaskUpdateDto : TaktFlowTaskCreateDto
{
    /// <summary>
    /// FlowTaskID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTaskId { get; set; }

}

// ========================================
// FlowTask 状态 DTO
// ========================================

/// <summary>
/// FlowTask 状态更新 DTO
/// </summary>
public class TaktFlowTaskStatusDto
{
    /// <summary>
    /// FlowTaskID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTaskId { get; set; }

    /// <summary>
    /// 任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）
    /// </summary>
    [Required(ErrorMessage = "任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）不能为空")]
    public int TaskStatus { get; set; } = 0;
}

// ========================================
// FlowTask 排序 DTO
// ========================================

/// <summary>
/// FlowTask 排序更新 DTO
/// </summary>
public class TaktFlowTaskSortDto
{
    /// <summary>
    /// FlowTaskID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTaskId { get; set; }

    /// <summary>
    /// 多实例序号
    /// </summary>
    [Required(ErrorMessage = "多实例序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// FlowTask 导入模板行 DTO
/// </summary>
public class TaktFlowTaskTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InstanceId { get; set; }

    /// <summary>
    /// 任务定义键（设计器 nodeId；与实例 CurrentActivityId 一致）
    /// </summary>
    public string? TaskDefinitionKey { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称（冗余字段，便于查询）
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 办理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeUserId { get; set; }

    /// <summary>
    /// 办理人姓名（冗余：按 AssigneeUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? AssigneeUserName { get; set; } = string.Empty;

    /// <summary>
    /// 任务所有者 ID（选项 TaktUsers/options；DictValue=Id；转办前原办理人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 任务所有者姓名（冗余：按 OwnerUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? OwnerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 会签类型（字典 sys_flow_sign_type；1=或签 2=会签）
    /// </summary>
    public int? SignType { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 认领时间
    /// </summary>
    public DateTime? ClaimTime { get; set; }

    /// <summary>
    /// 办结时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 是否加签任务（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsAddSign { get; set; }

    /// <summary>
    /// 加签记录 ID（选项 TaktFlowAddSigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AddSignId { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）
    /// </summary>
    public int? TaskStatus { get; set; }

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
/// FlowTask 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktFlowTaskImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InstanceId { get; set; }

    /// <summary>
    /// 任务定义键（设计器 nodeId；与实例 CurrentActivityId 一致）
    /// </summary>
    public string? TaskDefinitionKey { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称（冗余字段，便于查询）
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 办理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeUserId { get; set; }

    /// <summary>
    /// 办理人姓名（冗余：按 AssigneeUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? AssigneeUserName { get; set; } = string.Empty;

    /// <summary>
    /// 任务所有者 ID（选项 TaktUsers/options；DictValue=Id；转办前原办理人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 任务所有者姓名（冗余：按 OwnerUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? OwnerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 会签类型（字典 sys_flow_sign_type；1=或签 2=会签）
    /// </summary>
    public int? SignType { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 认领时间
    /// </summary>
    public DateTime? ClaimTime { get; set; }

    /// <summary>
    /// 办结时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 是否加签任务（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsAddSign { get; set; }

    /// <summary>
    /// 加签记录 ID（选项 TaktFlowAddSigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AddSignId { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）
    /// </summary>
    public int? TaskStatus { get; set; }

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
/// FlowTask 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFlowTaskExportDto
{
    /// <summary>
    /// FlowTaskID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTaskId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程实例 ID（选项 TaktFlowInstances/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InstanceId { get; set; }

    /// <summary>
    /// 任务定义键（设计器 nodeId；与实例 CurrentActivityId 一致）
    /// </summary>
    public string TaskDefinitionKey { get; set; } = string.Empty;

    /// <summary>
    /// 任务名称（冗余字段，便于查询）
    /// </summary>
    public string? TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 办理人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssigneeUserId { get; set; }

    /// <summary>
    /// 办理人姓名（冗余：按 AssigneeUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? AssigneeUserName { get; set; } = string.Empty;

    /// <summary>
    /// 任务所有者 ID（选项 TaktUsers/options；DictValue=Id；转办前原办理人）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? OwnerUserId { get; set; }

    /// <summary>
    /// 任务所有者姓名（冗余：按 OwnerUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? OwnerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 会签类型（字典 sys_flow_sign_type；1=或签 2=会签）
    /// </summary>
    public int SignType { get; set; } = 0;

    /// <summary>
    /// 优先级
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// 认领时间
    /// </summary>
    public DateTime? ClaimTime { get; set; }

    /// <summary>
    /// 办结时间
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// 是否加签任务（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsAddSign { get; set; } = 0;

    /// <summary>
    /// 加签记录 ID（选项 TaktFlowAddSigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AddSignId { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// 多实例序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 任务状态（字典 sys_flow_task_status；0=待办 1=已完成 2=已取消）
    /// </summary>
    public int TaskStatus { get; set; } = 0;

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
