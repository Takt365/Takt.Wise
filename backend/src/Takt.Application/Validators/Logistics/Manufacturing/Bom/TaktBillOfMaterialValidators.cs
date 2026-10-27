// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：BillOfMaterial 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktBillOfMaterial 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Validators.Logistics.Manufacturing.Bom;

// ========================================
// 创建BillOfMaterial 验证器
// ========================================

/// <summary>
/// 创建BillOfMaterial DTO 验证器
/// </summary>
public class TaktBillOfMaterialCreateValidator : AbstractValidator<TaktBillOfMaterialCreateDto>
{
    /// <summary>
    /// 初始化 创建BillOfMaterial 校验规则
    /// </summary>
    public TaktBillOfMaterialCreateValidator()
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
        RuleFor(x => x.BomCode)
            .NotEmpty().WithMessage("BOM编码不能为空")
            .MaximumLength(50).WithMessage("BOM编码长度不能超过50个字符");
        RuleFor(x => x.BomName)
            .NotEmpty().WithMessage("BOM名称不能为空")
            .MaximumLength(200).WithMessage("BOM名称长度不能超过200个字符");
        RuleFor(x => x.ParentMaterialCode)
            .NotEmpty().WithMessage("父物料编码不能为空")
            .MaximumLength(20).WithMessage("父物料编码长度不能超过20个字符");
        RuleFor(x => x.BomVersion)
            .NotEmpty().WithMessage("BOM版本号不能为空")
            .MaximumLength(20).WithMessage("BOM版本号长度不能超过20个字符");
        RuleFor(x => x.AlternativeBomNumber)
            .NotEmpty().WithMessage("备选BOM编码不能为空")
            .MaximumLength(10).WithMessage("备选BOM编码长度不能超过10个字符");
        RuleFor(x => x.ParentMaterialUnit)
            .NotEmpty().WithMessage("父物料单位不能为空")
            .MaximumLength(20).WithMessage("父物料单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新BillOfMaterial 验证器
// ========================================

/// <summary>
/// 更新BillOfMaterial DTO 验证器
/// </summary>
public class TaktBillOfMaterialUpdateValidator : AbstractValidator<TaktBillOfMaterialUpdateDto>
{
    /// <summary>
    /// 初始化 更新BillOfMaterial 校验规则
    /// </summary>
    public TaktBillOfMaterialUpdateValidator()
    {
        RuleFor(x => x.BillOfMaterialId)
            .GreaterThan(0).WithMessage("BillOfMaterialID无效");
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
        RuleFor(x => x.BomCode)
            .NotEmpty().WithMessage("BOM编码不能为空")
            .MaximumLength(50).WithMessage("BOM编码长度不能超过50个字符");
        RuleFor(x => x.BomName)
            .NotEmpty().WithMessage("BOM名称不能为空")
            .MaximumLength(200).WithMessage("BOM名称长度不能超过200个字符");
        RuleFor(x => x.ParentMaterialCode)
            .NotEmpty().WithMessage("父物料编码不能为空")
            .MaximumLength(20).WithMessage("父物料编码长度不能超过20个字符");
        RuleFor(x => x.BomVersion)
            .NotEmpty().WithMessage("BOM版本号不能为空")
            .MaximumLength(20).WithMessage("BOM版本号长度不能超过20个字符");
        RuleFor(x => x.AlternativeBomNumber)
            .NotEmpty().WithMessage("备选BOM编码不能为空")
            .MaximumLength(10).WithMessage("备选BOM编码长度不能超过10个字符");
        RuleFor(x => x.ParentMaterialUnit)
            .NotEmpty().WithMessage("父物料单位不能为空")
            .MaximumLength(20).WithMessage("父物料单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入BillOfMaterial 验证器
// ========================================

/// <summary>
/// 导入BillOfMaterial DTO 验证器
/// </summary>
public class TaktBillOfMaterialImportValidator : AbstractValidator<TaktBillOfMaterialImportDto>
{
    /// <summary>
    /// 初始化 导入BillOfMaterial 校验规则
    /// </summary>
    public TaktBillOfMaterialImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.BomCode)
            .NotEmpty().WithMessage("BOM编码不能为空")
            .MaximumLength(50).WithMessage("BOM编码长度不能超过50个字符");
        RuleFor(x => x.BomName)
            .NotEmpty().WithMessage("BOM名称不能为空")
            .MaximumLength(200).WithMessage("BOM名称长度不能超过200个字符");
        RuleFor(x => x.ParentMaterialCode)
            .NotEmpty().WithMessage("父物料编码不能为空")
            .MaximumLength(20).WithMessage("父物料编码长度不能超过20个字符");
        RuleFor(x => x.BomVersion)
            .NotEmpty().WithMessage("BOM版本号不能为空")
            .MaximumLength(20).WithMessage("BOM版本号长度不能超过20个字符");
        RuleFor(x => x.AlternativeBomNumber)
            .NotEmpty().WithMessage("备选BOM编码不能为空")
            .MaximumLength(10).WithMessage("备选BOM编码长度不能超过10个字符");
        RuleFor(x => x.ParentMaterialUnit)
            .NotEmpty().WithMessage("父物料单位不能为空")
            .MaximumLength(20).WithMessage("父物料单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
