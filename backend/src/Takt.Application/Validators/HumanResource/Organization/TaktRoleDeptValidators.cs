// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Organization
// 文件名称：TaktRoleDeptValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：RoleDept 关联 DTO FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktRoleDept 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Organization;

namespace Takt.Application.Validators.HumanResource.Organization;

// ========================================
// RoleDept 关联 DTO 验证器
// ========================================

/// <summary>
/// 关联 RoleDept DTO 验证器
/// </summary>
public class TaktRoleDeptDtoValidator : AbstractValidator<TaktRoleDeptDto>
{
    /// <summary>
    /// 初始化 关联 RoleDept 校验规则
    /// </summary>
    public TaktRoleDeptDtoValidator()
    {
        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("角色无效");
        RuleFor(x => x.DeptId)
            .GreaterThan(0).WithMessage("部门无效");
        RuleFor(x => x.RoleName)
            .MaximumLength(200).WithMessage("RoleName长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.RoleName));
        RuleFor(x => x.DeptName)
            .MaximumLength(200).WithMessage("DeptName长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.DeptName));
    }
}
