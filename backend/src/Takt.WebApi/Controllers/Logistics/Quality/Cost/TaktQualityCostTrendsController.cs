// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Cost
// 文件名称：TaktQualityCostTrendsController.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：质量成本月推移转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Application.Services.Logistics.Quality.Cost;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Quality.Cost;

/// <summary>
/// 质量成本月推移转置分析控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "质量成本推移")]
public class TaktQualityCostTrendsController : TaktControllerBase
{
  private readonly ITaktQualityCostTrendService _qualityCostTrendService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="qualityCostTrendService">质量成本推移服务</param>
  public TaktQualityCostTrendsController(ITaktQualityCostTrendService qualityCostTrendService)
  {
    _qualityCostTrendService = qualityCostTrendService;
  }

  /// <summary>
  /// 质量成本月推移转置分析（工厂×成本类别×币种×月份）
  /// </summary>
  /// <param name="queryDto">查询 DTO</param>
  /// <returns>转置分析结果</returns>
  [TaktPermission("logistics:quality:cost:trend:list", "质量成本推移")]
  [HttpGet("monthly-trend-analysis")]
  public async Task<IActionResult> GetQualityCostMonthlyTrendAnalysisAsync(
      [FromQuery] TaktQualityCostTrendQueryDto queryDto)
  {
    try
    {
      var result = await _qualityCostTrendService.GetQualityCostMonthlyTrendAnalysisAsync(queryDto);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 清单导出：质量成本月推移（工厂×成本类别×币种×月份转置）
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <param name="sheetName">工作表名称</param>
  /// <param name="exportName">导出文件名</param>
  /// <returns>Excel 文件</returns>
  [TaktPermission("logistics:quality:cost:trend:export", "清单导出质量成本推移")]
  [HttpGet("monthly-trend-analysis/export")]
  public async Task<IActionResult> ExportQualityCostMonthlyTrendAnalysisAsync(
      [FromQuery] TaktQualityCostTrendQueryDto query,
      [FromQuery] string? sheetName = null,
      [FromQuery] string? exportName = null)
  {
    try
    {
      var (resultFileName, fileContent) = await _qualityCostTrendService.ExportQualityCostMonthlyTrendAnalysisAsync(
          query, sheetName, exportName);
      return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }
}
