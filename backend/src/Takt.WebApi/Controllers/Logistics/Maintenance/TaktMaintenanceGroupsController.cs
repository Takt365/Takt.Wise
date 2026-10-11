// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Maintenance
// 文件名称：TaktMaintenanceGroupsController.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设备维护组主数据控制器
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
/// 设备维护组主数据控制器
/// 提供设备维护组主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设备维护组主数据")]
public class TaktMaintenanceGroupsController : TaktControllerBase
{
    private readonly ITaktMaintenanceGroupService _maintenanceGroupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceGroupService">设备维护组主数据服务</param>
    public TaktMaintenanceGroupsController(ITaktMaintenanceGroupService maintenanceGroupService)
    {
        _maintenanceGroupService = maintenanceGroupService;
    }

    /// <summary>
    /// 获取设备维护组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:maintenance:group:list", "设备维护组主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaintenanceGroupListAsync([FromQuery] TaktMaintenanceGroupQueryDto queryDto)
    {
        try
        {
            var result = await _maintenanceGroupService.GetMaintenanceGroupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设备维护组主数据
    /// </summary>
    /// <param name="id">设备维护组主数据ID</param>
    /// <returns>设备维护组主数据DTO</returns>
    [TaktPermission("logistics:maintenance:group:query", "设备维护组主数据详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetMaintenanceGroupByIdAsync(long id)
    {
        try
        {
            var result = await _maintenanceGroupService.GetMaintenanceGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound("设备维护组主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设备维护组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:maintenance:group:query", "设备维护组主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaintenanceGroupOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _maintenanceGroupService.GetMaintenanceGroupOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设备维护组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设备维护组主数据DTO</returns>
    [TaktPermission("logistics:maintenance:group:create", "创建设备维护组主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateMaintenanceGroupAsync([FromBody] TaktMaintenanceGroupCreateDto dto)
    {
        try
        {
            var result = await _maintenanceGroupService.CreateMaintenanceGroupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设备维护组主数据
    /// </summary>
    /// <param name="id">设备维护组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设备维护组主数据DTO</returns>
    [TaktPermission("logistics:maintenance:group:update", "更新设备维护组主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaintenanceGroupAsync(long id, [FromBody] TaktMaintenanceGroupUpdateDto dto)
    {
        try
        {
            var result = await _maintenanceGroupService.UpdateMaintenanceGroupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设备维护组主数据
    /// </summary>
    /// <param name="id">设备维护组主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:group:delete", "删除设备维护组主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaintenanceGroupByIdAsync(long id)
    {
        try
        {
            await _maintenanceGroupService.DeleteMaintenanceGroupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设备维护组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:group:delete", "批量删除设备维护组主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaintenanceGroupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _maintenanceGroupService.DeleteMaintenanceGroupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设备维护组主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>设备维护组主数据DTO</returns>
    [TaktPermission("logistics:maintenance:group:update", "更新设备维护组主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMaintenanceGroupStatusAsync([FromBody] TaktMaintenanceGroupStatusDto dto)
    {
        try
        {
            var result = await _maintenanceGroupService.UpdateMaintenanceGroupStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设备维护组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>设备维护组主数据DTO</returns>
    [TaktPermission("logistics:maintenance:group:update", "更新设备维护组主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateMaintenanceGroupSortAsync([FromBody] TaktMaintenanceGroupSortDto dto)
    {
        try
        {
            var result = await _maintenanceGroupService.UpdateMaintenanceGroupSortAsync(dto);
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
    [TaktPermission("logistics:maintenance:group:import", "获取设备维护组主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaintenanceGroupTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _maintenanceGroupService.GetMaintenanceGroupTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设备维护组主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:maintenance:group:import", "导入设备维护组主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaintenanceGroupAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _maintenanceGroupService.ImportMaintenanceGroupAsync(stream, sheetName);
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
    /// 导出设备维护组主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:maintenance:group:export", "导出设备维护组主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaintenanceGroupAsync([FromQuery] TaktMaintenanceGroupQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _maintenanceGroupService.ExportMaintenanceGroupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
