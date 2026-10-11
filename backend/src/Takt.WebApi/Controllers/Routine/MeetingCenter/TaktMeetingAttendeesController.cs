// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.MeetingCenter
// 文件名称：TaktMeetingAttendeesController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：参会人员控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.MeetingCenter;
using Takt.Application.Services.Routine.MeetingCenter;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.MeetingCenter;

/// <summary>
/// 参会人员控制器
/// 提供参会人员的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "参会人员")]
public class TaktMeetingAttendeesController : TaktControllerBase
{
    private readonly ITaktMeetingAttendeeService _meetingAttendeeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="meetingAttendeeService">参会人员服务</param>
    public TaktMeetingAttendeesController(ITaktMeetingAttendeeService meetingAttendeeService)
    {
        _meetingAttendeeService = meetingAttendeeService;
    }

    /// <summary>
    /// 获取参会人员列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:meeting:center:attendee:list", "参会人员列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMeetingAttendeeListAsync([FromQuery] TaktMeetingAttendeeQueryDto queryDto)
    {
        try
        {
            var result = await _meetingAttendeeService.GetMeetingAttendeeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取参会人员
    /// </summary>
    /// <param name="id">参会人员ID</param>
    /// <returns>参会人员DTO</returns>
    [TaktPermission("routine:meeting:center:attendee:query", "参会人员详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMeetingAttendeeByIdAsync(long id)
    {
        try
        {
            var result = await _meetingAttendeeService.GetMeetingAttendeeByIdAsync(id);
            if (result == null)
            {
                return NotFound("参会人员不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取参会人员选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:meeting:center:attendee:query", "参会人员选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMeetingAttendeeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _meetingAttendeeService.GetMeetingAttendeeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建参会人员
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>参会人员DTO</returns>
    [TaktPermission("routine:meeting:center:attendee:create", "创建参会人员")]
    [HttpPost]
    public async Task<IActionResult> CreateMeetingAttendeeAsync([FromBody] TaktMeetingAttendeeCreateDto dto)
    {
        try
        {
            var result = await _meetingAttendeeService.CreateMeetingAttendeeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新参会人员
    /// </summary>
    /// <param name="id">参会人员ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>参会人员DTO</returns>
    [TaktPermission("routine:meeting:center:attendee:update", "更新参会人员")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMeetingAttendeeAsync(long id, [FromBody] TaktMeetingAttendeeUpdateDto dto)
    {
        try
        {
            var result = await _meetingAttendeeService.UpdateMeetingAttendeeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除参会人员
    /// </summary>
    /// <param name="id">参会人员ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:meeting:center:attendee:delete", "删除参会人员")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeetingAttendeeByIdAsync(long id)
    {
        try
        {
            await _meetingAttendeeService.DeleteMeetingAttendeeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除参会人员
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:meeting:center:attendee:delete", "批量删除参会人员")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMeetingAttendeeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _meetingAttendeeService.DeleteMeetingAttendeeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新参会人员状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>参会人员DTO</returns>
    [TaktPermission("routine:meeting:center:attendee:update", "更新参会人员状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMeetingAttendeeStatusAsync([FromBody] TaktMeetingAttendeeStatusDto dto)
    {
        try
        {
            var result = await _meetingAttendeeService.UpdateMeetingAttendeeStatusAsync(dto);
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
    [TaktPermission("routine:meeting:center:attendee:import", "获取参会人员导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMeetingAttendeeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _meetingAttendeeService.GetMeetingAttendeeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入参会人员
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:meeting:center:attendee:import", "导入参会人员")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMeetingAttendeeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _meetingAttendeeService.ImportMeetingAttendeeAsync(stream, sheetName);
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
    /// 导出参会人员
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:meeting:center:attendee:export", "导出参会人员")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMeetingAttendeeAsync([FromQuery] TaktMeetingAttendeeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _meetingAttendeeService.ExportMeetingAttendeeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
