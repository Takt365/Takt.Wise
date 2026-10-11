// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Serial
// 文件名称：TaktSerialUploadValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SerialUpload 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSerialUpload 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Serial;

namespace Takt.Application.Validators.Logistics.Serial;

// ========================================
// 创建SerialUpload 验证器
// ========================================

/// <summary>
/// 创建SerialUpload DTO 验证器
/// </summary>
public class TaktSerialUploadCreateValidator : AbstractValidator<TaktSerialUploadCreateDto>
{
    /// <summary>
    /// 初始化 创建SerialUpload 校验规则
    /// </summary>
    public TaktSerialUploadCreateValidator()
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
        RuleFor(x => x.ShippingInvoiceCode)
            .NotEmpty().WithMessage("发货单号不能为空")
            .MaximumLength(9).WithMessage("发货单号长度不能超过9个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("产品物料不能为空")
            .MaximumLength(20).WithMessage("产品物料长度不能超过20个字符");
        RuleFor(x => x.SerialCode)
            .NotEmpty().WithMessage("序列号不能为空")
            .MaximumLength(7).WithMessage("序列号长度不能超过7个字符");
        RuleFor(x => x.TransportMode)
            .NotEmpty().WithMessage("运输方式不能为空")
            .MaximumLength(20).WithMessage("运输方式长度不能超过20个字符");
        RuleFor(x => x.MaterialText)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SerialUpload 验证器
// ========================================

/// <summary>
/// 更新SerialUpload DTO 验证器
/// </summary>
public class TaktSerialUploadUpdateValidator : AbstractValidator<TaktSerialUploadUpdateDto>
{
    /// <summary>
    /// 初始化 更新SerialUpload 校验规则
    /// </summary>
    public TaktSerialUploadUpdateValidator()
    {
        RuleFor(x => x.SerialUploadId)
            .GreaterThan(0).WithMessage("SerialUploadID无效");
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
        RuleFor(x => x.ShippingInvoiceCode)
            .NotEmpty().WithMessage("发货单号不能为空")
            .MaximumLength(9).WithMessage("发货单号长度不能超过9个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("产品物料不能为空")
            .MaximumLength(20).WithMessage("产品物料长度不能超过20个字符");
        RuleFor(x => x.SerialCode)
            .NotEmpty().WithMessage("序列号不能为空")
            .MaximumLength(7).WithMessage("序列号长度不能超过7个字符");
        RuleFor(x => x.TransportMode)
            .NotEmpty().WithMessage("运输方式不能为空")
            .MaximumLength(20).WithMessage("运输方式长度不能超过20个字符");
        RuleFor(x => x.MaterialText)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SerialUpload 验证器
// ========================================

/// <summary>
/// 导入SerialUpload DTO 验证器
/// </summary>
public class TaktSerialUploadImportValidator : AbstractValidator<TaktSerialUploadImportDto>
{
    /// <summary>
    /// 初始化 导入SerialUpload 校验规则
    /// </summary>
    public TaktSerialUploadImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ShippingInvoiceCode)
            .NotEmpty().WithMessage("发货单号不能为空")
            .MaximumLength(9).WithMessage("发货单号长度不能超过9个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("产品物料不能为空")
            .MaximumLength(20).WithMessage("产品物料长度不能超过20个字符");
        RuleFor(x => x.SerialCode)
            .NotEmpty().WithMessage("序列号不能为空")
            .MaximumLength(7).WithMessage("序列号长度不能超过7个字符");
        RuleFor(x => x.TransportMode)
            .NotEmpty().WithMessage("运输方式不能为空")
            .MaximumLength(20).WithMessage("运输方式长度不能超过20个字符");
        RuleFor(x => x.MaterialText)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
