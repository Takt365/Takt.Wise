// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktExpenseDetailsController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：费用单明细控制器
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
/// 费用单明细控制器
/// 提供费用单明细的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "费用单明细")]
public class TaktExpenseDetailsController : TaktControllerBase
{
    private readonly ITaktExpenseDetailService _expenseDetailService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="expenseDetailService">费用单明细服务</param>
    public TaktExpenseDetailsController(ITaktExpenseDetailService expenseDetailService)
    {
        _expenseDetailService = expenseDetailService;
    }

    /// <summary>
    /// 获取费用单明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:expense:list", "费用单明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetExpenseDetailListAsync([FromQuery] TaktExpenseDetailQueryDto queryDto)
    {
        try
        {
            var result = await _expenseDetailService.GetExpenseDetailListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取费用单明细
    /// </summary>
    /// <param name="id">费用单明细ID</param>
    /// <returns>费用单明细DTO</returns>
    [TaktPermission("accounting:financial:expense:query", "费用单明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpenseDetailByIdAsync(long id)
    {
        try
        {
            var result = await _expenseDetailService.GetExpenseDetailByIdAsync(id);
            if (result == null)
            {
                return NotFound("费用单明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取费用单明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:expense:query", "费用单明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetExpenseDetailOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _expenseDetailService.GetExpenseDetailOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建费用单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>费用单明细DTO</returns>
    [TaktPermission("accounting:financial:expense:create", "创建费用单明细")]
    [HttpPost]
    public async Task<IActionResult> CreateExpenseDetailAsync([FromBody] TaktExpenseDetailCreateDto dto)
    {
        try
        {
            var result = await _expenseDetailService.CreateExpenseDetailAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新费用单明细
    /// </summary>
    /// <param name="id">费用单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>费用单明细DTO</returns>
    [TaktPermission("accounting:financial:expense:update", "更新费用单明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpenseDetailAsync(long id, [FromBody] TaktExpenseDetailUpdateDto dto)
    {
        try
        {
            var result = await _expenseDetailService.UpdateExpenseDetailAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除费用单明细
    /// </summary>
    /// <param name="id">费用单明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:expense:delete", "删除费用单明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpenseDetailByIdAsync(long id)
    {
        try
        {
            await _expenseDetailService.DeleteExpenseDetailByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除费用单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:expense:delete", "批量删除费用单明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteExpenseDetailBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _expenseDetailService.DeleteExpenseDetailBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新费用单明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>费用单明细DTO</returns>
    [TaktPermission("accounting:financial:expense:update", "更新费用单明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateExpenseDetailObsoleteAsync([FromBody] TaktExpenseDetailObsoleteDto dto)
    {
        try
        {
            var result = await _expenseDetailService.UpdateExpenseDetailObsoleteAsync(dto);
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
    [TaktPermission("accounting:financial:expense:import", "获取费用单明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetExpenseDetailTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _expenseDetailService.GetExpenseDetailTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入费用单明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:expense:import", "导入费用单明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportExpenseDetailAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _expenseDetailService.ImportExpenseDetailAsync(stream, sheetName);
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
    /// 导出费用单明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:expense:export", "导出费用单明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportExpenseDetailAsync([FromQuery] TaktExpenseDetailQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _expenseDetailService.ExportExpenseDetailAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
