// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Code.Database
// 文件名称：TaktTableArchiveValidators.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Auto Generated)
// 功能描述：TableArchive 模块 FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktTableArchive 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Code.Database;

namespace Takt.Application.Validators.Code.Database;

// ========================================
// 创建TableArchive 验证器
// ========================================

/// <summary>
/// 创建TableArchive DTO 验证器
/// </summary>
public class TaktTableArchiveCreateValidator : AbstractValidator<TaktTableArchiveCreateDto>
{
    /// <summary>
    /// 初始化 创建TableArchive 校验规则
    /// </summary>
    public TaktTableArchiveCreateValidator()
    {
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.TargetTenantCode)
            .NotEmpty().WithMessage("目标租户不能为空")
            .MaximumLength(3).WithMessage("目标租户长度不能超过3个字符");
        RuleFor(x => x.TargetDatabaseName)
            .NotEmpty().WithMessage("目标数据库展示名不能为空")
            .MaximumLength(40).WithMessage("目标数据库展示名长度不能超过40个字符");
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("物理表名不能为空")
            .MaximumLength(128).WithMessage("物理表名长度不能超过128个字符");
        RuleFor(x => x.ArchiveKeyColumn)
            .NotEmpty().WithMessage("归档键列名不能为空")
            .MaximumLength(64).WithMessage("归档键列名长度不能超过64个字符");
        RuleFor(x => x.ArchiveKeyKind)
            .InclusiveBetween(1, 3).WithMessage("归档键类型须为 1/2/3");
        RuleFor(x => x.RetainHotYears)
            .Equal(1).WithMessage("热库保留年数固定为 1");
        RuleFor(x => x.PlantCode)
            .NotEmpty().WithMessage("工厂代码不能为空")
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符");

        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 更新TableArchive 验证器
// ========================================

/// <summary>
/// 更新TableArchive DTO 验证器
/// </summary>
public class TaktTableArchiveUpdateValidator : AbstractValidator<TaktTableArchiveUpdateDto>
{
    /// <summary>
    /// 初始化 更新TableArchive 校验规则
    /// </summary>
    public TaktTableArchiveUpdateValidator()
    {
        RuleFor(x => x.TableArchiveId)
            .GreaterThan(0).WithMessage("TableArchiveID无效");
        RuleFor(x => x.TenantCode)
            .NotEmpty().WithMessage("租户编码不能为空")
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符");
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("公司代码不能为空")
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符");
        RuleFor(x => x.TargetTenantCode)
            .NotEmpty().WithMessage("目标租户不能为空")
            .MaximumLength(3).WithMessage("目标租户长度不能超过3个字符");
        RuleFor(x => x.TargetDatabaseName)
            .NotEmpty().WithMessage("目标数据库展示名不能为空")
            .MaximumLength(40).WithMessage("目标数据库展示名长度不能超过40个字符");
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("物理表名不能为空")
            .MaximumLength(128).WithMessage("物理表名长度不能超过128个字符");
        RuleFor(x => x.ArchiveKeyColumn)
            .NotEmpty().WithMessage("归档键列名不能为空")
            .MaximumLength(64).WithMessage("归档键列名长度不能超过64个字符");
        RuleFor(x => x.ArchiveKeyKind)
            .InclusiveBetween(1, 3).WithMessage("归档键类型须为 1/2/3");
        RuleFor(x => x.RetainHotYears)
            .Equal(1).WithMessage("热库保留年数固定为 1");
        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符");
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符");
    }
}

// ========================================
// 导入TableArchive 验证器
// ========================================

/// <summary>
/// 导入TableArchive DTO 验证器
/// </summary>
public class TaktTableArchiveImportValidator : AbstractValidator<TaktTableArchiveImportDto>
{
    /// <summary>
    /// 初始化 导入TableArchive 校验规则
    /// </summary>
    public TaktTableArchiveImportValidator()
    {
        RuleFor(x => x.TenantCode)
            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符").When(x => !string.IsNullOrWhiteSpace(x.TenantCode));
        RuleFor(x => x.CompanyCode)
            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符").When(x => !string.IsNullOrWhiteSpace(x.CompanyCode));
        RuleFor(x => x.TargetTenantCode)
            .NotEmpty().WithMessage("目标租户不能为空")
            .MaximumLength(3).WithMessage("目标租户长度不能超过3个字符");
        RuleFor(x => x.TargetDatabaseName)
            .NotEmpty().WithMessage("目标数据库展示名不能为空")
            .MaximumLength(40).WithMessage("目标数据库展示名长度不能超过40个字符");
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("物理表名不能为空")
            .MaximumLength(128).WithMessage("物理表名长度不能超过128个字符");
        RuleFor(x => x.ArchiveKeyColumn)
            .NotEmpty().WithMessage("归档键列名不能为空")
            .MaximumLength(64).WithMessage("归档键列名长度不能超过64个字符");
        RuleFor(x => x.ArchiveKeyKind)
            .NotNull().WithMessage("归档键类型不能为空")
            .InclusiveBetween(1, 3).WithMessage("归档键类型须为 1/2/3");
        RuleFor(x => x.RetainHotYears)
            .Equal(1).WithMessage("热库保留年数固定为 1")
            .When(x => x.RetainHotYears.HasValue);
        RuleFor(x => x.PlantCode)
            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符")
            .When(x => !string.IsNullOrWhiteSpace(x.PlantCode));

        RuleFor(x => x.ExtField)
            .MaximumLength(4000).WithMessage("扩展字段JSON长度不能超过4000个字符").When(x => !string.IsNullOrWhiteSpace(x.ExtField));
        RuleFor(x => x.Remark)
            .MaximumLength(500).WithMessage("备注长度不能超过500个字符").When(x => !string.IsNullOrWhiteSpace(x.Remark));
    }
}
