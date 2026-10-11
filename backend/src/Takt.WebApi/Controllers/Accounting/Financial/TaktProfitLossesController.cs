// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktProfitLossesController.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：利润控制器
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
/// 利润控制器
/// 提供利润的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "利润")]
public class TaktProfitLossesController : TaktControllerBase
{
    private readonly ITaktProfitLossService _profitLossService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="profitLossService">利润服务</param>
    public TaktProfitLossesController(ITaktProfitLossService profitLossService)
    {
        _profitLossService = profitLossService;
    }

    /// <summary>
    /// 获取利润列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:profit:loss:list", "利润列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetProfitLossListAsync([FromQuery] TaktProfitLossQueryDto queryDto)
    {
        try
        {
            var result = await _profitLossService.GetProfitLossListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取利润
    /// </summary>
    /// <param name="id">利润ID</param>
    /// <returns>利润DTO</returns>
    [TaktPermission("accounting:financial:profit:loss:query", "利润详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfitLossByIdAsync(long id)
    {
        try
        {
            var result = await _profitLossService.GetProfitLossByIdAsync(id);
            if (result == null)
            {
                return NotFound("利润不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取利润选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:profit:loss:query", "利润选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetProfitLossOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _profitLossService.GetProfitLossOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建利润
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>利润DTO</returns>
    [TaktPermission("accounting:financial:profit:loss:create", "创建利润")]
    [HttpPost]
    public async Task<IActionResult> CreateProfitLossAsync([FromBody] TaktProfitLossCreateDto dto)
    {
        try
        {
            var result = await _profitLossService.CreateProfitLossAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新利润
    /// </summary>
    /// <param name="id">利润ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>利润DTO</returns>
    [TaktPermission("accounting:financial:profit:loss:update", "更新利润")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfitLossAsync(long id, [FromBody] TaktProfitLossUpdateDto dto)
    {
        try
        {
            var result = await _profitLossService.UpdateProfitLossAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除利润
    /// </summary>
    /// <param name="id">利润ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:profit:loss:delete", "删除利润")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfitLossByIdAsync(long id)
    {
        try
        {
            await _profitLossService.DeleteProfitLossByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除利润
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:profit:loss:delete", "批量删除利润")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteProfitLossBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _profitLossService.DeleteProfitLossBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新利润状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>利润DTO</returns>
    [TaktPermission("accounting:financial:profit:loss:update", "更新利润状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateProfitLossStatusAsync([FromBody] TaktProfitLossStatusDto dto)
    {
        try
        {
            var result = await _profitLossService.UpdateProfitLossStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新利润排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>利润DTO</returns>
    [TaktPermission("accounting:financial:profit:loss:update", "更新利润排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateProfitLossSortAsync([FromBody] TaktProfitLossSortDto dto)
    {
        try
        {
            var result = await _profitLossService.UpdateProfitLossSortAsync(dto);
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
    [TaktPermission("accounting:financial:profit:loss:import", "获取利润导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetProfitLossTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _profitLossService.GetProfitLossTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入利润
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:profit:loss:import", "导入利润")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportProfitLossAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _profitLossService.ImportProfitLossAsync(stream, sheetName);
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
    /// 导出利润
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:profit:loss:export", "导出利润")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportProfitLossAsync([FromQuery] TaktProfitLossQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _profitLossService.ExportProfitLossAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
