// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Identity
// 文件名称：TaktMenuValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：Menu 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMenu 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Identity;

namespace Takt.Application.Validators.Identity;

// ========================================
// 创建Menu 验证器
// ========================================

/// <summary>
/// 创建Menu DTO 验证器
/// </summary>
public class TaktMenuCreateValidator : AbstractValidator<TaktMenuCreateDto>
{
    /// <summary>
    /// 初始化 创建Menu 校验规则
    /// </summary>
    public TaktMenuCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.MenuCode)
            .NotEmpty().WithMessage("菜单编码不能为空")
            .MaximumLength(120).WithMessage("菜单编码长度不能超过120个字符");
        RuleFor(x => x.MenuName)
            .NotEmpty().WithMessage("菜单名称不能为空")
            .MaximumLength(100).WithMessage("菜单名称长度不能超过100个字符");
        RuleFor(x => x.I18nKey)
            .NotEmpty().WithMessage("国际化键不能为空")
            .MaximumLength(140).WithMessage("国际化键长度不能超过140个字符");
        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("菜单图标不能为空")
            .MaximumLength(50).WithMessage("菜单图标长度不能超过50个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父菜单ID不能为负数");
        RuleFor(x => x.MenuPath)
            .NotEmpty().WithMessage("菜单路径不能为空")
            .MaximumLength(500).WithMessage("菜单路径长度不能超过500个字符");
        RuleFor(x => x.Permission)
            .NotEmpty().WithMessage("权限标识不能为空")
            .MaximumLength(100).WithMessage("权限标识长度不能超过100个字符");
        RuleFor(x => x.RoutePath)
            .NotEmpty().WithMessage("路由地址不能为空")
            .MaximumLength(200).WithMessage("路由地址长度不能超过200个字符");
        RuleFor(x => x.ComponentPath)
            .NotEmpty().WithMessage("组件路径不能为空")
            .MaximumLength(200).WithMessage("组件路径长度不能超过200个字符");
        RuleFor(x => x.ExternalUrl)
            .NotEmpty().WithMessage("外部链接地址不能为空")
            .MaximumLength(500).WithMessage("外部链接地址长度不能超过500个字符");
        RuleFor(x => x.MenuDescription)
            .NotEmpty().WithMessage("菜单描述不能为空")
            .MaximumLength(500).WithMessage("菜单描述长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Menu 验证器
// ========================================

/// <summary>
/// 更新Menu DTO 验证器
/// </summary>
public class TaktMenuUpdateValidator : AbstractValidator<TaktMenuUpdateDto>
{
    /// <summary>
    /// 初始化 更新Menu 校验规则
    /// </summary>
    public TaktMenuUpdateValidator()
    {
        RuleFor(x => x.MenuId)
            .GreaterThan(0).WithMessage("MenuID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.MenuCode)
            .NotEmpty().WithMessage("菜单编码不能为空")
            .MaximumLength(120).WithMessage("菜单编码长度不能超过120个字符");
        RuleFor(x => x.MenuName)
            .NotEmpty().WithMessage("菜单名称不能为空")
            .MaximumLength(100).WithMessage("菜单名称长度不能超过100个字符");
        RuleFor(x => x.I18nKey)
            .NotEmpty().WithMessage("国际化键不能为空")
            .MaximumLength(140).WithMessage("国际化键长度不能超过140个字符");
        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("菜单图标不能为空")
            .MaximumLength(50).WithMessage("菜单图标长度不能超过50个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父菜单ID不能为负数");
        RuleFor(x => x.MenuPath)
            .NotEmpty().WithMessage("菜单路径不能为空")
            .MaximumLength(500).WithMessage("菜单路径长度不能超过500个字符");
        RuleFor(x => x.Permission)
            .NotEmpty().WithMessage("权限标识不能为空")
            .MaximumLength(100).WithMessage("权限标识长度不能超过100个字符");
        RuleFor(x => x.RoutePath)
            .NotEmpty().WithMessage("路由地址不能为空")
            .MaximumLength(200).WithMessage("路由地址长度不能超过200个字符");
        RuleFor(x => x.ComponentPath)
            .NotEmpty().WithMessage("组件路径不能为空")
            .MaximumLength(200).WithMessage("组件路径长度不能超过200个字符");
        RuleFor(x => x.ExternalUrl)
            .NotEmpty().WithMessage("外部链接地址不能为空")
            .MaximumLength(500).WithMessage("外部链接地址长度不能超过500个字符");
        RuleFor(x => x.MenuDescription)
            .NotEmpty().WithMessage("菜单描述不能为空")
            .MaximumLength(500).WithMessage("菜单描述长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Menu 验证器
// ========================================

/// <summary>
/// 导入Menu DTO 验证器
/// </summary>
public class TaktMenuImportValidator : AbstractValidator<TaktMenuImportDto>
{
    /// <summary>
    /// 初始化 导入Menu 校验规则
    /// </summary>
    public TaktMenuImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.MenuCode)
            .NotEmpty().WithMessage("菜单编码不能为空")
            .MaximumLength(120).WithMessage("菜单编码长度不能超过120个字符");
        RuleFor(x => x.MenuName)
            .NotEmpty().WithMessage("菜单名称不能为空")
            .MaximumLength(100).WithMessage("菜单名称长度不能超过100个字符");
        RuleFor(x => x.I18nKey)
            .NotEmpty().WithMessage("国际化键不能为空")
            .MaximumLength(140).WithMessage("国际化键长度不能超过140个字符");
        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("菜单图标不能为空")
            .MaximumLength(50).WithMessage("菜单图标长度不能超过50个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父菜单ID不能为负数");
        RuleFor(x => x.MenuPath)
            .NotEmpty().WithMessage("菜单路径不能为空")
            .MaximumLength(500).WithMessage("菜单路径长度不能超过500个字符");
        RuleFor(x => x.Permission)
            .NotEmpty().WithMessage("权限标识不能为空")
            .MaximumLength(100).WithMessage("权限标识长度不能超过100个字符");
        RuleFor(x => x.RoutePath)
            .NotEmpty().WithMessage("路由地址不能为空")
            .MaximumLength(200).WithMessage("路由地址长度不能超过200个字符");
        RuleFor(x => x.ComponentPath)
            .NotEmpty().WithMessage("组件路径不能为空")
            .MaximumLength(200).WithMessage("组件路径长度不能超过200个字符");
        RuleFor(x => x.ExternalUrl)
            .NotEmpty().WithMessage("外部链接地址不能为空")
            .MaximumLength(500).WithMessage("外部链接地址长度不能超过500个字符");
        RuleFor(x => x.MenuDescription)
            .NotEmpty().WithMessage("菜单描述不能为空")
            .MaximumLength(500).WithMessage("菜单描述长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
