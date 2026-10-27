// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Organization
// 文件名称：TaktDeptValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：Dept 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktDept 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Organization;

namespace Takt.Application.Validators.HumanResource.Organization;

// ========================================
// 创建Dept 验证器
// ========================================

/// <summary>
/// 创建Dept DTO 验证器
/// </summary>
public class TaktDeptCreateValidator : AbstractValidator<TaktDeptCreateDto>
{
    /// <summary>
    /// 初始化 创建Dept 校验规则
    /// </summary>
    public TaktDeptCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.HeadUserId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.HeadUserId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.DeptCode)
            .NotEmpty().WithMessage("部门编码不能为空")
            .MaximumLength(6).WithMessage("部门编码长度不能超过6个字符");
        RuleFor(x => x.DeptShortName)
            .NotEmpty().WithMessage("部门简称不能为空")
            .MaximumLength(6).WithMessage("部门简称长度不能超过6个字符");
        RuleFor(x => x.DeptName1)
            .NotEmpty().WithMessage("部门名称1不能为空")
            .MaximumLength(40).WithMessage("部门名称1长度不能超过40个字符");
        RuleFor(x => x.DeptName2)
            .NotEmpty().WithMessage("部门名称2不能为空")
            .MaximumLength(70).WithMessage("部门名称2长度不能超过70个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父部门不能为负数");
        RuleFor(x => x.IsoCode)
            .NotEmpty().WithMessage("ISO 编码不能为空")
            .MaximumLength(6).WithMessage("ISO 编码长度不能超过6个字符");
        RuleFor(x => x.CostCenterCode)
            .NotEmpty().WithMessage("成本中心编码不能为空")
            .MaximumLength(6).WithMessage("成本中心编码长度不能超过6个字符");
        RuleFor(x => x.HeadUserId)
            .GreaterThanOrEqualTo(0).WithMessage("部门负责人不能为负数");
        RuleFor(x => x.HeadUserName)
            .NotEmpty().WithMessage("部门负责人名称不能为空").When(x => x.HeadUserId <= 0)
            .MaximumLength(40).WithMessage("部门负责人名称长度不能超过40个字符");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("联系电话不能为空")
            .MaximumLength(20).WithMessage("联系电话长度不能超过20个字符");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("邮箱不能为空")
            .MaximumLength(100).WithMessage("邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("办公地点不能为空")
            .MaximumLength(200).WithMessage("办公地点长度不能超过200个字符");
        RuleFor(x => x.DeptDescription)
            .NotEmpty().WithMessage("部门描述不能为空")
            .MaximumLength(500).WithMessage("部门描述长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Dept 验证器
// ========================================

/// <summary>
/// 更新Dept DTO 验证器
/// </summary>
public class TaktDeptUpdateValidator : AbstractValidator<TaktDeptUpdateDto>
{
    /// <summary>
    /// 初始化 更新Dept 校验规则
    /// </summary>
    public TaktDeptUpdateValidator()
    {
        RuleFor(x => x.DeptId)
            .GreaterThan(0).WithMessage("DeptID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.HeadUserId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.HeadUserId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.DeptCode)
            .NotEmpty().WithMessage("部门编码不能为空")
            .MaximumLength(6).WithMessage("部门编码长度不能超过6个字符");
        RuleFor(x => x.DeptShortName)
            .NotEmpty().WithMessage("部门简称不能为空")
            .MaximumLength(6).WithMessage("部门简称长度不能超过6个字符");
        RuleFor(x => x.DeptName1)
            .NotEmpty().WithMessage("部门名称1不能为空")
            .MaximumLength(40).WithMessage("部门名称1长度不能超过40个字符");
        RuleFor(x => x.DeptName2)
            .NotEmpty().WithMessage("部门名称2不能为空")
            .MaximumLength(70).WithMessage("部门名称2长度不能超过70个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父部门不能为负数");
        RuleFor(x => x.IsoCode)
            .NotEmpty().WithMessage("ISO 编码不能为空")
            .MaximumLength(6).WithMessage("ISO 编码长度不能超过6个字符");
        RuleFor(x => x.CostCenterCode)
            .NotEmpty().WithMessage("成本中心编码不能为空")
            .MaximumLength(6).WithMessage("成本中心编码长度不能超过6个字符");
        RuleFor(x => x.HeadUserId)
            .GreaterThanOrEqualTo(0).WithMessage("部门负责人不能为负数");
        RuleFor(x => x.HeadUserName)
            .NotEmpty().WithMessage("部门负责人名称不能为空").When(x => x.HeadUserId <= 0)
            .MaximumLength(40).WithMessage("部门负责人名称长度不能超过40个字符");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("联系电话不能为空")
            .MaximumLength(20).WithMessage("联系电话长度不能超过20个字符");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("邮箱不能为空")
            .MaximumLength(100).WithMessage("邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("办公地点不能为空")
            .MaximumLength(200).WithMessage("办公地点长度不能超过200个字符");
        RuleFor(x => x.DeptDescription)
            .NotEmpty().WithMessage("部门描述不能为空")
            .MaximumLength(500).WithMessage("部门描述长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Dept 验证器
// ========================================

/// <summary>
/// 导入Dept DTO 验证器
/// </summary>
public class TaktDeptImportValidator : AbstractValidator<TaktDeptImportDto>
{
    /// <summary>
    /// 初始化 导入Dept 校验规则
    /// </summary>
    public TaktDeptImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.DeptCode)
            .NotEmpty().WithMessage("部门编码不能为空")
            .MaximumLength(6).WithMessage("部门编码长度不能超过6个字符");
        RuleFor(x => x.DeptShortName)
            .NotEmpty().WithMessage("部门简称不能为空")
            .MaximumLength(6).WithMessage("部门简称长度不能超过6个字符");
        RuleFor(x => x.DeptName1)
            .NotEmpty().WithMessage("部门名称1不能为空")
            .MaximumLength(40).WithMessage("部门名称1长度不能超过40个字符");
        RuleFor(x => x.DeptName2)
            .NotEmpty().WithMessage("部门名称2不能为空")
            .MaximumLength(70).WithMessage("部门名称2长度不能超过70个字符");
        RuleFor(x => x.ParentId)
            .GreaterThanOrEqualTo(0).WithMessage("父部门不能为负数");
        RuleFor(x => x.IsoCode)
            .NotEmpty().WithMessage("ISO 编码不能为空")
            .MaximumLength(6).WithMessage("ISO 编码长度不能超过6个字符");
        RuleFor(x => x.CostCenterCode)
            .NotEmpty().WithMessage("成本中心编码不能为空")
            .MaximumLength(6).WithMessage("成本中心编码长度不能超过6个字符");
        RuleFor(x => x.HeadUserId)
            .GreaterThanOrEqualTo(0).WithMessage("部门负责人不能为负数");
        RuleFor(x => x.HeadUserName)
            .NotEmpty().WithMessage("部门负责人名称不能为空")
            .MaximumLength(40).WithMessage("部门负责人名称长度不能超过40个字符");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("联系电话不能为空")
            .MaximumLength(20).WithMessage("联系电话长度不能超过20个字符");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("邮箱不能为空")
            .MaximumLength(100).WithMessage("邮箱长度不能超过100个字符")
            .EmailAddress().WithMessage("邮箱格式不正确").When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("办公地点不能为空")
            .MaximumLength(200).WithMessage("办公地点长度不能超过200个字符");
        RuleFor(x => x.DeptDescription)
            .NotEmpty().WithMessage("部门描述不能为空")
            .MaximumLength(500).WithMessage("部门描述长度不能超过500个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
