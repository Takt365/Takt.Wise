// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Routine.NewsCenter
// 文件名称：TaktNewsValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：News 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktNews 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Routine.NewsCenter;

namespace Takt.Application.Validators.Routine.NewsCenter;

// ========================================
// 创建News 验证器
// ========================================

/// <summary>
/// 创建News DTO 验证器
/// </summary>
public class TaktNewsCreateValidator : AbstractValidator<TaktNewsCreateDto>
{
    /// <summary>
    /// 初始化 创建News 校验规则
    /// </summary>
    public TaktNewsCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.NewsCode)
            .NotEmpty().WithMessage("新闻编码不能为空")
            .MaximumLength(50).WithMessage("新闻编码长度不能超过50个字符");
        RuleFor(x => x.NewsTitle)
            .NotEmpty().WithMessage("新闻标题不能为空")
            .MaximumLength(200).WithMessage("新闻标题长度不能超过200个字符");
        RuleFor(x => x.NewsContent)
            .NotEmpty().WithMessage("新闻内容不能为空");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("发布部门 ID不能为负数");
        RuleFor(x => x.PublisherId)
            .GreaterThanOrEqualTo(0).WithMessage("发布人 ID不能为负数");
        RuleFor(x => x.PublisherName)
            .NotEmpty().WithMessage("发布人姓名不能为空").When(x => x.PublisherId <= 0)
            .MaximumLength(20).WithMessage("发布人姓名长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新News 验证器
// ========================================

/// <summary>
/// 更新News DTO 验证器
/// </summary>
public class TaktNewsUpdateValidator : AbstractValidator<TaktNewsUpdateDto>
{
    /// <summary>
    /// 初始化 更新News 校验规则
    /// </summary>
    public TaktNewsUpdateValidator()
    {
        RuleFor(x => x.NewsId)
            .GreaterThan(0).WithMessage("NewsID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.NewsCode)
            .NotEmpty().WithMessage("新闻编码不能为空")
            .MaximumLength(50).WithMessage("新闻编码长度不能超过50个字符");
        RuleFor(x => x.NewsTitle)
            .NotEmpty().WithMessage("新闻标题不能为空")
            .MaximumLength(200).WithMessage("新闻标题长度不能超过200个字符");
        RuleFor(x => x.NewsContent)
            .NotEmpty().WithMessage("新闻内容不能为空");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("发布部门 ID不能为负数");
        RuleFor(x => x.PublisherId)
            .GreaterThanOrEqualTo(0).WithMessage("发布人 ID不能为负数");
        RuleFor(x => x.PublisherName)
            .NotEmpty().WithMessage("发布人姓名不能为空").When(x => x.PublisherId <= 0)
            .MaximumLength(20).WithMessage("发布人姓名长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入News 验证器
// ========================================

/// <summary>
/// 导入News DTO 验证器
/// </summary>
public class TaktNewsImportValidator : AbstractValidator<TaktNewsImportDto>
{
    /// <summary>
    /// 初始化 导入News 校验规则
    /// </summary>
    public TaktNewsImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.NewsCode)
            .NotEmpty().WithMessage("新闻编码不能为空")
            .MaximumLength(50).WithMessage("新闻编码长度不能超过50个字符");
        RuleFor(x => x.NewsTitle)
            .NotEmpty().WithMessage("新闻标题不能为空")
            .MaximumLength(200).WithMessage("新闻标题长度不能超过200个字符");
        RuleFor(x => x.NewsContent)
            .NotEmpty().WithMessage("新闻内容不能为空");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("发布部门 ID不能为负数");
        RuleFor(x => x.PublisherId)
            .GreaterThanOrEqualTo(0).WithMessage("发布人 ID不能为负数");
        RuleFor(x => x.PublisherName)
            .NotEmpty().WithMessage("发布人姓名不能为空")
            .MaximumLength(20).WithMessage("发布人姓名长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
