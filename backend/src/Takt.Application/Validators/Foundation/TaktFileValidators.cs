// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Foundation
// 文件名称：TaktFileValidators.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：File 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktFile 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Foundation;

namespace Takt.Application.Validators.Foundation;

// ========================================
// 创建File 验证器
// ========================================

/// <summary>
/// 创建File DTO 验证器
/// </summary>
public class TaktFileCreateValidator : AbstractValidator<TaktFileCreateDto>
{
    /// <summary>
    /// 初始化 创建File 校验规则
    /// </summary>
    public TaktFileCreateValidator()
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
        RuleFor(x => x.FileCode)
            .NotEmpty().WithMessage("文件编码不能为空")
            .MaximumLength(50).WithMessage("文件编码长度不能超过50个字符");
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名称不能为空")
            .MaximumLength(200).WithMessage("文件名称长度不能超过200个字符");
        RuleFor(x => x.FileOriginalName)
            .NotEmpty().WithMessage("文件原始名称不能为空")
            .MaximumLength(200).WithMessage("文件原始名称长度不能超过200个字符");
        RuleFor(x => x.FilePath)
            .NotEmpty().WithMessage("文件路径不能为空")
            .MaximumLength(500).WithMessage("文件路径长度不能超过500个字符");
        RuleFor(x => x.FileType)
            .NotEmpty().WithMessage("文件 MIME 类型不能为空")
            .MaximumLength(100).WithMessage("文件 MIME 类型长度不能超过100个字符");
        RuleFor(x => x.FileExtension)
            .NotEmpty().WithMessage("文件扩展名不能为空")
            .MaximumLength(20).WithMessage("文件扩展名长度不能超过20个字符");
        RuleFor(x => x.FileHash)
            .MaximumLength(64).WithMessage("文件哈希值长度不能超过64个字符");
        RuleFor(x => x.AccessUrl)
            .NotEmpty().WithMessage("访问地址不能为空")
            .MaximumLength(1000).WithMessage("访问地址长度不能超过1000个字符");
        RuleFor(x => x.FileDescription)
            .NotEmpty().WithMessage("文件描述不能为空")
            .MaximumLength(500).WithMessage("文件描述长度不能超过500个字符");
        RuleFor(x => x.FileTags)
            .NotEmpty().WithMessage("文件标签不能为空")
            .MaximumLength(200).WithMessage("文件标签长度不能超过200个字符");
        RuleFor(x => x.IpAddress)
            .NotEmpty().WithMessage("IP 地址不能为空")
            .MaximumLength(50).WithMessage("IP 地址长度不能超过50个字符");
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("位置不能为空")
            .MaximumLength(200).WithMessage("位置长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新File 验证器
// ========================================

/// <summary>
/// 更新File DTO 验证器
/// </summary>
public class TaktFileUpdateValidator : AbstractValidator<TaktFileUpdateDto>
{
    /// <summary>
    /// 初始化 更新File 校验规则
    /// </summary>
    public TaktFileUpdateValidator()
    {
        RuleFor(x => x.FileId)
            .GreaterThan(0).WithMessage("FileID无效");
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
        RuleFor(x => x.FileCode)
            .NotEmpty().WithMessage("文件编码不能为空")
            .MaximumLength(50).WithMessage("文件编码长度不能超过50个字符");
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名称不能为空")
            .MaximumLength(200).WithMessage("文件名称长度不能超过200个字符");
        RuleFor(x => x.FileOriginalName)
            .NotEmpty().WithMessage("文件原始名称不能为空")
            .MaximumLength(200).WithMessage("文件原始名称长度不能超过200个字符");
        RuleFor(x => x.FilePath)
            .NotEmpty().WithMessage("文件路径不能为空")
            .MaximumLength(500).WithMessage("文件路径长度不能超过500个字符");
        RuleFor(x => x.FileType)
            .NotEmpty().WithMessage("文件 MIME 类型不能为空")
            .MaximumLength(100).WithMessage("文件 MIME 类型长度不能超过100个字符");
        RuleFor(x => x.FileExtension)
            .NotEmpty().WithMessage("文件扩展名不能为空")
            .MaximumLength(20).WithMessage("文件扩展名长度不能超过20个字符");
        RuleFor(x => x.FileHash)
            .MaximumLength(64).WithMessage("文件哈希值长度不能超过64个字符");
        RuleFor(x => x.AccessUrl)
            .NotEmpty().WithMessage("访问地址不能为空")
            .MaximumLength(1000).WithMessage("访问地址长度不能超过1000个字符");
        RuleFor(x => x.FileDescription)
            .NotEmpty().WithMessage("文件描述不能为空")
            .MaximumLength(500).WithMessage("文件描述长度不能超过500个字符");
        RuleFor(x => x.FileTags)
            .NotEmpty().WithMessage("文件标签不能为空")
            .MaximumLength(200).WithMessage("文件标签长度不能超过200个字符");
        RuleFor(x => x.IpAddress)
            .NotEmpty().WithMessage("IP 地址不能为空")
            .MaximumLength(50).WithMessage("IP 地址长度不能超过50个字符");
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("位置不能为空")
            .MaximumLength(200).WithMessage("位置长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入File 验证器
// ========================================

/// <summary>
/// 导入File DTO 验证器
/// </summary>
public class TaktFileImportValidator : AbstractValidator<TaktFileImportDto>
{
    /// <summary>
    /// 初始化 导入File 校验规则
    /// </summary>
    public TaktFileImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.CultureCode)
            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符").When(x => !string.IsNullOrWhiteSpace(x.CultureCode));
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.PlantCode));
        RuleFor(x => x.FileCode)
            .NotEmpty().WithMessage("文件编码不能为空")
            .MaximumLength(50).WithMessage("文件编码长度不能超过50个字符");
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("文件名称不能为空")
            .MaximumLength(200).WithMessage("文件名称长度不能超过200个字符");
        RuleFor(x => x.FileOriginalName)
            .NotEmpty().WithMessage("文件原始名称不能为空")
            .MaximumLength(200).WithMessage("文件原始名称长度不能超过200个字符");
        RuleFor(x => x.FilePath)
            .NotEmpty().WithMessage("文件路径不能为空")
            .MaximumLength(500).WithMessage("文件路径长度不能超过500个字符");
        RuleFor(x => x.FileType)
            .NotEmpty().WithMessage("文件 MIME 类型不能为空")
            .MaximumLength(100).WithMessage("文件 MIME 类型长度不能超过100个字符");
        RuleFor(x => x.FileExtension)
            .NotEmpty().WithMessage("文件扩展名不能为空")
            .MaximumLength(20).WithMessage("文件扩展名长度不能超过20个字符");
        RuleFor(x => x.FileHash)
            .MaximumLength(64).WithMessage("文件哈希值长度不能超过64个字符").When(x => !string.IsNullOrWhiteSpace(x.FileHash));
        RuleFor(x => x.AccessUrl)
            .NotEmpty().WithMessage("访问地址不能为空")
            .MaximumLength(1000).WithMessage("访问地址长度不能超过1000个字符");
        RuleFor(x => x.FileDescription)
            .NotEmpty().WithMessage("文件描述不能为空")
            .MaximumLength(500).WithMessage("文件描述长度不能超过500个字符");
        RuleFor(x => x.FileTags)
            .NotEmpty().WithMessage("文件标签不能为空")
            .MaximumLength(200).WithMessage("文件标签长度不能超过200个字符");
        RuleFor(x => x.IpAddress)
            .NotEmpty().WithMessage("IP 地址不能为空")
            .MaximumLength(50).WithMessage("IP 地址长度不能超过50个字符");
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("位置不能为空")
            .MaximumLength(200).WithMessage("位置长度不能超过200个字符");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
