// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Performance
// 文件名称：TaktPerfObjectivesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效目标控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Performance;
using Takt.Application.Services.HumanResource.Performance;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Performance;

/// <summary>
/// 绩效目标控制器
/// 提供绩效目标的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "绩效目标")]
public class TaktPerfObjectivesController : TaktControllerBase
{
    private readonly ITaktPerfObjectiveService _perfObjectiveService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="perfObjectiveService">绩效目标服务</param>
    public TaktPerfObjectivesController(ITaktPerfObjectiveService perfObjectiveService)
    {
        _perfObjectiveService = perfObjectiveService;
    }

    /// <summary>
    /// 获取绩效目标列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:performance:perf:objective:list", "绩效目标列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPerfObjectiveListAsync([FromQuery] TaktPerfObjectiveQueryDto queryDto)
    {
        try
        {
            var result = await _perfObjectiveService.GetPerfObjectiveListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取绩效目标
    /// </summary>
    /// <param name="id">绩效目标ID</param>
    /// <returns>绩效目标DTO</returns>
    [TaktPermission("human:resource:performance:perf:objective:query", "绩效目标详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerfObjectiveByIdAsync(long id)
    {
        try
        {
            var result = await _perfObjectiveService.GetPerfObjectiveByIdAsync(id);
            if (result == null)
            {
                return NotFound("绩效目标不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取绩效目标选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:performance:perf:objective:query", "绩效目标选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPerfObjectiveOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _perfObjectiveService.GetPerfObjectiveOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建绩效目标
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>绩效目标DTO</returns>
    [TaktPermission("human:resource:performance:perf:objective:create", "创建绩效目标")]
    [HttpPost]
    public async Task<IActionResult> CreatePerfObjectiveAsync([FromBody] TaktPerfObjectiveCreateDto dto)
    {
        try
        {
            var result = await _perfObjectiveService.CreatePerfObjectiveAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效目标
    /// </summary>
    /// <param name="id">绩效目标ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>绩效目标DTO</returns>
    [TaktPermission("human:resource:performance:perf:objective:update", "更新绩效目标")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePerfObjectiveAsync(long id, [FromBody] TaktPerfObjectiveUpdateDto dto)
    {
        try
        {
            var result = await _perfObjectiveService.UpdatePerfObjectiveAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除绩效目标
    /// </summary>
    /// <param name="id">绩效目标ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:performance:perf:objective:delete", "删除绩效目标")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerfObjectiveByIdAsync(long id)
    {
        try
        {
            await _perfObjectiveService.DeletePerfObjectiveByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除绩效目标
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:performance:perf:objective:delete", "批量删除绩效目标")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePerfObjectiveBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _perfObjectiveService.DeletePerfObjectiveBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效目标状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>绩效目标DTO</returns>
    [TaktPermission("human:resource:performance:perf:objective:update", "更新绩效目标状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePerfObjectiveStatusAsync([FromBody] TaktPerfObjectiveStatusDto dto)
    {
        try
        {
            var result = await _perfObjectiveService.UpdatePerfObjectiveStatusAsync(dto);
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
    [TaktPermission("human:resource:performance:perf:objective:import", "获取绩效目标导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPerfObjectiveTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _perfObjectiveService.GetPerfObjectiveTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入绩效目标
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:performance:perf:objective:import", "导入绩效目标")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPerfObjectiveAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _perfObjectiveService.ImportPerfObjectiveAsync(stream, sheetName);
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
    /// 导出绩效目标
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:performance:perf:objective:export", "导出绩效目标")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPerfObjectiveAsync([FromQuery] TaktPerfObjectiveQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _perfObjectiveService.ExportPerfObjectiveAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
