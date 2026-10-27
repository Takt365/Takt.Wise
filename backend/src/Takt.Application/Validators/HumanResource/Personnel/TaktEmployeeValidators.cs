// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Personnel
// 文件名称：TaktEmployeeValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：Employee 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEmployee 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Personnel;

namespace Takt.Application.Validators.HumanResource.Personnel;

// ========================================
// 创建Employee 验证器
// ========================================

/// <summary>
/// 创建Employee DTO 验证器
/// </summary>
public class TaktEmployeeCreateValidator : AbstractValidator<TaktEmployeeCreateDto>
{
    /// <summary>
    /// 初始化 创建Employee 校验规则
    /// </summary>
    public TaktEmployeeCreateValidator()
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
        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("员工编码不能为空")
            .MaximumLength(6).WithMessage("员工编码长度不能超过6个字符");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("姓名不能为空")
            .MaximumLength(80).WithMessage("姓名长度不能超过80个字符");
        RuleFor(x => x.IdCardCode)
            .NotEmpty().WithMessage("身份证号不能为空")
            .MaximumLength(18).WithMessage("身份证号长度不能超过18个字符");
        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("手机号码不能为空")
            .MaximumLength(11).WithMessage("手机号码长度不能超过11个字符");
        RuleFor(x => x.NativePlace)
            .NotEmpty().WithMessage("籍贯不能为空")
            .MaximumLength(6).WithMessage("籍贯长度不能超过6个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新Employee 验证器
// ========================================

/// <summary>
/// 更新Employee DTO 验证器
/// </summary>
public class TaktEmployeeUpdateValidator : AbstractValidator<TaktEmployeeUpdateDto>
{
    /// <summary>
    /// 初始化 更新Employee 校验规则
    /// </summary>
    public TaktEmployeeUpdateValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("EmployeeID无效");
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
        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("员工编码不能为空")
            .MaximumLength(6).WithMessage("员工编码长度不能超过6个字符");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("姓名不能为空")
            .MaximumLength(80).WithMessage("姓名长度不能超过80个字符");
        RuleFor(x => x.IdCardCode)
            .NotEmpty().WithMessage("身份证号不能为空")
            .MaximumLength(18).WithMessage("身份证号长度不能超过18个字符");
        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("手机号码不能为空")
            .MaximumLength(11).WithMessage("手机号码长度不能超过11个字符");
        RuleFor(x => x.NativePlace)
            .NotEmpty().WithMessage("籍贯不能为空")
            .MaximumLength(6).WithMessage("籍贯长度不能超过6个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入Employee 验证器
// ========================================

/// <summary>
/// 导入Employee DTO 验证器
/// </summary>
public class TaktEmployeeImportValidator : AbstractValidator<TaktEmployeeImportDto>
{
    /// <summary>
    /// 初始化 导入Employee 校验规则
    /// </summary>
    public TaktEmployeeImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("员工编码不能为空")
            .MaximumLength(6).WithMessage("员工编码长度不能超过6个字符");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("姓名不能为空")
            .MaximumLength(80).WithMessage("姓名长度不能超过80个字符");
        RuleFor(x => x.IdCardCode)
            .NotEmpty().WithMessage("身份证号不能为空")
            .MaximumLength(18).WithMessage("身份证号长度不能超过18个字符");
        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("手机号码不能为空")
            .MaximumLength(11).WithMessage("手机号码长度不能超过11个字符");
        RuleFor(x => x.NativePlace)
            .NotEmpty().WithMessage("籍贯不能为空")
            .MaximumLength(6).WithMessage("籍贯长度不能超过6个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
