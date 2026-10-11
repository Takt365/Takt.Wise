// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Attendance
// 文件名称：TaktOvertimeItemsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：加班明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Attendance;
using Takt.Application.Services.HumanResource.Attendance;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Attendance;

/// <summary>
/// 加班明细控制器
/// 提供加班明细的 REST API
/// </summary>
[ApiModule(5, "考勤管理")]
[Route("api/[controller]", Name = "加班明细")]
public class TaktOvertimeItemsController : TaktControllerBase
{
    private readonly ITaktOvertimeItemService _overtimeItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="overtimeItemService">加班明细服务</param>
    public TaktOvertimeItemsController(ITaktOvertimeItemService overtimeItemService)
    {
        _overtimeItemService = overtimeItemService;
    }

    /// <summary>
    /// 获取加班明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:attendance:overtime:list", "加班明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetOvertimeItemListAsync([FromQuery] TaktOvertimeItemQueryDto queryDto)
    {
        try
        {
            var result = await _overtimeItemService.GetOvertimeItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取加班明细
    /// </summary>
    /// <param name="id">加班明细ID</param>
    /// <returns>加班明细DTO</returns>
    [TaktPermission("human:resource:attendance:overtime:query", "加班明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOvertimeItemByIdAsync(long id)
    {
        try
        {
            var result = await _overtimeItemService.GetOvertimeItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("加班明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取加班明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:attendance:overtime:query", "加班明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetOvertimeItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _overtimeItemService.GetOvertimeItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建加班明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>加班明细DTO</returns>
    [TaktPermission("human:resource:attendance:overtime:create", "创建加班明细")]
    [HttpPost]
    public async Task<IActionResult> CreateOvertimeItemAsync([FromBody] TaktOvertimeItemCreateDto dto)
    {
        try
        {
            var result = await _overtimeItemService.CreateOvertimeItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新加班明细
    /// </summary>
    /// <param name="id">加班明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>加班明细DTO</returns>
    [TaktPermission("human:resource:attendance:overtime:update", "更新加班明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOvertimeItemAsync(long id, [FromBody] TaktOvertimeItemUpdateDto dto)
    {
        try
        {
            var result = await _overtimeItemService.UpdateOvertimeItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除加班明细
    /// </summary>
    /// <param name="id">加班明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:overtime:delete", "删除加班明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOvertimeItemByIdAsync(long id)
    {
        try
        {
            await _overtimeItemService.DeleteOvertimeItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除加班明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:attendance:overtime:delete", "批量删除加班明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteOvertimeItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _overtimeItemService.DeleteOvertimeItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新加班明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>加班明细DTO</returns>
    [TaktPermission("human:resource:attendance:overtime:update", "更新加班明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateOvertimeItemObsoleteAsync([FromBody] TaktOvertimeItemObsoleteDto dto)
    {
        try
        {
            var result = await _overtimeItemService.UpdateOvertimeItemObsoleteAsync(dto);
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
    [TaktPermission("human:resource:attendance:overtime:import", "获取加班明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetOvertimeItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _overtimeItemService.GetOvertimeItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入加班明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:attendance:overtime:import", "导入加班明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportOvertimeItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _overtimeItemService.ImportOvertimeItemAsync(stream, sheetName);
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
    /// 导出加班明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:attendance:overtime:export", "导出加班明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportOvertimeItemAsync([FromQuery] TaktOvertimeItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _overtimeItemService.ExportOvertimeItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
