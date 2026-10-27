// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomModelCostTrendsController.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 机种成本推移分析控制器
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
/// BOM 机种成本推移分析控制器（与 CostItem CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM机种成本推移")]
public class TaktBomModelCostTrendsController : TaktControllerBase
{
    private readonly ITaktBomModelCostTrendService _bomModelCostTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomModelCostTrendService">机种成本推移服务</param>
    public TaktBomModelCostTrendsController(ITaktBomModelCostTrendService bomModelCostTrendService)
    {
        _bomModelCostTrendService = bomModelCostTrendService;
    }

    /// <summary>
    /// 机种成本推移：机种月材料成本 + 合并键分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>机种月成本与合并键分析行</returns>
    [TaktPermission("logistics:manufacturing:bom:model:cost:trend:list", "机种成本推移")]
    [HttpGet("model-cost-trend-analysis")]
    public async Task<IActionResult> GetBomModelCostTrendAnalysisAsync(
        [FromQuery] TaktBomModelCostTrendQueryDto queryDto)
    {
        try
        {
            var result = await _bomModelCostTrendService.GetBomModelCostTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出机种成本推移分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:model:cost:trend:export", "导出机种成本推移")]
    [HttpGet("model-cost-trend-analysis/export")]
    public async Task<IActionResult> ExportBomModelCostTrendAnalysisAsync(
        [FromQuery] TaktBomModelCostTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomModelCostTrendService.ExportBomModelCostTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
