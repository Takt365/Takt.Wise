// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mps
// 文件名称：TaktMasterProductionSchedulesController.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：主生产计划MPS头控制器
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
/// 主生产计划MPS头控制器
/// 提供主生产计划MPS头的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "主生产计划MPS头")]
public class TaktMasterProductionSchedulesController : TaktControllerBase
{
    private readonly ITaktMasterProductionScheduleService _masterProductionScheduleService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="masterProductionScheduleService">主生产计划MPS头服务</param>
    public TaktMasterProductionSchedulesController(ITaktMasterProductionScheduleService masterProductionScheduleService)
    {
        _masterProductionScheduleService = masterProductionScheduleService;
    }

    /// <summary>
    /// 获取主生产计划MPS头列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:list", "主生产计划MPS头列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMasterProductionScheduleListAsync([FromQuery] TaktMasterProductionScheduleQueryDto queryDto)
    {
        try
        {
            var result = await _masterProductionScheduleService.GetMasterProductionScheduleListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取主生产计划MPS头
    /// </summary>
    /// <param name="id">主生产计划MPS头ID</param>
    /// <returns>主生产计划MPS头DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:query", "主生产计划MPS头详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMasterProductionScheduleByIdAsync(long id)
    {
        try
        {
            var result = await _masterProductionScheduleService.GetMasterProductionScheduleByIdAsync(id);
            if (result == null)
            {
                return NotFound("主生产计划MPS头不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取主生产计划MPS头选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:query", "主生产计划MPS头选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMasterProductionScheduleOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _masterProductionScheduleService.GetMasterProductionScheduleOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建主生产计划MPS头
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>主生产计划MPS头DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:create", "创建主生产计划MPS头")]
    [HttpPost]
    public async Task<IActionResult> CreateMasterProductionScheduleAsync([FromBody] TaktMasterProductionScheduleCreateDto dto)
    {
        try
        {
            var result = await _masterProductionScheduleService.CreateMasterProductionScheduleAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新主生产计划MPS头
    /// </summary>
    /// <param name="id">主生产计划MPS头ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>主生产计划MPS头DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:update", "更新主生产计划MPS头")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMasterProductionScheduleAsync(long id, [FromBody] TaktMasterProductionScheduleUpdateDto dto)
    {
        try
        {
            var result = await _masterProductionScheduleService.UpdateMasterProductionScheduleAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除主生产计划MPS头
    /// </summary>
    /// <param name="id">主生产计划MPS头ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:delete", "删除主生产计划MPS头")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMasterProductionScheduleByIdAsync(long id)
    {
        try
        {
            await _masterProductionScheduleService.DeleteMasterProductionScheduleByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除主生产计划MPS头
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:delete", "批量删除主生产计划MPS头")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMasterProductionScheduleBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _masterProductionScheduleService.DeleteMasterProductionScheduleBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新主生产计划MPS头状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>主生产计划MPS头DTO</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:update", "更新主生产计划MPS头状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMasterProductionScheduleStatusAsync([FromBody] TaktMasterProductionScheduleStatusDto dto)
    {
        try
        {
            var result = await _masterProductionScheduleService.UpdateMasterProductionScheduleStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:import", "获取主生产计划MPS头导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMasterProductionScheduleTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _masterProductionScheduleService.GetMasterProductionScheduleTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入主生产计划MPS头
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:import", "导入主生产计划MPS头")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMasterProductionScheduleAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _masterProductionScheduleService.ImportMasterProductionScheduleAsync(stream, sheetName);
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
    /// 导出主生产计划MPS头
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mps:master:production:schedule:export", "导出主生产计划MPS头")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMasterProductionScheduleAsync([FromQuery] TaktMasterProductionScheduleQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _masterProductionScheduleService.ExportMasterProductionScheduleAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
