// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktMaterialGroupValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialGroup 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaterialGroup 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建MaterialGroup 验证器
// ========================================

/// <summary>
/// 创建MaterialGroup DTO 验证器
/// </summary>
public class TaktMaterialGroupCreateValidator : AbstractValidator<TaktMaterialGroupCreateDto>
{
    /// <summary>
    /// 初始化 创建MaterialGroup 校验规则
    /// </summary>
    public TaktMaterialGroupCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.MaterialGroupCode)
            .NotEmpty().WithMessage("物料组编码不能为空")
            .MaximumLength(6).WithMessage("物料组编码长度不能超过6个字符");
        RuleFor(x => x.MaterialGroupName)
            .NotEmpty().WithMessage("物料组名称不能为空")
            .MaximumLength(80).WithMessage("物料组名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MaterialGroup 验证器
// ========================================

/// <summary>
/// 更新MaterialGroup DTO 验证器
/// </summary>
public class TaktMaterialGroupUpdateValidator : AbstractValidator<TaktMaterialGroupUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaterialGroup 校验规则
    /// </summary>
    public TaktMaterialGroupUpdateValidator()
    {
        RuleFor(x => x.MaterialGroupId)
            .GreaterThan(0).WithMessage("MaterialGroupID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.MaterialGroupCode)
            .NotEmpty().WithMessage("物料组编码不能为空")
            .MaximumLength(6).WithMessage("物料组编码长度不能超过6个字符");
        RuleFor(x => x.MaterialGroupName)
            .NotEmpty().WithMessage("物料组名称不能为空")
            .MaximumLength(80).WithMessage("物料组名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入MaterialGroup 验证器
// ========================================

/// <summary>
/// 导入MaterialGroup DTO 验证器
/// </summary>
public class TaktMaterialGroupImportValidator : AbstractValidator<TaktMaterialGroupImportDto>
{
    /// <summary>
    /// 初始化 导入MaterialGroup 校验规则
    /// </summary>
    public TaktMaterialGroupImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.MaterialGroupCode)
            .NotEmpty().WithMessage("物料组编码不能为空")
            .MaximumLength(6).WithMessage("物料组编码长度不能超过6个字符");
        RuleFor(x => x.MaterialGroupName)
            .NotEmpty().WithMessage("物料组名称不能为空")
            .MaximumLength(80).WithMessage("物料组名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
