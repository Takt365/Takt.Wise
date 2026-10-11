// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mds
// 文件名称：TaktMasterDemandScheduleLinesController.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：主需求计划MDS行控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Mds;
using Takt.Application.Services.Logistics.Manufacturing.Mds;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Mds;

/// <summary>
/// 主需求计划MDS行控制器
/// 提供主需求计划MDS行的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "主需求计划MDS行")]
public class TaktMasterDemandScheduleLinesController : TaktControllerBase
{
    private readonly ITaktMasterDemandScheduleLineService _masterDemandScheduleLineService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="masterDemandScheduleLineService">主需求计划MDS行服务</param>
    public TaktMasterDemandScheduleLinesController(ITaktMasterDemandScheduleLineService masterDemandScheduleLineService)
    {
        _masterDemandScheduleLineService = masterDemandScheduleLineService;
    }

    /// <summary>
    /// 获取主需求计划MDS行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mds:master:demand:schedule:list", "主需求计划MDS行列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMasterDemandScheduleLineListAsync([FromQuery] TaktMasterDemandScheduleLineQueryDto queryDto)
    {
        try
        {
            var result = await _masterDemandScheduleLineService.GetMasterDemandScheduleLineListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取主需求计划MDS行
    /// </summary>
    /// <param name="id">主需求计划MDS行ID</param>
    /// <returns>主需求计划MDS行DTO</returns>
    [TaktPermission("logistics:manufacturing:mds:master:demand:schedule:query", "主需求计划MDS行详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMasterDemandScheduleLineByIdAsync(long id)
    {
        try
        {
            var result = await _masterDemandScheduleLineService.GetMasterDemandScheduleLineByIdAsync(id);
            if (result == null)
            {
                return NotFound("主需求计划MDS行不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取主需求计划MDS行选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mds:master:demand:schedule:query", "主需求计划MDS行选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMasterDemandScheduleLineOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _masterDemandScheduleLineService.GetMasterDemandScheduleLineOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建主需求计划MDS行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>主需求计划MDS行DTO</returns>
    [TaktPermission("logistics:manufacturing:mds:master:demand:schedule:create", "创建主需求计划MDS行")]
    [HttpPost]
    public async Task<IActionResult> CreateMasterDemandScheduleLineAsync([FromBody] TaktMasterDemandScheduleLineCreateDto dto)
    {
        try
        {
            var result = await _masterDemandScheduleLineService.CreateMasterDemandScheduleLineAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新主需求计划MDS行
    /// </summary>
    /// <param name="id">主需求计划MDS行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>主需求计划MDS行DTO</returns>
    [TaktPermission("logistics:manufacturing:mds:master:demand:schedule:update", "更新主需求计划MDS行")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMasterDemandScheduleLineAsync(long id, [FromBody] TaktMasterDemandScheduleLineUpdateDto dto)
    {
        try
        {
            var result = await _masterDemandScheduleLineService.UpdateMasterDemandScheduleLineAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除主需求计划MDS行
    /// </summary>
    /// <param name="id">主需求计划MDS行ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mds:master:demand:schedule:delete", "删除主需求计划MDS行")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMasterDemandScheduleLineByIdAsync(long id)
    {
        try
        {
            await _masterDemandScheduleLineService.DeleteMasterDemandScheduleLineByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除主需求计划MDS行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mds:master:demand:schedule:delete", "批量删除主需求计划MDS行")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMasterDemandScheduleLineBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _masterDemandScheduleLineService.DeleteMasterDemandScheduleLineBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:mds:master:demand:schedule:import", "获取主需求计划MDS行导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMasterDemandScheduleLineTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _masterDemandScheduleLineService.GetMasterDemandScheduleLineTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入主需求计划MDS行
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mds:master:demand:schedule:import", "导入主需求计划MDS行")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMasterDemandScheduleLineAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _masterDemandScheduleLineService.ImportMasterDemandScheduleLineAsync(stream, sheetName);
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
    /// 导出主需求计划MDS行
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mds:master:demand:schedule:export", "导出主需求计划MDS行")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMasterDemandScheduleLineAsync([FromQuery] TaktMasterDemandScheduleLineQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _masterDemandScheduleLineService.ExportMasterDemandScheduleLineAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
