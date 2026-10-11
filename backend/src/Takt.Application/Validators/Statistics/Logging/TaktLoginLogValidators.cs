// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Statistics.Logging
// 文件名称：TaktLoginLogValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：LoginLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktLoginLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Statistics.Logging;

namespace Takt.Application.Validators.Statistics.Logging;

// ========================================
// 创建LoginLog 验证器
// ========================================

/// <summary>
/// 创建LoginLog DTO 验证器
/// </summary>
public class TaktLoginLogCreateValidator : AbstractValidator<TaktLoginLogCreateDto>
{
    /// <summary>
    /// 初始化 创建LoginLog 校验规则
    /// </summary>
    public TaktLoginLogCreateValidator()
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
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空")
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.LoginType)
            .NotEmpty().WithMessage("登录方式不能为空")
            .MaximumLength(40).WithMessage("登录方式长度不能超过40个字符");
        RuleFor(x => x.Browser)
            .NotEmpty().WithMessage("浏览器不能为空")
            .MaximumLength(40).WithMessage("浏览器长度不能超过40个字符");
        RuleFor(x => x.Os)
            .NotEmpty().WithMessage("操作系统不能为空")
            .MaximumLength(40).WithMessage("操作系统长度不能超过40个字符");
        RuleFor(x => x.UserAgent)
            .NotEmpty().WithMessage("用户代理不能为空")
            .MaximumLength(500).WithMessage("用户代理长度不能超过500个字符");
        RuleFor(x => x.LoginResult)
            .NotEmpty().WithMessage("登录结果不能为空")
            .MaximumLength(40).WithMessage("登录结果长度不能超过40个字符");
        RuleFor(x => x.LoginMessage)
            .NotEmpty().WithMessage("登录结果消息不能为空")
            .MaximumLength(500).WithMessage("登录结果消息长度不能超过500个字符");
        RuleFor(x => x.LoginIp)
            .NotEmpty().WithMessage("登录IP地址不能为空")
            .MaximumLength(50).WithMessage("登录IP地址长度不能超过50个字符");
        RuleFor(x => x.LoginLocation)
            .NotEmpty().WithMessage("登录地点不能为空")
            .MaximumLength(200).WithMessage("登录地点长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新LoginLog 验证器
// ========================================

/// <summary>
/// 更新LoginLog DTO 验证器
/// </summary>
public class TaktLoginLogUpdateValidator : AbstractValidator<TaktLoginLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新LoginLog 校验规则
    /// </summary>
    public TaktLoginLogUpdateValidator()
    {
        RuleFor(x => x.LoginLogId)
            .GreaterThan(0).WithMessage("LoginLogID无效");
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
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空")
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.LoginType)
            .NotEmpty().WithMessage("登录方式不能为空")
            .MaximumLength(40).WithMessage("登录方式长度不能超过40个字符");
        RuleFor(x => x.Browser)
            .NotEmpty().WithMessage("浏览器不能为空")
            .MaximumLength(40).WithMessage("浏览器长度不能超过40个字符");
        RuleFor(x => x.Os)
            .NotEmpty().WithMessage("操作系统不能为空")
            .MaximumLength(40).WithMessage("操作系统长度不能超过40个字符");
        RuleFor(x => x.UserAgent)
            .NotEmpty().WithMessage("用户代理不能为空")
            .MaximumLength(500).WithMessage("用户代理长度不能超过500个字符");
        RuleFor(x => x.LoginResult)
            .NotEmpty().WithMessage("登录结果不能为空")
            .MaximumLength(40).WithMessage("登录结果长度不能超过40个字符");
        RuleFor(x => x.LoginMessage)
            .NotEmpty().WithMessage("登录结果消息不能为空")
            .MaximumLength(500).WithMessage("登录结果消息长度不能超过500个字符");
        RuleFor(x => x.LoginIp)
            .NotEmpty().WithMessage("登录IP地址不能为空")
            .MaximumLength(50).WithMessage("登录IP地址长度不能超过50个字符");
        RuleFor(x => x.LoginLocation)
            .NotEmpty().WithMessage("登录地点不能为空")
            .MaximumLength(200).WithMessage("登录地点长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}
