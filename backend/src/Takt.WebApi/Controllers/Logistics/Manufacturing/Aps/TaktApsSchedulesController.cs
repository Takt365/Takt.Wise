// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Aps
// 文件名称：TaktApsSchedulesController.cs
// 创建时间：2026-07-24
// 创建人：Takt365(Cursor AI)
// 功能描述：APS排程主控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Application.Services.Logistics.Manufacturing.Aps;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Aps;

/// <summary>
/// APS排程主控制器
/// 提供APS排程主的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "APS排程主")]
public class TaktApsSchedulesController : TaktControllerBase
{
    private readonly ITaktApsScheduleService _apsScheduleService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsScheduleService">APS排程主服务</param>
    public TaktApsSchedulesController(ITaktApsScheduleService apsScheduleService)
    {
        _apsScheduleService = apsScheduleService;
    }

    /// <summary>
    /// 获取APS排程主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:list", "APS排程主列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetApsScheduleListAsync([FromQuery] TaktApsScheduleQueryDto queryDto)
    {
        try
        {
            var result = await _apsScheduleService.GetApsScheduleListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取APS排程主
    /// </summary>
    /// <param name="id">APS排程主ID</param>
    /// <returns>APS排程主DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:query", "APS排程主详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetApsScheduleByIdAsync(long id)
    {
        try
        {
            var result = await _apsScheduleService.GetApsScheduleByIdAsync(id);
            if (result == null)
            {
                return NotFound("APS排程主不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取APS排程主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:query", "APS排程主选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetApsScheduleOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _apsScheduleService.GetApsScheduleOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建APS排程主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>APS排程主DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:create", "创建APS排程主")]
    [HttpPost]
    public async Task<IActionResult> CreateApsScheduleAsync([FromBody] TaktApsScheduleCreateDto dto)
    {
        try
        {
            var result = await _apsScheduleService.CreateApsScheduleAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS排程主
    /// </summary>
    /// <param name="id">APS排程主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>APS排程主DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:update", "更新APS排程主")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApsScheduleAsync(long id, [FromBody] TaktApsScheduleUpdateDto dto)
    {
        try
        {
            var result = await _apsScheduleService.UpdateApsScheduleAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除APS排程主
    /// </summary>
    /// <param name="id">APS排程主ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:delete", "删除APS排程主")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApsScheduleByIdAsync(long id)
    {
        try
        {
            await _apsScheduleService.DeleteApsScheduleByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除APS排程主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:delete", "批量删除APS排程主")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteApsScheduleBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _apsScheduleService.DeleteApsScheduleBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS排程主状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>APS排程主DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:update", "更新APS排程主状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateApsScheduleStatusAsync([FromBody] TaktApsScheduleStatusDto dto)
    {
        try
        {
            var result = await _apsScheduleService.UpdateApsScheduleStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:aps:schedule:import", "获取APS排程主导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetApsScheduleTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _apsScheduleService.GetApsScheduleTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入APS排程主
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:import", "导入APS排程主")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportApsScheduleAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _apsScheduleService.ImportApsScheduleAsync(stream, sheetName);
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
    /// 导出APS排程主
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:export", "导出APS排程主")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportApsScheduleAsync([FromQuery] TaktApsScheduleQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _apsScheduleService.ExportApsScheduleAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
