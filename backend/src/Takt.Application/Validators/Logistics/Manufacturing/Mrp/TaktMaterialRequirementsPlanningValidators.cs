// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Mrp
// 文件名称：TaktMaterialRequirementsPlanningValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialRequirementsPlanning 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaterialRequirementsPlanning 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;

namespace Takt.Application.Validators.Logistics.Manufacturing.Mrp;

// ========================================
// 创建MaterialRequirementsPlanning 验证器
// ========================================

/// <summary>
/// 创建MaterialRequirementsPlanning DTO 验证器
/// </summary>
public class TaktMaterialRequirementsPlanningCreateValidator : AbstractValidator<TaktMaterialRequirementsPlanningCreateDto>
{
    /// <summary>
    /// 初始化 创建MaterialRequirementsPlanning 校验规则
    /// </summary>
    public TaktMaterialRequirementsPlanningCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.PurchasePlanId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.PurchasePlanId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.MaterialRequirementsPlanningCode)
            .NotEmpty().WithMessage("MRP 编码不能为空")
            .MaximumLength(20).WithMessage("MRP 编码长度不能超过20个字符");
        RuleFor(x => x.MasterProductionScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MPS 头表 ID不能为负数");
        RuleFor(x => x.MasterDemandScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MDS 头表 ID不能为负数");
        RuleFor(x => x.ProductionPlanId)
            .GreaterThanOrEqualTo(0).WithMessage("产出生产计划 ID不能为负数");
        RuleFor(x => x.PurchasePlanId)
            .GreaterThanOrEqualTo(0).WithMessage("产出采购计划 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MaterialRequirementsPlanning 验证器
// ========================================

/// <summary>
/// 更新MaterialRequirementsPlanning DTO 验证器
/// </summary>
public class TaktMaterialRequirementsPlanningUpdateValidator : AbstractValidator<TaktMaterialRequirementsPlanningUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaterialRequirementsPlanning 校验规则
    /// </summary>
    public TaktMaterialRequirementsPlanningUpdateValidator()
    {
        RuleFor(x => x.MaterialRequirementsPlanningId)
            .GreaterThan(0).WithMessage("MaterialRequirementsPlanningID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.PurchasePlanId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.PurchasePlanId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.MaterialRequirementsPlanningCode)
            .NotEmpty().WithMessage("MRP 编码不能为空")
            .MaximumLength(20).WithMessage("MRP 编码长度不能超过20个字符");
        RuleFor(x => x.MasterProductionScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MPS 头表 ID不能为负数");
        RuleFor(x => x.MasterDemandScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MDS 头表 ID不能为负数");
        RuleFor(x => x.ProductionPlanId)
            .GreaterThanOrEqualTo(0).WithMessage("产出生产计划 ID不能为负数");
        RuleFor(x => x.PurchasePlanId)
            .GreaterThanOrEqualTo(0).WithMessage("产出采购计划 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入MaterialRequirementsPlanning 验证器
// ========================================

/// <summary>
/// 导入MaterialRequirementsPlanning DTO 验证器
/// </summary>
public class TaktMaterialRequirementsPlanningImportValidator : AbstractValidator<TaktMaterialRequirementsPlanningImportDto>
{
    /// <summary>
    /// 初始化 导入MaterialRequirementsPlanning 校验规则
    /// </summary>
    public TaktMaterialRequirementsPlanningImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.MaterialRequirementsPlanningCode)
            .NotEmpty().WithMessage("MRP 编码不能为空")
            .MaximumLength(20).WithMessage("MRP 编码长度不能超过20个字符");
        RuleFor(x => x.MasterProductionScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MPS 头表 ID不能为负数");
        RuleFor(x => x.MasterDemandScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MDS 头表 ID不能为负数");
        RuleFor(x => x.ProductionPlanId)
            .GreaterThanOrEqualTo(0).WithMessage("产出生产计划 ID不能为负数");
        RuleFor(x => x.PurchasePlanId)
            .GreaterThanOrEqualTo(0).WithMessage("产出采购计划 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
