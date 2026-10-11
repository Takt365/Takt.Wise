// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Performance
// 文件名称：TaktPerfSchemesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效方案指标控制器
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
/// 绩效方案指标控制器
/// 提供绩效方案指标的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "绩效方案指标")]
public class TaktPerfSchemesController : TaktControllerBase
{
    private readonly ITaktPerfSchemeService _perfSchemeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="perfSchemeService">绩效方案指标服务</param>
    public TaktPerfSchemesController(ITaktPerfSchemeService perfSchemeService)
    {
        _perfSchemeService = perfSchemeService;
    }

    /// <summary>
    /// 获取绩效方案指标列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:performance:perf:scheme:list", "绩效方案指标列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPerfSchemeListAsync([FromQuery] TaktPerfSchemeQueryDto queryDto)
    {
        try
        {
            var result = await _perfSchemeService.GetPerfSchemeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取绩效方案指标
    /// </summary>
    /// <param name="id">绩效方案指标ID</param>
    /// <returns>绩效方案指标DTO</returns>
    [TaktPermission("human:resource:performance:perf:scheme:query", "绩效方案指标详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerfSchemeByIdAsync(long id)
    {
        try
        {
            var result = await _perfSchemeService.GetPerfSchemeByIdAsync(id);
            if (result == null)
            {
                return NotFound("绩效方案指标不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取绩效方案指标选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:performance:perf:scheme:query", "绩效方案指标选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPerfSchemeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _perfSchemeService.GetPerfSchemeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建绩效方案指标
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>绩效方案指标DTO</returns>
    [TaktPermission("human:resource:performance:perf:scheme:create", "创建绩效方案指标")]
    [HttpPost]
    public async Task<IActionResult> CreatePerfSchemeAsync([FromBody] TaktPerfSchemeCreateDto dto)
    {
        try
        {
            var result = await _perfSchemeService.CreatePerfSchemeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效方案指标
    /// </summary>
    /// <param name="id">绩效方案指标ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>绩效方案指标DTO</returns>
    [TaktPermission("human:resource:performance:perf:scheme:update", "更新绩效方案指标")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePerfSchemeAsync(long id, [FromBody] TaktPerfSchemeUpdateDto dto)
    {
        try
        {
            var result = await _perfSchemeService.UpdatePerfSchemeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除绩效方案指标
    /// </summary>
    /// <param name="id">绩效方案指标ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:performance:perf:scheme:delete", "删除绩效方案指标")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerfSchemeByIdAsync(long id)
    {
        try
        {
            await _perfSchemeService.DeletePerfSchemeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除绩效方案指标
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:performance:perf:scheme:delete", "批量删除绩效方案指标")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePerfSchemeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _perfSchemeService.DeletePerfSchemeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效方案指标状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>绩效方案指标DTO</returns>
    [TaktPermission("human:resource:performance:perf:scheme:update", "更新绩效方案指标状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePerfSchemeStatusAsync([FromBody] TaktPerfSchemeStatusDto dto)
    {
        try
        {
            var result = await _perfSchemeService.UpdatePerfSchemeStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效方案指标排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>绩效方案指标DTO</returns>
    [TaktPermission("human:resource:performance:perf:scheme:update", "更新绩效方案指标排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdatePerfSchemeSortAsync([FromBody] TaktPerfSchemeSortDto dto)
    {
        try
        {
            var result = await _perfSchemeService.UpdatePerfSchemeSortAsync(dto);
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
    [TaktPermission("human:resource:performance:perf:scheme:import", "获取绩效方案指标导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPerfSchemeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _perfSchemeService.GetPerfSchemeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入绩效方案指标
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:performance:perf:scheme:import", "导入绩效方案指标")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPerfSchemeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _perfSchemeService.ImportPerfSchemeAsync(stream, sheetName);
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
    /// 导出绩效方案指标
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:performance:perf:scheme:export", "导出绩效方案指标")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPerfSchemeAsync([FromQuery] TaktPerfSchemeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _perfSchemeService.ExportPerfSchemeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
