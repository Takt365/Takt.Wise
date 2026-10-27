// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktQualityStatsController.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：质量统计控制器
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
/// 质量统计（进货统计、过程统计、完成品统计）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "质量统计")]
public class TaktQualityStatsController : TaktControllerBase
{
    private readonly ITaktQualityStatService _qualityStatService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityStatService">质量统计服务</param>
    public TaktQualityStatsController(ITaktQualityStatService qualityStatService)
    {
        _qualityStatService = qualityStatService;
    }

    /// <summary>
    /// 工厂选项
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:quality:operation:stat:list", "质量统计工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetQualityStatPlantOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _qualityStatService.GetQualityStatPlantOptionsAsync(statTab, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 维度选项
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:quality:operation:stat:list", "质量统计维度选项")]
    [HttpGet("dimension-options")]
    public async Task<IActionResult> GetQualityStatDimensionOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _qualityStatService.GetQualityStatDimensionOptionsAsync(statTab, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 质量统计列表
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页行</returns>
    [TaktPermission("logistics:quality:operation:stat:list", "质量统计")]
    [HttpGet("list")]
    public async Task<IActionResult> GetQualityStatListAsync([FromQuery] TaktQualityOperationStatQueryDto queryDto)
    {
        try
        {
            var result = await _qualityStatService.GetQualityStatListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出质量统计
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表</param>
    /// <param name="exportName">文件名</param>
    /// <returns>Excel</returns>
    [TaktPermission("logistics:quality:operation:stat:export", "清单导出质量统计")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportQualityStatAsync(
        [FromQuery] TaktQualityOperationStatQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (fileName, fileContent) = await _qualityStatService.ExportQualityStatAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, fileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
