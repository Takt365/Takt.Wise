// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.Logging
// 文件名称：TaktDurationLogsController.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线时长日志控制器
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
/// 在线时长日志控制器
/// 提供在线时长日志的 REST API
/// </summary>
[ApiModule(9, "统计日志")]
[Route("api/[controller]", Name = "在线时长日志")]
public class TaktDurationLogsController : TaktControllerBase
{
    private readonly ITaktDurationLogService _durationLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="durationLogService">在线时长日志服务</param>
    public TaktDurationLogsController(ITaktDurationLogService durationLogService)
    {
        _durationLogService = durationLogService;
    }

    /// <summary>
    /// 获取在线时长日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:logging:duration:log:list", "在线时长日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetDurationLogListAsync([FromQuery] TaktDurationLogQueryDto queryDto)
    {
        try
        {
            var result = await _durationLogService.GetDurationLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取在线时长日志
    /// </summary>
    /// <param name="id">在线时长日志ID</param>
    /// <returns>在线时长日志DTO</returns>
    [TaktPermission("statistics:logging:duration:log:query", "在线时长日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDurationLogByIdAsync(long id)
    {
        try
        {
            var result = await _durationLogService.GetDurationLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("在线时长日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取在线时长日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:logging:duration:log:query", "在线时长日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetDurationLogOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _durationLogService.GetDurationLogOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建在线时长日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>在线时长日志DTO</returns>
    [TaktPermission("statistics:logging:duration:log:create", "创建在线时长日志")]
    [HttpPost]
    public async Task<IActionResult> CreateDurationLogAsync([FromBody] TaktDurationLogCreateDto dto)
    {
        try
        {
            var result = await _durationLogService.CreateDurationLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新在线时长日志
    /// </summary>
    /// <param name="id">在线时长日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>在线时长日志DTO</returns>
    [TaktPermission("statistics:logging:duration:log:update", "更新在线时长日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDurationLogAsync(long id, [FromBody] TaktDurationLogUpdateDto dto)
    {
        try
        {
            var result = await _durationLogService.UpdateDurationLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除在线时长日志
    /// </summary>
    /// <param name="id">在线时长日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:duration:log:delete", "删除在线时长日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDurationLogByIdAsync(long id)
    {
        try
        {
            await _durationLogService.DeleteDurationLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除在线时长日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:duration:log:delete", "批量删除在线时长日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteDurationLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _durationLogService.DeleteDurationLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出在线时长日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:logging:duration:log:export", "导出在线时长日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportDurationLogAsync([FromQuery] TaktDurationLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _durationLogService.ExportDurationLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
