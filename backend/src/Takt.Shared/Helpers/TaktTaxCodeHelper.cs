// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktTaxCodeHelper.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：税码 accounting_financial_tax_code → 税率百分比整数（与字典 ExtValue / 标签百分比对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Text.RegularExpressions;

namespace Takt.Shared.Helpers;

/// <summary>
/// 税码与税率换算（纯静态；与字典 accounting_financial_tax_code 的 ExtValue=税率百分比 对齐）
/// </summary>
public static class TaktTaxCodeHelper
{
    /// <summary>
    /// 内置税码→税率%（与种子 accounting_financial_tax_code 一致；17.5% 等非整按四舍五入为 int）
    /// </summary>
    private static readonly Dictionary<string, int> BuiltInTaxCodeRates =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["J0"] = 0,
            ["J1"] = 17,
            ["J2"] = 13,
            ["J3"] = 11,
            ["J4"] = 6,
            ["J5"] = 3,
            ["J6"] = 16,
            ["J7"] = 10,
            ["J8"] = 1,
            ["L1"] = 5,
            ["X0"] = 0,
            ["X1"] = 17,
            ["X2"] = 13,
            ["X3"] = 16,
            ["A0"] = 0,
            ["A1"] = 0,
            ["A2"] = 0,
            ["A5"] = 5,
            ["A8"] = 8,
            ["AA"] = 25,
            ["AB"] = 19,
            ["AC"] = 19,
            ["AD"] = 16,
            ["AJ"] = 10,
            ["AZ"] = 18,
            ["E0"] = 0,
            ["I8"] = 8,
            ["IJ"] = 10,
            ["N8"] = 8,
            ["V0"] = 0,
            ["V1"] = 0,
            ["V2"] = 0,
            ["V3"] = 3,
            ["V4"] = 5,
            ["V5"] = 5,
            ["V8"] = 8,
            ["VA"] = 25,
            ["VB"] = 19,
            ["VC"] = 19,
            ["VD"] = 16,
            ["VH"] = 8,
            ["VJ"] = 10,
            ["VL"] = 8,
            ["VM"] = 10,
            ["VZ"] = 18,
        };

    /// <summary>
    /// 有税码时用税码覆盖税率；无税码或无法识别时保留 currentTaxRate
    /// </summary>
    /// <param name="taxCode">税码</param>
    /// <param name="currentTaxRate">当前税率</param>
    /// <returns>应用后的税率百分比</returns>
    public static int ApplyTaxRateFromTaxCode(string? taxCode, int currentTaxRate)
        => TryResolveTaxRatePercent(taxCode) ?? currentTaxRate;

    private static readonly Regex PercentInLabel = new(
        @"(\d+(?:\.\d+)?)\s*%",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);

    /// <summary>
    /// 由税码解析税率百分比整数（如 J2→13）。无法识别返回 null。
    /// </summary>
    /// <param name="taxCode">税码 DictValue</param>
    /// <returns>税率百分比；无法识别为 null</returns>
    public static int? TryResolveTaxRatePercent(string? taxCode)
    {
        if (string.IsNullOrWhiteSpace(taxCode))
        {
            return null;
        }
        var code = taxCode.Trim();
        if (BuiltInTaxCodeRates.TryGetValue(code, out var rate))
        {
            return rate;
        }
        return null;
    }

    /// <summary>
    /// 由税码解析税率；无法识别时用 defaultRate（默认 13）
    /// </summary>
    /// <param name="taxCode">税码</param>
    /// <param name="defaultRate">默认税率百分比</param>
    /// <returns>税率百分比</returns>
    public static int ResolveTaxRatePercent(string? taxCode, int defaultRate = 13)
        => TryResolveTaxRatePercent(taxCode) ?? defaultRate;

    /// <summary>
    /// 优先用字典 ExtValue，其次内置税码表，再次从 DictLabel 解析「13%」
    /// </summary>
    /// <param name="taxCode">税码</param>
    /// <param name="extValue">字典 ExtValue（税率百分比文本）</param>
    /// <param name="dictLabel">字典标签（可含百分比）</param>
    /// <returns>税率百分比；无法识别为 null</returns>
    public static int? TryResolveTaxRatePercent(string? taxCode, string? extValue, string? dictLabel)
    {
        if (TryParsePercentText(extValue, out var fromExt))
        {
            return fromExt;
        }
        var fromCode = TryResolveTaxRatePercent(taxCode);
        if (fromCode.HasValue)
        {
            return fromCode;
        }
        if (TryParsePercentFromLabel(dictLabel, out var fromLabel))
        {
            return fromLabel;
        }
        return null;
    }

    /// <summary>
    /// 解析纯数字或带 % 的税率文本为 int（小数四舍五入）
    /// </summary>
    /// <param name="text">如 13、13%、17.5</param>
    /// <param name="rate">解析结果</param>
    /// <returns>是否成功</returns>
    public static bool TryParsePercentText(string? text, out int rate)
    {
        rate = 0;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }
        var trimmed = text.Trim().TrimEnd('%').Trim();
        if (!decimal.TryParse(trimmed, NumberStyles.Number, CultureInfo.InvariantCulture, out var value)
            && !decimal.TryParse(trimmed, NumberStyles.Number, CultureInfo.CurrentCulture, out value))
        {
            return false;
        }
        if (value < 0)
        {
            return false;
        }
        rate = (int)Math.Round(value, MidpointRounding.AwayFromZero);
        return true;
    }

    /// <summary>
    /// 从字典标签中提取首个百分比（如「13% 进项税，中国」→13）
    /// </summary>
    /// <param name="dictLabel">字典标签</param>
    /// <param name="rate">税率</param>
    /// <returns>是否成功</returns>
    public static bool TryParsePercentFromLabel(string? dictLabel, out int rate)
    {
        rate = 0;
        if (string.IsNullOrWhiteSpace(dictLabel))
        {
            return false;
        }
        var match = PercentInLabel.Match(dictLabel);
        if (!match.Success)
        {
            return false;
        }
        return TryParsePercentText(match.Groups[1].Value, out rate);
    }
}
