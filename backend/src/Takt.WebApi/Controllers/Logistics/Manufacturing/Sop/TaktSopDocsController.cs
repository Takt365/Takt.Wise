// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Sop
// 文件名称：TaktSopDocsController.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP文档头控制器
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
/// SOP文档头控制器
/// 提供SOP文档头的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SOP文档头")]
public class TaktSopDocsController : TaktControllerBase
{
    private readonly ITaktSopDocService _sopDocService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopDocService">SOP文档头服务</param>
    public TaktSopDocsController(ITaktSopDocService sopDocService)
    {
        _sopDocService = sopDocService;
    }

    /// <summary>
    /// 获取SOP文档头列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:list", "SOP文档头列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSopDocListAsync([FromQuery] TaktSopDocQueryDto queryDto)
    {
        try
        {
            var result = await _sopDocService.GetSopDocListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取SOP文档头
    /// </summary>
    /// <param name="id">SOP文档头ID</param>
    /// <returns>SOP文档头DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:query", "SOP文档头详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSopDocByIdAsync(long id)
    {
        try
        {
            var result = await _sopDocService.GetSopDocByIdAsync(id);
            if (result == null)
            {
                return NotFound("SOP文档头不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取SOP文档头选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:query", "SOP文档头选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSopDocOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sopDocService.GetSopDocOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建SOP文档头
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>SOP文档头DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:create", "创建SOP文档头")]
    [HttpPost]
    public async Task<IActionResult> CreateSopDocAsync([FromBody] TaktSopDocCreateDto dto)
    {
        try
        {
            var result = await _sopDocService.CreateSopDocAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP文档头
    /// </summary>
    /// <param name="id">SOP文档头ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>SOP文档头DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:update", "更新SOP文档头")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSopDocAsync(long id, [FromBody] TaktSopDocUpdateDto dto)
    {
        try
        {
            var result = await _sopDocService.UpdateSopDocAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除SOP文档头
    /// </summary>
    /// <param name="id">SOP文档头ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:delete", "删除SOP文档头")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSopDocByIdAsync(long id)
    {
        try
        {
            await _sopDocService.DeleteSopDocByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除SOP文档头
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:delete", "批量删除SOP文档头")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSopDocBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sopDocService.DeleteSopDocBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP文档头状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>SOP文档头DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:update", "更新SOP文档头状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSopDocStatusAsync([FromBody] TaktSopDocStatusDto dto)
    {
        try
        {
            var result = await _sopDocService.UpdateSopDocStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:sop:doc:import", "获取SOP文档头导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSopDocTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sopDocService.GetSopDocTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入SOP文档头
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:import", "导入SOP文档头")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSopDocAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sopDocService.ImportSopDocAsync(stream, sheetName);
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
    /// 导出SOP文档头
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:export", "导出SOP文档头")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSopDocAsync([FromQuery] TaktSopDocQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sopDocService.ExportSopDocAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
