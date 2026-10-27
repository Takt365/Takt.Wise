// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktMessageValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：Message 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMessage 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建Message 验证器
// ========================================

/// <summary>
/// 创建Message DTO 验证器
/// </summary>
public class TaktMessageCreateValidator : AbstractValidator<TaktMessageCreateDto>
{
    /// <summary>
    /// 初始化 创建Message 校验规则
    /// </summary>
    public TaktMessageCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ToUserId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ToUserId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.FromUserId)
            .GreaterThanOrEqualTo(0).WithMessage("发送者用户不能为负数");
        RuleFor(x => x.FromUserName)
            .NotEmpty().WithMessage("发送者用户名不能为空").When(x => x.FromUserId <= 0)
            .MaximumLength(20).WithMessage("发送者用户名长度不能超过20个字符");
        RuleFor(x => x.ToUserId)
            .GreaterThanOrEqualTo(0).WithMessage("接收者用户不能为负数");
        RuleFor(x => x.ToUserName)
            .NotEmpty().WithMessage("接收者用户名不能为空").When(x => x.ToUserId <= 0)
            .MaximumLength(20).WithMessage("接收者用户名长度不能超过20个字符");
        RuleFor(x => x.MessageTitle)
            .NotEmpty().WithMessage("消息标题不能为空")
            .MaximumLength(200).WithMessage("消息标题长度不能超过200个字符");
        RuleFor(x => x.MessageContent)
            .NotEmpty().WithMessage("消息内容不能为空");
        RuleFor(x => x.MessageType)
            .NotEmpty().WithMessage("消息类型不能为空")
            .MaximumLength(40).WithMessage("消息类型长度不能超过40个字符");
        RuleFor(x => x.MessageGroup)
            .NotEmpty().WithMessage("消息分组不能为空")
            .MaximumLength(40).WithMessage("消息分组长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Message 验证器
// ========================================

/// <summary>
/// 更新Message DTO 验证器
/// </summary>
public class TaktMessageUpdateValidator : AbstractValidator<TaktMessageUpdateDto>
{
    /// <summary>
    /// 初始化 更新Message 校验规则
    /// </summary>
    public TaktMessageUpdateValidator()
    {
        RuleFor(x => x.MessageId)
            .GreaterThan(0).WithMessage("MessageID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ToUserId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ToUserId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.FromUserId)
            .GreaterThanOrEqualTo(0).WithMessage("发送者用户不能为负数");
        RuleFor(x => x.FromUserName)
            .NotEmpty().WithMessage("发送者用户名不能为空").When(x => x.FromUserId <= 0)
            .MaximumLength(20).WithMessage("发送者用户名长度不能超过20个字符");
        RuleFor(x => x.ToUserId)
            .GreaterThanOrEqualTo(0).WithMessage("接收者用户不能为负数");
        RuleFor(x => x.ToUserName)
            .NotEmpty().WithMessage("接收者用户名不能为空").When(x => x.ToUserId <= 0)
            .MaximumLength(20).WithMessage("接收者用户名长度不能超过20个字符");
        RuleFor(x => x.MessageTitle)
            .NotEmpty().WithMessage("消息标题不能为空")
            .MaximumLength(200).WithMessage("消息标题长度不能超过200个字符");
        RuleFor(x => x.MessageContent)
            .NotEmpty().WithMessage("消息内容不能为空");
        RuleFor(x => x.MessageType)
            .NotEmpty().WithMessage("消息类型不能为空")
            .MaximumLength(40).WithMessage("消息类型长度不能超过40个字符");
        RuleFor(x => x.MessageGroup)
            .NotEmpty().WithMessage("消息分组不能为空")
            .MaximumLength(40).WithMessage("消息分组长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Message 验证器
// ========================================

/// <summary>
/// 导入Message DTO 验证器
/// </summary>
public class TaktMessageImportValidator : AbstractValidator<TaktMessageImportDto>
{
    /// <summary>
    /// 初始化 导入Message 校验规则
    /// </summary>
    public TaktMessageImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.FromUserId)
            .GreaterThanOrEqualTo(0).WithMessage("发送者用户不能为负数");
        RuleFor(x => x.FromUserName)
            .NotEmpty().WithMessage("发送者用户名不能为空")
            .MaximumLength(20).WithMessage("发送者用户名长度不能超过20个字符");
        RuleFor(x => x.ToUserId)
            .GreaterThanOrEqualTo(0).WithMessage("接收者用户不能为负数");
        RuleFor(x => x.ToUserName)
            .NotEmpty().WithMessage("接收者用户名不能为空")
            .MaximumLength(20).WithMessage("接收者用户名长度不能超过20个字符");
        RuleFor(x => x.MessageTitle)
            .NotEmpty().WithMessage("消息标题不能为空")
            .MaximumLength(200).WithMessage("消息标题长度不能超过200个字符");
        RuleFor(x => x.MessageContent)
            .NotEmpty().WithMessage("消息内容不能为空");
        RuleFor(x => x.MessageType)
            .NotEmpty().WithMessage("消息类型不能为空")
            .MaximumLength(40).WithMessage("消息类型长度不能超过40个字符");
        RuleFor(x => x.MessageGroup)
            .NotEmpty().WithMessage("消息分组不能为空")
            .MaximumLength(40).WithMessage("消息分组长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
