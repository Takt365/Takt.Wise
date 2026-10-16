// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchasePriceTrendDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格推移转置分析 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Procurement;

/// <summary>
/// 工厂 × 物料 × 供应商采购单价转置分析查询
/// </summary>
public class TaktPurchasePriceTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格期间起（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 价格期间止（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间（可选；采购价格推移按各行最早对最晚 ValidFrom 计期间涨跌，相邻有效起始日另计，忽略本字段）
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 物料编码（可选，模糊匹配）
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 供应商编码（可选）
    /// </summary>
    public string? SupplierCode { get; set; }

    /// <summary>
    /// 价格类型（字典 logistics_procurement_price_type；可选；如 PB00）
    /// </summary>
    public string? PriceType { get; set; }

    /// <summary>
    /// 仅启用价格主表（主表已无 PriceStatus；保留兼容，忽略）
    /// </summary>
    public bool? OnlyEnabled { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }

    /// <summary>
    /// 明细导出/列表可见列键（逗号分隔；空=导出全部明细列）
    /// </summary>
    public string? DetailColumnKeys { get; set; }
}

/// <summary>
/// 采购价格推移转置行（行=工厂+物料+供应商；列=该行自己的第 n 笔 ValidFrom，不对齐全表日期）
/// </summary>
public class TaktPurchasePriceTrendDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价记录号（与采购信息头一致；只来自所选工厂）
    /// </summary>
    public string PurchaseInfoCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（回填：随物料）
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称（冗余，回填：随供应商）
    /// </summary>
    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 币种
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// 各有效起始日折算单价（键 yyyy-MM-dd；供导出/兼容）
    /// </summary>
    public Dictionary<string, decimal> PeriodUnitPrices { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各列单价来源（与日期键相同；无缺月回填）
    /// </summary>
    public Dictionary<string, string> PeriodPriceSourcePeriods { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 该行有效起始日序列（自早到晚；表格第 n 笔对应本列表第 n 项）
    /// </summary>
    public List<TaktPurchasePriceTrendPointDto> PeriodPoints { get; set; } = new();

    /// <summary>
    /// 相邻有效起始日比较（键=后一次 yyyy-MM-dd）
    /// </summary>
    public Dictionary<string, TaktPurchasePriceTrendAdjacentDto> PeriodAdjacents { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 期间涨跌（该行最早有效起始日 vs 最晚有效起始日）：none / up / down / flat
    /// </summary>
    public string Trend { get; set; } = "none";

    /// <summary>
    /// 期间基准有效起始日（该行最早）
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 期间对比有效起始日（该行最晚）
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 期间差额（采购价格推移=末笔单价-首笔单价；采购订单价格历史推移=各月「单价差×当月数量」合计，即因单价造成的采购金额浮动）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 期间变动率（小数比率，保留 4 位；如 0.2978 表示 29.78%）
    /// </summary>
    public decimal? VariancePercent { get; set; }

    /// <summary>
    /// MaterialDescription
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// CurrencyCode
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;
}

/// <summary>
/// 采购价格推移转置分析结果
/// </summary>
public class TaktPurchasePriceTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktPurchasePriceTrendDto> Paged { get; set; } = null!;

    /// <summary>
    /// 第 n 笔序号（1 起；各行按自己的 ValidFrom 取第 n 笔，不对齐全表日期）
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 有效起始日行总数（与导出条数一致；分页前，已应用涨跌筛选）
    /// </summary>
    public int MaterialCount { get; set; }

    /// <summary>
    /// 期间基准笔序号（第 1 笔）
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 期间对比笔序号（末笔）
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 涨价行数（筛选前全量统计）
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 跌价行数（筛选前全量统计）
    /// </summary>
    public int DownCount { get; set; }

    /// <summary>
    /// 持平行数（筛选前全量统计）
    /// </summary>
    public int FlatCount { get; set; }

    /// <summary>
    /// 无法比较行数（筛选前全量统计）
    /// </summary>
    public int NoneCount { get; set; }

    /// <summary>
    /// 各月采购金额 / 涨价合计 / 跌价合计（采购订单价格历史推移用）
    /// </summary>
    public List<TaktPurchasePriceTrendMonthSummaryDto> MonthSummaries { get; set; } = new();

    /// <summary>
    /// 期间采购金额合计（采购订单价格历史推移用）
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 期间涨价合计（各物料因单价上涨造成的采购金额之和）
    /// </summary>
    public decimal TotalUpAmount { get; set; }

    /// <summary>
    /// 期间跌价合计（各物料因单价下跌造成的采购金额之和）
    /// </summary>
    public decimal TotalDownAmount { get; set; }
}

/// <summary>
/// 价格推移单月汇总（金额 + 因单价涨/跌拆开的影响金额）
/// </summary>
public class TaktPurchasePriceTrendMonthSummaryDto
{
    /// <summary>
    /// 年月 yyyy-MM
    /// </summary>
    public string Period { get; set; } = string.Empty;

    /// <summary>
    /// 当月采购金额合计
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 当月涨价合计（单价上涨物料的「单价差×当月数量」之和）
    /// </summary>
    public decimal UpAmount { get; set; }

    /// <summary>
    /// 当月跌价合计（单价下跌物料的「单价差×当月数量」绝对值之和）
    /// </summary>
    public decimal DownAmount { get; set; }
}

/// <summary>
/// 采购价格推移单笔（该行自己的一次 ValidFrom）
/// </summary>
public class TaktPurchasePriceTrendPointDto
{
    /// <summary>
    /// 有效起始日 yyyy-MM-dd
    /// </summary>
    public string ValidFrom { get; set; } = string.Empty;

    /// <summary>
    /// 折算单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 当月采购数量（采购订单价格历史推移用；采购价格推移忽略）
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// 当月采购金额（采购订单价格历史推移用；采购价格推移忽略）
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 当月是否有采购（采购订单价格历史推移用）
    /// </summary>
    public bool HasPurchase { get; set; }

    /// <summary>
    /// 相对前一笔的涨跌：none / up / down / flat（第 1 笔为 none）
    /// </summary>
    public string AdjacentTrend { get; set; } = "none";

    /// <summary>
    /// 相对前一笔差额（采购价格推移=单价差额；采购订单价格历史推移=单价差额×当月数量，即因单价造成的采购金额变动）
    /// </summary>
    public decimal? AdjacentVarianceAmount { get; set; }

    /// <summary>
    /// 相对前一笔变动率（小数比率）
    /// </summary>
    public decimal? AdjacentVariancePercent { get; set; }

    /// <summary>
    /// 前一笔有效起始日 yyyy-MM-dd
    /// </summary>
    public string? AdjacentBasePeriod { get; set; }
}

/// <summary>
/// 相邻两次有效起始日比较（相对前一次）
/// </summary>
public class TaktPurchasePriceTrendAdjacentDto
{
    /// <summary>
    /// 前一次有效起始日 yyyy-MM-dd
    /// </summary>
    public string BasePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 本次有效起始日 yyyy-MM-dd
    /// </summary>
    public string ComparePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 相邻涨跌：up / down / flat
    /// </summary>
    public string Trend { get; set; } = "none";

    /// <summary>
    /// 相邻差额（本次单价 - 前一次单价）
    /// </summary>
    public decimal VarianceAmount { get; set; }

    /// <summary>
    /// 相邻变动率（小数比率，保留 4 位）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}
