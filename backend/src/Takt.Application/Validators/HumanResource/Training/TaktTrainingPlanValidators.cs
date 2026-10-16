// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Training
// 文件名称：TaktTrainingPlanValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TrainingPlan 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTrainingPlan 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Training;

namespace Takt.Application.Validators.HumanResource.Training;

// ========================================
// 创建TrainingPlan 验证器
// ========================================

/// <summary>
/// 创建TrainingPlan DTO 验证器
/// </summary>
public class TaktTrainingPlanCreateValidator : AbstractValidator<TaktTrainingPlanCreateDto>
{
    /// <summary>
    /// 初始化 创建TrainingPlan 校验规则
    /// </summary>
    public TaktTrainingPlanCreateValidator()
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
        RuleFor(x => x.PlanCode)
            .NotEmpty().WithMessage("计划编码不能为空")
            .MaximumLength(40).WithMessage("计划编码长度不能超过40个字符");
        RuleFor(x => x.PlanName)
            .NotEmpty().WithMessage("计划名称不能为空")
            .MaximumLength(40).WithMessage("计划名称长度不能超过40个字符");
        RuleFor(x => x.PlanType)
            .NotEmpty().WithMessage("计划类型不能为空")
            .MaximumLength(50).WithMessage("计划类型长度不能超过50个字符");
        RuleFor(x => x.ApplicableDepartment)
            .NotEmpty().WithMessage("适用部门不能为空")
            .MaximumLength(100).WithMessage("适用部门长度不能超过100个字符");
        RuleFor(x => x.TrainingObjectives)
            .NotEmpty().WithMessage("培训目标不能为空")
            .MaximumLength(1000).WithMessage("培训目标长度不能超过1000个字符");
        RuleFor(x => x.TrainingPlanDescription)
            .NotEmpty().WithMessage("计划说明不能为空")
            .MaximumLength(1000).WithMessage("计划说明长度不能超过1000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TrainingPlan 验证器
// ========================================

/// <summary>
/// 更新TrainingPlan DTO 验证器
/// </summary>
public class TaktTrainingPlanUpdateValidator : AbstractValidator<TaktTrainingPlanUpdateDto>
{
    /// <summary>
    /// 初始化 更新TrainingPlan 校验规则
    /// </summary>
    public TaktTrainingPlanUpdateValidator()
    {
        RuleFor(x => x.TrainingPlanId)
            .GreaterThan(0).WithMessage("TrainingPlanID无效");
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
        RuleFor(x => x.PlanCode)
            .NotEmpty().WithMessage("计划编码不能为空")
            .MaximumLength(40).WithMessage("计划编码长度不能超过40个字符");
        RuleFor(x => x.PlanName)
            .NotEmpty().WithMessage("计划名称不能为空")
            .MaximumLength(40).WithMessage("计划名称长度不能超过40个字符");
        RuleFor(x => x.PlanType)
            .NotEmpty().WithMessage("计划类型不能为空")
            .MaximumLength(50).WithMessage("计划类型长度不能超过50个字符");
        RuleFor(x => x.ApplicableDepartment)
            .NotEmpty().WithMessage("适用部门不能为空")
            .MaximumLength(100).WithMessage("适用部门长度不能超过100个字符");
        RuleFor(x => x.TrainingObjectives)
            .NotEmpty().WithMessage("培训目标不能为空")
            .MaximumLength(1000).WithMessage("培训目标长度不能超过1000个字符");
        RuleFor(x => x.TrainingPlanDescription)
            .NotEmpty().WithMessage("计划说明不能为空")
            .MaximumLength(1000).WithMessage("计划说明长度不能超过1000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入TrainingPlan 验证器
// ========================================

/// <summary>
/// 导入TrainingPlan DTO 验证器
/// </summary>
public class TaktTrainingPlanImportValidator : AbstractValidator<TaktTrainingPlanImportDto>
{
    /// <summary>
    /// 初始化 导入TrainingPlan 校验规则
    /// </summary>
    public TaktTrainingPlanImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.PlanCode)
            .NotEmpty().WithMessage("计划编码不能为空")
            .MaximumLength(40).WithMessage("计划编码长度不能超过40个字符");
        RuleFor(x => x.PlanName)
            .NotEmpty().WithMessage("计划名称不能为空")
            .MaximumLength(40).WithMessage("计划名称长度不能超过40个字符");
        RuleFor(x => x.PlanType)
            .NotEmpty().WithMessage("计划类型不能为空")
            .MaximumLength(50).WithMessage("计划类型长度不能超过50个字符");
        RuleFor(x => x.ApplicableDepartment)
            .NotEmpty().WithMessage("适用部门不能为空")
            .MaximumLength(100).WithMessage("适用部门长度不能超过100个字符");
        RuleFor(x => x.TrainingObjectives)
            .NotEmpty().WithMessage("培训目标不能为空")
            .MaximumLength(1000).WithMessage("培训目标长度不能超过1000个字符");
        RuleFor(x => x.TrainingPlanDescription)
            .NotEmpty().WithMessage("计划说明不能为空")
            .MaximumLength(1000).WithMessage("计划说明长度不能超过1000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
