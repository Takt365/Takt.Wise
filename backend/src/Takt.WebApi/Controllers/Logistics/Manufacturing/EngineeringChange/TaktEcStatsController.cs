// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcStatsController.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：设变统计控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变统计（各执行部门；全部、已实施、未实施）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变统计")]
public class TaktEcStatsController : TaktControllerBase
{
    private readonly ITaktEcStatService _ecStatService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecStatService">设变统计服务</param>
    public TaktEcStatsController(ITaktEcStatService ecStatService)
    {
        _ecStatService = ecStatService;
    }

    /// <summary>
    /// 工厂选项
    /// </summary>
    /// <param name="statTab">部门页签</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:stat:list", "设变统计工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetEcStatPlantOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ecStatService.GetEcStatPlantOptionsAsync(statTab, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 设变单号选项
    /// </summary>
    /// <param name="statTab">部门页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:stat:list", "设变统计设变选项")]
    [HttpGet("ec-options")]
    public async Task<IActionResult> GetEcStatEcOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ecStatService.GetEcStatEcOptionsAsync(statTab, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 设变统计列表
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页行</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:stat:list", "设变统计")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcStatListAsync([FromQuery] TaktEcDeptReportQueryDto queryDto)
    {
        try
        {
            var result = await _ecStatService.GetEcStatListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出设变统计
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表</param>
    /// <param name="exportName">文件名</param>
    /// <returns>Excel</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:stat:export", "清单导出设变统计")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcStatAsync(
        [FromQuery] TaktEcDeptReportQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (fileName, fileContent) = await _ecStatService.ExportEcStatAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, fileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
