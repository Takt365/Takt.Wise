// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesActualTrendsController.cs
// 创建时间：2026-09-17
// 创建人：Takt365(Cursor AI)
// 功能描述：销售实绩推移转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Sales;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Sales;

/// <summary>
/// 销售实绩推移转置分析控制器（与销售发票 CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售实绩推移")]
public class TaktSalesActualTrendsController : TaktControllerBase
{
    private readonly ITaktSalesActualTrendService _salesActualTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesActualTrendService">销售实绩推移服务</param>
    public TaktSalesActualTrendsController(ITaktSalesActualTrendService salesActualTrendService)
    {
        _salesActualTrendService = salesActualTrendService;
    }

    /// <summary>
    /// 推移查询栏：销售发票行工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:actual:trend:list", "销售实绩推移工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetSalesActualTrendPlantOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salesActualTrendService.GetSalesActualTrendPlantOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂去重客户
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:actual:trend:list", "销售实绩推移客户选项")]
    [HttpGet("customer-options")]
    public async Task<IActionResult> GetSalesActualTrendCustomerOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _salesActualTrendService.GetSalesActualTrendCustomerOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂去重物料（客户可选）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="customerCode">客户编码（可选）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:actual:trend:list", "销售实绩推移物料选项")]
    [HttpGet("material-options")]
    public async Task<IActionResult> GetSalesActualTrendMaterialOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? customerCode = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _salesActualTrendService.GetSalesActualTrendMaterialOptionsAsync(plantCode, keyword, customerCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 销售实绩推移转置分析（工厂×物料；列=过账年月）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    [TaktPermission("logistics:sales:actual:trend:list", "销售实绩推移")]
    [HttpGet("trend-analysis")]
    public async Task<IActionResult> GetSalesActualTrendAnalysisAsync(
        [FromQuery] TaktPurchasePriceTrendQueryDto queryDto)
    {
        try
        {
            var result = await _salesActualTrendService.GetSalesActualTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 销售发票金额统计（数据看板近三月；按开票日期）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>销售发票统计</returns>
    [TaktPermission("logistics:sales:actual:trend:list", "销售实绩发票统计")]
    [HttpGet("invoice-stat")]
    public async Task<IActionResult> GetSalesActualTrendInvoiceStatAsync([FromQuery] TaktSalesInvoiceStatQueryDto queryDto)
    {
        try
        {
            var result = await _salesActualTrendService.GetSalesActualTrendInvoiceStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 清单导出：销售实绩推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:sales:actual:trend:export", "清单导出销售实绩推移")]
    [HttpGet("trend-analysis/export")]
    public async Task<IActionResult> ExportSalesActualTrendAnalysisAsync(
        [FromQuery] TaktPurchasePriceTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesActualTrendService.ExportSalesActualTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.GetExportContentType(resultFileName), resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
