// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseGroupsController.cs
// 创建时间：2026-08-06
// 创建人：Takt365(Cursor AI)
// 功能描述：采购组主数据控制器
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
/// 采购组主数据控制器
/// 提供采购组主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购组主数据")]
public class TaktPurchaseGroupsController : TaktControllerBase
{
    private readonly ITaktPurchaseGroupService _purchaseGroupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseGroupService">采购组主数据服务</param>
    public TaktPurchaseGroupsController(ITaktPurchaseGroupService purchaseGroupService)
    {
        _purchaseGroupService = purchaseGroupService;
    }

    /// <summary>
    /// 获取采购组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:group:list", "采购组主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseGroupListAsync([FromQuery] TaktPurchaseGroupQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseGroupService.GetPurchaseGroupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购组主数据
    /// </summary>
    /// <param name="id">采购组主数据ID</param>
    /// <returns>采购组主数据DTO</returns>
    [TaktPermission("logistics:procurement:purchase:group:query", "采购组主数据详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchaseGroupByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseGroupService.GetPurchaseGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购组主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购组主数据选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:group:query", "采购组主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseGroupOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchaseGroupService.GetPurchaseGroupOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购组主数据DTO</returns>
    [TaktPermission("logistics:procurement:purchase:group:create", "创建采购组主数据")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseGroupAsync([FromBody] TaktPurchaseGroupCreateDto dto)
    {
        try
        {
            var result = await _purchaseGroupService.CreatePurchaseGroupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购组主数据
    /// </summary>
    /// <param name="id">采购组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购组主数据DTO</returns>
    [TaktPermission("logistics:procurement:purchase:group:update", "更新采购组主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseGroupAsync(long id, [FromBody] TaktPurchaseGroupUpdateDto dto)
    {
        try
        {
            var result = await _purchaseGroupService.UpdatePurchaseGroupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购组主数据
    /// </summary>
    /// <param name="id">采购组主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:group:delete", "删除采购组主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseGroupByIdAsync(long id)
    {
        try
        {
            await _purchaseGroupService.DeletePurchaseGroupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:group:delete", "批量删除采购组主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseGroupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseGroupService.DeletePurchaseGroupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购组主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>采购组主数据DTO</returns>
    [TaktPermission("logistics:procurement:purchase:group:update", "更新采购组主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePurchaseGroupStatusAsync([FromBody] TaktPurchaseGroupStatusDto dto)
    {
        try
        {
            var result = await _purchaseGroupService.UpdatePurchaseGroupStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>采购组主数据DTO</returns>
    [TaktPermission("logistics:procurement:purchase:group:update", "更新采购组主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdatePurchaseGroupSortAsync([FromBody] TaktPurchaseGroupSortDto dto)
    {
        try
        {
            var result = await _purchaseGroupService.UpdatePurchaseGroupSortAsync(dto);
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
    [TaktPermission("logistics:procurement:purchase:group:import", "获取采购组主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseGroupTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseGroupService.GetPurchaseGroupTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购组主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:group:import", "导入采购组主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseGroupAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseGroupService.ImportPurchaseGroupAsync(stream, sheetName);
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
    /// 导出采购组主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:group:export", "导出采购组主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseGroupAsync([FromQuery] TaktPurchaseGroupQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseGroupService.ExportPurchaseGroupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
