// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Aps
// 文件名称：TaktPlannedOrdersController.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：计划订单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Application.Services.Logistics.Manufacturing.Aps;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Aps;

/// <summary>
/// 计划订单控制器
/// 提供计划订单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "计划订单")]
public class TaktPlannedOrdersController : TaktControllerBase
{
    private readonly ITaktPlannedOrderService _plannedOrderService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="plannedOrderService">计划订单服务</param>
    public TaktPlannedOrdersController(ITaktPlannedOrderService plannedOrderService)
    {
        _plannedOrderService = plannedOrderService;
    }

    /// <summary>
    /// 获取计划订单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:planned:order:list", "计划订单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPlannedOrderListAsync([FromQuery] TaktPlannedOrderQueryDto queryDto)
    {
        try
        {
            var result = await _plannedOrderService.GetPlannedOrderListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取计划订单
    /// </summary>
    /// <param name="id">计划订单ID</param>
    /// <returns>计划订单DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:planned:order:query", "计划订单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlannedOrderByIdAsync(long id)
    {
        try
        {
            var result = await _plannedOrderService.GetPlannedOrderByIdAsync(id);
            if (result == null)
            {
                return NotFound("计划订单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取计划订单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mrp:planned:order:query", "计划订单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPlannedOrderOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _plannedOrderService.GetPlannedOrderOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建计划订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>计划订单DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:planned:order:create", "创建计划订单")]
    [HttpPost]
    public async Task<IActionResult> CreatePlannedOrderAsync([FromBody] TaktPlannedOrderCreateDto dto)
    {
        try
        {
            var result = await _plannedOrderService.CreatePlannedOrderAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新计划订单
    /// </summary>
    /// <param name="id">计划订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>计划订单DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:planned:order:update", "更新计划订单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlannedOrderAsync(long id, [FromBody] TaktPlannedOrderUpdateDto dto)
    {
        try
        {
            var result = await _plannedOrderService.UpdatePlannedOrderAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除计划订单
    /// </summary>
    /// <param name="id">计划订单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:planned:order:delete", "删除计划订单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlannedOrderByIdAsync(long id)
    {
        try
        {
            await _plannedOrderService.DeletePlannedOrderByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除计划订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:planned:order:delete", "批量删除计划订单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePlannedOrderBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _plannedOrderService.DeletePlannedOrderBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新计划订单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>计划订单DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:planned:order:update", "更新计划订单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePlannedOrderStatusAsync([FromBody] TaktPlannedOrderStatusDto dto)
    {
        try
        {
            var result = await _plannedOrderService.UpdatePlannedOrderStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:mrp:planned:order:import", "获取计划订单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPlannedOrderTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _plannedOrderService.GetPlannedOrderTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入计划订单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:planned:order:import", "导入计划订单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPlannedOrderAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _plannedOrderService.ImportPlannedOrderAsync(stream, sheetName);
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
    /// 导出计划订单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mrp:planned:order:export", "导出计划订单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPlannedOrderAsync([FromQuery] TaktPlannedOrderQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _plannedOrderService.ExportPlannedOrderAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
