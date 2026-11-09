// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchasePriceDeviationsController.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价偏离 分析控制器（PO / MIRO 页签）
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
/// 采购价偏离 分析控制器（PO/MIRO；与采购订单/采购价格 CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购价偏离")]
public class TaktPurchasePriceDeviationsController : TaktControllerBase
{
    private readonly ITaktPurchasePriceDeviationService _purchasePriceDeviationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceDeviationService">偏离分析服务</param>
    public TaktPurchasePriceDeviationsController(ITaktPurchasePriceDeviationService purchasePriceDeviationService)
    {
        _purchasePriceDeviationService = purchasePriceDeviationService;
    }

    /// <summary>
    /// 查询栏：采购订单工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:price:deviation:list", "采购价偏离工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetPurchasePriceDeviationPlantOptionsAsync(
        [FromQuery] string? plantCode = null,
        [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchasePriceDeviationService.GetPurchasePriceDeviationPlantOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 查询栏：供应商去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:price:deviation:list", "采购价偏离供应商选项")]
    [HttpGet("supplier-options")]
    public async Task<IActionResult> GetPurchasePriceDeviationSupplierOptionsAsync(
        [FromQuery] string? plantCode = null,
        [FromQuery] string? keyword = null,
        [FromQuery] DateTime? periodDateStart = null,
        [FromQuery] DateTime? periodDateEnd = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode) || periodDateStart == null || periodDateEnd == null)
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _purchasePriceDeviationService.GetPurchasePriceDeviationSupplierOptionsAsync(
                plantCode, keyword, periodDateStart, periodDateEnd);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 查询栏：物料去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:price:deviation:list", "采购价偏离物料选项")]
    [HttpGet("material-options")]
    public async Task<IActionResult> GetPurchasePriceDeviationMaterialOptionsAsync(
        [FromQuery] string? plantCode = null,
        [FromQuery] string? keyword = null,
        [FromQuery] string? supplierCode = null,
        [FromQuery] DateTime? periodDateStart = null,
        [FromQuery] DateTime? periodDateEnd = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode) || periodDateStart == null || periodDateEnd == null)
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _purchasePriceDeviationService.GetPurchasePriceDeviationMaterialOptionsAsync(
                plantCode, keyword, supplierCode, periodDateStart, periodDateEnd);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// PO 价偏离 PCR 分析
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>偏离结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:deviation:list", "采购价偏离")]
    [HttpGet("deviation-analysis")]
    public async Task<IActionResult> GetPurchasePriceDeviationAnalysisAsync(
        [FromQuery] TaktPurchasePriceDeviationQueryDto queryDto)
    {
        try
        {
            var result = await _purchasePriceDeviationService.GetPurchasePriceDeviationAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 PO 价偏离 PCR 清单
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:procurement:purchase:price:deviation:export", "清单导出采购价偏离")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchasePriceDeviationAnalysisAsync(
        [FromQuery] TaktPurchasePriceDeviationQueryDto queryDto,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchasePriceDeviationService.ExportPurchasePriceDeviationAnalysisAsync(
                queryDto, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 查询栏：MIRO（物料凭证）工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:price:deviation:list", "MIRO价偏离工厂选项")]
    [HttpGet("miro/plant-options")]
    public async Task<IActionResult> GetMiroPricePcrDeviationPlantOptionsAsync(
        [FromQuery] string? plantCode = null,
        [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchasePriceDeviationService.GetMiroPricePcrDeviationPlantOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 查询栏：MIRO 供应商去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:price:deviation:list", "MIRO价偏离供应商选项")]
    [HttpGet("miro/supplier-options")]
    public async Task<IActionResult> GetMiroPricePcrDeviationSupplierOptionsAsync(
        [FromQuery] string? plantCode = null,
        [FromQuery] string? keyword = null,
        [FromQuery] DateTime? periodDateStart = null,
        [FromQuery] DateTime? periodDateEnd = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode) || periodDateStart == null || periodDateEnd == null)
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _purchasePriceDeviationService.GetMiroPricePcrDeviationSupplierOptionsAsync(
                plantCode, keyword, periodDateStart, periodDateEnd);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 查询栏：MIRO 物料去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:price:deviation:list", "MIRO价偏离物料选项")]
    [HttpGet("miro/material-options")]
    public async Task<IActionResult> GetMiroPricePcrDeviationMaterialOptionsAsync(
        [FromQuery] string? plantCode = null,
        [FromQuery] string? keyword = null,
        [FromQuery] string? supplierCode = null,
        [FromQuery] DateTime? periodDateStart = null,
        [FromQuery] DateTime? periodDateEnd = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode) || periodDateStart == null || periodDateEnd == null)
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _purchasePriceDeviationService.GetMiroPricePcrDeviationMaterialOptionsAsync(
                plantCode, keyword, supplierCode, periodDateStart, periodDateEnd);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// MIRO 价偏离 PCR 分析（物料凭证行本位币单价 vs 同周期采购条件）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>偏离结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:deviation:list", "MIRO价偏离")]
    [HttpGet("miro/deviation-analysis")]
    public async Task<IActionResult> GetMiroPricePcrDeviationAnalysisAsync(
        [FromQuery] TaktMiroPricePcrDeviationQueryDto queryDto)
    {
        try
        {
            var result = await _purchasePriceDeviationService.GetMiroPricePcrDeviationAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 MIRO 价偏离 PCR 清单
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:procurement:purchase:price:deviation:export", "清单导出MIRO价偏离")]
    [HttpGet("miro/export")]
    public async Task<IActionResult> ExportMiroPricePcrDeviationAnalysisAsync(
        [FromQuery] TaktMiroPricePcrDeviationQueryDto queryDto,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchasePriceDeviationService.ExportMiroPricePcrDeviationAnalysisAsync(
                queryDto, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
