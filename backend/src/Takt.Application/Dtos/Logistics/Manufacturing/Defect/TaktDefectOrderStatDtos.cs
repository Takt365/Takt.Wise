// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Defect
// 文件名称：TaktDefectOrderStatDtos.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：不良统计（工单 / 批次）查询与行 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Manufacturing.Defect;

/// <summary>
/// 不良统计（工单 / 批次）查询
/// </summary>
public class TaktDefectOrderStatQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 页签：assy-order / assy-batch / smt-inspection / pcba-repair
    /// </summary>
    public string StatTab { get; set; } = string.Empty;

    /// <summary>
    /// 工厂（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间起
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 期间止
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 工单（可空）
    /// </summary>
    public string? ProdOrderCode { get; set; }

    /// <summary>
    /// 批次（可空）
    /// </summary>
    public string? BatchCode { get; set; }

    /// <summary>
    /// 机种（可空）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 物料（可空）
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 生产类别（可空，字典 logistics_manufacturing_prod_category）
    /// </summary>
    public string? ProdCategory { get; set; }
}

/// <summary>
/// 不良统计行。组立按工单/批次实体出列，SMT 检查与 PCBA 修理按明细行出列
/// </summary>
public class TaktDefectOrderStatRowDto
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
    /// 生产类别（字典 logistics_manufacturing_prod_category）
    /// </summary>
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期组
    /// </summary>
    public string ProdDateGroup { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单组
    /// </summary>
    public string ProdOrderGroup { get; set; } = string.Empty;

    /// <summary>
    /// 生产物料组
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 工单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 批次工单总数量
    /// </summary>
    public decimal BatchOrderQty { get; set; }

    /// <summary>
    /// 订单数量组
    /// </summary>
    public string ProdOrderQtyGroup { get; set; } = string.Empty;

    /// <summary>
    /// 累计生实实绩
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 日报笔数
    /// </summary>
    public int ReportCount { get; set; }

    /// <summary>
    /// 工单状态（字典 logistics_manufacturing_prod_status）
    /// </summary>
    public int OrderStatus { get; set; }

    /// <summary>
    /// 批次状态（字典 logistics_manufacturing_prod_status）
    /// </summary>
    public int BatchStatus { get; set; }

    /// <summary>
    /// 工单类别（字典 logistics_manufacturing_prod_order_type）
    /// </summary>
    public string ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// 行号
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// 明细生产班组。修理主表班组在 TeamCode
    /// </summary>
    public string DetailTeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 检查日期（SMT：落在查询期间内的实装日期，T 面优先，否则 B 面）
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 板别（字典 logistics_manufacturing_pcba_function）
    /// </summary>
    public string PcbaBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 目视线别（字典 logistics_manufacturing_visual_inspection_line_category）
    /// </summary>
    public string VisualInspectionLine { get; set; } = string.Empty;

    /// <summary>
    /// AOI 线别（字典 logistics_manufacturing_aoi_inspection_line_category）
    /// </summary>
    public string AoiLine { get; set; } = string.Empty;

    /// <summary>
    /// B 面实装日期
    /// </summary>
    public DateTime? BSideAssemblyDate { get; set; }

    /// <summary>
    /// T 面实装日期
    /// </summary>
    public DateTime? TSideAssemblyDate { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category）
    /// </summary>
    public int ShiftNo { get; set; }

    /// <summary>
    /// 检查员名称
    /// </summary>
    public string InspectorName { get; set; } = string.Empty;

    /// <summary>
    /// 当日完成数量
    /// </summary>
    public decimal DailyCompletedQty { get; set; }

    /// <summary>
    /// 检查数量
    /// </summary>
    public decimal InspectionQty { get; set; }

    /// <summary>
    /// 检查状态（字典 logistics_manufacturing_pcba_inspection_status）
    /// </summary>
    public int InspectionStatus { get; set; }

    /// <summary>
    /// 生产班组
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 检查工数
    /// </summary>
    public decimal InspectionWorkHours { get; set; }

    /// <summary>
    /// AOI 工数
    /// </summary>
    public decimal AoiWorkHours { get; set; }

    /// <summary>
    /// 手贴
    /// </summary>
    public string HandPlacement { get; set; } = string.Empty;

    /// <summary>
    /// 流水号
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 不良个所（字典 logistics_manufacturing_pcb_location_category）
    /// </summary>
    public string DefectLocation { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProdDate { get; set; }

    /// <summary>
    /// 卡号
    /// </summary>
    public string CardCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良症状
    /// </summary>
    public string DefectSymptom { get; set; } = string.Empty;

    /// <summary>
    /// 检出工程（字典 logistics_manufacturing_defect_category）
    /// </summary>
    public string DefectEngineering { get; set; } = string.Empty;

    /// <summary>
    /// 不良原因
    /// </summary>
    public string DefectReason { get; set; } = string.Empty;

    /// <summary>
    /// 责任归属（字典 logistics_manufacturing_defect_responsibility_category）
    /// </summary>
    public string DefectResponsibility { get; set; } = string.Empty;

    /// <summary>
    /// 不良性质（字典 logistics_manufacturing_defect_nature）
    /// </summary>
    public string DefectNature { get; set; } = string.Empty;

    /// <summary>
    /// 修理员名称
    /// </summary>
    public string RepairOperatorName { get; set; } = string.Empty;

    /// <summary>
    /// 工单
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    public string BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 实绩或检查数量
    /// </summary>
    public decimal ActualQty { get; set; }

    /// <summary>
    /// 无不良数量
    /// </summary>
    public decimal GoodQty { get; set; }

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; set; }

    /// <summary>
    /// 不良率（%）
    /// </summary>
    public decimal DefectRatePercent { get; set; }

    /// <summary>
    /// 直行率（%）
    /// </summary>
    public decimal YieldRatePercent { get; set; }

    /// <summary>
    /// 最近生产日期
    /// </summary>
    public DateTime? LastProdDate { get; set; }
}

/// <summary>
/// 不良统计（工单 / 批次）分页结果
/// </summary>
public class TaktDefectOrderStatResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktDefectOrderStatRowDto> Paged { get; set; } = TaktPagedResult<TaktDefectOrderStatRowDto>.Create([], 0, 1, 20);

    /// <summary>
    /// 行数
    /// </summary>
    public int RowCount { get; set; }
}
