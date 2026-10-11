// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktAdminDivisionValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：AdminDivision 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktAdminDivision 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建AdminDivision 验证器
// ========================================

/// <summary>
/// 创建AdminDivision DTO 验证器
/// </summary>
public class TaktAdminDivisionCreateValidator : AbstractValidator<TaktAdminDivisionCreateDto>
{
    /// <summary>
    /// 初始化 创建AdminDivision 校验规则
    /// </summary>
    public TaktAdminDivisionCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("国家代码不能为空")
            .MaximumLength(2).WithMessage("国家代码长度不能超过2个字符");
        RuleFor(x => x.DivisionCode)
            .NotEmpty().WithMessage("区划编码不能为空")
            .MaximumLength(40).WithMessage("区划编码长度不能超过40个字符");
        RuleFor(x => x.DivisionName)
            .NotEmpty().WithMessage("区划名称不能为空")
            .MaximumLength(200).WithMessage("区划名称长度不能超过200个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父级区划ID不能为负数");
        RuleFor(x => x.DivisionPath)
            .NotEmpty().WithMessage("区划路径不能为空")
            .MaximumLength(500).WithMessage("区划路径长度不能超过500个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.PhoneCode)
            .NotEmpty().WithMessage("电话区号不能为空")
            .MaximumLength(16).WithMessage("电话区号长度不能超过16个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新AdminDivision 验证器
// ========================================

/// <summary>
/// 更新AdminDivision DTO 验证器
/// </summary>
public class TaktAdminDivisionUpdateValidator : AbstractValidator<TaktAdminDivisionUpdateDto>
{
    /// <summary>
    /// 初始化 更新AdminDivision 校验规则
    /// </summary>
    public TaktAdminDivisionUpdateValidator()
    {
        RuleFor(x => x.AdminDivisionId)
            .GreaterThan(0).WithMessage("AdminDivisionID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("国家代码不能为空")
            .MaximumLength(2).WithMessage("国家代码长度不能超过2个字符");
        RuleFor(x => x.DivisionCode)
            .NotEmpty().WithMessage("区划编码不能为空")
            .MaximumLength(40).WithMessage("区划编码长度不能超过40个字符");
        RuleFor(x => x.DivisionName)
            .NotEmpty().WithMessage("区划名称不能为空")
            .MaximumLength(200).WithMessage("区划名称长度不能超过200个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父级区划ID不能为负数");
        RuleFor(x => x.DivisionPath)
            .NotEmpty().WithMessage("区划路径不能为空")
            .MaximumLength(500).WithMessage("区划路径长度不能超过500个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.PhoneCode)
            .NotEmpty().WithMessage("电话区号不能为空")
            .MaximumLength(16).WithMessage("电话区号长度不能超过16个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入AdminDivision 验证器
// ========================================

/// <summary>
/// 导入AdminDivision DTO 验证器
/// </summary>
public class TaktAdminDivisionImportValidator : AbstractValidator<TaktAdminDivisionImportDto>
{
    /// <summary>
    /// 初始化 导入AdminDivision 校验规则
    /// </summary>
    public TaktAdminDivisionImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("国家代码不能为空")
            .MaximumLength(2).WithMessage("国家代码长度不能超过2个字符");
        RuleFor(x => x.DivisionCode)
            .NotEmpty().WithMessage("区划编码不能为空")
            .MaximumLength(40).WithMessage("区划编码长度不能超过40个字符");
        RuleFor(x => x.DivisionName)
            .NotEmpty().WithMessage("区划名称不能为空")
            .MaximumLength(200).WithMessage("区划名称长度不能超过200个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父级区划ID不能为负数");
        RuleFor(x => x.DivisionPath)
            .NotEmpty().WithMessage("区划路径不能为空")
            .MaximumLength(500).WithMessage("区划路径长度不能超过500个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.PhoneCode)
            .NotEmpty().WithMessage("电话区号不能为空")
            .MaximumLength(16).WithMessage("电话区号长度不能超过16个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
