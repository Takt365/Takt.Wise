// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Statistics.Logging
// 文件名称：TaktArchiveLogValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：ArchiveLog 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktArchiveLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Statistics.Logging;

namespace Takt.Application.Validators.Statistics.Logging;

// ========================================
// 创建ArchiveLog 验证器
// ========================================

/// <summary>
/// 创建ArchiveLog DTO 验证器
/// </summary>
public class TaktArchiveLogCreateValidator : AbstractValidator<TaktArchiveLogCreateDto>
{
    /// <summary>
    /// 初始化 创建ArchiveLog 校验规则
    /// </summary>
    public TaktArchiveLogCreateValidator()
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
        RuleFor(x => x.ArchiveKind)
            .NotEmpty().WithMessage("归档种类不能为空")
            .MaximumLength(40).WithMessage("归档种类长度不能超过40个字符");
        RuleFor(x => x.SourceId)
            .NotEmpty().WithMessage("来源业务键不能为空")
            .MaximumLength(40).WithMessage("来源业务键长度不能超过40个字符");
        RuleFor(x => x.SourceName)
            .NotEmpty().WithMessage("来源名称不能为空")
            .MaximumLength(200).WithMessage("来源名称长度不能超过200个字符");
        RuleFor(x => x.TargetName)
            .NotEmpty().WithMessage("归档目标名称不能为空")
            .MaximumLength(200).WithMessage("归档目标名称长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ArchiveLog 验证器
// ========================================

/// <summary>
/// 更新ArchiveLog DTO 验证器
/// </summary>
public class TaktArchiveLogUpdateValidator : AbstractValidator<TaktArchiveLogUpdateDto>
{
    /// <summary>
    /// 初始化 更新ArchiveLog 校验规则
    /// </summary>
    public TaktArchiveLogUpdateValidator()
    {
        RuleFor(x => x.ArchiveLogId)
            .GreaterThan(0).WithMessage("ArchiveLogID无效");
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
        RuleFor(x => x.ArchiveKind)
            .NotEmpty().WithMessage("归档种类不能为空")
            .MaximumLength(40).WithMessage("归档种类长度不能超过40个字符");
        RuleFor(x => x.SourceId)
            .NotEmpty().WithMessage("来源业务键不能为空")
            .MaximumLength(40).WithMessage("来源业务键长度不能超过40个字符");
        RuleFor(x => x.SourceName)
            .NotEmpty().WithMessage("来源名称不能为空")
            .MaximumLength(200).WithMessage("来源名称长度不能超过200个字符");
        RuleFor(x => x.TargetName)
            .NotEmpty().WithMessage("归档目标名称不能为空")
            .MaximumLength(200).WithMessage("归档目标名称长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}
