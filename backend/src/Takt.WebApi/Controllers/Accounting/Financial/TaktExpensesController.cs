// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktExpensesController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：费用单控制器
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
/// 费用单控制器
/// 提供费用单的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "费用单")]
public class TaktExpensesController : TaktControllerBase
{
    private readonly ITaktExpenseService _expenseService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="expenseService">费用单服务</param>
    public TaktExpensesController(ITaktExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    /// <summary>
    /// 获取费用单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:expense:list", "费用单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetExpenseListAsync([FromQuery] TaktExpenseQueryDto queryDto)
    {
        try
        {
            var result = await _expenseService.GetExpenseListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取费用单
    /// </summary>
    /// <param name="id">费用单ID</param>
    /// <returns>费用单DTO</returns>
    [TaktPermission("accounting:financial:expense:query", "费用单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpenseByIdAsync(long id)
    {
        try
        {
            var result = await _expenseService.GetExpenseByIdAsync(id);
            if (result == null)
            {
                return NotFound("费用单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取费用单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:expense:query", "费用单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetExpenseOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _expenseService.GetExpenseOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建费用单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>费用单DTO</returns>
    [TaktPermission("accounting:financial:expense:create", "创建费用单")]
    [HttpPost]
    public async Task<IActionResult> CreateExpenseAsync([FromBody] TaktExpenseCreateDto dto)
    {
        try
        {
            var result = await _expenseService.CreateExpenseAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新费用单
    /// </summary>
    /// <param name="id">费用单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>费用单DTO</returns>
    [TaktPermission("accounting:financial:expense:update", "更新费用单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpenseAsync(long id, [FromBody] TaktExpenseUpdateDto dto)
    {
        try
        {
            var result = await _expenseService.UpdateExpenseAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除费用单
    /// </summary>
    /// <param name="id">费用单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:expense:delete", "删除费用单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpenseByIdAsync(long id)
    {
        try
        {
            await _expenseService.DeleteExpenseByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除费用单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:expense:delete", "批量删除费用单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteExpenseBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _expenseService.DeleteExpenseBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新费用单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>费用单DTO</returns>
    [TaktPermission("accounting:financial:expense:update", "更新费用单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateExpenseStatusAsync([FromBody] TaktExpenseStatusDto dto)
    {
        try
        {
            var result = await _expenseService.UpdateExpenseStatusAsync(dto);
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
    [TaktPermission("accounting:financial:expense:import", "获取费用单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetExpenseTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _expenseService.GetExpenseTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入费用单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:expense:import", "导入费用单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportExpenseAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _expenseService.ImportExpenseAsync(stream, sheetName);
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
    /// 导出费用单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:expense:export", "导出费用单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportExpenseAsync([FromQuery] TaktExpenseQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _expenseService.ExportExpenseAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
