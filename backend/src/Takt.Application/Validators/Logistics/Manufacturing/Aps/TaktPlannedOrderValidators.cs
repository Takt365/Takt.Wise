// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Aps
// 文件名称：TaktPlannedOrderValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：PlannedOrder 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPlannedOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;

namespace Takt.Application.Validators.Logistics.Manufacturing.Aps;

// ========================================
// 创建PlannedOrder 验证器
// ========================================

/// <summary>
/// 创建PlannedOrder DTO 验证器
/// </summary>
public class TaktPlannedOrderCreateValidator : AbstractValidator<TaktPlannedOrderCreateDto>
{
    /// <summary>
    /// 初始化 创建PlannedOrder 校验规则
    /// </summary>
    public TaktPlannedOrderCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.MaterialRequirementsPlanningId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.MaterialRequirementsPlanningId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.PlannedOrderCode)
            .NotEmpty().WithMessage("计划订单编码不能为空")
            .MaximumLength(12).WithMessage("计划订单编码长度不能超过12个字符");
        RuleFor(x => x.MaterialRequirementsPlanningId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MRP 头表 ID不能为负数");
        RuleFor(x => x.MaterialRequirementsPlanningItemId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MRP 明细行 ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(40).WithMessage("计量单位长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PlannedOrder 验证器
// ========================================

/// <summary>
/// 更新PlannedOrder DTO 验证器
/// </summary>
public class TaktPlannedOrderUpdateValidator : AbstractValidator<TaktPlannedOrderUpdateDto>
{
    /// <summary>
    /// 初始化 更新PlannedOrder 校验规则
    /// </summary>
    public TaktPlannedOrderUpdateValidator()
    {
        RuleFor(x => x.PlannedOrderId)
            .GreaterThan(0).WithMessage("PlannedOrderID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.MaterialRequirementsPlanningId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.MaterialRequirementsPlanningId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.PlannedOrderCode)
            .NotEmpty().WithMessage("计划订单编码不能为空")
            .MaximumLength(12).WithMessage("计划订单编码长度不能超过12个字符");
        RuleFor(x => x.MaterialRequirementsPlanningId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MRP 头表 ID不能为负数");
        RuleFor(x => x.MaterialRequirementsPlanningItemId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MRP 明细行 ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(40).WithMessage("计量单位长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入PlannedOrder 验证器
// ========================================

/// <summary>
/// 导入PlannedOrder DTO 验证器
/// </summary>
public class TaktPlannedOrderImportValidator : AbstractValidator<TaktPlannedOrderImportDto>
{
    /// <summary>
    /// 初始化 导入PlannedOrder 校验规则
    /// </summary>
    public TaktPlannedOrderImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.PlannedOrderCode)
            .NotEmpty().WithMessage("计划订单编码不能为空")
            .MaximumLength(12).WithMessage("计划订单编码长度不能超过12个字符");
        RuleFor(x => x.MaterialRequirementsPlanningId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MRP 头表 ID不能为负数");
        RuleFor(x => x.MaterialRequirementsPlanningItemId)
            .GreaterThanOrEqualTo(0).WithMessage("来源 MRP 明细行 ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(40).WithMessage("计量单位长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
