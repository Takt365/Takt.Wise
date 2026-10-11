// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Workflow
// 文件名称：TaktFlowFormDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：FlowForm 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFlowForm 生成，请按需审阅）
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
// FlowForm 响应 DTO
// ========================================

/// <summary>
/// 流程表单定义实体
/// 对应前端 TaktFlowFormDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktFlowFormDto : TaktCompanyDtoBase
{
    /// <summary>
    /// FlowFormID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowFormId { get; set; }

    /// <summary>
    /// 表单编码（公司内唯一）
    /// </summary>
    public string FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称
    /// </summary>
    public string FormName { get; set; } = string.Empty;

    /// <summary>
    /// 表单分类（字典 sys_form_category）
    /// </summary>
    public int FormCategory { get; set; } = 0;

    /// <summary>
    /// 表单类型（字典 sys_form_type）
    /// </summary>
    public int FormType { get; set; } = 0;

    /// <summary>
    /// 表单设计 JSON
    /// </summary>
    public string? FormConfig { get; set; } = string.Empty;

    /// <summary>
    /// 表单模板 JSON
    /// </summary>
    public string? FormTemplate { get; set; } = string.Empty;

    /// <summary>
    /// 表单版本标签
    /// </summary>
    public string FormVersion { get; set; } = string.Empty;

    /// <summary>
    /// 是否绑定数据源
    /// </summary>
    public int IsDatasource { get; set; } = 0;

    /// <summary>
    /// 关联库名
    /// </summary>
    public string? RelatedDataBaseName { get; set; } = string.Empty;

    /// <summary>
    /// 关联表名
    /// </summary>
    public string? RelatedTableName { get; set; } = string.Empty;

    /// <summary>
    /// 关联映射 JSON
    /// </summary>
    public string? RelatedFormField { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 表单状态
    /// </summary>
    public int FormStatus { get; set; } = 0;

}

// ========================================
// FlowForm 查询 DTO
// ========================================

/// <summary>
/// FlowForm 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFlowFormQueryDto : TaktPagedQuery
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
    /// 表单编码（公司内唯一）
    /// </summary>
    public string? FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 表单分类（字典 sys_form_category）
    /// </summary>
    public int? FormCategory { get; set; }

    /// <summary>
    /// 表单类型（字典 sys_form_type）
    /// </summary>
    public int? FormType { get; set; }

    /// <summary>
    /// 表单设计 JSON
    /// </summary>
    public string? FormConfig { get; set; } = string.Empty;

    /// <summary>
    /// 表单模板 JSON
    /// </summary>
    public string? FormTemplate { get; set; } = string.Empty;

    /// <summary>
    /// 表单版本标签
    /// </summary>
    public string? FormVersion { get; set; } = string.Empty;

    /// <summary>
    /// 是否绑定数据源
    /// </summary>
    public int? IsDatasource { get; set; }

    /// <summary>
    /// 关联库名
    /// </summary>
    public string? RelatedDataBaseName { get; set; } = string.Empty;

    /// <summary>
    /// 关联表名
    /// </summary>
    public string? RelatedTableName { get; set; } = string.Empty;

    /// <summary>
    /// 关联映射 JSON
    /// </summary>
    public string? RelatedFormField { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 表单状态
    /// </summary>
    public int? FormStatus { get; set; }

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
// 创建FlowForm DTO
// ========================================

/// <summary>
/// 创建FlowForm DTO
/// </summary>
public class TaktFlowFormCreateDto
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
    /// 表单编码（公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "表单编码（公司内唯一）不能为空")]
    public string FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 编码规则编码（按表单分类选规则取号；对应 TaktNumbering.RuleCode；不落库）
    /// </summary>
    public string? NumberingRuleCode { get; set; }

    /// <summary>
    /// 表单名称
    /// </summary>
    [Required(ErrorMessage = "表单名称不能为空")]
    public string FormName { get; set; } = string.Empty;

    /// <summary>
    /// 表单分类（字典 sys_form_category）
    /// </summary>
    public int FormCategory { get; set; } = 0;

    /// <summary>
    /// 表单类型（字典 sys_form_type）
    /// </summary>
    public int FormType { get; set; } = 0;

    /// <summary>
    /// 表单设计 JSON
    /// </summary>
    public string? FormConfig { get; set; } = string.Empty;

    /// <summary>
    /// 表单模板 JSON
    /// </summary>
    public string? FormTemplate { get; set; } = string.Empty;

    /// <summary>
    /// 表单版本标签
    /// </summary>
    [Required(ErrorMessage = "表单版本标签不能为空")]
    public string FormVersion { get; set; } = string.Empty;

    /// <summary>
    /// 是否绑定数据源
    /// </summary>
    public int IsDatasource { get; set; } = 0;

    /// <summary>
    /// 关联库名
    /// </summary>
    public string? RelatedDataBaseName { get; set; } = string.Empty;

    /// <summary>
    /// 关联表名
    /// </summary>
    public string? RelatedTableName { get; set; } = string.Empty;

    /// <summary>
    /// 关联映射 JSON
    /// </summary>
    public string? RelatedFormField { get; set; } = string.Empty;

    /// <summary>
    /// 表单状态
    /// </summary>
    public int FormStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }



    /// <summary>
    /// SortOrder
    /// </summary>
    public int SortOrder { get; set; }
}

// ========================================
// 更新FlowForm DTO
// ========================================

/// <summary>
/// 更新FlowForm DTO
/// 继承 TaktFlowFormCreateDto，添加 FlowFormId 字段
/// </summary>
public class TaktFlowFormUpdateDto : TaktFlowFormCreateDto
{
    /// <summary>
    /// FlowFormID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowFormId { get; set; }

}

// ========================================
// FlowForm 状态 DTO
// ========================================

/// <summary>
/// FlowForm 状态更新 DTO
/// </summary>
public class TaktFlowFormStatusDto
{
    /// <summary>
    /// FlowFormID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowFormId { get; set; }

    /// <summary>
    /// 表单状态
    /// </summary>
    [Required(ErrorMessage = "表单状态不能为空")]
    public int FormStatus { get; set; } = 0;
}

// ========================================
// FlowForm 排序 DTO
// ========================================

/// <summary>
/// FlowForm 排序更新 DTO
/// </summary>
public class TaktFlowFormSortDto
{
    /// <summary>
    /// FlowFormID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowFormId { get; set; }

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
/// FlowForm 导入模板行 DTO
/// </summary>
public class TaktFlowFormTemplateDto
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
    /// 表单编码（公司内唯一）
    /// </summary>
    public string? FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 表单分类（字典 sys_form_category）
    /// </summary>
    public int? FormCategory { get; set; }

    /// <summary>
    /// 表单类型（字典 sys_form_type）
    /// </summary>
    public int? FormType { get; set; }

    /// <summary>
    /// 表单设计 JSON
    /// </summary>
    public string? FormConfig { get; set; } = string.Empty;

    /// <summary>
    /// 表单模板 JSON
    /// </summary>
    public string? FormTemplate { get; set; } = string.Empty;

    /// <summary>
    /// 表单版本标签
    /// </summary>
    public string? FormVersion { get; set; } = string.Empty;

    /// <summary>
    /// 是否绑定数据源
    /// </summary>
    public int? IsDatasource { get; set; }

    /// <summary>
    /// 关联库名
    /// </summary>
    public string? RelatedDataBaseName { get; set; } = string.Empty;

    /// <summary>
    /// 关联表名
    /// </summary>
    public string? RelatedTableName { get; set; } = string.Empty;

    /// <summary>
    /// 关联映射 JSON
    /// </summary>
    public string? RelatedFormField { get; set; } = string.Empty;

    /// <summary>
    /// 表单状态
    /// </summary>
    public int? FormStatus { get; set; }

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
/// FlowForm 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktFlowFormImportDto
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
    /// 表单编码（公司内唯一）
    /// </summary>
    public string? FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称
    /// </summary>
    public string? FormName { get; set; } = string.Empty;

    /// <summary>
    /// 表单分类（字典 sys_form_category）
    /// </summary>
    public int? FormCategory { get; set; }

    /// <summary>
    /// 表单类型（字典 sys_form_type）
    /// </summary>
    public int? FormType { get; set; }

    /// <summary>
    /// 表单设计 JSON
    /// </summary>
    public string? FormConfig { get; set; } = string.Empty;

    /// <summary>
    /// 表单模板 JSON
    /// </summary>
    public string? FormTemplate { get; set; } = string.Empty;

    /// <summary>
    /// 表单版本标签
    /// </summary>
    public string? FormVersion { get; set; } = string.Empty;

    /// <summary>
    /// 是否绑定数据源
    /// </summary>
    public int? IsDatasource { get; set; }

    /// <summary>
    /// 关联库名
    /// </summary>
    public string? RelatedDataBaseName { get; set; } = string.Empty;

    /// <summary>
    /// 关联表名
    /// </summary>
    public string? RelatedTableName { get; set; } = string.Empty;

    /// <summary>
    /// 关联映射 JSON
    /// </summary>
    public string? RelatedFormField { get; set; } = string.Empty;

    /// <summary>
    /// 表单状态
    /// </summary>
    public int? FormStatus { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }



    /// <summary>
    /// SortOrder
    /// </summary>
    public int SortOrder { get; set; }
}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// FlowForm 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFlowFormExportDto
{
    /// <summary>
    /// FlowFormID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FlowFormId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 表单编码（公司内唯一）
    /// </summary>
    public string FormCode { get; set; } = string.Empty;

    /// <summary>
    /// 表单名称
    /// </summary>
    public string FormName { get; set; } = string.Empty;

    /// <summary>
    /// 表单分类（字典 sys_form_category）
    /// </summary>
    public int FormCategory { get; set; } = 0;

    /// <summary>
    /// 表单类型（字典 sys_form_type）
    /// </summary>
    public int FormType { get; set; } = 0;

    /// <summary>
    /// 表单设计 JSON
    /// </summary>
    public string? FormConfig { get; set; } = string.Empty;

    /// <summary>
    /// 表单模板 JSON
    /// </summary>
    public string? FormTemplate { get; set; } = string.Empty;

    /// <summary>
    /// 表单版本标签
    /// </summary>
    public string FormVersion { get; set; } = string.Empty;

    /// <summary>
    /// 是否绑定数据源
    /// </summary>
    public int IsDatasource { get; set; } = 0;

    /// <summary>
    /// 关联库名
    /// </summary>
    public string? RelatedDataBaseName { get; set; } = string.Empty;

    /// <summary>
    /// 关联表名
    /// </summary>
    public string? RelatedTableName { get; set; } = string.Empty;

    /// <summary>
    /// 关联映射 JSON
    /// </summary>
    public string? RelatedFormField { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 表单状态
    /// </summary>
    public int FormStatus { get; set; } = 0;

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
