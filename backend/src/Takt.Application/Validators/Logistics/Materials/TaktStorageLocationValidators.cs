// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktStorageLocationValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：StorageLocation 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktStorageLocation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建StorageLocation 验证器
// ========================================

/// <summary>
/// 创建StorageLocation DTO 验证器
/// </summary>
public class TaktStorageLocationCreateValidator : AbstractValidator<TaktStorageLocationCreateDto>
{
    /// <summary>
    /// 初始化 创建StorageLocation 校验规则
    /// </summary>
    public TaktStorageLocationCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.WarehouseId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.WarehouseId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.WarehouseId)
            .GreaterThanOrEqualTo(0).WithMessage("仓库 ID不能为负数");
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("仓库编码不能为空").When(x => x.WarehouseId <= 0)
            .MaximumLength(4).WithMessage("仓库编码长度不能超过4个字符");
        RuleFor(x => x.LocationCode)
            .NotEmpty().WithMessage("库位编码不能为空")
            .MaximumLength(40).WithMessage("库位编码长度不能超过40个字符");
        RuleFor(x => x.LocationName)
            .NotEmpty().WithMessage("库位名称不能为空")
            .MaximumLength(100).WithMessage("库位名称长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新StorageLocation 验证器
// ========================================

/// <summary>
/// 更新StorageLocation DTO 验证器
/// </summary>
public class TaktStorageLocationUpdateValidator : AbstractValidator<TaktStorageLocationUpdateDto>
{
    /// <summary>
    /// 初始化 更新StorageLocation 校验规则
    /// </summary>
    public TaktStorageLocationUpdateValidator()
    {
        RuleFor(x => x.StorageLocationId)
            .GreaterThan(0).WithMessage("StorageLocationID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.WarehouseId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.WarehouseId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.WarehouseId)
            .GreaterThanOrEqualTo(0).WithMessage("仓库 ID不能为负数");
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("仓库编码不能为空").When(x => x.WarehouseId <= 0)
            .MaximumLength(4).WithMessage("仓库编码长度不能超过4个字符");
        RuleFor(x => x.LocationCode)
            .NotEmpty().WithMessage("库位编码不能为空")
            .MaximumLength(40).WithMessage("库位编码长度不能超过40个字符");
        RuleFor(x => x.LocationName)
            .NotEmpty().WithMessage("库位名称不能为空")
            .MaximumLength(100).WithMessage("库位名称长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入StorageLocation 验证器
// ========================================

/// <summary>
/// 导入StorageLocation DTO 验证器
/// </summary>
public class TaktStorageLocationImportValidator : AbstractValidator<TaktStorageLocationImportDto>
{
    /// <summary>
    /// 初始化 导入StorageLocation 校验规则
    /// </summary>
    public TaktStorageLocationImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.WarehouseId)
            .GreaterThanOrEqualTo(0).WithMessage("仓库 ID不能为负数");
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("仓库编码不能为空")
            .MaximumLength(4).WithMessage("仓库编码长度不能超过4个字符");
        RuleFor(x => x.LocationCode)
            .NotEmpty().WithMessage("库位编码不能为空")
            .MaximumLength(40).WithMessage("库位编码长度不能超过40个字符");
        RuleFor(x => x.LocationName)
            .NotEmpty().WithMessage("库位名称不能为空")
            .MaximumLength(100).WithMessage("库位名称长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
