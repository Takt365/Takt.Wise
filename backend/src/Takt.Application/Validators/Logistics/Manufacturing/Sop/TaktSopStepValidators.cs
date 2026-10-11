// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SopStep 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSopStep 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;

namespace Takt.Application.Validators.Logistics.Manufacturing.Sop;

// ========================================
// 创建SopStep 验证器
// ========================================

/// <summary>
/// 创建SopStep DTO 验证器
/// </summary>
public class TaktSopStepCreateValidator : AbstractValidator<TaktSopStepCreateDto>
{
    /// <summary>
    /// 初始化 创建SopStep 校验规则
    /// </summary>
    public TaktSopStepCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ContentId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ContentId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ContentId)
            .GreaterThanOrEqualTo(0).WithMessage("正文 ID不能为负数");
        RuleFor(x => x.StepTitle)
            .NotEmpty().WithMessage("工步标题不能为空")
            .MaximumLength(200).WithMessage("工步标题长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SopStep 验证器
// ========================================

/// <summary>
/// 更新SopStep DTO 验证器
/// </summary>
public class TaktSopStepUpdateValidator : AbstractValidator<TaktSopStepUpdateDto>
{
    /// <summary>
    /// 初始化 更新SopStep 校验规则
    /// </summary>
    public TaktSopStepUpdateValidator()
    {
        RuleFor(x => x.SopStepId)
            .GreaterThan(0).WithMessage("SopStepID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ContentId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ContentId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ContentId)
            .GreaterThanOrEqualTo(0).WithMessage("正文 ID不能为负数");
        RuleFor(x => x.StepTitle)
            .NotEmpty().WithMessage("工步标题不能为空")
            .MaximumLength(200).WithMessage("工步标题长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SopStep 验证器
// ========================================

/// <summary>
/// 导入SopStep DTO 验证器
/// </summary>
public class TaktSopStepImportValidator : AbstractValidator<TaktSopStepImportDto>
{
    /// <summary>
    /// 初始化 导入SopStep 校验规则
    /// </summary>
    public TaktSopStepImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ContentId)
            .GreaterThanOrEqualTo(0).WithMessage("正文 ID不能为负数");
        RuleFor(x => x.StepTitle)
            .NotEmpty().WithMessage("工步标题不能为空")
            .MaximumLength(200).WithMessage("工步标题长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
