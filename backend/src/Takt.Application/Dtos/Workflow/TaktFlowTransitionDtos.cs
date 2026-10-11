// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Workflow
// 文件名称：TaktFlowTransitionDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：FlowTransition 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFlowTransition 生成，请按需审阅）
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
// FlowTransition 响应 DTO
// ========================================

/// <summary>
/// 流程流转历史实体
/// 对应前端 TaktFlowTransitionDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktFlowTransitionDto : TaktCompanyDtoBase
{
    /// <summary>
    /// FlowTransitionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTransitionId { get; set; }

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InstanceId { get; set; }

    /// <summary>
    /// 流程实例 名称（填充字段）
    /// </summary>
    public string? InstanceName { get; set; }

    /// <summary>
    /// 节点 ID
    /// </summary>
    public string? ActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 节点名称
    /// </summary>
    public string? ActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 节点类型（如 userTask、start、end）
    /// </summary>
    public string? ActivityType { get; set; } = string.Empty;

    /// <summary>
    /// 源节点 ID
    /// </summary>
    public string? FromNodeId { get; set; } = string.Empty;

    /// <summary>
    /// 源节点名称
    /// </summary>
    public string? FromNodeName { get; set; } = string.Empty;

    /// <summary>
    /// 目标节点 ID
    /// </summary>
    public string? ToNodeId { get; set; } = string.Empty;

    /// <summary>
    /// 目标节点名称
    /// </summary>
    public string? ToNodeName { get; set; } = string.Empty;

    /// <summary>
    /// 操作人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TransitionUserId { get; set; }

    /// <summary>
    /// 操作人姓名
    /// </summary>
    public string? TransitionUserName { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime TransitionTime { get; set; }

    /// <summary>
    /// 历时毫秒
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DurationMs { get; set; }

    /// <summary>
    /// 操作意见
    /// </summary>
    public string? TransitionComment { get; set; } = string.Empty;

    /// <summary>
    /// 动作类型
    /// </summary>
    public TaktFlowActionType ActionType { get; set; }

    /// <summary>
    /// 所属流程实例
    /// （主表：TaktFlowInstance）
    /// </summary>
    public TaktFlowInstanceDto? Instance { get; set; }

}

// ========================================
// FlowTransition 查询 DTO
// ========================================

/// <summary>
/// FlowTransition 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFlowTransitionQueryDto : TaktPagedQuery
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InstanceId { get; set; }

    /// <summary>
    /// 节点 ID
    /// </summary>
    public string? ActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 节点名称
    /// </summary>
    public string? ActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 节点类型（如 userTask、start、end）
    /// </summary>
    public string? ActivityType { get; set; } = string.Empty;

    /// <summary>
    /// 源节点 ID
    /// </summary>
    public string? FromNodeId { get; set; } = string.Empty;

    /// <summary>
    /// 源节点名称
    /// </summary>
    public string? FromNodeName { get; set; } = string.Empty;

    /// <summary>
    /// 目标节点 ID
    /// </summary>
    public string? ToNodeId { get; set; } = string.Empty;

    /// <summary>
    /// 目标节点名称
    /// </summary>
    public string? ToNodeName { get; set; } = string.Empty;

    /// <summary>
    /// 操作人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TransitionUserId { get; set; }

    /// <summary>
    /// 操作人姓名
    /// </summary>
    public string? TransitionUserName { get; set; } = string.Empty;

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
    public DateTime? TransitionTimeStart { get; set; }

    /// <summary>
    /// 结束时间（范围查询-结束）
    /// </summary>
    public DateTime? TransitionTimeEnd { get; set; }

    /// <summary>
    /// 历时毫秒
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DurationMs { get; set; }

    /// <summary>
    /// 操作意见
    /// </summary>
    public string? TransitionComment { get; set; } = string.Empty;

    /// <summary>
    /// 动作类型
    /// </summary>
    public TaktFlowActionType? ActionType { get; set; }

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
// 创建FlowTransition DTO
// ========================================

/// <summary>
/// 创建FlowTransition DTO
/// </summary>
public class TaktFlowTransitionCreateDto
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InstanceId { get; set; }

    /// <summary>
    /// 节点 ID
    /// </summary>
    public string? ActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 节点名称
    /// </summary>
    public string? ActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 节点类型（如 userTask、start、end）
    /// </summary>
    public string? ActivityType { get; set; } = string.Empty;

    /// <summary>
    /// 源节点 ID
    /// </summary>
    public string? FromNodeId { get; set; } = string.Empty;

    /// <summary>
    /// 源节点名称
    /// </summary>
    public string? FromNodeName { get; set; } = string.Empty;

    /// <summary>
    /// 目标节点 ID
    /// </summary>
    public string? ToNodeId { get; set; } = string.Empty;

    /// <summary>
    /// 目标节点名称
    /// </summary>
    public string? ToNodeName { get; set; } = string.Empty;

    /// <summary>
    /// 操作人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TransitionUserId { get; set; }

    /// <summary>
    /// 操作人姓名
    /// </summary>
    public string? TransitionUserName { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime TransitionTime { get; set; }

    /// <summary>
    /// 历时毫秒
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DurationMs { get; set; }

    /// <summary>
    /// 操作意见
    /// </summary>
    public string? TransitionComment { get; set; } = string.Empty;

    /// <summary>
    /// 动作类型
    /// </summary>
    public TaktFlowActionType ActionType { get; set; }

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
// 更新FlowTransition DTO
// ========================================

/// <summary>
/// 更新FlowTransition DTO
/// 继承 TaktFlowTransitionCreateDto，添加 FlowTransitionId 字段
/// </summary>
public class TaktFlowTransitionUpdateDto : TaktFlowTransitionCreateDto
{
    /// <summary>
    /// FlowTransitionID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTransitionId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// FlowTransition 导入模板行 DTO
/// </summary>
public class TaktFlowTransitionTemplateDto
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InstanceId { get; set; }

    /// <summary>
    /// 节点 ID
    /// </summary>
    public string? ActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 节点名称
    /// </summary>
    public string? ActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 节点类型（如 userTask、start、end）
    /// </summary>
    public string? ActivityType { get; set; } = string.Empty;

    /// <summary>
    /// 源节点 ID
    /// </summary>
    public string? FromNodeId { get; set; } = string.Empty;

    /// <summary>
    /// 源节点名称
    /// </summary>
    public string? FromNodeName { get; set; } = string.Empty;

    /// <summary>
    /// 目标节点 ID
    /// </summary>
    public string? ToNodeId { get; set; } = string.Empty;

    /// <summary>
    /// 目标节点名称
    /// </summary>
    public string? ToNodeName { get; set; } = string.Empty;

    /// <summary>
    /// 操作人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TransitionUserId { get; set; }

    /// <summary>
    /// 操作人姓名
    /// </summary>
    public string? TransitionUserName { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? TransitionTime { get; set; }

    /// <summary>
    /// 历时毫秒
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DurationMs { get; set; }

    /// <summary>
    /// 操作意见
    /// </summary>
    public string? TransitionComment { get; set; } = string.Empty;

    /// <summary>
    /// 动作类型
    /// </summary>
    public TaktFlowActionType? ActionType { get; set; }

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
/// FlowTransition 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktFlowTransitionImportDto
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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InstanceId { get; set; }

    /// <summary>
    /// 节点 ID
    /// </summary>
    public string? ActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 节点名称
    /// </summary>
    public string? ActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 节点类型（如 userTask、start、end）
    /// </summary>
    public string? ActivityType { get; set; } = string.Empty;

    /// <summary>
    /// 源节点 ID
    /// </summary>
    public string? FromNodeId { get; set; } = string.Empty;

    /// <summary>
    /// 源节点名称
    /// </summary>
    public string? FromNodeName { get; set; } = string.Empty;

    /// <summary>
    /// 目标节点 ID
    /// </summary>
    public string? ToNodeId { get; set; } = string.Empty;

    /// <summary>
    /// 目标节点名称
    /// </summary>
    public string? ToNodeName { get; set; } = string.Empty;

    /// <summary>
    /// 操作人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TransitionUserId { get; set; }

    /// <summary>
    /// 操作人姓名
    /// </summary>
    public string? TransitionUserName { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? TransitionTime { get; set; }

    /// <summary>
    /// 历时毫秒
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DurationMs { get; set; }

    /// <summary>
    /// 操作意见
    /// </summary>
    public string? TransitionComment { get; set; } = string.Empty;

    /// <summary>
    /// 动作类型
    /// </summary>
    public TaktFlowActionType? ActionType { get; set; }

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
/// FlowTransition 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFlowTransitionExportDto
{
    /// <summary>
    /// FlowTransitionID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowTransitionId { get; set; }

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
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InstanceId { get; set; }

    /// <summary>
    /// 节点 ID
    /// </summary>
    public string? ActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 节点名称
    /// </summary>
    public string? ActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 节点类型（如 userTask、start、end）
    /// </summary>
    public string? ActivityType { get; set; } = string.Empty;

    /// <summary>
    /// 源节点 ID
    /// </summary>
    public string? FromNodeId { get; set; } = string.Empty;

    /// <summary>
    /// 源节点名称
    /// </summary>
    public string? FromNodeName { get; set; } = string.Empty;

    /// <summary>
    /// 目标节点 ID
    /// </summary>
    public string? ToNodeId { get; set; } = string.Empty;

    /// <summary>
    /// 目标节点名称
    /// </summary>
    public string? ToNodeName { get; set; } = string.Empty;

    /// <summary>
    /// 操作人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TransitionUserId { get; set; }

    /// <summary>
    /// 操作人姓名
    /// </summary>
    public string? TransitionUserName { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime TransitionTime { get; set; }

    /// <summary>
    /// 历时毫秒
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DurationMs { get; set; }

    /// <summary>
    /// 操作意见
    /// </summary>
    public string? TransitionComment { get; set; } = string.Empty;

    /// <summary>
    /// 动作类型
    /// </summary>
    public TaktFlowActionType ActionType { get; set; }

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
