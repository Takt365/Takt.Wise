// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Procurement
// 文件名称：TaktPurchaseInquiryItemValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseInquiryItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPurchaseInquiryItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Validators.Logistics.Procurement;

// ========================================
// 创建PurchaseInquiryItem 验证器
// ========================================

/// <summary>
/// 创建PurchaseInquiryItem DTO 验证器
/// </summary>
public class TaktPurchaseInquiryItemCreateValidator : AbstractValidator<TaktPurchaseInquiryItemCreateDto>
{
    /// <summary>
    /// 初始化 创建PurchaseInquiryItem 校验规则
    /// </summary>
    public TaktPurchaseInquiryItemCreateValidator()
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
        RuleFor(x => x.PurchaseInquiryId)
            .GreaterThanOrEqualTo(0).WithMessage("采购询价 ID不能为负数");
        RuleFor(x => x.PurchaseInquiryCode)
            .NotEmpty().WithMessage("采购询价编码不能为空").When(x => x.PurchaseInquiryId <= 0)
            .MaximumLength(20).WithMessage("采购询价编码长度不能超过20个字符");
        RuleFor(x => x.AllocationCategory)
            .NotEmpty().WithMessage("分配类别不能为空")
            .MaximumLength(40).WithMessage("分配类别长度不能超过40个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.InquiryUnit)
            .NotEmpty().WithMessage("询价单位不能为空")
            .MaximumLength(40).WithMessage("询价单位长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PurchaseInquiryItem 验证器
// ========================================

/// <summary>
/// 更新PurchaseInquiryItem DTO 验证器
/// </summary>
public class TaktPurchaseInquiryItemUpdateValidator : AbstractValidator<TaktPurchaseInquiryItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新PurchaseInquiryItem 校验规则
    /// </summary>
    public TaktPurchaseInquiryItemUpdateValidator()
    {
        RuleFor(x => x.PurchaseInquiryItemId)
            .GreaterThan(0).WithMessage("PurchaseInquiryItemID无效");
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
        RuleFor(x => x.PurchaseInquiryId)
            .GreaterThanOrEqualTo(0).WithMessage("采购询价 ID不能为负数");
        RuleFor(x => x.PurchaseInquiryCode)
            .NotEmpty().WithMessage("采购询价编码不能为空").When(x => x.PurchaseInquiryId <= 0)
            .MaximumLength(20).WithMessage("采购询价编码长度不能超过20个字符");
        RuleFor(x => x.AllocationCategory)
            .NotEmpty().WithMessage("分配类别不能为空")
            .MaximumLength(40).WithMessage("分配类别长度不能超过40个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.InquiryUnit)
            .NotEmpty().WithMessage("询价单位不能为空")
            .MaximumLength(40).WithMessage("询价单位长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入PurchaseInquiryItem 验证器
// ========================================

/// <summary>
/// 导入PurchaseInquiryItem DTO 验证器
/// </summary>
public class TaktPurchaseInquiryItemImportValidator : AbstractValidator<TaktPurchaseInquiryItemImportDto>
{
    /// <summary>
    /// 初始化 导入PurchaseInquiryItem 校验规则
    /// </summary>
    public TaktPurchaseInquiryItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.PurchaseInquiryId)
            .GreaterThanOrEqualTo(0).WithMessage("采购询价 ID不能为负数");
        RuleFor(x => x.PurchaseInquiryCode)
            .NotEmpty().WithMessage("采购询价编码不能为空")
            .MaximumLength(20).WithMessage("采购询价编码长度不能超过20个字符");
        RuleFor(x => x.AllocationCategory)
            .NotEmpty().WithMessage("分配类别不能为空")
            .MaximumLength(40).WithMessage("分配类别长度不能超过40个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.InquiryUnit)
            .NotEmpty().WithMessage("询价单位不能为空")
            .MaximumLength(40).WithMessage("询价单位长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
