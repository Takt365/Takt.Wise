// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeeOnboardingsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：入职待办控制器
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
/// 入职待办控制器
/// 提供入职待办的 REST API
/// </summary>
[ApiModule(5, "人事管理")]
[Route("api/[controller]", Name = "入职待办")]
public class TaktEmployeeOnboardingsController : TaktControllerBase
{
    private readonly ITaktEmployeeOnboardingService _employeeOnboardingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeOnboardingService">入职待办服务</param>
    public TaktEmployeeOnboardingsController(ITaktEmployeeOnboardingService employeeOnboardingService)
    {
        _employeeOnboardingService = employeeOnboardingService;
    }

    /// <summary>
    /// 获取入职待办列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:personnel:employee:onboarding:list", "入职待办列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeOnboardingListAsync([FromQuery] TaktEmployeeOnboardingQueryDto queryDto)
    {
        try
        {
            var result = await _employeeOnboardingService.GetEmployeeOnboardingListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <returns>入职待办DTO</returns>
    [TaktPermission("human:resource:personnel:employee:onboarding:query", "入职待办详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeOnboardingByIdAsync(long id)
    {
        try
        {
            var result = await _employeeOnboardingService.GetEmployeeOnboardingByIdAsync(id);
            if (result == null)
            {
                return NotFound("入职待办不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取入职待办选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:personnel:employee:onboarding:query", "入职待办选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeOnboardingOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _employeeOnboardingService.GetEmployeeOnboardingOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建入职待办
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>入职待办DTO</returns>
    [TaktPermission("human:resource:personnel:employee:onboarding:create", "创建入职待办")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeOnboardingAsync([FromBody] TaktEmployeeOnboardingCreateDto dto)
    {
        try
        {
            var result = await _employeeOnboardingService.CreateEmployeeOnboardingAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>入职待办DTO</returns>
    [TaktPermission("human:resource:personnel:employee:onboarding:update", "更新入职待办")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeOnboardingAsync(long id, [FromBody] TaktEmployeeOnboardingUpdateDto dto)
    {
        try
        {
            var result = await _employeeOnboardingService.UpdateEmployeeOnboardingAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:onboarding:delete", "删除入职待办")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeOnboardingByIdAsync(long id)
    {
        try
        {
            await _employeeOnboardingService.DeleteEmployeeOnboardingByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除入职待办
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:onboarding:delete", "批量删除入职待办")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeOnboardingBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeOnboardingService.DeleteEmployeeOnboardingBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新入职待办状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>入职待办DTO</returns>
    [TaktPermission("human:resource:personnel:employee:onboarding:update", "更新入职待办状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEmployeeOnboardingStatusAsync([FromBody] TaktEmployeeOnboardingStatusDto dto)
    {
        try
        {
            var result = await _employeeOnboardingService.UpdateEmployeeOnboardingStatusAsync(dto);
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
    [TaktPermission("human:resource:personnel:employee:onboarding:import", "获取入职待办导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeOnboardingTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeOnboardingService.GetEmployeeOnboardingTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入入职待办
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:personnel:employee:onboarding:import", "导入入职待办")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeOnboardingAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeOnboardingService.ImportEmployeeOnboardingAsync(stream, sheetName);
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
    /// 导出入职待办
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:personnel:employee:onboarding:export", "导出入职待办")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeOnboardingAsync([FromQuery] TaktEmployeeOnboardingQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeOnboardingService.ExportEmployeeOnboardingAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
