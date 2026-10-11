// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecStepsController.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工步执行明细控制器
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
/// SOP工步执行明细控制器
/// 提供SOP工步执行明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SOP工步执行明细")]
public class TaktSopExecStepsController : TaktControllerBase
{
    private readonly ITaktSopExecStepService _sopExecStepService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopExecStepService">SOP工步执行明细服务</param>
    public TaktSopExecStepsController(ITaktSopExecStepService sopExecStepService)
    {
        _sopExecStepService = sopExecStepService;
    }

    /// <summary>
    /// 获取SOP工步执行明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:list", "SOP工步执行明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSopExecStepListAsync([FromQuery] TaktSopExecStepQueryDto queryDto)
    {
        try
        {
            var result = await _sopExecStepService.GetSopExecStepListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取SOP工步执行明细
    /// </summary>
    /// <param name="id">SOP工步执行明细ID</param>
    /// <returns>SOP工步执行明细DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:query", "SOP工步执行明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSopExecStepByIdAsync(long id)
    {
        try
        {
            var result = await _sopExecStepService.GetSopExecStepByIdAsync(id);
            if (result == null)
            {
                return NotFound("SOP工步执行明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取SOP工步执行明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:query", "SOP工步执行明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSopExecStepOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sopExecStepService.GetSopExecStepOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建SOP工步执行明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>SOP工步执行明细DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:create", "创建SOP工步执行明细")]
    [HttpPost]
    public async Task<IActionResult> CreateSopExecStepAsync([FromBody] TaktSopExecStepCreateDto dto)
    {
        try
        {
            var result = await _sopExecStepService.CreateSopExecStepAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP工步执行明细
    /// </summary>
    /// <param name="id">SOP工步执行明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>SOP工步执行明细DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:update", "更新SOP工步执行明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSopExecStepAsync(long id, [FromBody] TaktSopExecStepUpdateDto dto)
    {
        try
        {
            var result = await _sopExecStepService.UpdateSopExecStepAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除SOP工步执行明细
    /// </summary>
    /// <param name="id">SOP工步执行明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:delete", "删除SOP工步执行明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSopExecStepByIdAsync(long id)
    {
        try
        {
            await _sopExecStepService.DeleteSopExecStepByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除SOP工步执行明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:delete", "批量删除SOP工步执行明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSopExecStepBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sopExecStepService.DeleteSopExecStepBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:sop:exec:import", "获取SOP工步执行明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSopExecStepTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sopExecStepService.GetSopExecStepTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入SOP工步执行明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:import", "导入SOP工步执行明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSopExecStepAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sopExecStepService.ImportSopExecStepAsync(stream, sheetName);
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
    /// 导出SOP工步执行明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:export", "导出SOP工步执行明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSopExecStepAsync([FromQuery] TaktSopExecStepQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sopExecStepService.ExportSopExecStepAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
