// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：BomMaterialCost 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktBomMaterialCost 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Validators.Logistics.Manufacturing.Bom;

// ========================================
// 创建BomMaterialCost 验证器
// ========================================

/// <summary>
/// 创建BomMaterialCost DTO 验证器
/// </summary>
public class TaktBomMaterialCostCreateValidator : AbstractValidator<TaktBomMaterialCostCreateDto>
{
    /// <summary>
    /// 初始化 创建BomMaterialCost 校验规则
    /// </summary>
    public TaktBomMaterialCostCreateValidator()
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
        RuleFor(x => x.ModelCode)
            .NotEmpty().WithMessage("机种编码不能为空")
            .MaximumLength(40).WithMessage("机种编码长度不能超过40个字符");
        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("物料类型不能为空")
            .MaximumLength(4).WithMessage("物料类型长度不能超过4个字符");
        RuleFor(x => x.ProductCode)
            .NotEmpty().WithMessage("产品编码不能为空")
            .MaximumLength(20).WithMessage("产品编码长度不能超过20个字符");
        RuleFor(x => x.ProductDescription)
            .NotEmpty().WithMessage("产品描述不能为空")
            .MaximumLength(40).WithMessage("产品描述长度不能超过40个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.CostingPeriod)
            .NotEmpty().WithMessage("核算期间不能为空")
            .MaximumLength(7).WithMessage("核算期间长度不能超过7个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新BomMaterialCost 验证器
// ========================================

/// <summary>
/// 更新BomMaterialCost DTO 验证器
/// </summary>
public class TaktBomMaterialCostUpdateValidator : AbstractValidator<TaktBomMaterialCostUpdateDto>
{
    /// <summary>
    /// 初始化 更新BomMaterialCost 校验规则
    /// </summary>
    public TaktBomMaterialCostUpdateValidator()
    {
        RuleFor(x => x.BomMaterialCostId)
            .GreaterThan(0).WithMessage("BomMaterialCostID无效");
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
        RuleFor(x => x.ModelCode)
            .NotEmpty().WithMessage("机种编码不能为空")
            .MaximumLength(40).WithMessage("机种编码长度不能超过40个字符");
        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("物料类型不能为空")
            .MaximumLength(4).WithMessage("物料类型长度不能超过4个字符");
        RuleFor(x => x.ProductCode)
            .NotEmpty().WithMessage("产品编码不能为空")
            .MaximumLength(20).WithMessage("产品编码长度不能超过20个字符");
        RuleFor(x => x.ProductDescription)
            .NotEmpty().WithMessage("产品描述不能为空")
            .MaximumLength(40).WithMessage("产品描述长度不能超过40个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.CostingPeriod)
            .NotEmpty().WithMessage("核算期间不能为空")
            .MaximumLength(7).WithMessage("核算期间长度不能超过7个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入BomMaterialCost 验证器
// ========================================

/// <summary>
/// 导入BomMaterialCost DTO 验证器
/// </summary>
public class TaktBomMaterialCostImportValidator : AbstractValidator<TaktBomMaterialCostImportDto>
{
    /// <summary>
    /// 初始化 导入BomMaterialCost 校验规则
    /// </summary>
    public TaktBomMaterialCostImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ModelCode)
            .NotEmpty().WithMessage("机种编码不能为空")
            .MaximumLength(40).WithMessage("机种编码长度不能超过40个字符");
        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("物料类型不能为空")
            .MaximumLength(4).WithMessage("物料类型长度不能超过4个字符");
        RuleFor(x => x.ProductCode)
            .NotEmpty().WithMessage("产品编码不能为空")
            .MaximumLength(20).WithMessage("产品编码长度不能超过20个字符");
        RuleFor(x => x.ProductDescription)
            .NotEmpty().WithMessage("产品描述不能为空")
            .MaximumLength(40).WithMessage("产品描述长度不能超过40个字符");
        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("币种不能为空")
            .MaximumLength(3).WithMessage("币种长度不能超过3个字符");
        RuleFor(x => x.CostingPeriod)
            .NotEmpty().WithMessage("核算期间不能为空")
            .MaximumLength(7).WithMessage("核算期间长度不能超过7个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
