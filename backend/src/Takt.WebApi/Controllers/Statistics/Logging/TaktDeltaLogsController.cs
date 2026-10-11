// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.Logging
// 文件名称：TaktDeltaLogsController.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：差异日志控制器
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
/// 差异日志控制器
/// 提供差异日志的 REST API
/// </summary>
[ApiModule(9, "统计日志")]
[Route("api/[controller]", Name = "差异日志")]
public class TaktDeltaLogsController : TaktControllerBase
{
    private readonly ITaktDeltaLogService _deltaLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="deltaLogService">差异日志服务</param>
    public TaktDeltaLogsController(ITaktDeltaLogService deltaLogService)
    {
        _deltaLogService = deltaLogService;
    }

    /// <summary>
    /// 获取差异日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:logging:delta:log:list", "差异日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetDeltaLogListAsync([FromQuery] TaktDeltaLogQueryDto queryDto)
    {
        try
        {
            var result = await _deltaLogService.GetDeltaLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取差异日志
    /// </summary>
    /// <param name="id">差异日志ID</param>
    /// <returns>差异日志DTO</returns>
    [TaktPermission("statistics:logging:delta:log:query", "差异日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDeltaLogByIdAsync(long id)
    {
        try
        {
            var result = await _deltaLogService.GetDeltaLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("差异日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取差异日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:logging:delta:log:query", "差异日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetDeltaLogOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _deltaLogService.GetDeltaLogOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建差异日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>差异日志DTO</returns>
    [TaktPermission("statistics:logging:delta:log:create", "创建差异日志")]
    [HttpPost]
    public async Task<IActionResult> CreateDeltaLogAsync([FromBody] TaktDeltaLogCreateDto dto)
    {
        try
        {
            var result = await _deltaLogService.CreateDeltaLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新差异日志
    /// </summary>
    /// <param name="id">差异日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>差异日志DTO</returns>
    [TaktPermission("statistics:logging:delta:log:update", "更新差异日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDeltaLogAsync(long id, [FromBody] TaktDeltaLogUpdateDto dto)
    {
        try
        {
            var result = await _deltaLogService.UpdateDeltaLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除差异日志
    /// </summary>
    /// <param name="id">差异日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:delta:log:delete", "删除差异日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDeltaLogByIdAsync(long id)
    {
        try
        {
            await _deltaLogService.DeleteDeltaLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除差异日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:delta:log:delete", "批量删除差异日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteDeltaLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _deltaLogService.DeleteDeltaLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出差异日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:logging:delta:log:export", "导出差异日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportDeltaLogAsync([FromQuery] TaktDeltaLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _deltaLogService.ExportDeltaLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
