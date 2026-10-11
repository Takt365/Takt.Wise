// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Attendance
// 文件名称：TaktShiftSchedulesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：排班信息控制器
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
/// 排班信息控制器
/// 提供排班信息的 REST API
/// </summary>
[ApiModule(5, "考勤管理")]
[Route("api/[controller]", Name = "排班信息")]
public class TaktShiftSchedulesController : TaktControllerBase
{
    private readonly ITaktShiftScheduleService _shiftScheduleService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="shiftScheduleService">排班信息服务</param>
    public TaktShiftSchedulesController(ITaktShiftScheduleService shiftScheduleService)
    {
        _shiftScheduleService = shiftScheduleService;
    }

    /// <summary>
    /// 获取排班信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:attendance:shift:schedule:list", "排班信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetShiftScheduleListAsync([FromQuery] TaktShiftScheduleQueryDto queryDto)
    {
        try
        {
            var result = await _shiftScheduleService.GetShiftScheduleListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取排班信息
    /// </summary>
    /// <param name="id">排班信息ID</param>
    /// <returns>排班信息DTO</returns>
    [TaktPermission("human:resource:attendance:shift:schedule:query", "排班信息详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetShiftScheduleByIdAsync(long id)
    {
        try
        {
            var result = await _shiftScheduleService.GetShiftScheduleByIdAsync(id);
            if (result == null)
            {
                return NotFound("排班信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取排班信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:attendance:shift:schedule:query", "排班信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetShiftScheduleOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _shiftScheduleService.GetShiftScheduleOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建排班信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>排班信息DTO</returns>
    [TaktPermission("human:resource:attendance:shift:schedule:create", "创建排班信息")]
    [HttpPost]
    public async Task<IActionResult> CreateShiftScheduleAsync([FromBody] TaktShiftScheduleCreateDto dto)
    {
        try
        {
            var result = await _shiftScheduleService.CreateShiftScheduleAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新排班信息
    /// </summary>
    /// <param name="id">排班信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>排班信息DTO</returns>
    [TaktPermission("human:resource:attendance:shift:schedule:update", "更新排班信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShiftScheduleAsync(long id, [FromBody] TaktShiftScheduleUpdateDto dto)
    {
        try
        {
            var result = await _shiftScheduleService.UpdateShiftScheduleAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除排班信息
    /// </summary>
    /// <param name="id">排班信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:shift:schedule:delete", "删除排班信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShiftScheduleByIdAsync(long id)
    {
        try
        {
            await _shiftScheduleService.DeleteShiftScheduleByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除排班信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:shift:schedule:delete", "批量删除排班信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteShiftScheduleBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _shiftScheduleService.DeleteShiftScheduleBatchAsync(ids);
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
    [TaktPermission("human:resource:attendance:shift:schedule:import", "获取排班信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetShiftScheduleTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _shiftScheduleService.GetShiftScheduleTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入排班信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:attendance:shift:schedule:import", "导入排班信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportShiftScheduleAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _shiftScheduleService.ImportShiftScheduleAsync(stream, sheetName);
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
    /// 导出排班信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:attendance:shift:schedule:export", "导出排班信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportShiftScheduleAsync([FromQuery] TaktShiftScheduleQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _shiftScheduleService.ExportShiftScheduleAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
