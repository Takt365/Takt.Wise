// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.NewsCenter
// 文件名称：TaktNewsController.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心控制器
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
/// 新闻中心控制器
/// 提供新闻中心的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "新闻中心")]
public class TaktNewsController : TaktControllerBase
{
    private readonly ITaktNewsService _newsService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsService">新闻中心服务</param>
    public TaktNewsController(ITaktNewsService newsService)
    {
        _newsService = newsService;
    }

    /// <summary>
    /// 获取新闻中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:news:center:list", "新闻中心列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetNewsListAsync([FromQuery] TaktNewsQueryDto queryDto)
    {
        try
        {
            var result = await _newsService.GetNewsListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取新闻中心
    /// </summary>
    /// <param name="id">新闻中心ID</param>
    /// <returns>新闻中心DTO</returns>
    [TaktPermission("routine:news:center:query", "新闻中心详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNewsByIdAsync(long id)
    {
        try
        {
            var result = await _newsService.GetNewsByIdAsync(id);
            if (result == null)
            {
                return NotFound("新闻中心不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取新闻中心主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:news:center:query", "新闻中心选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetNewsOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _newsService.GetNewsOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建新闻中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>新闻中心DTO</returns>
    [TaktPermission("routine:news:center:create", "创建新闻中心")]
    [HttpPost]
    public async Task<IActionResult> CreateNewsAsync([FromBody] TaktNewsCreateDto dto)
    {
        try
        {
            var result = await _newsService.CreateNewsAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心
    /// </summary>
    /// <param name="id">新闻中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>新闻中心DTO</returns>
    [TaktPermission("routine:news:center:update", "更新新闻中心")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNewsAsync(long id, [FromBody] TaktNewsUpdateDto dto)
    {
        try
        {
            var result = await _newsService.UpdateNewsAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除新闻中心
    /// </summary>
    /// <param name="id">新闻中心ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:news:center:delete", "删除新闻中心")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNewsByIdAsync(long id)
    {
        try
        {
            await _newsService.DeleteNewsByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除新闻中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:news:center:delete", "批量删除新闻中心")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteNewsBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _newsService.DeleteNewsBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>新闻中心DTO</returns>
    [TaktPermission("routine:news:center:update", "更新新闻中心状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateNewsStatusAsync([FromBody] TaktNewsStatusDto dto)
    {
        try
        {
            var result = await _newsService.UpdateNewsStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>新闻中心DTO</returns>
    [TaktPermission("routine:news:center:update", "更新新闻中心排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateNewsSortAsync([FromBody] TaktNewsSortDto dto)
    {
        try
        {
            var result = await _newsService.UpdateNewsSortAsync(dto);
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
    [TaktPermission("routine:news:center:import", "获取新闻中心导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetNewsTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _newsService.GetNewsTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入新闻中心
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:news:center:import", "导入新闻中心")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportNewsAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _newsService.ImportNewsAsync(stream, sheetName);
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
    /// 导出新闻中心
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:news:center:export", "导出新闻中心")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportNewsAsync([FromQuery] TaktNewsQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _newsService.ExportNewsAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
