// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcTeValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：EcTe 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEcTe 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 创建EcTe 验证器
// ========================================

/// <summary>
/// 创建EcTe DTO 验证器
/// </summary>
public class TaktEcTeCreateValidator : AbstractValidator<TaktEcTeCreateDto>
{
    /// <summary>
    /// 初始化 创建EcTe 校验规则
    /// </summary>
    public TaktEcTeCreateValidator()
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
// 更新EcTe 验证器
// ========================================

/// <summary>
/// 更新EcTe DTO 验证器
/// </summary>
public class TaktEcTeUpdateValidator : AbstractValidator<TaktEcTeUpdateDto>
{
    /// <summary>
    /// 初始化 更新EcTe 校验规则
    /// </summary>
    public TaktEcTeUpdateValidator()
    {
        RuleFor(x => x.EcTeId)
            .GreaterThan(0).WithMessage("EcTeID无效");
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
// 导入EcTe 验证器
// ========================================

/// <summary>
/// 导入EcTe DTO 验证器
/// </summary>
public class TaktEcTeImportValidator : AbstractValidator<TaktEcTeImportDto>
{
    /// <summary>
    /// 初始化 导入EcTe 校验规则
    /// </summary>
    public TaktEcTeImportValidator()
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
