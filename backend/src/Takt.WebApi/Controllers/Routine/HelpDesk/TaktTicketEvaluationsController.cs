// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.HelpDesk
// 文件名称：TaktTicketEvaluationsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工单服务评价控制器
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
/// 工单服务评价控制器
/// 提供工单服务评价的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "工单服务评价")]
public class TaktTicketEvaluationsController : TaktControllerBase
{
    private readonly ITaktTicketEvaluationService _ticketEvaluationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketEvaluationService">工单服务评价服务</param>
    public TaktTicketEvaluationsController(ITaktTicketEvaluationService ticketEvaluationService)
    {
        _ticketEvaluationService = ticketEvaluationService;
    }

    /// <summary>
    /// 获取工单服务评价列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:help:desk:ticket:evaluation:list", "工单服务评价列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTicketEvaluationListAsync([FromQuery] TaktTicketEvaluationQueryDto queryDto)
    {
        try
        {
            var result = await _ticketEvaluationService.GetTicketEvaluationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工单服务评价
    /// </summary>
    /// <param name="id">工单服务评价ID</param>
    /// <returns>工单服务评价DTO</returns>
    [TaktPermission("routine:help:desk:ticket:evaluation:query", "工单服务评价详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketEvaluationByIdAsync(long id)
    {
        try
        {
            var result = await _ticketEvaluationService.GetTicketEvaluationByIdAsync(id);
            if (result == null)
            {
                return NotFound("工单服务评价不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工单服务评价选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:help:desk:ticket:evaluation:query", "工单服务评价选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTicketEvaluationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ticketEvaluationService.GetTicketEvaluationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工单服务评价
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工单服务评价DTO</returns>
    [TaktPermission("routine:help:desk:ticket:evaluation:create", "创建工单服务评价")]
    [HttpPost]
    public async Task<IActionResult> CreateTicketEvaluationAsync([FromBody] TaktTicketEvaluationCreateDto dto)
    {
        try
        {
            var result = await _ticketEvaluationService.CreateTicketEvaluationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工单服务评价
    /// </summary>
    /// <param name="id">工单服务评价ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工单服务评价DTO</returns>
    [TaktPermission("routine:help:desk:ticket:evaluation:update", "更新工单服务评价")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicketEvaluationAsync(long id, [FromBody] TaktTicketEvaluationUpdateDto dto)
    {
        try
        {
            var result = await _ticketEvaluationService.UpdateTicketEvaluationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工单服务评价
    /// </summary>
    /// <param name="id">工单服务评价ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:ticket:evaluation:delete", "删除工单服务评价")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicketEvaluationByIdAsync(long id)
    {
        try
        {
            await _ticketEvaluationService.DeleteTicketEvaluationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工单服务评价
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:ticket:evaluation:delete", "批量删除工单服务评价")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTicketEvaluationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ticketEvaluationService.DeleteTicketEvaluationBatchAsync(ids);
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
    [TaktPermission("routine:help:desk:ticket:evaluation:import", "获取工单服务评价导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTicketEvaluationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ticketEvaluationService.GetTicketEvaluationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工单服务评价
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:help:desk:ticket:evaluation:import", "导入工单服务评价")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTicketEvaluationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ticketEvaluationService.ImportTicketEvaluationAsync(stream, sheetName);
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
    /// 导出工单服务评价
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:help:desk:ticket:evaluation:export", "导出工单服务评价")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTicketEvaluationAsync([FromQuery] TaktTicketEvaluationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ticketEvaluationService.ExportTicketEvaluationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
