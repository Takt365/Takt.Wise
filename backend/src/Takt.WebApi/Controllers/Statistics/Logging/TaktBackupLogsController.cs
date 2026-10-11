// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.Logging
// 文件名称：TaktBackupLogsController.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：备份日志控制器
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
/// 备份日志控制器
/// 提供备份日志的 REST API
/// </summary>
[ApiModule(9, "统计日志")]
[Route("api/[controller]", Name = "备份日志")]
public class TaktBackupLogsController : TaktControllerBase
{
    private readonly ITaktBackupLogService _backupLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="backupLogService">备份日志服务</param>
    public TaktBackupLogsController(ITaktBackupLogService backupLogService)
    {
        _backupLogService = backupLogService;
    }

    /// <summary>
    /// 获取备份日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:logging:backup:log:list", "备份日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBackupLogListAsync([FromQuery] TaktBackupLogQueryDto queryDto)
    {
        try
        {
            var result = await _backupLogService.GetBackupLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取备份日志
    /// </summary>
    /// <param name="id">备份日志ID</param>
    /// <returns>备份日志DTO</returns>
    [TaktPermission("statistics:logging:backup:log:query", "备份日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBackupLogByIdAsync(long id)
    {
        try
        {
            var result = await _backupLogService.GetBackupLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("备份日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取备份日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:logging:backup:log:query", "备份日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBackupLogOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _backupLogService.GetBackupLogOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建备份日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>备份日志DTO</returns>
    [TaktPermission("statistics:logging:backup:log:create", "创建备份日志")]
    [HttpPost]
    public async Task<IActionResult> CreateBackupLogAsync([FromBody] TaktBackupLogCreateDto dto)
    {
        try
        {
            var result = await _backupLogService.CreateBackupLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新备份日志
    /// </summary>
    /// <param name="id">备份日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>备份日志DTO</returns>
    [TaktPermission("statistics:logging:backup:log:update", "更新备份日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBackupLogAsync(long id, [FromBody] TaktBackupLogUpdateDto dto)
    {
        try
        {
            var result = await _backupLogService.UpdateBackupLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除备份日志
    /// </summary>
    /// <param name="id">备份日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:backup:log:delete", "删除备份日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBackupLogByIdAsync(long id)
    {
        try
        {
            await _backupLogService.DeleteBackupLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除备份日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:backup:log:delete", "批量删除备份日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBackupLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _backupLogService.DeleteBackupLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新备份日志状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>备份日志DTO</returns>
    [TaktPermission("statistics:logging:backup:log:update", "更新备份日志状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateBackupLogStatusAsync([FromBody] TaktBackupLogStatusDto dto)
    {
        try
        {
            var result = await _backupLogService.UpdateBackupLogStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出备份日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:logging:backup:log:export", "导出备份日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBackupLogAsync([FromQuery] TaktBackupLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _backupLogService.ExportBackupLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
