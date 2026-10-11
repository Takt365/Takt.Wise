// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialZeroPriceMovingBackfillHelper.cs
// 创建时间：2026-08-17
// 创建人：Takt365(Cursor AI)
// 功能描述：零价格回填移动价：明细/主表 ExtField._bk.mp JSON 履历；主表另归档旧产品月成本（yyyy/M/d）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Text.Json.Nodes;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 零价格回填移动平均价（无 I/O；ExtField 与采购价回填同为 _bk JSON）
/// </summary>
public static class TaktBomMaterialZeroPriceMovingBackfillHelper
{
    /// <summary>
    /// ExtField 回填根键（与 TaktBomCalculatePurchaseInfoRecordHelper 一致）
    /// </summary>
    public const string ExtFieldBackfillRootKey = "_bk";

    /// <summary>
    /// 移动价回填在 _bk 下的子键（履历为 JSON 数组）
    /// </summary>
    public const string ExtFieldBackfillMpKey = "mp";

    /// <summary>
    /// ExtField nvarchar 上限（与公司基类一致）
    /// </summary>
    public const int ExtFieldMaxLength = 4000;

    /// <summary>
    /// 可读价格标识：yyyy-MM：价格：单位:币种（末段单位与币种之间为半角冒号，与业务约定一致）
    /// </summary>
    /// <param name="valuationPeriod">评估期间 yyyy-MM</param>
    /// <param name="movingPrice">移动价格原值</param>
    /// <param name="priceUnit">价格单位</param>
    /// <param name="currencyCode">币种</param>
    /// <returns>可读字符串</returns>
    public static string FormatMovingPriceInfo(
        string valuationPeriod,
        decimal movingPrice,
        int priceUnit,
        string currencyCode)
    {
        var period = (valuationPeriod ?? string.Empty).Trim();
        var currency = (currencyCode ?? string.Empty).Trim();
        var unit = priceUnit <= 0 ? 1 : priceUnit;
        return string.Concat(
            period,
            "：",
            movingPrice.ToString(CultureInfo.InvariantCulture),
            "：",
            unit.ToString(CultureInfo.InvariantCulture),
            ":",
            currency);
    }

    /// <summary>
    /// 回填移动平均价/单位/货币；默认仅 MovingAveragePrice=0 时写入；forceOverwrite 时同组件强制覆盖；并追加 ExtField._bk.mp 履历
    /// </summary>
    /// <param name="row">BOM 明细</param>
    /// <param name="sourceComponentCode">建议代替组件（逆推源）</param>
    /// <param name="valuationPeriod">源价评估期间 yyyy-MM</param>
    /// <param name="movingPrice">源移动价格原值</param>
    /// <param name="priceUnit">源价格单位</param>
    /// <param name="currencyCode">源币种</param>
    /// <param name="forceOverwrite">true 时忽略原价是否为零（手工更新）</param>
    /// <returns>写入则为 true</returns>
    public static bool ApplyMovingAveragePriceFields(
        TaktBomMaterialCostItem row,
        string sourceComponentCode,
        string valuationPeriod,
        decimal movingPrice,
        int priceUnit,
        string currencyCode,
        bool forceOverwrite = false)
    {
        ArgumentNullException.ThrowIfNull(row);
        if (movingPrice <= 0m)
        {
            return false;
        }
        if (!forceOverwrite && row.MovingAveragePrice != 0m)
        {
            return false;
        }
        var unit = priceUnit <= 0 ? 1 : priceUnit;
        var currency = (currencyCode ?? string.Empty).Trim();
        var source = (sourceComponentCode ?? string.Empty).Trim();
        var period = NormalizeValuationPeriod(valuationPeriod);
        var oldPrice = row.MovingAveragePrice;
        var oldUnit = row.MovingPriceUnit <= 0 ? 1 : row.MovingPriceUnit;
        var oldCurrency = row.MovingPriceCurrencyCode?.Trim() ?? string.Empty;
        var nextPrice = TaktBomMaterialCostItemLineCostHelper.RoundCost(movingPrice);
        // 价/单位/币种已一致则跳过写库与 ExtField（避免批量反复全表 Update 超时）
        if (oldPrice == nextPrice
            && oldUnit == unit
            && (string.IsNullOrWhiteSpace(currency)
                || string.Equals(oldCurrency, currency, StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }
        row.MovingAveragePrice = nextPrice;
        row.MovingPriceUnit = unit;
        if (!string.IsNullOrWhiteSpace(currency))
        {
            row.MovingPriceCurrencyCode = currency;
        }
        var priceInfo = FormatMovingPriceInfo(
            period,
            row.MovingAveragePrice,
            unit,
            row.MovingPriceCurrencyCode?.Trim() ?? string.Empty);
        var entry = new JsonObject
        {
            ["at"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            ["component_code"] = row.ComponentCode?.Trim() ?? string.Empty,
            ["source_component_code"] = source,
            ["valuation_period"] = period,
            ["old_moving_average_price"] = oldPrice,
            ["old_moving_price_unit"] = oldUnit,
            ["old_moving_price_currency_code"] = oldCurrency,
            ["moving_average_price"] = row.MovingAveragePrice,
            ["moving_price_unit"] = unit,
            ["moving_price_currency_code"] = row.MovingPriceCurrencyCode?.Trim() ?? string.Empty,
            ["price_info"] = priceInfo,
        };
        row.ExtField = AppendMpHistory(row.ExtField, entry);
        return true;
    }

    /// <summary>
    /// 主表回填履历：归档旧产品月成本（键=核算日 yyyy/M/d，与重算口径一致）并追加 ExtField._bk.mp
    /// </summary>
    /// <param name="header">成本主表行</param>
    /// <param name="componentCode">零价组件</param>
    /// <param name="sourceComponentCode">建议代替组件</param>
    /// <param name="valuationPeriod">源价期间</param>
    /// <param name="movingPrice">源移动价原值</param>
    /// <param name="priceUnit">源价格单位</param>
    /// <param name="currencyCode">源币种</param>
    /// <param name="oldProductMonthlyCost">回填前产品月成本</param>
    /// <param name="newProductMonthlyCost">回填后产品月成本</param>
    public static void ApplyHeaderProductCostMovingBackfillHistory(
        TaktBomMaterialCost header,
        string componentCode,
        string sourceComponentCode,
        string valuationPeriod,
        decimal movingPrice,
        int priceUnit,
        string currencyCode,
        decimal oldProductMonthlyCost,
        decimal newProductMonthlyCost)
    {
        ArgumentNullException.ThrowIfNull(header);
        var unit = priceUnit <= 0 ? 1 : priceUnit;
        var period = NormalizeValuationPeriod(valuationPeriod);
        var oldCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(oldProductMonthlyCost);
        var newCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(newProductMonthlyCost);
        var priceInfo = FormatMovingPriceInfo(period, movingPrice, unit, currencyCode);
        header.ExtField = ArchiveOldProductMonthlyCost(
            header.ExtField,
            header.CostingDate,
            oldCost);
        var entry = new JsonObject
        {
            ["at"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            ["scope"] = "product_monthly_cost",
            ["component_code"] = (componentCode ?? string.Empty).Trim(),
            ["source_component_code"] = (sourceComponentCode ?? string.Empty).Trim(),
            ["valuation_period"] = period,
            ["moving_average_price"] = TaktBomMaterialCostItemLineCostHelper.RoundCost(movingPrice),
            ["moving_price_unit"] = unit,
            ["moving_price_currency_code"] = (currencyCode ?? string.Empty).Trim(),
            ["price_info"] = priceInfo,
            ["old_product_monthly_cost"] = oldCost,
            ["new_product_monthly_cost"] = newCost,
            ["model_code"] = header.ModelCode?.Trim() ?? string.Empty,
            ["product_code"] = header.ProductCode?.Trim() ?? string.Empty,
        };
        header.ExtField = AppendMpHistory(header.ExtField, entry);
    }

    /// <summary>
    /// 主表机种月成本变更履历：追加 ExtField._bk.mp
    /// </summary>
    /// <param name="header">成本主表行</param>
    /// <param name="componentCode">零价组件</param>
    /// <param name="sourceComponentCode">建议代替组件</param>
    /// <param name="valuationPeriod">源价期间</param>
    /// <param name="movingPrice">源移动价原值</param>
    /// <param name="priceUnit">源价格单位</param>
    /// <param name="currencyCode">源币种</param>
    /// <param name="oldModelMonthlyAverageCost">回填前机种月成本</param>
    /// <param name="newModelMonthlyAverageCost">回填后机种月成本</param>
    public static void ApplyHeaderModelAverageMovingBackfillHistory(
        TaktBomMaterialCost header,
        string componentCode,
        string sourceComponentCode,
        string valuationPeriod,
        decimal movingPrice,
        int priceUnit,
        string currencyCode,
        decimal oldModelMonthlyAverageCost,
        decimal newModelMonthlyAverageCost)
    {
        ArgumentNullException.ThrowIfNull(header);
        var unit = priceUnit <= 0 ? 1 : priceUnit;
        var period = NormalizeValuationPeriod(valuationPeriod);
        var oldAvg = TaktBomMaterialCostItemLineCostHelper.RoundCost(oldModelMonthlyAverageCost);
        var newAvg = TaktBomMaterialCostItemLineCostHelper.RoundCost(newModelMonthlyAverageCost);
        var priceInfo = FormatMovingPriceInfo(period, movingPrice, unit, currencyCode);
        var entry = new JsonObject
        {
            ["at"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            ["scope"] = "model_monthly_average_cost",
            ["component_code"] = (componentCode ?? string.Empty).Trim(),
            ["source_component_code"] = (sourceComponentCode ?? string.Empty).Trim(),
            ["valuation_period"] = period,
            ["moving_average_price"] = TaktBomMaterialCostItemLineCostHelper.RoundCost(movingPrice),
            ["moving_price_unit"] = unit,
            ["moving_price_currency_code"] = (currencyCode ?? string.Empty).Trim(),
            ["price_info"] = priceInfo,
            ["old_model_monthly_average_cost"] = oldAvg,
            ["new_model_monthly_average_cost"] = newAvg,
            ["material_type"] = header.MaterialType?.Trim() ?? string.Empty,
            ["model_code"] = header.ModelCode?.Trim() ?? string.Empty,
            ["product_code"] = header.ProductCode?.Trim() ?? string.Empty,
        };
        header.ExtField = AppendMpHistory(header.ExtField, entry);
    }

    /// <summary>
    /// 追加旧产品月成本到 ExtField 根对象（键=yyyy/M/d，与重算归档一致；不删除 _bk）
    /// </summary>
    /// <param name="extField">当前扩展字段</param>
    /// <param name="oldCostingDate">旧核算日期</param>
    /// <param name="oldCost">旧产品月成本</param>
    /// <returns>序列化后的 JSON</returns>
    public static string ArchiveOldProductMonthlyCost(
        string? extField,
        DateTime oldCostingDate,
        decimal oldCost)
    {
        var obj = ParseExtFieldObject(extField);
        var previous = extField?.Trim() ?? string.Empty;
        var key = oldCostingDate.ToString("yyyy/M/d", CultureInfo.InvariantCulture);
        obj[key] = JsonValue.Create(
            decimal.Round(oldCost, TaktBomMaterialCostItemLineCostHelper.CostDecimalDigits, MidpointRounding.AwayFromZero));
        var json = obj.ToJsonString();
        while (json.Length > ExtFieldMaxLength)
        {
            var removable = obj
                .Select(p => p.Key)
                .Where(k => !string.Equals(k, ExtFieldBackfillRootKey, StringComparison.Ordinal))
                .OrderBy(ParseOldCostDateKey)
                .ToList();
            if (removable.Count <= 1)
            {
                break;
            }
            obj.Remove(removable[0]);
            json = obj.ToJsonString();
        }
        if (json.Length > ExtFieldMaxLength)
        {
            return previous;
        }
        return json;
    }

    /// <summary>
    /// 解析旧成本 JSON 键为日期；无法解析则排到最早（优先丢弃非法键）
    /// </summary>
    /// <param name="key">JSON 键</param>
    /// <returns>日期</returns>
    private static DateTime ParseOldCostDateKey(string key)
    {
        if (DateTime.TryParseExact(
                key,
                "yyyy/M/d",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsed))
        {
            return parsed;
        }
        return DateTime.MinValue;
    }

    /// <summary>
    /// 追加一条移动价回填履历到 ExtField._bk.mp（JSON 数组）；超长丢弃最早条目
    /// </summary>
    /// <param name="extField">原扩展字段</param>
    /// <param name="entry">本次履历</param>
    /// <returns>合并后 JSON；超长无法保留时尽量返回原值</returns>
    public static string? AppendMpHistory(string? extField, JsonObject entry)
    {
        return TaktBomExtFieldBackfillHistoryHelper.Append(
            extField,
            TaktBomExtFieldBackfillHistoryHelper.ScopeMp,
            entry);
    }

    /// <summary>
    /// 规范化评估期间为 yyyy-MM
    /// </summary>
    /// <param name="valuationPeriod">评估期间</param>
    /// <returns>yyyy-MM 或原串</returns>
    public static string NormalizeValuationPeriod(string? valuationPeriod)
    {
        var ym = (valuationPeriod ?? string.Empty).Trim().Replace('/', '-');
        if (ym.Length >= 7 && ym[4] == '-')
        {
            return ym[..7];
        }
        return ym;
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
            // 非法 JSON 视为空对象，避免阻断回填
        }
        return new JsonObject();
    }

}
