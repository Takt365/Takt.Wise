// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Personnel
// 文件名称：TaktEmployeeAddressValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：EmployeeAddress 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktEmployeeAddress 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Personnel;

namespace Takt.Application.Validators.HumanResource.Personnel;

// ========================================
// 创建EmployeeAddress 验证器
// ========================================

/// <summary>
/// 创建EmployeeAddress DTO 验证器
/// </summary>
public class TaktEmployeeAddressCreateValidator : AbstractValidator<TaktEmployeeAddressCreateDto>
{
    /// <summary>
    /// 初始化 创建EmployeeAddress 校验规则
    /// </summary>
    public TaktEmployeeAddressCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工不能为负数");
        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("员工编码不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(6).WithMessage("员工编码长度不能超过6个字符");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(80).WithMessage("员工姓名长度不能超过80个字符");
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("国家不能为空")
            .MaximumLength(2).WithMessage("国家长度不能超过2个字符");
        RuleFor(x => x.Province)
            .NotEmpty().WithMessage("省不能为空")
            .MaximumLength(70).WithMessage("省长度不能超过70个字符");
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("市不能为空")
            .MaximumLength(70).WithMessage("市长度不能超过70个字符");
        RuleFor(x => x.District)
            .NotEmpty().WithMessage("区县不能为空")
            .MaximumLength(70).WithMessage("区县长度不能超过70个字符");
        RuleFor(x => x.Address1)
            .NotEmpty().WithMessage("地址1不能为空")
            .MaximumLength(140).WithMessage("地址1长度不能超过140个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新EmployeeAddress 验证器
// ========================================

/// <summary>
/// 更新EmployeeAddress DTO 验证器
/// </summary>
public class TaktEmployeeAddressUpdateValidator : AbstractValidator<TaktEmployeeAddressUpdateDto>
{
    /// <summary>
    /// 初始化 更新EmployeeAddress 校验规则
    /// </summary>
    public TaktEmployeeAddressUpdateValidator()
    {
        RuleFor(x => x.EmployeeAddressId)
            .GreaterThan(0).WithMessage("EmployeeAddressID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工不能为负数");
        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("员工编码不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(6).WithMessage("员工编码长度不能超过6个字符");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空").When(x => x.EmployeeId <= 0)
            .MaximumLength(80).WithMessage("员工姓名长度不能超过80个字符");
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("国家不能为空")
            .MaximumLength(2).WithMessage("国家长度不能超过2个字符");
        RuleFor(x => x.Province)
            .NotEmpty().WithMessage("省不能为空")
            .MaximumLength(70).WithMessage("省长度不能超过70个字符");
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("市不能为空")
            .MaximumLength(70).WithMessage("市长度不能超过70个字符");
        RuleFor(x => x.District)
            .NotEmpty().WithMessage("区县不能为空")
            .MaximumLength(70).WithMessage("区县长度不能超过70个字符");
        RuleFor(x => x.Address1)
            .NotEmpty().WithMessage("地址1不能为空")
            .MaximumLength(140).WithMessage("地址1长度不能超过140个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入EmployeeAddress 验证器
// ========================================

/// <summary>
/// 导入EmployeeAddress DTO 验证器
/// </summary>
public class TaktEmployeeAddressImportValidator : AbstractValidator<TaktEmployeeAddressImportDto>
{
    /// <summary>
    /// 初始化 导入EmployeeAddress 校验规则
    /// </summary>
    public TaktEmployeeAddressImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.EmployeeId)
            .GreaterThanOrEqualTo(0).WithMessage("员工不能为负数");
        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("员工编码不能为空")
            .MaximumLength(6).WithMessage("员工编码长度不能超过6个字符");
        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("员工姓名不能为空")
            .MaximumLength(80).WithMessage("员工姓名长度不能超过80个字符");
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("国家不能为空")
            .MaximumLength(2).WithMessage("国家长度不能超过2个字符");
        RuleFor(x => x.Province)
            .NotEmpty().WithMessage("省不能为空")
            .MaximumLength(70).WithMessage("省长度不能超过70个字符");
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("市不能为空")
            .MaximumLength(70).WithMessage("市长度不能超过70个字符");
        RuleFor(x => x.District)
            .NotEmpty().WithMessage("区县不能为空")
            .MaximumLength(70).WithMessage("区县长度不能超过70个字符");
        RuleFor(x => x.Address1)
            .NotEmpty().WithMessage("地址1不能为空")
            .MaximumLength(140).WithMessage("地址1长度不能超过140个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
