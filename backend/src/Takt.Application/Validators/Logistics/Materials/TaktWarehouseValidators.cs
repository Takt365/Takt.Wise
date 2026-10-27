// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktWarehouseValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：Warehouse 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktWarehouse 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建Warehouse 验证器
// ========================================

/// <summary>
/// 创建Warehouse DTO 验证器
/// </summary>
public class TaktWarehouseCreateValidator : AbstractValidator<TaktWarehouseCreateDto>
{
    /// <summary>
    /// 初始化 创建Warehouse 校验规则
    /// </summary>
    public TaktWarehouseCreateValidator()
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
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("存货地点编码不能为空")
            .MaximumLength(4).WithMessage("存货地点编码长度不能超过4个字符");
        RuleFor(x => x.WarehouseName)
            .NotEmpty().WithMessage("仓库名称不能为空")
            .MaximumLength(80).WithMessage("仓库名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Warehouse 验证器
// ========================================

/// <summary>
/// 更新Warehouse DTO 验证器
/// </summary>
public class TaktWarehouseUpdateValidator : AbstractValidator<TaktWarehouseUpdateDto>
{
    /// <summary>
    /// 初始化 更新Warehouse 校验规则
    /// </summary>
    public TaktWarehouseUpdateValidator()
    {
        RuleFor(x => x.WarehouseId)
            .GreaterThan(0).WithMessage("WarehouseID无效");
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
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("存货地点编码不能为空")
            .MaximumLength(4).WithMessage("存货地点编码长度不能超过4个字符");
        RuleFor(x => x.WarehouseName)
            .NotEmpty().WithMessage("仓库名称不能为空")
            .MaximumLength(80).WithMessage("仓库名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Warehouse 验证器
// ========================================

/// <summary>
/// 导入Warehouse DTO 验证器
/// </summary>
public class TaktWarehouseImportValidator : AbstractValidator<TaktWarehouseImportDto>
{
    /// <summary>
    /// 初始化 导入Warehouse 校验规则
    /// </summary>
    public TaktWarehouseImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.WarehouseCode)
            .NotEmpty().WithMessage("存货地点编码不能为空")
            .MaximumLength(4).WithMessage("存货地点编码长度不能超过4个字符");
        RuleFor(x => x.WarehouseName)
            .NotEmpty().WithMessage("仓库名称不能为空")
            .MaximumLength(80).WithMessage("仓库名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
