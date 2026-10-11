// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Statistics.QuickQuery
// 文件名称：TaktConfigurableJoinDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ConfigurableJoin 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktConfigurableJoin 生成，请按需审阅）
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
// ConfigurableJoin 响应 DTO
// ========================================

/// <summary>
/// 定制报表多表关联定义
/// 对应前端 TaktConfigurableJoinDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktConfigurableJoinDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ConfigurableJoinID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableJoinId { get; set; }

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
    /// 关联类型（内/左/右/全连接）
    /// </summary>
    public int JoinType { get; set; } = 0;

    /// <summary>
    /// 左表数据源别名
    /// </summary>
    public string LeftSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 左表关联列名
    /// </summary>
    public string LeftColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 右表数据源别名
    /// </summary>
    public string RightSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 右表关联列名
    /// </summary>
    public string RightColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（JOIN 应用顺序）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 关联的定制报表主表
    /// （主表：TaktConfigurable）
    /// </summary>
    public TaktConfigurableDto? Configurable { get; set; }

}

// ========================================
// ConfigurableJoin 查询 DTO
// ========================================

/// <summary>
/// ConfigurableJoin 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktConfigurableJoinQueryDto : TaktPagedQuery
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
    /// 关联类型（内/左/右/全连接）
    /// </summary>
    public int? JoinType { get; set; }

    /// <summary>
    /// 左表数据源别名
    /// </summary>
    public string? LeftSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 左表关联列名
    /// </summary>
    public string? LeftColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 右表数据源别名
    /// </summary>
    public string? RightSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 右表关联列名
    /// </summary>
    public string? RightColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（JOIN 应用顺序）
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
// 创建ConfigurableJoin DTO
// ========================================

/// <summary>
/// 创建ConfigurableJoin DTO
/// </summary>
public class TaktConfigurableJoinCreateDto
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
    /// 关联类型（内/左/右/全连接）
    /// </summary>
    public int JoinType { get; set; } = 0;

    /// <summary>
    /// 左表数据源别名
    /// </summary>
    [Required(ErrorMessage = "左表数据源别名不能为空")]
    public string LeftSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 左表关联列名
    /// </summary>
    [Required(ErrorMessage = "左表关联列名不能为空")]
    public string LeftColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 右表数据源别名
    /// </summary>
    [Required(ErrorMessage = "右表数据源别名不能为空")]
    public string RightSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 右表关联列名
    /// </summary>
    [Required(ErrorMessage = "右表关联列名不能为空")]
    public string RightColumnName { get; set; } = string.Empty;

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
// 更新ConfigurableJoin DTO
// ========================================

/// <summary>
/// 更新ConfigurableJoin DTO
/// 继承 TaktConfigurableJoinCreateDto，添加 ConfigurableJoinId 字段
/// </summary>
public class TaktConfigurableJoinUpdateDto : TaktConfigurableJoinCreateDto
{
    /// <summary>
    /// ConfigurableJoinID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableJoinId { get; set; }

}

// ========================================
// ConfigurableJoin 排序 DTO
// ========================================

/// <summary>
/// ConfigurableJoin 排序更新 DTO
/// </summary>
public class TaktConfigurableJoinSortDto
{
    /// <summary>
    /// ConfigurableJoinID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableJoinId { get; set; }

    /// <summary>
    /// 排序号（回填）（JOIN 应用顺序）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）（JOIN 应用顺序）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ConfigurableJoin 导入模板行 DTO
/// </summary>
public class TaktConfigurableJoinTemplateDto
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
    /// 关联类型（内/左/右/全连接）
    /// </summary>
    public int? JoinType { get; set; }

    /// <summary>
    /// 左表数据源别名
    /// </summary>
    public string? LeftSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 左表关联列名
    /// </summary>
    public string? LeftColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 右表数据源别名
    /// </summary>
    public string? RightSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 右表关联列名
    /// </summary>
    public string? RightColumnName { get; set; } = string.Empty;

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
/// ConfigurableJoin 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktConfigurableJoinImportDto
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
    /// 关联类型（内/左/右/全连接）
    /// </summary>
    public int? JoinType { get; set; }

    /// <summary>
    /// 左表数据源别名
    /// </summary>
    public string? LeftSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 左表关联列名
    /// </summary>
    public string? LeftColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 右表数据源别名
    /// </summary>
    public string? RightSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 右表关联列名
    /// </summary>
    public string? RightColumnName { get; set; } = string.Empty;

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
/// ConfigurableJoin 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktConfigurableJoinExportDto
{
    /// <summary>
    /// ConfigurableJoinID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableJoinId { get; set; }

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
    /// 关联类型（内/左/右/全连接）
    /// </summary>
    public int JoinType { get; set; } = 0;

    /// <summary>
    /// 左表数据源别名
    /// </summary>
    public string LeftSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 左表关联列名
    /// </summary>
    public string LeftColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 右表数据源别名
    /// </summary>
    public string RightSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 右表关联列名
    /// </summary>
    public string RightColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（JOIN 应用顺序）
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
