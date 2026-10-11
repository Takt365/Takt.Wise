// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Compensation
// 文件名称：TaktBonusPlansController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：奖金方案控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Compensation;
using Takt.Application.Services.HumanResource.Compensation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Compensation;

/// <summary>
/// 奖金方案控制器
/// 提供奖金方案的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "奖金方案")]
public class TaktBonusPlansController : TaktControllerBase
{
    private readonly ITaktBonusPlanService _bonusPlanService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bonusPlanService">奖金方案服务</param>
    public TaktBonusPlansController(ITaktBonusPlanService bonusPlanService)
    {
        _bonusPlanService = bonusPlanService;
    }

    /// <summary>
    /// 获取奖金方案列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:compensation:bonus:plan:list", "奖金方案列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBonusPlanListAsync([FromQuery] TaktBonusPlanQueryDto queryDto)
    {
        try
        {
            var result = await _bonusPlanService.GetBonusPlanListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取奖金方案
    /// </summary>
    /// <param name="id">奖金方案ID</param>
    /// <returns>奖金方案DTO</returns>
    [TaktPermission("human:resource:compensation:bonus:plan:query", "奖金方案详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBonusPlanByIdAsync(long id)
    {
        try
        {
            var result = await _bonusPlanService.GetBonusPlanByIdAsync(id);
            if (result == null)
            {
                return NotFound("奖金方案不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取奖金方案选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:compensation:bonus:plan:query", "奖金方案选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBonusPlanOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _bonusPlanService.GetBonusPlanOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建奖金方案
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>奖金方案DTO</returns>
    [TaktPermission("human:resource:compensation:bonus:plan:create", "创建奖金方案")]
    [HttpPost]
    public async Task<IActionResult> CreateBonusPlanAsync([FromBody] TaktBonusPlanCreateDto dto)
    {
        try
        {
            var result = await _bonusPlanService.CreateBonusPlanAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新奖金方案
    /// </summary>
    /// <param name="id">奖金方案ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>奖金方案DTO</returns>
    [TaktPermission("human:resource:compensation:bonus:plan:update", "更新奖金方案")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBonusPlanAsync(long id, [FromBody] TaktBonusPlanUpdateDto dto)
    {
        try
        {
            var result = await _bonusPlanService.UpdateBonusPlanAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除奖金方案
    /// </summary>
    /// <param name="id">奖金方案ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:bonus:plan:delete", "删除奖金方案")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBonusPlanByIdAsync(long id)
    {
        try
        {
            await _bonusPlanService.DeleteBonusPlanByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除奖金方案
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:bonus:plan:delete", "批量删除奖金方案")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBonusPlanBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _bonusPlanService.DeleteBonusPlanBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新奖金方案状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>奖金方案DTO</returns>
    [TaktPermission("human:resource:compensation:bonus:plan:update", "更新奖金方案状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateBonusPlanStatusAsync([FromBody] TaktBonusPlanStatusDto dto)
    {
        try
        {
            var result = await _bonusPlanService.UpdateBonusPlanStatusAsync(dto);
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
    [TaktPermission("human:resource:compensation:bonus:plan:import", "获取奖金方案导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetBonusPlanTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _bonusPlanService.GetBonusPlanTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入奖金方案
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:compensation:bonus:plan:import", "导入奖金方案")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBonusPlanAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _bonusPlanService.ImportBonusPlanAsync(stream, sheetName);
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
    /// 导出奖金方案
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:compensation:bonus:plan:export", "导出奖金方案")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBonusPlanAsync([FromQuery] TaktBonusPlanQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bonusPlanService.ExportBonusPlanAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
