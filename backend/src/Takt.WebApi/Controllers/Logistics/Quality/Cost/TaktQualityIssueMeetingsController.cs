// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueMeetingsController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：质量问题会议调查试验费用明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Application.Services.Logistics.Quality.Cost;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Cost;

/// <summary>
/// 质量问题会议调查试验费用明细控制器
/// 提供质量问题会议调查试验费用明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "质量问题会议调查试验费用明细")]
public class TaktQualityIssueMeetingsController : TaktControllerBase
{
    private readonly ITaktQualityIssueMeetingService _qualityIssueMeetingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityIssueMeetingService">质量问题会议调查试验费用明细服务</param>
    public TaktQualityIssueMeetingsController(ITaktQualityIssueMeetingService qualityIssueMeetingService)
    {
        _qualityIssueMeetingService = qualityIssueMeetingService;
    }

    /// <summary>
    /// 获取质量问题会议调查试验费用明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:cost:issue:list", "质量问题会议调查试验费用明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetQualityIssueMeetingListAsync([FromQuery] TaktQualityIssueMeetingQueryDto queryDto)
    {
        try
        {
            var result = await _qualityIssueMeetingService.GetQualityIssueMeetingListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="id">质量问题会议调查试验费用明细ID</param>
    /// <returns>质量问题会议调查试验费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:issue:query", "质量问题会议调查试验费用明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQualityIssueMeetingByIdAsync(long id)
    {
        try
        {
            var result = await _qualityIssueMeetingService.GetQualityIssueMeetingByIdAsync(id);
            if (result == null)
            {
                return NotFound("质量问题会议调查试验费用明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取质量问题会议调查试验费用明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:cost:issue:query", "质量问题会议调查试验费用明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetQualityIssueMeetingOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _qualityIssueMeetingService.GetQualityIssueMeetingOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>质量问题会议调查试验费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:issue:create", "创建质量问题会议调查试验费用明细")]
    [HttpPost]
    public async Task<IActionResult> CreateQualityIssueMeetingAsync([FromBody] TaktQualityIssueMeetingCreateDto dto)
    {
        try
        {
            var result = await _qualityIssueMeetingService.CreateQualityIssueMeetingAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="id">质量问题会议调查试验费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>质量问题会议调查试验费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:issue:update", "更新质量问题会议调查试验费用明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQualityIssueMeetingAsync(long id, [FromBody] TaktQualityIssueMeetingUpdateDto dto)
    {
        try
        {
            var result = await _qualityIssueMeetingService.UpdateQualityIssueMeetingAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="id">质量问题会议调查试验费用明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:issue:delete", "删除质量问题会议调查试验费用明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQualityIssueMeetingByIdAsync(long id)
    {
        try
        {
            await _qualityIssueMeetingService.DeleteQualityIssueMeetingByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:cost:issue:delete", "批量删除质量问题会议调查试验费用明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteQualityIssueMeetingBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _qualityIssueMeetingService.DeleteQualityIssueMeetingBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新质量问题会议调查试验费用明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>质量问题会议调查试验费用明细DTO</returns>
    [TaktPermission("logistics:quality:cost:issue:update", "更新质量问题会议调查试验费用明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateQualityIssueMeetingObsoleteAsync([FromBody] TaktQualityIssueMeetingObsoleteDto dto)
    {
        try
        {
            var result = await _qualityIssueMeetingService.UpdateQualityIssueMeetingObsoleteAsync(dto);
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
    [TaktPermission("logistics:quality:cost:issue:import", "获取质量问题会议调查试验费用明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetQualityIssueMeetingTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _qualityIssueMeetingService.GetQualityIssueMeetingTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:cost:issue:import", "导入质量问题会议调查试验费用明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportQualityIssueMeetingAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _qualityIssueMeetingService.ImportQualityIssueMeetingAsync(stream, sheetName);
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
    /// 导出质量问题会议调查试验费用明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:cost:issue:export", "导出质量问题会议调查试验费用明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportQualityIssueMeetingAsync([FromQuery] TaktQualityIssueMeetingQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _qualityIssueMeetingService.ExportQualityIssueMeetingAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
