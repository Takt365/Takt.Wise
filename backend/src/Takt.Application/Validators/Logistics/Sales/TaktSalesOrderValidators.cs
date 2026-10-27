// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktSalesOrderValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesOrder 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalesOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建SalesOrder 验证器
// ========================================

/// <summary>
/// 创建SalesOrder DTO 验证器
/// </summary>
public class TaktSalesOrderCreateValidator : AbstractValidator<TaktSalesOrderCreateDto>
{
    /// <summary>
    /// 初始化 创建SalesOrder 校验规则
    /// </summary>
    public TaktSalesOrderCreateValidator()
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
        RuleFor(x => x.SalesOrderCode)
            .NotEmpty().WithMessage("销售订单编码不能为空")
            .MaximumLength(20).WithMessage("销售订单编码长度不能超过20个字符");
        RuleFor(x => x.CustomerCode)
            .NotEmpty().WithMessage("客户编码不能为空")
            .MaximumLength(10).WithMessage("客户编码长度不能超过10个字符");
        RuleFor(x => x.CustomerName1)
            .NotEmpty().WithMessage("客户名称1不能为空")
            .MaximumLength(140).WithMessage("客户名称1长度不能超过140个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("结算币种不能为空")
            .MaximumLength(3).WithMessage("结算币种长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalesOrder 验证器
// ========================================

/// <summary>
/// 更新SalesOrder DTO 验证器
/// </summary>
public class TaktSalesOrderUpdateValidator : AbstractValidator<TaktSalesOrderUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalesOrder 校验规则
    /// </summary>
    public TaktSalesOrderUpdateValidator()
    {
        RuleFor(x => x.SalesOrderId)
            .GreaterThan(0).WithMessage("SalesOrderID无效");
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
        RuleFor(x => x.SalesOrderCode)
            .NotEmpty().WithMessage("销售订单编码不能为空")
            .MaximumLength(20).WithMessage("销售订单编码长度不能超过20个字符");
        RuleFor(x => x.CustomerCode)
            .NotEmpty().WithMessage("客户编码不能为空")
            .MaximumLength(10).WithMessage("客户编码长度不能超过10个字符");
        RuleFor(x => x.CustomerName1)
            .NotEmpty().WithMessage("客户名称1不能为空")
            .MaximumLength(140).WithMessage("客户名称1长度不能超过140个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("结算币种不能为空")
            .MaximumLength(3).WithMessage("结算币种长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SalesOrder 验证器
// ========================================

/// <summary>
/// 导入SalesOrder DTO 验证器
/// </summary>
public class TaktSalesOrderImportValidator : AbstractValidator<TaktSalesOrderImportDto>
{
    /// <summary>
    /// 初始化 导入SalesOrder 校验规则
    /// </summary>
    public TaktSalesOrderImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SalesOrderCode)
            .NotEmpty().WithMessage("销售订单编码不能为空")
            .MaximumLength(20).WithMessage("销售订单编码长度不能超过20个字符");
        RuleFor(x => x.CustomerCode)
            .NotEmpty().WithMessage("客户编码不能为空")
            .MaximumLength(10).WithMessage("客户编码长度不能超过10个字符");
        RuleFor(x => x.CustomerName1)
            .NotEmpty().WithMessage("客户名称1不能为空")
            .MaximumLength(140).WithMessage("客户名称1长度不能超过140个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("结算币种不能为空")
            .MaximumLength(3).WithMessage("结算币种长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
