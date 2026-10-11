// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcDetailsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变来源子控制器
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
/// 设变来源子控制器
/// 提供设变来源子的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变来源子")]
public class TaktSourceEcDetailsController : TaktControllerBase
{
    private readonly ITaktSourceEcDetailService _sourceEcDetailService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sourceEcDetailService">设变来源子服务</param>
    public TaktSourceEcDetailsController(ITaktSourceEcDetailService sourceEcDetailService)
    {
        _sourceEcDetailService = sourceEcDetailService;
    }

    /// <summary>
    /// 获取设变来源子列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:list", "设变来源子列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSourceEcDetailListAsync([FromQuery] TaktSourceEcDetailQueryDto queryDto)
    {
        try
        {
            var result = await _sourceEcDetailService.GetSourceEcDetailListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变来源子
    /// </summary>
    /// <param name="id">设变来源子ID</param>
    /// <returns>设变来源子DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:query", "设变来源子详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSourceEcDetailByIdAsync(long id)
    {
        try
        {
            var result = await _sourceEcDetailService.GetSourceEcDetailByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变来源子不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变来源子选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:query", "设变来源子选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSourceEcDetailOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sourceEcDetailService.GetSourceEcDetailOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变来源子
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设变来源子DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:create", "创建设变来源子")]
    [HttpPost]
    public async Task<IActionResult> CreateSourceEcDetailAsync([FromBody] TaktSourceEcDetailCreateDto dto)
    {
        try
        {
            var result = await _sourceEcDetailService.CreateSourceEcDetailAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变来源子
    /// </summary>
    /// <param name="id">设变来源子ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设变来源子DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:update", "更新设变来源子")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSourceEcDetailAsync(long id, [FromBody] TaktSourceEcDetailUpdateDto dto)
    {
        try
        {
            var result = await _sourceEcDetailService.UpdateSourceEcDetailAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变来源子
    /// </summary>
    /// <param name="id">设变来源子ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:delete", "删除设变来源子")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSourceEcDetailByIdAsync(long id)
    {
        try
        {
            await _sourceEcDetailService.DeleteSourceEcDetailByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变来源子
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:delete", "批量删除设变来源子")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSourceEcDetailBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sourceEcDetailService.DeleteSourceEcDetailBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变来源子作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>设变来源子DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:update", "更新设变来源子作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateSourceEcDetailObsoleteAsync([FromBody] TaktSourceEcDetailObsoleteDto dto)
    {
        try
        {
            var result = await _sourceEcDetailService.UpdateSourceEcDetailObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:import", "获取设变来源子导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSourceEcDetailTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sourceEcDetailService.GetSourceEcDetailTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变来源子
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:import", "导入设变来源子")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSourceEcDetailAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sourceEcDetailService.ImportSourceEcDetailAsync(stream, sheetName);
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
    /// 导出设变来源子
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:source:ec:export", "导出设变来源子")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSourceEcDetailAsync([FromQuery] TaktSourceEcDetailQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sourceEcDetailService.ExportSourceEcDetailAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
