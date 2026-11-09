// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseConditionsController.cs
// 创建时间：2026-10-30
// 创建人：Takt365(Cursor AI)
// 功能描述：采购条件控制器
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
/// 采购条件控制器
/// 提供采购条件的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购条件")]
public class TaktPurchaseConditionsController : TaktControllerBase
{
    private readonly ITaktPurchaseConditionService _purchaseConditionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseConditionService">采购条件服务</param>
    public TaktPurchaseConditionsController(ITaktPurchaseConditionService purchaseConditionService)
    {
        _purchaseConditionService = purchaseConditionService;
    }

    /// <summary>
    /// 获取采购条件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:condition:list", "采购条件列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseConditionListAsync([FromQuery] TaktPurchaseConditionQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseConditionService.GetPurchaseConditionListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购条件
    /// </summary>
    /// <param name="id">采购条件ID</param>
    /// <returns>采购条件DTO</returns>
    [TaktPermission("logistics:procurement:purchase:condition:query", "采购条件详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetPurchaseConditionByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseConditionService.GetPurchaseConditionByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购条件不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:condition:query", "采购条件选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseConditionOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchaseConditionService.GetPurchaseConditionOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购条件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购条件DTO</returns>
    [TaktPermission("logistics:procurement:purchase:condition:create", "创建采购条件")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseConditionAsync([FromBody] TaktPurchaseConditionCreateDto dto)
    {
        try
        {
            var result = await _purchaseConditionService.CreatePurchaseConditionAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购条件
    /// </summary>
    /// <param name="id">采购条件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购条件DTO</returns>
    [TaktPermission("logistics:procurement:purchase:condition:update", "更新采购条件")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseConditionAsync(long id, [FromBody] TaktPurchaseConditionUpdateDto dto)
    {
        try
        {
            var result = await _purchaseConditionService.UpdatePurchaseConditionAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购条件
    /// </summary>
    /// <param name="id">采购条件ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:condition:delete", "删除采购条件")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseConditionByIdAsync(long id)
    {
        try
        {
            await _purchaseConditionService.DeletePurchaseConditionByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购条件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:condition:delete", "批量删除采购条件")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseConditionBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseConditionService.DeletePurchaseConditionBatchAsync(ids);
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
    [TaktPermission("logistics:procurement:purchase:condition:import", "获取采购条件导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseConditionTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseConditionService.GetPurchaseConditionTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购条件
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:condition:import", "导入采购条件")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseConditionAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseConditionService.ImportPurchaseConditionAsync(stream, sheetName);
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
    /// 导出采购条件
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:condition:export", "导出采购条件")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseConditionAsync([FromQuery] TaktPurchaseConditionQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseConditionService.ExportPurchaseConditionAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
