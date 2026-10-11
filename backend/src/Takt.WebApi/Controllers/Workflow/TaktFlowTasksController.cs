// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Workflow
// 文件名称：TaktFlowTasksController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：流程用户任务控制器
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
/// 流程用户任务控制器
/// 提供流程用户任务的 REST API
/// </summary>
[ApiModule(6, "工作流")]
[Route("api/[controller]", Name = "流程用户任务")]
public class TaktFlowTasksController : TaktControllerBase
{
    private readonly ITaktFlowTaskService _flowTaskService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowTaskService">流程用户任务服务</param>
    public TaktFlowTasksController(ITaktFlowTaskService flowTaskService)
    {
        _flowTaskService = flowTaskService;
    }

    /// <summary>
    /// 获取流程用户任务列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("workflow:todo:list", "流程用户任务列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFlowTaskListAsync([FromQuery] TaktFlowTaskQueryDto queryDto)
    {
        try
        {
            var result = await _flowTaskService.GetFlowTaskListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取流程用户任务
    /// </summary>
    /// <param name="id">流程用户任务ID</param>
    /// <returns>流程用户任务DTO</returns>
    [TaktPermission("workflow:todo:query", "流程用户任务详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlowTaskByIdAsync(long id)
    {
        try
        {
            var result = await _flowTaskService.GetFlowTaskByIdAsync(id);
            if (result == null)
            {
                return NotFound("流程用户任务不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取流程用户任务选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("workflow:todo:query", "流程用户任务选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFlowTaskOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _flowTaskService.GetFlowTaskOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建流程用户任务
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>流程用户任务DTO</returns>
    [TaktPermission("workflow:todo:create", "创建流程用户任务")]
    [HttpPost]
    public async Task<IActionResult> CreateFlowTaskAsync([FromBody] TaktFlowTaskCreateDto dto)
    {
        try
        {
            var result = await _flowTaskService.CreateFlowTaskAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程用户任务
    /// </summary>
    /// <param name="id">流程用户任务ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>流程用户任务DTO</returns>
    [TaktPermission("workflow:todo:update", "更新流程用户任务")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFlowTaskAsync(long id, [FromBody] TaktFlowTaskUpdateDto dto)
    {
        try
        {
            var result = await _flowTaskService.UpdateFlowTaskAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除流程用户任务
    /// </summary>
    /// <param name="id">流程用户任务ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:todo:delete", "删除流程用户任务")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFlowTaskByIdAsync(long id)
    {
        try
        {
            await _flowTaskService.DeleteFlowTaskByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除流程用户任务
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:todo:delete", "批量删除流程用户任务")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFlowTaskBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _flowTaskService.DeleteFlowTaskBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程用户任务状态
    /// </summary>
    /// <param name="dto">状态 DTO（TaktFlowTaskStatus 枚举）</param>
    /// <returns>流程用户任务DTO</returns>
    [TaktPermission("workflow:todo:approve", "更新流程用户任务状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateFlowTaskStatusAsync([FromBody] TaktFlowTaskStatusDto dto)
    {
        try
        {
            var result = await _flowTaskService.UpdateFlowTaskStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程用户任务排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>流程用户任务DTO</returns>
    [TaktPermission("workflow:todo:update", "更新流程用户任务排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateFlowTaskSortAsync([FromBody] TaktFlowTaskSortDto dto)
    {
        try
        {
            var result = await _flowTaskService.UpdateFlowTaskSortAsync(dto);
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
    [TaktPermission("workflow:todo:import", "获取流程用户任务导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFlowTaskTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _flowTaskService.GetFlowTaskTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入流程用户任务
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("workflow:todo:import", "导入流程用户任务")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFlowTaskAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _flowTaskService.ImportFlowTaskAsync(stream, sheetName);
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
    /// 导出流程用户任务
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("workflow:todo:export", "导出流程用户任务")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFlowTaskAsync([FromQuery] TaktFlowTaskQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _flowTaskService.ExportFlowTaskAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
