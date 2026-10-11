// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktVocabulariesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：敏感词控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 敏感词控制器
/// 提供敏感词的 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "敏感词")]
public class TaktVocabulariesController : TaktControllerBase
{
    private readonly ITaktVocabularyService _vocabularyService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="vocabularyService">敏感词服务</param>
    public TaktVocabulariesController(ITaktVocabularyService vocabularyService)
    {
        _vocabularyService = vocabularyService;
    }

    /// <summary>
    /// 获取敏感词列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:vocabulary:list", "敏感词列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetVocabularyListAsync([FromQuery] TaktVocabularyQueryDto queryDto)
    {
        try
        {
            var result = await _vocabularyService.GetVocabularyListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取敏感词
    /// </summary>
    /// <param name="id">敏感词ID</param>
    /// <returns>敏感词DTO</returns>
    [TaktPermission("foundation:vocabulary:query", "敏感词详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVocabularyByIdAsync(long id)
    {
        try
        {
            var result = await _vocabularyService.GetVocabularyByIdAsync(id);
            if (result == null)
            {
                return NotFound("敏感词不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取敏感词选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:vocabulary:query", "敏感词选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetVocabularyOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _vocabularyService.GetVocabularyOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建敏感词
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>敏感词DTO</returns>
    [TaktPermission("foundation:vocabulary:create", "创建敏感词")]
    [HttpPost]
    public async Task<IActionResult> CreateVocabularyAsync([FromBody] TaktVocabularyCreateDto dto)
    {
        try
        {
            var result = await _vocabularyService.CreateVocabularyAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新敏感词
    /// </summary>
    /// <param name="id">敏感词ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>敏感词DTO</returns>
    [TaktPermission("foundation:vocabulary:update", "更新敏感词")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVocabularyAsync(long id, [FromBody] TaktVocabularyUpdateDto dto)
    {
        try
        {
            var result = await _vocabularyService.UpdateVocabularyAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除敏感词
    /// </summary>
    /// <param name="id">敏感词ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:vocabulary:delete", "删除敏感词")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVocabularyByIdAsync(long id)
    {
        try
        {
            await _vocabularyService.DeleteVocabularyByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除敏感词
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:vocabulary:delete", "批量删除敏感词")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteVocabularyBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _vocabularyService.DeleteVocabularyBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新敏感词状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>敏感词DTO</returns>
    [TaktPermission("foundation:vocabulary:update", "更新敏感词状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateVocabularyStatusAsync([FromBody] TaktVocabularyStatusDto dto)
    {
        try
        {
            var result = await _vocabularyService.UpdateVocabularyStatusAsync(dto);
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
    [TaktPermission("foundation:vocabulary:import", "获取敏感词导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetVocabularyTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _vocabularyService.GetVocabularyTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入敏感词
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:vocabulary:import", "导入敏感词")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportVocabularyAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _vocabularyService.ImportVocabularyAsync(stream, sheetName);
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
    /// 导出敏感词
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:vocabulary:export", "导出敏感词")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportVocabularyAsync([FromQuery] TaktVocabularyQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _vocabularyService.ExportVocabularyAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
