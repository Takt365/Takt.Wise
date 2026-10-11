// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostAnalysisTrendsController.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析月度涨跌控制器（与转置/差异分析分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 成本分析月度涨跌控制器（与 TaktBomMaterialCostAnalysesController 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM物料成本月度涨跌")]
public class TaktBomMaterialCostAnalysisTrendsController : TaktControllerBase
{
    private readonly ITaktBomMaterialCostAnalysisTrendService _bomMaterialCostAnalysisTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostAnalysisTrendService">月度涨跌服务</param>
    public TaktBomMaterialCostAnalysisTrendsController(
        ITaktBomMaterialCostAnalysisTrendService bomMaterialCostAnalysisTrendService)
    {
        _bomMaterialCostAnalysisTrendService = bomMaterialCostAnalysisTrendService;
    }

    /// <summary>
    /// 获取 BOM 物料成本月度涨跌分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>月度涨跌结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:query", "BOM成本分析月度涨跌")]
    [HttpGet("monthly-trend-analysis")]
    public async Task<IActionResult> GetBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
        [FromQuery] TaktBomMaterialCostAnalysisMonthlyTrendQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostAnalysisTrendService.GetBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 BOM 物料成本月度涨跌分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:export", "导出BOM成本分析月度涨跌")]
    [HttpGet("monthly-trend-analysis/export")]
    public async Task<IActionResult> ExportBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
        [FromQuery] TaktBomMaterialCostAnalysisMonthlyTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostAnalysisTrendService.ExportBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
