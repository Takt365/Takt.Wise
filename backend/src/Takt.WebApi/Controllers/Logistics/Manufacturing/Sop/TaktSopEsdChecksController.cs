// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Sop
// 文件名称：TaktSopEsdChecksController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP ESD检查控制器
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
/// SOP ESD检查控制器
/// 提供SOP ESD检查的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SOP ESD检查")]
public class TaktSopEsdChecksController : TaktControllerBase
{
    private readonly ITaktSopEsdCheckService _sopEsdCheckService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopEsdCheckService">SOP ESD检查服务</param>
    public TaktSopEsdChecksController(ITaktSopEsdCheckService sopEsdCheckService)
    {
        _sopEsdCheckService = sopEsdCheckService;
    }

    /// <summary>
    /// 获取SOP ESD检查列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:sop:esd:check:list", "SOP ESD检查列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSopEsdCheckListAsync([FromQuery] TaktSopEsdCheckQueryDto queryDto)
    {
        try
        {
            var result = await _sopEsdCheckService.GetSopEsdCheckListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取SOP ESD检查
    /// </summary>
    /// <param name="id">SOP ESD检查ID</param>
    /// <returns>SOP ESD检查DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:esd:check:query", "SOP ESD检查详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSopEsdCheckByIdAsync(long id)
    {
        try
        {
            var result = await _sopEsdCheckService.GetSopEsdCheckByIdAsync(id);
            if (result == null)
            {
                return NotFound("SOP ESD检查不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取SOP ESD检查选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:sop:esd:check:query", "SOP ESD检查选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSopEsdCheckOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sopEsdCheckService.GetSopEsdCheckOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建SOP ESD检查
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>SOP ESD检查DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:esd:check:create", "创建SOP ESD检查")]
    [HttpPost]
    public async Task<IActionResult> CreateSopEsdCheckAsync([FromBody] TaktSopEsdCheckCreateDto dto)
    {
        try
        {
            var result = await _sopEsdCheckService.CreateSopEsdCheckAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP ESD检查
    /// </summary>
    /// <param name="id">SOP ESD检查ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>SOP ESD检查DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:esd:check:update", "更新SOP ESD检查")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSopEsdCheckAsync(long id, [FromBody] TaktSopEsdCheckUpdateDto dto)
    {
        try
        {
            var result = await _sopEsdCheckService.UpdateSopEsdCheckAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除SOP ESD检查
    /// </summary>
    /// <param name="id">SOP ESD检查ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:esd:check:delete", "删除SOP ESD检查")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSopEsdCheckByIdAsync(long id)
    {
        try
        {
            await _sopEsdCheckService.DeleteSopEsdCheckByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除SOP ESD检查
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:esd:check:delete", "批量删除SOP ESD检查")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSopEsdCheckBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sopEsdCheckService.DeleteSopEsdCheckBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:sop:esd:check:import", "获取SOP ESD检查导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSopEsdCheckTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sopEsdCheckService.GetSopEsdCheckTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入SOP ESD检查
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:sop:esd:check:import", "导入SOP ESD检查")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSopEsdCheckAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sopEsdCheckService.ImportSopEsdCheckAsync(stream, sheetName);
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
    /// 导出SOP ESD检查
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:sop:esd:check:export", "导出SOP ESD检查")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSopEsdCheckAsync([FromQuery] TaktSopEsdCheckQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sopEsdCheckService.ExportSopEsdCheckAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
