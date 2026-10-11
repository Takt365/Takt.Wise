// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.QuickQuery
// 文件名称：TaktConfigurableJoinsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表关联控制器
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
/// 定制报表关联控制器
/// 提供定制报表关联的 REST API
/// </summary>
[ApiModule(9, "统计看板")]
[Route("api/[controller]", Name = "定制报表关联")]
public class TaktConfigurableJoinsController : TaktControllerBase
{
    private readonly ITaktConfigurableJoinService _configurableJoinService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configurableJoinService">定制报表关联服务</param>
    public TaktConfigurableJoinsController(ITaktConfigurableJoinService configurableJoinService)
    {
        _configurableJoinService = configurableJoinService;
    }

    /// <summary>
    /// 获取定制报表关联列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:quickquery:configurable:list", "定制报表关联列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetConfigurableJoinListAsync([FromQuery] TaktConfigurableJoinQueryDto queryDto)
    {
        try
        {
            var result = await _configurableJoinService.GetConfigurableJoinListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取定制报表关联
    /// </summary>
    /// <param name="id">定制报表关联ID</param>
    /// <returns>定制报表关联DTO</returns>
    [TaktPermission("statistics:quickquery:configurable:query", "定制报表关联详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConfigurableJoinByIdAsync(long id)
    {
        try
        {
            var result = await _configurableJoinService.GetConfigurableJoinByIdAsync(id);
            if (result == null)
            {
                return NotFound("定制报表关联不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取定制报表关联选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:quickquery:configurable:query", "定制报表关联选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetConfigurableJoinOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _configurableJoinService.GetConfigurableJoinOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建定制报表关联
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>定制报表关联DTO</returns>
    [TaktPermission("statistics:quickquery:configurable:create", "创建定制报表关联")]
    [HttpPost]
    public async Task<IActionResult> CreateConfigurableJoinAsync([FromBody] TaktConfigurableJoinCreateDto dto)
    {
        try
        {
            var result = await _configurableJoinService.CreateConfigurableJoinAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新定制报表关联
    /// </summary>
    /// <param name="id">定制报表关联ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>定制报表关联DTO</returns>
    [TaktPermission("statistics:quickquery:configurable:update", "更新定制报表关联")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConfigurableJoinAsync(long id, [FromBody] TaktConfigurableJoinUpdateDto dto)
    {
        try
        {
            var result = await _configurableJoinService.UpdateConfigurableJoinAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除定制报表关联
    /// </summary>
    /// <param name="id">定制报表关联ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:quickquery:configurable:delete", "删除定制报表关联")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConfigurableJoinByIdAsync(long id)
    {
        try
        {
            await _configurableJoinService.DeleteConfigurableJoinByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除定制报表关联
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:quickquery:configurable:delete", "批量删除定制报表关联")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteConfigurableJoinBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _configurableJoinService.DeleteConfigurableJoinBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新定制报表关联排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>定制报表关联DTO</returns>
    [TaktPermission("statistics:quickquery:configurable:update", "更新定制报表关联排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateConfigurableJoinSortAsync([FromBody] TaktConfigurableJoinSortDto dto)
    {
        try
        {
            var result = await _configurableJoinService.UpdateConfigurableJoinSortAsync(dto);
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
    [TaktPermission("statistics:quickquery:configurable:import", "获取定制报表关联导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetConfigurableJoinTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _configurableJoinService.GetConfigurableJoinTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入定制报表关联
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("statistics:quickquery:configurable:import", "导入定制报表关联")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportConfigurableJoinAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _configurableJoinService.ImportConfigurableJoinAsync(stream, sheetName);
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
    /// 导出定制报表关联
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:quickquery:configurable:export", "导出定制报表关联")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportConfigurableJoinAsync([FromQuery] TaktConfigurableJoinQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _configurableJoinService.ExportConfigurableJoinAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
