// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktSalesOrderItemValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesOrderItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalesOrderItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建SalesOrderItem 验证器
// ========================================

/// <summary>
/// 创建SalesOrderItem DTO 验证器
/// </summary>
public class TaktSalesOrderItemCreateValidator : AbstractValidator<TaktSalesOrderItemCreateDto>
{
    /// <summary>
    /// 初始化 创建SalesOrderItem 校验规则
    /// </summary>
    public TaktSalesOrderItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.SalesOrderId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.SalesOrderId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.SalesOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("销售订单不能为负数");
        RuleFor(x => x.SalesOrderCode)
            .NotEmpty().WithMessage("销售订单编码不能为空").When(x => x.SalesOrderId <= 0)
            .MaximumLength(20).WithMessage("销售订单编码长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.SalesUnit)
            .NotEmpty().WithMessage("销售单位不能为空")
            .MaximumLength(5).WithMessage("销售单位长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalesOrderItem 验证器
// ========================================

/// <summary>
/// 更新SalesOrderItem DTO 验证器
/// </summary>
public class TaktSalesOrderItemUpdateValidator : AbstractValidator<TaktSalesOrderItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalesOrderItem 校验规则
    /// </summary>
    public TaktSalesOrderItemUpdateValidator()
    {
        RuleFor(x => x.SalesOrderItemId)
            .GreaterThan(0).WithMessage("SalesOrderItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.SalesOrderId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.SalesOrderId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.SalesOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("销售订单不能为负数");
        RuleFor(x => x.SalesOrderCode)
            .NotEmpty().WithMessage("销售订单编码不能为空").When(x => x.SalesOrderId <= 0)
            .MaximumLength(20).WithMessage("销售订单编码长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.SalesUnit)
            .NotEmpty().WithMessage("销售单位不能为空")
            .MaximumLength(5).WithMessage("销售单位长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SalesOrderItem 验证器
// ========================================

/// <summary>
/// 导入SalesOrderItem DTO 验证器
/// </summary>
public class TaktSalesOrderItemImportValidator : AbstractValidator<TaktSalesOrderItemImportDto>
{
    /// <summary>
    /// 初始化 导入SalesOrderItem 校验规则
    /// </summary>
    public TaktSalesOrderItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SalesOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("销售订单不能为负数");
        RuleFor(x => x.SalesOrderCode)
            .NotEmpty().WithMessage("销售订单编码不能为空")
            .MaximumLength(20).WithMessage("销售订单编码长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.SalesUnit)
            .NotEmpty().WithMessage("销售单位不能为空")
            .MaximumLength(5).WithMessage("销售单位长度不能超过5个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
