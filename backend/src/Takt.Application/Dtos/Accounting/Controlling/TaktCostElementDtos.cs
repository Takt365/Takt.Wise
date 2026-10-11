// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Controlling
// 文件名称：TaktCostElementDtos.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：CostElement 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCostElement 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Controlling;

// ========================================
// CostElement 响应 DTO
// ========================================

/// <summary>
/// 成本要素实体
/// 对应前端 TaktCostElementDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktCostElementDto : TaktCompanyDtoBase
{
    /// <summary>
    /// CostElementID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CostElementId { get; set; }

    /// <summary>
    /// 成本要素编码（4位，租户+公司内唯一）
    /// </summary>
    public string CostElementCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素名称
    /// </summary>
    public string CostElementName { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
    /// </summary>
    public int CostElementType { get; set; } = 0;

    /// <summary>
    /// 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
    /// </summary>
    public int CostElementCategory { get; set; } = 0;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 成本要素层级
    /// </summary>
    public int CostElementLevel { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int CostElementStatus { get; set; } = 0;

}

// ========================================
// CostElement 树形响应 DTO
// ========================================

/// <summary>
/// CostElement 树形列表/树选择 DTO（含子节点）
/// 对应 GetCostElementTreeAsync 等接口
/// </summary>
public class TaktCostElementTreeDto : TaktCostElementDto
{
    /// <summary>
    /// 子节点（懒加载树接口返回 null，表示尚未加载；勿用空 List 冒充已加载）
    /// </summary>
    public List<TaktCostElementTreeDto>? Children { get; set; }
}

// ========================================
// CostElement 查询 DTO
// ========================================

/// <summary>
/// CostElement 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCostElementQueryDto : TaktPagedQuery
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
    /// 成本要素编码（4位，租户+公司内唯一）
    /// </summary>
    public string? CostElementCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素名称
    /// </summary>
    public string? CostElementName { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
    /// </summary>
    public int? CostElementType { get; set; }

    /// <summary>
    /// 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
    /// </summary>
    public int? CostElementCategory { get; set; }

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 成本要素层级
    /// </summary>
    public int? CostElementLevel { get; set; }

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? ValidFromStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? ValidFromEnd { get; set; }

    /// <summary>
    /// 失效日期（范围查询-开始）
    /// </summary>
    public DateTime? ValidToStart { get; set; }

    /// <summary>
    /// 失效日期（范围查询-结束）
    /// </summary>
    public DateTime? ValidToEnd { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? CostElementStatus { get; set; }

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
// 创建CostElement DTO
// ========================================

/// <summary>
/// 创建CostElement DTO
/// </summary>
public class TaktCostElementCreateDto
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
    /// 成本要素编码（4位，租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "成本要素编码（4位，租户+公司内唯一）不能为空")]
    public string CostElementCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素名称
    /// </summary>
    [Required(ErrorMessage = "成本要素名称不能为空")]
    public string CostElementName { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
    /// </summary>
    public int CostElementType { get; set; } = 0;

    /// <summary>
    /// 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
    /// </summary>
    public int CostElementCategory { get; set; } = 0;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 成本要素层级
    /// </summary>
    public int CostElementLevel { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int CostElementStatus { get; set; } = 0;

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
// 更新CostElement DTO
// ========================================

/// <summary>
/// 更新CostElement DTO
/// 继承 TaktCostElementCreateDto，添加 CostElementId 字段
/// </summary>
public class TaktCostElementUpdateDto : TaktCostElementCreateDto
{
    /// <summary>
    /// CostElementID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CostElementId { get; set; }

}

// ========================================
// CostElement 状态 DTO
// ========================================

/// <summary>
/// CostElement 状态更新 DTO
/// </summary>
public class TaktCostElementStatusDto
{
    /// <summary>
    /// CostElementID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CostElementId { get; set; }

    /// <summary>
    /// 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int CostElementStatus { get; set; } = 0;
}

// ========================================
// CostElement 排序 DTO
// ========================================

/// <summary>
/// CostElement 排序更新 DTO
/// </summary>
public class TaktCostElementSortDto
{
    /// <summary>
    /// CostElementID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CostElementId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// CostElement 导入模板行 DTO
/// </summary>
public class TaktCostElementTemplateDto
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
    /// 成本要素编码（4位，租户+公司内唯一）
    /// </summary>
    public string? CostElementCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素名称
    /// </summary>
    public string? CostElementName { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
    /// </summary>
    public int? CostElementType { get; set; }

    /// <summary>
    /// 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
    /// </summary>
    public int? CostElementCategory { get; set; }

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 成本要素层级
    /// </summary>
    public int? CostElementLevel { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? CostElementStatus { get; set; }

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
/// CostElement 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCostElementImportDto
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
    /// 成本要素编码（4位，租户+公司内唯一）
    /// </summary>
    public string? CostElementCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素名称
    /// </summary>
    public string? CostElementName { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
    /// </summary>
    public int? CostElementType { get; set; }

    /// <summary>
    /// 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
    /// </summary>
    public int? CostElementCategory { get; set; }

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentId { get; set; }

    /// <summary>
    /// 成本要素层级
    /// </summary>
    public int? CostElementLevel { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? CostElementStatus { get; set; }

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
/// CostElement 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCostElementExportDto
{
    /// <summary>
    /// CostElementID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CostElementId { get; set; }

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
    /// 成本要素编码（4位，租户+公司内唯一）
    /// </summary>
    public string CostElementCode { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素名称
    /// </summary>
    public string CostElementName { get; set; } = string.Empty;

    /// <summary>
    /// 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
    /// </summary>
    public int CostElementType { get; set; } = 0;

    /// <summary>
    /// 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
    /// </summary>
    public int CostElementCategory { get; set; } = 0;

    /// <summary>
    /// 父级 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentId { get; set; }

    /// <summary>
    /// 成本要素层级
    /// </summary>
    public int CostElementLevel { get; set; } = 0;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int CostElementStatus { get; set; } = 0;

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
