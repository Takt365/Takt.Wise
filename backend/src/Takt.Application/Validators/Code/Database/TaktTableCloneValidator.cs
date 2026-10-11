// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Code.Database
// 文件名称：TaktTableCloneValidator.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：跨租户整表数据克隆请求 FluentValidation 验证器（一次 1~5 张表）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Code.Database;
using Takt.Application.Services.Code.Database;

namespace Takt.Application.Validators.Code.Database;

/// <summary>
/// 跨租户整表数据克隆请求验证器
/// </summary>
public class TaktTableCloneValidator : AbstractValidator<TaktTableCloneDto>
{
    /// <summary>
    /// 初始化验证规则
    /// </summary>
    public TaktTableCloneValidator()
    {
        RuleFor(x => x.SourceTenantCode)
            .NotEmpty().WithMessage("源租户编码不能为空")
            .Length(3).WithMessage("源租户编码必须为 3 位");
        RuleFor(x => x.SourceDatabaseName)
            .NotEmpty().WithMessage("源数据库不能为空");
        RuleFor(x => x.TargetTenantCode)
            .NotEmpty().WithMessage("目标租户不能为空")
            .Length(3).WithMessage("目标租户必须为 3 位");
        RuleFor(x => x.TargetDatabaseName)
            .NotEmpty().WithMessage("目标数据库不能为空");
        RuleFor(x => x)
            .Must(x => !string.Equals(x.SourceTenantCode?.Trim(), x.TargetTenantCode?.Trim(), StringComparison.OrdinalIgnoreCase))
            .WithMessage("数据表克隆仅支持跨租户，同租户内不可克隆");
        RuleFor(x => x.Tables)
            .NotNull().WithMessage("表清单不能为空")
            .Must(tables => tables.Count >= 1 && tables.Count <= ITaktTableCloneService.MaxTableCountPerRequest)
            .WithMessage($"一次最多克隆 {ITaktTableCloneService.MaxTableCountPerRequest} 张表，且至少 1 张");
        RuleFor(x => x.Tables)
            .Must(tables => tables
                .Select(t => t.SourceTableName?.Trim().ToLowerInvariant())
                .Where(name => !string.IsNullOrEmpty(name))
                .Distinct()
                .Count() == tables.Count)
            .WithMessage("源数据表不能重复");
        RuleFor(x => x.Tables)
            .Must(tables => tables
                .Select(t => t.TargetTableName?.Trim().ToLowerInvariant())
                .Where(name => !string.IsNullOrEmpty(name))
                .Distinct()
                .Count() == tables.Count)
            .WithMessage("目标数据表不能重复");
        RuleForEach(x => x.Tables).SetValidator(new TaktTableCloneItemValidator());
    }
}
