// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Routine.HelpDesk
// 文件名称：TaktTicketValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：Ticket 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTicket 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.HelpDesk;

namespace Takt.Application.Validators.Routine.HelpDesk;

// ========================================
// 创建Ticket 验证器
// ========================================

/// <summary>
/// 创建Ticket DTO 验证器
/// </summary>
public class TaktTicketCreateValidator : AbstractValidator<TaktTicketCreateDto>
{
    /// <summary>
    /// 初始化 创建Ticket 校验规则
    /// </summary>
    public TaktTicketCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.AssigneeId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.AssigneeId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.TicketCode)
            .NotEmpty().WithMessage("工单编码不能为空")
            .MaximumLength(50).WithMessage("工单编码长度不能超过50个字符");
        RuleFor(x => x.TicketTitle)
            .NotEmpty().WithMessage("工单标题不能为空")
            .MaximumLength(200).WithMessage("工单标题长度不能超过200个字符");
        RuleFor(x => x.SubmitterId)
            .GreaterThanOrEqualTo(0).WithMessage("提交人 ID不能为负数");
        RuleFor(x => x.AssigneeId)
            .GreaterThanOrEqualTo(0).WithMessage("处理人 ID不能为负数");
        RuleFor(x => x.KnowledgeId)
            .GreaterThanOrEqualTo(0).WithMessage("关联知识 ID不能为负数");
        RuleFor(x => x.ParentTicketId)
            .GreaterThanOrEqualTo(0).WithMessage("父工单 ID不能为负数");
        RuleFor(x => x.ItAssetId)
            .GreaterThanOrEqualTo(0).WithMessage("IT 设备保修扩展 ID不能为负数");
        RuleFor(x => x.ApplicantDeptId)
            .GreaterThanOrEqualTo(0).WithMessage("申请部门 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Ticket 验证器
// ========================================

/// <summary>
/// 更新Ticket DTO 验证器
/// </summary>
public class TaktTicketUpdateValidator : AbstractValidator<TaktTicketUpdateDto>
{
    /// <summary>
    /// 初始化 更新Ticket 校验规则
    /// </summary>
    public TaktTicketUpdateValidator()
    {
        RuleFor(x => x.TicketId)
            .GreaterThan(0).WithMessage("TicketID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.AssigneeId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.AssigneeId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.TicketCode)
            .NotEmpty().WithMessage("工单编码不能为空")
            .MaximumLength(50).WithMessage("工单编码长度不能超过50个字符");
        RuleFor(x => x.TicketTitle)
            .NotEmpty().WithMessage("工单标题不能为空")
            .MaximumLength(200).WithMessage("工单标题长度不能超过200个字符");
        RuleFor(x => x.SubmitterId)
            .GreaterThanOrEqualTo(0).WithMessage("提交人 ID不能为负数");
        RuleFor(x => x.AssigneeId)
            .GreaterThanOrEqualTo(0).WithMessage("处理人 ID不能为负数");
        RuleFor(x => x.KnowledgeId)
            .GreaterThanOrEqualTo(0).WithMessage("关联知识 ID不能为负数");
        RuleFor(x => x.ParentTicketId)
            .GreaterThanOrEqualTo(0).WithMessage("父工单 ID不能为负数");
        RuleFor(x => x.ItAssetId)
            .GreaterThanOrEqualTo(0).WithMessage("IT 设备保修扩展 ID不能为负数");
        RuleFor(x => x.ApplicantDeptId)
            .GreaterThanOrEqualTo(0).WithMessage("申请部门 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Ticket 验证器
// ========================================

/// <summary>
/// 导入Ticket DTO 验证器
/// </summary>
public class TaktTicketImportValidator : AbstractValidator<TaktTicketImportDto>
{
    /// <summary>
    /// 初始化 导入Ticket 校验规则
    /// </summary>
    public TaktTicketImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.TicketCode)
            .NotEmpty().WithMessage("工单编码不能为空")
            .MaximumLength(50).WithMessage("工单编码长度不能超过50个字符");
        RuleFor(x => x.TicketTitle)
            .NotEmpty().WithMessage("工单标题不能为空")
            .MaximumLength(200).WithMessage("工单标题长度不能超过200个字符");
        RuleFor(x => x.SubmitterId)
            .GreaterThanOrEqualTo(0).WithMessage("提交人 ID不能为负数");
        RuleFor(x => x.AssigneeId)
            .GreaterThanOrEqualTo(0).WithMessage("处理人 ID不能为负数");
        RuleFor(x => x.KnowledgeId)
            .GreaterThanOrEqualTo(0).WithMessage("关联知识 ID不能为负数");
        RuleFor(x => x.ParentTicketId)
            .GreaterThanOrEqualTo(0).WithMessage("父工单 ID不能为负数");
        RuleFor(x => x.ItAssetId)
            .GreaterThanOrEqualTo(0).WithMessage("IT 设备保修扩展 ID不能为负数");
        RuleFor(x => x.ApplicantDeptId)
            .GreaterThanOrEqualTo(0).WithMessage("申请部门 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
