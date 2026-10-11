// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeeJoinedsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工入职上岗控制器
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
/// 员工入职上岗控制器
/// 提供员工入职上岗的 REST API
/// </summary>
[ApiModule(5, "人事管理")]
[Route("api/[controller]", Name = "员工入职上岗")]
public class TaktEmployeeJoinedsController : TaktControllerBase
{
    private readonly ITaktEmployeeJoinedService _employeeJoinedService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeJoinedService">员工入职上岗服务</param>
    public TaktEmployeeJoinedsController(ITaktEmployeeJoinedService employeeJoinedService)
    {
        _employeeJoinedService = employeeJoinedService;
    }

    /// <summary>
    /// 获取员工入职上岗列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:personnel:employee:joined:list", "员工入职上岗列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeJoinedListAsync([FromQuery] TaktEmployeeJoinedQueryDto queryDto)
    {
        try
        {
            var result = await _employeeJoinedService.GetEmployeeJoinedListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工入职上岗
    /// </summary>
    /// <param name="id">员工入职上岗ID</param>
    /// <returns>员工入职上岗DTO</returns>
    [TaktPermission("human:resource:personnel:employee:joined:query", "员工入职上岗详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeJoinedByIdAsync(long id)
    {
        try
        {
            var result = await _employeeJoinedService.GetEmployeeJoinedByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工入职上岗不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工入职上岗选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:personnel:employee:joined:query", "员工入职上岗选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeJoinedOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _employeeJoinedService.GetEmployeeJoinedOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工入职上岗
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工入职上岗DTO</returns>
    [TaktPermission("human:resource:personnel:employee:joined:create", "创建员工入职上岗")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeJoinedAsync([FromBody] TaktEmployeeJoinedCreateDto dto)
    {
        try
        {
            var result = await _employeeJoinedService.CreateEmployeeJoinedAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工入职上岗
    /// </summary>
    /// <param name="id">员工入职上岗ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工入职上岗DTO</returns>
    [TaktPermission("human:resource:personnel:employee:joined:update", "更新员工入职上岗")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeJoinedAsync(long id, [FromBody] TaktEmployeeJoinedUpdateDto dto)
    {
        try
        {
            var result = await _employeeJoinedService.UpdateEmployeeJoinedAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工入职上岗
    /// </summary>
    /// <param name="id">员工入职上岗ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:joined:delete", "删除员工入职上岗")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeJoinedByIdAsync(long id)
    {
        try
        {
            await _employeeJoinedService.DeleteEmployeeJoinedByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工入职上岗
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:joined:delete", "批量删除员工入职上岗")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeJoinedBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeJoinedService.DeleteEmployeeJoinedBatchAsync(ids);
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
    [TaktPermission("human:resource:personnel:employee:joined:import", "获取员工入职上岗导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeJoinedTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeJoinedService.GetEmployeeJoinedTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工入职上岗
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:personnel:employee:joined:import", "导入员工入职上岗")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeJoinedAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeJoinedService.ImportEmployeeJoinedAsync(stream, sheetName);
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
    /// 导出员工入职上岗
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:personnel:employee:joined:export", "导出员工入职上岗")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeJoinedAsync([FromQuery] TaktEmployeeJoinedQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeJoinedService.ExportEmployeeJoinedAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
