// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktProductionTrendsController.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：生产推移控制器，按组立或 PCBA 机种汇总月份产量
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services.Logistics.Manufacturing.Output;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Output;

/// <summary>
/// 生产推移（组立 / PCBA，按机种）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "生产推移")]
public class TaktProductionTrendsController : TaktControllerBase
{
    private readonly ITaktProductionMonthlyTrendService _productionMonthlyTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionMonthlyTrendService">生产统计服务，页签限定为按机种</param>
    public TaktProductionTrendsController(ITaktProductionMonthlyTrendService productionMonthlyTrendService)
    {
        _productionMonthlyTrendService = productionMonthlyTrendService;
    }

    /// <summary>
    /// 当前页签产出表的工厂
    /// </summary>
    /// <param name="statTab">assy 或 pcba</param>
    /// <param name="keyword">关键字</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:output:production:trend:list", "生产推移工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetProductionTrendPlantOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var tab = MapModelTab(statTab);
            if (tab.Length == 0)
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _productionMonthlyTrendService.GetProductionMonthlyTrendPlantOptionsAsync(tab, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 当前页签产出表的机种
    /// </summary>
    /// <param name="statTab">assy 或 pcba</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:output:production:trend:list", "生产推移机种选项")]
    [HttpGet("model-options")]
    public async Task<IActionResult> GetProductionTrendModelOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var tab = MapModelTab(statTab);
            if (tab.Length == 0)
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _productionMonthlyTrendService.GetProductionMonthlyTrendModelOptionsAsync(tab, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 生产推移分析
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>按机种的月份产量</returns>
    [TaktPermission("logistics:manufacturing:output:production:trend:list", "生产推移")]
    [HttpGet("analysis")]
    public async Task<IActionResult> GetProductionTrendAnalysisAsync([FromQuery] TaktProductionMonthlyTrendQueryDto queryDto)
    {
        try
        {
            queryDto.StatTab = MapModelTab(queryDto.StatTab);
            var result = await _productionMonthlyTrendService.GetProductionMonthlyTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出生产推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="exportName">文件名</param>
    /// <returns>Excel</returns>
    [TaktPermission("logistics:manufacturing:output:production:trend:export", "清单导出生产推移")]
    [HttpGet("analysis/export")]
    public async Task<IActionResult> ExportProductionTrendAnalysisAsync(
        [FromQuery] TaktProductionMonthlyTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            query.StatTab = MapModelTab(query.StatTab);
            var (resultFileName, fileContent) = await _productionMonthlyTrendService.ExportProductionMonthlyTrendAnalysisAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 页签映射到按机种统计
    /// </summary>
    /// <param name="statTab">assy / pcba / assy-model / pcba-model</param>
    /// <returns>服务页签；无法识别时为空</returns>
    private static string MapModelTab(string? statTab) =>
        statTab?.Trim().ToLowerInvariant() switch
        {
            "assy" or "assy-model" => "assy-model",
            "pcba" or "pcba-model" => "pcba-model",
            _ => string.Empty,
        };
}
