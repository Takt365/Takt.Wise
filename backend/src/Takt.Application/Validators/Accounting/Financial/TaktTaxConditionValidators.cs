// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Accounting.Financial
// 文件名称：TaktTaxConditionValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaxCondition 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTaxCondition 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Financial;

namespace Takt.Application.Validators.Accounting.Financial;

// ========================================
// 创建TaxCondition 验证器
// ========================================

/// <summary>
/// 创建TaxCondition DTO 验证器
/// </summary>
public class TaktTaxConditionCreateValidator : AbstractValidator<TaktTaxConditionCreateDto>
{
    /// <summary>
    /// 初始化 创建TaxCondition 校验规则
    /// </summary>
    public TaktTaxConditionCreateValidator()
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
        RuleFor(x => x.ApplicationCode)
            .NotEmpty().WithMessage("应用程序不能为空")
            .MaximumLength(2).WithMessage("应用程序长度不能超过2个字符");
        RuleFor(x => x.ConditionTypeCode)
            .NotEmpty().WithMessage("条件类型不能为空")
            .MaximumLength(4).WithMessage("条件类型长度不能超过4个字符");
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("国家不能为空")
            .MaximumLength(2).WithMessage("国家长度不能超过2个字符");
        RuleFor(x => x.TaxCode)
            .NotEmpty().WithMessage("税码不能为空")
            .MaximumLength(2).WithMessage("税码长度不能超过2个字符");
        RuleFor(x => x.ConditionRecordCode)
            .NotEmpty().WithMessage("条件记录号不能为空")
            .MaximumLength(10).WithMessage("条件记录号长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TaxCondition 验证器
// ========================================

/// <summary>
/// 更新TaxCondition DTO 验证器
/// </summary>
public class TaktTaxConditionUpdateValidator : AbstractValidator<TaktTaxConditionUpdateDto>
{
    /// <summary>
    /// 初始化 更新TaxCondition 校验规则
    /// </summary>
    public TaktTaxConditionUpdateValidator()
    {
        RuleFor(x => x.TaxConditionId)
            .GreaterThan(0).WithMessage("TaxConditionID无效");
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
        RuleFor(x => x.ApplicationCode)
            .NotEmpty().WithMessage("应用程序不能为空")
            .MaximumLength(2).WithMessage("应用程序长度不能超过2个字符");
        RuleFor(x => x.ConditionTypeCode)
            .NotEmpty().WithMessage("条件类型不能为空")
            .MaximumLength(4).WithMessage("条件类型长度不能超过4个字符");
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("国家不能为空")
            .MaximumLength(2).WithMessage("国家长度不能超过2个字符");
        RuleFor(x => x.TaxCode)
            .NotEmpty().WithMessage("税码不能为空")
            .MaximumLength(2).WithMessage("税码长度不能超过2个字符");
        RuleFor(x => x.ConditionRecordCode)
            .NotEmpty().WithMessage("条件记录号不能为空")
            .MaximumLength(10).WithMessage("条件记录号长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入TaxCondition 验证器
// ========================================

/// <summary>
/// 导入TaxCondition DTO 验证器
/// </summary>
public class TaktTaxConditionImportValidator : AbstractValidator<TaktTaxConditionImportDto>
{
    /// <summary>
    /// 初始化 导入TaxCondition 校验规则
    /// </summary>
    public TaktTaxConditionImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ApplicationCode)
            .NotEmpty().WithMessage("应用程序不能为空")
            .MaximumLength(2).WithMessage("应用程序长度不能超过2个字符");
        RuleFor(x => x.ConditionTypeCode)
            .NotEmpty().WithMessage("条件类型不能为空")
            .MaximumLength(4).WithMessage("条件类型长度不能超过4个字符");
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("国家不能为空")
            .MaximumLength(2).WithMessage("国家长度不能超过2个字符");
        RuleFor(x => x.TaxCode)
            .NotEmpty().WithMessage("税码不能为空")
            .MaximumLength(2).WithMessage("税码长度不能超过2个字符");
        RuleFor(x => x.ConditionRecordCode)
            .NotEmpty().WithMessage("条件记录号不能为空")
            .MaximumLength(10).WithMessage("条件记录号长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
