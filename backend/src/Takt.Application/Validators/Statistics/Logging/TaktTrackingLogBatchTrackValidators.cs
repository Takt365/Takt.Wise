// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Statistics.Logging
// 文件名称：TaktTrackingLogBatchTrackValidators.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：交互日志批量上报 DTO 验证器（非实体 CRUD，不随 generate-validators-from-entity 生成）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Statistics.Logging;

namespace Takt.Application.Validators.Statistics.Logging;

/// <summary>
/// Long Task 单条上报 DTO 验证器
/// </summary>
public class TaktTrackingLogTrackItemValidator : AbstractValidator<TaktTrackingLogTrackDto>
{
    /// <summary>
    /// 初始化单条上报校验规则
    /// </summary>
    public TaktTrackingLogTrackItemValidator()
    {
        RuleFor(x => x.EventTrackingType)
            .NotEmpty().WithMessage("事件类型不能为空")
            .MaximumLength(40).WithMessage("事件类型长度不能超过40个字符");
        RuleFor(x => x.EventTrackingCategory)
            .NotEmpty().WithMessage("事件分类不能为空")
            .MaximumLength(40).WithMessage("事件分类长度不能超过40个字符");
        RuleFor(x => x.EntryName)
            .MaximumLength(40).WithMessage("PerformanceEntry.name长度不能超过40个字符");
        RuleFor(x => x.RoutePath)
            .MaximumLength(500).WithMessage("SPA 路由路径长度不能超过500个字符");
        RuleFor(x => x.PageUrl)
            .MaximumLength(500).WithMessage("页面完整 URL长度不能超过500个字符");
        RuleFor(x => x.ContainerType)
            .MaximumLength(40).WithMessage("TaskAttribution.containerType长度不能超过40个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.ContainerType));
        RuleFor(x => x.ContainerName)
            .MaximumLength(200).WithMessage("TaskAttribution.containerName长度不能超过200个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.ContainerName));
        RuleFor(x => x.ContainerSrc)
            .MaximumLength(500).WithMessage("TaskAttribution.containerSrc长度不能超过500个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.ContainerSrc));
        RuleFor(x => x.ContainerId)
            .MaximumLength(40).WithMessage("TaskAttribution.containerId长度不能超过40个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.ContainerId));
        RuleFor(x => x.UserAgent)
            .MaximumLength(500).WithMessage("用户代理长度不能超过500个字符");
        RuleFor(x => x.DurationMs)
            .GreaterThanOrEqualTo(0).WithMessage("阻塞时长不能为负数");
        RuleFor(x => x.PerformanceStartMs)
            .GreaterThanOrEqualTo(0).WithMessage("Performance开始毫秒不能为负数");
        RuleFor(x => x.TrackingLevel)
            .InclusiveBetween(0, 2).WithMessage("追踪级别无效");
    }
}

/// <summary>
/// Long Task 批量上报 DTO 验证器
/// </summary>
public class TaktTrackingLogBatchTrackValidator : AbstractValidator<TaktTrackingLogBatchTrackDto>
{
    private const int MaxBatchSize = 50;

    /// <summary>
    /// 初始化批量上报校验规则
    /// </summary>
    public TaktTrackingLogBatchTrackValidator()
    {
        RuleFor(x => x.Items)
            .NotNull().WithMessage("上报条目不能为空")
            .Must(items => items != null && items.Count <= MaxBatchSize)
            .WithMessage($"单次最多上报{MaxBatchSize}条");
        RuleForEach(x => x.Items)
            .SetValidator(new TaktTrackingLogTrackItemValidator());
    }
}
