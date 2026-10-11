// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepsController.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工步控制器
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
/// SOP工步控制器
/// 提供SOP工步的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SOP工步")]
public class TaktSopStepsController : TaktControllerBase
{
    private readonly ITaktSopStepService _sopStepService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopStepService">SOP工步服务</param>
    public TaktSopStepsController(ITaktSopStepService sopStepService)
    {
        _sopStepService = sopStepService;
    }

    /// <summary>
    /// 获取SOP工步列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:list", "SOP工步列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSopStepListAsync([FromQuery] TaktSopStepQueryDto queryDto)
    {
        try
        {
            var result = await _sopStepService.GetSopStepListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取SOP工步
    /// </summary>
    /// <param name="id">SOP工步ID</param>
    /// <returns>SOP工步DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:query", "SOP工步详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSopStepByIdAsync(long id)
    {
        try
        {
            var result = await _sopStepService.GetSopStepByIdAsync(id);
            if (result == null)
            {
                return NotFound("SOP工步不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取SOP工步选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:query", "SOP工步选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSopStepOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sopStepService.GetSopStepOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建SOP工步
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>SOP工步DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:create", "创建SOP工步")]
    [HttpPost]
    public async Task<IActionResult> CreateSopStepAsync([FromBody] TaktSopStepCreateDto dto)
    {
        try
        {
            var result = await _sopStepService.CreateSopStepAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP工步
    /// </summary>
    /// <param name="id">SOP工步ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>SOP工步DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:update", "更新SOP工步")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSopStepAsync(long id, [FromBody] TaktSopStepUpdateDto dto)
    {
        try
        {
            var result = await _sopStepService.UpdateSopStepAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除SOP工步
    /// </summary>
    /// <param name="id">SOP工步ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:delete", "删除SOP工步")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSopStepByIdAsync(long id)
    {
        try
        {
            await _sopStepService.DeleteSopStepByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除SOP工步
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:delete", "批量删除SOP工步")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSopStepBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sopStepService.DeleteSopStepBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:sop:doc:import", "获取SOP工步导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSopStepTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sopStepService.GetSopStepTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入SOP工步
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:import", "导入SOP工步")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSopStepAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sopStepService.ImportSopStepAsync(stream, sheetName);
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
    /// 导出SOP工步
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:sop:doc:export", "导出SOP工步")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSopStepAsync([FromQuery] TaktSopStepQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sopStepService.ExportSopStepAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
