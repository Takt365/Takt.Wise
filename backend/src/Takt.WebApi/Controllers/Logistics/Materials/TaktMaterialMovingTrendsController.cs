// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialMovingTrendsController.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料移动价格/机种推移分析核心转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 物料移动价格推移转置分析控制器（与 TaktMaterialMovingPrices CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料移动价格推移")]
public class TaktMaterialMovingTrendsController : TaktControllerBase
{
    private readonly ITaktMaterialMovingTrendService _materialMovingTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialMovingTrendService">物料移动价格推移服务</param>
    public TaktMaterialMovingTrendsController(ITaktMaterialMovingTrendService materialMovingTrendService)
    {
        _materialMovingTrendService = materialMovingTrendService;
    }

    /// <summary>
    /// 推移查询栏：移动价格本表工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:moving:trend:list", "物料移动价格推移工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetMaterialMovingTrendPlantOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _materialMovingTrendService.GetMaterialMovingTrendPlantOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂去重评估类别
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:moving:trend:list", "物料移动价格推移评估类别选项")]
    [HttpGet("valuation-options")]
    public async Task<IActionResult> GetMaterialMovingTrendValuationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _materialMovingTrendService.GetMaterialMovingTrendValuationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂去重物料；评估类别可空，有值时再收窄
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="valuation">评估类别（可空）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:moving:trend:list", "物料移动价格推移物料选项")]
    [HttpGet("material-options")]
    public async Task<IActionResult> GetMaterialMovingTrendMaterialOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? valuation = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _materialMovingTrendService.GetMaterialMovingTrendMaterialOptionsAsync(plantCode, keyword, valuation);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 物料月移动价格推移分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    [TaktPermission("logistics:materials:material:moving:trend:list", "物料移动价格推移")]
    [HttpGet("trend-analysis")]
    public async Task<IActionResult> GetMaterialMovingTrendAnalysisAsync(
        [FromQuery] TaktMaterialMovingTrendQueryDto queryDto)
    {
        try
        {
            var result = await _materialMovingTrendService.GetMaterialMovingTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 清单导出：物料月移动价格推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:materials:material:moving:trend:export", "清单导出物料移动价格推移")]
    [HttpGet("trend-analysis/export")]
    public async Task<IActionResult> ExportMaterialMovingTrendAnalysisAsync(
        [FromQuery] TaktMaterialMovingTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialMovingTrendService.ExportMaterialMovingTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
