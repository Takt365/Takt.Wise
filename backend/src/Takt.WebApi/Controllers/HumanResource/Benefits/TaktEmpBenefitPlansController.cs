// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Benefits
// 文件名称：TaktEmpBenefitPlansController.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：员工福利方案控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Benefits;
using Takt.Application.Services.HumanResource.Benefits;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Benefits;

/// <summary>
/// 员工福利方案控制器
/// 提供员工福利方案的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "员工福利方案")]
public class TaktEmpBenefitPlansController : TaktControllerBase
{
    private readonly ITaktEmpBenefitPlanService _empBenefitPlanService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="empBenefitPlanService">员工福利方案服务</param>
    public TaktEmpBenefitPlansController(ITaktEmpBenefitPlanService empBenefitPlanService)
    {
        _empBenefitPlanService = empBenefitPlanService;
    }

    /// <summary>
    /// 获取员工福利方案列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:benefits:emp:benefit:plan:list", "员工福利方案列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmpBenefitPlanListAsync([FromQuery] TaktEmpBenefitPlanQueryDto queryDto)
    {
        try
        {
            var result = await _empBenefitPlanService.GetEmpBenefitPlanListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取员工福利方案
    /// </summary>
    /// <param name="id">员工福利方案ID</param>
    /// <returns>员工福利方案DTO</returns>
    [TaktPermission("human:resource:benefits:emp:benefit:plan:query", "员工福利方案详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmpBenefitPlanByIdAsync(long id)
    {
        try
        {
            var result = await _empBenefitPlanService.GetEmpBenefitPlanByIdAsync(id);
            if (result == null)
            {
                return NotFound("员工福利方案不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工福利方案选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:benefits:emp:benefit:plan:query", "员工福利方案选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmpBenefitPlanOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _empBenefitPlanService.GetEmpBenefitPlanOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建员工福利方案
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>员工福利方案DTO</returns>
    [TaktPermission("human:resource:benefits:emp:benefit:plan:create", "创建员工福利方案")]
    [HttpPost]
    public async Task<IActionResult> CreateEmpBenefitPlanAsync([FromBody] TaktEmpBenefitPlanCreateDto dto)
    {
        try
        {
            var result = await _empBenefitPlanService.CreateEmpBenefitPlanAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工福利方案
    /// </summary>
    /// <param name="id">员工福利方案ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>员工福利方案DTO</returns>
    [TaktPermission("human:resource:benefits:emp:benefit:plan:update", "更新员工福利方案")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmpBenefitPlanAsync(long id, [FromBody] TaktEmpBenefitPlanUpdateDto dto)
    {
        try
        {
            var result = await _empBenefitPlanService.UpdateEmpBenefitPlanAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除员工福利方案
    /// </summary>
    /// <param name="id">员工福利方案ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:benefits:emp:benefit:plan:delete", "删除员工福利方案")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmpBenefitPlanByIdAsync(long id)
    {
        try
        {
            await _empBenefitPlanService.DeleteEmpBenefitPlanByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除员工福利方案
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:benefits:emp:benefit:plan:delete", "批量删除员工福利方案")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmpBenefitPlanBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _empBenefitPlanService.DeleteEmpBenefitPlanBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新员工福利方案状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>员工福利方案DTO</returns>
    [TaktPermission("human:resource:benefits:emp:benefit:plan:update", "更新员工福利方案状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEmpBenefitPlanStatusAsync([FromBody] TaktEmpBenefitPlanStatusDto dto)
    {
        try
        {
            var result = await _empBenefitPlanService.UpdateEmpBenefitPlanStatusAsync(dto);
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
    [TaktPermission("human:resource:benefits:emp:benefit:plan:import", "获取员工福利方案导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmpBenefitPlanTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _empBenefitPlanService.GetEmpBenefitPlanTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入员工福利方案
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:benefits:emp:benefit:plan:import", "导入员工福利方案")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmpBenefitPlanAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _empBenefitPlanService.ImportEmpBenefitPlanAsync(stream, sheetName);
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
    /// 导出员工福利方案
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:benefits:emp:benefit:plan:export", "导出员工福利方案")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmpBenefitPlanAsync([FromQuery] TaktEmpBenefitPlanQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _empBenefitPlanService.ExportEmpBenefitPlanAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
