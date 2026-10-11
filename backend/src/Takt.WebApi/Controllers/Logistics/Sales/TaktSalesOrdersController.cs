// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesOrdersController.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：销售订单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Application.Services.Logistics.Sales;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Sales;

/// <summary>
/// 销售订单控制器
/// 提供销售订单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售订单")]
public class TaktSalesOrdersController : TaktControllerBase
{
    private readonly ITaktSalesOrderService _salesOrderService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesOrderService">销售订单服务</param>
    public TaktSalesOrdersController(ITaktSalesOrderService salesOrderService)
    {
        _salesOrderService = salesOrderService;
    }

    /// <summary>
    /// 获取销售订单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:order:list", "销售订单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesOrderListAsync([FromQuery] TaktSalesOrderQueryDto queryDto)
    {
        try
        {
            var result = await _salesOrderService.GetSalesOrderListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售订单
    /// </summary>
    /// <param name="id">销售订单ID</param>
    /// <returns>销售订单DTO</returns>
    [TaktPermission("logistics:sales:order:query", "销售订单详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetSalesOrderByIdAsync(long id)
    {
        try
        {
            var result = await _salesOrderService.GetSalesOrderByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售订单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售订单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:order:query", "销售订单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesOrderOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salesOrderService.GetSalesOrderOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售订单DTO</returns>
    [TaktPermission("logistics:sales:order:create", "创建销售订单")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesOrderAsync([FromBody] TaktSalesOrderCreateDto dto)
    {
        try
        {
            var result = await _salesOrderService.CreateSalesOrderAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售订单
    /// </summary>
    /// <param name="id">销售订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售订单DTO</returns>
    [TaktPermission("logistics:sales:order:update", "更新销售订单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesOrderAsync(long id, [FromBody] TaktSalesOrderUpdateDto dto)
    {
        try
        {
            var result = await _salesOrderService.UpdateSalesOrderAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售订单
    /// </summary>
    /// <param name="id">销售订单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:order:delete", "删除销售订单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesOrderByIdAsync(long id)
    {
        try
        {
            await _salesOrderService.DeleteSalesOrderByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:order:delete", "批量删除销售订单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesOrderBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesOrderService.DeleteSalesOrderBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售订单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>销售订单DTO</returns>
    [TaktPermission("logistics:sales:order:update", "更新销售订单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSalesOrderStatusAsync([FromBody] TaktSalesOrderStatusDto dto)
    {
        try
        {
            var result = await _salesOrderService.UpdateSalesOrderStatusAsync(dto);
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
    [TaktPermission("logistics:sales:order:import", "获取销售订单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesOrderTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesOrderService.GetSalesOrderTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售订单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:order:import", "导入销售订单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesOrderAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesOrderService.ImportSalesOrderAsync(stream, sheetName);
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
    /// 导出销售订单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:order:export", "导出销售订单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesOrderAsync([FromQuery] TaktSalesOrderQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesOrderService.ExportSalesOrderAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
