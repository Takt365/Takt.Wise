// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Routine.VisitorCenter
// 文件名称：TaktVisitorCompanionValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：VisitorCompanion 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktVisitorCompanion 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.VisitorCenter;

namespace Takt.Application.Validators.Routine.VisitorCenter;

// ========================================
// 创建VisitorCompanion 验证器
// ========================================

/// <summary>
/// 创建VisitorCompanion DTO 验证器
/// </summary>
public class TaktVisitorCompanionCreateValidator : AbstractValidator<TaktVisitorCompanionCreateDto>
{
    /// <summary>
    /// 初始化 创建VisitorCompanion 校验规则
    /// </summary>
    public TaktVisitorCompanionCreateValidator()
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
        RuleFor(x => x.VisitorId)
            .GreaterThanOrEqualTo(0).WithMessage("来访记录 ID不能为负数");
        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("部门不能为空")
            .MaximumLength(100).WithMessage("部门长度不能超过100个字符");
        RuleFor(x => x.JobTitle)
            .NotEmpty().WithMessage("职称不能为空")
            .MaximumLength(100).WithMessage("职称长度不能超过100个字符");
        RuleFor(x => x.CompanionName)
            .NotEmpty().WithMessage("来访人员姓名不能为空")
            .MaximumLength(50).WithMessage("来访人员姓名长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新VisitorCompanion 验证器
// ========================================

/// <summary>
/// 更新VisitorCompanion DTO 验证器
/// </summary>
public class TaktVisitorCompanionUpdateValidator : AbstractValidator<TaktVisitorCompanionUpdateDto>
{
    /// <summary>
    /// 初始化 更新VisitorCompanion 校验规则
    /// </summary>
    public TaktVisitorCompanionUpdateValidator()
    {
        RuleFor(x => x.VisitorCompanionId)
            .GreaterThan(0).WithMessage("VisitorCompanionID无效");
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
        RuleFor(x => x.VisitorId)
            .GreaterThanOrEqualTo(0).WithMessage("来访记录 ID不能为负数");
        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("部门不能为空")
            .MaximumLength(100).WithMessage("部门长度不能超过100个字符");
        RuleFor(x => x.JobTitle)
            .NotEmpty().WithMessage("职称不能为空")
            .MaximumLength(100).WithMessage("职称长度不能超过100个字符");
        RuleFor(x => x.CompanionName)
            .NotEmpty().WithMessage("来访人员姓名不能为空")
            .MaximumLength(50).WithMessage("来访人员姓名长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入VisitorCompanion 验证器
// ========================================

/// <summary>
/// 导入VisitorCompanion DTO 验证器
/// </summary>
public class TaktVisitorCompanionImportValidator : AbstractValidator<TaktVisitorCompanionImportDto>
{
    /// <summary>
    /// 初始化 导入VisitorCompanion 校验规则
    /// </summary>
    public TaktVisitorCompanionImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.VisitorId)
            .GreaterThanOrEqualTo(0).WithMessage("来访记录 ID不能为负数");
        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("部门不能为空")
            .MaximumLength(100).WithMessage("部门长度不能超过100个字符");
        RuleFor(x => x.JobTitle)
            .NotEmpty().WithMessage("职称不能为空")
            .MaximumLength(100).WithMessage("职称长度不能超过100个字符");
        RuleFor(x => x.CompanionName)
            .NotEmpty().WithMessage("来访人员姓名不能为空")
            .MaximumLength(50).WithMessage("来访人员姓名长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
