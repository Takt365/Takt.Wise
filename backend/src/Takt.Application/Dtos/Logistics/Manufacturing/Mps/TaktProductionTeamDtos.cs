// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionTeamDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionTeam 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductionTeam 生成，请按需审阅）
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
// ProductionTeam 响应 DTO
// ========================================

/// <summary>
/// 生产班组实体（生产线班组主数据）
/// 对应前端 TaktProductionTeamDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductionTeamDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductionTeamID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamId { get; set; }

    /// <summary>
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）
    /// </summary>
    public string TeamCategory { get; set; } = string.Empty;

    /// <summary>
    /// 班组长姓名（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeaderName { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用）
    /// </summary>
    public int TeamStatus { get; set; } = 0;

    /// <summary>
    /// 设备组明细（PCBA 线体 SMT/AI/手插等生产设备及台数）
    /// （子表：TaktProductionTeamEquipment）
    /// </summary>
    public List<TaktProductionTeamEquipmentDto>? TeamEquipmentList { get; set; }

}

// ========================================
// ProductionTeam 查询 DTO
// ========================================

/// <summary>
/// ProductionTeam 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductionTeamQueryDto : TaktPagedQuery
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
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    public string? TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）
    /// </summary>
    public string? TeamCategory { get; set; } = string.Empty;

    /// <summary>
    /// 班组长姓名（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeaderName { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用）
    /// </summary>
    public int? TeamStatus { get; set; }

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
// 创建ProductionTeam DTO
// ========================================

/// <summary>
/// 创建ProductionTeam DTO
/// </summary>
public class TaktProductionTeamCreateDto
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
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    [Required(ErrorMessage = "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）不能为空")]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    [Required(ErrorMessage = "班组名称（显示名称，如：SMT一班、手插二班等）不能为空")]
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）
    /// </summary>
    [Required(ErrorMessage = "班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）不能为空")]
    public string TeamCategory { get; set; } = string.Empty;

    /// <summary>
    /// 班组长姓名（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeaderName { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用）
    /// </summary>
    public int TeamStatus { get; set; } = 0;

    /// <summary>
    /// 设备组明细（PCBA 线体 SMT/AI/手插等生产设备及台数）（子表，级联保存）
    /// </summary>
    public List<TaktProductionTeamEquipmentCreateDto>? TeamEquipmentList { get; set; }

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
// 更新ProductionTeam DTO
// ========================================

/// <summary>
/// 更新ProductionTeam DTO
/// 继承 TaktProductionTeamCreateDto，添加 ProductionTeamId 字段
/// </summary>
public class TaktProductionTeamUpdateDto : TaktProductionTeamCreateDto
{
    /// <summary>
    /// ProductionTeamID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamId { get; set; }

    /// <summary>
    /// 设备组明细（PCBA 线体 SMT/AI/手插等生产设备及台数）（子表，级联保存）
    /// </summary>
    public new List<TaktProductionTeamEquipmentUpdateDto>? TeamEquipmentList { get; set; }

}

// ========================================
// ProductionTeam 状态 DTO
// ========================================

/// <summary>
/// ProductionTeam 状态更新 DTO
/// </summary>
public class TaktProductionTeamStatusDto
{
    /// <summary>
    /// ProductionTeamID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamId { get; set; }

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用）
    /// </summary>
    [Required(ErrorMessage = "启用状态（字典 sys_normal_disable；0=禁用，1=启用）不能为空")]
    public int TeamStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductionTeam 导入模板行 DTO
/// </summary>
public class TaktProductionTeamTemplateDto
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
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    public string? TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）
    /// </summary>
    public string? TeamCategory { get; set; } = string.Empty;

    /// <summary>
    /// 班组长姓名（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeaderName { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用）
    /// </summary>
    public int? TeamStatus { get; set; }

    /// <summary>
    /// 设备组明细（PCBA 线体 SMT/AI/手插等生产设备及台数）（子表，级联保存）
    /// </summary>
    public List<TaktProductionTeamEquipmentCreateDto>? TeamEquipmentList { get; set; }

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
/// ProductionTeam 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductionTeamImportDto
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
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    public string? TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）
    /// </summary>
    public string? TeamCategory { get; set; } = string.Empty;

    /// <summary>
    /// 班组长姓名（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeaderName { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int? ShiftNo { get; set; }

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用）
    /// </summary>
    public int? TeamStatus { get; set; }

    /// <summary>
    /// 设备组明细（PCBA 线体 SMT/AI/手插等生产设备及台数）（子表，级联保存）
    /// </summary>
    public List<TaktProductionTeamEquipmentCreateDto>? TeamEquipmentList { get; set; }

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
/// ProductionTeam 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductionTeamExportDto
{
    /// <summary>
    /// ProductionTeamID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionTeamId { get; set; }

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
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）
    /// </summary>
    public string TeamCategory { get; set; } = string.Empty;

    /// <summary>
    /// 班组长姓名（选项 TaktEmployees/options，存员工姓名或工号）
    /// </summary>
    public string? TeamLeaderName { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    public int ShiftNo { get; set; } = 0;

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用）
    /// </summary>
    public int TeamStatus { get; set; } = 0;

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
