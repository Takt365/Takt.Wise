// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktPurchaseSalesInventoriesController.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：进销存控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Application.Services.Accounting.Financial;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Accounting.Financial;

/// <summary>
/// 进销存控制器
/// 提供进销存的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "进销存")]
public class TaktPurchaseSalesInventoriesController : TaktControllerBase
{
    private readonly ITaktPurchaseSalesInventoryService _purchaseSalesInventoryService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseSalesInventoryService">进销存服务</param>
    public TaktPurchaseSalesInventoriesController(ITaktPurchaseSalesInventoryService purchaseSalesInventoryService)
    {
        _purchaseSalesInventoryService = purchaseSalesInventoryService;
    }

    /// <summary>
    /// 获取进销存列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:purchase:sales:inventory:list", "进销存列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseSalesInventoryListAsync([FromQuery] TaktPurchaseSalesInventoryQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseSalesInventoryService.GetPurchaseSalesInventoryListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取进销存
    /// </summary>
    /// <param name="id">进销存ID</param>
    /// <returns>进销存DTO</returns>
    [TaktPermission("accounting:financial:purchase:sales:inventory:query", "进销存详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchaseSalesInventoryByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseSalesInventoryService.GetPurchaseSalesInventoryByIdAsync(id);
            if (result == null)
            {
                return NotFound("进销存不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取进销存选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:purchase:sales:inventory:query", "进销存选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseSalesInventoryOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchaseSalesInventoryService.GetPurchaseSalesInventoryOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建进销存
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>进销存DTO</returns>
    [TaktPermission("accounting:financial:purchase:sales:inventory:create", "创建进销存")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseSalesInventoryAsync([FromBody] TaktPurchaseSalesInventoryCreateDto dto)
    {
        try
        {
            var result = await _purchaseSalesInventoryService.CreatePurchaseSalesInventoryAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新进销存
    /// </summary>
    /// <param name="id">进销存ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>进销存DTO</returns>
    [TaktPermission("accounting:financial:purchase:sales:inventory:update", "更新进销存")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseSalesInventoryAsync(long id, [FromBody] TaktPurchaseSalesInventoryUpdateDto dto)
    {
        try
        {
            var result = await _purchaseSalesInventoryService.UpdatePurchaseSalesInventoryAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除进销存
    /// </summary>
    /// <param name="id">进销存ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:purchase:sales:inventory:delete", "删除进销存")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseSalesInventoryByIdAsync(long id)
    {
        try
        {
            await _purchaseSalesInventoryService.DeletePurchaseSalesInventoryByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除进销存
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:purchase:sales:inventory:delete", "批量删除进销存")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseSalesInventoryBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseSalesInventoryService.DeletePurchaseSalesInventoryBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新进销存状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>进销存DTO</returns>
    [TaktPermission("accounting:financial:purchase:sales:inventory:update", "更新进销存状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePurchaseSalesInventoryStatusAsync([FromBody] TaktPurchaseSalesInventoryStatusDto dto)
    {
        try
        {
            var result = await _purchaseSalesInventoryService.UpdatePurchaseSalesInventoryStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新进销存排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>进销存DTO</returns>
    [TaktPermission("accounting:financial:purchase:sales:inventory:update", "更新进销存排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdatePurchaseSalesInventorySortAsync([FromBody] TaktPurchaseSalesInventorySortDto dto)
    {
        try
        {
            var result = await _purchaseSalesInventoryService.UpdatePurchaseSalesInventorySortAsync(dto);
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
    [TaktPermission("accounting:financial:purchase:sales:inventory:import", "获取进销存导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseSalesInventoryTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseSalesInventoryService.GetPurchaseSalesInventoryTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入进销存
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:purchase:sales:inventory:import", "导入进销存")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseSalesInventoryAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseSalesInventoryService.ImportPurchaseSalesInventoryAsync(stream, sheetName);
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
    /// 导出进销存
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:purchase:sales:inventory:export", "导出进销存")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseSalesInventoryAsync([FromQuery] TaktPurchaseSalesInventoryQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseSalesInventoryService.ExportPurchaseSalesInventoryAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
