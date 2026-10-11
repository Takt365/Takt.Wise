// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktCountersignsController.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：会签单控制器
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
/// 会签单控制器
/// 提供会签单的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "会签单")]
public class TaktCountersignsController : TaktControllerBase
{
    private readonly ITaktCountersignService _countersignService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="countersignService">会签单服务</param>
    public TaktCountersignsController(ITaktCountersignService countersignService)
    {
        _countersignService = countersignService;
    }

    /// <summary>
    /// 获取会签单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:countersign:list", "会签单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCountersignListAsync([FromQuery] TaktCountersignQueryDto queryDto)
    {
        try
        {
            var result = await _countersignService.GetCountersignListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取会签单
    /// </summary>
    /// <param name="id">会签单ID</param>
    /// <returns>会签单DTO</returns>
    [TaktPermission("accounting:financial:countersign:query", "会签单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCountersignByIdAsync(long id)
    {
        try
        {
            var result = await _countersignService.GetCountersignByIdAsync(id);
            if (result == null)
            {
                return NotFound("会签单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会签单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:countersign:query", "会签单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCountersignOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _countersignService.GetCountersignOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建会签单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>会签单DTO</returns>
    [TaktPermission("accounting:financial:countersign:create", "创建会签单")]
    [HttpPost]
    public async Task<IActionResult> CreateCountersignAsync([FromBody] TaktCountersignCreateDto dto)
    {
        try
        {
            var result = await _countersignService.CreateCountersignAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会签单
    /// </summary>
    /// <param name="id">会签单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>会签单DTO</returns>
    [TaktPermission("accounting:financial:countersign:update", "更新会签单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCountersignAsync(long id, [FromBody] TaktCountersignUpdateDto dto)
    {
        try
        {
            var result = await _countersignService.UpdateCountersignAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除会签单
    /// </summary>
    /// <param name="id">会签单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:countersign:delete", "删除会签单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountersignByIdAsync(long id)
    {
        try
        {
            await _countersignService.DeleteCountersignByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除会签单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:countersign:delete", "批量删除会签单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCountersignBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _countersignService.DeleteCountersignBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会签单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>会签单DTO</returns>
    [TaktPermission("accounting:financial:countersign:update", "更新会签单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCountersignStatusAsync([FromBody] TaktCountersignStatusDto dto)
    {
        try
        {
            var result = await _countersignService.UpdateCountersignStatusAsync(dto);
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
    [TaktPermission("accounting:financial:countersign:import", "获取会签单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCountersignTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _countersignService.GetCountersignTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入会签单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:countersign:import", "导入会签单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCountersignAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _countersignService.ImportCountersignAsync(stream, sheetName);
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
    /// 导出会签单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:countersign:export", "导出会签单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCountersignAsync([FromQuery] TaktCountersignQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _countersignService.ExportCountersignAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
