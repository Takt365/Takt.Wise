// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionEquipmentValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionEquipment 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktProductionEquipment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Mps;

namespace Takt.Application.Validators.Logistics.Manufacturing.Mps;

// ========================================
// 创建ProductionEquipment 验证器
// ========================================

/// <summary>
/// 创建ProductionEquipment DTO 验证器
/// </summary>
public class TaktProductionEquipmentCreateValidator : AbstractValidator<TaktProductionEquipmentCreateDto>
{
    /// <summary>
    /// 初始化 创建ProductionEquipment 校验规则
    /// </summary>
    public TaktProductionEquipmentCreateValidator()
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
        RuleFor(x => x.ProdEquipCode)
            .NotEmpty().WithMessage("生产设备编码不能为空")
            .MaximumLength(18).WithMessage("生产设备编码长度不能超过18个字符");
        RuleFor(x => x.ProdEquipName)
            .NotEmpty().WithMessage("生产设备名称不能为空")
            .MaximumLength(40).WithMessage("生产设备名称长度不能超过40个字符");
        RuleFor(x => x.MachineType)
            .NotEmpty().WithMessage("机型名称不能为空")
            .MaximumLength(40).WithMessage("机型名称长度不能超过40个字符");
        RuleFor(x => x.StorageLocation)
            .NotEmpty().WithMessage("存放位置不能为空")
            .MaximumLength(40).WithMessage("存放位置长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ProductionEquipment 验证器
// ========================================

/// <summary>
/// 更新ProductionEquipment DTO 验证器
/// </summary>
public class TaktProductionEquipmentUpdateValidator : AbstractValidator<TaktProductionEquipmentUpdateDto>
{
    /// <summary>
    /// 初始化 更新ProductionEquipment 校验规则
    /// </summary>
    public TaktProductionEquipmentUpdateValidator()
    {
        RuleFor(x => x.ProductionEquipmentId)
            .GreaterThan(0).WithMessage("ProductionEquipmentID无效");
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
        RuleFor(x => x.ProdEquipCode)
            .NotEmpty().WithMessage("生产设备编码不能为空")
            .MaximumLength(18).WithMessage("生产设备编码长度不能超过18个字符");
        RuleFor(x => x.ProdEquipName)
            .NotEmpty().WithMessage("生产设备名称不能为空")
            .MaximumLength(40).WithMessage("生产设备名称长度不能超过40个字符");
        RuleFor(x => x.MachineType)
            .NotEmpty().WithMessage("机型名称不能为空")
            .MaximumLength(40).WithMessage("机型名称长度不能超过40个字符");
        RuleFor(x => x.StorageLocation)
            .NotEmpty().WithMessage("存放位置不能为空")
            .MaximumLength(40).WithMessage("存放位置长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入ProductionEquipment 验证器
// ========================================

/// <summary>
/// 导入ProductionEquipment DTO 验证器
/// </summary>
public class TaktProductionEquipmentImportValidator : AbstractValidator<TaktProductionEquipmentImportDto>
{
    /// <summary>
    /// 初始化 导入ProductionEquipment 校验规则
    /// </summary>
    public TaktProductionEquipmentImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ProdEquipCode)
            .NotEmpty().WithMessage("生产设备编码不能为空")
            .MaximumLength(18).WithMessage("生产设备编码长度不能超过18个字符");
        RuleFor(x => x.ProdEquipName)
            .NotEmpty().WithMessage("生产设备名称不能为空")
            .MaximumLength(40).WithMessage("生产设备名称长度不能超过40个字符");
        RuleFor(x => x.MachineType)
            .NotEmpty().WithMessage("机型名称不能为空")
            .MaximumLength(40).WithMessage("机型名称长度不能超过40个字符");
        RuleFor(x => x.StorageLocation)
            .NotEmpty().WithMessage("存放位置不能为空")
            .MaximumLength(40).WithMessage("存放位置长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
