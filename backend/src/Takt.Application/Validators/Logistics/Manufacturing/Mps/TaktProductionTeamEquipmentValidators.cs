// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionTeamEquipmentValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionTeamEquipment 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktProductionTeamEquipment 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Mps;

namespace Takt.Application.Validators.Logistics.Manufacturing.Mps;

// ========================================
// 创建ProductionTeamEquipment 验证器
// ========================================

/// <summary>
/// 创建ProductionTeamEquipment DTO 验证器
/// </summary>
public class TaktProductionTeamEquipmentCreateValidator : AbstractValidator<TaktProductionTeamEquipmentCreateDto>
{
    /// <summary>
    /// 初始化 创建ProductionTeamEquipment 校验规则
    /// </summary>
    public TaktProductionTeamEquipmentCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ProdEquipId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ProdEquipId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ProdTeamId)
            .GreaterThanOrEqualTo(0).WithMessage("生产班组主键不能为负数");
        RuleFor(x => x.TeamCode)
            .NotEmpty().WithMessage("班组编码不能为空")
            .MaximumLength(8).WithMessage("班组编码长度不能超过8个字符");
        RuleFor(x => x.ProdEquipId)
            .GreaterThanOrEqualTo(0).WithMessage("生产设备主键不能为负数");
        RuleFor(x => x.ProdEquipCode)
            .NotEmpty().WithMessage("生产设备编码不能为空").When(x => x.ProdEquipId <= 0)
            .MaximumLength(18).WithMessage("生产设备编码长度不能超过18个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ProductionTeamEquipment 验证器
// ========================================

/// <summary>
/// 更新ProductionTeamEquipment DTO 验证器
/// </summary>
public class TaktProductionTeamEquipmentUpdateValidator : AbstractValidator<TaktProductionTeamEquipmentUpdateDto>
{
    /// <summary>
    /// 初始化 更新ProductionTeamEquipment 校验规则
    /// </summary>
    public TaktProductionTeamEquipmentUpdateValidator()
    {
        RuleFor(x => x.ProductionTeamEquipmentId)
            .GreaterThan(0).WithMessage("ProductionTeamEquipmentID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ProdEquipId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ProdEquipId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ProdTeamId)
            .GreaterThanOrEqualTo(0).WithMessage("生产班组主键不能为负数");
        RuleFor(x => x.TeamCode)
            .NotEmpty().WithMessage("班组编码不能为空")
            .MaximumLength(8).WithMessage("班组编码长度不能超过8个字符");
        RuleFor(x => x.ProdEquipId)
            .GreaterThanOrEqualTo(0).WithMessage("生产设备主键不能为负数");
        RuleFor(x => x.ProdEquipCode)
            .NotEmpty().WithMessage("生产设备编码不能为空").When(x => x.ProdEquipId <= 0)
            .MaximumLength(18).WithMessage("生产设备编码长度不能超过18个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入ProductionTeamEquipment 验证器
// ========================================

/// <summary>
/// 导入ProductionTeamEquipment DTO 验证器
/// </summary>
public class TaktProductionTeamEquipmentImportValidator : AbstractValidator<TaktProductionTeamEquipmentImportDto>
{
    /// <summary>
    /// 初始化 导入ProductionTeamEquipment 校验规则
    /// </summary>
    public TaktProductionTeamEquipmentImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ProdTeamId)
            .GreaterThanOrEqualTo(0).WithMessage("生产班组主键不能为负数");
        RuleFor(x => x.TeamCode)
            .NotEmpty().WithMessage("班组编码不能为空")
            .MaximumLength(8).WithMessage("班组编码长度不能超过8个字符");
        RuleFor(x => x.ProdEquipId)
            .GreaterThanOrEqualTo(0).WithMessage("生产设备主键不能为负数");
        RuleFor(x => x.ProdEquipCode)
            .NotEmpty().WithMessage("生产设备编码不能为空")
            .MaximumLength(18).WithMessage("生产设备编码长度不能超过18个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
