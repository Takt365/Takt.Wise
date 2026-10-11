// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeeAttachmentsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：员工附件控制器
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
/// 员工附件控制器
/// 提供员工附件的 REST API
/// </summary>
[ApiModule(5, "人事管理")]
[Route("api/[controller]", Name = "员工附件")]
public class TaktEmployeeAttachmentsController : TaktControllerBase
{
    private readonly ITaktEmployeeAttachmentService _employeeAttachmentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeAttachmentService">员工附件服务</param>
    public TaktEmployeeAttachmentsController(ITaktEmployeeAttachmentService employeeAttachmentService)
    {
        _employeeAttachmentService = employeeAttachmentService;
    }

    /// <summary>
    /// 获取员工附件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:personnel:employee:attachment:list", "员工附件列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeAttachmentListAsync([FromQuery] TaktEmployeeAttachmentQueryDto queryDto)
    {
        try
        {
            var result = await _employeeAttachmentService.GetEmployeeAttachmentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工附件
    /// </summary>
    /// <param name="id">员工附件ID</param>
    /// <returns>员工附件DTO</returns>
    [TaktPermission("human:resource:personnel:employee:attachment:query", "员工附件详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeAttachmentByIdAsync(long id)
    {
        try
        {
            var result = await _employeeAttachmentService.GetEmployeeAttachmentByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工附件不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工附件选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:personnel:employee:attachment:query", "员工附件选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeAttachmentOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _employeeAttachmentService.GetEmployeeAttachmentOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工附件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工附件DTO</returns>
    [TaktPermission("human:resource:personnel:employee:attachment:create", "创建员工附件")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeAttachmentAsync([FromBody] TaktEmployeeAttachmentCreateDto dto)
    {
        try
        {
            var result = await _employeeAttachmentService.CreateEmployeeAttachmentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工附件
    /// </summary>
    /// <param name="id">员工附件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工附件DTO</returns>
    [TaktPermission("human:resource:personnel:employee:attachment:update", "更新员工附件")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeAttachmentAsync(long id, [FromBody] TaktEmployeeAttachmentUpdateDto dto)
    {
        try
        {
            var result = await _employeeAttachmentService.UpdateEmployeeAttachmentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工附件
    /// </summary>
    /// <param name="id">员工附件ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:attachment:delete", "删除员工附件")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeAttachmentByIdAsync(long id)
    {
        try
        {
            await _employeeAttachmentService.DeleteEmployeeAttachmentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工附件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:personnel:employee:attachment:delete", "批量删除员工附件")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeAttachmentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeAttachmentService.DeleteEmployeeAttachmentBatchAsync(ids);
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
    [TaktPermission("human:resource:personnel:employee:attachment:import", "获取员工附件导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeAttachmentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeAttachmentService.GetEmployeeAttachmentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工附件
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:personnel:employee:attachment:import", "导入员工附件")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeAttachmentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeAttachmentService.ImportEmployeeAttachmentAsync(stream, sheetName);
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
    /// 导出员工附件
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:personnel:employee:attachment:export", "导出员工附件")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeAttachmentAsync([FromQuery] TaktEmployeeAttachmentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeAttachmentService.ExportEmployeeAttachmentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
