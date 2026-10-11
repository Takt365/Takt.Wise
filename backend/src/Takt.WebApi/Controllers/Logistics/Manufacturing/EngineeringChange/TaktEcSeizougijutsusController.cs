// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizougijutsusController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制技执行控制器
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
/// 设变制技执行控制器
/// 提供设变制技执行的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变制技执行")]
public class TaktEcSeizougijutsusController : TaktControllerBase
{
    private readonly ITaktEcSeizougijutsuService _ecSeizougijutsuService;
    private readonly ITaktEcExecMasterQueryService _ecExecMasterQueryService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecSeizougijutsuService">设变制技执行服务</param>
    /// <param name="ecExecMasterQueryService">左栏设变明细主表查询</param>
    public TaktEcSeizougijutsusController(
        ITaktEcSeizougijutsuService ecSeizougijutsuService,
        ITaktEcExecMasterQueryService ecExecMasterQueryService)
    {
        _ecSeizougijutsuService = ecSeizougijutsuService;
        _ecExecMasterQueryService = ecExecMasterQueryService;
    }

    /// <summary>
    /// 获取设变制技执行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:list", "设变制技执行列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcSeizougijutsuListAsync([FromQuery] TaktEcSeizougijutsuQueryDto queryDto)
    {
        try
        {
            var result = await _ecSeizougijutsuService.GetEcSeizougijutsuListAsync(queryDto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:list", "设变制技执行主表")]
    [HttpGet("masters")]
    public async Task<IActionResult> GetEcSeizougijutsuMasterListAsync([FromQuery] TaktEcDetailQueryDto queryDto)
    {
        try
        {
            var result = await _ecExecMasterQueryService.GetEcDetailMasterListAsync(queryDto, TaktEcDeptCodes.Te);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变制技执行
    /// </summary>
    /// <param name="id">设变制技执行ID</param>
    /// <returns>设变制技执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:query", "设变制技执行详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetEcSeizougijutsuByIdAsync(long id)
    {
        try
        {
            var result = await _ecSeizougijutsuService.GetEcSeizougijutsuByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变制技执行不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变制技执行选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:query", "设变制技执行选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcSeizougijutsuOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ecSeizougijutsuService.GetEcSeizougijutsuOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变制技执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设变制技执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:create", "创建设变制技执行")]
    [HttpPost]
    public async Task<IActionResult> CreateEcSeizougijutsuAsync([FromBody] TaktEcSeizougijutsuCreateDto dto)
    {
        try
        {
            var result = await _ecSeizougijutsuService.CreateEcSeizougijutsuAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变制技执行
    /// </summary>
    /// <param name="id">设变制技执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设变制技执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:update", "更新设变制技执行")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEcSeizougijutsuAsync(long id, [FromBody] TaktEcSeizougijutsuUpdateDto dto)
    {
        try
        {
            var result = await _ecSeizougijutsuService.UpdateEcSeizougijutsuAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变制技执行
    /// </summary>
    /// <param name="id">设变制技执行ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:delete", "删除设变制技执行")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcSeizougijutsuByIdAsync(long id)
    {
        try
        {
            await _ecSeizougijutsuService.DeleteEcSeizougijutsuByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变制技执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:delete", "批量删除设变制技执行")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcSeizougijutsuBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecSeizougijutsuService.DeleteEcSeizougijutsuBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变制技执行停产状态
    /// </summary>
    /// <param name="dto">停产状态 DTO</param>
    /// <returns>设变制技执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:update", "更新设变制技执行停产状态")]
    [HttpPut("discontinued-status")]
    public async Task<IActionResult> UpdateEcSeizougijutsuDiscontinuedStatusAsync([FromBody] TaktEcSeizougijutsuDiscontinuedStatusDto dto)
    {
        try
        {
            var result = await _ecSeizougijutsuService.UpdateEcSeizougijutsuDiscontinuedStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变制技执行作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>设变制技执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:update", "更新设变制技执行作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateEcSeizougijutsuObsoleteAsync([FromBody] TaktEcSeizougijutsuObsoleteDto dto)
    {
        try
        {
            var result = await _ecSeizougijutsuService.UpdateEcSeizougijutsuObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:import", "获取设变制技执行导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcSeizougijutsuTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecSeizougijutsuService.GetEcSeizougijutsuTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变制技执行
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:import", "导入设变制技执行")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcSeizougijutsuAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecSeizougijutsuService.ImportEcSeizougijutsuAsync(stream, sheetName);
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
    /// 导出设变制技执行
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizougijutsu:export", "导出设变制技执行")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcSeizougijutsuAsync([FromQuery] TaktEcSeizougijutsuQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecSeizougijutsuService.ExportEcSeizougijutsuAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
