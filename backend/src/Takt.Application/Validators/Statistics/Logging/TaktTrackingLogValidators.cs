// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Statistics.Logging
// 文件名称：TaktTrackingLogValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TrackingLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTrackingLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Statistics.Logging;

namespace Takt.Application.Validators.Statistics.Logging;

// ========================================
// 创建TrackingLog 验证器
// ========================================

/// <summary>
/// 创建TrackingLog DTO 验证器
/// </summary>
public class TaktTrackingLogCreateValidator : AbstractValidator<TaktTrackingLogCreateDto>
{
    /// <summary>
    /// 初始化 创建TrackingLog 校验规则
    /// </summary>
    public TaktTrackingLogCreateValidator()
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
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空").When(x => x.UserId <= 0)
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.EventTrackingType)
            .NotEmpty().WithMessage("事件类型不能为空")
            .MaximumLength(40).WithMessage("事件类型长度不能超过40个字符");
        RuleFor(x => x.EventTrackingCategory)
            .NotEmpty().WithMessage("事件分类不能为空")
            .MaximumLength(40).WithMessage("事件分类长度不能超过40个字符");
        RuleFor(x => x.EntryName)
            .NotEmpty().WithMessage("PerformanceEntry.name不能为空")
            .MaximumLength(40).WithMessage("PerformanceEntry.name长度不能超过40个字符");
        RuleFor(x => x.RoutePath)
            .NotEmpty().WithMessage("SPA 路由路径不能为空")
            .MaximumLength(500).WithMessage("SPA 路由路径长度不能超过500个字符");
        RuleFor(x => x.PageUrl)
            .NotEmpty().WithMessage("页面完整 URL不能为空")
            .MaximumLength(500).WithMessage("页面完整 URL长度不能超过500个字符");
        RuleFor(x => x.ContainerType)
            .NotEmpty().WithMessage("TaskAttribution.containerType不能为空")
            .MaximumLength(40).WithMessage("TaskAttribution.containerType长度不能超过40个字符");
        RuleFor(x => x.ContainerName)
            .NotEmpty().WithMessage("TaskAttribution.containerName不能为空")
            .MaximumLength(200).WithMessage("TaskAttribution.containerName长度不能超过200个字符");
        RuleFor(x => x.ContainerSrc)
            .NotEmpty().WithMessage("TaskAttribution.containerSrc不能为空")
            .MaximumLength(500).WithMessage("TaskAttribution.containerSrc长度不能超过500个字符");
        RuleFor(x => x.ContainerId)
            .NotEmpty().WithMessage("TaskAttribution.containerId不能为空")
            .MaximumLength(40).WithMessage("TaskAttribution.containerId长度不能超过40个字符");
        RuleFor(x => x.AttributionJson)
            .NotEmpty().WithMessage("完整 attribution JSON 数组不能为空");
        RuleFor(x => x.UserAgent)
            .NotEmpty().WithMessage("用户代理不能为空")
            .MaximumLength(500).WithMessage("用户代理长度不能超过500个字符");
        RuleFor(x => x.ClientIp)
            .NotEmpty().WithMessage("客户端 IP不能为空")
            .MaximumLength(50).WithMessage("客户端 IP长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TrackingLog 验证器
// ========================================

/// <summary>
/// 更新TrackingLog DTO 验证器
/// </summary>
public class TaktTrackingLogUpdateValidator : AbstractValidator<TaktTrackingLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新TrackingLog 校验规则
    /// </summary>
    public TaktTrackingLogUpdateValidator()
    {
        RuleFor(x => x.TrackingLogId)
            .GreaterThan(0).WithMessage("TrackingLogID无效");
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
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户名不能为空").When(x => x.UserId <= 0)
            .MaximumLength(20).WithMessage("用户名长度不能超过20个字符");
        RuleFor(x => x.EventTrackingType)
            .NotEmpty().WithMessage("事件类型不能为空")
            .MaximumLength(40).WithMessage("事件类型长度不能超过40个字符");
        RuleFor(x => x.EventTrackingCategory)
            .NotEmpty().WithMessage("事件分类不能为空")
            .MaximumLength(40).WithMessage("事件分类长度不能超过40个字符");
        RuleFor(x => x.EntryName)
            .NotEmpty().WithMessage("PerformanceEntry.name不能为空")
            .MaximumLength(40).WithMessage("PerformanceEntry.name长度不能超过40个字符");
        RuleFor(x => x.RoutePath)
            .NotEmpty().WithMessage("SPA 路由路径不能为空")
            .MaximumLength(500).WithMessage("SPA 路由路径长度不能超过500个字符");
        RuleFor(x => x.PageUrl)
            .NotEmpty().WithMessage("页面完整 URL不能为空")
            .MaximumLength(500).WithMessage("页面完整 URL长度不能超过500个字符");
        RuleFor(x => x.ContainerType)
            .NotEmpty().WithMessage("TaskAttribution.containerType不能为空")
            .MaximumLength(40).WithMessage("TaskAttribution.containerType长度不能超过40个字符");
        RuleFor(x => x.ContainerName)
            .NotEmpty().WithMessage("TaskAttribution.containerName不能为空")
            .MaximumLength(200).WithMessage("TaskAttribution.containerName长度不能超过200个字符");
        RuleFor(x => x.ContainerSrc)
            .NotEmpty().WithMessage("TaskAttribution.containerSrc不能为空")
            .MaximumLength(500).WithMessage("TaskAttribution.containerSrc长度不能超过500个字符");
        RuleFor(x => x.ContainerId)
            .NotEmpty().WithMessage("TaskAttribution.containerId不能为空")
            .MaximumLength(40).WithMessage("TaskAttribution.containerId长度不能超过40个字符");
        RuleFor(x => x.AttributionJson)
            .NotEmpty().WithMessage("完整 attribution JSON 数组不能为空");
        RuleFor(x => x.UserAgent)
            .NotEmpty().WithMessage("用户代理不能为空")
            .MaximumLength(500).WithMessage("用户代理长度不能超过500个字符");
        RuleFor(x => x.ClientIp)
            .NotEmpty().WithMessage("客户端 IP不能为空")
            .MaximumLength(50).WithMessage("客户端 IP长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}
