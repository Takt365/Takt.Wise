// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostTrendDtos.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 产品成本推移 DTO（组件×月材料成本；与机种推移 / 成本分析分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

// ========================================
// 机种合并组件 × 移动价格期间转置分析 DTO
// ========================================

/// <summary>
/// BOM 成本推移：按单个产品汇总月材料成本转置查询（不按机种合并组件）
/// </summary>
public class TaktBomMaterialCostTrendComponentMovingPriceQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（可选；仅缩小产品范围）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（必填；仅分析该单个产品）
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（本表 MaterialType；空=不过滤）
    /// </summary>
    public string? MaterialType { get; set; }

    /// <summary>
    /// 核算期间起（CostingDate 月初语义；传任意日由服务归一到月初）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 核算期间止（CostingDate 月初语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选）；按产品月材料成本环比
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// BOM 成本推移明细行：单个产品下组件（明细表）各核算月材料成本转置；缺月不回填
/// </summary>
public class TaktBomMaterialCostTrendComponentMovingPriceDto
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
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// BOM 层级（明细表）
    /// </summary>
    public string BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（明细表）
    /// </summary>
    public string BomItemCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（明细表）
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述（明细表）
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量（明细表）
    /// </summary>
    public decimal ComponentQuantity { get; set; }

    /// <summary>
    /// 生产相关（明细表）
    /// </summary>
    public string? ProductionRelated { get; set; }

    /// <summary>
    /// 采购类型（明细表）
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 币种
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 各核算月组件移动单价（键 yyyy-MM；MovingAveragePrice÷MovingPriceUnit，不乘组件数量；缺月无键；API PeriodCostTotals 为各组件单价合计）
    /// </summary>
    public Dictionary<string, decimal> PeriodMaterialCosts { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各核算月相对上一展示月的存在/价格变动（键 yyyy-MM，与 PeriodOrder 对齐）。
    /// present=首月有；absent=本月无且上月无；new=本月有上月无；removed=本月无上月有；up/down/flat=两月都有且价格对比。
    /// </summary>
    public Dictionary<string, string> PeriodChangeTypes { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 环比涨跌：none / up / down / flat / new / removed
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
    /// 环比差额（对比月材料成本 - 基准月材料成本）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（百分点，如 -0.34 表示 -0.34%；导出 Excel 时 ÷100）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// BOM 成本推移（单个产品明细组件×月材料成本）分析结果
/// </summary>
public class TaktBomMaterialCostTrendComponentMovingPriceResultDto
{
    /// <summary>
    /// 分页明细组件行
    /// </summary>
    public TaktPagedResult<TaktBomMaterialCostTrendComponentMovingPriceDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 产品编码列表（通常仅 1 个）
    /// </summary>
    public List<string> ProductCodes { get; set; } = new();

    /// <summary>
    /// 明细组件行总数（分页前，已应用涨跌筛选）
    /// </summary>
    public int ComponentCount { get; set; }

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间（关注月）
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 涨价产品数（筛选前全量统计）
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 跌价产品数（筛选前全量统计）
    /// </summary>
    public int DownCount { get; set; }

    /// <summary>
    /// 持平产品数（筛选前全量统计）
    /// </summary>
    public int FlatCount { get; set; }

    /// <summary>
    /// 关注月新增组件数（筛选前全量统计）
    /// </summary>
    public int NewCount { get; set; }

    /// <summary>
    /// 关注月剔除组件数（筛选前全量统计）
    /// </summary>
    public int RemovedCount { get; set; }

    /// <summary>
    /// 无法比较产品数（筛选前全量统计）
    /// </summary>
    public int NoneCount { get; set; }

    /// <summary>
    /// 全量行各期间材料成本合计（分页前、已应用涨跌筛选）
    /// </summary>
    public Dictionary<string, decimal> PeriodCostTotals { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 全量行环比差额合计（分页前、已应用涨跌筛选）
    /// </summary>
    public decimal? VarianceAmountTotal { get; set; }
}
