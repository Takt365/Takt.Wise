// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Sales
// 文件名称：TaktSellerMaterialValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：SellerMaterial 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSellerMaterial 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Validators.Logistics.Sales;

// ========================================
// 创建SellerMaterial 验证器
// ========================================

/// <summary>
/// 创建SellerMaterial DTO 验证器
/// </summary>
public class TaktSellerMaterialCreateValidator : AbstractValidator<TaktSellerMaterialCreateDto>
{
    /// <summary>
    /// 初始化 创建SellerMaterial 校验规则
    /// </summary>
    public TaktSellerMaterialCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("物料类型不能为空")
            .MaximumLength(4).WithMessage("物料类型长度不能超过4个字符");
        RuleFor(x => x.MaterialGroup)
            .NotEmpty().WithMessage("物料组不能为空")
            .MaximumLength(9).WithMessage("物料组长度不能超过9个字符");
        RuleFor(x => x.InternalMaterialCode)
            .NotEmpty().WithMessage("内部物料编码不能为空")
            .MaximumLength(20).WithMessage("内部物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.SellerMaterialCode)
            .NotEmpty().WithMessage("销售商物料编码不能为空")
            .MaximumLength(40).WithMessage("销售商物料编码长度不能超过40个字符");
        RuleFor(x => x.SellerMaterialDescription)
            .NotEmpty().WithMessage("销售商物料描述不能为空")
            .MaximumLength(40).WithMessage("销售商物料描述长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SellerMaterial 验证器
// ========================================

/// <summary>
/// 更新SellerMaterial DTO 验证器
/// </summary>
public class TaktSellerMaterialUpdateValidator : AbstractValidator<TaktSellerMaterialUpdateDto>
{
    /// <summary>
    /// 初始化 更新SellerMaterial 校验规则
    /// </summary>
    public TaktSellerMaterialUpdateValidator()
    {
        RuleFor(x => x.SellerMaterialId)
            .GreaterThan(0).WithMessage("SellerMaterialID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("物料类型不能为空")
            .MaximumLength(4).WithMessage("物料类型长度不能超过4个字符");
        RuleFor(x => x.MaterialGroup)
            .NotEmpty().WithMessage("物料组不能为空")
            .MaximumLength(9).WithMessage("物料组长度不能超过9个字符");
        RuleFor(x => x.InternalMaterialCode)
            .NotEmpty().WithMessage("内部物料编码不能为空")
            .MaximumLength(20).WithMessage("内部物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.SellerMaterialCode)
            .NotEmpty().WithMessage("销售商物料编码不能为空")
            .MaximumLength(40).WithMessage("销售商物料编码长度不能超过40个字符");
        RuleFor(x => x.SellerMaterialDescription)
            .NotEmpty().WithMessage("销售商物料描述不能为空")
            .MaximumLength(40).WithMessage("销售商物料描述长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SellerMaterial 验证器
// ========================================

/// <summary>
/// 导入SellerMaterial DTO 验证器
/// </summary>
public class TaktSellerMaterialImportValidator : AbstractValidator<TaktSellerMaterialImportDto>
{
    /// <summary>
    /// 初始化 导入SellerMaterial 校验规则
    /// </summary>
    public TaktSellerMaterialImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("物料类型不能为空")
            .MaximumLength(4).WithMessage("物料类型长度不能超过4个字符");
        RuleFor(x => x.MaterialGroup)
            .NotEmpty().WithMessage("物料组不能为空")
            .MaximumLength(9).WithMessage("物料组长度不能超过9个字符");
        RuleFor(x => x.InternalMaterialCode)
            .NotEmpty().WithMessage("内部物料编码不能为空")
            .MaximumLength(20).WithMessage("内部物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.SellerMaterialCode)
            .NotEmpty().WithMessage("销售商物料编码不能为空")
            .MaximumLength(40).WithMessage("销售商物料编码长度不能超过40个字符");
        RuleFor(x => x.SellerMaterialDescription)
            .NotEmpty().WithMessage("销售商物料描述不能为空")
            .MaximumLength(40).WithMessage("销售商物料描述长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
