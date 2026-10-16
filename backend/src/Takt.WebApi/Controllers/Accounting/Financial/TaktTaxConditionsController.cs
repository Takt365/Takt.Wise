// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktTaxConditionsController.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Cursor AI)
// 功能描述：税务条件控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Application.Services.Accounting.Financial;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Accounting.Financial;

/// <summary>
/// 税务条件控制器
/// 提供税务条件的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "税务条件")]
public class TaktTaxConditionsController : TaktControllerBase
{
    private readonly ITaktTaxConditionService _taxConditionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="taxConditionService">税务条件服务</param>
    public TaktTaxConditionsController(ITaktTaxConditionService taxConditionService)
    {
        _taxConditionService = taxConditionService;
    }

    /// <summary>
    /// 获取税务条件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:tax:condition:list", "税务条件列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTaxConditionListAsync([FromQuery] TaktTaxConditionQueryDto queryDto)
    {
        try
        {
            var result = await _taxConditionService.GetTaxConditionListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取税务条件
    /// </summary>
    /// <param name="id">税务条件ID</param>
    /// <returns>税务条件DTO</returns>
    [TaktPermission("accounting:financial:tax:condition:query", "税务条件详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetTaxConditionByIdAsync(long id)
    {
        try
        {
            var result = await _taxConditionService.GetTaxConditionByIdAsync(id);
            if (result == null)
            {
                return NotFound("税务条件不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取税务条件选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:tax:condition:query", "税务条件选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTaxConditionOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _taxConditionService.GetTaxConditionOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建税务条件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>税务条件DTO</returns>
    [TaktPermission("accounting:financial:tax:condition:create", "创建税务条件")]
    [HttpPost]
    public async Task<IActionResult> CreateTaxConditionAsync([FromBody] TaktTaxConditionCreateDto dto)
    {
        try
        {
            var result = await _taxConditionService.CreateTaxConditionAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新税务条件
    /// </summary>
    /// <param name="id">税务条件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>税务条件DTO</returns>
    [TaktPermission("accounting:financial:tax:condition:update", "更新税务条件")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTaxConditionAsync(long id, [FromBody] TaktTaxConditionUpdateDto dto)
    {
        try
        {
            var result = await _taxConditionService.UpdateTaxConditionAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除税务条件
    /// </summary>
    /// <param name="id">税务条件ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:tax:condition:delete", "删除税务条件")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaxConditionByIdAsync(long id)
    {
        try
        {
            await _taxConditionService.DeleteTaxConditionByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除税务条件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:tax:condition:delete", "批量删除税务条件")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTaxConditionBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _taxConditionService.DeleteTaxConditionBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新税务条件状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>税务条件DTO</returns>
    [TaktPermission("accounting:financial:tax:condition:update", "更新税务条件状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTaxConditionStatusAsync([FromBody] TaktTaxConditionStatusDto dto)
    {
        try
        {
            var result = await _taxConditionService.UpdateTaxConditionStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新税务条件排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>税务条件DTO</returns>
    [TaktPermission("accounting:financial:tax:condition:update", "更新税务条件排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateTaxConditionSortAsync([FromBody] TaktTaxConditionSortDto dto)
    {
        try
        {
            var result = await _taxConditionService.UpdateTaxConditionSortAsync(dto);
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
    [TaktPermission("accounting:financial:tax:condition:import", "获取税务条件导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTaxConditionTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _taxConditionService.GetTaxConditionTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入税务条件
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:tax:condition:import", "导入税务条件")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTaxConditionAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _taxConditionService.ImportTaxConditionAsync(stream, sheetName);
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
    /// 导出税务条件
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:tax:condition:export", "导出税务条件")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTaxConditionAsync([FromQuery] TaktTaxConditionQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _taxConditionService.ExportTaxConditionAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
