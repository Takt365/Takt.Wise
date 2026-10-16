// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Identity
// 文件名称：TaktUserPreferenceValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：UserPreference 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktUserPreference 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Identity;

namespace Takt.Application.Validators.Identity;

// ========================================
// 创建UserPreference 验证器
// ========================================

/// <summary>
/// 创建UserPreference DTO 验证器
/// </summary>
public class TaktUserPreferenceCreateValidator : AbstractValidator<TaktUserPreferenceCreateDto>
{
    /// <summary>
    /// 初始化 创建UserPreference 校验规则
    /// </summary>
    public TaktUserPreferenceCreateValidator()
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
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.PreferenceJson)
            .NotEmpty().WithMessage("偏好 JSON不能为空");
        RuleFor(x => x.UserLayout)
            .NotEmpty().WithMessage("用户布局不能为空")
            .MaximumLength(40).WithMessage("用户布局长度不能超过40个字符");
        RuleFor(x => x.UserTheme)
            .NotEmpty().WithMessage("用户主题不能为空")
            .MaximumLength(40).WithMessage("用户主题长度不能超过40个字符");
        RuleFor(x => x.UserColor)
            .NotEmpty().WithMessage("用户颜色不能为空")
            .MaximumLength(40).WithMessage("用户颜色长度不能超过40个字符");
        RuleFor(x => x.UserLocale)
            .NotEmpty().WithMessage("用户界面语言不能为空")
            .MaximumLength(5).WithMessage("用户界面语言长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新UserPreference 验证器
// ========================================

/// <summary>
/// 更新UserPreference DTO 验证器
/// </summary>
public class TaktUserPreferenceUpdateValidator : AbstractValidator<TaktUserPreferenceUpdateDto>
{
    /// <summary>
    /// 初始化 更新UserPreference 校验规则
    /// </summary>
    public TaktUserPreferenceUpdateValidator()
    {
        RuleFor(x => x.UserPreferenceId)
            .GreaterThan(0).WithMessage("UserPreferenceID无效");
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
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.PreferenceJson)
            .NotEmpty().WithMessage("偏好 JSON不能为空");
        RuleFor(x => x.UserLayout)
            .NotEmpty().WithMessage("用户布局不能为空")
            .MaximumLength(40).WithMessage("用户布局长度不能超过40个字符");
        RuleFor(x => x.UserTheme)
            .NotEmpty().WithMessage("用户主题不能为空")
            .MaximumLength(40).WithMessage("用户主题长度不能超过40个字符");
        RuleFor(x => x.UserColor)
            .NotEmpty().WithMessage("用户颜色不能为空")
            .MaximumLength(40).WithMessage("用户颜色长度不能超过40个字符");
        RuleFor(x => x.UserLocale)
            .NotEmpty().WithMessage("用户界面语言不能为空")
            .MaximumLength(5).WithMessage("用户界面语言长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入UserPreference 验证器
// ========================================

/// <summary>
/// 导入UserPreference DTO 验证器
/// </summary>
public class TaktUserPreferenceImportValidator : AbstractValidator<TaktUserPreferenceImportDto>
{
    /// <summary>
    /// 初始化 导入UserPreference 校验规则
    /// </summary>
    public TaktUserPreferenceImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.PreferenceJson)
            .NotEmpty().WithMessage("偏好 JSON不能为空");
        RuleFor(x => x.UserLayout)
            .NotEmpty().WithMessage("用户布局不能为空")
            .MaximumLength(40).WithMessage("用户布局长度不能超过40个字符");
        RuleFor(x => x.UserTheme)
            .NotEmpty().WithMessage("用户主题不能为空")
            .MaximumLength(40).WithMessage("用户主题长度不能超过40个字符");
        RuleFor(x => x.UserColor)
            .NotEmpty().WithMessage("用户颜色不能为空")
            .MaximumLength(40).WithMessage("用户颜色长度不能超过40个字符");
        RuleFor(x => x.UserLocale)
            .NotEmpty().WithMessage("用户界面语言不能为空")
            .MaximumLength(5).WithMessage("用户界面语言长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
