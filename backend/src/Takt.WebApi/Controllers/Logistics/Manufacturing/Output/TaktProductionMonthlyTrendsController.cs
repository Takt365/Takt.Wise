// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktProductionMonthlyTrendsController.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产推移转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services.Logistics.Manufacturing.Output;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Output;

/// <summary>
/// 生产统计控制器（组立/PCBA 按班组、按机种分表查询）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "生产统计")]
public class TaktProductionMonthlyTrendsController : TaktControllerBase
{
  private readonly ITaktProductionMonthlyTrendService _productionMonthlyTrendService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="productionMonthlyTrendService">月生产推移服务</param>
  public TaktProductionMonthlyTrendsController(ITaktProductionMonthlyTrendService productionMonthlyTrendService)
  {
    _productionMonthlyTrendService = productionMonthlyTrendService;
  }

  /// <summary>
  /// 当前页签产出表的工厂去重
  /// </summary>
  /// <param name="statTab">页签</param>
  /// <param name="keyword">关键字</param>
  /// <returns>下拉选项</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:list", "生产统计工厂选项")]
  [HttpGet("plant-options")]
  public async Task<IActionResult> GetProductionMonthlyTrendPlantOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? keyword = null)
  {
    try
    {
      var result = await _productionMonthlyTrendService.GetProductionMonthlyTrendPlantOptionsAsync(statTab, keyword);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 当前页签产出表的班组去重
  /// </summary>
  /// <param name="statTab">页签</param>
  /// <param name="plantCode">工厂</param>
  /// <param name="keyword">关键字</param>
  /// <returns>下拉选项</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:list", "生产统计班组选项")]
  [HttpGet("team-options")]
  public async Task<IActionResult> GetProductionMonthlyTrendTeamOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
  {
    try
    {
      var result = await _productionMonthlyTrendService.GetProductionMonthlyTrendTeamOptionsAsync(statTab, plantCode, keyword);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 当前页签产出表的机种去重
  /// </summary>
  /// <param name="statTab">页签</param>
  /// <param name="plantCode">工厂</param>
  /// <param name="keyword">关键字</param>
  /// <returns>下拉选项</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:list", "生产统计机种选项")]
  [HttpGet("model-options")]
  public async Task<IActionResult> GetProductionMonthlyTrendModelOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
  {
    try
    {
      var result = await _productionMonthlyTrendService.GetProductionMonthlyTrendModelOptionsAsync(statTab, plantCode, keyword);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 当前页签产出表的物料去重
  /// </summary>
  /// <param name="statTab">页签</param>
  /// <param name="plantCode">工厂</param>
  /// <param name="keyword">关键字</param>
  /// <returns>下拉选项</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:list", "生产统计物料选项")]
  [HttpGet("material-options")]
  public async Task<IActionResult> GetProductionMonthlyTrendMaterialOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
  {
    try
    {
      var result = await _productionMonthlyTrendService.GetProductionMonthlyTrendMaterialOptionsAsync(statTab, plantCode, keyword);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 生产统计明细（组立日报行或 PCBA 明细行）
  /// </summary>
  /// <param name="queryDto">查询</param>
  /// <returns>分页行</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:list", "生产统计")]
  [HttpGet("lines")]
  public async Task<IActionResult> GetProductionStatLineListAsync([FromQuery] TaktProductionMonthlyTrendQueryDto queryDto)
  {
    try
    {
      var result = await _productionMonthlyTrendService.GetProductionStatLineListAsync(queryDto);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 清单导出生产统计明细
  /// </summary>
  /// <param name="query">查询</param>
  /// <param name="sheetName">工作表</param>
  /// <param name="exportName">文件名</param>
  /// <returns>Excel</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:export", "清单导出生产统计")]
  [HttpGet("lines/export")]
  public async Task<IActionResult> ExportProductionStatLineAsync(
      [FromQuery] TaktProductionMonthlyTrendQueryDto query,
      [FromQuery] string? sheetName = null,
      [FromQuery] string? exportName = null)
  {
    try
    {
      var (resultFileName, fileContent) = await _productionMonthlyTrendService.ExportProductionStatLineAsync(query, sheetName, exportName);
      return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 生产统计（当前页签按班组或按机种汇总月份产量）
  /// </summary>
  /// <param name="queryDto">查询 DTO</param>
  /// <returns>转置分析结果</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:list", "生产统计")]
  [HttpGet("monthly-trend-analysis")]
  public async Task<IActionResult> GetProductionMonthlyTrendAnalysisAsync(
      [FromQuery] TaktProductionMonthlyTrendQueryDto queryDto)
  {
    try
    {
      var result = await _productionMonthlyTrendService.GetProductionMonthlyTrendAnalysisAsync(queryDto);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 清单导出：生产统计（当前页签）
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <param name="sheetName">工作表名称</param>
  /// <param name="exportName">导出文件名</param>
  /// <returns>Excel 文件</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:export", "清单导出生产统计")]
  [HttpGet("monthly-trend-analysis/export")]
  public async Task<IActionResult> ExportProductionMonthlyTrendAnalysisAsync(
      [FromQuery] TaktProductionMonthlyTrendQueryDto query,
      [FromQuery] string? sheetName = null,
      [FromQuery] string? exportName = null)
  {
    try
    {
      var (resultFileName, fileContent) = await _productionMonthlyTrendService.ExportProductionMonthlyTrendAnalysisAsync(
          query, sheetName, exportName);
      return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }
}
