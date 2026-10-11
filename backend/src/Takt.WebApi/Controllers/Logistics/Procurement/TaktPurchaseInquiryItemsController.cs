// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseInquiryItemsController.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：采购询价明细控制器
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
/// 采购询价明细控制器
/// 提供采购询价明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购询价明细")]
public class TaktPurchaseInquiryItemsController : TaktControllerBase
{
    private readonly ITaktPurchaseInquiryItemService _purchaseInquiryItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInquiryItemService">采购询价明细服务</param>
    public TaktPurchaseInquiryItemsController(ITaktPurchaseInquiryItemService purchaseInquiryItemService)
    {
        _purchaseInquiryItemService = purchaseInquiryItemService;
    }

    /// <summary>
    /// 获取采购询价明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:list", "采购询价明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseInquiryItemListAsync([FromQuery] TaktPurchaseInquiryItemQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseInquiryItemService.GetPurchaseInquiryItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购询价明细
    /// </summary>
    /// <param name="id">采购询价明细ID</param>
    /// <returns>采购询价明细DTO</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:query", "采购询价明细详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetPurchaseInquiryItemByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseInquiryItemService.GetPurchaseInquiryItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购询价明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购询价明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:query", "采购询价明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseInquiryItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchaseInquiryItemService.GetPurchaseInquiryItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购询价明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购询价明细DTO</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:create", "创建采购询价明细")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseInquiryItemAsync([FromBody] TaktPurchaseInquiryItemCreateDto dto)
    {
        try
        {
            var result = await _purchaseInquiryItemService.CreatePurchaseInquiryItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购询价明细
    /// </summary>
    /// <param name="id">采购询价明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购询价明细DTO</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:update", "更新采购询价明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseInquiryItemAsync(long id, [FromBody] TaktPurchaseInquiryItemUpdateDto dto)
    {
        try
        {
            var result = await _purchaseInquiryItemService.UpdatePurchaseInquiryItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购询价明细
    /// </summary>
    /// <param name="id">采购询价明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:delete", "删除采购询价明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseInquiryItemByIdAsync(long id)
    {
        try
        {
            await _purchaseInquiryItemService.DeletePurchaseInquiryItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购询价明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:delete", "批量删除采购询价明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseInquiryItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseInquiryItemService.DeletePurchaseInquiryItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购询价明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>采购询价明细DTO</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:update", "更新采购询价明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdatePurchaseInquiryItemObsoleteAsync([FromBody] TaktPurchaseInquiryItemObsoleteDto dto)
    {
        try
        {
            var result = await _purchaseInquiryItemService.UpdatePurchaseInquiryItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:procurement:purchase:inquiry:import", "获取采购询价明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseInquiryItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseInquiryItemService.GetPurchaseInquiryItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购询价明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:import", "导入采购询价明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseInquiryItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseInquiryItemService.ImportPurchaseInquiryItemAsync(stream, sheetName);
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
    /// 导出采购询价明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:inquiry:export", "导出采购询价明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseInquiryItemAsync([FromQuery] TaktPurchaseInquiryItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseInquiryItemService.ExportPurchaseInquiryItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
