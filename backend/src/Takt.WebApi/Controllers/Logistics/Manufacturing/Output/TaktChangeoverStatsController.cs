// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktChangeoverStatsController.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：切换统计控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services.Logistics.Manufacturing.Output;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Output;

/// <summary>
/// 切换统计（按期间列出生产切换记录）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "切换统计")]
public class TaktChangeoverStatsController : TaktControllerBase
{
    private readonly ITaktChangeoverStatService _changeoverStatService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="changeoverStatService">切换统计服务</param>
    public TaktChangeoverStatsController(ITaktChangeoverStatService changeoverStatService)
    {
        _changeoverStatService = changeoverStatService;
    }

    /// <summary>
    /// 工厂选项
    /// </summary>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:stat:list", "切换统计工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetChangeoverStatPlantOptionsAsync([FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _changeoverStatService.GetChangeoverStatPlantOptionsAsync(keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 班组选项
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:stat:list", "切换统计班组选项")]
    [HttpGet("team-options")]
    public async Task<IActionResult> GetChangeoverStatTeamOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _changeoverStatService.GetChangeoverStatTeamOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 切换统计清单
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页行</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:stat:list", "切换统计")]
    [HttpGet("list")]
    public async Task<IActionResult> GetChangeoverStatListAsync([FromQuery] TaktChangeoverStatQueryDto queryDto)
    {
        try
        {
            var result = await _changeoverStatService.GetChangeoverStatListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出切换统计
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="exportName">文件名</param>
    /// <returns>Excel</returns>
    [TaktPermission("logistics:manufacturing:output:changeover:stat:export", "导出切换统计")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportChangeoverStatAsync(
        [FromQuery] TaktChangeoverStatQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _changeoverStatService.ExportChangeoverStatAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
