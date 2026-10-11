// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.VisitorCenter
// 文件名称：TaktVisitorCompanionsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：来访人员控制器
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
/// 来访人员控制器
/// 提供来访人员的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "来访人员")]
public class TaktVisitorCompanionsController : TaktControllerBase
{
    private readonly ITaktVisitorCompanionService _visitorCompanionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="visitorCompanionService">来访人员服务</param>
    public TaktVisitorCompanionsController(ITaktVisitorCompanionService visitorCompanionService)
    {
        _visitorCompanionService = visitorCompanionService;
    }

    /// <summary>
    /// 获取来访人员列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:visitor:center:list", "来访人员列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetVisitorCompanionListAsync([FromQuery] TaktVisitorCompanionQueryDto queryDto)
    {
        try
        {
            var result = await _visitorCompanionService.GetVisitorCompanionListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取来访人员
    /// </summary>
    /// <param name="id">来访人员ID</param>
    /// <returns>来访人员DTO</returns>
    [TaktPermission("routine:visitor:center:query", "来访人员详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVisitorCompanionByIdAsync(long id)
    {
        try
        {
            var result = await _visitorCompanionService.GetVisitorCompanionByIdAsync(id);
            if (result == null)
            {
                return NotFound("来访人员不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取来访人员选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:visitor:center:query", "来访人员选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetVisitorCompanionOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _visitorCompanionService.GetVisitorCompanionOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建来访人员
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>来访人员DTO</returns>
    [TaktPermission("routine:visitor:center:create", "创建来访人员")]
    [HttpPost]
    public async Task<IActionResult> CreateVisitorCompanionAsync([FromBody] TaktVisitorCompanionCreateDto dto)
    {
        try
        {
            var result = await _visitorCompanionService.CreateVisitorCompanionAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新来访人员
    /// </summary>
    /// <param name="id">来访人员ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>来访人员DTO</returns>
    [TaktPermission("routine:visitor:center:update", "更新来访人员")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVisitorCompanionAsync(long id, [FromBody] TaktVisitorCompanionUpdateDto dto)
    {
        try
        {
            var result = await _visitorCompanionService.UpdateVisitorCompanionAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除来访人员
    /// </summary>
    /// <param name="id">来访人员ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:visitor:center:delete", "删除来访人员")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVisitorCompanionByIdAsync(long id)
    {
        try
        {
            await _visitorCompanionService.DeleteVisitorCompanionByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除来访人员
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:visitor:center:delete", "批量删除来访人员")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteVisitorCompanionBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _visitorCompanionService.DeleteVisitorCompanionBatchAsync(ids);
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
    [TaktPermission("routine:visitor:center:import", "获取来访人员导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetVisitorCompanionTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _visitorCompanionService.GetVisitorCompanionTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入来访人员
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:visitor:center:import", "导入来访人员")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportVisitorCompanionAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _visitorCompanionService.ImportVisitorCompanionAsync(stream, sheetName);
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
    /// 导出来访人员
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:visitor:center:export", "导出来访人员")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportVisitorCompanionAsync([FromQuery] TaktVisitorCompanionQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _visitorCompanionService.ExportVisitorCompanionAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
