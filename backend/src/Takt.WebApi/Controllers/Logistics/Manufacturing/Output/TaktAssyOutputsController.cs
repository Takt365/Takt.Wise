// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services.Logistics.Manufacturing.Output;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报控制器
/// 提供组立日报的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "组立日报")]
public class TaktAssyOutputsController : TaktControllerBase
{
    private readonly ITaktAssyOutputService _assyOutputService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyOutputService">组立日报服务</param>
    public TaktAssyOutputsController(ITaktAssyOutputService assyOutputService)
    {
        _assyOutputService = assyOutputService;
    }

    /// <summary>
    /// 获取组立日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:output:mfg1:list", "组立日报列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAssyOutputListAsync([FromQuery] TaktAssyOutputQueryDto queryDto)
    {
        try
        {
            var result = await _assyOutputService.GetAssyOutputListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取组立日报
    /// </summary>
    /// <param name="id">组立日报ID</param>
    /// <returns>组立日报DTO</returns>
    [TaktPermission("logistics:manufacturing:output:mfg1:query", "组立日报详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssyOutputByIdAsync(long id)
    {
        try
        {
            var result = await _assyOutputService.GetAssyOutputByIdAsync(id);
            if (result == null)
            {
                return NotFound("组立日报不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取组立日报选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:output:mfg1:query", "组立日报选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetAssyOutputOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _assyOutputService.GetAssyOutputOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取组立日报新增时固定的生产时段列表（13 条）
    /// </summary>
    /// <returns>生产时段字符串列表</returns>
    [TaktPermission("logistics:manufacturing:output:mfg1:query", "组立日报默认生产时段")]
    [HttpGet("default-time-periods")]
    public async Task<IActionResult> GetAssyOutputDefaultTimePeriodsAsync()
    {
        try
        {
            var result = await _assyOutputService.GetAssyOutputDefaultTimePeriodsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建组立日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>组立日报DTO</returns>
    [TaktPermission("logistics:manufacturing:output:mfg1:create", "创建组立日报")]
    [HttpPost]
    public async Task<IActionResult> CreateAssyOutputAsync([FromBody] TaktAssyOutputCreateDto dto)
    {
        try
        {
            var result = await _assyOutputService.CreateAssyOutputAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新组立日报
    /// </summary>
    /// <param name="id">组立日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>组立日报DTO</returns>
    [TaktPermission("logistics:manufacturing:output:mfg1:update", "更新组立日报")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssyOutputAsync(long id, [FromBody] TaktAssyOutputUpdateDto dto)
    {
        try
        {
            var result = await _assyOutputService.UpdateAssyOutputAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除组立日报
    /// </summary>
    /// <param name="id">组立日报ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:mfg1:delete", "删除组立日报")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssyOutputByIdAsync(long id)
    {
        try
        {
            await _assyOutputService.DeleteAssyOutputByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除组立日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:mfg1:delete", "批量删除组立日报")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteAssyOutputBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _assyOutputService.DeleteAssyOutputBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:output:mfg1:import", "获取组立日报导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetAssyOutputTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _assyOutputService.GetAssyOutputTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入组立日报
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:output:mfg1:import", "导入组立日报")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportAssyOutputAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _assyOutputService.ImportAssyOutputAsync(stream, sheetName);
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
    /// 导出组立日报
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:output:mfg1:export", "导出组立日报")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportAssyOutputAsync([FromQuery] TaktAssyOutputQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _assyOutputService.ExportAssyOutputAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
