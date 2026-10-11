// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktDatabaseSchemaProvider.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：租户库元数据 introspect 与按实体 CodeFirst 建表（Infrastructure 实现）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Models.Code;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 租户库 Schema 元数据提供者
/// </summary>
public interface ITaktDatabaseSchemaProvider
{
    /// <summary>
    /// 获取可 introspect 的租户业务库列表
    /// </summary>
    /// <returns>数据库摘要列表</returns>
    Task<IReadOnlyList<TaktDatabaseInfo>> GetDatabasesAsync();

    /// <summary>
    /// 获取指定租户库下所有用户表
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>SqlSugar 表元数据列表</returns>
    Task<IReadOnlyList<DbTableInfo>> GetTablesAsync(string tenantCode);

    /// <summary>
    /// 获取指定表的列元数据
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="tableName">表名</param>
    /// <returns>列摘要列表</returns>
    Task<IReadOnlyList<TaktDatabaseTableColumnInfo>> GetColumnsAsync(string tenantCode, string tableName);

    /// <summary>
    /// 获取表注释
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="tableName">表名</param>
    /// <returns>表注释</returns>
    Task<string?> GetTableCommentAsync(string tenantCode, string tableName);

    /// <summary>
    /// 按实体类型在指定租户库 CodeFirst 建表
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="entityTypeFullName">实体类型全名</param>
    /// <returns>任务</returns>
    Task InitializeTableFromEntityTypeAsync(string tenantCode, string entityTypeFullName);

    /// <summary>
    /// 获取 Domain 已加载的实体基类派生类型全名
    /// </summary>
    /// <returns>类型全名列表</returns>
    Task<IReadOnlyList<string>> GetAvailableEntityTypeFullNamesAsync();
}
