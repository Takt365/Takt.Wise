// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionTeamEquipmentDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionTeamEquipment 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductionTeamEquipment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Mps;

// ========================================
// ProductionTeamEquipment 响应 DTO
// ========================================

/// <summary>
/// 生产班组设备组明细（主子表；PCBA 线体生产设备及台数）
/// 对应前端 TaktProductionTeamEquipmentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductionTeamEquipmentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductionTeamEquipmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamEquipmentId { get; set; }

    /// <summary>
    /// 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProdTeamId { get; set; }

    /// <summary>
    /// 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
    /// </summary>
    public string? ProdTeamName { get; set; }

    /// <summary>
    /// 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产设备主键（关联 TaktProductionEquipment.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProdEquipId { get; set; }

    /// <summary>
    /// 生产设备主键（关联 TaktProductionEquipment.Id）
    /// </summary>
    public string? ProdEquipName { get; set; }

    /// <summary>
    /// 生产设备编码（冗余快照，与 TaktProductionEquipment.ProdEquipCode 一致）
    /// </summary>
    public string ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备台数（同型号多台时 &gt;1）
    /// </summary>
    public int EquipQuantity { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int TeamEquipStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

}

// ========================================
// ProductionTeamEquipment 查询 DTO
// ========================================

/// <summary>
/// ProductionTeamEquipment 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductionTeamEquipmentQueryDto : TaktPagedQuery
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
    /// 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProdTeamId { get; set; }

    /// <summary>
    /// 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产设备主键（关联 TaktProductionEquipment.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProdEquipId { get; set; }

    /// <summary>
    /// 生产设备编码（冗余快照，与 TaktProductionEquipment.ProdEquipCode 一致）
    /// </summary>
    public string? ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备台数（同型号多台时 &gt;1）
    /// </summary>
    public int? EquipQuantity { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? TeamEquipStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建ProductionTeamEquipment DTO
// ========================================

/// <summary>
/// 创建ProductionTeamEquipment DTO
/// </summary>
public class TaktProductionTeamEquipmentCreateDto
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
    /// 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProdTeamId { get; set; }

    /// <summary>
    /// 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
    /// </summary>
    [Required(ErrorMessage = "班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）不能为空")]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产设备主键（关联 TaktProductionEquipment.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProdEquipId { get; set; }

    /// <summary>
    /// 生产设备编码（冗余快照，与 TaktProductionEquipment.ProdEquipCode 一致）
    /// </summary>
    public string ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备台数（同型号多台时 &gt;1）
    /// </summary>
    public int EquipQuantity { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int TeamEquipStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新ProductionTeamEquipment DTO
// ========================================

/// <summary>
/// 更新ProductionTeamEquipment DTO
/// 继承 TaktProductionTeamEquipmentCreateDto，添加 ProductionTeamEquipmentId 字段
/// </summary>
public class TaktProductionTeamEquipmentUpdateDto : TaktProductionTeamEquipmentCreateDto
{
    /// <summary>
    /// ProductionTeamEquipmentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamEquipmentId { get; set; }

}

// ========================================
// ProductionTeamEquipment 状态 DTO
// ========================================

/// <summary>
/// ProductionTeamEquipment 状态更新 DTO
/// </summary>
public class TaktProductionTeamEquipmentStatusDto
{
    /// <summary>
    /// ProductionTeamEquipmentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamEquipmentId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable；1=启用，0=禁用）不能为空")]
    public int TeamEquipStatus { get; set; } = 0;
}

// ========================================
// ProductionTeamEquipment 作废 DTO
// ========================================

/// <summary>
/// ProductionTeamEquipment 作废/撤销作废 DTO
/// </summary>
public class TaktProductionTeamEquipmentObsoleteDto
{
    /// <summary>
    /// ProductionTeamEquipmentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamEquipmentId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductionTeamEquipment 导入模板行 DTO
/// </summary>
public class TaktProductionTeamEquipmentTemplateDto
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
    /// 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProdTeamId { get; set; }

    /// <summary>
    /// 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产设备主键（关联 TaktProductionEquipment.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProdEquipId { get; set; }

    /// <summary>
    /// 生产设备编码（冗余快照，与 TaktProductionEquipment.ProdEquipCode 一致）
    /// </summary>
    public string? ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备台数（同型号多台时 &gt;1）
    /// </summary>
    public int? EquipQuantity { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? TeamEquipStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// ProductionTeamEquipment 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductionTeamEquipmentImportDto
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
    /// 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProdTeamId { get; set; }

    /// <summary>
    /// 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 生产设备主键（关联 TaktProductionEquipment.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProdEquipId { get; set; }

    /// <summary>
    /// 生产设备编码（冗余快照，与 TaktProductionEquipment.ProdEquipCode 一致）
    /// </summary>
    public string? ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备台数（同型号多台时 &gt;1）
    /// </summary>
    public int? EquipQuantity { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int? TeamEquipStatus { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// ProductionTeamEquipment 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductionTeamEquipmentExportDto
{
    /// <summary>
    /// ProductionTeamEquipmentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamEquipmentId { get; set; }

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
    /// 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProdTeamId { get; set; }

    /// <summary>
    /// 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产设备主键（关联 TaktProductionEquipment.Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProdEquipId { get; set; }

    /// <summary>
    /// 生产设备编码（冗余快照，与 TaktProductionEquipment.ProdEquipCode 一致）
    /// </summary>
    public string ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 设备台数（同型号多台时 &gt;1）
    /// </summary>
    public int EquipQuantity { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    public int TeamEquipStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
