// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Accounting.Financial
// 文件名称：TaktAssetValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：Asset 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktAsset 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Accounting.Financial;

namespace Takt.Application.Validators.Accounting.Financial;

// ========================================
// 创建Asset 验证器
// ========================================

/// <summary>
/// 创建Asset DTO 验证器
/// </summary>
public class TaktAssetCreateValidator : AbstractValidator<TaktAssetCreateDto>
{
    /// <summary>
    /// 初始化 创建Asset 校验规则
    /// </summary>
    public TaktAssetCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.AssetCode)
            .NotEmpty().WithMessage("资产代码不能为空")
            .MaximumLength(50).WithMessage("资产代码长度不能超过50个字符");
        RuleFor(x => x.AssetName)
            .NotEmpty().WithMessage("资产名称不能为空")
            .MaximumLength(200).WithMessage("资产名称长度不能超过200个字符");
        RuleFor(x => x.AssetCategory)
            .NotEmpty().WithMessage("资产分类不能为空")
            .MaximumLength(8).WithMessage("资产分类长度不能超过8个字符");
        RuleFor(x => x.AssetType)
            .NotEmpty().WithMessage("资产类型不能为空")
            .MaximumLength(4).WithMessage("资产类型长度不能超过4个字符");
        RuleFor(x => x.CostCenterId)
            .GreaterThanOrEqualTo(0).WithMessage("成本中心不能为负数");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("部门不能为负数");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("使用者不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Asset 验证器
// ========================================

/// <summary>
/// 更新Asset DTO 验证器
/// </summary>
public class TaktAssetUpdateValidator : AbstractValidator<TaktAssetUpdateDto>
{
    /// <summary>
    /// 初始化 更新Asset 校验规则
    /// </summary>
    public TaktAssetUpdateValidator()
    {
        RuleFor(x => x.AssetId)
            .GreaterThan(0).WithMessage("AssetID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.AssetCode)
            .NotEmpty().WithMessage("资产代码不能为空")
            .MaximumLength(50).WithMessage("资产代码长度不能超过50个字符");
        RuleFor(x => x.AssetName)
            .NotEmpty().WithMessage("资产名称不能为空")
            .MaximumLength(200).WithMessage("资产名称长度不能超过200个字符");
        RuleFor(x => x.AssetCategory)
            .NotEmpty().WithMessage("资产分类不能为空")
            .MaximumLength(8).WithMessage("资产分类长度不能超过8个字符");
        RuleFor(x => x.AssetType)
            .NotEmpty().WithMessage("资产类型不能为空")
            .MaximumLength(4).WithMessage("资产类型长度不能超过4个字符");
        RuleFor(x => x.CostCenterId)
            .GreaterThanOrEqualTo(0).WithMessage("成本中心不能为负数");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("部门不能为负数");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("使用者不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Asset 验证器
// ========================================

/// <summary>
/// 导入Asset DTO 验证器
/// </summary>
public class TaktAssetImportValidator : AbstractValidator<TaktAssetImportDto>
{
    /// <summary>
    /// 初始化 导入Asset 校验规则
    /// </summary>
    public TaktAssetImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.AssetCode)
            .NotEmpty().WithMessage("资产代码不能为空")
            .MaximumLength(50).WithMessage("资产代码长度不能超过50个字符");
        RuleFor(x => x.AssetName)
            .NotEmpty().WithMessage("资产名称不能为空")
            .MaximumLength(200).WithMessage("资产名称长度不能超过200个字符");
        RuleFor(x => x.AssetCategory)
            .NotEmpty().WithMessage("资产分类不能为空")
            .MaximumLength(8).WithMessage("资产分类长度不能超过8个字符");
        RuleFor(x => x.AssetType)
            .NotEmpty().WithMessage("资产类型不能为空")
            .MaximumLength(4).WithMessage("资产类型长度不能超过4个字符");
        RuleFor(x => x.CostCenterId)
            .GreaterThanOrEqualTo(0).WithMessage("成本中心不能为负数");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("部门不能为负数");
        RuleFor(x => x.UserId)
            .GreaterThanOrEqualTo(0).WithMessage("使用者不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
