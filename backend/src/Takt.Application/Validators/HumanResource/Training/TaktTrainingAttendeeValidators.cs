// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Training
// 文件名称：TaktTrainingAttendeeValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TrainingAttendee 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTrainingAttendee 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Training;

namespace Takt.Application.Validators.HumanResource.Training;

// ========================================
// 创建TrainingAttendee 验证器
// ========================================

/// <summary>
/// 创建TrainingAttendee DTO 验证器
/// </summary>
public class TaktTrainingAttendeeCreateValidator : AbstractValidator<TaktTrainingAttendeeCreateDto>
{
    /// <summary>
    /// 初始化 创建TrainingAttendee 校验规则
    /// </summary>
    public TaktTrainingAttendeeCreateValidator()
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
        RuleFor(x => x.TrainingCourseId)
            .GreaterThanOrEqualTo(0).WithMessage("培训课程不能为负数");
        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("培训课程名称不能为空")
            .MaximumLength(40).WithMessage("培训课程名称长度不能超过40个字符");
        RuleFor(x => x.TrainingType)
            .NotEmpty().WithMessage("培训类型不能为空")
            .MaximumLength(50).WithMessage("培训类型长度不能超过50个字符");
        RuleFor(x => x.Instructor)
            .NotEmpty().WithMessage("培训讲师不能为空")
            .MaximumLength(50).WithMessage("培训讲师长度不能超过50个字符");
        RuleFor(x => x.CertificateCode)
            .NotEmpty().WithMessage("证书编码不能为空")
            .MaximumLength(50).WithMessage("证书编码长度不能超过50个字符");
        RuleFor(x => x.TrainingEvaluation)
            .NotEmpty().WithMessage("培训评价不能为空")
            .MaximumLength(500).WithMessage("培训评价长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TrainingAttendee 验证器
// ========================================

/// <summary>
/// 更新TrainingAttendee DTO 验证器
/// </summary>
public class TaktTrainingAttendeeUpdateValidator : AbstractValidator<TaktTrainingAttendeeUpdateDto>
{
    /// <summary>
    /// 初始化 更新TrainingAttendee 校验规则
    /// </summary>
    public TaktTrainingAttendeeUpdateValidator()
    {
        RuleFor(x => x.TrainingAttendeeId)
            .GreaterThan(0).WithMessage("TrainingAttendeeID无效");
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
        RuleFor(x => x.TrainingCourseId)
            .GreaterThanOrEqualTo(0).WithMessage("培训课程不能为负数");
        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("培训课程名称不能为空")
            .MaximumLength(40).WithMessage("培训课程名称长度不能超过40个字符");
        RuleFor(x => x.TrainingType)
            .NotEmpty().WithMessage("培训类型不能为空")
            .MaximumLength(50).WithMessage("培训类型长度不能超过50个字符");
        RuleFor(x => x.Instructor)
            .NotEmpty().WithMessage("培训讲师不能为空")
            .MaximumLength(50).WithMessage("培训讲师长度不能超过50个字符");
        RuleFor(x => x.CertificateCode)
            .NotEmpty().WithMessage("证书编码不能为空")
            .MaximumLength(50).WithMessage("证书编码长度不能超过50个字符");
        RuleFor(x => x.TrainingEvaluation)
            .NotEmpty().WithMessage("培训评价不能为空")
            .MaximumLength(500).WithMessage("培训评价长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入TrainingAttendee 验证器
// ========================================

/// <summary>
/// 导入TrainingAttendee DTO 验证器
/// </summary>
public class TaktTrainingAttendeeImportValidator : AbstractValidator<TaktTrainingAttendeeImportDto>
{
    /// <summary>
    /// 初始化 导入TrainingAttendee 校验规则
    /// </summary>
    public TaktTrainingAttendeeImportValidator()
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
        RuleFor(x => x.TrainingCourseId)
            .GreaterThanOrEqualTo(0).WithMessage("培训课程不能为负数");
        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("培训课程名称不能为空")
            .MaximumLength(40).WithMessage("培训课程名称长度不能超过40个字符");
        RuleFor(x => x.TrainingType)
            .NotEmpty().WithMessage("培训类型不能为空")
            .MaximumLength(50).WithMessage("培训类型长度不能超过50个字符");
        RuleFor(x => x.Instructor)
            .NotEmpty().WithMessage("培训讲师不能为空")
            .MaximumLength(50).WithMessage("培训讲师长度不能超过50个字符");
        RuleFor(x => x.CertificateCode)
            .NotEmpty().WithMessage("证书编码不能为空")
            .MaximumLength(50).WithMessage("证书编码长度不能超过50个字符");
        RuleFor(x => x.TrainingEvaluation)
            .NotEmpty().WithMessage("培训评价不能为空")
            .MaximumLength(500).WithMessage("培训评价长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
