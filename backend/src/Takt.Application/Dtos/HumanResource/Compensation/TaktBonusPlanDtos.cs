// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Compensation
// 文件名称：TaktBonusPlanDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：BonusPlan 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBonusPlan 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Compensation;

// ========================================
// BonusPlan 响应 DTO
// ========================================

/// <summary>
/// 奖金方案（现金奖金）
/// 对应前端 TaktBonusPlanDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBonusPlanDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BonusPlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BonusPlanId { get; set; }


    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int PlanStatus { get; set; } = 0;

}

// ========================================
// BonusPlan 查询 DTO
// ========================================

/// <summary>
/// BonusPlan 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBonusPlanQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案编码（租户+公司内唯一）
    /// </summary>
    public string? PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string? PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 奖金类型（字典 humanresource_compensation_bonus_type）
    /// </summary>
    public int? BonusType { get; set; }

    /// <summary>
    /// 计算方式（字典 humanresource_compensation_bonus_calc_method）
    /// </summary>
    public int? CalcMethod { get; set; }

    /// <summary>
    /// 关联计算公式 ID（按公式计算时引用 TaktSalaryFormula）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryFormulaId { get; set; }

    /// <summary>
    /// 默认奖金金额或基数（元）
    /// </summary>
    public decimal? DefaultAmount { get; set; }

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 方案说明
    /// </summary>
    public string? BonusPlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? PlanStatus { get; set; }

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
// 创建BonusPlan DTO
// ========================================

/// <summary>
/// 创建BonusPlan DTO
/// </summary>
public class TaktBonusPlanCreateDto
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
    /// 方案编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "方案编码（租户+公司内唯一）不能为空")]
    public string PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    [Required(ErrorMessage = "方案名称不能为空")]
    public string PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 奖金类型（字典 humanresource_compensation_bonus_type）
    /// </summary>
    public int BonusType { get; set; } = 0;

    /// <summary>
    /// 计算方式（字典 humanresource_compensation_bonus_calc_method）
    /// </summary>
    public int CalcMethod { get; set; } = 0;

    /// <summary>
    /// 关联计算公式 ID（按公式计算时引用 TaktSalaryFormula）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryFormulaId { get; set; }

    /// <summary>
    /// 默认奖金金额或基数（元）
    /// </summary>
    public decimal DefaultAmount { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 方案说明
    /// </summary>
    public string? BonusPlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int PlanStatus { get; set; } = 0;

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
// 更新BonusPlan DTO
// ========================================

/// <summary>
/// 更新BonusPlan DTO
/// 继承 TaktBonusPlanCreateDto，添加 BonusPlanId 字段
/// </summary>
public class TaktBonusPlanUpdateDto : TaktBonusPlanCreateDto
{
    /// <summary>
    /// BonusPlanID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BonusPlanId { get; set; }

}

// ========================================
// BonusPlan 状态 DTO
// ========================================

/// <summary>
/// BonusPlan 状态更新 DTO
/// </summary>
public class TaktBonusPlanStatusDto
{
    /// <summary>
    /// BonusPlanID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BonusPlanId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable）不能为空")]
    public int PlanStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// BonusPlan 导入模板行 DTO
/// </summary>
public class TaktBonusPlanTemplateDto
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
    /// 方案编码（租户+公司内唯一）
    /// </summary>
    public string? PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string? PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 奖金类型（字典 humanresource_compensation_bonus_type）
    /// </summary>
    public int? BonusType { get; set; }

    /// <summary>
    /// 计算方式（字典 humanresource_compensation_bonus_calc_method）
    /// </summary>
    public int? CalcMethod { get; set; }

    /// <summary>
    /// 关联计算公式 ID（按公式计算时引用 TaktSalaryFormula）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryFormulaId { get; set; }

    /// <summary>
    /// 默认奖金金额或基数（元）
    /// </summary>
    public decimal? DefaultAmount { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 方案说明
    /// </summary>
    public string? BonusPlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? PlanStatus { get; set; }

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
/// BonusPlan 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBonusPlanImportDto
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
    /// 方案编码（租户+公司内唯一）
    /// </summary>
    public string? PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string? PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 奖金类型（字典 humanresource_compensation_bonus_type）
    /// </summary>
    public int? BonusType { get; set; }

    /// <summary>
    /// 计算方式（字典 humanresource_compensation_bonus_calc_method）
    /// </summary>
    public int? CalcMethod { get; set; }

    /// <summary>
    /// 关联计算公式 ID（按公式计算时引用 TaktSalaryFormula）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryFormulaId { get; set; }

    /// <summary>
    /// 默认奖金金额或基数（元）
    /// </summary>
    public decimal? DefaultAmount { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 方案说明
    /// </summary>
    public string? BonusPlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? PlanStatus { get; set; }

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
/// BonusPlan 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBonusPlanExportDto
{
    /// <summary>
    /// BonusPlanID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BonusPlanId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案编码（租户+公司内唯一）
    /// </summary>
    public string PlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 方案名称
    /// </summary>
    public string PlanName { get; set; } = string.Empty;

    /// <summary>
    /// 奖金类型（字典 humanresource_compensation_bonus_type）
    /// </summary>
    public int BonusType { get; set; } = 0;

    /// <summary>
    /// 计算方式（字典 humanresource_compensation_bonus_calc_method）
    /// </summary>
    public int CalcMethod { get; set; } = 0;

    /// <summary>
    /// 关联计算公式 ID（按公式计算时引用 TaktSalaryFormula）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryFormulaId { get; set; }

    /// <summary>
    /// 默认奖金金额或基数（元）
    /// </summary>
    public decimal DefaultAmount { get; set; }

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 方案说明
    /// </summary>
    public string? BonusPlanDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int PlanStatus { get; set; } = 0;

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
