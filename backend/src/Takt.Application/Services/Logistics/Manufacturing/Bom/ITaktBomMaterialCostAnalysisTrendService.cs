// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomMaterialCostAnalysisTrendService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析月度涨跌服务接口（与转置/差异分析分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 成本分析月度涨跌服务（与 TaktBomMaterialCostAnalysisService 分离）
/// </summary>
public interface ITaktBomMaterialCostAnalysisTrendService
{
    /// <summary>
    /// 获取成本分析月度涨跌（机种下单产品或产品平均月成本序列）
    /// </summary>
    /// <param name="queryDto">月度涨跌查询 DTO</param>
    /// <returns>月度涨跌结果</returns>
    Task<TaktBomMaterialCostAnalysisMonthlyTrendResultDto> GetBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostAnalysisMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出成本分析月度涨跌 Excel
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>实际文件名与文件字节</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostAnalysisMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
