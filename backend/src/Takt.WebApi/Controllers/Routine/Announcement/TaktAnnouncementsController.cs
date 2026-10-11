// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.Announcement
// 文件名称：TaktAnnouncementsController.cs
// 创建时间：2026-09-11
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.Announcement;
using Takt.Application.Services.Routine.Announcement;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.Announcement;

/// <summary>
/// 公告通知控制器
/// 提供公告通知的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "公告通知")]
public class TaktAnnouncementsController : TaktControllerBase
{
    private readonly ITaktAnnouncementService _announcementService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="announcementService">公告通知服务</param>
    public TaktAnnouncementsController(ITaktAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    /// <summary>
    /// 获取公告通知列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:announcement:list", "公告通知列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAnnouncementListAsync([FromQuery] TaktAnnouncementQueryDto queryDto)
    {
        try
        {
            var result = await _announcementService.GetAnnouncementListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取公告通知
    /// </summary>
    /// <param name="id">公告通知ID</param>
    /// <returns>公告通知DTO</returns>
    [TaktPermission("routine:announcement:query", "公告通知详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetAnnouncementByIdAsync(long id)
    {
        try
        {
            var result = await _announcementService.GetAnnouncementByIdAsync(id);
            if (result == null)
            {
                return NotFound("公告通知不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取公告通知选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:announcement:query", "公告通知选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetAnnouncementOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _announcementService.GetAnnouncementOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建公告通知
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>公告通知DTO</returns>
    [TaktPermission("routine:announcement:create", "创建公告通知")]
    [HttpPost]
    public async Task<IActionResult> CreateAnnouncementAsync([FromBody] TaktAnnouncementCreateDto dto)
    {
        try
        {
            var result = await _announcementService.CreateAnnouncementAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新公告通知
    /// </summary>
    /// <param name="id">公告通知ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>公告通知DTO</returns>
    [TaktPermission("routine:announcement:update", "更新公告通知")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnnouncementAsync(long id, [FromBody] TaktAnnouncementUpdateDto dto)
    {
        try
        {
            var result = await _announcementService.UpdateAnnouncementAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除公告通知
    /// </summary>
    /// <param name="id">公告通知ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:announcement:delete", "删除公告通知")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnnouncementByIdAsync(long id)
    {
        try
        {
            await _announcementService.DeleteAnnouncementByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除公告通知
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:announcement:delete", "批量删除公告通知")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteAnnouncementBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _announcementService.DeleteAnnouncementBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新公告通知状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>公告通知DTO</returns>
    [TaktPermission("routine:announcement:update", "更新公告通知状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateAnnouncementStatusAsync([FromBody] TaktAnnouncementStatusDto dto)
    {
        try
        {
            var result = await _announcementService.UpdateAnnouncementStatusAsync(dto);
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
    [TaktPermission("routine:announcement:import", "获取公告通知导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetAnnouncementTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _announcementService.GetAnnouncementTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入公告通知
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:announcement:import", "导入公告通知")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportAnnouncementAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _announcementService.ImportAnnouncementAsync(stream, sheetName);
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
    /// 导出公告通知
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:announcement:export", "导出公告通知")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportAnnouncementAsync([FromQuery] TaktAnnouncementQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _announcementService.ExportAnnouncementAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
