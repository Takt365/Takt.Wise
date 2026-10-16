// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Accounting.Financial
// 文件名称：TaktExchangeRateValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：ExchangeRate 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktExchangeRate 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Financial;

namespace Takt.Application.Validators.Accounting.Financial;

// ========================================
// 创建ExchangeRate 验证器
// ========================================

/// <summary>
/// 创建ExchangeRate DTO 验证器
/// </summary>
public class TaktExchangeRateCreateValidator : AbstractValidator<TaktExchangeRateCreateDto>
{
    /// <summary>
    /// 初始化 创建ExchangeRate 校验规则
    /// </summary>
    public TaktExchangeRateCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.FromCurrencyCode)
            .NotEmpty().WithMessage("源币种不能为空")
            .MaximumLength(3).WithMessage("源币种长度不能超过3个字符");
        RuleFor(x => x.ToCurrencyCode)
            .NotEmpty().WithMessage("目标币种不能为空")
            .MaximumLength(3).WithMessage("目标币种长度不能超过3个字符");
        RuleFor(x => x.ExchangeRateType)
            .NotEmpty().WithMessage("汇率类型不能为空")
            .MaximumLength(1).WithMessage("汇率类型长度不能超过1个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ExchangeRate 验证器
// ========================================

/// <summary>
/// 更新ExchangeRate DTO 验证器
/// </summary>
public class TaktExchangeRateUpdateValidator : AbstractValidator<TaktExchangeRateUpdateDto>
{
    /// <summary>
    /// 初始化 更新ExchangeRate 校验规则
    /// </summary>
    public TaktExchangeRateUpdateValidator()
    {
        RuleFor(x => x.ExchangeRateId)
            .GreaterThan(0).WithMessage("ExchangeRateID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.FromCurrencyCode)
            .NotEmpty().WithMessage("源币种不能为空")
            .MaximumLength(3).WithMessage("源币种长度不能超过3个字符");
        RuleFor(x => x.ToCurrencyCode)
            .NotEmpty().WithMessage("目标币种不能为空")
            .MaximumLength(3).WithMessage("目标币种长度不能超过3个字符");
        RuleFor(x => x.ExchangeRateType)
            .NotEmpty().WithMessage("汇率类型不能为空")
            .MaximumLength(1).WithMessage("汇率类型长度不能超过1个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入ExchangeRate 验证器
// ========================================

/// <summary>
/// 导入ExchangeRate DTO 验证器
/// </summary>
public class TaktExchangeRateImportValidator : AbstractValidator<TaktExchangeRateImportDto>
{
    /// <summary>
    /// 初始化 导入ExchangeRate 校验规则
    /// </summary>
    public TaktExchangeRateImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.FromCurrencyCode)
            .NotEmpty().WithMessage("源币种不能为空")
            .MaximumLength(3).WithMessage("源币种长度不能超过3个字符");
        RuleFor(x => x.ToCurrencyCode)
            .NotEmpty().WithMessage("目标币种不能为空")
            .MaximumLength(3).WithMessage("目标币种长度不能超过3个字符");
        RuleFor(x => x.ExchangeRateType)
            .NotEmpty().WithMessage("汇率类型不能为空")
            .MaximumLength(1).WithMessage("汇率类型长度不能超过1个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
