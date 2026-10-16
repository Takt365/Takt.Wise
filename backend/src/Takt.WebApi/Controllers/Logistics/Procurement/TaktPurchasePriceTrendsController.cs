// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchasePriceTrendsController.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格推移转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Procurement;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Procurement;

/// <summary>
/// 采购价格推移转置分析控制器（与 TaktPurchaseInfos CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购价格推移")]
public class TaktPurchasePriceTrendsController : TaktControllerBase
{
    private readonly ITaktPurchasePriceTrendService _purchasePriceTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceTrendService">采购价格推移服务</param>
    public TaktPurchasePriceTrendsController(ITaktPurchasePriceTrendService purchasePriceTrendService)
    {
        _purchasePriceTrendService = purchasePriceTrendService;
    }

    /// <summary>
    /// 推移查询栏：采购价格本表工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:price:trend:list", "采购价格推移工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetPurchasePriceTrendPlantOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchasePriceTrendService.GetPurchasePriceTrendPlantOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂去重条件类型
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:price:trend:list", "采购价格推移条件类型选项")]
    [HttpGet("price-type-options")]
    public async Task<IActionResult> GetPurchasePriceTrendPriceTypeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _purchasePriceTrendService.GetPurchasePriceTrendPriceTypeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重供应商
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:price:trend:list", "采购价格推移供应商选项")]
    [HttpGet("supplier-options")]
    public async Task<IActionResult> GetPurchasePriceTrendSupplierOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? priceType = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(priceType))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _purchasePriceTrendService.GetPurchasePriceTrendSupplierOptionsAsync(plantCode, keyword, priceType);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重物料（供应商可选）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="supplierCode">供应商编码（可选）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:price:trend:list", "采购价格推移物料选项")]
    [HttpGet("material-options")]
    public async Task<IActionResult> GetPurchasePriceTrendMaterialOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? priceType = null, [FromQuery] string? supplierCode = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(priceType))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _purchasePriceTrendService.GetPurchasePriceTrendMaterialOptionsAsync(plantCode, keyword, priceType, supplierCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 采购价格推移转置分析（工厂×物料×供应商×月份）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:trend:list", "采购价格推移")]
    [HttpGet("trend-analysis")]
    public async Task<IActionResult> GetPurchasePriceTrendAnalysisAsync(
        [FromQuery] TaktPurchasePriceTrendQueryDto queryDto)
    {
        try
        {
            var result = await _purchasePriceTrendService.GetPurchasePriceTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 清单导出：采购价格推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:procurement:purchase:price:trend:export", "清单导出采购价格推移")]
    [HttpGet("trend-analysis/export")]
    public async Task<IActionResult> ExportPurchasePriceTrendAnalysisAsync(
        [FromQuery] TaktPurchasePriceTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchasePriceTrendService.ExportPurchasePriceTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
