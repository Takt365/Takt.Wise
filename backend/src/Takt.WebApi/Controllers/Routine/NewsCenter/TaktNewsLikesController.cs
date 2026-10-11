// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.NewsCenter
// 文件名称：TaktNewsLikesController.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心点赞记录控制器
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
/// 新闻中心点赞记录控制器
/// 提供新闻中心点赞记录的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "新闻中心点赞记录")]
public class TaktNewsLikesController : TaktControllerBase
{
    private readonly ITaktNewsLikeService _newsLikeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsLikeService">新闻中心点赞记录服务</param>
    public TaktNewsLikesController(ITaktNewsLikeService newsLikeService)
    {
        _newsLikeService = newsLikeService;
    }

    /// <summary>
    /// 获取新闻中心点赞记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:news:center:like:list", "新闻中心点赞记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetNewsLikeListAsync([FromQuery] TaktNewsLikeQueryDto queryDto)
    {
        try
        {
            var result = await _newsLikeService.GetNewsLikeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取新闻中心点赞记录
    /// </summary>
    /// <param name="id">新闻中心点赞记录ID</param>
    /// <returns>新闻中心点赞记录DTO</returns>
    [TaktPermission("routine:news:center:like:query", "新闻中心点赞记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNewsLikeByIdAsync(long id)
    {
        try
        {
            var result = await _newsLikeService.GetNewsLikeByIdAsync(id);
            if (result == null)
            {
                return NotFound("新闻中心点赞记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取新闻点赞记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:news:center:like:query", "新闻中心点赞记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetNewsLikeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _newsLikeService.GetNewsLikeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建新闻中心点赞记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>新闻中心点赞记录DTO</returns>
    [TaktPermission("routine:news:center:like:create", "创建新闻中心点赞记录")]
    [HttpPost]
    public async Task<IActionResult> CreateNewsLikeAsync([FromBody] TaktNewsLikeCreateDto dto)
    {
        try
        {
            var result = await _newsLikeService.CreateNewsLikeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心点赞记录
    /// </summary>
    /// <param name="id">新闻中心点赞记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>新闻中心点赞记录DTO</returns>
    [TaktPermission("routine:news:center:like:update", "更新新闻中心点赞记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNewsLikeAsync(long id, [FromBody] TaktNewsLikeUpdateDto dto)
    {
        try
        {
            var result = await _newsLikeService.UpdateNewsLikeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除新闻中心点赞记录
    /// </summary>
    /// <param name="id">新闻中心点赞记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:news:center:like:delete", "删除新闻中心点赞记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNewsLikeByIdAsync(long id)
    {
        try
        {
            await _newsLikeService.DeleteNewsLikeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除新闻中心点赞记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:news:center:like:delete", "批量删除新闻中心点赞记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteNewsLikeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _newsLikeService.DeleteNewsLikeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心点赞记录作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>新闻中心点赞记录DTO</returns>
    [TaktPermission("routine:news:center:like:update", "更新新闻中心点赞记录作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateNewsLikeObsoleteAsync([FromBody] TaktNewsLikeObsoleteDto dto)
    {
        try
        {
            var result = await _newsLikeService.UpdateNewsLikeObsoleteAsync(dto);
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
    [TaktPermission("routine:news:center:like:import", "获取新闻中心点赞记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetNewsLikeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _newsLikeService.GetNewsLikeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入新闻中心点赞记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:news:center:like:import", "导入新闻中心点赞记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportNewsLikeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _newsLikeService.ImportNewsLikeAsync(stream, sheetName);
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
    /// 导出新闻中心点赞记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:news:center:like:export", "导出新闻中心点赞记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportNewsLikeAsync([FromQuery] TaktNewsLikeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _newsLikeService.ExportNewsLikeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
