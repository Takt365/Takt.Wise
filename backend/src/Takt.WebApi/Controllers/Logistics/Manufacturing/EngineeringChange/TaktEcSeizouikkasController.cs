// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizouikkasController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变制一执行控制器
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
/// 设变制一执行控制器
/// 提供设变制一执行的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变制一执行")]
public class TaktEcSeizouikkasController : TaktControllerBase
{
    private readonly ITaktEcSeizouikkaService _ecSeizouikkaService;
    private readonly ITaktEcExecMasterQueryService _ecExecMasterQueryService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecSeizouikkaService">设变制一执行服务</param>
    /// <param name="ecExecMasterQueryService">左栏设变明细主表查询</param>
    public TaktEcSeizouikkasController(
        ITaktEcSeizouikkaService ecSeizouikkaService,
        ITaktEcExecMasterQueryService ecExecMasterQueryService)
    {
        _ecSeizouikkaService = ecSeizouikkaService;
        _ecExecMasterQueryService = ecExecMasterQueryService;
    }

    /// <summary>
    /// 获取设变制一执行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:list", "设变制一执行列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcSeizouikkaListAsync([FromQuery] TaktEcSeizouikkaQueryDto queryDto)
    {
        try
        {
            var result = await _ecSeizouikkaService.GetEcSeizouikkaListAsync(queryDto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:list", "设变制一执行主表")]
    [HttpGet("masters")]
    public async Task<IActionResult> GetEcSeizouikkaMasterListAsync([FromQuery] TaktEcDetailQueryDto queryDto)
    {
        try
        {
            var result = await _ecExecMasterQueryService.GetEcDetailMasterListAsync(queryDto, TaktEcDeptCodes.Assy);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变制一执行
    /// </summary>
    /// <param name="id">设变制一执行ID</param>
    /// <returns>设变制一执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:query", "设变制一执行详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetEcSeizouikkaByIdAsync(long id)
    {
        try
        {
            var result = await _ecSeizouikkaService.GetEcSeizouikkaByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变制一执行不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变制一执行选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:query", "设变制一执行选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcSeizouikkaOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ecSeizouikkaService.GetEcSeizouikkaOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变制一执行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设变制一执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:create", "创建设变制一执行")]
    [HttpPost]
    public async Task<IActionResult> CreateEcSeizouikkaAsync([FromBody] TaktEcSeizouikkaCreateDto dto)
    {
        try
        {
            var result = await _ecSeizouikkaService.CreateEcSeizouikkaAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变制一执行
    /// </summary>
    /// <param name="id">设变制一执行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设变制一执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:update", "更新设变制一执行")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEcSeizouikkaAsync(long id, [FromBody] TaktEcSeizouikkaUpdateDto dto)
    {
        try
        {
            var result = await _ecSeizouikkaService.UpdateEcSeizouikkaAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变制一执行
    /// </summary>
    /// <param name="id">设变制一执行ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:delete", "删除设变制一执行")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcSeizouikkaByIdAsync(long id)
    {
        try
        {
            await _ecSeizouikkaService.DeleteEcSeizouikkaByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变制一执行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:delete", "批量删除设变制一执行")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcSeizouikkaBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecSeizouikkaService.DeleteEcSeizouikkaBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变制造一课执行停产状态
    /// </summary>
    /// <param name="dto">停产状态 DTO</param>
    /// <returns>设变制造一课执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:update", "更新设变制造一课执行停产状态")]
    [HttpPut("discontinued-status")]
    public async Task<IActionResult> UpdateEcSeizouikkaDiscontinuedStatusAsync([FromBody] TaktEcSeizouikkaDiscontinuedStatusDto dto)
    {
        try
        {
            var result = await _ecSeizouikkaService.UpdateEcSeizouikkaDiscontinuedStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变制一执行作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>设变制一执行DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:update", "更新设变制一执行作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateEcSeizouikkaObsoleteAsync([FromBody] TaktEcSeizouikkaObsoleteDto dto)
    {
        try
        {
            var result = await _ecSeizouikkaService.UpdateEcSeizouikkaObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:import", "获取设变制一执行导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcSeizouikkaTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecSeizouikkaService.GetEcSeizouikkaTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变制一执行
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:import", "导入设变制一执行")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcSeizouikkaAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecSeizouikkaService.ImportEcSeizouikkaAsync(stream, sheetName);
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
    /// 导出设变制一执行
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:seizouikka:export", "导出设变制一执行")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcSeizouikkaAsync([FromQuery] TaktEcSeizouikkaQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecSeizouikkaService.ExportEcSeizouikkaAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
