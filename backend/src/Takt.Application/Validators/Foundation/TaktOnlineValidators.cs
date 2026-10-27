// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktOnlineValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：Online 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktOnline 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建Online 验证器
// ========================================

/// <summary>
/// 创建Online DTO 验证器
/// </summary>
public class TaktOnlineCreateValidator : AbstractValidator<TaktOnlineCreateDto>
{
    /// <summary>
    /// 初始化 创建Online 校验规则
    /// </summary>
    public TaktOnlineCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.UserId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.UserId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ConnectionId)
            .NotEmpty().WithMessage("SignalR 连接 ID不能为空")
            .MaximumLength(200).WithMessage("SignalR 连接 ID长度不能超过200个字符");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空").When(x => x.UserId <= 0)
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.ConnectIp)
            .NotEmpty().WithMessage("连接 IP 地址不能为空")
            .MaximumLength(50).WithMessage("连接 IP 地址长度不能超过50个字符");
        RuleFor(x => x.ConnectLocation)
            .NotEmpty().WithMessage("连接地点不能为空")
            .MaximumLength(200).WithMessage("连接地点长度不能超过200个字符");
        RuleFor(x => x.UserAgent)
            .NotEmpty().WithMessage("用户代理不能为空")
            .MaximumLength(500).WithMessage("用户代理长度不能超过500个字符");
        RuleFor(x => x.DeviceType)
            .NotEmpty().WithMessage("登录设备不能为空")
            .MaximumLength(40).WithMessage("登录设备长度不能超过40个字符");
        RuleFor(x => x.BrowserType)
            .NotEmpty().WithMessage("浏览器不能为空")
            .MaximumLength(40).WithMessage("浏览器长度不能超过40个字符");
        RuleFor(x => x.OperatingSystem)
            .NotEmpty().WithMessage("操作系统不能为空")
            .MaximumLength(40).WithMessage("操作系统长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Online 验证器
// ========================================

/// <summary>
/// 更新Online DTO 验证器
/// </summary>
public class TaktOnlineUpdateValidator : AbstractValidator<TaktOnlineUpdateDto>
{
    /// <summary>
    /// 初始化 更新Online 校验规则
    /// </summary>
    public TaktOnlineUpdateValidator()
    {
        RuleFor(x => x.OnlineId)
            .GreaterThan(0).WithMessage("OnlineID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.UserId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.UserId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ConnectionId)
            .NotEmpty().WithMessage("SignalR 连接 ID不能为空")
            .MaximumLength(200).WithMessage("SignalR 连接 ID长度不能超过200个字符");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空").When(x => x.UserId <= 0)
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.ConnectIp)
            .NotEmpty().WithMessage("连接 IP 地址不能为空")
            .MaximumLength(50).WithMessage("连接 IP 地址长度不能超过50个字符");
        RuleFor(x => x.ConnectLocation)
            .NotEmpty().WithMessage("连接地点不能为空")
            .MaximumLength(200).WithMessage("连接地点长度不能超过200个字符");
        RuleFor(x => x.UserAgent)
            .NotEmpty().WithMessage("用户代理不能为空")
            .MaximumLength(500).WithMessage("用户代理长度不能超过500个字符");
        RuleFor(x => x.DeviceType)
            .NotEmpty().WithMessage("登录设备不能为空")
            .MaximumLength(40).WithMessage("登录设备长度不能超过40个字符");
        RuleFor(x => x.BrowserType)
            .NotEmpty().WithMessage("浏览器不能为空")
            .MaximumLength(40).WithMessage("浏览器长度不能超过40个字符");
        RuleFor(x => x.OperatingSystem)
            .NotEmpty().WithMessage("操作系统不能为空")
            .MaximumLength(40).WithMessage("操作系统长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Online 验证器
// ========================================

/// <summary>
/// 导入Online DTO 验证器
/// </summary>
public class TaktOnlineImportValidator : AbstractValidator<TaktOnlineImportDto>
{
    /// <summary>
    /// 初始化 导入Online 校验规则
    /// </summary>
    public TaktOnlineImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ConnectionId)
            .NotEmpty().WithMessage("SignalR 连接 ID不能为空")
            .MaximumLength(200).WithMessage("SignalR 连接 ID长度不能超过200个字符");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空")
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.ConnectIp)
            .NotEmpty().WithMessage("连接 IP 地址不能为空")
            .MaximumLength(50).WithMessage("连接 IP 地址长度不能超过50个字符");
        RuleFor(x => x.ConnectLocation)
            .NotEmpty().WithMessage("连接地点不能为空")
            .MaximumLength(200).WithMessage("连接地点长度不能超过200个字符");
        RuleFor(x => x.UserAgent)
            .NotEmpty().WithMessage("用户代理不能为空")
            .MaximumLength(500).WithMessage("用户代理长度不能超过500个字符");
        RuleFor(x => x.DeviceType)
            .NotEmpty().WithMessage("登录设备不能为空")
            .MaximumLength(40).WithMessage("登录设备长度不能超过40个字符");
        RuleFor(x => x.BrowserType)
            .NotEmpty().WithMessage("浏览器不能为空")
            .MaximumLength(40).WithMessage("浏览器长度不能超过40个字符");
        RuleFor(x => x.OperatingSystem)
            .NotEmpty().WithMessage("操作系统不能为空")
            .MaximumLength(40).WithMessage("操作系统长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
