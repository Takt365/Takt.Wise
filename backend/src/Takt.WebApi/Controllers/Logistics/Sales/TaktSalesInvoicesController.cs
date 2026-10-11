// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesInvoicesController.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：销售发票控制器
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
/// 销售发票控制器
/// 提供销售发票的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售发票")]
public class TaktSalesInvoicesController : TaktControllerBase
{
    private readonly ITaktSalesInvoiceService _salesInvoiceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesInvoiceService">销售发票服务</param>
    public TaktSalesInvoicesController(ITaktSalesInvoiceService salesInvoiceService)
    {
        _salesInvoiceService = salesInvoiceService;
    }

    /// <summary>
    /// 获取销售发票列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:invoice:list", "销售发票列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesInvoiceListAsync([FromQuery] TaktSalesInvoiceQueryDto queryDto)
    {
        try
        {
            var result = await _salesInvoiceService.GetSalesInvoiceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售发票
    /// </summary>
    /// <param name="id">销售发票ID</param>
    /// <returns>销售发票DTO</returns>
    [TaktPermission("logistics:sales:invoice:query", "销售发票详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetSalesInvoiceByIdAsync(long id)
    {
        try
        {
            var result = await _salesInvoiceService.GetSalesInvoiceByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售发票不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售发票选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:invoice:query", "销售发票选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesInvoiceOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salesInvoiceService.GetSalesInvoiceOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售发票
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售发票DTO</returns>
    [TaktPermission("logistics:sales:invoice:create", "创建销售发票")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesInvoiceAsync([FromBody] TaktSalesInvoiceCreateDto dto)
    {
        try
        {
            var result = await _salesInvoiceService.CreateSalesInvoiceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售发票
    /// </summary>
    /// <param name="id">销售发票ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售发票DTO</returns>
    [TaktPermission("logistics:sales:invoice:update", "更新销售发票")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesInvoiceAsync(long id, [FromBody] TaktSalesInvoiceUpdateDto dto)
    {
        try
        {
            var result = await _salesInvoiceService.UpdateSalesInvoiceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售发票
    /// </summary>
    /// <param name="id">销售发票ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:invoice:delete", "删除销售发票")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesInvoiceByIdAsync(long id)
    {
        try
        {
            await _salesInvoiceService.DeleteSalesInvoiceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售发票
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:invoice:delete", "批量删除销售发票")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesInvoiceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesInvoiceService.DeleteSalesInvoiceBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售发票状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>销售发票DTO</returns>
    [TaktPermission("logistics:sales:invoice:update", "更新销售发票状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSalesInvoiceStatusAsync([FromBody] TaktSalesInvoiceStatusDto dto)
    {
        try
        {
            var result = await _salesInvoiceService.UpdateSalesInvoiceStatusAsync(dto);
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
    [TaktPermission("logistics:sales:invoice:import", "获取销售发票导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesInvoiceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesInvoiceService.GetSalesInvoiceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售发票
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:invoice:import", "导入销售发票")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesInvoiceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesInvoiceService.ImportSalesInvoiceAsync(stream, sheetName);
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
    /// 导出销售发票
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:invoice:export", "导出销售发票")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesInvoiceAsync([FromQuery] TaktSalesInvoiceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesInvoiceService.ExportSalesInvoiceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
