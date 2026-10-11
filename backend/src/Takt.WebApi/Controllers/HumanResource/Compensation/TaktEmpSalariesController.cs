// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Compensation
// 文件名称：TaktEmpSalariesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：员工薪酬控制器
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
/// 员工薪酬控制器
/// 提供员工薪酬的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "员工薪酬")]
public class TaktEmpSalariesController : TaktControllerBase
{
    private readonly ITaktEmpSalaryService _empSalaryService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="empSalaryService">员工薪酬服务</param>
    public TaktEmpSalariesController(ITaktEmpSalaryService empSalaryService)
    {
        _empSalaryService = empSalaryService;
    }

    /// <summary>
    /// 获取员工薪酬列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:compensation:emp:salary:list", "员工薪酬列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmpSalaryListAsync([FromQuery] TaktEmpSalaryQueryDto queryDto)
    {
        try
        {
            var result = await _empSalaryService.GetEmpSalaryListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工薪酬
    /// </summary>
    /// <param name="id">员工薪酬ID</param>
    /// <returns>员工薪酬DTO</returns>
    [TaktPermission("human:resource:compensation:emp:salary:query", "员工薪酬详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmpSalaryByIdAsync(long id)
    {
        try
        {
            var result = await _empSalaryService.GetEmpSalaryByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工薪酬不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工薪酬选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:compensation:emp:salary:query", "员工薪酬选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmpSalaryOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _empSalaryService.GetEmpSalaryOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工薪酬
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工薪酬DTO</returns>
    [TaktPermission("human:resource:compensation:emp:salary:create", "创建员工薪酬")]
    [HttpPost]
    public async Task<IActionResult> CreateEmpSalaryAsync([FromBody] TaktEmpSalaryCreateDto dto)
    {
        try
        {
            var result = await _empSalaryService.CreateEmpSalaryAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工薪酬
    /// </summary>
    /// <param name="id">员工薪酬ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工薪酬DTO</returns>
    [TaktPermission("human:resource:compensation:emp:salary:update", "更新员工薪酬")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmpSalaryAsync(long id, [FromBody] TaktEmpSalaryUpdateDto dto)
    {
        try
        {
            var result = await _empSalaryService.UpdateEmpSalaryAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工薪酬
    /// </summary>
    /// <param name="id">员工薪酬ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:emp:salary:delete", "删除员工薪酬")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmpSalaryByIdAsync(long id)
    {
        try
        {
            await _empSalaryService.DeleteEmpSalaryByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工薪酬
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:emp:salary:delete", "批量删除员工薪酬")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmpSalaryBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _empSalaryService.DeleteEmpSalaryBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工薪酬状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>员工薪酬DTO</returns>
    [TaktPermission("human:resource:compensation:emp:salary:update", "更新员工薪酬状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEmpSalaryStatusAsync([FromBody] TaktEmpSalaryStatusDto dto)
    {
        try
        {
            var result = await _empSalaryService.UpdateEmpSalaryStatusAsync(dto);
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
    [TaktPermission("human:resource:compensation:emp:salary:import", "获取员工薪酬导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmpSalaryTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _empSalaryService.GetEmpSalaryTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工薪酬
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:compensation:emp:salary:import", "导入员工薪酬")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmpSalaryAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _empSalaryService.ImportEmpSalaryAsync(stream, sheetName);
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
    /// 导出员工薪酬
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:compensation:emp:salary:export", "导出员工薪酬")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmpSalaryAsync([FromQuery] TaktEmpSalaryQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _empSalaryService.ExportEmpSalaryAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
