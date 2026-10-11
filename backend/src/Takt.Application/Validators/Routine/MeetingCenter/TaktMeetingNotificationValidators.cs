// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Routine.MeetingCenter
// 文件名称：TaktMeetingNotificationValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：MeetingNotification 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMeetingNotification 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.MeetingCenter;

namespace Takt.Application.Validators.Routine.MeetingCenter;

// ========================================
// 创建MeetingNotification 验证器
// ========================================

/// <summary>
/// 创建MeetingNotification DTO 验证器
/// </summary>
public class TaktMeetingNotificationCreateValidator : AbstractValidator<TaktMeetingNotificationCreateDto>
{
    /// <summary>
    /// 初始化 创建MeetingNotification 校验规则
    /// </summary>
    public TaktMeetingNotificationCreateValidator()
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
        RuleFor(x => x.MeetingId)
            .GreaterThanOrEqualTo(0).WithMessage("会议 ID不能为负数");
        RuleFor(x => x.MeetingAttendeeId)
            .GreaterThanOrEqualTo(0).WithMessage("参会人员 ID不能为负数");
        RuleFor(x => x.MeetingTitle)
            .NotEmpty().WithMessage("会议标题不能为空")
            .MaximumLength(200).WithMessage("会议标题长度不能超过200个字符");
        RuleFor(x => x.MeetingCode)
            .NotEmpty().WithMessage("会议编码不能为空").When(x => x.MeetingId <= 0)
            .MaximumLength(50).WithMessage("会议编码长度不能超过50个字符");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户姓名不能为空").When(x => x.UserId <= 0)
            .MaximumLength(40).WithMessage("用户姓名长度不能超过40个字符");
        RuleFor(x => x.RecipientEmail)
            .NotEmpty().WithMessage("收件邮箱不能为空")
            .MaximumLength(100).WithMessage("收件邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("收件邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.RecipientEmail));
        RuleFor(x => x.NotificationSubject)
            .NotEmpty().WithMessage("邮件主题不能为空")
            .MaximumLength(200).WithMessage("邮件主题长度不能超过200个字符");
        RuleFor(x => x.ConfirmReceiptToken)
            .NotEmpty().WithMessage("回执确认令牌不能为空")
            .MaximumLength(64).WithMessage("回执确认令牌长度不能超过64个字符");
        RuleFor(x => x.ConfirmedByUserId)
            .GreaterThanOrEqualTo(0).WithMessage("确认人用户 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MeetingNotification 验证器
// ========================================

/// <summary>
/// 更新MeetingNotification DTO 验证器
/// </summary>
public class TaktMeetingNotificationUpdateValidator : AbstractValidator<TaktMeetingNotificationUpdateDto>
{
    /// <summary>
    /// 初始化 更新MeetingNotification 校验规则
    /// </summary>
    public TaktMeetingNotificationUpdateValidator()
    {
        RuleFor(x => x.MeetingNotificationId)
            .GreaterThan(0).WithMessage("MeetingNotificationID无效");
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
        RuleFor(x => x.MeetingId)
            .GreaterThanOrEqualTo(0).WithMessage("会议 ID不能为负数");
        RuleFor(x => x.MeetingAttendeeId)
            .GreaterThanOrEqualTo(0).WithMessage("参会人员 ID不能为负数");
        RuleFor(x => x.MeetingTitle)
            .NotEmpty().WithMessage("会议标题不能为空")
            .MaximumLength(200).WithMessage("会议标题长度不能超过200个字符");
        RuleFor(x => x.MeetingCode)
            .NotEmpty().WithMessage("会议编码不能为空").When(x => x.MeetingId <= 0)
            .MaximumLength(50).WithMessage("会议编码长度不能超过50个字符");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户姓名不能为空").When(x => x.UserId <= 0)
            .MaximumLength(40).WithMessage("用户姓名长度不能超过40个字符");
        RuleFor(x => x.RecipientEmail)
            .NotEmpty().WithMessage("收件邮箱不能为空")
            .MaximumLength(100).WithMessage("收件邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("收件邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.RecipientEmail));
        RuleFor(x => x.NotificationSubject)
            .NotEmpty().WithMessage("邮件主题不能为空")
            .MaximumLength(200).WithMessage("邮件主题长度不能超过200个字符");
        RuleFor(x => x.ConfirmReceiptToken)
            .NotEmpty().WithMessage("回执确认令牌不能为空")
            .MaximumLength(64).WithMessage("回执确认令牌长度不能超过64个字符");
        RuleFor(x => x.ConfirmedByUserId)
            .GreaterThanOrEqualTo(0).WithMessage("确认人用户 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入MeetingNotification 验证器
// ========================================

/// <summary>
/// 导入MeetingNotification DTO 验证器
/// </summary>
public class TaktMeetingNotificationImportValidator : AbstractValidator<TaktMeetingNotificationImportDto>
{
    /// <summary>
    /// 初始化 导入MeetingNotification 校验规则
    /// </summary>
    public TaktMeetingNotificationImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.MeetingId)
            .GreaterThanOrEqualTo(0).WithMessage("会议 ID不能为负数");
        RuleFor(x => x.MeetingAttendeeId)
            .GreaterThanOrEqualTo(0).WithMessage("参会人员 ID不能为负数");
        RuleFor(x => x.MeetingTitle)
            .NotEmpty().WithMessage("会议标题不能为空")
            .MaximumLength(200).WithMessage("会议标题长度不能超过200个字符");
        RuleFor(x => x.MeetingCode)
            .NotEmpty().WithMessage("会议编码不能为空")
            .MaximumLength(50).WithMessage("会议编码长度不能超过50个字符");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("用户 ID不能为负数");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("用户姓名不能为空")
            .MaximumLength(40).WithMessage("用户姓名长度不能超过40个字符");
        RuleFor(x => x.RecipientEmail)
            .NotEmpty().WithMessage("收件邮箱不能为空")
            .MaximumLength(100).WithMessage("收件邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("收件邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.RecipientEmail));
        RuleFor(x => x.NotificationSubject)
            .NotEmpty().WithMessage("邮件主题不能为空")
            .MaximumLength(200).WithMessage("邮件主题长度不能超过200个字符");
        RuleFor(x => x.ConfirmReceiptToken)
            .NotEmpty().WithMessage("回执确认令牌不能为空")
            .MaximumLength(64).WithMessage("回执确认令牌长度不能超过64个字符");
        RuleFor(x => x.ConfirmedByUserId)
            .GreaterThanOrEqualTo(0).WithMessage("确认人用户 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
