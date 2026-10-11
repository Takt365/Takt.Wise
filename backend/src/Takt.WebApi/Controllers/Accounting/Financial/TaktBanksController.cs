// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktBanksController.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：银行信息控制器
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
/// 银行信息控制器
/// 提供银行信息的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "银行信息")]
public class TaktBanksController : TaktControllerBase
{
    private readonly ITaktBankService _bankService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bankService">银行信息服务</param>
    public TaktBanksController(ITaktBankService bankService)
    {
        _bankService = bankService;
    }

    /// <summary>
    /// 获取银行信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:bank:list", "银行信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBankListAsync([FromQuery] TaktBankQueryDto queryDto)
    {
        try
        {
            var result = await _bankService.GetBankListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取银行信息
    /// </summary>
    /// <param name="id">银行信息ID</param>
    /// <returns>银行信息DTO</returns>
    [TaktPermission("accounting:financial:bank:query", "银行信息详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBankByIdAsync(long id)
    {
        try
        {
            var result = await _bankService.GetBankByIdAsync(id);
            if (result == null)
            {
                return NotFound("银行信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取银行信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:bank:query", "银行信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBankOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _bankService.GetBankOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建银行信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>银行信息DTO</returns>
    [TaktPermission("accounting:financial:bank:create", "创建银行信息")]
    [HttpPost]
    public async Task<IActionResult> CreateBankAsync([FromBody] TaktBankCreateDto dto)
    {
        try
        {
            var result = await _bankService.CreateBankAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新银行信息
    /// </summary>
    /// <param name="id">银行信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>银行信息DTO</returns>
    [TaktPermission("accounting:financial:bank:update", "更新银行信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBankAsync(long id, [FromBody] TaktBankUpdateDto dto)
    {
        try
        {
            var result = await _bankService.UpdateBankAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除银行信息
    /// </summary>
    /// <param name="id">银行信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:bank:delete", "删除银行信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBankByIdAsync(long id)
    {
        try
        {
            await _bankService.DeleteBankByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除银行信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:bank:delete", "批量删除银行信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBankBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _bankService.DeleteBankBatchAsync(ids);
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
    [TaktPermission("accounting:financial:bank:import", "获取银行信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetBankTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _bankService.GetBankTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入银行信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:bank:import", "导入银行信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBankAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _bankService.ImportBankAsync(stream, sheetName);
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
    /// 导出银行信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:bank:export", "导出银行信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBankAsync([FromQuery] TaktBankQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bankService.ExportBankAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
