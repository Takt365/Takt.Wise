// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktFqcOrderTrendDtos.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：FQC 成品检验月推移转置分析 DTO（独立于检验单 CRUD）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Quality.Operation;

/// <summary>
/// FQC 检验月推移查询
/// </summary>
public class TaktFqcOrderMonthlyTrendQueryDto : TaktQualityInspectionMonthlyTrendQueryDto
{
    /// <summary>
    /// 客户编码（可选）
    /// </summary>
    public string? CustomerCode { get; set; }
}

/// <summary>
/// FQC 检验月推移转置行（工厂×客户）
/// </summary>
public class TaktFqcOrderMonthlyTrendDto : TaktQualityInspectionMonthlyTrendDto
{
    /// <summary>
    /// 客户编码（空串表示无客户）
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;
}
