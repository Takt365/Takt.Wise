// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintTrendDtos.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：顾客投诉月度推移分析 DTO（customer-complaint-trend）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Complaint;

/// <summary>
/// 顾客投诉月度推移分析查询 DTO
/// </summary>
public class TaktCustomerComplaintMonthlyTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填；映射实体 PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 投诉期间起（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 投诉期间止（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选）；缺省取期间末月，相对上月算环比
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 客户编码（可选）
    /// </summary>
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 投诉类型（字典 logistics_quality_complaint_type；可选）
    /// </summary>
    public int? ComplaintType { get; set; }

    /// <summary>
    /// 投诉等级（字典 logistics_quality_complaint_level；可选）
    /// </summary>
    public int? ComplaintLevel { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 顾客投诉月度推移转置行（行=工厂+客户，列=各月投诉件数）
/// </summary>
public class TaktCustomerComplaintMonthlyTrendDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（空串表示无编码）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// 各期间投诉件数（键 yyyy-MM）
    /// </summary>
    public Dictionary<string, int> PeriodValues { get; set; } = new(StringComparer.Ordinal);

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
    /// 环比差额（对比件数 - 基准件数）
    /// </summary>
    public int? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（小数比率，保留 4 位）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 顾客投诉月度推移分析结果
/// </summary>
public class TaktCustomerComplaintMonthlyTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktCustomerComplaintMonthlyTrendDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 客户行总数（分页前，已应用涨跌筛选）
    /// </summary>
    public int CustomerCount { get; set; }

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
