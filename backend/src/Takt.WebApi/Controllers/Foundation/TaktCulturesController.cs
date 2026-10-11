// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktCulturesController.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：区域控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 区域控制器
/// 提供区域的 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "区域")]
public class TaktCulturesController : TaktControllerBase
{
    private readonly ITaktCultureService _cultureService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cultureService">区域服务</param>
    public TaktCulturesController(ITaktCultureService cultureService)
    {
        _cultureService = cultureService;
    }

    /// <summary>
    /// 获取区域列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:i18n:list", "区域列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCultureListAsync([FromQuery] TaktCultureQueryDto queryDto)
    {
        try
        {
            var result = await _cultureService.GetCultureListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取区域
    /// </summary>
    /// <param name="id">区域ID</param>
    /// <returns>区域DTO</returns>
    [TaktPermission("foundation:i18n:query", "区域详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCultureByIdAsync(long id)
    {
        try
        {
            var result = await _cultureService.GetCultureByIdAsync(id);
            if (result == null)
            {
                return NotFound("区域不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取语言切换选项列表（仅启用，TaktSelectOption）
    /// </summary>
    /// <returns>下拉选项</returns>
    [AllowAnonymous]
    [HttpGet("options")]
    public async Task<IActionResult> GetCultureOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _cultureService.GetCultureOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建区域
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>区域DTO</returns>
    [TaktPermission("foundation:i18n:create", "创建区域")]
    [HttpPost]
    public async Task<IActionResult> CreateCultureAsync([FromBody] TaktCultureCreateDto dto)
    {
        try
        {
            var result = await _cultureService.CreateCultureAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新区域
    /// </summary>
    /// <param name="id">区域ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>区域DTO</returns>
    [TaktPermission("foundation:i18n:update", "更新区域")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCultureAsync(long id, [FromBody] TaktCultureUpdateDto dto)
    {
        try
        {
            var result = await _cultureService.UpdateCultureAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除区域
    /// </summary>
    /// <param name="id">区域ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:i18n:delete", "删除区域")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCultureByIdAsync(long id)
    {
        try
        {
            await _cultureService.DeleteCultureByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除区域
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:i18n:delete", "批量删除区域")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCultureBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _cultureService.DeleteCultureBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新区域排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>区域DTO</returns>
    [TaktPermission("foundation:i18n:update", "更新区域排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCultureSortAsync([FromBody] TaktCultureSortDto dto)
    {
        try
        {
            var result = await _cultureService.UpdateCultureSortAsync(dto);
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
    [TaktPermission("foundation:i18n:import", "获取区域导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCultureTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _cultureService.GetCultureTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入区域
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:i18n:import", "导入区域")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCultureAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _cultureService.ImportCultureAsync(stream, sheetName);
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
    /// 导出区域
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:i18n:export", "导出区域")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCultureAsync([FromQuery] TaktCultureQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _cultureService.ExportCultureAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
