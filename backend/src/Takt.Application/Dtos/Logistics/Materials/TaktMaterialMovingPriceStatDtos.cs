// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialMovingPriceStatDtos.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：移动价格在库金额统计 DTO（数据看板 stock-stat；按评估类别汇总）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Materials;

/// <summary>
/// 在库金额统计查询 DTO（按评估期间 yyyy-MM）
/// </summary>
public class TaktMaterialMovingPriceStatQueryDto
{
    /// <summary>
    /// 评估期间（yyyy-MM；必填）
    /// </summary>
    public string? ValuationPeriod { get; set; }
}

/// <summary>
/// 在库金额按评估类别分项
/// </summary>
public class TaktMaterialMovingPriceValuationAmountDto
{
    /// <summary>
    /// 评估类别（字典 logistics_materials_valuation_class；如 Z792/Z790/Z300）
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 该评估类别库存金额合计（元，2 位小数）
    /// </summary>
    public decimal StockAmount { get; set; }
}

/// <summary>
/// 在库金额统计 DTO（按评估期间汇总；含评估类别分项）
/// </summary>
public class TaktMaterialMovingPriceStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM，与评估期间一致）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 在库金额合计（元，2 位小数）
    /// </summary>
    public decimal MonthStockAmount { get; set; }

    /// <summary>
    /// 物料行数（该评估期间移动价格记录数）
    /// </summary>
    public int MonthRowCount { get; set; }

    /// <summary>
    /// 按评估类别分项金额（Z792/Z790/Z300 等）
    /// </summary>
    public List<TaktMaterialMovingPriceValuationAmountDto> ByValuation { get; set; } = new();
}
