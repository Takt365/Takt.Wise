// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktSamplingSchemesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：抽样方案控制器
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
/// 抽样方案控制器
/// 提供抽样方案的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "抽样方案")]
public class TaktSamplingSchemesController : TaktControllerBase
{
    private readonly ITaktSamplingSchemeService _samplingSchemeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="samplingSchemeService">抽样方案服务</param>
    public TaktSamplingSchemesController(ITaktSamplingSchemeService samplingSchemeService)
    {
        _samplingSchemeService = samplingSchemeService;
    }

    /// <summary>
    /// 获取抽样方案列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:sampling:scheme:list", "抽样方案列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSamplingSchemeListAsync([FromQuery] TaktSamplingSchemeQueryDto queryDto)
    {
        try
        {
            var result = await _samplingSchemeService.GetSamplingSchemeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取抽样方案
    /// </summary>
    /// <param name="id">抽样方案ID</param>
    /// <returns>抽样方案DTO</returns>
    [TaktPermission("logistics:quality:operation:sampling:scheme:query", "抽样方案详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSamplingSchemeByIdAsync(long id)
    {
        try
        {
            var result = await _samplingSchemeService.GetSamplingSchemeByIdAsync(id);
            if (result == null)
            {
                return NotFound("抽样方案不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取抽样方案选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:sampling:scheme:query", "抽样方案选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSamplingSchemeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _samplingSchemeService.GetSamplingSchemeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建抽样方案
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>抽样方案DTO</returns>
    [TaktPermission("logistics:quality:operation:sampling:scheme:create", "创建抽样方案")]
    [HttpPost]
    public async Task<IActionResult> CreateSamplingSchemeAsync([FromBody] TaktSamplingSchemeCreateDto dto)
    {
        try
        {
            var result = await _samplingSchemeService.CreateSamplingSchemeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新抽样方案
    /// </summary>
    /// <param name="id">抽样方案ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>抽样方案DTO</returns>
    [TaktPermission("logistics:quality:operation:sampling:scheme:update", "更新抽样方案")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSamplingSchemeAsync(long id, [FromBody] TaktSamplingSchemeUpdateDto dto)
    {
        try
        {
            var result = await _samplingSchemeService.UpdateSamplingSchemeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除抽样方案
    /// </summary>
    /// <param name="id">抽样方案ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:sampling:scheme:delete", "删除抽样方案")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSamplingSchemeByIdAsync(long id)
    {
        try
        {
            await _samplingSchemeService.DeleteSamplingSchemeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除抽样方案
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:sampling:scheme:delete", "批量删除抽样方案")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSamplingSchemeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _samplingSchemeService.DeleteSamplingSchemeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新抽样方案状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>抽样方案DTO</returns>
    [TaktPermission("logistics:quality:operation:sampling:scheme:update", "更新抽样方案状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSamplingSchemeStatusAsync([FromBody] TaktSamplingSchemeStatusDto dto)
    {
        try
        {
            var result = await _samplingSchemeService.UpdateSamplingSchemeStatusAsync(dto);
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
    [TaktPermission("logistics:quality:operation:sampling:scheme:import", "获取抽样方案导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSamplingSchemeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _samplingSchemeService.GetSamplingSchemeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入抽样方案
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:operation:sampling:scheme:import", "导入抽样方案")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSamplingSchemeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _samplingSchemeService.ImportSamplingSchemeAsync(stream, sheetName);
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
    /// 导出抽样方案
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:sampling:scheme:export", "导出抽样方案")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSamplingSchemeAsync([FromQuery] TaktSamplingSchemeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _samplingSchemeService.ExportSamplingSchemeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
