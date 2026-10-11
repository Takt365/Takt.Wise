// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktQuartzSyncExecuteParamsHelper.cs
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：同步类 Quartz SQL 执行参数解析（sourceDatabase / targetDatabase）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 同步 SQL 任务 ExecuteParams 解析（源库/目标库；防注入校验库名）
/// </summary>
public static class TaktQuartzSyncExecuteParamsHelper
{
    /// <summary>
    /// 源库 JSON 键
    /// </summary>
    public const string SourceDatabaseKey = "sourceDatabase";

    /// <summary>
    /// 目标库 JSON 键
    /// </summary>
    public const string TargetDatabaseKey = "targetDatabase";

    private static readonly Regex DatabaseNamePattern = new(
        @"^[A-Za-z][A-Za-z0-9_]*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>
    /// 从执行参数解析源库/目标库名（JSON：sourceDatabase、targetDatabase）
    /// </summary>
    /// <param name="executeParams">任务执行参数</param>
    /// <returns>源库与目标库（未提供则为 null）</returns>
    public static (string? SourceDatabase, string? TargetDatabase) Parse(string? executeParams)
    {
        if (string.IsNullOrWhiteSpace(executeParams))
        {
            return (null, null);
        }
        var raw = executeParams.Trim();
        try
        {
            var token = JToken.Parse(raw);
            if (token is not JObject root)
            {
                return (null, null);
            }
            var source = TryGetStringProperty(root, SourceDatabaseKey, "SourceDatabase");
            var target = TryGetStringProperty(root, TargetDatabaseKey, "TargetDatabase");
            if (!string.IsNullOrWhiteSpace(source))
            {
                ValidateDatabaseName(source);
            }
            if (!string.IsNullOrWhiteSpace(target))
            {
                ValidateDatabaseName(target);
            }
            return (
                string.IsNullOrWhiteSpace(source) ? null : source.Trim(),
                string.IsNullOrWhiteSpace(target) ? null : target.Trim());
        }
        catch (JsonException)
        {
            return (null, null);
        }
    }

    /// <summary>
    /// 校验 SQL Server 数据库标识符（字母开头，仅字母数字下划线）
    /// </summary>
    /// <param name="databaseName">数据库名</param>
    /// <exception cref="ArgumentException">非法库名</exception>
    public static void ValidateDatabaseName(string databaseName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        if (!DatabaseNamePattern.IsMatch(databaseName.Trim()))
        {
            throw new ArgumentException($"非法数据库名: {databaseName}");
        }
    }

    /// <summary>
    /// 读取 JSON 对象字符串属性（尝试多个键名）
    /// </summary>
    /// <param name="root">JSON 根对象</param>
    /// <param name="keys">候选键</param>
    /// <returns>字符串或 null</returns>
    private static string? TryGetStringProperty(JObject root, params string[] keys)
    {
        foreach (var key in keys)
        {
            if (!root.TryGetValue(key, StringComparison.Ordinal, out var prop) || prop == null || prop.Type == JTokenType.Null)
            {
                continue;
            }
            if (prop.Type == JTokenType.String)
            {
                return prop.Value<string>();
            }
            if (prop.Type is JTokenType.Integer or JTokenType.Float or JTokenType.Boolean)
            {
                return prop.ToString();
            }
        }
        return null;
    }
}
