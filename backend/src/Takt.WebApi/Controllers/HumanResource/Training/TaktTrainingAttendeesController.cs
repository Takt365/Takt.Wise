// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Training
// 文件名称：TaktTrainingAttendeesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：培训参训记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Training;
using Takt.Application.Services.HumanResource.Training;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Training;

/// <summary>
/// 培训参训记录控制器
/// 提供培训参训记录的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "培训参训记录")]
public class TaktTrainingAttendeesController : TaktControllerBase
{
    private readonly ITaktTrainingAttendeeService _trainingAttendeeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="trainingAttendeeService">培训参训记录服务</param>
    public TaktTrainingAttendeesController(ITaktTrainingAttendeeService trainingAttendeeService)
    {
        _trainingAttendeeService = trainingAttendeeService;
    }

    /// <summary>
    /// 获取培训参训记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:training:attendee:list", "培训参训记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTrainingAttendeeListAsync([FromQuery] TaktTrainingAttendeeQueryDto queryDto)
    {
        try
        {
            var result = await _trainingAttendeeService.GetTrainingAttendeeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取培训参训记录
    /// </summary>
    /// <param name="id">培训参训记录ID</param>
    /// <returns>培训参训记录DTO</returns>
    [TaktPermission("human:resource:training:attendee:query", "培训参训记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTrainingAttendeeByIdAsync(long id)
    {
        try
        {
            var result = await _trainingAttendeeService.GetTrainingAttendeeByIdAsync(id);
            if (result == null)
            {
                return NotFound("培训参训记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取培训结果选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:training:attendee:query", "培训参训记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTrainingAttendeeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _trainingAttendeeService.GetTrainingAttendeeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建培训参训记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>培训参训记录DTO</returns>
    [TaktPermission("human:resource:training:attendee:create", "创建培训参训记录")]
    [HttpPost]
    public async Task<IActionResult> CreateTrainingAttendeeAsync([FromBody] TaktTrainingAttendeeCreateDto dto)
    {
        try
        {
            var result = await _trainingAttendeeService.CreateTrainingAttendeeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新培训参训记录
    /// </summary>
    /// <param name="id">培训参训记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>培训参训记录DTO</returns>
    [TaktPermission("human:resource:training:attendee:update", "更新培训参训记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrainingAttendeeAsync(long id, [FromBody] TaktTrainingAttendeeUpdateDto dto)
    {
        try
        {
            var result = await _trainingAttendeeService.UpdateTrainingAttendeeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除培训参训记录
    /// </summary>
    /// <param name="id">培训参训记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:training:attendee:delete", "删除培训参训记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainingAttendeeByIdAsync(long id)
    {
        try
        {
            await _trainingAttendeeService.DeleteTrainingAttendeeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除培训参训记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:training:attendee:delete", "批量删除培训参训记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTrainingAttendeeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _trainingAttendeeService.DeleteTrainingAttendeeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新培训参训记录状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>培训参训记录DTO</returns>
    [TaktPermission("human:resource:training:attendee:update", "更新培训参训记录状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTrainingAttendeeStatusAsync([FromBody] TaktTrainingAttendeeStatusDto dto)
    {
        try
        {
            var result = await _trainingAttendeeService.UpdateTrainingAttendeeStatusAsync(dto);
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
    [TaktPermission("human:resource:training:attendee:import", "获取培训参训记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTrainingAttendeeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _trainingAttendeeService.GetTrainingAttendeeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入培训参训记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:training:attendee:import", "导入培训参训记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTrainingAttendeeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _trainingAttendeeService.ImportTrainingAttendeeAsync(stream, sheetName);
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
    /// 导出培训参训记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:training:attendee:export", "导出培训参训记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTrainingAttendeeAsync([FromQuery] TaktTrainingAttendeeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _trainingAttendeeService.ExportTrainingAttendeeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
