// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Attendance
// 文件名称：TaktOvertimesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：加班信息控制器
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
/// 加班信息控制器
/// 提供加班信息的 REST API
/// </summary>
[ApiModule(5, "考勤管理")]
[Route("api/[controller]", Name = "加班信息")]
public class TaktOvertimesController : TaktControllerBase
{
    private readonly ITaktOvertimeService _overtimeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="overtimeService">加班信息服务</param>
    public TaktOvertimesController(ITaktOvertimeService overtimeService)
    {
        _overtimeService = overtimeService;
    }

    /// <summary>
    /// 获取加班信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:attendance:overtime:list", "加班信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetOvertimeListAsync([FromQuery] TaktOvertimeQueryDto queryDto)
    {
        try
        {
            var result = await _overtimeService.GetOvertimeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取加班信息
    /// </summary>
    /// <param name="id">加班信息ID</param>
    /// <returns>加班信息DTO</returns>
    [TaktPermission("human:resource:attendance:overtime:query", "加班信息详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOvertimeByIdAsync(long id)
    {
        try
        {
            var result = await _overtimeService.GetOvertimeByIdAsync(id);
            if (result == null)
            {
                return NotFound("加班信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取加班信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:attendance:overtime:query", "加班信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetOvertimeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _overtimeService.GetOvertimeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建加班信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>加班信息DTO</returns>
    [TaktPermission("human:resource:attendance:overtime:create", "创建加班信息")]
    [HttpPost]
    public async Task<IActionResult> CreateOvertimeAsync([FromBody] TaktOvertimeCreateDto dto)
    {
        try
        {
            var result = await _overtimeService.CreateOvertimeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新加班信息
    /// </summary>
    /// <param name="id">加班信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>加班信息DTO</returns>
    [TaktPermission("human:resource:attendance:overtime:update", "更新加班信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOvertimeAsync(long id, [FromBody] TaktOvertimeUpdateDto dto)
    {
        try
        {
            var result = await _overtimeService.UpdateOvertimeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除加班信息
    /// </summary>
    /// <param name="id">加班信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:overtime:delete", "删除加班信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOvertimeByIdAsync(long id)
    {
        try
        {
            await _overtimeService.DeleteOvertimeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除加班信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:overtime:delete", "批量删除加班信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteOvertimeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _overtimeService.DeleteOvertimeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新加班信息状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>加班信息DTO</returns>
    [TaktPermission("human:resource:attendance:overtime:update", "更新加班信息状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateOvertimeStatusAsync([FromBody] TaktOvertimeStatusDto dto)
    {
        try
        {
            var result = await _overtimeService.UpdateOvertimeStatusAsync(dto);
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
    [TaktPermission("human:resource:attendance:overtime:import", "获取加班信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetOvertimeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _overtimeService.GetOvertimeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入加班信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:attendance:overtime:import", "导入加班信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportOvertimeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _overtimeService.ImportOvertimeAsync(stream, sheetName);
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
    /// 导出加班信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:attendance:overtime:export", "导出加班信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportOvertimeAsync([FromQuery] TaktOvertimeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _overtimeService.ExportOvertimeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
