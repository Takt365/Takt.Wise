// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Accounting.Controlling
// 文件名称：TaktCostElementValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：CostElement 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCostElement 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Controlling;

namespace Takt.Application.Validators.Accounting.Controlling;

// ========================================
// 创建CostElement 验证器
// ========================================

/// <summary>
/// 创建CostElement DTO 验证器
/// </summary>
public class TaktCostElementCreateValidator : AbstractValidator<TaktCostElementCreateDto>
{
    /// <summary>
    /// 初始化 创建CostElement 校验规则
    /// </summary>
    public TaktCostElementCreateValidator()
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
        RuleFor(x => x.CostElementCode)
            .NotEmpty().WithMessage("成本要素编码不能为空")
            .MaximumLength(4).WithMessage("成本要素编码长度不能超过4个字符");
        RuleFor(x => x.CostElementName)
            .NotEmpty().WithMessage("成本要素名称不能为空")
            .MaximumLength(100).WithMessage("成本要素名称长度不能超过100个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父级 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新CostElement 验证器
// ========================================

/// <summary>
/// 更新CostElement DTO 验证器
/// </summary>
public class TaktCostElementUpdateValidator : AbstractValidator<TaktCostElementUpdateDto>
{
    /// <summary>
    /// 初始化 更新CostElement 校验规则
    /// </summary>
    public TaktCostElementUpdateValidator()
    {
        RuleFor(x => x.CostElementId)
            .GreaterThan(0).WithMessage("CostElementID无效");
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
        RuleFor(x => x.CostElementCode)
            .NotEmpty().WithMessage("成本要素编码不能为空")
            .MaximumLength(4).WithMessage("成本要素编码长度不能超过4个字符");
        RuleFor(x => x.CostElementName)
            .NotEmpty().WithMessage("成本要素名称不能为空")
            .MaximumLength(100).WithMessage("成本要素名称长度不能超过100个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父级 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入CostElement 验证器
// ========================================

/// <summary>
/// 导入CostElement DTO 验证器
/// </summary>
public class TaktCostElementImportValidator : AbstractValidator<TaktCostElementImportDto>
{
    /// <summary>
    /// 初始化 导入CostElement 校验规则
    /// </summary>
    public TaktCostElementImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.CostElementCode)
            .NotEmpty().WithMessage("成本要素编码不能为空")
            .MaximumLength(4).WithMessage("成本要素编码长度不能超过4个字符");
        RuleFor(x => x.CostElementName)
            .NotEmpty().WithMessage("成本要素名称不能为空")
            .MaximumLength(100).WithMessage("成本要素名称长度不能超过100个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父级 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
