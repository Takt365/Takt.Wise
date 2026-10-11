// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyBatchDefectValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：AssyBatchDefect 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktAssyBatchDefect 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;

namespace Takt.Application.Validators.Logistics.Manufacturing.Defect;

// ========================================
// 创建AssyBatchDefect 验证器
// ========================================

/// <summary>
/// 创建AssyBatchDefect DTO 验证器
/// </summary>
public class TaktAssyBatchDefectCreateValidator : AbstractValidator<TaktAssyBatchDefectCreateDto>
{
    /// <summary>
    /// 初始化 创建AssyBatchDefect 校验规则
    /// </summary>
    public TaktAssyBatchDefectCreateValidator()
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
        RuleFor(x => x.ProdCategory)
            .NotEmpty().WithMessage("生产类别不能为空")
            .MaximumLength(4).WithMessage("生产类别长度不能超过4个字符");
        RuleFor(x => x.BatchCode)
            .NotEmpty().WithMessage("批次不能为空")
            .MaximumLength(20).WithMessage("批次长度不能超过20个字符");
        RuleFor(x => x.ModelCode)
            .NotEmpty().WithMessage("机种不能为空")
            .MaximumLength(40).WithMessage("机种长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新AssyBatchDefect 验证器
// ========================================

/// <summary>
/// 更新AssyBatchDefect DTO 验证器
/// </summary>
public class TaktAssyBatchDefectUpdateValidator : AbstractValidator<TaktAssyBatchDefectUpdateDto>
{
    /// <summary>
    /// 初始化 更新AssyBatchDefect 校验规则
    /// </summary>
    public TaktAssyBatchDefectUpdateValidator()
    {
        RuleFor(x => x.AssyBatchDefectId)
            .GreaterThan(0).WithMessage("AssyBatchDefectID无效");
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
        RuleFor(x => x.ProdCategory)
            .NotEmpty().WithMessage("生产类别不能为空")
            .MaximumLength(4).WithMessage("生产类别长度不能超过4个字符");
        RuleFor(x => x.BatchCode)
            .NotEmpty().WithMessage("批次不能为空")
            .MaximumLength(20).WithMessage("批次长度不能超过20个字符");
        RuleFor(x => x.ModelCode)
            .NotEmpty().WithMessage("机种不能为空")
            .MaximumLength(40).WithMessage("机种长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入AssyBatchDefect 验证器
// ========================================

/// <summary>
/// 导入AssyBatchDefect DTO 验证器
/// </summary>
public class TaktAssyBatchDefectImportValidator : AbstractValidator<TaktAssyBatchDefectImportDto>
{
    /// <summary>
    /// 初始化 导入AssyBatchDefect 校验规则
    /// </summary>
    public TaktAssyBatchDefectImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ProdCategory)
            .NotEmpty().WithMessage("生产类别不能为空")
            .MaximumLength(4).WithMessage("生产类别长度不能超过4个字符");
        RuleFor(x => x.BatchCode)
            .NotEmpty().WithMessage("批次不能为空")
            .MaximumLength(20).WithMessage("批次长度不能超过20个字符");
        RuleFor(x => x.ModelCode)
            .NotEmpty().WithMessage("机种不能为空")
            .MaximumLength(40).WithMessage("机种长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
