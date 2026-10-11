// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Maintenance
// 文件名称：TaktMaintenanceHistoriesController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：设备维护履历控制器
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
/// 设备维护履历控制器
/// 提供设备维护履历的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设备维护履历")]
public class TaktMaintenanceHistoriesController : TaktControllerBase
{
    private readonly ITaktMaintenanceHistoryService _maintenanceHistoryService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceHistoryService">设备维护履历服务</param>
    public TaktMaintenanceHistoriesController(ITaktMaintenanceHistoryService maintenanceHistoryService)
    {
        _maintenanceHistoryService = maintenanceHistoryService;
    }

    /// <summary>
    /// 获取设备维护履历列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:maintenance:equipment:list", "设备维护履历列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaintenanceHistoryListAsync([FromQuery] TaktMaintenanceHistoryQueryDto queryDto)
    {
        try
        {
            var result = await _maintenanceHistoryService.GetMaintenanceHistoryListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取维护履历统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>维护履历统计</returns>
    [TaktPermission("logistics:maintenance:history:list", "维护履历统计")]
    [HttpGet("history-stat")]
    public async Task<IActionResult> GetMaintenanceHistoryStatAsync([FromQuery] TaktMaintenanceHistoryStatQueryDto queryDto)
    {
        try
        {
            var result = await _maintenanceHistoryService.GetMaintenanceHistoryStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设备维护履历
    /// </summary>
    /// <param name="id">设备维护履历ID</param>
    /// <returns>设备维护履历DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:query", "设备维护履历详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaintenanceHistoryByIdAsync(long id)
    {
        try
        {
            var result = await _maintenanceHistoryService.GetMaintenanceHistoryByIdAsync(id);
            if (result == null)
            {
                return NotFound("设备维护履历不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设备维护履历选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:maintenance:equipment:query", "设备维护履历选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaintenanceHistoryOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _maintenanceHistoryService.GetMaintenanceHistoryOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设备维护履历
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设备维护履历DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:create", "创建设备维护履历")]
    [HttpPost]
    public async Task<IActionResult> CreateMaintenanceHistoryAsync([FromBody] TaktMaintenanceHistoryCreateDto dto)
    {
        try
        {
            var result = await _maintenanceHistoryService.CreateMaintenanceHistoryAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设备维护履历
    /// </summary>
    /// <param name="id">设备维护履历ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设备维护履历DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:update", "更新设备维护履历")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaintenanceHistoryAsync(long id, [FromBody] TaktMaintenanceHistoryUpdateDto dto)
    {
        try
        {
            var result = await _maintenanceHistoryService.UpdateMaintenanceHistoryAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设备维护履历
    /// </summary>
    /// <param name="id">设备维护履历ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:equipment:delete", "删除设备维护履历")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaintenanceHistoryByIdAsync(long id)
    {
        try
        {
            await _maintenanceHistoryService.DeleteMaintenanceHistoryByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设备维护履历
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:equipment:delete", "批量删除设备维护履历")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaintenanceHistoryBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _maintenanceHistoryService.DeleteMaintenanceHistoryBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设备维护履历状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>设备维护履历DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:update", "更新设备维护履历状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMaintenanceHistoryStatusAsync([FromBody] TaktMaintenanceHistoryStatusDto dto)
    {
        try
        {
            var result = await _maintenanceHistoryService.UpdateMaintenanceHistoryStatusAsync(dto);
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
    [TaktPermission("logistics:maintenance:equipment:import", "获取设备维护履历导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaintenanceHistoryTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _maintenanceHistoryService.GetMaintenanceHistoryTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设备维护履历
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:maintenance:equipment:import", "导入设备维护履历")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaintenanceHistoryAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _maintenanceHistoryService.ImportMaintenanceHistoryAsync(stream, sheetName);
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
    /// 导出设备维护履历
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:maintenance:equipment:export", "导出设备维护履历")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaintenanceHistoryAsync([FromQuery] TaktMaintenanceHistoryQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _maintenanceHistoryService.ExportMaintenanceHistoryAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
