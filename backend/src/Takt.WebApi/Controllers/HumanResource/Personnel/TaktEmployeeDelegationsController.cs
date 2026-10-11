// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeeDelegationsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工代理关系控制器
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
/// 员工代理关系控制器
/// 提供员工代理关系的 REST API
/// </summary>
[ApiModule(5, "人事管理")]
[Route("api/[controller]", Name = "员工代理关系")]
public class TaktEmployeeDelegationsController : TaktControllerBase
{
    private readonly ITaktEmployeeDelegationService _employeeDelegationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeDelegationService">员工代理关系服务</param>
    public TaktEmployeeDelegationsController(ITaktEmployeeDelegationService employeeDelegationService)
    {
        _employeeDelegationService = employeeDelegationService;
    }

    /// <summary>
    /// 获取员工代理关系列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:personnel:employee:delegation:list", "员工代理关系列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeDelegationListAsync([FromQuery] TaktEmployeeDelegationQueryDto queryDto)
    {
        try
        {
            var result = await _employeeDelegationService.GetEmployeeDelegationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工代理关系
    /// </summary>
    /// <param name="id">员工代理关系ID</param>
    /// <returns>员工代理关系DTO</returns>
    [TaktPermission("human:resource:personnel:employee:delegation:query", "员工代理关系详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeDelegationByIdAsync(long id)
    {
        try
        {
            var result = await _employeeDelegationService.GetEmployeeDelegationByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工代理关系不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工代理关系选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:personnel:employee:delegation:query", "员工代理关系选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeDelegationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _employeeDelegationService.GetEmployeeDelegationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工代理关系
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工代理关系DTO</returns>
    [TaktPermission("human:resource:personnel:employee:delegation:create", "创建员工代理关系")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeDelegationAsync([FromBody] TaktEmployeeDelegationCreateDto dto)
    {
        try
        {
            var result = await _employeeDelegationService.CreateEmployeeDelegationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工代理关系
    /// </summary>
    /// <param name="id">员工代理关系ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工代理关系DTO</returns>
    [TaktPermission("human:resource:personnel:employee:delegation:update", "更新员工代理关系")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeDelegationAsync(long id, [FromBody] TaktEmployeeDelegationUpdateDto dto)
    {
        try
        {
            var result = await _employeeDelegationService.UpdateEmployeeDelegationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工代理关系
    /// </summary>
    /// <param name="id">员工代理关系ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:delegation:delete", "删除员工代理关系")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeDelegationByIdAsync(long id)
    {
        try
        {
            await _employeeDelegationService.DeleteEmployeeDelegationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工代理关系
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:delegation:delete", "批量删除员工代理关系")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeDelegationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeDelegationService.DeleteEmployeeDelegationBatchAsync(ids);
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
    [TaktPermission("human:resource:personnel:employee:delegation:import", "获取员工代理关系导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeDelegationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeDelegationService.GetEmployeeDelegationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工代理关系
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:personnel:employee:delegation:import", "导入员工代理关系")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeDelegationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeDelegationService.ImportEmployeeDelegationAsync(stream, sheetName);
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
    /// 导出员工代理关系
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:personnel:employee:delegation:export", "导出员工代理关系")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeDelegationAsync([FromQuery] TaktEmployeeDelegationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeDelegationService.ExportEmployeeDelegationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
