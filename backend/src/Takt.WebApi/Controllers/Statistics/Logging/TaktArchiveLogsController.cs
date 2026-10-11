// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.Logging
// 文件名称：TaktArchiveLogsController.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：归档日志控制器
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
/// 归档日志控制器
/// 提供归档日志的 REST API
/// </summary>
[ApiModule(9, "统计日志")]
[Route("api/[controller]", Name = "归档日志")]
public class TaktArchiveLogsController : TaktControllerBase
{
    private readonly ITaktArchiveLogService _archiveLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="archiveLogService">归档日志服务</param>
    public TaktArchiveLogsController(ITaktArchiveLogService archiveLogService)
    {
        _archiveLogService = archiveLogService;
    }

    /// <summary>
    /// 获取归档日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:logging:archive:log:list", "归档日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetArchiveLogListAsync([FromQuery] TaktArchiveLogQueryDto queryDto)
    {
        try
        {
            var result = await _archiveLogService.GetArchiveLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取归档日志
    /// </summary>
    /// <param name="id">归档日志ID</param>
    /// <returns>归档日志DTO</returns>
    [TaktPermission("statistics:logging:archive:log:query", "归档日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetArchiveLogByIdAsync(long id)
    {
        try
        {
            var result = await _archiveLogService.GetArchiveLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("归档日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取归档日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:logging:archive:log:query", "归档日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetArchiveLogOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _archiveLogService.GetArchiveLogOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建归档日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>归档日志DTO</returns>
    [TaktPermission("statistics:logging:archive:log:create", "创建归档日志")]
    [HttpPost]
    public async Task<IActionResult> CreateArchiveLogAsync([FromBody] TaktArchiveLogCreateDto dto)
    {
        try
        {
            var result = await _archiveLogService.CreateArchiveLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新归档日志
    /// </summary>
    /// <param name="id">归档日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>归档日志DTO</returns>
    [TaktPermission("statistics:logging:archive:log:update", "更新归档日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArchiveLogAsync(long id, [FromBody] TaktArchiveLogUpdateDto dto)
    {
        try
        {
            var result = await _archiveLogService.UpdateArchiveLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除归档日志
    /// </summary>
    /// <param name="id">归档日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:archive:log:delete", "删除归档日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArchiveLogByIdAsync(long id)
    {
        try
        {
            await _archiveLogService.DeleteArchiveLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除归档日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:archive:log:delete", "批量删除归档日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteArchiveLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _archiveLogService.DeleteArchiveLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新归档日志状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>归档日志DTO</returns>
    [TaktPermission("statistics:logging:archive:log:update", "更新归档日志状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateArchiveLogStatusAsync([FromBody] TaktArchiveLogStatusDto dto)
    {
        try
        {
            var result = await _archiveLogService.UpdateArchiveLogStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出归档日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:logging:archive:log:export", "导出归档日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportArchiveLogAsync([FromQuery] TaktArchiveLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _archiveLogService.ExportArchiveLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
