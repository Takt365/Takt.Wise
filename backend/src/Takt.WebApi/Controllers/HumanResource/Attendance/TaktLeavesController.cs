// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Attendance
// 文件名称：TaktLeavesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：请假信息控制器
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
/// 请假信息控制器
/// 提供请假信息的 REST API
/// </summary>
[ApiModule(5, "考勤管理")]
[Route("api/[controller]", Name = "请假信息")]
public class TaktLeavesController : TaktControllerBase
{
    private readonly ITaktLeaveService _leaveService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="leaveService">请假信息服务</param>
    public TaktLeavesController(ITaktLeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    /// <summary>
    /// 获取请假信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:attendance:leave:list", "请假信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetLeaveListAsync([FromQuery] TaktLeaveQueryDto queryDto)
    {
        try
        {
            var result = await _leaveService.GetLeaveListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取请假信息
    /// </summary>
    /// <param name="id">请假信息ID</param>
    /// <returns>请假信息DTO</returns>
    [TaktPermission("human:resource:attendance:leave:query", "请假信息详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeaveByIdAsync(long id)
    {
        try
        {
            var result = await _leaveService.GetLeaveByIdAsync(id);
            if (result == null)
            {
                return NotFound("请假信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取请假信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:attendance:leave:query", "请假信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetLeaveOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _leaveService.GetLeaveOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建请假信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>请假信息DTO</returns>
    [TaktPermission("human:resource:attendance:leave:create", "创建请假信息")]
    [HttpPost]
    public async Task<IActionResult> CreateLeaveAsync([FromBody] TaktLeaveCreateDto dto)
    {
        try
        {
            var result = await _leaveService.CreateLeaveAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新请假信息
    /// </summary>
    /// <param name="id">请假信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>请假信息DTO</returns>
    [TaktPermission("human:resource:attendance:leave:update", "更新请假信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLeaveAsync(long id, [FromBody] TaktLeaveUpdateDto dto)
    {
        try
        {
            var result = await _leaveService.UpdateLeaveAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除请假信息
    /// </summary>
    /// <param name="id">请假信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:leave:delete", "删除请假信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeaveByIdAsync(long id)
    {
        try
        {
            await _leaveService.DeleteLeaveByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除请假信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:leave:delete", "批量删除请假信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteLeaveBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _leaveService.DeleteLeaveBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新请假信息状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>请假信息DTO</returns>
    [TaktPermission("human:resource:attendance:leave:update", "更新请假信息状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateLeaveStatusAsync([FromBody] TaktLeaveStatusDto dto)
    {
        try
        {
            var result = await _leaveService.UpdateLeaveStatusAsync(dto);
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
    [TaktPermission("human:resource:attendance:leave:import", "获取请假信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetLeaveTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _leaveService.GetLeaveTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入请假信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:attendance:leave:import", "导入请假信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportLeaveAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _leaveService.ImportLeaveAsync(stream, sheetName);
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
    /// 导出请假信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:attendance:leave:export", "导出请假信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportLeaveAsync([FromQuery] TaktLeaveQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _leaveService.ExportLeaveAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
