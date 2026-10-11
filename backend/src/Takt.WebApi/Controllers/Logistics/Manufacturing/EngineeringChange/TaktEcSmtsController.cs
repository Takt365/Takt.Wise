// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSmtsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变SMT执行控制器
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
/// 设变SMT执行控制器
/// 提供设变SMT执行的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变SMT执行")]
public class TaktEcSmtsController : TaktControllerBase
{
    private readonly ITaktEcSmtService _ecSmtService;
    private readonly ITaktEcExecMasterQueryService _ecExecMasterQueryService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecSmtService">设变SMT执行服务</param>
    /// <param name="ecExecMasterQueryService">左栏设变明细主表查询</param>
    public TaktEcSmtsController(
        ITaktEcSmtService ecSmtService,
        ITaktEcExecMasterQueryService ecExecMasterQueryService)
    {
        _ecSmtService = ecSmtService;
        _ecExecMasterQueryService = ecExecMasterQueryService;
    }

    /// <summary>
    /// 获取设变SMT执行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:smt:list", "设变SMT执行列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcSmtListAsync([FromQuery] TaktEcSmtQueryDto queryDto)
    {
        try
        {
            var result = await _ecSmtService.GetEcSmtListAsync(queryDto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:smt:list", "设变SMT执行主表")]
    [HttpGet("masters")]
    public async Task<IActionResult> GetEcSmtMasterListAsync([FromQuery] TaktEcDetailQueryDto queryDto)
    {
        try
        {
            var result = await _ecExecMasterQueryService.GetEcDetailMasterListAsync(queryDto, TaktEcDeptCodes.Pcba);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变SMT执行
    /// </summary>
    /// <param name="id">设变SMT执行ID</param>
    /// <returns>设变SMT执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:smt:query", "设变SMT执行详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetEcSmtByIdAsync(long id)
    {
        try
        {
            var result = await _ecSmtService.GetEcSmtByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变SMT执行不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变SMT执行选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:smt:query", "设变SMT执行选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcSmtOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ecSmtService.GetEcSmtOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变SMT执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设变SMT执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:smt:create", "创建设变SMT执行")]
    [HttpPost]
    public async Task<IActionResult> CreateEcSmtAsync([FromBody] TaktEcSmtCreateDto dto)
    {
        try
        {
            var result = await _ecSmtService.CreateEcSmtAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变SMT执行
    /// </summary>
    /// <param name="id">设变SMT执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设变SMT执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:smt:update", "更新设变SMT执行")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEcSmtAsync(long id, [FromBody] TaktEcSmtUpdateDto dto)
    {
        try
        {
            var result = await _ecSmtService.UpdateEcSmtAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变SMT执行
    /// </summary>
    /// <param name="id">设变SMT执行ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:smt:delete", "删除设变SMT执行")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcSmtByIdAsync(long id)
    {
        try
        {
            await _ecSmtService.DeleteEcSmtByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变SMT执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:smt:delete", "批量删除设变SMT执行")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcSmtBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecSmtService.DeleteEcSmtBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变SMT执行停产状态
    /// </summary>
    /// <param name="dto">停产状态 DTO</param>
    /// <returns>设变SMT执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:smt:update", "更新设变SMT执行停产状态")]
    [HttpPut("discontinued-status")]
    public async Task<IActionResult> UpdateEcSmtDiscontinuedStatusAsync([FromBody] TaktEcSmtDiscontinuedStatusDto dto)
    {
        try
        {
            var result = await _ecSmtService.UpdateEcSmtDiscontinuedStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变SMT执行作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>设变SMT执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:smt:update", "更新设变SMT执行作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateEcSmtObsoleteAsync([FromBody] TaktEcSmtObsoleteDto dto)
    {
        try
        {
            var result = await _ecSmtService.UpdateEcSmtObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:smt:import", "获取设变SMT执行导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcSmtTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecSmtService.GetEcSmtTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变SMT执行
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:smt:import", "导入设变SMT执行")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcSmtAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecSmtService.ImportEcSmtAsync(stream, sheetName);
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
    /// 导出设变SMT执行
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:smt:export", "导出设变SMT执行")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcSmtAsync([FromQuery] TaktEcSmtQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecSmtService.ExportEcSmtAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
