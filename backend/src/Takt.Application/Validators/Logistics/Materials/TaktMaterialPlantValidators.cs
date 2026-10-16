// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktMaterialPlantValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialPlant 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaterialPlant 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建MaterialPlant 验证器
// ========================================

/// <summary>
/// 创建MaterialPlant DTO 验证器
/// </summary>
public class TaktMaterialPlantCreateValidator : AbstractValidator<TaktMaterialPlantCreateDto>
{
    /// <summary>
    /// 初始化 创建MaterialPlant 校验规则
    /// </summary>
    public TaktMaterialPlantCreateValidator()
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
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.IndustrySector)
            .NotEmpty().WithMessage("行业领域不能为空")
            .MaximumLength(1).WithMessage("行业领域长度不能超过1个字符");
        RuleFor(x => x.MaterialGroup)
            .NotEmpty().WithMessage("物料组不能为空")
            .MaximumLength(20).WithMessage("物料组长度不能超过20个字符");
        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("物料类型不能为空")
            .MaximumLength(4).WithMessage("物料类型长度不能超过4个字符");
        RuleFor(x => x.BaseUnit)
            .NotEmpty().WithMessage("基本单位不能为空")
            .MaximumLength(5).WithMessage("基本单位长度不能超过5个字符");
        RuleFor(x => x.PurchaseGroup)
            .NotEmpty().WithMessage("采购组不能为空")
            .MaximumLength(3).WithMessage("采购组长度不能超过3个字符");
        RuleFor(x => x.PurchaseType)
            .NotEmpty().WithMessage("采购类型不能为空")
            .MaximumLength(1).WithMessage("采购类型长度不能超过1个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.PriceControl)
            .NotEmpty().WithMessage("价格控制不能为空")
            .MaximumLength(1).WithMessage("价格控制长度不能超过1个字符");
        RuleFor(x => x.Valuation)
            .NotEmpty().WithMessage("评估类别不能为空")
            .MaximumLength(4).WithMessage("评估类别长度不能超过4个字符");
        RuleFor(x => x.ProfitCenter)
            .NotEmpty().WithMessage("利润中心不能为空")
            .MaximumLength(4).WithMessage("利润中心长度不能超过4个字符");
        RuleFor(x => x.ProductionLocation)
            .NotEmpty().WithMessage("生产仓储不能为空")
            .MaximumLength(4).WithMessage("生产仓储长度不能超过4个字符");
        RuleFor(x => x.PurchasingLocation)
            .NotEmpty().WithMessage("采购仓储不能为空")
            .MaximumLength(4).WithMessage("采购仓储长度不能超过4个字符");
        RuleFor(x => x.StorageLocation)
            .NotEmpty().WithMessage("库位不能为空")
            .MaximumLength(40).WithMessage("库位长度不能超过40个字符");
        RuleFor(x => x.DiscontinuedStatus)
            .NotEmpty().WithMessage("停产状态不能为空")
            .MaximumLength(4).WithMessage("停产状态长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MaterialPlant 验证器
// ========================================

/// <summary>
/// 更新MaterialPlant DTO 验证器
/// </summary>
public class TaktMaterialPlantUpdateValidator : AbstractValidator<TaktMaterialPlantUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaterialPlant 校验规则
    /// </summary>
    public TaktMaterialPlantUpdateValidator()
    {
        RuleFor(x => x.MaterialPlantId)
            .GreaterThan(0).WithMessage("MaterialPlantID无效");
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
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.IndustrySector)
            .NotEmpty().WithMessage("行业领域不能为空")
            .MaximumLength(1).WithMessage("行业领域长度不能超过1个字符");
        RuleFor(x => x.MaterialGroup)
            .NotEmpty().WithMessage("物料组不能为空")
            .MaximumLength(20).WithMessage("物料组长度不能超过20个字符");
        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("物料类型不能为空")
            .MaximumLength(4).WithMessage("物料类型长度不能超过4个字符");
        RuleFor(x => x.BaseUnit)
            .NotEmpty().WithMessage("基本单位不能为空")
            .MaximumLength(5).WithMessage("基本单位长度不能超过5个字符");
        RuleFor(x => x.PurchaseGroup)
            .NotEmpty().WithMessage("采购组不能为空")
            .MaximumLength(3).WithMessage("采购组长度不能超过3个字符");
        RuleFor(x => x.PurchaseType)
            .NotEmpty().WithMessage("采购类型不能为空")
            .MaximumLength(1).WithMessage("采购类型长度不能超过1个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.PriceControl)
            .NotEmpty().WithMessage("价格控制不能为空")
            .MaximumLength(1).WithMessage("价格控制长度不能超过1个字符");
        RuleFor(x => x.Valuation)
            .NotEmpty().WithMessage("评估类别不能为空")
            .MaximumLength(4).WithMessage("评估类别长度不能超过4个字符");
        RuleFor(x => x.ProfitCenter)
            .NotEmpty().WithMessage("利润中心不能为空")
            .MaximumLength(4).WithMessage("利润中心长度不能超过4个字符");
        RuleFor(x => x.ProductionLocation)
            .NotEmpty().WithMessage("生产仓储不能为空")
            .MaximumLength(4).WithMessage("生产仓储长度不能超过4个字符");
        RuleFor(x => x.PurchasingLocation)
            .NotEmpty().WithMessage("采购仓储不能为空")
            .MaximumLength(4).WithMessage("采购仓储长度不能超过4个字符");
        RuleFor(x => x.StorageLocation)
            .NotEmpty().WithMessage("库位不能为空")
            .MaximumLength(40).WithMessage("库位长度不能超过40个字符");
        RuleFor(x => x.DiscontinuedStatus)
            .NotEmpty().WithMessage("停产状态不能为空")
            .MaximumLength(4).WithMessage("停产状态长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入MaterialPlant 验证器
// ========================================

/// <summary>
/// 导入MaterialPlant DTO 验证器
/// </summary>
public class TaktMaterialPlantImportValidator : AbstractValidator<TaktMaterialPlantImportDto>
{
    /// <summary>
    /// 初始化 导入MaterialPlant 校验规则
    /// </summary>
    public TaktMaterialPlantImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.IndustrySector)
            .NotEmpty().WithMessage("行业领域不能为空")
            .MaximumLength(1).WithMessage("行业领域长度不能超过1个字符");
        RuleFor(x => x.MaterialGroup)
            .NotEmpty().WithMessage("物料组不能为空")
            .MaximumLength(20).WithMessage("物料组长度不能超过20个字符");
        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("物料类型不能为空")
            .MaximumLength(4).WithMessage("物料类型长度不能超过4个字符");
        RuleFor(x => x.BaseUnit)
            .NotEmpty().WithMessage("基本单位不能为空")
            .MaximumLength(5).WithMessage("基本单位长度不能超过5个字符");
        RuleFor(x => x.PurchaseGroup)
            .NotEmpty().WithMessage("采购组不能为空")
            .MaximumLength(3).WithMessage("采购组长度不能超过3个字符");
        RuleFor(x => x.PurchaseType)
            .NotEmpty().WithMessage("采购类型不能为空")
            .MaximumLength(1).WithMessage("采购类型长度不能超过1个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.PriceControl)
            .NotEmpty().WithMessage("价格控制不能为空")
            .MaximumLength(1).WithMessage("价格控制长度不能超过1个字符");
        RuleFor(x => x.Valuation)
            .NotEmpty().WithMessage("评估类别不能为空")
            .MaximumLength(4).WithMessage("评估类别长度不能超过4个字符");
        RuleFor(x => x.ProfitCenter)
            .NotEmpty().WithMessage("利润中心不能为空")
            .MaximumLength(4).WithMessage("利润中心长度不能超过4个字符");
        RuleFor(x => x.ProductionLocation)
            .NotEmpty().WithMessage("生产仓储不能为空")
            .MaximumLength(4).WithMessage("生产仓储长度不能超过4个字符");
        RuleFor(x => x.PurchasingLocation)
            .NotEmpty().WithMessage("采购仓储不能为空")
            .MaximumLength(4).WithMessage("采购仓储长度不能超过4个字符");
        RuleFor(x => x.StorageLocation)
            .NotEmpty().WithMessage("库位不能为空")
            .MaximumLength(40).WithMessage("库位长度不能超过40个字符");
        RuleFor(x => x.DiscontinuedStatus)
            .NotEmpty().WithMessage("停产状态不能为空")
            .MaximumLength(4).WithMessage("停产状态长度不能超过4个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
