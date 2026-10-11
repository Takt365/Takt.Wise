// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Sop
// 文件名称：TaktSopRevisionsController.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP版本控制器
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
/// SOP版本控制器
/// 提供SOP版本的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SOP版本")]
public class TaktSopRevisionsController : TaktControllerBase
{
    private readonly ITaktSopRevisionService _sopRevisionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopRevisionService">SOP版本服务</param>
    public TaktSopRevisionsController(ITaktSopRevisionService sopRevisionService)
    {
        _sopRevisionService = sopRevisionService;
    }

    /// <summary>
    /// 获取SOP版本列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:list", "SOP版本列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSopRevisionListAsync([FromQuery] TaktSopRevisionQueryDto queryDto)
    {
        try
        {
            var result = await _sopRevisionService.GetSopRevisionListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取SOP版本
    /// </summary>
    /// <param name="id">SOP版本ID</param>
    /// <returns>SOP版本DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:query", "SOP版本详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSopRevisionByIdAsync(long id)
    {
        try
        {
            var result = await _sopRevisionService.GetSopRevisionByIdAsync(id);
            if (result == null)
            {
                return NotFound("SOP版本不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取SOP版本选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:query", "SOP版本选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSopRevisionOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sopRevisionService.GetSopRevisionOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建SOP版本
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>SOP版本DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:create", "创建SOP版本")]
    [HttpPost]
    public async Task<IActionResult> CreateSopRevisionAsync([FromBody] TaktSopRevisionCreateDto dto)
    {
        try
        {
            var result = await _sopRevisionService.CreateSopRevisionAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP版本
    /// </summary>
    /// <param name="id">SOP版本ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>SOP版本DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:update", "更新SOP版本")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSopRevisionAsync(long id, [FromBody] TaktSopRevisionUpdateDto dto)
    {
        try
        {
            var result = await _sopRevisionService.UpdateSopRevisionAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除SOP版本
    /// </summary>
    /// <param name="id">SOP版本ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:delete", "删除SOP版本")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSopRevisionByIdAsync(long id)
    {
        try
        {
            await _sopRevisionService.DeleteSopRevisionByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除SOP版本
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:delete", "批量删除SOP版本")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSopRevisionBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sopRevisionService.DeleteSopRevisionBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP版本状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>SOP版本DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:update", "更新SOP版本状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSopRevisionStatusAsync([FromBody] TaktSopRevisionStatusDto dto)
    {
        try
        {
            var result = await _sopRevisionService.UpdateSopRevisionStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:sop:doc:import", "获取SOP版本导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSopRevisionTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sopRevisionService.GetSopRevisionTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入SOP版本
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:import", "导入SOP版本")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSopRevisionAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sopRevisionService.ImportSopRevisionAsync(stream, sheetName);
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
    /// 导出SOP版本
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:export", "导出SOP版本")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSopRevisionAsync([FromQuery] TaktSopRevisionQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sopRevisionService.ExportSopRevisionAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
