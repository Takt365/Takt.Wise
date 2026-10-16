// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：IqcOrder 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktIqcOrder 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Operation;

namespace Takt.Application.Validators.Logistics.Quality.Operation;

// ========================================
// 创建IqcOrder 验证器
// ========================================

/// <summary>
/// 创建IqcOrder DTO 验证器
/// </summary>
public class TaktIqcOrderCreateValidator : AbstractValidator<TaktIqcOrderCreateDto>
{
    /// <summary>
    /// 初始化 创建IqcOrder 校验规则
    /// </summary>
    public TaktIqcOrderCreateValidator()
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
        RuleFor(x => x.SourceCode)
            .NotEmpty().WithMessage("来源单号不能为空")
            .MaximumLength(20).WithMessage("来源单号长度不能超过20个字符");
        RuleFor(x => x.IqcOrderCode)
            .NotEmpty().WithMessage("IQC检验单编码不能为空")
            .MaximumLength(20).WithMessage("IQC检验单编码长度不能超过20个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(10).WithMessage("供应商编码长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新IqcOrder 验证器
// ========================================

/// <summary>
/// 更新IqcOrder DTO 验证器
/// </summary>
public class TaktIqcOrderUpdateValidator : AbstractValidator<TaktIqcOrderUpdateDto>
{
    /// <summary>
    /// 初始化 更新IqcOrder 校验规则
    /// </summary>
    public TaktIqcOrderUpdateValidator()
    {
        RuleFor(x => x.IqcOrderId)
            .GreaterThan(0).WithMessage("IqcOrderID无效");
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
        RuleFor(x => x.SourceCode)
            .NotEmpty().WithMessage("来源单号不能为空")
            .MaximumLength(20).WithMessage("来源单号长度不能超过20个字符");
        RuleFor(x => x.IqcOrderCode)
            .NotEmpty().WithMessage("IQC检验单编码不能为空")
            .MaximumLength(20).WithMessage("IQC检验单编码长度不能超过20个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(10).WithMessage("供应商编码长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入IqcOrder 验证器
// ========================================

/// <summary>
/// 导入IqcOrder DTO 验证器
/// </summary>
public class TaktIqcOrderImportValidator : AbstractValidator<TaktIqcOrderImportDto>
{
    /// <summary>
    /// 初始化 导入IqcOrder 校验规则
    /// </summary>
    public TaktIqcOrderImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SourceCode)
            .NotEmpty().WithMessage("来源单号不能为空")
            .MaximumLength(20).WithMessage("来源单号长度不能超过20个字符");
        RuleFor(x => x.IqcOrderCode)
            .NotEmpty().WithMessage("IQC检验单编码不能为空")
            .MaximumLength(20).WithMessage("IQC检验单编码长度不能超过20个字符");
        RuleFor(x => x.SupplierCode)
            .NotEmpty().WithMessage("供应商编码不能为空")
            .MaximumLength(10).WithMessage("供应商编码长度不能超过10个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
