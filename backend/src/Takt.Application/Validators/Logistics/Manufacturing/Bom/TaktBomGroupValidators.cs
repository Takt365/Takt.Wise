// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Manufacturing.Bom
// 文件名称：TaktBomGroupValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：BomGroup 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktBomGroup 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Validators.Logistics.Manufacturing.Bom;

// ========================================
// 创建BomGroup 验证器
// ========================================

/// <summary>
/// 创建BomGroup DTO 验证器
/// </summary>
public class TaktBomGroupCreateValidator : AbstractValidator<TaktBomGroupCreateDto>
{
    /// <summary>
    /// 初始化 创建BomGroup 校验规则
    /// </summary>
    public TaktBomGroupCreateValidator()
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
        RuleFor(x => x.BomGroupCode)
            .NotEmpty().WithMessage("BOM组编码不能为空")
            .MaximumLength(6).WithMessage("BOM组编码长度不能超过6个字符");
        RuleFor(x => x.BomGroupName)
            .NotEmpty().WithMessage("BOM组名称不能为空")
            .MaximumLength(80).WithMessage("BOM组名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新BomGroup 验证器
// ========================================

/// <summary>
/// 更新BomGroup DTO 验证器
/// </summary>
public class TaktBomGroupUpdateValidator : AbstractValidator<TaktBomGroupUpdateDto>
{
    /// <summary>
    /// 初始化 更新BomGroup 校验规则
    /// </summary>
    public TaktBomGroupUpdateValidator()
    {
        RuleFor(x => x.BomGroupId)
            .GreaterThan(0).WithMessage("BomGroupID无效");
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
        RuleFor(x => x.BomGroupCode)
            .NotEmpty().WithMessage("BOM组编码不能为空")
            .MaximumLength(6).WithMessage("BOM组编码长度不能超过6个字符");
        RuleFor(x => x.BomGroupName)
            .NotEmpty().WithMessage("BOM组名称不能为空")
            .MaximumLength(80).WithMessage("BOM组名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入BomGroup 验证器
// ========================================

/// <summary>
/// 导入BomGroup DTO 验证器
/// </summary>
public class TaktBomGroupImportValidator : AbstractValidator<TaktBomGroupImportDto>
{
    /// <summary>
    /// 初始化 导入BomGroup 校验规则
    /// </summary>
    public TaktBomGroupImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.BomGroupCode)
            .NotEmpty().WithMessage("BOM组编码不能为空")
            .MaximumLength(6).WithMessage("BOM组编码长度不能超过6个字符");
        RuleFor(x => x.BomGroupName)
            .NotEmpty().WithMessage("BOM组名称不能为空")
            .MaximumLength(80).WithMessage("BOM组名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
