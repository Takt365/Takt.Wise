// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Workflow
// 文件名称：TaktFlowTransitionsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：流程流转历史控制器
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
/// 流程流转历史控制器
/// 提供流程流转历史的 REST API
/// </summary>
[ApiModule(6, "工作流")]
[Route("api/[controller]", Name = "流程流转历史")]
public class TaktFlowTransitionsController : TaktControllerBase
{
    private readonly ITaktFlowTransitionService _flowTransitionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowTransitionService">流程流转历史服务</param>
    public TaktFlowTransitionsController(ITaktFlowTransitionService flowTransitionService)
    {
        _flowTransitionService = flowTransitionService;
    }

    /// <summary>
    /// 获取流程流转历史列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("workflow:instance:list", "流程流转历史列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFlowTransitionListAsync([FromQuery] TaktFlowTransitionQueryDto queryDto)
    {
        try
        {
            var result = await _flowTransitionService.GetFlowTransitionListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取流程流转历史
    /// </summary>
    /// <param name="id">流程流转历史ID</param>
    /// <returns>流程流转历史DTO</returns>
    [TaktPermission("workflow:instance:query", "流程流转历史详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlowTransitionByIdAsync(long id)
    {
        try
        {
            var result = await _flowTransitionService.GetFlowTransitionByIdAsync(id);
            if (result == null)
            {
                return NotFound("流程流转历史不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取流程流转历史选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("workflow:instance:query", "流程流转历史选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFlowTransitionOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _flowTransitionService.GetFlowTransitionOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建流程流转历史
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>流程流转历史DTO</returns>
    [TaktPermission("workflow:instance:create", "创建流程流转历史")]
    [HttpPost]
    public async Task<IActionResult> CreateFlowTransitionAsync([FromBody] TaktFlowTransitionCreateDto dto)
    {
        try
        {
            var result = await _flowTransitionService.CreateFlowTransitionAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程流转历史
    /// </summary>
    /// <param name="id">流程流转历史ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>流程流转历史DTO</returns>
    [TaktPermission("workflow:instance:update", "更新流程流转历史")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFlowTransitionAsync(long id, [FromBody] TaktFlowTransitionUpdateDto dto)
    {
        try
        {
            var result = await _flowTransitionService.UpdateFlowTransitionAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除流程流转历史
    /// </summary>
    /// <param name="id">流程流转历史ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:instance:delete", "删除流程流转历史")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFlowTransitionByIdAsync(long id)
    {
        try
        {
            await _flowTransitionService.DeleteFlowTransitionByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除流程流转历史
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:instance:delete", "批量删除流程流转历史")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFlowTransitionBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _flowTransitionService.DeleteFlowTransitionBatchAsync(ids);
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
    [TaktPermission("workflow:instance:import", "获取流程流转历史导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFlowTransitionTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _flowTransitionService.GetFlowTransitionTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入流程流转历史
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("workflow:instance:import", "导入流程流转历史")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFlowTransitionAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _flowTransitionService.ImportFlowTransitionAsync(stream, sheetName);
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
    /// 导出流程流转历史
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("workflow:instance:export", "导出流程流转历史")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFlowTransitionAsync([FromQuery] TaktFlowTransitionQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _flowTransitionService.ExportFlowTransitionAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
