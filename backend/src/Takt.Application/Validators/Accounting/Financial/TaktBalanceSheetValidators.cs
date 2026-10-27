// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Accounting.Financial
// 文件名称：TaktBalanceSheetValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：BalanceSheet 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktBalanceSheet 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Financial;

namespace Takt.Application.Validators.Accounting.Financial;

// ========================================
// 创建BalanceSheet 验证器
// ========================================

/// <summary>
/// 创建BalanceSheet DTO 验证器
/// </summary>
public class TaktBalanceSheetCreateValidator : AbstractValidator<TaktBalanceSheetCreateDto>
{
    /// <summary>
    /// 初始化 创建BalanceSheet 校验规则
    /// </summary>
    public TaktBalanceSheetCreateValidator()
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
        RuleFor(x => x.PeriodCode)
            .NotEmpty().WithMessage("会计期间编码不能为空")
            .MaximumLength(6).WithMessage("会计期间编码长度不能超过6个字符");
        RuleFor(x => x.StatementLineCode)
            .NotEmpty().WithMessage("报表项目编码不能为空")
            .MaximumLength(40).WithMessage("报表项目编码长度不能超过40个字符");
        RuleFor(x => x.StatementLineName)
            .NotEmpty().WithMessage("报表项目名称不能为空")
            .MaximumLength(200).WithMessage("报表项目名称长度不能超过200个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新BalanceSheet 验证器
// ========================================

/// <summary>
/// 更新BalanceSheet DTO 验证器
/// </summary>
public class TaktBalanceSheetUpdateValidator : AbstractValidator<TaktBalanceSheetUpdateDto>
{
    /// <summary>
    /// 初始化 更新BalanceSheet 校验规则
    /// </summary>
    public TaktBalanceSheetUpdateValidator()
    {
        RuleFor(x => x.BalanceSheetId)
            .GreaterThan(0).WithMessage("BalanceSheetID无效");
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
        RuleFor(x => x.PeriodCode)
            .NotEmpty().WithMessage("会计期间编码不能为空")
            .MaximumLength(6).WithMessage("会计期间编码长度不能超过6个字符");
        RuleFor(x => x.StatementLineCode)
            .NotEmpty().WithMessage("报表项目编码不能为空")
            .MaximumLength(40).WithMessage("报表项目编码长度不能超过40个字符");
        RuleFor(x => x.StatementLineName)
            .NotEmpty().WithMessage("报表项目名称不能为空")
            .MaximumLength(200).WithMessage("报表项目名称长度不能超过200个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入BalanceSheet 验证器
// ========================================

/// <summary>
/// 导入BalanceSheet DTO 验证器
/// </summary>
public class TaktBalanceSheetImportValidator : AbstractValidator<TaktBalanceSheetImportDto>
{
    /// <summary>
    /// 初始化 导入BalanceSheet 校验规则
    /// </summary>
    public TaktBalanceSheetImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.PeriodCode)
            .NotEmpty().WithMessage("会计期间编码不能为空")
            .MaximumLength(6).WithMessage("会计期间编码长度不能超过6个字符");
        RuleFor(x => x.StatementLineCode)
            .NotEmpty().WithMessage("报表项目编码不能为空")
            .MaximumLength(40).WithMessage("报表项目编码长度不能超过40个字符");
        RuleFor(x => x.StatementLineName)
            .NotEmpty().WithMessage("报表项目名称不能为空")
            .MaximumLength(200).WithMessage("报表项目名称长度不能超过200个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
