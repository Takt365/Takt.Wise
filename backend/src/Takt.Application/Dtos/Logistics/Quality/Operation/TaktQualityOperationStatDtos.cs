// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktQualityOperationStatDtos.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：质量统计（进货 / 过程 / 完成品）查询与行 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Quality.Operation;

/// <summary>
/// 质量统计查询
/// </summary>
public class TaktQualityOperationStatQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 页签：iqc 进货 / ipqc 过程 / fqc 完成品
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
    /// 维度编码（进货=供应商，过程=工序，完成品=客户；可空）
    /// </summary>
    public string? DimensionCode { get; set; }
}

/// <summary>
/// 质量统计行
/// </summary>
public class TaktQualityOperationStatRowDto
{
    /// <summary>
    /// 工厂
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 维度编码
    /// </summary>
    public string DimensionCode { get; set; } = string.Empty;

    /// <summary>
    /// 维度名称
    /// </summary>
    public string DimensionName { get; set; } = string.Empty;

    /// <summary>
    /// 检验单数
    /// </summary>
    public int OrderCount { get; set; }

    /// <summary>
    /// 抽样数量
    /// </summary>
    public decimal SampleQty { get; set; }

    /// <summary>
    /// 合格数量
    /// </summary>
    public decimal QualifiedQty { get; set; }

    /// <summary>
    /// 不合格数量
    /// </summary>
    public decimal UnqualifiedQty { get; set; }

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal ReturnQty { get; set; }

    /// <summary>
    /// 不良率（%）
    /// </summary>
    public decimal DefectRatePercent { get; set; }

    /// <summary>
    /// 合格率（%）
    /// </summary>
    public decimal QualifiedRatePercent { get; set; }
}

/// <summary>
/// 质量统计分页结果
/// </summary>
public class TaktQualityOperationStatResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktQualityOperationStatRowDto> Paged { get; set; } = TaktPagedResult<TaktQualityOperationStatRowDto>.Create([], 0, 1, 20);

    /// <summary>
    /// 行数
    /// </summary>
    public int RowCount { get; set; }
}
