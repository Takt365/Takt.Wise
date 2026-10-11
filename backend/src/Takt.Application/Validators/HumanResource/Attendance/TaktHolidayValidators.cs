// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Attendance
// 文件名称：TaktHolidayValidators.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Auto Generated)
// 功能描述：Holiday 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktHoliday 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Attendance;

namespace Takt.Application.Validators.HumanResource.Attendance;

// ========================================
// 创建Holiday 验证器
// ========================================

/// <summary>
/// 创建Holiday DTO 验证器
/// </summary>
public class TaktHolidayCreateValidator : AbstractValidator<TaktHolidayCreateDto>
{
    /// <summary>
    /// 初始化 创建Holiday 校验规则
    /// </summary>
    public TaktHolidayCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.HolidayName)
            .NotEmpty().WithMessage("假日名称不能为空")
            .MaximumLength(100).WithMessage("假日名称长度不能超过100个字符");
        RuleFor(x => x.HolidayGreeting)
            .NotEmpty().WithMessage("假日问候语不能为空")
            .MaximumLength(200).WithMessage("假日问候语长度不能超过200个字符");
        RuleFor(x => x.HolidayQuote)
            .NotEmpty().WithMessage("假日引用/诗句不能为空")
            .MaximumLength(500).WithMessage("假日引用/诗句长度不能超过500个字符");
        RuleFor(x => x.HolidayTheme)
            .NotEmpty().WithMessage("假日主题不能为空")
            .MaximumLength(20).WithMessage("假日主题长度不能超过20个字符");
        RuleFor(x => x.CompensatoryWorkDates)
            .MaximumLength(200).WithMessage("调休上班日长度不能超过200个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");

        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Holiday 验证器
// ========================================

/// <summary>
/// 更新Holiday DTO 验证器
/// </summary>
public class TaktHolidayUpdateValidator : AbstractValidator<TaktHolidayUpdateDto>
{
    /// <summary>
    /// 初始化 更新Holiday 校验规则
    /// </summary>
    public TaktHolidayUpdateValidator()
    {
        RuleFor(x => x.HolidayId)
            .GreaterThan(0).WithMessage("HolidayID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.HolidayName)
            .NotEmpty().WithMessage("假日名称不能为空")
            .MaximumLength(100).WithMessage("假日名称长度不能超过100个字符");
        RuleFor(x => x.HolidayGreeting)
            .NotEmpty().WithMessage("假日问候语不能为空")
            .MaximumLength(200).WithMessage("假日问候语长度不能超过200个字符");
        RuleFor(x => x.HolidayQuote)
            .NotEmpty().WithMessage("假日引用/诗句不能为空")
            .MaximumLength(500).WithMessage("假日引用/诗句长度不能超过500个字符");
        RuleFor(x => x.HolidayTheme)
            .NotEmpty().WithMessage("假日主题不能为空")
            .MaximumLength(20).WithMessage("假日主题长度不能超过20个字符");
        RuleFor(x => x.CompensatoryWorkDates)
            .MaximumLength(200).WithMessage("调休上班日长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Holiday 验证器
// ========================================

/// <summary>
/// 导入Holiday DTO 验证器
/// </summary>
public class TaktHolidayImportValidator : AbstractValidator<TaktHolidayImportDto>
{
    /// <summary>
    /// 初始化 导入Holiday 校验规则
    /// </summary>
    public TaktHolidayImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.HolidayName)
            .NotEmpty().WithMessage("假日名称不能为空")
            .MaximumLength(100).WithMessage("假日名称长度不能超过100个字符");
        RuleFor(x => x.HolidayGreeting)
            .NotEmpty().WithMessage("假日问候语不能为空")
            .MaximumLength(200).WithMessage("假日问候语长度不能超过200个字符");
        RuleFor(x => x.HolidayQuote)
            .NotEmpty().WithMessage("假日引用/诗句不能为空")
            .MaximumLength(500).WithMessage("假日引用/诗句长度不能超过500个字符");
        RuleFor(x => x.HolidayTheme)
            .NotEmpty().WithMessage("假日主题不能为空")
            .MaximumLength(20).WithMessage("假日主题长度不能超过20个字符");
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.PlantCode));

        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
