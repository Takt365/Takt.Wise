// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktQuartzTasksController.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：定时任务控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 定时任务控制器
/// 提供定时任务的 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "定时任务")]
public class TaktQuartzTasksController : TaktControllerBase
{
    private readonly ITaktQuartzTaskService _quartzTaskService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="quartzTaskService">定时任务服务</param>
    public TaktQuartzTasksController(ITaktQuartzTaskService quartzTaskService)
    {
        _quartzTaskService = quartzTaskService;
    }

    /// <summary>
    /// 获取定时任务列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:quartz:task:list", "定时任务列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetQuartzTaskListAsync([FromQuery] TaktQuartzTaskQueryDto queryDto)
    {
        try
        {
            var result = await _quartzTaskService.GetQuartzTaskListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取定时任务
    /// </summary>
    /// <param name="id">定时任务ID</param>
    /// <returns>定时任务DTO</returns>
    [TaktPermission("foundation:quartz:task:query", "定时任务详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuartzTaskByIdAsync(long id)
    {
        try
        {
            var result = await _quartzTaskService.GetQuartzTaskByIdAsync(id);
            if (result == null)
            {
                return NotFound("定时任务不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取定时任务选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:quartz:task:query", "定时任务选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetQuartzTaskOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _quartzTaskService.GetQuartzTaskOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建定时任务
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>定时任务DTO</returns>
    [TaktPermission("foundation:quartz:task:create", "创建定时任务")]
    [HttpPost]
    public async Task<IActionResult> CreateQuartzTaskAsync([FromBody] TaktQuartzTaskCreateDto dto)
    {
        try
        {
            var result = await _quartzTaskService.CreateQuartzTaskAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新定时任务
    /// </summary>
    /// <param name="id">定时任务ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>定时任务DTO</returns>
    [TaktPermission("foundation:quartz:task:update", "更新定时任务")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuartzTaskAsync(long id, [FromBody] TaktQuartzTaskUpdateDto dto)
    {
        try
        {
            var result = await _quartzTaskService.UpdateQuartzTaskAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除定时任务
    /// </summary>
    /// <param name="id">定时任务ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:quartz:task:delete", "删除定时任务")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuartzTaskByIdAsync(long id)
    {
        try
        {
            await _quartzTaskService.DeleteQuartzTaskByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除定时任务
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:quartz:task:delete", "批量删除定时任务")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteQuartzTaskBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _quartzTaskService.DeleteQuartzTaskBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新定时任务状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>定时任务DTO</returns>
    [TaktPermission("foundation:quartz:task:update", "更新定时任务状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateQuartzTaskStatusAsync([FromBody] TaktQuartzTaskStatusDto dto)
    {
        try
        {
            var result = await _quartzTaskService.UpdateQuartzTaskStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 立即执行一次定时任务
    /// </summary>
    /// <param name="id">定时任务ID</param>
    /// <param name="dto">可选执行参数</param>
    /// <returns>定时任务DTO</returns>
    [TaktPermission("foundation:quartz:task:execute", "立即执行定时任务")]
    [HttpPost("{id}/execute")]
    public async Task<IActionResult> ExecuteQuartzTaskNowAsync(long id, [FromBody] TaktQuartzTaskExecuteDto? dto = null)
    {
        try
        {
            var result = await _quartzTaskService.ExecuteQuartzTaskNowAsync(id, dto?.ExecuteParams);
            return Success(result, "已触发执行");
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
    [TaktPermission("foundation:quartz:task:import", "获取定时任务导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetQuartzTaskTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _quartzTaskService.GetQuartzTaskTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入定时任务
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:quartz:task:import", "导入定时任务")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportQuartzTaskAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _quartzTaskService.ImportQuartzTaskAsync(stream, sheetName);
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
    /// 导出定时任务
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:quartz:task:export", "导出定时任务")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportQuartzTaskAsync([FromQuery] TaktQuartzTaskQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _quartzTaskService.ExportQuartzTaskAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
