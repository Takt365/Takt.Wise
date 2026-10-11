// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesQuotationsController.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：销售报价控制器
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
/// 销售报价控制器
/// 提供销售报价的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售报价")]
public class TaktSalesQuotationsController : TaktControllerBase
{
    private readonly ITaktSalesQuotationService _salesQuotationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesQuotationService">销售报价服务</param>
    public TaktSalesQuotationsController(ITaktSalesQuotationService salesQuotationService)
    {
        _salesQuotationService = salesQuotationService;
    }

    /// <summary>
    /// 获取销售报价列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:quotation:list", "销售报价列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesQuotationListAsync([FromQuery] TaktSalesQuotationQueryDto queryDto)
    {
        try
        {
            var result = await _salesQuotationService.GetSalesQuotationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售报价
    /// </summary>
    /// <param name="id">销售报价ID</param>
    /// <returns>销售报价DTO</returns>
    [TaktPermission("logistics:sales:quotation:query", "销售报价详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetSalesQuotationByIdAsync(long id)
    {
        try
        {
            var result = await _salesQuotationService.GetSalesQuotationByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售报价不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售报价选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:quotation:query", "销售报价选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesQuotationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salesQuotationService.GetSalesQuotationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售报价
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售报价DTO</returns>
    [TaktPermission("logistics:sales:quotation:create", "创建销售报价")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesQuotationAsync([FromBody] TaktSalesQuotationCreateDto dto)
    {
        try
        {
            var result = await _salesQuotationService.CreateSalesQuotationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售报价
    /// </summary>
    /// <param name="id">销售报价ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售报价DTO</returns>
    [TaktPermission("logistics:sales:quotation:update", "更新销售报价")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesQuotationAsync(long id, [FromBody] TaktSalesQuotationUpdateDto dto)
    {
        try
        {
            var result = await _salesQuotationService.UpdateSalesQuotationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售报价
    /// </summary>
    /// <param name="id">销售报价ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:quotation:delete", "删除销售报价")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesQuotationByIdAsync(long id)
    {
        try
        {
            await _salesQuotationService.DeleteSalesQuotationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售报价
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:quotation:delete", "批量删除销售报价")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesQuotationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesQuotationService.DeleteSalesQuotationBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售报价状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>销售报价DTO</returns>
    [TaktPermission("logistics:sales:quotation:update", "更新销售报价状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSalesQuotationStatusAsync([FromBody] TaktSalesQuotationStatusDto dto)
    {
        try
        {
            var result = await _salesQuotationService.UpdateSalesQuotationStatusAsync(dto);
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
    [TaktPermission("logistics:sales:quotation:import", "获取销售报价导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesQuotationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesQuotationService.GetSalesQuotationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售报价
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:quotation:import", "导入销售报价")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesQuotationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesQuotationService.ImportSalesQuotationAsync(stream, sheetName);
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
    /// 导出销售报价
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:quotation:export", "导出销售报价")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesQuotationAsync([FromQuery] TaktSalesQuotationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesQuotationService.ExportSalesQuotationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
