// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.QuickQuery
// 文件名称：TaktConfigurableFieldDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ConfigurableField 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktConfigurableField 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Statistics.QuickQuery;

// ========================================
// ConfigurableField 响应 DTO
// ========================================

/// <summary>
/// 定制报表输出字段定义
/// 对应前端 TaktConfigurableFieldDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktConfigurableFieldDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ConfigurableFieldID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableFieldId { get; set; }

    /// <summary>
    /// 关联定制报表主表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 关联定制报表主表 名称（填充字段）
    /// </summary>
    public string? ConfigurableName { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称（表头/Excel 列标题）
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 输出别名（SELECT AS，为空时使用 display_name）
    /// </summary>
    public string? OutputAlias { get; set; } = string.Empty;

    /// <summary>
    /// 聚合函数（无分组时为 None）
    /// </summary>
    public int AggregateFunc { get; set; } = 0;

    /// <summary>
    /// 是否输出（0=隐藏 1=显示）
    /// </summary>
    public int IsVisible { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（SELECT 列顺序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 关联的定制报表主表
    /// （主表：TaktConfigurable）
    /// </summary>
    public TaktConfigurableDto? Configurable { get; set; }

}

// ========================================
// ConfigurableField 查询 DTO
// ========================================

/// <summary>
/// ConfigurableField 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktConfigurableFieldQueryDto : TaktPagedQuery
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
    /// 关联定制报表主表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    public string? SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string? ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称（表头/Excel 列标题）
    /// </summary>
    public string? DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 输出别名（SELECT AS，为空时使用 display_name）
    /// </summary>
    public string? OutputAlias { get; set; } = string.Empty;

    /// <summary>
    /// 聚合函数（无分组时为 None）
    /// </summary>
    public int? AggregateFunc { get; set; }

    /// <summary>
    /// 是否输出（0=隐藏 1=显示）
    /// </summary>
    public int? IsVisible { get; set; }

    /// <summary>
    /// 排序号（回填）（SELECT 列顺序）
    /// </summary>
    public int? SortOrder { get; set; }

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
// 创建ConfigurableField DTO
// ========================================

/// <summary>
/// 创建ConfigurableField DTO
/// </summary>
public class TaktConfigurableFieldCreateDto
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
    /// 关联定制报表主表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    [Required(ErrorMessage = "数据源别名不能为空")]
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    [Required(ErrorMessage = "列名不能为空")]
    public string ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称（表头/Excel 列标题）
    /// </summary>
    [Required(ErrorMessage = "显示名称（表头/Excel 列标题）不能为空")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 输出别名（SELECT AS，为空时使用 display_name）
    /// </summary>
    public string? OutputAlias { get; set; } = string.Empty;

    /// <summary>
    /// 聚合函数（无分组时为 None）
    /// </summary>
    public int AggregateFunc { get; set; } = 0;

    /// <summary>
    /// 是否输出（0=隐藏 1=显示）
    /// </summary>
    public int IsVisible { get; set; } = 0;

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
// 更新ConfigurableField DTO
// ========================================

/// <summary>
/// 更新ConfigurableField DTO
/// 继承 TaktConfigurableFieldCreateDto，添加 ConfigurableFieldId 字段
/// </summary>
public class TaktConfigurableFieldUpdateDto : TaktConfigurableFieldCreateDto
{
    /// <summary>
    /// ConfigurableFieldID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableFieldId { get; set; }

}

// ========================================
// ConfigurableField 排序 DTO
// ========================================

/// <summary>
/// ConfigurableField 排序更新 DTO
/// </summary>
public class TaktConfigurableFieldSortDto
{
    /// <summary>
    /// ConfigurableFieldID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableFieldId { get; set; }

    /// <summary>
    /// 排序号（回填）（SELECT 列顺序）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）（SELECT 列顺序）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ConfigurableField 导入模板行 DTO
/// </summary>
public class TaktConfigurableFieldTemplateDto
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
    /// 关联定制报表主表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    public string? SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string? ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称（表头/Excel 列标题）
    /// </summary>
    public string? DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 输出别名（SELECT AS，为空时使用 display_name）
    /// </summary>
    public string? OutputAlias { get; set; } = string.Empty;

    /// <summary>
    /// 聚合函数（无分组时为 None）
    /// </summary>
    public int? AggregateFunc { get; set; }

    /// <summary>
    /// 是否输出（0=隐藏 1=显示）
    /// </summary>
    public int? IsVisible { get; set; }

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
/// ConfigurableField 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktConfigurableFieldImportDto
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
    /// 关联定制报表主表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    public string? SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string? ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称（表头/Excel 列标题）
    /// </summary>
    public string? DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 输出别名（SELECT AS，为空时使用 display_name）
    /// </summary>
    public string? OutputAlias { get; set; } = string.Empty;

    /// <summary>
    /// 聚合函数（无分组时为 None）
    /// </summary>
    public int? AggregateFunc { get; set; }

    /// <summary>
    /// 是否输出（0=隐藏 1=显示）
    /// </summary>
    public int? IsVisible { get; set; }

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
/// ConfigurableField 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktConfigurableFieldExportDto
{
    /// <summary>
    /// ConfigurableFieldID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableFieldId { get; set; }

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
    /// 关联定制报表主表 ID（主子表关系）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名
    /// </summary>
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称（表头/Excel 列标题）
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 输出别名（SELECT AS，为空时使用 display_name）
    /// </summary>
    public string? OutputAlias { get; set; } = string.Empty;

    /// <summary>
    /// 聚合函数（无分组时为 None）
    /// </summary>
    public int AggregateFunc { get; set; } = 0;

    /// <summary>
    /// 是否输出（0=隐藏 1=显示）
    /// </summary>
    public int IsVisible { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（SELECT 列顺序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
