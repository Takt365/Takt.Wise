// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Materials
// 文件名称：TaktMaterialDocumentItemValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialDocumentItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktMaterialDocumentItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Materials;

namespace Takt.Application.Validators.Logistics.Materials;

// ========================================
// 创建MaterialDocumentItem 验证器
// ========================================

/// <summary>
/// 创建MaterialDocumentItem DTO 验证器
/// </summary>
public class TaktMaterialDocumentItemCreateValidator : AbstractValidator<TaktMaterialDocumentItemCreateDto>
{
    /// <summary>
    /// 初始化 创建MaterialDocumentItem 校验规则
    /// </summary>
    public TaktMaterialDocumentItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.MaterialDocumentId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.MaterialDocumentId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.MaterialDocumentId)
            .GreaterThanOrEqualTo(0).WithMessage("物料凭证ID不能为负数");
        RuleFor(x => x.MaterialDocumentCode)
            .NotEmpty().WithMessage("物料凭证不能为空").When(x => x.MaterialDocumentId <= 0)
            .MaximumLength(10).WithMessage("物料凭证长度不能超过10个字符");
        RuleFor(x => x.MovementType)
            .NotEmpty().WithMessage("移动类型不能为空")
            .MaximumLength(3).WithMessage("移动类型长度不能超过3个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料不能为空")
            .MaximumLength(20).WithMessage("物料长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新MaterialDocumentItem 验证器
// ========================================

/// <summary>
/// 更新MaterialDocumentItem DTO 验证器
/// </summary>
public class TaktMaterialDocumentItemUpdateValidator : AbstractValidator<TaktMaterialDocumentItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新MaterialDocumentItem 校验规则
    /// </summary>
    public TaktMaterialDocumentItemUpdateValidator()
    {
        RuleFor(x => x.MaterialDocumentItemId)
            .GreaterThan(0).WithMessage("MaterialDocumentItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.MaterialDocumentId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.MaterialDocumentId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.MaterialDocumentId)
            .GreaterThanOrEqualTo(0).WithMessage("物料凭证ID不能为负数");
        RuleFor(x => x.MaterialDocumentCode)
            .NotEmpty().WithMessage("物料凭证不能为空").When(x => x.MaterialDocumentId <= 0)
            .MaximumLength(10).WithMessage("物料凭证长度不能超过10个字符");
        RuleFor(x => x.MovementType)
            .NotEmpty().WithMessage("移动类型不能为空")
            .MaximumLength(3).WithMessage("移动类型长度不能超过3个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料不能为空")
            .MaximumLength(20).WithMessage("物料长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入MaterialDocumentItem 验证器
// ========================================

/// <summary>
/// 导入MaterialDocumentItem DTO 验证器
/// </summary>
public class TaktMaterialDocumentItemImportValidator : AbstractValidator<TaktMaterialDocumentItemImportDto>
{
    /// <summary>
    /// 初始化 导入MaterialDocumentItem 校验规则
    /// </summary>
    public TaktMaterialDocumentItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.MaterialDocumentId)
            .GreaterThanOrEqualTo(0).WithMessage("物料凭证ID不能为负数");
        RuleFor(x => x.MaterialDocumentCode)
            .NotEmpty().WithMessage("物料凭证不能为空")
            .MaximumLength(10).WithMessage("物料凭证长度不能超过10个字符");
        RuleFor(x => x.MovementType)
            .NotEmpty().WithMessage("移动类型不能为空")
            .MaximumLength(3).WithMessage("移动类型长度不能超过3个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料不能为空")
            .MaximumLength(20).WithMessage("物料长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
