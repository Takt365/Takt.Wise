// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcMonthlyTrendsController.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：设变推移转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变推移转置分析控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变推移")]
public class TaktEcMonthlyTrendsController : TaktControllerBase
{
    private readonly ITaktEcMonthlyTrendService _ecMonthlyTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecMonthlyTrendService">设变推移服务</param>
    public TaktEcMonthlyTrendsController(ITaktEcMonthlyTrendService ecMonthlyTrendService)
    {
        _ecMonthlyTrendService = ecMonthlyTrendService;
    }

    /// <summary>
    /// 推移查询栏：工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:monthly:trend:list", "设变推移工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetEcMonthlyTrendPlantOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ecMonthlyTrendService.GetEcMonthlyTrendPlantOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂去重部门
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:monthly:trend:list", "设变推移部门选项")]
    [HttpGet("dept-options")]
    public async Task<IActionResult> GetEcMonthlyTrendDeptOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _ecMonthlyTrendService.GetEcMonthlyTrendDeptOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂+部门去重设变单号（部门可空）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:monthly:trend:list", "设变推移设变单号选项")]
    [HttpGet("ec-code-options")]
    public async Task<IActionResult> GetEcMonthlyTrendEcCodeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? deptCode = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _ecMonthlyTrendService.GetEcMonthlyTrendEcCodeOptionsAsync(plantCode, keyword, deptCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 设变推移转置分析（工厂×设变号×部门×月份完成件数）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:monthly:trend:list", "设变推移")]
    [HttpGet("monthly-trend-analysis")]
    public async Task<IActionResult> GetEcMonthlyTrendAnalysisAsync(
        [FromQuery] TaktEcMonthlyTrendQueryDto queryDto)
    {
        try
        {
            var result = await _ecMonthlyTrendService.GetEcMonthlyTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 清单导出：设变推移（工厂×设变号×部门×月份转置）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:monthly:trend:export", "清单导出设变推移")]
    [HttpGet("monthly-trend-analysis/export")]
    public async Task<IActionResult> ExportEcMonthlyTrendAnalysisAsync(
        [FromQuery] TaktEcMonthlyTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecMonthlyTrendService.ExportEcMonthlyTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 月实施推移转置分析（工厂×部门×月份）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:monthly:trend:list", "设变推移")]
    [HttpGet("implementation-monthly-trend-analysis")]
    public async Task<IActionResult> GetEcImplementationMonthlyTrendAnalysisAsync(
        [FromQuery] TaktEcImplementationMonthlyTrendQueryDto queryDto)
    {
        try
        {
            var result = await _ecMonthlyTrendService.GetEcImplementationMonthlyTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 清单导出：月实施推移（工厂×部门×月份转置）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:monthly:trend:export", "清单导出设变推移")]
    [HttpGet("implementation-monthly-trend-analysis/export")]
    public async Task<IActionResult> ExportEcImplementationMonthlyTrendAnalysisAsync(
        [FromQuery] TaktEcImplementationMonthlyTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecMonthlyTrendService.ExportEcImplementationMonthlyTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
