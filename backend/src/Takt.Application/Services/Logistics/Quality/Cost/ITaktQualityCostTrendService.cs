// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：ITaktQualityCostTrendService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：质量成本月推移转置分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Cost;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 质量成本月推移转置分析服务
/// </summary>
public interface ITaktQualityCostTrendService
{
    /// <summary>
    /// 获取质量成本月推移转置分析（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    Task<TaktQualityCostTrendResultDto> GetQualityCostMonthlyTrendAnalysisAsync(
        TaktQualityCostTrendQueryDto queryDto);

    /// <summary>
    /// 导出质量成本月推移转置分析 Excel
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">导出文件名</param>
    /// <returns>文件名与内容</returns>
    Task<(string fileName, byte[] fileContent)> ExportQualityCostMonthlyTrendAnalysisAsync(
        TaktQualityCostTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
