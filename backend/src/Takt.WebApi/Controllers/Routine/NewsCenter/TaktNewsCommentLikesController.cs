// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.NewsCenter
// 文件名称：TaktNewsCommentLikesController.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心评论点赞记录控制器
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
/// 新闻中心评论点赞记录控制器
/// 提供新闻中心评论点赞记录的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "新闻中心评论点赞记录")]
public class TaktNewsCommentLikesController : TaktControllerBase
{
    private readonly ITaktNewsCommentLikeService _newsCommentLikeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsCommentLikeService">新闻中心评论点赞记录服务</param>
    public TaktNewsCommentLikesController(ITaktNewsCommentLikeService newsCommentLikeService)
    {
        _newsCommentLikeService = newsCommentLikeService;
    }

    /// <summary>
    /// 获取新闻中心评论点赞记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:news:center:comment:like:list", "新闻中心评论点赞记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetNewsCommentLikeListAsync([FromQuery] TaktNewsCommentLikeQueryDto queryDto)
    {
        try
        {
            var result = await _newsCommentLikeService.GetNewsCommentLikeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取新闻中心评论点赞记录
    /// </summary>
    /// <param name="id">新闻中心评论点赞记录ID</param>
    /// <returns>新闻中心评论点赞记录DTO</returns>
    [TaktPermission("routine:news:center:comment:like:query", "新闻中心评论点赞记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNewsCommentLikeByIdAsync(long id)
    {
        try
        {
            var result = await _newsCommentLikeService.GetNewsCommentLikeByIdAsync(id);
            if (result == null)
            {
                return NotFound("新闻中心评论点赞记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取新闻评论点赞记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:news:center:comment:like:query", "新闻中心评论点赞记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetNewsCommentLikeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _newsCommentLikeService.GetNewsCommentLikeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建新闻中心评论点赞记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>新闻中心评论点赞记录DTO</returns>
    [TaktPermission("routine:news:center:comment:like:create", "创建新闻中心评论点赞记录")]
    [HttpPost]
    public async Task<IActionResult> CreateNewsCommentLikeAsync([FromBody] TaktNewsCommentLikeCreateDto dto)
    {
        try
        {
            var result = await _newsCommentLikeService.CreateNewsCommentLikeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心评论点赞记录
    /// </summary>
    /// <param name="id">新闻中心评论点赞记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>新闻中心评论点赞记录DTO</returns>
    [TaktPermission("routine:news:center:comment:like:update", "更新新闻中心评论点赞记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNewsCommentLikeAsync(long id, [FromBody] TaktNewsCommentLikeUpdateDto dto)
    {
        try
        {
            var result = await _newsCommentLikeService.UpdateNewsCommentLikeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除新闻中心评论点赞记录
    /// </summary>
    /// <param name="id">新闻中心评论点赞记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:news:center:comment:like:delete", "删除新闻中心评论点赞记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNewsCommentLikeByIdAsync(long id)
    {
        try
        {
            await _newsCommentLikeService.DeleteNewsCommentLikeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除新闻中心评论点赞记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:news:center:comment:like:delete", "批量删除新闻中心评论点赞记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteNewsCommentLikeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _newsCommentLikeService.DeleteNewsCommentLikeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心评论点赞记录作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>新闻中心评论点赞记录DTO</returns>
    [TaktPermission("routine:news:center:comment:like:update", "更新新闻中心评论点赞记录作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateNewsCommentLikeObsoleteAsync([FromBody] TaktNewsCommentLikeObsoleteDto dto)
    {
        try
        {
            var result = await _newsCommentLikeService.UpdateNewsCommentLikeObsoleteAsync(dto);
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
    [TaktPermission("routine:news:center:comment:like:import", "获取新闻中心评论点赞记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetNewsCommentLikeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _newsCommentLikeService.GetNewsCommentLikeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入新闻中心评论点赞记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:news:center:comment:like:import", "导入新闻中心评论点赞记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportNewsCommentLikeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _newsCommentLikeService.ImportNewsCommentLikeAsync(stream, sheetName);
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
    /// 导出新闻中心评论点赞记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:news:center:comment:like:export", "导出新闻中心评论点赞记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportNewsCommentLikeAsync([FromQuery] TaktNewsCommentLikeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _newsCommentLikeService.ExportNewsCommentLikeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
