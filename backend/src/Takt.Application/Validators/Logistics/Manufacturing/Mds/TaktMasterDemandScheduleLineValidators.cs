// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Mds
// 文件名称：TaktMasterDemandScheduleLineValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：MasterDemandScheduleLine 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMasterDemandScheduleLine 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Mds;

namespace Takt.Application.Validators.Logistics.Manufacturing.Mds;

// ========================================
// 创建MasterDemandScheduleLine 验证器
// ========================================

/// <summary>
/// 创建MasterDemandScheduleLine DTO 验证器
/// </summary>
public class TaktMasterDemandScheduleLineCreateValidator : AbstractValidator<TaktMasterDemandScheduleLineCreateDto>
{
    /// <summary>
    /// 初始化 创建MasterDemandScheduleLine 校验规则
    /// </summary>
    public TaktMasterDemandScheduleLineCreateValidator()
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
        RuleFor(x => x.MasterDemandScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("MDS 头表 ID不能为负数");
        RuleFor(x => x.MdsCode)
            .NotEmpty().WithMessage("MDS 编码不能为空")
            .MaximumLength(20).WithMessage("MDS 编码长度不能超过20个字符");
        RuleFor(x => x.SalesOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("来源销售订单 ID不能为负数");
        RuleFor(x => x.SalesForecastId)
            .GreaterThanOrEqualTo(0).WithMessage("来源销售预测 ID不能为负数");
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
// 更新MasterDemandScheduleLine 验证器
// ========================================

/// <summary>
/// 更新MasterDemandScheduleLine DTO 验证器
/// </summary>
public class TaktMasterDemandScheduleLineUpdateValidator : AbstractValidator<TaktMasterDemandScheduleLineUpdateDto>
{
    /// <summary>
    /// 初始化 更新MasterDemandScheduleLine 校验规则
    /// </summary>
    public TaktMasterDemandScheduleLineUpdateValidator()
    {
        RuleFor(x => x.MasterDemandScheduleLineId)
            .GreaterThan(0).WithMessage("MasterDemandScheduleLineID无效");
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
        RuleFor(x => x.MasterDemandScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("MDS 头表 ID不能为负数");
        RuleFor(x => x.MdsCode)
            .NotEmpty().WithMessage("MDS 编码不能为空")
            .MaximumLength(20).WithMessage("MDS 编码长度不能超过20个字符");
        RuleFor(x => x.SalesOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("来源销售订单 ID不能为负数");
        RuleFor(x => x.SalesForecastId)
            .GreaterThanOrEqualTo(0).WithMessage("来源销售预测 ID不能为负数");
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
// 导入MasterDemandScheduleLine 验证器
// ========================================

/// <summary>
/// 导入MasterDemandScheduleLine DTO 验证器
/// </summary>
public class TaktMasterDemandScheduleLineImportValidator : AbstractValidator<TaktMasterDemandScheduleLineImportDto>
{
    /// <summary>
    /// 初始化 导入MasterDemandScheduleLine 校验规则
    /// </summary>
    public TaktMasterDemandScheduleLineImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.MasterDemandScheduleId)
            .GreaterThanOrEqualTo(0).WithMessage("MDS 头表 ID不能为负数");
        RuleFor(x => x.MdsCode)
            .NotEmpty().WithMessage("MDS 编码不能为空")
            .MaximumLength(20).WithMessage("MDS 编码长度不能超过20个字符");
        RuleFor(x => x.SalesOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("来源销售订单 ID不能为负数");
        RuleFor(x => x.SalesForecastId)
            .GreaterThanOrEqualTo(0).WithMessage("来源销售预测 ID不能为负数");
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
