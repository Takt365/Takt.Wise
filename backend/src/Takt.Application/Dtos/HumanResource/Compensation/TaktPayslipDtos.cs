// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Compensation
// 文件名称：TaktPayslipDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：Payslip 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPayslip 生成，请按需审阅）
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
// Payslip 响应 DTO
// ========================================

/// <summary>
/// 员工工资条（发薪结果单据，区别于 TaktEmpSalary 定薪档案）
/// 对应前端 TaktPayslipDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPayslipDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PayslipID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayslipId { get; set; }


    /// <summary>
    /// 发放状态（字典 humanresource_compensation_payslip_issue_status：0=待发放 1=已发放 2=已确认）
    /// </summary>
    public int IssueStatus { get; set; } = 0;

}

// ========================================
// Payslip 查询 DTO
// ========================================

/// <summary>
/// Payslip 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPayslipQueryDto : TaktPagedQuery
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    public string? PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 基本工资（元）
    /// </summary>
    public decimal? BaseSalary { get; set; }

    /// <summary>
    /// 岗位工资（元）
    /// </summary>
    public decimal? PositionSalary { get; set; }

    /// <summary>
    /// 绩效/奖金（元）
    /// </summary>
    public decimal? BonusAmount { get; set; }

    /// <summary>
    /// 加班费（元）
    /// </summary>
    public decimal? OvertimePay { get; set; }

    /// <summary>
    /// 津贴合计（元）
    /// </summary>
    public decimal? AllowanceTotal { get; set; }

    /// <summary>
    /// 应发合计（元）
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 社保扣款（元）
    /// </summary>
    public decimal? SocialSecurityDeduction { get; set; }

    /// <summary>
    /// 公积金扣款（元）
    /// </summary>
    public decimal? HousingFundDeduction { get; set; }

    /// <summary>
    /// 个税扣款（元）
    /// </summary>
    public decimal? TaxDeduction { get; set; }

    /// <summary>
    /// 其他扣款（元）
    /// </summary>
    public decimal? OtherDeduction { get; set; }

    /// <summary>
    /// 实发金额（元）
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 关联计算公式方案编码（核算时按 TaktSalaryFormula.set_code 加载步骤并执行）
    /// </summary>
    public string? FormulaSetCode { get; set; } = string.Empty;

    /// <summary>
    /// 发放日期（范围查询-开始）
    /// </summary>
    public DateTime? IssueDateStart { get; set; }

    /// <summary>
    /// 发放日期（范围查询-结束）
    /// </summary>
    public DateTime? IssueDateEnd { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 发放状态（字典 humanresource_compensation_payslip_issue_status：0=待发放 1=已发放 2=已确认）
    /// </summary>
    public int? IssueStatus { get; set; }

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
// 创建Payslip DTO
// ========================================

/// <summary>
/// 创建Payslip DTO
/// </summary>
public class TaktPayslipCreateDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    [Required(ErrorMessage = "员工姓名不能为空")]
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    [Required(ErrorMessage = "发薪期间（如 2026-06）不能为空")]
    public string PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 基本工资（元）
    /// </summary>
    public decimal BaseSalary { get; set; }

    /// <summary>
    /// 岗位工资（元）
    /// </summary>
    public decimal PositionSalary { get; set; }

    /// <summary>
    /// 绩效/奖金（元）
    /// </summary>
    public decimal BonusAmount { get; set; }

    /// <summary>
    /// 加班费（元）
    /// </summary>
    public decimal OvertimePay { get; set; }

    /// <summary>
    /// 津贴合计（元）
    /// </summary>
    public decimal AllowanceTotal { get; set; }

    /// <summary>
    /// 应发合计（元）
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// 社保扣款（元）
    /// </summary>
    public decimal SocialSecurityDeduction { get; set; }

    /// <summary>
    /// 公积金扣款（元）
    /// </summary>
    public decimal HousingFundDeduction { get; set; }

    /// <summary>
    /// 个税扣款（元）
    /// </summary>
    public decimal TaxDeduction { get; set; }

    /// <summary>
    /// 其他扣款（元）
    /// </summary>
    public decimal OtherDeduction { get; set; }

    /// <summary>
    /// 实发金额（元）
    /// </summary>
    public decimal NetAmount { get; set; }

    /// <summary>
    /// 关联计算公式方案编码（核算时按 TaktSalaryFormula.set_code 加载步骤并执行）
    /// </summary>
    public string? FormulaSetCode { get; set; } = string.Empty;

    /// <summary>
    /// 发放日期
    /// </summary>
    public DateTime? IssueDate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 发放状态（字典 humanresource_compensation_payslip_issue_status：0=待发放 1=已发放 2=已确认）
    /// </summary>
    public int IssueStatus { get; set; } = 0;

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
// 更新Payslip DTO
// ========================================

/// <summary>
/// 更新Payslip DTO
/// 继承 TaktPayslipCreateDto，添加 PayslipId 字段
/// </summary>
public class TaktPayslipUpdateDto : TaktPayslipCreateDto
{
    /// <summary>
    /// PayslipID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayslipId { get; set; }

}

// ========================================
// Payslip 状态 DTO
// ========================================

/// <summary>
/// Payslip 状态更新 DTO
/// </summary>
public class TaktPayslipStatusDto
{
    /// <summary>
    /// PayslipID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayslipId { get; set; }

    /// <summary>
    /// 发放状态（字典 humanresource_compensation_payslip_issue_status：0=待发放 1=已发放 2=已确认）
    /// </summary>
    [Required(ErrorMessage = "发放状态（字典 humanresource_compensation_payslip_issue_status：0=待发放 1=已发放 2=已确认）不能为空")]
    public int IssueStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Payslip 导入模板行 DTO
/// </summary>
public class TaktPayslipTemplateDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    public string? PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 基本工资（元）
    /// </summary>
    public decimal? BaseSalary { get; set; }

    /// <summary>
    /// 岗位工资（元）
    /// </summary>
    public decimal? PositionSalary { get; set; }

    /// <summary>
    /// 绩效/奖金（元）
    /// </summary>
    public decimal? BonusAmount { get; set; }

    /// <summary>
    /// 加班费（元）
    /// </summary>
    public decimal? OvertimePay { get; set; }

    /// <summary>
    /// 津贴合计（元）
    /// </summary>
    public decimal? AllowanceTotal { get; set; }

    /// <summary>
    /// 应发合计（元）
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 社保扣款（元）
    /// </summary>
    public decimal? SocialSecurityDeduction { get; set; }

    /// <summary>
    /// 公积金扣款（元）
    /// </summary>
    public decimal? HousingFundDeduction { get; set; }

    /// <summary>
    /// 个税扣款（元）
    /// </summary>
    public decimal? TaxDeduction { get; set; }

    /// <summary>
    /// 其他扣款（元）
    /// </summary>
    public decimal? OtherDeduction { get; set; }

    /// <summary>
    /// 实发金额（元）
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 关联计算公式方案编码（核算时按 TaktSalaryFormula.set_code 加载步骤并执行）
    /// </summary>
    public string? FormulaSetCode { get; set; } = string.Empty;

    /// <summary>
    /// 发放日期
    /// </summary>
    public DateTime? IssueDate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 发放状态（字典 humanresource_compensation_payslip_issue_status：0=待发放 1=已发放 2=已确认）
    /// </summary>
    public int? IssueStatus { get; set; }

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
/// Payslip 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPayslipImportDto
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
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string? EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    public string? PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 基本工资（元）
    /// </summary>
    public decimal? BaseSalary { get; set; }

    /// <summary>
    /// 岗位工资（元）
    /// </summary>
    public decimal? PositionSalary { get; set; }

    /// <summary>
    /// 绩效/奖金（元）
    /// </summary>
    public decimal? BonusAmount { get; set; }

    /// <summary>
    /// 加班费（元）
    /// </summary>
    public decimal? OvertimePay { get; set; }

    /// <summary>
    /// 津贴合计（元）
    /// </summary>
    public decimal? AllowanceTotal { get; set; }

    /// <summary>
    /// 应发合计（元）
    /// </summary>
    public decimal? GrossAmount { get; set; }

    /// <summary>
    /// 社保扣款（元）
    /// </summary>
    public decimal? SocialSecurityDeduction { get; set; }

    /// <summary>
    /// 公积金扣款（元）
    /// </summary>
    public decimal? HousingFundDeduction { get; set; }

    /// <summary>
    /// 个税扣款（元）
    /// </summary>
    public decimal? TaxDeduction { get; set; }

    /// <summary>
    /// 其他扣款（元）
    /// </summary>
    public decimal? OtherDeduction { get; set; }

    /// <summary>
    /// 实发金额（元）
    /// </summary>
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// 关联计算公式方案编码（核算时按 TaktSalaryFormula.set_code 加载步骤并执行）
    /// </summary>
    public string? FormulaSetCode { get; set; } = string.Empty;

    /// <summary>
    /// 发放日期
    /// </summary>
    public DateTime? IssueDate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 发放状态（字典 humanresource_compensation_payslip_issue_status：0=待发放 1=已发放 2=已确认）
    /// </summary>
    public int? IssueStatus { get; set; }

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
/// Payslip 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPayslipExportDto
{
    /// <summary>
    /// PayslipID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PayslipId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 员工 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// 发薪期间（如 2026-06）
    /// </summary>
    public string PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 基本工资（元）
    /// </summary>
    public decimal BaseSalary { get; set; }

    /// <summary>
    /// 岗位工资（元）
    /// </summary>
    public decimal PositionSalary { get; set; }

    /// <summary>
    /// 绩效/奖金（元）
    /// </summary>
    public decimal BonusAmount { get; set; }

    /// <summary>
    /// 加班费（元）
    /// </summary>
    public decimal OvertimePay { get; set; }

    /// <summary>
    /// 津贴合计（元）
    /// </summary>
    public decimal AllowanceTotal { get; set; }

    /// <summary>
    /// 应发合计（元）
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// 社保扣款（元）
    /// </summary>
    public decimal SocialSecurityDeduction { get; set; }

    /// <summary>
    /// 公积金扣款（元）
    /// </summary>
    public decimal HousingFundDeduction { get; set; }

    /// <summary>
    /// 个税扣款（元）
    /// </summary>
    public decimal TaxDeduction { get; set; }

    /// <summary>
    /// 其他扣款（元）
    /// </summary>
    public decimal OtherDeduction { get; set; }

    /// <summary>
    /// 实发金额（元）
    /// </summary>
    public decimal NetAmount { get; set; }

    /// <summary>
    /// 关联计算公式方案编码（核算时按 TaktSalaryFormula.set_code 加载步骤并执行）
    /// </summary>
    public string? FormulaSetCode { get; set; } = string.Empty;

    /// <summary>
    /// 发放日期
    /// </summary>
    public DateTime? IssueDate { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 发放状态（字典 humanresource_compensation_payslip_issue_status：0=待发放 1=已发放 2=已确认）
    /// </summary>
    public int IssueStatus { get; set; } = 0;

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
