// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.QuickQuery
// 文件名称：TaktConfigurableSourcesController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表数据源控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Statistics.QuickQuery;
using Takt.Application.Services.Statistics.QuickQuery;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Statistics.QuickQuery;

/// <summary>
/// 定制报表数据源控制器
/// 提供定制报表数据源的 REST API
/// </summary>
[ApiModule(9, "统计看板")]
[Route("api/[controller]", Name = "定制报表数据源")]
public class TaktConfigurableSourcesController : TaktControllerBase
{
    private readonly ITaktConfigurableSourceService _configurableSourceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configurableSourceService">定制报表数据源服务</param>
    public TaktConfigurableSourcesController(ITaktConfigurableSourceService configurableSourceService)
    {
        _configurableSourceService = configurableSourceService;
    }

    /// <summary>
    /// 获取定制报表数据源列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:quickquery:configurable:list", "定制报表数据源列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetConfigurableSourceListAsync([FromQuery] TaktConfigurableSourceQueryDto queryDto)
    {
        try
        {
            var result = await _configurableSourceService.GetConfigurableSourceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取定制报表数据源
    /// </summary>
    /// <param name="id">定制报表数据源ID</param>
    /// <returns>定制报表数据源DTO</returns>
    [TaktPermission("statistics:quickquery:configurable:query", "定制报表数据源详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConfigurableSourceByIdAsync(long id)
    {
        try
        {
            var result = await _configurableSourceService.GetConfigurableSourceByIdAsync(id);
            if (result == null)
            {
                return NotFound("定制报表数据源不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取定制报表数据源选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:quickquery:configurable:query", "定制报表数据源选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetConfigurableSourceOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _configurableSourceService.GetConfigurableSourceOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建定制报表数据源
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>定制报表数据源DTO</returns>
    [TaktPermission("statistics:quickquery:configurable:create", "创建定制报表数据源")]
    [HttpPost]
    public async Task<IActionResult> CreateConfigurableSourceAsync([FromBody] TaktConfigurableSourceCreateDto dto)
    {
        try
        {
            var result = await _configurableSourceService.CreateConfigurableSourceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新定制报表数据源
    /// </summary>
    /// <param name="id">定制报表数据源ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>定制报表数据源DTO</returns>
    [TaktPermission("statistics:quickquery:configurable:update", "更新定制报表数据源")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConfigurableSourceAsync(long id, [FromBody] TaktConfigurableSourceUpdateDto dto)
    {
        try
        {
            var result = await _configurableSourceService.UpdateConfigurableSourceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除定制报表数据源
    /// </summary>
    /// <param name="id">定制报表数据源ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:quickquery:configurable:delete", "删除定制报表数据源")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConfigurableSourceByIdAsync(long id)
    {
        try
        {
            await _configurableSourceService.DeleteConfigurableSourceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除定制报表数据源
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:quickquery:configurable:delete", "批量删除定制报表数据源")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteConfigurableSourceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _configurableSourceService.DeleteConfigurableSourceBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新定制报表数据源排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>定制报表数据源DTO</returns>
    [TaktPermission("statistics:quickquery:configurable:update", "更新定制报表数据源排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateConfigurableSourceSortAsync([FromBody] TaktConfigurableSourceSortDto dto)
    {
        try
        {
            var result = await _configurableSourceService.UpdateConfigurableSourceSortAsync(dto);
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
    [TaktPermission("statistics:quickquery:configurable:import", "获取定制报表数据源导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetConfigurableSourceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _configurableSourceService.GetConfigurableSourceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入定制报表数据源
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("statistics:quickquery:configurable:import", "导入定制报表数据源")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportConfigurableSourceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _configurableSourceService.ImportConfigurableSourceAsync(stream, sheetName);
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
    /// 导出定制报表数据源
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:quickquery:configurable:export", "导出定制报表数据源")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportConfigurableSourceAsync([FromQuery] TaktConfigurableSourceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _configurableSourceService.ExportConfigurableSourceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
