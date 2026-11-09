// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseConditionItemsController.cs
// 创建时间：2026-10-30
// 创建人：Takt365(Cursor AI)
// 功能描述：采购条件行控制器
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
/// 采购条件行控制器
/// 提供采购条件行的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购条件行")]
public class TaktPurchaseConditionItemsController : TaktControllerBase
{
    private readonly ITaktPurchaseConditionItemService _purchaseConditionItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseConditionItemService">采购条件行服务</param>
    public TaktPurchaseConditionItemsController(ITaktPurchaseConditionItemService purchaseConditionItemService)
    {
        _purchaseConditionItemService = purchaseConditionItemService;
    }

    /// <summary>
    /// 获取采购条件行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:condition:list", "采购条件行列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseConditionItemListAsync([FromQuery] TaktPurchaseConditionItemQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseConditionItemService.GetPurchaseConditionItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购条件行
    /// </summary>
    /// <param name="id">采购条件行ID</param>
    /// <returns>采购条件行DTO</returns>
    [TaktPermission("logistics:procurement:purchase:condition:query", "采购条件行详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetPurchaseConditionItemByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseConditionItemService.GetPurchaseConditionItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购条件行不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购条件行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:condition:query", "采购条件行选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseConditionItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchaseConditionItemService.GetPurchaseConditionItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购条件行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购条件行DTO</returns>
    [TaktPermission("logistics:procurement:purchase:condition:create", "创建采购条件行")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseConditionItemAsync([FromBody] TaktPurchaseConditionItemCreateDto dto)
    {
        try
        {
            var result = await _purchaseConditionItemService.CreatePurchaseConditionItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购条件行
    /// </summary>
    /// <param name="id">采购条件行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购条件行DTO</returns>
    [TaktPermission("logistics:procurement:purchase:condition:update", "更新采购条件行")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseConditionItemAsync(long id, [FromBody] TaktPurchaseConditionItemUpdateDto dto)
    {
        try
        {
            var result = await _purchaseConditionItemService.UpdatePurchaseConditionItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购条件行
    /// </summary>
    /// <param name="id">采购条件行ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:condition:delete", "删除采购条件行")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseConditionItemByIdAsync(long id)
    {
        try
        {
            await _purchaseConditionItemService.DeletePurchaseConditionItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购条件行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:condition:delete", "批量删除采购条件行")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseConditionItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseConditionItemService.DeletePurchaseConditionItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购条件行作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>采购条件行DTO</returns>
    [TaktPermission("logistics:procurement:purchase:condition:update", "更新采购条件行作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdatePurchaseConditionItemObsoleteAsync([FromBody] TaktPurchaseConditionItemObsoleteDto dto)
    {
        try
        {
            var result = await _purchaseConditionItemService.UpdatePurchaseConditionItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:procurement:purchase:condition:import", "获取采购条件行导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseConditionItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseConditionItemService.GetPurchaseConditionItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购条件行
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:condition:import", "导入采购条件行")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseConditionItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseConditionItemService.ImportPurchaseConditionItemAsync(stream, sheetName);
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
    /// 导出采购条件行
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:condition:export", "导出采购条件行")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseConditionItemAsync([FromQuery] TaktPurchaseConditionItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseConditionItemService.ExportPurchaseConditionItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
