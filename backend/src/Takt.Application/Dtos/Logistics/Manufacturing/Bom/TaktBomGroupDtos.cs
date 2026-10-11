// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomGroupDtos.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Auto Generated)
// 功能描述：BomGroup 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBomGroup 生成，请按需审阅）
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
// BomGroup 响应 DTO
// ========================================

/// <summary>
/// BOM组主数据实体（公司级；按类别区分的 BOM/工艺业务组织分组）
/// 对应前端 TaktBomGroupDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBomGroupDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BomGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomGroupId { get; set; }

    /// <summary>
    /// BOM组编码（与员工编码同长 6）
    /// </summary>
    public string BomGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM组名称
    /// </summary>
    public string BomGroupName { get; set; } = string.Empty;

    /// <summary>
    /// BOM组描述
    /// </summary>
    public string? BomGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
    /// </summary>
    public int BomGroupCategory { get; set; } = 0;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int GroupStatus { get; set; } = 0;

}

// ========================================
// BomGroup 查询 DTO
// ========================================

/// <summary>
/// BomGroup 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBomGroupQueryDto : TaktPagedQuery
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
    /// BOM组编码（与员工编码同长 6）
    /// </summary>
    public string? BomGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM组名称
    /// </summary>
    public string? BomGroupName { get; set; } = string.Empty;

    /// <summary>
    /// BOM组描述
    /// </summary>
    public string? BomGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
    /// </summary>
    public int? BomGroupCategory { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? GroupStatus { get; set; }

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
// 创建BomGroup DTO
// ========================================

/// <summary>
/// 创建BomGroup DTO
/// </summary>
public class TaktBomGroupCreateDto
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
    /// BOM组编码（与员工编码同长 6）
    /// </summary>
    [Required(ErrorMessage = "BOM组编码（与员工编码同长 6）不能为空")]
    public string BomGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM组名称
    /// </summary>
    [Required(ErrorMessage = "BOM组名称不能为空")]
    public string BomGroupName { get; set; } = string.Empty;

    /// <summary>
    /// BOM组描述
    /// </summary>
    public string? BomGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
    /// </summary>
    public int BomGroupCategory { get; set; } = 0;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int GroupStatus { get; set; } = 0;

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
// 更新BomGroup DTO
// ========================================

/// <summary>
/// 更新BomGroup DTO
/// 继承 TaktBomGroupCreateDto，添加 BomGroupId 字段
/// </summary>
public class TaktBomGroupUpdateDto : TaktBomGroupCreateDto
{
    /// <summary>
    /// BomGroupID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomGroupId { get; set; }

}

// ========================================
// BomGroup 状态 DTO
// ========================================

/// <summary>
/// BomGroup 状态更新 DTO
/// </summary>
public class TaktBomGroupStatusDto
{
    /// <summary>
    /// BomGroupID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomGroupId { get; set; }

    /// <summary>
    /// BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int GroupStatus { get; set; } = 0;
}

// ========================================
// BomGroup 排序 DTO
// ========================================

/// <summary>
/// BomGroup 排序更新 DTO
/// </summary>
public class TaktBomGroupSortDto
{
    /// <summary>
    /// BomGroupID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomGroupId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// BomGroup 内置 DTO
// ========================================

/// <summary>
/// BomGroup 内置更新 DTO
/// </summary>
public class TaktBomGroupBuiltInDto
{
    /// <summary>
    /// BomGroupID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomGroupId { get; set; }

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
/// BomGroup 导入模板行 DTO
/// </summary>
public class TaktBomGroupTemplateDto
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
    /// BOM组编码（与员工编码同长 6）
    /// </summary>
    public string? BomGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM组名称
    /// </summary>
    public string? BomGroupName { get; set; } = string.Empty;

    /// <summary>
    /// BOM组描述
    /// </summary>
    public string? BomGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
    /// </summary>
    public int? BomGroupCategory { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? GroupStatus { get; set; }

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
/// BomGroup 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBomGroupImportDto
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
    /// BOM组编码（与员工编码同长 6）
    /// </summary>
    public string? BomGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM组名称
    /// </summary>
    public string? BomGroupName { get; set; } = string.Empty;

    /// <summary>
    /// BOM组描述
    /// </summary>
    public string? BomGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
    /// </summary>
    public int? BomGroupCategory { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? GroupStatus { get; set; }

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
/// BomGroup 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBomGroupExportDto
{
    /// <summary>
    /// BomGroupID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomGroupId { get; set; }

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
    /// BOM组编码（与员工编码同长 6）
    /// </summary>
    public string BomGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM组名称
    /// </summary>
    public string BomGroupName { get; set; } = string.Empty;

    /// <summary>
    /// BOM组描述
    /// </summary>
    public string? BomGroupDescription { get; set; } = string.Empty;

    /// <summary>
    /// BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
    /// </summary>
    public int BomGroupCategory { get; set; } = 0;

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// 联系邮箱
    /// </summary>
    public string? ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int GroupStatus { get; set; } = 0;

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
