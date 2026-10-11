// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Aps
// 文件名称：TaktApsOperationValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：ApsOperation 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktApsOperation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;

namespace Takt.Application.Validators.Logistics.Manufacturing.Aps;

// ========================================
// 创建ApsOperation 验证器
// ========================================

/// <summary>
/// 创建ApsOperation DTO 验证器
/// </summary>
public class TaktApsOperationCreateValidator : AbstractValidator<TaktApsOperationCreateDto>
{
    /// <summary>
    /// 初始化 创建ApsOperation 校验规则
    /// </summary>
    public TaktApsOperationCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ApsOrderId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ApsOrderId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ApsOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("APS 订单 ID不能为负数");
        RuleFor(x => x.ApsOrderCode)
            .NotEmpty().WithMessage("APS 订单编码不能为空").When(x => x.ApsOrderId <= 0)
            .MaximumLength(12).WithMessage("APS 订单编码长度不能超过12个字符");
        RuleFor(x => x.RoutingItemId)
            .GreaterThanOrEqualTo(0).WithMessage("工艺路线工序 ID不能为负数");
        RuleFor(x => x.ProcessCode)
            .NotEmpty().WithMessage("工序编码不能为空")
            .MaximumLength(4).WithMessage("工序编码长度不能超过4个字符");
        RuleFor(x => x.WorkCenterResourceId)
            .GreaterThanOrEqualTo(0).WithMessage("工作中心资源 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ApsOperation 验证器
// ========================================

/// <summary>
/// 更新ApsOperation DTO 验证器
/// </summary>
public class TaktApsOperationUpdateValidator : AbstractValidator<TaktApsOperationUpdateDto>
{
    /// <summary>
    /// 初始化 更新ApsOperation 校验规则
    /// </summary>
    public TaktApsOperationUpdateValidator()
    {
        RuleFor(x => x.ApsOperationId)
            .GreaterThan(0).WithMessage("ApsOperationID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ApsOrderId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ApsOrderId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ApsOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("APS 订单 ID不能为负数");
        RuleFor(x => x.ApsOrderCode)
            .NotEmpty().WithMessage("APS 订单编码不能为空").When(x => x.ApsOrderId <= 0)
            .MaximumLength(12).WithMessage("APS 订单编码长度不能超过12个字符");
        RuleFor(x => x.RoutingItemId)
            .GreaterThanOrEqualTo(0).WithMessage("工艺路线工序 ID不能为负数");
        RuleFor(x => x.ProcessCode)
            .NotEmpty().WithMessage("工序编码不能为空")
            .MaximumLength(4).WithMessage("工序编码长度不能超过4个字符");
        RuleFor(x => x.WorkCenterResourceId)
            .GreaterThanOrEqualTo(0).WithMessage("工作中心资源 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入ApsOperation 验证器
// ========================================

/// <summary>
/// 导入ApsOperation DTO 验证器
/// </summary>
public class TaktApsOperationImportValidator : AbstractValidator<TaktApsOperationImportDto>
{
    /// <summary>
    /// 初始化 导入ApsOperation 校验规则
    /// </summary>
    public TaktApsOperationImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ApsOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("APS 订单 ID不能为负数");
        RuleFor(x => x.ApsOrderCode)
            .NotEmpty().WithMessage("APS 订单编码不能为空")
            .MaximumLength(12).WithMessage("APS 订单编码长度不能超过12个字符");
        RuleFor(x => x.RoutingItemId)
            .GreaterThanOrEqualTo(0).WithMessage("工艺路线工序 ID不能为负数");
        RuleFor(x => x.ProcessCode)
            .NotEmpty().WithMessage("工序编码不能为空")
            .MaximumLength(4).WithMessage("工序编码长度不能超过4个字符");
        RuleFor(x => x.WorkCenterResourceId)
            .GreaterThanOrEqualTo(0).WithMessage("工作中心资源 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
