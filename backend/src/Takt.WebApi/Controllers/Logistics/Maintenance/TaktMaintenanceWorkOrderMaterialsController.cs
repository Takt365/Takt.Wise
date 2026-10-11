// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderMaterialsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：维护工单领料控制器
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
/// 维护工单领料控制器
/// 提供维护工单领料的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "维护工单领料")]
public class TaktMaintenanceWorkOrderMaterialsController : TaktControllerBase
{
    private readonly ITaktMaintenanceWorkOrderMaterialService _maintenanceWorkOrderMaterialService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceWorkOrderMaterialService">维护工单领料服务</param>
    public TaktMaintenanceWorkOrderMaterialsController(ITaktMaintenanceWorkOrderMaterialService maintenanceWorkOrderMaterialService)
    {
        _maintenanceWorkOrderMaterialService = maintenanceWorkOrderMaterialService;
    }

    /// <summary>
    /// 获取维护工单领料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:maintenance:equipment:list", "维护工单领料列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaintenanceWorkOrderMaterialListAsync([FromQuery] TaktMaintenanceWorkOrderMaterialQueryDto queryDto)
    {
        try
        {
            var result = await _maintenanceWorkOrderMaterialService.GetMaintenanceWorkOrderMaterialListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取维护工单领料
    /// </summary>
    /// <param name="id">维护工单领料ID</param>
    /// <returns>维护工单领料DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:query", "维护工单领料详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaintenanceWorkOrderMaterialByIdAsync(long id)
    {
        try
        {
            var result = await _maintenanceWorkOrderMaterialService.GetMaintenanceWorkOrderMaterialByIdAsync(id);
            if (result == null)
            {
                return NotFound("维护工单领料不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取维护工单领料选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:maintenance:equipment:query", "维护工单领料选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaintenanceWorkOrderMaterialOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _maintenanceWorkOrderMaterialService.GetMaintenanceWorkOrderMaterialOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建维护工单领料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>维护工单领料DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:create", "创建维护工单领料")]
    [HttpPost]
    public async Task<IActionResult> CreateMaintenanceWorkOrderMaterialAsync([FromBody] TaktMaintenanceWorkOrderMaterialCreateDto dto)
    {
        try
        {
            var result = await _maintenanceWorkOrderMaterialService.CreateMaintenanceWorkOrderMaterialAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新维护工单领料
    /// </summary>
    /// <param name="id">维护工单领料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>维护工单领料DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:update", "更新维护工单领料")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaintenanceWorkOrderMaterialAsync(long id, [FromBody] TaktMaintenanceWorkOrderMaterialUpdateDto dto)
    {
        try
        {
            var result = await _maintenanceWorkOrderMaterialService.UpdateMaintenanceWorkOrderMaterialAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除维护工单领料
    /// </summary>
    /// <param name="id">维护工单领料ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:equipment:delete", "删除维护工单领料")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaintenanceWorkOrderMaterialByIdAsync(long id)
    {
        try
        {
            await _maintenanceWorkOrderMaterialService.DeleteMaintenanceWorkOrderMaterialByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除维护工单领料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:equipment:delete", "批量删除维护工单领料")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaintenanceWorkOrderMaterialBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _maintenanceWorkOrderMaterialService.DeleteMaintenanceWorkOrderMaterialBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新维护工单领料状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>维护工单领料DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:update", "更新维护工单领料状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMaintenanceWorkOrderMaterialStatusAsync([FromBody] TaktMaintenanceWorkOrderMaterialStatusDto dto)
    {
        try
        {
            var result = await _maintenanceWorkOrderMaterialService.UpdateMaintenanceWorkOrderMaterialStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新维护工单领料作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>维护工单领料DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:update", "更新维护工单领料作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateMaintenanceWorkOrderMaterialObsoleteAsync([FromBody] TaktMaintenanceWorkOrderMaterialObsoleteDto dto)
    {
        try
        {
            var result = await _maintenanceWorkOrderMaterialService.UpdateMaintenanceWorkOrderMaterialObsoleteAsync(dto);
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
    [TaktPermission("logistics:maintenance:equipment:import", "获取维护工单领料导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaintenanceWorkOrderMaterialTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _maintenanceWorkOrderMaterialService.GetMaintenanceWorkOrderMaterialTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入维护工单领料
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:maintenance:equipment:import", "导入维护工单领料")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaintenanceWorkOrderMaterialAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _maintenanceWorkOrderMaterialService.ImportMaintenanceWorkOrderMaterialAsync(stream, sheetName);
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
    /// 导出维护工单领料
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:maintenance:equipment:export", "导出维护工单领料")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaintenanceWorkOrderMaterialAsync([FromQuery] TaktMaintenanceWorkOrderMaterialQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _maintenanceWorkOrderMaterialService.ExportMaintenanceWorkOrderMaterialAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
