// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：ITaktBomModelCostTrendService.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 机种成本推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 机种成本推移（一行一个成品：各月成本、相邻月差额、组件差异）。
/// </summary>
public interface ITaktBomModelCostTrendService
{
    /// <summary>
    /// 机种成本推移分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析结果</returns>
    Task<TaktBomModelCostTrendResultDto> GetBomModelCostTrendAnalysisAsync(
        TaktBomModelCostTrendQueryDto queryDto);

    /// <summary>
    /// 导出机种成本推移分析
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportBomModelCostTrendAnalysisAsync(
        TaktBomModelCostTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
