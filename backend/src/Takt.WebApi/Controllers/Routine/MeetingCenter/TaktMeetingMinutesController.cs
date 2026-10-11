// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.MeetingCenter
// 文件名称：TaktMeetingMinutesController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：会后纪要控制器
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
/// 会后纪要控制器
/// 提供会后纪要的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "会后纪要")]
public class TaktMeetingMinutesController : TaktControllerBase
{
    private readonly ITaktMeetingMinutesService _meetingMinutesService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="meetingMinutesService">会后纪要服务</param>
    public TaktMeetingMinutesController(ITaktMeetingMinutesService meetingMinutesService)
    {
        _meetingMinutesService = meetingMinutesService;
    }

    /// <summary>
    /// 获取会后纪要列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:meeting:center:minutes:list", "会后纪要列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMeetingMinutesListAsync([FromQuery] TaktMeetingMinutesQueryDto queryDto)
    {
        try
        {
            var result = await _meetingMinutesService.GetMeetingMinutesListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取会后纪要
    /// </summary>
    /// <param name="id">会后纪要ID</param>
    /// <returns>会后纪要DTO</returns>
    [TaktPermission("routine:meeting:center:minutes:query", "会后纪要详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMeetingMinutesByIdAsync(long id)
    {
        try
        {
            var result = await _meetingMinutesService.GetMeetingMinutesByIdAsync(id);
            if (result == null)
            {
                return NotFound("会后纪要不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会后纪要选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:meeting:center:minutes:query", "会后纪要选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMeetingMinutesOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _meetingMinutesService.GetMeetingMinutesOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建会后纪要
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>会后纪要DTO</returns>
    [TaktPermission("routine:meeting:center:minutes:create", "创建会后纪要")]
    [HttpPost]
    public async Task<IActionResult> CreateMeetingMinutesAsync([FromBody] TaktMeetingMinutesCreateDto dto)
    {
        try
        {
            var result = await _meetingMinutesService.CreateMeetingMinutesAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会后纪要
    /// </summary>
    /// <param name="id">会后纪要ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>会后纪要DTO</returns>
    [TaktPermission("routine:meeting:center:minutes:update", "更新会后纪要")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMeetingMinutesAsync(long id, [FromBody] TaktMeetingMinutesUpdateDto dto)
    {
        try
        {
            var result = await _meetingMinutesService.UpdateMeetingMinutesAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除会后纪要
    /// </summary>
    /// <param name="id">会后纪要ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:meeting:center:minutes:delete", "删除会后纪要")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeetingMinutesByIdAsync(long id)
    {
        try
        {
            await _meetingMinutesService.DeleteMeetingMinutesByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除会后纪要
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:meeting:center:minutes:delete", "批量删除会后纪要")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMeetingMinutesBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _meetingMinutesService.DeleteMeetingMinutesBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会后纪要作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>会后纪要DTO</returns>
    [TaktPermission("routine:meeting:center:minutes:update", "更新会后纪要作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateMeetingMinutesObsoleteAsync([FromBody] TaktMeetingMinutesObsoleteDto dto)
    {
        try
        {
            var result = await _meetingMinutesService.UpdateMeetingMinutesObsoleteAsync(dto);
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
    [TaktPermission("routine:meeting:center:minutes:import", "获取会后纪要导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMeetingMinutesTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _meetingMinutesService.GetMeetingMinutesTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入会后纪要
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:meeting:center:minutes:import", "导入会后纪要")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMeetingMinutesAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _meetingMinutesService.ImportMeetingMinutesAsync(stream, sheetName);
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
    /// 导出会后纪要
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:meeting:center:minutes:export", "导出会后纪要")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMeetingMinutesAsync([FromQuery] TaktMeetingMinutesQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _meetingMinutesService.ExportMeetingMinutesAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
