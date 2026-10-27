// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktClientValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：Client 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktClient 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建Client 验证器
// ========================================

/// <summary>
/// 创建Client DTO 验证器
/// </summary>
public class TaktClientCreateValidator : AbstractValidator<TaktClientCreateDto>
{
    /// <summary>
    /// 初始化 创建Client 校验规则
    /// </summary>
    public TaktClientCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(10).WithMessage("客户端编码长度不能超过10个字符");
        RuleFor(x => x.AccountGroup)
            .NotEmpty().WithMessage("科目组不能为空")
            .MaximumLength(4).WithMessage("科目组长度不能超过4个字符");
        RuleFor(x => x.SalesOrganization)
            .NotEmpty().WithMessage("销售组织不能为空")
            .MaximumLength(4).WithMessage("销售组织长度不能超过4个字符");
        RuleFor(x => x.DistributionChannel)
            .NotEmpty().WithMessage("分销渠道不能为空")
            .MaximumLength(2).WithMessage("分销渠道长度不能超过2个字符");
        RuleFor(x => x.ProductGroup)
            .NotEmpty().WithMessage("产品组不能为空")
            .MaximumLength(2).WithMessage("产品组长度不能超过2个字符");
        RuleFor(x => x.ClientName1)
            .NotEmpty().WithMessage("名称1不能为空")
            .MaximumLength(140).WithMessage("名称1长度不能超过140个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商不能为空")
            .MaximumLength(10).WithMessage("供应商长度不能超过10个字符");
        RuleFor(x => x.TradingPartner)
            .NotEmpty().WithMessage("贸易伙伴不能为空")
            .MaximumLength(4).WithMessage("贸易伙伴长度不能超过4个字符");
        RuleFor(x => x.EnterpriseNature)
            .NotEmpty().WithMessage("企业性质不能为空")
            .MaximumLength(4).WithMessage("企业性质长度不能超过4个字符");
        RuleFor(x => x.IndustryAttribute)
            .NotEmpty().WithMessage("行业属性不能为空")
            .MaximumLength(4).WithMessage("行业属性长度不能超过4个字符");
        RuleFor(x => x.ReconciliationAccount)
            .NotEmpty().WithMessage("统驭科目不能为空")
            .MaximumLength(40).WithMessage("统驭科目长度不能超过40个字符");
        RuleFor(x => x.PaymentTerms)
            .NotEmpty().WithMessage("付款条件不能为空")
            .MaximumLength(40).WithMessage("付款条件长度不能超过40个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("货币不能为空")
            .MaximumLength(3).WithMessage("货币长度不能超过3个字符");
        RuleFor(x => x.CustomerPricingProcedure)
            .NotEmpty().WithMessage("客户定价过程不能为空")
            .MaximumLength(2).WithMessage("客户定价过程长度不能超过2个字符");
        RuleFor(x => x.ShippingConditions)
            .NotEmpty().WithMessage("装运条件不能为空")
            .MaximumLength(2).WithMessage("装运条件长度不能超过2个字符");
        RuleFor(x => x.DeliveringPlant)
            .NotEmpty().WithMessage("交货工厂不能为空")
            .MaximumLength(4).WithMessage("交货工厂长度不能超过4个字符");
        RuleFor(x => x.Incoterms1)
            .NotEmpty().WithMessage("国际贸易条款1不能为空")
            .MaximumLength(3).WithMessage("国际贸易条款1长度不能超过3个字符");
        RuleFor(x => x.Incoterms2)
            .NotEmpty().WithMessage("国际贸易条款2不能为空")
            .MaximumLength(40).WithMessage("国际贸易条款2长度不能超过40个字符");
        RuleFor(x => x.AccountAssignmentGroup)
            .NotEmpty().WithMessage("帐户分配组不能为空")
            .MaximumLength(2).WithMessage("帐户分配组长度不能超过2个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Client 验证器
// ========================================

/// <summary>
/// 更新Client DTO 验证器
/// </summary>
public class TaktClientUpdateValidator : AbstractValidator<TaktClientUpdateDto>
{
    /// <summary>
    /// 初始化 更新Client 校验规则
    /// </summary>
    public TaktClientUpdateValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("ClientID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(10).WithMessage("客户端编码长度不能超过10个字符");
        RuleFor(x => x.AccountGroup)
            .NotEmpty().WithMessage("科目组不能为空")
            .MaximumLength(4).WithMessage("科目组长度不能超过4个字符");
        RuleFor(x => x.SalesOrganization)
            .NotEmpty().WithMessage("销售组织不能为空")
            .MaximumLength(4).WithMessage("销售组织长度不能超过4个字符");
        RuleFor(x => x.DistributionChannel)
            .NotEmpty().WithMessage("分销渠道不能为空")
            .MaximumLength(2).WithMessage("分销渠道长度不能超过2个字符");
        RuleFor(x => x.ProductGroup)
            .NotEmpty().WithMessage("产品组不能为空")
            .MaximumLength(2).WithMessage("产品组长度不能超过2个字符");
        RuleFor(x => x.ClientName1)
            .NotEmpty().WithMessage("名称1不能为空")
            .MaximumLength(140).WithMessage("名称1长度不能超过140个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商不能为空")
            .MaximumLength(10).WithMessage("供应商长度不能超过10个字符");
        RuleFor(x => x.TradingPartner)
            .NotEmpty().WithMessage("贸易伙伴不能为空")
            .MaximumLength(4).WithMessage("贸易伙伴长度不能超过4个字符");
        RuleFor(x => x.EnterpriseNature)
            .NotEmpty().WithMessage("企业性质不能为空")
            .MaximumLength(4).WithMessage("企业性质长度不能超过4个字符");
        RuleFor(x => x.IndustryAttribute)
            .NotEmpty().WithMessage("行业属性不能为空")
            .MaximumLength(4).WithMessage("行业属性长度不能超过4个字符");
        RuleFor(x => x.ReconciliationAccount)
            .NotEmpty().WithMessage("统驭科目不能为空")
            .MaximumLength(40).WithMessage("统驭科目长度不能超过40个字符");
        RuleFor(x => x.PaymentTerms)
            .NotEmpty().WithMessage("付款条件不能为空")
            .MaximumLength(40).WithMessage("付款条件长度不能超过40个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("货币不能为空")
            .MaximumLength(3).WithMessage("货币长度不能超过3个字符");
        RuleFor(x => x.CustomerPricingProcedure)
            .NotEmpty().WithMessage("客户定价过程不能为空")
            .MaximumLength(2).WithMessage("客户定价过程长度不能超过2个字符");
        RuleFor(x => x.ShippingConditions)
            .NotEmpty().WithMessage("装运条件不能为空")
            .MaximumLength(2).WithMessage("装运条件长度不能超过2个字符");
        RuleFor(x => x.DeliveringPlant)
            .NotEmpty().WithMessage("交货工厂不能为空")
            .MaximumLength(4).WithMessage("交货工厂长度不能超过4个字符");
        RuleFor(x => x.Incoterms1)
            .NotEmpty().WithMessage("国际贸易条款1不能为空")
            .MaximumLength(3).WithMessage("国际贸易条款1长度不能超过3个字符");
        RuleFor(x => x.Incoterms2)
            .NotEmpty().WithMessage("国际贸易条款2不能为空")
            .MaximumLength(40).WithMessage("国际贸易条款2长度不能超过40个字符");
        RuleFor(x => x.AccountAssignmentGroup)
            .NotEmpty().WithMessage("帐户分配组不能为空")
            .MaximumLength(2).WithMessage("帐户分配组长度不能超过2个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Client 验证器
// ========================================

/// <summary>
/// 导入Client DTO 验证器
/// </summary>
public class TaktClientImportValidator : AbstractValidator<TaktClientImportDto>
{
    /// <summary>
    /// 初始化 导入Client 校验规则
    /// </summary>
    public TaktClientImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ClientCode)
            .NotEmpty().WithMessage("客户端编码不能为空")
            .MaximumLength(10).WithMessage("客户端编码长度不能超过10个字符");
        RuleFor(x => x.AccountGroup)
            .NotEmpty().WithMessage("科目组不能为空")
            .MaximumLength(4).WithMessage("科目组长度不能超过4个字符");
        RuleFor(x => x.SalesOrganization)
            .NotEmpty().WithMessage("销售组织不能为空")
            .MaximumLength(4).WithMessage("销售组织长度不能超过4个字符");
        RuleFor(x => x.DistributionChannel)
            .NotEmpty().WithMessage("分销渠道不能为空")
            .MaximumLength(2).WithMessage("分销渠道长度不能超过2个字符");
        RuleFor(x => x.ProductGroup)
            .NotEmpty().WithMessage("产品组不能为空")
            .MaximumLength(2).WithMessage("产品组长度不能超过2个字符");
        RuleFor(x => x.ClientName1)
            .NotEmpty().WithMessage("名称1不能为空")
            .MaximumLength(140).WithMessage("名称1长度不能超过140个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商不能为空")
            .MaximumLength(10).WithMessage("供应商长度不能超过10个字符");
        RuleFor(x => x.TradingPartner)
            .NotEmpty().WithMessage("贸易伙伴不能为空")
            .MaximumLength(4).WithMessage("贸易伙伴长度不能超过4个字符");
        RuleFor(x => x.EnterpriseNature)
            .NotEmpty().WithMessage("企业性质不能为空")
            .MaximumLength(4).WithMessage("企业性质长度不能超过4个字符");
        RuleFor(x => x.IndustryAttribute)
            .NotEmpty().WithMessage("行业属性不能为空")
            .MaximumLength(4).WithMessage("行业属性长度不能超过4个字符");
        RuleFor(x => x.ReconciliationAccount)
            .NotEmpty().WithMessage("统驭科目不能为空")
            .MaximumLength(40).WithMessage("统驭科目长度不能超过40个字符");
        RuleFor(x => x.PaymentTerms)
            .NotEmpty().WithMessage("付款条件不能为空")
            .MaximumLength(40).WithMessage("付款条件长度不能超过40个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("货币不能为空")
            .MaximumLength(3).WithMessage("货币长度不能超过3个字符");
        RuleFor(x => x.CustomerPricingProcedure)
            .NotEmpty().WithMessage("客户定价过程不能为空")
            .MaximumLength(2).WithMessage("客户定价过程长度不能超过2个字符");
        RuleFor(x => x.ShippingConditions)
            .NotEmpty().WithMessage("装运条件不能为空")
            .MaximumLength(2).WithMessage("装运条件长度不能超过2个字符");
        RuleFor(x => x.DeliveringPlant)
            .NotEmpty().WithMessage("交货工厂不能为空")
            .MaximumLength(4).WithMessage("交货工厂长度不能超过4个字符");
        RuleFor(x => x.Incoterms1)
            .NotEmpty().WithMessage("国际贸易条款1不能为空")
            .MaximumLength(3).WithMessage("国际贸易条款1长度不能超过3个字符");
        RuleFor(x => x.Incoterms2)
            .NotEmpty().WithMessage("国际贸易条款2不能为空")
            .MaximumLength(40).WithMessage("国际贸易条款2长度不能超过40个字符");
        RuleFor(x => x.AccountAssignmentGroup)
            .NotEmpty().WithMessage("帐户分配组不能为空")
            .MaximumLength(2).WithMessage("帐户分配组长度不能超过2个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
