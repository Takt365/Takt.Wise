// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.DocumentCenter
// 文件名称：TaktDocumentVersionsController.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：文管文档版本控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.DocumentCenter;
using Takt.Application.Services.Routine.DocumentCenter;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.DocumentCenter;

/// <summary>
/// 文管文档版本控制器
/// 提供文管文档版本的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "文管文档版本")]
public class TaktDocumentVersionsController : TaktControllerBase
{
    private readonly ITaktDocumentVersionService _documentVersionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="documentVersionService">文管文档版本服务</param>
    public TaktDocumentVersionsController(ITaktDocumentVersionService documentVersionService)
    {
        _documentVersionService = documentVersionService;
    }

    /// <summary>
    /// 获取文管文档版本列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:document:center:version:list", "文管文档版本列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetDocumentVersionListAsync([FromQuery] TaktDocumentVersionQueryDto queryDto)
    {
        try
        {
            var result = await _documentVersionService.GetDocumentVersionListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取文管文档版本
    /// </summary>
    /// <param name="id">文管文档版本ID</param>
    /// <returns>文管文档版本DTO</returns>
    [TaktPermission("routine:document:center:version:query", "文管文档版本详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentVersionByIdAsync(long id)
    {
        try
        {
            var result = await _documentVersionService.GetDocumentVersionByIdAsync(id);
            if (result == null)
            {
                return NotFound("文管文档版本不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取文管文档版本选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:document:center:version:query", "文管文档版本选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetDocumentVersionOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _documentVersionService.GetDocumentVersionOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建文管文档版本
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>文管文档版本DTO</returns>
    [TaktPermission("routine:document:center:version:create", "创建文管文档版本")]
    [HttpPost]
    public async Task<IActionResult> CreateDocumentVersionAsync([FromBody] TaktDocumentVersionCreateDto dto)
    {
        try
        {
            var result = await _documentVersionService.CreateDocumentVersionAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新文管文档版本
    /// </summary>
    /// <param name="id">文管文档版本ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>文管文档版本DTO</returns>
    [TaktPermission("routine:document:center:version:update", "更新文管文档版本")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDocumentVersionAsync(long id, [FromBody] TaktDocumentVersionUpdateDto dto)
    {
        try
        {
            var result = await _documentVersionService.UpdateDocumentVersionAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除文管文档版本
    /// </summary>
    /// <param name="id">文管文档版本ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:document:center:version:delete", "删除文管文档版本")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocumentVersionByIdAsync(long id)
    {
        try
        {
            await _documentVersionService.DeleteDocumentVersionByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除文管文档版本
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:document:center:version:delete", "批量删除文管文档版本")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteDocumentVersionBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _documentVersionService.DeleteDocumentVersionBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新文管文档版本作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>文管文档版本DTO</returns>
    [TaktPermission("routine:document:center:version:update", "更新文管文档版本作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateDocumentVersionObsoleteAsync([FromBody] TaktDocumentVersionObsoleteDto dto)
    {
        try
        {
            var result = await _documentVersionService.UpdateDocumentVersionObsoleteAsync(dto);
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
    [TaktPermission("routine:document:center:version:import", "获取文管文档版本导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetDocumentVersionTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _documentVersionService.GetDocumentVersionTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入文管文档版本
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:document:center:version:import", "导入文管文档版本")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportDocumentVersionAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _documentVersionService.ImportDocumentVersionAsync(stream, sheetName);
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
    /// 导出文管文档版本
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:document:center:version:export", "导出文管文档版本")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportDocumentVersionAsync([FromQuery] TaktDocumentVersionQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _documentVersionService.ExportDocumentVersionAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
