// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktSalesConditionItemValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesConditionItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalesConditionItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建SalesConditionItem 验证器
// ========================================

/// <summary>
/// 创建SalesConditionItem DTO 验证器
/// </summary>
public class TaktSalesConditionItemCreateValidator : AbstractValidator<TaktSalesConditionItemCreateDto>
{
    /// <summary>
    /// 初始化 创建SalesConditionItem 校验规则
    /// </summary>
    public TaktSalesConditionItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.SalesConditionId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.SalesConditionId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.SalesConditionId)
            .GreaterThanOrEqualTo(0).WithMessage("销售价格 ID不能为负数");
        RuleFor(x => x.SalesConditionCode)
            .NotEmpty().WithMessage("定价记录号不能为空").When(x => x.SalesConditionId <= 0)
            .MaximumLength(20).WithMessage("定价记录号长度不能超过20个字符");
        RuleFor(x => x.PriceType)
            .NotEmpty().WithMessage("条件类型不能为空")
            .MaximumLength(4).WithMessage("条件类型长度不能超过4个字符");
        RuleFor(x => x.CalculationType)
            .NotEmpty().WithMessage("计算类型不能为空")
            .MaximumLength(1).WithMessage("计算类型长度不能超过1个字符");
        RuleFor(x => x.ConditionCurrencyCode)
            .NotEmpty().WithMessage("条件货币不能为空")
            .MaximumLength(3).WithMessage("条件货币长度不能超过3个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(5).WithMessage("计量单位长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalesConditionItem 验证器
// ========================================

/// <summary>
/// 更新SalesConditionItem DTO 验证器
/// </summary>
public class TaktSalesConditionItemUpdateValidator : AbstractValidator<TaktSalesConditionItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalesConditionItem 校验规则
    /// </summary>
    public TaktSalesConditionItemUpdateValidator()
    {
        RuleFor(x => x.SalesConditionItemId)
            .GreaterThan(0).WithMessage("SalesConditionItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.SalesConditionId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.SalesConditionId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.SalesConditionId)
            .GreaterThanOrEqualTo(0).WithMessage("销售价格 ID不能为负数");
        RuleFor(x => x.SalesConditionCode)
            .NotEmpty().WithMessage("定价记录号不能为空").When(x => x.SalesConditionId <= 0)
            .MaximumLength(20).WithMessage("定价记录号长度不能超过20个字符");
        RuleFor(x => x.PriceType)
            .NotEmpty().WithMessage("条件类型不能为空")
            .MaximumLength(4).WithMessage("条件类型长度不能超过4个字符");
        RuleFor(x => x.CalculationType)
            .NotEmpty().WithMessage("计算类型不能为空")
            .MaximumLength(1).WithMessage("计算类型长度不能超过1个字符");
        RuleFor(x => x.ConditionCurrencyCode)
            .NotEmpty().WithMessage("条件货币不能为空")
            .MaximumLength(3).WithMessage("条件货币长度不能超过3个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(5).WithMessage("计量单位长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SalesConditionItem 验证器
// ========================================

/// <summary>
/// 导入SalesConditionItem DTO 验证器
/// </summary>
public class TaktSalesConditionItemImportValidator : AbstractValidator<TaktSalesConditionItemImportDto>
{
    /// <summary>
    /// 初始化 导入SalesConditionItem 校验规则
    /// </summary>
    public TaktSalesConditionItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SalesConditionId)
            .GreaterThanOrEqualTo(0).WithMessage("销售价格 ID不能为负数");
        RuleFor(x => x.SalesConditionCode)
            .NotEmpty().WithMessage("定价记录号不能为空")
            .MaximumLength(20).WithMessage("定价记录号长度不能超过20个字符");
        RuleFor(x => x.PriceType)
            .NotEmpty().WithMessage("条件类型不能为空")
            .MaximumLength(4).WithMessage("条件类型长度不能超过4个字符");
        RuleFor(x => x.CalculationType)
            .NotEmpty().WithMessage("计算类型不能为空")
            .MaximumLength(1).WithMessage("计算类型长度不能超过1个字符");
        RuleFor(x => x.ConditionCurrencyCode)
            .NotEmpty().WithMessage("条件货币不能为空")
            .MaximumLength(3).WithMessage("条件货币长度不能超过3个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(5).WithMessage("计量单位长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
