// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktPlantValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：Plant 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPlant 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建Plant 验证器
// ========================================

/// <summary>
/// 创建Plant DTO 验证器
/// </summary>
public class TaktPlantCreateValidator : AbstractValidator<TaktPlantCreateDto>
{
    /// <summary>
    /// 初始化 创建Plant 校验规则
    /// </summary>
    public TaktPlantCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantName1)
            .NotEmpty().WithMessage("工厂名称1不能为空")
            .MaximumLength(140).WithMessage("工厂名称1长度不能超过140个字符");
        RuleFor(x => x.PlantShortName)
            .NotEmpty().WithMessage("工厂简称不能为空")
            .MaximumLength(50).WithMessage("工厂简称长度不能超过50个字符");
        RuleFor(x => x.CodeAlias)
            .NotEmpty().WithMessage("编码代号不能为空")
            .MaximumLength(3).WithMessage("编码代号长度不能超过3个字符");
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
        RuleFor(x => x.PlantPhone)
            .NotEmpty().WithMessage("工厂电话不能为空")
            .MaximumLength(50).WithMessage("工厂电话长度不能超过50个字符");
        RuleFor(x => x.PlantEmail)
            .NotEmpty().WithMessage("工厂邮箱不能为空")
            .MaximumLength(100).WithMessage("工厂邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("工厂邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.PlantEmail));
        RuleFor(x => x.PlantFax)
            .NotEmpty().WithMessage("工厂传真不能为空")
            .MaximumLength(50).WithMessage("工厂传真长度不能超过50个字符");
        RuleFor(x => x.PlantWebsite)
            .NotEmpty().WithMessage("工厂网站不能为空")
            .MaximumLength(200).WithMessage("工厂网站长度不能超过200个字符");
        RuleFor(x => x.UnifiedSocialCreditCode)
            .NotEmpty().WithMessage("统一社会信用代码不能为空")
            .MaximumLength(50).WithMessage("统一社会信用代码长度不能超过50个字符");
        RuleFor(x => x.TaxRegistrationNumber)
            .NotEmpty().WithMessage("税务登记号不能为空")
            .MaximumLength(50).WithMessage("税务登记号长度不能超过50个字符");
        RuleFor(x => x.LegalRepresentative)
            .NotEmpty().WithMessage("法定代表人不能为空")
            .MaximumLength(50).WithMessage("法定代表人长度不能超过50个字符");
        RuleFor(x => x.BankCode)
            .NotEmpty().WithMessage("银行代码不能为空")
            .MaximumLength(15).WithMessage("银行代码长度不能超过15个字符");
        RuleFor(x => x.BankAccount)
            .NotEmpty().WithMessage("银行帐号不能为空")
            .MaximumLength(40).WithMessage("银行帐号长度不能超过40个字符");
        RuleFor(x => x.AccountHolder)
            .NotEmpty().WithMessage("帐户持有人不能为空")
            .MaximumLength(100).WithMessage("帐户持有人长度不能超过100个字符");
        RuleFor(x => x.PurchasingOrganization)
            .NotEmpty().WithMessage("采购组织不能为空")
            .MaximumLength(4).WithMessage("采购组织长度不能超过4个字符");
        RuleFor(x => x.SalesOrganization)
            .NotEmpty().WithMessage("销售组织不能为空")
            .MaximumLength(4).WithMessage("销售组织长度不能超过4个字符");
        RuleFor(x => x.MaterialRequirementsPlanning)
            .NotEmpty().WithMessage("物料需求计划不能为空")
            .MaximumLength(10).WithMessage("物料需求计划长度不能超过10个字符");
        RuleFor(x => x.DistributionChannel)
            .NotEmpty().WithMessage("分销渠道不能为空")
            .MaximumLength(2).WithMessage("分销渠道长度不能超过2个字符");
        RuleFor(x => x.IntercompanyBillingProductGroup)
            .NotEmpty().WithMessage("公司间出具发票产品组不能为空")
            .MaximumLength(2).WithMessage("公司间出具发票产品组长度不能超过2个字符");
        RuleFor(x => x.TaxIndicator)
            .NotEmpty().WithMessage("税收标识不能为空")
            .MaximumLength(2).WithMessage("税收标识长度不能超过2个字符");
        RuleFor(x => x.ValuationArea)
            .NotEmpty().WithMessage("评估范围不能为空")
            .MaximumLength(4).WithMessage("评估范围长度不能超过4个字符");
        RuleFor(x => x.PlantVendorNumber)
            .NotEmpty().WithMessage("工厂供应商号码不能为空")
            .MaximumLength(10).WithMessage("工厂供应商号码长度不能超过10个字符");
        RuleFor(x => x.PlantCustomerNumber)
            .NotEmpty().WithMessage("客户编码-工厂不能为空")
            .MaximumLength(10).WithMessage("客户编码-工厂长度不能超过10个字符");
        RuleFor(x => x.FactoryCalendar)
            .NotEmpty().WithMessage("工厂日历不能为空")
            .MaximumLength(2).WithMessage("工厂日历长度不能超过2个字符");
        RuleFor(x => x.RelatedCompany)
            .NotEmpty().WithMessage("关联公司不能为空")
            .MaximumLength(4).WithMessage("关联公司长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Plant 验证器
// ========================================

/// <summary>
/// 更新Plant DTO 验证器
/// </summary>
public class TaktPlantUpdateValidator : AbstractValidator<TaktPlantUpdateDto>
{
    /// <summary>
    /// 初始化 更新Plant 校验规则
    /// </summary>
    public TaktPlantUpdateValidator()
    {
        RuleFor(x => x.PlantId)
            .GreaterThan(0).WithMessage("PlantID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantName1)
            .NotEmpty().WithMessage("工厂名称1不能为空")
            .MaximumLength(140).WithMessage("工厂名称1长度不能超过140个字符");
        RuleFor(x => x.PlantShortName)
            .NotEmpty().WithMessage("工厂简称不能为空")
            .MaximumLength(50).WithMessage("工厂简称长度不能超过50个字符");
        RuleFor(x => x.CodeAlias)
            .NotEmpty().WithMessage("编码代号不能为空")
            .MaximumLength(3).WithMessage("编码代号长度不能超过3个字符");
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
        RuleFor(x => x.PlantPhone)
            .NotEmpty().WithMessage("工厂电话不能为空")
            .MaximumLength(50).WithMessage("工厂电话长度不能超过50个字符");
        RuleFor(x => x.PlantEmail)
            .NotEmpty().WithMessage("工厂邮箱不能为空")
            .MaximumLength(100).WithMessage("工厂邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("工厂邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.PlantEmail));
        RuleFor(x => x.PlantFax)
            .NotEmpty().WithMessage("工厂传真不能为空")
            .MaximumLength(50).WithMessage("工厂传真长度不能超过50个字符");
        RuleFor(x => x.PlantWebsite)
            .NotEmpty().WithMessage("工厂网站不能为空")
            .MaximumLength(200).WithMessage("工厂网站长度不能超过200个字符");
        RuleFor(x => x.UnifiedSocialCreditCode)
            .NotEmpty().WithMessage("统一社会信用代码不能为空")
            .MaximumLength(50).WithMessage("统一社会信用代码长度不能超过50个字符");
        RuleFor(x => x.TaxRegistrationNumber)
            .NotEmpty().WithMessage("税务登记号不能为空")
            .MaximumLength(50).WithMessage("税务登记号长度不能超过50个字符");
        RuleFor(x => x.LegalRepresentative)
            .NotEmpty().WithMessage("法定代表人不能为空")
            .MaximumLength(50).WithMessage("法定代表人长度不能超过50个字符");
        RuleFor(x => x.BankCode)
            .NotEmpty().WithMessage("银行代码不能为空")
            .MaximumLength(15).WithMessage("银行代码长度不能超过15个字符");
        RuleFor(x => x.BankAccount)
            .NotEmpty().WithMessage("银行帐号不能为空")
            .MaximumLength(40).WithMessage("银行帐号长度不能超过40个字符");
        RuleFor(x => x.AccountHolder)
            .NotEmpty().WithMessage("帐户持有人不能为空")
            .MaximumLength(100).WithMessage("帐户持有人长度不能超过100个字符");
        RuleFor(x => x.PurchasingOrganization)
            .NotEmpty().WithMessage("采购组织不能为空")
            .MaximumLength(4).WithMessage("采购组织长度不能超过4个字符");
        RuleFor(x => x.SalesOrganization)
            .NotEmpty().WithMessage("销售组织不能为空")
            .MaximumLength(4).WithMessage("销售组织长度不能超过4个字符");
        RuleFor(x => x.MaterialRequirementsPlanning)
            .NotEmpty().WithMessage("物料需求计划不能为空")
            .MaximumLength(10).WithMessage("物料需求计划长度不能超过10个字符");
        RuleFor(x => x.DistributionChannel)
            .NotEmpty().WithMessage("分销渠道不能为空")
            .MaximumLength(2).WithMessage("分销渠道长度不能超过2个字符");
        RuleFor(x => x.IntercompanyBillingProductGroup)
            .NotEmpty().WithMessage("公司间出具发票产品组不能为空")
            .MaximumLength(2).WithMessage("公司间出具发票产品组长度不能超过2个字符");
        RuleFor(x => x.TaxIndicator)
            .NotEmpty().WithMessage("税收标识不能为空")
            .MaximumLength(2).WithMessage("税收标识长度不能超过2个字符");
        RuleFor(x => x.ValuationArea)
            .NotEmpty().WithMessage("评估范围不能为空")
            .MaximumLength(4).WithMessage("评估范围长度不能超过4个字符");
        RuleFor(x => x.PlantVendorNumber)
            .NotEmpty().WithMessage("工厂供应商号码不能为空")
            .MaximumLength(10).WithMessage("工厂供应商号码长度不能超过10个字符");
        RuleFor(x => x.PlantCustomerNumber)
            .NotEmpty().WithMessage("客户编码-工厂不能为空")
            .MaximumLength(10).WithMessage("客户编码-工厂长度不能超过10个字符");
        RuleFor(x => x.FactoryCalendar)
            .NotEmpty().WithMessage("工厂日历不能为空")
            .MaximumLength(2).WithMessage("工厂日历长度不能超过2个字符");
        RuleFor(x => x.RelatedCompany)
            .NotEmpty().WithMessage("关联公司不能为空")
            .MaximumLength(4).WithMessage("关联公司长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Plant 验证器
// ========================================

/// <summary>
/// 导入Plant DTO 验证器
/// </summary>
public class TaktPlantImportValidator : AbstractValidator<TaktPlantImportDto>
{
    /// <summary>
    /// 初始化 导入Plant 校验规则
    /// </summary>
    public TaktPlantImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantName1)
            .NotEmpty().WithMessage("工厂名称1不能为空")
            .MaximumLength(140).WithMessage("工厂名称1长度不能超过140个字符");
        RuleFor(x => x.PlantShortName)
            .NotEmpty().WithMessage("工厂简称不能为空")
            .MaximumLength(50).WithMessage("工厂简称长度不能超过50个字符");
        RuleFor(x => x.CodeAlias)
            .NotEmpty().WithMessage("编码代号不能为空")
            .MaximumLength(3).WithMessage("编码代号长度不能超过3个字符");
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
        RuleFor(x => x.PlantPhone)
            .NotEmpty().WithMessage("工厂电话不能为空")
            .MaximumLength(50).WithMessage("工厂电话长度不能超过50个字符");
        RuleFor(x => x.PlantEmail)
            .NotEmpty().WithMessage("工厂邮箱不能为空")
            .MaximumLength(100).WithMessage("工厂邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("工厂邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.PlantEmail));
        RuleFor(x => x.PlantFax)
            .NotEmpty().WithMessage("工厂传真不能为空")
            .MaximumLength(50).WithMessage("工厂传真长度不能超过50个字符");
        RuleFor(x => x.PlantWebsite)
            .NotEmpty().WithMessage("工厂网站不能为空")
            .MaximumLength(200).WithMessage("工厂网站长度不能超过200个字符");
        RuleFor(x => x.UnifiedSocialCreditCode)
            .NotEmpty().WithMessage("统一社会信用代码不能为空")
            .MaximumLength(50).WithMessage("统一社会信用代码长度不能超过50个字符");
        RuleFor(x => x.TaxRegistrationNumber)
            .NotEmpty().WithMessage("税务登记号不能为空")
            .MaximumLength(50).WithMessage("税务登记号长度不能超过50个字符");
        RuleFor(x => x.LegalRepresentative)
            .NotEmpty().WithMessage("法定代表人不能为空")
            .MaximumLength(50).WithMessage("法定代表人长度不能超过50个字符");
        RuleFor(x => x.BankCode)
            .NotEmpty().WithMessage("银行代码不能为空")
            .MaximumLength(15).WithMessage("银行代码长度不能超过15个字符");
        RuleFor(x => x.BankAccount)
            .NotEmpty().WithMessage("银行帐号不能为空")
            .MaximumLength(40).WithMessage("银行帐号长度不能超过40个字符");
        RuleFor(x => x.AccountHolder)
            .NotEmpty().WithMessage("帐户持有人不能为空")
            .MaximumLength(100).WithMessage("帐户持有人长度不能超过100个字符");
        RuleFor(x => x.PurchasingOrganization)
            .NotEmpty().WithMessage("采购组织不能为空")
            .MaximumLength(4).WithMessage("采购组织长度不能超过4个字符");
        RuleFor(x => x.SalesOrganization)
            .NotEmpty().WithMessage("销售组织不能为空")
            .MaximumLength(4).WithMessage("销售组织长度不能超过4个字符");
        RuleFor(x => x.MaterialRequirementsPlanning)
            .NotEmpty().WithMessage("物料需求计划不能为空")
            .MaximumLength(10).WithMessage("物料需求计划长度不能超过10个字符");
        RuleFor(x => x.DistributionChannel)
            .NotEmpty().WithMessage("分销渠道不能为空")
            .MaximumLength(2).WithMessage("分销渠道长度不能超过2个字符");
        RuleFor(x => x.IntercompanyBillingProductGroup)
            .NotEmpty().WithMessage("公司间出具发票产品组不能为空")
            .MaximumLength(2).WithMessage("公司间出具发票产品组长度不能超过2个字符");
        RuleFor(x => x.TaxIndicator)
            .NotEmpty().WithMessage("税收标识不能为空")
            .MaximumLength(2).WithMessage("税收标识长度不能超过2个字符");
        RuleFor(x => x.ValuationArea)
            .NotEmpty().WithMessage("评估范围不能为空")
            .MaximumLength(4).WithMessage("评估范围长度不能超过4个字符");
        RuleFor(x => x.PlantVendorNumber)
            .NotEmpty().WithMessage("工厂供应商号码不能为空")
            .MaximumLength(10).WithMessage("工厂供应商号码长度不能超过10个字符");
        RuleFor(x => x.PlantCustomerNumber)
            .NotEmpty().WithMessage("客户编码-工厂不能为空")
            .MaximumLength(10).WithMessage("客户编码-工厂长度不能超过10个字符");
        RuleFor(x => x.FactoryCalendar)
            .NotEmpty().WithMessage("工厂日历不能为空")
            .MaximumLength(2).WithMessage("工厂日历长度不能超过2个字符");
        RuleFor(x => x.RelatedCompany)
            .NotEmpty().WithMessage("关联公司不能为空")
            .MaximumLength(4).WithMessage("关联公司长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
