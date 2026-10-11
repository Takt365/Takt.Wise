// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.HumanResource.Benefits
// 文件名称：TaktSocialInsuranceDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：SocialInsurance 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSocialInsurance 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.HumanResource.Benefits;

// ========================================
// SocialInsurance 响应 DTO
// ========================================

/// <summary>
/// 社保与公积金月度缴纳流水（分项金额明细；福利类型配置不在此表重复建模）
/// 对应前端 TaktSocialInsuranceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSocialInsuranceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SocialInsuranceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SocialInsuranceId { get; set; }


    /// <summary>
    /// 缴纳状态（字典 humanresource_benefits_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    public int PayStatus { get; set; } = 0;

}

// ========================================
// SocialInsurance 查询 DTO
// ========================================

/// <summary>
/// SocialInsurance 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSocialInsuranceQueryDto : TaktPagedQuery
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
    /// 关联福利项目 ID（通常对应 humanresource_benefits_benefit_type 为社保/公积金的 TaktBenefitItem）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BenefitItemId { get; set; }

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
    /// 缴纳期间（如 2026-06）
    /// </summary>
    public string? PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 社保缴纳基数（元）
    /// </summary>
    public decimal? SocialSecurityBase { get; set; }

    /// <summary>
    /// 养老保险（元）
    /// </summary>
    public decimal? PensionAmount { get; set; }

    /// <summary>
    /// 医疗保险（元）
    /// </summary>
    public decimal? MedicalAmount { get; set; }

    /// <summary>
    /// 失业保险（元）
    /// </summary>
    public decimal? UnemploymentAmount { get; set; }

    /// <summary>
    /// 工伤保险（元）
    /// </summary>
    public decimal? InjuryAmount { get; set; }

    /// <summary>
    /// 生育保险（元）
    /// </summary>
    public decimal? MaternityAmount { get; set; }

    /// <summary>
    /// 公积金缴纳基数（元）
    /// </summary>
    public decimal? HousingFundBase { get; set; }

    /// <summary>
    /// 公积金（元）
    /// </summary>
    public decimal? HousingFundAmount { get; set; }

    /// <summary>
    /// 缴纳合计（元）
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 缴纳状态（字典 humanresource_benefits_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    public int? PayStatus { get; set; }

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
// 创建SocialInsurance DTO
// ========================================

/// <summary>
/// 创建SocialInsurance DTO
/// </summary>
public class TaktSocialInsuranceCreateDto
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
    /// 关联福利项目 ID（通常对应 humanresource_benefits_benefit_type 为社保/公积金的 TaktBenefitItem）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BenefitItemId { get; set; }

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
    /// 缴纳期间（如 2026-06）
    /// </summary>
    [Required(ErrorMessage = "缴纳期间（如 2026-06）不能为空")]
    public string PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 社保缴纳基数（元）
    /// </summary>
    public decimal SocialSecurityBase { get; set; }

    /// <summary>
    /// 养老保险（元）
    /// </summary>
    public decimal PensionAmount { get; set; }

    /// <summary>
    /// 医疗保险（元）
    /// </summary>
    public decimal MedicalAmount { get; set; }

    /// <summary>
    /// 失业保险（元）
    /// </summary>
    public decimal UnemploymentAmount { get; set; }

    /// <summary>
    /// 工伤保险（元）
    /// </summary>
    public decimal InjuryAmount { get; set; }

    /// <summary>
    /// 生育保险（元）
    /// </summary>
    public decimal MaternityAmount { get; set; }

    /// <summary>
    /// 公积金缴纳基数（元）
    /// </summary>
    public decimal HousingFundBase { get; set; }

    /// <summary>
    /// 公积金（元）
    /// </summary>
    public decimal HousingFundAmount { get; set; }

    /// <summary>
    /// 缴纳合计（元）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    [Required(ErrorMessage = "关联工厂不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 缴纳状态（字典 humanresource_benefits_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    public int PayStatus { get; set; } = 0;

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
// 更新SocialInsurance DTO
// ========================================

/// <summary>
/// 更新SocialInsurance DTO
/// 继承 TaktSocialInsuranceCreateDto，添加 SocialInsuranceId 字段
/// </summary>
public class TaktSocialInsuranceUpdateDto : TaktSocialInsuranceCreateDto
{
    /// <summary>
    /// SocialInsuranceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SocialInsuranceId { get; set; }

}

// ========================================
// SocialInsurance 状态 DTO
// ========================================

/// <summary>
/// SocialInsurance 状态更新 DTO
/// </summary>
public class TaktSocialInsuranceStatusDto
{
    /// <summary>
    /// SocialInsuranceID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SocialInsuranceId { get; set; }

    /// <summary>
    /// 缴纳状态（字典 humanresource_benefits_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    [Required(ErrorMessage = "缴纳状态（字典 humanresource_benefits_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）不能为空")]
    public int PayStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SocialInsurance 导入模板行 DTO
/// </summary>
public class TaktSocialInsuranceTemplateDto
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
    /// 关联福利项目 ID（通常对应 humanresource_benefits_benefit_type 为社保/公积金的 TaktBenefitItem）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BenefitItemId { get; set; }

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
    /// 缴纳期间（如 2026-06）
    /// </summary>
    public string? PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 社保缴纳基数（元）
    /// </summary>
    public decimal? SocialSecurityBase { get; set; }

    /// <summary>
    /// 养老保险（元）
    /// </summary>
    public decimal? PensionAmount { get; set; }

    /// <summary>
    /// 医疗保险（元）
    /// </summary>
    public decimal? MedicalAmount { get; set; }

    /// <summary>
    /// 失业保险（元）
    /// </summary>
    public decimal? UnemploymentAmount { get; set; }

    /// <summary>
    /// 工伤保险（元）
    /// </summary>
    public decimal? InjuryAmount { get; set; }

    /// <summary>
    /// 生育保险（元）
    /// </summary>
    public decimal? MaternityAmount { get; set; }

    /// <summary>
    /// 公积金缴纳基数（元）
    /// </summary>
    public decimal? HousingFundBase { get; set; }

    /// <summary>
    /// 公积金（元）
    /// </summary>
    public decimal? HousingFundAmount { get; set; }

    /// <summary>
    /// 缴纳合计（元）
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 缴纳状态（字典 humanresource_benefits_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    public int? PayStatus { get; set; }

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
/// SocialInsurance 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSocialInsuranceImportDto
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
    /// 关联福利项目 ID（通常对应 humanresource_benefits_benefit_type 为社保/公积金的 TaktBenefitItem）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BenefitItemId { get; set; }

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
    /// 缴纳期间（如 2026-06）
    /// </summary>
    public string? PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 社保缴纳基数（元）
    /// </summary>
    public decimal? SocialSecurityBase { get; set; }

    /// <summary>
    /// 养老保险（元）
    /// </summary>
    public decimal? PensionAmount { get; set; }

    /// <summary>
    /// 医疗保险（元）
    /// </summary>
    public decimal? MedicalAmount { get; set; }

    /// <summary>
    /// 失业保险（元）
    /// </summary>
    public decimal? UnemploymentAmount { get; set; }

    /// <summary>
    /// 工伤保险（元）
    /// </summary>
    public decimal? InjuryAmount { get; set; }

    /// <summary>
    /// 生育保险（元）
    /// </summary>
    public decimal? MaternityAmount { get; set; }

    /// <summary>
    /// 公积金缴纳基数（元）
    /// </summary>
    public decimal? HousingFundBase { get; set; }

    /// <summary>
    /// 公积金（元）
    /// </summary>
    public decimal? HousingFundAmount { get; set; }

    /// <summary>
    /// 缴纳合计（元）
    /// </summary>
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 缴纳状态（字典 humanresource_benefits_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    public int? PayStatus { get; set; }

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
/// SocialInsurance 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSocialInsuranceExportDto
{
    /// <summary>
    /// SocialInsuranceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SocialInsuranceId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联福利项目 ID（通常对应 humanresource_benefits_benefit_type 为社保/公积金的 TaktBenefitItem）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BenefitItemId { get; set; }

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
    /// 缴纳期间（如 2026-06）
    /// </summary>
    public string PayPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 社保缴纳基数（元）
    /// </summary>
    public decimal SocialSecurityBase { get; set; }

    /// <summary>
    /// 养老保险（元）
    /// </summary>
    public decimal PensionAmount { get; set; }

    /// <summary>
    /// 医疗保险（元）
    /// </summary>
    public decimal MedicalAmount { get; set; }

    /// <summary>
    /// 失业保险（元）
    /// </summary>
    public decimal UnemploymentAmount { get; set; }

    /// <summary>
    /// 工伤保险（元）
    /// </summary>
    public decimal InjuryAmount { get; set; }

    /// <summary>
    /// 生育保险（元）
    /// </summary>
    public decimal MaternityAmount { get; set; }

    /// <summary>
    /// 公积金缴纳基数（元）
    /// </summary>
    public decimal HousingFundBase { get; set; }

    /// <summary>
    /// 公积金（元）
    /// </summary>
    public decimal HousingFundAmount { get; set; }

    /// <summary>
    /// 缴纳合计（元）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 关联工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 缴纳状态（字典 humanresource_benefits_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    public int PayStatus { get; set; } = 0;

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
