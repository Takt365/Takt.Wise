// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Talent
// 文件名称：TaktTalentOffersController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：录用信息控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Talent;
using Takt.Application.Services.HumanResource.Talent;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Talent;

/// <summary>
/// 录用信息控制器
/// 提供录用信息的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "录用信息")]
public class TaktTalentOffersController : TaktControllerBase
{
    private readonly ITaktTalentOfferService _talentOfferService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentOfferService">录用信息服务</param>
    public TaktTalentOffersController(ITaktTalentOfferService talentOfferService)
    {
        _talentOfferService = talentOfferService;
    }

    /// <summary>
    /// 获取录用信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:talent:job:posting:list", "录用信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTalentOfferListAsync([FromQuery] TaktTalentOfferQueryDto queryDto)
    {
        try
        {
            var result = await _talentOfferService.GetTalentOfferListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取录用信息
    /// </summary>
    /// <param name="id">录用信息ID</param>
    /// <returns>录用信息DTO</returns>
    [TaktPermission("human:resource:talent:job:posting:query", "录用信息详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTalentOfferByIdAsync(long id)
    {
        try
        {
            var result = await _talentOfferService.GetTalentOfferByIdAsync(id);
            if (result == null)
            {
                return NotFound("录用信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取录用信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:talent:job:posting:query", "录用信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTalentOfferOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _talentOfferService.GetTalentOfferOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建录用信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>录用信息DTO</returns>
    [TaktPermission("human:resource:talent:job:posting:create", "创建录用信息")]
    [HttpPost]
    public async Task<IActionResult> CreateTalentOfferAsync([FromBody] TaktTalentOfferCreateDto dto)
    {
        try
        {
            var result = await _talentOfferService.CreateTalentOfferAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新录用信息
    /// </summary>
    /// <param name="id">录用信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>录用信息DTO</returns>
    [TaktPermission("human:resource:talent:job:posting:update", "更新录用信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTalentOfferAsync(long id, [FromBody] TaktTalentOfferUpdateDto dto)
    {
        try
        {
            var result = await _talentOfferService.UpdateTalentOfferAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除录用信息
    /// </summary>
    /// <param name="id">录用信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:talent:job:posting:delete", "删除录用信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTalentOfferByIdAsync(long id)
    {
        try
        {
            await _talentOfferService.DeleteTalentOfferByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除录用信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:talent:job:posting:delete", "批量删除录用信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTalentOfferBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _talentOfferService.DeleteTalentOfferBatchAsync(ids);
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
    [TaktPermission("human:resource:talent:job:posting:import", "获取录用信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTalentOfferTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _talentOfferService.GetTalentOfferTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入录用信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:talent:job:posting:import", "导入录用信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTalentOfferAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _talentOfferService.ImportTalentOfferAsync(stream, sheetName);
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
    /// 导出录用信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:talent:job:posting:export", "导出录用信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTalentOfferAsync([FromQuery] TaktTalentOfferQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _talentOfferService.ExportTalentOfferAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
