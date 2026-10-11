// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPoPricePirDeviationsController.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：PO 价偏离 PIR 分析控制器
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
/// PO 价偏离 PIR 分析控制器（与采购订单/采购价格 CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "PO价偏离PIR")]
public class TaktPoPricePirDeviationsController : TaktControllerBase
{
    private readonly ITaktPoPricePirDeviationService _poPricePirDeviationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="poPricePirDeviationService">偏离分析服务</param>
    public TaktPoPricePirDeviationsController(ITaktPoPricePirDeviationService poPricePirDeviationService)
    {
        _poPricePirDeviationService = poPricePirDeviationService;
    }

    /// <summary>
    /// 查询栏：采购订单工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:po:price:pir:deviation:list", "PO价偏离PIR工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetPoPricePirDeviationPlantOptionsAsync(
        [FromQuery] string? plantCode = null,
        [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _poPricePirDeviationService.GetPoPricePirDeviationPlantOptionsAsync(plantCode, keyword);
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
    [TaktPermission("logistics:procurement:po:price:pir:deviation:list", "PO价偏离PIR供应商选项")]
    [HttpGet("supplier-options")]
    public async Task<IActionResult> GetPoPricePirDeviationSupplierOptionsAsync(
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
            var result = await _poPricePirDeviationService.GetPoPricePirDeviationSupplierOptionsAsync(
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
    [TaktPermission("logistics:procurement:po:price:pir:deviation:list", "PO价偏离PIR物料选项")]
    [HttpGet("material-options")]
    public async Task<IActionResult> GetPoPricePirDeviationMaterialOptionsAsync(
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
            var result = await _poPricePirDeviationService.GetPoPricePirDeviationMaterialOptionsAsync(
                plantCode, keyword, supplierCode, periodDateStart, periodDateEnd);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// PO 价偏离 PIR 分析
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>偏离结果</returns>
    [TaktPermission("logistics:procurement:po:price:pir:deviation:list", "PO价偏离PIR")]
    [HttpGet("deviation-analysis")]
    public async Task<IActionResult> GetPoPricePirDeviationAnalysisAsync(
        [FromQuery] TaktPoPricePirDeviationQueryDto queryDto)
    {
        try
        {
            var result = await _poPricePirDeviationService.GetPoPricePirDeviationAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 PO 价偏离 PIR 清单
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:procurement:po:price:pir:deviation:export", "清单导出PO价偏离PIR")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPoPricePirDeviationAnalysisAsync(
        [FromQuery] TaktPoPricePirDeviationQueryDto queryDto,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _poPricePirDeviationService.ExportPoPricePirDeviationAnalysisAsync(
                queryDto, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
