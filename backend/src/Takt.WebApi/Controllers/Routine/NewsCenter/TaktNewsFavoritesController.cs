// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.NewsCenter
// 文件名称：TaktNewsFavoritesController.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心收藏记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.NewsCenter;
using Takt.Application.Services.Routine.NewsCenter;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.NewsCenter;

/// <summary>
/// 新闻中心收藏记录控制器
/// 提供新闻中心收藏记录的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "新闻中心收藏记录")]
public class TaktNewsFavoritesController : TaktControllerBase
{
    private readonly ITaktNewsFavoriteService _newsFavoriteService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsFavoriteService">新闻中心收藏记录服务</param>
    public TaktNewsFavoritesController(ITaktNewsFavoriteService newsFavoriteService)
    {
        _newsFavoriteService = newsFavoriteService;
    }

    /// <summary>
    /// 获取新闻中心收藏记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:news:center:favorite:list", "新闻中心收藏记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetNewsFavoriteListAsync([FromQuery] TaktNewsFavoriteQueryDto queryDto)
    {
        try
        {
            var result = await _newsFavoriteService.GetNewsFavoriteListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取新闻中心收藏记录
    /// </summary>
    /// <param name="id">新闻中心收藏记录ID</param>
    /// <returns>新闻中心收藏记录DTO</returns>
    [TaktPermission("routine:news:center:favorite:query", "新闻中心收藏记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNewsFavoriteByIdAsync(long id)
    {
        try
        {
            var result = await _newsFavoriteService.GetNewsFavoriteByIdAsync(id);
            if (result == null)
            {
                return NotFound("新闻中心收藏记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取新闻收藏记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:news:center:favorite:query", "新闻中心收藏记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetNewsFavoriteOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _newsFavoriteService.GetNewsFavoriteOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建新闻中心收藏记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>新闻中心收藏记录DTO</returns>
    [TaktPermission("routine:news:center:favorite:create", "创建新闻中心收藏记录")]
    [HttpPost]
    public async Task<IActionResult> CreateNewsFavoriteAsync([FromBody] TaktNewsFavoriteCreateDto dto)
    {
        try
        {
            var result = await _newsFavoriteService.CreateNewsFavoriteAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心收藏记录
    /// </summary>
    /// <param name="id">新闻中心收藏记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>新闻中心收藏记录DTO</returns>
    [TaktPermission("routine:news:center:favorite:update", "更新新闻中心收藏记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNewsFavoriteAsync(long id, [FromBody] TaktNewsFavoriteUpdateDto dto)
    {
        try
        {
            var result = await _newsFavoriteService.UpdateNewsFavoriteAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除新闻中心收藏记录
    /// </summary>
    /// <param name="id">新闻中心收藏记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:news:center:favorite:delete", "删除新闻中心收藏记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNewsFavoriteByIdAsync(long id)
    {
        try
        {
            await _newsFavoriteService.DeleteNewsFavoriteByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除新闻中心收藏记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:news:center:favorite:delete", "批量删除新闻中心收藏记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteNewsFavoriteBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _newsFavoriteService.DeleteNewsFavoriteBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心收藏记录作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>新闻中心收藏记录DTO</returns>
    [TaktPermission("routine:news:center:favorite:update", "更新新闻中心收藏记录作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateNewsFavoriteObsoleteAsync([FromBody] TaktNewsFavoriteObsoleteDto dto)
    {
        try
        {
            var result = await _newsFavoriteService.UpdateNewsFavoriteObsoleteAsync(dto);
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
    [TaktPermission("routine:news:center:favorite:import", "获取新闻中心收藏记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetNewsFavoriteTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _newsFavoriteService.GetNewsFavoriteTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入新闻中心收藏记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:news:center:favorite:import", "导入新闻中心收藏记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportNewsFavoriteAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _newsFavoriteService.ImportNewsFavoriteAsync(stream, sheetName);
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
    /// 导出新闻中心收藏记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:news:center:favorite:export", "导出新闻中心收藏记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportNewsFavoriteAsync([FromQuery] TaktNewsFavoriteQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _newsFavoriteService.ExportNewsFavoriteAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
