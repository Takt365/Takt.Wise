// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktIpqcDefectHandlingsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：制程检验不良处理记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Application.Services.Logistics.Quality.Operation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Operation;

/// <summary>
/// 制程检验不良处理记录控制器
/// 提供制程检验不良处理记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "制程检验不良处理记录")]
public class TaktIpqcDefectHandlingsController : TaktControllerBase
{
    private readonly ITaktIpqcDefectHandlingService _ipqcDefectHandlingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ipqcDefectHandlingService">制程检验不良处理记录服务</param>
    public TaktIpqcDefectHandlingsController(ITaktIpqcDefectHandlingService ipqcDefectHandlingService)
    {
        _ipqcDefectHandlingService = ipqcDefectHandlingService;
    }

    /// <summary>
    /// 获取制程检验不良处理记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:list", "制程检验不良处理记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetIpqcDefectHandlingListAsync([FromQuery] TaktIpqcDefectHandlingQueryDto queryDto)
    {
        try
        {
            var result = await _ipqcDefectHandlingService.GetIpqcDefectHandlingListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取制程检验不良处理记录
    /// </summary>
    /// <param name="id">制程检验不良处理记录ID</param>
    /// <returns>制程检验不良处理记录DTO</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:query", "制程检验不良处理记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetIpqcDefectHandlingByIdAsync(long id)
    {
        try
        {
            var result = await _ipqcDefectHandlingService.GetIpqcDefectHandlingByIdAsync(id);
            if (result == null)
            {
                return NotFound("制程检验不良处理记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取制程检验不良处理记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:query", "制程检验不良处理记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetIpqcDefectHandlingOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ipqcDefectHandlingService.GetIpqcDefectHandlingOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建制程检验不良处理记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>制程检验不良处理记录DTO</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:create", "创建制程检验不良处理记录")]
    [HttpPost]
    public async Task<IActionResult> CreateIpqcDefectHandlingAsync([FromBody] TaktIpqcDefectHandlingCreateDto dto)
    {
        try
        {
            var result = await _ipqcDefectHandlingService.CreateIpqcDefectHandlingAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新制程检验不良处理记录
    /// </summary>
    /// <param name="id">制程检验不良处理记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>制程检验不良处理记录DTO</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:update", "更新制程检验不良处理记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIpqcDefectHandlingAsync(long id, [FromBody] TaktIpqcDefectHandlingUpdateDto dto)
    {
        try
        {
            var result = await _ipqcDefectHandlingService.UpdateIpqcDefectHandlingAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除制程检验不良处理记录
    /// </summary>
    /// <param name="id">制程检验不良处理记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:delete", "删除制程检验不良处理记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIpqcDefectHandlingByIdAsync(long id)
    {
        try
        {
            await _ipqcDefectHandlingService.DeleteIpqcDefectHandlingByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除制程检验不良处理记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:delete", "批量删除制程检验不良处理记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteIpqcDefectHandlingBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ipqcDefectHandlingService.DeleteIpqcDefectHandlingBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新制程检验不良处理记录状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>制程检验不良处理记录DTO</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:update", "更新制程检验不良处理记录状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateIpqcDefectHandlingStatusAsync([FromBody] TaktIpqcDefectHandlingStatusDto dto)
    {
        try
        {
            var result = await _ipqcDefectHandlingService.UpdateIpqcDefectHandlingStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新制程检验不良处理记录作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>制程检验不良处理记录DTO</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:update", "更新制程检验不良处理记录作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateIpqcDefectHandlingObsoleteAsync([FromBody] TaktIpqcDefectHandlingObsoleteDto dto)
    {
        try
        {
            var result = await _ipqcDefectHandlingService.UpdateIpqcDefectHandlingObsoleteAsync(dto);
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
    [TaktPermission("logistics:quality:operation:ipqc:order:import", "获取制程检验不良处理记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetIpqcDefectHandlingTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ipqcDefectHandlingService.GetIpqcDefectHandlingTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入制程检验不良处理记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:import", "导入制程检验不良处理记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportIpqcDefectHandlingAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ipqcDefectHandlingService.ImportIpqcDefectHandlingAsync(stream, sheetName);
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
    /// 导出制程检验不良处理记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:export", "导出制程检验不良处理记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportIpqcDefectHandlingAsync([FromQuery] TaktIpqcDefectHandlingQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ipqcDefectHandlingService.ExportIpqcDefectHandlingAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
