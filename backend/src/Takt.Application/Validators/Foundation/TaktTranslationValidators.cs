// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktTranslationValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：Translation 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTranslation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建Translation 验证器
// ========================================

/// <summary>
/// 创建Translation DTO 验证器
/// </summary>
public class TaktTranslationCreateValidator : AbstractValidator<TaktTranslationCreateDto>
{
    /// <summary>
    /// 初始化 创建Translation 校验规则
    /// </summary>
    public TaktTranslationCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CultureId)
            .GreaterThanOrEqualTo(0).WithMessage("区域文化不能为负数");
        RuleFor(x => x.I18nKey)
            .NotEmpty().WithMessage("翻译键不能为空")
            .MaximumLength(140).WithMessage("翻译键长度不能超过140个字符");
        RuleFor(x => x.TranslationText)
            .NotEmpty().WithMessage("翻译文本不能为空")
            .MaximumLength(2000).WithMessage("翻译文本长度不能超过2000个字符");
        RuleFor(x => x.ResourceGroup)
            .NotEmpty().WithMessage("资源分组不能为空")
            .MaximumLength(40).WithMessage("资源分组长度不能超过40个字符");
        RuleFor(x => x.ResourceType)
            .NotEmpty().WithMessage("资源类别不能为空")
            .MaximumLength(40).WithMessage("资源类别长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Translation 验证器
// ========================================

/// <summary>
/// 更新Translation DTO 验证器
/// </summary>
public class TaktTranslationUpdateValidator : AbstractValidator<TaktTranslationUpdateDto>
{
    /// <summary>
    /// 初始化 更新Translation 校验规则
    /// </summary>
    public TaktTranslationUpdateValidator()
    {
        RuleFor(x => x.TranslationId)
            .GreaterThan(0).WithMessage("TranslationID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CultureId)
            .GreaterThanOrEqualTo(0).WithMessage("区域文化不能为负数");
        RuleFor(x => x.I18nKey)
            .NotEmpty().WithMessage("翻译键不能为空")
            .MaximumLength(140).WithMessage("翻译键长度不能超过140个字符");
        RuleFor(x => x.TranslationText)
            .NotEmpty().WithMessage("翻译文本不能为空")
            .MaximumLength(2000).WithMessage("翻译文本长度不能超过2000个字符");
        RuleFor(x => x.ResourceGroup)
            .NotEmpty().WithMessage("资源分组不能为空")
            .MaximumLength(40).WithMessage("资源分组长度不能超过40个字符");
        RuleFor(x => x.ResourceType)
            .NotEmpty().WithMessage("资源类别不能为空")
            .MaximumLength(40).WithMessage("资源类别长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Translation 验证器
// ========================================

/// <summary>
/// 导入Translation DTO 验证器
/// </summary>
public class TaktTranslationImportValidator : AbstractValidator<TaktTranslationImportDto>
{
    /// <summary>
    /// 初始化 导入Translation 校验规则
    /// </summary>
    public TaktTranslationImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CultureId)
            .GreaterThanOrEqualTo(0).WithMessage("区域文化不能为负数");
        RuleFor(x => x.I18nKey)
            .NotEmpty().WithMessage("翻译键不能为空")
            .MaximumLength(140).WithMessage("翻译键长度不能超过140个字符");
        RuleFor(x => x.TranslationText)
            .NotEmpty().WithMessage("翻译文本不能为空")
            .MaximumLength(2000).WithMessage("翻译文本长度不能超过2000个字符");
        RuleFor(x => x.ResourceGroup)
            .NotEmpty().WithMessage("资源分组不能为空")
            .MaximumLength(40).WithMessage("资源分组长度不能超过40个字符");
        RuleFor(x => x.ResourceType)
            .NotEmpty().WithMessage("资源类别不能为空")
            .MaximumLength(40).WithMessage("资源类别长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
