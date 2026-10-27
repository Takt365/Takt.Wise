// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktCompany.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：公司实体，代表租户下的独立公司/工厂（第二层数据隔离）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities.Identity;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 公司实体
/// 代表租户下的独立公司（第二层数据隔离业务主档）
/// 组合 1：有关联工厂、有语言（TaktTenantEntityBase）
/// </summary>
[SugarTable("takt_accounting_financial_company", "公司表")]
[SugarIndex("ix_company_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_company_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_company_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, true)]
public class TaktCompany : TaktTenantEntityBase
{
    /// <summary>
    /// 公司代码（唯一索引：租户内唯一，见 ix_company_code_unique；前端表单选择编码规则后自动通过 TaktNumbering 公司代码规则生成并展示，非手输；单据类型菜单：公司信息）
    /// </summary>
    [SugarColumn(ColumnName = "company_code", ColumnDescription = "公司代码", ColumnDataType = "varchar", Length = 4, IsNullable = false)]
    public string CompanyCode { get; set; } = string.Empty;
    /// <summary>
    /// 公司名称1
    /// </summary>
    [SugarColumn(ColumnName = "company_name1", ColumnDescription = "公司名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false)]
    public string CompanyName1 { get; set; } = string.Empty;
    /// <summary>
    /// 公司名称2
    /// </summary>
    [SugarColumn(ColumnName = "company_name2", ColumnDescription = "公司名称2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? CompanyName2 { get; set; }
    /// <summary>
    /// 公司简称
    /// </summary>
    [SugarColumn(ColumnName = "company_short_name", ColumnDescription = "公司简称", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string CompanyShortName { get; set; } = string.Empty;
    /// <summary>
    /// 企业性质（字典 sys_enterprise_nature）
    /// </summary>
    [SugarColumn(ColumnName = "enterprise_nature", ColumnDescription = "企业性质", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "150")]
    public string EnterpriseNature { get; set; } = "150";
    /// <summary>
    /// 行业属性（字典 sys_industry_attribute）
    /// </summary>
    [SugarColumn(ColumnName = "industry_attribute", ColumnDescription = "行业属性", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "C")]
    public string IndustryAttribute { get; set; } = "C";
    /// <summary>
    /// 企业规模（字典 sys_enterprise_scale）
    /// </summary>
    [SugarColumn(ColumnName = "enterprise_scale", ColumnDescription = "企业规模", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "M")]
    public string EnterpriseScale { get; set; } = "M";
    /// <summary>
    /// 经营范围
    /// </summary>
    [SugarColumn(ColumnName = "business_scope", ColumnDescription = "经营范围", ColumnDataType = "nvarchar", Length = -1, IsNullable = false, DefaultValue = "")]
    public string BusinessScope { get; set; } = string.Empty;
    /// <summary>
    /// 注册地址1
    /// </summary>
    [SugarColumn(ColumnName = "registration_address1", ColumnDescription = "注册地址1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false, DefaultValue = "")]
    public string RegistrationAddress1 { get; set; } = string.Empty;
    /// <summary>
    /// 注册地址2
    /// </summary>
    [SugarColumn(ColumnName = "registration_address2", ColumnDescription = "注册地址2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? RegistrationAddress2 { get; set; }
    /// <summary>
    /// 注册国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "registration_region", ColumnDescription = "注册国家", ColumnDataType = "nvarchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string RegistrationRegion { get; set; } = string.Empty;
    /// <summary>
    /// 注册省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    [SugarColumn(ColumnName = "registration_province", ColumnDescription = "注册省", ColumnDataType = "nvarchar", Length = 70, IsNullable = false, DefaultValue = "")]
    public string RegistrationProvince { get; set; } = string.Empty;
    /// <summary>
    /// 注册市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    [SugarColumn(ColumnName = "registration_city", ColumnDescription = "注册市", ColumnDataType = "nvarchar", Length = 70, IsNullable = false, DefaultValue = "")]
    public string RegistrationCity { get; set; } = string.Empty;
    /// <summary>
    /// 经营国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "business_region", ColumnDescription = "经营国家", ColumnDataType = "nvarchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string BusinessRegion { get; set; } = string.Empty;
    /// <summary>
    /// 经营地区-省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
    /// </summary>
    [SugarColumn(ColumnName = "business_province", ColumnDescription = "经营地区-省", ColumnDataType = "nvarchar", Length = 70, IsNullable = false, DefaultValue = "")]
    public string BusinessProvince { get; set; } = string.Empty;
    /// <summary>
    /// 经营地区-市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
    /// </summary>
    [SugarColumn(ColumnName = "business_city", ColumnDescription = "经营地区-市", ColumnDataType = "nvarchar", Length = 70, IsNullable = false, DefaultValue = "")]
    public string BusinessCity { get; set; } = string.Empty;
    /// <summary>
    /// 经营地址1
    /// </summary>
    [SugarColumn(ColumnName = "business_address1", ColumnDescription = "经营地址1", ColumnDataType = "nvarchar", Length = 140, IsNullable = false, DefaultValue = "")]
    public string BusinessAddress1 { get; set; } = string.Empty;
    /// <summary>
    /// 经营地址2
    /// </summary>
    [SugarColumn(ColumnName = "business_address2", ColumnDescription = "经营地址2", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? BusinessAddress2 { get; set; }
    /// <summary>
    /// 公司电话
    /// </summary>
    [SugarColumn(ColumnName = "company_phone", ColumnDescription = "公司电话", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string CompanyPhone { get; set; } = string.Empty;
    /// <summary>
    /// 公司邮箱
    /// </summary>
    [SugarColumn(ColumnName = "company_email", ColumnDescription = "公司邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string CompanyEmail { get; set; } = string.Empty;
    /// <summary>
    /// 公司传真
    /// </summary>
    [SugarColumn(ColumnName = "company_fax", ColumnDescription = "公司传真", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string CompanyFax { get; set; } = string.Empty;
    /// <summary>
    /// 公司网站
    /// </summary>
    [SugarColumn(ColumnName = "company_website", ColumnDescription = "公司网站", ColumnDataType = "nvarchar", Length = 200, IsNullable = false, DefaultValue = "")]
    public string CompanyWebsite { get; set; } = string.Empty;
    /// <summary>
    /// 统一社会信用代码
    /// </summary>
    [SugarColumn(ColumnName = "unified_social_credit_code", ColumnDescription = "统一社会信用代码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string UnifiedSocialCreditCode { get; set; } = string.Empty;
    /// <summary>
    /// 税务登记号
    /// </summary>
    [SugarColumn(ColumnName = "tax_registration_number", ColumnDescription = "税务登记号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string TaxRegistrationNumber { get; set; } = string.Empty;
    /// <summary>
    /// 法定代表人
    /// </summary>
    [SugarColumn(ColumnName = "legal_representative", ColumnDescription = "法定代表人", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string LegalRepresentative { get; set; } = string.Empty;
    /// <summary>
    /// 公司负责人（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "company_manager_user_id", ColumnDescription = "公司负责人用户ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CompanyManagerUserId { get; set; }
    /// <summary>
    /// 公司负责人名称（冗余：按 CompanyManagerUserId 取 TaktUser.UserName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "company_manager_user_name", ColumnDescription = "公司负责人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? CompanyManagerUserName { get; set; }
    /// <summary>
    /// 注册资本（万元）
    /// </summary>
    [SugarColumn(ColumnName = "registered_capital", ColumnDescription = "注册资本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal RegisteredCapital { get; set; } = 0;
    /// <summary>
    /// 成立日期
    /// </summary>
    [SugarColumn(ColumnName = "establishment_date", ColumnDescription = "成立日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime EstablishmentDate { get; set; }
    /// <summary>
    /// 关闭日期（注销/停业；未关闭则为 null）
    /// </summary>
    [SugarColumn(ColumnName = "closing_date", ColumnDescription = "关闭日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ClosingDate { get; set; }
    /// <summary>
    /// 存续状态（字典 sys_entity_existence_status）
    /// </summary>
    [SugarColumn(ColumnName = "company_existence", ColumnDescription = "存续状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CompanyExistence { get; set; } = 1;
    /// <summary>
    /// 编码代号（如 TKC、TCJ、DTA；前端字典录入）
    /// </summary>
    [SugarColumn(ColumnName = "code_alias", ColumnDescription = "编码代号", ColumnDataType = "varchar", Length = 3, IsNullable = false, DefaultValue = "TKC")]
    public string CodeAlias { get; set; } = string.Empty;
    /// <summary>
    /// 银行代码（选项 TaktBanks/options；DictValue=BankCode）
    /// </summary>
    [SugarColumn(ColumnName = "bank_code", ColumnDescription = "银行代码", ColumnDataType = "nvarchar", Length = 15, IsNullable = false, DefaultValue = "")]
    public string BankCode { get; set; } = string.Empty;
    /// <summary>
    /// 银行帐号
    /// </summary>
    [SugarColumn(ColumnName = "bank_account", ColumnDescription = "银行帐号", ColumnDataType = "nvarchar", Length = 40, IsNullable = false, DefaultValue = "")]
    public string BankAccount { get; set; } = string.Empty;
    /// <summary>
    /// 帐户持有人
    /// </summary>
    [SugarColumn(ColumnName = "account_holder", ColumnDescription = "帐户持有人", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string AccountHolder { get; set; } = string.Empty;
    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "币种", ColumnDataType = "varchar", Length = 3, IsNullable = false, DefaultValue = "")]
    public string CurrencyCode { get; set; } = string.Empty;
    /// <summary>
    /// 帐目表（字典 accounting_financial_chart_of_accounts；如 INT/TEAC）
    /// </summary>
    [SugarColumn(ColumnName = "chart_of_accounts", ColumnDescription = "帐目表", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string ChartOfAccounts { get; set; } = string.Empty;
    /// <summary>
    /// 进项税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    [SugarColumn(ColumnName = "input_tax_code", ColumnDescription = "进项税码", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string InputTaxCode { get; set; } = string.Empty;
    /// <summary>
    /// 销项税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    [SugarColumn(ColumnName = "output_tax_code", ColumnDescription = "销项税码", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string OutputTaxCode { get; set; } = string.Empty;
    /// <summary>
    /// 营业税主体（营业场所）
    /// </summary>
    [SugarColumn(ColumnName = "business_place", ColumnDescription = "营业税主体", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string BusinessPlace { get; set; } = string.Empty;
    /// <summary>
    /// 记帐期间变式（字典 accounting_financial_posting_period_variant；原则上一个公司对应一个变式）
    /// </summary>
    [SugarColumn(ColumnName = "posting_period_variant", ColumnDescription = "记帐期间变式", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string PostingPeriodVariant { get; set; } = string.Empty;
    /// <summary>
    /// 会计年度变式（字典 accounting_financial_fiscal_year_variant；如 K4/Z1/Z2）
    /// </summary>
    [SugarColumn(ColumnName = "fiscal_year_variant", ColumnDescription = "会计年度变式", ColumnDataType = "varchar", Length = 2, IsNullable = false, DefaultValue = "")]
    public string FiscalYearVariant { get; set; } = string.Empty;
    /// <summary>
    /// 贷方控制范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    [SugarColumn(ColumnName = "credit_control_area", ColumnDescription = "贷方控制范围", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string CreditControlArea { get; set; } = string.Empty;
    /// <summary>
    /// 财务管理范围（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    [SugarColumn(ColumnName = "financial_management_area", ColumnDescription = "财务管理范围", ColumnDataType = "varchar", Length = 4, IsNullable = false, DefaultValue = "")]
    public string FinancialManagementArea { get; set; } = string.Empty;
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 公司状态（字典 sys_normal_disable）
    /// </summary>
    [SugarColumn(ColumnName = "company_status", ColumnDescription = "公司状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CompanyStatus { get; set; } = 1;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 可访问该公司的角色关联（RBAC，表 takt_identity_role_company）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktRoleCompany.CompanyCode))]
    public List<TaktRoleCompany>? RoleCompanies { get; set; }

    /// <summary>
    /// 可访问该公司的用户关联（RBAC，表 takt_identity_user_company）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktUserCompany.CompanyCode))]
    public List<TaktUserCompany>? UserCompanies { get; set; }

}
