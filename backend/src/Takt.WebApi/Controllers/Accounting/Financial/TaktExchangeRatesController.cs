// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktExchangeRatesController.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Cursor AI)
// 功能描述：汇率控制器
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
/// 汇率控制器
/// 提供汇率的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "汇率")]
public class TaktExchangeRatesController : TaktControllerBase
{
    private readonly ITaktExchangeRateService _exchangeRateService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="exchangeRateService">汇率服务</param>
    public TaktExchangeRatesController(ITaktExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    /// <summary>
    /// 获取汇率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:exchange:rate:list", "汇率列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetExchangeRateListAsync([FromQuery] TaktExchangeRateQueryDto queryDto)
    {
        try
        {
            var result = await _exchangeRateService.GetExchangeRateListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取汇率
    /// </summary>
    /// <param name="id">汇率ID</param>
    /// <returns>汇率DTO</returns>
    [TaktPermission("accounting:financial:exchange:rate:query", "汇率详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetExchangeRateByIdAsync(long id)
    {
        try
        {
            var result = await _exchangeRateService.GetExchangeRateByIdAsync(id);
            if (result == null)
            {
                return NotFound("汇率不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取汇率选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:exchange:rate:query", "汇率选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetExchangeRateOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _exchangeRateService.GetExchangeRateOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建汇率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>汇率DTO</returns>
    [TaktPermission("accounting:financial:exchange:rate:create", "创建汇率")]
    [HttpPost]
    public async Task<IActionResult> CreateExchangeRateAsync([FromBody] TaktExchangeRateCreateDto dto)
    {
        try
        {
            var result = await _exchangeRateService.CreateExchangeRateAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新汇率
    /// </summary>
    /// <param name="id">汇率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>汇率DTO</returns>
    [TaktPermission("accounting:financial:exchange:rate:update", "更新汇率")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExchangeRateAsync(long id, [FromBody] TaktExchangeRateUpdateDto dto)
    {
        try
        {
            var result = await _exchangeRateService.UpdateExchangeRateAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除汇率
    /// </summary>
    /// <param name="id">汇率ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:exchange:rate:delete", "删除汇率")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExchangeRateByIdAsync(long id)
    {
        try
        {
            await _exchangeRateService.DeleteExchangeRateByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除汇率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:exchange:rate:delete", "批量删除汇率")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteExchangeRateBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _exchangeRateService.DeleteExchangeRateBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新汇率状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>汇率DTO</returns>
    [TaktPermission("accounting:financial:exchange:rate:update", "更新汇率状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateExchangeRateStatusAsync([FromBody] TaktExchangeRateStatusDto dto)
    {
        try
        {
            var result = await _exchangeRateService.UpdateExchangeRateStatusAsync(dto);
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
    [TaktPermission("accounting:financial:exchange:rate:import", "获取汇率导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetExchangeRateTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _exchangeRateService.GetExchangeRateTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入汇率
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:exchange:rate:import", "导入汇率")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportExchangeRateAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _exchangeRateService.ImportExchangeRateAsync(stream, sheetName);
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
    /// 导出汇率
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:exchange:rate:export", "导出汇率")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportExchangeRateAsync([FromQuery] TaktExchangeRateQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _exchangeRateService.ExportExchangeRateAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
