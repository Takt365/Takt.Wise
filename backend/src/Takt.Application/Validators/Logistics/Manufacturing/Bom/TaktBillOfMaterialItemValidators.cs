// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialItemValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：BillOfMaterialItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktBillOfMaterialItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Validators.Logistics.Manufacturing.Bom;

// ========================================
// 创建BillOfMaterialItem 验证器
// ========================================

/// <summary>
/// 创建BillOfMaterialItem DTO 验证器
/// </summary>
public class TaktBillOfMaterialItemCreateValidator : AbstractValidator<TaktBillOfMaterialItemCreateDto>
{
    /// <summary>
    /// 初始化 创建BillOfMaterialItem 校验规则
    /// </summary>
    public TaktBillOfMaterialItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.BillOfMaterialId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.BillOfMaterialId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.BillOfMaterialId)
            .GreaterThanOrEqualTo(0).WithMessage("物料清单ID不能为负数");
        RuleFor(x => x.BomCode)
            .NotEmpty().WithMessage("BOM编码不能为空")
            .MaximumLength(50).WithMessage("BOM编码长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("子项物料编码不能为空")
            .MaximumLength(20).WithMessage("子项物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialUnit)
            .NotEmpty().WithMessage("单位不能为空")
            .MaximumLength(20).WithMessage("单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新BillOfMaterialItem 验证器
// ========================================

/// <summary>
/// 更新BillOfMaterialItem DTO 验证器
/// </summary>
public class TaktBillOfMaterialItemUpdateValidator : AbstractValidator<TaktBillOfMaterialItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新BillOfMaterialItem 校验规则
    /// </summary>
    public TaktBillOfMaterialItemUpdateValidator()
    {
        RuleFor(x => x.BillOfMaterialItemId)
            .GreaterThan(0).WithMessage("BillOfMaterialItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.BillOfMaterialId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.BillOfMaterialId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.BillOfMaterialId)
            .GreaterThanOrEqualTo(0).WithMessage("物料清单ID不能为负数");
        RuleFor(x => x.BomCode)
            .NotEmpty().WithMessage("BOM编码不能为空")
            .MaximumLength(50).WithMessage("BOM编码长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("子项物料编码不能为空")
            .MaximumLength(20).WithMessage("子项物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialUnit)
            .NotEmpty().WithMessage("单位不能为空")
            .MaximumLength(20).WithMessage("单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入BillOfMaterialItem 验证器
// ========================================

/// <summary>
/// 导入BillOfMaterialItem DTO 验证器
/// </summary>
public class TaktBillOfMaterialItemImportValidator : AbstractValidator<TaktBillOfMaterialItemImportDto>
{
    /// <summary>
    /// 初始化 导入BillOfMaterialItem 校验规则
    /// </summary>
    public TaktBillOfMaterialItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.BillOfMaterialId)
            .GreaterThanOrEqualTo(0).WithMessage("物料清单ID不能为负数");
        RuleFor(x => x.BomCode)
            .NotEmpty().WithMessage("BOM编码不能为空")
            .MaximumLength(50).WithMessage("BOM编码长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("子项物料编码不能为空")
            .MaximumLength(20).WithMessage("子项物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialUnit)
            .NotEmpty().WithMessage("单位不能为空")
            .MaximumLength(20).WithMessage("单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
