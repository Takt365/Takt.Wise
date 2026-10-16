// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Serial
// 文件名称：TaktSerialOutboundValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：SerialOutbound 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSerialOutbound 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Serial;

namespace Takt.Application.Validators.Logistics.Serial;

// ========================================
// 创建SerialOutbound 验证器
// ========================================

/// <summary>
/// 创建SerialOutbound DTO 验证器
/// </summary>
public class TaktSerialOutboundCreateValidator : AbstractValidator<TaktSerialOutboundCreateDto>
{
    /// <summary>
    /// 初始化 创建SerialOutbound 校验规则
    /// </summary>
    public TaktSerialOutboundCreateValidator()
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
        RuleFor(x => x.OutboundCode)
            .NotEmpty().WithMessage("出库单号不能为空")
            .MaximumLength(10).WithMessage("出库单号长度不能超过10个字符");
        RuleFor(x => x.ShippingInvoiceCode)
            .NotEmpty().WithMessage("发货单号不能为空")
            .MaximumLength(50).WithMessage("发货单号长度不能超过50个字符");
        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("仕向地不能为空")
            .MaximumLength(40).WithMessage("仕向地长度不能超过40个字符");
        RuleFor(x => x.DestinationPort)
            .NotEmpty().WithMessage("目的地港不能为空")
            .MaximumLength(40).WithMessage("目的地港长度不能超过40个字符");
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("仓库编码不能为空")
            .MaximumLength(4).WithMessage("仓库编码长度不能超过4个字符");
        RuleFor(x => x.LocationCode)
            .NotEmpty().WithMessage("库位编码不能为空")
            .MaximumLength(40).WithMessage("库位编码长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SerialOutbound 验证器
// ========================================

/// <summary>
/// 更新SerialOutbound DTO 验证器
/// </summary>
public class TaktSerialOutboundUpdateValidator : AbstractValidator<TaktSerialOutboundUpdateDto>
{
    /// <summary>
    /// 初始化 更新SerialOutbound 校验规则
    /// </summary>
    public TaktSerialOutboundUpdateValidator()
    {
        RuleFor(x => x.SerialOutboundId)
            .GreaterThan(0).WithMessage("SerialOutboundID无效");
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
        RuleFor(x => x.OutboundCode)
            .NotEmpty().WithMessage("出库单号不能为空")
            .MaximumLength(10).WithMessage("出库单号长度不能超过10个字符");
        RuleFor(x => x.ShippingInvoiceCode)
            .NotEmpty().WithMessage("发货单号不能为空")
            .MaximumLength(50).WithMessage("发货单号长度不能超过50个字符");
        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("仕向地不能为空")
            .MaximumLength(40).WithMessage("仕向地长度不能超过40个字符");
        RuleFor(x => x.DestinationPort)
            .NotEmpty().WithMessage("目的地港不能为空")
            .MaximumLength(40).WithMessage("目的地港长度不能超过40个字符");
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("仓库编码不能为空")
            .MaximumLength(4).WithMessage("仓库编码长度不能超过4个字符");
        RuleFor(x => x.LocationCode)
            .NotEmpty().WithMessage("库位编码不能为空")
            .MaximumLength(40).WithMessage("库位编码长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SerialOutbound 验证器
// ========================================

/// <summary>
/// 导入SerialOutbound DTO 验证器
/// </summary>
public class TaktSerialOutboundImportValidator : AbstractValidator<TaktSerialOutboundImportDto>
{
    /// <summary>
    /// 初始化 导入SerialOutbound 校验规则
    /// </summary>
    public TaktSerialOutboundImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.OutboundCode)
            .NotEmpty().WithMessage("出库单号不能为空")
            .MaximumLength(10).WithMessage("出库单号长度不能超过10个字符");
        RuleFor(x => x.ShippingInvoiceCode)
            .NotEmpty().WithMessage("发货单号不能为空")
            .MaximumLength(50).WithMessage("发货单号长度不能超过50个字符");
        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("仕向地不能为空")
            .MaximumLength(40).WithMessage("仕向地长度不能超过40个字符");
        RuleFor(x => x.DestinationPort)
            .NotEmpty().WithMessage("目的地港不能为空")
            .MaximumLength(40).WithMessage("目的地港长度不能超过40个字符");
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("仓库编码不能为空")
            .MaximumLength(4).WithMessage("仓库编码长度不能超过4个字符");
        RuleFor(x => x.LocationCode)
            .NotEmpty().WithMessage("库位编码不能为空")
            .MaximumLength(40).WithMessage("库位编码长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
