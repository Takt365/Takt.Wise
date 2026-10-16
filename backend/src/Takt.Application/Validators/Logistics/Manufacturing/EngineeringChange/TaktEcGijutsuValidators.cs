// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcGijutsuValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：EcGijutsu 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEcGijutsu 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 创建EcGijutsu 验证器
// ========================================

/// <summary>
/// 创建EcGijutsu DTO 验证器
/// </summary>
public class TaktEcGijutsuCreateValidator : AbstractValidator<TaktEcGijutsuCreateDto>
{
    /// <summary>
    /// 初始化 创建EcGijutsu 校验规则
    /// </summary>
    public TaktEcGijutsuCreateValidator()
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
        RuleFor(x => x.EcCode)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.EcTitle)
            .NotEmpty().WithMessage("设变标题不能为空")
            .MaximumLength(500).WithMessage("设变标题长度不能超过500个字符");
        RuleFor(x => x.EcContent)
            .NotEmpty().WithMessage("设变内容不能为空");
        RuleFor(x => x.EcLeader)
            .NotEmpty().WithMessage("设变担当不能为空")
            .MaximumLength(80).WithMessage("设变担当长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EcGijutsu 验证器
// ========================================

/// <summary>
/// 更新EcGijutsu DTO 验证器
/// </summary>
public class TaktEcGijutsuUpdateValidator : AbstractValidator<TaktEcGijutsuUpdateDto>
{
    /// <summary>
    /// 初始化 更新EcGijutsu 校验规则
    /// </summary>
    public TaktEcGijutsuUpdateValidator()
    {
        RuleFor(x => x.EcGijutsuId)
            .GreaterThan(0).WithMessage("EcGijutsuID无效");
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
        RuleFor(x => x.EcCode)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.EcTitle)
            .NotEmpty().WithMessage("设变标题不能为空")
            .MaximumLength(500).WithMessage("设变标题长度不能超过500个字符");
        RuleFor(x => x.EcContent)
            .NotEmpty().WithMessage("设变内容不能为空");
        RuleFor(x => x.EcLeader)
            .NotEmpty().WithMessage("设变担当不能为空")
            .MaximumLength(80).WithMessage("设变担当长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入EcGijutsu 验证器
// ========================================

/// <summary>
/// 导入EcGijutsu DTO 验证器
/// </summary>
public class TaktEcGijutsuImportValidator : AbstractValidator<TaktEcGijutsuImportDto>
{
    /// <summary>
    /// 初始化 导入EcGijutsu 校验规则
    /// </summary>
    public TaktEcGijutsuImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.EcCode)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.EcTitle)
            .NotEmpty().WithMessage("设变标题不能为空")
            .MaximumLength(500).WithMessage("设变标题长度不能超过500个字符");
        RuleFor(x => x.EcContent)
            .NotEmpty().WithMessage("设变内容不能为空");
        RuleFor(x => x.EcLeader)
            .NotEmpty().WithMessage("设变担当不能为空")
            .MaximumLength(80).WithMessage("设变担当长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
