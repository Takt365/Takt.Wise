// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationItemValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：SupplierEvaluationItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSupplierEvaluationItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Complaint;

namespace Takt.Application.Validators.Logistics.Quality.Complaint;

// ========================================
// 创建SupplierEvaluationItem 验证器
// ========================================

/// <summary>
/// 创建SupplierEvaluationItem DTO 验证器
/// </summary>
public class TaktSupplierEvaluationItemCreateValidator : AbstractValidator<TaktSupplierEvaluationItemCreateDto>
{
    /// <summary>
    /// 初始化 创建SupplierEvaluationItem 校验规则
    /// </summary>
    public TaktSupplierEvaluationItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.EvaluationId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.EvaluationId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EvaluationId)
            .GreaterThanOrEqualTo(0).WithMessage("评价表 ID不能为负数");
        RuleFor(x => x.SupplierEvaluationCode)
            .NotEmpty().WithMessage("评价表编码不能为空")
            .MaximumLength(20).WithMessage("评价表编码长度不能超过20个字符");
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("评价项目名称不能为空")
            .MaximumLength(200).WithMessage("评价项目名称长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SupplierEvaluationItem 验证器
// ========================================

/// <summary>
/// 更新SupplierEvaluationItem DTO 验证器
/// </summary>
public class TaktSupplierEvaluationItemUpdateValidator : AbstractValidator<TaktSupplierEvaluationItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新SupplierEvaluationItem 校验规则
    /// </summary>
    public TaktSupplierEvaluationItemUpdateValidator()
    {
        RuleFor(x => x.SupplierEvaluationItemId)
            .GreaterThan(0).WithMessage("SupplierEvaluationItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.EvaluationId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.EvaluationId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EvaluationId)
            .GreaterThanOrEqualTo(0).WithMessage("评价表 ID不能为负数");
        RuleFor(x => x.SupplierEvaluationCode)
            .NotEmpty().WithMessage("评价表编码不能为空")
            .MaximumLength(20).WithMessage("评价表编码长度不能超过20个字符");
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("评价项目名称不能为空")
            .MaximumLength(200).WithMessage("评价项目名称长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SupplierEvaluationItem 验证器
// ========================================

/// <summary>
/// 导入SupplierEvaluationItem DTO 验证器
/// </summary>
public class TaktSupplierEvaluationItemImportValidator : AbstractValidator<TaktSupplierEvaluationItemImportDto>
{
    /// <summary>
    /// 初始化 导入SupplierEvaluationItem 校验规则
    /// </summary>
    public TaktSupplierEvaluationItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.EvaluationId)
            .GreaterThanOrEqualTo(0).WithMessage("评价表 ID不能为负数");
        RuleFor(x => x.SupplierEvaluationCode)
            .NotEmpty().WithMessage("评价表编码不能为空")
            .MaximumLength(20).WithMessage("评价表编码长度不能超过20个字符");
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("评价项目名称不能为空")
            .MaximumLength(200).WithMessage("评价项目名称长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
