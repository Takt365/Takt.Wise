// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrdersController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：维护工单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Maintenance;
using Takt.Application.Services.Logistics.Maintenance;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Maintenance;

/// <summary>
/// 维护工单控制器
/// 提供维护工单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "维护工单")]
public class TaktMaintenanceWorkOrdersController : TaktControllerBase
{
    private readonly ITaktMaintenanceWorkOrderService _maintenanceWorkOrderService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceWorkOrderService">维护工单服务</param>
    public TaktMaintenanceWorkOrdersController(ITaktMaintenanceWorkOrderService maintenanceWorkOrderService)
    {
        _maintenanceWorkOrderService = maintenanceWorkOrderService;
    }

    /// <summary>
    /// 获取维护工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:maintenance:equipment:list", "维护工单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaintenanceWorkOrderListAsync([FromQuery] TaktMaintenanceWorkOrderQueryDto queryDto)
    {
        try
        {
            var result = await _maintenanceWorkOrderService.GetMaintenanceWorkOrderListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取维护工单
    /// </summary>
    /// <param name="id">维护工单ID</param>
    /// <returns>维护工单DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:query", "维护工单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaintenanceWorkOrderByIdAsync(long id)
    {
        try
        {
            var result = await _maintenanceWorkOrderService.GetMaintenanceWorkOrderByIdAsync(id);
            if (result == null)
            {
                return NotFound("维护工单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取维护工单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:maintenance:equipment:query", "维护工单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaintenanceWorkOrderOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _maintenanceWorkOrderService.GetMaintenanceWorkOrderOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建维护工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>维护工单DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:create", "创建维护工单")]
    [HttpPost]
    public async Task<IActionResult> CreateMaintenanceWorkOrderAsync([FromBody] TaktMaintenanceWorkOrderCreateDto dto)
    {
        try
        {
            var result = await _maintenanceWorkOrderService.CreateMaintenanceWorkOrderAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新维护工单
    /// </summary>
    /// <param name="id">维护工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>维护工单DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:update", "更新维护工单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaintenanceWorkOrderAsync(long id, [FromBody] TaktMaintenanceWorkOrderUpdateDto dto)
    {
        try
        {
            var result = await _maintenanceWorkOrderService.UpdateMaintenanceWorkOrderAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除维护工单
    /// </summary>
    /// <param name="id">维护工单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:equipment:delete", "删除维护工单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaintenanceWorkOrderByIdAsync(long id)
    {
        try
        {
            await _maintenanceWorkOrderService.DeleteMaintenanceWorkOrderByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除维护工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:equipment:delete", "批量删除维护工单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaintenanceWorkOrderBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _maintenanceWorkOrderService.DeleteMaintenanceWorkOrderBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新维护工单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>维护工单DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:update", "更新维护工单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMaintenanceWorkOrderStatusAsync([FromBody] TaktMaintenanceWorkOrderStatusDto dto)
    {
        try
        {
            var result = await _maintenanceWorkOrderService.UpdateMaintenanceWorkOrderStatusAsync(dto);
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
    [TaktPermission("logistics:maintenance:equipment:import", "获取维护工单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaintenanceWorkOrderTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _maintenanceWorkOrderService.GetMaintenanceWorkOrderTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入维护工单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:maintenance:equipment:import", "导入维护工单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaintenanceWorkOrderAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _maintenanceWorkOrderService.ImportMaintenanceWorkOrderAsync(stream, sheetName);
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
    /// 导出维护工单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:maintenance:equipment:export", "导出维护工单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaintenanceWorkOrderAsync([FromQuery] TaktMaintenanceWorkOrderQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _maintenanceWorkOrderService.ExportMaintenanceWorkOrderAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
