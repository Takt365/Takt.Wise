// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeeEducationsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工教育经历控制器
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
/// 员工教育经历控制器
/// 提供员工教育经历的 REST API
/// </summary>
[ApiModule(5, "人事管理")]
[Route("api/[controller]", Name = "员工教育经历")]
public class TaktEmployeeEducationsController : TaktControllerBase
{
    private readonly ITaktEmployeeEducationService _employeeEducationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeEducationService">员工教育经历服务</param>
    public TaktEmployeeEducationsController(ITaktEmployeeEducationService employeeEducationService)
    {
        _employeeEducationService = employeeEducationService;
    }

    /// <summary>
    /// 获取员工教育经历列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:personnel:employee:education:list", "员工教育经历列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeEducationListAsync([FromQuery] TaktEmployeeEducationQueryDto queryDto)
    {
        try
        {
            var result = await _employeeEducationService.GetEmployeeEducationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工教育经历
    /// </summary>
    /// <param name="id">员工教育经历ID</param>
    /// <returns>员工教育经历DTO</returns>
    [TaktPermission("human:resource:personnel:employee:education:query", "员工教育经历详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeEducationByIdAsync(long id)
    {
        try
        {
            var result = await _employeeEducationService.GetEmployeeEducationByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工教育经历不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工教育经历选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:personnel:employee:education:query", "员工教育经历选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeEducationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _employeeEducationService.GetEmployeeEducationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工教育经历
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工教育经历DTO</returns>
    [TaktPermission("human:resource:personnel:employee:education:create", "创建员工教育经历")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeEducationAsync([FromBody] TaktEmployeeEducationCreateDto dto)
    {
        try
        {
            var result = await _employeeEducationService.CreateEmployeeEducationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工教育经历
    /// </summary>
    /// <param name="id">员工教育经历ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工教育经历DTO</returns>
    [TaktPermission("human:resource:personnel:employee:education:update", "更新员工教育经历")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeEducationAsync(long id, [FromBody] TaktEmployeeEducationUpdateDto dto)
    {
        try
        {
            var result = await _employeeEducationService.UpdateEmployeeEducationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工教育经历
    /// </summary>
    /// <param name="id">员工教育经历ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:education:delete", "删除员工教育经历")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeEducationByIdAsync(long id)
    {
        try
        {
            await _employeeEducationService.DeleteEmployeeEducationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工教育经历
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:education:delete", "批量删除员工教育经历")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeEducationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeEducationService.DeleteEmployeeEducationBatchAsync(ids);
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
    [TaktPermission("human:resource:personnel:employee:education:import", "获取员工教育经历导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeEducationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeEducationService.GetEmployeeEducationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工教育经历
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:personnel:employee:education:import", "导入员工教育经历")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeEducationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeEducationService.ImportEmployeeEducationAsync(stream, sheetName);
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
    /// 导出员工教育经历
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:personnel:employee:education:export", "导出员工教育经历")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeEducationAsync([FromQuery] TaktEmployeeEducationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeEducationService.ExportEmployeeEducationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
