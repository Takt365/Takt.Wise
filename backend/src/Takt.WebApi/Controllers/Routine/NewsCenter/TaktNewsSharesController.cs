// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.NewsCenter
// 文件名称：TaktNewsSharesController.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心分享记录控制器
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
/// 新闻中心分享记录控制器
/// 提供新闻中心分享记录的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "新闻中心分享记录")]
public class TaktNewsSharesController : TaktControllerBase
{
    private readonly ITaktNewsShareService _newsShareService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsShareService">新闻中心分享记录服务</param>
    public TaktNewsSharesController(ITaktNewsShareService newsShareService)
    {
        _newsShareService = newsShareService;
    }

    /// <summary>
    /// 获取新闻中心分享记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:news:center:share:list", "新闻中心分享记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetNewsShareListAsync([FromQuery] TaktNewsShareQueryDto queryDto)
    {
        try
        {
            var result = await _newsShareService.GetNewsShareListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取新闻中心分享记录
    /// </summary>
    /// <param name="id">新闻中心分享记录ID</param>
    /// <returns>新闻中心分享记录DTO</returns>
    [TaktPermission("routine:news:center:share:query", "新闻中心分享记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNewsShareByIdAsync(long id)
    {
        try
        {
            var result = await _newsShareService.GetNewsShareByIdAsync(id);
            if (result == null)
            {
                return NotFound("新闻中心分享记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取新闻分享记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:news:center:share:query", "新闻中心分享记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetNewsShareOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _newsShareService.GetNewsShareOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建新闻中心分享记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>新闻中心分享记录DTO</returns>
    [TaktPermission("routine:news:center:share:create", "创建新闻中心分享记录")]
    [HttpPost]
    public async Task<IActionResult> CreateNewsShareAsync([FromBody] TaktNewsShareCreateDto dto)
    {
        try
        {
            var result = await _newsShareService.CreateNewsShareAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心分享记录
    /// </summary>
    /// <param name="id">新闻中心分享记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>新闻中心分享记录DTO</returns>
    [TaktPermission("routine:news:center:share:update", "更新新闻中心分享记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNewsShareAsync(long id, [FromBody] TaktNewsShareUpdateDto dto)
    {
        try
        {
            var result = await _newsShareService.UpdateNewsShareAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除新闻中心分享记录
    /// </summary>
    /// <param name="id">新闻中心分享记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:news:center:share:delete", "删除新闻中心分享记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNewsShareByIdAsync(long id)
    {
        try
        {
            await _newsShareService.DeleteNewsShareByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除新闻中心分享记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:news:center:share:delete", "批量删除新闻中心分享记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteNewsShareBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _newsShareService.DeleteNewsShareBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新新闻中心分享记录作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>新闻中心分享记录DTO</returns>
    [TaktPermission("routine:news:center:share:update", "更新新闻中心分享记录作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateNewsShareObsoleteAsync([FromBody] TaktNewsShareObsoleteDto dto)
    {
        try
        {
            var result = await _newsShareService.UpdateNewsShareObsoleteAsync(dto);
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
    [TaktPermission("routine:news:center:share:import", "获取新闻中心分享记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetNewsShareTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _newsShareService.GetNewsShareTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入新闻中心分享记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:news:center:share:import", "导入新闻中心分享记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportNewsShareAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _newsShareService.ImportNewsShareAsync(stream, sheetName);
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
    /// 导出新闻中心分享记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:news:center:share:export", "导出新闻中心分享记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportNewsShareAsync([FromQuery] TaktNewsShareQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _newsShareService.ExportNewsShareAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
