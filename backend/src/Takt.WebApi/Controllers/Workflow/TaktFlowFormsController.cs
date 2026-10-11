// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Workflow
// 文件名称：TaktFlowFormsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：流程表单控制器
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
/// 流程表单控制器
/// 提供流程表单的 REST API
/// </summary>
[ApiModule(6, "工作流")]
[Route("api/[controller]", Name = "流程表单")]
public class TaktFlowFormsController : TaktControllerBase
{
    private readonly ITaktFlowFormService _flowFormService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowFormService">流程表单服务</param>
    public TaktFlowFormsController(ITaktFlowFormService flowFormService)
    {
        _flowFormService = flowFormService;
    }

    /// <summary>
    /// 获取流程表单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("workflow:form:list", "流程表单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFlowFormListAsync([FromQuery] TaktFlowFormQueryDto queryDto)
    {
        try
        {
            var result = await _flowFormService.GetFlowFormListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取流程表单
    /// </summary>
    /// <param name="id">流程表单ID</param>
    /// <returns>流程表单DTO</returns>
    [TaktPermission("workflow:form:query", "流程表单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlowFormByIdAsync(long id)
    {
        try
        {
            var result = await _flowFormService.GetFlowFormByIdAsync(id);
            if (result == null)
            {
                return NotFound("流程表单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取流程表单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("workflow:form:query", "流程表单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFlowFormOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _flowFormService.GetFlowFormOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建流程表单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>流程表单DTO</returns>
    [TaktPermission("workflow:form:create", "创建流程表单")]
    [HttpPost]
    public async Task<IActionResult> CreateFlowFormAsync([FromBody] TaktFlowFormCreateDto dto)
    {
        try
        {
            var result = await _flowFormService.CreateFlowFormAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程表单
    /// </summary>
    /// <param name="id">流程表单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>流程表单DTO</returns>
    [TaktPermission("workflow:form:update", "更新流程表单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFlowFormAsync(long id, [FromBody] TaktFlowFormUpdateDto dto)
    {
        try
        {
            var result = await _flowFormService.UpdateFlowFormAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除流程表单
    /// </summary>
    /// <param name="id">流程表单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:form:delete", "删除流程表单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFlowFormByIdAsync(long id)
    {
        try
        {
            await _flowFormService.DeleteFlowFormByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除流程表单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:form:delete", "批量删除流程表单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFlowFormBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _flowFormService.DeleteFlowFormBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程表单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>流程表单DTO</returns>
    [TaktPermission("workflow:form:update", "更新流程表单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateFlowFormStatusAsync([FromBody] TaktFlowFormStatusDto dto)
    {
        try
        {
            var result = await _flowFormService.UpdateFlowFormStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程表单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>流程表单DTO</returns>
    [TaktPermission("workflow:form:update", "更新流程表单排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateFlowFormSortAsync([FromBody] TaktFlowFormSortDto dto)
    {
        try
        {
            var result = await _flowFormService.UpdateFlowFormSortAsync(dto);
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
    [TaktPermission("workflow:form:import", "获取流程表单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFlowFormTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _flowFormService.GetFlowFormTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入流程表单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("workflow:form:import", "导入流程表单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFlowFormAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _flowFormService.ImportFlowFormAsync(stream, sheetName);
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
    /// 导出流程表单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("workflow:form:export", "导出流程表单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFlowFormAsync([FromQuery] TaktFlowFormQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _flowFormService.ExportFlowFormAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
