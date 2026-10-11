// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomExtFieldBackfillHistoryHelper.cs
// 创建时间：2026-08-19
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM ExtField._bk.{scope} 履历追加（JSON 数组；单对象自动迁入；超长丢弃最早；禁止覆盖整段）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Text.Json.Nodes;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 扩展字段回填履历（无 I/O；仅追加）
/// </summary>
public static class TaktBomExtFieldBackfillHistoryHelper
{
    /// <summary>
    /// ExtField 回填根键
    /// </summary>
    public const string ExtFieldBackfillRootKey = "_bk";

    /// <summary>
    /// 成本合计
    /// </summary>
    public const string ScopeSum = "sum";

    /// <summary>
    /// 成本重算
    /// </summary>
    public const string ScopeRecalc = "recalc";

    /// <summary>
    /// 机种月均
    /// </summary>
    public const string ScopeModelAvg = "model_avg";

    /// <summary>
    /// 采购价回填
    /// </summary>
    public const string ScopeBc = "bc";

    /// <summary>
    /// 移动价回填
    /// </summary>
    public const string ScopeMp = "mp";

    /// <summary>
    /// 最近采购成本
    /// </summary>
    public const string ScopeLatestPurchase = "lpc";

    /// <summary>
    /// PCB SECT 打标
    /// </summary>
    public const string ScopePcbSect = "pcb_sect";

    /// <summary>
    /// ExtField nvarchar 上限（与公司基类一致）
    /// </summary>
    public const int ExtFieldMaxLength = 4000;

    /// <summary>
    /// 追加一条履历到 ExtField._bk.{scope}（数组；若曾为单对象则先迁入数组）
    /// </summary>
    /// <param name="extField">原扩展字段</param>
    /// <param name="scope">作用域键（sum/recalc/model_avg/bc/mp/lpc/pcb_sect）</param>
    /// <param name="entry">本次履历（建议含 at）</param>
    /// <returns>合并后 JSON；超长无法保留时尽量返回原值</returns>
    public static string? Append(string? extField, string scope, JsonObject entry)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(scope);
        ArgumentNullException.ThrowIfNull(entry);
        var scopeKey = scope.Trim();
        var root = ParseExtFieldObject(extField);
        var previous = extField?.Trim();
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
        var json = root.ToJsonString();
        while (json.Length > ExtFieldMaxLength && hist.Count > 1)
        {
            hist.RemoveAt(0);
            json = root.ToJsonString();
        }
        if (json.Length <= ExtFieldMaxLength)
        {
            return json;
        }
        return string.IsNullOrWhiteSpace(previous) ? null : previous;
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
