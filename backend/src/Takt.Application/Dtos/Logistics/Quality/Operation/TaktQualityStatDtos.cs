// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktQualityStatDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：质量检验统计 DTO（数据看板 inspection-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Quality.Operation;

/// <summary>
/// 质量检验统计查询 DTO（按检验日期区间）
/// </summary>
public class TaktQualityStatQueryDto
{
    /// <summary>
    /// 检验日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? InspectionDateStart { get; set; }

    /// <summary>
    /// 检验日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? InspectionDateEnd { get; set; }
}

/// <summary>
/// 质量检验统计 DTO（IQC/FQC/IPQC 共用结构）
/// </summary>
public class TaktQualityInspectionStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月检验单数量
    /// </summary>
    public int MonthOrderCount { get; set; }

    /// <summary>
    /// 月抽样数量合计
    /// </summary>
    public int MonthSampleQuantity { get; set; }

    /// <summary>
    /// 月合格数量合计
    /// </summary>
    public int MonthQualifiedQuantity { get; set; }

    /// <summary>
    /// 月不合格数量合计
    /// </summary>
    public int MonthUnqualifiedQuantity { get; set; }

    /// <summary>
    /// 月合格率（%）
    /// </summary>
    public decimal MonthPassRatePercent { get; set; }
}

/// <summary>
/// IQC 检验统计 DTO
/// </summary>
public class TaktIqcOrderStatDto : TaktQualityInspectionStatDto
{
}

/// <summary>
/// FQC 检验统计 DTO
/// </summary>
public class TaktFqcOrderStatDto : TaktQualityInspectionStatDto
{
}

/// <summary>
/// IPQC 检验统计 DTO
/// </summary>
public class TaktIpqcOrderStatDto : TaktQualityInspectionStatDto
{
}
