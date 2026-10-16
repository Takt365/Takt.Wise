// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Serial
// 文件名称：TaktSerialInboundItemValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：SerialInboundItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSerialInboundItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Serial;

namespace Takt.Application.Validators.Logistics.Serial;

// ========================================
// 创建SerialInboundItem 验证器
// ========================================

/// <summary>
/// 创建SerialInboundItem DTO 验证器
/// </summary>
public class TaktSerialInboundItemCreateValidator : AbstractValidator<TaktSerialInboundItemCreateDto>
{
    /// <summary>
    /// 初始化 创建SerialInboundItem 校验规则
    /// </summary>
    public TaktSerialInboundItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.InboundId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.InboundId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.InboundId)
            .GreaterThanOrEqualTo(0).WithMessage("入库主表 ID不能为负数");
        RuleFor(x => x.InboundCode)
            .NotEmpty().WithMessage("入库单号不能为空").When(x => x.InboundId <= 0)
            .MaximumLength(10).WithMessage("入库单号长度不能超过10个字符");
        RuleFor(x => x.InboundSerialCode)
            .NotEmpty().WithMessage("入库序列号不能为空")
            .MaximumLength(100).WithMessage("入库序列号长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SerialInboundItem 验证器
// ========================================

/// <summary>
/// 更新SerialInboundItem DTO 验证器
/// </summary>
public class TaktSerialInboundItemUpdateValidator : AbstractValidator<TaktSerialInboundItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新SerialInboundItem 校验规则
    /// </summary>
    public TaktSerialInboundItemUpdateValidator()
    {
        RuleFor(x => x.SerialInboundItemId)
            .GreaterThan(0).WithMessage("SerialInboundItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.InboundId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.InboundId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.InboundId)
            .GreaterThanOrEqualTo(0).WithMessage("入库主表 ID不能为负数");
        RuleFor(x => x.InboundCode)
            .NotEmpty().WithMessage("入库单号不能为空").When(x => x.InboundId <= 0)
            .MaximumLength(10).WithMessage("入库单号长度不能超过10个字符");
        RuleFor(x => x.InboundSerialCode)
            .NotEmpty().WithMessage("入库序列号不能为空")
            .MaximumLength(100).WithMessage("入库序列号长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SerialInboundItem 验证器
// ========================================

/// <summary>
/// 导入SerialInboundItem DTO 验证器
/// </summary>
public class TaktSerialInboundItemImportValidator : AbstractValidator<TaktSerialInboundItemImportDto>
{
    /// <summary>
    /// 初始化 导入SerialInboundItem 校验规则
    /// </summary>
    public TaktSerialInboundItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.InboundId)
            .GreaterThanOrEqualTo(0).WithMessage("入库主表 ID不能为负数");
        RuleFor(x => x.InboundCode)
            .NotEmpty().WithMessage("入库单号不能为空")
            .MaximumLength(10).WithMessage("入库单号长度不能超过10个字符");
        RuleFor(x => x.InboundSerialCode)
            .NotEmpty().WithMessage("入库序列号不能为空")
            .MaximumLength(100).WithMessage("入库序列号长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
