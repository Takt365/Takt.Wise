// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Identity
// 文件名称：TaktRoleValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：Role 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktRole 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Identity;

namespace Takt.Application.Validators.Identity;

// ========================================
// 创建Role 验证器
// ========================================

/// <summary>
/// 创建Role DTO 验证器
/// </summary>
public class TaktRoleCreateValidator : AbstractValidator<TaktRoleCreateDto>
{
    /// <summary>
    /// 初始化 创建Role 校验规则
    /// </summary>
    public TaktRoleCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.RoleCode)
            .NotEmpty().WithMessage("角色编码不能为空")
            .MaximumLength(50).WithMessage("角色编码长度不能超过50个字符");
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("角色名称不能为空")
            .MaximumLength(100).WithMessage("角色名称长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Role 验证器
// ========================================

/// <summary>
/// 更新Role DTO 验证器
/// </summary>
public class TaktRoleUpdateValidator : AbstractValidator<TaktRoleUpdateDto>
{
    /// <summary>
    /// 初始化 更新Role 校验规则
    /// </summary>
    public TaktRoleUpdateValidator()
    {
        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("RoleID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.RoleCode)
            .NotEmpty().WithMessage("角色编码不能为空")
            .MaximumLength(50).WithMessage("角色编码长度不能超过50个字符");
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("角色名称不能为空")
            .MaximumLength(100).WithMessage("角色名称长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Role 验证器
// ========================================

/// <summary>
/// 导入Role DTO 验证器
/// </summary>
public class TaktRoleImportValidator : AbstractValidator<TaktRoleImportDto>
{
    /// <summary>
    /// 初始化 导入Role 校验规则
    /// </summary>
    public TaktRoleImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.RoleCode)
            .NotEmpty().WithMessage("角色编码不能为空")
            .MaximumLength(50).WithMessage("角色编码长度不能超过50个字符");
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("角色名称不能为空")
            .MaximumLength(100).WithMessage("角色名称长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
