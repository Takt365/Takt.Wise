// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Logistics.Quality.Operation
// 文件名称：TaktFqcOrderItemValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：FqcOrderItem 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktFqcOrderItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Logistics.Quality.Operation;

namespace Takt.Application.Validators.Logistics.Quality.Operation;

// ========================================
// 创建FqcOrderItem 验证器
// ========================================

/// <summary>
/// 创建FqcOrderItem DTO 验证器
/// </summary>
public class TaktFqcOrderItemCreateValidator : AbstractValidator<TaktFqcOrderItemCreateDto>
{
    /// <summary>
    /// 初始化 创建FqcOrderItem 校验规则
    /// </summary>
    public TaktFqcOrderItemCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.FqcOrderId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.FqcOrderId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.FqcOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("FQC检验单 ID不能为负数");
        RuleFor(x => x.FqcOrderCode)
            .NotEmpty().WithMessage("FQC检验单编码不能为空").When(x => x.FqcOrderId <= 0)
            .MaximumLength(20).WithMessage("FQC检验单编码长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(70).WithMessage("物料描述长度不能超过70个字符");
        RuleFor(x => x.StandardCode)
            .NotEmpty().WithMessage("检验标准编码不能为空")
            .MaximumLength(20).WithMessage("检验标准编码长度不能超过20个字符");
        RuleFor(x => x.SamplingSchemeCode)
            .NotEmpty().WithMessage("抽样方案编码不能为空")
            .MaximumLength(20).WithMessage("抽样方案编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新FqcOrderItem 验证器
// ========================================

/// <summary>
/// 更新FqcOrderItem DTO 验证器
/// </summary>
public class TaktFqcOrderItemUpdateValidator : AbstractValidator<TaktFqcOrderItemUpdateDto>
{
    /// <summary>
    /// 初始化 更新FqcOrderItem 校验规则
    /// </summary>
    public TaktFqcOrderItemUpdateValidator()
    {
        RuleFor(x => x.FqcOrderItemId)
            .GreaterThan(0).WithMessage("FqcOrderItemID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.FqcOrderId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.FqcOrderId <= 0)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");
        RuleFor(x => x.FqcOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("FQC检验单 ID不能为负数");
        RuleFor(x => x.FqcOrderCode)
            .NotEmpty().WithMessage("FQC检验单编码不能为空").When(x => x.FqcOrderId <= 0)
            .MaximumLength(20).WithMessage("FQC检验单编码长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(70).WithMessage("物料描述长度不能超过70个字符");
        RuleFor(x => x.StandardCode)
            .NotEmpty().WithMessage("检验标准编码不能为空")
            .MaximumLength(20).WithMessage("检验标准编码长度不能超过20个字符");
        RuleFor(x => x.SamplingSchemeCode)
            .NotEmpty().WithMessage("抽样方案编码不能为空")
            .MaximumLength(20).WithMessage("抽样方案编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入FqcOrderItem 验证器
// ========================================

/// <summary>
/// 导入FqcOrderItem DTO 验证器
/// </summary>
public class TaktFqcOrderItemImportValidator : AbstractValidator<TaktFqcOrderItemImportDto>
{
    /// <summary>
    /// 初始化 导入FqcOrderItem 校验规则
    /// </summary>
    public TaktFqcOrderItemImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.FqcOrderId)
            .GreaterThanOrEqualTo(0).WithMessage("FQC检验单 ID不能为负数");
        RuleFor(x => x.FqcOrderCode)
            .NotEmpty().WithMessage("FQC检验单编码不能为空")
            .MaximumLength(20).WithMessage("FQC检验单编码长度不能超过20个字符");
        RuleFor(x => x.MaterialCode)
            .NotEmpty().WithMessage("物料编码不能为空")
            .MaximumLength(20).WithMessage("物料编码长度不能超过20个字符");
        RuleFor(x => x.MaterialDescription)
            .NotEmpty().WithMessage("物料描述不能为空")
            .MaximumLength(70).WithMessage("物料描述长度不能超过70个字符");
        RuleFor(x => x.StandardCode)
            .NotEmpty().WithMessage("检验标准编码不能为空")
            .MaximumLength(20).WithMessage("检验标准编码长度不能超过20个字符");
        RuleFor(x => x.SamplingSchemeCode)
            .NotEmpty().WithMessage("抽样方案编码不能为空")
            .MaximumLength(20).WithMessage("抽样方案编码长度不能超过20个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
