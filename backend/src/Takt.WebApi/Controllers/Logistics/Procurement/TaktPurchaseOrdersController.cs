// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseOrdersController.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：采购订单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Procurement;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Procurement;

/// <summary>
/// 采购订单控制器
/// 提供采购订单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购订单")]
public class TaktPurchaseOrdersController : TaktControllerBase
{
    private readonly ITaktPurchaseOrderService _purchaseOrderService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseOrderService">采购订单服务</param>
    public TaktPurchaseOrdersController(ITaktPurchaseOrderService purchaseOrderService)
    {
        _purchaseOrderService = purchaseOrderService;
    }

    /// <summary>
    /// 获取采购订单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:order:list", "采购订单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseOrderListAsync([FromQuery] TaktPurchaseOrderQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseOrderService.GetPurchaseOrderListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购订单
    /// </summary>
    /// <param name="id">采购订单ID</param>
    /// <returns>采购订单DTO</returns>
    [TaktPermission("logistics:procurement:purchase:order:query", "采购订单详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetPurchaseOrderByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseOrderService.GetPurchaseOrderByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购订单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购订单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:order:query", "采购订单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseOrderOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchaseOrderService.GetPurchaseOrderOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购订单DTO</returns>
    [TaktPermission("logistics:procurement:purchase:order:create", "创建采购订单")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseOrderAsync([FromBody] TaktPurchaseOrderCreateDto dto)
    {
        try
        {
            var result = await _purchaseOrderService.CreatePurchaseOrderAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购订单
    /// </summary>
    /// <param name="id">采购订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购订单DTO</returns>
    [TaktPermission("logistics:procurement:purchase:order:update", "更新采购订单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseOrderAsync(long id, [FromBody] TaktPurchaseOrderUpdateDto dto)
    {
        try
        {
            var result = await _purchaseOrderService.UpdatePurchaseOrderAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购订单
    /// </summary>
    /// <param name="id">采购订单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:order:delete", "删除采购订单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseOrderByIdAsync(long id)
    {
        try
        {
            await _purchaseOrderService.DeletePurchaseOrderByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:order:delete", "批量删除采购订单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseOrderBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseOrderService.DeletePurchaseOrderBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购订单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>采购订单DTO</returns>
    [TaktPermission("logistics:procurement:purchase:order:update", "更新采购订单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePurchaseOrderStatusAsync([FromBody] TaktPurchaseOrderStatusDto dto)
    {
        try
        {
            var result = await _purchaseOrderService.UpdatePurchaseOrderStatusAsync(dto);
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
    [TaktPermission("logistics:procurement:purchase:order:import", "获取采购订单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseOrderTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseOrderService.GetPurchaseOrderTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购订单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:order:import", "导入采购订单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseOrderAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseOrderService.ImportPurchaseOrderAsync(stream, sheetName);
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
    /// 导出采购订单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:order:export", "导出采购订单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseOrderAsync([FromQuery] TaktPurchaseOrderQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseOrderService.ExportPurchaseOrderAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
