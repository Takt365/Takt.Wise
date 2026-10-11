// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Sop
// 文件名称：TaktSopCallsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP安灯呼叫控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Application.Services.Logistics.Manufacturing.Sop;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP安灯呼叫控制器
/// 提供SOP安灯呼叫的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SOP安灯呼叫")]
public class TaktSopCallsController : TaktControllerBase
{
    private readonly ITaktSopCallService _sopCallService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopCallService">SOP安灯呼叫服务</param>
    public TaktSopCallsController(ITaktSopCallService sopCallService)
    {
        _sopCallService = sopCallService;
    }

    /// <summary>
    /// 获取SOP安灯呼叫列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:sop:call:list", "SOP安灯呼叫列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSopCallListAsync([FromQuery] TaktSopCallQueryDto queryDto)
    {
        try
        {
            var result = await _sopCallService.GetSopCallListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取SOP安灯呼叫
    /// </summary>
    /// <param name="id">SOP安灯呼叫ID</param>
    /// <returns>SOP安灯呼叫DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:call:query", "SOP安灯呼叫详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSopCallByIdAsync(long id)
    {
        try
        {
            var result = await _sopCallService.GetSopCallByIdAsync(id);
            if (result == null)
            {
                return NotFound("SOP安灯呼叫不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取SOP安灯呼叫选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:sop:call:query", "SOP安灯呼叫选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSopCallOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sopCallService.GetSopCallOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建SOP安灯呼叫
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>SOP安灯呼叫DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:call:create", "创建SOP安灯呼叫")]
    [HttpPost]
    public async Task<IActionResult> CreateSopCallAsync([FromBody] TaktSopCallCreateDto dto)
    {
        try
        {
            var result = await _sopCallService.CreateSopCallAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP安灯呼叫
    /// </summary>
    /// <param name="id">SOP安灯呼叫ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>SOP安灯呼叫DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:call:update", "更新SOP安灯呼叫")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSopCallAsync(long id, [FromBody] TaktSopCallUpdateDto dto)
    {
        try
        {
            var result = await _sopCallService.UpdateSopCallAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除SOP安灯呼叫
    /// </summary>
    /// <param name="id">SOP安灯呼叫ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:call:delete", "删除SOP安灯呼叫")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSopCallByIdAsync(long id)
    {
        try
        {
            await _sopCallService.DeleteSopCallByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除SOP安灯呼叫
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:call:delete", "批量删除SOP安灯呼叫")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSopCallBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sopCallService.DeleteSopCallBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP安灯呼叫状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>SOP安灯呼叫DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:call:update", "更新SOP安灯呼叫状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSopCallStatusAsync([FromBody] TaktSopCallStatusDto dto)
    {
        try
        {
            var result = await _sopCallService.UpdateSopCallStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:sop:call:import", "获取SOP安灯呼叫导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSopCallTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sopCallService.GetSopCallTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入SOP安灯呼叫
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:sop:call:import", "导入SOP安灯呼叫")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSopCallAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sopCallService.ImportSopCallAsync(stream, sheetName);
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
    /// 导出SOP安灯呼叫
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:sop:call:export", "导出SOP安灯呼叫")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSopCallAsync([FromQuery] TaktSopCallQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sopCallService.ExportSopCallAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
