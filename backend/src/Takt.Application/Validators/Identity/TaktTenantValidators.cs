// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Identity
// 文件名称：TaktTenantValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：Tenant 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTenant 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Identity;

namespace Takt.Application.Validators.Identity;

// ========================================
// 创建Tenant 验证器
// ========================================

/// <summary>
/// 创建Tenant DTO 验证器
/// </summary>
public class TaktTenantCreateValidator : AbstractValidator<TaktTenantCreateDto>
{
    /// <summary>
    /// 初始化 创建Tenant 校验规则
    /// </summary>
    public TaktTenantCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.TenantName)
            .NotEmpty().WithMessage("租户名称不能为空")
            .MaximumLength(100).WithMessage("租户名称长度不能超过100个字符");
        RuleFor(x => x.ContactEmail)
            .NotEmpty().WithMessage("联系邮箱不能为空")
            .MaximumLength(100).WithMessage("联系邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("联系邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Tenant 验证器
// ========================================

/// <summary>
/// 更新Tenant DTO 验证器
/// </summary>
public class TaktTenantUpdateValidator : AbstractValidator<TaktTenantUpdateDto>
{
    /// <summary>
    /// 初始化 更新Tenant 校验规则
    /// </summary>
    public TaktTenantUpdateValidator()
    {
        RuleFor(x => x.TenantId)
            .GreaterThan(0).WithMessage("TenantID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.TenantName)
            .NotEmpty().WithMessage("租户名称不能为空")
            .MaximumLength(100).WithMessage("租户名称长度不能超过100个字符");
        RuleFor(x => x.ContactEmail)
            .NotEmpty().WithMessage("联系邮箱不能为空")
            .MaximumLength(100).WithMessage("联系邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("联系邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Tenant 验证器
// ========================================

/// <summary>
/// 导入Tenant DTO 验证器
/// </summary>
public class TaktTenantImportValidator : AbstractValidator<TaktTenantImportDto>
{
    /// <summary>
    /// 初始化 导入Tenant 校验规则
    /// </summary>
    public TaktTenantImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.TenantName)
            .NotEmpty().WithMessage("租户名称不能为空")
            .MaximumLength(100).WithMessage("租户名称长度不能超过100个字符");
        RuleFor(x => x.ContactEmail)
            .NotEmpty().WithMessage("联系邮箱不能为空")
            .MaximumLength(100).WithMessage("联系邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("联系邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
