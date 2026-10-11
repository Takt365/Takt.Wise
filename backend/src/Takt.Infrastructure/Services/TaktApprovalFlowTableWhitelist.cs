// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktApprovalFlowTableWhitelist.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：审批实体物理表名白名单（扫描 TaktApprovalEntityBase 子类 SugarTable）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Reflection;
using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Infrastructure.Services;

/// <summary>
/// 审批业务表白名单（禁止任意表名写入）
/// </summary>
internal static class TaktApprovalFlowTableWhitelist
{
    private static readonly Lazy<HashSet<string>> AllowedTables = new(BuildAllowedTables);

    /// <summary>
    /// 表名是否在白名单内
    /// </summary>
    /// <param name="tableName">物理表名</param>
    /// <returns>是否允许</returns>
    public static bool IsAllowed(string tableName)
    {
        return !string.IsNullOrWhiteSpace(tableName) && AllowedTables.Value.Contains(tableName);
    }

    /// <summary>
    /// 获取全部白名单表名（供表单数据源与引擎元数据 API）
    /// </summary>
    /// <returns>物理表名只读列表</returns>
    public static IReadOnlyList<string> GetAllowedTableNames()
    {
        return AllowedTables.Value.ToList();
    }

    /// <summary>
    /// 扫描审批实体表名
    /// </summary>
    private static HashSet<string> BuildAllowedTables()
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var assembly = typeof(TaktApprovalEntityBase).Assembly;
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract || !typeof(TaktApprovalEntityBase).IsAssignableFrom(type))
            {
                continue;
            }
            var sugarTable = type.GetCustomAttribute<SugarTable>();
            if (!string.IsNullOrWhiteSpace(sugarTable?.TableName))
            {
                set.Add(sugarTable.TableName);
            }
        }
        return set;
    }
}
