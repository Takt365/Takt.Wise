// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.NewsCenter
// 文件名称：TaktNewsCommentsController.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心评论控制器
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
/// 新闻中心评论控制器
/// 提供新闻中心评论的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "新闻中心评论")]
public class TaktNewsCommentsController : TaktControllerBase
{
    private readonly ITaktNewsCommentService _newsCommentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsCommentService">新闻中心评论服务</param>
    public TaktNewsCommentsController(ITaktNewsCommentService newsCommentService)
    {
        _newsCommentService = newsCommentService;
    }

    /// <summary>
    /// 获取新闻中心评论列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:news:center:comment:list", "新闻中心评论列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetNewsCommentListAsync([FromQuery] TaktNewsCommentQueryDto queryDto)
    {
        try
        {
            var result = await _newsCommentService.GetNewsCommentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取新闻中心评论
    /// </summary>
    /// <param name="id">新闻中心评论ID</param>
    /// <returns>新闻中心评论DTO</returns>
    [TaktPermission("routine:news:center:comment:query", "新闻中心评论详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNewsCommentByIdAsync(long id)
    {
        try
        {
            var result = await _newsCommentService.GetNewsCommentByIdAsync(id);
            if (result == null)
            {
                return NotFound("新闻中心评论不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取新闻中心评论树形选项列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>
    /// <returns>树形选项</returns>
    [TaktPermission("routine:news:center:comment:query", "新闻中心评论树形选项")]
    [HttpGet("tree-options")]
    public async Task<IActionResult> GetNewsCommentTreeOptionsAsync([FromQuery] long parentId = 0, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _newsCommentService.GetNewsCommentTreeOptionsAsync(parentId, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取新闻中心评论树形列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>
    /// <param name="includeDisabled">为 false 时过滤禁用项（按实体 *Status 字段）</param>
    /// <returns>树形数据</returns>
    [TaktPermission("routine:news:center:comment:query", "新闻中心评论树")]
    [HttpGet("tree")]
    public async Task<IActionResult> GetNewsCommentTreeAsync([FromQuery] long parentId = 0, [FromQuery] bool includeDisabled = false)
    {
        try
        {
            var result = await _newsCommentService.GetNewsCommentTreeAsync(parentId, includeDisabled);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建新闻中心评论
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>新闻中心评论DTO</returns>
    [TaktPermission("routine:news:center:comment:create", "创建新闻中心评论")]
    [HttpPost]
    public async Task<IActionResult> CreateNewsCommentAsync([FromBody] TaktNewsCommentCreateDto dto)
    {
        try
        {
            var result = await _newsCommentService.CreateNewsCommentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心评论
    /// </summary>
    /// <param name="id">新闻中心评论ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>新闻中心评论DTO</returns>
    [TaktPermission("routine:news:center:comment:update", "更新新闻中心评论")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNewsCommentAsync(long id, [FromBody] TaktNewsCommentUpdateDto dto)
    {
        try
        {
            var result = await _newsCommentService.UpdateNewsCommentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除新闻中心评论
    /// </summary>
    /// <param name="id">新闻中心评论ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:news:center:comment:delete", "删除新闻中心评论")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNewsCommentByIdAsync(long id)
    {
        try
        {
            await _newsCommentService.DeleteNewsCommentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除新闻中心评论
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:news:center:comment:delete", "批量删除新闻中心评论")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteNewsCommentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _newsCommentService.DeleteNewsCommentBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心评论状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>新闻中心评论DTO</returns>
    [TaktPermission("routine:news:center:comment:update", "更新新闻中心评论状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateNewsCommentStatusAsync([FromBody] TaktNewsCommentStatusDto dto)
    {
        try
        {
            var result = await _newsCommentService.UpdateNewsCommentStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心评论作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>新闻中心评论DTO</returns>
    [TaktPermission("routine:news:center:comment:update", "更新新闻中心评论作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateNewsCommentObsoleteAsync([FromBody] TaktNewsCommentObsoleteDto dto)
    {
        try
        {
            var result = await _newsCommentService.UpdateNewsCommentObsoleteAsync(dto);
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
    [TaktPermission("routine:news:center:comment:import", "获取新闻中心评论导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetNewsCommentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _newsCommentService.GetNewsCommentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入新闻中心评论
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:news:center:comment:import", "导入新闻中心评论")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportNewsCommentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _newsCommentService.ImportNewsCommentAsync(stream, sheetName);
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
    /// 导出新闻中心评论
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:news:center:comment:export", "导出新闻中心评论")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportNewsCommentAsync([FromQuery] TaktNewsCommentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _newsCommentService.ExportNewsCommentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
