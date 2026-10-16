// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Talent
// 文件名称：TaktTalentStaffingRequirementValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TalentStaffingRequirement 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTalentStaffingRequirement 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Talent;

namespace Takt.Application.Validators.HumanResource.Talent;

// ========================================
// 创建TalentStaffingRequirement 验证器
// ========================================

/// <summary>
/// 创建TalentStaffingRequirement DTO 验证器
/// </summary>
public class TaktTalentStaffingRequirementCreateValidator : AbstractValidator<TaktTalentStaffingRequirementCreateDto>
{
    /// <summary>
    /// 初始化 创建TalentStaffingRequirement 校验规则
    /// </summary>
    public TaktTalentStaffingRequirementCreateValidator()
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
        RuleFor(x => x.ReqCode)
            .NotEmpty().WithMessage("需求单号不能为空")
            .MaximumLength(30).WithMessage("需求单号长度不能超过30个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("申请部门不能为负数");
        RuleFor(x => x.PostId)
            .GreaterThanOrEqualTo(0).WithMessage("申请岗位不能为负数");
        RuleFor(x => x.HeadcountType)
            .NotEmpty().WithMessage("编制类型不能为空")
            .MaximumLength(20).WithMessage("编制类型长度不能超过20个字符");
        RuleFor(x => x.ReasonCode)
            .NotEmpty().WithMessage("需求原因不能为空")
            .MaximumLength(30).WithMessage("需求原因长度不能超过30个字符");
        RuleFor(x => x.ReplaceEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("替补员工不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TalentStaffingRequirement 验证器
// ========================================

/// <summary>
/// 更新TalentStaffingRequirement DTO 验证器
/// </summary>
public class TaktTalentStaffingRequirementUpdateValidator : AbstractValidator<TaktTalentStaffingRequirementUpdateDto>
{
    /// <summary>
    /// 初始化 更新TalentStaffingRequirement 校验规则
    /// </summary>
    public TaktTalentStaffingRequirementUpdateValidator()
    {
        RuleFor(x => x.TalentStaffingRequirementId)
            .GreaterThan(0).WithMessage("TalentStaffingRequirementID无效");
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
        RuleFor(x => x.ReqCode)
            .NotEmpty().WithMessage("需求单号不能为空")
            .MaximumLength(30).WithMessage("需求单号长度不能超过30个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("申请部门不能为负数");
        RuleFor(x => x.PostId)
            .GreaterThanOrEqualTo(0).WithMessage("申请岗位不能为负数");
        RuleFor(x => x.HeadcountType)
            .NotEmpty().WithMessage("编制类型不能为空")
            .MaximumLength(20).WithMessage("编制类型长度不能超过20个字符");
        RuleFor(x => x.ReasonCode)
            .NotEmpty().WithMessage("需求原因不能为空")
            .MaximumLength(30).WithMessage("需求原因长度不能超过30个字符");
        RuleFor(x => x.ReplaceEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("替补员工不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入TalentStaffingRequirement 验证器
// ========================================

/// <summary>
/// 导入TalentStaffingRequirement DTO 验证器
/// </summary>
public class TaktTalentStaffingRequirementImportValidator : AbstractValidator<TaktTalentStaffingRequirementImportDto>
{
    /// <summary>
    /// 初始化 导入TalentStaffingRequirement 校验规则
    /// </summary>
    public TaktTalentStaffingRequirementImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ReqCode)
            .NotEmpty().WithMessage("需求单号不能为空")
            .MaximumLength(30).WithMessage("需求单号长度不能超过30个字符");
        RuleFor(x => x.DeptId)
            .GreaterThanOrEqualTo(0).WithMessage("申请部门不能为负数");
        RuleFor(x => x.PostId)
            .GreaterThanOrEqualTo(0).WithMessage("申请岗位不能为负数");
        RuleFor(x => x.HeadcountType)
            .NotEmpty().WithMessage("编制类型不能为空")
            .MaximumLength(20).WithMessage("编制类型长度不能超过20个字符");
        RuleFor(x => x.ReasonCode)
            .NotEmpty().WithMessage("需求原因不能为空")
            .MaximumLength(30).WithMessage("需求原因长度不能超过30个字符");
        RuleFor(x => x.ReplaceEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("替补员工不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
