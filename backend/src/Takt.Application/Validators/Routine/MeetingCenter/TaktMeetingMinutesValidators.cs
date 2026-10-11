// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Routine.MeetingCenter
// 文件名称：TaktMeetingMinutesValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：MeetingMinutes 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMeetingMinutes 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.MeetingCenter;

namespace Takt.Application.Validators.Routine.MeetingCenter;

// ========================================
// 创建MeetingMinutes 验证器
// ========================================

/// <summary>
/// 创建MeetingMinutes DTO 验证器
/// </summary>
public class TaktMeetingMinutesCreateValidator : AbstractValidator<TaktMeetingMinutesCreateDto>
{
    /// <summary>
    /// 初始化 创建MeetingMinutes 校验规则
    /// </summary>
    public TaktMeetingMinutesCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.RecorderId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.RecorderId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.MeetingId)
            .GreaterThanOrEqualTo(0).WithMessage("会议 ID不能为负数");
        RuleFor(x => x.RecorderId)
            .GreaterThanOrEqualTo(0).WithMessage("记录 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MeetingMinutes 验证器
// ========================================

/// <summary>
/// 更新MeetingMinutes DTO 验证器
/// </summary>
public class TaktMeetingMinutesUpdateValidator : AbstractValidator<TaktMeetingMinutesUpdateDto>
{
    /// <summary>
    /// 初始化 更新MeetingMinutes 校验规则
    /// </summary>
    public TaktMeetingMinutesUpdateValidator()
    {
        RuleFor(x => x.MeetingMinutesId)
            .GreaterThan(0).WithMessage("MeetingMinutesID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.RecorderId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.RecorderId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.MeetingId)
            .GreaterThanOrEqualTo(0).WithMessage("会议 ID不能为负数");
        RuleFor(x => x.RecorderId)
            .GreaterThanOrEqualTo(0).WithMessage("记录 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入MeetingMinutes 验证器
// ========================================

/// <summary>
/// 导入MeetingMinutes DTO 验证器
/// </summary>
public class TaktMeetingMinutesImportValidator : AbstractValidator<TaktMeetingMinutesImportDto>
{
    /// <summary>
    /// 初始化 导入MeetingMinutes 校验规则
    /// </summary>
    public TaktMeetingMinutesImportValidator()
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
        RuleFor(x => x.RecorderId)
            .GreaterThanOrEqualTo(0).WithMessage("记录 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
