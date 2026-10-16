// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Personnel
// 文件名称：TaktEmployeeJoinedValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeJoined 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEmployeeJoined 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Personnel;

namespace Takt.Application.Validators.HumanResource.Personnel;

// ========================================
// 创建EmployeeJoined 验证器
// ========================================

/// <summary>
/// 创建EmployeeJoined DTO 验证器
/// </summary>
public class TaktEmployeeJoinedCreateValidator : AbstractValidator<TaktEmployeeJoinedCreateDto>
{
    /// <summary>
    /// 初始化 创建EmployeeJoined 校验规则
    /// </summary>
    public TaktEmployeeJoinedCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工不能为负数");
        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("员工编码不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(6).WithMessage("员工编码长度不能超过6个字符");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(80).WithMessage("员工姓名长度不能超过80个字符");
        RuleFor(x => x.OnboardingId)
            .GreaterThanOrEqualTo(0).WithMessage("入职待办不能为负数");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("上岗部门不能为负数");
        RuleFor(x => x.DeptName)
            .NotEmpty().WithMessage("上岗部门名称不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(100).WithMessage("上岗部门名称长度不能超过100个字符");
        RuleFor(x => x.PostId)
            .GreaterThanOrEqualTo(0).WithMessage("上岗岗位不能为负数");
        RuleFor(x => x.DirectManagerId)
            .GreaterThanOrEqualTo(0).WithMessage("直属上级不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EmployeeJoined 验证器
// ========================================

/// <summary>
/// 更新EmployeeJoined DTO 验证器
/// </summary>
public class TaktEmployeeJoinedUpdateValidator : AbstractValidator<TaktEmployeeJoinedUpdateDto>
{
    /// <summary>
    /// 初始化 更新EmployeeJoined 校验规则
    /// </summary>
    public TaktEmployeeJoinedUpdateValidator()
    {
        RuleFor(x => x.EmployeeJoinedId)
            .GreaterThan(0).WithMessage("EmployeeJoinedID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工不能为负数");
        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("员工编码不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(6).WithMessage("员工编码长度不能超过6个字符");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(80).WithMessage("员工姓名长度不能超过80个字符");
        RuleFor(x => x.OnboardingId)
            .GreaterThanOrEqualTo(0).WithMessage("入职待办不能为负数");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("上岗部门不能为负数");
        RuleFor(x => x.DeptName)
            .NotEmpty().WithMessage("上岗部门名称不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(100).WithMessage("上岗部门名称长度不能超过100个字符");
        RuleFor(x => x.PostId)
            .GreaterThanOrEqualTo(0).WithMessage("上岗岗位不能为负数");
        RuleFor(x => x.DirectManagerId)
            .GreaterThanOrEqualTo(0).WithMessage("直属上级不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入EmployeeJoined 验证器
// ========================================

/// <summary>
/// 导入EmployeeJoined DTO 验证器
/// </summary>
public class TaktEmployeeJoinedImportValidator : AbstractValidator<TaktEmployeeJoinedImportDto>
{
    /// <summary>
    /// 初始化 导入EmployeeJoined 校验规则
    /// </summary>
    public TaktEmployeeJoinedImportValidator()
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
        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("员工编码不能为空")
            .MaximumLength(6).WithMessage("员工编码长度不能超过6个字符");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空")
            .MaximumLength(80).WithMessage("员工姓名长度不能超过80个字符");
        RuleFor(x => x.OnboardingId)
            .GreaterThanOrEqualTo(0).WithMessage("入职待办不能为负数");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("上岗部门不能为负数");
        RuleFor(x => x.DeptName)
            .NotEmpty().WithMessage("上岗部门名称不能为空")
            .MaximumLength(100).WithMessage("上岗部门名称长度不能超过100个字符");
        RuleFor(x => x.PostId)
            .GreaterThanOrEqualTo(0).WithMessage("上岗岗位不能为负数");
        RuleFor(x => x.DirectManagerId)
            .GreaterThanOrEqualTo(0).WithMessage("直属上级不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
