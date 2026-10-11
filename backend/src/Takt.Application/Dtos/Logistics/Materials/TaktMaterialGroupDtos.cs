// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialGroupDtos.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialGroup 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterialGroup 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// MaterialGroup 响应 DTO
// ========================================

/// <summary>
/// Takt物料组主数据实体（租户级） 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase；仅租户）
/// 对应前端 TaktMaterialGroupDto
/// 继承 TaktTenantCoreDtoBase
/// </summary>
public class TaktMaterialGroupDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// MaterialGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialGroupId { get; set; }

    /// <summary>
    /// 物料组编码（group_code；租户内唯一；与物料 material_group 对齐）
    /// </summary>
    public string MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组名称（group_name）
    /// </summary>
    public string MaterialGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（sort；越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 物料组描述（description）
    /// </summary>
    public string? MaterialGroupDescription { get; set; } = string.Empty;

}

// ========================================
// MaterialGroup 查询 DTO
// ========================================

/// <summary>
/// MaterialGroup 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialGroupQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组编码（group_code；租户内唯一；与物料 material_group 对齐）
    /// </summary>
    public string? MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组名称（group_name）
    /// </summary>
    public string? MaterialGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（sort；越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 物料组描述（description）
    /// </summary>
    public string? MaterialGroupDescription { get; set; } = string.Empty;

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
// 创建MaterialGroup DTO
// ========================================

/// <summary>
/// 创建MaterialGroup DTO
/// </summary>
public class TaktMaterialGroupCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组编码（group_code；租户内唯一；与物料 material_group 对齐）
    /// </summary>
    [Required(ErrorMessage = "物料组编码（group_code；租户内唯一；与物料 material_group 对齐）不能为空")]
    public string MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组名称（group_name）
    /// </summary>
    [Required(ErrorMessage = "物料组名称（group_name）不能为空")]
    public string MaterialGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 物料组描述（description）
    /// </summary>
    public string? MaterialGroupDescription { get; set; } = string.Empty;

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
// 更新MaterialGroup DTO
// ========================================

/// <summary>
/// 更新MaterialGroup DTO
/// 继承 TaktMaterialGroupCreateDto，添加 MaterialGroupId 字段
/// </summary>
public class TaktMaterialGroupUpdateDto : TaktMaterialGroupCreateDto
{
    /// <summary>
    /// MaterialGroupID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialGroupId { get; set; }

}

// ========================================
// MaterialGroup 排序 DTO
// ========================================

/// <summary>
/// MaterialGroup 排序更新 DTO
/// </summary>
public class TaktMaterialGroupSortDto
{
    /// <summary>
    /// MaterialGroupID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialGroupId { get; set; }

    /// <summary>
    /// 排序号（回填）（sort；越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（sort；越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaterialGroup 导入模板行 DTO
/// </summary>
public class TaktMaterialGroupTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组编码（group_code；租户内唯一；与物料 material_group 对齐）
    /// </summary>
    public string? MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组名称（group_name）
    /// </summary>
    public string? MaterialGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 物料组描述（description）
    /// </summary>
    public string? MaterialGroupDescription { get; set; } = string.Empty;

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
/// MaterialGroup 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialGroupImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组编码（group_code；租户内唯一；与物料 material_group 对齐）
    /// </summary>
    public string? MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组名称（group_name）
    /// </summary>
    public string? MaterialGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 物料组描述（description）
    /// </summary>
    public string? MaterialGroupDescription { get; set; } = string.Empty;

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
/// MaterialGroup 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialGroupExportDto
{
    /// <summary>
    /// MaterialGroupID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialGroupId { get; set; }

    /// <summary>
    /// 物料组编码（group_code；租户内唯一；与物料 material_group 对齐）
    /// </summary>
    public string MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料组名称（group_name）
    /// </summary>
    public string MaterialGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（sort；越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 物料组描述（description）
    /// </summary>
    public string? MaterialGroupDescription { get; set; } = string.Empty;

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
