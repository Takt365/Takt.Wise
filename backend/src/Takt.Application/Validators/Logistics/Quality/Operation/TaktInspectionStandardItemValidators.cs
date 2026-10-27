// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Quality.Operation
// 文件名称：TaktInspectionStandardItemValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：InspectionStandardItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktInspectionStandardItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Operation;

namespace Takt.Application.Validators.Logistics.Quality.Operation;

// ========================================
// 创建InspectionStandardItem 验证器
// ========================================

/// <summary>
/// 创建InspectionStandardItem DTO 验证器
/// </summary>
public class TaktInspectionStandardItemCreateValidator : AbstractValidator<TaktInspectionStandardItemCreateDto>
{
    /// <summary>
    /// 初始化 创建InspectionStandardItem 校验规则
    /// </summary>
    public TaktInspectionStandardItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.InspectionStandardId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.InspectionStandardId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.InspectionStandardId)
            .GreaterThanOrEqualTo(0).WithMessage("检验标准 ID不能为负数");
        RuleFor(x => x.ItemCode)
            .NotEmpty().WithMessage("检验项目编码不能为空")
            .MaximumLength(20).WithMessage("检验项目编码长度不能超过20个字符");
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("检验项目名称不能为空")
            .MaximumLength(200).WithMessage("检验项目名称长度不能超过200个字符");
        RuleFor(x => x.DefectLevel)
            .NotEmpty().WithMessage("缺点等级不能为空")
            .MaximumLength(2).WithMessage("缺点等级长度不能超过2个字符");
        RuleFor(x => x.StandardValue)
            .NotEmpty().WithMessage("检验标准值不能为空")
            .MaximumLength(500).WithMessage("检验标准值长度不能超过500个字符");
        RuleFor(x => x.UpperLimit)
            .NotEmpty().WithMessage("检验上限值不能为空")
            .MaximumLength(100).WithMessage("检验上限值长度不能超过100个字符");
        RuleFor(x => x.LowerLimit)
            .NotEmpty().WithMessage("检验下限值不能为空")
            .MaximumLength(100).WithMessage("检验下限值长度不能超过100个字符");
        RuleFor(x => x.InspectionTool)
            .NotEmpty().WithMessage("检验工具不能为空")
            .MaximumLength(200).WithMessage("检验工具长度不能超过200个字符");
        RuleFor(x => x.InspectionMethodDescription)
            .NotEmpty().WithMessage("检验方法说明不能为空")
            .MaximumLength(70).WithMessage("检验方法说明长度不能超过70个字符");
        RuleFor(x => x.AcceptanceCriteria)
            .NotEmpty().WithMessage("接收标准不能为空")
            .MaximumLength(50).WithMessage("接收标准长度不能超过50个字符");
        RuleFor(x => x.RejectionCriteria)
            .NotEmpty().WithMessage("拒收标准不能为空")
            .MaximumLength(50).WithMessage("拒收标准长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新InspectionStandardItem 验证器
// ========================================

/// <summary>
/// 更新InspectionStandardItem DTO 验证器
/// </summary>
public class TaktInspectionStandardItemUpdateValidator : AbstractValidator<TaktInspectionStandardItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新InspectionStandardItem 校验规则
    /// </summary>
    public TaktInspectionStandardItemUpdateValidator()
    {
        RuleFor(x => x.InspectionStandardItemId)
            .GreaterThan(0).WithMessage("InspectionStandardItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.InspectionStandardId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.InspectionStandardId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.InspectionStandardId)
            .GreaterThanOrEqualTo(0).WithMessage("检验标准 ID不能为负数");
        RuleFor(x => x.ItemCode)
            .NotEmpty().WithMessage("检验项目编码不能为空")
            .MaximumLength(20).WithMessage("检验项目编码长度不能超过20个字符");
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("检验项目名称不能为空")
            .MaximumLength(200).WithMessage("检验项目名称长度不能超过200个字符");
        RuleFor(x => x.DefectLevel)
            .NotEmpty().WithMessage("缺点等级不能为空")
            .MaximumLength(2).WithMessage("缺点等级长度不能超过2个字符");
        RuleFor(x => x.StandardValue)
            .NotEmpty().WithMessage("检验标准值不能为空")
            .MaximumLength(500).WithMessage("检验标准值长度不能超过500个字符");
        RuleFor(x => x.UpperLimit)
            .NotEmpty().WithMessage("检验上限值不能为空")
            .MaximumLength(100).WithMessage("检验上限值长度不能超过100个字符");
        RuleFor(x => x.LowerLimit)
            .NotEmpty().WithMessage("检验下限值不能为空")
            .MaximumLength(100).WithMessage("检验下限值长度不能超过100个字符");
        RuleFor(x => x.InspectionTool)
            .NotEmpty().WithMessage("检验工具不能为空")
            .MaximumLength(200).WithMessage("检验工具长度不能超过200个字符");
        RuleFor(x => x.InspectionMethodDescription)
            .NotEmpty().WithMessage("检验方法说明不能为空")
            .MaximumLength(70).WithMessage("检验方法说明长度不能超过70个字符");
        RuleFor(x => x.AcceptanceCriteria)
            .NotEmpty().WithMessage("接收标准不能为空")
            .MaximumLength(50).WithMessage("接收标准长度不能超过50个字符");
        RuleFor(x => x.RejectionCriteria)
            .NotEmpty().WithMessage("拒收标准不能为空")
            .MaximumLength(50).WithMessage("拒收标准长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入InspectionStandardItem 验证器
// ========================================

/// <summary>
/// 导入InspectionStandardItem DTO 验证器
/// </summary>
public class TaktInspectionStandardItemImportValidator : AbstractValidator<TaktInspectionStandardItemImportDto>
{
    /// <summary>
    /// 初始化 导入InspectionStandardItem 校验规则
    /// </summary>
    public TaktInspectionStandardItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.InspectionStandardId)
            .GreaterThanOrEqualTo(0).WithMessage("检验标准 ID不能为负数");
        RuleFor(x => x.ItemCode)
            .NotEmpty().WithMessage("检验项目编码不能为空")
            .MaximumLength(20).WithMessage("检验项目编码长度不能超过20个字符");
        RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("检验项目名称不能为空")
            .MaximumLength(200).WithMessage("检验项目名称长度不能超过200个字符");
        RuleFor(x => x.DefectLevel)
            .NotEmpty().WithMessage("缺点等级不能为空")
            .MaximumLength(2).WithMessage("缺点等级长度不能超过2个字符");
        RuleFor(x => x.StandardValue)
            .NotEmpty().WithMessage("检验标准值不能为空")
            .MaximumLength(500).WithMessage("检验标准值长度不能超过500个字符");
        RuleFor(x => x.UpperLimit)
            .NotEmpty().WithMessage("检验上限值不能为空")
            .MaximumLength(100).WithMessage("检验上限值长度不能超过100个字符");
        RuleFor(x => x.LowerLimit)
            .NotEmpty().WithMessage("检验下限值不能为空")
            .MaximumLength(100).WithMessage("检验下限值长度不能超过100个字符");
        RuleFor(x => x.InspectionTool)
            .NotEmpty().WithMessage("检验工具不能为空")
            .MaximumLength(200).WithMessage("检验工具长度不能超过200个字符");
        RuleFor(x => x.InspectionMethodDescription)
            .NotEmpty().WithMessage("检验方法说明不能为空")
            .MaximumLength(70).WithMessage("检验方法说明长度不能超过70个字符");
        RuleFor(x => x.AcceptanceCriteria)
            .NotEmpty().WithMessage("接收标准不能为空")
            .MaximumLength(50).WithMessage("接收标准长度不能超过50个字符");
        RuleFor(x => x.RejectionCriteria)
            .NotEmpty().WithMessage("拒收标准不能为空")
            .MaximumLength(50).WithMessage("拒收标准长度不能超过50个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
