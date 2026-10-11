// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMonthlyTrendDtos.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月设变推移转置分析 DTO（设变号×部门×月份完成件数）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 月设变推移转置分析查询 DTO（按设变号×部门统计）
/// </summary>
public class TaktEcMonthlyTrendQueryDto : TaktPagedQuery
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
    /// 设变单号（可选，模糊）
    /// </summary>
    public string? EcNo { get; set; }

    /// <summary>
    /// 责任部门编码（可选）
    /// </summary>
    public string? DeptCode { get; set; }

    /// <summary>
    /// 实施范围（字典 logistics_manufacturing_ec_scope_category；可选，按主表过滤）
    /// </summary>
    public int? EcScope { get; set; }
    /// <summary>
    /// 部门实施范围（字典 logistics_manufacturing_ec_dept_scope_category；1=按成品，2=按机种，3=按设变）
    /// </summary>
    public int? EcDeptScope { get; set; }

    /// <summary>
    /// 变更状态（字典 logistics_manufacturing_ec_status；可选，按主表过滤）
    /// </summary>
    public int? ChangeStatus { get; set; }

    /// <summary>
    /// 设变状态（字典 logistics_manufacturing_ec_gijutsu_status；可选，按主表过滤）
    /// </summary>
    public int? EcStatus { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }

    /// <summary>
    /// EcCode
    /// </summary>
    public string? EcCode { get; set; }
}

/// <summary>
/// 月设变推移转置行（行=工厂+设变号+部门，列=各月完成任务件数）
/// </summary>
public class TaktEcMonthlyTrendDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 各期间完成件数（键 yyyy-MM，按 CompletedAt）
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

    /// <summary>
    /// 设变单号
    /// </summary>
    public string EcCode { get; set; } = string.Empty;
}

/// <summary>
/// 月设变推移转置分析结果
/// </summary>
public class TaktEcMonthlyTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktEcMonthlyTrendDto> Paged { get; set; } = null!;

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

/// <summary>
/// 月实施推移转置分析查询 DTO（按部门汇总）
/// </summary>
public class TaktEcImplementationMonthlyTrendQueryDto : TaktPagedQuery
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
    /// 责任部门编码（可选）
    /// </summary>
    public string? DeptCode { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 月实施推移转置行（行=工厂+部门，列=各月完成任务件数）
/// </summary>
public class TaktEcImplementationMonthlyTrendDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 责任部门编码
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 各期间实施件数（键 yyyy-MM，按 CompletedAt）
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
/// 月实施推移转置分析结果
/// </summary>
public class TaktEcImplementationMonthlyTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktEcImplementationMonthlyTrendDto> Paged { get; set; } = null!;

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
