// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.QuickQuery
// 文件名称：TaktConfigurableDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Configurable 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktConfigurable 生成，请按需审阅）
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
// Configurable 响应 DTO
// ========================================

/// <summary>
/// 定制报表主实体（快速查询定义）
/// 对应前端 TaktConfigurableDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktConfigurableDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ConfigurableID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 定制报表编码（租户+公司内唯一）
    /// </summary>
    public string ConfigurableCode { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表名称
    /// </summary>
    public string ConfigurableName { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表业务域（TaktModule 整型，与一级目录菜单 MenuCode 映射；展示名取自菜单 i18n）
    /// </summary>
    public int ConfigurableDomain { get; set; } = 0;

    /// <summary>
    /// 定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    public string? ConfigurableSubCategory { get; set; } = string.Empty;

    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int DistinctRows { get; set; } = 1;

    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    public int MaxExportRows { get; set; } = 0;

    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    public int MaxQueryRows { get; set; } = 0;

    /// <summary>
    /// 公开（字典 sys_public_type；0=公开，1=私有）
    /// </summary>
    public int IsPublic { get; set; } = 0;

    /// <summary>
    /// 定制报表描述
    /// </summary>
    public string? ConfigurableDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 定制报表状态（0=禁用 1=启用）
    /// </summary>
    public int ConfigurableStatus { get; set; } = 0;

    /// <summary>
    /// 数据源表列表（FROM）
    /// （子表：TaktConfigurableSource）
    /// </summary>
    public List<TaktConfigurableSourceDto>? Sources { get; set; }

    /// <summary>
    /// 多表关联列表（JOIN）
    /// （子表：TaktConfigurableJoin）
    /// </summary>
    public List<TaktConfigurableJoinDto>? Joins { get; set; }

    /// <summary>
    /// 输出字段列表（SELECT）
    /// （子表：TaktConfigurableField）
    /// </summary>
    public List<TaktConfigurableFieldDto>? Fields { get; set; }

    /// <summary>
    /// 筛选条件列表（WHERE）
    /// （子表：TaktConfigurableSelection）
    /// </summary>
    public List<TaktConfigurableSelectionDto>? Selections { get; set; }

    /// <summary>
    /// 分组字段列表（GROUP BY）
    /// （子表：TaktConfigurableGroupBy）
    /// </summary>
    public List<TaktConfigurableGroupByDto>? GroupBys { get; set; }

    /// <summary>
    /// 排序字段列表（ORDER BY）
    /// （子表：TaktConfigurableOrderBy）
    /// </summary>
    public List<TaktConfigurableOrderByDto>? OrderBys { get; set; }

}

// ========================================
// Configurable 查询 DTO
// ========================================

/// <summary>
/// Configurable 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktConfigurableQueryDto : TaktPagedQuery
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
    /// 定制报表编码（租户+公司内唯一）
    /// </summary>
    public string? ConfigurableCode { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表名称
    /// </summary>
    public string? ConfigurableName { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表业务域（TaktModule 整型，与一级目录菜单 MenuCode 映射；展示名取自菜单 i18n）
    /// </summary>
    public int? ConfigurableDomain { get; set; }

    /// <summary>
    /// 定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    public string? ConfigurableSubCategory { get; set; } = string.Empty;

    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int? DistinctRows { get; set; }

    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    public int? MaxExportRows { get; set; }

    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    public int? MaxQueryRows { get; set; }

    /// <summary>
    /// 公开（字典 sys_public_type；0=公开，1=私有）
    /// </summary>
    public int? IsPublic { get; set; }

    /// <summary>
    /// 定制报表描述
    /// </summary>
    public string? ConfigurableDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 定制报表状态（0=禁用 1=启用）
    /// </summary>
    public int? ConfigurableStatus { get; set; }

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
// 创建Configurable DTO
// ========================================

/// <summary>
/// 创建Configurable DTO
/// </summary>
public class TaktConfigurableCreateDto
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
    /// 定制报表编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "定制报表编码（租户+公司内唯一）不能为空")]
    public string ConfigurableCode { get; set; } = string.Empty;

    /// <summary>
    /// 编码规则编码（表单选规则后取号；对应 TaktNumbering.RuleCode；不落库）
    /// </summary>
    public string? NumberingRuleCode { get; set; }

    /// <summary>
    /// 定制报表名称
    /// </summary>
    [Required(ErrorMessage = "定制报表名称不能为空")]
    public string ConfigurableName { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表业务域（TaktModule 整型，与一级目录菜单 MenuCode 映射；展示名取自菜单 i18n）
    /// </summary>
    public int ConfigurableDomain { get; set; } = 0;

    /// <summary>
    /// 定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    public string? ConfigurableSubCategory { get; set; } = string.Empty;

    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int DistinctRows { get; set; } = 1;

    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    public int MaxExportRows { get; set; } = 0;

    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    public int MaxQueryRows { get; set; } = 0;

    /// <summary>
    /// 公开（字典 sys_public_type；0=公开，1=私有）
    /// </summary>
    public int IsPublic { get; set; } = 0;

    /// <summary>
    /// 定制报表描述
    /// </summary>
    public string? ConfigurableDescription { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表状态（0=禁用 1=启用）
    /// </summary>
    public int ConfigurableStatus { get; set; } = 0;

    /// <summary>
    /// 数据源表列表（FROM）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableSourceCreateDto>? Sources { get; set; }

    /// <summary>
    /// 多表关联列表（JOIN）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableJoinCreateDto>? Joins { get; set; }

    /// <summary>
    /// 输出字段列表（SELECT）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableFieldCreateDto>? Fields { get; set; }

    /// <summary>
    /// 筛选条件列表（WHERE）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableSelectionCreateDto>? Selections { get; set; }

    /// <summary>
    /// 分组字段列表（GROUP BY）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableGroupByCreateDto>? GroupBys { get; set; }

    /// <summary>
    /// 排序字段列表（ORDER BY）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableOrderByCreateDto>? OrderBys { get; set; }

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
// 更新Configurable DTO
// ========================================

/// <summary>
/// 更新Configurable DTO
/// 继承 TaktConfigurableCreateDto，添加 ConfigurableId 字段
/// </summary>
public class TaktConfigurableUpdateDto : TaktConfigurableCreateDto
{
    /// <summary>
    /// ConfigurableID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 数据源表列表（FROM）（子表，级联保存）
    /// </summary>
    public new List<TaktConfigurableSourceUpdateDto>? Sources { get; set; }

    /// <summary>
    /// 多表关联列表（JOIN）（子表，级联保存）
    /// </summary>
    public new List<TaktConfigurableJoinUpdateDto>? Joins { get; set; }

    /// <summary>
    /// 输出字段列表（SELECT）（子表，级联保存）
    /// </summary>
    public new List<TaktConfigurableFieldUpdateDto>? Fields { get; set; }

    /// <summary>
    /// 筛选条件列表（WHERE）（子表，级联保存）
    /// </summary>
    public new List<TaktConfigurableSelectionUpdateDto>? Selections { get; set; }

    /// <summary>
    /// 分组字段列表（GROUP BY）（子表，级联保存）
    /// </summary>
    public new List<TaktConfigurableGroupByUpdateDto>? GroupBys { get; set; }

    /// <summary>
    /// 排序字段列表（ORDER BY）（子表，级联保存）
    /// </summary>
    public new List<TaktConfigurableOrderByUpdateDto>? OrderBys { get; set; }

}

// ========================================
// Configurable 状态 DTO
// ========================================

/// <summary>
/// Configurable 状态更新 DTO
/// </summary>
public class TaktConfigurableStatusDto
{
    /// <summary>
    /// ConfigurableID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 定制报表状态（0=禁用 1=启用）
    /// </summary>
    [Required(ErrorMessage = "定制报表状态（0=禁用 1=启用）不能为空")]
    public int ConfigurableStatus { get; set; } = 0;
}

// ========================================
// Configurable 排序 DTO
// ========================================

/// <summary>
/// Configurable 排序更新 DTO
/// </summary>
public class TaktConfigurableSortDto
{
    /// <summary>
    /// ConfigurableID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

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
/// Configurable 导入模板行 DTO
/// </summary>
public class TaktConfigurableTemplateDto
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
    /// 定制报表编码（租户+公司内唯一）
    /// </summary>
    public string? ConfigurableCode { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表名称
    /// </summary>
    public string? ConfigurableName { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表业务域（TaktModule 整型，与一级目录菜单 MenuCode 映射；展示名取自菜单 i18n）
    /// </summary>
    public int? ConfigurableDomain { get; set; }

    /// <summary>
    /// 定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    public string? ConfigurableSubCategory { get; set; } = string.Empty;

    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int? DistinctRows { get; set; }

    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    public int? MaxExportRows { get; set; }

    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    public int? MaxQueryRows { get; set; }

    /// <summary>
    /// 公开（字典 sys_public_type；0=公开，1=私有）
    /// </summary>
    public int? IsPublic { get; set; }

    /// <summary>
    /// 定制报表描述
    /// </summary>
    public string? ConfigurableDescription { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表状态（0=禁用 1=启用）
    /// </summary>
    public int? ConfigurableStatus { get; set; }

    /// <summary>
    /// 数据源表列表（FROM）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableSourceCreateDto>? Sources { get; set; }

    /// <summary>
    /// 多表关联列表（JOIN）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableJoinCreateDto>? Joins { get; set; }

    /// <summary>
    /// 输出字段列表（SELECT）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableFieldCreateDto>? Fields { get; set; }

    /// <summary>
    /// 筛选条件列表（WHERE）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableSelectionCreateDto>? Selections { get; set; }

    /// <summary>
    /// 分组字段列表（GROUP BY）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableGroupByCreateDto>? GroupBys { get; set; }

    /// <summary>
    /// 排序字段列表（ORDER BY）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableOrderByCreateDto>? OrderBys { get; set; }

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
/// Configurable 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktConfigurableImportDto
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
    /// 定制报表编码（租户+公司内唯一）
    /// </summary>
    public string? ConfigurableCode { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表名称
    /// </summary>
    public string? ConfigurableName { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表业务域（TaktModule 整型，与一级目录菜单 MenuCode 映射；展示名取自菜单 i18n）
    /// </summary>
    public int? ConfigurableDomain { get; set; }

    /// <summary>
    /// 定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    public string? ConfigurableSubCategory { get; set; } = string.Empty;

    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int? DistinctRows { get; set; }

    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    public int? MaxExportRows { get; set; }

    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    public int? MaxQueryRows { get; set; }

    /// <summary>
    /// 公开（字典 sys_public_type；0=公开，1=私有）
    /// </summary>
    public int? IsPublic { get; set; }

    /// <summary>
    /// 定制报表描述
    /// </summary>
    public string? ConfigurableDescription { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表状态（0=禁用 1=启用）
    /// </summary>
    public int? ConfigurableStatus { get; set; }

    /// <summary>
    /// 数据源表列表（FROM）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableSourceCreateDto>? Sources { get; set; }

    /// <summary>
    /// 多表关联列表（JOIN）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableJoinCreateDto>? Joins { get; set; }

    /// <summary>
    /// 输出字段列表（SELECT）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableFieldCreateDto>? Fields { get; set; }

    /// <summary>
    /// 筛选条件列表（WHERE）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableSelectionCreateDto>? Selections { get; set; }

    /// <summary>
    /// 分组字段列表（GROUP BY）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableGroupByCreateDto>? GroupBys { get; set; }

    /// <summary>
    /// 排序字段列表（ORDER BY）（子表，级联保存）
    /// </summary>
    public List<TaktConfigurableOrderByCreateDto>? OrderBys { get; set; }

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
/// Configurable 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktConfigurableExportDto
{
    /// <summary>
    /// ConfigurableID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

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
    /// 定制报表编码（租户+公司内唯一）
    /// </summary>
    public string ConfigurableCode { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表名称
    /// </summary>
    public string ConfigurableName { get; set; } = string.Empty;

    /// <summary>
    /// 定制报表业务域（TaktModule 整型，与一级目录菜单 MenuCode 映射；展示名取自菜单 i18n）
    /// </summary>
    public int ConfigurableDomain { get; set; } = 0;

    /// <summary>
    /// 定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    public string? ConfigurableSubCategory { get; set; } = string.Empty;

    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    public int DistinctRows { get; set; } = 1;

    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    public int MaxExportRows { get; set; } = 0;

    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    public int MaxQueryRows { get; set; } = 0;

    /// <summary>
    /// 公开（字典 sys_public_type；0=公开，1=私有）
    /// </summary>
    public int IsPublic { get; set; } = 0;

    /// <summary>
    /// 定制报表描述
    /// </summary>
    public string? ConfigurableDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 定制报表状态（0=禁用 1=启用）
    /// </summary>
    public int ConfigurableStatus { get; set; } = 0;

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
