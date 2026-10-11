// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItemArgumentsController.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线工序参数控制器
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
/// 工艺路线工序参数控制器
/// 提供工艺路线工序参数的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "工艺路线工序参数")]
public class TaktRoutingItemArgumentsController : TaktControllerBase
{
    private readonly ITaktRoutingItemArgumentService _routingItemArgumentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="routingItemArgumentService">工艺路线工序参数服务</param>
    public TaktRoutingItemArgumentsController(ITaktRoutingItemArgumentService routingItemArgumentService)
    {
        _routingItemArgumentService = routingItemArgumentService;
    }

    /// <summary>
    /// 获取工艺路线工序参数列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:list", "工艺路线工序参数列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetRoutingItemArgumentListAsync([FromQuery] TaktRoutingItemArgumentQueryDto queryDto)
    {
        try
        {
            var result = await _routingItemArgumentService.GetRoutingItemArgumentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工艺路线工序参数
    /// </summary>
    /// <param name="id">工艺路线工序参数ID</param>
    /// <returns>工艺路线工序参数DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:query", "工艺路线工序参数详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoutingItemArgumentByIdAsync(long id)
    {
        try
        {
            var result = await _routingItemArgumentService.GetRoutingItemArgumentByIdAsync(id);
            if (result == null)
            {
                return NotFound("工艺路线工序参数不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工艺路线工序参数选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:query", "工艺路线工序参数选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetRoutingItemArgumentOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _routingItemArgumentService.GetRoutingItemArgumentOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工艺路线工序参数
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工艺路线工序参数DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:create", "创建工艺路线工序参数")]
    [HttpPost]
    public async Task<IActionResult> CreateRoutingItemArgumentAsync([FromBody] TaktRoutingItemArgumentCreateDto dto)
    {
        try
        {
            var result = await _routingItemArgumentService.CreateRoutingItemArgumentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工艺路线工序参数
    /// </summary>
    /// <param name="id">工艺路线工序参数ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工艺路线工序参数DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:update", "更新工艺路线工序参数")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoutingItemArgumentAsync(long id, [FromBody] TaktRoutingItemArgumentUpdateDto dto)
    {
        try
        {
            var result = await _routingItemArgumentService.UpdateRoutingItemArgumentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工艺路线工序参数
    /// </summary>
    /// <param name="id">工艺路线工序参数ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:delete", "删除工艺路线工序参数")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoutingItemArgumentByIdAsync(long id)
    {
        try
        {
            await _routingItemArgumentService.DeleteRoutingItemArgumentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工艺路线工序参数
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:delete", "批量删除工艺路线工序参数")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteRoutingItemArgumentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _routingItemArgumentService.DeleteRoutingItemArgumentBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工艺路线工序参数排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>工艺路线工序参数DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:update", "更新工艺路线工序参数排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateRoutingItemArgumentSortAsync([FromBody] TaktRoutingItemArgumentSortDto dto)
    {
        try
        {
            var result = await _routingItemArgumentService.UpdateRoutingItemArgumentSortAsync(dto);
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
    [TaktPermission("logistics:manufacturing:bom:routing:import", "获取工艺路线工序参数导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetRoutingItemArgumentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _routingItemArgumentService.GetRoutingItemArgumentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工艺路线工序参数
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:import", "导入工艺路线工序参数")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportRoutingItemArgumentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _routingItemArgumentService.ImportRoutingItemArgumentAsync(stream, sheetName);
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
    /// 导出工艺路线工序参数
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:export", "导出工艺路线工序参数")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportRoutingItemArgumentAsync([FromQuery] TaktRoutingItemArgumentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _routingItemArgumentService.ExportRoutingItemArgumentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
