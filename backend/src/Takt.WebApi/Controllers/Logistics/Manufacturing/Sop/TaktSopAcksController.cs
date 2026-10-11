// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Sop
// 文件名称：TaktSopAcksController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP确认控制器
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
/// SOP确认控制器
/// 提供SOP确认的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SOP确认")]
public class TaktSopAcksController : TaktControllerBase
{
    private readonly ITaktSopAckService _sopAckService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopAckService">SOP确认服务</param>
    public TaktSopAcksController(ITaktSopAckService sopAckService)
    {
        _sopAckService = sopAckService;
    }

    /// <summary>
    /// 获取SOP确认列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:sop:ack:list", "SOP确认列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSopAckListAsync([FromQuery] TaktSopAckQueryDto queryDto)
    {
        try
        {
            var result = await _sopAckService.GetSopAckListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取SOP确认
    /// </summary>
    /// <param name="id">SOP确认ID</param>
    /// <returns>SOP确认DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:ack:query", "SOP确认详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSopAckByIdAsync(long id)
    {
        try
        {
            var result = await _sopAckService.GetSopAckByIdAsync(id);
            if (result == null)
            {
                return NotFound("SOP确认不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取SOP确认选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:sop:ack:query", "SOP确认选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSopAckOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sopAckService.GetSopAckOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建SOP确认
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>SOP确认DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:ack:create", "创建SOP确认")]
    [HttpPost]
    public async Task<IActionResult> CreateSopAckAsync([FromBody] TaktSopAckCreateDto dto)
    {
        try
        {
            var result = await _sopAckService.CreateSopAckAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP确认
    /// </summary>
    /// <param name="id">SOP确认ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>SOP确认DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:ack:update", "更新SOP确认")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSopAckAsync(long id, [FromBody] TaktSopAckUpdateDto dto)
    {
        try
        {
            var result = await _sopAckService.UpdateSopAckAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除SOP确认
    /// </summary>
    /// <param name="id">SOP确认ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:ack:delete", "删除SOP确认")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSopAckByIdAsync(long id)
    {
        try
        {
            await _sopAckService.DeleteSopAckByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除SOP确认
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:ack:delete", "批量删除SOP确认")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSopAckBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sopAckService.DeleteSopAckBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:sop:ack:import", "获取SOP确认导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSopAckTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sopAckService.GetSopAckTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入SOP确认
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:sop:ack:import", "导入SOP确认")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSopAckAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sopAckService.ImportSopAckAsync(stream, sheetName);
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
    /// 导出SOP确认
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:sop:ack:export", "导出SOP确认")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSopAckAsync([FromQuery] TaktSopAckQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sopAckService.ExportSopAckAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
