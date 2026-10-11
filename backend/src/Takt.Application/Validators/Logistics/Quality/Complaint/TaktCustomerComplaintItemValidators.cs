// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintItemValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：CustomerComplaintItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktCustomerComplaintItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Complaint;

namespace Takt.Application.Validators.Logistics.Quality.Complaint;

// ========================================
// 创建CustomerComplaintItem 验证器
// ========================================

/// <summary>
/// 创建CustomerComplaintItem DTO 验证器
/// </summary>
public class TaktCustomerComplaintItemCreateValidator : AbstractValidator<TaktCustomerComplaintItemCreateDto>
{
    /// <summary>
    /// 初始化 创建CustomerComplaintItem 校验规则
    /// </summary>
    public TaktCustomerComplaintItemCreateValidator()
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
        RuleFor(x => x.ComplaintId)
            .GreaterThanOrEqualTo(0).WithMessage("客诉 ID不能为负数");
        RuleFor(x => x.CustomerComplaintCode)
            .NotEmpty().WithMessage("客诉单号不能为空")
            .MaximumLength(20).WithMessage("客诉单号长度不能超过20个字符");
        RuleFor(x => x.DefectDescription)
            .NotEmpty().WithMessage("不良现象描述不能为空")
            .MaximumLength(70).WithMessage("不良现象描述长度不能超过70个字符");
        RuleFor(x => x.DefectLevel)
            .NotEmpty().WithMessage("缺点等级不能为空")
            .MaximumLength(2).WithMessage("缺点等级长度不能超过2个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新CustomerComplaintItem 验证器
// ========================================

/// <summary>
/// 更新CustomerComplaintItem DTO 验证器
/// </summary>
public class TaktCustomerComplaintItemUpdateValidator : AbstractValidator<TaktCustomerComplaintItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新CustomerComplaintItem 校验规则
    /// </summary>
    public TaktCustomerComplaintItemUpdateValidator()
    {
        RuleFor(x => x.CustomerComplaintItemId)
            .GreaterThan(0).WithMessage("CustomerComplaintItemID无效");
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
        RuleFor(x => x.ComplaintId)
            .GreaterThanOrEqualTo(0).WithMessage("客诉 ID不能为负数");
        RuleFor(x => x.CustomerComplaintCode)
            .NotEmpty().WithMessage("客诉单号不能为空")
            .MaximumLength(20).WithMessage("客诉单号长度不能超过20个字符");
        RuleFor(x => x.DefectDescription)
            .NotEmpty().WithMessage("不良现象描述不能为空")
            .MaximumLength(70).WithMessage("不良现象描述长度不能超过70个字符");
        RuleFor(x => x.DefectLevel)
            .NotEmpty().WithMessage("缺点等级不能为空")
            .MaximumLength(2).WithMessage("缺点等级长度不能超过2个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入CustomerComplaintItem 验证器
// ========================================

/// <summary>
/// 导入CustomerComplaintItem DTO 验证器
/// </summary>
public class TaktCustomerComplaintItemImportValidator : AbstractValidator<TaktCustomerComplaintItemImportDto>
{
    /// <summary>
    /// 初始化 导入CustomerComplaintItem 校验规则
    /// </summary>
    public TaktCustomerComplaintItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ComplaintId)
            .GreaterThanOrEqualTo(0).WithMessage("客诉 ID不能为负数");
        RuleFor(x => x.CustomerComplaintCode)
            .NotEmpty().WithMessage("客诉单号不能为空")
            .MaximumLength(20).WithMessage("客诉单号长度不能超过20个字符");
        RuleFor(x => x.DefectDescription)
            .NotEmpty().WithMessage("不良现象描述不能为空")
            .MaximumLength(70).WithMessage("不良现象描述长度不能超过70个字符");
        RuleFor(x => x.DefectLevel)
            .NotEmpty().WithMessage("缺点等级不能为空")
            .MaximumLength(2).WithMessage("缺点等级长度不能超过2个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
