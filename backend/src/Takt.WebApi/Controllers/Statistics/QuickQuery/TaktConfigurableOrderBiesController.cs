// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.QuickQuery
// 文件名称：TaktConfigurableOrderBiesController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表排序控制器
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
/// 定制报表排序控制器
/// 提供定制报表排序的 REST API
/// </summary>
[ApiModule(9, "统计看板")]
[Route("api/[controller]", Name = "定制报表排序")]
public class TaktConfigurableOrderBiesController : TaktControllerBase
{
    private readonly ITaktConfigurableOrderByService _configurableOrderByService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configurableOrderByService">定制报表排序服务</param>
    public TaktConfigurableOrderBiesController(ITaktConfigurableOrderByService configurableOrderByService)
    {
        _configurableOrderByService = configurableOrderByService;
    }

    /// <summary>
    /// 获取定制报表排序列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:quickquery:configurable:list", "定制报表排序列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetConfigurableOrderByListAsync([FromQuery] TaktConfigurableOrderByQueryDto queryDto)
    {
        try
        {
            var result = await _configurableOrderByService.GetConfigurableOrderByListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取定制报表排序
    /// </summary>
    /// <param name="id">定制报表排序ID</param>
    /// <returns>定制报表排序DTO</returns>
    [TaktPermission("statistics:quickquery:configurable:query", "定制报表排序详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConfigurableOrderByByIdAsync(long id)
    {
        try
        {
            var result = await _configurableOrderByService.GetConfigurableOrderByByIdAsync(id);
            if (result == null)
            {
                return NotFound("定制报表排序不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取定制报表排序选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:quickquery:configurable:query", "定制报表排序选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetConfigurableOrderByOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _configurableOrderByService.GetConfigurableOrderByOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建定制报表排序
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>定制报表排序DTO</returns>
    [TaktPermission("statistics:quickquery:configurable:create", "创建定制报表排序")]
    [HttpPost]
    public async Task<IActionResult> CreateConfigurableOrderByAsync([FromBody] TaktConfigurableOrderByCreateDto dto)
    {
        try
        {
            var result = await _configurableOrderByService.CreateConfigurableOrderByAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新定制报表排序
    /// </summary>
    /// <param name="id">定制报表排序ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>定制报表排序DTO</returns>
    [TaktPermission("statistics:quickquery:configurable:update", "更新定制报表排序")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConfigurableOrderByAsync(long id, [FromBody] TaktConfigurableOrderByUpdateDto dto)
    {
        try
        {
            var result = await _configurableOrderByService.UpdateConfigurableOrderByAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除定制报表排序
    /// </summary>
    /// <param name="id">定制报表排序ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:quickquery:configurable:delete", "删除定制报表排序")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConfigurableOrderByByIdAsync(long id)
    {
        try
        {
            await _configurableOrderByService.DeleteConfigurableOrderByByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除定制报表排序
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:quickquery:configurable:delete", "批量删除定制报表排序")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteConfigurableOrderByBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _configurableOrderByService.DeleteConfigurableOrderByBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新定制报表排序排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>定制报表排序DTO</returns>
    [TaktPermission("statistics:quickquery:configurable:update", "更新定制报表排序排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateConfigurableOrderBySortAsync([FromBody] TaktConfigurableOrderBySortDto dto)
    {
        try
        {
            var result = await _configurableOrderByService.UpdateConfigurableOrderBySortAsync(dto);
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
    [TaktPermission("statistics:quickquery:configurable:import", "获取定制报表排序导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetConfigurableOrderByTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _configurableOrderByService.GetConfigurableOrderByTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入定制报表排序
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("statistics:quickquery:configurable:import", "导入定制报表排序")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportConfigurableOrderByAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _configurableOrderByService.ImportConfigurableOrderByAsync(stream, sheetName);
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
    /// 导出定制报表排序
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:quickquery:configurable:export", "导出定制报表排序")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportConfigurableOrderByAsync([FromQuery] TaktConfigurableOrderByQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _configurableOrderByService.ExportConfigurableOrderByAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
