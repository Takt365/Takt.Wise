// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Talent
// 文件名称：TaktTalentJobPostingsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：职位发布控制器
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
/// 职位发布控制器
/// 提供职位发布的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "职位发布")]
public class TaktTalentJobPostingsController : TaktControllerBase
{
    private readonly ITaktTalentJobPostingService _talentJobPostingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentJobPostingService">职位发布服务</param>
    public TaktTalentJobPostingsController(ITaktTalentJobPostingService talentJobPostingService)
    {
        _talentJobPostingService = talentJobPostingService;
    }

    /// <summary>
    /// 获取职位发布列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:list", "职位发布列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTalentJobPostingListAsync([FromQuery] TaktTalentJobPostingQueryDto queryDto)
    {
        try
        {
            var result = await _talentJobPostingService.GetTalentJobPostingListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取职位发布
    /// </summary>
    /// <param name="id">职位发布ID</param>
    /// <returns>职位发布DTO</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:query", "职位发布详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTalentJobPostingByIdAsync(long id)
    {
        try
        {
            var result = await _talentJobPostingService.GetTalentJobPostingByIdAsync(id);
            if (result == null)
            {
                return NotFound("职位发布不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取职位发布选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:query", "职位发布选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTalentJobPostingOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _talentJobPostingService.GetTalentJobPostingOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建职位发布
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>职位发布DTO</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:create", "创建职位发布")]
    [HttpPost]
    public async Task<IActionResult> CreateTalentJobPostingAsync([FromBody] TaktTalentJobPostingCreateDto dto)
    {
        try
        {
            var result = await _talentJobPostingService.CreateTalentJobPostingAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新职位发布
    /// </summary>
    /// <param name="id">职位发布ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>职位发布DTO</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:update", "更新职位发布")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTalentJobPostingAsync(long id, [FromBody] TaktTalentJobPostingUpdateDto dto)
    {
        try
        {
            var result = await _talentJobPostingService.UpdateTalentJobPostingAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除职位发布
    /// </summary>
    /// <param name="id">职位发布ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:delete", "删除职位发布")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTalentJobPostingByIdAsync(long id)
    {
        try
        {
            await _talentJobPostingService.DeleteTalentJobPostingByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除职位发布
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:delete", "批量删除职位发布")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTalentJobPostingBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _talentJobPostingService.DeleteTalentJobPostingBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新职位发布状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>职位发布DTO</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:update", "更新职位发布状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTalentJobPostingStatusAsync([FromBody] TaktTalentJobPostingStatusDto dto)
    {
        try
        {
            var result = await _talentJobPostingService.UpdateTalentJobPostingStatusAsync(dto);
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
    [TaktPermission("human:resource:talent:staffing:requirement:import", "获取职位发布导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTalentJobPostingTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _talentJobPostingService.GetTalentJobPostingTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入职位发布
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:import", "导入职位发布")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTalentJobPostingAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _talentJobPostingService.ImportTalentJobPostingAsync(stream, sheetName);
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
    /// 导出职位发布
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:talent:staffing:requirement:export", "导出职位发布")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTalentJobPostingAsync([FromQuery] TaktTalentJobPostingQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _talentJobPostingService.ExportTalentJobPostingAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
