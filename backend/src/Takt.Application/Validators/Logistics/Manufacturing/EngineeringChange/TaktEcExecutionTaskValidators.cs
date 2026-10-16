// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecutionTaskValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：EcExecutionTask 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEcExecutionTask 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 创建EcExecutionTask 验证器
// ========================================

/// <summary>
/// 创建EcExecutionTask DTO 验证器
/// </summary>
public class TaktEcExecutionTaskCreateValidator : AbstractValidator<TaktEcExecutionTaskCreateDto>
{
    /// <summary>
    /// 初始化 创建EcExecutionTask 校验规则
    /// </summary>
    public TaktEcExecutionTaskCreateValidator()
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
        RuleFor(x => x.EcNotificationId)
            .GreaterThanOrEqualTo(0).WithMessage("通知单 ID不能为负数");
        RuleFor(x => x.EcGijutsuId)
            .GreaterThanOrEqualTo(0).WithMessage("技术课主表 ID不能为负数");
        RuleFor(x => x.EcCode)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(40).WithMessage("设变单号长度不能超过40个字符");
        RuleFor(x => x.EcExecId)
            .GreaterThanOrEqualTo(0).WithMessage("关联设变部门行 ID不能为负数");
        RuleFor(x => x.EcDetailId)
            .GreaterThanOrEqualTo(0).WithMessage("设变明细 ID不能为负数");
        RuleFor(x => x.DeptCode)
            .NotEmpty().WithMessage("责任部门编码不能为空")
            .MaximumLength(6).WithMessage("责任部门编码长度不能超过6个字符");
        RuleFor(x => x.TaskTitle)
            .NotEmpty().WithMessage("任务标题不能为空")
            .MaximumLength(200).WithMessage("任务标题长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EcExecutionTask 验证器
// ========================================

/// <summary>
/// 更新EcExecutionTask DTO 验证器
/// </summary>
public class TaktEcExecutionTaskUpdateValidator : AbstractValidator<TaktEcExecutionTaskUpdateDto>
{
    /// <summary>
    /// 初始化 更新EcExecutionTask 校验规则
    /// </summary>
    public TaktEcExecutionTaskUpdateValidator()
    {
        RuleFor(x => x.EcExecutionTaskId)
            .GreaterThan(0).WithMessage("EcExecutionTaskID无效");
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
        RuleFor(x => x.EcNotificationId)
            .GreaterThanOrEqualTo(0).WithMessage("通知单 ID不能为负数");
        RuleFor(x => x.EcGijutsuId)
            .GreaterThanOrEqualTo(0).WithMessage("技术课主表 ID不能为负数");
        RuleFor(x => x.EcCode)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(40).WithMessage("设变单号长度不能超过40个字符");
        RuleFor(x => x.EcExecId)
            .GreaterThanOrEqualTo(0).WithMessage("关联设变部门行 ID不能为负数");
        RuleFor(x => x.EcDetailId)
            .GreaterThanOrEqualTo(0).WithMessage("设变明细 ID不能为负数");
        RuleFor(x => x.DeptCode)
            .NotEmpty().WithMessage("责任部门编码不能为空")
            .MaximumLength(6).WithMessage("责任部门编码长度不能超过6个字符");
        RuleFor(x => x.TaskTitle)
            .NotEmpty().WithMessage("任务标题不能为空")
            .MaximumLength(200).WithMessage("任务标题长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入EcExecutionTask 验证器
// ========================================

/// <summary>
/// 导入EcExecutionTask DTO 验证器
/// </summary>
public class TaktEcExecutionTaskImportValidator : AbstractValidator<TaktEcExecutionTaskImportDto>
{
    /// <summary>
    /// 初始化 导入EcExecutionTask 校验规则
    /// </summary>
    public TaktEcExecutionTaskImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.EcNotificationId)
            .GreaterThanOrEqualTo(0).WithMessage("通知单 ID不能为负数");
        RuleFor(x => x.EcGijutsuId)
            .GreaterThanOrEqualTo(0).WithMessage("技术课主表 ID不能为负数");
        RuleFor(x => x.EcCode)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(40).WithMessage("设变单号长度不能超过40个字符");
        RuleFor(x => x.EcExecId)
            .GreaterThanOrEqualTo(0).WithMessage("关联设变部门行 ID不能为负数");
        RuleFor(x => x.EcDetailId)
            .GreaterThanOrEqualTo(0).WithMessage("设变明细 ID不能为负数");
        RuleFor(x => x.DeptCode)
            .NotEmpty().WithMessage("责任部门编码不能为空")
            .MaximumLength(6).WithMessage("责任部门编码长度不能超过6个字符");
        RuleFor(x => x.TaskTitle)
            .NotEmpty().WithMessage("任务标题不能为空")
            .MaximumLength(200).WithMessage("任务标题长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
