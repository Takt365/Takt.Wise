// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Talent
// 文件名称：TaktTalentStaffingRequirementsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：用人需求控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Talent;
using Takt.Application.Services.HumanResource.Talent;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Talent;

/// <summary>
/// 用人需求控制器
/// 提供用人需求的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "用人需求")]
public class TaktTalentStaffingRequirementsController : TaktControllerBase
{
    private readonly ITaktTalentStaffingRequirementService _talentStaffingRequirementService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentStaffingRequirementService">用人需求服务</param>
    public TaktTalentStaffingRequirementsController(ITaktTalentStaffingRequirementService talentStaffingRequirementService)
    {
        _talentStaffingRequirementService = talentStaffingRequirementService;
    }

    /// <summary>
    /// 获取用人需求列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:list", "用人需求列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTalentStaffingRequirementListAsync([FromQuery] TaktTalentStaffingRequirementQueryDto queryDto)
    {
        try
        {
            var result = await _talentStaffingRequirementService.GetTalentStaffingRequirementListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取用人需求
    /// </summary>
    /// <param name="id">用人需求ID</param>
    /// <returns>用人需求DTO</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:query", "用人需求详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTalentStaffingRequirementByIdAsync(long id)
    {
        try
        {
            var result = await _talentStaffingRequirementService.GetTalentStaffingRequirementByIdAsync(id);
            if (result == null)
            {
                return NotFound("用人需求不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取用人需求选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:query", "用人需求选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTalentStaffingRequirementOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _talentStaffingRequirementService.GetTalentStaffingRequirementOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建用人需求
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>用人需求DTO</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:create", "创建用人需求")]
    [HttpPost]
    public async Task<IActionResult> CreateTalentStaffingRequirementAsync([FromBody] TaktTalentStaffingRequirementCreateDto dto)
    {
        try
        {
            var result = await _talentStaffingRequirementService.CreateTalentStaffingRequirementAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新用人需求
    /// </summary>
    /// <param name="id">用人需求ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>用人需求DTO</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:update", "更新用人需求")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTalentStaffingRequirementAsync(long id, [FromBody] TaktTalentStaffingRequirementUpdateDto dto)
    {
        try
        {
            var result = await _talentStaffingRequirementService.UpdateTalentStaffingRequirementAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除用人需求
    /// </summary>
    /// <param name="id">用人需求ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:delete", "删除用人需求")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTalentStaffingRequirementByIdAsync(long id)
    {
        try
        {
            await _talentStaffingRequirementService.DeleteTalentStaffingRequirementByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除用人需求
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:delete", "批量删除用人需求")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTalentStaffingRequirementBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _talentStaffingRequirementService.DeleteTalentStaffingRequirementBatchAsync(ids);
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
    [TaktPermission("human:resource:talent:staffing:requirement:import", "获取用人需求导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTalentStaffingRequirementTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _talentStaffingRequirementService.GetTalentStaffingRequirementTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入用人需求
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:import", "导入用人需求")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTalentStaffingRequirementAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _talentStaffingRequirementService.ImportTalentStaffingRequirementAsync(stream, sheetName);
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
    /// 导出用人需求
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:export", "导出用人需求")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTalentStaffingRequirementAsync([FromQuery] TaktTalentStaffingRequirementQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _talentStaffingRequirementService.ExportTalentStaffingRequirementAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
