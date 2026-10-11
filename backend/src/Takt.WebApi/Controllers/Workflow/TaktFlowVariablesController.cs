// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Workflow
// 文件名称：TaktFlowVariablesController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：流程变量控制器
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
/// 流程变量控制器
/// 提供流程变量的 REST API
/// </summary>
[ApiModule(6, "工作流")]
[Route("api/[controller]", Name = "流程变量")]
public class TaktFlowVariablesController : TaktControllerBase
{
    private readonly ITaktFlowVariableService _flowVariableService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowVariableService">流程变量服务</param>
    public TaktFlowVariablesController(ITaktFlowVariableService flowVariableService)
    {
        _flowVariableService = flowVariableService;
    }

    /// <summary>
    /// 获取流程变量列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("workflow:instance:list", "流程变量列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFlowVariableListAsync([FromQuery] TaktFlowVariableQueryDto queryDto)
    {
        try
        {
            var result = await _flowVariableService.GetFlowVariableListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取流程变量
    /// </summary>
    /// <param name="id">流程变量ID</param>
    /// <returns>流程变量DTO</returns>
    [TaktPermission("workflow:instance:query", "流程变量详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlowVariableByIdAsync(long id)
    {
        try
        {
            var result = await _flowVariableService.GetFlowVariableByIdAsync(id);
            if (result == null)
            {
                return NotFound("流程变量不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取流程变量选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("workflow:instance:query", "流程变量选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFlowVariableOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _flowVariableService.GetFlowVariableOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建流程变量
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>流程变量DTO</returns>
    [TaktPermission("workflow:instance:create", "创建流程变量")]
    [HttpPost]
    public async Task<IActionResult> CreateFlowVariableAsync([FromBody] TaktFlowVariableCreateDto dto)
    {
        try
        {
            var result = await _flowVariableService.CreateFlowVariableAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程变量
    /// </summary>
    /// <param name="id">流程变量ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>流程变量DTO</returns>
    [TaktPermission("workflow:instance:update", "更新流程变量")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFlowVariableAsync(long id, [FromBody] TaktFlowVariableUpdateDto dto)
    {
        try
        {
            var result = await _flowVariableService.UpdateFlowVariableAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除流程变量
    /// </summary>
    /// <param name="id">流程变量ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:instance:delete", "删除流程变量")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFlowVariableByIdAsync(long id)
    {
        try
        {
            await _flowVariableService.DeleteFlowVariableByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除流程变量
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:instance:delete", "批量删除流程变量")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFlowVariableBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _flowVariableService.DeleteFlowVariableBatchAsync(ids);
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
    [TaktPermission("workflow:instance:import", "获取流程变量导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFlowVariableTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _flowVariableService.GetFlowVariableTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入流程变量
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("workflow:instance:import", "导入流程变量")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFlowVariableAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _flowVariableService.ImportFlowVariableAsync(stream, sheetName);
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
    /// 导出流程变量
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("workflow:instance:export", "导出流程变量")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFlowVariableAsync([FromQuery] TaktFlowVariableQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _flowVariableService.ExportFlowVariableAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
