// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Performance
// 文件名称：TaktPerfAnalysisValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：PerfAnalysis 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPerfAnalysis 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Performance;

namespace Takt.Application.Validators.HumanResource.Performance;

// ========================================
// 创建PerfAnalysis 验证器
// ========================================

/// <summary>
/// 创建PerfAnalysis DTO 验证器
/// </summary>
public class TaktPerfAnalysisCreateValidator : AbstractValidator<TaktPerfAnalysisCreateDto>
{
    /// <summary>
    /// 初始化 创建PerfAnalysis 校验规则
    /// </summary>
    public TaktPerfAnalysisCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工不能为负数");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(80).WithMessage("员工姓名长度不能超过80个字符");
        RuleFor(x => x.AssessmentId)
            .GreaterThanOrEqualTo(0).WithMessage("考核评估不能为负数");
        RuleFor(x => x.PlanTitle)
            .NotEmpty().WithMessage("改进计划标题不能为空")
            .MaximumLength(200).WithMessage("改进计划标题长度不能超过200个字符");
        RuleFor(x => x.ImprovementArea)
            .NotEmpty().WithMessage("改进领域不能为空")
            .MaximumLength(50).WithMessage("改进领域长度不能超过50个字符");
        RuleFor(x => x.CurrentSituation)
            .NotEmpty().WithMessage("当前状况描述不能为空")
            .MaximumLength(1000).WithMessage("当前状况描述长度不能超过1000个字符");
        RuleFor(x => x.ImprovementGoal)
            .NotEmpty().WithMessage("改进目标不能为空")
            .MaximumLength(500).WithMessage("改进目标长度不能超过500个字符");
        RuleFor(x => x.ImprovementActions)
            .NotEmpty().WithMessage("改进措施不能为空")
            .MaximumLength(1000).WithMessage("改进措施长度不能超过1000个字符");
        RuleFor(x => x.ResultDescription)
            .NotEmpty().WithMessage("改进结果说明不能为空")
            .MaximumLength(1000).WithMessage("改进结果说明长度不能超过1000个字符");
        RuleFor(x => x.MentorId)
            .GreaterThanOrEqualTo(0).WithMessage("指导老师不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PerfAnalysis 验证器
// ========================================

/// <summary>
/// 更新PerfAnalysis DTO 验证器
/// </summary>
public class TaktPerfAnalysisUpdateValidator : AbstractValidator<TaktPerfAnalysisUpdateDto>
{
    /// <summary>
    /// 初始化 更新PerfAnalysis 校验规则
    /// </summary>
    public TaktPerfAnalysisUpdateValidator()
    {
        RuleFor(x => x.PerfAnalysisId)
            .GreaterThan(0).WithMessage("PerfAnalysisID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工不能为负数");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(80).WithMessage("员工姓名长度不能超过80个字符");
        RuleFor(x => x.AssessmentId)
            .GreaterThanOrEqualTo(0).WithMessage("考核评估不能为负数");
        RuleFor(x => x.PlanTitle)
            .NotEmpty().WithMessage("改进计划标题不能为空")
            .MaximumLength(200).WithMessage("改进计划标题长度不能超过200个字符");
        RuleFor(x => x.ImprovementArea)
            .NotEmpty().WithMessage("改进领域不能为空")
            .MaximumLength(50).WithMessage("改进领域长度不能超过50个字符");
        RuleFor(x => x.CurrentSituation)
            .NotEmpty().WithMessage("当前状况描述不能为空")
            .MaximumLength(1000).WithMessage("当前状况描述长度不能超过1000个字符");
        RuleFor(x => x.ImprovementGoal)
            .NotEmpty().WithMessage("改进目标不能为空")
            .MaximumLength(500).WithMessage("改进目标长度不能超过500个字符");
        RuleFor(x => x.ImprovementActions)
            .NotEmpty().WithMessage("改进措施不能为空")
            .MaximumLength(1000).WithMessage("改进措施长度不能超过1000个字符");
        RuleFor(x => x.ResultDescription)
            .NotEmpty().WithMessage("改进结果说明不能为空")
            .MaximumLength(1000).WithMessage("改进结果说明长度不能超过1000个字符");
        RuleFor(x => x.MentorId)
            .GreaterThanOrEqualTo(0).WithMessage("指导老师不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入PerfAnalysis 验证器
// ========================================

/// <summary>
/// 导入PerfAnalysis DTO 验证器
/// </summary>
public class TaktPerfAnalysisImportValidator : AbstractValidator<TaktPerfAnalysisImportDto>
{
    /// <summary>
    /// 初始化 导入PerfAnalysis 校验规则
    /// </summary>
    public TaktPerfAnalysisImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工不能为负数");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空")
            .MaximumLength(80).WithMessage("员工姓名长度不能超过80个字符");
        RuleFor(x => x.AssessmentId)
            .GreaterThanOrEqualTo(0).WithMessage("考核评估不能为负数");
        RuleFor(x => x.PlanTitle)
            .NotEmpty().WithMessage("改进计划标题不能为空")
            .MaximumLength(200).WithMessage("改进计划标题长度不能超过200个字符");
        RuleFor(x => x.ImprovementArea)
            .NotEmpty().WithMessage("改进领域不能为空")
            .MaximumLength(50).WithMessage("改进领域长度不能超过50个字符");
        RuleFor(x => x.CurrentSituation)
            .NotEmpty().WithMessage("当前状况描述不能为空")
            .MaximumLength(1000).WithMessage("当前状况描述长度不能超过1000个字符");
        RuleFor(x => x.ImprovementGoal)
            .NotEmpty().WithMessage("改进目标不能为空")
            .MaximumLength(500).WithMessage("改进目标长度不能超过500个字符");
        RuleFor(x => x.ImprovementActions)
            .NotEmpty().WithMessage("改进措施不能为空")
            .MaximumLength(1000).WithMessage("改进措施长度不能超过1000个字符");
        RuleFor(x => x.ResultDescription)
            .NotEmpty().WithMessage("改进结果说明不能为空")
            .MaximumLength(1000).WithMessage("改进结果说明长度不能超过1000个字符");
        RuleFor(x => x.MentorId)
            .GreaterThanOrEqualTo(0).WithMessage("指导老师不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
