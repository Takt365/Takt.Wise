// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Workflow
// 文件名称：TaktFlowAddSignsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：流程加签记录控制器
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
/// 流程加签记录控制器
/// 提供流程加签记录的 REST API
/// </summary>
[ApiModule(6, "工作流")]
[Route("api/[controller]", Name = "流程加签记录")]
public class TaktFlowAddSignsController : TaktControllerBase
{
    private readonly ITaktFlowAddSignService _flowAddSignService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowAddSignService">流程加签记录服务</param>
    public TaktFlowAddSignsController(ITaktFlowAddSignService flowAddSignService)
    {
        _flowAddSignService = flowAddSignService;
    }

    /// <summary>
    /// 获取流程加签记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("workflow:todo:list", "流程加签记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFlowAddSignListAsync([FromQuery] TaktFlowAddSignQueryDto queryDto)
    {
        try
        {
            var result = await _flowAddSignService.GetFlowAddSignListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取流程加签记录
    /// </summary>
    /// <param name="id">流程加签记录ID</param>
    /// <returns>流程加签记录DTO</returns>
    [TaktPermission("workflow:todo:query", "流程加签记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlowAddSignByIdAsync(long id)
    {
        try
        {
            var result = await _flowAddSignService.GetFlowAddSignByIdAsync(id);
            if (result == null)
            {
                return NotFound("流程加签记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取流程加签记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("workflow:todo:query", "流程加签记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFlowAddSignOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _flowAddSignService.GetFlowAddSignOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建流程加签记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>流程加签记录DTO</returns>
    [TaktPermission("workflow:todo:addsign", "创建流程加签记录")]
    [HttpPost]
    public async Task<IActionResult> CreateFlowAddSignAsync([FromBody] TaktFlowAddSignCreateDto dto)
    {
        try
        {
            var result = await _flowAddSignService.CreateFlowAddSignAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程加签记录
    /// </summary>
    /// <param name="id">流程加签记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>流程加签记录DTO</returns>
    [TaktPermission("workflow:todo:addsign", "更新流程加签记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFlowAddSignAsync(long id, [FromBody] TaktFlowAddSignUpdateDto dto)
    {
        try
        {
            var result = await _flowAddSignService.UpdateFlowAddSignAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除流程加签记录
    /// </summary>
    /// <param name="id">流程加签记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:todo:reducesign", "删除流程加签记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFlowAddSignByIdAsync(long id)
    {
        try
        {
            await _flowAddSignService.DeleteFlowAddSignByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除流程加签记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:todo:reducesign", "批量删除流程加签记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFlowAddSignBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _flowAddSignService.DeleteFlowAddSignBatchAsync(ids);
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
    [TaktPermission("workflow:todo:import", "获取流程加签记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFlowAddSignTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _flowAddSignService.GetFlowAddSignTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入流程加签记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("workflow:todo:import", "导入流程加签记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFlowAddSignAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _flowAddSignService.ImportFlowAddSignAsync(stream, sheetName);
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
    /// 导出流程加签记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("workflow:todo:export", "导出流程加签记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFlowAddSignAsync([FromQuery] TaktFlowAddSignQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _flowAddSignService.ExportFlowAddSignAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
