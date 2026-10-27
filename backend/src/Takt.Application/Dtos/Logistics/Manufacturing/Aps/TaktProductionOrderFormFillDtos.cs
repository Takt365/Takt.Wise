// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Aps
// 文件名称：TaktProductionOrderFormFillDtos.cs
// 创建时间：2026-08-30
// 创建人：Takt365(Cursor AI)
// 功能描述：生产工单表单回填 DTO（独立非实体 CRUD；generate-dtos / generate-all 不会覆盖）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Manufacturing.Aps;

/// <summary>
/// 按工单号回填制造产出等表单的查询参数
/// </summary>
public class TaktProductionOrderFormFillQueryDto
{
    /// <summary>
    /// 生产工单号（必填）
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（可选；传入时与工单精确匹配）
    /// </summary>
    public string? PlantCode { get; set; }

    /// <summary>
    /// 生产日期（可选；用于解析标准工时/稼动率，缺省为当天）
    /// </summary>
    public DateTime? ProdDate { get; set; }

    /// <summary>
    /// 直接作业人数（可选；传入时计算 StdCapacity）
    /// </summary>
    public int? DirectLabor { get; set; }

    /// <summary>
    /// 是否附带按标准工时生成的默认明细预览（PCBA 日报子表）
    /// </summary>
    public bool IncludeDefaultDetails { get; set; }
}

/// <summary>
/// 生产工单表单回填结果（工单主档 + 机种 + 工时 + 可选默认明细）
/// </summary>
public class TaktProductionOrderFormFillDto
{
    /// <summary>
    /// 生产工单号
    /// </summary>
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单类别
    /// </summary>
    public string ProdOrderType { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（来自型号目的地）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次（工单 ProdBatch）
    /// </summary>
    public string? BatchCode { get; set; }

    /// <summary>
    /// 工单数量
    /// </summary>
    public decimal ProdOrderQty { get; set; }

    /// <summary>
    /// 序列号
    /// </summary>
    public string? SerialCode { get; set; }

    /// <summary>
    /// 标准工时（分钟；按物料标准工时汇总）
    /// </summary>
    public decimal StdMinutes { get; set; }

    /// <summary>
    /// 人员标准生产稼动率（%）
    /// </summary>
    public decimal OperationRatePercent { get; set; }

    /// <summary>
    /// 标准产能（小时产能；仅 DirectLabor 有值时计算）
    /// </summary>
    public decimal? StdCapacity { get; set; }

    /// <summary>
    /// 默认明细预览（IncludeDefaultDetails=true 时）
    /// </summary>
    public List<TaktProductionOrderFormFillDefaultDetailDto>? DefaultDetails { get; set; }
}

/// <summary>
/// 按标准工时生成的默认明细预览行（通用；PCBA 子表 timePeriod=WorkCenter）
/// </summary>
public class TaktProductionOrderFormFillDefaultDetailDto
{
    /// <summary>
    /// 行号（10 起递增）
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// 工作中心
    /// </summary>
    public string WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工序描述
    /// </summary>
    public string? OperationDesc { get; set; }

    /// <summary>
    /// 标准点数 / 标准短时
    /// </summary>
    public int StandardShorts { get; set; }

    /// <summary>
    /// 标准工时（分钟；ConvertedMinutes 优先，否则 StandardMinutes）
    /// </summary>
    public decimal StandardMinutes { get; set; }
}
