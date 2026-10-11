// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizougijutsuQueryHelper.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：制技课主表/子表可见明细：设变单号+机种+根物料编码去重
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 制技课列表可见设变明细条件
/// </summary>
internal static class TaktEcSeizougijutsuQueryHelper
{
    /// <summary>
    /// 制技课明细可见条件（设变+机种+根物料编码去重）
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> VisibleDetailExpression() =>
        TaktEcExecModelRootMaterialDedup.VisibleDetailExpression();

    /// <summary>
    /// 制技执行行对应可见明细
    /// </summary>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TaktEcSeizougijutsu, bool>> VisibleExecExpression() =>
        TaktEcExecModelRootMaterialDedup.VisibleExecExpression<TaktEcSeizougijutsu>();
}
