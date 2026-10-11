// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Organization
// 文件名称：TaktPostsController.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：岗位控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Organization;
using Takt.Application.Services.HumanResource.Organization;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Organization;

/// <summary>
/// 岗位控制器
/// 提供岗位的 REST API
/// </summary>
[ApiModule(5, "组织管理")]
[Route("api/[controller]", Name = "岗位")]
public class TaktPostsController : TaktControllerBase
{
    private readonly ITaktPostService _postService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="postService">岗位服务</param>
    public TaktPostsController(ITaktPostService postService)
    {
        _postService = postService;
    }

    /// <summary>
    /// 获取岗位列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:organization:post:list", "岗位列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPostListAsync([FromQuery] TaktPostQueryDto queryDto)
    {
        try
        {
            var result = await _postService.GetPostListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取岗位
    /// </summary>
    /// <param name="id">岗位ID</param>
    /// <returns>岗位DTO</returns>
    [TaktPermission("human:resource:organization:post:query", "岗位详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostByIdAsync(long id)
    {
        try
        {
            var result = await _postService.GetPostByIdAsync(id);
            if (result == null)
            {
                return NotFound("岗位不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取岗位选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:organization:post:query", "岗位选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPostOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _postService.GetPostOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建岗位
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>岗位DTO</returns>
    [TaktPermission("human:resource:organization:post:create", "创建岗位")]
    [HttpPost]
    public async Task<IActionResult> CreatePostAsync([FromBody] TaktPostCreateDto dto)
    {
        try
        {
            var result = await _postService.CreatePostAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新岗位
    /// </summary>
    /// <param name="id">岗位ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>岗位DTO</returns>
    [TaktPermission("human:resource:organization:post:update", "更新岗位")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePostAsync(long id, [FromBody] TaktPostUpdateDto dto)
    {
        try
        {
            var result = await _postService.UpdatePostAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除岗位
    /// </summary>
    /// <param name="id">岗位ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:organization:post:delete", "删除岗位")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePostByIdAsync(long id)
    {
        try
        {
            await _postService.DeletePostByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除岗位
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:organization:post:delete", "批量删除岗位")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePostBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _postService.DeletePostBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新岗位状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>岗位DTO</returns>
    [TaktPermission("human:resource:organization:post:update", "更新岗位状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePostStatusAsync([FromBody] TaktPostStatusDto dto)
    {
        try
        {
            var result = await _postService.UpdatePostStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新岗位排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>岗位DTO</returns>
    [TaktPermission("human:resource:organization:post:update", "更新岗位排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdatePostSortAsync([FromBody] TaktPostSortDto dto)
    {
        try
        {
            var result = await _postService.UpdatePostSortAsync(dto);
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
    [TaktPermission("human:resource:organization:post:import", "获取岗位导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPostTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _postService.GetPostTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入岗位
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:organization:post:import", "导入岗位")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPostAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _postService.ImportPostAsync(stream, sheetName);
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
    /// 导出岗位
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:organization:post:export", "导出岗位")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPostAsync([FromQuery] TaktPostQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _postService.ExportPostAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
