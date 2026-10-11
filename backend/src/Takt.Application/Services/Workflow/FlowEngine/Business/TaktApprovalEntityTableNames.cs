// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow.FlowEngine.Business
// 文件名称：TaktApprovalEntityTableNames.cs
// 创建时间：2026-06-18
// 创建人：Takt365(Cursor AI)
// 功能描述：从 TaktApprovalEntityBase 子类 SugarTable 解析物理表名（与表单 RelatedTableName 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Reflection;
using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Application.Services.Workflow.FlowEngine.Business;

/// <summary>
/// 审批实体物理表名（单一来源：实体 SugarTable）
/// </summary>
internal static class TaktApprovalEntityTableNames
{
    /// <summary>
    /// 获取审批实体物理表名
    /// </summary>
    /// <typeparam name="TEntity">审批实体类型</typeparam>
    /// <returns>物理表名</returns>
    /// <exception cref="InvalidOperationException">实体未配置 SugarTable.TableName</exception>
    public static string Of<TEntity>() where TEntity : TaktApprovalEntityBase
    {
        var sugarTable = typeof(TEntity).GetCustomAttribute<SugarTable>();
        if (string.IsNullOrWhiteSpace(sugarTable?.TableName))
        {
            throw new InvalidOperationException($"实体 {typeof(TEntity).Name} 未配置 SugarTable.TableName");
        }
        return sugarTable.TableName;
    }
}
