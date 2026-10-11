// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.HelpDesk
// 文件名称：TaktTicketRepliesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工单回复控制器
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
/// 工单回复控制器
/// 提供工单回复的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "工单回复")]
public class TaktTicketRepliesController : TaktControllerBase
{
    private readonly ITaktTicketReplyService _ticketReplyService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketReplyService">工单回复服务</param>
    public TaktTicketRepliesController(ITaktTicketReplyService ticketReplyService)
    {
        _ticketReplyService = ticketReplyService;
    }

    /// <summary>
    /// 获取工单回复列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:help:desk:ticket:reply:list", "工单回复列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTicketReplyListAsync([FromQuery] TaktTicketReplyQueryDto queryDto)
    {
        try
        {
            var result = await _ticketReplyService.GetTicketReplyListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工单回复
    /// </summary>
    /// <param name="id">工单回复ID</param>
    /// <returns>工单回复DTO</returns>
    [TaktPermission("routine:help:desk:ticket:reply:query", "工单回复详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketReplyByIdAsync(long id)
    {
        try
        {
            var result = await _ticketReplyService.GetTicketReplyByIdAsync(id);
            if (result == null)
            {
                return NotFound("工单回复不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工单回复选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:help:desk:ticket:reply:query", "工单回复选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTicketReplyOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ticketReplyService.GetTicketReplyOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工单回复
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工单回复DTO</returns>
    [TaktPermission("routine:help:desk:ticket:reply:create", "创建工单回复")]
    [HttpPost]
    public async Task<IActionResult> CreateTicketReplyAsync([FromBody] TaktTicketReplyCreateDto dto)
    {
        try
        {
            var result = await _ticketReplyService.CreateTicketReplyAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工单回复
    /// </summary>
    /// <param name="id">工单回复ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工单回复DTO</returns>
    [TaktPermission("routine:help:desk:ticket:reply:update", "更新工单回复")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicketReplyAsync(long id, [FromBody] TaktTicketReplyUpdateDto dto)
    {
        try
        {
            var result = await _ticketReplyService.UpdateTicketReplyAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工单回复
    /// </summary>
    /// <param name="id">工单回复ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:ticket:reply:delete", "删除工单回复")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicketReplyByIdAsync(long id)
    {
        try
        {
            await _ticketReplyService.DeleteTicketReplyByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工单回复
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:ticket:reply:delete", "批量删除工单回复")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTicketReplyBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ticketReplyService.DeleteTicketReplyBatchAsync(ids);
            return Success("删除成功");
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
    [TaktPermission("routine:help:desk:ticket:reply:import", "获取工单回复导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTicketReplyTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ticketReplyService.GetTicketReplyTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工单回复
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:help:desk:ticket:reply:import", "导入工单回复")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTicketReplyAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ticketReplyService.ImportTicketReplyAsync(stream, sheetName);
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
    /// 导出工单回复
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:help:desk:ticket:reply:export", "导出工单回复")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTicketReplyAsync([FromQuery] TaktTicketReplyQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ticketReplyService.ExportTicketReplyAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
