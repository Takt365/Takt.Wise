// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.QuickQuery
// 文件名称：TaktConfigurableSourceDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ConfigurableSource 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktConfigurableSource 生成，请按需审阅）
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
// ConfigurableSource 响应 DTO
// ========================================

/// <summary>
/// 定制报表数据源（单表及别名）
/// 对应前端 TaktConfigurableSourceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktConfigurableSourceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ConfigurableSourceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableSourceId { get; set; }

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
    /// 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
    /// </summary>
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 是否主表（驱动 FROM 的第一张表）
    /// </summary>
    public int IsPrimary { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（多表 FROM 顺序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 关联的定制报表主表
    /// （主表：TaktConfigurable）
    /// </summary>
    public TaktConfigurableDto? Configurable { get; set; }

}

// ========================================
// ConfigurableSource 查询 DTO
// ========================================

/// <summary>
/// ConfigurableSource 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktConfigurableSourceQueryDto : TaktPagedQuery
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
    /// 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
    /// </summary>
    public string? SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 是否主表（驱动 FROM 的第一张表）
    /// </summary>
    public int? IsPrimary { get; set; }

    /// <summary>
    /// 排序号（回填）（多表 FROM 顺序）
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
// 创建ConfigurableSource DTO
// ========================================

/// <summary>
/// 创建ConfigurableSource DTO
/// </summary>
public class TaktConfigurableSourceCreateDto
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
    /// 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
    /// </summary>
    [Required(ErrorMessage = "数据源别名（如 A、B、C，用于 JOIN 与字段引用）不能为空")]
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
    /// </summary>
    [Required(ErrorMessage = "物理表名（须为 takt_ 前缀业务表，运行时白名单校验）不能为空")]
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 是否主表（驱动 FROM 的第一张表）
    /// </summary>
    public int IsPrimary { get; set; } = 0;

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
// 更新ConfigurableSource DTO
// ========================================

/// <summary>
/// 更新ConfigurableSource DTO
/// 继承 TaktConfigurableSourceCreateDto，添加 ConfigurableSourceId 字段
/// </summary>
public class TaktConfigurableSourceUpdateDto : TaktConfigurableSourceCreateDto
{
    /// <summary>
    /// ConfigurableSourceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableSourceId { get; set; }

}

// ========================================
// ConfigurableSource 排序 DTO
// ========================================

/// <summary>
/// ConfigurableSource 排序更新 DTO
/// </summary>
public class TaktConfigurableSourceSortDto
{
    /// <summary>
    /// ConfigurableSourceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableSourceId { get; set; }

    /// <summary>
    /// 排序号（回填）（多表 FROM 顺序）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）（多表 FROM 顺序）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ConfigurableSource 导入模板行 DTO
/// </summary>
public class TaktConfigurableSourceTemplateDto
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
    /// 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
    /// </summary>
    public string? SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 是否主表（驱动 FROM 的第一张表）
    /// </summary>
    public int? IsPrimary { get; set; }

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
/// ConfigurableSource 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktConfigurableSourceImportDto
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
    /// 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
    /// </summary>
    public string? SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 是否主表（驱动 FROM 的第一张表）
    /// </summary>
    public int? IsPrimary { get; set; }

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
/// ConfigurableSource 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktConfigurableSourceExportDto
{
    /// <summary>
    /// ConfigurableSourceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableSourceId { get; set; }

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
    /// 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
    /// </summary>
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 是否主表（驱动 FROM 的第一张表）
    /// </summary>
    public int IsPrimary { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（多表 FROM 顺序）
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
