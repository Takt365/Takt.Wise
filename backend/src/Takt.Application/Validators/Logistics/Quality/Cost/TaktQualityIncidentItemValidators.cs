// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Quality.Cost
// 文件名称：TaktQualityIncidentItemValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityIncidentItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktQualityIncidentItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Cost;

namespace Takt.Application.Validators.Logistics.Quality.Cost;

// ========================================
// 创建QualityIncidentItem 验证器
// ========================================

/// <summary>
/// 创建QualityIncidentItem DTO 验证器
/// </summary>
public class TaktQualityIncidentItemCreateValidator : AbstractValidator<TaktQualityIncidentItemCreateDto>
{
    /// <summary>
    /// 初始化 创建QualityIncidentItem 校验规则
    /// </summary>
    public TaktQualityIncidentItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.QualityIncidentId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.QualityIncidentId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.QualityIncidentId)
            .GreaterThanOrEqualTo(0).WithMessage("品质事故主表 ID不能为负数");
        RuleFor(x => x.QualityIncidentCode)
            .NotEmpty().WithMessage("品质事故编码不能为空").When(x => x.QualityIncidentId <= 0)
            .MaximumLength(20).WithMessage("品质事故编码长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(70).WithMessage("物料描述长度不能超过70个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新QualityIncidentItem 验证器
// ========================================

/// <summary>
/// 更新QualityIncidentItem DTO 验证器
/// </summary>
public class TaktQualityIncidentItemUpdateValidator : AbstractValidator<TaktQualityIncidentItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新QualityIncidentItem 校验规则
    /// </summary>
    public TaktQualityIncidentItemUpdateValidator()
    {
        RuleFor(x => x.QualityIncidentItemId)
            .GreaterThan(0).WithMessage("QualityIncidentItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.QualityIncidentId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.QualityIncidentId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.QualityIncidentId)
            .GreaterThanOrEqualTo(0).WithMessage("品质事故主表 ID不能为负数");
        RuleFor(x => x.QualityIncidentCode)
            .NotEmpty().WithMessage("品质事故编码不能为空").When(x => x.QualityIncidentId <= 0)
            .MaximumLength(20).WithMessage("品质事故编码长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(70).WithMessage("物料描述长度不能超过70个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入QualityIncidentItem 验证器
// ========================================

/// <summary>
/// 导入QualityIncidentItem DTO 验证器
/// </summary>
public class TaktQualityIncidentItemImportValidator : AbstractValidator<TaktQualityIncidentItemImportDto>
{
    /// <summary>
    /// 初始化 导入QualityIncidentItem 校验规则
    /// </summary>
    public TaktQualityIncidentItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.QualityIncidentId)
            .GreaterThanOrEqualTo(0).WithMessage("品质事故主表 ID不能为负数");
        RuleFor(x => x.QualityIncidentCode)
            .NotEmpty().WithMessage("品质事故编码不能为空")
            .MaximumLength(20).WithMessage("品质事故编码长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(70).WithMessage("物料描述长度不能超过70个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
