// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktSalesConditionRecordScaleQuantityValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesConditionRecordScaleQuantity 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalesConditionRecordScaleQuantity 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建SalesConditionRecordScaleQuantity 验证器
// ========================================

/// <summary>
/// 创建SalesConditionRecordScaleQuantity DTO 验证器
/// </summary>
public class TaktSalesConditionRecordScaleQuantityCreateValidator : AbstractValidator<TaktSalesConditionRecordScaleQuantityCreateDto>
{
    /// <summary>
    /// 初始化 创建SalesConditionRecordScaleQuantity 校验规则
    /// </summary>
    public TaktSalesConditionRecordScaleQuantityCreateValidator()
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
        RuleFor(x => x.SalesConditionRecordItemId)
            .GreaterThanOrEqualTo(0).WithMessage("销售条件记录行 ID不能为负数");
        RuleFor(x => x.SalesConditionRecordCode)
            .NotEmpty().WithMessage("定价记录号不能为空")
            .MaximumLength(20).WithMessage("定价记录号长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalesConditionRecordScaleQuantity 验证器
// ========================================

/// <summary>
/// 更新SalesConditionRecordScaleQuantity DTO 验证器
/// </summary>
public class TaktSalesConditionRecordScaleQuantityUpdateValidator : AbstractValidator<TaktSalesConditionRecordScaleQuantityUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalesConditionRecordScaleQuantity 校验规则
    /// </summary>
    public TaktSalesConditionRecordScaleQuantityUpdateValidator()
    {
        RuleFor(x => x.SalesConditionRecordScaleQuantityId)
            .GreaterThan(0).WithMessage("SalesConditionRecordScaleQuantityID无效");
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
        RuleFor(x => x.SalesConditionRecordItemId)
            .GreaterThanOrEqualTo(0).WithMessage("销售条件记录行 ID不能为负数");
        RuleFor(x => x.SalesConditionRecordCode)
            .NotEmpty().WithMessage("定价记录号不能为空")
            .MaximumLength(20).WithMessage("定价记录号长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SalesConditionRecordScaleQuantity 验证器
// ========================================

/// <summary>
/// 导入SalesConditionRecordScaleQuantity DTO 验证器
/// </summary>
public class TaktSalesConditionRecordScaleQuantityImportValidator : AbstractValidator<TaktSalesConditionRecordScaleQuantityImportDto>
{
    /// <summary>
    /// 初始化 导入SalesConditionRecordScaleQuantity 校验规则
    /// </summary>
    public TaktSalesConditionRecordScaleQuantityImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SalesConditionRecordItemId)
            .GreaterThanOrEqualTo(0).WithMessage("销售条件记录行 ID不能为负数");
        RuleFor(x => x.SalesConditionRecordCode)
            .NotEmpty().WithMessage("定价记录号不能为空")
            .MaximumLength(20).WithMessage("定价记录号长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
