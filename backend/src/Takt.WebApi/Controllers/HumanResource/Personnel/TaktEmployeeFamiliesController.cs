// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeeFamiliesController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工家庭成员控制器
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
/// 员工家庭成员控制器
/// 提供员工家庭成员的 REST API
/// </summary>
[ApiModule(5, "人事管理")]
[Route("api/[controller]", Name = "员工家庭成员")]
public class TaktEmployeeFamiliesController : TaktControllerBase
{
    private readonly ITaktEmployeeFamilyService _employeeFamilyService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeFamilyService">员工家庭成员服务</param>
    public TaktEmployeeFamiliesController(ITaktEmployeeFamilyService employeeFamilyService)
    {
        _employeeFamilyService = employeeFamilyService;
    }

    /// <summary>
    /// 获取员工家庭成员列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:personnel:employee:family:list", "员工家庭成员列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeFamilyListAsync([FromQuery] TaktEmployeeFamilyQueryDto queryDto)
    {
        try
        {
            var result = await _employeeFamilyService.GetEmployeeFamilyListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工家庭成员
    /// </summary>
    /// <param name="id">员工家庭成员ID</param>
    /// <returns>员工家庭成员DTO</returns>
    [TaktPermission("human:resource:personnel:employee:family:query", "员工家庭成员详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeFamilyByIdAsync(long id)
    {
        try
        {
            var result = await _employeeFamilyService.GetEmployeeFamilyByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工家庭成员不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工家庭成员选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:personnel:employee:family:query", "员工家庭成员选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeFamilyOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _employeeFamilyService.GetEmployeeFamilyOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工家庭成员
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工家庭成员DTO</returns>
    [TaktPermission("human:resource:personnel:employee:family:create", "创建员工家庭成员")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeFamilyAsync([FromBody] TaktEmployeeFamilyCreateDto dto)
    {
        try
        {
            var result = await _employeeFamilyService.CreateEmployeeFamilyAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工家庭成员
    /// </summary>
    /// <param name="id">员工家庭成员ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工家庭成员DTO</returns>
    [TaktPermission("human:resource:personnel:employee:family:update", "更新员工家庭成员")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeFamilyAsync(long id, [FromBody] TaktEmployeeFamilyUpdateDto dto)
    {
        try
        {
            var result = await _employeeFamilyService.UpdateEmployeeFamilyAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工家庭成员
    /// </summary>
    /// <param name="id">员工家庭成员ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:family:delete", "删除员工家庭成员")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeFamilyByIdAsync(long id)
    {
        try
        {
            await _employeeFamilyService.DeleteEmployeeFamilyByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工家庭成员
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:family:delete", "批量删除员工家庭成员")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeFamilyBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeFamilyService.DeleteEmployeeFamilyBatchAsync(ids);
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
    [TaktPermission("human:resource:personnel:employee:family:import", "获取员工家庭成员导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeFamilyTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeFamilyService.GetEmployeeFamilyTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工家庭成员
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:personnel:employee:family:import", "导入员工家庭成员")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeFamilyAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeFamilyService.ImportEmployeeFamilyAsync(stream, sheetName);
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
    /// 导出员工家庭成员
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:personnel:employee:family:export", "导出员工家庭成员")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeFamilyAsync([FromQuery] TaktEmployeeFamilyQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeFamilyService.ExportEmployeeFamilyAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
