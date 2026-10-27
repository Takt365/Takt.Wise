// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Defect
// 文件名称：TaktDefectStatsController.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：不良统计控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Application.Services.Logistics.Manufacturing.Defect;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Defect;

/// <summary>
/// 不良统计（组立按工单、组立按批次、PCBA按工单、PCBA按批次）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "不良统计")]
public class TaktDefectStatsController : TaktControllerBase
{
    private readonly ITaktDefectStatService _defectStatService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="defectStatService">不良统计服务</param>
    public TaktDefectStatsController(ITaktDefectStatService defectStatService)
    {
        _defectStatService = defectStatService;
    }

    /// <summary>
    /// 工厂选项
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:manufacturing:defect:stat:list", "不良统计工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetDefectStatPlantOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _defectStatService.GetDefectStatPlantOptionsAsync(statTab, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 工单选项
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:manufacturing:defect:stat:list", "不良统计工单选项")]
    [HttpGet("order-options")]
    public async Task<IActionResult> GetDefectStatOrderOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _defectStatService.GetDefectStatOrderOptionsAsync(statTab, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批次选项
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:manufacturing:defect:stat:list", "不良统计批次选项")]
    [HttpGet("batch-options")]
    public async Task<IActionResult> GetDefectStatBatchOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _defectStatService.GetDefectStatBatchOptionsAsync(statTab, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 机种选项
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:manufacturing:defect:stat:list", "不良统计机种选项")]
    [HttpGet("model-options")]
    public async Task<IActionResult> GetDefectStatModelOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _defectStatService.GetDefectStatModelOptionsAsync(statTab, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 物料选项
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:manufacturing:defect:stat:list", "不良统计物料选项")]
    [HttpGet("material-options")]
    public async Task<IActionResult> GetDefectStatMaterialOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _defectStatService.GetDefectStatMaterialOptionsAsync(statTab, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 不良统计列表
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页行</returns>
    [TaktPermission("logistics:manufacturing:defect:stat:list", "不良统计")]
    [HttpGet("list")]
    public async Task<IActionResult> GetDefectStatListAsync([FromQuery] TaktDefectOrderStatQueryDto queryDto)
    {
        try
        {
            var result = await _defectStatService.GetDefectStatListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出不良统计
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表</param>
    /// <param name="exportName">文件名</param>
    /// <returns>Excel</returns>
    [TaktPermission("logistics:manufacturing:defect:stat:export", "清单导出不良统计")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportDefectStatAsync(
        [FromQuery] TaktDefectOrderStatQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (fileName, fileContent) = await _defectStatService.ExportDefectStatAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, fileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
