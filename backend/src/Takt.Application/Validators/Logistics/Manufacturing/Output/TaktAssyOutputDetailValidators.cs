// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDetailValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：AssyOutputDetail 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktAssyOutputDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;

namespace Takt.Application.Validators.Logistics.Manufacturing.Output;

// ========================================
// 创建AssyOutputDetail 验证器
// ========================================

/// <summary>
/// 创建AssyOutputDetail DTO 验证器
/// </summary>
public class TaktAssyOutputDetailCreateValidator : AbstractValidator<TaktAssyOutputDetailCreateDto>
{
    /// <summary>
    /// 初始化 创建AssyOutputDetail 校验规则
    /// </summary>
    public TaktAssyOutputDetailCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.AssyOutputId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.AssyOutputId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.AssyOutputId)
            .GreaterThanOrEqualTo(0).WithMessage("组立日报ID不能为负数");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("工单号不能为空")
            .MaximumLength(12).WithMessage("工单号长度不能超过12个字符");
        RuleFor(x => x.TimePeriod)
            .NotEmpty().WithMessage("生产时段不能为空")
            .MaximumLength(20).WithMessage("生产时段长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新AssyOutputDetail 验证器
// ========================================

/// <summary>
/// 更新AssyOutputDetail DTO 验证器
/// </summary>
public class TaktAssyOutputDetailUpdateValidator : AbstractValidator<TaktAssyOutputDetailUpdateDto>
{
    /// <summary>
    /// 初始化 更新AssyOutputDetail 校验规则
    /// </summary>
    public TaktAssyOutputDetailUpdateValidator()
    {
        RuleFor(x => x.AssyOutputDetailId)
            .GreaterThan(0).WithMessage("AssyOutputDetailID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.AssyOutputId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.AssyOutputId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.AssyOutputId)
            .GreaterThanOrEqualTo(0).WithMessage("组立日报ID不能为负数");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("工单号不能为空")
            .MaximumLength(12).WithMessage("工单号长度不能超过12个字符");
        RuleFor(x => x.TimePeriod)
            .NotEmpty().WithMessage("生产时段不能为空")
            .MaximumLength(20).WithMessage("生产时段长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入AssyOutputDetail 验证器
// ========================================

/// <summary>
/// 导入AssyOutputDetail DTO 验证器
/// </summary>
public class TaktAssyOutputDetailImportValidator : AbstractValidator<TaktAssyOutputDetailImportDto>
{
    /// <summary>
    /// 初始化 导入AssyOutputDetail 校验规则
    /// </summary>
    public TaktAssyOutputDetailImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.AssyOutputId)
            .GreaterThanOrEqualTo(0).WithMessage("组立日报ID不能为负数");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("工单号不能为空")
            .MaximumLength(12).WithMessage("工单号长度不能超过12个字符");
        RuleFor(x => x.TimePeriod)
            .NotEmpty().WithMessage("生产时段不能为空")
            .MaximumLength(20).WithMessage("生产时段长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
