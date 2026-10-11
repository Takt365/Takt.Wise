// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Routine.HelpDesk
// 文件名称：TaktTicketWorkflowValidators.cs
// 创建时间：2026-06-10
// 创建人：Takt365(Cursor AI)
// 功能描述：工单工作流 FluentValidation 验证器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.HelpDesk;

namespace Takt.Application.Validators.Routine.HelpDesk;

/// <summary>
/// 用户提交工单验证器
/// </summary>
public class TaktTicketSubmitValidator : AbstractValidator<TaktTicketSubmitDto>
{
    /// <summary>
    /// 初始化校验规则
    /// </summary>
    public TaktTicketSubmitValidator()
    {
        RuleFor(x => x.TicketTitle)
            .NotEmpty().WithMessage("工单标题不能为空")
            .MaximumLength(200).WithMessage("工单标题长度不能超过200个字符");
        RuleFor(x => x.CategoryCode)
            .MaximumLength(40).WithMessage("分类编码长度不能超过40个字符");
        RuleFor(x => x.AssetCode)
            .MaximumLength(40).WithMessage("资产号码长度不能超过40个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.AssetCode));
        RuleFor(x => x.ItAssetId)
            .GreaterThanOrEqualTo(0).WithMessage("IT设备保修扩展ID不能为负数")
            .When(x => x.ItAssetId.HasValue);
        RuleFor(x => x.Urgency)
            .InclusiveBetween(1, 3).WithMessage("紧急度须为1～3");
        RuleFor(x => x.Impact)
            .InclusiveBetween(1, 3).WithMessage("影响范围须为1～3");
    }
}

/// <summary>
/// 渠道建单验证器
/// </summary>
public class TaktTicketCreateFromChannelValidator : AbstractValidator<TaktTicketCreateFromChannelDto>
{
    /// <summary>
    /// 初始化校验规则
    /// </summary>
    public TaktTicketCreateFromChannelValidator()
    {
        RuleFor(x => x.TicketTitle)
            .NotEmpty().WithMessage("工单标题不能为空")
            .MaximumLength(200).WithMessage("工单标题长度不能超过200个字符");
        RuleFor(x => x.CategoryCode)
            .MaximumLength(40).WithMessage("分类编码长度不能超过40个字符");
    }
}

/// <summary>
/// 指派工单验证器
/// </summary>
public class TaktTicketAssignValidator : AbstractValidator<TaktTicketAssignDto>
{
    /// <summary>
    /// 初始化校验规则
    /// </summary>
    public TaktTicketAssignValidator()
    {
        RuleFor(x => x.TicketId)
            .GreaterThan(0).WithMessage("工单ID无效");
    }
}

/// <summary>
/// 工作流动作验证器
/// </summary>
public class TaktTicketWorkflowActionValidator : AbstractValidator<TaktTicketWorkflowActionDto>
{
    /// <summary>
    /// 初始化校验规则
    /// </summary>
    public TaktTicketWorkflowActionValidator()
    {
        RuleFor(x => x.TicketId)
            .GreaterThan(0).WithMessage("工单ID无效");
    }
}



/// <summary>
/// 工单回复查询验证器
/// </summary>
public class TaktTicketReplyQueryValidator : AbstractValidator<TaktTicketReplyQueryDto>
{
    /// <summary>
    /// 初始化校验规则
    /// </summary>
    public TaktTicketReplyQueryValidator()
    {
        RuleFor(x => x.TicketId)
            .Must(id => id.HasValue && id.Value > 0)
            .WithMessage("工单ID无效");
    }
}

/// <summary>
/// 工单会话回复验证器（工作流入口）
/// </summary>
public class TaktTicketSessionReplyCreateValidator : AbstractValidator<TaktTicketSessionReplyCreateDto>
{
    /// <summary>
    /// 初始化校验规则
    /// </summary>
    public TaktTicketSessionReplyCreateValidator()
    {
        RuleFor(x => x.TicketId)
            .GreaterThan(0).WithMessage("工单ID无效");
        RuleFor(x => x.TicketContent)
            .NotEmpty().WithMessage("回复内容不能为空")
            .MaximumLength(4000).WithMessage("回复内容长度不能超过4000个字符");
        RuleFor(x => x.IsInternal)
            .InclusiveBetween(0, 1).WithMessage("内部备注标识须为0或1");
    }
}