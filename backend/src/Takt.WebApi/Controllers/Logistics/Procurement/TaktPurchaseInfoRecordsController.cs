// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseInfoRecordsController.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：采购信息记录控制器
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
/// 采购信息记录控制器
/// 提供采购信息记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购信息记录")]
public class TaktPurchaseInfoRecordsController : TaktControllerBase
{
    private readonly ITaktPurchaseInfoRecordService _purchaseInfoRecordService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInfoRecordService">采购信息记录服务</param>
    public TaktPurchaseInfoRecordsController(ITaktPurchaseInfoRecordService purchaseInfoRecordService)
    {
        _purchaseInfoRecordService = purchaseInfoRecordService;
    }

    /// <summary>
    /// 获取采购信息记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:info:record:list", "采购信息记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseInfoRecordListAsync([FromQuery] TaktPurchaseInfoRecordQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseInfoRecordService.GetPurchaseInfoRecordListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购信息
    /// </summary>
    /// <param name="id">采购信息记录ID</param>
    /// <returns>采购信息记录DTO</returns>
    [TaktPermission("logistics:procurement:purchase:info:record:query", "采购信息记录详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetPurchaseInfoRecordByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseInfoRecordService.GetPurchaseInfoRecordByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购信息记录不存在");
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
    [TaktPermission("logistics:procurement:purchase:info:record:query", "采购信息记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseInfoRecordOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchaseInfoRecordService.GetPurchaseInfoRecordOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购信息记录DTO</returns>
    [TaktPermission("logistics:procurement:purchase:info:record:create", "创建采购信息记录")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseInfoRecordAsync([FromBody] TaktPurchaseInfoRecordCreateDto dto)
    {
        try
        {
            var result = await _purchaseInfoRecordService.CreatePurchaseInfoRecordAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购信息
    /// </summary>
    /// <param name="id">采购信息记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购信息记录DTO</returns>
    [TaktPermission("logistics:procurement:purchase:info:record:update", "更新采购信息记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseInfoRecordAsync(long id, [FromBody] TaktPurchaseInfoRecordUpdateDto dto)
    {
        try
        {
            var result = await _purchaseInfoRecordService.UpdatePurchaseInfoRecordAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购信息
    /// </summary>
    /// <param name="id">采购信息记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:info:record:delete", "删除采购信息记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseInfoRecordByIdAsync(long id)
    {
        try
        {
            await _purchaseInfoRecordService.DeletePurchaseInfoRecordByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:info:record:delete", "批量删除采购信息记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseInfoRecordBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseInfoRecordService.DeletePurchaseInfoRecordBatchAsync(ids);
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
    [TaktPermission("logistics:procurement:purchase:info:record:import", "获取采购信息记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseInfoRecordTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseInfoRecordService.GetPurchaseInfoRecordTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:info:record:import", "导入采购信息记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseInfoRecordAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseInfoRecordService.ImportPurchaseInfoRecordAsync(stream, sheetName);
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
    /// 导出采购信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:info:record:export", "导出采购信息记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseInfoRecordAsync([FromQuery] TaktPurchaseInfoRecordQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseInfoRecordService.ExportPurchaseInfoRecordAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
