// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintHandlingValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：CustomerComplaintHandling 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCustomerComplaintHandling 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Complaint;

namespace Takt.Application.Validators.Logistics.Quality.Complaint;

// ========================================
// 创建CustomerComplaintHandling 验证器
// ========================================

/// <summary>
/// 创建CustomerComplaintHandling DTO 验证器
/// </summary>
public class TaktCustomerComplaintHandlingCreateValidator : AbstractValidator<TaktCustomerComplaintHandlingCreateDto>
{
    /// <summary>
    /// 初始化 创建CustomerComplaintHandling 校验规则
    /// </summary>
    public TaktCustomerComplaintHandlingCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ComplaintId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ComplaintId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ComplaintHandlingCode)
            .NotEmpty().WithMessage("客诉处理记录编码不能为空")
            .MaximumLength(20).WithMessage("客诉处理记录编码长度不能超过20个字符");
        RuleFor(x => x.ComplaintId)
            .GreaterThanOrEqualTo(0).WithMessage("客诉 ID不能为负数");
        RuleFor(x => x.ComplaintCode)
            .NotEmpty().WithMessage("客诉单号不能为空").When(x => x.ComplaintId <= 0)
            .MaximumLength(20).WithMessage("客诉单号长度不能超过20个字符");
        RuleFor(x => x.ComplaintItemId)
            .GreaterThanOrEqualTo(0).WithMessage("客诉明细 ID不能为负数");
        RuleFor(x => x.HandlingDescription)
            .NotEmpty().WithMessage("处理说明不能为空")
            .MaximumLength(70).WithMessage("处理说明长度不能超过70个字符");
        RuleFor(x => x.ResponsibleDeptId)
            .GreaterThanOrEqualTo(0).WithMessage("责任部门不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新CustomerComplaintHandling 验证器
// ========================================

/// <summary>
/// 更新CustomerComplaintHandling DTO 验证器
/// </summary>
public class TaktCustomerComplaintHandlingUpdateValidator : AbstractValidator<TaktCustomerComplaintHandlingUpdateDto>
{
    /// <summary>
    /// 初始化 更新CustomerComplaintHandling 校验规则
    /// </summary>
    public TaktCustomerComplaintHandlingUpdateValidator()
    {
        RuleFor(x => x.CustomerComplaintHandlingId)
            .GreaterThan(0).WithMessage("CustomerComplaintHandlingID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.ComplaintId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.ComplaintId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.ComplaintHandlingCode)
            .NotEmpty().WithMessage("客诉处理记录编码不能为空")
            .MaximumLength(20).WithMessage("客诉处理记录编码长度不能超过20个字符");
        RuleFor(x => x.ComplaintId)
            .GreaterThanOrEqualTo(0).WithMessage("客诉 ID不能为负数");
        RuleFor(x => x.ComplaintCode)
            .NotEmpty().WithMessage("客诉单号不能为空").When(x => x.ComplaintId <= 0)
            .MaximumLength(20).WithMessage("客诉单号长度不能超过20个字符");
        RuleFor(x => x.ComplaintItemId)
            .GreaterThanOrEqualTo(0).WithMessage("客诉明细 ID不能为负数");
        RuleFor(x => x.HandlingDescription)
            .NotEmpty().WithMessage("处理说明不能为空")
            .MaximumLength(70).WithMessage("处理说明长度不能超过70个字符");
        RuleFor(x => x.ResponsibleDeptId)
            .GreaterThanOrEqualTo(0).WithMessage("责任部门不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入CustomerComplaintHandling 验证器
// ========================================

/// <summary>
/// 导入CustomerComplaintHandling DTO 验证器
/// </summary>
public class TaktCustomerComplaintHandlingImportValidator : AbstractValidator<TaktCustomerComplaintHandlingImportDto>
{
    /// <summary>
    /// 初始化 导入CustomerComplaintHandling 校验规则
    /// </summary>
    public TaktCustomerComplaintHandlingImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ComplaintHandlingCode)
            .NotEmpty().WithMessage("客诉处理记录编码不能为空")
            .MaximumLength(20).WithMessage("客诉处理记录编码长度不能超过20个字符");
        RuleFor(x => x.ComplaintId)
            .GreaterThanOrEqualTo(0).WithMessage("客诉 ID不能为负数");
        RuleFor(x => x.ComplaintCode)
            .NotEmpty().WithMessage("客诉单号不能为空")
            .MaximumLength(20).WithMessage("客诉单号长度不能超过20个字符");
        RuleFor(x => x.ComplaintItemId)
            .GreaterThanOrEqualTo(0).WithMessage("客诉明细 ID不能为负数");
        RuleFor(x => x.HandlingDescription)
            .NotEmpty().WithMessage("处理说明不能为空")
            .MaximumLength(70).WithMessage("处理说明长度不能超过70个字符");
        RuleFor(x => x.ResponsibleDeptId)
            .GreaterThanOrEqualTo(0).WithMessage("责任部门不能为负数");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
