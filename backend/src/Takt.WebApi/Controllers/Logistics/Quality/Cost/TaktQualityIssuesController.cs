// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Cost
// 文件名称：TaktQualityIssuesController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品质问题应对主控制器
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
/// 品质问题应对主控制器
/// 提供品质问题应对主的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "品质问题应对主")]
public class TaktQualityIssuesController : TaktControllerBase
{
    private readonly ITaktQualityIssueService _qualityIssueService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityIssueService">品质问题应对主服务</param>
    public TaktQualityIssuesController(ITaktQualityIssueService qualityIssueService)
    {
        _qualityIssueService = qualityIssueService;
    }

    /// <summary>
    /// 获取品质问题应对主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:cost:issue:list", "品质问题应对主列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetQualityIssueListAsync([FromQuery] TaktQualityIssueQueryDto queryDto)
    {
        try
        {
            var result = await _qualityIssueService.GetQualityIssueListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取品质应对金额统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>品质应对金额统计</returns>
    [TaktPermission("logistics:quality:cost:issue:list", "品质应对金额统计")]
    [HttpGet("cost-stat")]
    public async Task<IActionResult> GetQualityIssueCostStatAsync([FromQuery] TaktQualityCostStatQueryDto queryDto)
    {
        try
        {
            var result = await _qualityIssueService.GetQualityIssueCostStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取品质问题应对主
    /// </summary>
    /// <param name="id">品质问题应对主ID</param>
    /// <returns>品质问题应对主DTO</returns>
    [TaktPermission("logistics:quality:cost:issue:query", "品质问题应对主详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQualityIssueByIdAsync(long id)
    {
        try
        {
            var result = await _qualityIssueService.GetQualityIssueByIdAsync(id);
            if (result == null)
            {
                return NotFound("品质问题应对主不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取品质问题应对主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:cost:issue:query", "品质问题应对主选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetQualityIssueOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _qualityIssueService.GetQualityIssueOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建品质问题应对主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>品质问题应对主DTO</returns>
    [TaktPermission("logistics:quality:cost:issue:create", "创建品质问题应对主")]
    [HttpPost]
    public async Task<IActionResult> CreateQualityIssueAsync([FromBody] TaktQualityIssueCreateDto dto)
    {
        try
        {
            var result = await _qualityIssueService.CreateQualityIssueAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新品质问题应对主
    /// </summary>
    /// <param name="id">品质问题应对主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>品质问题应对主DTO</returns>
    [TaktPermission("logistics:quality:cost:issue:update", "更新品质问题应对主")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQualityIssueAsync(long id, [FromBody] TaktQualityIssueUpdateDto dto)
    {
        try
        {
            var result = await _qualityIssueService.UpdateQualityIssueAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除品质问题应对主
    /// </summary>
    /// <param name="id">品质问题应对主ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:issue:delete", "删除品质问题应对主")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQualityIssueByIdAsync(long id)
    {
        try
        {
            await _qualityIssueService.DeleteQualityIssueByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除品质问题应对主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:issue:delete", "批量删除品质问题应对主")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteQualityIssueBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _qualityIssueService.DeleteQualityIssueBatchAsync(ids);
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
    [TaktPermission("logistics:quality:cost:issue:import", "获取品质问题应对主导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetQualityIssueTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _qualityIssueService.GetQualityIssueTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入品质问题应对主
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:cost:issue:import", "导入品质问题应对主")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportQualityIssueAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _qualityIssueService.ImportQualityIssueAsync(stream, sheetName);
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
    /// 导出品质问题应对主
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:cost:issue:export", "导出品质问题应对主")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportQualityIssueAsync([FromQuery] TaktQualityIssueQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _qualityIssueService.ExportQualityIssueAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
