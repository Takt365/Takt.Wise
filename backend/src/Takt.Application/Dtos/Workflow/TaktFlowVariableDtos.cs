// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Workflow
// 文件名称：TaktFlowVariableDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：FlowVariable 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFlowVariable 生成，请按需审阅）
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
// FlowVariable 响应 DTO
// ========================================

/// <summary>
/// 流程变量实体
/// 对应前端 TaktFlowVariableDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktFlowVariableDto : TaktCompanyDtoBase
{
    /// <summary>
    /// FlowVariableID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowVariableId { get; set; }

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
    /// 任务 ID（任务级变量时填写）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TaskId { get; set; }

    /// <summary>
    /// 任务 名称（填充字段）
    /// </summary>
    public string? TaskName { get; set; }

    /// <summary>
    /// 变量名
    /// </summary>
    public string VariableName { get; set; } = string.Empty;

    /// <summary>
    /// 变量类型
    /// </summary>
    public TaktFlowVariableType VariableType { get; set; }

    /// <summary>
    /// 文本值（JSON 变量存此列）
    /// </summary>
    public string? TextValue { get; set; } = string.Empty;

    /// <summary>
    /// 长整型值
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? LongValue { get; set; }

    /// <summary>
    /// 双精度值
    /// </summary>
    public double? DoubleValue { get; set; }

    /// <summary>
    /// 所属流程实例
    /// （主表：TaktFlowInstance）
    /// </summary>
    public TaktFlowInstanceDto? Instance { get; set; }

}

// ========================================
// FlowVariable 查询 DTO
// ========================================

/// <summary>
/// FlowVariable 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFlowVariableQueryDto : TaktPagedQuery
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
    /// 任务 ID（任务级变量时填写）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TaskId { get; set; }

    /// <summary>
    /// 变量名
    /// </summary>
    public string? VariableName { get; set; } = string.Empty;

    /// <summary>
    /// 变量类型
    /// </summary>
    public TaktFlowVariableType? VariableType { get; set; }

    /// <summary>
    /// 文本值（JSON 变量存此列）
    /// </summary>
    public string? TextValue { get; set; } = string.Empty;

    /// <summary>
    /// 长整型值
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? LongValue { get; set; }

    /// <summary>
    /// 双精度值
    /// </summary>
    public double? DoubleValue { get; set; }

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
// 创建FlowVariable DTO
// ========================================

/// <summary>
/// 创建FlowVariable DTO
/// </summary>
public class TaktFlowVariableCreateDto
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
    /// 任务 ID（任务级变量时填写）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TaskId { get; set; }

    /// <summary>
    /// 变量名
    /// </summary>
    [Required(ErrorMessage = "变量名不能为空")]
    public string VariableName { get; set; } = string.Empty;

    /// <summary>
    /// 变量类型
    /// </summary>
    public TaktFlowVariableType VariableType { get; set; }

    /// <summary>
    /// 文本值（JSON 变量存此列）
    /// </summary>
    public string? TextValue { get; set; } = string.Empty;

    /// <summary>
    /// 长整型值
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? LongValue { get; set; }

    /// <summary>
    /// 双精度值
    /// </summary>
    public double? DoubleValue { get; set; }

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
// 更新FlowVariable DTO
// ========================================

/// <summary>
/// 更新FlowVariable DTO
/// 继承 TaktFlowVariableCreateDto，添加 FlowVariableId 字段
/// </summary>
public class TaktFlowVariableUpdateDto : TaktFlowVariableCreateDto
{
    /// <summary>
    /// FlowVariableID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowVariableId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// FlowVariable 导入模板行 DTO
/// </summary>
public class TaktFlowVariableTemplateDto
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
    /// 任务 ID（任务级变量时填写）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TaskId { get; set; }

    /// <summary>
    /// 变量名
    /// </summary>
    public string? VariableName { get; set; } = string.Empty;

    /// <summary>
    /// 变量类型
    /// </summary>
    public TaktFlowVariableType? VariableType { get; set; }

    /// <summary>
    /// 文本值（JSON 变量存此列）
    /// </summary>
    public string? TextValue { get; set; } = string.Empty;

    /// <summary>
    /// 长整型值
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? LongValue { get; set; }

    /// <summary>
    /// 双精度值
    /// </summary>
    public double? DoubleValue { get; set; }

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
/// FlowVariable 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktFlowVariableImportDto
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
    /// 任务 ID（任务级变量时填写）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TaskId { get; set; }

    /// <summary>
    /// 变量名
    /// </summary>
    public string? VariableName { get; set; } = string.Empty;

    /// <summary>
    /// 变量类型
    /// </summary>
    public TaktFlowVariableType? VariableType { get; set; }

    /// <summary>
    /// 文本值（JSON 变量存此列）
    /// </summary>
    public string? TextValue { get; set; } = string.Empty;

    /// <summary>
    /// 长整型值
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? LongValue { get; set; }

    /// <summary>
    /// 双精度值
    /// </summary>
    public double? DoubleValue { get; set; }

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
/// FlowVariable 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFlowVariableExportDto
{
    /// <summary>
    /// FlowVariableID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowVariableId { get; set; }

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
    /// 任务 ID（任务级变量时填写）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TaskId { get; set; }

    /// <summary>
    /// 变量名
    /// </summary>
    public string VariableName { get; set; } = string.Empty;

    /// <summary>
    /// 变量类型
    /// </summary>
    public TaktFlowVariableType VariableType { get; set; }

    /// <summary>
    /// 文本值（JSON 变量存此列）
    /// </summary>
    public string? TextValue { get; set; } = string.Empty;

    /// <summary>
    /// 长整型值
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? LongValue { get; set; }

    /// <summary>
    /// 双精度值
    /// </summary>
    public double? DoubleValue { get; set; }

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
