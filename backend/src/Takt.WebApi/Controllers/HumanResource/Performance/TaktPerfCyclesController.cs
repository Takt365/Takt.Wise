// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Performance
// 文件名称：TaktPerfCyclesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效周期日程控制器
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
/// 绩效周期日程控制器
/// 提供绩效周期日程的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "绩效周期日程")]
public class TaktPerfCyclesController : TaktControllerBase
{
    private readonly ITaktPerfCycleService _perfCycleService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="perfCycleService">绩效周期日程服务</param>
    public TaktPerfCyclesController(ITaktPerfCycleService perfCycleService)
    {
        _perfCycleService = perfCycleService;
    }

    /// <summary>
    /// 获取绩效周期日程列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:performance:perf:cycle:list", "绩效周期日程列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPerfCycleListAsync([FromQuery] TaktPerfCycleQueryDto queryDto)
    {
        try
        {
            var result = await _perfCycleService.GetPerfCycleListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取绩效周期日程
    /// </summary>
    /// <param name="id">绩效周期日程ID</param>
    /// <returns>绩效周期日程DTO</returns>
    [TaktPermission("human:resource:performance:perf:cycle:query", "绩效周期日程详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerfCycleByIdAsync(long id)
    {
        try
        {
            var result = await _perfCycleService.GetPerfCycleByIdAsync(id);
            if (result == null)
            {
                return NotFound("绩效周期日程不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取绩效周期日程选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:performance:perf:cycle:query", "绩效周期日程选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPerfCycleOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _perfCycleService.GetPerfCycleOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建绩效周期日程
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>绩效周期日程DTO</returns>
    [TaktPermission("human:resource:performance:perf:cycle:create", "创建绩效周期日程")]
    [HttpPost]
    public async Task<IActionResult> CreatePerfCycleAsync([FromBody] TaktPerfCycleCreateDto dto)
    {
        try
        {
            var result = await _perfCycleService.CreatePerfCycleAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效周期日程
    /// </summary>
    /// <param name="id">绩效周期日程ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>绩效周期日程DTO</returns>
    [TaktPermission("human:resource:performance:perf:cycle:update", "更新绩效周期日程")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePerfCycleAsync(long id, [FromBody] TaktPerfCycleUpdateDto dto)
    {
        try
        {
            var result = await _perfCycleService.UpdatePerfCycleAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除绩效周期日程
    /// </summary>
    /// <param name="id">绩效周期日程ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:performance:perf:cycle:delete", "删除绩效周期日程")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerfCycleByIdAsync(long id)
    {
        try
        {
            await _perfCycleService.DeletePerfCycleByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除绩效周期日程
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:performance:perf:cycle:delete", "批量删除绩效周期日程")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePerfCycleBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _perfCycleService.DeletePerfCycleBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效周期日程状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>绩效周期日程DTO</returns>
    [TaktPermission("human:resource:performance:perf:cycle:update", "更新绩效周期日程状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePerfCycleStatusAsync([FromBody] TaktPerfCycleStatusDto dto)
    {
        try
        {
            var result = await _perfCycleService.UpdatePerfCycleStatusAsync(dto);
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
    [TaktPermission("human:resource:performance:perf:cycle:import", "获取绩效周期日程导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPerfCycleTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _perfCycleService.GetPerfCycleTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入绩效周期日程
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:performance:perf:cycle:import", "导入绩效周期日程")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPerfCycleAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _perfCycleService.ImportPerfCycleAsync(stream, sheetName);
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
    /// 导出绩效周期日程
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:performance:perf:cycle:export", "导出绩效周期日程")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPerfCycleAsync([FromQuery] TaktPerfCycleQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _perfCycleService.ExportPerfCycleAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
