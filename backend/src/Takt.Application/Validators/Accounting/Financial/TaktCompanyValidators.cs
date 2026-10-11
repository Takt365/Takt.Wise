// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Accounting.Financial
// 文件名称：TaktCompanyValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：Company 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCompany 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Financial;

namespace Takt.Application.Validators.Accounting.Financial;

// ========================================
// 创建Company 验证器
// ========================================

/// <summary>
/// 创建Company DTO 验证器
/// </summary>
public class TaktCompanyCreateValidator : AbstractValidator<TaktCompanyCreateDto>
{
    /// <summary>
    /// 初始化 创建Company 校验规则
    /// </summary>
    public TaktCompanyCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.RelatedPlant)
            .NotEmpty().WithMessage("关联工厂不能为空")
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.CompanyName1)
            .NotEmpty().WithMessage("公司名称1不能为空")
            .MaximumLength(140).WithMessage("公司名称1长度不能超过140个字符");
        RuleFor(x => x.CompanyShortName)
            .NotEmpty().WithMessage("公司简称不能为空")
            .MaximumLength(50).WithMessage("公司简称长度不能超过50个字符");
        RuleFor(x => x.EnterpriseNature)
            .NotEmpty().WithMessage("企业性质不能为空")
            .MaximumLength(4).WithMessage("企业性质长度不能超过4个字符");
        RuleFor(x => x.IndustryAttribute)
            .NotEmpty().WithMessage("行业属性不能为空")
            .MaximumLength(4).WithMessage("行业属性长度不能超过4个字符");
        RuleFor(x => x.EnterpriseScale)
            .NotEmpty().WithMessage("企业规模不能为空")
            .MaximumLength(2).WithMessage("企业规模长度不能超过2个字符");
        RuleFor(x => x.BusinessScope)
            .NotEmpty().WithMessage("经营范围不能为空");
        RuleFor(x => x.RegistrationAddress1)
            .NotEmpty().WithMessage("注册地址1不能为空")
            .MaximumLength(140).WithMessage("注册地址1长度不能超过140个字符");
        RuleFor(x => x.RegistrationRegion)
            .NotEmpty().WithMessage("注册国家不能为空")
            .MaximumLength(2).WithMessage("注册国家长度不能超过2个字符");
        RuleFor(x => x.RegistrationProvince)
            .NotEmpty().WithMessage("注册省不能为空")
            .MaximumLength(70).WithMessage("注册省长度不能超过70个字符");
        RuleFor(x => x.RegistrationCity)
            .NotEmpty().WithMessage("注册市不能为空")
            .MaximumLength(70).WithMessage("注册市长度不能超过70个字符");
        RuleFor(x => x.BusinessRegion)
            .NotEmpty().WithMessage("经营国家不能为空")
            .MaximumLength(2).WithMessage("经营国家长度不能超过2个字符");
        RuleFor(x => x.BusinessProvince)
            .NotEmpty().WithMessage("经营地区-省不能为空")
            .MaximumLength(70).WithMessage("经营地区-省长度不能超过70个字符");
        RuleFor(x => x.BusinessCity)
            .NotEmpty().WithMessage("经营地区-市不能为空")
            .MaximumLength(70).WithMessage("经营地区-市长度不能超过70个字符");
        RuleFor(x => x.BusinessAddress1)
            .NotEmpty().WithMessage("经营地址1不能为空")
            .MaximumLength(140).WithMessage("经营地址1长度不能超过140个字符");
        RuleFor(x => x.CompanyPhone)
            .NotEmpty().WithMessage("公司电话不能为空")
            .MaximumLength(50).WithMessage("公司电话长度不能超过50个字符");
        RuleFor(x => x.CompanyEmail)
            .NotEmpty().WithMessage("公司邮箱不能为空")
            .MaximumLength(100).WithMessage("公司邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("公司邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.CompanyEmail));
        RuleFor(x => x.CompanyFax)
            .NotEmpty().WithMessage("公司传真不能为空")
            .MaximumLength(50).WithMessage("公司传真长度不能超过50个字符");
        RuleFor(x => x.CompanyWebsite)
            .NotEmpty().WithMessage("公司网站不能为空")
            .MaximumLength(200).WithMessage("公司网站长度不能超过200个字符");
        RuleFor(x => x.UnifiedSocialCreditCode)
            .NotEmpty().WithMessage("统一社会信用代码不能为空")
            .MaximumLength(50).WithMessage("统一社会信用代码长度不能超过50个字符");
        RuleFor(x => x.TaxRegistrationNumber)
            .NotEmpty().WithMessage("税务登记号不能为空")
            .MaximumLength(50).WithMessage("税务登记号长度不能超过50个字符");
        RuleFor(x => x.LegalRepresentative)
            .NotEmpty().WithMessage("法定代表人不能为空")
            .MaximumLength(50).WithMessage("法定代表人长度不能超过50个字符");
        RuleFor(x => x.CodeAlias)
            .NotEmpty().WithMessage("编码代号不能为空")
            .MaximumLength(3).WithMessage("编码代号长度不能超过3个字符");
        RuleFor(x => x.BankCode)
            .NotEmpty().WithMessage("银行代码不能为空")
            .MaximumLength(15).WithMessage("银行代码长度不能超过15个字符");
        RuleFor(x => x.BankAccount)
            .NotEmpty().WithMessage("银行帐号不能为空")
            .MaximumLength(40).WithMessage("银行帐号长度不能超过40个字符");
        RuleFor(x => x.AccountHolder)
            .NotEmpty().WithMessage("帐户持有人不能为空")
            .MaximumLength(100).WithMessage("帐户持有人长度不能超过100个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.ChartOfAccounts)
            .NotEmpty().WithMessage("帐目表不能为空")
            .MaximumLength(4).WithMessage("帐目表长度不能超过4个字符");
        RuleFor(x => x.InputTaxCode)
            .NotEmpty().WithMessage("进项税码不能为空")
            .MaximumLength(2).WithMessage("进项税码长度不能超过2个字符");
        RuleFor(x => x.OutputTaxCode)
            .NotEmpty().WithMessage("销项税码不能为空")
            .MaximumLength(2).WithMessage("销项税码长度不能超过2个字符");
        RuleFor(x => x.BusinessPlace)
            .NotEmpty().WithMessage("营业税主体不能为空")
            .MaximumLength(4).WithMessage("营业税主体长度不能超过4个字符");
        RuleFor(x => x.PostingPeriodVariant)
            .NotEmpty().WithMessage("记帐期间变式不能为空")
            .MaximumLength(4).WithMessage("记帐期间变式长度不能超过4个字符");
        RuleFor(x => x.FiscalYearVariant)
            .NotEmpty().WithMessage("会计年度变式不能为空")
            .MaximumLength(2).WithMessage("会计年度变式长度不能超过2个字符");
        RuleFor(x => x.CreditControlArea)
            .NotEmpty().WithMessage("贷方控制范围不能为空")
            .MaximumLength(4).WithMessage("贷方控制范围长度不能超过4个字符");
        RuleFor(x => x.FinancialManagementArea)
            .NotEmpty().WithMessage("财务管理范围不能为空")
            .MaximumLength(4).WithMessage("财务管理范围长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Company 验证器
// ========================================

/// <summary>
/// 更新Company DTO 验证器
/// </summary>
public class TaktCompanyUpdateValidator : AbstractValidator<TaktCompanyUpdateDto>
{
    /// <summary>
    /// 初始化 更新Company 校验规则
    /// </summary>
    public TaktCompanyUpdateValidator()
    {
        RuleFor(x => x.CompanyId)
            .GreaterThan(0).WithMessage("CompanyID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.RelatedPlant)
            .NotEmpty().WithMessage("关联工厂不能为空")
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符");
        RuleFor(x => x.CompanyName1)
            .NotEmpty().WithMessage("公司名称1不能为空")
            .MaximumLength(140).WithMessage("公司名称1长度不能超过140个字符");
        RuleFor(x => x.CompanyShortName)
            .NotEmpty().WithMessage("公司简称不能为空")
            .MaximumLength(50).WithMessage("公司简称长度不能超过50个字符");
        RuleFor(x => x.EnterpriseNature)
            .NotEmpty().WithMessage("企业性质不能为空")
            .MaximumLength(4).WithMessage("企业性质长度不能超过4个字符");
        RuleFor(x => x.IndustryAttribute)
            .NotEmpty().WithMessage("行业属性不能为空")
            .MaximumLength(4).WithMessage("行业属性长度不能超过4个字符");
        RuleFor(x => x.EnterpriseScale)
            .NotEmpty().WithMessage("企业规模不能为空")
            .MaximumLength(2).WithMessage("企业规模长度不能超过2个字符");
        RuleFor(x => x.BusinessScope)
            .NotEmpty().WithMessage("经营范围不能为空");
        RuleFor(x => x.RegistrationAddress1)
            .NotEmpty().WithMessage("注册地址1不能为空")
            .MaximumLength(140).WithMessage("注册地址1长度不能超过140个字符");
        RuleFor(x => x.RegistrationRegion)
            .NotEmpty().WithMessage("注册国家不能为空")
            .MaximumLength(2).WithMessage("注册国家长度不能超过2个字符");
        RuleFor(x => x.RegistrationProvince)
            .NotEmpty().WithMessage("注册省不能为空")
            .MaximumLength(70).WithMessage("注册省长度不能超过70个字符");
        RuleFor(x => x.RegistrationCity)
            .NotEmpty().WithMessage("注册市不能为空")
            .MaximumLength(70).WithMessage("注册市长度不能超过70个字符");
        RuleFor(x => x.BusinessRegion)
            .NotEmpty().WithMessage("经营国家不能为空")
            .MaximumLength(2).WithMessage("经营国家长度不能超过2个字符");
        RuleFor(x => x.BusinessProvince)
            .NotEmpty().WithMessage("经营地区-省不能为空")
            .MaximumLength(70).WithMessage("经营地区-省长度不能超过70个字符");
        RuleFor(x => x.BusinessCity)
            .NotEmpty().WithMessage("经营地区-市不能为空")
            .MaximumLength(70).WithMessage("经营地区-市长度不能超过70个字符");
        RuleFor(x => x.BusinessAddress1)
            .NotEmpty().WithMessage("经营地址1不能为空")
            .MaximumLength(140).WithMessage("经营地址1长度不能超过140个字符");
        RuleFor(x => x.CompanyPhone)
            .NotEmpty().WithMessage("公司电话不能为空")
            .MaximumLength(50).WithMessage("公司电话长度不能超过50个字符");
        RuleFor(x => x.CompanyEmail)
            .NotEmpty().WithMessage("公司邮箱不能为空")
            .MaximumLength(100).WithMessage("公司邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("公司邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.CompanyEmail));
        RuleFor(x => x.CompanyFax)
            .NotEmpty().WithMessage("公司传真不能为空")
            .MaximumLength(50).WithMessage("公司传真长度不能超过50个字符");
        RuleFor(x => x.CompanyWebsite)
            .NotEmpty().WithMessage("公司网站不能为空")
            .MaximumLength(200).WithMessage("公司网站长度不能超过200个字符");
        RuleFor(x => x.UnifiedSocialCreditCode)
            .NotEmpty().WithMessage("统一社会信用代码不能为空")
            .MaximumLength(50).WithMessage("统一社会信用代码长度不能超过50个字符");
        RuleFor(x => x.TaxRegistrationNumber)
            .NotEmpty().WithMessage("税务登记号不能为空")
            .MaximumLength(50).WithMessage("税务登记号长度不能超过50个字符");
        RuleFor(x => x.LegalRepresentative)
            .NotEmpty().WithMessage("法定代表人不能为空")
            .MaximumLength(50).WithMessage("法定代表人长度不能超过50个字符");
        RuleFor(x => x.CodeAlias)
            .NotEmpty().WithMessage("编码代号不能为空")
            .MaximumLength(3).WithMessage("编码代号长度不能超过3个字符");
        RuleFor(x => x.BankCode)
            .NotEmpty().WithMessage("银行代码不能为空")
            .MaximumLength(15).WithMessage("银行代码长度不能超过15个字符");
        RuleFor(x => x.BankAccount)
            .NotEmpty().WithMessage("银行帐号不能为空")
            .MaximumLength(40).WithMessage("银行帐号长度不能超过40个字符");
        RuleFor(x => x.AccountHolder)
            .NotEmpty().WithMessage("帐户持有人不能为空")
            .MaximumLength(100).WithMessage("帐户持有人长度不能超过100个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.ChartOfAccounts)
            .NotEmpty().WithMessage("帐目表不能为空")
            .MaximumLength(4).WithMessage("帐目表长度不能超过4个字符");
        RuleFor(x => x.InputTaxCode)
            .NotEmpty().WithMessage("进项税码不能为空")
            .MaximumLength(2).WithMessage("进项税码长度不能超过2个字符");
        RuleFor(x => x.OutputTaxCode)
            .NotEmpty().WithMessage("销项税码不能为空")
            .MaximumLength(2).WithMessage("销项税码长度不能超过2个字符");
        RuleFor(x => x.BusinessPlace)
            .NotEmpty().WithMessage("营业税主体不能为空")
            .MaximumLength(4).WithMessage("营业税主体长度不能超过4个字符");
        RuleFor(x => x.PostingPeriodVariant)
            .NotEmpty().WithMessage("记帐期间变式不能为空")
            .MaximumLength(4).WithMessage("记帐期间变式长度不能超过4个字符");
        RuleFor(x => x.FiscalYearVariant)
            .NotEmpty().WithMessage("会计年度变式不能为空")
            .MaximumLength(2).WithMessage("会计年度变式长度不能超过2个字符");
        RuleFor(x => x.CreditControlArea)
            .NotEmpty().WithMessage("贷方控制范围不能为空")
            .MaximumLength(4).WithMessage("贷方控制范围长度不能超过4个字符");
        RuleFor(x => x.FinancialManagementArea)
            .NotEmpty().WithMessage("财务管理范围不能为空")
            .MaximumLength(4).WithMessage("财务管理范围长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Company 验证器
// ========================================

/// <summary>
/// 导入Company DTO 验证器
/// </summary>
public class TaktCompanyImportValidator : AbstractValidator<TaktCompanyImportDto>
{
    /// <summary>
    /// 初始化 导入Company 校验规则
    /// </summary>
    public TaktCompanyImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.RelatedPlant)
            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.RelatedPlant));
        RuleFor(x => x.CompanyName1)
            .NotEmpty().WithMessage("公司名称1不能为空")
            .MaximumLength(140).WithMessage("公司名称1长度不能超过140个字符");
        RuleFor(x => x.CompanyShortName)
            .NotEmpty().WithMessage("公司简称不能为空")
            .MaximumLength(50).WithMessage("公司简称长度不能超过50个字符");
        RuleFor(x => x.EnterpriseNature)
            .NotEmpty().WithMessage("企业性质不能为空")
            .MaximumLength(4).WithMessage("企业性质长度不能超过4个字符");
        RuleFor(x => x.IndustryAttribute)
            .NotEmpty().WithMessage("行业属性不能为空")
            .MaximumLength(4).WithMessage("行业属性长度不能超过4个字符");
        RuleFor(x => x.EnterpriseScale)
            .NotEmpty().WithMessage("企业规模不能为空")
            .MaximumLength(2).WithMessage("企业规模长度不能超过2个字符");
        RuleFor(x => x.BusinessScope)
            .NotEmpty().WithMessage("经营范围不能为空");
        RuleFor(x => x.RegistrationAddress1)
            .NotEmpty().WithMessage("注册地址1不能为空")
            .MaximumLength(140).WithMessage("注册地址1长度不能超过140个字符");
        RuleFor(x => x.RegistrationRegion)
            .NotEmpty().WithMessage("注册国家不能为空")
            .MaximumLength(2).WithMessage("注册国家长度不能超过2个字符");
        RuleFor(x => x.RegistrationProvince)
            .NotEmpty().WithMessage("注册省不能为空")
            .MaximumLength(70).WithMessage("注册省长度不能超过70个字符");
        RuleFor(x => x.RegistrationCity)
            .NotEmpty().WithMessage("注册市不能为空")
            .MaximumLength(70).WithMessage("注册市长度不能超过70个字符");
        RuleFor(x => x.BusinessRegion)
            .NotEmpty().WithMessage("经营国家不能为空")
            .MaximumLength(2).WithMessage("经营国家长度不能超过2个字符");
        RuleFor(x => x.BusinessProvince)
            .NotEmpty().WithMessage("经营地区-省不能为空")
            .MaximumLength(70).WithMessage("经营地区-省长度不能超过70个字符");
        RuleFor(x => x.BusinessCity)
            .NotEmpty().WithMessage("经营地区-市不能为空")
            .MaximumLength(70).WithMessage("经营地区-市长度不能超过70个字符");
        RuleFor(x => x.BusinessAddress1)
            .NotEmpty().WithMessage("经营地址1不能为空")
            .MaximumLength(140).WithMessage("经营地址1长度不能超过140个字符");
        RuleFor(x => x.CompanyPhone)
            .NotEmpty().WithMessage("公司电话不能为空")
            .MaximumLength(50).WithMessage("公司电话长度不能超过50个字符");
        RuleFor(x => x.CompanyEmail)
            .NotEmpty().WithMessage("公司邮箱不能为空")
            .MaximumLength(100).WithMessage("公司邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("公司邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.CompanyEmail));
        RuleFor(x => x.CompanyFax)
            .NotEmpty().WithMessage("公司传真不能为空")
            .MaximumLength(50).WithMessage("公司传真长度不能超过50个字符");
        RuleFor(x => x.CompanyWebsite)
            .NotEmpty().WithMessage("公司网站不能为空")
            .MaximumLength(200).WithMessage("公司网站长度不能超过200个字符");
        RuleFor(x => x.UnifiedSocialCreditCode)
            .NotEmpty().WithMessage("统一社会信用代码不能为空")
            .MaximumLength(50).WithMessage("统一社会信用代码长度不能超过50个字符");
        RuleFor(x => x.TaxRegistrationNumber)
            .NotEmpty().WithMessage("税务登记号不能为空")
            .MaximumLength(50).WithMessage("税务登记号长度不能超过50个字符");
        RuleFor(x => x.LegalRepresentative)
            .NotEmpty().WithMessage("法定代表人不能为空")
            .MaximumLength(50).WithMessage("法定代表人长度不能超过50个字符");
        RuleFor(x => x.CodeAlias)
            .NotEmpty().WithMessage("编码代号不能为空")
            .MaximumLength(3).WithMessage("编码代号长度不能超过3个字符");
        RuleFor(x => x.BankCode)
            .NotEmpty().WithMessage("银行代码不能为空")
            .MaximumLength(15).WithMessage("银行代码长度不能超过15个字符");
        RuleFor(x => x.BankAccount)
            .NotEmpty().WithMessage("银行帐号不能为空")
            .MaximumLength(40).WithMessage("银行帐号长度不能超过40个字符");
        RuleFor(x => x.AccountHolder)
            .NotEmpty().WithMessage("帐户持有人不能为空")
            .MaximumLength(100).WithMessage("帐户持有人长度不能超过100个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.ChartOfAccounts)
            .NotEmpty().WithMessage("帐目表不能为空")
            .MaximumLength(4).WithMessage("帐目表长度不能超过4个字符");
        RuleFor(x => x.InputTaxCode)
            .NotEmpty().WithMessage("进项税码不能为空")
            .MaximumLength(2).WithMessage("进项税码长度不能超过2个字符");
        RuleFor(x => x.OutputTaxCode)
            .NotEmpty().WithMessage("销项税码不能为空")
            .MaximumLength(2).WithMessage("销项税码长度不能超过2个字符");
        RuleFor(x => x.BusinessPlace)
            .NotEmpty().WithMessage("营业税主体不能为空")
            .MaximumLength(4).WithMessage("营业税主体长度不能超过4个字符");
        RuleFor(x => x.PostingPeriodVariant)
            .NotEmpty().WithMessage("记帐期间变式不能为空")
            .MaximumLength(4).WithMessage("记帐期间变式长度不能超过4个字符");
        RuleFor(x => x.FiscalYearVariant)
            .NotEmpty().WithMessage("会计年度变式不能为空")
            .MaximumLength(2).WithMessage("会计年度变式长度不能超过2个字符");
        RuleFor(x => x.CreditControlArea)
            .NotEmpty().WithMessage("贷方控制范围不能为空")
            .MaximumLength(4).WithMessage("贷方控制范围长度不能超过4个字符");
        RuleFor(x => x.FinancialManagementArea)
            .NotEmpty().WithMessage("财务管理范围不能为空")
            .MaximumLength(4).WithMessage("财务管理范围长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
