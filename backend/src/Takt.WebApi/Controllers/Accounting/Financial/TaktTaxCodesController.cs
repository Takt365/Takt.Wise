// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktTaxCodesController.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Cursor AI)
// 功能描述：税收代码控制器
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
/// 税收代码控制器
/// 提供税收代码的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "税收代码")]
public class TaktTaxCodesController : TaktControllerBase
{
    private readonly ITaktTaxCodeService _taxCodeService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="taxCodeService">税收代码服务</param>
    public TaktTaxCodesController(ITaktTaxCodeService taxCodeService)
    {
        _taxCodeService = taxCodeService;
    }

    /// <summary>
    /// 获取税收代码列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:tax:code:list", "税收代码列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTaxCodeListAsync([FromQuery] TaktTaxCodeQueryDto queryDto)
    {
        try
        {
            var result = await _taxCodeService.GetTaxCodeListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取税收代码
    /// </summary>
    /// <param name="id">税收代码ID</param>
    /// <returns>税收代码DTO</returns>
    [TaktPermission("accounting:financial:tax:code:query", "税收代码详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetTaxCodeByIdAsync(long id)
    {
        try
        {
            var result = await _taxCodeService.GetTaxCodeByIdAsync(id);
            if (result == null)
            {
                return NotFound("税收代码不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取税收代码选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:tax:code:query", "税收代码选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTaxCodeOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _taxCodeService.GetTaxCodeOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建税收代码
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>税收代码DTO</returns>
    [TaktPermission("accounting:financial:tax:code:create", "创建税收代码")]
    [HttpPost]
    public async Task<IActionResult> CreateTaxCodeAsync([FromBody] TaktTaxCodeCreateDto dto)
    {
        try
        {
            var result = await _taxCodeService.CreateTaxCodeAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新税收代码
    /// </summary>
    /// <param name="id">税收代码ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>税收代码DTO</returns>
    [TaktPermission("accounting:financial:tax:code:update", "更新税收代码")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTaxCodeAsync(long id, [FromBody] TaktTaxCodeUpdateDto dto)
    {
        try
        {
            var result = await _taxCodeService.UpdateTaxCodeAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除税收代码
    /// </summary>
    /// <param name="id">税收代码ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:tax:code:delete", "删除税收代码")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaxCodeByIdAsync(long id)
    {
        try
        {
            await _taxCodeService.DeleteTaxCodeByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除税收代码
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:tax:code:delete", "批量删除税收代码")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTaxCodeBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _taxCodeService.DeleteTaxCodeBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新税收代码状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>税收代码DTO</returns>
    [TaktPermission("accounting:financial:tax:code:update", "更新税收代码状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTaxCodeStatusAsync([FromBody] TaktTaxCodeStatusDto dto)
    {
        try
        {
            var result = await _taxCodeService.UpdateTaxCodeStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新税收代码排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>税收代码DTO</returns>
    [TaktPermission("accounting:financial:tax:code:update", "更新税收代码排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateTaxCodeSortAsync([FromBody] TaktTaxCodeSortDto dto)
    {
        try
        {
            var result = await _taxCodeService.UpdateTaxCodeSortAsync(dto);
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
    [TaktPermission("accounting:financial:tax:code:import", "获取税收代码导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTaxCodeTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _taxCodeService.GetTaxCodeTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入税收代码
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:tax:code:import", "导入税收代码")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTaxCodeAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _taxCodeService.ImportTaxCodeAsync(stream, sheetName);
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
    /// 导出税收代码
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:tax:code:export", "导出税收代码")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTaxCodeAsync([FromQuery] TaktTaxCodeQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _taxCodeService.ExportTaxCodeAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
