// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcExtFieldHistoryHelper.cs
// 创建时间：2026-10-30
// 创建人：Takt365(Cursor AI)
// 功能描述：设变 ExtField._bk.opd 旧品处理履历追加（JSON 数组；nvarchar(max)；纯函数无 I/O）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Text.Json.Nodes;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变扩展字段履历（无 I/O；仅追加）
/// </summary>
internal static class TaktEcExtFieldHistoryHelper
{
    /// <summary>
    /// ExtField 回填根键
    /// </summary>
    public const string ExtFieldBackfillRootKey = "_bk";

    /// <summary>
    /// 旧品处理履历作用域
    /// </summary>
    public const string ScopeOldPartDisposition = "opd";

    /// <summary>
    /// 单作用域最多保留履历条数（nvarchar(max)；超限丢弃最早）
    /// </summary>
    public const int MaxHistoryEntries = 200;

    /// <summary>
    /// 追加一条旧品处理履历到 ExtField._bk.opd（值未变则原样返回）
    /// </summary>
    /// <param name="extField">原扩展字段</param>
    /// <param name="fromValue">变更前（主表为整段文本；明细为单码）</param>
    /// <param name="toValue">变更后</param>
    /// <param name="userId">操作人 Id（可空）</param>
    /// <param name="userName">操作人用户名（可空）</param>
    /// <param name="materialCode">明细履历时的旧物料编码（主表履历可空）</param>
    /// <returns>合并后 JSON；未变更返回原值</returns>
    public static string? AppendOldPartDispositionHistory(
        string? extField,
        string? fromValue,
        string? toValue,
        long? userId,
        string? userName,
        string? materialCode = null)
    {
        var from = NormalizeHistoryValue(fromValue);
        var to = NormalizeHistoryValue(toValue);
        if (string.Equals(from, to, StringComparison.Ordinal))
        {
            return extField;
        }

        var entry = new JsonObject
        {
            ["at"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            ["from"] = from,
            ["to"] = to,
        };
        if (userId is > 0)
        {
            entry["uid"] = userId.Value.ToString(CultureInfo.InvariantCulture);
        }
        if (!string.IsNullOrWhiteSpace(userName))
        {
            entry["by"] = userName.Trim();
        }
        if (!string.IsNullOrWhiteSpace(materialCode))
        {
            entry["material"] = materialCode.Trim();
        }

        return Append(extField, ScopeOldPartDisposition, entry);
    }

    /// <summary>
    /// 追加一条履历到 ExtField._bk.{scope}
    /// </summary>
    /// <param name="extField">原扩展字段</param>
    /// <param name="scope">作用域键</param>
    /// <param name="entry">本次履历</param>
    /// <returns>合并后 JSON</returns>
    public static string? Append(string? extField, string scope, JsonObject entry)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(scope);
        ArgumentNullException.ThrowIfNull(entry);
        var scopeKey = scope.Trim();
        var root = ParseExtFieldObject(extField);
        if (root[ExtFieldBackfillRootKey] is not JsonObject bk)
        {
            bk = new JsonObject();
            root[ExtFieldBackfillRootKey] = bk;
        }
        var hist = ResolveHistoryArray(bk, scopeKey);
        if (!entry.ContainsKey("at"))
        {
            entry["at"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        }
        hist.Add(entry.DeepClone());
        while (hist.Count > MaxHistoryEntries)
        {
            hist.RemoveAt(0);
        }
        return root.ToJsonString();
    }

    /// <summary>
    /// 解析 ExtField 为 JSON 对象；空或非法返回空对象
    /// </summary>
    /// <param name="extField">扩展字段</param>
    /// <returns>JSON 对象</returns>
    public static JsonObject ParseExtFieldObject(string? extField)
    {
        if (string.IsNullOrWhiteSpace(extField))
        {
            return new JsonObject();
        }
        try
        {
            var node = JsonNode.Parse(extField);
            if (node is JsonObject obj)
            {
                return obj;
            }
        }
        catch (System.Text.Json.JsonException)
        {
            // 非法 JSON 视为空对象
        }
        return new JsonObject();
    }

    /// <summary>
    /// 履历值规范化（null/空白 → 空串）
    /// </summary>
    /// <param name="value">原值</param>
    /// <returns>规范化文本</returns>
    private static string NormalizeHistoryValue(string? value)
        => string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

    /// <summary>
    /// 取得或创建 _bk.{scope} 履历数组（单对象迁入数组）
    /// </summary>
    /// <param name="bk">_bk 对象</param>
    /// <param name="scopeKey">作用域</param>
    /// <returns>履历数组</returns>
    private static JsonArray ResolveHistoryArray(JsonObject bk, string scopeKey)
    {
        if (bk[scopeKey] is JsonArray arr)
        {
            return arr;
        }
        var hist = new JsonArray();
        if (bk[scopeKey] is JsonObject single)
        {
            hist.Add(single.DeepClone());
        }
        bk[scopeKey] = hist;
        return hist;
    }
}
