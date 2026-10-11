// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingsController.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线主控制器
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
/// 工艺路线主控制器
/// 提供工艺路线主的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "工艺路线主")]
public class TaktRoutingsController : TaktControllerBase
{
    private readonly ITaktRoutingService _routingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="routingService">工艺路线主服务</param>
    public TaktRoutingsController(ITaktRoutingService routingService)
    {
        _routingService = routingService;
    }

    /// <summary>
    /// 获取工艺路线主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:list", "工艺路线主列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetRoutingListAsync([FromQuery] TaktRoutingQueryDto queryDto)
    {
        try
        {
            var result = await _routingService.GetRoutingListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工艺路线主
    /// </summary>
    /// <param name="id">工艺路线主ID</param>
    /// <returns>工艺路线主DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:query", "工艺路线主详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoutingByIdAsync(long id)
    {
        try
        {
            var result = await _routingService.GetRoutingByIdAsync(id);
            if (result == null)
            {
                return NotFound("工艺路线主不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工艺路线主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:query", "工艺路线主选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetRoutingOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _routingService.GetRoutingOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工艺路线主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工艺路线主DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:create", "创建工艺路线主")]
    [HttpPost]
    public async Task<IActionResult> CreateRoutingAsync([FromBody] TaktRoutingCreateDto dto)
    {
        try
        {
            var result = await _routingService.CreateRoutingAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工艺路线主
    /// </summary>
    /// <param name="id">工艺路线主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工艺路线主DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:update", "更新工艺路线主")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoutingAsync(long id, [FromBody] TaktRoutingUpdateDto dto)
    {
        try
        {
            var result = await _routingService.UpdateRoutingAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工艺路线主
    /// </summary>
    /// <param name="id">工艺路线主ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:delete", "删除工艺路线主")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoutingByIdAsync(long id)
    {
        try
        {
            await _routingService.DeleteRoutingByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工艺路线主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:delete", "批量删除工艺路线主")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteRoutingBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _routingService.DeleteRoutingBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工艺路线主状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>工艺路线主DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:update", "更新工艺路线主状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateRoutingStatusAsync([FromBody] TaktRoutingStatusDto dto)
    {
        try
        {
            var result = await _routingService.UpdateRoutingStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:bom:routing:import", "获取工艺路线主导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetRoutingTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _routingService.GetRoutingTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工艺路线主
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:import", "导入工艺路线主")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportRoutingAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _routingService.ImportRoutingAsync(stream, sheetName);
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
    /// 导出工艺路线主
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:routing:export", "导出工艺路线主")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportRoutingAsync([FromQuery] TaktRoutingQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _routingService.ExportRoutingAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
