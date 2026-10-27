// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Identity
// 文件名称：TaktRoleCompanyValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：RoleCompany 关联 DTO FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktRoleCompany 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Identity;

namespace Takt.Application.Validators.Identity;

// ========================================
// RoleCompany 关联 DTO 验证器
// ========================================

/// <summary>
/// 关联 RoleCompany DTO 验证器
/// </summary>
public class TaktRoleCompanyDtoValidator : AbstractValidator<TaktRoleCompanyDto>
{
    /// <summary>
    /// 初始化 关联 RoleCompany 校验规则
    /// </summary>
    public TaktRoleCompanyDtoValidator()
    {
        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("角色ID无效");
        RuleFor(x => x.RoleName)
            .MaximumLength(200).WithMessage("RoleName长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.RoleName));
    }
}
