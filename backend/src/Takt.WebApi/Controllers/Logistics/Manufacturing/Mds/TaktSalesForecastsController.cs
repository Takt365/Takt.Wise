// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mds
// 文件名称：TaktSalesForecastsController.cs
// 创建时间：2026-07-29
// 创建人：Takt365(Cursor AI)
// 功能描述：销售预测控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Mds;
using Takt.Application.Services.Logistics.Manufacturing.Mds;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Mds;

/// <summary>
/// 销售预测控制器
/// 提供销售预测的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售预测")]
public class TaktSalesForecastsController : TaktControllerBase
{
    private readonly ITaktSalesForecastService _salesForecastService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesForecastService">销售预测服务</param>
    public TaktSalesForecastsController(ITaktSalesForecastService salesForecastService)
    {
        _salesForecastService = salesForecastService;
    }

    /// <summary>
    /// 获取销售预测列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mds:sales:forecast:list", "销售预测列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesForecastListAsync([FromQuery] TaktSalesForecastQueryDto queryDto)
    {
        try
        {
            var result = await _salesForecastService.GetSalesForecastListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售预测
    /// </summary>
    /// <param name="id">销售预测ID</param>
    /// <returns>销售预测DTO</returns>
    [TaktPermission("logistics:manufacturing:mds:sales:forecast:query", "销售预测详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalesForecastByIdAsync(long id)
    {
        try
        {
            var result = await _salesForecastService.GetSalesForecastByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售预测不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售计划选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mds:sales:forecast:query", "销售预测选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesForecastOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salesForecastService.GetSalesForecastOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售预测
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售预测DTO</returns>
    [TaktPermission("logistics:manufacturing:mds:sales:forecast:create", "创建销售预测")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesForecastAsync([FromBody] TaktSalesForecastCreateDto dto)
    {
        try
        {
            var result = await _salesForecastService.CreateSalesForecastAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售预测
    /// </summary>
    /// <param name="id">销售预测ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售预测DTO</returns>
    [TaktPermission("logistics:manufacturing:mds:sales:forecast:update", "更新销售预测")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesForecastAsync(long id, [FromBody] TaktSalesForecastUpdateDto dto)
    {
        try
        {
            var result = await _salesForecastService.UpdateSalesForecastAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售预测
    /// </summary>
    /// <param name="id">销售预测ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mds:sales:forecast:delete", "删除销售预测")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesForecastByIdAsync(long id)
    {
        try
        {
            await _salesForecastService.DeleteSalesForecastByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售预测
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mds:sales:forecast:delete", "批量删除销售预测")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesForecastBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesForecastService.DeleteSalesForecastBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售预测状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>销售预测DTO</returns>
    [TaktPermission("logistics:manufacturing:mds:sales:forecast:update", "更新销售预测状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSalesForecastStatusAsync([FromBody] TaktSalesForecastStatusDto dto)
    {
        try
        {
            var result = await _salesForecastService.UpdateSalesForecastStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:mds:sales:forecast:import", "获取销售预测导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesForecastTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesForecastService.GetSalesForecastTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售预测
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mds:sales:forecast:import", "导入销售预测")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesForecastAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesForecastService.ImportSalesForecastAsync(stream, sheetName);
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
    /// 导出销售预测
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mds:sales:forecast:export", "导出销售预测")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesForecastAsync([FromQuery] TaktSalesForecastQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesForecastService.ExportSalesForecastAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
