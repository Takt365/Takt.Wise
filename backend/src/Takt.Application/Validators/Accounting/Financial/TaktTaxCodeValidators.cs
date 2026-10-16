// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Accounting.Financial
// 文件名称：TaktTaxCodeValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaxCode 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTaxCode 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Financial;

namespace Takt.Application.Validators.Accounting.Financial;

// ========================================
// 创建TaxCode 验证器
// ========================================

/// <summary>
/// 创建TaxCode DTO 验证器
/// </summary>
public class TaktTaxCodeCreateValidator : AbstractValidator<TaktTaxCodeCreateDto>
{
    /// <summary>
    /// 初始化 创建TaxCode 校验规则
    /// </summary>
    public TaktTaxCodeCreateValidator()
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
        RuleFor(x => x.TaxProcedureCode)
            .NotEmpty().WithMessage("过程不能为空")
            .MaximumLength(6).WithMessage("过程长度不能超过6个字符");
        RuleFor(x => x.TaxCode)
            .NotEmpty().WithMessage("税码不能为空")
            .MaximumLength(2).WithMessage("税码长度不能超过2个字符");
        RuleFor(x => x.TaxCategory)
            .NotEmpty().WithMessage("税类别不能为空")
            .MaximumLength(1).WithMessage("税类别长度不能超过1个字符");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("描述不能为空")
            .MaximumLength(50).WithMessage("描述长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TaxCode 验证器
// ========================================

/// <summary>
/// 更新TaxCode DTO 验证器
/// </summary>
public class TaktTaxCodeUpdateValidator : AbstractValidator<TaktTaxCodeUpdateDto>
{
    /// <summary>
    /// 初始化 更新TaxCode 校验规则
    /// </summary>
    public TaktTaxCodeUpdateValidator()
    {
        RuleFor(x => x.TaxCodeId)
            .GreaterThan(0).WithMessage("TaxCodeID无效");
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
        RuleFor(x => x.TaxProcedureCode)
            .NotEmpty().WithMessage("过程不能为空")
            .MaximumLength(6).WithMessage("过程长度不能超过6个字符");
        RuleFor(x => x.TaxCode)
            .NotEmpty().WithMessage("税码不能为空")
            .MaximumLength(2).WithMessage("税码长度不能超过2个字符");
        RuleFor(x => x.TaxCategory)
            .NotEmpty().WithMessage("税类别不能为空")
            .MaximumLength(1).WithMessage("税类别长度不能超过1个字符");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("描述不能为空")
            .MaximumLength(50).WithMessage("描述长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入TaxCode 验证器
// ========================================

/// <summary>
/// 导入TaxCode DTO 验证器
/// </summary>
public class TaktTaxCodeImportValidator : AbstractValidator<TaktTaxCodeImportDto>
{
    /// <summary>
    /// 初始化 导入TaxCode 校验规则
    /// </summary>
    public TaktTaxCodeImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.TaxProcedureCode)
            .NotEmpty().WithMessage("过程不能为空")
            .MaximumLength(6).WithMessage("过程长度不能超过6个字符");
        RuleFor(x => x.TaxCode)
            .NotEmpty().WithMessage("税码不能为空")
            .MaximumLength(2).WithMessage("税码长度不能超过2个字符");
        RuleFor(x => x.TaxCategory)
            .NotEmpty().WithMessage("税类别不能为空")
            .MaximumLength(1).WithMessage("税类别长度不能超过1个字符");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("描述不能为空")
            .MaximumLength(50).WithMessage("描述长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
