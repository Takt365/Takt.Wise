// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialMovingTrendDtos.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料移动价格推移转置分析 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Materials;

/// <summary>
/// 物料 × 月份移动单价转置分析查询
/// </summary>
public class TaktMaterialMovingTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格期间起（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 移动价格期间止（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选）；缺省取期间末月，相对上月算环比
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 评估类别（可选；为空时按物料+估值分行）
    /// </summary>
    public string? Valuation { get; set; }

    /// <summary>
    /// 物料编码（可选，模糊匹配）
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 物料移动价格推移转置行（行=物料+估值，列=各月单价 MovingPrice÷PriceUnit）
/// </summary>
public class TaktMaterialMovingTrendDto
{
    public string PlantCode { get; set; } = string.Empty;
    public string MaterialCode { get; set; } = string.Empty;
    public string MaterialName { get; set; } = string.Empty;
    public string Valuation { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public Dictionary<string, decimal> PeriodUnitPrices { get; set; } = new(StringComparer.Ordinal);
    public Dictionary<string, string> PeriodPriceSourcePeriods { get; set; } = new(StringComparer.Ordinal);
    public string Trend { get; set; } = "none";
    public string? BasePeriod { get; set; }
    public string? ComparePeriod { get; set; }
    public decimal? VarianceAmount { get; set; }
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 物料移动价格推移转置分析结果
/// </summary>
public class TaktMaterialMovingTrendResultDto
{
    public TaktPagedResult<TaktMaterialMovingTrendDto> Paged { get; set; } = null!;
    public List<string> PeriodOrder { get; set; } = new();
    public int MaterialCount { get; set; }
    public string? BasePeriod { get; set; }
    public string? ComparePeriod { get; set; }
    public int UpCount { get; set; }
    public int DownCount { get; set; }
    public int FlatCount { get; set; }
    public int NoneCount { get; set; }
}
