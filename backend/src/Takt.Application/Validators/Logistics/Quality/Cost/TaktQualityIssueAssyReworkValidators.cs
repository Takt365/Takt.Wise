// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueAssyReworkValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityIssueAssyRework 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktQualityIssueAssyRework 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Cost;

namespace Takt.Application.Validators.Logistics.Quality.Cost;

// ========================================
// 创建QualityIssueAssyRework 验证器
// ========================================

/// <summary>
/// 创建QualityIssueAssyRework DTO 验证器
/// </summary>
public class TaktQualityIssueAssyReworkCreateValidator : AbstractValidator<TaktQualityIssueAssyReworkCreateDto>
{
    /// <summary>
    /// 初始化 创建QualityIssueAssyRework 校验规则
    /// </summary>
    public TaktQualityIssueAssyReworkCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.QualityIssueId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.QualityIssueId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.QualityIssueId)
            .GreaterThanOrEqualTo(0).WithMessage("品质问题主表 ID不能为负数");
        RuleFor(x => x.QualityIssueCode)
            .NotEmpty().WithMessage("品质问题编码不能为空").When(x => x.QualityIssueId <= 0)
            .MaximumLength(20).WithMessage("品质问题编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新QualityIssueAssyRework 验证器
// ========================================

/// <summary>
/// 更新QualityIssueAssyRework DTO 验证器
/// </summary>
public class TaktQualityIssueAssyReworkUpdateValidator : AbstractValidator<TaktQualityIssueAssyReworkUpdateDto>
{
    /// <summary>
    /// 初始化 更新QualityIssueAssyRework 校验规则
    /// </summary>
    public TaktQualityIssueAssyReworkUpdateValidator()
    {
        RuleFor(x => x.QualityIssueAssyReworkId)
            .GreaterThan(0).WithMessage("QualityIssueAssyReworkID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.QualityIssueId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.QualityIssueId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.QualityIssueId)
            .GreaterThanOrEqualTo(0).WithMessage("品质问题主表 ID不能为负数");
        RuleFor(x => x.QualityIssueCode)
            .NotEmpty().WithMessage("品质问题编码不能为空").When(x => x.QualityIssueId <= 0)
            .MaximumLength(20).WithMessage("品质问题编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入QualityIssueAssyRework 验证器
// ========================================

/// <summary>
/// 导入QualityIssueAssyRework DTO 验证器
/// </summary>
public class TaktQualityIssueAssyReworkImportValidator : AbstractValidator<TaktQualityIssueAssyReworkImportDto>
{
    /// <summary>
    /// 初始化 导入QualityIssueAssyRework 校验规则
    /// </summary>
    public TaktQualityIssueAssyReworkImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.QualityIssueId)
            .GreaterThanOrEqualTo(0).WithMessage("品质问题主表 ID不能为负数");
        RuleFor(x => x.QualityIssueCode)
            .NotEmpty().WithMessage("品质问题编码不能为空")
            .MaximumLength(20).WithMessage("品质问题编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
