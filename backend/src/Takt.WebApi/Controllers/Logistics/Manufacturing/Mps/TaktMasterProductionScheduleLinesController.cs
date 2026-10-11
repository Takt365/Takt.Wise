// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mps
// 文件名称：TaktMasterProductionScheduleLinesController.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：主生产计划MPS行控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Mps;
using Takt.Application.Services.Logistics.Manufacturing.Mps;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Mps;

/// <summary>
/// 主生产计划MPS行控制器
/// 提供主生产计划MPS行的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "主生产计划MPS行")]
public class TaktMasterProductionScheduleLinesController : TaktControllerBase
{
    private readonly ITaktMasterProductionScheduleLineService _masterProductionScheduleLineService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="masterProductionScheduleLineService">主生产计划MPS行服务</param>
    public TaktMasterProductionScheduleLinesController(ITaktMasterProductionScheduleLineService masterProductionScheduleLineService)
    {
        _masterProductionScheduleLineService = masterProductionScheduleLineService;
    }

    /// <summary>
    /// 获取主生产计划MPS行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:list", "主生产计划MPS行列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMasterProductionScheduleLineListAsync([FromQuery] TaktMasterProductionScheduleLineQueryDto queryDto)
    {
        try
        {
            var result = await _masterProductionScheduleLineService.GetMasterProductionScheduleLineListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取主生产计划MPS行
    /// </summary>
    /// <param name="id">主生产计划MPS行ID</param>
    /// <returns>主生产计划MPS行DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:query", "主生产计划MPS行详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMasterProductionScheduleLineByIdAsync(long id)
    {
        try
        {
            var result = await _masterProductionScheduleLineService.GetMasterProductionScheduleLineByIdAsync(id);
            if (result == null)
            {
                return NotFound("主生产计划MPS行不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取主生产计划MPS行选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:query", "主生产计划MPS行选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMasterProductionScheduleLineOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _masterProductionScheduleLineService.GetMasterProductionScheduleLineOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建主生产计划MPS行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>主生产计划MPS行DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:create", "创建主生产计划MPS行")]
    [HttpPost]
    public async Task<IActionResult> CreateMasterProductionScheduleLineAsync([FromBody] TaktMasterProductionScheduleLineCreateDto dto)
    {
        try
        {
            var result = await _masterProductionScheduleLineService.CreateMasterProductionScheduleLineAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新主生产计划MPS行
    /// </summary>
    /// <param name="id">主生产计划MPS行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>主生产计划MPS行DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:update", "更新主生产计划MPS行")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMasterProductionScheduleLineAsync(long id, [FromBody] TaktMasterProductionScheduleLineUpdateDto dto)
    {
        try
        {
            var result = await _masterProductionScheduleLineService.UpdateMasterProductionScheduleLineAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除主生产计划MPS行
    /// </summary>
    /// <param name="id">主生产计划MPS行ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:delete", "删除主生产计划MPS行")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMasterProductionScheduleLineByIdAsync(long id)
    {
        try
        {
            await _masterProductionScheduleLineService.DeleteMasterProductionScheduleLineByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除主生产计划MPS行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:delete", "批量删除主生产计划MPS行")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMasterProductionScheduleLineBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _masterProductionScheduleLineService.DeleteMasterProductionScheduleLineBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:import", "获取主生产计划MPS行导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMasterProductionScheduleLineTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _masterProductionScheduleLineService.GetMasterProductionScheduleLineTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入主生产计划MPS行
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:import", "导入主生产计划MPS行")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMasterProductionScheduleLineAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _masterProductionScheduleLineService.ImportMasterProductionScheduleLineAsync(stream, sheetName);
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
    /// 导出主生产计划MPS行
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:export", "导出主生产计划MPS行")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMasterProductionScheduleLineAsync([FromQuery] TaktMasterProductionScheduleLineQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _masterProductionScheduleLineService.ExportMasterProductionScheduleLineAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
