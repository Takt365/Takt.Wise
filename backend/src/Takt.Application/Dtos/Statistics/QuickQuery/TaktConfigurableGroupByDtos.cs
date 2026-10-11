// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.QuickQuery
// 文件名称：TaktConfigurableGroupByDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ConfigurableGroupBy 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktConfigurableGroupBy 生成，请按需审阅）
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
// ConfigurableGroupBy 响应 DTO
// ========================================

/// <summary>
/// 定制报表分组字段定义
/// 对应前端 TaktConfigurableGroupByDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktConfigurableGroupByDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ConfigurableGroupByID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableGroupById { get; set; }

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
    /// 排序号（回填）（GROUP BY 列顺序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 关联的定制报表主表
    /// （主表：TaktConfigurable）
    /// </summary>
    public TaktConfigurableDto? Configurable { get; set; }

}

// ========================================
// ConfigurableGroupBy 查询 DTO
// ========================================

/// <summary>
/// ConfigurableGroupBy 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktConfigurableGroupByQueryDto : TaktPagedQuery
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
    /// 排序号（回填）（GROUP BY 列顺序）
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
// 创建ConfigurableGroupBy DTO
// ========================================

/// <summary>
/// 创建ConfigurableGroupBy DTO
/// </summary>
public class TaktConfigurableGroupByCreateDto
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
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新ConfigurableGroupBy DTO
// ========================================

/// <summary>
/// 更新ConfigurableGroupBy DTO
/// 继承 TaktConfigurableGroupByCreateDto，添加 ConfigurableGroupById 字段
/// </summary>
public class TaktConfigurableGroupByUpdateDto : TaktConfigurableGroupByCreateDto
{
    /// <summary>
    /// ConfigurableGroupByID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableGroupById { get; set; }

}

// ========================================
// ConfigurableGroupBy 排序 DTO
// ========================================

/// <summary>
/// ConfigurableGroupBy 排序更新 DTO
/// </summary>
public class TaktConfigurableGroupBySortDto
{
    /// <summary>
    /// ConfigurableGroupByID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableGroupById { get; set; }

    /// <summary>
    /// 排序号（回填）（GROUP BY 列顺序）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）（GROUP BY 列顺序）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ConfigurableGroupBy 导入模板行 DTO
/// </summary>
public class TaktConfigurableGroupByTemplateDto
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
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// ConfigurableGroupBy 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktConfigurableGroupByImportDto
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
/// ConfigurableGroupBy 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktConfigurableGroupByExportDto
{
    /// <summary>
    /// ConfigurableGroupByID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableGroupById { get; set; }

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
    /// 排序号（回填）（GROUP BY 列顺序）
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
