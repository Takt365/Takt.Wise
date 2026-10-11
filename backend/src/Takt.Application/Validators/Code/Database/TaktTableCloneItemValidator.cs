// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Validators.Code.Database
// 文件名称：TaktTableCloneItemValidator.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：跨租户整表克隆单表项 FluentValidation 验证器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using FluentValidation;
using Takt.Application.Dtos.Code.Database;

namespace Takt.Application.Validators.Code.Database;

/// <summary>
/// 跨租户整表克隆单表项验证器
/// </summary>
public class TaktTableCloneItemValidator : AbstractValidator<TaktTableCloneItemDto>
{
    /// <summary>
    /// 初始化验证规则
    /// </summary>
    public TaktTableCloneItemValidator()
    {
        RuleFor(x => x.SourceTableName)
            .NotEmpty().WithMessage("源数据表不能为空");
        RuleFor(x => x.TargetTableName)
            .NotEmpty().WithMessage("目标数据表不能为空");
    }
}
