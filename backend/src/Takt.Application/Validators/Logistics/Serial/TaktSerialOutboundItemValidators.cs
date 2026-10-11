// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Serial
// 文件名称：TaktSerialOutboundItemValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SerialOutboundItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSerialOutboundItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Serial;

namespace Takt.Application.Validators.Logistics.Serial;

// ========================================
// 创建SerialOutboundItem 验证器
// ========================================

/// <summary>
/// 创建SerialOutboundItem DTO 验证器
/// </summary>
public class TaktSerialOutboundItemCreateValidator : AbstractValidator<TaktSerialOutboundItemCreateDto>
{
    /// <summary>
    /// 初始化 创建SerialOutboundItem 校验规则
    /// </summary>
    public TaktSerialOutboundItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.OutboundId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.OutboundId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.OutboundId)
            .GreaterThanOrEqualTo(0).WithMessage("出库主表 ID不能为负数");
        RuleFor(x => x.OutboundCode)
            .NotEmpty().WithMessage("出库单号不能为空").When(x => x.OutboundId <= 0)
            .MaximumLength(10).WithMessage("出库单号长度不能超过10个字符");
        RuleFor(x => x.OutboundSerialCode)
            .NotEmpty().WithMessage("出库序列号不能为空")
            .MaximumLength(100).WithMessage("出库序列号长度不能超过100个字符");
        RuleFor(x => x.ReferenceInboundId)
            .GreaterThanOrEqualTo(0).WithMessage("关联入库主表 ID不能为负数");
        RuleFor(x => x.ReferenceInboundCode)
            .NotEmpty().WithMessage("关联入库单号不能为空").When(x => x.ReferenceInboundId <= 0)
            .MaximumLength(10).WithMessage("关联入库单号长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SerialOutboundItem 验证器
// ========================================

/// <summary>
/// 更新SerialOutboundItem DTO 验证器
/// </summary>
public class TaktSerialOutboundItemUpdateValidator : AbstractValidator<TaktSerialOutboundItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新SerialOutboundItem 校验规则
    /// </summary>
    public TaktSerialOutboundItemUpdateValidator()
    {
        RuleFor(x => x.SerialOutboundItemId)
            .GreaterThan(0).WithMessage("SerialOutboundItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.OutboundId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.OutboundId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.OutboundId)
            .GreaterThanOrEqualTo(0).WithMessage("出库主表 ID不能为负数");
        RuleFor(x => x.OutboundCode)
            .NotEmpty().WithMessage("出库单号不能为空").When(x => x.OutboundId <= 0)
            .MaximumLength(10).WithMessage("出库单号长度不能超过10个字符");
        RuleFor(x => x.OutboundSerialCode)
            .NotEmpty().WithMessage("出库序列号不能为空")
            .MaximumLength(100).WithMessage("出库序列号长度不能超过100个字符");
        RuleFor(x => x.ReferenceInboundId)
            .GreaterThanOrEqualTo(0).WithMessage("关联入库主表 ID不能为负数");
        RuleFor(x => x.ReferenceInboundCode)
            .NotEmpty().WithMessage("关联入库单号不能为空").When(x => x.ReferenceInboundId <= 0)
            .MaximumLength(10).WithMessage("关联入库单号长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SerialOutboundItem 验证器
// ========================================

/// <summary>
/// 导入SerialOutboundItem DTO 验证器
/// </summary>
public class TaktSerialOutboundItemImportValidator : AbstractValidator<TaktSerialOutboundItemImportDto>
{
    /// <summary>
    /// 初始化 导入SerialOutboundItem 校验规则
    /// </summary>
    public TaktSerialOutboundItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.OutboundId)
            .GreaterThanOrEqualTo(0).WithMessage("出库主表 ID不能为负数");
        RuleFor(x => x.OutboundCode)
            .NotEmpty().WithMessage("出库单号不能为空")
            .MaximumLength(10).WithMessage("出库单号长度不能超过10个字符");
        RuleFor(x => x.OutboundSerialCode)
            .NotEmpty().WithMessage("出库序列号不能为空")
            .MaximumLength(100).WithMessage("出库序列号长度不能超过100个字符");
        RuleFor(x => x.ReferenceInboundId)
            .GreaterThanOrEqualTo(0).WithMessage("关联入库主表 ID不能为负数");
        RuleFor(x => x.ReferenceInboundCode)
            .NotEmpty().WithMessage("关联入库单号不能为空")
            .MaximumLength(10).WithMessage("关联入库单号长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
