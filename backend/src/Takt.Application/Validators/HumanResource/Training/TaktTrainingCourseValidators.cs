// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Training
// 文件名称：TaktTrainingCourseValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TrainingCourse 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTrainingCourse 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Training;

namespace Takt.Application.Validators.HumanResource.Training;

// ========================================
// 创建TrainingCourse 验证器
// ========================================

/// <summary>
/// 创建TrainingCourse DTO 验证器
/// </summary>
public class TaktTrainingCourseCreateValidator : AbstractValidator<TaktTrainingCourseCreateDto>
{
    /// <summary>
    /// 初始化 创建TrainingCourse 校验规则
    /// </summary>
    public TaktTrainingCourseCreateValidator()
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
        RuleFor(x => x.CourseCode)
            .NotEmpty().WithMessage("课程编码不能为空")
            .MaximumLength(40).WithMessage("课程编码长度不能超过40个字符");
        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("课程名称不能为空")
            .MaximumLength(40).WithMessage("课程名称长度不能超过40个字符");
        RuleFor(x => x.CourseType)
            .NotEmpty().WithMessage("课程类型不能为空")
            .MaximumLength(50).WithMessage("课程类型长度不能超过50个字符");
        RuleFor(x => x.CourseLevel)
            .NotEmpty().WithMessage("课程级别不能为空")
            .MaximumLength(50).WithMessage("课程级别长度不能超过50个字符");
        RuleFor(x => x.CourseDescription)
            .NotEmpty().WithMessage("课程描述不能为空")
            .MaximumLength(1000).WithMessage("课程描述长度不能超过1000个字符");
        RuleFor(x => x.CourseObjectives)
            .NotEmpty().WithMessage("课程目标不能为空")
            .MaximumLength(1000).WithMessage("课程目标长度不能超过1000个字符");
        RuleFor(x => x.MainInstructor)
            .NotEmpty().WithMessage("主讲讲师不能为空")
            .MaximumLength(50).WithMessage("主讲讲师长度不能超过50个字符");
        RuleFor(x => x.TrainingMethod)
            .NotEmpty().WithMessage("培训方式不能为空")
            .MaximumLength(50).WithMessage("培训方式长度不能超过50个字符");
        RuleFor(x => x.AssessmentMethod)
            .NotEmpty().WithMessage("考核方式不能为空")
            .MaximumLength(50).WithMessage("考核方式长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TrainingCourse 验证器
// ========================================

/// <summary>
/// 更新TrainingCourse DTO 验证器
/// </summary>
public class TaktTrainingCourseUpdateValidator : AbstractValidator<TaktTrainingCourseUpdateDto>
{
    /// <summary>
    /// 初始化 更新TrainingCourse 校验规则
    /// </summary>
    public TaktTrainingCourseUpdateValidator()
    {
        RuleFor(x => x.TrainingCourseId)
            .GreaterThan(0).WithMessage("TrainingCourseID无效");
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
        RuleFor(x => x.CourseCode)
            .NotEmpty().WithMessage("课程编码不能为空")
            .MaximumLength(40).WithMessage("课程编码长度不能超过40个字符");
        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("课程名称不能为空")
            .MaximumLength(40).WithMessage("课程名称长度不能超过40个字符");
        RuleFor(x => x.CourseType)
            .NotEmpty().WithMessage("课程类型不能为空")
            .MaximumLength(50).WithMessage("课程类型长度不能超过50个字符");
        RuleFor(x => x.CourseLevel)
            .NotEmpty().WithMessage("课程级别不能为空")
            .MaximumLength(50).WithMessage("课程级别长度不能超过50个字符");
        RuleFor(x => x.CourseDescription)
            .NotEmpty().WithMessage("课程描述不能为空")
            .MaximumLength(1000).WithMessage("课程描述长度不能超过1000个字符");
        RuleFor(x => x.CourseObjectives)
            .NotEmpty().WithMessage("课程目标不能为空")
            .MaximumLength(1000).WithMessage("课程目标长度不能超过1000个字符");
        RuleFor(x => x.MainInstructor)
            .NotEmpty().WithMessage("主讲讲师不能为空")
            .MaximumLength(50).WithMessage("主讲讲师长度不能超过50个字符");
        RuleFor(x => x.TrainingMethod)
            .NotEmpty().WithMessage("培训方式不能为空")
            .MaximumLength(50).WithMessage("培训方式长度不能超过50个字符");
        RuleFor(x => x.AssessmentMethod)
            .NotEmpty().WithMessage("考核方式不能为空")
            .MaximumLength(50).WithMessage("考核方式长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入TrainingCourse 验证器
// ========================================

/// <summary>
/// 导入TrainingCourse DTO 验证器
/// </summary>
public class TaktTrainingCourseImportValidator : AbstractValidator<TaktTrainingCourseImportDto>
{
    /// <summary>
    /// 初始化 导入TrainingCourse 校验规则
    /// </summary>
    public TaktTrainingCourseImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.CourseCode)
            .NotEmpty().WithMessage("课程编码不能为空")
            .MaximumLength(40).WithMessage("课程编码长度不能超过40个字符");
        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("课程名称不能为空")
            .MaximumLength(40).WithMessage("课程名称长度不能超过40个字符");
        RuleFor(x => x.CourseType)
            .NotEmpty().WithMessage("课程类型不能为空")
            .MaximumLength(50).WithMessage("课程类型长度不能超过50个字符");
        RuleFor(x => x.CourseLevel)
            .NotEmpty().WithMessage("课程级别不能为空")
            .MaximumLength(50).WithMessage("课程级别长度不能超过50个字符");
        RuleFor(x => x.CourseDescription)
            .NotEmpty().WithMessage("课程描述不能为空")
            .MaximumLength(1000).WithMessage("课程描述长度不能超过1000个字符");
        RuleFor(x => x.CourseObjectives)
            .NotEmpty().WithMessage("课程目标不能为空")
            .MaximumLength(1000).WithMessage("课程目标长度不能超过1000个字符");
        RuleFor(x => x.MainInstructor)
            .NotEmpty().WithMessage("主讲讲师不能为空")
            .MaximumLength(50).WithMessage("主讲讲师长度不能超过50个字符");
        RuleFor(x => x.TrainingMethod)
            .NotEmpty().WithMessage("培训方式不能为空")
            .MaximumLength(50).WithMessage("培训方式长度不能超过50个字符");
        RuleFor(x => x.AssessmentMethod)
            .NotEmpty().WithMessage("考核方式不能为空")
            .MaximumLength(50).WithMessage("考核方式长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
