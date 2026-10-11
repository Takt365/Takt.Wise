// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktCultureValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：Culture 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCulture 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建Culture 验证器
// ========================================

/// <summary>
/// 创建Culture DTO 验证器
/// </summary>
public class TaktCultureCreateValidator : AbstractValidator<TaktCultureCreateDto>
{
    /// <summary>
    /// 初始化 创建Culture 校验规则
    /// </summary>
    public TaktCultureCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.NativeName)
            .NotEmpty().WithMessage("本地化名称不能为空")
            .MaximumLength(40).WithMessage("本地化名称长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Culture 验证器
// ========================================

/// <summary>
/// 更新Culture DTO 验证器
/// </summary>
public class TaktCultureUpdateValidator : AbstractValidator<TaktCultureUpdateDto>
{
    /// <summary>
    /// 初始化 更新Culture 校验规则
    /// </summary>
    public TaktCultureUpdateValidator()
    {
        RuleFor(x => x.CultureId)
            .GreaterThan(0).WithMessage("CultureID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.NativeName)
            .NotEmpty().WithMessage("本地化名称不能为空")
            .MaximumLength(40).WithMessage("本地化名称长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Culture 验证器
// ========================================

/// <summary>
/// 导入Culture DTO 验证器
/// </summary>
public class TaktCultureImportValidator : AbstractValidator<TaktCultureImportDto>
{
    /// <summary>
    /// 初始化 导入Culture 校验规则
    /// </summary>
    public TaktCultureImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.NativeName)
            .NotEmpty().WithMessage("本地化名称不能为空")
            .MaximumLength(40).WithMessage("本地化名称长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
