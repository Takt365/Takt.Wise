// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktPurchaseInfoRecordTaxHelper.cs
// 创建时间：2026-07-16
// 创建人：Takt365(Cursor AI)
// 功能描述：采购信息记录行含税/未税单价冗余换算（与主表税别、税率对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 采购信息记录税务换算工具
/// </summary>
public static class TaktPurchaseInfoRecordTaxHelper
{
    /// <summary>
    /// 按主表税别/税率，由录入单价换算含税单价与未税单价（decimal 保留 5 位）
    /// </summary>
    /// <param name="unitPrice">录入单价</param>
    /// <param name="taxCategory">税别（字典 accounting_financial_tax_category；1=含税视为含税价，其余视为未税价）</param>
    /// <param name="taxRate">税率（字典 accounting_financial_tax_rate_param；13 表示 13%）</param>
    /// <returns>含税单价与未税单价</returns>
    /// <exception cref="ArgumentOutOfRangeException">税率为负</exception>
    public static (decimal TaxIncludedUnitPrice, decimal TaxExcludedUnitPrice) ResolveRedundantUnitPrices(
        decimal unitPrice,
        int taxCategory,
        int taxRate)
    {
        if (taxRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(taxRate), "税率不能为负数");
        }
        if (taxCategory == 2)
        {
            return (unitPrice, unitPrice);
        }
        var factor = 1m + taxRate / 100m;
        if (taxCategory == 1)
        {
            var excluded = factor == 0m ? unitPrice : RoundUnitPrice(unitPrice / factor);
            return (unitPrice, excluded);
        }
        var included = RoundUnitPrice(unitPrice * factor);
        return (included, unitPrice);
    }

    /// <summary>
    /// 单价四舍五入至 5 位小数
    /// </summary>
    /// <param name="value">原值</param>
    /// <returns>舍入结果</returns>
    private static decimal RoundUnitPrice(decimal value) => Math.Round(value, 5, MidpointRounding.AwayFromZero);
}
