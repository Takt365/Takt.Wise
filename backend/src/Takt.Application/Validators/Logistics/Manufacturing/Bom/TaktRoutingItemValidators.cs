// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItemValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：RoutingItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktRoutingItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Validators.Logistics.Manufacturing.Bom;

// ========================================
// 创建RoutingItem 验证器
// ========================================

/// <summary>
/// 创建RoutingItem DTO 验证器
/// </summary>
public class TaktRoutingItemCreateValidator : AbstractValidator<TaktRoutingItemCreateDto>
{
    /// <summary>
    /// 初始化 创建RoutingItem 校验规则
    /// </summary>
    public TaktRoutingItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.RoutingId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.RoutingId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.RoutingId)
            .GreaterThanOrEqualTo(0).WithMessage("工艺路线主表ID不能为负数");
        RuleFor(x => x.RoutingCode)
            .NotEmpty().WithMessage("工艺路线编码不能为空").When(x => x.RoutingId <= 0)
            .MaximumLength(8).WithMessage("工艺路线编码长度不能超过8个字符");
        RuleFor(x => x.BaseUnit)
            .NotEmpty().WithMessage("作业/工序计量单位不能为空")
            .MaximumLength(10).WithMessage("作业/工序计量单位长度不能超过10个字符");
        RuleFor(x => x.TimeUnit)
            .NotEmpty().WithMessage("工时单位不能为空")
            .MaximumLength(3).WithMessage("工时单位长度不能超过3个字符");
        RuleFor(x => x.PointsUnit)
            .NotEmpty().WithMessage("点数单位不能为空")
            .MaximumLength(5).WithMessage("点数单位长度不能超过5个字符");
        RuleFor(x => x.PointsToMinutesRate)
            .NotEmpty().WithMessage("点数转分钟汇率不能为空")
            .MaximumLength(10).WithMessage("点数转分钟汇率长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新RoutingItem 验证器
// ========================================

/// <summary>
/// 更新RoutingItem DTO 验证器
/// </summary>
public class TaktRoutingItemUpdateValidator : AbstractValidator<TaktRoutingItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新RoutingItem 校验规则
    /// </summary>
    public TaktRoutingItemUpdateValidator()
    {
        RuleFor(x => x.RoutingItemId)
            .GreaterThan(0).WithMessage("RoutingItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.RoutingId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.RoutingId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.RoutingId)
            .GreaterThanOrEqualTo(0).WithMessage("工艺路线主表ID不能为负数");
        RuleFor(x => x.RoutingCode)
            .NotEmpty().WithMessage("工艺路线编码不能为空").When(x => x.RoutingId <= 0)
            .MaximumLength(8).WithMessage("工艺路线编码长度不能超过8个字符");
        RuleFor(x => x.BaseUnit)
            .NotEmpty().WithMessage("作业/工序计量单位不能为空")
            .MaximumLength(10).WithMessage("作业/工序计量单位长度不能超过10个字符");
        RuleFor(x => x.TimeUnit)
            .NotEmpty().WithMessage("工时单位不能为空")
            .MaximumLength(3).WithMessage("工时单位长度不能超过3个字符");
        RuleFor(x => x.PointsUnit)
            .NotEmpty().WithMessage("点数单位不能为空")
            .MaximumLength(5).WithMessage("点数单位长度不能超过5个字符");
        RuleFor(x => x.PointsToMinutesRate)
            .NotEmpty().WithMessage("点数转分钟汇率不能为空")
            .MaximumLength(10).WithMessage("点数转分钟汇率长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入RoutingItem 验证器
// ========================================

/// <summary>
/// 导入RoutingItem DTO 验证器
/// </summary>
public class TaktRoutingItemImportValidator : AbstractValidator<TaktRoutingItemImportDto>
{
    /// <summary>
    /// 初始化 导入RoutingItem 校验规则
    /// </summary>
    public TaktRoutingItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.RoutingId)
            .GreaterThanOrEqualTo(0).WithMessage("工艺路线主表ID不能为负数");
        RuleFor(x => x.RoutingCode)
            .NotEmpty().WithMessage("工艺路线编码不能为空")
            .MaximumLength(8).WithMessage("工艺路线编码长度不能超过8个字符");
        RuleFor(x => x.BaseUnit)
            .NotEmpty().WithMessage("作业/工序计量单位不能为空")
            .MaximumLength(10).WithMessage("作业/工序计量单位长度不能超过10个字符");
        RuleFor(x => x.TimeUnit)
            .NotEmpty().WithMessage("工时单位不能为空")
            .MaximumLength(3).WithMessage("工时单位长度不能超过3个字符");
        RuleFor(x => x.PointsUnit)
            .NotEmpty().WithMessage("点数单位不能为空")
            .MaximumLength(5).WithMessage("点数单位长度不能超过5个字符");
        RuleFor(x => x.PointsToMinutesRate)
            .NotEmpty().WithMessage("点数转分钟汇率不能为空")
            .MaximumLength(10).WithMessage("点数转分钟汇率长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
