// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Workflow
// 文件名称：TaktFlowInstanceDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：FlowInstance 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFlowInstance 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Workflow;

// ========================================
// FlowInstance 响应 DTO
// ========================================

/// <summary>
/// 流程实例实体
/// 对应前端 TaktFlowInstanceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktFlowInstanceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// FlowInstanceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }

    /// <summary>
    /// 实例编码（对外业务单号）
    /// </summary>
    public string InstanceCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程定义 ID（TaktFlowScheme Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProcessDefinitionId { get; set; }

    /// <summary>
    /// 流程定义 名称（填充字段）
    /// </summary>
    public string? ProcessDefinitionName { get; set; }

    /// <summary>
    /// 流程键（冗余）
    /// </summary>
    public string ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称（冗余）
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 发起时锁定的定义版本号
    /// </summary>
    public int DefinitionVersion { get; set; } = 0;

    /// <summary>
    /// 申请标题
    /// </summary>
    public string? ProcessTitle { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点 ID（设计器 nodeId）
    /// </summary>
    public string? CurrentActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点名称
    /// </summary>
    public string? CurrentActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 发起人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StartUserId { get; set; }

    /// <summary>
    /// 发起人姓名
    /// </summary>
    public string? StartUserName { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 历时毫秒
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DurationMs { get; set; }

    /// <summary>
    /// 业务主键（关联业务单据 Id 等）
    /// </summary>
    public string? BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 业务类型（由业务模块约定，用于回写）
    /// </summary>
    public string? BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 父流程实例 ID（子流程场景）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SuperInstanceId { get; set; }

    /// <summary>
    /// 父流程实例 名称（填充字段）
    /// </summary>
    public string? SuperInstanceName { get; set; }

    /// <summary>
    /// 终止原因
    /// </summary>
    public string? DeleteReason { get; set; } = string.Empty;

    /// <summary>
    /// 表单数据 JSON（前端 frmData；细粒度字段可同步至 TaktFlowVariable）
    /// </summary>
    public string? FrmData { get; set; } = string.Empty;

    /// <summary>
    /// 关联表单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FormId { get; set; }

    /// <summary>
    /// 关联表单 名称（填充字段）
    /// </summary>
    public string? FormName { get; set; }

    /// <summary>
    /// 关联表单编码
    /// </summary>
    public string? FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程设计快照（启动时复制，避免定义变更影响在途实例）
    /// </summary>
    public string? ProcessContentSnapshot { get; set; } = string.Empty;

    /// <summary>
    /// 实例状态
    /// </summary>
    public TaktFlowInstanceStatus InstanceStatus { get; set; }

    /// <summary>
    /// 流程定义
    /// （主表：TaktFlowScheme）
    /// </summary>
    public TaktFlowSchemeDto? ProcessDefinition { get; set; }

    /// <summary>
    /// 待办任务
    /// （子表：TaktFlowTask）
    /// </summary>
    public List<TaktFlowTaskDto>? Tasks { get; set; }

    /// <summary>
    /// 流转历史
    /// （子表：TaktFlowTransition）
    /// </summary>
    public List<TaktFlowTransitionDto>? HistoricActivities { get; set; }

    /// <summary>
    /// 流程变量
    /// （子表：TaktFlowVariable）
    /// </summary>
    public List<TaktFlowVariableDto>? Variables { get; set; }

    /// <summary>
    /// 加签记录
    /// （子表：TaktFlowAddSign）
    /// </summary>
    public List<TaktFlowAddSignDto>? AddSigns { get; set; }

}

// ========================================
// FlowInstance 查询 DTO
// ========================================

/// <summary>
/// FlowInstance 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFlowInstanceQueryDto : TaktPagedQuery
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
    /// 实例编码（对外业务单号）
    /// </summary>
    public string? InstanceCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程定义 ID（TaktFlowScheme Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProcessDefinitionId { get; set; }

    /// <summary>
    /// 流程键（冗余）
    /// </summary>
    public string? ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称（冗余）
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 发起时锁定的定义版本号
    /// </summary>
    public int? DefinitionVersion { get; set; }

    /// <summary>
    /// 申请标题
    /// </summary>
    public string? ProcessTitle { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点 ID（设计器 nodeId）
    /// </summary>
    public string? CurrentActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点名称
    /// </summary>
    public string? CurrentActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 发起人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StartUserId { get; set; }

    /// <summary>
    /// 发起人姓名
    /// </summary>
    public string? StartUserName { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间（范围查询-开始）
    /// </summary>
    public DateTime? StartTimeStart { get; set; }

    /// <summary>
    /// 开始时间（范围查询-结束）
    /// </summary>
    public DateTime? StartTimeEnd { get; set; }

    /// <summary>
    /// 结束时间（范围查询-开始）
    /// </summary>
    public DateTime? EndTimeStart { get; set; }

    /// <summary>
    /// 结束时间（范围查询-结束）
    /// </summary>
    public DateTime? EndTimeEnd { get; set; }

    /// <summary>
    /// 历时毫秒
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DurationMs { get; set; }

    /// <summary>
    /// 业务主键（关联业务单据 Id 等）
    /// </summary>
    public string? BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 业务类型（由业务模块约定，用于回写）
    /// </summary>
    public string? BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 父流程实例 ID（子流程场景）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SuperInstanceId { get; set; }

    /// <summary>
    /// 终止原因
    /// </summary>
    public string? DeleteReason { get; set; } = string.Empty;

    /// <summary>
    /// 表单数据 JSON（前端 frmData；细粒度字段可同步至 TaktFlowVariable）
    /// </summary>
    public string? FrmData { get; set; } = string.Empty;

    /// <summary>
    /// 关联表单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FormId { get; set; }

    /// <summary>
    /// 关联表单编码
    /// </summary>
    public string? FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程设计快照（启动时复制，避免定义变更影响在途实例）
    /// </summary>
    public string? ProcessContentSnapshot { get; set; } = string.Empty;

    /// <summary>
    /// 实例状态
    /// </summary>
    public TaktFlowInstanceStatus? InstanceStatus { get; set; }

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
// 创建FlowInstance DTO
// ========================================

/// <summary>
/// 创建FlowInstance DTO
/// </summary>
public class TaktFlowInstanceCreateDto
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
    /// 实例编码（对外业务单号）
    /// </summary>
    [Required(ErrorMessage = "实例编码（对外业务单号）不能为空")]
    public string InstanceCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程定义 ID（TaktFlowScheme Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProcessDefinitionId { get; set; }

    /// <summary>
    /// 流程键（冗余）
    /// </summary>
    [Required(ErrorMessage = "流程键（冗余）不能为空")]
    public string ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称（冗余）
    /// </summary>
    [Required(ErrorMessage = "流程名称（冗余）不能为空")]
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 发起时锁定的定义版本号
    /// </summary>
    public int DefinitionVersion { get; set; } = 0;

    /// <summary>
    /// 申请标题
    /// </summary>
    public string? ProcessTitle { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点 ID（设计器 nodeId）
    /// </summary>
    public string? CurrentActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点名称
    /// </summary>
    public string? CurrentActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 发起人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StartUserId { get; set; }

    /// <summary>
    /// 发起人姓名
    /// </summary>
    public string? StartUserName { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 历时毫秒
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DurationMs { get; set; }

    /// <summary>
    /// 业务主键（关联业务单据 Id 等）
    /// </summary>
    public string? BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 业务类型（由业务模块约定，用于回写）
    /// </summary>
    public string? BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 父流程实例 ID（子流程场景）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SuperInstanceId { get; set; }

    /// <summary>
    /// 终止原因
    /// </summary>
    public string? DeleteReason { get; set; } = string.Empty;

    /// <summary>
    /// 表单数据 JSON（前端 frmData；细粒度字段可同步至 TaktFlowVariable）
    /// </summary>
    public string? FrmData { get; set; } = string.Empty;

    /// <summary>
    /// 关联表单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FormId { get; set; }

    /// <summary>
    /// 关联表单编码
    /// </summary>
    public string? FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程设计快照（启动时复制，避免定义变更影响在途实例）
    /// </summary>
    public string? ProcessContentSnapshot { get; set; } = string.Empty;

    /// <summary>
    /// 实例状态
    /// </summary>
    public TaktFlowInstanceStatus InstanceStatus { get; set; }

    /// <summary>
    /// 待办任务（子表，级联保存）
    /// </summary>
    public List<TaktFlowTaskCreateDto>? Tasks { get; set; }

    /// <summary>
    /// 流转历史（子表，级联保存）
    /// </summary>
    public List<TaktFlowTransitionCreateDto>? HistoricActivities { get; set; }

    /// <summary>
    /// 流程变量（子表，级联保存）
    /// </summary>
    public List<TaktFlowVariableCreateDto>? Variables { get; set; }

    /// <summary>
    /// 加签记录（子表，级联保存）
    /// </summary>
    public List<TaktFlowAddSignCreateDto>? AddSigns { get; set; }

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
// 更新FlowInstance DTO
// ========================================

/// <summary>
/// 更新FlowInstance DTO
/// 继承 TaktFlowInstanceCreateDto，添加 FlowInstanceId 字段
/// </summary>
public class TaktFlowInstanceUpdateDto : TaktFlowInstanceCreateDto
{
    /// <summary>
    /// FlowInstanceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }

    /// <summary>
    /// 待办任务（子表，级联保存）
    /// </summary>
    public new List<TaktFlowTaskUpdateDto>? Tasks { get; set; }

    /// <summary>
    /// 流转历史（子表，级联保存）
    /// </summary>
    public new List<TaktFlowTransitionUpdateDto>? HistoricActivities { get; set; }

    /// <summary>
    /// 流程变量（子表，级联保存）
    /// </summary>
    public new List<TaktFlowVariableUpdateDto>? Variables { get; set; }

    /// <summary>
    /// 加签记录（子表，级联保存）
    /// </summary>
    public new List<TaktFlowAddSignUpdateDto>? AddSigns { get; set; }

}

// ========================================
// FlowInstance 状态 DTO
// ========================================

/// <summary>
/// FlowInstance 状态更新 DTO
/// </summary>
public class TaktFlowInstanceStatusDto
{
    /// <summary>
    /// FlowInstanceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }

    /// <summary>
    /// 实例状态
    /// </summary>
    [Required(ErrorMessage = "实例状态不能为空")]
    public TaktFlowInstanceStatus InstanceStatus { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// FlowInstance 导入模板行 DTO
/// </summary>
public class TaktFlowInstanceTemplateDto
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
    /// 实例编码（对外业务单号）
    /// </summary>
    public string? InstanceCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程定义 ID（TaktFlowScheme Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProcessDefinitionId { get; set; }

    /// <summary>
    /// 流程键（冗余）
    /// </summary>
    public string? ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称（冗余）
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 发起时锁定的定义版本号
    /// </summary>
    public int? DefinitionVersion { get; set; }

    /// <summary>
    /// 申请标题
    /// </summary>
    public string? ProcessTitle { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点 ID（设计器 nodeId）
    /// </summary>
    public string? CurrentActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点名称
    /// </summary>
    public string? CurrentActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 发起人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StartUserId { get; set; }

    /// <summary>
    /// 发起人姓名
    /// </summary>
    public string? StartUserName { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 历时毫秒
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DurationMs { get; set; }

    /// <summary>
    /// 业务主键（关联业务单据 Id 等）
    /// </summary>
    public string? BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 业务类型（由业务模块约定，用于回写）
    /// </summary>
    public string? BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 父流程实例 ID（子流程场景）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SuperInstanceId { get; set; }

    /// <summary>
    /// 终止原因
    /// </summary>
    public string? DeleteReason { get; set; } = string.Empty;

    /// <summary>
    /// 表单数据 JSON（前端 frmData；细粒度字段可同步至 TaktFlowVariable）
    /// </summary>
    public string? FrmData { get; set; } = string.Empty;

    /// <summary>
    /// 关联表单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FormId { get; set; }

    /// <summary>
    /// 关联表单编码
    /// </summary>
    public string? FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程设计快照（启动时复制，避免定义变更影响在途实例）
    /// </summary>
    public string? ProcessContentSnapshot { get; set; } = string.Empty;

    /// <summary>
    /// 实例状态
    /// </summary>
    public TaktFlowInstanceStatus? InstanceStatus { get; set; }

    /// <summary>
    /// 待办任务（子表，级联保存）
    /// </summary>
    public List<TaktFlowTaskCreateDto>? Tasks { get; set; }

    /// <summary>
    /// 流转历史（子表，级联保存）
    /// </summary>
    public List<TaktFlowTransitionCreateDto>? HistoricActivities { get; set; }

    /// <summary>
    /// 流程变量（子表，级联保存）
    /// </summary>
    public List<TaktFlowVariableCreateDto>? Variables { get; set; }

    /// <summary>
    /// 加签记录（子表，级联保存）
    /// </summary>
    public List<TaktFlowAddSignCreateDto>? AddSigns { get; set; }

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
/// FlowInstance 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktFlowInstanceImportDto
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
    /// 实例编码（对外业务单号）
    /// </summary>
    public string? InstanceCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程定义 ID（TaktFlowScheme Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProcessDefinitionId { get; set; }

    /// <summary>
    /// 流程键（冗余）
    /// </summary>
    public string? ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称（冗余）
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 发起时锁定的定义版本号
    /// </summary>
    public int? DefinitionVersion { get; set; }

    /// <summary>
    /// 申请标题
    /// </summary>
    public string? ProcessTitle { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点 ID（设计器 nodeId）
    /// </summary>
    public string? CurrentActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点名称
    /// </summary>
    public string? CurrentActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 发起人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StartUserId { get; set; }

    /// <summary>
    /// 发起人姓名
    /// </summary>
    public string? StartUserName { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 历时毫秒
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DurationMs { get; set; }

    /// <summary>
    /// 业务主键（关联业务单据 Id 等）
    /// </summary>
    public string? BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 业务类型（由业务模块约定，用于回写）
    /// </summary>
    public string? BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 父流程实例 ID（子流程场景）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SuperInstanceId { get; set; }

    /// <summary>
    /// 终止原因
    /// </summary>
    public string? DeleteReason { get; set; } = string.Empty;

    /// <summary>
    /// 表单数据 JSON（前端 frmData；细粒度字段可同步至 TaktFlowVariable）
    /// </summary>
    public string? FrmData { get; set; } = string.Empty;

    /// <summary>
    /// 关联表单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FormId { get; set; }

    /// <summary>
    /// 关联表单编码
    /// </summary>
    public string? FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程设计快照（启动时复制，避免定义变更影响在途实例）
    /// </summary>
    public string? ProcessContentSnapshot { get; set; } = string.Empty;

    /// <summary>
    /// 实例状态
    /// </summary>
    public TaktFlowInstanceStatus? InstanceStatus { get; set; }

    /// <summary>
    /// 待办任务（子表，级联保存）
    /// </summary>
    public List<TaktFlowTaskCreateDto>? Tasks { get; set; }

    /// <summary>
    /// 流转历史（子表，级联保存）
    /// </summary>
    public List<TaktFlowTransitionCreateDto>? HistoricActivities { get; set; }

    /// <summary>
    /// 流程变量（子表，级联保存）
    /// </summary>
    public List<TaktFlowVariableCreateDto>? Variables { get; set; }

    /// <summary>
    /// 加签记录（子表，级联保存）
    /// </summary>
    public List<TaktFlowAddSignCreateDto>? AddSigns { get; set; }

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
/// FlowInstance 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFlowInstanceExportDto
{
    /// <summary>
    /// FlowInstanceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowInstanceId { get; set; }

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
    /// 实例编码（对外业务单号）
    /// </summary>
    public string InstanceCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程定义 ID（TaktFlowScheme Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProcessDefinitionId { get; set; }

    /// <summary>
    /// 流程键（冗余）
    /// </summary>
    public string ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称（冗余）
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 发起时锁定的定义版本号
    /// </summary>
    public int DefinitionVersion { get; set; } = 0;

    /// <summary>
    /// 申请标题
    /// </summary>
    public string? ProcessTitle { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点 ID（设计器 nodeId）
    /// </summary>
    public string? CurrentActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点名称
    /// </summary>
    public string? CurrentActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 发起人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StartUserId { get; set; }

    /// <summary>
    /// 发起人姓名
    /// </summary>
    public string? StartUserName { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 历时毫秒
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DurationMs { get; set; }

    /// <summary>
    /// 业务主键（关联业务单据 Id 等）
    /// </summary>
    public string? BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 业务类型（由业务模块约定，用于回写）
    /// </summary>
    public string? BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 父流程实例 ID（子流程场景）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SuperInstanceId { get; set; }

    /// <summary>
    /// 终止原因
    /// </summary>
    public string? DeleteReason { get; set; } = string.Empty;

    /// <summary>
    /// 表单数据 JSON（前端 frmData；细粒度字段可同步至 TaktFlowVariable）
    /// </summary>
    public string? FrmData { get; set; } = string.Empty;

    /// <summary>
    /// 关联表单 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FormId { get; set; }

    /// <summary>
    /// 关联表单编码
    /// </summary>
    public string? FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程设计快照（启动时复制，避免定义变更影响在途实例）
    /// </summary>
    public string? ProcessContentSnapshot { get; set; } = string.Empty;

    /// <summary>
    /// 实例状态
    /// </summary>
    public TaktFlowInstanceStatus InstanceStatus { get; set; }

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
