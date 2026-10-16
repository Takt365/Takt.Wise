// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetailValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：PcbaOutputDetail 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPcbaOutputDetail 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;

namespace Takt.Application.Validators.Logistics.Manufacturing.Output;

// ========================================
// 创建PcbaOutputDetail 验证器
// ========================================

/// <summary>
/// 创建PcbaOutputDetail DTO 验证器
/// </summary>
public class TaktPcbaOutputDetailCreateValidator : AbstractValidator<TaktPcbaOutputDetailCreateDto>
{
    /// <summary>
    /// 初始化 创建PcbaOutputDetail 校验规则
    /// </summary>
    public TaktPcbaOutputDetailCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.PcbaOutputId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.PcbaOutputId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.PcbaOutputId)
            .GreaterThanOrEqualTo(0).WithMessage("PCBA日报ID不能为负数");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("工单号不能为空")
            .MaximumLength(12).WithMessage("工单号长度不能超过12个字符");
        RuleFor(x => x.TimePeriod)
            .NotEmpty().WithMessage("生产时段不能为空")
            .MaximumLength(20).WithMessage("生产时段长度不能超过20个字符");
        RuleFor(x => x.TeamCode)
            .NotEmpty().WithMessage("生产班组不能为空")
            .MaximumLength(8).WithMessage("生产班组长度不能超过8个字符");
        RuleFor(x => x.ProdEquipCode)
            .NotEmpty().WithMessage("生产设备编码不能为空")
            .MaximumLength(18).WithMessage("生产设备编码长度不能超过18个字符");
        RuleFor(x => x.PcbBoardType)
            .NotEmpty().WithMessage("PCB板别不能为空")
            .MaximumLength(40).WithMessage("PCB板别长度不能超过40个字符");
        RuleFor(x => x.PanelSide)
            .NotEmpty().WithMessage("面板别不能为空")
            .MaximumLength(40).WithMessage("面板别长度不能超过40个字符");
        RuleFor(x => x.SerialCode)
            .NotEmpty().WithMessage("序列号不能为空")
            .MaximumLength(80).WithMessage("序列号长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新PcbaOutputDetail 验证器
// ========================================

/// <summary>
/// 更新PcbaOutputDetail DTO 验证器
/// </summary>
public class TaktPcbaOutputDetailUpdateValidator : AbstractValidator<TaktPcbaOutputDetailUpdateDto>
{
    /// <summary>
    /// 初始化 更新PcbaOutputDetail 校验规则
    /// </summary>
    public TaktPcbaOutputDetailUpdateValidator()
    {
        RuleFor(x => x.PcbaOutputDetailId)
            .GreaterThan(0).WithMessage("PcbaOutputDetailID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.PcbaOutputId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.PcbaOutputId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.PcbaOutputId)
            .GreaterThanOrEqualTo(0).WithMessage("PCBA日报ID不能为负数");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("工单号不能为空")
            .MaximumLength(12).WithMessage("工单号长度不能超过12个字符");
        RuleFor(x => x.TimePeriod)
            .NotEmpty().WithMessage("生产时段不能为空")
            .MaximumLength(20).WithMessage("生产时段长度不能超过20个字符");
        RuleFor(x => x.TeamCode)
            .NotEmpty().WithMessage("生产班组不能为空")
            .MaximumLength(8).WithMessage("生产班组长度不能超过8个字符");
        RuleFor(x => x.ProdEquipCode)
            .NotEmpty().WithMessage("生产设备编码不能为空")
            .MaximumLength(18).WithMessage("生产设备编码长度不能超过18个字符");
        RuleFor(x => x.PcbBoardType)
            .NotEmpty().WithMessage("PCB板别不能为空")
            .MaximumLength(40).WithMessage("PCB板别长度不能超过40个字符");
        RuleFor(x => x.PanelSide)
            .NotEmpty().WithMessage("面板别不能为空")
            .MaximumLength(40).WithMessage("面板别长度不能超过40个字符");
        RuleFor(x => x.SerialCode)
            .NotEmpty().WithMessage("序列号不能为空")
            .MaximumLength(80).WithMessage("序列号长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入PcbaOutputDetail 验证器
// ========================================

/// <summary>
/// 导入PcbaOutputDetail DTO 验证器
/// </summary>
public class TaktPcbaOutputDetailImportValidator : AbstractValidator<TaktPcbaOutputDetailImportDto>
{
    /// <summary>
    /// 初始化 导入PcbaOutputDetail 校验规则
    /// </summary>
    public TaktPcbaOutputDetailImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.PcbaOutputId)
            .GreaterThanOrEqualTo(0).WithMessage("PCBA日报ID不能为负数");
        RuleFor(x => x.ProdOrderCode)
            .NotEmpty().WithMessage("工单号不能为空")
            .MaximumLength(12).WithMessage("工单号长度不能超过12个字符");
        RuleFor(x => x.TimePeriod)
            .NotEmpty().WithMessage("生产时段不能为空")
            .MaximumLength(20).WithMessage("生产时段长度不能超过20个字符");
        RuleFor(x => x.TeamCode)
            .NotEmpty().WithMessage("生产班组不能为空")
            .MaximumLength(8).WithMessage("生产班组长度不能超过8个字符");
        RuleFor(x => x.ProdEquipCode)
            .NotEmpty().WithMessage("生产设备编码不能为空")
            .MaximumLength(18).WithMessage("生产设备编码长度不能超过18个字符");
        RuleFor(x => x.PcbBoardType)
            .NotEmpty().WithMessage("PCB板别不能为空")
            .MaximumLength(40).WithMessage("PCB板别长度不能超过40个字符");
        RuleFor(x => x.PanelSide)
            .NotEmpty().WithMessage("面板别不能为空")
            .MaximumLength(40).WithMessage("面板别长度不能超过40个字符");
        RuleFor(x => x.SerialCode)
            .NotEmpty().WithMessage("序列号不能为空")
            .MaximumLength(80).WithMessage("序列号长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
