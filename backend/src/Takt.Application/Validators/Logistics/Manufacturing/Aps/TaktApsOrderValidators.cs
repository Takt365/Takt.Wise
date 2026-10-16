// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Aps
// 文件名称：TaktApsOrderValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：ApsOrder 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktApsOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;

namespace Takt.Application.Validators.Logistics.Manufacturing.Aps;

// ========================================
// 创建ApsOrder 验证器
// ========================================

/// <summary>
/// 创建ApsOrder DTO 验证器
/// </summary>
public class TaktApsOrderCreateValidator : AbstractValidator<TaktApsOrderCreateDto>
{
    /// <summary>
    /// 初始化 创建ApsOrder 校验规则
    /// </summary>
    public TaktApsOrderCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.PlannedOrderId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.PlannedOrderId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ApsOrderCode)
            .NotEmpty().WithMessage("APS 订单编码不能为空")
            .MaximumLength(12).WithMessage("APS 订单编码长度不能超过12个字符");
        RuleFor(x => x.PlannedOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("来源计划订单 ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(40).WithMessage("计量单位长度不能超过40个字符");
        RuleFor(x => x.ApsScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("关联 APS 排程批次 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ApsOrder 验证器
// ========================================

/// <summary>
/// 更新ApsOrder DTO 验证器
/// </summary>
public class TaktApsOrderUpdateValidator : AbstractValidator<TaktApsOrderUpdateDto>
{
    /// <summary>
    /// 初始化 更新ApsOrder 校验规则
    /// </summary>
    public TaktApsOrderUpdateValidator()
    {
        RuleFor(x => x.ApsOrderId)
            .GreaterThan(0).WithMessage("ApsOrderID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.PlannedOrderId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.PlannedOrderId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ApsOrderCode)
            .NotEmpty().WithMessage("APS 订单编码不能为空")
            .MaximumLength(12).WithMessage("APS 订单编码长度不能超过12个字符");
        RuleFor(x => x.PlannedOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("来源计划订单 ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(40).WithMessage("计量单位长度不能超过40个字符");
        RuleFor(x => x.ApsScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("关联 APS 排程批次 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入ApsOrder 验证器
// ========================================

/// <summary>
/// 导入ApsOrder DTO 验证器
/// </summary>
public class TaktApsOrderImportValidator : AbstractValidator<TaktApsOrderImportDto>
{
    /// <summary>
    /// 初始化 导入ApsOrder 校验规则
    /// </summary>
    public TaktApsOrderImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ApsOrderCode)
            .NotEmpty().WithMessage("APS 订单编码不能为空")
            .MaximumLength(12).WithMessage("APS 订单编码长度不能超过12个字符");
        RuleFor(x => x.PlannedOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("来源计划订单 ID不能为负数");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.UnitOfMeasure)
            .NotEmpty().WithMessage("计量单位不能为空")
            .MaximumLength(40).WithMessage("计量单位长度不能超过40个字符");
        RuleFor(x => x.ApsScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("关联 APS 排程批次 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
