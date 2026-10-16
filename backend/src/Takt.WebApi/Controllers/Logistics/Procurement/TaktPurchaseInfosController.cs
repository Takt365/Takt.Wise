// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseInfosController.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：采购信息控制器
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
/// 采购信息控制器
/// 提供采购信息的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购信息")]
public class TaktPurchaseInfosController : TaktControllerBase
{
    private readonly ITaktPurchaseInfoService _purchaseInfoService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInfoService">采购信息服务</param>
    public TaktPurchaseInfosController(ITaktPurchaseInfoService purchaseInfoService)
    {
        _purchaseInfoService = purchaseInfoService;
    }

    /// <summary>
    /// 获取采购信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:info:list", "采购信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseInfoListAsync([FromQuery] TaktPurchaseInfoQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseInfoService.GetPurchaseInfoListAsync(queryDto);
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
    /// <param name="id">采购信息ID</param>
    /// <returns>采购信息DTO</returns>
    [TaktPermission("logistics:procurement:purchase:info:query", "采购信息详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetPurchaseInfoByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseInfoService.GetPurchaseInfoByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购信息不存在");
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
    [TaktPermission("logistics:procurement:purchase:info:query", "采购信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseInfoOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchaseInfoService.GetPurchaseInfoOptionsAsync(plantCode, keyword);
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
    /// <returns>采购信息DTO</returns>
    [TaktPermission("logistics:procurement:purchase:info:create", "创建采购信息")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseInfoAsync([FromBody] TaktPurchaseInfoCreateDto dto)
    {
        try
        {
            var result = await _purchaseInfoService.CreatePurchaseInfoAsync(dto);
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
    /// <param name="id">采购信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购信息DTO</returns>
    [TaktPermission("logistics:procurement:purchase:info:update", "更新采购信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseInfoAsync(long id, [FromBody] TaktPurchaseInfoUpdateDto dto)
    {
        try
        {
            var result = await _purchaseInfoService.UpdatePurchaseInfoAsync(id, dto);
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
    /// <param name="id">采购信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:info:delete", "删除采购信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseInfoByIdAsync(long id)
    {
        try
        {
            await _purchaseInfoService.DeletePurchaseInfoByIdAsync(id);
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
    [TaktPermission("logistics:procurement:purchase:info:delete", "批量删除采购信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseInfoBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseInfoService.DeletePurchaseInfoBatchAsync(ids);
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
    [TaktPermission("logistics:procurement:purchase:info:import", "获取采购信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseInfoTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseInfoService.GetPurchaseInfoTemplateAsync(sheetName, templateName);
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
    [TaktPermission("logistics:procurement:purchase:info:import", "导入采购信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseInfoAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseInfoService.ImportPurchaseInfoAsync(stream, sheetName);
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
    [TaktPermission("logistics:procurement:purchase:info:export", "导出采购信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseInfoAsync([FromQuery] TaktPurchaseInfoQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseInfoService.ExportPurchaseInfoAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
