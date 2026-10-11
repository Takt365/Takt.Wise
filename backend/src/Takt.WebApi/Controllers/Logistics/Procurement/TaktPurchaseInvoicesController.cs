// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseInvoicesController.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：采购发票控制器
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
/// 采购发票控制器
/// 提供采购发票的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购发票")]
public class TaktPurchaseInvoicesController : TaktControllerBase
{
    private readonly ITaktPurchaseInvoiceService _purchaseInvoiceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInvoiceService">采购发票服务</param>
    public TaktPurchaseInvoicesController(ITaktPurchaseInvoiceService purchaseInvoiceService)
    {
        _purchaseInvoiceService = purchaseInvoiceService;
    }

    /// <summary>
    /// 获取采购发票列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:invoice:list", "采购发票列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseInvoiceListAsync([FromQuery] TaktPurchaseInvoiceQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseInvoiceService.GetPurchaseInvoiceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购发票
    /// </summary>
    /// <param name="id">采购发票ID</param>
    /// <returns>采购发票DTO</returns>
    [TaktPermission("logistics:procurement:purchase:invoice:query", "采购发票详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetPurchaseInvoiceByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseInvoiceService.GetPurchaseInvoiceByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购发票不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购发票选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:invoice:query", "采购发票选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseInvoiceOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchaseInvoiceService.GetPurchaseInvoiceOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购发票
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购发票DTO</returns>
    [TaktPermission("logistics:procurement:purchase:invoice:create", "创建采购发票")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseInvoiceAsync([FromBody] TaktPurchaseInvoiceCreateDto dto)
    {
        try
        {
            var result = await _purchaseInvoiceService.CreatePurchaseInvoiceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购发票
    /// </summary>
    /// <param name="id">采购发票ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购发票DTO</returns>
    [TaktPermission("logistics:procurement:purchase:invoice:update", "更新采购发票")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseInvoiceAsync(long id, [FromBody] TaktPurchaseInvoiceUpdateDto dto)
    {
        try
        {
            var result = await _purchaseInvoiceService.UpdatePurchaseInvoiceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购发票
    /// </summary>
    /// <param name="id">采购发票ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:invoice:delete", "删除采购发票")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseInvoiceByIdAsync(long id)
    {
        try
        {
            await _purchaseInvoiceService.DeletePurchaseInvoiceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购发票
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:invoice:delete", "批量删除采购发票")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseInvoiceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseInvoiceService.DeletePurchaseInvoiceBatchAsync(ids);
            return Success("删除成功");
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
    [TaktPermission("logistics:procurement:purchase:invoice:import", "获取采购发票导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseInvoiceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseInvoiceService.GetPurchaseInvoiceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购发票
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:invoice:import", "导入采购发票")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseInvoiceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseInvoiceService.ImportPurchaseInvoiceAsync(stream, sheetName);
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
    /// 导出采购发票
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:invoice:export", "导出采购发票")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseInvoiceAsync([FromQuery] TaktPurchaseInvoiceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseInvoiceService.ExportPurchaseInvoiceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
