// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostTrendsController.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 产品成本推移分析控制器（仅组件移动价推移）
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
/// BOM 产品成本推移分析控制器（与机种推移 / CostItem CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM物料成本推移")]
public class TaktBomMaterialCostTrendsController : TaktControllerBase
{
    private readonly ITaktBomMaterialCostTrendService _bomMaterialCostTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostTrendService">BOM 产品成本推移服务</param>
    public TaktBomMaterialCostTrendsController(ITaktBomMaterialCostTrendService bomMaterialCostTrendService)
    {
        _bomMaterialCostTrendService = bomMaterialCostTrendService;
    }

    /// <summary>
    /// 产品成本推移：单个产品下明细组件 × 月材料成本转置
    /// </summary>
    /// <param name="queryDto">查询 DTO（工厂 + 产品必填）</param>
    /// <returns>明细组件月材料成本结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:trend:list", "产品成本推移")]
    [HttpGet("component-moving-price-analysis")]
    public async Task<IActionResult> GetBomMaterialCostTrendComponentMovingPriceAnalysisAsync(
        [FromQuery] TaktBomMaterialCostTrendComponentMovingPriceQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostTrendService.GetBomMaterialCostTrendComponentMovingPriceAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出产品成本推移（单个产品明细组件×月材料成本）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:trend:export", "导出产品成本推移")]
    [HttpGet("component-moving-price-analysis/export")]
    public async Task<IActionResult> ExportBomMaterialCostTrendComponentMovingPriceAnalysisAsync(
        [FromQuery] TaktBomMaterialCostTrendComponentMovingPriceQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostTrendService.ExportBomMaterialCostTrendComponentMovingPriceAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
