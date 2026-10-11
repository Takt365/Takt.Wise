// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Performance
// 文件名称：TaktPerfAssessmentValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：PerfAssessment 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPerfAssessment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Performance;

namespace Takt.Application.Validators.HumanResource.Performance;

// ========================================
// 创建PerfAssessment 验证器
// ========================================

/// <summary>
/// 创建PerfAssessment DTO 验证器
/// </summary>
public class TaktPerfAssessmentCreateValidator : AbstractValidator<TaktPerfAssessmentCreateDto>
{
    /// <summary>
    /// 初始化 创建PerfAssessment 校验规则
    /// </summary>
    public TaktPerfAssessmentCreateValidator()
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
        RuleFor(x => x.AssessmentPeriod)
            .NotEmpty().WithMessage("考核周期不能为空")
            .MaximumLength(50).WithMessage("考核周期长度不能超过50个字符");
        RuleFor(x => x.SchemeMetricId)
            .GreaterThanOrEqualTo(0).WithMessage("方案指标不能为负数");
        RuleFor(x => x.SelfEvaluationNotes)
            .NotEmpty().WithMessage("自评说明不能为空")
            .MaximumLength(1000).WithMessage("自评说明长度不能超过1000个字符");
        RuleFor(x => x.SupervisorComments)
            .NotEmpty().WithMessage("主管评语不能为空")
            .MaximumLength(1000).WithMessage("主管评语长度不能超过1000个字符");
        RuleFor(x => x.PerformanceGrade)
            .NotEmpty().WithMessage("绩效等级不能为空")
            .MaximumLength(10).WithMessage("绩效等级长度不能超过10个字符");
        RuleFor(x => x.ReviewerId)
            .GreaterThanOrEqualTo(0).WithMessage("评审人不能为负数");
        RuleFor(x => x.InterviewNotes)
            .NotEmpty().WithMessage("面谈记录不能为空")
            .MaximumLength(1000).WithMessage("面谈记录长度不能超过1000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PerfAssessment 验证器
// ========================================

/// <summary>
/// 更新PerfAssessment DTO 验证器
/// </summary>
public class TaktPerfAssessmentUpdateValidator : AbstractValidator<TaktPerfAssessmentUpdateDto>
{
    /// <summary>
    /// 初始化 更新PerfAssessment 校验规则
    /// </summary>
    public TaktPerfAssessmentUpdateValidator()
    {
        RuleFor(x => x.PerfAssessmentId)
            .GreaterThan(0).WithMessage("PerfAssessmentID无效");
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
        RuleFor(x => x.AssessmentPeriod)
            .NotEmpty().WithMessage("考核周期不能为空")
            .MaximumLength(50).WithMessage("考核周期长度不能超过50个字符");
        RuleFor(x => x.SchemeMetricId)
            .GreaterThanOrEqualTo(0).WithMessage("方案指标不能为负数");
        RuleFor(x => x.SelfEvaluationNotes)
            .NotEmpty().WithMessage("自评说明不能为空")
            .MaximumLength(1000).WithMessage("自评说明长度不能超过1000个字符");
        RuleFor(x => x.SupervisorComments)
            .NotEmpty().WithMessage("主管评语不能为空")
            .MaximumLength(1000).WithMessage("主管评语长度不能超过1000个字符");
        RuleFor(x => x.PerformanceGrade)
            .NotEmpty().WithMessage("绩效等级不能为空")
            .MaximumLength(10).WithMessage("绩效等级长度不能超过10个字符");
        RuleFor(x => x.ReviewerId)
            .GreaterThanOrEqualTo(0).WithMessage("评审人不能为负数");
        RuleFor(x => x.InterviewNotes)
            .NotEmpty().WithMessage("面谈记录不能为空")
            .MaximumLength(1000).WithMessage("面谈记录长度不能超过1000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入PerfAssessment 验证器
// ========================================

/// <summary>
/// 导入PerfAssessment DTO 验证器
/// </summary>
public class TaktPerfAssessmentImportValidator : AbstractValidator<TaktPerfAssessmentImportDto>
{
    /// <summary>
    /// 初始化 导入PerfAssessment 校验规则
    /// </summary>
    public TaktPerfAssessmentImportValidator()
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
        RuleFor(x => x.AssessmentPeriod)
            .NotEmpty().WithMessage("考核周期不能为空")
            .MaximumLength(50).WithMessage("考核周期长度不能超过50个字符");
        RuleFor(x => x.SchemeMetricId)
            .GreaterThanOrEqualTo(0).WithMessage("方案指标不能为负数");
        RuleFor(x => x.SelfEvaluationNotes)
            .NotEmpty().WithMessage("自评说明不能为空")
            .MaximumLength(1000).WithMessage("自评说明长度不能超过1000个字符");
        RuleFor(x => x.SupervisorComments)
            .NotEmpty().WithMessage("主管评语不能为空")
            .MaximumLength(1000).WithMessage("主管评语长度不能超过1000个字符");
        RuleFor(x => x.PerformanceGrade)
            .NotEmpty().WithMessage("绩效等级不能为空")
            .MaximumLength(10).WithMessage("绩效等级长度不能超过10个字符");
        RuleFor(x => x.ReviewerId)
            .GreaterThanOrEqualTo(0).WithMessage("评审人不能为负数");
        RuleFor(x => x.InterviewNotes)
            .NotEmpty().WithMessage("面谈记录不能为空")
            .MaximumLength(1000).WithMessage("面谈记录长度不能超过1000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
