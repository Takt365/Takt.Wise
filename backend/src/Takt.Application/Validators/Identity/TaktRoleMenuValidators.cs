// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Identity
// 文件名称：TaktRoleMenuValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：RoleMenu 关联 DTO FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktRoleMenu 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Identity;

namespace Takt.Application.Validators.Identity;

// ========================================
// RoleMenu 关联 DTO 验证器
// ========================================

/// <summary>
/// 关联 RoleMenu DTO 验证器
/// </summary>
public class TaktRoleMenuDtoValidator : AbstractValidator<TaktRoleMenuDto>
{
    /// <summary>
    /// 初始化 关联 RoleMenu 校验规则
    /// </summary>
    public TaktRoleMenuDtoValidator()
    {
        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("角色ID无效");
        RuleFor(x => x.MenuId)
            .GreaterThan(0).WithMessage("菜单ID无效");
        RuleFor(x => x.RoleName)
            .MaximumLength(200).WithMessage("RoleName长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.RoleName));
        RuleFor(x => x.MenuName)
            .MaximumLength(200).WithMessage("MenuName长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.MenuName));
    }
}
