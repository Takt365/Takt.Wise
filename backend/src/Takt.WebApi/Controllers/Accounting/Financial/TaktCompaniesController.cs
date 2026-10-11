// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktCompaniesController.cs
// 创建时间：2026-08-15
// 创建人：Takt365(Cursor AI)
// 功能描述：公司控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Application.Services.Accounting.Financial;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Accounting.Financial;

/// <summary>
/// 公司控制器
/// 提供公司的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "公司")]
public class TaktCompaniesController : TaktControllerBase
{
    private readonly ITaktCompanyService _companyService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="companyService">公司服务</param>
    public TaktCompaniesController(ITaktCompanyService companyService)
    {
        _companyService = companyService;
    }

    /// <summary>
    /// 获取公司列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:company:list", "公司列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCompanyListAsync([FromQuery] TaktCompanyQueryDto queryDto)
    {
        try
        {
            var result = await _companyService.GetCompanyListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取公司
    /// </summary>
    /// <param name="id">公司ID</param>
    /// <returns>公司DTO</returns>
    [TaktPermission("accounting:financial:company:query", "公司详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCompanyByIdAsync(long id)
    {
        try
        {
            var result = await _companyService.GetCompanyByIdAsync(id);
            if (result == null)
            {
                return NotFound("公司不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取公司选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [AllowAnonymous]
    [HttpGet("options")]
    public async Task<IActionResult> GetCompanyOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _companyService.GetCompanyOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建公司
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>公司DTO</returns>
    [TaktPermission("accounting:financial:company:create", "创建公司")]
    [HttpPost]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] TaktCompanyCreateDto dto)
    {
        try
        {
            var result = await _companyService.CreateCompanyAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新公司
    /// </summary>
    /// <param name="id">公司ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>公司DTO</returns>
    [TaktPermission("accounting:financial:company:update", "更新公司")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCompanyAsync(long id, [FromBody] TaktCompanyUpdateDto dto)
    {
        try
        {
            var result = await _companyService.UpdateCompanyAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除公司
    /// </summary>
    /// <param name="id">公司ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:company:delete", "删除公司")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompanyByIdAsync(long id)
    {
        try
        {
            await _companyService.DeleteCompanyByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除公司
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:company:delete", "批量删除公司")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCompanyBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _companyService.DeleteCompanyBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新公司状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>公司DTO</returns>
    [TaktPermission("accounting:financial:company:update", "更新公司状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCompanyStatusAsync([FromBody] TaktCompanyStatusDto dto)
    {
        try
        {
            var result = await _companyService.UpdateCompanyStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新公司排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>公司DTO</returns>
    [TaktPermission("accounting:financial:company:update", "更新公司排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCompanySortAsync([FromBody] TaktCompanySortDto dto)
    {
        try
        {
            var result = await _companyService.UpdateCompanySortAsync(dto);
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
    [TaktPermission("accounting:financial:company:import", "获取公司导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCompanyTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _companyService.GetCompanyTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入公司
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:company:import", "导入公司")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCompanyAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _companyService.ImportCompanyAsync(stream, sheetName);
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
    /// 导出公司
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:company:export", "导出公司")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCompanyAsync([FromQuery] TaktCompanyQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _companyService.ExportCompanyAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
