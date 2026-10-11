// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostAnalysisTrendDtos.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析月度涨跌 DTO（与转置/差异分析、产品成本推移分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

/// <summary>
/// 成本月度涨跌分析查询
/// </summary>
public class TaktBomMaterialCostAnalysisMonthlyTrendQueryDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    [Required]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（选定单物料时必填；为空表示机种下全部物料）
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 起始年月 yyyy-MM
    /// </summary>
    public string? PeriodStart { get; set; }

    /// <summary>
    /// 结束年月 yyyy-MM
    /// </summary>
    public string? PeriodEnd { get; set; }
}

/// <summary>
/// 成本月度涨跌分析行
/// </summary>
public class TaktBomMaterialCostAnalysisMonthlyTrendLineDto
{
    /// <summary>
    /// 期间 yyyy-MM
    /// </summary>
    public string Period { get; set; } = string.Empty;

    /// <summary>
    /// 材料总成本
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// 对比基准月
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 基准月成本
    /// </summary>
    public decimal? BaseTotalCost { get; set; }

    /// <summary>
    /// 环比差额
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（百分点，如 -0.34 表示 -0.34%；导出 Excel 时 ÷100）
    /// </summary>
    public decimal? VariancePercent { get; set; }

    /// <summary>
    /// 涨跌：none / up / down / flat
    /// </summary>
    public string Trend { get; set; } = string.Empty;
}

/// <summary>
/// 成本月度涨跌分析结果
/// </summary>
public class TaktBomMaterialCostAnalysisMonthlyTrendResultDto
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
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 是否为机种下全部物料汇总
    /// </summary>
    public bool AllMaterialsUnderModel { get; set; }

    /// <summary>
    /// 月度涨跌行
    /// </summary>
    public List<TaktBomMaterialCostAnalysisMonthlyTrendLineDto> Lines { get; set; } = new();
}


