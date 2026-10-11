// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Workflow
// 文件名称：TaktFlowSchemeDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：FlowScheme 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFlowScheme 生成，请按需审阅）
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
// FlowScheme 响应 DTO
// ========================================

/// <summary>
/// 流程定义实体（前端流程方案 FlowScheme）
/// 对应前端 TaktFlowSchemeDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktFlowSchemeDto : TaktCompanyDtoBase
{
    /// <summary>
    /// FlowSchemeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowSchemeId { get; set; }

    /// <summary>
    /// 流程键（公司内业务唯一标识，如 leave）
    /// </summary>
    public string ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 定义版本号（同流程键可多版本）
    /// </summary>
    public int DefinitionVersion { get; set; } = 0;

    /// <summary>
    /// 版本标签（如 v1.0.0）
    /// </summary>
    public string ProcessVersion { get; set; } = string.Empty;

    /// <summary>
    /// 是否当前最新版（同键仅一条为 1）
    /// </summary>
    public int IsLatest { get; set; } = 0;

    /// <summary>
    /// 流程分类
    /// </summary>
    public int ProcessCategory { get; set; } = 0;

    /// <summary>
    /// 流程说明
    /// </summary>
    public string? ProcessDescription { get; set; } = string.Empty;

    /// <summary>
    /// 挂起状态（1 激活，2 挂起）
    /// </summary>
    public int SuspensionState { get; set; } = 0;

    /// <summary>
    /// 流程设计 JSON（节点、网关、条件、审批人配置）
    /// </summary>
    public string? ProcessContent { get; set; } = string.Empty;

    /// <summary>
    /// 部署批次号
    /// </summary>
    public string? DeploymentId { get; set; } = string.Empty;

    /// <summary>
    /// 部署批次号
    /// </summary>
    public string? DeploymentName { get; set; }

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
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 发布状态
    /// </summary>
    public int ProcessStatus { get; set; } = 0;

    /// <summary>
    /// 关联表单
    /// （主表：TaktFlowForm）
    /// </summary>
    public TaktFlowFormDto? Form { get; set; }

}

// ========================================
// FlowScheme 查询 DTO
// ========================================

/// <summary>
/// FlowScheme 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFlowSchemeQueryDto : TaktPagedQuery
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
    /// 流程键（公司内业务唯一标识，如 leave）
    /// </summary>
    public string? ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 定义版本号（同流程键可多版本）
    /// </summary>
    public int? DefinitionVersion { get; set; }

    /// <summary>
    /// 版本标签（如 v1.0.0）
    /// </summary>
    public string? ProcessVersion { get; set; } = string.Empty;

    /// <summary>
    /// 是否当前最新版（同键仅一条为 1）
    /// </summary>
    public int? IsLatest { get; set; }

    /// <summary>
    /// 流程分类
    /// </summary>
    public int? ProcessCategory { get; set; }

    /// <summary>
    /// 流程说明
    /// </summary>
    public string? ProcessDescription { get; set; } = string.Empty;

    /// <summary>
    /// 挂起状态（1 激活，2 挂起）
    /// </summary>
    public int? SuspensionState { get; set; }

    /// <summary>
    /// 流程设计 JSON（节点、网关、条件、审批人配置）
    /// </summary>
    public string? ProcessContent { get; set; } = string.Empty;

    /// <summary>
    /// 部署批次号
    /// </summary>
    public string? DeploymentId { get; set; } = string.Empty;

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
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 发布状态
    /// </summary>
    public int? ProcessStatus { get; set; }

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
// 创建FlowScheme DTO
// ========================================

/// <summary>
/// 创建FlowScheme DTO
/// </summary>
public class TaktFlowSchemeCreateDto
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
    /// 流程键（公司内业务唯一标识，如 leave）
    /// </summary>
    [Required(ErrorMessage = "流程键（公司内业务唯一标识，如 leave）不能为空")]
    public string ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称
    /// </summary>
    [Required(ErrorMessage = "流程名称不能为空")]
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 定义版本号（同流程键可多版本）
    /// </summary>
    public int DefinitionVersion { get; set; } = 0;

    /// <summary>
    /// 版本标签（如 v1.0.0）
    /// </summary>
    [Required(ErrorMessage = "版本标签（如 v1.0.0）不能为空")]
    public string ProcessVersion { get; set; } = string.Empty;

    /// <summary>
    /// 是否当前最新版（同键仅一条为 1）
    /// </summary>
    public int IsLatest { get; set; } = 0;

    /// <summary>
    /// 流程分类
    /// </summary>
    public int ProcessCategory { get; set; } = 0;

    /// <summary>
    /// 流程说明
    /// </summary>
    public string? ProcessDescription { get; set; } = string.Empty;

    /// <summary>
    /// 挂起状态（1 激活，2 挂起）
    /// </summary>
    public int SuspensionState { get; set; } = 0;

    /// <summary>
    /// 流程设计 JSON（节点、网关、条件、审批人配置）
    /// </summary>
    public string? ProcessContent { get; set; } = string.Empty;

    /// <summary>
    /// 部署批次号
    /// </summary>
    public string? DeploymentId { get; set; } = string.Empty;

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
    /// 发布状态
    /// </summary>
    public int ProcessStatus { get; set; } = 0;

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
// 更新FlowScheme DTO
// ========================================

/// <summary>
/// 更新FlowScheme DTO
/// 继承 TaktFlowSchemeCreateDto，添加 FlowSchemeId 字段
/// </summary>
public class TaktFlowSchemeUpdateDto : TaktFlowSchemeCreateDto
{
    /// <summary>
    /// FlowSchemeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowSchemeId { get; set; }

}

// ========================================
// FlowScheme 状态 DTO
// ========================================

/// <summary>
/// FlowScheme 状态更新 DTO
/// </summary>
public class TaktFlowSchemeStatusDto
{
    /// <summary>
    /// FlowSchemeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowSchemeId { get; set; }

    /// <summary>
    /// 发布状态
    /// </summary>
    [Required(ErrorMessage = "发布状态不能为空")]
    public int ProcessStatus { get; set; } = 0;
}

// ========================================
// FlowScheme 排序 DTO
// ========================================

/// <summary>
/// FlowScheme 排序更新 DTO
/// </summary>
public class TaktFlowSchemeSortDto
{
    /// <summary>
    /// FlowSchemeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowSchemeId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// FlowScheme 导入模板行 DTO
/// </summary>
public class TaktFlowSchemeTemplateDto
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
    /// 流程键（公司内业务唯一标识，如 leave）
    /// </summary>
    public string? ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 定义版本号（同流程键可多版本）
    /// </summary>
    public int? DefinitionVersion { get; set; }

    /// <summary>
    /// 版本标签（如 v1.0.0）
    /// </summary>
    public string? ProcessVersion { get; set; } = string.Empty;

    /// <summary>
    /// 是否当前最新版（同键仅一条为 1）
    /// </summary>
    public int? IsLatest { get; set; }

    /// <summary>
    /// 流程分类
    /// </summary>
    public int? ProcessCategory { get; set; }

    /// <summary>
    /// 流程说明
    /// </summary>
    public string? ProcessDescription { get; set; } = string.Empty;

    /// <summary>
    /// 挂起状态（1 激活，2 挂起）
    /// </summary>
    public int? SuspensionState { get; set; }

    /// <summary>
    /// 流程设计 JSON（节点、网关、条件、审批人配置）
    /// </summary>
    public string? ProcessContent { get; set; } = string.Empty;

    /// <summary>
    /// 部署批次号
    /// </summary>
    public string? DeploymentId { get; set; } = string.Empty;

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
    /// 发布状态
    /// </summary>
    public int? ProcessStatus { get; set; }

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
/// FlowScheme 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktFlowSchemeImportDto
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
    /// 流程键（公司内业务唯一标识，如 leave）
    /// </summary>
    public string? ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称
    /// </summary>
    public string? ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 定义版本号（同流程键可多版本）
    /// </summary>
    public int? DefinitionVersion { get; set; }

    /// <summary>
    /// 版本标签（如 v1.0.0）
    /// </summary>
    public string? ProcessVersion { get; set; } = string.Empty;

    /// <summary>
    /// 是否当前最新版（同键仅一条为 1）
    /// </summary>
    public int? IsLatest { get; set; }

    /// <summary>
    /// 流程分类
    /// </summary>
    public int? ProcessCategory { get; set; }

    /// <summary>
    /// 流程说明
    /// </summary>
    public string? ProcessDescription { get; set; } = string.Empty;

    /// <summary>
    /// 挂起状态（1 激活，2 挂起）
    /// </summary>
    public int? SuspensionState { get; set; }

    /// <summary>
    /// 流程设计 JSON（节点、网关、条件、审批人配置）
    /// </summary>
    public string? ProcessContent { get; set; } = string.Empty;

    /// <summary>
    /// 部署批次号
    /// </summary>
    public string? DeploymentId { get; set; } = string.Empty;

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
    /// 发布状态
    /// </summary>
    public int? ProcessStatus { get; set; }

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
/// FlowScheme 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFlowSchemeExportDto
{
    /// <summary>
    /// FlowSchemeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowSchemeId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 流程键（公司内业务唯一标识，如 leave）
    /// </summary>
    public string ProcessKey { get; set; } = string.Empty;

    /// <summary>
    /// 流程名称
    /// </summary>
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 定义版本号（同流程键可多版本）
    /// </summary>
    public int DefinitionVersion { get; set; } = 0;

    /// <summary>
    /// 版本标签（如 v1.0.0）
    /// </summary>
    public string ProcessVersion { get; set; } = string.Empty;

    /// <summary>
    /// 是否当前最新版（同键仅一条为 1）
    /// </summary>
    public int IsLatest { get; set; } = 0;

    /// <summary>
    /// 流程分类
    /// </summary>
    public int ProcessCategory { get; set; } = 0;

    /// <summary>
    /// 流程说明
    /// </summary>
    public string? ProcessDescription { get; set; } = string.Empty;

    /// <summary>
    /// 挂起状态（1 激活，2 挂起）
    /// </summary>
    public int SuspensionState { get; set; } = 0;

    /// <summary>
    /// 流程设计 JSON（节点、网关、条件、审批人配置）
    /// </summary>
    public string? ProcessContent { get; set; } = string.Empty;

    /// <summary>
    /// 部署批次号
    /// </summary>
    public string? DeploymentId { get; set; } = string.Empty;

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
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 发布状态
    /// </summary>
    public int ProcessStatus { get; set; } = 0;

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
