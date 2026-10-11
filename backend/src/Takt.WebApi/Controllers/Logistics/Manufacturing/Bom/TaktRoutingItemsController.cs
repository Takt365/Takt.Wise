// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItemsController.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// 工艺路线明细控制器
/// 提供工艺路线明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "工艺路线明细")]
public class TaktRoutingItemsController : TaktControllerBase
{
    private readonly ITaktRoutingItemService _routingItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="routingItemService">工艺路线明细服务</param>
    public TaktRoutingItemsController(ITaktRoutingItemService routingItemService)
    {
        _routingItemService = routingItemService;
    }

    /// <summary>
    /// 获取工艺路线明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:list", "工艺路线明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetRoutingItemListAsync([FromQuery] TaktRoutingItemQueryDto queryDto)
    {
        try
        {
            var result = await _routingItemService.GetRoutingItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工艺路线明细
    /// </summary>
    /// <param name="id">工艺路线明细ID</param>
    /// <returns>工艺路线明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:query", "工艺路线明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoutingItemByIdAsync(long id)
    {
        try
        {
            var result = await _routingItemService.GetRoutingItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("工艺路线明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工艺路线明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:query", "工艺路线明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetRoutingItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _routingItemService.GetRoutingItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工艺路线明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工艺路线明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:create", "创建工艺路线明细")]
    [HttpPost]
    public async Task<IActionResult> CreateRoutingItemAsync([FromBody] TaktRoutingItemCreateDto dto)
    {
        try
        {
            var result = await _routingItemService.CreateRoutingItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工艺路线明细
    /// </summary>
    /// <param name="id">工艺路线明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工艺路线明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:update", "更新工艺路线明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoutingItemAsync(long id, [FromBody] TaktRoutingItemUpdateDto dto)
    {
        try
        {
            var result = await _routingItemService.UpdateRoutingItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工艺路线明细
    /// </summary>
    /// <param name="id">工艺路线明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:delete", "删除工艺路线明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoutingItemByIdAsync(long id)
    {
        try
        {
            await _routingItemService.DeleteRoutingItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工艺路线明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:delete", "批量删除工艺路线明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteRoutingItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _routingItemService.DeleteRoutingItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工艺路线明细排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>工艺路线明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:update", "更新工艺路线明细排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateRoutingItemSortAsync([FromBody] TaktRoutingItemSortDto dto)
    {
        try
        {
            var result = await _routingItemService.UpdateRoutingItemSortAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工艺路线明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>工艺路线明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:update", "更新工艺路线明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateRoutingItemObsoleteAsync([FromBody] TaktRoutingItemObsoleteDto dto)
    {
        try
        {
            var result = await _routingItemService.UpdateRoutingItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:bom:routing:import", "获取工艺路线明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetRoutingItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _routingItemService.GetRoutingItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工艺路线明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:import", "导入工艺路线明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportRoutingItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _routingItemService.ImportRoutingItemAsync(stream, sheetName);
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
    /// 导出工艺路线明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:export", "导出工艺路线明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportRoutingItemAsync([FromQuery] TaktRoutingItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _routingItemService.ExportRoutingItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
