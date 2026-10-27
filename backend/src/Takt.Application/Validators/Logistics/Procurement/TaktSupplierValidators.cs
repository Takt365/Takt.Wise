// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Procurement
// 文件名称：TaktSupplierValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：Supplier 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSupplier 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Validators.Logistics.Procurement;

// ========================================
// 创建Supplier 验证器
// ========================================

/// <summary>
/// 创建Supplier DTO 验证器
/// </summary>
public class TaktSupplierCreateValidator : AbstractValidator<TaktSupplierCreateDto>
{
    /// <summary>
    /// 初始化 创建Supplier 校验规则
    /// </summary>
    public TaktSupplierCreateValidator()
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
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商不能为空")
            .MaximumLength(10).WithMessage("供应商长度不能超过10个字符");
        RuleFor(x => x.AccountGroup)
            .NotEmpty().WithMessage("账户组不能为空")
            .MaximumLength(4).WithMessage("账户组长度不能超过4个字符");
        RuleFor(x => x.SupplierName1)
            .NotEmpty().WithMessage("名称 1不能为空")
            .MaximumLength(140).WithMessage("名称 1长度不能超过140个字符");
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
        RuleFor(x => x.PurchasingOrganization)
            .NotEmpty().WithMessage("采购组织不能为空")
            .MaximumLength(4).WithMessage("采购组织长度不能超过4个字符");
        RuleFor(x => x.OrderCurrencyCode)
            .NotEmpty().WithMessage("订单货币不能为空")
            .MaximumLength(3).WithMessage("订单货币长度不能超过3个字符");
        RuleFor(x => x.Incoterms1)
            .NotEmpty().WithMessage("国际贸易条款1不能为空")
            .MaximumLength(3).WithMessage("国际贸易条款1长度不能超过3个字符");
        RuleFor(x => x.Incoterms2)
            .NotEmpty().WithMessage("国际贸易条款2不能为空")
            .MaximumLength(40).WithMessage("国际贸易条款2长度不能超过40个字符");
        RuleFor(x => x.PurchaseGroup)
            .NotEmpty().WithMessage("采购组不能为空")
            .MaximumLength(6).WithMessage("采购组长度不能超过6个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Supplier 验证器
// ========================================

/// <summary>
/// 更新Supplier DTO 验证器
/// </summary>
public class TaktSupplierUpdateValidator : AbstractValidator<TaktSupplierUpdateDto>
{
    /// <summary>
    /// 初始化 更新Supplier 校验规则
    /// </summary>
    public TaktSupplierUpdateValidator()
    {
        RuleFor(x => x.SupplierId)
            .GreaterThan(0).WithMessage("SupplierID无效");
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
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商不能为空")
            .MaximumLength(10).WithMessage("供应商长度不能超过10个字符");
        RuleFor(x => x.AccountGroup)
            .NotEmpty().WithMessage("账户组不能为空")
            .MaximumLength(4).WithMessage("账户组长度不能超过4个字符");
        RuleFor(x => x.SupplierName1)
            .NotEmpty().WithMessage("名称 1不能为空")
            .MaximumLength(140).WithMessage("名称 1长度不能超过140个字符");
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
        RuleFor(x => x.PurchasingOrganization)
            .NotEmpty().WithMessage("采购组织不能为空")
            .MaximumLength(4).WithMessage("采购组织长度不能超过4个字符");
        RuleFor(x => x.OrderCurrencyCode)
            .NotEmpty().WithMessage("订单货币不能为空")
            .MaximumLength(3).WithMessage("订单货币长度不能超过3个字符");
        RuleFor(x => x.Incoterms1)
            .NotEmpty().WithMessage("国际贸易条款1不能为空")
            .MaximumLength(3).WithMessage("国际贸易条款1长度不能超过3个字符");
        RuleFor(x => x.Incoterms2)
            .NotEmpty().WithMessage("国际贸易条款2不能为空")
            .MaximumLength(40).WithMessage("国际贸易条款2长度不能超过40个字符");
        RuleFor(x => x.PurchaseGroup)
            .NotEmpty().WithMessage("采购组不能为空")
            .MaximumLength(6).WithMessage("采购组长度不能超过6个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Supplier 验证器
// ========================================

/// <summary>
/// 导入Supplier DTO 验证器
/// </summary>
public class TaktSupplierImportValidator : AbstractValidator<TaktSupplierImportDto>
{
    /// <summary>
    /// 初始化 导入Supplier 校验规则
    /// </summary>
    public TaktSupplierImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商不能为空")
            .MaximumLength(10).WithMessage("供应商长度不能超过10个字符");
        RuleFor(x => x.AccountGroup)
            .NotEmpty().WithMessage("账户组不能为空")
            .MaximumLength(4).WithMessage("账户组长度不能超过4个字符");
        RuleFor(x => x.SupplierName1)
            .NotEmpty().WithMessage("名称 1不能为空")
            .MaximumLength(140).WithMessage("名称 1长度不能超过140个字符");
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
        RuleFor(x => x.PurchasingOrganization)
            .NotEmpty().WithMessage("采购组织不能为空")
            .MaximumLength(4).WithMessage("采购组织长度不能超过4个字符");
        RuleFor(x => x.OrderCurrencyCode)
            .NotEmpty().WithMessage("订单货币不能为空")
            .MaximumLength(3).WithMessage("订单货币长度不能超过3个字符");
        RuleFor(x => x.Incoterms1)
            .NotEmpty().WithMessage("国际贸易条款1不能为空")
            .MaximumLength(3).WithMessage("国际贸易条款1长度不能超过3个字符");
        RuleFor(x => x.Incoterms2)
            .NotEmpty().WithMessage("国际贸易条款2不能为空")
            .MaximumLength(40).WithMessage("国际贸易条款2长度不能超过40个字符");
        RuleFor(x => x.PurchaseGroup)
            .NotEmpty().WithMessage("采购组不能为空")
            .MaximumLength(6).WithMessage("采购组长度不能超过6个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
