// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Procurement
// 文件名称：TaktPurchaseInfoRecordValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseInfoRecord 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPurchaseInfoRecord 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Validators.Logistics.Procurement;

// ========================================
// 创建PurchaseInfoRecord 验证器
// ========================================

/// <summary>
/// 创建PurchaseInfoRecord DTO 验证器
/// </summary>
public class TaktPurchaseInfoRecordCreateValidator : AbstractValidator<TaktPurchaseInfoRecordCreateDto>
{
    /// <summary>
    /// 初始化 创建PurchaseInfoRecord 校验规则
    /// </summary>
    public TaktPurchaseInfoRecordCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.PurchaseInquiryId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.PurchaseInquiryId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.PurchaseInfoRecordCode)
            .NotEmpty().WithMessage("定价记录号不能为空")
            .MaximumLength(20).WithMessage("定价记录号长度不能超过20个字符");
        RuleFor(x => x.PriceType)
            .NotEmpty().WithMessage("条件类型不能为空")
            .MaximumLength(4).WithMessage("条件类型长度不能超过4个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(10).WithMessage("供应商编码长度不能超过10个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.PurchaseInquiryId)
            .GreaterThanOrEqualTo(0).WithMessage("来源采购询价 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PurchaseInfoRecord 验证器
// ========================================

/// <summary>
/// 更新PurchaseInfoRecord DTO 验证器
/// </summary>
public class TaktPurchaseInfoRecordUpdateValidator : AbstractValidator<TaktPurchaseInfoRecordUpdateDto>
{
    /// <summary>
    /// 初始化 更新PurchaseInfoRecord 校验规则
    /// </summary>
    public TaktPurchaseInfoRecordUpdateValidator()
    {
        RuleFor(x => x.PurchaseInfoRecordId)
            .GreaterThan(0).WithMessage("PurchaseInfoRecordID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.PurchaseInquiryId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.PurchaseInquiryId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.PurchaseInfoRecordCode)
            .NotEmpty().WithMessage("定价记录号不能为空")
            .MaximumLength(20).WithMessage("定价记录号长度不能超过20个字符");
        RuleFor(x => x.PriceType)
            .NotEmpty().WithMessage("条件类型不能为空")
            .MaximumLength(4).WithMessage("条件类型长度不能超过4个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(10).WithMessage("供应商编码长度不能超过10个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.PurchaseInquiryId)
            .GreaterThanOrEqualTo(0).WithMessage("来源采购询价 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入PurchaseInfoRecord 验证器
// ========================================

/// <summary>
/// 导入PurchaseInfoRecord DTO 验证器
/// </summary>
public class TaktPurchaseInfoRecordImportValidator : AbstractValidator<TaktPurchaseInfoRecordImportDto>
{
    /// <summary>
    /// 初始化 导入PurchaseInfoRecord 校验规则
    /// </summary>
    public TaktPurchaseInfoRecordImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.PurchaseInfoRecordCode)
            .NotEmpty().WithMessage("定价记录号不能为空")
            .MaximumLength(20).WithMessage("定价记录号长度不能超过20个字符");
        RuleFor(x => x.PriceType)
            .NotEmpty().WithMessage("条件类型不能为空")
            .MaximumLength(4).WithMessage("条件类型长度不能超过4个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(10).WithMessage("供应商编码长度不能超过10个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.PurchaseInquiryId)
            .GreaterThanOrEqualTo(0).WithMessage("来源采购询价 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
