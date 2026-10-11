// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Workflow
// 文件名称：TaktFlowInstancesController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：流程实例控制器
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
/// 流程实例控制器
/// 提供流程实例的 REST API
/// </summary>
[ApiModule(6, "工作流")]
[Route("api/[controller]", Name = "流程实例")]
public class TaktFlowInstancesController : TaktControllerBase
{
    private readonly ITaktFlowInstanceService _flowInstanceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowInstanceService">流程实例服务</param>
    public TaktFlowInstancesController(ITaktFlowInstanceService flowInstanceService)
    {
        _flowInstanceService = flowInstanceService;
    }

    /// <summary>
    /// 获取流程实例列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("workflow:instance:list", "流程实例列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFlowInstanceListAsync([FromQuery] TaktFlowInstanceQueryDto queryDto)
    {
        try
        {
            var result = await _flowInstanceService.GetFlowInstanceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取流程实例统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>流程实例统计</returns>
    [TaktPermission("workflow:instance:list", "流程实例统计")]
    [HttpGet("instance-stat")]
    public async Task<IActionResult> GetWorkflowInstanceStatAsync([FromQuery] TaktWorkflowInstanceStatQueryDto queryDto)
    {
        try
        {
            var result = await _flowInstanceService.GetWorkflowInstanceStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取流程实例
    /// </summary>
    /// <param name="id">流程实例ID</param>
    /// <returns>流程实例DTO</returns>
    [TaktPermission("workflow:instance:query", "流程实例详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlowInstanceByIdAsync(long id)
    {
        try
        {
            var result = await _flowInstanceService.GetFlowInstanceByIdAsync(id);
            if (result == null)
            {
                return NotFound("流程实例不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取流程实例选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("workflow:instance:query", "流程实例选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFlowInstanceOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _flowInstanceService.GetFlowInstanceOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建流程实例
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>流程实例DTO</returns>
    [TaktPermission("workflow:instance:create", "创建流程实例")]
    [HttpPost]
    public async Task<IActionResult> CreateFlowInstanceAsync([FromBody] TaktFlowInstanceCreateDto dto)
    {
        try
        {
            var result = await _flowInstanceService.CreateFlowInstanceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程实例
    /// </summary>
    /// <param name="id">流程实例ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>流程实例DTO</returns>
    [TaktPermission("workflow:instance:update", "更新流程实例")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFlowInstanceAsync(long id, [FromBody] TaktFlowInstanceUpdateDto dto)
    {
        try
        {
            var result = await _flowInstanceService.UpdateFlowInstanceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除流程实例
    /// </summary>
    /// <param name="id">流程实例ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:instance:delete", "删除流程实例")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFlowInstanceByIdAsync(long id)
    {
        try
        {
            await _flowInstanceService.DeleteFlowInstanceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除流程实例
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("workflow:instance:delete", "批量删除流程实例")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFlowInstanceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _flowInstanceService.DeleteFlowInstanceBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新流程实例状态
    /// </summary>
    /// <param name="dto">状态 DTO（TaktFlowInstanceStatus 枚举）</param>
    /// <returns>流程实例DTO</returns>
    [TaktPermission("workflow:instance:update", "更新流程实例状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateFlowInstanceStatusAsync([FromBody] TaktFlowInstanceStatusDto dto)
    {
        try
        {
            var result = await _flowInstanceService.UpdateFlowInstanceStatusAsync(dto);
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
    [TaktPermission("workflow:instance:import", "获取流程实例导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFlowInstanceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _flowInstanceService.GetFlowInstanceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入流程实例
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("workflow:instance:import", "导入流程实例")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFlowInstanceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _flowInstanceService.ImportFlowInstanceAsync(stream, sheetName);
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
    /// 导出流程实例
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("workflow:instance:export", "导出流程实例")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFlowInstanceAsync([FromQuery] TaktFlowInstanceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _flowInstanceService.ExportFlowInstanceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
