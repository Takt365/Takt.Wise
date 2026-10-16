// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.HumanResource.Compensation
// 文件名称：TaktSalaryFormulaValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：SalaryFormula 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktSalaryFormula 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.HumanResource.Compensation;

namespace Takt.Application.Validators.HumanResource.Compensation;

// ========================================
// 创建SalaryFormula 验证器
// ========================================

/// <summary>
/// 创建SalaryFormula DTO 验证器
/// </summary>
public class TaktSalaryFormulaCreateValidator : AbstractValidator<TaktSalaryFormulaCreateDto>
{
    /// <summary>
    /// 初始化 创建SalaryFormula 校验规则
    /// </summary>
    public TaktSalaryFormulaCreateValidator()
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
        RuleFor(x => x.SetCode)
            .NotEmpty().WithMessage("公式方案编码不能为空")
            .MaximumLength(40).WithMessage("公式方案编码长度不能超过40个字符");
        RuleFor(x => x.SetName)
            .NotEmpty().WithMessage("公式方案名称不能为空")
            .MaximumLength(80).WithMessage("公式方案名称长度不能超过80个字符");
        RuleFor(x => x.PayrollId)
            .GreaterThanOrEqualTo(0).WithMessage("薪酬体系不能为负数");
        RuleFor(x => x.FormulaCode)
            .NotEmpty().WithMessage("步骤编码不能为空")
            .MaximumLength(40).WithMessage("步骤编码长度不能超过40个字符");
        RuleFor(x => x.FormulaName)
            .NotEmpty().WithMessage("步骤名称不能为空")
            .MaximumLength(80).WithMessage("步骤名称长度不能超过80个字符");
        RuleFor(x => x.TargetField)
            .NotEmpty().WithMessage("结果写入字段不能为空")
            .MaximumLength(64).WithMessage("结果写入字段长度不能超过64个字符");
        RuleFor(x => x.FormulaExpression)
            .NotEmpty().WithMessage("计算公式表达式不能为空")
            .MaximumLength(1000).WithMessage("计算公式表达式长度不能超过1000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新SalaryFormula 验证器
// ========================================

/// <summary>
/// 更新SalaryFormula DTO 验证器
/// </summary>
public class TaktSalaryFormulaUpdateValidator : AbstractValidator<TaktSalaryFormulaUpdateDto>
{
    /// <summary>
    /// 初始化 更新SalaryFormula 校验规则
    /// </summary>
    public TaktSalaryFormulaUpdateValidator()
    {
        RuleFor(x => x.SalaryFormulaId)
            .GreaterThan(0).WithMessage("SalaryFormulaID无效");
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
        RuleFor(x => x.SetCode)
            .NotEmpty().WithMessage("公式方案编码不能为空")
            .MaximumLength(40).WithMessage("公式方案编码长度不能超过40个字符");
        RuleFor(x => x.SetName)
            .NotEmpty().WithMessage("公式方案名称不能为空")
            .MaximumLength(80).WithMessage("公式方案名称长度不能超过80个字符");
        RuleFor(x => x.PayrollId)
            .GreaterThanOrEqualTo(0).WithMessage("薪酬体系不能为负数");
        RuleFor(x => x.FormulaCode)
            .NotEmpty().WithMessage("步骤编码不能为空")
            .MaximumLength(40).WithMessage("步骤编码长度不能超过40个字符");
        RuleFor(x => x.FormulaName)
            .NotEmpty().WithMessage("步骤名称不能为空")
            .MaximumLength(80).WithMessage("步骤名称长度不能超过80个字符");
        RuleFor(x => x.TargetField)
            .NotEmpty().WithMessage("结果写入字段不能为空")
            .MaximumLength(64).WithMessage("结果写入字段长度不能超过64个字符");
        RuleFor(x => x.FormulaExpression)
            .NotEmpty().WithMessage("计算公式表达式不能为空")
            .MaximumLength(1000).WithMessage("计算公式表达式长度不能超过1000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入SalaryFormula 验证器
// ========================================

/// <summary>
/// 导入SalaryFormula DTO 验证器
/// </summary>
public class TaktSalaryFormulaImportValidator : AbstractValidator<TaktSalaryFormulaImportDto>
{
    /// <summary>
    /// 初始化 导入SalaryFormula 校验规则
    /// </summary>
    public TaktSalaryFormulaImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.SetCode)
            .NotEmpty().WithMessage("公式方案编码不能为空")
            .MaximumLength(40).WithMessage("公式方案编码长度不能超过40个字符");
        RuleFor(x => x.SetName)
            .NotEmpty().WithMessage("公式方案名称不能为空")
            .MaximumLength(80).WithMessage("公式方案名称长度不能超过80个字符");
        RuleFor(x => x.PayrollId)
            .GreaterThanOrEqualTo(0).WithMessage("薪酬体系不能为负数");
        RuleFor(x => x.FormulaCode)
            .NotEmpty().WithMessage("步骤编码不能为空")
            .MaximumLength(40).WithMessage("步骤编码长度不能超过40个字符");
        RuleFor(x => x.FormulaName)
            .NotEmpty().WithMessage("步骤名称不能为空")
            .MaximumLength(80).WithMessage("步骤名称长度不能超过80个字符");
        RuleFor(x => x.TargetField)
            .NotEmpty().WithMessage("结果写入字段不能为空")
            .MaximumLength(64).WithMessage("结果写入字段长度不能超过64个字符");
        RuleFor(x => x.FormulaExpression)
            .NotEmpty().WithMessage("计算公式表达式不能为空")
            .MaximumLength(1000).WithMessage("计算公式表达式长度不能超过1000个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
