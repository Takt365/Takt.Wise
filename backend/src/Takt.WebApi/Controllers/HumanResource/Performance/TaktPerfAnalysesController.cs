// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Performance
// 文件名称：TaktPerfAnalysesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：分析改进控制器
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
/// 分析改进控制器
/// 提供分析改进的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "分析改进")]
public class TaktPerfAnalysesController : TaktControllerBase
{
    private readonly ITaktPerfAnalysisService _perfAnalysisService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="perfAnalysisService">分析改进服务</param>
    public TaktPerfAnalysesController(ITaktPerfAnalysisService perfAnalysisService)
    {
        _perfAnalysisService = perfAnalysisService;
    }

    /// <summary>
    /// 获取分析改进列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:performance:perf:analysis:list", "分析改进列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPerfAnalysisListAsync([FromQuery] TaktPerfAnalysisQueryDto queryDto)
    {
        try
        {
            var result = await _perfAnalysisService.GetPerfAnalysisListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取分析改进
    /// </summary>
    /// <param name="id">分析改进ID</param>
    /// <returns>分析改进DTO</returns>
    [TaktPermission("human:resource:performance:perf:analysis:query", "分析改进详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerfAnalysisByIdAsync(long id)
    {
        try
        {
            var result = await _perfAnalysisService.GetPerfAnalysisByIdAsync(id);
            if (result == null)
            {
                return NotFound("分析改进不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取分析改进选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:performance:perf:analysis:query", "分析改进选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPerfAnalysisOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _perfAnalysisService.GetPerfAnalysisOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建分析改进
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>分析改进DTO</returns>
    [TaktPermission("human:resource:performance:perf:analysis:create", "创建分析改进")]
    [HttpPost]
    public async Task<IActionResult> CreatePerfAnalysisAsync([FromBody] TaktPerfAnalysisCreateDto dto)
    {
        try
        {
            var result = await _perfAnalysisService.CreatePerfAnalysisAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新分析改进
    /// </summary>
    /// <param name="id">分析改进ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>分析改进DTO</returns>
    [TaktPermission("human:resource:performance:perf:analysis:update", "更新分析改进")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePerfAnalysisAsync(long id, [FromBody] TaktPerfAnalysisUpdateDto dto)
    {
        try
        {
            var result = await _perfAnalysisService.UpdatePerfAnalysisAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除分析改进
    /// </summary>
    /// <param name="id">分析改进ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:performance:perf:analysis:delete", "删除分析改进")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerfAnalysisByIdAsync(long id)
    {
        try
        {
            await _perfAnalysisService.DeletePerfAnalysisByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除分析改进
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:performance:perf:analysis:delete", "批量删除分析改进")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePerfAnalysisBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _perfAnalysisService.DeletePerfAnalysisBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新分析改进状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>分析改进DTO</returns>
    [TaktPermission("human:resource:performance:perf:analysis:update", "更新分析改进状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePerfAnalysisStatusAsync([FromBody] TaktPerfAnalysisStatusDto dto)
    {
        try
        {
            var result = await _perfAnalysisService.UpdatePerfAnalysisStatusAsync(dto);
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
    [TaktPermission("human:resource:performance:perf:analysis:import", "获取分析改进导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPerfAnalysisTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _perfAnalysisService.GetPerfAnalysisTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入分析改进
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:performance:perf:analysis:import", "导入分析改进")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPerfAnalysisAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _perfAnalysisService.ImportPerfAnalysisAsync(stream, sheetName);
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
    /// 导出分析改进
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:performance:perf:analysis:export", "导出分析改进")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPerfAnalysisAsync([FromQuery] TaktPerfAnalysisQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _perfAnalysisService.ExportPerfAnalysisAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
