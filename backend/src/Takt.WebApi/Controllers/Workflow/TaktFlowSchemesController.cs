// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Workflow
// 文件名称：TaktFlowSchemesController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：流程定义控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Workflow;
using Takt.Application.Services.Workflow;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Workflow;

/// <summary>
/// 流程定义控制器
/// 提供流程定义的 REST API
/// </summary>
[ApiModule(6, "工作流")]
[Route("api/[controller]", Name = "流程定义")]
public class TaktFlowSchemesController : TaktControllerBase
{
    private readonly ITaktFlowSchemeService _flowSchemeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowSchemeService">流程定义服务</param>
    public TaktFlowSchemesController(ITaktFlowSchemeService flowSchemeService)
    {
        _flowSchemeService = flowSchemeService;
    }

    /// <summary>
    /// 获取流程定义列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("workflow:scheme:list", "流程定义列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFlowSchemeListAsync([FromQuery] TaktFlowSchemeQueryDto queryDto)
    {
        try
        {
            var result = await _flowSchemeService.GetFlowSchemeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取流程定义
    /// </summary>
    /// <param name="id">流程定义ID</param>
    /// <returns>流程定义DTO</returns>
    [TaktPermission("workflow:scheme:query", "流程定义详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlowSchemeByIdAsync(long id)
    {
        try
        {
            var result = await _flowSchemeService.GetFlowSchemeByIdAsync(id);
            if (result == null)
            {
                return NotFound("流程定义不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取流程定义选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("workflow:scheme:query", "流程定义选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFlowSchemeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _flowSchemeService.GetFlowSchemeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建流程定义
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>流程定义DTO</returns>
    [TaktPermission("workflow:scheme:create", "创建流程定义")]
    [HttpPost]
    public async Task<IActionResult> CreateFlowSchemeAsync([FromBody] TaktFlowSchemeCreateDto dto)
    {
        try
        {
            var result = await _flowSchemeService.CreateFlowSchemeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程定义
    /// </summary>
    /// <param name="id">流程定义ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>流程定义DTO</returns>
    [TaktPermission("workflow:scheme:update", "更新流程定义")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFlowSchemeAsync(long id, [FromBody] TaktFlowSchemeUpdateDto dto)
    {
        try
        {
            var result = await _flowSchemeService.UpdateFlowSchemeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除流程定义
    /// </summary>
    /// <param name="id">流程定义ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:scheme:delete", "删除流程定义")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFlowSchemeByIdAsync(long id)
    {
        try
        {
            await _flowSchemeService.DeleteFlowSchemeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除流程定义
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:scheme:delete", "批量删除流程定义")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFlowSchemeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _flowSchemeService.DeleteFlowSchemeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程定义状态
    /// </summary>
    /// <param name="dto">状态 DTO（processStatus 与字典 sys_scheme_status 一致）</param>
    /// <returns>流程定义DTO</returns>
    [TaktPermission("workflow:scheme:update", "更新流程定义状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateFlowSchemeStatusAsync([FromBody] TaktFlowSchemeStatusDto dto)
    {
        try
        {
            var result = await _flowSchemeService.UpdateFlowSchemeStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程定义排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>流程定义DTO</returns>
    [TaktPermission("workflow:scheme:update", "更新流程定义排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateFlowSchemeSortAsync([FromBody] TaktFlowSchemeSortDto dto)
    {
        try
        {
            var result = await _flowSchemeService.UpdateFlowSchemeSortAsync(dto);
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
    [TaktPermission("workflow:scheme:import", "获取流程定义导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFlowSchemeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _flowSchemeService.GetFlowSchemeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入流程定义
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("workflow:scheme:import", "导入流程定义")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFlowSchemeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _flowSchemeService.ImportFlowSchemeAsync(stream, sheetName);
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
    /// 导出流程定义
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("workflow:scheme:export", "导出流程定义")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFlowSchemeAsync([FromQuery] TaktFlowSchemeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _flowSchemeService.ExportFlowSchemeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
