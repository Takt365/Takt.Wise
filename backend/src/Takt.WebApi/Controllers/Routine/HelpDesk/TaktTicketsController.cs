// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.HelpDesk
// 文件名称：TaktTicketsController.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：工单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Application.Services.Routine.HelpDesk;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.HelpDesk;

/// <summary>
/// 工单控制器
/// 提供工单的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "工单")]
public class TaktTicketsController : TaktControllerBase
{
    private readonly ITaktTicketService _ticketService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketService">工单服务</param>
    public TaktTicketsController(ITaktTicketService ticketService)
    {
        _ticketService = ticketService;
    }

    /// <summary>
    /// 获取工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:help:desk:ticket:list", "工单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTicketListAsync([FromQuery] TaktTicketQueryDto queryDto)
    {
        try
        {
            var result = await _ticketService.GetTicketListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取服务台工单件数统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>工单件数统计</returns>
    [TaktPermission("routine:help:desk:ticket:list", "服务台工单件数统计")]
    [HttpGet("ticket-stat")]
    public async Task<IActionResult> GetTicketStatAsync([FromQuery] TaktTicketStatQueryDto queryDto)
    {
        try
        {
            var result = await _ticketService.GetTicketStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:query", "工单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketByIdAsync(long id)
    {
        try
        {
            var result = await _ticketService.GetTicketByIdAsync(id);
            if (result == null)
            {
                return NotFound("工单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:help:desk:ticket:query", "工单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTicketOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ticketService.GetTicketOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:create", "创建工单")]
    [HttpPost]
    public async Task<IActionResult> CreateTicketAsync([FromBody] TaktTicketCreateDto dto)
    {
        try
        {
            var result = await _ticketService.CreateTicketAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:update", "更新工单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicketAsync(long id, [FromBody] TaktTicketUpdateDto dto)
    {
        try
        {
            var result = await _ticketService.UpdateTicketAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:ticket:delete", "删除工单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicketByIdAsync(long id)
    {
        try
        {
            await _ticketService.DeleteTicketByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:ticket:delete", "批量删除工单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTicketBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ticketService.DeleteTicketBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:update", "更新工单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTicketStatusAsync([FromBody] TaktTicketStatusDto dto)
    {
        try
        {
            var result = await _ticketService.UpdateTicketStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:help:desk:ticket:import", "获取工单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTicketTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ticketService.GetTicketTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:help:desk:ticket:import", "导入工单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTicketAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ticketService.ImportTicketAsync(stream, sheetName);
            return Success(new
            {
                SuccessCount = success,
                FailCount = fail,
                Errors = errors
            }, $"导入完成：成功{success}条，失败{fail}条");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出工单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:help:desk:ticket:export", "导出工单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTicketAsync([FromQuery] TaktTicketQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ticketService.ExportTicketAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
