// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Performance
// 文件名称：TaktPerfAssessmentsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效考核控制器
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
/// 绩效考核控制器
/// 提供绩效考核的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "绩效考核")]
public class TaktPerfAssessmentsController : TaktControllerBase
{
    private readonly ITaktPerfAssessmentService _perfAssessmentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="perfAssessmentService">绩效考核服务</param>
    public TaktPerfAssessmentsController(ITaktPerfAssessmentService perfAssessmentService)
    {
        _perfAssessmentService = perfAssessmentService;
    }

    /// <summary>
    /// 获取绩效考核列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:performance:perf:assessment:list", "绩效考核列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPerfAssessmentListAsync([FromQuery] TaktPerfAssessmentQueryDto queryDto)
    {
        try
        {
            var result = await _perfAssessmentService.GetPerfAssessmentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取绩效考核
    /// </summary>
    /// <param name="id">绩效考核ID</param>
    /// <returns>绩效考核DTO</returns>
    [TaktPermission("human:resource:performance:perf:assessment:query", "绩效考核详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPerfAssessmentByIdAsync(long id)
    {
        try
        {
            var result = await _perfAssessmentService.GetPerfAssessmentByIdAsync(id);
            if (result == null)
            {
                return NotFound("绩效考核不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取绩效考核选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:performance:perf:assessment:query", "绩效考核选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPerfAssessmentOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _perfAssessmentService.GetPerfAssessmentOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建绩效考核
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>绩效考核DTO</returns>
    [TaktPermission("human:resource:performance:perf:assessment:create", "创建绩效考核")]
    [HttpPost]
    public async Task<IActionResult> CreatePerfAssessmentAsync([FromBody] TaktPerfAssessmentCreateDto dto)
    {
        try
        {
            var result = await _perfAssessmentService.CreatePerfAssessmentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效考核
    /// </summary>
    /// <param name="id">绩效考核ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>绩效考核DTO</returns>
    [TaktPermission("human:resource:performance:perf:assessment:update", "更新绩效考核")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePerfAssessmentAsync(long id, [FromBody] TaktPerfAssessmentUpdateDto dto)
    {
        try
        {
            var result = await _perfAssessmentService.UpdatePerfAssessmentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除绩效考核
    /// </summary>
    /// <param name="id">绩效考核ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:performance:perf:assessment:delete", "删除绩效考核")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerfAssessmentByIdAsync(long id)
    {
        try
        {
            await _perfAssessmentService.DeletePerfAssessmentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除绩效考核
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:performance:perf:assessment:delete", "批量删除绩效考核")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePerfAssessmentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _perfAssessmentService.DeletePerfAssessmentBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效考核状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>绩效考核DTO</returns>
    [TaktPermission("human:resource:performance:perf:assessment:update", "更新绩效考核状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePerfAssessmentStatusAsync([FromBody] TaktPerfAssessmentStatusDto dto)
    {
        try
        {
            var result = await _perfAssessmentService.UpdatePerfAssessmentStatusAsync(dto);
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
    [TaktPermission("human:resource:performance:perf:assessment:import", "获取绩效考核导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPerfAssessmentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _perfAssessmentService.GetPerfAssessmentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入绩效考核
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:performance:perf:assessment:import", "导入绩效考核")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPerfAssessmentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _perfAssessmentService.ImportPerfAssessmentAsync(stream, sheetName);
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
    /// 导出绩效考核
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:performance:perf:assessment:export", "导出绩效考核")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPerfAssessmentAsync([FromQuery] TaktPerfAssessmentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _perfAssessmentService.ExportPerfAssessmentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
