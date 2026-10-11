// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.Logging
// 文件名称：TaktQuartzLogsController.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：任务执行日志控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Statistics.Logging;
using Takt.Application.Services.Statistics.Logging;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Statistics.Logging;

/// <summary>
/// 任务执行日志控制器
/// 提供任务执行日志的 REST API
/// </summary>
[ApiModule(9, "统计日志")]
[Route("api/[controller]", Name = "任务执行日志")]
public class TaktQuartzLogsController : TaktControllerBase
{
    private readonly ITaktQuartzLogService _quartzLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="quartzLogService">任务执行日志服务</param>
    public TaktQuartzLogsController(ITaktQuartzLogService quartzLogService)
    {
        _quartzLogService = quartzLogService;
    }

    /// <summary>
    /// 获取任务执行日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:logging:quartz:log:list", "任务执行日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetQuartzLogListAsync([FromQuery] TaktQuartzLogQueryDto queryDto)
    {
        try
        {
            var result = await _quartzLogService.GetQuartzLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取任务执行日志
    /// </summary>
    /// <param name="id">任务执行日志ID</param>
    /// <returns>任务执行日志DTO</returns>
    [TaktPermission("statistics:logging:quartz:log:query", "任务执行日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuartzLogByIdAsync(long id)
    {
        try
        {
            var result = await _quartzLogService.GetQuartzLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("任务执行日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取任务执行日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:logging:quartz:log:query", "任务执行日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetQuartzLogOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _quartzLogService.GetQuartzLogOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建任务执行日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>任务执行日志DTO</returns>
    [TaktPermission("statistics:logging:quartz:log:create", "创建任务执行日志")]
    [HttpPost]
    public async Task<IActionResult> CreateQuartzLogAsync([FromBody] TaktQuartzLogCreateDto dto)
    {
        try
        {
            var result = await _quartzLogService.CreateQuartzLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新任务执行日志
    /// </summary>
    /// <param name="id">任务执行日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>任务执行日志DTO</returns>
    [TaktPermission("statistics:logging:quartz:log:update", "更新任务执行日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuartzLogAsync(long id, [FromBody] TaktQuartzLogUpdateDto dto)
    {
        try
        {
            var result = await _quartzLogService.UpdateQuartzLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除任务执行日志
    /// </summary>
    /// <param name="id">任务执行日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:quartz:log:delete", "删除任务执行日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuartzLogByIdAsync(long id)
    {
        try
        {
            await _quartzLogService.DeleteQuartzLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除任务执行日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:quartz:log:delete", "批量删除任务执行日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteQuartzLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _quartzLogService.DeleteQuartzLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新任务执行日志状态
    /// </summary>
    /// <param name="dto">状态 DTO（TaktExecuteStatus 枚举）</param>
    /// <returns>任务执行日志DTO</returns>
    [TaktPermission("statistics:logging:quartz:log:update", "更新任务执行日志状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateQuartzLogStatusAsync([FromBody] TaktQuartzLogStatusDto dto)
    {
        try
        {
            var result = await _quartzLogService.UpdateQuartzLogStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出任务执行日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:logging:quartz:log:export", "导出任务执行日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportQuartzLogAsync([FromQuery] TaktQuartzLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _quartzLogService.ExportQuartzLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
