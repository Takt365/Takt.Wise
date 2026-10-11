// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Attendance
// 文件名称：TaktCalendarsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂日历控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Attendance;
using Takt.Application.Services.HumanResource.Attendance;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Attendance;

/// <summary>
/// 工厂日历控制器
/// 提供工厂日历的 REST API
/// </summary>
[ApiModule(5, "考勤管理")]
[Route("api/[controller]", Name = "工厂日历")]
public class TaktCalendarsController : TaktControllerBase
{
    private readonly ITaktCalendarService _calendarService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="calendarService">工厂日历服务</param>
    public TaktCalendarsController(ITaktCalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    /// <summary>
    /// 获取工厂日历列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:attendance:calendar:list", "工厂日历列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCalendarListAsync([FromQuery] TaktCalendarQueryDto queryDto)
    {
        try
        {
            var result = await _calendarService.GetCalendarListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工厂日历
    /// </summary>
    /// <param name="id">工厂日历ID</param>
    /// <returns>工厂日历DTO</returns>
    [TaktPermission("human:resource:attendance:calendar:query", "工厂日历详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCalendarByIdAsync(long id)
    {
        try
        {
            var result = await _calendarService.GetCalendarByIdAsync(id);
            if (result == null)
            {
                return NotFound("工厂日历不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工厂日历选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:attendance:calendar:query", "工厂日历选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCalendarOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _calendarService.GetCalendarOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工厂日历
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工厂日历DTO</returns>
    [TaktPermission("human:resource:attendance:calendar:create", "创建工厂日历")]
    [HttpPost]
    public async Task<IActionResult> CreateCalendarAsync([FromBody] TaktCalendarCreateDto dto)
    {
        try
        {
            var result = await _calendarService.CreateCalendarAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工厂日历
    /// </summary>
    /// <param name="id">工厂日历ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工厂日历DTO</returns>
    [TaktPermission("human:resource:attendance:calendar:update", "更新工厂日历")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCalendarAsync(long id, [FromBody] TaktCalendarUpdateDto dto)
    {
        try
        {
            var result = await _calendarService.UpdateCalendarAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工厂日历
    /// </summary>
    /// <param name="id">工厂日历ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:calendar:delete", "删除工厂日历")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCalendarByIdAsync(long id)
    {
        try
        {
            await _calendarService.DeleteCalendarByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工厂日历
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:calendar:delete", "批量删除工厂日历")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCalendarBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _calendarService.DeleteCalendarBatchAsync(ids);
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
    [TaktPermission("human:resource:attendance:calendar:import", "获取工厂日历导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCalendarTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _calendarService.GetCalendarTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工厂日历
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:attendance:calendar:import", "导入工厂日历")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCalendarAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _calendarService.ImportCalendarAsync(stream, sheetName);
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
    /// 导出工厂日历
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:attendance:calendar:export", "导出工厂日历")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCalendarAsync([FromQuery] TaktCalendarQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _calendarService.ExportCalendarAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
