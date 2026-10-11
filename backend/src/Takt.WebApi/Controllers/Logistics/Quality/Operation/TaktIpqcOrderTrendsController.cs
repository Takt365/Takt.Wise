// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktIpqcOrderTrendsController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：IPQC 过程质量月推移转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Application.Services.Logistics.Quality.Operation;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Quality.Operation;

/// <summary>
/// IPQC 过程质量月推移转置分析控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "过程质量推移")]
public class TaktIpqcOrderTrendsController : TaktControllerBase
{
    private readonly ITaktIpqcOrderTrendService _ipqcOrderTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ipqcOrderTrendService">过程质量推移服务</param>
    public TaktIpqcOrderTrendsController(ITaktIpqcOrderTrendService ipqcOrderTrendService)
    {
        _ipqcOrderTrendService = ipqcOrderTrendService;
    }

    /// <summary>
    /// IPQC 过程质量月推移转置分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    [TaktPermission("logistics:quality:operation:ipqc:trend:list", "过程质量推移")]
    [HttpGet("monthly-trend-analysis")]
    public async Task<IActionResult> GetIpqcOrderMonthlyTrendAnalysisAsync(
        [FromQuery] TaktIpqcOrderMonthlyTrendQueryDto queryDto)
    {
        try
        {
            var result = await _ipqcOrderTrendService.GetIpqcOrderMonthlyTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 清单导出：IPQC 过程质量月推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:quality:operation:ipqc:trend:export", "导出过程质量推移")]
    [HttpGet("monthly-trend-analysis/export")]
    public async Task<IActionResult> ExportIpqcOrderMonthlyTrendAnalysisAsync(
        [FromQuery] TaktIpqcOrderMonthlyTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ipqcOrderTrendService.ExportIpqcOrderMonthlyTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
