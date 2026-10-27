// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Defect
// 文件名称：TaktDefectMonthlyTrendsController.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产不良推移转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Application.Services.Logistics.Manufacturing.Defect;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Defect;

/// <summary>
/// 月生产不良推移转置分析控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "不良推移")]
public class TaktDefectMonthlyTrendsController : TaktControllerBase
{
  private readonly ITaktDefectMonthlyTrendService _defectMonthlyTrendService;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="defectMonthlyTrendService">月生产不良推移服务</param>
  public TaktDefectMonthlyTrendsController(ITaktDefectMonthlyTrendService defectMonthlyTrendService)
  {
    _defectMonthlyTrendService = defectMonthlyTrendService;
  }

  /// <summary>
  /// 推移查询栏：组立不良 ∪ PCBA 检查工厂去重选项
  /// </summary>
  /// <returns>下拉选项</returns>
  [TaktPermission("logistics:manufacturing:defect:monthly:list", "月生产不良推移工厂选项")]
  [HttpGet("plant-options")]
  public async Task<IActionResult> GetDefectMonthlyTrendPlantOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? statTab = null)
    {
    try
    {
      var result = await _defectMonthlyTrendService.GetDefectMonthlyTrendPlantOptionsAsync(plantCode, keyword, statTab);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 推移查询栏：按工厂可用不良类别（assy / pcba）
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <returns>下拉选项</returns>
  [TaktPermission("logistics:manufacturing:defect:monthly:list", "月生产不良推移不良类别选项")]
  [HttpGet("defect-category-options")]
  public async Task<IActionResult> GetDefectMonthlyTrendDefectCategoryOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
    try
    {
      if (string.IsNullOrWhiteSpace(plantCode))
      {
        return Success(new List<TaktSelectOption>(), "查询成功");
      }
      var result = await _defectMonthlyTrendService.GetDefectMonthlyTrendDefectCategoryOptionsAsync(plantCode, keyword);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 推移查询栏：按工厂（及可选不良类别）去重机种
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="defectCategory">不良类别（assy / pcba；可空）</param>
  /// <returns>下拉选项</returns>
  [TaktPermission("logistics:manufacturing:defect:monthly:list", "月生产不良推移机种选项")]
  [HttpGet("model-options")]
  public async Task<IActionResult> GetDefectMonthlyTrendModelOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? defectCategory = null)
    {
    try
    {
      if (string.IsNullOrWhiteSpace(plantCode))
      {
        return Success(new List<TaktSelectOption>(), "查询成功");
      }
      var result = await _defectMonthlyTrendService.GetDefectMonthlyTrendModelOptionsAsync(plantCode, keyword, defectCategory);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 月生产不良推移转置分析（工厂×机种×不良类别×月份）
  /// </summary>
  /// <param name="queryDto">查询 DTO</param>
  /// <returns>转置分析结果</returns>
  [TaktPermission("logistics:manufacturing:defect:monthly:list", "月生产不良推移")]
  [HttpGet("monthly-trend-analysis")]
  public async Task<IActionResult> GetDefectMonthlyTrendAnalysisAsync(
      [FromQuery] TaktDefectMonthlyTrendQueryDto queryDto)
  {
    try
    {
      var result = await _defectMonthlyTrendService.GetDefectMonthlyTrendAnalysisAsync(queryDto);
      return Success(result, "查询成功");
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }

  /// <summary>
  /// 清单导出：月生产不良推移（工厂×机种×不良类别×月份转置）
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <param name="sheetName">工作表名称</param>
  /// <param name="exportName">导出文件名</param>
  /// <returns>Excel 文件</returns>
  [TaktPermission("logistics:manufacturing:defect:monthly:export", "清单导出月生产不良推移")]
  [HttpGet("monthly-trend-analysis/export")]
  public async Task<IActionResult> ExportDefectMonthlyTrendAnalysisAsync(
      [FromQuery] TaktDefectMonthlyTrendQueryDto query,
      [FromQuery] string? sheetName = null,
      [FromQuery] string? exportName = null)
  {
    try
    {
      var (resultFileName, fileContent) = await _defectMonthlyTrendService.ExportDefectMonthlyTrendAnalysisAsync(
          query, sheetName, exportName);
      return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
    }
    catch (Exception ex)
    {
      return HandleException(ex);
    }
  }
}
