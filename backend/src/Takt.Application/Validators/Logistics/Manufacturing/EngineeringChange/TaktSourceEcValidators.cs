// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SourceEc 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSourceEc 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 创建SourceEc 验证器
// ========================================

/// <summary>
/// 创建SourceEc DTO 验证器
/// </summary>
public class TaktSourceEcCreateValidator : AbstractValidator<TaktSourceEcCreateDto>
{
    /// <summary>
    /// 初始化 创建SourceEc 校验规则
    /// </summary>
    public TaktSourceEcCreateValidator()
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
        RuleFor(x => x.SourceEcCode)
            .NotEmpty().WithMessage("设变号码不能为空")
            .MaximumLength(6).WithMessage("设变号码长度不能超过6个字符");
        RuleFor(x => x.SourceModel)
            .NotEmpty().WithMessage("机种不能为空")
            .MaximumLength(40).WithMessage("机种长度不能超过40个字符");
        RuleFor(x => x.SourceTitle)
            .NotEmpty().WithMessage("标题不能为空")
            .MaximumLength(40).WithMessage("标题长度不能超过40个字符");
        RuleFor(x => x.SourceStatus)
            .NotEmpty().WithMessage("状态不能为空")
            .MaximumLength(40).WithMessage("状态长度不能超过40个字符");
        RuleFor(x => x.SourceEcContent)
            .NotEmpty().WithMessage("设变内容不能为空");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SourceEc 验证器
// ========================================

/// <summary>
/// 更新SourceEc DTO 验证器
/// </summary>
public class TaktSourceEcUpdateValidator : AbstractValidator<TaktSourceEcUpdateDto>
{
    /// <summary>
    /// 初始化 更新SourceEc 校验规则
    /// </summary>
    public TaktSourceEcUpdateValidator()
    {
        RuleFor(x => x.SourceEcId)
            .GreaterThan(0).WithMessage("SourceEcID无效");
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
        RuleFor(x => x.SourceEcCode)
            .NotEmpty().WithMessage("设变号码不能为空")
            .MaximumLength(6).WithMessage("设变号码长度不能超过6个字符");
        RuleFor(x => x.SourceModel)
            .NotEmpty().WithMessage("机种不能为空")
            .MaximumLength(40).WithMessage("机种长度不能超过40个字符");
        RuleFor(x => x.SourceTitle)
            .NotEmpty().WithMessage("标题不能为空")
            .MaximumLength(40).WithMessage("标题长度不能超过40个字符");
        RuleFor(x => x.SourceStatus)
            .NotEmpty().WithMessage("状态不能为空")
            .MaximumLength(40).WithMessage("状态长度不能超过40个字符");
        RuleFor(x => x.SourceEcContent)
            .NotEmpty().WithMessage("设变内容不能为空");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SourceEc 验证器
// ========================================

/// <summary>
/// 导入SourceEc DTO 验证器
/// </summary>
public class TaktSourceEcImportValidator : AbstractValidator<TaktSourceEcImportDto>
{
    /// <summary>
    /// 初始化 导入SourceEc 校验规则
    /// </summary>
    public TaktSourceEcImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SourceEcCode)
            .NotEmpty().WithMessage("设变号码不能为空")
            .MaximumLength(6).WithMessage("设变号码长度不能超过6个字符");
        RuleFor(x => x.SourceModel)
            .NotEmpty().WithMessage("机种不能为空")
            .MaximumLength(40).WithMessage("机种长度不能超过40个字符");
        RuleFor(x => x.SourceTitle)
            .NotEmpty().WithMessage("标题不能为空")
            .MaximumLength(40).WithMessage("标题长度不能超过40个字符");
        RuleFor(x => x.SourceStatus)
            .NotEmpty().WithMessage("状态不能为空")
            .MaximumLength(40).WithMessage("状态长度不能超过40个字符");
        RuleFor(x => x.SourceEcContent)
            .NotEmpty().WithMessage("设变内容不能为空");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
