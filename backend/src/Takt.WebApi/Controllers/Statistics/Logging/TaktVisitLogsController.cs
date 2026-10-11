// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.Logging
// 文件名称：TaktVisitLogsController.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：用户日访问量控制器
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
/// 用户日访问量控制器
/// 提供用户日访问量的 REST API
/// </summary>
[ApiModule(9, "统计日志")]
[Route("api/[controller]", Name = "用户日访问量")]
public class TaktVisitLogsController : TaktControllerBase
{
    private readonly ITaktVisitLogService _visitLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="visitLogService">用户日访问量服务</param>
    public TaktVisitLogsController(ITaktVisitLogService visitLogService)
    {
        _visitLogService = visitLogService;
    }

    /// <summary>
    /// 获取用户日访问量列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:logging:visit:log:list", "用户日访问量列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetVisitLogListAsync([FromQuery] TaktVisitLogQueryDto queryDto)
    {
        try
        {
            var result = await _visitLogService.GetVisitLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取用户日访问量
    /// </summary>
    /// <param name="id">用户日访问量ID</param>
    /// <returns>用户日访问量DTO</returns>
    [TaktPermission("statistics:logging:visit:log:query", "用户日访问量详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVisitLogByIdAsync(long id)
    {
        try
        {
            var result = await _visitLogService.GetVisitLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("用户日访问量不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取用户日访问量选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:logging:visit:log:query", "用户日访问量选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetVisitLogOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _visitLogService.GetVisitLogOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建用户日访问量
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>用户日访问量DTO</returns>
    [TaktPermission("statistics:logging:visit:log:create", "创建用户日访问量")]
    [HttpPost]
    public async Task<IActionResult> CreateVisitLogAsync([FromBody] TaktVisitLogCreateDto dto)
    {
        try
        {
            var result = await _visitLogService.CreateVisitLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新用户日访问量
    /// </summary>
    /// <param name="id">用户日访问量ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>用户日访问量DTO</returns>
    [TaktPermission("statistics:logging:visit:log:update", "更新用户日访问量")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVisitLogAsync(long id, [FromBody] TaktVisitLogUpdateDto dto)
    {
        try
        {
            var result = await _visitLogService.UpdateVisitLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除用户日访问量
    /// </summary>
    /// <param name="id">用户日访问量ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:visit:log:delete", "删除用户日访问量")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVisitLogByIdAsync(long id)
    {
        try
        {
            await _visitLogService.DeleteVisitLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除用户日访问量
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:visit:log:delete", "批量删除用户日访问量")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteVisitLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _visitLogService.DeleteVisitLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出用户日访问量
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:logging:visit:log:export", "导出用户日访问量")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportVisitLogAsync([FromQuery] TaktVisitLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _visitLogService.ExportVisitLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
