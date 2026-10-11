// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktMaterialMovingPriceValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialMovingPrice 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaterialMovingPrice 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建MaterialMovingPrice 验证器
// ========================================

/// <summary>
/// 创建MaterialMovingPrice DTO 验证器
/// </summary>
public class TaktMaterialMovingPriceCreateValidator : AbstractValidator<TaktMaterialMovingPriceCreateDto>
{
    /// <summary>
    /// 初始化 创建MaterialMovingPrice 校验规则
    /// </summary>
    public TaktMaterialMovingPriceCreateValidator()
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
        RuleFor(x => x.ValuationPeriod)
            .NotEmpty().WithMessage("评估期间不能为空")
            .MaximumLength(7).WithMessage("评估期间长度不能超过7个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.Valuation)
            .NotEmpty().WithMessage("评估类别不能为空")
            .MaximumLength(4).WithMessage("评估类别长度不能超过4个字符");
        RuleFor(x => x.PriceControl)
            .NotEmpty().WithMessage("价格控制不能为空")
            .MaximumLength(1).WithMessage("价格控制长度不能超过1个字符");
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
// 更新MaterialMovingPrice 验证器
// ========================================

/// <summary>
/// 更新MaterialMovingPrice DTO 验证器
/// </summary>
public class TaktMaterialMovingPriceUpdateValidator : AbstractValidator<TaktMaterialMovingPriceUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaterialMovingPrice 校验规则
    /// </summary>
    public TaktMaterialMovingPriceUpdateValidator()
    {
        RuleFor(x => x.MaterialMovingPriceId)
            .GreaterThan(0).WithMessage("MaterialMovingPriceID无效");
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
        RuleFor(x => x.ValuationPeriod)
            .NotEmpty().WithMessage("评估期间不能为空")
            .MaximumLength(7).WithMessage("评估期间长度不能超过7个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.Valuation)
            .NotEmpty().WithMessage("评估类别不能为空")
            .MaximumLength(4).WithMessage("评估类别长度不能超过4个字符");
        RuleFor(x => x.PriceControl)
            .NotEmpty().WithMessage("价格控制不能为空")
            .MaximumLength(1).WithMessage("价格控制长度不能超过1个字符");
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
// 导入MaterialMovingPrice 验证器
// ========================================

/// <summary>
/// 导入MaterialMovingPrice DTO 验证器
/// </summary>
public class TaktMaterialMovingPriceImportValidator : AbstractValidator<TaktMaterialMovingPriceImportDto>
{
    /// <summary>
    /// 初始化 导入MaterialMovingPrice 校验规则
    /// </summary>
    public TaktMaterialMovingPriceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ValuationPeriod)
            .NotEmpty().WithMessage("评估期间不能为空")
            .MaximumLength(7).WithMessage("评估期间长度不能超过7个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.Valuation)
            .NotEmpty().WithMessage("评估类别不能为空")
            .MaximumLength(4).WithMessage("评估类别长度不能超过4个字符");
        RuleFor(x => x.PriceControl)
            .NotEmpty().WithMessage("价格控制不能为空")
            .MaximumLength(1).WithMessage("价格控制长度不能超过1个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
