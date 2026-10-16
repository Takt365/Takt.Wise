// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：SopExec 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSopExec 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;

namespace Takt.Application.Validators.Logistics.Manufacturing.Sop;

// ========================================
// 创建SopExec 验证器
// ========================================

/// <summary>
/// 创建SopExec DTO 验证器
/// </summary>
public class TaktSopExecCreateValidator : AbstractValidator<TaktSopExecCreateDto>
{
    /// <summary>
    /// 初始化 创建SopExec 校验规则
    /// </summary>
    public TaktSopExecCreateValidator()
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
        RuleFor(x => x.ProductionOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("生产工单 ID不能为负数");
        RuleFor(x => x.WorkOrderCode)
            .NotEmpty().WithMessage("MES 工单号不能为空")
            .MaximumLength(50).WithMessage("MES 工单号长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("产品/机种物料编码不能为空")
            .MaximumLength(20).WithMessage("产品/机种物料编码长度不能超过20个字符");
        RuleFor(x => x.RoutingItemId)
            .GreaterThanOrEqualTo(0).WithMessage("工序 ID不能为负数");
        RuleFor(x => x.WorkstationId)
            .GreaterThanOrEqualTo(0).WithMessage("工位 ID不能为负数");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工 ID不能为负数");
        RuleFor(x => x.SopId)
            .GreaterThanOrEqualTo(0).WithMessage("SOP 主档 ID不能为负数");
        RuleFor(x => x.RevisionId)
            .GreaterThanOrEqualTo(0).WithMessage("SOP 版本 ID不能为负数");
        RuleFor(x => x.Revision)
            .NotEmpty().WithMessage("版本号快照不能为空")
            .MaximumLength(20).WithMessage("版本号快照长度不能超过20个字符");
        RuleFor(x => x.CurrentStepId)
            .GreaterThanOrEqualTo(0).WithMessage("当前工步 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SopExec 验证器
// ========================================

/// <summary>
/// 更新SopExec DTO 验证器
/// </summary>
public class TaktSopExecUpdateValidator : AbstractValidator<TaktSopExecUpdateDto>
{
    /// <summary>
    /// 初始化 更新SopExec 校验规则
    /// </summary>
    public TaktSopExecUpdateValidator()
    {
        RuleFor(x => x.SopExecId)
            .GreaterThan(0).WithMessage("SopExecID无效");
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
        RuleFor(x => x.ProductionOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("生产工单 ID不能为负数");
        RuleFor(x => x.WorkOrderCode)
            .NotEmpty().WithMessage("MES 工单号不能为空")
            .MaximumLength(50).WithMessage("MES 工单号长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("产品/机种物料编码不能为空")
            .MaximumLength(20).WithMessage("产品/机种物料编码长度不能超过20个字符");
        RuleFor(x => x.RoutingItemId)
            .GreaterThanOrEqualTo(0).WithMessage("工序 ID不能为负数");
        RuleFor(x => x.WorkstationId)
            .GreaterThanOrEqualTo(0).WithMessage("工位 ID不能为负数");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工 ID不能为负数");
        RuleFor(x => x.SopId)
            .GreaterThanOrEqualTo(0).WithMessage("SOP 主档 ID不能为负数");
        RuleFor(x => x.RevisionId)
            .GreaterThanOrEqualTo(0).WithMessage("SOP 版本 ID不能为负数");
        RuleFor(x => x.Revision)
            .NotEmpty().WithMessage("版本号快照不能为空")
            .MaximumLength(20).WithMessage("版本号快照长度不能超过20个字符");
        RuleFor(x => x.CurrentStepId)
            .GreaterThanOrEqualTo(0).WithMessage("当前工步 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SopExec 验证器
// ========================================

/// <summary>
/// 导入SopExec DTO 验证器
/// </summary>
public class TaktSopExecImportValidator : AbstractValidator<TaktSopExecImportDto>
{
    /// <summary>
    /// 初始化 导入SopExec 校验规则
    /// </summary>
    public TaktSopExecImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ProductionOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("生产工单 ID不能为负数");
        RuleFor(x => x.WorkOrderCode)
            .NotEmpty().WithMessage("MES 工单号不能为空")
            .MaximumLength(50).WithMessage("MES 工单号长度不能超过50个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("产品/机种物料编码不能为空")
            .MaximumLength(20).WithMessage("产品/机种物料编码长度不能超过20个字符");
        RuleFor(x => x.RoutingItemId)
            .GreaterThanOrEqualTo(0).WithMessage("工序 ID不能为负数");
        RuleFor(x => x.WorkstationId)
            .GreaterThanOrEqualTo(0).WithMessage("工位 ID不能为负数");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工 ID不能为负数");
        RuleFor(x => x.SopId)
            .GreaterThanOrEqualTo(0).WithMessage("SOP 主档 ID不能为负数");
        RuleFor(x => x.RevisionId)
            .GreaterThanOrEqualTo(0).WithMessage("SOP 版本 ID不能为负数");
        RuleFor(x => x.Revision)
            .NotEmpty().WithMessage("版本号快照不能为空")
            .MaximumLength(20).WithMessage("版本号快照长度不能超过20个字符");
        RuleFor(x => x.CurrentStepId)
            .GreaterThanOrEqualTo(0).WithMessage("当前工步 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
