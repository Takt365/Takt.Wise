// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Defect
// 文件名称：TaktDefectMonthlyTrendDtos.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产不良推移转置分析 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Manufacturing.Defect;

/// <summary>
/// 月生产不良推移转置分析查询 DTO
/// </summary>
public class TaktDefectMonthlyTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 分析期间起（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 分析期间止（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选）；缺省取期间末月，相对上月算环比
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 机种编码（可选）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 不良类别：assy / pcba；空表示全部
    /// </summary>
    public string? DefectCategory { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 月生产不良推移转置行（行=工厂+机种+不良类别，列=各月不良率）
/// </summary>
public class TaktDefectMonthlyTrendDto
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
    /// 不良类别：assy / pcba
    /// </summary>
    public string DefectCategory { get; set; } = string.Empty;

    /// <summary>
    /// 不良类别显示名（可空）
    /// </summary>
    public string? DefectCategoryName { get; set; }

    /// <summary>
    /// 各期间不良率（键 yyyy-MM；小数 0~1）
    /// </summary>
    public Dictionary<string, decimal> PeriodValues { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各期间生实/检查数量合计（键 yyyy-MM）
    /// </summary>
    public Dictionary<string, decimal> PeriodActualQuantities { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各期间不良数量合计（键 yyyy-MM）
    /// </summary>
    public Dictionary<string, decimal> PeriodDefectQuantities { get; set; } = new(StringComparer.Ordinal);

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
    /// 环比不良率差额（对比率 - 基准率，小数 0~1）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（小数比率，保留 4 位）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 月生产不良推移转置分析结果
/// </summary>
public class TaktDefectMonthlyTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktDefectMonthlyTrendDto> Paged { get; set; } = null!;

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
    /// 上涨行数（筛选前全量统计）
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 下跌行数（筛选前全量统计）
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
