// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Mds
// 文件名称：TaktSalesForecastValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesForecast 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalesForecast 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Mds;

namespace Takt.Application.Validators.Logistics.Manufacturing.Mds;

// ========================================
// 创建SalesForecast 验证器
// ========================================

/// <summary>
/// 创建SalesForecast DTO 验证器
/// </summary>
public class TaktSalesForecastCreateValidator : AbstractValidator<TaktSalesForecastCreateDto>
{
    /// <summary>
    /// 初始化 创建SalesForecast 校验规则
    /// </summary>
    public TaktSalesForecastCreateValidator()
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
        RuleFor(x => x.SalesForecastCode)
            .NotEmpty().WithMessage("销售预测编码不能为空")
            .MaximumLength(20).WithMessage("销售预测编码长度不能超过20个字符");
        RuleFor(x => x.SalesProduct)
            .NotEmpty().WithMessage("产品不能为空")
            .MaximumLength(7).WithMessage("产品长度不能超过7个字符");
        RuleFor(x => x.ProductCategoryCode)
            .NotEmpty().WithMessage("产品类别不能为空")
            .MaximumLength(4).WithMessage("产品类别长度不能超过4个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalesForecast 验证器
// ========================================

/// <summary>
/// 更新SalesForecast DTO 验证器
/// </summary>
public class TaktSalesForecastUpdateValidator : AbstractValidator<TaktSalesForecastUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalesForecast 校验规则
    /// </summary>
    public TaktSalesForecastUpdateValidator()
    {
        RuleFor(x => x.SalesForecastId)
            .GreaterThan(0).WithMessage("SalesForecastID无效");
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
        RuleFor(x => x.SalesForecastCode)
            .NotEmpty().WithMessage("销售预测编码不能为空")
            .MaximumLength(20).WithMessage("销售预测编码长度不能超过20个字符");
        RuleFor(x => x.SalesProduct)
            .NotEmpty().WithMessage("产品不能为空")
            .MaximumLength(7).WithMessage("产品长度不能超过7个字符");
        RuleFor(x => x.ProductCategoryCode)
            .NotEmpty().WithMessage("产品类别不能为空")
            .MaximumLength(4).WithMessage("产品类别长度不能超过4个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SalesForecast 验证器
// ========================================

/// <summary>
/// 导入SalesForecast DTO 验证器
/// </summary>
public class TaktSalesForecastImportValidator : AbstractValidator<TaktSalesForecastImportDto>
{
    /// <summary>
    /// 初始化 导入SalesForecast 校验规则
    /// </summary>
    public TaktSalesForecastImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SalesForecastCode)
            .NotEmpty().WithMessage("销售预测编码不能为空")
            .MaximumLength(20).WithMessage("销售预测编码长度不能超过20个字符");
        RuleFor(x => x.SalesProduct)
            .NotEmpty().WithMessage("产品不能为空")
            .MaximumLength(7).WithMessage("产品长度不能超过7个字符");
        RuleFor(x => x.ProductCategoryCode)
            .NotEmpty().WithMessage("产品类别不能为空")
            .MaximumLength(4).WithMessage("产品类别长度不能超过4个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
