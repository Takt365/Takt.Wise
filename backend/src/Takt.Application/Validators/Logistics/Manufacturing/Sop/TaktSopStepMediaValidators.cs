// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepMediaValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：SopStepMedia 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSopStepMedia 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;

namespace Takt.Application.Validators.Logistics.Manufacturing.Sop;

// ========================================
// 创建SopStepMedia 验证器
// ========================================

/// <summary>
/// 创建SopStepMedia DTO 验证器
/// </summary>
public class TaktSopStepMediaCreateValidator : AbstractValidator<TaktSopStepMediaCreateDto>
{
    /// <summary>
    /// 初始化 创建SopStepMedia 校验规则
    /// </summary>
    public TaktSopStepMediaCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.StepId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.StepId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.StepId)
            .GreaterThanOrEqualTo(0).WithMessage("工步 ID不能为负数");
        RuleFor(x => x.FileUrl)
            .NotEmpty().WithMessage("文件 URL不能为空")
            .MaximumLength(500).WithMessage("文件 URL长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SopStepMedia 验证器
// ========================================

/// <summary>
/// 更新SopStepMedia DTO 验证器
/// </summary>
public class TaktSopStepMediaUpdateValidator : AbstractValidator<TaktSopStepMediaUpdateDto>
{
    /// <summary>
    /// 初始化 更新SopStepMedia 校验规则
    /// </summary>
    public TaktSopStepMediaUpdateValidator()
    {
        RuleFor(x => x.SopStepMediaId)
            .GreaterThan(0).WithMessage("SopStepMediaID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.StepId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.StepId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.StepId)
            .GreaterThanOrEqualTo(0).WithMessage("工步 ID不能为负数");
        RuleFor(x => x.FileUrl)
            .NotEmpty().WithMessage("文件 URL不能为空")
            .MaximumLength(500).WithMessage("文件 URL长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SopStepMedia 验证器
// ========================================

/// <summary>
/// 导入SopStepMedia DTO 验证器
/// </summary>
public class TaktSopStepMediaImportValidator : AbstractValidator<TaktSopStepMediaImportDto>
{
    /// <summary>
    /// 初始化 导入SopStepMedia 校验规则
    /// </summary>
    public TaktSopStepMediaImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.StepId)
            .GreaterThanOrEqualTo(0).WithMessage("工步 ID不能为负数");
        RuleFor(x => x.FileUrl)
            .NotEmpty().WithMessage("文件 URL不能为空")
            .MaximumLength(500).WithMessage("文件 URL长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
