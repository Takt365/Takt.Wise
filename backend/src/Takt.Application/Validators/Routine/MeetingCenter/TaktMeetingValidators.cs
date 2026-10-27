// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Routine.MeetingCenter
// 文件名称：TaktMeetingValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：Meeting 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMeeting 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.MeetingCenter;

namespace Takt.Application.Validators.Routine.MeetingCenter;

// ========================================
// 创建Meeting 验证器
// ========================================

/// <summary>
/// 创建Meeting DTO 验证器
/// </summary>
public class TaktMeetingCreateValidator : AbstractValidator<TaktMeetingCreateDto>
{
    /// <summary>
    /// 初始化 创建Meeting 校验规则
    /// </summary>
    public TaktMeetingCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.MeetingCode)
            .NotEmpty().WithMessage("会议编码不能为空")
            .MaximumLength(50).WithMessage("会议编码长度不能超过50个字符");
        RuleFor(x => x.MeetingTitle)
            .NotEmpty().WithMessage("会议标题不能为空")
            .MaximumLength(200).WithMessage("会议标题长度不能超过200个字符");
        RuleFor(x => x.OrganizerId)
            .GreaterThanOrEqualTo(0).WithMessage("组织人 ID不能为负数");
        RuleFor(x => x.OrganizerName)
            .NotEmpty().WithMessage("组织人姓名不能为空").When(x => x.OrganizerId <= 0)
            .MaximumLength(20).WithMessage("组织人姓名长度不能超过20个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("主办部门 ID不能为负数");
        RuleFor(x => x.MeetingRoomId)
            .GreaterThanOrEqualTo(0).WithMessage("会议室 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Meeting 验证器
// ========================================

/// <summary>
/// 更新Meeting DTO 验证器
/// </summary>
public class TaktMeetingUpdateValidator : AbstractValidator<TaktMeetingUpdateDto>
{
    /// <summary>
    /// 初始化 更新Meeting 校验规则
    /// </summary>
    public TaktMeetingUpdateValidator()
    {
        RuleFor(x => x.MeetingId)
            .GreaterThan(0).WithMessage("MeetingID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.MeetingCode)
            .NotEmpty().WithMessage("会议编码不能为空")
            .MaximumLength(50).WithMessage("会议编码长度不能超过50个字符");
        RuleFor(x => x.MeetingTitle)
            .NotEmpty().WithMessage("会议标题不能为空")
            .MaximumLength(200).WithMessage("会议标题长度不能超过200个字符");
        RuleFor(x => x.OrganizerId)
            .GreaterThanOrEqualTo(0).WithMessage("组织人 ID不能为负数");
        RuleFor(x => x.OrganizerName)
            .NotEmpty().WithMessage("组织人姓名不能为空").When(x => x.OrganizerId <= 0)
            .MaximumLength(20).WithMessage("组织人姓名长度不能超过20个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("主办部门 ID不能为负数");
        RuleFor(x => x.MeetingRoomId)
            .GreaterThanOrEqualTo(0).WithMessage("会议室 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Meeting 验证器
// ========================================

/// <summary>
/// 导入Meeting DTO 验证器
/// </summary>
public class TaktMeetingImportValidator : AbstractValidator<TaktMeetingImportDto>
{
    /// <summary>
    /// 初始化 导入Meeting 校验规则
    /// </summary>
    public TaktMeetingImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.MeetingCode)
            .NotEmpty().WithMessage("会议编码不能为空")
            .MaximumLength(50).WithMessage("会议编码长度不能超过50个字符");
        RuleFor(x => x.MeetingTitle)
            .NotEmpty().WithMessage("会议标题不能为空")
            .MaximumLength(200).WithMessage("会议标题长度不能超过200个字符");
        RuleFor(x => x.OrganizerId)
            .GreaterThanOrEqualTo(0).WithMessage("组织人 ID不能为负数");
        RuleFor(x => x.OrganizerName)
            .NotEmpty().WithMessage("组织人姓名不能为空")
            .MaximumLength(20).WithMessage("组织人姓名长度不能超过20个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("主办部门 ID不能为负数");
        RuleFor(x => x.MeetingRoomId)
            .GreaterThanOrEqualTo(0).WithMessage("会议室 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
