// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityCostReportDtos.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品质成本报告查询与行 DTO
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Quality.Cost;

/// <summary>
/// 品质成本报告查询
/// </summary>
public class TaktQualityCostReportQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 页签：assurance 品质保证 / issue 品质问题 / incident 品质事故
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
    /// 维度编码（保证=记录组，问题=机种，事故=废弃组；可空）
    /// </summary>
    public string? DimensionCode { get; set; }

    /// <summary>
    /// 币种（可空）
    /// </summary>
    public string? CurrencyCode { get; set; }
}

/// <summary>
/// 品质成本报告行
/// </summary>
public class TaktQualityCostReportRowDto
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
    /// 币种
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 单据数
    /// </summary>
    public int OrderCount { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 数量（问题=总时间分钟，事故=废弃数量，保证为 0）
    /// </summary>
    public decimal Quantity { get; set; }
}

/// <summary>
/// 品质成本报告分页结果
/// </summary>
public class TaktQualityCostReportResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktQualityCostReportRowDto> Paged { get; set; } = TaktPagedResult<TaktQualityCostReportRowDto>.Create([], 0, 1, 20);

    /// <summary>
    /// 行数
    /// </summary>
    public int RowCount { get; set; }
}
