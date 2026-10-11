// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Maintenance
// 文件名称：TaktEquipmentsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂设备控制器
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
/// 工厂设备控制器
/// 提供工厂设备的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "工厂设备")]
public class TaktEquipmentsController : TaktControllerBase
{
    private readonly ITaktEquipmentService _equipmentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="equipmentService">工厂设备服务</param>
    public TaktEquipmentsController(ITaktEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    /// <summary>
    /// 获取工厂设备列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:maintenance:equipment:list", "工厂设备列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEquipmentListAsync([FromQuery] TaktEquipmentQueryDto queryDto)
    {
        try
        {
            var result = await _equipmentService.GetEquipmentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设备统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>设备统计</returns>
    [TaktPermission("logistics:maintenance:equipment:list", "工厂设备统计")]
    [HttpGet("equipment-stat")]
    public async Task<IActionResult> GetEquipmentStatAsync([FromQuery] TaktEquipmentStatQueryDto queryDto)
    {
        try
        {
            var result = await _equipmentService.GetEquipmentStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工厂设备
    /// </summary>
    /// <param name="id">工厂设备ID</param>
    /// <returns>工厂设备DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:query", "工厂设备详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEquipmentByIdAsync(long id)
    {
        try
        {
            var result = await _equipmentService.GetEquipmentByIdAsync(id);
            if (result == null)
            {
                return NotFound("工厂设备不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工厂设备选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:maintenance:equipment:query", "工厂设备选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEquipmentOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _equipmentService.GetEquipmentOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工厂设备
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工厂设备DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:create", "创建工厂设备")]
    [HttpPost]
    public async Task<IActionResult> CreateEquipmentAsync([FromBody] TaktEquipmentCreateDto dto)
    {
        try
        {
            var result = await _equipmentService.CreateEquipmentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工厂设备
    /// </summary>
    /// <param name="id">工厂设备ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工厂设备DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:update", "更新工厂设备")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEquipmentAsync(long id, [FromBody] TaktEquipmentUpdateDto dto)
    {
        try
        {
            var result = await _equipmentService.UpdateEquipmentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工厂设备
    /// </summary>
    /// <param name="id">工厂设备ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:equipment:delete", "删除工厂设备")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipmentByIdAsync(long id)
    {
        try
        {
            await _equipmentService.DeleteEquipmentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工厂设备
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:equipment:delete", "批量删除工厂设备")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEquipmentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _equipmentService.DeleteEquipmentBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工厂设备状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>工厂设备DTO</returns>
    [TaktPermission("logistics:maintenance:equipment:update", "更新工厂设备状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEquipmentStatusAsync([FromBody] TaktEquipmentStatusDto dto)
    {
        try
        {
            var result = await _equipmentService.UpdateEquipmentStatusAsync(dto);
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
    [TaktPermission("logistics:maintenance:equipment:import", "获取工厂设备导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEquipmentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _equipmentService.GetEquipmentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工厂设备
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:maintenance:equipment:import", "导入工厂设备")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEquipmentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _equipmentService.ImportEquipmentAsync(stream, sheetName);
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
    /// 导出工厂设备
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:maintenance:equipment:export", "导出工厂设备")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEquipmentAsync([FromQuery] TaktEquipmentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _equipmentService.ExportEquipmentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
