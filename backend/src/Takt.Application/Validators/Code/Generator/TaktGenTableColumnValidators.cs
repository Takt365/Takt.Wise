// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Code.Generator
// 文件名称：TaktGenTableColumnValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：GenTableColumn 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktGenTableColumn 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Code.Generator;

namespace Takt.Application.Validators.Code.Generator;

// ========================================
// 创建GenTableColumn 验证器
// ========================================

/// <summary>
/// 创建GenTableColumn DTO 验证器
/// </summary>
public class TaktGenTableColumnCreateValidator : AbstractValidator<TaktGenTableColumnCreateDto>
{
    /// <summary>
    /// 初始化 创建GenTableColumn 校验规则
    /// </summary>
    public TaktGenTableColumnCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.GenTableId)
            .GreaterThanOrEqualTo(0).WithMessage("生成表ID不能为负数");
        RuleFor(x => x.DatabaseColumnName)
            .NotEmpty().WithMessage("数据库列名称不能为空")
            .MaximumLength(200).WithMessage("数据库列名称长度不能超过200个字符");
        RuleFor(x => x.DatabaseDataType)
            .NotEmpty().WithMessage("数据类型不能为空")
            .MaximumLength(100).WithMessage("数据类型长度不能超过100个字符");
        RuleFor(x => x.CsharpDataType)
            .NotEmpty().WithMessage("C#类型不能为空")
            .MaximumLength(100).WithMessage("C#类型长度不能超过100个字符");
        RuleFor(x => x.CsharpColumnName)
            .NotEmpty().WithMessage("C#列名不能为空")
            .MaximumLength(100).WithMessage("C#列名长度不能超过100个字符");
        RuleFor(x => x.QueryType)
            .NotEmpty().WithMessage("查询方式不能为空")
            .MaximumLength(20).WithMessage("查询方式长度不能超过20个字符");
        RuleFor(x => x.HtmlType)
            .NotEmpty().WithMessage("显示类型不能为空")
            .MaximumLength(50).WithMessage("显示类型长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新GenTableColumn 验证器
// ========================================

/// <summary>
/// 更新GenTableColumn DTO 验证器
/// </summary>
public class TaktGenTableColumnUpdateValidator : AbstractValidator<TaktGenTableColumnUpdateDto>
{
    /// <summary>
    /// 初始化 更新GenTableColumn 校验规则
    /// </summary>
    public TaktGenTableColumnUpdateValidator()
    {
        RuleFor(x => x.GenTableColumnId)
            .GreaterThan(0).WithMessage("GenTableColumnID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.GenTableId)
            .GreaterThanOrEqualTo(0).WithMessage("生成表ID不能为负数");
        RuleFor(x => x.DatabaseColumnName)
            .NotEmpty().WithMessage("数据库列名称不能为空")
            .MaximumLength(200).WithMessage("数据库列名称长度不能超过200个字符");
        RuleFor(x => x.DatabaseDataType)
            .NotEmpty().WithMessage("数据类型不能为空")
            .MaximumLength(100).WithMessage("数据类型长度不能超过100个字符");
        RuleFor(x => x.CsharpDataType)
            .NotEmpty().WithMessage("C#类型不能为空")
            .MaximumLength(100).WithMessage("C#类型长度不能超过100个字符");
        RuleFor(x => x.CsharpColumnName)
            .NotEmpty().WithMessage("C#列名不能为空")
            .MaximumLength(100).WithMessage("C#列名长度不能超过100个字符");
        RuleFor(x => x.QueryType)
            .NotEmpty().WithMessage("查询方式不能为空")
            .MaximumLength(20).WithMessage("查询方式长度不能超过20个字符");
        RuleFor(x => x.HtmlType)
            .NotEmpty().WithMessage("显示类型不能为空")
            .MaximumLength(50).WithMessage("显示类型长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入GenTableColumn 验证器
// ========================================

/// <summary>
/// 导入GenTableColumn DTO 验证器
/// </summary>
public class TaktGenTableColumnImportValidator : AbstractValidator<TaktGenTableColumnImportDto>
{
    /// <summary>
    /// 初始化 导入GenTableColumn 校验规则
    /// </summary>
    public TaktGenTableColumnImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.GenTableId)
            .GreaterThanOrEqualTo(0).WithMessage("生成表ID不能为负数");
        RuleFor(x => x.DatabaseColumnName)
            .NotEmpty().WithMessage("数据库列名称不能为空")
            .MaximumLength(200).WithMessage("数据库列名称长度不能超过200个字符");
        RuleFor(x => x.DatabaseDataType)
            .NotEmpty().WithMessage("数据类型不能为空")
            .MaximumLength(100).WithMessage("数据类型长度不能超过100个字符");
        RuleFor(x => x.CsharpDataType)
            .NotEmpty().WithMessage("C#类型不能为空")
            .MaximumLength(100).WithMessage("C#类型长度不能超过100个字符");
        RuleFor(x => x.CsharpColumnName)
            .NotEmpty().WithMessage("C#列名不能为空")
            .MaximumLength(100).WithMessage("C#列名长度不能超过100个字符");
        RuleFor(x => x.QueryType)
            .NotEmpty().WithMessage("查询方式不能为空")
            .MaximumLength(20).WithMessage("查询方式长度不能超过20个字符");
        RuleFor(x => x.HtmlType)
            .NotEmpty().WithMessage("显示类型不能为空")
            .MaximumLength(50).WithMessage("显示类型长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
