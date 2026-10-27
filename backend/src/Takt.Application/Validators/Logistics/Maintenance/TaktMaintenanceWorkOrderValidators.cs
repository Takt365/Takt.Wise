// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：MaintenanceWorkOrder 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaintenanceWorkOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Maintenance;

namespace Takt.Application.Validators.Logistics.Maintenance;

// ========================================
// 创建MaintenanceWorkOrder 验证器
// ========================================

/// <summary>
/// 创建MaintenanceWorkOrder DTO 验证器
/// </summary>
public class TaktMaintenanceWorkOrderCreateValidator : AbstractValidator<TaktMaintenanceWorkOrderCreateDto>
{
    /// <summary>
    /// 初始化 创建MaintenanceWorkOrder 校验规则
    /// </summary>
    public TaktMaintenanceWorkOrderCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.EquipmentId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.EquipmentId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.WorkOrderCode)
            .NotEmpty().WithMessage("维护工单号不能为空")
            .MaximumLength(50).WithMessage("维护工单号长度不能超过50个字符");
        RuleFor(x => x.MaintenanceNotificationId)
            .GreaterThanOrEqualTo(0).WithMessage("来源维护通知单ID不能为负数");
        RuleFor(x => x.EquipmentId)
            .GreaterThanOrEqualTo(0).WithMessage("设备ID不能为负数");
        RuleFor(x => x.EquipCode)
            .NotEmpty().WithMessage("设备编码不能为空")
            .MaximumLength(18).WithMessage("设备编码长度不能超过18个字符");
        RuleFor(x => x.EquipmentName)
            .NotEmpty().WithMessage("设备名称不能为空").When(x => x.EquipmentId <= 0)
            .MaximumLength(200).WithMessage("设备名称长度不能超过200个字符");
        RuleFor(x => x.CostCenterId)
            .GreaterThanOrEqualTo(0).WithMessage("结算成本中心ID不能为负数");
        RuleFor(x => x.CostElementId)
            .GreaterThanOrEqualTo(0).WithMessage("成本要素ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MaintenanceWorkOrder 验证器
// ========================================

/// <summary>
/// 更新MaintenanceWorkOrder DTO 验证器
/// </summary>
public class TaktMaintenanceWorkOrderUpdateValidator : AbstractValidator<TaktMaintenanceWorkOrderUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaintenanceWorkOrder 校验规则
    /// </summary>
    public TaktMaintenanceWorkOrderUpdateValidator()
    {
        RuleFor(x => x.MaintenanceWorkOrderId)
            .GreaterThan(0).WithMessage("MaintenanceWorkOrderID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.EquipmentId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.EquipmentId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.WorkOrderCode)
            .NotEmpty().WithMessage("维护工单号不能为空")
            .MaximumLength(50).WithMessage("维护工单号长度不能超过50个字符");
        RuleFor(x => x.MaintenanceNotificationId)
            .GreaterThanOrEqualTo(0).WithMessage("来源维护通知单ID不能为负数");
        RuleFor(x => x.EquipmentId)
            .GreaterThanOrEqualTo(0).WithMessage("设备ID不能为负数");
        RuleFor(x => x.EquipCode)
            .NotEmpty().WithMessage("设备编码不能为空")
            .MaximumLength(18).WithMessage("设备编码长度不能超过18个字符");
        RuleFor(x => x.EquipmentName)
            .NotEmpty().WithMessage("设备名称不能为空").When(x => x.EquipmentId <= 0)
            .MaximumLength(200).WithMessage("设备名称长度不能超过200个字符");
        RuleFor(x => x.CostCenterId)
            .GreaterThanOrEqualTo(0).WithMessage("结算成本中心ID不能为负数");
        RuleFor(x => x.CostElementId)
            .GreaterThanOrEqualTo(0).WithMessage("成本要素ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入MaintenanceWorkOrder 验证器
// ========================================

/// <summary>
/// 导入MaintenanceWorkOrder DTO 验证器
/// </summary>
public class TaktMaintenanceWorkOrderImportValidator : AbstractValidator<TaktMaintenanceWorkOrderImportDto>
{
    /// <summary>
    /// 初始化 导入MaintenanceWorkOrder 校验规则
    /// </summary>
    public TaktMaintenanceWorkOrderImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.WorkOrderCode)
            .NotEmpty().WithMessage("维护工单号不能为空")
            .MaximumLength(50).WithMessage("维护工单号长度不能超过50个字符");
        RuleFor(x => x.MaintenanceNotificationId)
            .GreaterThanOrEqualTo(0).WithMessage("来源维护通知单ID不能为负数");
        RuleFor(x => x.EquipmentId)
            .GreaterThanOrEqualTo(0).WithMessage("设备ID不能为负数");
        RuleFor(x => x.EquipCode)
            .NotEmpty().WithMessage("设备编码不能为空")
            .MaximumLength(18).WithMessage("设备编码长度不能超过18个字符");
        RuleFor(x => x.EquipmentName)
            .NotEmpty().WithMessage("设备名称不能为空")
            .MaximumLength(200).WithMessage("设备名称长度不能超过200个字符");
        RuleFor(x => x.CostCenterId)
            .GreaterThanOrEqualTo(0).WithMessage("结算成本中心ID不能为负数");
        RuleFor(x => x.CostElementId)
            .GreaterThanOrEqualTo(0).WithMessage("成本要素ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
