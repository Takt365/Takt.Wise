// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktTranslationsController.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：翻译控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 翻译控制器
/// 提供翻译的 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "翻译")]
public class TaktTranslationsController : TaktControllerBase
{
    private readonly ITaktTranslationService _translationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="translationService">翻译服务</param>
    public TaktTranslationsController(ITaktTranslationService translationService)
    {
        _translationService = translationService;
    }

    /// <summary>
    /// 获取指定文化下的前端动态翻译键值（登录页/SPA 合并 vue-i18n，允许匿名）
    /// </summary>
    /// <param name="cultureCode">区域文化编码（BCP47，如 zh-CN）</param>
    /// <returns>扁平 messages 包</returns>
    [AllowAnonymous]
    [HttpGet("messages")]
    public async Task<IActionResult> GetTranslationMessagesAsync([FromQuery] string cultureCode)
    {
        try
        {
            var result = await _translationService.GetTranslationMessagesAsync(cultureCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取翻译列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:i18n:list", "翻译列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTranslationListAsync([FromQuery] TaktTranslationQueryDto queryDto)
    {
        try
        {
            var result = await _translationService.GetTranslationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取翻译
    /// </summary>
    /// <param name="id">翻译ID</param>
    /// <returns>翻译DTO</returns>
    [TaktPermission("foundation:i18n:query", "翻译详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTranslationByIdAsync(long id)
    {
        try
        {
            var result = await _translationService.GetTranslationByIdAsync(id);
            if (result == null)
            {
                return NotFound("翻译不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取翻译选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:i18n:query", "翻译选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTranslationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _translationService.GetTranslationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建翻译
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>翻译DTO</returns>
    [TaktPermission("foundation:i18n:create", "创建翻译")]
    [HttpPost]
    public async Task<IActionResult> CreateTranslationAsync([FromBody] TaktTranslationCreateDto dto)
    {
        try
        {
            var result = await _translationService.CreateTranslationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新翻译
    /// </summary>
    /// <param name="id">翻译ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>翻译DTO</returns>
    [TaktPermission("foundation:i18n:update", "更新翻译")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTranslationAsync(long id, [FromBody] TaktTranslationUpdateDto dto)
    {
        try
        {
            var result = await _translationService.UpdateTranslationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除翻译
    /// </summary>
    /// <param name="id">翻译ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:i18n:delete", "删除翻译")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTranslationByIdAsync(long id)
    {
        try
        {
            await _translationService.DeleteTranslationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除翻译
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:i18n:delete", "批量删除翻译")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTranslationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _translationService.DeleteTranslationBatchAsync(ids);
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
    [TaktPermission("foundation:i18n:import", "获取翻译导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTranslationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _translationService.GetTranslationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入翻译
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:i18n:import", "导入翻译")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTranslationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _translationService.ImportTranslationAsync(stream, sheetName);
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
    /// 导出翻译
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:i18n:export", "导出翻译")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTranslationAsync([FromQuery] TaktTranslationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _translationService.ExportTranslationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取翻译转置列表（分页）
    /// </summary>
    [TaktPermission("foundation:i18n:query", "查询翻译转置列表")]
    [HttpGet("transposed")]
    public async Task<IActionResult> GetTranslationTransposedListAsync([FromQuery] TaktTranslationTransposedQueryDto queryDto)
    {
        try
        {
            var result = await _translationService.GetTranslationTransposedListAsync(queryDto);
            return Success(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量保存翻译转置数据
    /// </summary>
    [TaktPermission("foundation:i18n:edit", "保存翻译转置数据")]
    [HttpPost("transposed/batch")]
    public async Task<IActionResult> SaveTranslationTransposedBatchAsync([FromBody] TaktTranslationTransposedBatchDto dto)
    {
        try
        {
            var count = await _translationService.SaveTranslationTransposedBatchAsync(dto);
            return Success(count, $"已保存 {count} 条");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
