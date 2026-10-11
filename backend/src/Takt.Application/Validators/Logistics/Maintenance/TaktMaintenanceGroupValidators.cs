// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Maintenance
// 文件名称：TaktMaintenanceGroupValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：MaintenanceGroup 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaintenanceGroup 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Maintenance;

namespace Takt.Application.Validators.Logistics.Maintenance;

// ========================================
// 创建MaintenanceGroup 验证器
// ========================================

/// <summary>
/// 创建MaintenanceGroup DTO 验证器
/// </summary>
public class TaktMaintenanceGroupCreateValidator : AbstractValidator<TaktMaintenanceGroupCreateDto>
{
    /// <summary>
    /// 初始化 创建MaintenanceGroup 校验规则
    /// </summary>
    public TaktMaintenanceGroupCreateValidator()
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
        RuleFor(x => x.MaintenanceGroupCode)
            .NotEmpty().WithMessage("设备维护组编码不能为空")
            .MaximumLength(6).WithMessage("设备维护组编码长度不能超过6个字符");
        RuleFor(x => x.MaintenanceGroupName)
            .NotEmpty().WithMessage("设备维护组名称不能为空")
            .MaximumLength(80).WithMessage("设备维护组名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MaintenanceGroup 验证器
// ========================================

/// <summary>
/// 更新MaintenanceGroup DTO 验证器
/// </summary>
public class TaktMaintenanceGroupUpdateValidator : AbstractValidator<TaktMaintenanceGroupUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaintenanceGroup 校验规则
    /// </summary>
    public TaktMaintenanceGroupUpdateValidator()
    {
        RuleFor(x => x.MaintenanceGroupId)
            .GreaterThan(0).WithMessage("MaintenanceGroupID无效");
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
        RuleFor(x => x.MaintenanceGroupCode)
            .NotEmpty().WithMessage("设备维护组编码不能为空")
            .MaximumLength(6).WithMessage("设备维护组编码长度不能超过6个字符");
        RuleFor(x => x.MaintenanceGroupName)
            .NotEmpty().WithMessage("设备维护组名称不能为空")
            .MaximumLength(80).WithMessage("设备维护组名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入MaintenanceGroup 验证器
// ========================================

/// <summary>
/// 导入MaintenanceGroup DTO 验证器
/// </summary>
public class TaktMaintenanceGroupImportValidator : AbstractValidator<TaktMaintenanceGroupImportDto>
{
    /// <summary>
    /// 初始化 导入MaintenanceGroup 校验规则
    /// </summary>
    public TaktMaintenanceGroupImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.MaintenanceGroupCode)
            .NotEmpty().WithMessage("设备维护组编码不能为空")
            .MaximumLength(6).WithMessage("设备维护组编码长度不能超过6个字符");
        RuleFor(x => x.MaintenanceGroupName)
            .NotEmpty().WithMessage("设备维护组名称不能为空")
            .MaximumLength(80).WithMessage("设备维护组名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
