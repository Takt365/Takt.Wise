// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktDictDataValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：DictData 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktDictData 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建DictData 验证器
// ========================================

/// <summary>
/// 创建DictData DTO 验证器
/// </summary>
public class TaktDictDataCreateValidator : AbstractValidator<TaktDictDataCreateDto>
{
    /// <summary>
    /// 初始化 创建DictData 校验规则
    /// </summary>
    public TaktDictDataCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.DictTypeId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.DictTypeId)
            .GreaterThanOrEqualTo(0).WithMessage("字典类型不能为负数");
        RuleFor(x => x.DictTypeCode)
            .NotEmpty().WithMessage("字典类型编码不能为空").When(x => x.DictTypeId <= 0)
            .MaximumLength(140).WithMessage("字典类型编码长度不能超过140个字符");
        RuleFor(x => x.DictLabel)
            .NotEmpty().WithMessage("字典项标签不能为空")
            .MaximumLength(40).WithMessage("字典项标签长度不能超过40个字符");
        RuleFor(x => x.DictValue)
            .NotEmpty().WithMessage("字典项值不能为空")
            .MaximumLength(40).WithMessage("字典项值长度不能超过40个字符");
        RuleFor(x => x.I18nKey)
            .NotEmpty().WithMessage("国际化键不能为空")
            .MaximumLength(140).WithMessage("国际化键长度不能超过140个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新DictData 验证器
// ========================================

/// <summary>
/// 更新DictData DTO 验证器
/// </summary>
public class TaktDictDataUpdateValidator : AbstractValidator<TaktDictDataUpdateDto>
{
    /// <summary>
    /// 初始化 更新DictData 校验规则
    /// </summary>
    public TaktDictDataUpdateValidator()
    {
        RuleFor(x => x.DictDataId)
            .GreaterThan(0).WithMessage("DictDataID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CultureCode)
            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.DictTypeId <= 0)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符");
        RuleFor(x => x.DictTypeId)
            .GreaterThanOrEqualTo(0).WithMessage("字典类型不能为负数");
        RuleFor(x => x.DictTypeCode)
            .NotEmpty().WithMessage("字典类型编码不能为空").When(x => x.DictTypeId <= 0)
            .MaximumLength(140).WithMessage("字典类型编码长度不能超过140个字符");
        RuleFor(x => x.DictLabel)
            .NotEmpty().WithMessage("字典项标签不能为空")
            .MaximumLength(40).WithMessage("字典项标签长度不能超过40个字符");
        RuleFor(x => x.DictValue)
            .NotEmpty().WithMessage("字典项值不能为空")
            .MaximumLength(40).WithMessage("字典项值长度不能超过40个字符");
        RuleFor(x => x.I18nKey)
            .NotEmpty().WithMessage("国际化键不能为空")
            .MaximumLength(140).WithMessage("国际化键长度不能超过140个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入DictData 验证器
// ========================================

/// <summary>
/// 导入DictData DTO 验证器
/// </summary>
public class TaktDictDataImportValidator : AbstractValidator<TaktDictDataImportDto>
{
    /// <summary>
    /// 初始化 导入DictData 校验规则
    /// </summary>
    public TaktDictDataImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.DictTypeId)
            .GreaterThanOrEqualTo(0).WithMessage("字典类型不能为负数");
        RuleFor(x => x.DictTypeCode)
            .NotEmpty().WithMessage("字典类型编码不能为空")
            .MaximumLength(140).WithMessage("字典类型编码长度不能超过140个字符");
        RuleFor(x => x.DictLabel)
            .NotEmpty().WithMessage("字典项标签不能为空")
            .MaximumLength(40).WithMessage("字典项标签长度不能超过40个字符");
        RuleFor(x => x.DictValue)
            .NotEmpty().WithMessage("字典项值不能为空")
            .MaximumLength(40).WithMessage("字典项值长度不能超过40个字符");
        RuleFor(x => x.I18nKey)
            .NotEmpty().WithMessage("国际化键不能为空")
            .MaximumLength(140).WithMessage("国际化键长度不能超过140个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
