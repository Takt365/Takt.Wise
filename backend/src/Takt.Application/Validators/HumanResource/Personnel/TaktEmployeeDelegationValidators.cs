// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Personnel
// 文件名称：TaktEmployeeDelegationValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeDelegation 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEmployeeDelegation 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Personnel;

namespace Takt.Application.Validators.HumanResource.Personnel;

// ========================================
// 创建EmployeeDelegation 验证器
// ========================================

/// <summary>
/// 创建EmployeeDelegation DTO 验证器
/// </summary>
public class TaktEmployeeDelegationCreateValidator : AbstractValidator<TaktEmployeeDelegationCreateDto>
{
    /// <summary>
    /// 初始化 创建EmployeeDelegation 校验规则
    /// </summary>
    public TaktEmployeeDelegationCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ProxyEmployeeId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ProxyEmployeeId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ProxyEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("代理人不能为负数");
        RuleFor(x => x.ProxyEmployeeCode)
            .NotEmpty().WithMessage("代理人编码不能为空").When(x => x.ProxyEmployeeId <= 0)
            .MaximumLength(6).WithMessage("代理人编码长度不能超过6个字符");
        RuleFor(x => x.ProxyEmployeeName)
            .NotEmpty().WithMessage("代理人姓名不能为空").When(x => x.ProxyEmployeeId <= 0)
            .MaximumLength(80).WithMessage("代理人姓名长度不能超过80个字符");
        RuleFor(x => x.OriginalEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("被代理人不能为负数");
        RuleFor(x => x.OriginalEmployeeCode)
            .NotEmpty().WithMessage("被代理人编码不能为空").When(x => x.OriginalEmployeeId <= 0)
            .MaximumLength(6).WithMessage("被代理人编码长度不能超过6个字符");
        RuleFor(x => x.OriginalEmployeeName)
            .NotEmpty().WithMessage("被代理人姓名不能为空").When(x => x.OriginalEmployeeId <= 0)
            .MaximumLength(80).WithMessage("被代理人姓名长度不能超过80个字符");
        RuleFor(x => x.ScopeId)
            .GreaterThanOrEqualTo(0).WithMessage("代理范围 ID不能为负数");
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("代理原因不能为空")
            .MaximumLength(200).WithMessage("代理原因长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EmployeeDelegation 验证器
// ========================================

/// <summary>
/// 更新EmployeeDelegation DTO 验证器
/// </summary>
public class TaktEmployeeDelegationUpdateValidator : AbstractValidator<TaktEmployeeDelegationUpdateDto>
{
    /// <summary>
    /// 初始化 更新EmployeeDelegation 校验规则
    /// </summary>
    public TaktEmployeeDelegationUpdateValidator()
    {
        RuleFor(x => x.EmployeeDelegationId)
            .GreaterThan(0).WithMessage("EmployeeDelegationID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ProxyEmployeeId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ProxyEmployeeId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ProxyEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("代理人不能为负数");
        RuleFor(x => x.ProxyEmployeeCode)
            .NotEmpty().WithMessage("代理人编码不能为空").When(x => x.ProxyEmployeeId <= 0)
            .MaximumLength(6).WithMessage("代理人编码长度不能超过6个字符");
        RuleFor(x => x.ProxyEmployeeName)
            .NotEmpty().WithMessage("代理人姓名不能为空").When(x => x.ProxyEmployeeId <= 0)
            .MaximumLength(80).WithMessage("代理人姓名长度不能超过80个字符");
        RuleFor(x => x.OriginalEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("被代理人不能为负数");
        RuleFor(x => x.OriginalEmployeeCode)
            .NotEmpty().WithMessage("被代理人编码不能为空").When(x => x.OriginalEmployeeId <= 0)
            .MaximumLength(6).WithMessage("被代理人编码长度不能超过6个字符");
        RuleFor(x => x.OriginalEmployeeName)
            .NotEmpty().WithMessage("被代理人姓名不能为空").When(x => x.OriginalEmployeeId <= 0)
            .MaximumLength(80).WithMessage("被代理人姓名长度不能超过80个字符");
        RuleFor(x => x.ScopeId)
            .GreaterThanOrEqualTo(0).WithMessage("代理范围 ID不能为负数");
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("代理原因不能为空")
            .MaximumLength(200).WithMessage("代理原因长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入EmployeeDelegation 验证器
// ========================================

/// <summary>
/// 导入EmployeeDelegation DTO 验证器
/// </summary>
public class TaktEmployeeDelegationImportValidator : AbstractValidator<TaktEmployeeDelegationImportDto>
{
    /// <summary>
    /// 初始化 导入EmployeeDelegation 校验规则
    /// </summary>
    public TaktEmployeeDelegationImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ProxyEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("代理人不能为负数");
        RuleFor(x => x.ProxyEmployeeCode)
            .NotEmpty().WithMessage("代理人编码不能为空")
            .MaximumLength(6).WithMessage("代理人编码长度不能超过6个字符");
        RuleFor(x => x.ProxyEmployeeName)
            .NotEmpty().WithMessage("代理人姓名不能为空")
            .MaximumLength(80).WithMessage("代理人姓名长度不能超过80个字符");
        RuleFor(x => x.OriginalEmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("被代理人不能为负数");
        RuleFor(x => x.OriginalEmployeeCode)
            .NotEmpty().WithMessage("被代理人编码不能为空")
            .MaximumLength(6).WithMessage("被代理人编码长度不能超过6个字符");
        RuleFor(x => x.OriginalEmployeeName)
            .NotEmpty().WithMessage("被代理人姓名不能为空")
            .MaximumLength(80).WithMessage("被代理人姓名长度不能超过80个字符");
        RuleFor(x => x.ScopeId)
            .GreaterThanOrEqualTo(0).WithMessage("代理范围 ID不能为负数");
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("代理原因不能为空")
            .MaximumLength(200).WithMessage("代理原因长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
