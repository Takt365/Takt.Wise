// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktVocabularyValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：Vocabulary 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktVocabulary 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建Vocabulary 验证器
// ========================================

/// <summary>
/// 创建Vocabulary DTO 验证器
/// </summary>
public class TaktVocabularyCreateValidator : AbstractValidator<TaktVocabularyCreateDto>
{
    /// <summary>
    /// 初始化 创建Vocabulary 校验规则
    /// </summary>
    public TaktVocabularyCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.WordText)
            .NotEmpty().WithMessage("敏感词文本不能为空")
            .MaximumLength(100).WithMessage("敏感词文本长度不能超过100个字符");
        RuleFor(x => x.ReplaceText)
            .NotEmpty().WithMessage("替换文本不能为空")
            .MaximumLength(6).WithMessage("替换文本长度不能超过6个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Vocabulary 验证器
// ========================================

/// <summary>
/// 更新Vocabulary DTO 验证器
/// </summary>
public class TaktVocabularyUpdateValidator : AbstractValidator<TaktVocabularyUpdateDto>
{
    /// <summary>
    /// 初始化 更新Vocabulary 校验规则
    /// </summary>
    public TaktVocabularyUpdateValidator()
    {
        RuleFor(x => x.VocabularyId)
            .GreaterThan(0).WithMessage("VocabularyID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.WordText)
            .NotEmpty().WithMessage("敏感词文本不能为空")
            .MaximumLength(100).WithMessage("敏感词文本长度不能超过100个字符");
        RuleFor(x => x.ReplaceText)
            .NotEmpty().WithMessage("替换文本不能为空")
            .MaximumLength(6).WithMessage("替换文本长度不能超过6个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Vocabulary 验证器
// ========================================

/// <summary>
/// 导入Vocabulary DTO 验证器
/// </summary>
public class TaktVocabularyImportValidator : AbstractValidator<TaktVocabularyImportDto>
{
    /// <summary>
    /// 初始化 导入Vocabulary 校验规则
    /// </summary>
    public TaktVocabularyImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.WordText)
            .NotEmpty().WithMessage("敏感词文本不能为空")
            .MaximumLength(100).WithMessage("敏感词文本长度不能超过100个字符");
        RuleFor(x => x.ReplaceText)
            .NotEmpty().WithMessage("替换文本不能为空")
            .MaximumLength(6).WithMessage("替换文本长度不能超过6个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
