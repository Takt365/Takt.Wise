// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.VisitorCenter
// 文件名称：TaktVisitorsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：来访接待控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.VisitorCenter;
using Takt.Application.Services.Routine.VisitorCenter;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.VisitorCenter;

/// <summary>
/// 来访接待控制器
/// 提供来访接待的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "来访接待")]
public class TaktVisitorsController : TaktControllerBase
{
    private readonly ITaktVisitorService _visitorService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="visitorService">来访接待服务</param>
    public TaktVisitorsController(ITaktVisitorService visitorService)
    {
        _visitorService = visitorService;
    }

    /// <summary>
    /// 获取来访接待列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:visitor:center:list", "来访接待列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetVisitorListAsync([FromQuery] TaktVisitorQueryDto queryDto)
    {
        try
        {
            var result = await _visitorService.GetVisitorListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取来访接待
    /// </summary>
    /// <param name="id">来访接待ID</param>
    /// <returns>来访接待DTO</returns>
    [TaktPermission("routine:visitor:center:query", "来访接待详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVisitorByIdAsync(long id)
    {
        try
        {
            var result = await _visitorService.GetVisitorByIdAsync(id);
            if (result == null)
            {
                return NotFound("来访接待不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取访客中心来访记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:visitor:center:query", "来访接待选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetVisitorOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _visitorService.GetVisitorOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建来访接待
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>来访接待DTO</returns>
    [TaktPermission("routine:visitor:center:create", "创建来访接待")]
    [HttpPost]
    public async Task<IActionResult> CreateVisitorAsync([FromBody] TaktVisitorCreateDto dto)
    {
        try
        {
            var result = await _visitorService.CreateVisitorAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新来访接待
    /// </summary>
    /// <param name="id">来访接待ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>来访接待DTO</returns>
    [TaktPermission("routine:visitor:center:update", "更新来访接待")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVisitorAsync(long id, [FromBody] TaktVisitorUpdateDto dto)
    {
        try
        {
            var result = await _visitorService.UpdateVisitorAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除来访接待
    /// </summary>
    /// <param name="id">来访接待ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:visitor:center:delete", "删除来访接待")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVisitorByIdAsync(long id)
    {
        try
        {
            await _visitorService.DeleteVisitorByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除来访接待
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:visitor:center:delete", "批量删除来访接待")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteVisitorBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _visitorService.DeleteVisitorBatchAsync(ids);
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
    [TaktPermission("routine:visitor:center:import", "获取来访接待导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetVisitorTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _visitorService.GetVisitorTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入来访接待
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:visitor:center:import", "导入来访接待")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportVisitorAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _visitorService.ImportVisitorAsync(stream, sheetName);
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
    /// 导出来访接待
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:visitor:center:export", "导出来访接待")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportVisitorAsync([FromQuery] TaktVisitorQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _visitorService.ExportVisitorAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
