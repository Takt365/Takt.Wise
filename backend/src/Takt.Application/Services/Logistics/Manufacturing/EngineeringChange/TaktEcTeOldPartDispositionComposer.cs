// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcTeOldPartDispositionComposer.cs
// 创建时间：2026-10-30
// 创建人：Takt365(Cursor AI)
// 功能描述：技术课主表旧品处理文本「旧物料:处理方式;」由明细聚合（纯函数，无 I/O）
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 技术课主表 EcOldPartDisposition 聚合（旧物料:处理方式;）
/// </summary>
internal static class TaktEcTeOldPartDispositionComposer
{
    /// <summary>
    /// 由明细旧物料编码 + 旧品处理码聚合为主表 ntext；同旧物料取首个有效处理方式；无有效段返回 null。
    /// </summary>
    /// <param name="rows">明细行（物料编码、处理方式字典值）</param>
    /// <returns>「旧物料:处理方式;…」或 null</returns>
    public static string? ComposeFromDetailRows(IEnumerable<(string? MaterialCode, string? Disposition)> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        var parts = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (materialCode, disposition) in rows)
        {
            var material = materialCode?.Trim() ?? string.Empty;
            var mode = disposition?.Trim() ?? string.Empty;
            if (!TaktEcScopeConstants.HasEffectiveMaterialCode(material) || string.IsNullOrEmpty(mode))
            {
                continue;
            }

            var segment = $"{material}:{mode}";
            if (!TaktRegexHelper.IsMatch(TaktRegexHelper.EcTeOldPartDispositionPart, segment))
            {
                continue;
            }

            if (!seen.Add(material))
            {
                continue;
            }

            parts.Add(segment);
        }

        if (parts.Count == 0)
        {
            return null;
        }

        return string.Join(";", parts) + ";";
    }

    /// <summary>
    /// 解析主表旧品处理文本为「旧物料 → 处理方式」映射（非法段跳过；同物料后者覆盖前者）。
    /// </summary>
    /// <param name="text">「旧物料:处理方式;…」</param>
    /// <returns>映射；空文本返回空字典</returns>
    public static Dictionary<string, string> ParseToMap(string? text)
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(text))
        {
            return map;
        }

        foreach (var raw in text.Split([';', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (!TaktRegexHelper.IsMatch(TaktRegexHelper.EcTeOldPartDispositionPart, raw))
            {
                continue;
            }

            var sep = raw.LastIndexOf(':');
            if (sep <= 0 || sep >= raw.Length - 1)
            {
                continue;
            }

            var material = raw[..sep].Trim();
            if (!TaktEcScopeConstants.HasEffectiveMaterialCode(material))
            {
                continue;
            }

            map[material] = raw[(sep + 1)..].Trim();
        }

        return map;
    }
}
