// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktOutboundActualTrendsController.cs
// 创建时间：2026-09-17
// 创建人：Takt365(Cursor AI)
// 功能描述：物料出库推移转置分析控制器（物料凭证行 CustomerCode 非空）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 物料出库推移转置分析控制器（与物料凭证 CRUD、销售实绩推移分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料出库推移")]
public class TaktOutboundActualTrendsController : TaktControllerBase
{
    private readonly ITaktOutboundActualTrendService _outboundActualTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="outboundActualTrendService">物料出库推移服务</param>
    public TaktOutboundActualTrendsController(ITaktOutboundActualTrendService outboundActualTrendService)
    {
        _outboundActualTrendService = outboundActualTrendService;
    }

    /// <summary>
    /// 推移查询栏：客户非空凭证行工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:outbound:trend:list", "物料出库推移工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetOutboundActualTrendPlantOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _outboundActualTrendService.GetOutboundActualTrendPlantOptionsAsync(plantCode, keyword);
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
    [TaktPermission("logistics:materials:material:outbound:trend:list", "物料出库推移客户选项")]
    [HttpGet("customer-options")]
    public async Task<IActionResult> GetOutboundActualTrendCustomerOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _outboundActualTrendService.GetOutboundActualTrendCustomerOptionsAsync(plantCode, keyword);
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
    [TaktPermission("logistics:materials:material:outbound:trend:list", "物料出库推移物料选项")]
    [HttpGet("material-options")]
    public async Task<IActionResult> GetOutboundActualTrendMaterialOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? customerCode = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _outboundActualTrendService.GetOutboundActualTrendMaterialOptionsAsync(plantCode, keyword, customerCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 物料出库推移转置分析（工厂×物料；列=过账年月）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    [TaktPermission("logistics:materials:material:outbound:trend:list", "物料出库推移")]
    [HttpGet("trend-analysis")]
    public async Task<IActionResult> GetOutboundActualTrendAnalysisAsync(
        [FromQuery] TaktPurchasePriceTrendQueryDto queryDto)
    {
        try
        {
            var result = await _outboundActualTrendService.GetOutboundActualTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 清单导出：物料出库推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:materials:material:outbound:trend:export", "清单导出物料出库推移")]
    [HttpGet("trend-analysis/export")]
    public async Task<IActionResult> ExportOutboundActualTrendAnalysisAsync(
        [FromQuery] TaktPurchasePriceTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _outboundActualTrendService.ExportOutboundActualTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.GetExportContentType(resultFileName), resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
