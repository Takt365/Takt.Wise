// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Benefits
// 文件名称：TaktSocialInsurancesController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：社保公积金控制器
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
/// 社保公积金控制器
/// 提供社保公积金的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "社保公积金")]
public class TaktSocialInsurancesController : TaktControllerBase
{
    private readonly ITaktSocialInsuranceService _socialInsuranceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="socialInsuranceService">社保公积金服务</param>
    public TaktSocialInsurancesController(ITaktSocialInsuranceService socialInsuranceService)
    {
        _socialInsuranceService = socialInsuranceService;
    }

    /// <summary>
    /// 获取社保公积金列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:benefits:social:insurance:list", "社保公积金列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSocialInsuranceListAsync([FromQuery] TaktSocialInsuranceQueryDto queryDto)
    {
        try
        {
            var result = await _socialInsuranceService.GetSocialInsuranceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取社保公积金
    /// </summary>
    /// <param name="id">社保公积金ID</param>
    /// <returns>社保公积金DTO</returns>
    [TaktPermission("human:resource:benefits:social:insurance:query", "社保公积金详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSocialInsuranceByIdAsync(long id)
    {
        try
        {
            var result = await _socialInsuranceService.GetSocialInsuranceByIdAsync(id);
            if (result == null)
            {
                return NotFound("社保公积金不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取社保公积金选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:benefits:social:insurance:query", "社保公积金选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSocialInsuranceOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _socialInsuranceService.GetSocialInsuranceOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建社保公积金
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>社保公积金DTO</returns>
    [TaktPermission("human:resource:benefits:social:insurance:create", "创建社保公积金")]
    [HttpPost]
    public async Task<IActionResult> CreateSocialInsuranceAsync([FromBody] TaktSocialInsuranceCreateDto dto)
    {
        try
        {
            var result = await _socialInsuranceService.CreateSocialInsuranceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新社保公积金
    /// </summary>
    /// <param name="id">社保公积金ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>社保公积金DTO</returns>
    [TaktPermission("human:resource:benefits:social:insurance:update", "更新社保公积金")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSocialInsuranceAsync(long id, [FromBody] TaktSocialInsuranceUpdateDto dto)
    {
        try
        {
            var result = await _socialInsuranceService.UpdateSocialInsuranceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除社保公积金
    /// </summary>
    /// <param name="id">社保公积金ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:benefits:social:insurance:delete", "删除社保公积金")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSocialInsuranceByIdAsync(long id)
    {
        try
        {
            await _socialInsuranceService.DeleteSocialInsuranceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除社保公积金
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:benefits:social:insurance:delete", "批量删除社保公积金")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSocialInsuranceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _socialInsuranceService.DeleteSocialInsuranceBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新社保公积金状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>社保公积金DTO</returns>
    [TaktPermission("human:resource:benefits:social:insurance:update", "更新社保公积金状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSocialInsuranceStatusAsync([FromBody] TaktSocialInsuranceStatusDto dto)
    {
        try
        {
            var result = await _socialInsuranceService.UpdateSocialInsuranceStatusAsync(dto);
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
    [TaktPermission("human:resource:benefits:social:insurance:import", "获取社保公积金导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSocialInsuranceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _socialInsuranceService.GetSocialInsuranceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入社保公积金
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:benefits:social:insurance:import", "导入社保公积金")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSocialInsuranceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _socialInsuranceService.ImportSocialInsuranceAsync(stream, sheetName);
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
    /// 导出社保公积金
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:benefits:social:insurance:export", "导出社保公积金")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSocialInsuranceAsync([FromQuery] TaktSocialInsuranceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _socialInsuranceService.ExportSocialInsuranceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
