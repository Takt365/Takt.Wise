// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Sop
// 文件名称：TaktSopCallValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：SopCall 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSopCall 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;

namespace Takt.Application.Validators.Logistics.Manufacturing.Sop;

// ========================================
// 创建SopCall 验证器
// ========================================

/// <summary>
/// 创建SopCall DTO 验证器
/// </summary>
public class TaktSopCallCreateValidator : AbstractValidator<TaktSopCallCreateDto>
{
    /// <summary>
    /// 初始化 创建SopCall 校验规则
    /// </summary>
    public TaktSopCallCreateValidator()
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
        RuleFor(x => x.WorkstationId)
            .GreaterThanOrEqualTo(0).WithMessage("工位 ID不能为负数");
        RuleFor(x => x.ExecId)
            .GreaterThanOrEqualTo(0).WithMessage("执行追溯 ID不能为负数");
        RuleFor(x => x.CallerId)
            .GreaterThanOrEqualTo(0).WithMessage("呼叫人 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SopCall 验证器
// ========================================

/// <summary>
/// 更新SopCall DTO 验证器
/// </summary>
public class TaktSopCallUpdateValidator : AbstractValidator<TaktSopCallUpdateDto>
{
    /// <summary>
    /// 初始化 更新SopCall 校验规则
    /// </summary>
    public TaktSopCallUpdateValidator()
    {
        RuleFor(x => x.SopCallId)
            .GreaterThan(0).WithMessage("SopCallID无效");
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
        RuleFor(x => x.WorkstationId)
            .GreaterThanOrEqualTo(0).WithMessage("工位 ID不能为负数");
        RuleFor(x => x.ExecId)
            .GreaterThanOrEqualTo(0).WithMessage("执行追溯 ID不能为负数");
        RuleFor(x => x.CallerId)
            .GreaterThanOrEqualTo(0).WithMessage("呼叫人 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SopCall 验证器
// ========================================

/// <summary>
/// 导入SopCall DTO 验证器
/// </summary>
public class TaktSopCallImportValidator : AbstractValidator<TaktSopCallImportDto>
{
    /// <summary>
    /// 初始化 导入SopCall 校验规则
    /// </summary>
    public TaktSopCallImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.WorkstationId)
            .GreaterThanOrEqualTo(0).WithMessage("工位 ID不能为负数");
        RuleFor(x => x.ExecId)
            .GreaterThanOrEqualTo(0).WithMessage("执行追溯 ID不能为负数");
        RuleFor(x => x.CallerId)
            .GreaterThanOrEqualTo(0).WithMessage("呼叫人 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
