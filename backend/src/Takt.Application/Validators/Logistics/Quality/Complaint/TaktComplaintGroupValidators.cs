// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Quality.Complaint
// 文件名称：TaktComplaintGroupValidators.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：ComplaintGroup 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktComplaintGroup 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Complaint;

namespace Takt.Application.Validators.Logistics.Quality.Complaint;

// ========================================
// 创建ComplaintGroup 验证器
// ========================================

/// <summary>
/// 创建ComplaintGroup DTO 验证器
/// </summary>
public class TaktComplaintGroupCreateValidator : AbstractValidator<TaktComplaintGroupCreateDto>
{
    /// <summary>
    /// 初始化 创建ComplaintGroup 校验规则
    /// </summary>
    public TaktComplaintGroupCreateValidator()
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
        RuleFor(x => x.ComplaintGroupCode)
            .NotEmpty().WithMessage("客诉组编码不能为空")
            .MaximumLength(6).WithMessage("客诉组编码长度不能超过6个字符");
        RuleFor(x => x.ComplaintGroupName)
            .NotEmpty().WithMessage("客诉组名称不能为空")
            .MaximumLength(80).WithMessage("客诉组名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新ComplaintGroup 验证器
// ========================================

/// <summary>
/// 更新ComplaintGroup DTO 验证器
/// </summary>
public class TaktComplaintGroupUpdateValidator : AbstractValidator<TaktComplaintGroupUpdateDto>
{
    /// <summary>
    /// 初始化 更新ComplaintGroup 校验规则
    /// </summary>
    public TaktComplaintGroupUpdateValidator()
    {
        RuleFor(x => x.ComplaintGroupId)
            .GreaterThan(0).WithMessage("ComplaintGroupID无效");
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
        RuleFor(x => x.ComplaintGroupCode)
            .NotEmpty().WithMessage("客诉组编码不能为空")
            .MaximumLength(6).WithMessage("客诉组编码长度不能超过6个字符");
        RuleFor(x => x.ComplaintGroupName)
            .NotEmpty().WithMessage("客诉组名称不能为空")
            .MaximumLength(80).WithMessage("客诉组名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入ComplaintGroup 验证器
// ========================================

/// <summary>
/// 导入ComplaintGroup DTO 验证器
/// </summary>
public class TaktComplaintGroupImportValidator : AbstractValidator<TaktComplaintGroupImportDto>
{
    /// <summary>
    /// 初始化 导入ComplaintGroup 校验规则
    /// </summary>
    public TaktComplaintGroupImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.ComplaintGroupCode)
            .NotEmpty().WithMessage("客诉组编码不能为空")
            .MaximumLength(6).WithMessage("客诉组编码长度不能超过6个字符");
        RuleFor(x => x.ComplaintGroupName)
            .NotEmpty().WithMessage("客诉组名称不能为空")
            .MaximumLength(80).WithMessage("客诉组名称长度不能超过80个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
