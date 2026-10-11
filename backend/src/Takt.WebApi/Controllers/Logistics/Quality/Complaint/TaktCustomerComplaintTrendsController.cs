// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintTrendsController.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：顾客投诉月度推移转置分析控制器（与客诉 CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Application.Services.Logistics.Quality.Complaint;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Quality.Complaint;

/// <summary>
/// 顾客投诉月度推移转置分析控制器（与 TaktCustomerComplaintsController 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "顾客投诉推移")]
public class TaktCustomerComplaintTrendsController : TaktControllerBase
{
    private readonly ITaktCustomerComplaintTrendService _customerComplaintTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerComplaintTrendService">顾客投诉推移服务</param>
    public TaktCustomerComplaintTrendsController(ITaktCustomerComplaintTrendService customerComplaintTrendService)
    {
        _customerComplaintTrendService = customerComplaintTrendService;
    }

    /// <summary>
    /// 顾客投诉月度推移转置分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:trend:list", "顾客投诉推移列表")]
    [HttpGet("monthly-trend-analysis")]
    public async Task<IActionResult> GetCustomerComplaintMonthlyTrendAnalysisAsync(
        [FromQuery] TaktCustomerComplaintMonthlyTrendQueryDto queryDto)
    {
        try
        {
            var result = await _customerComplaintTrendService.GetCustomerComplaintMonthlyTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出顾客投诉月度推移转置分析
    /// </summary>
    /// <param name="query">查询 DTO</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:quality:complaint:customer:trend:export", "导出顾客投诉推移")]
    [HttpGet("monthly-trend-analysis/export")]
    public async Task<IActionResult> ExportCustomerComplaintMonthlyTrendAnalysisAsync(
        [FromQuery] TaktCustomerComplaintMonthlyTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _customerComplaintTrendService.ExportCustomerComplaintMonthlyTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
