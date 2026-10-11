// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderLaborsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：维护工单报工控制器
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
/// 维护工单报工控制器
/// 提供维护工单报工的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "维护工单报工")]
public class TaktMaintenanceWorkOrderLaborsController : TaktControllerBase
{
    private readonly ITaktMaintenanceWorkOrderLaborService _maintenanceWorkOrderLaborService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceWorkOrderLaborService">维护工单报工服务</param>
    public TaktMaintenanceWorkOrderLaborsController(ITaktMaintenanceWorkOrderLaborService maintenanceWorkOrderLaborService)
    {
        _maintenanceWorkOrderLaborService = maintenanceWorkOrderLaborService;
    }

    /// <summary>
    /// 获取维护工单报工列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:maintenance:equipment:list", "维护工单报工列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaintenanceWorkOrderLaborListAsync([FromQuery] TaktMaintenanceWorkOrderLaborQueryDto queryDto)
    {
        try
        {
            var result = await _maintenanceWorkOrderLaborService.GetMaintenanceWorkOrderLaborListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取维护工单报工
    /// </summary>
    /// <param name="id">维护工单报工ID</param>
    /// <returns>维护工单报工DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:query", "维护工单报工详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaintenanceWorkOrderLaborByIdAsync(long id)
    {
        try
        {
            var result = await _maintenanceWorkOrderLaborService.GetMaintenanceWorkOrderLaborByIdAsync(id);
            if (result == null)
            {
                return NotFound("维护工单报工不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取维护工单报工选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:maintenance:equipment:query", "维护工单报工选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaintenanceWorkOrderLaborOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _maintenanceWorkOrderLaborService.GetMaintenanceWorkOrderLaborOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建维护工单报工
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>维护工单报工DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:create", "创建维护工单报工")]
    [HttpPost]
    public async Task<IActionResult> CreateMaintenanceWorkOrderLaborAsync([FromBody] TaktMaintenanceWorkOrderLaborCreateDto dto)
    {
        try
        {
            var result = await _maintenanceWorkOrderLaborService.CreateMaintenanceWorkOrderLaborAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新维护工单报工
    /// </summary>
    /// <param name="id">维护工单报工ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>维护工单报工DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:update", "更新维护工单报工")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaintenanceWorkOrderLaborAsync(long id, [FromBody] TaktMaintenanceWorkOrderLaborUpdateDto dto)
    {
        try
        {
            var result = await _maintenanceWorkOrderLaborService.UpdateMaintenanceWorkOrderLaborAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除维护工单报工
    /// </summary>
    /// <param name="id">维护工单报工ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:equipment:delete", "删除维护工单报工")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaintenanceWorkOrderLaborByIdAsync(long id)
    {
        try
        {
            await _maintenanceWorkOrderLaborService.DeleteMaintenanceWorkOrderLaborByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除维护工单报工
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:equipment:delete", "批量删除维护工单报工")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaintenanceWorkOrderLaborBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _maintenanceWorkOrderLaborService.DeleteMaintenanceWorkOrderLaborBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新维护工单报工状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>维护工单报工DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:update", "更新维护工单报工状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMaintenanceWorkOrderLaborStatusAsync([FromBody] TaktMaintenanceWorkOrderLaborStatusDto dto)
    {
        try
        {
            var result = await _maintenanceWorkOrderLaborService.UpdateMaintenanceWorkOrderLaborStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新维护工单报工作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>维护工单报工DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:update", "更新维护工单报工作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateMaintenanceWorkOrderLaborObsoleteAsync([FromBody] TaktMaintenanceWorkOrderLaborObsoleteDto dto)
    {
        try
        {
            var result = await _maintenanceWorkOrderLaborService.UpdateMaintenanceWorkOrderLaborObsoleteAsync(dto);
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
    [TaktPermission("logistics:maintenance:equipment:import", "获取维护工单报工导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaintenanceWorkOrderLaborTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _maintenanceWorkOrderLaborService.GetMaintenanceWorkOrderLaborTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入维护工单报工
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:maintenance:equipment:import", "导入维护工单报工")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaintenanceWorkOrderLaborAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _maintenanceWorkOrderLaborService.ImportMaintenanceWorkOrderLaborAsync(stream, sheetName);
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
    /// 导出维护工单报工
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:maintenance:equipment:export", "导出维护工单报工")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaintenanceWorkOrderLaborAsync([FromQuery] TaktMaintenanceWorkOrderLaborQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _maintenanceWorkOrderLaborService.ExportMaintenanceWorkOrderLaborAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
