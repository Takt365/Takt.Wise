// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktSqlFuncMedian.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：SqlSugar 中位数 SqlFunc 扩展（PERCENTILE_CONT 多库兼容；MySql/Sqlite 由仓储层有序切片回退）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Collections.Frozen;
using SqlSugar;

namespace Takt.Shared.Helpers;

/// <summary>
/// SqlSugar 中位数扩展函数（须在 ConnectionConfig 注册 SqlFuncExternal）
/// </summary>
public static class TaktSqlFuncMedian
{
    /// <summary>
    /// SqlFuncExternal 注册名（与 Median 方法名一致）
    /// </summary>
    public const string UniqueMethodName = nameof(Median);

    private static readonly FrozenDictionary<DbType, string> PercentileMedianSqlFormats = new Dictionary<DbType, string>
    {
        [DbType.SqlServer] = "PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY {0})",
        [DbType.PostgreSQL] = "percentile_cont(0.5) WITHIN GROUP (ORDER BY {0})",
        [DbType.Oracle] = "PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY {0})",
        [DbType.Kdbndp] = "percentile_cont(0.5) WITHIN GROUP (ORDER BY {0})",
        [DbType.Dm] = "PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY {0})",
    }.ToFrozenDictionary();

    /// <summary>
    /// Lambda 表达式占位：由 SqlSugar 翻译为库内中位数聚合 SQL
    /// </summary>
    /// <typeparam name="T">数值字段类型</typeparam>
    /// <param name="field">实体字段</param>
    /// <returns>不执行</returns>
    /// <exception cref="NotSupportedException">仅用于表达式树</exception>
    public static T Median<T>(T field) where T : struct =>
        throw new NotSupportedException("TaktSqlFuncMedian.Median 仅用于 SqlSugar Lambda 表达式翻译");

    /// <summary>
    /// 当前库是否支持 PERCENTILE_CONT 原生中位数 SQL
    /// </summary>
    /// <param name="dbType">SqlSugar 数据库类型</param>
    /// <returns>支持为 true</returns>
    public static bool SupportsNativePercentile(DbType dbType) =>
        PercentileMedianSqlFormats.ContainsKey(dbType);

    /// <summary>
    /// 创建中位数 SqlFuncExternal 定义
    /// </summary>
    /// <returns>SqlFunc 扩展项</returns>
    public static SqlFuncExternal CreateSqlFuncExternal() => new()
    {
        UniqueMethodName = UniqueMethodName,
        MethodValue = (expInfo, dbType, _) =>
        {
            ArgumentNullException.ThrowIfNull(expInfo.Args);
            if (expInfo.Args.Count == 0)
            {
                throw new ArgumentException("Median 需要字段参数", nameof(expInfo));
            }

            var columnSql = expInfo.Args[0].MemberName as string;
            ArgumentException.ThrowIfNullOrWhiteSpace(columnSql);
            if (!SupportsNativePercentile(dbType))
            {
                throw new NotSupportedException(
                    $"数据库类型 {dbType} 不支持 PERCENTILE_CONT 中位数 SqlFunc，请使用仓储 MedianAsync 有序切片回退。");
            }

            return BuildPercentileMedianSql(columnSql, dbType);
        },
    };

    /// <summary>
    /// 按库类型生成 PERCENTILE_CONT(0.5) 中位数 SQL 片段
    /// </summary>
    /// <param name="columnSql">已解析列 SQL（含表别名）</param>
    /// <param name="dbType">数据库类型</param>
    /// <returns>中位数 SQL 表达式</returns>
    /// <exception cref="NotSupportedException">不支持的数据库类型</exception>
    public static string BuildPercentileMedianSql(string columnSql, DbType dbType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(columnSql);
        if (!PercentileMedianSqlFormats.TryGetValue(dbType, out var format))
        {
            throw new NotSupportedException($"数据库类型 {dbType} 未实现 Median SqlFunc 翻译。");
        }

        return string.Format(format, columnSql);
    }
}
