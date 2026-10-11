// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostAnalysesController.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析控制器（转置 / 差异；月度涨跌见 TaktBomMaterialCostAnalysisTrendsController）
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
/// BOM 成本分析控制器（转置 / 差异；月度涨跌独立控制器）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM物料成本分析")]
public class TaktBomMaterialCostAnalysesController : TaktControllerBase
{
    private readonly ITaktBomMaterialCostAnalysisService _bomMaterialCostAnalysisService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostAnalysisService">BOM 成本分析服务</param>
    public TaktBomMaterialCostAnalysesController(
        ITaktBomMaterialCostAnalysisService bomMaterialCostAnalysisService)
    {
        _bomMaterialCostAnalysisService = bomMaterialCostAnalysisService;
    }

    /// <summary>
    /// 获取 BOM 物料成本转置列表（行=产品，列=月份总成本）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:list", "BOM成本分析转置列表")]
    [HttpGet("transposed")]
    public async Task<IActionResult> GetBomMaterialCostAnalysisTransposedListAsync(
        [FromQuery] TaktBomMaterialCostAnalysisTransposedQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostAnalysisService.GetBomMaterialCostAnalysisTransposedListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 BOM 物料成本转置报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:export", "导出BOM成本分析转置")]
    [HttpGet("transposed/export")]
    public async Task<IActionResult> ExportBomMaterialCostAnalysisTransposedAsync(
        [FromQuery] TaktBomMaterialCostAnalysisTransposedQueryDto? query = null,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostAnalysisService.ExportBomMaterialCostAnalysisTransposedAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 BOM 物料成本差异分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>差异分析结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:query", "BOM成本分析差异")]
    [HttpGet("variance-analysis")]
    public async Task<IActionResult> GetBomMaterialCostAnalysisVarianceAnalysisAsync(
        [FromQuery] TaktBomMaterialCostAnalysisVarianceQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostAnalysisService.GetBomMaterialCostAnalysisVarianceAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 BOM 物料成本差异分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:export", "导出BOM成本分析差异")]
    [HttpGet("variance-analysis/export")]
    public async Task<IActionResult> ExportBomMaterialCostAnalysisVarianceAnalysisAsync(
        [FromQuery] TaktBomMaterialCostAnalysisVarianceQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostAnalysisService.ExportBomMaterialCostAnalysisVarianceAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
