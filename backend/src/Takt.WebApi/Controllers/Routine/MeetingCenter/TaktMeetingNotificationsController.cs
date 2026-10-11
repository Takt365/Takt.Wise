// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.MeetingCenter
// 文件名称：TaktMeetingNotificationsController.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：会议通知控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.MeetingCenter;
using Takt.Application.Services.Routine.MeetingCenter;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.MeetingCenter;

/// <summary>
/// 会议通知控制器
/// 提供会议通知的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "会议通知")]
public class TaktMeetingNotificationsController : TaktControllerBase
{
    private readonly ITaktMeetingNotificationService _meetingNotificationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="meetingNotificationService">会议通知服务</param>
    public TaktMeetingNotificationsController(ITaktMeetingNotificationService meetingNotificationService)
    {
        _meetingNotificationService = meetingNotificationService;
    }

    /// <summary>
    /// 获取会议通知列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:meeting:center:notification:list", "会议通知列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMeetingNotificationListAsync([FromQuery] TaktMeetingNotificationQueryDto queryDto)
    {
        try
        {
            var result = await _meetingNotificationService.GetMeetingNotificationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取会议通知
    /// </summary>
    /// <param name="id">会议通知ID</param>
    /// <returns>会议通知DTO</returns>
    [TaktPermission("routine:meeting:center:notification:query", "会议通知详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMeetingNotificationByIdAsync(long id)
    {
        try
        {
            var result = await _meetingNotificationService.GetMeetingNotificationByIdAsync(id);
            if (result == null)
            {
                return NotFound("会议通知不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会议通知选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:meeting:center:notification:query", "会议通知选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMeetingNotificationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _meetingNotificationService.GetMeetingNotificationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建会议通知
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>会议通知DTO</returns>
    [TaktPermission("routine:meeting:center:notification:create", "创建会议通知")]
    [HttpPost]
    public async Task<IActionResult> CreateMeetingNotificationAsync([FromBody] TaktMeetingNotificationCreateDto dto)
    {
        try
        {
            var result = await _meetingNotificationService.CreateMeetingNotificationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会议通知
    /// </summary>
    /// <param name="id">会议通知ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>会议通知DTO</returns>
    [TaktPermission("routine:meeting:center:notification:update", "更新会议通知")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMeetingNotificationAsync(long id, [FromBody] TaktMeetingNotificationUpdateDto dto)
    {
        try
        {
            var result = await _meetingNotificationService.UpdateMeetingNotificationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除会议通知
    /// </summary>
    /// <param name="id">会议通知ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:meeting:center:notification:delete", "删除会议通知")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeetingNotificationByIdAsync(long id)
    {
        try
        {
            await _meetingNotificationService.DeleteMeetingNotificationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除会议通知
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:meeting:center:notification:delete", "批量删除会议通知")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMeetingNotificationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _meetingNotificationService.DeleteMeetingNotificationBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会议通知状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>会议通知DTO</returns>
    [TaktPermission("routine:meeting:center:notification:update", "更新会议通知状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMeetingNotificationStatusAsync([FromBody] TaktMeetingNotificationStatusDto dto)
    {
        try
        {
            var result = await _meetingNotificationService.UpdateMeetingNotificationStatusAsync(dto);
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
    [TaktPermission("routine:meeting:center:notification:import", "获取会议通知导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMeetingNotificationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _meetingNotificationService.GetMeetingNotificationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入会议通知
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:meeting:center:notification:import", "导入会议通知")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMeetingNotificationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _meetingNotificationService.ImportMeetingNotificationAsync(stream, sheetName);
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
    /// 导出会议通知
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:meeting:center:notification:export", "导出会议通知")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMeetingNotificationAsync([FromQuery] TaktMeetingNotificationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _meetingNotificationService.ExportMeetingNotificationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 按邮件令牌确认收到会议通知（匿名）
    /// </summary>
    /// <param name="dto">令牌 DTO</param>
    /// <returns>确认结果</returns>
    [AllowAnonymous]
    [HttpPost("confirm-receipt")]
    public async Task<IActionResult> ConfirmMeetingNotificationReceiptByTokenAsync(
        [FromBody] TaktMeetingNotificationConfirmReceiptByTokenDto dto)
    {
        try
        {
            var result = await _meetingNotificationService.ConfirmMeetingNotificationReceiptByTokenAsync(dto);
            return Success(result, result.AlreadyConfirmed ? "已确认收到" : "确认成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 当前用户确认收到会议通知
    /// </summary>
    /// <param name="id">会议通知 ID</param>
    /// <returns>确认结果</returns>
    [TaktPermission("routine:meeting:center:notification:update", "确认会议通知回执")]
    [HttpPut("{id}/confirm-receipt")]
    public async Task<IActionResult> ConfirmMeetingNotificationReceiptAsync(long id)
    {
        try
        {
            var result = await _meetingNotificationService.ConfirmMeetingNotificationReceiptAsync(id);
            return Success(result, result.AlreadyConfirmed ? "已确认收到" : "确认成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
