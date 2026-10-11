// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcBukansController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部管执行控制器
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
/// 设变部管执行控制器
/// 提供设变部管执行的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变部管执行")]
public class TaktEcBukansController : TaktControllerBase
{
    private readonly ITaktEcBukanService _ecBukanService;
    private readonly ITaktEcExecMasterQueryService _ecExecMasterQueryService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecBukanService">设变部管执行服务</param>
    /// <param name="ecExecMasterQueryService">左栏设变明细主表查询</param>
    public TaktEcBukansController(
        ITaktEcBukanService ecBukanService,
        ITaktEcExecMasterQueryService ecExecMasterQueryService)
    {
        _ecBukanService = ecBukanService;
        _ecExecMasterQueryService = ecExecMasterQueryService;
    }

    /// <summary>
    /// 获取设变部管执行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:list", "设变部管执行列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcBukanListAsync([FromQuery] TaktEcBukanQueryDto queryDto)
    {
        try
        {
            var result = await _ecBukanService.GetEcBukanListAsync(queryDto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:list", "设变部管执行主表")]
    [HttpGet("masters")]
    public async Task<IActionResult> GetEcBukanMasterListAsync([FromQuery] TaktEcDetailQueryDto queryDto)
    {
        try
        {
            var result = await _ecExecMasterQueryService.GetEcDetailMasterListAsync(queryDto, TaktEcDeptCodes.Mc);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变部管执行
    /// </summary>
    /// <param name="id">设变部管执行ID</param>
    /// <returns>设变部管执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:query", "设变部管执行详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetEcBukanByIdAsync(long id)
    {
        try
        {
            var result = await _ecBukanService.GetEcBukanByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变部管执行不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变部管执行选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:query", "设变部管执行选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcBukanOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ecBukanService.GetEcBukanOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变部管执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设变部管执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:create", "创建设变部管执行")]
    [HttpPost]
    public async Task<IActionResult> CreateEcBukanAsync([FromBody] TaktEcBukanCreateDto dto)
    {
        try
        {
            var result = await _ecBukanService.CreateEcBukanAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变部管执行
    /// </summary>
    /// <param name="id">设变部管执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设变部管执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:update", "更新设变部管执行")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEcBukanAsync(long id, [FromBody] TaktEcBukanUpdateDto dto)
    {
        try
        {
            var result = await _ecBukanService.UpdateEcBukanAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变部管执行
    /// </summary>
    /// <param name="id">设变部管执行ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:delete", "删除设变部管执行")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcBukanByIdAsync(long id)
    {
        try
        {
            await _ecBukanService.DeleteEcBukanByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变部管执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:delete", "批量删除设变部管执行")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcBukanBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecBukanService.DeleteEcBukanBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变部管执行停产状态
    /// </summary>
    /// <param name="dto">停产状态 DTO</param>
    /// <returns>设变部管执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:update", "更新设变部管执行停产状态")]
    [HttpPut("discontinued-status")]
    public async Task<IActionResult> UpdateEcBukanDiscontinuedStatusAsync([FromBody] TaktEcBukanDiscontinuedStatusDto dto)
    {
        try
        {
            var result = await _ecBukanService.UpdateEcBukanDiscontinuedStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变部管执行作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>设变部管执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:update", "更新设变部管执行作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateEcBukanObsoleteAsync([FromBody] TaktEcBukanObsoleteDto dto)
    {
        try
        {
            var result = await _ecBukanService.UpdateEcBukanObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:import", "获取设变部管执行导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcBukanTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecBukanService.GetEcBukanTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变部管执行
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:import", "导入设变部管执行")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcBukanAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecBukanService.ImportEcBukanAsync(stream, sheetName);
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
    /// 导出设变部管执行
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:bukan:export", "导出设变部管执行")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcBukanAsync([FromQuery] TaktEcBukanQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecBukanService.ExportEcBukanAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
