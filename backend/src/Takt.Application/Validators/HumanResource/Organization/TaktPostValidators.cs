// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Organization
// 文件名称：TaktPostValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：Post 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktPost 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Organization;

namespace Takt.Application.Validators.HumanResource.Organization;

// ========================================
// 创建Post 验证器
// ========================================

/// <summary>
/// 创建Post DTO 验证器
/// </summary>
public class TaktPostCreateValidator : AbstractValidator<TaktPostCreateDto>
{
    /// <summary>
    /// 初始化 创建Post 校验规则
    /// </summary>
    public TaktPostCreateValidator()
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
        RuleFor(x => x.PostCode)
            .NotEmpty().WithMessage("岗位编码不能为空")
            .MaximumLength(50).WithMessage("岗位编码长度不能超过50个字符");
        RuleFor(x => x.PostName)
            .NotEmpty().WithMessage("岗位名称不能为空")
            .MaximumLength(100).WithMessage("岗位名称长度不能超过100个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("所属部门不能为负数");
        RuleFor(x => x.DeptName)
            .NotEmpty().WithMessage("所属部门名称不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(100).WithMessage("所属部门名称长度不能超过100个字符");
        RuleFor(x => x.PostCategory)
            .NotEmpty().WithMessage("岗位类别不能为空")
            .MaximumLength(40).WithMessage("岗位类别长度不能超过40个字符");
        RuleFor(x => x.PostLevel)
            .NotEmpty().WithMessage("岗位职级不能为空")
            .MaximumLength(40).WithMessage("岗位职级长度不能超过40个字符");
        RuleFor(x => x.Responsibilities)
            .NotEmpty().WithMessage("岗位职责不能为空")
            .MaximumLength(2000).WithMessage("岗位职责长度不能超过2000个字符");
        RuleFor(x => x.Requirements)
            .NotEmpty().WithMessage("任职要求不能为空")
            .MaximumLength(2000).WithMessage("任职要求长度不能超过2000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Post 验证器
// ========================================

/// <summary>
/// 更新Post DTO 验证器
/// </summary>
public class TaktPostUpdateValidator : AbstractValidator<TaktPostUpdateDto>
{
    /// <summary>
    /// 初始化 更新Post 校验规则
    /// </summary>
    public TaktPostUpdateValidator()
    {
        RuleFor(x => x.PostId)
            .GreaterThan(0).WithMessage("PostID无效");
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
        RuleFor(x => x.PostCode)
            .NotEmpty().WithMessage("岗位编码不能为空")
            .MaximumLength(50).WithMessage("岗位编码长度不能超过50个字符");
        RuleFor(x => x.PostName)
            .NotEmpty().WithMessage("岗位名称不能为空")
            .MaximumLength(100).WithMessage("岗位名称长度不能超过100个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("所属部门不能为负数");
        RuleFor(x => x.DeptName)
            .NotEmpty().WithMessage("所属部门名称不能为空").When(x => x.DeptId <= 0)
            .MaximumLength(100).WithMessage("所属部门名称长度不能超过100个字符");
        RuleFor(x => x.PostCategory)
            .NotEmpty().WithMessage("岗位类别不能为空")
            .MaximumLength(40).WithMessage("岗位类别长度不能超过40个字符");
        RuleFor(x => x.PostLevel)
            .NotEmpty().WithMessage("岗位职级不能为空")
            .MaximumLength(40).WithMessage("岗位职级长度不能超过40个字符");
        RuleFor(x => x.Responsibilities)
            .NotEmpty().WithMessage("岗位职责不能为空")
            .MaximumLength(2000).WithMessage("岗位职责长度不能超过2000个字符");
        RuleFor(x => x.Requirements)
            .NotEmpty().WithMessage("任职要求不能为空")
            .MaximumLength(2000).WithMessage("任职要求长度不能超过2000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Post 验证器
// ========================================

/// <summary>
/// 导入Post DTO 验证器
/// </summary>
public class TaktPostImportValidator : AbstractValidator<TaktPostImportDto>
{
    /// <summary>
    /// 初始化 导入Post 校验规则
    /// </summary>
    public TaktPostImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.PostCode)
            .NotEmpty().WithMessage("岗位编码不能为空")
            .MaximumLength(50).WithMessage("岗位编码长度不能超过50个字符");
        RuleFor(x => x.PostName)
            .NotEmpty().WithMessage("岗位名称不能为空")
            .MaximumLength(100).WithMessage("岗位名称长度不能超过100个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("所属部门不能为负数");
        RuleFor(x => x.DeptName)
            .NotEmpty().WithMessage("所属部门名称不能为空")
            .MaximumLength(100).WithMessage("所属部门名称长度不能超过100个字符");
        RuleFor(x => x.PostCategory)
            .NotEmpty().WithMessage("岗位类别不能为空")
            .MaximumLength(40).WithMessage("岗位类别长度不能超过40个字符");
        RuleFor(x => x.PostLevel)
            .NotEmpty().WithMessage("岗位职级不能为空")
            .MaximumLength(40).WithMessage("岗位职级长度不能超过40个字符");
        RuleFor(x => x.Responsibilities)
            .NotEmpty().WithMessage("岗位职责不能为空")
            .MaximumLength(2000).WithMessage("岗位职责长度不能超过2000个字符");
        RuleFor(x => x.Requirements)
            .NotEmpty().WithMessage("任职要求不能为空")
            .MaximumLength(2000).WithMessage("任职要求长度不能超过2000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
