// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktBudgetActualsController.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：预算实绩控制器
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
/// 预算实绩控制器
/// 提供预算实绩的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "预算实绩")]
public class TaktBudgetActualsController : TaktControllerBase
{
    private readonly ITaktBudgetActualService _budgetActualService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="budgetActualService">预算实绩服务</param>
    public TaktBudgetActualsController(ITaktBudgetActualService budgetActualService)
    {
        _budgetActualService = budgetActualService;
    }

    /// <summary>
    /// 获取预算实绩列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:budget:actual:list", "预算实绩列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBudgetActualListAsync([FromQuery] TaktBudgetActualQueryDto queryDto)
    {
        try
        {
            var result = await _budgetActualService.GetBudgetActualListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取预算实绩
    /// </summary>
    /// <param name="id">预算实绩ID</param>
    /// <returns>预算实绩DTO</returns>
    [TaktPermission("accounting:financial:budget:actual:query", "预算实绩详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBudgetActualByIdAsync(long id)
    {
        try
        {
            var result = await _budgetActualService.GetBudgetActualByIdAsync(id);
            if (result == null)
            {
                return NotFound("预算实绩不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取预算实绩选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:budget:actual:query", "预算实绩选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBudgetActualOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _budgetActualService.GetBudgetActualOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建预算实绩
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>预算实绩DTO</returns>
    [TaktPermission("accounting:financial:budget:actual:create", "创建预算实绩")]
    [HttpPost]
    public async Task<IActionResult> CreateBudgetActualAsync([FromBody] TaktBudgetActualCreateDto dto)
    {
        try
        {
            var result = await _budgetActualService.CreateBudgetActualAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新预算实绩
    /// </summary>
    /// <param name="id">预算实绩ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>预算实绩DTO</returns>
    [TaktPermission("accounting:financial:budget:actual:update", "更新预算实绩")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudgetActualAsync(long id, [FromBody] TaktBudgetActualUpdateDto dto)
    {
        try
        {
            var result = await _budgetActualService.UpdateBudgetActualAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除预算实绩
    /// </summary>
    /// <param name="id">预算实绩ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:budget:actual:delete", "删除预算实绩")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudgetActualByIdAsync(long id)
    {
        try
        {
            await _budgetActualService.DeleteBudgetActualByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除预算实绩
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:budget:actual:delete", "批量删除预算实绩")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBudgetActualBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _budgetActualService.DeleteBudgetActualBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新预算实绩状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>预算实绩DTO</returns>
    [TaktPermission("accounting:financial:budget:actual:update", "更新预算实绩状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateBudgetActualStatusAsync([FromBody] TaktBudgetActualStatusDto dto)
    {
        try
        {
            var result = await _budgetActualService.UpdateBudgetActualStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新预算实绩排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>预算实绩DTO</returns>
    [TaktPermission("accounting:financial:budget:actual:update", "更新预算实绩排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateBudgetActualSortAsync([FromBody] TaktBudgetActualSortDto dto)
    {
        try
        {
            var result = await _budgetActualService.UpdateBudgetActualSortAsync(dto);
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
    [TaktPermission("accounting:financial:budget:actual:import", "获取预算实绩导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetBudgetActualTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _budgetActualService.GetBudgetActualTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入预算实绩
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:budget:actual:import", "导入预算实绩")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBudgetActualAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _budgetActualService.ImportBudgetActualAsync(stream, sheetName);
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
    /// 导出预算实绩
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:budget:actual:export", "导出预算实绩")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBudgetActualAsync([FromQuery] TaktBudgetActualQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _budgetActualService.ExportBudgetActualAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
