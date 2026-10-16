// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Statistics.QuickQuery
// 文件名称：TaktConfigurableSelectionValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：ConfigurableSelection 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktConfigurableSelection 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Statistics.QuickQuery;

namespace Takt.Application.Validators.Statistics.QuickQuery;

// ========================================
// 创建ConfigurableSelection 验证器
// ========================================

/// <summary>
/// 创建ConfigurableSelection DTO 验证器
/// </summary>
public class TaktConfigurableSelectionCreateValidator : AbstractValidator<TaktConfigurableSelectionCreateDto>
{
    /// <summary>
    /// 初始化 创建ConfigurableSelection 校验规则
    /// </summary>
    public TaktConfigurableSelectionCreateValidator()
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
        RuleFor(x => x.ConfigurableId)
            .GreaterThanOrEqualTo(0).WithMessage("关联定制报表主表 ID不能为负数");
        RuleFor(x => x.SourceAlias)
            .NotEmpty().WithMessage("数据源别名不能为空")
            .MaximumLength(10).WithMessage("数据源别名长度不能超过10个字符");
        RuleFor(x => x.ColumnName)
            .NotEmpty().WithMessage("列名不能为空")
            .MaximumLength(128).WithMessage("列名长度不能超过128个字符");
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("显示名称不能为空")
            .MaximumLength(100).WithMessage("显示名称长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ConfigurableSelection 验证器
// ========================================

/// <summary>
/// 更新ConfigurableSelection DTO 验证器
/// </summary>
public class TaktConfigurableSelectionUpdateValidator : AbstractValidator<TaktConfigurableSelectionUpdateDto>
{
    /// <summary>
    /// 初始化 更新ConfigurableSelection 校验规则
    /// </summary>
    public TaktConfigurableSelectionUpdateValidator()
    {
        RuleFor(x => x.ConfigurableSelectionId)
            .GreaterThan(0).WithMessage("ConfigurableSelectionID无效");
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
        RuleFor(x => x.ConfigurableId)
            .GreaterThanOrEqualTo(0).WithMessage("关联定制报表主表 ID不能为负数");
        RuleFor(x => x.SourceAlias)
            .NotEmpty().WithMessage("数据源别名不能为空")
            .MaximumLength(10).WithMessage("数据源别名长度不能超过10个字符");
        RuleFor(x => x.ColumnName)
            .NotEmpty().WithMessage("列名不能为空")
            .MaximumLength(128).WithMessage("列名长度不能超过128个字符");
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("显示名称不能为空")
            .MaximumLength(100).WithMessage("显示名称长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入ConfigurableSelection 验证器
// ========================================

/// <summary>
/// 导入ConfigurableSelection DTO 验证器
/// </summary>
public class TaktConfigurableSelectionImportValidator : AbstractValidator<TaktConfigurableSelectionImportDto>
{
    /// <summary>
    /// 初始化 导入ConfigurableSelection 校验规则
    /// </summary>
    public TaktConfigurableSelectionImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ConfigurableId)
            .GreaterThanOrEqualTo(0).WithMessage("关联定制报表主表 ID不能为负数");
        RuleFor(x => x.SourceAlias)
            .NotEmpty().WithMessage("数据源别名不能为空")
            .MaximumLength(10).WithMessage("数据源别名长度不能超过10个字符");
        RuleFor(x => x.ColumnName)
            .NotEmpty().WithMessage("列名不能为空")
            .MaximumLength(128).WithMessage("列名长度不能超过128个字符");
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("显示名称不能为空")
            .MaximumLength(100).WithMessage("显示名称长度不能超过100个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
