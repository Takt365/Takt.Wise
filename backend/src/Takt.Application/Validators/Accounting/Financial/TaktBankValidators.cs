// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Accounting.Financial
// 文件名称：TaktBankValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：Bank 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktBank 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Financial;

namespace Takt.Application.Validators.Accounting.Financial;

// ========================================
// 创建Bank 验证器
// ========================================

/// <summary>
/// 创建Bank DTO 验证器
/// </summary>
public class TaktBankCreateValidator : AbstractValidator<TaktBankCreateDto>
{
    /// <summary>
    /// 初始化 创建Bank 校验规则
    /// </summary>
    public TaktBankCreateValidator()
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
        RuleFor(x => x.CountryRegion)
            .NotEmpty().WithMessage("国家地区不能为空")
            .MaximumLength(2).WithMessage("国家地区长度不能超过2个字符");
        RuleFor(x => x.BankCode)
            .NotEmpty().WithMessage("银行代码不能为空")
            .MaximumLength(15).WithMessage("银行代码长度不能超过15个字符");
        RuleFor(x => x.BankName1)
            .NotEmpty().WithMessage("银行名称1不能为空")
            .MaximumLength(140).WithMessage("银行名称1长度不能超过140个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Bank 验证器
// ========================================

/// <summary>
/// 更新Bank DTO 验证器
/// </summary>
public class TaktBankUpdateValidator : AbstractValidator<TaktBankUpdateDto>
{
    /// <summary>
    /// 初始化 更新Bank 校验规则
    /// </summary>
    public TaktBankUpdateValidator()
    {
        RuleFor(x => x.BankId)
            .GreaterThan(0).WithMessage("BankID无效");
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
        RuleFor(x => x.CountryRegion)
            .NotEmpty().WithMessage("国家地区不能为空")
            .MaximumLength(2).WithMessage("国家地区长度不能超过2个字符");
        RuleFor(x => x.BankCode)
            .NotEmpty().WithMessage("银行代码不能为空")
            .MaximumLength(15).WithMessage("银行代码长度不能超过15个字符");
        RuleFor(x => x.BankName1)
            .NotEmpty().WithMessage("银行名称1不能为空")
            .MaximumLength(140).WithMessage("银行名称1长度不能超过140个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Bank 验证器
// ========================================

/// <summary>
/// 导入Bank DTO 验证器
/// </summary>
public class TaktBankImportValidator : AbstractValidator<TaktBankImportDto>
{
    /// <summary>
    /// 初始化 导入Bank 校验规则
    /// </summary>
    public TaktBankImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.CountryRegion)
            .NotEmpty().WithMessage("国家地区不能为空")
            .MaximumLength(2).WithMessage("国家地区长度不能超过2个字符");
        RuleFor(x => x.BankCode)
            .NotEmpty().WithMessage("银行代码不能为空")
            .MaximumLength(15).WithMessage("银行代码长度不能超过15个字符");
        RuleFor(x => x.BankName1)
            .NotEmpty().WithMessage("银行名称1不能为空")
            .MaximumLength(140).WithMessage("银行名称1长度不能超过140个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
