// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Accounting.Financial
// 文件名称：TaktFinancialPeriodValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：FinancialPeriod 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktFinancialPeriod 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Financial;

namespace Takt.Application.Validators.Accounting.Financial;

// ========================================
// 创建FinancialPeriod 验证器
// ========================================

/// <summary>
/// 创建FinancialPeriod DTO 验证器
/// </summary>
public class TaktFinancialPeriodCreateValidator : AbstractValidator<TaktFinancialPeriodCreateDto>
{
    /// <summary>
    /// 初始化 创建FinancialPeriod 校验规则
    /// </summary>
    public TaktFinancialPeriodCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("国家代码不能为空")
            .MaximumLength(2).WithMessage("国家代码长度不能超过2个字符");
        RuleFor(x => x.FinancialYearCode)
            .NotEmpty().WithMessage("财务年度编码不能为空")
            .MaximumLength(6).WithMessage("财务年度编码长度不能超过6个字符");
        RuleFor(x => x.PeriodCode)
            .NotEmpty().WithMessage("会计期间编码不能为空")
            .MaximumLength(6).WithMessage("会计期间编码长度不能超过6个字符");
        RuleFor(x => x.FinancialQuarterCode)
            .NotEmpty().WithMessage("财季编码不能为空")
            .MaximumLength(2).WithMessage("财季编码长度不能超过2个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新FinancialPeriod 验证器
// ========================================

/// <summary>
/// 更新FinancialPeriod DTO 验证器
/// </summary>
public class TaktFinancialPeriodUpdateValidator : AbstractValidator<TaktFinancialPeriodUpdateDto>
{
    /// <summary>
    /// 初始化 更新FinancialPeriod 校验规则
    /// </summary>
    public TaktFinancialPeriodUpdateValidator()
    {
        RuleFor(x => x.FinancialPeriodId)
            .GreaterThan(0).WithMessage("FinancialPeriodID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("国家代码不能为空")
            .MaximumLength(2).WithMessage("国家代码长度不能超过2个字符");
        RuleFor(x => x.FinancialYearCode)
            .NotEmpty().WithMessage("财务年度编码不能为空")
            .MaximumLength(6).WithMessage("财务年度编码长度不能超过6个字符");
        RuleFor(x => x.PeriodCode)
            .NotEmpty().WithMessage("会计期间编码不能为空")
            .MaximumLength(6).WithMessage("会计期间编码长度不能超过6个字符");
        RuleFor(x => x.FinancialQuarterCode)
            .NotEmpty().WithMessage("财季编码不能为空")
            .MaximumLength(2).WithMessage("财季编码长度不能超过2个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入FinancialPeriod 验证器
// ========================================

/// <summary>
/// 导入FinancialPeriod DTO 验证器
/// </summary>
public class TaktFinancialPeriodImportValidator : AbstractValidator<TaktFinancialPeriodImportDto>
{
    /// <summary>
    /// 初始化 导入FinancialPeriod 校验规则
    /// </summary>
    public TaktFinancialPeriodImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("国家代码不能为空")
            .MaximumLength(2).WithMessage("国家代码长度不能超过2个字符");
        RuleFor(x => x.FinancialYearCode)
            .NotEmpty().WithMessage("财务年度编码不能为空")
            .MaximumLength(6).WithMessage("财务年度编码长度不能超过6个字符");
        RuleFor(x => x.PeriodCode)
            .NotEmpty().WithMessage("会计期间编码不能为空")
            .MaximumLength(6).WithMessage("会计期间编码长度不能超过6个字符");
        RuleFor(x => x.FinancialQuarterCode)
            .NotEmpty().WithMessage("财季编码不能为空")
            .MaximumLength(2).WithMessage("财季编码长度不能超过2个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
