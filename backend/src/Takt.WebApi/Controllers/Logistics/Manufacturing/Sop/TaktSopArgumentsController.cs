// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Sop
// 文件名称：TaktSopArgumentsController.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP作业参数控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Application.Services.Logistics.Manufacturing.Sop;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP作业参数控制器
/// 提供SOP作业参数的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SOP作业参数")]
public class TaktSopArgumentsController : TaktControllerBase
{
    private readonly ITaktSopArgumentService _sopArgumentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopArgumentService">SOP作业参数服务</param>
    public TaktSopArgumentsController(ITaktSopArgumentService sopArgumentService)
    {
        _sopArgumentService = sopArgumentService;
    }

    /// <summary>
    /// 获取SOP作业参数列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:list", "SOP作业参数列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSopArgumentListAsync([FromQuery] TaktSopArgumentQueryDto queryDto)
    {
        try
        {
            var result = await _sopArgumentService.GetSopArgumentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取SOP作业参数
    /// </summary>
    /// <param name="id">SOP作业参数ID</param>
    /// <returns>SOP作业参数DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:query", "SOP作业参数详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSopArgumentByIdAsync(long id)
    {
        try
        {
            var result = await _sopArgumentService.GetSopArgumentByIdAsync(id);
            if (result == null)
            {
                return NotFound("SOP作业参数不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取SOP作业参数选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:query", "SOP作业参数选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSopArgumentOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sopArgumentService.GetSopArgumentOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建SOP作业参数
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>SOP作业参数DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:create", "创建SOP作业参数")]
    [HttpPost]
    public async Task<IActionResult> CreateSopArgumentAsync([FromBody] TaktSopArgumentCreateDto dto)
    {
        try
        {
            var result = await _sopArgumentService.CreateSopArgumentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP作业参数
    /// </summary>
    /// <param name="id">SOP作业参数ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>SOP作业参数DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:update", "更新SOP作业参数")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSopArgumentAsync(long id, [FromBody] TaktSopArgumentUpdateDto dto)
    {
        try
        {
            var result = await _sopArgumentService.UpdateSopArgumentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除SOP作业参数
    /// </summary>
    /// <param name="id">SOP作业参数ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:delete", "删除SOP作业参数")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSopArgumentByIdAsync(long id)
    {
        try
        {
            await _sopArgumentService.DeleteSopArgumentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除SOP作业参数
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:delete", "批量删除SOP作业参数")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSopArgumentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sopArgumentService.DeleteSopArgumentBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:sop:exec:import", "获取SOP作业参数导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSopArgumentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sopArgumentService.GetSopArgumentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入SOP作业参数
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:import", "导入SOP作业参数")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSopArgumentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sopArgumentService.ImportSopArgumentAsync(stream, sheetName);
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
    /// 导出SOP作业参数
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:export", "导出SOP作业参数")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSopArgumentAsync([FromQuery] TaktSopArgumentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sopArgumentService.ExportSopArgumentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
