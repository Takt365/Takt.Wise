// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Workflow
// 文件名称：TaktFlowFormValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：FlowForm 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktFlowForm 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Workflow;

namespace Takt.Application.Validators.Workflow;

// ========================================
// 创建FlowForm 验证器
// ========================================

/// <summary>
/// 创建FlowForm DTO 验证器
/// </summary>
public class TaktFlowFormCreateValidator : AbstractValidator<TaktFlowFormCreateDto>
{
    /// <summary>
    /// 初始化 创建FlowForm 校验规则
    /// </summary>
    public TaktFlowFormCreateValidator()
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
        RuleFor(x => x.FormCode)
            .NotEmpty().WithMessage("表单编码不能为空")
            .MaximumLength(64).WithMessage("表单编码长度不能超过64个字符");
        RuleFor(x => x.FormName)
            .NotEmpty().WithMessage("表单名称不能为空")
            .MaximumLength(200).WithMessage("表单名称长度不能超过200个字符");
        RuleFor(x => x.FormVersion)
            .NotEmpty().WithMessage("表单版本标签不能为空")
            .MaximumLength(32).WithMessage("表单版本标签长度不能超过32个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新FlowForm 验证器
// ========================================

/// <summary>
/// 更新FlowForm DTO 验证器
/// </summary>
public class TaktFlowFormUpdateValidator : AbstractValidator<TaktFlowFormUpdateDto>
{
    /// <summary>
    /// 初始化 更新FlowForm 校验规则
    /// </summary>
    public TaktFlowFormUpdateValidator()
    {
        RuleFor(x => x.FlowFormId)
            .GreaterThan(0).WithMessage("FlowFormID无效");
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
        RuleFor(x => x.FormCode)
            .NotEmpty().WithMessage("表单编码不能为空")
            .MaximumLength(64).WithMessage("表单编码长度不能超过64个字符");
        RuleFor(x => x.FormName)
            .NotEmpty().WithMessage("表单名称不能为空")
            .MaximumLength(200).WithMessage("表单名称长度不能超过200个字符");
        RuleFor(x => x.FormVersion)
            .NotEmpty().WithMessage("表单版本标签不能为空")
            .MaximumLength(32).WithMessage("表单版本标签长度不能超过32个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入FlowForm 验证器
// ========================================

/// <summary>
/// 导入FlowForm DTO 验证器
/// </summary>
public class TaktFlowFormImportValidator : AbstractValidator<TaktFlowFormImportDto>
{
    /// <summary>
    /// 初始化 导入FlowForm 校验规则
    /// </summary>
    public TaktFlowFormImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.FormCode)
            .NotEmpty().WithMessage("表单编码不能为空")
            .MaximumLength(64).WithMessage("表单编码长度不能超过64个字符");
        RuleFor(x => x.FormName)
            .NotEmpty().WithMessage("表单名称不能为空")
            .MaximumLength(200).WithMessage("表单名称长度不能超过200个字符");
        RuleFor(x => x.FormVersion)
            .NotEmpty().WithMessage("表单版本标签不能为空")
            .MaximumLength(32).WithMessage("表单版本标签长度不能超过32个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
