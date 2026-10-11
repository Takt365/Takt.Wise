// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Attendance
// 文件名称：TaktWorkShiftsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：班次信息控制器
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
/// 班次信息控制器
/// 提供班次信息的 REST API
/// </summary>
[ApiModule(5, "考勤管理")]
[Route("api/[controller]", Name = "班次信息")]
public class TaktWorkShiftsController : TaktControllerBase
{
    private readonly ITaktWorkShiftService _workShiftService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="workShiftService">班次信息服务</param>
    public TaktWorkShiftsController(ITaktWorkShiftService workShiftService)
    {
        _workShiftService = workShiftService;
    }

    /// <summary>
    /// 获取班次信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:attendance:work:shift:list", "班次信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetWorkShiftListAsync([FromQuery] TaktWorkShiftQueryDto queryDto)
    {
        try
        {
            var result = await _workShiftService.GetWorkShiftListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取班次信息
    /// </summary>
    /// <param name="id">班次信息ID</param>
    /// <returns>班次信息DTO</returns>
    [TaktPermission("human:resource:attendance:work:shift:query", "班次信息详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkShiftByIdAsync(long id)
    {
        try
        {
            var result = await _workShiftService.GetWorkShiftByIdAsync(id);
            if (result == null)
            {
                return NotFound("班次信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取班次信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:attendance:work:shift:query", "班次信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetWorkShiftOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _workShiftService.GetWorkShiftOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建班次信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>班次信息DTO</returns>
    [TaktPermission("human:resource:attendance:work:shift:create", "创建班次信息")]
    [HttpPost]
    public async Task<IActionResult> CreateWorkShiftAsync([FromBody] TaktWorkShiftCreateDto dto)
    {
        try
        {
            var result = await _workShiftService.CreateWorkShiftAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新班次信息
    /// </summary>
    /// <param name="id">班次信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>班次信息DTO</returns>
    [TaktPermission("human:resource:attendance:work:shift:update", "更新班次信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorkShiftAsync(long id, [FromBody] TaktWorkShiftUpdateDto dto)
    {
        try
        {
            var result = await _workShiftService.UpdateWorkShiftAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除班次信息
    /// </summary>
    /// <param name="id">班次信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:work:shift:delete", "删除班次信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkShiftByIdAsync(long id)
    {
        try
        {
            await _workShiftService.DeleteWorkShiftByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除班次信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:work:shift:delete", "批量删除班次信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteWorkShiftBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _workShiftService.DeleteWorkShiftBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新班次信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>班次信息DTO</returns>
    [TaktPermission("human:resource:attendance:work:shift:update", "更新班次信息排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateWorkShiftSortAsync([FromBody] TaktWorkShiftSortDto dto)
    {
        try
        {
            var result = await _workShiftService.UpdateWorkShiftSortAsync(dto);
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
    [TaktPermission("human:resource:attendance:work:shift:import", "获取班次信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetWorkShiftTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _workShiftService.GetWorkShiftTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入班次信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:attendance:work:shift:import", "导入班次信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportWorkShiftAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _workShiftService.ImportWorkShiftAsync(stream, sheetName);
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
    /// 导出班次信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:attendance:work:shift:export", "导出班次信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportWorkShiftAsync([FromQuery] TaktWorkShiftQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _workShiftService.ExportWorkShiftAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
