// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Routine.Announcement
// 文件名称：TaktAnnouncementValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：Announcement 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktAnnouncement 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.Announcement;

namespace Takt.Application.Validators.Routine.Announcement;

// ========================================
// 创建Announcement 验证器
// ========================================

/// <summary>
/// 创建Announcement DTO 验证器
/// </summary>
public class TaktAnnouncementCreateValidator : AbstractValidator<TaktAnnouncementCreateDto>
{
    /// <summary>
    /// 初始化 创建Announcement 校验规则
    /// </summary>
    public TaktAnnouncementCreateValidator()
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
        RuleFor(x => x.AnnouncementCode)
            .NotEmpty().WithMessage("公告编码不能为空")
            .MaximumLength(50).WithMessage("公告编码长度不能超过50个字符");
        RuleFor(x => x.AnnouncementTitle)
            .NotEmpty().WithMessage("公告标题不能为空")
            .MaximumLength(200).WithMessage("公告标题长度不能超过200个字符");
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("公告内容不能为空");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Announcement 验证器
// ========================================

/// <summary>
/// 更新Announcement DTO 验证器
/// </summary>
public class TaktAnnouncementUpdateValidator : AbstractValidator<TaktAnnouncementUpdateDto>
{
    /// <summary>
    /// 初始化 更新Announcement 校验规则
    /// </summary>
    public TaktAnnouncementUpdateValidator()
    {
        RuleFor(x => x.AnnouncementId)
            .GreaterThan(0).WithMessage("AnnouncementID无效");
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
        RuleFor(x => x.AnnouncementCode)
            .NotEmpty().WithMessage("公告编码不能为空")
            .MaximumLength(50).WithMessage("公告编码长度不能超过50个字符");
        RuleFor(x => x.AnnouncementTitle)
            .NotEmpty().WithMessage("公告标题不能为空")
            .MaximumLength(200).WithMessage("公告标题长度不能超过200个字符");
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("公告内容不能为空");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Announcement 验证器
// ========================================

/// <summary>
/// 导入Announcement DTO 验证器
/// </summary>
public class TaktAnnouncementImportValidator : AbstractValidator<TaktAnnouncementImportDto>
{
    /// <summary>
    /// 初始化 导入Announcement 校验规则
    /// </summary>
    public TaktAnnouncementImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.AnnouncementCode)
            .NotEmpty().WithMessage("公告编码不能为空")
            .MaximumLength(50).WithMessage("公告编码长度不能超过50个字符");
        RuleFor(x => x.AnnouncementTitle)
            .NotEmpty().WithMessage("公告标题不能为空")
            .MaximumLength(200).WithMessage("公告标题长度不能超过200个字符");
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("公告内容不能为空");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
