// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomModelCostTrendDtos.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 机种成本推移 DTO（一行一个成品：各月成本、相邻月差额、组件差异）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

/// <summary>
/// 机种成本推移查询（工厂 + 期间 + 物料类型；机种可空=全部机种）
/// </summary>
public class TaktBomModelCostTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（可选；空=范围内全部机种，各机种单独对比其成品）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 物料类型（本表 MaterialType；必填）
    /// </summary>
    public string? MaterialType { get; set; }

    /// <summary>
    /// 核算期间起（月初）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 核算期间止（月初；组件差异取该月）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }
}

/// <summary>
/// 机种成本推移行（一个成品）
/// </summary>
public class TaktBomModelCostTrendDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 币种
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 各月成品成本（键 yyyy-MM；ProductMonthlyCalculation；缺月无键）
    /// </summary>
    public Dictionary<string, decimal> PeriodCosts { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 相邻月差额（期间最后月 − 前一月；缺月则为空）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 共通价格（期间止月、同机种各成品用量与单价都相同的组件行成本合计；5 位小数；无止月成本则为空）
    /// </summary>
    public decimal? CommonPrice { get; set; }

    /// <summary>
    /// 差异价格（期间止月、同机种各成品间用量或单价不一致的组件行成本合计；5 位小数；无止月成本则为空）
    /// </summary>
    public decimal? VariancePrice { get; set; }

    /// <summary>
    /// 组件差异（同机种各成品间用量或单价不一致的组件。首行 Net 为差异行成本合计，其后全量 编码:用量-行成本，换行分隔）
    /// </summary>
    public string ComponentVarianceText { get; set; } = string.Empty;
}

/// <summary>
/// 机种成本推移分析结果
/// </summary>
public class TaktBomModelCostTrendResultDto
{
    /// <summary>
    /// 分页成品行
    /// </summary>
    public TaktPagedResult<TaktBomModelCostTrendDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 组件差异所取月份（期间止月）
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 机种数
    /// </summary>
    public int ModelCount { get; set; }

    /// <summary>
    /// 成品数（分页前）
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// 各月成品成本合计（分页前）
    /// </summary>
    public Dictionary<string, decimal> PeriodCostTotals { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 相邻月差额合计（分页前）
    /// </summary>
    public decimal? VarianceAmountTotal { get; set; }

    /// <summary>
    /// 共通价格合计（分页前）
    /// </summary>
    public decimal? CommonPriceTotal { get; set; }

    /// <summary>
    /// 差异价格合计（分页前）
    /// </summary>
    public decimal? VariancePriceTotal { get; set; }
}
