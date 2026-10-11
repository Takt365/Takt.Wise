// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.MeetingCenter
// 文件名称：TaktMeetingsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：会议中心控制器
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
/// 会议中心控制器
/// 提供会议中心的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "会议中心")]
public class TaktMeetingsController : TaktControllerBase
{
    private readonly ITaktMeetingService _meetingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="meetingService">会议中心服务</param>
    public TaktMeetingsController(ITaktMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    /// <summary>
    /// 获取会议中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:meeting:center:list", "会议中心列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMeetingListAsync([FromQuery] TaktMeetingQueryDto queryDto)
    {
        try
        {
            var result = await _meetingService.GetMeetingListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会议件数统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>会议件数统计</returns>
    [TaktPermission("routine:meeting:center:list", "会议件数统计")]
    [HttpGet("meeting-stat")]
    public async Task<IActionResult> GetMeetingStatAsync([FromQuery] TaktMeetingStatQueryDto queryDto)
    {
        try
        {
            var result = await _meetingService.GetMeetingStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <returns>会议中心DTO</returns>
    [TaktPermission("routine:meeting:center:query", "会议中心详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMeetingByIdAsync(long id)
    {
        try
        {
            var result = await _meetingService.GetMeetingByIdAsync(id);
            if (result == null)
            {
                return NotFound("会议中心不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会议中心主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:meeting:center:query", "会议中心选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMeetingOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _meetingService.GetMeetingOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建会议中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>会议中心DTO</returns>
    [TaktPermission("routine:meeting:center:create", "创建会议中心")]
    [HttpPost]
    public async Task<IActionResult> CreateMeetingAsync([FromBody] TaktMeetingCreateDto dto)
    {
        try
        {
            var result = await _meetingService.CreateMeetingAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>会议中心DTO</returns>
    [TaktPermission("routine:meeting:center:update", "更新会议中心")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMeetingAsync(long id, [FromBody] TaktMeetingUpdateDto dto)
    {
        try
        {
            var result = await _meetingService.UpdateMeetingAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:meeting:center:delete", "删除会议中心")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeetingByIdAsync(long id)
    {
        try
        {
            await _meetingService.DeleteMeetingByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除会议中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:meeting:center:delete", "批量删除会议中心")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMeetingBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _meetingService.DeleteMeetingBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会议中心状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>会议中心DTO</returns>
    [TaktPermission("routine:meeting:center:update", "更新会议中心状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMeetingStatusAsync([FromBody] TaktMeetingStatusDto dto)
    {
        try
        {
            var result = await _meetingService.UpdateMeetingStatusAsync(dto);
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
    [TaktPermission("routine:meeting:center:import", "获取会议中心导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMeetingTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _meetingService.GetMeetingTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入会议中心
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:meeting:center:import", "导入会议中心")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMeetingAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _meetingService.ImportMeetingAsync(stream, sheetName);
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
    /// 导出会议中心
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:meeting:center:export", "导出会议中心")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMeetingAsync([FromQuery] TaktMeetingQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _meetingService.ExportMeetingAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
