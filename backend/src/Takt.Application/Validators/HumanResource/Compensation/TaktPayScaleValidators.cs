// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Compensation
// 文件名称：TaktPayScaleValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：PayScale 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPayScale 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Compensation;

namespace Takt.Application.Validators.HumanResource.Compensation;

// ========================================
// 创建PayScale 验证器
// ========================================

/// <summary>
/// 创建PayScale DTO 验证器
/// </summary>
public class TaktPayScaleCreateValidator : AbstractValidator<TaktPayScaleCreateDto>
{
    /// <summary>
    /// 初始化 创建PayScale 校验规则
    /// </summary>
    public TaktPayScaleCreateValidator()
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
        RuleFor(x => x.ScaleCode)
            .NotEmpty().WithMessage("薪级编码不能为空")
            .MaximumLength(40).WithMessage("薪级编码长度不能超过40个字符");
        RuleFor(x => x.ScaleName)
            .NotEmpty().WithMessage("薪级名称不能为空")
            .MaximumLength(80).WithMessage("薪级名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PayScale 验证器
// ========================================

/// <summary>
/// 更新PayScale DTO 验证器
/// </summary>
public class TaktPayScaleUpdateValidator : AbstractValidator<TaktPayScaleUpdateDto>
{
    /// <summary>
    /// 初始化 更新PayScale 校验规则
    /// </summary>
    public TaktPayScaleUpdateValidator()
    {
        RuleFor(x => x.PayScaleId)
            .GreaterThan(0).WithMessage("PayScaleID无效");
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
        RuleFor(x => x.ScaleCode)
            .NotEmpty().WithMessage("薪级编码不能为空")
            .MaximumLength(40).WithMessage("薪级编码长度不能超过40个字符");
        RuleFor(x => x.ScaleName)
            .NotEmpty().WithMessage("薪级名称不能为空")
            .MaximumLength(80).WithMessage("薪级名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入PayScale 验证器
// ========================================

/// <summary>
/// 导入PayScale DTO 验证器
/// </summary>
public class TaktPayScaleImportValidator : AbstractValidator<TaktPayScaleImportDto>
{
    /// <summary>
    /// 初始化 导入PayScale 校验规则
    /// </summary>
    public TaktPayScaleImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ScaleCode)
            .NotEmpty().WithMessage("薪级编码不能为空")
            .MaximumLength(40).WithMessage("薪级编码长度不能超过40个字符");
        RuleFor(x => x.ScaleName)
            .NotEmpty().WithMessage("薪级名称不能为空")
            .MaximumLength(80).WithMessage("薪级名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
