// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseInfoScaleQuantitiesController.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：采购阶梯数量控制器
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
/// 采购阶梯数量控制器
/// 提供采购阶梯数量的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购阶梯数量")]
public class TaktPurchaseInfoScaleQuantitiesController : TaktControllerBase
{
    private readonly ITaktPurchaseInfoScaleQuantityService _purchaseInfoScaleQuantityService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInfoScaleQuantityService">采购阶梯数量服务</param>
    public TaktPurchaseInfoScaleQuantitiesController(ITaktPurchaseInfoScaleQuantityService purchaseInfoScaleQuantityService)
    {
        _purchaseInfoScaleQuantityService = purchaseInfoScaleQuantityService;
    }

    /// <summary>
    /// 获取采购阶梯数量列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:info:list", "采购阶梯数量列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseInfoScaleQuantityListAsync([FromQuery] TaktPurchaseInfoScaleQuantityQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseInfoScaleQuantityService.GetPurchaseInfoScaleQuantityListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购阶梯数量
    /// </summary>
    /// <param name="id">采购阶梯数量ID</param>
    /// <returns>采购阶梯数量DTO</returns>
    [TaktPermission("logistics:procurement:purchase:info:query", "采购阶梯数量详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetPurchaseInfoScaleQuantityByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseInfoScaleQuantityService.GetPurchaseInfoScaleQuantityByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购阶梯数量不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购阶梯数量选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:info:query", "采购阶梯数量选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseInfoScaleQuantityOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchaseInfoScaleQuantityService.GetPurchaseInfoScaleQuantityOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购阶梯数量
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购阶梯数量DTO</returns>
    [TaktPermission("logistics:procurement:purchase:info:create", "创建采购阶梯数量")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseInfoScaleQuantityAsync([FromBody] TaktPurchaseInfoScaleQuantityCreateDto dto)
    {
        try
        {
            var result = await _purchaseInfoScaleQuantityService.CreatePurchaseInfoScaleQuantityAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购阶梯数量
    /// </summary>
    /// <param name="id">采购阶梯数量ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购阶梯数量DTO</returns>
    [TaktPermission("logistics:procurement:purchase:info:update", "更新采购阶梯数量")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseInfoScaleQuantityAsync(long id, [FromBody] TaktPurchaseInfoScaleQuantityUpdateDto dto)
    {
        try
        {
            var result = await _purchaseInfoScaleQuantityService.UpdatePurchaseInfoScaleQuantityAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购阶梯数量
    /// </summary>
    /// <param name="id">采购阶梯数量ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:info:delete", "删除采购阶梯数量")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseInfoScaleQuantityByIdAsync(long id)
    {
        try
        {
            await _purchaseInfoScaleQuantityService.DeletePurchaseInfoScaleQuantityByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购阶梯数量
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:info:delete", "批量删除采购阶梯数量")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseInfoScaleQuantityBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseInfoScaleQuantityService.DeletePurchaseInfoScaleQuantityBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购阶梯数量作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>采购阶梯数量DTO</returns>
    [TaktPermission("logistics:procurement:purchase:info:update", "更新采购阶梯数量作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdatePurchaseInfoScaleQuantityObsoleteAsync([FromBody] TaktPurchaseInfoScaleQuantityObsoleteDto dto)
    {
        try
        {
            var result = await _purchaseInfoScaleQuantityService.UpdatePurchaseInfoScaleQuantityObsoleteAsync(dto);
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
    [TaktPermission("logistics:procurement:purchase:info:import", "获取采购阶梯数量导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseInfoScaleQuantityTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseInfoScaleQuantityService.GetPurchaseInfoScaleQuantityTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购阶梯数量
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:info:import", "导入采购阶梯数量")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseInfoScaleQuantityAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseInfoScaleQuantityService.ImportPurchaseInfoScaleQuantityAsync(stream, sheetName);
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
    /// 导出采购阶梯数量
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:info:export", "导出采购阶梯数量")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseInfoScaleQuantityAsync([FromQuery] TaktPurchaseInfoScaleQuantityQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseInfoScaleQuantityService.ExportPurchaseInfoScaleQuantityAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
