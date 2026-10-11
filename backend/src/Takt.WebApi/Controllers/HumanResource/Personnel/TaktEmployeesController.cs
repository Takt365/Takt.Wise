// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeesController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工控制器
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
/// 员工控制器
/// 提供员工的 REST API
/// </summary>
[ApiModule(5, "人事管理")]
[Route("api/[controller]", Name = "员工")]
public class TaktEmployeesController : TaktControllerBase
{
    private readonly ITaktEmployeeService _employeeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeService">员工服务</param>
    public TaktEmployeesController(ITaktEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    /// <summary>
    /// 获取员工列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:personnel:employee:list", "员工列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeListAsync([FromQuery] TaktEmployeeQueryDto queryDto)
    {
        try
        {
            var result = await _employeeService.GetEmployeeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工
    /// </summary>
    /// <param name="id">员工ID</param>
    /// <returns>员工DTO</returns>
    [TaktPermission("human:resource:personnel:employee:query", "员工详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeByIdAsync(long id)
    {
        try
        {
            var result = await _employeeService.GetEmployeeByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:personnel:employee:query", "员工选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _employeeService.GetEmployeeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工DTO</returns>
    [TaktPermission("human:resource:personnel:employee:create", "创建员工")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeAsync([FromBody] TaktEmployeeCreateDto dto)
    {
        try
        {
            var result = await _employeeService.CreateEmployeeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工
    /// </summary>
    /// <param name="id">员工ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工DTO</returns>
    [TaktPermission("human:resource:personnel:employee:update", "更新员工")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeAsync(long id, [FromBody] TaktEmployeeUpdateDto dto)
    {
        try
        {
            var result = await _employeeService.UpdateEmployeeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工
    /// </summary>
    /// <param name="id">员工ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:delete", "删除员工")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeByIdAsync(long id)
    {
        try
        {
            await _employeeService.DeleteEmployeeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:delete", "批量删除员工")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeService.DeleteEmployeeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>员工DTO</returns>
    [TaktPermission("human:resource:personnel:employee:update", "更新员工状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEmployeeStatusAsync([FromBody] TaktEmployeeStatusDto dto)
    {
        try
        {
            var result = await _employeeService.UpdateEmployeeStatusAsync(dto);
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
    [TaktPermission("human:resource:personnel:employee:import", "获取员工导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeService.GetEmployeeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:personnel:employee:import", "导入员工")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeService.ImportEmployeeAsync(stream, sheetName);
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
    /// 导出员工
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:personnel:employee:export", "导出员工")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeAsync([FromQuery] TaktEmployeeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeService.ExportEmployeeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
