// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcBukanValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：EcBukan 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEcBukan 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Validators.Logistics.Manufacturing.EngineeringChange;

// ========================================
// 创建EcBukan 验证器
// ========================================

/// <summary>
/// 创建EcBukan DTO 验证器
/// </summary>
public class TaktEcBukanCreateValidator : AbstractValidator<TaktEcBukanCreateDto>
{
    /// <summary>
    /// 初始化 创建EcBukan 校验规则
    /// </summary>
    public TaktEcBukanCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.EcDetailId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.EcDetailId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EcCode)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.DiscontinuedStatus)
            .NotEmpty().WithMessage("停产状态不能为空")
            .MaximumLength(4).WithMessage("停产状态长度不能超过4个字符");
        RuleFor(x => x.EcModelCodeGroup)
            .NotEmpty().WithMessage("机种编码组不能为空");
        RuleFor(x => x.EcRootMaterialCodeGroup)
            .NotEmpty().WithMessage("根物料编码组不能为空");
        RuleFor(x => x.EcRootMaterialDescription)
            .NotEmpty().WithMessage("根物料描述不能为空")
            .MaximumLength(40).WithMessage("根物料描述长度不能超过40个字符");
        RuleFor(x => x.EcNewMaterialCode)
            .NotEmpty().WithMessage("新物料编码不能为空")
            .MaximumLength(20).WithMessage("新物料编码长度不能超过20个字符");
        RuleFor(x => x.EcNewMaterialDescription)
            .NotEmpty().WithMessage("新物料描述不能为空")
            .MaximumLength(40).WithMessage("新物料描述长度不能超过40个字符");
        RuleFor(x => x.EcNewPurchaseType)
            .NotEmpty().WithMessage("新采购类型不能为空")
            .MaximumLength(1).WithMessage("新采购类型长度不能超过1个字符");
        RuleFor(x => x.EcNewWarehouse)
            .NotEmpty().WithMessage("新品仓库不能为空")
            .MaximumLength(4).WithMessage("新品仓库长度不能超过4个字符");
        RuleFor(x => x.DeptCode)
            .NotEmpty().WithMessage("部门编码不能为空")
            .MaximumLength(6).WithMessage("部门编码长度不能超过6个字符");
        RuleFor(x => x.DeptName)
            .NotEmpty().WithMessage("部门名称不能为空")
            .MaximumLength(40).WithMessage("部门名称长度不能超过40个字符");
        RuleFor(x => x.EcDetailId)
            .GreaterThanOrEqualTo(0).WithMessage("设变明细 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EcBukan 验证器
// ========================================

/// <summary>
/// 更新EcBukan DTO 验证器
/// </summary>
public class TaktEcBukanUpdateValidator : AbstractValidator<TaktEcBukanUpdateDto>
{
    /// <summary>
    /// 初始化 更新EcBukan 校验规则
    /// </summary>
    public TaktEcBukanUpdateValidator()
    {
        RuleFor(x => x.EcBukanId)
            .GreaterThan(0).WithMessage("EcBukanID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.EcDetailId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.EcDetailId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EcCode)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.DiscontinuedStatus)
            .NotEmpty().WithMessage("停产状态不能为空")
            .MaximumLength(4).WithMessage("停产状态长度不能超过4个字符");
        RuleFor(x => x.EcModelCodeGroup)
            .NotEmpty().WithMessage("机种编码组不能为空");
        RuleFor(x => x.EcRootMaterialCodeGroup)
            .NotEmpty().WithMessage("根物料编码组不能为空");
        RuleFor(x => x.EcRootMaterialDescription)
            .NotEmpty().WithMessage("根物料描述不能为空")
            .MaximumLength(40).WithMessage("根物料描述长度不能超过40个字符");
        RuleFor(x => x.EcNewMaterialCode)
            .NotEmpty().WithMessage("新物料编码不能为空")
            .MaximumLength(20).WithMessage("新物料编码长度不能超过20个字符");
        RuleFor(x => x.EcNewMaterialDescription)
            .NotEmpty().WithMessage("新物料描述不能为空")
            .MaximumLength(40).WithMessage("新物料描述长度不能超过40个字符");
        RuleFor(x => x.EcNewPurchaseType)
            .NotEmpty().WithMessage("新采购类型不能为空")
            .MaximumLength(1).WithMessage("新采购类型长度不能超过1个字符");
        RuleFor(x => x.EcNewWarehouse)
            .NotEmpty().WithMessage("新品仓库不能为空")
            .MaximumLength(4).WithMessage("新品仓库长度不能超过4个字符");
        RuleFor(x => x.DeptCode)
            .NotEmpty().WithMessage("部门编码不能为空")
            .MaximumLength(6).WithMessage("部门编码长度不能超过6个字符");
        RuleFor(x => x.DeptName)
            .NotEmpty().WithMessage("部门名称不能为空")
            .MaximumLength(40).WithMessage("部门名称长度不能超过40个字符");
        RuleFor(x => x.EcDetailId)
            .GreaterThanOrEqualTo(0).WithMessage("设变明细 ID不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入EcBukan 验证器
// ========================================

/// <summary>
/// 导入EcBukan DTO 验证器
/// </summary>
public class TaktEcBukanImportValidator : AbstractValidator<TaktEcBukanImportDto>
{
    /// <summary>
    /// 初始化 导入EcBukan 校验规则
    /// </summary>
    public TaktEcBukanImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.EcDetailId)
            .GreaterThanOrEqualTo(0).WithMessage("设变明细 ID不能为负数");
        RuleFor(x => x.EcCode)
            .NotEmpty().WithMessage("设变单号不能为空")
            .MaximumLength(10).WithMessage("设变单号长度不能超过10个字符");
        RuleFor(x => x.EcModelCodeGroup)
            .NotEmpty().WithMessage("机种编码组不能为空");
        RuleFor(x => x.EcRootMaterialCodeGroup)
            .NotEmpty().WithMessage("根物料编码组不能为空");
        RuleFor(x => x.EcRootMaterialDescription)
            .NotEmpty().WithMessage("根物料描述不能为空")
            .MaximumLength(40).WithMessage("根物料描述长度不能超过40个字符");
        RuleFor(x => x.DiscontinuedStatus)
            .NotEmpty().WithMessage("停产状态不能为空")
            .MaximumLength(4).WithMessage("停产状态长度不能超过4个字符");
        RuleFor(x => x.EcNewMaterialCode)
            .NotEmpty().WithMessage("新物料编码不能为空")
            .MaximumLength(20).WithMessage("新物料编码长度不能超过20个字符");
        RuleFor(x => x.EcNewMaterialDescription)
            .NotEmpty().WithMessage("新物料描述不能为空")
            .MaximumLength(40).WithMessage("新物料描述长度不能超过40个字符");
        RuleFor(x => x.EcNewPurchaseType)
            .NotEmpty().WithMessage("新采购类型不能为空")
            .MaximumLength(1).WithMessage("新采购类型长度不能超过1个字符");
        RuleFor(x => x.EcNewWarehouse)
            .NotEmpty().WithMessage("新品仓库不能为空")
            .MaximumLength(4).WithMessage("新品仓库长度不能超过4个字符");
        RuleFor(x => x.DeptCode)
            .NotEmpty().WithMessage("部门编码不能为空")
            .MaximumLength(6).WithMessage("部门编码长度不能超过6个字符");
        RuleFor(x => x.DeptName)
            .NotEmpty().WithMessage("部门名称不能为空")
            .MaximumLength(40).WithMessage("部门名称长度不能超过40个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
