// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItemArgumentDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：RoutingItemArgument 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktRoutingItemArgument 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

// ========================================
// RoutingItemArgument 响应 DTO
// ========================================

/// <summary>
/// 工艺路线工序参数定义实体
/// 对应前端 TaktRoutingItemArgumentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktRoutingItemArgumentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// RoutingItemArgumentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemArgumentId { get; set; }

    /// <summary>
    /// 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 工艺路线明细 名称（填充字段）
    /// </summary>
    public string? RoutingItemName { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    public string ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 参数名称
    /// </summary>
    public string ParamName { get; set; } = string.Empty;

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? ParamUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准值
    /// </summary>
    public decimal? StandardValue { get; set; }

    /// <summary>
    /// 下限
    /// </summary>
    public decimal? LowerLimit { get; set; }

    /// <summary>
    /// 上限
    /// </summary>
    public decimal? UpperLimit { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工序
    /// （主表：TaktRoutingItem）
    /// </summary>
    public TaktRoutingItemDto? RoutingItem { get; set; }

}

// ========================================
// RoutingItemArgument 查询 DTO
// ========================================

/// <summary>
/// RoutingItemArgument 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktRoutingItemArgumentQueryDto : TaktPagedQuery
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
    /// 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    public string? ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 参数名称
    /// </summary>
    public string? ParamName { get; set; } = string.Empty;

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? ParamUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准值
    /// </summary>
    public decimal? StandardValue { get; set; }

    /// <summary>
    /// 下限
    /// </summary>
    public decimal? LowerLimit { get; set; }

    /// <summary>
    /// 上限
    /// </summary>
    public decimal? UpperLimit { get; set; }

    /// <summary>
    /// 排序号（回填）
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
// 创建RoutingItemArgument DTO
// ========================================

/// <summary>
/// 创建RoutingItemArgument DTO
/// </summary>
public class TaktRoutingItemArgumentCreateDto
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
    /// 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    [Required(ErrorMessage = "参数编码不能为空")]
    public string ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 参数名称
    /// </summary>
    [Required(ErrorMessage = "参数名称不能为空")]
    public string ParamName { get; set; } = string.Empty;

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? ParamUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准值
    /// </summary>
    public decimal? StandardValue { get; set; }

    /// <summary>
    /// 下限
    /// </summary>
    public decimal? LowerLimit { get; set; }

    /// <summary>
    /// 上限
    /// </summary>
    public decimal? UpperLimit { get; set; }

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
// 更新RoutingItemArgument DTO
// ========================================

/// <summary>
/// 更新RoutingItemArgument DTO
/// 继承 TaktRoutingItemArgumentCreateDto，添加 RoutingItemArgumentId 字段
/// </summary>
public class TaktRoutingItemArgumentUpdateDto : TaktRoutingItemArgumentCreateDto
{
    /// <summary>
    /// RoutingItemArgumentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemArgumentId { get; set; }

}

// ========================================
// RoutingItemArgument 排序 DTO
// ========================================

/// <summary>
/// RoutingItemArgument 排序更新 DTO
/// </summary>
public class TaktRoutingItemArgumentSortDto
{
    /// <summary>
    /// RoutingItemArgumentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemArgumentId { get; set; }

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
/// RoutingItemArgument 导入模板行 DTO
/// </summary>
public class TaktRoutingItemArgumentTemplateDto
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
    /// 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    public string? ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 参数名称
    /// </summary>
    public string? ParamName { get; set; } = string.Empty;

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? ParamUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准值
    /// </summary>
    public decimal? StandardValue { get; set; }

    /// <summary>
    /// 下限
    /// </summary>
    public decimal? LowerLimit { get; set; }

    /// <summary>
    /// 上限
    /// </summary>
    public decimal? UpperLimit { get; set; }

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
/// RoutingItemArgument 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktRoutingItemArgumentImportDto
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
    /// 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    public string? ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 参数名称
    /// </summary>
    public string? ParamName { get; set; } = string.Empty;

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? ParamUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准值
    /// </summary>
    public decimal? StandardValue { get; set; }

    /// <summary>
    /// 下限
    /// </summary>
    public decimal? LowerLimit { get; set; }

    /// <summary>
    /// 上限
    /// </summary>
    public decimal? UpperLimit { get; set; }

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
/// RoutingItemArgument 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktRoutingItemArgumentExportDto
{
    /// <summary>
    /// RoutingItemArgumentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemArgumentId { get; set; }

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
    /// 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    public string ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 参数名称
    /// </summary>
    public string ParamName { get; set; } = string.Empty;

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    public string? ParamUnit { get; set; } = string.Empty;

    /// <summary>
    /// 标准值
    /// </summary>
    public decimal? StandardValue { get; set; }

    /// <summary>
    /// 下限
    /// </summary>
    public decimal? LowerLimit { get; set; }

    /// <summary>
    /// 上限
    /// </summary>
    public decimal? UpperLimit { get; set; }

    /// <summary>
    /// 排序号（回填）
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
