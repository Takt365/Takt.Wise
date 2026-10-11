// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktFinancialPeriodsController.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：财务期间控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Application.Services.Accounting.Financial;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Accounting.Financial;

/// <summary>
/// 财务期间控制器
/// 提供财务期间的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "财务期间")]
public class TaktFinancialPeriodsController : TaktControllerBase
{
    private readonly ITaktFinancialPeriodService _financialPeriodService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="financialPeriodService">财务期间服务</param>
    public TaktFinancialPeriodsController(ITaktFinancialPeriodService financialPeriodService)
    {
        _financialPeriodService = financialPeriodService;
    }

    /// <summary>
    /// 获取财务期间列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:period:list", "财务期间列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFinancialPeriodListAsync([FromQuery] TaktFinancialPeriodQueryDto queryDto)
    {
        try
        {
            var result = await _financialPeriodService.GetFinancialPeriodListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取财务期间
    /// </summary>
    /// <param name="id">财务期间ID</param>
    /// <returns>财务期间DTO</returns>
    [TaktPermission("accounting:financial:period:query", "财务期间详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFinancialPeriodByIdAsync(long id)
    {
        try
        {
            var result = await _financialPeriodService.GetFinancialPeriodByIdAsync(id);
            if (result == null)
            {
                return NotFound("财务期间不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取财务期间选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:period:query", "财务期间选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFinancialPeriodOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _financialPeriodService.GetFinancialPeriodOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建财务期间
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>财务期间DTO</returns>
    [TaktPermission("accounting:financial:period:create", "创建财务期间")]
    [HttpPost]
    public async Task<IActionResult> CreateFinancialPeriodAsync([FromBody] TaktFinancialPeriodCreateDto dto)
    {
        try
        {
            var result = await _financialPeriodService.CreateFinancialPeriodAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新财务期间
    /// </summary>
    /// <param name="id">财务期间ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>财务期间DTO</returns>
    [TaktPermission("accounting:financial:period:update", "更新财务期间")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFinancialPeriodAsync(long id, [FromBody] TaktFinancialPeriodUpdateDto dto)
    {
        try
        {
            var result = await _financialPeriodService.UpdateFinancialPeriodAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除财务期间
    /// </summary>
    /// <param name="id">财务期间ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:period:delete", "删除财务期间")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFinancialPeriodByIdAsync(long id)
    {
        try
        {
            await _financialPeriodService.DeleteFinancialPeriodByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除财务期间
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:period:delete", "批量删除财务期间")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFinancialPeriodBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _financialPeriodService.DeleteFinancialPeriodBatchAsync(ids);
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
    [TaktPermission("accounting:financial:period:import", "获取财务期间导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFinancialPeriodTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _financialPeriodService.GetFinancialPeriodTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入财务期间
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:period:import", "导入财务期间")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFinancialPeriodAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _financialPeriodService.ImportFinancialPeriodAsync(stream, sheetName);
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
    /// 导出财务期间
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:period:export", "导出财务期间")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFinancialPeriodAsync([FromQuery] TaktFinancialPeriodQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _financialPeriodService.ExportFinancialPeriodAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
