// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Serial
// 文件名称：TaktSerialOutboundsController.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号出库控制器
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
/// 序列号出库控制器
/// 提供序列号出库的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "序列号出库")]
public class TaktSerialOutboundsController : TaktControllerBase
{
    private readonly ITaktSerialOutboundService _serialOutboundService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serialOutboundService">序列号出库服务</param>
    public TaktSerialOutboundsController(ITaktSerialOutboundService serialOutboundService)
    {
        _serialOutboundService = serialOutboundService;
    }

    /// <summary>
    /// 获取序列号出库列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:serial:outbound:list", "序列号出库列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSerialOutboundListAsync([FromQuery] TaktSerialOutboundQueryDto queryDto)
    {
        try
        {
            var result = await _serialOutboundService.GetSerialOutboundListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取序列号出库
    /// </summary>
    /// <param name="id">序列号出库ID</param>
    /// <returns>序列号出库DTO</returns>
    [TaktPermission("logistics:serial:outbound:query", "序列号出库详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSerialOutboundByIdAsync(long id)
    {
        try
        {
            var result = await _serialOutboundService.GetSerialOutboundByIdAsync(id);
            if (result == null)
            {
                return NotFound("序列号出库不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取产品序列号出库选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:serial:outbound:query", "序列号出库选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSerialOutboundOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _serialOutboundService.GetSerialOutboundOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建序列号出库
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>序列号出库DTO</returns>
    [TaktPermission("logistics:serial:outbound:create", "创建序列号出库")]
    [HttpPost]
    public async Task<IActionResult> CreateSerialOutboundAsync([FromBody] TaktSerialOutboundCreateDto dto)
    {
        try
        {
            var result = await _serialOutboundService.CreateSerialOutboundAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新序列号出库
    /// </summary>
    /// <param name="id">序列号出库ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>序列号出库DTO</returns>
    [TaktPermission("logistics:serial:outbound:update", "更新序列号出库")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSerialOutboundAsync(long id, [FromBody] TaktSerialOutboundUpdateDto dto)
    {
        try
        {
            var result = await _serialOutboundService.UpdateSerialOutboundAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除序列号出库
    /// </summary>
    /// <param name="id">序列号出库ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:serial:outbound:delete", "删除序列号出库")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSerialOutboundByIdAsync(long id)
    {
        try
        {
            await _serialOutboundService.DeleteSerialOutboundByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除序列号出库
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:serial:outbound:delete", "批量删除序列号出库")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSerialOutboundBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _serialOutboundService.DeleteSerialOutboundBatchAsync(ids);
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
    [TaktPermission("logistics:serial:outbound:import", "获取序列号出库导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSerialOutboundTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _serialOutboundService.GetSerialOutboundTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入序列号出库
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:serial:outbound:import", "导入序列号出库")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSerialOutboundAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _serialOutboundService.ImportSerialOutboundAsync(stream, sheetName);
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
    /// 导出序列号出库
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:serial:outbound:export", "导出序列号出库")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSerialOutboundAsync([FromQuery] TaktSerialOutboundQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _serialOutboundService.ExportSerialOutboundAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
