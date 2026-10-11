// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeeResignationsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工离职控制器
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
/// 员工离职控制器
/// 提供员工离职的 REST API
/// </summary>
[ApiModule(5, "人事管理")]
[Route("api/[controller]", Name = "员工离职")]
public class TaktEmployeeResignationsController : TaktControllerBase
{
    private readonly ITaktEmployeeResignationService _employeeResignationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeResignationService">员工离职服务</param>
    public TaktEmployeeResignationsController(ITaktEmployeeResignationService employeeResignationService)
    {
        _employeeResignationService = employeeResignationService;
    }

    /// <summary>
    /// 获取员工离职列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:personnel:employee:resignation:list", "员工离职列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeResignationListAsync([FromQuery] TaktEmployeeResignationQueryDto queryDto)
    {
        try
        {
            var result = await _employeeResignationService.GetEmployeeResignationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工离职
    /// </summary>
    /// <param name="id">员工离职ID</param>
    /// <returns>员工离职DTO</returns>
    [TaktPermission("human:resource:personnel:employee:resignation:query", "员工离职详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeResignationByIdAsync(long id)
    {
        try
        {
            var result = await _employeeResignationService.GetEmployeeResignationByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工离职不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工离职选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:personnel:employee:resignation:query", "员工离职选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeResignationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _employeeResignationService.GetEmployeeResignationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工离职
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工离职DTO</returns>
    [TaktPermission("human:resource:personnel:employee:resignation:create", "创建员工离职")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeResignationAsync([FromBody] TaktEmployeeResignationCreateDto dto)
    {
        try
        {
            var result = await _employeeResignationService.CreateEmployeeResignationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工离职
    /// </summary>
    /// <param name="id">员工离职ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工离职DTO</returns>
    [TaktPermission("human:resource:personnel:employee:resignation:update", "更新员工离职")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeResignationAsync(long id, [FromBody] TaktEmployeeResignationUpdateDto dto)
    {
        try
        {
            var result = await _employeeResignationService.UpdateEmployeeResignationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工离职
    /// </summary>
    /// <param name="id">员工离职ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:resignation:delete", "删除员工离职")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeResignationByIdAsync(long id)
    {
        try
        {
            await _employeeResignationService.DeleteEmployeeResignationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工离职
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:resignation:delete", "批量删除员工离职")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeResignationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeResignationService.DeleteEmployeeResignationBatchAsync(ids);
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
    [TaktPermission("human:resource:personnel:employee:resignation:import", "获取员工离职导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeResignationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeResignationService.GetEmployeeResignationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工离职
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:personnel:employee:resignation:import", "导入员工离职")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeResignationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeResignationService.ImportEmployeeResignationAsync(stream, sheetName);
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
    /// 导出员工离职
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:personnel:employee:resignation:export", "导出员工离职")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeResignationAsync([FromQuery] TaktEmployeeResignationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeResignationService.ExportEmployeeResignationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
