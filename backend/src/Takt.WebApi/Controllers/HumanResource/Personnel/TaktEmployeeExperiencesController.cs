// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeeExperiencesController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工工作经历控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Application.Services.HumanResource.Personnel;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Personnel;

/// <summary>
/// 员工工作经历控制器
/// 提供员工工作经历的 REST API
/// </summary>
[ApiModule(5, "人事管理")]
[Route("api/[controller]", Name = "员工工作经历")]
public class TaktEmployeeExperiencesController : TaktControllerBase
{
    private readonly ITaktEmployeeExperienceService _employeeExperienceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeExperienceService">员工工作经历服务</param>
    public TaktEmployeeExperiencesController(ITaktEmployeeExperienceService employeeExperienceService)
    {
        _employeeExperienceService = employeeExperienceService;
    }

    /// <summary>
    /// 获取员工工作经历列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:personnel:employee:experience:list", "员工工作经历列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeExperienceListAsync([FromQuery] TaktEmployeeExperienceQueryDto queryDto)
    {
        try
        {
            var result = await _employeeExperienceService.GetEmployeeExperienceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工工作经历
    /// </summary>
    /// <param name="id">员工工作经历ID</param>
    /// <returns>员工工作经历DTO</returns>
    [TaktPermission("human:resource:personnel:employee:experience:query", "员工工作经历详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeExperienceByIdAsync(long id)
    {
        try
        {
            var result = await _employeeExperienceService.GetEmployeeExperienceByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工工作经历不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工工作经历选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:personnel:employee:experience:query", "员工工作经历选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeExperienceOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _employeeExperienceService.GetEmployeeExperienceOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工工作经历
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工工作经历DTO</returns>
    [TaktPermission("human:resource:personnel:employee:experience:create", "创建员工工作经历")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeExperienceAsync([FromBody] TaktEmployeeExperienceCreateDto dto)
    {
        try
        {
            var result = await _employeeExperienceService.CreateEmployeeExperienceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工工作经历
    /// </summary>
    /// <param name="id">员工工作经历ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工工作经历DTO</returns>
    [TaktPermission("human:resource:personnel:employee:experience:update", "更新员工工作经历")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeExperienceAsync(long id, [FromBody] TaktEmployeeExperienceUpdateDto dto)
    {
        try
        {
            var result = await _employeeExperienceService.UpdateEmployeeExperienceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工工作经历
    /// </summary>
    /// <param name="id">员工工作经历ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:experience:delete", "删除员工工作经历")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeExperienceByIdAsync(long id)
    {
        try
        {
            await _employeeExperienceService.DeleteEmployeeExperienceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工工作经历
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:experience:delete", "批量删除员工工作经历")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeExperienceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeExperienceService.DeleteEmployeeExperienceBatchAsync(ids);
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
    [TaktPermission("human:resource:personnel:employee:experience:import", "获取员工工作经历导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeExperienceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeExperienceService.GetEmployeeExperienceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工工作经历
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:personnel:employee:experience:import", "导入员工工作经历")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeExperienceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeExperienceService.ImportEmployeeExperienceAsync(stream, sheetName);
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
    /// 导出员工工作经历
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:personnel:employee:experience:export", "导出员工工作经历")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeExperienceAsync([FromQuery] TaktEmployeeExperienceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeExperienceService.ExportEmployeeExperienceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
