// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktDictTypeDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：DictType 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktDictType 生成，请按需审阅）
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
// DictType 响应 DTO
// ========================================

/// <summary>
/// 字典类型实体 用于定义系统中使用的各种字典分类，如：订单状态、用户类型、审批状态等 租户级实体：字典类型在租户内共享，不需要公司隔离 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// 对应前端 TaktDictTypeDto
/// 继承 TaktTenantCoreDtoBase
/// </summary>
public class TaktDictTypeDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// DictTypeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictTypeId { get; set; }

    /// <summary>
    /// 字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，varchar Length=140；如 accounting_controlling_cost_center_type）
    /// </summary>
    public string DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型名称（如：订单状态、用户类型）
    /// </summary>
    public string DictTypeName { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（字典 sys_data_source；0=系统表 1=SQL查询）
    /// </summary>
    public int DataSource { get; set; } = 0;

    /// <summary>
    /// SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
    /// </summary>
    public string? DictScript { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int DictStatus { get; set; } = 0;

    /// <summary>
    /// 字典数据列表（一对多关联）
    /// （子表：TaktDictData）
    /// </summary>
    public List<TaktDictDataDto>? DictDataList { get; set; }

}

// ========================================
// DictType 查询 DTO
// ========================================

/// <summary>
/// DictType 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktDictTypeQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，varchar Length=140；如 accounting_controlling_cost_center_type）
    /// </summary>
    public string? DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型名称（如：订单状态、用户类型）
    /// </summary>
    public string? DictTypeName { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（字典 sys_data_source；0=系统表 1=SQL查询）
    /// </summary>
    public int? DataSource { get; set; }

    /// <summary>
    /// SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
    /// </summary>
    public string? DictScript { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? DictStatus { get; set; }

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
// 创建DictType DTO
// ========================================

/// <summary>
/// 创建DictType DTO
/// </summary>
public class TaktDictTypeCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，varchar Length=140；如 accounting_controlling_cost_center_type）
    /// </summary>
    [Required(ErrorMessage = "字典类型编码不能为空")]
    public string DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型名称（如：订单状态、用户类型）
    /// </summary>
    [Required(ErrorMessage = "字典类型名称（如：订单状态、用户类型）不能为空")]
    public string DictTypeName { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（字典 sys_data_source；0=系统表 1=SQL查询）
    /// </summary>
    public int DataSource { get; set; } = 0;

    /// <summary>
    /// SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
    /// </summary>
    public string? DictScript { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int DictStatus { get; set; } = 0;

    /// <summary>
    /// 字典数据列表（一对多关联）（子表，级联保存）
    /// </summary>
    public List<TaktDictDataCreateDto>? DictDataList { get; set; }

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
// 更新DictType DTO
// ========================================

/// <summary>
/// 更新DictType DTO
/// 继承 TaktDictTypeCreateDto，添加 DictTypeId 字段
/// </summary>
public class TaktDictTypeUpdateDto : TaktDictTypeCreateDto
{
    /// <summary>
    /// DictTypeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictTypeId { get; set; }

    /// <summary>
    /// 字典数据列表（一对多关联）（子表，级联保存）
    /// </summary>
    public new List<TaktDictDataUpdateDto>? DictDataList { get; set; }

}

// ========================================
// DictType 状态 DTO
// ========================================

/// <summary>
/// DictType 状态更新 DTO
/// </summary>
public class TaktDictTypeStatusDto
{
    /// <summary>
    /// DictTypeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictTypeId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用 0=禁用）不能为空")]
    public int DictStatus { get; set; } = 0;
}

// ========================================
// DictType 排序 DTO
// ========================================

/// <summary>
/// DictType 排序更新 DTO
/// </summary>
public class TaktDictTypeSortDto
{
    /// <summary>
    /// DictTypeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictTypeId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// DictType 内置 DTO
// ========================================

/// <summary>
/// DictType 内置更新 DTO
/// </summary>
public class TaktDictTypeBuiltInDto
{
    /// <summary>
    /// DictTypeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictTypeId { get; set; }

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
/// DictType 导入模板行 DTO
/// </summary>
public class TaktDictTypeTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，varchar Length=140；如 accounting_controlling_cost_center_type）
    /// </summary>
    public string? DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型名称（如：订单状态、用户类型）
    /// </summary>
    public string? DictTypeName { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（字典 sys_data_source；0=系统表 1=SQL查询）
    /// </summary>
    public int? DataSource { get; set; }

    /// <summary>
    /// SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
    /// </summary>
    public string? DictScript { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? DictStatus { get; set; }

    /// <summary>
    /// 字典数据列表（一对多关联）（子表，级联保存）
    /// </summary>
    public List<TaktDictDataCreateDto>? DictDataList { get; set; }

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
/// DictType 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktDictTypeImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，varchar Length=140；如 accounting_controlling_cost_center_type）
    /// </summary>
    public string? DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型名称（如：订单状态、用户类型）
    /// </summary>
    public string? DictTypeName { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（字典 sys_data_source；0=系统表 1=SQL查询）
    /// </summary>
    public int? DataSource { get; set; }

    /// <summary>
    /// SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
    /// </summary>
    public string? DictScript { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int? DictStatus { get; set; }

    /// <summary>
    /// 字典数据列表（一对多关联）（子表，级联保存）
    /// </summary>
    public List<TaktDictDataCreateDto>? DictDataList { get; set; }

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
/// DictType 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktDictTypeExportDto
{
    /// <summary>
    /// DictTypeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DictTypeId { get; set; }

    /// <summary>
    /// 字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，varchar Length=140；如 accounting_controlling_cost_center_type）
    /// </summary>
    public string DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型名称（如：订单状态、用户类型）
    /// </summary>
    public string DictTypeName { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（字典 sys_data_source；0=系统表 1=SQL查询）
    /// </summary>
    public int DataSource { get; set; } = 0;

    /// <summary>
    /// SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
    /// </summary>
    public string? DictScript { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    public int DictStatus { get; set; } = 0;

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
