// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktSystemPropertyDtos.cs
// 创建时间：2026-09-16
// 创建人：Takt365(Auto Generated)
// 功能描述：SystemProperty 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSystemProperty 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// SystemProperty 响应 DTO
// ========================================

/// <summary>
/// 系统属性实体 存储系统的各种配置参数，支持租户级配置隔离
/// 对应前端 TaktSystemPropertyDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSystemPropertyDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SystemPropertyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SystemPropertyId { get; set; }

    /// <summary>
    /// 属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    public string SystemPropertyKey { get; set; } = string.Empty;

    /// <summary>
    /// 属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）
    /// </summary>
    public string? SystemPropertyValue { get; set; } = string.Empty;

    /// <summary>
    /// 属性名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    public string SystemPropertyName { get; set; } = string.Empty;

    /// <summary>
    /// 属性描述
    /// </summary>
    public string? SystemPropertyDescription { get; set; } = string.Empty;

    /// <summary>
    /// 属性类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string SystemPropertyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string ValueType { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 只读（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsReadonly { get; set; } = 0;

    /// <summary>
    /// 加密（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsEncrypted { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int SystemPropertyStatus { get; set; } = 0;

}

// ========================================
// SystemProperty 查询 DTO
// ========================================

/// <summary>
/// SystemProperty 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSystemPropertyQueryDto : TaktPagedQuery
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
    /// 属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    public string? SystemPropertyKey { get; set; } = string.Empty;

    /// <summary>
    /// 属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）
    /// </summary>
    public string? SystemPropertyValue { get; set; } = string.Empty;

    /// <summary>
    /// 属性名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    public string? SystemPropertyName { get; set; } = string.Empty;

    /// <summary>
    /// 属性描述
    /// </summary>
    public string? SystemPropertyDescription { get; set; } = string.Empty;

    /// <summary>
    /// 属性类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string? SystemPropertyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string? ValueType { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 只读（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsReadonly { get; set; }

    /// <summary>
    /// 加密（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsEncrypted { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? SystemPropertyStatus { get; set; }

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
// 创建SystemProperty DTO
// ========================================

/// <summary>
/// 创建SystemProperty DTO
/// </summary>
public class TaktSystemPropertyCreateDto
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
    /// 属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    [Required(ErrorMessage = "属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）不能为空")]
    public string SystemPropertyKey { get; set; } = string.Empty;

    /// <summary>
    /// 属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）
    /// </summary>
    public string? SystemPropertyValue { get; set; } = string.Empty;

    /// <summary>
    /// 属性名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    [Required(ErrorMessage = "属性名称（显示名称，如：站点名称、最大上传大小）不能为空")]
    public string SystemPropertyName { get; set; } = string.Empty;

    /// <summary>
    /// 属性描述
    /// </summary>
    public string? SystemPropertyDescription { get; set; } = string.Empty;

    /// <summary>
    /// 属性类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    [Required(ErrorMessage = "属性类别（字典 sys_resource_type；frontend=前端 backend=后端）不能为空")]
    public string SystemPropertyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    [Required(ErrorMessage = "值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）不能为空")]
    public string ValueType { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 只读（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsReadonly { get; set; } = 0;

    /// <summary>
    /// 加密（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsEncrypted { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int SystemPropertyStatus { get; set; } = 0;

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
// 更新SystemProperty DTO
// ========================================

/// <summary>
/// 更新SystemProperty DTO
/// 继承 TaktSystemPropertyCreateDto，添加 SystemPropertyId 字段
/// </summary>
public class TaktSystemPropertyUpdateDto : TaktSystemPropertyCreateDto
{
    /// <summary>
    /// SystemPropertyID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SystemPropertyId { get; set; }

}

// ========================================
// SystemProperty 状态 DTO
// ========================================

/// <summary>
/// SystemProperty 状态更新 DTO
/// </summary>
public class TaktSystemPropertyStatusDto
{
    /// <summary>
    /// SystemPropertyID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SystemPropertyId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用 0=禁用）不能为空")]
    public int SystemPropertyStatus { get; set; } = 0;
}

// ========================================
// SystemProperty 排序 DTO
// ========================================

/// <summary>
/// SystemProperty 排序更新 DTO
/// </summary>
public class TaktSystemPropertySortDto
{
    /// <summary>
    /// SystemPropertyID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SystemPropertyId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// SystemProperty 内置 DTO
// ========================================

/// <summary>
/// SystemProperty 内置更新 DTO
/// </summary>
public class TaktSystemPropertyBuiltInDto
{
    /// <summary>
    /// SystemPropertyID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SystemPropertyId { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否）
    /// </summary>
    [Required(ErrorMessage = "内置不能为空")]
    public int IsBuiltIn { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SystemProperty 导入模板行 DTO
/// </summary>
public class TaktSystemPropertyTemplateDto
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
    /// 属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    public string? SystemPropertyKey { get; set; } = string.Empty;

    /// <summary>
    /// 属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）
    /// </summary>
    public string? SystemPropertyValue { get; set; } = string.Empty;

    /// <summary>
    /// 属性名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    public string? SystemPropertyName { get; set; } = string.Empty;

    /// <summary>
    /// 属性描述
    /// </summary>
    public string? SystemPropertyDescription { get; set; } = string.Empty;

    /// <summary>
    /// 属性类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string? SystemPropertyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string? ValueType { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 只读（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsReadonly { get; set; }

    /// <summary>
    /// 加密（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsEncrypted { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? SystemPropertyStatus { get; set; }

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
/// SystemProperty 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSystemPropertyImportDto
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
    /// 属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    public string? SystemPropertyKey { get; set; } = string.Empty;

    /// <summary>
    /// 属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）
    /// </summary>
    public string? SystemPropertyValue { get; set; } = string.Empty;

    /// <summary>
    /// 属性名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    public string? SystemPropertyName { get; set; } = string.Empty;

    /// <summary>
    /// 属性描述
    /// </summary>
    public string? SystemPropertyDescription { get; set; } = string.Empty;

    /// <summary>
    /// 属性类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string? SystemPropertyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string? ValueType { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 只读（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsReadonly { get; set; }

    /// <summary>
    /// 加密（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsEncrypted { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? SystemPropertyStatus { get; set; }

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
/// SystemProperty 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSystemPropertyExportDto
{
    /// <summary>
    /// SystemPropertyID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SystemPropertyId { get; set; }

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
    /// 属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    public string SystemPropertyKey { get; set; } = string.Empty;

    /// <summary>
    /// 属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）
    /// </summary>
    public string? SystemPropertyValue { get; set; } = string.Empty;

    /// <summary>
    /// 属性名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    public string SystemPropertyName { get; set; } = string.Empty;

    /// <summary>
    /// 属性描述
    /// </summary>
    public string? SystemPropertyDescription { get; set; } = string.Empty;

    /// <summary>
    /// 属性类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string SystemPropertyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string ValueType { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 只读（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsReadonly { get; set; } = 0;

    /// <summary>
    /// 加密（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsEncrypted { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int SystemPropertyStatus { get; set; } = 0;

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
