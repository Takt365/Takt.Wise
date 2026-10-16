// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Organization
// 文件名称：TaktEmployeeDeptValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeDept 关联 DTO FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEmployeeDept 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Organization;

namespace Takt.Application.Validators.HumanResource.Organization;

// ========================================
// EmployeeDept 关联 DTO 验证器
// ========================================

/// <summary>
/// 关联 EmployeeDept DTO 验证器
/// </summary>
public class TaktEmployeeDeptDtoValidator : AbstractValidator<TaktEmployeeDeptDto>
{
    /// <summary>
    /// 初始化 关联 EmployeeDept 校验规则
    /// </summary>
    public TaktEmployeeDeptDtoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("员工无效");
        RuleFor(x => x.DeptId)
            .GreaterThan(0).WithMessage("部门无效");
        RuleFor(x => x.EmployeeName)
            .MaximumLength(200).WithMessage("EmployeeName长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.EmployeeName));
        RuleFor(x => x.DeptName)
            .MaximumLength(200).WithMessage("DeptName长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.DeptName));
    }
}
