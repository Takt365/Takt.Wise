// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktBalanceSheetsController.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：资产负债控制器
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
/// 资产负债控制器
/// 提供资产负债的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "资产负债")]
public class TaktBalanceSheetsController : TaktControllerBase
{
    private readonly ITaktBalanceSheetService _balanceSheetService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="balanceSheetService">资产负债服务</param>
    public TaktBalanceSheetsController(ITaktBalanceSheetService balanceSheetService)
    {
        _balanceSheetService = balanceSheetService;
    }

    /// <summary>
    /// 获取资产负债列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:balance:sheet:list", "资产负债列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBalanceSheetListAsync([FromQuery] TaktBalanceSheetQueryDto queryDto)
    {
        try
        {
            var result = await _balanceSheetService.GetBalanceSheetListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取资产负债
    /// </summary>
    /// <param name="id">资产负债ID</param>
    /// <returns>资产负债DTO</returns>
    [TaktPermission("accounting:financial:balance:sheet:query", "资产负债详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBalanceSheetByIdAsync(long id)
    {
        try
        {
            var result = await _balanceSheetService.GetBalanceSheetByIdAsync(id);
            if (result == null)
            {
                return NotFound("资产负债不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取资产负债选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:balance:sheet:query", "资产负债选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBalanceSheetOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _balanceSheetService.GetBalanceSheetOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建资产负债
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>资产负债DTO</returns>
    [TaktPermission("accounting:financial:balance:sheet:create", "创建资产负债")]
    [HttpPost]
    public async Task<IActionResult> CreateBalanceSheetAsync([FromBody] TaktBalanceSheetCreateDto dto)
    {
        try
        {
            var result = await _balanceSheetService.CreateBalanceSheetAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新资产负债
    /// </summary>
    /// <param name="id">资产负债ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>资产负债DTO</returns>
    [TaktPermission("accounting:financial:balance:sheet:update", "更新资产负债")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBalanceSheetAsync(long id, [FromBody] TaktBalanceSheetUpdateDto dto)
    {
        try
        {
            var result = await _balanceSheetService.UpdateBalanceSheetAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除资产负债
    /// </summary>
    /// <param name="id">资产负债ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:balance:sheet:delete", "删除资产负债")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBalanceSheetByIdAsync(long id)
    {
        try
        {
            await _balanceSheetService.DeleteBalanceSheetByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除资产负债
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:balance:sheet:delete", "批量删除资产负债")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBalanceSheetBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _balanceSheetService.DeleteBalanceSheetBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新资产负债状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>资产负债DTO</returns>
    [TaktPermission("accounting:financial:balance:sheet:update", "更新资产负债状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateBalanceSheetStatusAsync([FromBody] TaktBalanceSheetStatusDto dto)
    {
        try
        {
            var result = await _balanceSheetService.UpdateBalanceSheetStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新资产负债排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>资产负债DTO</returns>
    [TaktPermission("accounting:financial:balance:sheet:update", "更新资产负债排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateBalanceSheetSortAsync([FromBody] TaktBalanceSheetSortDto dto)
    {
        try
        {
            var result = await _balanceSheetService.UpdateBalanceSheetSortAsync(dto);
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
    [TaktPermission("accounting:financial:balance:sheet:import", "获取资产负债导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetBalanceSheetTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _balanceSheetService.GetBalanceSheetTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入资产负债
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:balance:sheet:import", "导入资产负债")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBalanceSheetAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _balanceSheetService.ImportBalanceSheetAsync(stream, sheetName);
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
    /// 导出资产负债
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:balance:sheet:export", "导出资产负债")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBalanceSheetAsync([FromQuery] TaktBalanceSheetQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _balanceSheetService.ExportBalanceSheetAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
