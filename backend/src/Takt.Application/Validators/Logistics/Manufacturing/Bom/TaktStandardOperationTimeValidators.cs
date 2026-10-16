// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Bom
// 文件名称：TaktStandardOperationTimeValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：StandardOperationTime 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktStandardOperationTime 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Validators.Logistics.Manufacturing.Bom;

// ========================================
// 创建StandardOperationTime 验证器
// ========================================

/// <summary>
/// 创建StandardOperationTime DTO 验证器
/// </summary>
public class TaktStandardOperationTimeCreateValidator : AbstractValidator<TaktStandardOperationTimeCreateDto>
{
    /// <summary>
    /// 初始化 创建StandardOperationTime 校验规则
    /// </summary>
    public TaktStandardOperationTimeCreateValidator()
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
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.WorkCenter)
            .NotEmpty().WithMessage("工作中心不能为空")
            .MaximumLength(10).WithMessage("工作中心长度不能超过10个字符");
        RuleFor(x => x.TimeUnit)
            .NotEmpty().WithMessage("工时单位不能为空")
            .MaximumLength(3).WithMessage("工时单位长度不能超过3个字符");
        RuleFor(x => x.PointsUnit)
            .NotEmpty().WithMessage("点数单位不能为空")
            .MaximumLength(5).WithMessage("点数单位长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新StandardOperationTime 验证器
// ========================================

/// <summary>
/// 更新StandardOperationTime DTO 验证器
/// </summary>
public class TaktStandardOperationTimeUpdateValidator : AbstractValidator<TaktStandardOperationTimeUpdateDto>
{
    /// <summary>
    /// 初始化 更新StandardOperationTime 校验规则
    /// </summary>
    public TaktStandardOperationTimeUpdateValidator()
    {
        RuleFor(x => x.StandardOperationTimeId)
            .GreaterThan(0).WithMessage("StandardOperationTimeID无效");
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
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.WorkCenter)
            .NotEmpty().WithMessage("工作中心不能为空")
            .MaximumLength(10).WithMessage("工作中心长度不能超过10个字符");
        RuleFor(x => x.TimeUnit)
            .NotEmpty().WithMessage("工时单位不能为空")
            .MaximumLength(3).WithMessage("工时单位长度不能超过3个字符");
        RuleFor(x => x.PointsUnit)
            .NotEmpty().WithMessage("点数单位不能为空")
            .MaximumLength(5).WithMessage("点数单位长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入StandardOperationTime 验证器
// ========================================

/// <summary>
/// 导入StandardOperationTime DTO 验证器
/// </summary>
public class TaktStandardOperationTimeImportValidator : AbstractValidator<TaktStandardOperationTimeImportDto>
{
    /// <summary>
    /// 初始化 导入StandardOperationTime 校验规则
    /// </summary>
    public TaktStandardOperationTimeImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.WorkCenter)
            .NotEmpty().WithMessage("工作中心不能为空")
            .MaximumLength(10).WithMessage("工作中心长度不能超过10个字符");
        RuleFor(x => x.TimeUnit)
            .NotEmpty().WithMessage("工时单位不能为空")
            .MaximumLength(3).WithMessage("工时单位长度不能超过3个字符");
        RuleFor(x => x.PointsUnit)
            .NotEmpty().WithMessage("点数单位不能为空")
            .MaximumLength(5).WithMessage("点数单位长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
