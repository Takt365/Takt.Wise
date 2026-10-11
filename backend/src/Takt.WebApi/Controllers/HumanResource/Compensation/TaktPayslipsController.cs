// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Compensation
// 文件名称：TaktPayslipsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工资条控制器
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
/// 工资条控制器
/// 提供工资条的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "工资条")]
public class TaktPayslipsController : TaktControllerBase
{
    private readonly ITaktPayslipService _payslipService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="payslipService">工资条服务</param>
    public TaktPayslipsController(ITaktPayslipService payslipService)
    {
        _payslipService = payslipService;
    }

    /// <summary>
    /// 获取工资条列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:compensation:payslip:list", "工资条列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPayslipListAsync([FromQuery] TaktPayslipQueryDto queryDto)
    {
        try
        {
            var result = await _payslipService.GetPayslipListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工资条
    /// </summary>
    /// <param name="id">工资条ID</param>
    /// <returns>工资条DTO</returns>
    [TaktPermission("human:resource:compensation:payslip:query", "工资条详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPayslipByIdAsync(long id)
    {
        try
        {
            var result = await _payslipService.GetPayslipByIdAsync(id);
            if (result == null)
            {
                return NotFound("工资条不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工资条选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:compensation:payslip:query", "工资条选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPayslipOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _payslipService.GetPayslipOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工资条
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工资条DTO</returns>
    [TaktPermission("human:resource:compensation:payslip:create", "创建工资条")]
    [HttpPost]
    public async Task<IActionResult> CreatePayslipAsync([FromBody] TaktPayslipCreateDto dto)
    {
        try
        {
            var result = await _payslipService.CreatePayslipAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工资条
    /// </summary>
    /// <param name="id">工资条ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工资条DTO</returns>
    [TaktPermission("human:resource:compensation:payslip:update", "更新工资条")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayslipAsync(long id, [FromBody] TaktPayslipUpdateDto dto)
    {
        try
        {
            var result = await _payslipService.UpdatePayslipAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工资条
    /// </summary>
    /// <param name="id">工资条ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:payslip:delete", "删除工资条")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayslipByIdAsync(long id)
    {
        try
        {
            await _payslipService.DeletePayslipByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工资条
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:payslip:delete", "批量删除工资条")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePayslipBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _payslipService.DeletePayslipBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工资条状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>工资条DTO</returns>
    [TaktPermission("human:resource:compensation:payslip:update", "更新工资条状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePayslipStatusAsync([FromBody] TaktPayslipStatusDto dto)
    {
        try
        {
            var result = await _payslipService.UpdatePayslipStatusAsync(dto);
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
    [TaktPermission("human:resource:compensation:payslip:import", "获取工资条导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPayslipTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _payslipService.GetPayslipTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工资条
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:compensation:payslip:import", "导入工资条")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPayslipAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _payslipService.ImportPayslipAsync(stream, sheetName);
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
    /// 导出工资条
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:compensation:payslip:export", "导出工资条")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPayslipAsync([FromQuery] TaktPayslipQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _payslipService.ExportPayslipAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
