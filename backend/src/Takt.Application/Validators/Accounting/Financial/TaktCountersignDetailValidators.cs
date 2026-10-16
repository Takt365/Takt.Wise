// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Accounting.Financial
// 文件名称：TaktCountersignDetailValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：CountersignDetail 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCountersignDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Financial;

namespace Takt.Application.Validators.Accounting.Financial;

// ========================================
// 创建CountersignDetail 验证器
// ========================================

/// <summary>
/// 创建CountersignDetail DTO 验证器
/// </summary>
public class TaktCountersignDetailCreateValidator : AbstractValidator<TaktCountersignDetailCreateDto>
{
    /// <summary>
    /// 初始化 创建CountersignDetail 校验规则
    /// </summary>
    public TaktCountersignDetailCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.CountersignId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.CountersignId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.CountersignId)
            .GreaterThanOrEqualTo(0).WithMessage("会签单 ID不能为负数");
        RuleFor(x => x.CountersignCode)
            .NotEmpty().WithMessage("会签编码不能为空").When(x => x.CountersignId <= 0)
            .MaximumLength(50).WithMessage("会签编码长度不能超过50个字符");
        RuleFor(x => x.AllocationCategory)
            .NotEmpty().WithMessage("分配类别不能为空")
            .MaximumLength(40).WithMessage("分配类别长度不能超过40个字符");
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("明细项名称不能为空")
            .MaximumLength(200).WithMessage("明细项名称长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新CountersignDetail 验证器
// ========================================

/// <summary>
/// 更新CountersignDetail DTO 验证器
/// </summary>
public class TaktCountersignDetailUpdateValidator : AbstractValidator<TaktCountersignDetailUpdateDto>
{
    /// <summary>
    /// 初始化 更新CountersignDetail 校验规则
    /// </summary>
    public TaktCountersignDetailUpdateValidator()
    {
        RuleFor(x => x.CountersignDetailId)
            .GreaterThan(0).WithMessage("CountersignDetailID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.CountersignId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.CountersignId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.CountersignId)
            .GreaterThanOrEqualTo(0).WithMessage("会签单 ID不能为负数");
        RuleFor(x => x.CountersignCode)
            .NotEmpty().WithMessage("会签编码不能为空").When(x => x.CountersignId <= 0)
            .MaximumLength(50).WithMessage("会签编码长度不能超过50个字符");
        RuleFor(x => x.AllocationCategory)
            .NotEmpty().WithMessage("分配类别不能为空")
            .MaximumLength(40).WithMessage("分配类别长度不能超过40个字符");
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("明细项名称不能为空")
            .MaximumLength(200).WithMessage("明细项名称长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入CountersignDetail 验证器
// ========================================

/// <summary>
/// 导入CountersignDetail DTO 验证器
/// </summary>
public class TaktCountersignDetailImportValidator : AbstractValidator<TaktCountersignDetailImportDto>
{
    /// <summary>
    /// 初始化 导入CountersignDetail 校验规则
    /// </summary>
    public TaktCountersignDetailImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.CountersignId)
            .GreaterThanOrEqualTo(0).WithMessage("会签单 ID不能为负数");
        RuleFor(x => x.CountersignCode)
            .NotEmpty().WithMessage("会签编码不能为空")
            .MaximumLength(50).WithMessage("会签编码长度不能超过50个字符");
        RuleFor(x => x.AllocationCategory)
            .NotEmpty().WithMessage("分配类别不能为空")
            .MaximumLength(40).WithMessage("分配类别长度不能超过40个字符");
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("明细项名称不能为空")
            .MaximumLength(200).WithMessage("明细项名称长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
