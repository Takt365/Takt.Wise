// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Cost
// 文件名称：TaktQualityIncidentsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品质事故主控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Application.Services.Logistics.Quality.Cost;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Cost;

/// <summary>
/// 品质事故主控制器
/// 提供品质事故主的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "品质事故主")]
public class TaktQualityIncidentsController : TaktControllerBase
{
    private readonly ITaktQualityIncidentService _qualityIncidentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityIncidentService">品质事故主服务</param>
    public TaktQualityIncidentsController(ITaktQualityIncidentService qualityIncidentService)
    {
        _qualityIncidentService = qualityIncidentService;
    }

    /// <summary>
    /// 获取品质事故主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:cost:incident:list", "品质事故主列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetQualityIncidentListAsync([FromQuery] TaktQualityIncidentQueryDto queryDto)
    {
        try
        {
            var result = await _qualityIncidentService.GetQualityIncidentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取品质事故金额统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>品质事故金额统计</returns>
    [TaktPermission("logistics:quality:cost:incident:list", "品质事故金额统计")]
    [HttpGet("cost-stat")]
    public async Task<IActionResult> GetQualityIncidentCostStatAsync([FromQuery] TaktQualityCostStatQueryDto queryDto)
    {
        try
        {
            var result = await _qualityIncidentService.GetQualityIncidentCostStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取品质事故主
    /// </summary>
    /// <param name="id">品质事故主ID</param>
    /// <returns>品质事故主DTO</returns>
    [TaktPermission("logistics:quality:cost:incident:query", "品质事故主详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQualityIncidentByIdAsync(long id)
    {
        try
        {
            var result = await _qualityIncidentService.GetQualityIncidentByIdAsync(id);
            if (result == null)
            {
                return NotFound("品质事故主不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取品质事故主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:cost:incident:query", "品质事故主选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetQualityIncidentOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _qualityIncidentService.GetQualityIncidentOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建品质事故主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>品质事故主DTO</returns>
    [TaktPermission("logistics:quality:cost:incident:create", "创建品质事故主")]
    [HttpPost]
    public async Task<IActionResult> CreateQualityIncidentAsync([FromBody] TaktQualityIncidentCreateDto dto)
    {
        try
        {
            var result = await _qualityIncidentService.CreateQualityIncidentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新品质事故主
    /// </summary>
    /// <param name="id">品质事故主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>品质事故主DTO</returns>
    [TaktPermission("logistics:quality:cost:incident:update", "更新品质事故主")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQualityIncidentAsync(long id, [FromBody] TaktQualityIncidentUpdateDto dto)
    {
        try
        {
            var result = await _qualityIncidentService.UpdateQualityIncidentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除品质事故主
    /// </summary>
    /// <param name="id">品质事故主ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:incident:delete", "删除品质事故主")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQualityIncidentByIdAsync(long id)
    {
        try
        {
            await _qualityIncidentService.DeleteQualityIncidentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除品质事故主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:incident:delete", "批量删除品质事故主")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteQualityIncidentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _qualityIncidentService.DeleteQualityIncidentBatchAsync(ids);
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
    [TaktPermission("logistics:quality:cost:incident:import", "获取品质事故主导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetQualityIncidentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _qualityIncidentService.GetQualityIncidentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入品质事故主
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:cost:incident:import", "导入品质事故主")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportQualityIncidentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _qualityIncidentService.ImportQualityIncidentAsync(stream, sheetName);
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
    /// 导出品质事故主
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:cost:incident:export", "导出品质事故主")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportQualityIncidentAsync([FromQuery] TaktQualityIncidentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _qualityIncidentService.ExportQualityIncidentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
