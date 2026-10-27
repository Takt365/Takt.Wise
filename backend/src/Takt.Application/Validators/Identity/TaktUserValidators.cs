// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Identity
// 文件名称：TaktUserValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：User 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktUser 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Identity;

namespace Takt.Application.Validators.Identity;

// ========================================
// 创建User 验证器
// ========================================

/// <summary>
/// 创建User DTO 验证器
/// </summary>
public class TaktUserCreateValidator : AbstractValidator<TaktUserCreateDto>
{
    /// <summary>
    /// 初始化 创建User 校验规则
    /// </summary>
    public TaktUserCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空")
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.NickName)
            .NotEmpty().WithMessage("昵称不能为空")
            .MaximumLength(20).WithMessage("昵称长度不能超过20个字符");
        RuleFor(x => x.PasswordHash)
            .MaximumLength(255).WithMessage("密码哈希值长度不能超过255个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("关联的员工ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新User 验证器
// ========================================

/// <summary>
/// 更新User DTO 验证器
/// </summary>
public class TaktUserUpdateValidator : AbstractValidator<TaktUserUpdateDto>
{
    /// <summary>
    /// 初始化 更新User 校验规则
    /// </summary>
    public TaktUserUpdateValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空")
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空")
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.NickName)
            .NotEmpty().WithMessage("昵称不能为空")
            .MaximumLength(20).WithMessage("昵称长度不能超过20个字符");
        RuleFor(x => x.PasswordHash)
            .MaximumLength(255).WithMessage("密码哈希值长度不能超过255个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("关联的员工ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入User 验证器
// ========================================

/// <summary>
/// 导入User DTO 验证器
/// </summary>
public class TaktUserImportValidator : AbstractValidator<TaktUserImportDto>
{
    /// <summary>
    /// 初始化 导入User 校验规则
    /// </summary>
    public TaktUserImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空")
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.NickName)
            .NotEmpty().WithMessage("昵称不能为空")
            .MaximumLength(20).WithMessage("昵称长度不能超过20个字符");
        RuleFor(x => x.PasswordHash)
            .MaximumLength(255).WithMessage("密码哈希值长度不能超过255个字符").When(x => !string.IsNullOrWhiteSpace(x.PasswordHash));
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("关联的员工ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
