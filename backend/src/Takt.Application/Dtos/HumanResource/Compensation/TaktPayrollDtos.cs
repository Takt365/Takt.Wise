// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Compensation
// 文件名称：TaktPayrollDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：Payroll 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPayroll 生成，请按需审阅）
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
// Payroll 响应 DTO
// ========================================

/// <summary>
/// 薪酬体系（现金报酬方案头；组成项引用 TaktSalaryItem，不另建多种薪资实体）
/// 对应前端 TaktPayrollDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPayrollDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PayrollID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayrollId { get; set; }


    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int PayrollStatus { get; set; } = 0;

}

// ========================================
// Payroll 查询 DTO
// ========================================

/// <summary>
/// Payroll 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPayrollQueryDto : TaktPagedQuery
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
    /// 薪酬体系编码（租户+公司内唯一）
    /// </summary>
    public string? PayrollCode { get; set; } = string.Empty;

    /// <summary>
    /// 薪酬体系名称
    /// </summary>
    public string? PayrollName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪级表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayScaleId { get; set; }

    /// <summary>
    /// 默认公式方案编码（整单工资核算，见 TaktSalaryFormula.set_code）
    /// </summary>
    public string? FormulaSetCode { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 失效日期（范围查询-开始）
    /// </summary>
    public DateTime? ExpiryDateStart { get; set; }

    /// <summary>
    /// 失效日期（范围查询-结束）
    /// </summary>
    public DateTime? ExpiryDateEnd { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string? PayrollDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? PayrollStatus { get; set; }

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
// 创建Payroll DTO
// ========================================

/// <summary>
/// 创建Payroll DTO
/// </summary>
public class TaktPayrollCreateDto
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
    /// 薪酬体系编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "薪酬体系编码（租户+公司内唯一）不能为空")]
    public string PayrollCode { get; set; } = string.Empty;

    /// <summary>
    /// 薪酬体系名称
    /// </summary>
    [Required(ErrorMessage = "薪酬体系名称不能为空")]
    public string PayrollName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪级表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayScaleId { get; set; }

    /// <summary>
    /// 默认公式方案编码（整单工资核算，见 TaktSalaryFormula.set_code）
    /// </summary>
    public string? FormulaSetCode { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string? PayrollDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int PayrollStatus { get; set; } = 0;

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
// 更新Payroll DTO
// ========================================

/// <summary>
/// 更新Payroll DTO
/// 继承 TaktPayrollCreateDto，添加 PayrollId 字段
/// </summary>
public class TaktPayrollUpdateDto : TaktPayrollCreateDto
{
    /// <summary>
    /// PayrollID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayrollId { get; set; }

}

// ========================================
// Payroll 状态 DTO
// ========================================

/// <summary>
/// Payroll 状态更新 DTO
/// </summary>
public class TaktPayrollStatusDto
{
    /// <summary>
    /// PayrollID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayrollId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable）不能为空")]
    public int PayrollStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Payroll 导入模板行 DTO
/// </summary>
public class TaktPayrollTemplateDto
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
    /// 薪酬体系编码（租户+公司内唯一）
    /// </summary>
    public string? PayrollCode { get; set; } = string.Empty;

    /// <summary>
    /// 薪酬体系名称
    /// </summary>
    public string? PayrollName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪级表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayScaleId { get; set; }

    /// <summary>
    /// 默认公式方案编码（整单工资核算，见 TaktSalaryFormula.set_code）
    /// </summary>
    public string? FormulaSetCode { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string? PayrollDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? PayrollStatus { get; set; }

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
/// Payroll 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPayrollImportDto
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
    /// 薪酬体系编码（租户+公司内唯一）
    /// </summary>
    public string? PayrollCode { get; set; } = string.Empty;

    /// <summary>
    /// 薪酬体系名称
    /// </summary>
    public string? PayrollName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪级表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayScaleId { get; set; }

    /// <summary>
    /// 默认公式方案编码（整单工资核算，见 TaktSalaryFormula.set_code）
    /// </summary>
    public string? FormulaSetCode { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string? PayrollDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? PayrollStatus { get; set; }

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
/// Payroll 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPayrollExportDto
{
    /// <summary>
    /// PayrollID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayrollId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 薪酬体系编码（租户+公司内唯一）
    /// </summary>
    public string PayrollCode { get; set; } = string.Empty;

    /// <summary>
    /// 薪酬体系名称
    /// </summary>
    public string PayrollName { get; set; } = string.Empty;

    /// <summary>
    /// 关联薪级表 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayScaleId { get; set; }

    /// <summary>
    /// 默认公式方案编码（整单工资核算，见 TaktSalaryFormula.set_code）
    /// </summary>
    public string? FormulaSetCode { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string? PayrollDescription { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int PayrollStatus { get; set; } = 0;

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
