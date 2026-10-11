// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.QuickQuery
// 文件名称：TaktConfigurableSelectionDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ConfigurableSelection 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktConfigurableSelection 生成，请按需审阅）
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
// ConfigurableSelection 响应 DTO
// ========================================

/// <summary>
/// 定制报表筛选条件
/// 对应前端 TaktConfigurableSelectionDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktConfigurableSelectionDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ConfigurableSelectionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableSelectionId { get; set; }

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
    /// 显示名称（筛选项标签）
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 比较运算符
    /// </summary>
    public int FilterOperator { get; set; } = 0;

    /// <summary>
    /// 默认值（单值或 IN 列表逗号分隔）
    /// </summary>
    public string? DefaultValue { get; set; } = string.Empty;

    /// <summary>
    /// 区间结束值（BETWEEN 时使用）
    /// </summary>
    public string? DefaultValueTo { get; set; } = string.Empty;

    /// <summary>
    /// 是否必填（0=否 1=是）
    /// </summary>
    public int IsRequired { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（筛选项展示顺序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 关联的定制报表主表
    /// （主表：TaktConfigurable）
    /// </summary>
    public TaktConfigurableDto? Configurable { get; set; }

}

// ========================================
// ConfigurableSelection 查询 DTO
// ========================================

/// <summary>
/// ConfigurableSelection 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktConfigurableSelectionQueryDto : TaktPagedQuery
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
    /// 显示名称（筛选项标签）
    /// </summary>
    public string? DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 比较运算符
    /// </summary>
    public int? FilterOperator { get; set; }

    /// <summary>
    /// 默认值（单值或 IN 列表逗号分隔）
    /// </summary>
    public string? DefaultValue { get; set; } = string.Empty;

    /// <summary>
    /// 区间结束值（BETWEEN 时使用）
    /// </summary>
    public string? DefaultValueTo { get; set; } = string.Empty;

    /// <summary>
    /// 是否必填（0=否 1=是）
    /// </summary>
    public int? IsRequired { get; set; }

    /// <summary>
    /// 排序号（回填）（筛选项展示顺序）
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
// 创建ConfigurableSelection DTO
// ========================================

/// <summary>
/// 创建ConfigurableSelection DTO
/// </summary>
public class TaktConfigurableSelectionCreateDto
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
    /// 显示名称（筛选项标签）
    /// </summary>
    [Required(ErrorMessage = "显示名称（筛选项标签）不能为空")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 比较运算符
    /// </summary>
    public int FilterOperator { get; set; } = 0;

    /// <summary>
    /// 默认值（单值或 IN 列表逗号分隔）
    /// </summary>
    public string? DefaultValue { get; set; } = string.Empty;

    /// <summary>
    /// 区间结束值（BETWEEN 时使用）
    /// </summary>
    public string? DefaultValueTo { get; set; } = string.Empty;

    /// <summary>
    /// 是否必填（0=否 1=是）
    /// </summary>
    public int IsRequired { get; set; } = 0;

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
// 更新ConfigurableSelection DTO
// ========================================

/// <summary>
/// 更新ConfigurableSelection DTO
/// 继承 TaktConfigurableSelectionCreateDto，添加 ConfigurableSelectionId 字段
/// </summary>
public class TaktConfigurableSelectionUpdateDto : TaktConfigurableSelectionCreateDto
{
    /// <summary>
    /// ConfigurableSelectionID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableSelectionId { get; set; }

}

// ========================================
// ConfigurableSelection 排序 DTO
// ========================================

/// <summary>
/// ConfigurableSelection 排序更新 DTO
/// </summary>
public class TaktConfigurableSelectionSortDto
{
    /// <summary>
    /// ConfigurableSelectionID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableSelectionId { get; set; }

    /// <summary>
    /// 排序号（回填）（筛选项展示顺序）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）（筛选项展示顺序）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ConfigurableSelection 导入模板行 DTO
/// </summary>
public class TaktConfigurableSelectionTemplateDto
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
    /// 显示名称（筛选项标签）
    /// </summary>
    public string? DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 比较运算符
    /// </summary>
    public int? FilterOperator { get; set; }

    /// <summary>
    /// 默认值（单值或 IN 列表逗号分隔）
    /// </summary>
    public string? DefaultValue { get; set; } = string.Empty;

    /// <summary>
    /// 区间结束值（BETWEEN 时使用）
    /// </summary>
    public string? DefaultValueTo { get; set; } = string.Empty;

    /// <summary>
    /// 是否必填（0=否 1=是）
    /// </summary>
    public int? IsRequired { get; set; }

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
/// ConfigurableSelection 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktConfigurableSelectionImportDto
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
    /// 显示名称（筛选项标签）
    /// </summary>
    public string? DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 比较运算符
    /// </summary>
    public int? FilterOperator { get; set; }

    /// <summary>
    /// 默认值（单值或 IN 列表逗号分隔）
    /// </summary>
    public string? DefaultValue { get; set; } = string.Empty;

    /// <summary>
    /// 区间结束值（BETWEEN 时使用）
    /// </summary>
    public string? DefaultValueTo { get; set; } = string.Empty;

    /// <summary>
    /// 是否必填（0=否 1=是）
    /// </summary>
    public int? IsRequired { get; set; }

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
/// ConfigurableSelection 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktConfigurableSelectionExportDto
{
    /// <summary>
    /// ConfigurableSelectionID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableSelectionId { get; set; }

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
    /// 显示名称（筛选项标签）
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 比较运算符
    /// </summary>
    public int FilterOperator { get; set; } = 0;

    /// <summary>
    /// 默认值（单值或 IN 列表逗号分隔）
    /// </summary>
    public string? DefaultValue { get; set; } = string.Empty;

    /// <summary>
    /// 区间结束值（BETWEEN 时使用）
    /// </summary>
    public string? DefaultValueTo { get; set; } = string.Empty;

    /// <summary>
    /// 是否必填（0=否 1=是）
    /// </summary>
    public int IsRequired { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（筛选项展示顺序）
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
