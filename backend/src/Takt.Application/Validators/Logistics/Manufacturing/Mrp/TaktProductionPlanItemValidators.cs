// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Mrp
// 文件名称：TaktProductionPlanItemValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionPlanItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktProductionPlanItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;

namespace Takt.Application.Validators.Logistics.Manufacturing.Mrp;

// ========================================
// 创建ProductionPlanItem 验证器
// ========================================

/// <summary>
/// 创建ProductionPlanItem DTO 验证器
/// </summary>
public class TaktProductionPlanItemCreateValidator : AbstractValidator<TaktProductionPlanItemCreateDto>
{
    /// <summary>
    /// 初始化 创建ProductionPlanItem 校验规则
    /// </summary>
    public TaktProductionPlanItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.SalesForecastId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.SalesForecastId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ProductionPlanId)
            .GreaterThanOrEqualTo(0).WithMessage("生产计划ID不能为负数");
        RuleFor(x => x.ProductionPlanCode)
            .NotEmpty().WithMessage("生产计划编码不能为空").When(x => x.ProductionPlanId <= 0)
            .MaximumLength(10).WithMessage("生产计划编码长度不能超过10个字符");
        RuleFor(x => x.SalesForecastId)
            .GreaterThanOrEqualTo(0).WithMessage("来源销售计划ID不能为负数");
        RuleFor(x => x.MaterialRequirementsPlanningItemId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MRP 明细 ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.PlanUnit)
            .NotEmpty().WithMessage("计划单位不能为空")
            .MaximumLength(20).WithMessage("计划单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ProductionPlanItem 验证器
// ========================================

/// <summary>
/// 更新ProductionPlanItem DTO 验证器
/// </summary>
public class TaktProductionPlanItemUpdateValidator : AbstractValidator<TaktProductionPlanItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新ProductionPlanItem 校验规则
    /// </summary>
    public TaktProductionPlanItemUpdateValidator()
    {
        RuleFor(x => x.ProductionPlanItemId)
            .GreaterThan(0).WithMessage("ProductionPlanItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.SalesForecastId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.SalesForecastId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ProductionPlanId)
            .GreaterThanOrEqualTo(0).WithMessage("生产计划ID不能为负数");
        RuleFor(x => x.ProductionPlanCode)
            .NotEmpty().WithMessage("生产计划编码不能为空").When(x => x.ProductionPlanId <= 0)
            .MaximumLength(10).WithMessage("生产计划编码长度不能超过10个字符");
        RuleFor(x => x.SalesForecastId)
            .GreaterThanOrEqualTo(0).WithMessage("来源销售计划ID不能为负数");
        RuleFor(x => x.MaterialRequirementsPlanningItemId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MRP 明细 ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.PlanUnit)
            .NotEmpty().WithMessage("计划单位不能为空")
            .MaximumLength(20).WithMessage("计划单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入ProductionPlanItem 验证器
// ========================================

/// <summary>
/// 导入ProductionPlanItem DTO 验证器
/// </summary>
public class TaktProductionPlanItemImportValidator : AbstractValidator<TaktProductionPlanItemImportDto>
{
    /// <summary>
    /// 初始化 导入ProductionPlanItem 校验规则
    /// </summary>
    public TaktProductionPlanItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ProductionPlanId)
            .GreaterThanOrEqualTo(0).WithMessage("生产计划ID不能为负数");
        RuleFor(x => x.ProductionPlanCode)
            .NotEmpty().WithMessage("生产计划编码不能为空")
            .MaximumLength(10).WithMessage("生产计划编码长度不能超过10个字符");
        RuleFor(x => x.SalesForecastId)
            .GreaterThanOrEqualTo(0).WithMessage("来源销售计划ID不能为负数");
        RuleFor(x => x.MaterialRequirementsPlanningItemId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MRP 明细 ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(40).WithMessage("物料描述长度不能超过40个字符");
        RuleFor(x => x.PlanUnit)
            .NotEmpty().WithMessage("计划单位不能为空")
            .MaximumLength(20).WithMessage("计划单位长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
