// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Compensation
// 文件名称：TaktPayrollsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：薪酬体系控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Compensation;
using Takt.Application.Services.HumanResource.Compensation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Compensation;

/// <summary>
/// 薪酬体系控制器
/// 提供薪酬体系的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "薪酬体系")]
public class TaktPayrollsController : TaktControllerBase
{
    private readonly ITaktPayrollService _payrollService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="payrollService">薪酬体系服务</param>
    public TaktPayrollsController(ITaktPayrollService payrollService)
    {
        _payrollService = payrollService;
    }

    /// <summary>
    /// 获取薪酬体系列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:compensation:payroll:list", "薪酬体系列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPayrollListAsync([FromQuery] TaktPayrollQueryDto queryDto)
    {
        try
        {
            var result = await _payrollService.GetPayrollListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取薪酬体系
    /// </summary>
    /// <param name="id">薪酬体系ID</param>
    /// <returns>薪酬体系DTO</returns>
    [TaktPermission("human:resource:compensation:payroll:query", "薪酬体系详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPayrollByIdAsync(long id)
    {
        try
        {
            var result = await _payrollService.GetPayrollByIdAsync(id);
            if (result == null)
            {
                return NotFound("薪酬体系不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取薪酬体系选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:compensation:payroll:query", "薪酬体系选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPayrollOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _payrollService.GetPayrollOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建薪酬体系
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>薪酬体系DTO</returns>
    [TaktPermission("human:resource:compensation:payroll:create", "创建薪酬体系")]
    [HttpPost]
    public async Task<IActionResult> CreatePayrollAsync([FromBody] TaktPayrollCreateDto dto)
    {
        try
        {
            var result = await _payrollService.CreatePayrollAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪酬体系
    /// </summary>
    /// <param name="id">薪酬体系ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>薪酬体系DTO</returns>
    [TaktPermission("human:resource:compensation:payroll:update", "更新薪酬体系")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayrollAsync(long id, [FromBody] TaktPayrollUpdateDto dto)
    {
        try
        {
            var result = await _payrollService.UpdatePayrollAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除薪酬体系
    /// </summary>
    /// <param name="id">薪酬体系ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:payroll:delete", "删除薪酬体系")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayrollByIdAsync(long id)
    {
        try
        {
            await _payrollService.DeletePayrollByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除薪酬体系
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:payroll:delete", "批量删除薪酬体系")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePayrollBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _payrollService.DeletePayrollBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪酬体系状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>薪酬体系DTO</returns>
    [TaktPermission("human:resource:compensation:payroll:update", "更新薪酬体系状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePayrollStatusAsync([FromBody] TaktPayrollStatusDto dto)
    {
        try
        {
            var result = await _payrollService.UpdatePayrollStatusAsync(dto);
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
    [TaktPermission("human:resource:compensation:payroll:import", "获取薪酬体系导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPayrollTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _payrollService.GetPayrollTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入薪酬体系
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:compensation:payroll:import", "导入薪酬体系")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPayrollAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _payrollService.ImportPayrollAsync(stream, sheetName);
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
    /// 导出薪酬体系
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:compensation:payroll:export", "导出薪酬体系")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPayrollAsync([FromQuery] TaktPayrollQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _payrollService.ExportPayrollAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
