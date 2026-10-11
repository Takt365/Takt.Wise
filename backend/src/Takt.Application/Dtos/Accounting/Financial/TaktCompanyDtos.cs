// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktCompanyDtos.cs
// 创建时间：2026-08-15
// 创建人：Takt365(Auto Generated)
// 功能描述：Company 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCompany 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Financial;

// ========================================
// Company 响应 DTO
// ========================================

/// <summary>
/// 公司实体 代表租户下的独立公司（第二层数据隔离业务主档） 组合 1：有关联工厂、有语言（TaktTenantEntityBase）
/// 对应前端 TaktCompanyDto
/// 继承 TaktTenantDtoBase
/// </summary>
public class TaktCompanyDto : TaktTenantDtoBase
{
    /// <summary>
    /// CompanyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CompanyId { get; set; }

    /// <summary>
    /// 公司名称1
    /// </summary>
    public string CompanyName1 { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称2
    /// </summary>
    public string? CompanyName2 { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    public string CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale）
    /// </summary>
    public string EnterpriseScale { get; set; } = string.Empty;

    /// <summary>
    /// 经营范围
    /// </summary>
    public string BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    public string BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    public string? BusinessAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 公司电话
    /// </summary>
    public string CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    public string CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    public string CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    public string CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    public string UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    public string TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    public string LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    public string CompanyManagerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    public decimal RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期
    /// </summary>
    public DateTime EstablishmentDate { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    public int CompanyExistence { get; set; } = 0;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行帐号
    /// </summary>
    public string BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 帐户持有人
    /// </summary>
    public string AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 帐目表（字典 accounting_financial_chart_of_accounts；如 INT/TEAC）
    /// </summary>
    public string ChartOfAccounts { get; set; } = string.Empty;

    /// <summary>
    /// 进项税码（字典 accounting_financial_tax_code）
    /// </summary>
    public string InputTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 销项税码（字典 accounting_financial_tax_code）
    /// </summary>
    public string OutputTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 营业税主体（营业场所）
    /// </summary>
    public string BusinessPlace { get; set; } = string.Empty;

    /// <summary>
    /// 记帐期间变式（字典 accounting_financial_posting_period_variant；原则上一个公司对应一个变式）
    /// </summary>
    public string PostingPeriodVariant { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度变式（字典 accounting_financial_fiscal_year_variant；如 K4/Z1/Z2）
    /// </summary>
    public string FiscalYearVariant { get; set; } = string.Empty;

    /// <summary>
    /// 贷方控制范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CreditControlArea { get; set; } = string.Empty;

    /// <summary>
    /// 财务管理范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string FinancialManagementArea { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 公司状态（字典 sys_normal_disable）
    /// </summary>
    public int CompanyStatus { get; set; } = 0;

}

// ========================================
// Company 查询 DTO
// ========================================

/// <summary>
/// Company 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCompanyQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称1
    /// </summary>
    public string? CompanyName1 { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称2
    /// </summary>
    public string? CompanyName2 { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    public string? CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale）
    /// </summary>
    public string? EnterpriseScale { get; set; } = string.Empty;

    /// <summary>
    /// 经营范围
    /// </summary>
    public string? BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    public string? BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    public string? BusinessAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 公司电话
    /// </summary>
    public string? CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    public string? CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    public string? CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    public string? CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    public string? UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    public string? TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    public string? LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    public string? CompanyManagerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    public decimal? RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期（范围查询-开始）
    /// </summary>
    public DateTime? EstablishmentDateStart { get; set; }

    /// <summary>
    /// 成立日期（范围查询-结束）
    /// </summary>
    public DateTime? EstablishmentDateEnd { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）（范围查询-开始）
    /// </summary>
    public DateTime? ClosingDateStart { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）（范围查询-结束）
    /// </summary>
    public DateTime? ClosingDateEnd { get; set; }

    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    public int? CompanyExistence { get; set; }

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string? CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行帐号
    /// </summary>
    public string? BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 帐户持有人
    /// </summary>
    public string? AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 帐目表（字典 accounting_financial_chart_of_accounts；如 INT/TEAC）
    /// </summary>
    public string? ChartOfAccounts { get; set; } = string.Empty;

    /// <summary>
    /// 进项税码（字典 accounting_financial_tax_code）
    /// </summary>
    public string? InputTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 销项税码（字典 accounting_financial_tax_code）
    /// </summary>
    public string? OutputTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 营业税主体（营业场所）
    /// </summary>
    public string? BusinessPlace { get; set; } = string.Empty;

    /// <summary>
    /// 记帐期间变式（字典 accounting_financial_posting_period_variant；原则上一个公司对应一个变式）
    /// </summary>
    public string? PostingPeriodVariant { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度变式（字典 accounting_financial_fiscal_year_variant；如 K4/Z1/Z2）
    /// </summary>
    public string? FiscalYearVariant { get; set; } = string.Empty;

    /// <summary>
    /// 贷方控制范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CreditControlArea { get; set; } = string.Empty;

    /// <summary>
    /// 财务管理范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? FinancialManagementArea { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 公司状态（字典 sys_normal_disable）
    /// </summary>
    public int? CompanyStatus { get; set; }

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
// 创建Company DTO
// ========================================

/// <summary>
/// 创建Company DTO
/// </summary>
public class TaktCompanyCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称1
    /// </summary>
    [Required(ErrorMessage = "公司名称1不能为空")]
    public string CompanyName1 { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称2
    /// </summary>
    public string? CompanyName2 { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    [Required(ErrorMessage = "公司简称不能为空")]
    public string CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    [Required(ErrorMessage = "企业性质（字典 sys_enterprise_nature）不能为空")]
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    [Required(ErrorMessage = "行业属性（字典 sys_industry_attribute）不能为空")]
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale）
    /// </summary>
    [Required(ErrorMessage = "企业规模（字典 sys_enterprise_scale）不能为空")]
    public string EnterpriseScale { get; set; } = string.Empty;

    /// <summary>
    /// 经营范围
    /// </summary>
    [Required(ErrorMessage = "经营范围不能为空")]
    public string BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    [Required(ErrorMessage = "注册地址1不能为空")]
    public string RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [Required(ErrorMessage = "注册国家（字典 sys_country_code；DictValue=ISO alpha-2）不能为空")]
    public string RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    [Required(ErrorMessage = "注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）不能为空")]
    public string RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    [Required(ErrorMessage = "注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）不能为空")]
    public string RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [Required(ErrorMessage = "经营国家（字典 sys_country_code；DictValue=ISO alpha-2）不能为空")]
    public string BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    [Required(ErrorMessage = "经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）不能为空")]
    public string BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    [Required(ErrorMessage = "经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）不能为空")]
    public string BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    [Required(ErrorMessage = "经营地址1不能为空")]
    public string BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    public string? BusinessAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 公司电话
    /// </summary>
    [Required(ErrorMessage = "公司电话不能为空")]
    public string CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    [Required(ErrorMessage = "公司邮箱不能为空")]
    public string CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    [Required(ErrorMessage = "公司传真不能为空")]
    public string CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    [Required(ErrorMessage = "公司网站不能为空")]
    public string CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    [Required(ErrorMessage = "统一社会信用代码不能为空")]
    public string UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    [Required(ErrorMessage = "税务登记号不能为空")]
    public string TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    [Required(ErrorMessage = "法定代表人不能为空")]
    public string LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    [Required(ErrorMessage = "公司负责人不能为空")]
    public string CompanyManagerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    public decimal RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期
    /// </summary>
    public DateTime EstablishmentDate { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    public int CompanyExistence { get; set; } = 0;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    [Required(ErrorMessage = "编码代号（如 TKC、TCJ、DTA；前端字典录入）不能为空")]
    public string CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    [Required(ErrorMessage = "银行代码（选项 TaktBanks/options；DictValue=BankCode）不能为空")]
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行帐号
    /// </summary>
    [Required(ErrorMessage = "银行帐号不能为空")]
    public string BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 帐户持有人
    /// </summary>
    [Required(ErrorMessage = "帐户持有人不能为空")]
    public string AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    [Required(ErrorMessage = "币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 帐目表（字典 accounting_financial_chart_of_accounts；如 INT/TEAC）
    /// </summary>
    [Required(ErrorMessage = "帐目表（字典 accounting_financial_chart_of_accounts；如 INT/TEAC）不能为空")]
    public string ChartOfAccounts { get; set; } = string.Empty;

    /// <summary>
    /// 进项税码（字典 accounting_financial_tax_code）
    /// </summary>
    [Required(ErrorMessage = "进项税码（字典 accounting_financial_tax_code）不能为空")]
    public string InputTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 销项税码（字典 accounting_financial_tax_code）
    /// </summary>
    [Required(ErrorMessage = "销项税码（字典 accounting_financial_tax_code）不能为空")]
    public string OutputTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 营业税主体（营业场所）
    /// </summary>
    [Required(ErrorMessage = "营业税主体（营业场所）不能为空")]
    public string BusinessPlace { get; set; } = string.Empty;

    /// <summary>
    /// 记帐期间变式（字典 accounting_financial_posting_period_variant；原则上一个公司对应一个变式）
    /// </summary>
    [Required(ErrorMessage = "记帐期间变式（字典 accounting_financial_posting_period_variant；原则上一个公司对应一个变式）不能为空")]
    public string PostingPeriodVariant { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度变式（字典 accounting_financial_fiscal_year_variant；如 K4/Z1/Z2）
    /// </summary>
    [Required(ErrorMessage = "会计年度变式（字典 accounting_financial_fiscal_year_variant；如 K4/Z1/Z2）不能为空")]
    public string FiscalYearVariant { get; set; } = string.Empty;

    /// <summary>
    /// 贷方控制范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    [Required(ErrorMessage = "贷方控制范围（选项 TaktCompanies/options；DictValue=CompanyCode）不能为空")]
    public string CreditControlArea { get; set; } = string.Empty;

    /// <summary>
    /// 财务管理范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    [Required(ErrorMessage = "财务管理范围（选项 TaktCompanies/options；DictValue=CompanyCode）不能为空")]
    public string FinancialManagementArea { get; set; } = string.Empty;

    /// <summary>
    /// 公司状态（字典 sys_normal_disable）
    /// </summary>
    public int CompanyStatus { get; set; } = 0;

    /// <summary>
    /// 可访问该公司的角色 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 可访问该公司的用户 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? UserIds { get; set; }

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
// 更新Company DTO
// ========================================

/// <summary>
/// 更新Company DTO
/// 继承 TaktCompanyCreateDto，添加 CompanyId 字段
/// </summary>
public class TaktCompanyUpdateDto : TaktCompanyCreateDto
{
    /// <summary>
    /// CompanyID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CompanyId { get; set; }

}

// ========================================
// Company 状态 DTO
// ========================================

/// <summary>
/// Company 状态更新 DTO
/// </summary>
public class TaktCompanyStatusDto
{
    /// <summary>
    /// CompanyID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CompanyId { get; set; }

    /// <summary>
    /// 公司状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "公司状态（字典 sys_normal_disable）不能为空")]
    public int CompanyStatus { get; set; } = 0;
}

// ========================================
// Company 排序 DTO
// ========================================

/// <summary>
/// Company 排序更新 DTO
/// </summary>
public class TaktCompanySortDto
{
    /// <summary>
    /// CompanyID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CompanyId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Company 导入模板行 DTO
/// </summary>
public class TaktCompanyTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称1
    /// </summary>
    public string? CompanyName1 { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称2
    /// </summary>
    public string? CompanyName2 { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    public string? CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale）
    /// </summary>
    public string? EnterpriseScale { get; set; } = string.Empty;

    /// <summary>
    /// 经营范围
    /// </summary>
    public string? BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    public string? BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    public string? BusinessAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 公司电话
    /// </summary>
    public string? CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    public string? CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    public string? CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    public string? CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    public string? UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    public string? TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    public string? LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    public string? CompanyManagerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    public decimal? RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期
    /// </summary>
    public DateTime? EstablishmentDate { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    public int? CompanyExistence { get; set; }

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string? CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行帐号
    /// </summary>
    public string? BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 帐户持有人
    /// </summary>
    public string? AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 帐目表（字典 accounting_financial_chart_of_accounts；如 INT/TEAC）
    /// </summary>
    public string? ChartOfAccounts { get; set; } = string.Empty;

    /// <summary>
    /// 进项税码（字典 accounting_financial_tax_code）
    /// </summary>
    public string? InputTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 销项税码（字典 accounting_financial_tax_code）
    /// </summary>
    public string? OutputTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 营业税主体（营业场所）
    /// </summary>
    public string? BusinessPlace { get; set; } = string.Empty;

    /// <summary>
    /// 记帐期间变式（字典 accounting_financial_posting_period_variant；原则上一个公司对应一个变式）
    /// </summary>
    public string? PostingPeriodVariant { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度变式（字典 accounting_financial_fiscal_year_variant；如 K4/Z1/Z2）
    /// </summary>
    public string? FiscalYearVariant { get; set; } = string.Empty;

    /// <summary>
    /// 贷方控制范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CreditControlArea { get; set; } = string.Empty;

    /// <summary>
    /// 财务管理范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? FinancialManagementArea { get; set; } = string.Empty;

    /// <summary>
    /// 公司状态（字典 sys_normal_disable）
    /// </summary>
    public int? CompanyStatus { get; set; }

    /// <summary>
    /// 可访问该公司的角色 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 可访问该公司的用户 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? UserIds { get; set; }

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
/// Company 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCompanyImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称1
    /// </summary>
    public string? CompanyName1 { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称2
    /// </summary>
    public string? CompanyName2 { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    public string? CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string? EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string? IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale）
    /// </summary>
    public string? EnterpriseScale { get; set; } = string.Empty;

    /// <summary>
    /// 经营范围
    /// </summary>
    public string? BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string? RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string? BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string? BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string? BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    public string? BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    public string? BusinessAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 公司电话
    /// </summary>
    public string? CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    public string? CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    public string? CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    public string? CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    public string? UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    public string? TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    public string? LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    public string? CompanyManagerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    public decimal? RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期
    /// </summary>
    public DateTime? EstablishmentDate { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    public int? CompanyExistence { get; set; }

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string? CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string? BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行帐号
    /// </summary>
    public string? BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 帐户持有人
    /// </summary>
    public string? AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 帐目表（字典 accounting_financial_chart_of_accounts；如 INT/TEAC）
    /// </summary>
    public string? ChartOfAccounts { get; set; } = string.Empty;

    /// <summary>
    /// 进项税码（字典 accounting_financial_tax_code）
    /// </summary>
    public string? InputTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 销项税码（字典 accounting_financial_tax_code）
    /// </summary>
    public string? OutputTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 营业税主体（营业场所）
    /// </summary>
    public string? BusinessPlace { get; set; } = string.Empty;

    /// <summary>
    /// 记帐期间变式（字典 accounting_financial_posting_period_variant；原则上一个公司对应一个变式）
    /// </summary>
    public string? PostingPeriodVariant { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度变式（字典 accounting_financial_fiscal_year_variant；如 K4/Z1/Z2）
    /// </summary>
    public string? FiscalYearVariant { get; set; } = string.Empty;

    /// <summary>
    /// 贷方控制范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CreditControlArea { get; set; } = string.Empty;

    /// <summary>
    /// 财务管理范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? FinancialManagementArea { get; set; } = string.Empty;

    /// <summary>
    /// 公司状态（字典 sys_normal_disable）
    /// </summary>
    public int? CompanyStatus { get; set; }

    /// <summary>
    /// 可访问该公司的角色 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 可访问该公司的用户 ID 列表（RBAC 反向合并）
    /// </summary>
    public long[]? UserIds { get; set; }

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
/// Company 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCompanyExportDto
{
    /// <summary>
    /// CompanyID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CompanyId { get; set; }

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称1
    /// </summary>
    public string CompanyName1 { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称2
    /// </summary>
    public string? CompanyName2 { get; set; } = string.Empty;

    /// <summary>
    /// 公司简称
    /// </summary>
    public string CompanyShortName { get; set; } = string.Empty;

    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    public string EnterpriseNature { get; set; } = string.Empty;

    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    public string IndustryAttribute { get; set; } = string.Empty;

    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale）
    /// </summary>
    public string EnterpriseScale { get; set; } = string.Empty;

    /// <summary>
    /// 经营范围
    /// </summary>
    public string BusinessScope { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址1
    /// </summary>
    public string RegistrationAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 注册地址2
    /// </summary>
    public string? RegistrationAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string RegistrationRegion { get; set; } = string.Empty;

    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string RegistrationProvince { get; set; } = string.Empty;

    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string RegistrationCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    public string BusinessRegion { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    public string BusinessProvince { get; set; } = string.Empty;

    /// <summary>
    /// 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    public string BusinessCity { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址1
    /// </summary>
    public string BusinessAddress1 { get; set; } = string.Empty;

    /// <summary>
    /// 经营地址2
    /// </summary>
    public string? BusinessAddress2 { get; set; } = string.Empty;

    /// <summary>
    /// 公司电话
    /// </summary>
    public string CompanyPhone { get; set; } = string.Empty;

    /// <summary>
    /// 公司邮箱
    /// </summary>
    public string CompanyEmail { get; set; } = string.Empty;

    /// <summary>
    /// 公司传真
    /// </summary>
    public string CompanyFax { get; set; } = string.Empty;

    /// <summary>
    /// 公司网站
    /// </summary>
    public string CompanyWebsite { get; set; } = string.Empty;

    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    public string UnifiedSocialCreditCode { get; set; } = string.Empty;

    /// <summary>
    /// 税务登记号
    /// </summary>
    public string TaxRegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// 法定代表人
    /// </summary>
    public string LegalRepresentative { get; set; } = string.Empty;

    /// <summary>
    /// 公司负责人
    /// </summary>
    public string CompanyManagerUserName { get; set; } = string.Empty;

    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    public decimal RegisteredCapital { get; set; }

    /// <summary>
    /// 成立日期
    /// </summary>
    public DateTime EstablishmentDate { get; set; }

    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    public int CompanyExistence { get; set; } = 0;

    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    public string CodeAlias { get; set; } = string.Empty;

    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    public string BankCode { get; set; } = string.Empty;

    /// <summary>
    /// 银行帐号
    /// </summary>
    public string BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// 帐户持有人
    /// </summary>
    public string AccountHolder { get; set; } = string.Empty;

    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 帐目表（字典 accounting_financial_chart_of_accounts；如 INT/TEAC）
    /// </summary>
    public string ChartOfAccounts { get; set; } = string.Empty;

    /// <summary>
    /// 进项税码（字典 accounting_financial_tax_code）
    /// </summary>
    public string InputTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 销项税码（字典 accounting_financial_tax_code）
    /// </summary>
    public string OutputTaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 营业税主体（营业场所）
    /// </summary>
    public string BusinessPlace { get; set; } = string.Empty;

    /// <summary>
    /// 记帐期间变式（字典 accounting_financial_posting_period_variant；原则上一个公司对应一个变式）
    /// </summary>
    public string PostingPeriodVariant { get; set; } = string.Empty;

    /// <summary>
    /// 会计年度变式（字典 accounting_financial_fiscal_year_variant；如 K4/Z1/Z2）
    /// </summary>
    public string FiscalYearVariant { get; set; } = string.Empty;

    /// <summary>
    /// 贷方控制范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CreditControlArea { get; set; } = string.Empty;

    /// <summary>
    /// 财务管理范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string FinancialManagementArea { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 公司状态（字典 sys_normal_disable）
    /// </summary>
    public int CompanyStatus { get; set; } = 0;

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
