// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Code.Generator
// 文件名称：TaktGenTablesController.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成数据表配置控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Code.Generator;
using Takt.Application.Services.Code.Generator;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Code.Generator;

/// <summary>
/// 代码生成数据表配置控制器
/// 提供代码生成数据表配置的 REST API
/// </summary>
[ApiModule(7, "代码管理")]
[Route("api/[controller]", Name = "代码生成数据表配置")]
public class TaktGenTablesController : TaktControllerBase
{
    private readonly ITaktGenTableService _genTableService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="genTableService">代码生成数据表配置服务</param>
    public TaktGenTablesController(ITaktGenTableService genTableService)
    {
        _genTableService = genTableService;
    }

    /// <summary>
    /// 获取代码生成数据表配置列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("code:generator:gen:table:list", "代码生成数据表配置列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetGenTableListAsync([FromQuery] TaktGenTableQueryDto queryDto)
    {
        try
        {
            var result = await _genTableService.GetGenTableListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取代码生成数据表配置
    /// </summary>
    /// <param name="id">代码生成数据表配置ID</param>
    /// <returns>代码生成数据表配置DTO</returns>
    [TaktPermission("code:generator:gen:table:query", "代码生成数据表配置详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenTableByIdAsync(long id)
    {
        try
        {
            var result = await _genTableService.GetGenTableByIdAsync(id);
            if (result == null)
            {
                return NotFound("代码生成数据表配置不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取代码生成表配置选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("code:generator:gen:table:query", "代码生成数据表配置选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetGenTableOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _genTableService.GetGenTableOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建代码生成数据表配置
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>代码生成数据表配置DTO</returns>
    [TaktPermission("code:generator:gen:table:create", "创建代码生成数据表配置")]
    [HttpPost]
    public async Task<IActionResult> CreateGenTableAsync([FromBody] TaktGenTableCreateDto dto)
    {
        try
        {
            var result = await _genTableService.CreateGenTableAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新代码生成数据表配置
    /// </summary>
    /// <param name="id">代码生成数据表配置ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>代码生成数据表配置DTO</returns>
    [TaktPermission("code:generator:gen:table:update", "更新代码生成数据表配置")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGenTableAsync(long id, [FromBody] TaktGenTableUpdateDto dto)
    {
        try
        {
            var result = await _genTableService.UpdateGenTableAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除代码生成数据表配置
    /// </summary>
    /// <param name="id">代码生成数据表配置ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("code:generator:gen:table:delete", "删除代码生成数据表配置")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenTableByIdAsync(long id)
    {
        try
        {
            await _genTableService.DeleteGenTableByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除代码生成数据表配置
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("code:generator:gen:table:delete", "批量删除代码生成数据表配置")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteGenTableBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _genTableService.DeleteGenTableBatchAsync(ids);
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
    [TaktPermission("code:generator:gen:table:import", "获取代码生成数据表配置导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetGenTableTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _genTableService.GetGenTableTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入代码生成数据表配置
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("code:generator:gen:table:import", "导入代码生成数据表配置")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportGenTableAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _genTableService.ImportGenTableAsync(stream, sheetName);
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
    /// 导出代码生成数据表配置
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("code:generator:gen:table:export", "导出代码生成数据表配置")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportGenTableAsync([FromQuery] TaktGenTableQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _genTableService.ExportGenTableAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
