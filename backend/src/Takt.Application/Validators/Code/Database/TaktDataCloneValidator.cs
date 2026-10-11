// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Code.Database
// 文件名称：TaktDataCloneValidator.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级数据克隆请求 FluentValidation 验证器（一次一个公司、一张表）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Code.Database;

namespace Takt.Application.Validators.Code.Database;

/// <summary>
/// 公司级数据克隆请求验证器（DTO 结构即单源公司、单源表、单目标公司、单目标表）
/// </summary>
public class TaktDataCloneValidator : AbstractValidator<TaktDataCloneDto>
{
    /// <summary>
    /// 初始化验证规则
    /// </summary>
    public TaktDataCloneValidator()
    {
        RuleFor(x => x.SourceTenantCode)
            .NotEmpty().WithMessage("源租户编码不能为空")
            .Length(3).WithMessage("源租户编码必须为 3 位");
        RuleFor(x => x.SourceDatabaseName)
            .NotEmpty().WithMessage("源数据库不能为空");
        RuleFor(x => x.SourceTableName)
            .NotEmpty().WithMessage("源数据表不能为空");
        RuleFor(x => x.SourceCompanyCode)
            .NotEmpty().WithMessage("源公司编码不能为空")
            .Length(4).WithMessage("源公司编码必须为 4 位");
        RuleFor(x => x.TargetTenantCode)
            .NotEmpty().WithMessage("目标租户不能为空")
            .Length(3).WithMessage("目标租户必须为 3 位");
        RuleFor(x => x.TargetDatabaseName)
            .NotEmpty().WithMessage("目标数据库不能为空");
        RuleFor(x => x.TargetTableName)
            .NotEmpty().WithMessage("目标数据表不能为空");
        RuleFor(x => x.TargetCompanyCode)
            .NotEmpty().WithMessage("目标公司编码不能为空")
            .Length(4).WithMessage("目标公司编码必须为 4 位");
        RuleFor(x => x)
            .Must(x => !IsSameScope(x))
            .WithMessage("源与目标租户、公司、数据库、数据表不能完全相同");
    }

    /// <summary>
    /// 判断源与目标是否完全相同
    /// </summary>
    private static bool IsSameScope(TaktDataCloneDto dto)
    {
        return string.Equals(dto.SourceTenantCode?.Trim(), dto.TargetTenantCode?.Trim(), StringComparison.OrdinalIgnoreCase)
            && string.Equals(dto.SourceDatabaseName?.Trim(), dto.TargetDatabaseName?.Trim(), StringComparison.OrdinalIgnoreCase)
            && string.Equals(dto.SourceTableName?.Trim(), dto.TargetTableName?.Trim(), StringComparison.OrdinalIgnoreCase)
            && string.Equals(dto.SourceCompanyCode?.Trim(), dto.TargetCompanyCode?.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}
