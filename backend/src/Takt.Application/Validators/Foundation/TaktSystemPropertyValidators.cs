// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktSystemPropertyValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：SystemProperty 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSystemProperty 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建SystemProperty 验证器
// ========================================

/// <summary>
/// 创建SystemProperty DTO 验证器
/// </summary>
public class TaktSystemPropertyCreateValidator : AbstractValidator<TaktSystemPropertyCreateDto>
{
    /// <summary>
    /// 初始化 创建SystemProperty 校验规则
    /// </summary>
    public TaktSystemPropertyCreateValidator()
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
        RuleFor(x => x.SystemPropertyKey)
            .NotEmpty().WithMessage("属性键不能为空")
            .MaximumLength(100).WithMessage("属性键长度不能超过100个字符");
        RuleFor(x => x.SystemPropertyName)
            .NotEmpty().WithMessage("属性名称不能为空")
            .MaximumLength(100).WithMessage("属性名称长度不能超过100个字符");
        RuleFor(x => x.SystemPropertyGroup)
            .NotEmpty().WithMessage("属性类别不能为空")
            .MaximumLength(40).WithMessage("属性类别长度不能超过40个字符");
        RuleFor(x => x.ValueType)
            .NotEmpty().WithMessage("值类型不能为空")
            .MaximumLength(40).WithMessage("值类型长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SystemProperty 验证器
// ========================================

/// <summary>
/// 更新SystemProperty DTO 验证器
/// </summary>
public class TaktSystemPropertyUpdateValidator : AbstractValidator<TaktSystemPropertyUpdateDto>
{
    /// <summary>
    /// 初始化 更新SystemProperty 校验规则
    /// </summary>
    public TaktSystemPropertyUpdateValidator()
    {
        RuleFor(x => x.SystemPropertyId)
            .GreaterThan(0).WithMessage("SystemPropertyID无效");
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
        RuleFor(x => x.SystemPropertyKey)
            .NotEmpty().WithMessage("属性键不能为空")
            .MaximumLength(100).WithMessage("属性键长度不能超过100个字符");
        RuleFor(x => x.SystemPropertyName)
            .NotEmpty().WithMessage("属性名称不能为空")
            .MaximumLength(100).WithMessage("属性名称长度不能超过100个字符");
        RuleFor(x => x.SystemPropertyGroup)
            .NotEmpty().WithMessage("属性类别不能为空")
            .MaximumLength(40).WithMessage("属性类别长度不能超过40个字符");
        RuleFor(x => x.ValueType)
            .NotEmpty().WithMessage("值类型不能为空")
            .MaximumLength(40).WithMessage("值类型长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SystemProperty 验证器
// ========================================

/// <summary>
/// 导入SystemProperty DTO 验证器
/// </summary>
public class TaktSystemPropertyImportValidator : AbstractValidator<TaktSystemPropertyImportDto>
{
    /// <summary>
    /// 初始化 导入SystemProperty 校验规则
    /// </summary>
    public TaktSystemPropertyImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SystemPropertyKey)
            .NotEmpty().WithMessage("属性键不能为空")
            .MaximumLength(100).WithMessage("属性键长度不能超过100个字符");
        RuleFor(x => x.SystemPropertyName)
            .NotEmpty().WithMessage("属性名称不能为空")
            .MaximumLength(100).WithMessage("属性名称长度不能超过100个字符");
        RuleFor(x => x.SystemPropertyGroup)
            .NotEmpty().WithMessage("属性类别不能为空")
            .MaximumLength(40).WithMessage("属性类别长度不能超过40个字符");
        RuleFor(x => x.ValueType)
            .NotEmpty().WithMessage("值类型不能为空")
            .MaximumLength(40).WithMessage("值类型长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
