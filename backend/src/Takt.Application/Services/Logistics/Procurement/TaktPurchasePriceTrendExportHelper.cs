// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchasePriceTrendExportHelper.cs
// 创建时间：2026-09-16
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格/入库实绩推移 Excel：展开行+数值列+自动筛选；历史推移金额月汇总
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购价格推移导出单元格格式（与前端表格展示对齐）
/// </summary>
internal static class TaktPurchasePriceTrendExportHelper
{
    /// <summary>
    /// 单价列 Excel 列宽
    /// </summary>
    public const double UnitPriceColumnWidth = 14d;

    /// <summary>
    /// 有效起始日列 Excel 列宽
    /// </summary>
    public const double ValidFromColumnWidth = 14d;

    /// <summary>
    /// 历史推移「金额 / 月份」列 Excel 列宽
    /// </summary>
    public const double PointsColumnWidth = 56d;

    /// <summary>
    /// 推移表采购份额：1 万元 = 10,000 元
    /// </summary>
    public const decimal WanYuanScale = 10000m;

    /// <summary>
    /// 涨：与列表 text-red-500 对齐
    /// </summary>
    public const int TrendUpFontArgb = unchecked((int)0xFFEF4444);

    /// <summary>
    /// 跌：与列表 text-green-600 对齐
    /// </summary>
    public const int TrendDownFontArgb = unchecked((int)0xFF16A34A);

    /// <summary>
    /// 相对前一笔涨跌单元格（涨红跌绿）；首笔或无比较为空
    /// </summary>
    /// <param name="point">当前笔</param>
    /// <returns>着色文案或空串</returns>
    public static object FormatAdjacentCell(TaktPurchasePriceTrendPointDto? point)
    {
        if (point == null)
        {
            return string.Empty;
        }
        var text = FormatAdjacent(point);
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }
        var argb = ResolveTrendFontArgb(point.AdjacentTrend);
        if (!argb.HasValue)
        {
            return text;
        }
        return TaktExcelRichText.FromSingle(text, argb);
    }

    /// <summary>
    /// 只保留所选工厂的推移行（导出与列表共用）
    /// </summary>
    /// <param name="rows">推移行</param>
    /// <param name="plantCode">所选工厂</param>
    /// <returns>该工厂的行</returns>
    public static List<TaktPurchasePriceTrendDto> WhereSelectedPlant(
        IReadOnlyList<TaktPurchasePriceTrendDto> rows,
        string plantCode)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        var plant = plantCode.Trim();
        return rows
            .Where(r => string.Equals(r.PlantCode?.Trim(), plant, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// 展开为与导出相同的有效起始日行（一组多笔日期变成多行；列表分页总数与此一致）
    /// </summary>
    /// <param name="rows">物料×供应商组</param>
    /// <returns>每笔有效起始日一行</returns>
    public static List<TaktPurchasePriceTrendDto> ExpandToValidFromLines(
        IReadOnlyList<TaktPurchasePriceTrendDto> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        var lines = new List<TaktPurchasePriceTrendDto>();
        foreach (var row in rows)
        {
            var points = row.PeriodPoints;
            if (points == null || points.Count == 0)
            {
                lines.Add(row);
                continue;
            }
            foreach (var point in points)
            {
                lines.Add(CopyValidFromLine(row, point));
            }
        }
        return lines;
    }

    /// <summary>
    /// 复制一组上的单笔有效起始日（列表页与导出共用这一行）
    /// </summary>
    /// <param name="row">物料×供应商组</param>
    /// <param name="point">当前笔</param>
    /// <returns>仅含该笔的行</returns>
    private static TaktPurchasePriceTrendDto CopyValidFromLine(
        TaktPurchasePriceTrendDto row,
        TaktPurchasePriceTrendPointDto point)
    {
        return new TaktPurchasePriceTrendDto
        {
            PlantCode = row.PlantCode,
            PurchaseInfoCode = row.PurchaseInfoCode,
            MaterialCode = row.MaterialCode,
            MaterialName = row.MaterialName,
            MaterialDescription = row.MaterialDescription,
            SupplierCode = row.SupplierCode,
            SupplierName = row.SupplierName,
            Currency = row.Currency,
            CurrencyCode = row.CurrencyCode,
            Unit = row.Unit,
            Trend = row.Trend,
            BasePeriod = row.BasePeriod,
            ComparePeriod = row.ComparePeriod,
            VarianceAmount = row.VarianceAmount,
            VariancePercent = row.VariancePercent,
            PeriodPoints = new List<TaktPurchasePriceTrendPointDto> { point },
            PeriodUnitPrices = new Dictionary<string, decimal>(StringComparer.Ordinal)
            {
                [point.ValidFrom] = point.UnitPrice,
            },
            PeriodPriceSourcePeriods = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [point.ValidFrom] = point.ValidFrom,
            },
            PeriodAdjacents = new Dictionary<string, TaktPurchasePriceTrendAdjacentDto>(StringComparer.Ordinal),
        };
    }

    /// <summary>
    /// 采购价格推移明细导出行：每笔 ValidFrom 一行；单价 / 有效起始日 / 涨跌分列；组内续行物料维留空
    /// </summary>
    /// <param name="rows">物料推移行</param>
    /// <returns>展开后的导出行</returns>
    public static List<IReadOnlyDictionary<string, object?>> BuildPriceTrendDetailSheetRows(
        IReadOnlyList<TaktPurchasePriceTrendDto> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        var result = new List<IReadOnlyDictionary<string, object?>>();
        foreach (var row in rows)
        {
            var points = row.PeriodPoints ?? new List<TaktPurchasePriceTrendPointDto>();
            if (points.Count == 0)
            {
                result.Add(CreatePriceTrendDetailRow(row, point: null, isFirst: true));
                continue;
            }
            for (var i = 0; i < points.Count; i++)
            {
                result.Add(CreatePriceTrendDetailRow(row, points[i], isFirst: i == 0));
            }
        }
        return result;
    }

    /// <summary>
    /// 构建采购价格推移单行导出字典（物料维每行齐全；单价/涨跌%为数值，便于 Excel 筛选）
    /// </summary>
    /// <param name="row">物料推移行</param>
    /// <param name="point">当前笔；无点时为空</param>
    /// <param name="isFirst">是否组内首行（期间汇总仅首行填写时已改为每行齐全）</param>
    /// <returns>导出行</returns>
    private static Dictionary<string, object?> CreatePriceTrendDetailRow(
        TaktPurchasePriceTrendDto row,
        TaktPurchasePriceTrendPointDto? point,
        bool isFirst)
    {
        ArgumentNullException.ThrowIfNull(row);
        _ = isFirst;
        return new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["plantCode"] = row.PlantCode,
            ["purchaseInfoCode"] = row.PurchaseInfoCode,
            ["materialCode"] = row.MaterialCode,
            ["materialDescription"] = row.MaterialDescription,
            ["supplierCode"] = row.SupplierCode,
            ["supplierName"] = row.SupplierName,
            ["currencyCode"] = row.CurrencyCode,
            ["unit"] = row.Unit,
            ["unitPrice"] = point?.UnitPrice,
            ["validFrom"] = TryParseExportDate(point?.ValidFrom),
            ["adjacentPercent"] = ResolvePriceTrendAdjacentPercent(point),
            ["trend"] = FormatTrend(row.Trend),
            ["varianceAmount"] = row.VarianceAmount,
            ["variancePercent"] = row.VariancePercent is 0m ? null : row.VariancePercent,
        };
    }

    /// <summary>
    /// 相对前一笔变动率（小数比率；无可比则为空）
    /// </summary>
    /// <param name="point">当前笔</param>
    /// <returns>变动率；无可比则为空</returns>
    private static decimal? ResolvePriceTrendAdjacentPercent(TaktPurchasePriceTrendPointDto? point)
    {
        if (point == null || string.IsNullOrWhiteSpace(point.AdjacentBasePeriod))
        {
            return null;
        }
        if (!point.AdjacentVariancePercent.HasValue || point.AdjacentVariancePercent.Value == 0m)
        {
            return null;
        }
        return point.AdjacentVariancePercent;
    }

    /// <summary>
    /// 期间涨跌文案（与列表 trend.* 中文一致）
    /// </summary>
    /// <param name="trend">none / up / down / flat</param>
    /// <returns>展示文案</returns>
    public static string FormatTrend(string? trend)
    {
        return trend switch
        {
            "up" => "涨",
            "down" => "跌",
            "flat" => "平",
            _ => "—",
        };
    }

    /// <summary>
    /// 期间变动率（小数比率 → 百分比；0% 返回空串，仅该格不写 0.00%，不省略导出行）
    /// </summary>
    /// <param name="ratio">小数比率</param>
    /// <returns>百分比文本；0 或空则为空串</returns>
    public static string FormatPercent(decimal? ratio)
    {
        if (!ratio.HasValue || ratio.Value == 0m)
        {
            return string.Empty;
        }
        return string.Create(CultureInfo.InvariantCulture, $"{ratio.Value * 100m:0.00}%");
    }

    /// <summary>
    /// 期间变动率（涨红跌绿）
    /// </summary>
    /// <param name="ratio">小数比率</param>
    /// <param name="trend">none / up / down / flat</param>
    /// <returns>着色百分比；无趋势色则为纯文本</returns>
    public static object FormatPercentCell(decimal? ratio, string? trend)
    {
        var text = FormatPercent(ratio);
        var argb = ResolveTrendFontArgb(trend);
        if (!argb.HasValue || string.IsNullOrEmpty(text))
        {
            return text;
        }
        return TaktExcelRichText.FromSingle(text, argb);
    }

    /// <summary>
    /// 涨跌字体色（涨红跌绿）；平/无比较为空
    /// </summary>
    /// <param name="trend">none / up / down / flat</param>
    /// <returns>ARGB；无需着色则为空</returns>
    public static int? ResolveTrendFontArgb(string? trend)
    {
        return trend switch
        {
            "up" => TrendUpFontArgb,
            "down" => TrendDownFontArgb,
            _ => null,
        };
    }

    /// <summary>
    /// 单价五位小数（与列表 formatCost 一致）
    /// </summary>
    /// <param name="unitPrice">折算单价</param>
    /// <returns>文本</returns>
    public static string FormatUnitPrice(decimal unitPrice)
    {
        return unitPrice.ToString("0.00000", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 有效起始日 yyyy-MM-dd → yyyy/M/d；采购订单价格历史推移年月 yyyy-MM → yyyy/M
    /// </summary>
    /// <param name="validFrom">yyyy-MM-dd 或 yyyy-MM</param>
    /// <returns>展示日期</returns>
    public static string FormatValidFrom(string? validFrom)
    {
        if (string.IsNullOrWhiteSpace(validFrom))
        {
            return "—";
        }
        var parts = validFrom.Trim().Split('-');
        if (parts.Length >= 2
            && int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var month))
        {
            if (parts.Length == 2)
            {
                return string.Create(CultureInfo.InvariantCulture, $"{parts[0]}/{month}");
            }
            if (parts.Length == 3
                && int.TryParse(parts[2], NumberStyles.None, CultureInfo.InvariantCulture, out var day))
            {
                return string.Create(CultureInfo.InvariantCulture, $"{parts[0]}/{month}/{day}");
            }
        }
        return validFrom.Trim();
    }

    /// <summary>
    /// 导出用：将 yyyy-MM-dd 解析为 DateTime（日期类型，供 Excel 字典导出套日期格式）
    /// </summary>
    /// <param name="isoDate">yyyy-MM-dd；空则返回 null</param>
    /// <returns>日期；无法解析则为 null</returns>
    private static DateTime? TryParseExportDate(string? isoDate)
    {
        if (string.IsNullOrWhiteSpace(isoDate))
        {
            return null;
        }
        if (DateTime.TryParseExact(
                isoDate.Trim(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
        {
            return date.Date;
        }
        if (DateTime.TryParse(isoDate.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var fallback))
        {
            return fallback.Date;
        }
        return null;
    }

    /// <summary>
    /// 相对前一笔涨跌（第 1 笔无）
    /// </summary>
    /// <param name="point">当前笔</param>
    /// <returns>↑/↓/→ 文案；无比较为空</returns>
    public static string FormatAdjacent(TaktPurchasePriceTrendPointDto point)
    {
        ArgumentNullException.ThrowIfNull(point);
        if (string.IsNullOrWhiteSpace(point.AdjacentBasePeriod))
        {
            return string.Empty;
        }
        var arrow = point.AdjacentTrend switch
        {
            "up" => "↑",
            "down" => "↓",
            _ => "→",
        };
        if (point.AdjacentVariancePercent.HasValue)
        {
            if (point.AdjacentVariancePercent.Value == 0m)
            {
                return string.Empty;
            }
            return $"{arrow} {FormatPercent(point.AdjacentVariancePercent)}";
        }
        if ((point.AdjacentVarianceAmount ?? 0m) == 0m)
        {
            return string.Empty;
        }
        return $"{arrow} {FormatUnitPrice(point.AdjacentVarianceAmount ?? 0m)}";
    }

    /// <summary>
    /// 采购订单价格历史推移月列表头（同年 4月；跨年 2026年4月）
    /// </summary>
    /// <param name="yearMonth">yyyy-MM</param>
    /// <param name="allYearMonths">期间全部年月</param>
    /// <returns>表头</returns>
    public static string FormatHistoryMonthHeader(string yearMonth, IReadOnlyList<string> allYearMonths)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(yearMonth);
        ArgumentNullException.ThrowIfNull(allYearMonths);
        var parts = yearMonth.Trim().Split('-');
        if (parts.Length < 2
            || !int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var month))
        {
            return yearMonth.Trim();
        }
        var years = allYearMonths
            .Select(k => k.Trim().Split('-').FirstOrDefault() ?? string.Empty)
            .Where(y => y.Length > 0)
            .Distinct(StringComparer.Ordinal)
            .ToList();
        if (years.Count > 1)
        {
            return $"{parts[0]}年{month}月";
        }
        return $"{month}月";
    }

    /// <summary>
    /// 金额展示（千分位、3 位小数；明细 Tab 用元）
    /// </summary>
    /// <param name="amount">金额（元）</param>
    /// <returns>文本</returns>
    public static string FormatHistoryAmount(decimal amount)
    {
        return amount.ToString("#,##0.000", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 单价展示（5 位小数）
    /// </summary>
    /// <param name="unitPrice">单价</param>
    /// <returns>文本</returns>
    public static string FormatHistoryUnitPrice(decimal unitPrice)
    {
        return unitPrice.ToString("#,##0.00000", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 数量展示（最多 5 位小数，去尾零）
    /// </summary>
    /// <param name="quantity">数量</param>
    /// <returns>文本</returns>
    public static string FormatHistoryQuantity(decimal quantity)
    {
        return quantity.ToString("0.#####", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 采购份额（万元、5 位小数数值；供 Excel 数值列）
    /// </summary>
    /// <param name="amountYuan">金额（元）</param>
    /// <returns>万元数值</returns>
    public static decimal ToHistoryShareWanYuanValue(decimal amountYuan)
    {
        return Math.Round(amountYuan / WanYuanScale, 5, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// 采购份额展示（万元、5 位小数；仅富文本/文案）
    /// </summary>
    /// <param name="amountYuan">金额（元）</param>
    /// <returns>万元文本</returns>
    public static string FormatHistoryShareWanYuan(decimal amountYuan)
    {
        return ToHistoryShareWanYuanValue(amountYuan).ToString("#,##0.00000", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 相比上月涨跌金额（万元、5 位小数；首月无对比则为空）
    /// </summary>
    /// <param name="upAmount">涨价合计（元）</param>
    /// <param name="downAmount">跌价合计（元）</param>
    /// <param name="hasPreviousMonth">是否有上一月可比</param>
    /// <returns>如 0.40000 涨, 0.35000 跌</returns>
    public static string FormatHistoryImpactText(decimal upAmount, decimal downAmount, bool hasPreviousMonth)
    {
        if (!hasPreviousMonth)
        {
            return string.Empty;
        }
        return $"{FormatHistoryShareWanYuan(upAmount)} 涨, {FormatHistoryShareWanYuan(downAmount)} 跌";
    }

    /// <summary>
    /// 相比上月涨跌金额单元格（涨红跌绿富文本；首月无对比则为空）
    /// </summary>
    /// <param name="upAmount">涨价合计（元）</param>
    /// <param name="downAmount">跌价合计（元）</param>
    /// <param name="hasPreviousMonth">是否有上一月可比</param>
    /// <returns>着色文案或空串</returns>
    public static object FormatHistoryImpactCell(decimal upAmount, decimal downAmount, bool hasPreviousMonth)
    {
        if (!hasPreviousMonth)
        {
            return string.Empty;
        }
        return new TaktExcelRichText
        {
            Runs = new[]
            {
                new TaktExcelRichRun
                {
                    Text = $"{FormatHistoryShareWanYuan(upAmount)} 涨",
                    FontArgb = TrendUpFontArgb,
                },
                new TaktExcelRichRun { Text = ", " },
                new TaktExcelRichRun
                {
                    Text = $"{FormatHistoryShareWanYuan(downAmount)} 跌",
                    FontArgb = TrendDownFontArgb,
                },
            },
        };
    }

    /// <summary>
    /// 采购订单价格历史推移表导出行（月份 / 采购份额万元 / 涨跌金额万元；末行合计）
    /// </summary>
    /// <param name="summaries">各月汇总</param>
    /// <param name="totalAmount">期间采购份额合计（元，导出走万元）</param>
    /// <param name="totalUpAmount">期间涨价合计</param>
    /// <param name="totalDownAmount">期间跌价合计</param>
    /// <returns>推移表行</returns>
    public static List<IReadOnlyDictionary<string, object?>> BuildHistoryTrendSheetRows(
        IReadOnlyList<TaktPurchasePriceTrendMonthSummaryDto> summaries,
        decimal totalAmount,
        decimal totalUpAmount,
        decimal totalDownAmount)
    {
        ArgumentNullException.ThrowIfNull(summaries);
        var rows = new List<IReadOnlyDictionary<string, object?>>(checked(summaries.Count + 1));
        var monthKeys = summaries.Select(s => s.Period).ToList();
        for (var i = 0; i < summaries.Count; i++)
        {
            var summary = summaries[i];
            rows.Add(new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["month"] = FormatHistoryMonthHeader(summary.Period, monthKeys),
                ["amount"] = ToHistoryShareWanYuanValue(summary.Amount),
                ["impact"] = FormatHistoryImpactCell(summary.UpAmount, summary.DownAmount, i > 0),
            });
        }
        rows.Add(new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["month"] = "合计",
            ["amount"] = ToHistoryShareWanYuanValue(totalAmount),
            ["impact"] = FormatHistoryImpactCell(totalUpAmount, totalDownAmount, summaries.Count > 1),
        });
        return rows;
    }

    /// <summary>
    /// 采购订单价格历史推移明细：每月一个合计金额 / 月份，相邻月比较涨跌（红/绿）
    /// </summary>
    /// <param name="points">该行 PeriodPoints</param>
    /// <returns>富文本；无点为空串</returns>
    public static object FormatHistoryAmountPointsCell(IReadOnlyList<TaktPurchasePriceTrendPointDto>? points)
    {
        var own = (points ?? new List<TaktPurchasePriceTrendPointDto>())
            .Where(p => p.HasPurchase)
            .ToList();
        if (own.Count == 0)
        {
            return string.Empty;
        }
        var runs = new List<TaktExcelRichRun>(checked(own.Count * 3));
        TaktPurchasePriceTrendPointDto? previous = null;
        for (var i = 0; i < own.Count; i++)
        {
            if (i > 0)
            {
                runs.Add(new TaktExcelRichRun { Text = "\n" });
            }
            var current = own[i];
            runs.Add(new TaktExcelRichRun
            {
                Text = $"{FormatHistoryAmount(current.Amount)} / {FormatValidFrom(current.ValidFrom)}"
                    + $"\n单价 {FormatHistoryUnitPrice(current.UnitPrice)}  数量 {FormatHistoryQuantity(current.Quantity)}",
            });
            var adjacent = FormatHistoryAmountAdjacent(current, previous);
            if (!string.IsNullOrEmpty(adjacent))
            {
                var trend = string.IsNullOrWhiteSpace(current.AdjacentTrend)
                    ? "flat"
                    : current.AdjacentTrend;
                runs.Add(new TaktExcelRichRun
                {
                    Text = " " + adjacent,
                    FontArgb = ResolveTrendFontArgb(trend),
                });
            }
            previous = current;
        }
        return new TaktExcelRichText { Runs = runs };
    }

    /// <summary>
    /// 相邻月因单价变动造成的采购金额浮动（第 1 个月无；= (本月单价−上月单价)×本月数量；只记金额，不记%）
    /// </summary>
    /// <param name="current">本月</param>
    /// <param name="previous">上一个有采购的月</param>
    /// <returns>↑/↓/→ 金额；无比较为空</returns>
    public static string FormatHistoryAmountAdjacent(
        TaktPurchasePriceTrendPointDto current,
        TaktPurchasePriceTrendPointDto? previous)
    {
        ArgumentNullException.ThrowIfNull(current);
        if (previous == null || string.IsNullOrWhiteSpace(current.AdjacentBasePeriod))
        {
            return string.Empty;
        }
        var impact = current.AdjacentVarianceAmount
            ?? RoundHistoryImpact((current.UnitPrice - previous.UnitPrice) * current.Quantity);
        var arrow = impact > 0m ? "↑" : impact < 0m ? "↓" : "→";
        var abs = impact < 0m ? -impact : impact;
        return $"{arrow} {FormatHistoryAmount(abs)}";
    }

    /// <summary>
    /// 单价影响金额取整（与构建器一致，3 位）
    /// </summary>
    /// <param name="value">影响金额</param>
    /// <returns>取整后金额</returns>
    private static decimal RoundHistoryImpact(decimal value) =>
        Math.Round(value, 3, MidpointRounding.AwayFromZero);

    /// <summary>
    /// 采购/入库历史推移明细导出行（金额 / 月份挤在一格；兼容旧导出）
    /// </summary>
    /// <param name="rows">物料推移行</param>
    /// <returns>明细行</returns>
    public static List<IReadOnlyDictionary<string, object?>> BuildHistoryDetailSheetRows(
        IReadOnlyList<TaktPurchasePriceTrendDto> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        var result = new List<IReadOnlyDictionary<string, object?>>(rows.Count);
        foreach (var row in rows)
        {
            result.Add(new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["materialCode"] = row.MaterialCode,
                ["materialDescription"] = row.MaterialDescription,
                ["supplierCode"] = row.SupplierCode,
                ["supplierName"] = row.SupplierName,
                ["currencyCode"] = row.CurrencyCode,
                ["unit"] = row.Unit,
                ["points"] = FormatHistoryAmountPointsCell(row.PeriodPoints),
                ["trend"] = FormatTrend(row.Trend),
                ["varianceAmount"] = row.VarianceAmount,
            });
        }
        return result;
    }

    /// <summary>
    /// 历史推移明细导出行（展开）：每月一行；物料维每行齐全；金额/单价/数量/涨跌为数值
    /// </summary>
    /// <param name="rows">物料推移行</param>
    /// <returns>展开后的明细行</returns>
    public static List<IReadOnlyDictionary<string, object?>> BuildHistoryDetailExpandedSheetRows(
        IReadOnlyList<TaktPurchasePriceTrendDto> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        var result = new List<IReadOnlyDictionary<string, object?>>();
        foreach (var row in rows)
        {
            var own = (row.PeriodPoints ?? new List<TaktPurchasePriceTrendPointDto>())
                .Where(p => p.HasPurchase)
                .ToList();
            if (own.Count == 0)
            {
                result.Add(CreateHistoryDetailExpandedRow(row, point: null, previous: null));
                continue;
            }
            TaktPurchasePriceTrendPointDto? previous = null;
            for (var i = 0; i < own.Count; i++)
            {
                var current = own[i];
                result.Add(CreateHistoryDetailExpandedRow(row, current, previous));
                previous = current;
            }
        }
        return result;
    }

    /// <summary>
    /// 构建历史推移展开单行（物料维每行齐全；金额/单价/数量/涨跌为数值，便于 Excel 筛选）
    /// </summary>
    /// <param name="row">物料推移行</param>
    /// <param name="point">当月点；无则空</param>
    /// <param name="previous">上一有业务月</param>
    /// <returns>导出行</returns>
    private static Dictionary<string, object?> CreateHistoryDetailExpandedRow(
        TaktPurchasePriceTrendDto row,
        TaktPurchasePriceTrendPointDto? point,
        TaktPurchasePriceTrendPointDto? previous)
    {
        ArgumentNullException.ThrowIfNull(row);
        return new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["plantCode"] = row.PlantCode,
            ["materialCode"] = row.MaterialCode,
            ["materialDescription"] = row.MaterialDescription,
            ["supplierCode"] = row.SupplierCode,
            ["supplierName"] = row.SupplierName,
            // 出库推移复用 Supplier* 存客户；导出键兼容 customer*
            ["customerCode"] = row.SupplierCode,
            ["customerName"] = row.SupplierName,
            ["currencyCode"] = row.CurrencyCode,
            ["unit"] = row.Unit,
            ["amount"] = point?.Amount,
            ["unitPrice"] = point?.UnitPrice,
            ["quantity"] = point?.Quantity,
            ["month"] = point?.ValidFrom,
            ["adjacent"] = ResolveHistoryAdjacentPercent(point, previous),
            ["trend"] = FormatTrend(row.Trend),
            ["varianceAmount"] = row.VarianceAmount,
        };
    }

    /// <summary>
    /// 相邻月单价涨跌率（小数比率：正=涨、负=跌；首月或无比较为空）
    /// </summary>
    /// <param name="current">本月</param>
    /// <param name="previous">上一有业务月</param>
    /// <returns>涨跌率；无可比则为空</returns>
    public static decimal? ResolveHistoryAdjacentPercent(
        TaktPurchasePriceTrendPointDto? current,
        TaktPurchasePriceTrendPointDto? previous)
    {
        if (current == null || previous == null || string.IsNullOrWhiteSpace(current.AdjacentBasePeriod))
        {
            return null;
        }
        if (current.AdjacentVariancePercent.HasValue)
        {
            return current.AdjacentVariancePercent.Value == 0m
                ? null
                : current.AdjacentVariancePercent;
        }
        if (previous.UnitPrice == 0m)
        {
            return null;
        }
        var ratio = Math.Round(
            (current.UnitPrice - previous.UnitPrice) / previous.UnitPrice,
            4,
            MidpointRounding.AwayFromZero);
        return ratio == 0m ? null : ratio;
    }

    /// <summary>
    /// 相邻月单价影响金额（有符号数值：正=涨、负=跌；首月或无比较为空）
    /// </summary>
    /// <param name="current">本月</param>
    /// <param name="previous">上一有业务月</param>
    /// <returns>影响金额；无可比则为空</returns>
    public static decimal? ResolveHistoryImpactAmount(
        TaktPurchasePriceTrendPointDto? current,
        TaktPurchasePriceTrendPointDto? previous)
    {
        if (current == null || previous == null || string.IsNullOrWhiteSpace(current.AdjacentBasePeriod))
        {
            return null;
        }
        return current.AdjacentVarianceAmount
            ?? RoundHistoryImpact((current.UnitPrice - previous.UnitPrice) * current.Quantity);
    }

    /// <summary>
    /// 相邻月涨跌单元格（涨红跌绿文案；兼容旧调用）；首月或无比较为空
    /// </summary>
    /// <param name="current">本月</param>
    /// <param name="previous">上一有业务月</param>
    /// <returns>着色文案或空串</returns>
    public static object FormatHistoryAdjacentCell(
        TaktPurchasePriceTrendPointDto? current,
        TaktPurchasePriceTrendPointDto? previous)
    {
        if (current == null)
        {
            return string.Empty;
        }
        var text = FormatHistoryAmountAdjacent(current, previous);
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }
        var trend = string.IsNullOrWhiteSpace(current.AdjacentTrend)
            ? (previous == null ? "none" : InferHistoryAdjacentTrend(current, previous))
            : current.AdjacentTrend;
        var argb = ResolveTrendFontArgb(trend);
        if (!argb.HasValue)
        {
            return text;
        }
        return TaktExcelRichText.FromSingle(text, argb);
    }

    /// <summary>
    /// 由单价影响金额推导相邻涨跌码
    /// </summary>
    /// <param name="current">本月</param>
    /// <param name="previous">上月</param>
    /// <returns>up / down / flat</returns>
    private static string InferHistoryAdjacentTrend(
        TaktPurchasePriceTrendPointDto current,
        TaktPurchasePriceTrendPointDto previous)
    {
        var impact = ResolveHistoryImpactAmount(current, previous) ?? 0m;
        if (impact > 0m)
        {
            return "up";
        }
        if (impact < 0m)
        {
            return "down";
        }
        return "flat";
    }
}
