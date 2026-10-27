// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：BomMaterialCostItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktBomMaterialCostItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Validators.Logistics.Manufacturing.Bom;

// ========================================
// 创建BomMaterialCostItem 验证器
// ========================================

/// <summary>
/// 创建BomMaterialCostItem DTO 验证器
/// </summary>
public class TaktBomMaterialCostItemCreateValidator : AbstractValidator<TaktBomMaterialCostItemCreateDto>
{
    /// <summary>
    /// 初始化 创建BomMaterialCostItem 校验规则
    /// </summary>
    public TaktBomMaterialCostItemCreateValidator()
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
        RuleFor(x => x.BomLevel)
            .NotEmpty().WithMessage("层级不能为空")
            .MaximumLength(20).WithMessage("层级长度不能超过20个字符");
        RuleFor(x => x.BomItemCode)
            .NotEmpty().WithMessage("BOM 项目号不能为空")
            .MaximumLength(4).WithMessage("BOM 项目号长度不能超过4个字符");
        RuleFor(x => x.ProductCode)
            .NotEmpty().WithMessage("产品编码不能为空")
            .MaximumLength(20).WithMessage("产品编码长度不能超过20个字符");
        RuleFor(x => x.ProductDescription)
            .NotEmpty().WithMessage("产品描述不能为空")
            .MaximumLength(40).WithMessage("产品描述长度不能超过40个字符");
        RuleFor(x => x.ComponentCode)
            .NotEmpty().WithMessage("组件编码不能为空")
            .MaximumLength(20).WithMessage("组件编码长度不能超过20个字符");
        RuleFor(x => x.ComponentDescription)
            .NotEmpty().WithMessage("组件描述不能为空")
            .MaximumLength(40).WithMessage("组件描述长度不能超过40个字符");
        RuleFor(x => x.PurchaseType)
            .NotEmpty().WithMessage("采购类型不能为空")
            .MaximumLength(1).WithMessage("采购类型长度不能超过1个字符");
        RuleFor(x => x.ProfitCenterCode)
            .NotEmpty().WithMessage("利润中心不能为空")
            .MaximumLength(4).WithMessage("利润中心长度不能超过4个字符");
        RuleFor(x => x.MovingPriceCurrencyCode)
            .NotEmpty().WithMessage("移动价格货币不能为空")
            .MaximumLength(3).WithMessage("移动价格货币长度不能超过3个字符");
        RuleFor(x => x.PurchaseOrganization)
            .NotEmpty().WithMessage("采购组织不能为空")
            .MaximumLength(4).WithMessage("采购组织长度不能超过4个字符");
        RuleFor(x => x.PurchaseGroup)
            .NotEmpty().WithMessage("采购组不能为空")
            .MaximumLength(3).WithMessage("采购组长度不能超过3个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(10).WithMessage("供应商编码长度不能超过10个字符");
        RuleFor(x => x.PurchaseCurrencyCode)
            .NotEmpty().WithMessage("采购货币不能为空")
            .MaximumLength(3).WithMessage("采购货币长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新BomMaterialCostItem 验证器
// ========================================

/// <summary>
/// 更新BomMaterialCostItem DTO 验证器
/// </summary>
public class TaktBomMaterialCostItemUpdateValidator : AbstractValidator<TaktBomMaterialCostItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新BomMaterialCostItem 校验规则
    /// </summary>
    public TaktBomMaterialCostItemUpdateValidator()
    {
        RuleFor(x => x.BomMaterialCostItemId)
            .GreaterThan(0).WithMessage("BomMaterialCostItemID无效");
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
        RuleFor(x => x.BomLevel)
            .NotEmpty().WithMessage("层级不能为空")
            .MaximumLength(20).WithMessage("层级长度不能超过20个字符");
        RuleFor(x => x.BomItemCode)
            .NotEmpty().WithMessage("BOM 项目号不能为空")
            .MaximumLength(4).WithMessage("BOM 项目号长度不能超过4个字符");
        RuleFor(x => x.ProductCode)
            .NotEmpty().WithMessage("产品编码不能为空")
            .MaximumLength(20).WithMessage("产品编码长度不能超过20个字符");
        RuleFor(x => x.ProductDescription)
            .NotEmpty().WithMessage("产品描述不能为空")
            .MaximumLength(40).WithMessage("产品描述长度不能超过40个字符");
        RuleFor(x => x.ComponentCode)
            .NotEmpty().WithMessage("组件编码不能为空")
            .MaximumLength(20).WithMessage("组件编码长度不能超过20个字符");
        RuleFor(x => x.ComponentDescription)
            .NotEmpty().WithMessage("组件描述不能为空")
            .MaximumLength(40).WithMessage("组件描述长度不能超过40个字符");
        RuleFor(x => x.PurchaseType)
            .NotEmpty().WithMessage("采购类型不能为空")
            .MaximumLength(1).WithMessage("采购类型长度不能超过1个字符");
        RuleFor(x => x.ProfitCenterCode)
            .NotEmpty().WithMessage("利润中心不能为空")
            .MaximumLength(4).WithMessage("利润中心长度不能超过4个字符");
        RuleFor(x => x.MovingPriceCurrencyCode)
            .NotEmpty().WithMessage("移动价格货币不能为空")
            .MaximumLength(3).WithMessage("移动价格货币长度不能超过3个字符");
        RuleFor(x => x.PurchaseOrganization)
            .NotEmpty().WithMessage("采购组织不能为空")
            .MaximumLength(4).WithMessage("采购组织长度不能超过4个字符");
        RuleFor(x => x.PurchaseGroup)
            .NotEmpty().WithMessage("采购组不能为空")
            .MaximumLength(3).WithMessage("采购组长度不能超过3个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(10).WithMessage("供应商编码长度不能超过10个字符");
        RuleFor(x => x.PurchaseCurrencyCode)
            .NotEmpty().WithMessage("采购货币不能为空")
            .MaximumLength(3).WithMessage("采购货币长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入BomMaterialCostItem 验证器
// ========================================

/// <summary>
/// 导入BomMaterialCostItem DTO 验证器
/// </summary>
public class TaktBomMaterialCostItemImportValidator : AbstractValidator<TaktBomMaterialCostItemImportDto>
{
    /// <summary>
    /// 初始化 导入BomMaterialCostItem 校验规则
    /// </summary>
    public TaktBomMaterialCostItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.BomLevel)
            .NotEmpty().WithMessage("层级不能为空")
            .MaximumLength(20).WithMessage("层级长度不能超过20个字符");
        RuleFor(x => x.BomItemCode)
            .NotEmpty().WithMessage("BOM 项目号不能为空")
            .MaximumLength(4).WithMessage("BOM 项目号长度不能超过4个字符");
        RuleFor(x => x.ProductCode)
            .NotEmpty().WithMessage("产品编码不能为空")
            .MaximumLength(20).WithMessage("产品编码长度不能超过20个字符");
        RuleFor(x => x.ProductDescription)
            .NotEmpty().WithMessage("产品描述不能为空")
            .MaximumLength(40).WithMessage("产品描述长度不能超过40个字符");
        RuleFor(x => x.ComponentCode)
            .NotEmpty().WithMessage("组件编码不能为空")
            .MaximumLength(20).WithMessage("组件编码长度不能超过20个字符");
        RuleFor(x => x.ComponentDescription)
            .NotEmpty().WithMessage("组件描述不能为空")
            .MaximumLength(40).WithMessage("组件描述长度不能超过40个字符");
        RuleFor(x => x.PurchaseType)
            .NotEmpty().WithMessage("采购类型不能为空")
            .MaximumLength(1).WithMessage("采购类型长度不能超过1个字符");
        RuleFor(x => x.ProfitCenterCode)
            .NotEmpty().WithMessage("利润中心不能为空")
            .MaximumLength(4).WithMessage("利润中心长度不能超过4个字符");
        RuleFor(x => x.MovingPriceCurrencyCode)
            .NotEmpty().WithMessage("移动价格货币不能为空")
            .MaximumLength(3).WithMessage("移动价格货币长度不能超过3个字符");
        RuleFor(x => x.PurchaseOrganization)
            .NotEmpty().WithMessage("采购组织不能为空")
            .MaximumLength(4).WithMessage("采购组织长度不能超过4个字符");
        RuleFor(x => x.PurchaseGroup)
            .NotEmpty().WithMessage("采购组不能为空")
            .MaximumLength(3).WithMessage("采购组长度不能超过3个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(10).WithMessage("供应商编码长度不能超过10个字符");
        RuleFor(x => x.PurchaseCurrencyCode)
            .NotEmpty().WithMessage("采购货币不能为空")
            .MaximumLength(3).WithMessage("采购货币长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
