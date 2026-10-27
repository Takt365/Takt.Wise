// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Code.Generator
// 文件名称：TaktGenTableValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：GenTable 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktGenTable 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Code.Generator;

namespace Takt.Application.Validators.Code.Generator;

// ========================================
// 创建GenTable 验证器
// ========================================

/// <summary>
/// 创建GenTable DTO 验证器
/// </summary>
public class TaktGenTableCreateValidator : AbstractValidator<TaktGenTableCreateDto>
{
    /// <summary>
    /// 初始化 创建GenTable 校验规则
    /// </summary>
    public TaktGenTableCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.DataSource)
            .NotEmpty().WithMessage("数据源不能为空")
            .MaximumLength(200).WithMessage("数据源长度不能超过200个字符");
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("表名称不能为空")
            .MaximumLength(200).WithMessage("表名称长度不能超过200个字符");
        RuleFor(x => x.GenTemplateCategory)
            .NotEmpty().WithMessage("生成模板类型不能为空")
            .MaximumLength(50).WithMessage("生成模板类型长度不能超过50个字符");
        RuleFor(x => x.GenBusinessName)
            .NotEmpty().WithMessage("业务名不能为空")
            .MaximumLength(50).WithMessage("业务名长度不能超过50个字符");
        RuleFor(x => x.PermsPrefix)
            .NotEmpty().WithMessage("权限前缀不能为空")
            .MaximumLength(100).WithMessage("权限前缀长度不能超过100个字符");
        RuleFor(x => x.EntityClassName)
            .NotEmpty().WithMessage("实体类名称不能为空")
            .MaximumLength(100).WithMessage("实体类名称长度不能超过100个字符");
        RuleFor(x => x.GenPath)
            .NotEmpty().WithMessage("生成路径不能为空")
            .MaximumLength(500).WithMessage("生成路径长度不能超过500个字符");
        RuleFor(x => x.ParentMenuId)
            .GreaterThanOrEqualTo(0).WithMessage("上级菜单不能为负数");
        RuleFor(x => x.SortField)
            .NotEmpty().WithMessage("排序字段不能为空")
            .MaximumLength(100).WithMessage("排序字段长度不能超过100个字符");
        RuleFor(x => x.SortType)
            .NotEmpty().WithMessage("排序方向不能为空")
            .MaximumLength(10).WithMessage("排序方向长度不能超过10个字符");
        RuleFor(x => x.GenAuthor)
            .NotEmpty().WithMessage("作者不能为空")
            .MaximumLength(50).WithMessage("作者长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新GenTable 验证器
// ========================================

/// <summary>
/// 更新GenTable DTO 验证器
/// </summary>
public class TaktGenTableUpdateValidator : AbstractValidator<TaktGenTableUpdateDto>
{
    /// <summary>
    /// 初始化 更新GenTable 校验规则
    /// </summary>
    public TaktGenTableUpdateValidator()
    {
        RuleFor(x => x.GenTableId)
            .GreaterThan(0).WithMessage("GenTableID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.DataSource)
            .NotEmpty().WithMessage("数据源不能为空")
            .MaximumLength(200).WithMessage("数据源长度不能超过200个字符");
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("表名称不能为空")
            .MaximumLength(200).WithMessage("表名称长度不能超过200个字符");
        RuleFor(x => x.GenTemplateCategory)
            .NotEmpty().WithMessage("生成模板类型不能为空")
            .MaximumLength(50).WithMessage("生成模板类型长度不能超过50个字符");
        RuleFor(x => x.GenBusinessName)
            .NotEmpty().WithMessage("业务名不能为空")
            .MaximumLength(50).WithMessage("业务名长度不能超过50个字符");
        RuleFor(x => x.PermsPrefix)
            .NotEmpty().WithMessage("权限前缀不能为空")
            .MaximumLength(100).WithMessage("权限前缀长度不能超过100个字符");
        RuleFor(x => x.EntityClassName)
            .NotEmpty().WithMessage("实体类名称不能为空")
            .MaximumLength(100).WithMessage("实体类名称长度不能超过100个字符");
        RuleFor(x => x.GenPath)
            .NotEmpty().WithMessage("生成路径不能为空")
            .MaximumLength(500).WithMessage("生成路径长度不能超过500个字符");
        RuleFor(x => x.ParentMenuId)
            .GreaterThanOrEqualTo(0).WithMessage("上级菜单不能为负数");
        RuleFor(x => x.SortField)
            .NotEmpty().WithMessage("排序字段不能为空")
            .MaximumLength(100).WithMessage("排序字段长度不能超过100个字符");
        RuleFor(x => x.SortType)
            .NotEmpty().WithMessage("排序方向不能为空")
            .MaximumLength(10).WithMessage("排序方向长度不能超过10个字符");
        RuleFor(x => x.GenAuthor)
            .NotEmpty().WithMessage("作者不能为空")
            .MaximumLength(50).WithMessage("作者长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入GenTable 验证器
// ========================================

/// <summary>
/// 导入GenTable DTO 验证器
/// </summary>
public class TaktGenTableImportValidator : AbstractValidator<TaktGenTableImportDto>
{
    /// <summary>
    /// 初始化 导入GenTable 校验规则
    /// </summary>
    public TaktGenTableImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.DataSource)
            .NotEmpty().WithMessage("数据源不能为空")
            .MaximumLength(200).WithMessage("数据源长度不能超过200个字符");
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("表名称不能为空")
            .MaximumLength(200).WithMessage("表名称长度不能超过200个字符");
        RuleFor(x => x.GenTemplateCategory)
            .NotEmpty().WithMessage("生成模板类型不能为空")
            .MaximumLength(50).WithMessage("生成模板类型长度不能超过50个字符");
        RuleFor(x => x.GenBusinessName)
            .NotEmpty().WithMessage("业务名不能为空")
            .MaximumLength(50).WithMessage("业务名长度不能超过50个字符");
        RuleFor(x => x.PermsPrefix)
            .NotEmpty().WithMessage("权限前缀不能为空")
            .MaximumLength(100).WithMessage("权限前缀长度不能超过100个字符");
        RuleFor(x => x.EntityClassName)
            .NotEmpty().WithMessage("实体类名称不能为空")
            .MaximumLength(100).WithMessage("实体类名称长度不能超过100个字符");
        RuleFor(x => x.GenPath)
            .NotEmpty().WithMessage("生成路径不能为空")
            .MaximumLength(500).WithMessage("生成路径长度不能超过500个字符");
        RuleFor(x => x.ParentMenuId)
            .GreaterThanOrEqualTo(0).WithMessage("上级菜单不能为负数");
        RuleFor(x => x.SortField)
            .NotEmpty().WithMessage("排序字段不能为空")
            .MaximumLength(100).WithMessage("排序字段长度不能超过100个字符");
        RuleFor(x => x.SortType)
            .NotEmpty().WithMessage("排序方向不能为空")
            .MaximumLength(10).WithMessage("排序方向长度不能超过10个字符");
        RuleFor(x => x.GenAuthor)
            .NotEmpty().WithMessage("作者不能为空")
            .MaximumLength(50).WithMessage("作者长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
