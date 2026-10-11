// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变来源主控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变来源主控制器
/// 提供设变来源主的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变来源主")]
public class TaktSourceEcsController : TaktControllerBase
{
    private readonly ITaktSourceEcService _sourceEcService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sourceEcService">设变来源主服务</param>
    public TaktSourceEcsController(ITaktSourceEcService sourceEcService)
    {
        _sourceEcService = sourceEcService;
    }

    /// <summary>
    /// 获取设变来源主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:list", "设变来源主列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSourceEcListAsync([FromQuery] TaktSourceEcQueryDto queryDto)
    {
        try
        {
            var result = await _sourceEcService.GetSourceEcListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变来源主
    /// </summary>
    /// <param name="id">设变来源主ID</param>
    /// <returns>设变来源主DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:query", "设变来源主详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSourceEcByIdAsync(long id)
    {
        try
        {
            var result = await _sourceEcService.GetSourceEcByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变来源主不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变来源主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:query", "设变来源主选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSourceEcOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sourceEcService.GetSourceEcOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变来源主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设变来源主DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:create", "创建设变来源主")]
    [HttpPost]
    public async Task<IActionResult> CreateSourceEcAsync([FromBody] TaktSourceEcCreateDto dto)
    {
        try
        {
            var result = await _sourceEcService.CreateSourceEcAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变来源主
    /// </summary>
    /// <param name="id">设变来源主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设变来源主DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:update", "更新设变来源主")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSourceEcAsync(long id, [FromBody] TaktSourceEcUpdateDto dto)
    {
        try
        {
            var result = await _sourceEcService.UpdateSourceEcAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变来源主
    /// </summary>
    /// <param name="id">设变来源主ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:delete", "删除设变来源主")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSourceEcByIdAsync(long id)
    {
        try
        {
            await _sourceEcService.DeleteSourceEcByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变来源主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:delete", "批量删除设变来源主")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSourceEcBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sourceEcService.DeleteSourceEcBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变来源主状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>设变来源主DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:update", "更新设变来源主状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSourceEcStatusAsync([FromBody] TaktSourceEcStatusDto dto)
    {
        try
        {
            var result = await _sourceEcService.UpdateSourceEcStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:import", "获取设变来源主导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSourceEcTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sourceEcService.GetSourceEcTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变来源主
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:import", "导入设变来源主")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSourceEcAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sourceEcService.ImportSourceEcAsync(stream, sheetName);
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
    /// 导出设变来源主
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:export", "导出设变来源主")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSourceEcAsync([FromQuery] TaktSourceEcQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sourceEcService.ExportSourceEcAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
