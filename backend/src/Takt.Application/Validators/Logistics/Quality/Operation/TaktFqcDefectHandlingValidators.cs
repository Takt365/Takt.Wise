// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Quality.Operation
// 文件名称：TaktFqcDefectHandlingValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：FqcDefectHandling 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktFqcDefectHandling 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Operation;

namespace Takt.Application.Validators.Logistics.Quality.Operation;

// ========================================
// 创建FqcDefectHandling 验证器
// ========================================

/// <summary>
/// 创建FqcDefectHandling DTO 验证器
/// </summary>
public class TaktFqcDefectHandlingCreateValidator : AbstractValidator<TaktFqcDefectHandlingCreateDto>
{
    /// <summary>
    /// 初始化 创建FqcDefectHandling 校验规则
    /// </summary>
    public TaktFqcDefectHandlingCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.VerifyDeptId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.VerifyDeptId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.FqcDefectHandlingCode)
            .NotEmpty().WithMessage("FQC不良处理编码不能为空")
            .MaximumLength(20).WithMessage("FQC不良处理编码长度不能超过20个字符");
        RuleFor(x => x.FqcOrderItemId)
            .GreaterThanOrEqualTo(0).WithMessage("FQC检验单明细 ID不能为负数");
        RuleFor(x => x.FqcOrderCode)
            .NotEmpty().WithMessage("FQC检验单编码不能为空")
            .MaximumLength(20).WithMessage("FQC检验单编码长度不能超过20个字符");
        RuleFor(x => x.DefectCode)
            .NotEmpty().WithMessage("不良现象编码不能为空")
            .MaximumLength(20).WithMessage("不良现象编码长度不能超过20个字符");
        RuleFor(x => x.DefectDescription)
            .NotEmpty().WithMessage("不良现象描述不能为空")
            .MaximumLength(70).WithMessage("不良现象描述长度不能超过70个字符");
        RuleFor(x => x.VerifyDeptId)
            .GreaterThanOrEqualTo(0).WithMessage("验证部门不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新FqcDefectHandling 验证器
// ========================================

/// <summary>
/// 更新FqcDefectHandling DTO 验证器
/// </summary>
public class TaktFqcDefectHandlingUpdateValidator : AbstractValidator<TaktFqcDefectHandlingUpdateDto>
{
    /// <summary>
    /// 初始化 更新FqcDefectHandling 校验规则
    /// </summary>
    public TaktFqcDefectHandlingUpdateValidator()
    {
        RuleFor(x => x.FqcDefectHandlingId)
            .GreaterThan(0).WithMessage("FqcDefectHandlingID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.VerifyDeptId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.VerifyDeptId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.FqcDefectHandlingCode)
            .NotEmpty().WithMessage("FQC不良处理编码不能为空")
            .MaximumLength(20).WithMessage("FQC不良处理编码长度不能超过20个字符");
        RuleFor(x => x.FqcOrderItemId)
            .GreaterThanOrEqualTo(0).WithMessage("FQC检验单明细 ID不能为负数");
        RuleFor(x => x.FqcOrderCode)
            .NotEmpty().WithMessage("FQC检验单编码不能为空")
            .MaximumLength(20).WithMessage("FQC检验单编码长度不能超过20个字符");
        RuleFor(x => x.DefectCode)
            .NotEmpty().WithMessage("不良现象编码不能为空")
            .MaximumLength(20).WithMessage("不良现象编码长度不能超过20个字符");
        RuleFor(x => x.DefectDescription)
            .NotEmpty().WithMessage("不良现象描述不能为空")
            .MaximumLength(70).WithMessage("不良现象描述长度不能超过70个字符");
        RuleFor(x => x.VerifyDeptId)
            .GreaterThanOrEqualTo(0).WithMessage("验证部门不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入FqcDefectHandling 验证器
// ========================================

/// <summary>
/// 导入FqcDefectHandling DTO 验证器
/// </summary>
public class TaktFqcDefectHandlingImportValidator : AbstractValidator<TaktFqcDefectHandlingImportDto>
{
    /// <summary>
    /// 初始化 导入FqcDefectHandling 校验规则
    /// </summary>
    public TaktFqcDefectHandlingImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.FqcDefectHandlingCode)
            .NotEmpty().WithMessage("FQC不良处理编码不能为空")
            .MaximumLength(20).WithMessage("FQC不良处理编码长度不能超过20个字符");
        RuleFor(x => x.FqcOrderItemId)
            .GreaterThanOrEqualTo(0).WithMessage("FQC检验单明细 ID不能为负数");
        RuleFor(x => x.FqcOrderCode)
            .NotEmpty().WithMessage("FQC检验单编码不能为空")
            .MaximumLength(20).WithMessage("FQC检验单编码长度不能超过20个字符");
        RuleFor(x => x.DefectCode)
            .NotEmpty().WithMessage("不良现象编码不能为空")
            .MaximumLength(20).WithMessage("不良现象编码长度不能超过20个字符");
        RuleFor(x => x.DefectDescription)
            .NotEmpty().WithMessage("不良现象描述不能为空")
            .MaximumLength(70).WithMessage("不良现象描述长度不能超过70个字符");
        RuleFor(x => x.VerifyDeptId)
            .GreaterThanOrEqualTo(0).WithMessage("验证部门不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
