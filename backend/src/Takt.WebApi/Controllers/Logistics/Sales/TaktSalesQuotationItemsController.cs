// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesQuotationItemsController.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：销售报价明细控制器
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
/// 销售报价明细控制器
/// 提供销售报价明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售报价明细")]
public class TaktSalesQuotationItemsController : TaktControllerBase
{
    private readonly ITaktSalesQuotationItemService _salesQuotationItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesQuotationItemService">销售报价明细服务</param>
    public TaktSalesQuotationItemsController(ITaktSalesQuotationItemService salesQuotationItemService)
    {
        _salesQuotationItemService = salesQuotationItemService;
    }

    /// <summary>
    /// 获取销售报价明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:quotation:list", "销售报价明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesQuotationItemListAsync([FromQuery] TaktSalesQuotationItemQueryDto queryDto)
    {
        try
        {
            var result = await _salesQuotationItemService.GetSalesQuotationItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售报价明细
    /// </summary>
    /// <param name="id">销售报价明细ID</param>
    /// <returns>销售报价明细DTO</returns>
    [TaktPermission("logistics:sales:quotation:query", "销售报价明细详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetSalesQuotationItemByIdAsync(long id)
    {
        try
        {
            var result = await _salesQuotationItemService.GetSalesQuotationItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售报价明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售报价明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:quotation:query", "销售报价明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesQuotationItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salesQuotationItemService.GetSalesQuotationItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售报价明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售报价明细DTO</returns>
    [TaktPermission("logistics:sales:quotation:create", "创建销售报价明细")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesQuotationItemAsync([FromBody] TaktSalesQuotationItemCreateDto dto)
    {
        try
        {
            var result = await _salesQuotationItemService.CreateSalesQuotationItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售报价明细
    /// </summary>
    /// <param name="id">销售报价明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售报价明细DTO</returns>
    [TaktPermission("logistics:sales:quotation:update", "更新销售报价明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesQuotationItemAsync(long id, [FromBody] TaktSalesQuotationItemUpdateDto dto)
    {
        try
        {
            var result = await _salesQuotationItemService.UpdateSalesQuotationItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售报价明细
    /// </summary>
    /// <param name="id">销售报价明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:quotation:delete", "删除销售报价明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesQuotationItemByIdAsync(long id)
    {
        try
        {
            await _salesQuotationItemService.DeleteSalesQuotationItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售报价明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:quotation:delete", "批量删除销售报价明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesQuotationItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesQuotationItemService.DeleteSalesQuotationItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售报价明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>销售报价明细DTO</returns>
    [TaktPermission("logistics:sales:quotation:update", "更新销售报价明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateSalesQuotationItemObsoleteAsync([FromBody] TaktSalesQuotationItemObsoleteDto dto)
    {
        try
        {
            var result = await _salesQuotationItemService.UpdateSalesQuotationItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:sales:quotation:import", "获取销售报价明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesQuotationItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesQuotationItemService.GetSalesQuotationItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售报价明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:quotation:import", "导入销售报价明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesQuotationItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesQuotationItemService.ImportSalesQuotationItemAsync(stream, sheetName);
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
    /// 导出销售报价明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:quotation:export", "导出销售报价明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesQuotationItemAsync([FromQuery] TaktSalesQuotationItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesQuotationItemService.ExportSalesQuotationItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
