// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变明细控制器
/// 提供设变明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变明细")]
public class TaktEcDetailsController : TaktControllerBase
{
    private readonly ITaktEcDetailService _ecDetailService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecDetailService">设变明细服务</param>
    public TaktEcDetailsController(ITaktEcDetailService ecDetailService)
    {
        _ecDetailService = ecDetailService;
    }

    /// <summary>
    /// 获取设变明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:list", "设变明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcDetailListAsync([FromQuery] TaktEcDetailQueryDto queryDto)
    {
        try
        {
            var result = await _ecDetailService.GetEcDetailListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变明细
    /// </summary>
    /// <param name="id">设变明细ID</param>
    /// <returns>设变明细DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:query", "设变明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEcDetailByIdAsync(long id)
    {
        try
        {
            var result = await _ecDetailService.GetEcDetailByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:query", "设变明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcDetailOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ecDetailService.GetEcDetailOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设变明细DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:create", "创建设变明细")]
    [HttpPost]
    public async Task<IActionResult> CreateEcDetailAsync([FromBody] TaktEcDetailCreateDto dto)
    {
        try
        {
            var result = await _ecDetailService.CreateEcDetailAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变明细
    /// </summary>
    /// <param name="id">设变明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设变明细DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:update", "更新设变明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEcDetailAsync(long id, [FromBody] TaktEcDetailUpdateDto dto)
    {
        try
        {
            var result = await _ecDetailService.UpdateEcDetailAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变明细
    /// </summary>
    /// <param name="id">设变明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:delete", "删除设变明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcDetailByIdAsync(long id)
    {
        try
        {
            await _ecDetailService.DeleteEcDetailByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:delete", "批量删除设变明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcDetailBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecDetailService.DeleteEcDetailBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>设变明细DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:update", "更新设变明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateEcDetailObsoleteAsync([FromBody] TaktEcDetailObsoleteDto dto)
    {
        try
        {
            var result = await _ecDetailService.UpdateEcDetailObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:import", "获取设变明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcDetailTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecDetailService.GetEcDetailTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:import", "导入设变明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcDetailAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecDetailService.ImportEcDetailAsync(stream, sheetName);
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
    /// 导出设变明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:gijutsu:export", "导出设变明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcDetailAsync([FromQuery] TaktEcDetailQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecDetailService.ExportEcDetailAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
