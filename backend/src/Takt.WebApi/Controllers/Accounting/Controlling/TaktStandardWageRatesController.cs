// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Controlling
// 文件名称：TaktStandardWageRatesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：标准工资率控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Accounting.Controlling;
using Takt.Application.Services.Accounting.Controlling;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Accounting.Controlling;

/// <summary>
/// 标准工资率控制器
/// 提供标准工资率的 REST API
/// </summary>
[ApiModule(3, "管控会计")]
[Route("api/[controller]", Name = "标准工资率")]
public class TaktStandardWageRatesController : TaktControllerBase
{
    private readonly ITaktStandardWageRateService _standardWageRateService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="standardWageRateService">标准工资率服务</param>
    public TaktStandardWageRatesController(ITaktStandardWageRateService standardWageRateService)
    {
        _standardWageRateService = standardWageRateService;
    }

    /// <summary>
    /// 获取标准工资率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:controlling:standard:wage:rate:list", "标准工资率列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetStandardWageRateListAsync([FromQuery] TaktStandardWageRateQueryDto queryDto)
    {
        try
        {
            var result = await _standardWageRateService.GetStandardWageRateListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取标准工资率
    /// </summary>
    /// <param name="id">标准工资率ID</param>
    /// <returns>标准工资率DTO</returns>
    [TaktPermission("accounting:controlling:standard:wage:rate:query", "标准工资率详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStandardWageRateByIdAsync(long id)
    {
        try
        {
            var result = await _standardWageRateService.GetStandardWageRateByIdAsync(id);
            if (result == null)
            {
                return NotFound("标准工资率不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取标准工资率选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:controlling:standard:wage:rate:query", "标准工资率选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetStandardWageRateOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _standardWageRateService.GetStandardWageRateOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建标准工资率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>标准工资率DTO</returns>
    [TaktPermission("accounting:controlling:standard:wage:rate:create", "创建标准工资率")]
    [HttpPost]
    public async Task<IActionResult> CreateStandardWageRateAsync([FromBody] TaktStandardWageRateCreateDto dto)
    {
        try
        {
            var result = await _standardWageRateService.CreateStandardWageRateAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新标准工资率
    /// </summary>
    /// <param name="id">标准工资率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>标准工资率DTO</returns>
    [TaktPermission("accounting:controlling:standard:wage:rate:update", "更新标准工资率")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStandardWageRateAsync(long id, [FromBody] TaktStandardWageRateUpdateDto dto)
    {
        try
        {
            var result = await _standardWageRateService.UpdateStandardWageRateAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除标准工资率
    /// </summary>
    /// <param name="id">标准工资率ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:standard:wage:rate:delete", "删除标准工资率")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStandardWageRateByIdAsync(long id)
    {
        try
        {
            await _standardWageRateService.DeleteStandardWageRateByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除标准工资率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:standard:wage:rate:delete", "批量删除标准工资率")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteStandardWageRateBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _standardWageRateService.DeleteStandardWageRateBatchAsync(ids);
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
    [TaktPermission("accounting:controlling:standard:wage:rate:import", "获取标准工资率导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetStandardWageRateTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _standardWageRateService.GetStandardWageRateTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入标准工资率
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:controlling:standard:wage:rate:import", "导入标准工资率")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportStandardWageRateAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _standardWageRateService.ImportStandardWageRateAsync(stream, sheetName);
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
    /// 导出标准工资率
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:controlling:standard:wage:rate:export", "导出标准工资率")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportStandardWageRateAsync([FromQuery] TaktStandardWageRateQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _standardWageRateService.ExportStandardWageRateAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
