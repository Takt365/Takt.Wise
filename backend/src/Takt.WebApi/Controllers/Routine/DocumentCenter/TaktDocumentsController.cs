// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.DocumentCenter
// 文件名称：TaktDocumentsController.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：文管中心控制器
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
/// 文管中心控制器
/// 提供文管中心的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "文管中心")]
public class TaktDocumentsController : TaktControllerBase
{
    private readonly ITaktDocumentService _documentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="documentService">文管中心服务</param>
    public TaktDocumentsController(ITaktDocumentService documentService)
    {
        _documentService = documentService;
    }

    /// <summary>
    /// 获取文管中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:document:center:list", "文管中心列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetDocumentListAsync([FromQuery] TaktDocumentQueryDto queryDto)
    {
        try
        {
            var result = await _documentService.GetDocumentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取文管中心
    /// </summary>
    /// <param name="id">文管中心ID</param>
    /// <returns>文管中心DTO</returns>
    [TaktPermission("routine:document:center:query", "文管中心详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentByIdAsync(long id)
    {
        try
        {
            var result = await _documentService.GetDocumentByIdAsync(id);
            if (result == null)
            {
                return NotFound("文管中心不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取文管中心主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:document:center:query", "文管中心选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetDocumentOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _documentService.GetDocumentOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建文管中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>文管中心DTO</returns>
    [TaktPermission("routine:document:center:create", "创建文管中心")]
    [HttpPost]
    public async Task<IActionResult> CreateDocumentAsync([FromBody] TaktDocumentCreateDto dto)
    {
        try
        {
            var result = await _documentService.CreateDocumentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新文管中心
    /// </summary>
    /// <param name="id">文管中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>文管中心DTO</returns>
    [TaktPermission("routine:document:center:update", "更新文管中心")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDocumentAsync(long id, [FromBody] TaktDocumentUpdateDto dto)
    {
        try
        {
            var result = await _documentService.UpdateDocumentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除文管中心
    /// </summary>
    /// <param name="id">文管中心ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:document:center:delete", "删除文管中心")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocumentByIdAsync(long id)
    {
        try
        {
            await _documentService.DeleteDocumentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除文管中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:document:center:delete", "批量删除文管中心")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteDocumentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _documentService.DeleteDocumentBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新文管中心状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>文管中心DTO</returns>
    [TaktPermission("routine:document:center:update", "更新文管中心状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateDocumentStatusAsync([FromBody] TaktDocumentStatusDto dto)
    {
        try
        {
            var result = await _documentService.UpdateDocumentStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新文管中心排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>文管中心DTO</returns>
    [TaktPermission("routine:document:center:update", "更新文管中心排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateDocumentSortAsync([FromBody] TaktDocumentSortDto dto)
    {
        try
        {
            var result = await _documentService.UpdateDocumentSortAsync(dto);
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
    [TaktPermission("routine:document:center:import", "获取文管中心导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetDocumentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _documentService.GetDocumentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入文管中心
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:document:center:import", "导入文管中心")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportDocumentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _documentService.ImportDocumentAsync(stream, sheetName);
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
    /// 导出文管中心
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:document:center:export", "导出文管中心")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportDocumentAsync([FromQuery] TaktDocumentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _documentService.ExportDocumentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
