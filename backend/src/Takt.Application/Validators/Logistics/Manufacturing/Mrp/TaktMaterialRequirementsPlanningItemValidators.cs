// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Mrp
// 文件名称：TaktMaterialRequirementsPlanningItemValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialRequirementsPlanningItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaterialRequirementsPlanningItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;

namespace Takt.Application.Validators.Logistics.Manufacturing.Mrp;

// ========================================
// 创建MaterialRequirementsPlanningItem 验证器
// ========================================

/// <summary>
/// 创建MaterialRequirementsPlanningItem DTO 验证器
/// </summary>
public class TaktMaterialRequirementsPlanningItemCreateValidator : AbstractValidator<TaktMaterialRequirementsPlanningItemCreateDto>
{
    /// <summary>
    /// 初始化 创建MaterialRequirementsPlanningItem 校验规则
    /// </summary>
    public TaktMaterialRequirementsPlanningItemCreateValidator()
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
        RuleFor(x => x.MaterialRequirementsPlanningId)
            .GreaterThanOrEqualTo(0).WithMessage("MRP 头表 ID不能为负数");
        RuleFor(x => x.MaterialRequirementsPlanningCode)
            .NotEmpty().WithMessage("MRP 编码不能为空").When(x => x.MaterialRequirementsPlanningId <= 0)
            .MaximumLength(20).WithMessage("MRP 编码长度不能超过20个字符");
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
// 更新MaterialRequirementsPlanningItem 验证器
// ========================================

/// <summary>
/// 更新MaterialRequirementsPlanningItem DTO 验证器
/// </summary>
public class TaktMaterialRequirementsPlanningItemUpdateValidator : AbstractValidator<TaktMaterialRequirementsPlanningItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaterialRequirementsPlanningItem 校验规则
    /// </summary>
    public TaktMaterialRequirementsPlanningItemUpdateValidator()
    {
        RuleFor(x => x.MaterialRequirementsPlanningItemId)
            .GreaterThan(0).WithMessage("MaterialRequirementsPlanningItemID无效");
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
        RuleFor(x => x.MaterialRequirementsPlanningId)
            .GreaterThanOrEqualTo(0).WithMessage("MRP 头表 ID不能为负数");
        RuleFor(x => x.MaterialRequirementsPlanningCode)
            .NotEmpty().WithMessage("MRP 编码不能为空").When(x => x.MaterialRequirementsPlanningId <= 0)
            .MaximumLength(20).WithMessage("MRP 编码长度不能超过20个字符");
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
// 导入MaterialRequirementsPlanningItem 验证器
// ========================================

/// <summary>
/// 导入MaterialRequirementsPlanningItem DTO 验证器
/// </summary>
public class TaktMaterialRequirementsPlanningItemImportValidator : AbstractValidator<TaktMaterialRequirementsPlanningItemImportDto>
{
    /// <summary>
    /// 初始化 导入MaterialRequirementsPlanningItem 校验规则
    /// </summary>
    public TaktMaterialRequirementsPlanningItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.MaterialRequirementsPlanningId)
            .GreaterThanOrEqualTo(0).WithMessage("MRP 头表 ID不能为负数");
        RuleFor(x => x.MaterialRequirementsPlanningCode)
            .NotEmpty().WithMessage("MRP 编码不能为空")
            .MaximumLength(20).WithMessage("MRP 编码长度不能超过20个字符");
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
