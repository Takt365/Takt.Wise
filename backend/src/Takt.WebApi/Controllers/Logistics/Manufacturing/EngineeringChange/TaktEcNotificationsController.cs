// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNotificationsController.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更通知单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 工程变更通知单控制器
/// 提供工程变更通知单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "工程变更通知单")]
public class TaktEcNotificationsController : TaktControllerBase
{
    private readonly ITaktEcNotificationService _ecNotificationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecNotificationService">工程变更通知单服务</param>
    public TaktEcNotificationsController(ITaktEcNotificationService ecNotificationService)
    {
        _ecNotificationService = ecNotificationService;
    }

    /// <summary>
    /// 获取工程变更通知单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:notification:list", "工程变更通知单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcNotificationListAsync([FromQuery] TaktEcNotificationQueryDto queryDto)
    {
        try
        {
            var result = await _ecNotificationService.GetEcNotificationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <returns>工程变更通知单DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:notification:query", "工程变更通知单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEcNotificationByIdAsync(long id)
    {
        try
        {
            var result = await _ecNotificationService.GetEcNotificationByIdAsync(id);
            if (result == null)
            {
                return NotFound("工程变更通知单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工程变更通知单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:notification:query", "工程变更通知单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcNotificationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ecNotificationService.GetEcNotificationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工程变更通知单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工程变更通知单DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:notification:create", "创建工程变更通知单")]
    [HttpPost]
    public async Task<IActionResult> CreateEcNotificationAsync([FromBody] TaktEcNotificationCreateDto dto)
    {
        try
        {
            var result = await _ecNotificationService.CreateEcNotificationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工程变更通知单DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:notification:update", "更新工程变更通知单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEcNotificationAsync(long id, [FromBody] TaktEcNotificationUpdateDto dto)
    {
        try
        {
            var result = await _ecNotificationService.UpdateEcNotificationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:notification:delete", "删除工程变更通知单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcNotificationByIdAsync(long id)
    {
        try
        {
            await _ecNotificationService.DeleteEcNotificationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工程变更通知单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:notification:delete", "批量删除工程变更通知单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcNotificationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecNotificationService.DeleteEcNotificationBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工程变更通知单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>工程变更通知单DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:notification:update", "更新工程变更通知单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEcNotificationStatusAsync([FromBody] TaktEcNotificationStatusDto dto)
    {
        try
        {
            var result = await _ecNotificationService.UpdateEcNotificationStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:notification:import", "获取工程变更通知单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcNotificationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecNotificationService.GetEcNotificationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工程变更通知单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:notification:import", "导入工程变更通知单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcNotificationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecNotificationService.ImportEcNotificationAsync(stream, sheetName);
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
    /// 导出工程变更通知单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:notification:export", "导出工程变更通知单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcNotificationAsync([FromQuery] TaktEcNotificationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecNotificationService.ExportEcNotificationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
