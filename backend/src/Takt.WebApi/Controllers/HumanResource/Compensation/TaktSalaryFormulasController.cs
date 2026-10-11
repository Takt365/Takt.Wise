// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Compensation
// 文件名称：TaktSalaryFormulasController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：薪资计算公式控制器
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
/// 薪资计算公式控制器
/// 提供薪资计算公式的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "薪资计算公式")]
public class TaktSalaryFormulasController : TaktControllerBase
{
    private readonly ITaktSalaryFormulaService _salaryFormulaService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salaryFormulaService">薪资计算公式服务</param>
    public TaktSalaryFormulasController(ITaktSalaryFormulaService salaryFormulaService)
    {
        _salaryFormulaService = salaryFormulaService;
    }

    /// <summary>
    /// 获取薪资计算公式列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:compensation:salary:formula:list", "薪资计算公式列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalaryFormulaListAsync([FromQuery] TaktSalaryFormulaQueryDto queryDto)
    {
        try
        {
            var result = await _salaryFormulaService.GetSalaryFormulaListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取薪资计算公式
    /// </summary>
    /// <param name="id">薪资计算公式ID</param>
    /// <returns>薪资计算公式DTO</returns>
    [TaktPermission("human:resource:compensation:salary:formula:query", "薪资计算公式详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalaryFormulaByIdAsync(long id)
    {
        try
        {
            var result = await _salaryFormulaService.GetSalaryFormulaByIdAsync(id);
            if (result == null)
            {
                return NotFound("薪资计算公式不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取薪资计算公式选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:compensation:salary:formula:query", "薪资计算公式选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalaryFormulaOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salaryFormulaService.GetSalaryFormulaOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建薪资计算公式
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>薪资计算公式DTO</returns>
    [TaktPermission("human:resource:compensation:salary:formula:create", "创建薪资计算公式")]
    [HttpPost]
    public async Task<IActionResult> CreateSalaryFormulaAsync([FromBody] TaktSalaryFormulaCreateDto dto)
    {
        try
        {
            var result = await _salaryFormulaService.CreateSalaryFormulaAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪资计算公式
    /// </summary>
    /// <param name="id">薪资计算公式ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>薪资计算公式DTO</returns>
    [TaktPermission("human:resource:compensation:salary:formula:update", "更新薪资计算公式")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalaryFormulaAsync(long id, [FromBody] TaktSalaryFormulaUpdateDto dto)
    {
        try
        {
            var result = await _salaryFormulaService.UpdateSalaryFormulaAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除薪资计算公式
    /// </summary>
    /// <param name="id">薪资计算公式ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:salary:formula:delete", "删除薪资计算公式")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalaryFormulaByIdAsync(long id)
    {
        try
        {
            await _salaryFormulaService.DeleteSalaryFormulaByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除薪资计算公式
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:salary:formula:delete", "批量删除薪资计算公式")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalaryFormulaBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salaryFormulaService.DeleteSalaryFormulaBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪资计算公式状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>薪资计算公式DTO</returns>
    [TaktPermission("human:resource:compensation:salary:formula:update", "更新薪资计算公式状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSalaryFormulaStatusAsync([FromBody] TaktSalaryFormulaStatusDto dto)
    {
        try
        {
            var result = await _salaryFormulaService.UpdateSalaryFormulaStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪资计算公式排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>薪资计算公式DTO</returns>
    [TaktPermission("human:resource:compensation:salary:formula:update", "更新薪资计算公式排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSalaryFormulaSortAsync([FromBody] TaktSalaryFormulaSortDto dto)
    {
        try
        {
            var result = await _salaryFormulaService.UpdateSalaryFormulaSortAsync(dto);
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
    [TaktPermission("human:resource:compensation:salary:formula:import", "获取薪资计算公式导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalaryFormulaTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salaryFormulaService.GetSalaryFormulaTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入薪资计算公式
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:compensation:salary:formula:import", "导入薪资计算公式")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalaryFormulaAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salaryFormulaService.ImportSalaryFormulaAsync(stream, sheetName);
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
    /// 导出薪资计算公式
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:compensation:salary:formula:export", "导出薪资计算公式")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalaryFormulaAsync([FromQuery] TaktSalaryFormulaQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salaryFormulaService.ExportSalaryFormulaAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
