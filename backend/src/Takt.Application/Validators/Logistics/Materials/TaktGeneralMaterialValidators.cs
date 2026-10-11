// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktGeneralMaterialValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：GeneralMaterial 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktGeneralMaterial 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建GeneralMaterial 验证器
// ========================================

/// <summary>
/// 创建GeneralMaterial DTO 验证器
/// </summary>
public class TaktGeneralMaterialCreateValidator : AbstractValidator<TaktGeneralMaterialCreateDto>
{
    /// <summary>
    /// 初始化 创建GeneralMaterial 校验规则
    /// </summary>
    public TaktGeneralMaterialCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("物料类型不能为空")
            .MaximumLength(4).WithMessage("物料类型长度不能超过4个字符");
        RuleFor(x => x.IndustrySector)
            .NotEmpty().WithMessage("行业领域不能为空")
            .MaximumLength(1).WithMessage("行业领域长度不能超过1个字符");
        RuleFor(x => x.MaterialGroup)
            .NotEmpty().WithMessage("物料组不能为空")
            .MaximumLength(9).WithMessage("物料组长度不能超过9个字符");
        RuleFor(x => x.BaseUnit)
            .NotEmpty().WithMessage("基本计量单位不能为空")
            .MaximumLength(3).WithMessage("基本计量单位长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新GeneralMaterial 验证器
// ========================================

/// <summary>
/// 更新GeneralMaterial DTO 验证器
/// </summary>
public class TaktGeneralMaterialUpdateValidator : AbstractValidator<TaktGeneralMaterialUpdateDto>
{
    /// <summary>
    /// 初始化 更新GeneralMaterial 校验规则
    /// </summary>
    public TaktGeneralMaterialUpdateValidator()
    {
        RuleFor(x => x.GeneralMaterialId)
            .GreaterThan(0).WithMessage("GeneralMaterialID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("物料类型不能为空")
            .MaximumLength(4).WithMessage("物料类型长度不能超过4个字符");
        RuleFor(x => x.IndustrySector)
            .NotEmpty().WithMessage("行业领域不能为空")
            .MaximumLength(1).WithMessage("行业领域长度不能超过1个字符");
        RuleFor(x => x.MaterialGroup)
            .NotEmpty().WithMessage("物料组不能为空")
            .MaximumLength(9).WithMessage("物料组长度不能超过9个字符");
        RuleFor(x => x.BaseUnit)
            .NotEmpty().WithMessage("基本计量单位不能为空")
            .MaximumLength(3).WithMessage("基本计量单位长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入GeneralMaterial 验证器
// ========================================

/// <summary>
/// 导入GeneralMaterial DTO 验证器
/// </summary>
public class TaktGeneralMaterialImportValidator : AbstractValidator<TaktGeneralMaterialImportDto>
{
    /// <summary>
    /// 初始化 导入GeneralMaterial 校验规则
    /// </summary>
    public TaktGeneralMaterialImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("物料类型不能为空")
            .MaximumLength(4).WithMessage("物料类型长度不能超过4个字符");
        RuleFor(x => x.IndustrySector)
            .NotEmpty().WithMessage("行业领域不能为空")
            .MaximumLength(1).WithMessage("行业领域长度不能超过1个字符");
        RuleFor(x => x.MaterialGroup)
            .NotEmpty().WithMessage("物料组不能为空")
            .MaximumLength(9).WithMessage("物料组长度不能超过9个字符");
        RuleFor(x => x.BaseUnit)
            .NotEmpty().WithMessage("基本计量单位不能为空")
            .MaximumLength(3).WithMessage("基本计量单位长度不能超过3个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
