// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Procurement
// 文件名称：TaktPurchaseRequestItemValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchaseRequestItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPurchaseRequestItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Validators.Logistics.Procurement;

// ========================================
// 创建PurchaseRequestItem 验证器
// ========================================

/// <summary>
/// 创建PurchaseRequestItem DTO 验证器
/// </summary>
public class TaktPurchaseRequestItemCreateValidator : AbstractValidator<TaktPurchaseRequestItemCreateDto>
{
    /// <summary>
    /// 初始化 创建PurchaseRequestItem 校验规则
    /// </summary>
    public TaktPurchaseRequestItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.PurchaseRequestId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.PurchaseRequestId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.PurchaseRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("采购申请 ID不能为负数");
        RuleFor(x => x.PurchaseRequestCode)
            .NotEmpty().WithMessage("采购申请编码不能为空").When(x => x.PurchaseRequestId <= 0)
            .MaximumLength(20).WithMessage("采购申请编码长度不能超过20个字符");
        RuleFor(x => x.PurchasePlanItemId)
            .GreaterThanOrEqualTo(0).WithMessage("来源采购计划明细 ID不能为负数");
        RuleFor(x => x.AllocationCategory)
            .NotEmpty().WithMessage("分配类别不能为空")
            .MaximumLength(40).WithMessage("分配类别长度不能超过40个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.RequestUnit)
            .NotEmpty().WithMessage("申请单位不能为空")
            .MaximumLength(20).WithMessage("申请单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PurchaseRequestItem 验证器
// ========================================

/// <summary>
/// 更新PurchaseRequestItem DTO 验证器
/// </summary>
public class TaktPurchaseRequestItemUpdateValidator : AbstractValidator<TaktPurchaseRequestItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新PurchaseRequestItem 校验规则
    /// </summary>
    public TaktPurchaseRequestItemUpdateValidator()
    {
        RuleFor(x => x.PurchaseRequestItemId)
            .GreaterThan(0).WithMessage("PurchaseRequestItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.PurchaseRequestId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.PurchaseRequestId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.PurchaseRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("采购申请 ID不能为负数");
        RuleFor(x => x.PurchaseRequestCode)
            .NotEmpty().WithMessage("采购申请编码不能为空").When(x => x.PurchaseRequestId <= 0)
            .MaximumLength(20).WithMessage("采购申请编码长度不能超过20个字符");
        RuleFor(x => x.PurchasePlanItemId)
            .GreaterThanOrEqualTo(0).WithMessage("来源采购计划明细 ID不能为负数");
        RuleFor(x => x.AllocationCategory)
            .NotEmpty().WithMessage("分配类别不能为空")
            .MaximumLength(40).WithMessage("分配类别长度不能超过40个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.RequestUnit)
            .NotEmpty().WithMessage("申请单位不能为空")
            .MaximumLength(20).WithMessage("申请单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入PurchaseRequestItem 验证器
// ========================================

/// <summary>
/// 导入PurchaseRequestItem DTO 验证器
/// </summary>
public class TaktPurchaseRequestItemImportValidator : AbstractValidator<TaktPurchaseRequestItemImportDto>
{
    /// <summary>
    /// 初始化 导入PurchaseRequestItem 校验规则
    /// </summary>
    public TaktPurchaseRequestItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.PurchaseRequestId)
            .GreaterThanOrEqualTo(0).WithMessage("采购申请 ID不能为负数");
        RuleFor(x => x.PurchaseRequestCode)
            .NotEmpty().WithMessage("采购申请编码不能为空")
            .MaximumLength(20).WithMessage("采购申请编码长度不能超过20个字符");
        RuleFor(x => x.PurchasePlanItemId)
            .GreaterThanOrEqualTo(0).WithMessage("来源采购计划明细 ID不能为负数");
        RuleFor(x => x.AllocationCategory)
            .NotEmpty().WithMessage("分配类别不能为空")
            .MaximumLength(40).WithMessage("分配类别长度不能超过40个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.RequestUnit)
            .NotEmpty().WithMessage("申请单位不能为空")
            .MaximumLength(20).WithMessage("申请单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
