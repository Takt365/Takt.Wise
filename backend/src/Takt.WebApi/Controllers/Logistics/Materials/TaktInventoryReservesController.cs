// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktInventoryReservesController.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：存货跌价准备控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 存货跌价准备控制器
/// 提供存货跌价准备的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "存货跌价准备")]
public class TaktInventoryReservesController : TaktControllerBase
{
    private readonly ITaktInventoryReserveService _inventoryReserveService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="inventoryReserveService">存货跌价准备服务</param>
    public TaktInventoryReservesController(ITaktInventoryReserveService inventoryReserveService)
    {
        _inventoryReserveService = inventoryReserveService;
    }

    /// <summary>
    /// 获取存货跌价准备列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:inventory:reserve:list", "存货跌价准备列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetInventoryReserveListAsync([FromQuery] TaktInventoryReserveQueryDto queryDto)
    {
        try
        {
            var result = await _inventoryReserveService.GetInventoryReserveListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取存货跌价准备
    /// </summary>
    /// <param name="id">存货跌价准备ID</param>
    /// <returns>存货跌价准备DTO</returns>
    [TaktPermission("logistics:materials:inventory:reserve:query", "存货跌价准备详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInventoryReserveByIdAsync(long id)
    {
        try
        {
            var result = await _inventoryReserveService.GetInventoryReserveByIdAsync(id);
            if (result == null)
            {
                return NotFound("存货跌价准备不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取存货跌价准备选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:inventory:reserve:query", "存货跌价准备选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetInventoryReserveOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _inventoryReserveService.GetInventoryReserveOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建存货跌价准备
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>存货跌价准备DTO</returns>
    [TaktPermission("logistics:materials:inventory:reserve:create", "创建存货跌价准备")]
    [HttpPost]
    public async Task<IActionResult> CreateInventoryReserveAsync([FromBody] TaktInventoryReserveCreateDto dto)
    {
        try
        {
            var result = await _inventoryReserveService.CreateInventoryReserveAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新存货跌价准备
    /// </summary>
    /// <param name="id">存货跌价准备ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>存货跌价准备DTO</returns>
    [TaktPermission("logistics:materials:inventory:reserve:update", "更新存货跌价准备")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInventoryReserveAsync(long id, [FromBody] TaktInventoryReserveUpdateDto dto)
    {
        try
        {
            var result = await _inventoryReserveService.UpdateInventoryReserveAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除存货跌价准备
    /// </summary>
    /// <param name="id">存货跌价准备ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:inventory:reserve:delete", "删除存货跌价准备")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInventoryReserveByIdAsync(long id)
    {
        try
        {
            await _inventoryReserveService.DeleteInventoryReserveByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除存货跌价准备
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:inventory:reserve:delete", "批量删除存货跌价准备")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteInventoryReserveBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _inventoryReserveService.DeleteInventoryReserveBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新存货跌价准备状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>存货跌价准备DTO</returns>
    [TaktPermission("logistics:materials:inventory:reserve:update", "更新存货跌价准备状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateInventoryReserveStatusAsync([FromBody] TaktInventoryReserveStatusDto dto)
    {
        try
        {
            var result = await _inventoryReserveService.UpdateInventoryReserveStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新存货跌价准备排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>存货跌价准备DTO</returns>
    [TaktPermission("logistics:materials:inventory:reserve:update", "更新存货跌价准备排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateInventoryReserveSortAsync([FromBody] TaktInventoryReserveSortDto dto)
    {
        try
        {
            var result = await _inventoryReserveService.UpdateInventoryReserveSortAsync(dto);
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
    [TaktPermission("logistics:materials:inventory:reserve:import", "获取存货跌价准备导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetInventoryReserveTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _inventoryReserveService.GetInventoryReserveTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入存货跌价准备
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:inventory:reserve:import", "导入存货跌价准备")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportInventoryReserveAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _inventoryReserveService.ImportInventoryReserveAsync(stream, sheetName);
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
    /// 导出存货跌价准备
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:inventory:reserve:export", "导出存货跌价准备")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportInventoryReserveAsync([FromQuery] TaktInventoryReserveQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _inventoryReserveService.ExportInventoryReserveAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
