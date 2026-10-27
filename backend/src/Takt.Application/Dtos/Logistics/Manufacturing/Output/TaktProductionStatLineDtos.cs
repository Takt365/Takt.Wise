// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktProductionStatLineDtos.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：生产统计明细行 DTO。组立按日报主表，PCBA 按日报明细
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Manufacturing.Output;

/// <summary>
/// 生产统计明细行
/// </summary>
public class TaktProductionStatLineDto
{
    /// <summary>
    /// 行键
    /// </summary>
    public string RowKey { get; set; } = string.Empty;

    /// <summary>
    /// 工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产批次
    /// </summary>
    public string BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 标准工时 ST（分钟）
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 计划台数（组立：明细标准产能合计）
    /// </summary>
    public decimal PlanQty { get; set; }

    /// <summary>
    /// 直接人数
    /// </summary>
    public int DirectLabor { get; set; }

    /// <summary>
    /// 间接人数
    /// </summary>
    public int IndirectLabor { get; set; }

    /// <summary>
    /// 生产工数（分钟，组立明细投入工时合计）
    /// </summary>
    public decimal LaborMinutes { get; set; }

    /// <summary>
    /// 实绩台数。PCBA 为当日完成数
    /// </summary>
    public decimal ActualQty { get; set; }

    /// <summary>
    /// 实绩 ST（生产工数 / 实绩台数）
    /// </summary>
    public decimal ActualStdMinutes { get; set; }

    /// <summary>
    /// ST 差异（实绩 ST - ST）
    /// </summary>
    public decimal StdDiff { get; set; }

    /// <summary>
    /// 台数差异（实绩台数 - 计划台数）
    /// </summary>
    public decimal QtyDiff { get; set; }

    /// <summary>
    /// 达成率（%）
    /// </summary>
    public decimal AchievementRate { get; set; }

    /// <summary>
    /// LOT 数量（PCBA 批次数量）
    /// </summary>
    public decimal LotQty { get; set; }

    /// <summary>
    /// PCB 类别
    /// </summary>
    public string PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 板面
    /// </summary>
    public string PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 累计生产
    /// </summary>
    public decimal CumulativeQty { get; set; }

    /// <summary>
    /// 完成状态（字典 logistics_manufacturing_pcba_completed_status）
    /// </summary>
    public int CompletedStatus { get; set; }

    /// <summary>
    /// 不良数量 / 不良台数
    /// </summary>
    public int DefectQty { get; set; }
}

/// <summary>
/// 生产统计明细结果
/// </summary>
public class TaktProductionStatLineResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktProductionStatLineDto> Paged { get; set; } = TaktPagedResult<TaktProductionStatLineDto>.Create([], 0, 1, 20);

    /// <summary>
    /// 总行数
    /// </summary>
    public int RowCount { get; set; }
}
