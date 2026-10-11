// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomCalculatePurchaseInfoRecordHelper.cs
// 创建时间：2026-08-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 明细采购价回填：ValidFrom≤核算日最近主表；仅空字段写入；快照合并 ExtField._bk
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Text.Json.Nodes;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Procurement;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 明细采购价回填解析（无 I/O）
/// </summary>
public static class TaktBomCalculatePurchaseInfoRecordHelper
{
    /// <summary>
    /// 采购总价条件类型（与 TaktPurchaseInfoRecord.PriceType 默认 PB00 一致）
    /// </summary>
    public const string GrossPriceType = "PB00";
    /// <summary>
    /// 数量等级基础（字典 logistics_procurement_scale_basis；C=数量规模）
    /// </summary>
    public const string QuantityScaleBasis = "C";
    /// <summary>
    /// 价值等级基础（字典 logistics_procurement_scale_basis；B=价值等级）
    /// </summary>
    public const string ValueScaleBasis = "B";
    /// <summary>
    /// ExtField 中回填记录根键（与核算日成本归档键并存）
    /// </summary>
    public const string ExtFieldBackfillRootKey = "_bk";
    /// <summary>
    /// BOM 明细采购价回填在 _bk 下的子键
    /// </summary>
    public const string ExtFieldBackfillBcKey = "bc";
    /// <summary>
    /// ExtField nvarchar 上限（与公司基类一致）
    /// </summary>
    public const int ExtFieldMaxLength = 4000;

    /// <summary>
    /// 按核算日取 ValidFrom≤核算日且最晚的一条；无则 null（❌ 不用未来 ValidFrom，不回填 0）
    /// 例：有效起始 6/1 与 7/1，核算 6/30→6/1，核算 7/31→7/1；核算 5/31→无价格
    /// </summary>
    /// <param name="headers">同组件物料的采购价格主表（不按工厂过滤）</param>
    /// <param name="costingDate">核算日期</param>
    /// <returns>主表；无 ≤核算日 的有效起始则 null</returns>
    public static TaktPurchaseInfoRecord? ResolveNearestHeader(
        IReadOnlyList<TaktPurchaseInfoRecord> headers,
        DateTime costingDate)
    {
        ArgumentNullException.ThrowIfNull(headers);
        if (headers.Count == 0)
        {
            return null;
        }
        var day = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(costingDate);
        var pastOrOn = headers
            .Where(h => TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(h.ValidFrom) <= day)
            .ToList();
        if (pastOrOn.Count == 0)
        {
            return null;
        }
        return pastOrOn
            .OrderByDescending(h => TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(h.ValidFrom))
            .ThenBy(h => PreferGrossPriceRank(h.PriceType))
            .ThenByDescending(h => h.Id)
            .First();
    }

    /// <summary>
    /// 取未作废条件行（优先 PB00，再按定价序号）
    /// </summary>
    /// <param name="items">同一主表下条件行</param>
    /// <returns>条件行；全作废或空则 null</returns>
    public static TaktPurchaseInfoRecordItem? ResolveActiveItem(IReadOnlyList<TaktPurchaseInfoRecordItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        return items
            .Where(i => i.IsObsolete == 0)
            .OrderBy(i => PreferGrossPriceRank(i.PriceType))
            .ThenBy(i => i.PurchaseInfoRecordSeq)
            .ThenBy(i => i.Id)
            .FirstOrDefault();
    }

    /// <summary>
    /// 解析净价：有数量等级按组件数量取档；否则有价值等级按组件数量比门槛；否则用条件行价格
    /// </summary>
    /// <param name="item">条件行</param>
    /// <param name="quantityScales">数量等级</param>
    /// <param name="valueScales">价值等级</param>
    /// <param name="componentQuantity">组件数量</param>
    /// <returns>净价（5 位小数）</returns>
    public static decimal ResolveNetPrice(
        TaktPurchaseInfoRecordItem item,
        IReadOnlyList<TaktPurchaseInfoRecordScaleQuantity> quantityScales,
        IReadOnlyList<TaktPurchaseInfoRecordScaleValue> valueScales,
        decimal componentQuantity)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(quantityScales);
        ArgumentNullException.ThrowIfNull(valueScales);
        var qtyRows = quantityScales.Where(s => s.IsObsolete == 0).ToList();
        var valueRows = valueScales.Where(s => s.IsObsolete == 0).ToList();
        var basis = item.ScaleBasis?.Trim() ?? string.Empty;
        var preferQuantity = string.Equals(basis, QuantityScaleBasis, StringComparison.OrdinalIgnoreCase)
            || (!string.Equals(basis, ValueScaleBasis, StringComparison.OrdinalIgnoreCase) && qtyRows.Count > 0);
        if (preferQuantity && qtyRows.Count > 0)
        {
            return TaktBomMaterialCostItemLineCostHelper.RoundCost(
                PickScalePrice(qtyRows, s => s.ScaleQuantity, s => s.Price, componentQuantity));
        }
        if (valueRows.Count > 0)
        {
            return TaktBomMaterialCostItemLineCostHelper.RoundCost(
                PickScalePrice(valueRows, s => s.ScaleValue, s => s.Price, componentQuantity));
        }
        return TaktBomMaterialCostItemLineCostHelper.RoundCost(item.Price);
    }

    /// <summary>
    /// 仅空字段回填采购组织/组/供应商/净价/货币/单位；非空不覆盖；源值为空或净价 0 不写入；写入内容合并到 ExtField._bk.bc
    /// </summary>
    /// <param name="row">BOM 明细</param>
    /// <param name="header">采购价格主表</param>
    /// <param name="item">条件行</param>
    /// <param name="netPrice">净价</param>
    /// <returns>任一空字段被写入则为 true</returns>
    public static bool ApplyPurchaseFields(
        TaktBomMaterialCostItem row,
        TaktPurchaseInfoRecord header,
        TaktPurchaseInfoRecordItem item,
        decimal netPrice)
    {
        ArgumentNullException.ThrowIfNull(row);
        ArgumentNullException.ThrowIfNull(header);
        ArgumentNullException.ThrowIfNull(item);
        var organization = header.PlantCode?.Trim() ?? string.Empty;
        var group = header.PurchaseGroup?.Trim() ?? string.Empty;
        var supplier = header.SupplierCode?.Trim() ?? string.Empty;
        var currency = item.ConditionCurrencyCode?.Trim() ?? string.Empty;
        var unit = item.PriceUnit <= 0 ? 1 : item.PriceUnit;
        var filled = new JsonObject();
        var changed = false;
        if (string.IsNullOrWhiteSpace(row.PurchaseOrganization) && !string.IsNullOrWhiteSpace(organization))
        {
            row.PurchaseOrganization = organization;
            filled["purchase_organization"] = organization;
            changed = true;
        }
        if (string.IsNullOrWhiteSpace(row.PurchaseGroup) && !string.IsNullOrWhiteSpace(group))
        {
            row.PurchaseGroup = group;
            filled["purchase_group"] = group;
            changed = true;
        }
        if (string.IsNullOrWhiteSpace(row.SupplierCode) && !string.IsNullOrWhiteSpace(supplier))
        {
            row.SupplierCode = supplier;
            filled["supplier_code"] = supplier;
            changed = true;
        }
        if (row.NetPurchasePrice == 0m && netPrice != 0m)
        {
            row.NetPurchasePrice = netPrice;
            filled["net_purchase_price"] = netPrice;
            changed = true;
        }
        if (string.IsNullOrWhiteSpace(row.PurchaseCurrencyCode) && !string.IsNullOrWhiteSpace(currency))
        {
            row.PurchaseCurrencyCode = currency;
            filled["purchase_currency_code"] = currency;
            changed = true;
        }
        if (row.PurchasePriceUnit <= 1 && unit > 1)
        {
            row.PurchasePriceUnit = unit;
            filled["purchase_price_unit"] = unit;
            changed = true;
        }
        if (!changed)
        {
            return false;
        }
        var validFrom = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(header.ValidFrom);
        var priceCode = header.PurchaseInfoRecordCode?.Trim() ?? string.Empty;
        filled["at"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        filled["purchase_info_record_code"] = priceCode;
        filled["valid_from"] = validFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        filled["price"] = netPrice;
        filled["price_info"] = FormatPurchaseInfoRecordInfo(priceCode, supplier, validFrom, netPrice);
        row.ExtField = MergeBkExtField(row.ExtField, ExtFieldBackfillBcKey, filled);
        return true;
    }

    /// <summary>
    /// 可读采购价标识：定价记录号：供应商：有效起始日：价格
    /// </summary>
    /// <param name="purchaseInfoRecordCode">定价记录号</param>
    /// <param name="supplierCode">供应商</param>
    /// <param name="validFrom">有效起始日</param>
    /// <param name="price">价格</param>
    /// <returns>可读字符串</returns>
    public static string FormatPurchaseInfoRecordInfo(
        string purchaseInfoRecordCode,
        string supplierCode,
        DateTime validFrom,
        decimal price)
    {
        var day = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(validFrom)
            .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return string.Concat(
            "定价记录号：",
            purchaseInfoRecordCode?.Trim() ?? string.Empty,
            "：供应商：",
            supplierCode?.Trim() ?? string.Empty,
            "：有效起始日：",
            day,
            "：价格：",
            price.ToString(CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// 追加回填快照到 ExtField._bk.{scope}（数组追加，不覆盖历史；单对象自动迁入）
    /// </summary>
    /// <param name="extField">原扩展字段</param>
    /// <param name="scope">回填作用域（bc/bv/pup/sp）</param>
    /// <param name="payload">本次写入字段（含 at）</param>
    /// <returns>合并后 JSON；超长则尽量保留原值</returns>
    public static string? MergeBkExtField(string? extField, string scope, JsonObject payload)
    {
        return TaktBomExtFieldBackfillHistoryHelper.Append(extField, scope, payload);
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
    /// PB00 排 0，其余排 1
    /// </summary>
    /// <param name="priceType">条件类型</param>
    /// <returns>排序键</returns>
    private static int PreferGrossPriceRank(string? priceType)
    {
        return string.Equals(priceType?.Trim(), GrossPriceType, StringComparison.OrdinalIgnoreCase) ? 0 : 1;
    }

    /// <summary>
    /// 按门槛取档：组件数量 ≥ 门槛取最大门槛；否则取最小门槛
    /// </summary>
    /// <typeparam name="T">等级行</typeparam>
    /// <param name="rows">等级行</param>
    /// <param name="thresholdSelector">门槛</param>
    /// <param name="priceSelector">价格</param>
    /// <param name="quantity">组件数量</param>
    /// <returns>档位价格</returns>
    private static decimal PickScalePrice<T>(
        IReadOnlyList<T> rows,
        Func<T, decimal> thresholdSelector,
        Func<T, decimal> priceSelector,
        decimal quantity)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(thresholdSelector);
        ArgumentNullException.ThrowIfNull(priceSelector);
        if (rows.Count == 0)
        {
            return 0m;
        }
        var matched = rows
            .Where(r => thresholdSelector(r) <= quantity)
            .OrderByDescending(thresholdSelector)
            .FirstOrDefault();
        if (matched != null)
        {
            return priceSelector(matched);
        }
        return priceSelector(rows.OrderBy(thresholdSelector).First());
    }
}
