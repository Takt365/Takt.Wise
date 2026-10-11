// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktQuartzTaskValidators.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：QuartzTask 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktQuartzTask 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建QuartzTask 验证器
// ========================================

/// <summary>
/// 创建QuartzTask DTO 验证器
/// </summary>
public class TaktQuartzTaskCreateValidator : AbstractValidator<TaktQuartzTaskCreateDto>
{
    /// <summary>
    /// 初始化 创建QuartzTask 校验规则
    /// </summary>
    public TaktQuartzTaskCreateValidator()
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
        RuleFor(x => x.TaskCode)
            .NotEmpty().WithMessage("任务编码不能为空")
            .MaximumLength(50).WithMessage("任务编码长度不能超过50个字符");
        RuleFor(x => x.TaskName)
            .NotEmpty().WithMessage("任务名称不能为空")
            .MaximumLength(100).WithMessage("任务名称长度不能超过100个字符");
        RuleFor(x => x.JobName)
            .NotEmpty().WithMessage("Quartz Job 名称不能为空")
            .MaximumLength(100).WithMessage("Quartz Job 名称长度不能超过100个字符");
        RuleFor(x => x.JobGroup)
            .NotEmpty().WithMessage("Quartz Job 分组不能为空")
            .MaximumLength(40).WithMessage("Quartz Job 分组长度不能超过40个字符");
        RuleFor(x => x.TaskType)
            .NotEmpty().WithMessage("任务类型不能为空")
            .MaximumLength(40).WithMessage("任务类型长度不能超过40个字符");
        RuleFor(x => x.AssemblyName)
            .NotEmpty().WithMessage("程序集名称不能为空")
            .MaximumLength(255).WithMessage("程序集名称长度不能超过255个字符");
        RuleFor(x => x.ClassName)
            .NotEmpty().WithMessage("任务类名不能为空")
            .MaximumLength(255).WithMessage("任务类名长度不能超过255个字符");
        RuleFor(x => x.CronExpression)
            .NotEmpty().WithMessage("Cron 表达式不能为空")
            .MaximumLength(100).WithMessage("Cron 表达式长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新QuartzTask 验证器
// ========================================

/// <summary>
/// 更新QuartzTask DTO 验证器
/// </summary>
public class TaktQuartzTaskUpdateValidator : AbstractValidator<TaktQuartzTaskUpdateDto>
{
    /// <summary>
    /// 初始化 更新QuartzTask 校验规则
    /// </summary>
    public TaktQuartzTaskUpdateValidator()
    {
        RuleFor(x => x.QuartzTaskId)
            .GreaterThan(0).WithMessage("QuartzTaskID无效");
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
        RuleFor(x => x.TaskCode)
            .NotEmpty().WithMessage("任务编码不能为空")
            .MaximumLength(50).WithMessage("任务编码长度不能超过50个字符");
        RuleFor(x => x.TaskName)
            .NotEmpty().WithMessage("任务名称不能为空")
            .MaximumLength(100).WithMessage("任务名称长度不能超过100个字符");
        RuleFor(x => x.JobName)
            .NotEmpty().WithMessage("Quartz Job 名称不能为空")
            .MaximumLength(100).WithMessage("Quartz Job 名称长度不能超过100个字符");
        RuleFor(x => x.JobGroup)
            .NotEmpty().WithMessage("Quartz Job 分组不能为空")
            .MaximumLength(40).WithMessage("Quartz Job 分组长度不能超过40个字符");
        RuleFor(x => x.TaskType)
            .NotEmpty().WithMessage("任务类型不能为空")
            .MaximumLength(40).WithMessage("任务类型长度不能超过40个字符");
        RuleFor(x => x.AssemblyName)
            .NotEmpty().WithMessage("程序集名称不能为空")
            .MaximumLength(255).WithMessage("程序集名称长度不能超过255个字符");
        RuleFor(x => x.ClassName)
            .NotEmpty().WithMessage("任务类名不能为空")
            .MaximumLength(255).WithMessage("任务类名长度不能超过255个字符");
        RuleFor(x => x.CronExpression)
            .NotEmpty().WithMessage("Cron 表达式不能为空")
            .MaximumLength(100).WithMessage("Cron 表达式长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入QuartzTask 验证器
// ========================================

/// <summary>
/// 导入QuartzTask DTO 验证器
/// </summary>
public class TaktQuartzTaskImportValidator : AbstractValidator<TaktQuartzTaskImportDto>
{
    /// <summary>
    /// 初始化 导入QuartzTask 校验规则
    /// </summary>
    public TaktQuartzTaskImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.TaskCode)
            .NotEmpty().WithMessage("任务编码不能为空")
            .MaximumLength(50).WithMessage("任务编码长度不能超过50个字符");
        RuleFor(x => x.TaskName)
            .NotEmpty().WithMessage("任务名称不能为空")
            .MaximumLength(100).WithMessage("任务名称长度不能超过100个字符");
        RuleFor(x => x.JobName)
            .NotEmpty().WithMessage("Quartz Job 名称不能为空")
            .MaximumLength(100).WithMessage("Quartz Job 名称长度不能超过100个字符");
        RuleFor(x => x.JobGroup)
            .NotEmpty().WithMessage("Quartz Job 分组不能为空")
            .MaximumLength(40).WithMessage("Quartz Job 分组长度不能超过40个字符");
        RuleFor(x => x.TaskType)
            .NotEmpty().WithMessage("任务类型不能为空")
            .MaximumLength(40).WithMessage("任务类型长度不能超过40个字符");
        RuleFor(x => x.AssemblyName)
            .NotEmpty().WithMessage("程序集名称不能为空")
            .MaximumLength(255).WithMessage("程序集名称长度不能超过255个字符");
        RuleFor(x => x.ClassName)
            .NotEmpty().WithMessage("任务类名不能为空")
            .MaximumLength(255).WithMessage("任务类名长度不能超过255个字符");
        RuleFor(x => x.CronExpression)
            .NotEmpty().WithMessage("Cron 表达式不能为空")
            .MaximumLength(100).WithMessage("Cron 表达式长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
