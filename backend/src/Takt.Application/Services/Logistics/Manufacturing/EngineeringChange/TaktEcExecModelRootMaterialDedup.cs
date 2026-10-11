// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExecModelRootMaterialDedup.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：生管/制一/品管/制技共用：设变单号+机种+根物料编码去重（保留最大 Id）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 同设变单号+机种编码+根物料编码仅保留最大 Id 一行（生管/制一/品管/制技同一套去重）
/// </summary>
internal static class TaktEcExecModelRootMaterialDedup
{
    /// <summary>
    /// 明细可见条件（明细表列表/主查询；数据量大时慎用于全表 Count）
    /// </summary>
    /// <returns>明细过滤表达式</returns>
    internal static Expression<Func<TaktEcDetail, bool>> VisibleDetailExpression()
    {
        var exp = Expressionable.Create<TaktEcDetail>();
        exp = exp.And(x =>
            !SqlFunc.Subqueryable<TaktEcDetail>()
                .Where(s =>
                    s.TenantCode == x.TenantCode
                    && s.CompanyCode == x.CompanyCode
                    && s.IsDeleted == 0
                    && s.IsObsolete == 0
                    && s.EcCode == x.EcCode
                    && s.EcModelCode == x.EcModelCode
                    && s.EcRootMaterialCode == x.EcRootMaterialCode
                    && s.Id > x.Id)
                .Any());
        return exp.ToExpression();
    }

    /// <summary>
    /// 部门执行列表可见：在本执行表内自去重（同设变+机种+根物料编码保留最大 EcDetailId）。
    /// 禁止嵌套扫 TaktEcDetail——大数据量下 CountAsync 会触发 SQL 超时。
    /// 落库阶段已按同键跳过非最大明细并作废旧行；本条件兜底历史重复行。
    /// </summary>
    /// <typeparam name="TExec">部门执行实体</typeparam>
    /// <returns>执行表过滤表达式</returns>
    internal static Expression<Func<TExec, bool>> VisibleExecExpression<TExec>()
        where TExec : class, ITaktEcExecModelRootMaterialRow, new()
    {
        return x => !SqlFunc.Subqueryable<TExec>()
            .Where(s =>
                s.TenantCode == x.TenantCode
                && s.CompanyCode == x.CompanyCode
                && s.IsDeleted == 0
                && s.IsObsolete == 0
                && s.EcCode == x.EcCode
                && s.EcModelCode == x.EcModelCode
                && s.EcRootMaterialCodeGroup == x.EcRootMaterialCodeGroup
                && s.EcDetailId > x.EcDetailId)
            .Any();
    }
}
