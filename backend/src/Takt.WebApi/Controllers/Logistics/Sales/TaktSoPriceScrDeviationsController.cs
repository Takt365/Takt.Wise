// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSoPriceScrDeviationsController.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SO 价偏离 SCR 分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Application.Services.Logistics.Sales;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Sales;

/// <summary>
/// SO 价偏离 SCR 分析控制器（与销售订单/销售条件 CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SO价偏离SCR")]
public class TaktSoPriceScrDeviationsController : TaktControllerBase
{
    private readonly ITaktSoPriceScrDeviationService _soPriceScrDeviationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="soPriceScrDeviationService">偏离分析服务</param>
    public TaktSoPriceScrDeviationsController(ITaktSoPriceScrDeviationService soPriceScrDeviationService)
    {
        _soPriceScrDeviationService = soPriceScrDeviationService;
    }

    /// <summary>
    /// 查询栏：销售订单工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:so:price:scr:deviation:list", "SO价偏离SCR工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetSoPriceScrDeviationPlantOptionsAsync(
        [FromQuery] string? plantCode = null,
        [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _soPriceScrDeviationService.GetSoPriceScrDeviationPlantOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 查询栏：客户去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:so:price:scr:deviation:list", "SO价偏离SCR客户选项")]
    [HttpGet("customer-options")]
    public async Task<IActionResult> GetSoPriceScrDeviationCustomerOptionsAsync(
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
            var result = await _soPriceScrDeviationService.GetSoPriceScrDeviationCustomerOptionsAsync(
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
    [TaktPermission("logistics:sales:so:price:scr:deviation:list", "SO价偏离SCR物料选项")]
    [HttpGet("material-options")]
    public async Task<IActionResult> GetSoPriceScrDeviationMaterialOptionsAsync(
        [FromQuery] string? plantCode = null,
        [FromQuery] string? keyword = null,
        [FromQuery] string? customerCode = null,
        [FromQuery] DateTime? periodDateStart = null,
        [FromQuery] DateTime? periodDateEnd = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode) || periodDateStart == null || periodDateEnd == null)
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _soPriceScrDeviationService.GetSoPriceScrDeviationMaterialOptionsAsync(
                plantCode, keyword, customerCode, periodDateStart, periodDateEnd);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// SO 价偏离 SCR 分析
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>偏离结果</returns>
    [TaktPermission("logistics:sales:so:price:scr:deviation:list", "SO价偏离SCR")]
    [HttpGet("deviation-analysis")]
    public async Task<IActionResult> GetSoPriceScrDeviationAnalysisAsync(
        [FromQuery] TaktSoPriceScrDeviationQueryDto queryDto)
    {
        try
        {
            var result = await _soPriceScrDeviationService.GetSoPriceScrDeviationAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 SO 价偏离 SCR 清单
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:sales:so:price:scr:deviation:export", "清单导出SO价偏离SCR")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSoPriceScrDeviationAnalysisAsync(
        [FromQuery] TaktSoPriceScrDeviationQueryDto queryDto,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _soPriceScrDeviationService.ExportSoPriceScrDeviationAnalysisAsync(
                queryDto, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
