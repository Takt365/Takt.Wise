// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktQualityInspectionMonthlyTrendDtos.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：IQC/IPQC/FQC 检验月推移转置分析共用基类 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Operation;

// ========================================
// 检验月推移转置分析（共用基类；Iqc/Ipqc/Fqc 各有独立 *OrderTrendDtos）
// ========================================

/// <summary>
/// 检验月推移转置分析查询基类
/// </summary>
public class TaktQualityInspectionMonthlyTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验期间起（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 检验期间止（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选）；缺省取期间末月，相对上月算环比
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 检验月推移转置行基类（列=各月不良率及辅助指标）
/// </summary>
public class TaktQualityInspectionMonthlyTrendDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 各期间不良率（键 yyyy-MM；0~1 小数比率；无抽样为 null）
    /// </summary>
    public Dictionary<string, decimal?> PeriodDefectRates { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各期间检验单数
    /// </summary>
    public Dictionary<string, int> PeriodOrderCounts { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各期间抽样数量合计
    /// </summary>
    public Dictionary<string, int> PeriodSampleQuantities { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各期间不合格数量合计
    /// </summary>
    public Dictionary<string, int> PeriodUnqualifiedQuantities { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 环比涨跌：none / up / down / flat
    /// </summary>
    public string Trend { get; set; } = "none";

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 环比不良率差额（对比率 - 基准率）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（小数比率，保留 4 位）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 检验月推移转置分析结果
/// </summary>
/// <typeparam name="TRow">行 DTO 类型</typeparam>
public class TaktQualityInspectionMonthlyTrendResultDto<TRow> where TRow : TaktQualityInspectionMonthlyTrendDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TRow> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 行总数（分页前，已应用涨跌筛选）
    /// </summary>
    public int RowCount { get; set; }

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间（关注月）
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 不良率上升行数（筛选前全量统计）
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 不良率下降行数（筛选前全量统计）
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
}
