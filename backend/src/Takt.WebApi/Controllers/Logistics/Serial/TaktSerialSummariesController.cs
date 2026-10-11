// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Serial
// 文件名称：TaktSerialSummariesController.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号汇总控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Serial;
using Takt.Application.Services.Logistics.Serial;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Serial;

/// <summary>
/// 序列号汇总控制器
/// 提供序列号汇总的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "序列号汇总")]
public class TaktSerialSummariesController : TaktControllerBase
{
    private readonly ITaktSerialSummaryService _serialSummaryService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serialSummaryService">序列号汇总服务</param>
    public TaktSerialSummariesController(ITaktSerialSummaryService serialSummaryService)
    {
        _serialSummaryService = serialSummaryService;
    }

    /// <summary>
    /// 获取序列号汇总列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:serial:summary:list", "序列号汇总列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSerialSummaryListAsync([FromQuery] TaktSerialSummaryQueryDto queryDto)
    {
        try
        {
            var result = await _serialSummaryService.GetSerialSummaryListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取序列号汇总
    /// </summary>
    /// <param name="id">序列号汇总ID</param>
    /// <returns>序列号汇总DTO</returns>
    [TaktPermission("logistics:serial:summary:query", "序列号汇总详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSerialSummaryByIdAsync(long id)
    {
        try
        {
            var result = await _serialSummaryService.GetSerialSummaryByIdAsync(id);
            if (result == null)
            {
                return NotFound("序列号汇总不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取序列号汇总选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:serial:summary:query", "序列号汇总选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSerialSummaryOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _serialSummaryService.GetSerialSummaryOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建序列号汇总
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>序列号汇总DTO</returns>
    [TaktPermission("logistics:serial:summary:create", "创建序列号汇总")]
    [HttpPost]
    public async Task<IActionResult> CreateSerialSummaryAsync([FromBody] TaktSerialSummaryCreateDto dto)
    {
        try
        {
            var result = await _serialSummaryService.CreateSerialSummaryAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新序列号汇总
    /// </summary>
    /// <param name="id">序列号汇总ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>序列号汇总DTO</returns>
    [TaktPermission("logistics:serial:summary:update", "更新序列号汇总")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSerialSummaryAsync(long id, [FromBody] TaktSerialSummaryUpdateDto dto)
    {
        try
        {
            var result = await _serialSummaryService.UpdateSerialSummaryAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除序列号汇总
    /// </summary>
    /// <param name="id">序列号汇总ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:serial:summary:delete", "删除序列号汇总")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSerialSummaryByIdAsync(long id)
    {
        try
        {
            await _serialSummaryService.DeleteSerialSummaryByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除序列号汇总
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:serial:summary:delete", "批量删除序列号汇总")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSerialSummaryBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _serialSummaryService.DeleteSerialSummaryBatchAsync(ids);
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
    [TaktPermission("logistics:serial:summary:import", "获取序列号汇总导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSerialSummaryTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _serialSummaryService.GetSerialSummaryTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入序列号汇总
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:serial:summary:import", "导入序列号汇总")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSerialSummaryAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _serialSummaryService.ImportSerialSummaryAsync(stream, sheetName);
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
    /// 导出序列号汇总
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:serial:summary:export", "导出序列号汇总")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSerialSummaryAsync([FromQuery] TaktSerialSummaryQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _serialSummaryService.ExportSerialSummaryAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
