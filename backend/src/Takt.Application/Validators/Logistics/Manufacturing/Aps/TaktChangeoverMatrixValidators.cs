// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Aps
// 文件名称：TaktChangeoverMatrixValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：ChangeoverMatrix 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktChangeoverMatrix 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;

namespace Takt.Application.Validators.Logistics.Manufacturing.Aps;

// ========================================
// 创建ChangeoverMatrix 验证器
// ========================================

/// <summary>
/// 创建ChangeoverMatrix DTO 验证器
/// </summary>
public class TaktChangeoverMatrixCreateValidator : AbstractValidator<TaktChangeoverMatrixCreateDto>
{
    /// <summary>
    /// 初始化 创建ChangeoverMatrix 校验规则
    /// </summary>
    public TaktChangeoverMatrixCreateValidator()
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
        RuleFor(x => x.WorkCenterCode)
            .NotEmpty().WithMessage("工作中心编码不能为空")
            .MaximumLength(10).WithMessage("工作中心编码长度不能超过10个字符");
        RuleFor(x => x.FromMaterialCode)
            .NotEmpty().WithMessage("换型前物料编码不能为空")
            .MaximumLength(20).WithMessage("换型前物料编码长度不能超过20个字符");
        RuleFor(x => x.ToMaterialCode)
            .NotEmpty().WithMessage("换型后物料编码不能为空")
            .MaximumLength(20).WithMessage("换型后物料编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ChangeoverMatrix 验证器
// ========================================

/// <summary>
/// 更新ChangeoverMatrix DTO 验证器
/// </summary>
public class TaktChangeoverMatrixUpdateValidator : AbstractValidator<TaktChangeoverMatrixUpdateDto>
{
    /// <summary>
    /// 初始化 更新ChangeoverMatrix 校验规则
    /// </summary>
    public TaktChangeoverMatrixUpdateValidator()
    {
        RuleFor(x => x.ChangeoverMatrixId)
            .GreaterThan(0).WithMessage("ChangeoverMatrixID无效");
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
        RuleFor(x => x.WorkCenterCode)
            .NotEmpty().WithMessage("工作中心编码不能为空")
            .MaximumLength(10).WithMessage("工作中心编码长度不能超过10个字符");
        RuleFor(x => x.FromMaterialCode)
            .NotEmpty().WithMessage("换型前物料编码不能为空")
            .MaximumLength(20).WithMessage("换型前物料编码长度不能超过20个字符");
        RuleFor(x => x.ToMaterialCode)
            .NotEmpty().WithMessage("换型后物料编码不能为空")
            .MaximumLength(20).WithMessage("换型后物料编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入ChangeoverMatrix 验证器
// ========================================

/// <summary>
/// 导入ChangeoverMatrix DTO 验证器
/// </summary>
public class TaktChangeoverMatrixImportValidator : AbstractValidator<TaktChangeoverMatrixImportDto>
{
    /// <summary>
    /// 初始化 导入ChangeoverMatrix 校验规则
    /// </summary>
    public TaktChangeoverMatrixImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.WorkCenterCode)
            .NotEmpty().WithMessage("工作中心编码不能为空")
            .MaximumLength(10).WithMessage("工作中心编码长度不能超过10个字符");
        RuleFor(x => x.FromMaterialCode)
            .NotEmpty().WithMessage("换型前物料编码不能为空")
            .MaximumLength(20).WithMessage("换型前物料编码长度不能超过20个字符");
        RuleFor(x => x.ToMaterialCode)
            .NotEmpty().WithMessage("换型后物料编码不能为空")
            .MaximumLength(20).WithMessage("换型后物料编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
