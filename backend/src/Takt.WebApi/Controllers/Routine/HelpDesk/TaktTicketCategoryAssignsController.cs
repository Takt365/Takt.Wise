// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.HelpDesk
// 文件名称：TaktTicketCategoryAssignsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工单分类默认处理人控制器
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
/// 工单分类默认处理人控制器
/// 提供工单分类默认处理人的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "工单分类默认处理人")]
public class TaktTicketCategoryAssignsController : TaktControllerBase
{
    private readonly ITaktTicketCategoryAssignService _ticketCategoryAssignService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketCategoryAssignService">工单分类默认处理人服务</param>
    public TaktTicketCategoryAssignsController(ITaktTicketCategoryAssignService ticketCategoryAssignService)
    {
        _ticketCategoryAssignService = ticketCategoryAssignService;
    }

    /// <summary>
    /// 获取工单分类默认处理人列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:help:desk:ticket:category:assign:list", "工单分类默认处理人列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTicketCategoryAssignListAsync([FromQuery] TaktTicketCategoryAssignQueryDto queryDto)
    {
        try
        {
            var result = await _ticketCategoryAssignService.GetTicketCategoryAssignListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工单分类默认处理人
    /// </summary>
    /// <param name="id">工单分类默认处理人ID</param>
    /// <returns>工单分类默认处理人DTO</returns>
    [TaktPermission("routine:help:desk:ticket:category:assign:query", "工单分类默认处理人详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketCategoryAssignByIdAsync(long id)
    {
        try
        {
            var result = await _ticketCategoryAssignService.GetTicketCategoryAssignByIdAsync(id);
            if (result == null)
            {
                return NotFound("工单分类默认处理人不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工单分类默认处理人选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:help:desk:ticket:category:assign:query", "工单分类默认处理人选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTicketCategoryAssignOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ticketCategoryAssignService.GetTicketCategoryAssignOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工单分类默认处理人
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工单分类默认处理人DTO</returns>
    [TaktPermission("routine:help:desk:ticket:category:assign:create", "创建工单分类默认处理人")]
    [HttpPost]
    public async Task<IActionResult> CreateTicketCategoryAssignAsync([FromBody] TaktTicketCategoryAssignCreateDto dto)
    {
        try
        {
            var result = await _ticketCategoryAssignService.CreateTicketCategoryAssignAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工单分类默认处理人
    /// </summary>
    /// <param name="id">工单分类默认处理人ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工单分类默认处理人DTO</returns>
    [TaktPermission("routine:help:desk:ticket:category:assign:update", "更新工单分类默认处理人")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicketCategoryAssignAsync(long id, [FromBody] TaktTicketCategoryAssignUpdateDto dto)
    {
        try
        {
            var result = await _ticketCategoryAssignService.UpdateTicketCategoryAssignAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工单分类默认处理人
    /// </summary>
    /// <param name="id">工单分类默认处理人ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:ticket:category:assign:delete", "删除工单分类默认处理人")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicketCategoryAssignByIdAsync(long id)
    {
        try
        {
            await _ticketCategoryAssignService.DeleteTicketCategoryAssignByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工单分类默认处理人
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:ticket:category:assign:delete", "批量删除工单分类默认处理人")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTicketCategoryAssignBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ticketCategoryAssignService.DeleteTicketCategoryAssignBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工单分类默认处理人排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>工单分类默认处理人DTO</returns>
    [TaktPermission("routine:help:desk:ticket:category:assign:update", "更新工单分类默认处理人排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateTicketCategoryAssignSortAsync([FromBody] TaktTicketCategoryAssignSortDto dto)
    {
        try
        {
            var result = await _ticketCategoryAssignService.UpdateTicketCategoryAssignSortAsync(dto);
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
    [TaktPermission("routine:help:desk:ticket:category:assign:import", "获取工单分类默认处理人导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTicketCategoryAssignTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ticketCategoryAssignService.GetTicketCategoryAssignTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工单分类默认处理人
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:help:desk:ticket:category:assign:import", "导入工单分类默认处理人")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTicketCategoryAssignAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ticketCategoryAssignService.ImportTicketCategoryAssignAsync(stream, sheetName);
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
    /// 导出工单分类默认处理人
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:help:desk:ticket:category:assign:export", "导出工单分类默认处理人")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTicketCategoryAssignAsync([FromQuery] TaktTicketCategoryAssignQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ticketCategoryAssignService.ExportTicketCategoryAssignAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
