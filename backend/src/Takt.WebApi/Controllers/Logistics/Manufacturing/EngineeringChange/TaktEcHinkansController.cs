// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcHinkansController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变品管执行控制器
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
/// 设变品管执行控制器
/// 提供设变品管执行的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变品管执行")]
public class TaktEcHinkansController : TaktControllerBase
{
    private readonly ITaktEcHinkanService _ecHinkanService;
    private readonly ITaktEcExecMasterQueryService _ecExecMasterQueryService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecHinkanService">设变品管执行服务</param>
    /// <param name="ecExecMasterQueryService">左栏设变明细主表查询</param>
    public TaktEcHinkansController(
        ITaktEcHinkanService ecHinkanService,
        ITaktEcExecMasterQueryService ecExecMasterQueryService)
    {
        _ecHinkanService = ecHinkanService;
        _ecExecMasterQueryService = ecExecMasterQueryService;
    }

    /// <summary>
    /// 获取设变品管执行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:list", "设变品管执行列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcHinkanListAsync([FromQuery] TaktEcHinkanQueryDto queryDto)
    {
        try
        {
            var result = await _ecHinkanService.GetEcHinkanListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变明细主表列表（左栏；TaktEcDetail；权限与本部门 list 一致）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:list", "设变品管执行主表")]
    [HttpGet("masters")]
    public async Task<IActionResult> GetEcHinkanMasterListAsync([FromQuery] TaktEcDetailQueryDto queryDto)
    {
        try
        {
            var result = await _ecExecMasterQueryService.GetEcDetailMasterListAsync(queryDto, TaktEcDeptCodes.Qa);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变品管执行
    /// </summary>
    /// <param name="id">设变品管执行ID</param>
    /// <returns>设变品管执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:query", "设变品管执行详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetEcHinkanByIdAsync(long id)
    {
        try
        {
            var result = await _ecHinkanService.GetEcHinkanByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变品管执行不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变品管执行选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:query", "设变品管执行选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcHinkanOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ecHinkanService.GetEcHinkanOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变品管执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设变品管执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:create", "创建设变品管执行")]
    [HttpPost]
    public async Task<IActionResult> CreateEcHinkanAsync([FromBody] TaktEcHinkanCreateDto dto)
    {
        try
        {
            var result = await _ecHinkanService.CreateEcHinkanAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变品管执行
    /// </summary>
    /// <param name="id">设变品管执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设变品管执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:update", "更新设变品管执行")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEcHinkanAsync(long id, [FromBody] TaktEcHinkanUpdateDto dto)
    {
        try
        {
            var result = await _ecHinkanService.UpdateEcHinkanAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变品管执行
    /// </summary>
    /// <param name="id">设变品管执行ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:delete", "删除设变品管执行")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcHinkanByIdAsync(long id)
    {
        try
        {
            await _ecHinkanService.DeleteEcHinkanByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变品管执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:delete", "批量删除设变品管执行")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcHinkanBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecHinkanService.DeleteEcHinkanBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变品管执行停产状态
    /// </summary>
    /// <param name="dto">停产状态 DTO</param>
    /// <returns>设变品管执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:update", "更新设变品管执行停产状态")]
    [HttpPut("discontinued-status")]
    public async Task<IActionResult> UpdateEcHinkanDiscontinuedStatusAsync([FromBody] TaktEcHinkanDiscontinuedStatusDto dto)
    {
        try
        {
            var result = await _ecHinkanService.UpdateEcHinkanDiscontinuedStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变品管执行作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>设变品管执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:update", "更新设变品管执行作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateEcHinkanObsoleteAsync([FromBody] TaktEcHinkanObsoleteDto dto)
    {
        try
        {
            var result = await _ecHinkanService.UpdateEcHinkanObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:import", "获取设变品管执行导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcHinkanTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecHinkanService.GetEcHinkanTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变品管执行
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:import", "导入设变品管执行")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcHinkanAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecHinkanService.ImportEcHinkanAsync(stream, sheetName);
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
    /// 导出设变品管执行
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:hinkan:export", "导出设变品管执行")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcHinkanAsync([FromQuery] TaktEcHinkanQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecHinkanService.ExportEcHinkanAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
