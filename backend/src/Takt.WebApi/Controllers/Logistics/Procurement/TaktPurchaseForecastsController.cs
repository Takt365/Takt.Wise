// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseForecastsController.cs
// 创建时间：2026-08-06
// 创建人：Takt365(Cursor AI)
// 功能描述：采购预测控制器
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
/// 采购预测控制器
/// 提供采购预测的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购预测")]
public class TaktPurchaseForecastsController : TaktControllerBase
{
    private readonly ITaktPurchaseForecastService _purchaseForecastService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseForecastService">采购预测服务</param>
    public TaktPurchaseForecastsController(ITaktPurchaseForecastService purchaseForecastService)
    {
        _purchaseForecastService = purchaseForecastService;
    }

    /// <summary>
    /// 获取采购预测列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:forecast:list", "采购预测列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseForecastListAsync([FromQuery] TaktPurchaseForecastQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseForecastService.GetPurchaseForecastListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购预测
    /// </summary>
    /// <param name="id">采购预测ID</param>
    /// <returns>采购预测DTO</returns>
    [TaktPermission("logistics:procurement:purchase:forecast:query", "采购预测详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchaseForecastByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseForecastService.GetPurchaseForecastByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购预测不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购预测选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:forecast:query", "采购预测选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseForecastOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchaseForecastService.GetPurchaseForecastOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购预测
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购预测DTO</returns>
    [TaktPermission("logistics:procurement:purchase:forecast:create", "创建采购预测")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseForecastAsync([FromBody] TaktPurchaseForecastCreateDto dto)
    {
        try
        {
            var result = await _purchaseForecastService.CreatePurchaseForecastAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购预测
    /// </summary>
    /// <param name="id">采购预测ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购预测DTO</returns>
    [TaktPermission("logistics:procurement:purchase:forecast:update", "更新采购预测")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseForecastAsync(long id, [FromBody] TaktPurchaseForecastUpdateDto dto)
    {
        try
        {
            var result = await _purchaseForecastService.UpdatePurchaseForecastAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购预测
    /// </summary>
    /// <param name="id">采购预测ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:forecast:delete", "删除采购预测")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseForecastByIdAsync(long id)
    {
        try
        {
            await _purchaseForecastService.DeletePurchaseForecastByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购预测
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:forecast:delete", "批量删除采购预测")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseForecastBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseForecastService.DeletePurchaseForecastBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购预测状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>采购预测DTO</returns>
    [TaktPermission("logistics:procurement:purchase:forecast:update", "更新采购预测状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePurchaseForecastStatusAsync([FromBody] TaktPurchaseForecastStatusDto dto)
    {
        try
        {
            var result = await _purchaseForecastService.UpdatePurchaseForecastStatusAsync(dto);
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
    [TaktPermission("logistics:procurement:purchase:forecast:import", "获取采购预测导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseForecastTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseForecastService.GetPurchaseForecastTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购预测
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:forecast:import", "导入采购预测")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseForecastAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseForecastService.ImportPurchaseForecastAsync(stream, sheetName);
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
    /// 导出采购预测
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:forecast:export", "导出采购预测")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseForecastAsync([FromQuery] TaktPurchaseForecastQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseForecastService.ExportPurchaseForecastAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
