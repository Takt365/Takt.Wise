// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.Logging
// 文件名称：TaktTrackingLogsController.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：交互日志控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Statistics.Logging;
using Takt.Application.Services.Statistics.Logging;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Statistics.Logging;

/// <summary>
/// 交互日志控制器
/// 提供交互日志的 REST API
/// </summary>
[ApiModule(9, "统计日志")]
[Route("api/[controller]", Name = "交互日志")]
public class TaktTrackingLogsController : TaktControllerBase
{
    private readonly ITaktTrackingLogService _trackingLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="trackingLogService">交互日志服务</param>
    public TaktTrackingLogsController(ITaktTrackingLogService trackingLogService)
    {
        _trackingLogService = trackingLogService;
    }

    /// <summary>
    /// 获取交互日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:logging:tracking:log:list", "交互日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTrackingLogListAsync([FromQuery] TaktTrackingLogQueryDto queryDto)
    {
        try
        {
            var result = await _trackingLogService.GetTrackingLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取交互日志
    /// </summary>
    /// <param name="id">交互日志ID</param>
    /// <returns>交互日志DTO</returns>
    [TaktPermission("statistics:logging:tracking:log:query", "交互日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTrackingLogByIdAsync(long id)
    {
        try
        {
            var result = await _trackingLogService.GetTrackingLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("交互日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取交互日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:logging:tracking:log:query", "交互日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTrackingLogOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _trackingLogService.GetTrackingLogOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建交互日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>交互日志DTO</returns>
    [TaktPermission("statistics:logging:tracking:log:create", "创建交互日志")]
    [HttpPost]
    public async Task<IActionResult> CreateTrackingLogAsync([FromBody] TaktTrackingLogCreateDto dto)
    {
        try
        {
            var result = await _trackingLogService.CreateTrackingLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新交互日志
    /// </summary>
    /// <param name="id">交互日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>交互日志DTO</returns>
    [TaktPermission("statistics:logging:tracking:log:update", "更新交互日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrackingLogAsync(long id, [FromBody] TaktTrackingLogUpdateDto dto)
    {
        try
        {
            var result = await _trackingLogService.UpdateTrackingLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除交互日志
    /// </summary>
    /// <param name="id">交互日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:tracking:log:delete", "删除交互日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrackingLogByIdAsync(long id)
    {
        try
        {
            await _trackingLogService.DeleteTrackingLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除交互日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:tracking:log:delete", "批量删除交互日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTrackingLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _trackingLogService.DeleteTrackingLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量上报 Long Task 客户端性能事件（仅需登录，不要求功能权限码）
    /// </summary>
    /// <param name="dto">批量上报 DTO</param>
    /// <returns>写入条数</returns>
    [HttpPost("track-batch")]
    public async Task<IActionResult> TrackTrackingLogBatchAsync([FromBody] TaktTrackingLogBatchTrackDto dto)
    {
        try
        {
            var clientIp = TaktLocationHelper.ResolveClientIp(HttpContext);
            var count = await _trackingLogService.TrackTrackingLogBatchAsync(dto, clientIp);
            return Success(new { count }, "上报成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出交互日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:logging:tracking:log:export", "导出交互日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTrackingLogAsync([FromQuery] TaktTrackingLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _trackingLogService.ExportTrackingLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
