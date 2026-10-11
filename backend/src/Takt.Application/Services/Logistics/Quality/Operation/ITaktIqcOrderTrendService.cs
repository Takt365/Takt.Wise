// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：ITaktIqcOrderTrendService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：IQC 进货检验月推移转置分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Operation;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// IQC 进货检验月推移转置分析服务
/// </summary>
public interface ITaktIqcOrderTrendService
{
    /// <summary>
    /// IQC 进货检验月推移转置分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析结果</returns>
    Task<TaktQualityInspectionMonthlyTrendResultDto<TaktIqcOrderMonthlyTrendDto>> GetIqcOrderMonthlyTrendAnalysisAsync(
        TaktIqcOrderMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出 IQC 进货检验月推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportIqcOrderMonthlyTrendAnalysisAsync(
        TaktIqcOrderMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
