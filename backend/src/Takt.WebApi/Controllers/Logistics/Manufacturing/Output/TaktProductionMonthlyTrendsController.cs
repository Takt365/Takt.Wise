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
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Output;

/// <summary>
/// 月生产推移转置分析控制器（与组立/PCBA 产出 CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "月生产推移")]
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
  /// 推移查询栏：组立/PCBA 产出本表工厂去重选项
  /// </summary>
  /// <returns>下拉选项</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:list", "月生产推移工厂选项")]
  [HttpGet("plant-options")]
  public async Task<IActionResult> GetProductionMonthlyTrendPlantOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
    try
    {
      var result = await _productionMonthlyTrendService.GetProductionMonthlyTrendPlantOptionsAsync(plantCode, keyword);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 推移查询栏：按工厂返回有数据的产出类别
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <returns>下拉选项</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:list", "月生产推移产出类别选项")]
  [HttpGet("output-category-options")]
  public async Task<IActionResult> GetProductionMonthlyTrendOutputCategoryOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
    try
    {
      if (string.IsNullOrWhiteSpace(plantCode))
      {
        return Success(new List<TaktSelectOption>(), "查询成功");
      }
      var result = await _productionMonthlyTrendService.GetProductionMonthlyTrendOutputCategoryOptionsAsync(plantCode, keyword);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 推移查询栏：按工厂+产出类别去重机种
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="outputCategory">产出类别（assy/pcba；可空）</param>
  /// <returns>下拉选项</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:list", "月生产推移机种选项")]
  [HttpGet("model-options")]
  public async Task<IActionResult> GetProductionMonthlyTrendModelOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? outputCategory = null)
    {
    try
    {
      if (string.IsNullOrWhiteSpace(plantCode))
      {
        return Success(new List<TaktSelectOption>(), "查询成功");
      }
      var result = await _productionMonthlyTrendService.GetProductionMonthlyTrendModelOptionsAsync(plantCode, keyword, outputCategory);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 月生产推移转置分析（工厂×机种×产出类别×月份）
  /// </summary>
  /// <param name="queryDto">查询 DTO</param>
  /// <returns>转置分析结果</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:list", "月生产推移")]
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
  /// 清单导出：月生产推移（工厂×机种×产出类别×月份转置）
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <param name="sheetName">工作表名称</param>
  /// <param name="exportName">导出文件名</param>
  /// <returns>Excel 文件</returns>
  [TaktPermission("logistics:manufacturing:output:production:monthly:export", "清单导出月生产推移")]
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
