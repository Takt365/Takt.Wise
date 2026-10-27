// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcDetailValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：SourceEcDetail 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSourceEcDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 创建SourceEcDetail 验证器
// ========================================

/// <summary>
/// 创建SourceEcDetail DTO 验证器
/// </summary>
public class TaktSourceEcDetailCreateValidator : AbstractValidator<TaktSourceEcDetailCreateDto>
{
    /// <summary>
    /// 初始化 创建SourceEcDetail 校验规则
    /// </summary>
    public TaktSourceEcDetailCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.SourceEcId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.SourceEcId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.SourceEcId)
            .GreaterThanOrEqualTo(0).WithMessage("主ID不能为负数");
        RuleFor(x => x.SourceEcCode)
            .NotEmpty().WithMessage("设变号码不能为空").When(x => x.SourceEcId <= 0)
            .MaximumLength(6).WithMessage("设变号码长度不能超过6个字符");
        RuleFor(x => x.SourceRootMaterialCode)
            .NotEmpty().WithMessage("根物料编码不能为空")
            .MaximumLength(20).WithMessage("根物料编码长度不能超过20个字符");
        RuleFor(x => x.SourceParentMaterialCode)
            .NotEmpty().WithMessage("上阶物料编码不能为空")
            .MaximumLength(20).WithMessage("上阶物料编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SourceEcDetail 验证器
// ========================================

/// <summary>
/// 更新SourceEcDetail DTO 验证器
/// </summary>
public class TaktSourceEcDetailUpdateValidator : AbstractValidator<TaktSourceEcDetailUpdateDto>
{
    /// <summary>
    /// 初始化 更新SourceEcDetail 校验规则
    /// </summary>
    public TaktSourceEcDetailUpdateValidator()
    {
        RuleFor(x => x.SourceEcDetailId)
            .GreaterThan(0).WithMessage("SourceEcDetailID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.SourceEcId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.SourceEcId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.SourceEcId)
            .GreaterThanOrEqualTo(0).WithMessage("主ID不能为负数");
        RuleFor(x => x.SourceEcCode)
            .NotEmpty().WithMessage("设变号码不能为空").When(x => x.SourceEcId <= 0)
            .MaximumLength(6).WithMessage("设变号码长度不能超过6个字符");
        RuleFor(x => x.SourceRootMaterialCode)
            .NotEmpty().WithMessage("根物料编码不能为空")
            .MaximumLength(20).WithMessage("根物料编码长度不能超过20个字符");
        RuleFor(x => x.SourceParentMaterialCode)
            .NotEmpty().WithMessage("上阶物料编码不能为空")
            .MaximumLength(20).WithMessage("上阶物料编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SourceEcDetail 验证器
// ========================================

/// <summary>
/// 导入SourceEcDetail DTO 验证器
/// </summary>
public class TaktSourceEcDetailImportValidator : AbstractValidator<TaktSourceEcDetailImportDto>
{
    /// <summary>
    /// 初始化 导入SourceEcDetail 校验规则
    /// </summary>
    public TaktSourceEcDetailImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SourceEcId)
            .GreaterThanOrEqualTo(0).WithMessage("主ID不能为负数");
        RuleFor(x => x.SourceEcCode)
            .NotEmpty().WithMessage("设变号码不能为空")
            .MaximumLength(6).WithMessage("设变号码长度不能超过6个字符");
        RuleFor(x => x.SourceRootMaterialCode)
            .NotEmpty().WithMessage("根物料编码不能为空")
            .MaximumLength(20).WithMessage("根物料编码长度不能超过20个字符");
        RuleFor(x => x.SourceParentMaterialCode)
            .NotEmpty().WithMessage("上阶物料编码不能为空")
            .MaximumLength(20).WithMessage("上阶物料编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
