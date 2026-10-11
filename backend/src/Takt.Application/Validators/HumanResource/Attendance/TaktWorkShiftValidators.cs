// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Attendance
// 文件名称：TaktWorkShiftValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：WorkShift 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktWorkShift 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Attendance;

namespace Takt.Application.Validators.HumanResource.Attendance;

// ========================================
// 创建WorkShift 验证器
// ========================================

/// <summary>
/// 创建WorkShift DTO 验证器
/// </summary>
public class TaktWorkShiftCreateValidator : AbstractValidator<TaktWorkShiftCreateDto>
{
    /// <summary>
    /// 初始化 创建WorkShift 校验规则
    /// </summary>
    public TaktWorkShiftCreateValidator()
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
        RuleFor(x => x.ShiftCode)
            .NotEmpty().WithMessage("班次编码不能为空")
            .MaximumLength(64).WithMessage("班次编码长度不能超过64个字符");
        RuleFor(x => x.ShiftName)
            .NotEmpty().WithMessage("班次名称不能为空")
            .MaximumLength(128).WithMessage("班次名称长度不能超过128个字符");
        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("当班开始时间不能为空")
            .MaximumLength(8).WithMessage("当班开始时间长度不能超过8个字符");
        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("当班结束时间不能为空")
            .MaximumLength(8).WithMessage("当班结束时间长度不能超过8个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新WorkShift 验证器
// ========================================

/// <summary>
/// 更新WorkShift DTO 验证器
/// </summary>
public class TaktWorkShiftUpdateValidator : AbstractValidator<TaktWorkShiftUpdateDto>
{
    /// <summary>
    /// 初始化 更新WorkShift 校验规则
    /// </summary>
    public TaktWorkShiftUpdateValidator()
    {
        RuleFor(x => x.WorkShiftId)
            .GreaterThan(0).WithMessage("WorkShiftID无效");
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
        RuleFor(x => x.ShiftCode)
            .NotEmpty().WithMessage("班次编码不能为空")
            .MaximumLength(64).WithMessage("班次编码长度不能超过64个字符");
        RuleFor(x => x.ShiftName)
            .NotEmpty().WithMessage("班次名称不能为空")
            .MaximumLength(128).WithMessage("班次名称长度不能超过128个字符");
        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("当班开始时间不能为空")
            .MaximumLength(8).WithMessage("当班开始时间长度不能超过8个字符");
        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("当班结束时间不能为空")
            .MaximumLength(8).WithMessage("当班结束时间长度不能超过8个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入WorkShift 验证器
// ========================================

/// <summary>
/// 导入WorkShift DTO 验证器
/// </summary>
public class TaktWorkShiftImportValidator : AbstractValidator<TaktWorkShiftImportDto>
{
    /// <summary>
    /// 初始化 导入WorkShift 校验规则
    /// </summary>
    public TaktWorkShiftImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ShiftCode)
            .NotEmpty().WithMessage("班次编码不能为空")
            .MaximumLength(64).WithMessage("班次编码长度不能超过64个字符");
        RuleFor(x => x.ShiftName)
            .NotEmpty().WithMessage("班次名称不能为空")
            .MaximumLength(128).WithMessage("班次名称长度不能超过128个字符");
        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("当班开始时间不能为空")
            .MaximumLength(8).WithMessage("当班开始时间长度不能超过8个字符");
        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("当班结束时间不能为空")
            .MaximumLength(8).WithMessage("当班结束时间长度不能超过8个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
