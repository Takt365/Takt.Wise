// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Code.Database
// 文件名称：TaktDataCloneValidators.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：DataClone 关联 DTO FluentValidation 验证器（由 generate-validators-from-entity.cjs 根据 TaktDataClone 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Code.Database;

namespace Takt.Application.Validators.Code.Database;

// ========================================
// DataClone 关联 DTO 验证器
// ========================================

/// <summary>
/// 关联 DataClone DTO 验证器
/// </summary>
public class TaktDataCloneDtoValidator : AbstractValidator<TaktDataCloneDto>
{
    /// <summary>
    /// 初始化 关联 DataClone 校验规则
    /// </summary>
    public TaktDataCloneDtoValidator()
    {
        RuleFor(x => x.SourceTenantCode)
            .NotEmpty().WithMessage("源租户编码不能为空")
            .MaximumLength(3).WithMessage("源租户编码长度不能超过3个字符");
        RuleFor(x => x.SourceDatabaseName)
            .NotEmpty().WithMessage("源数据库展示名不能为空")
            .MaximumLength(40).WithMessage("源数据库展示名长度不能超过40个字符");
        RuleFor(x => x.SourceTableName)
            .NotEmpty().WithMessage("源物理表名不能为空")
            .MaximumLength(40).WithMessage("源物理表名长度不能超过40个字符");
        RuleFor(x => x.SourceCompanyCode)
            .NotEmpty().WithMessage("源公司编码不能为空")
            .MaximumLength(4).WithMessage("源公司编码长度不能超过4个字符");
        RuleFor(x => x.TargetTenantCode)
            .NotEmpty().WithMessage("目标租户不能为空")
            .MaximumLength(3).WithMessage("目标租户长度不能超过3个字符");
        RuleFor(x => x.TargetDatabaseName)
            .NotEmpty().WithMessage("目标数据库展示名不能为空")
            .MaximumLength(40).WithMessage("目标数据库展示名长度不能超过40个字符");
        RuleFor(x => x.TargetTableName)
            .NotEmpty().WithMessage("目标物理表名不能为空")
            .MaximumLength(40).WithMessage("目标物理表名长度不能超过40个字符");
        RuleFor(x => x.TargetCompanyCode)
            .NotEmpty().WithMessage("目标公司编码不能为空")
            .MaximumLength(4).WithMessage("目标公司编码长度不能超过4个字符");
    }
}
