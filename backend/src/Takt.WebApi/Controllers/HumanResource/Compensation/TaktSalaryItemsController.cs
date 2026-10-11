// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Compensation
// 文件名称：TaktSalaryItemsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：薪资项目控制器
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
/// 薪资项目控制器
/// 提供薪资项目的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "薪资项目")]
public class TaktSalaryItemsController : TaktControllerBase
{
    private readonly ITaktSalaryItemService _salaryItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salaryItemService">薪资项目服务</param>
    public TaktSalaryItemsController(ITaktSalaryItemService salaryItemService)
    {
        _salaryItemService = salaryItemService;
    }

    /// <summary>
    /// 获取薪资项目列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:compensation:salary:item:list", "薪资项目列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalaryItemListAsync([FromQuery] TaktSalaryItemQueryDto queryDto)
    {
        try
        {
            var result = await _salaryItemService.GetSalaryItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取薪资项目
    /// </summary>
    /// <param name="id">薪资项目ID</param>
    /// <returns>薪资项目DTO</returns>
    [TaktPermission("human:resource:compensation:salary:item:query", "薪资项目详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalaryItemByIdAsync(long id)
    {
        try
        {
            var result = await _salaryItemService.GetSalaryItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("薪资项目不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取薪资项目选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:compensation:salary:item:query", "薪资项目选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalaryItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salaryItemService.GetSalaryItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建薪资项目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>薪资项目DTO</returns>
    [TaktPermission("human:resource:compensation:salary:item:create", "创建薪资项目")]
    [HttpPost]
    public async Task<IActionResult> CreateSalaryItemAsync([FromBody] TaktSalaryItemCreateDto dto)
    {
        try
        {
            var result = await _salaryItemService.CreateSalaryItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪资项目
    /// </summary>
    /// <param name="id">薪资项目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>薪资项目DTO</returns>
    [TaktPermission("human:resource:compensation:salary:item:update", "更新薪资项目")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalaryItemAsync(long id, [FromBody] TaktSalaryItemUpdateDto dto)
    {
        try
        {
            var result = await _salaryItemService.UpdateSalaryItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除薪资项目
    /// </summary>
    /// <param name="id">薪资项目ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:salary:item:delete", "删除薪资项目")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalaryItemByIdAsync(long id)
    {
        try
        {
            await _salaryItemService.DeleteSalaryItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除薪资项目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:compensation:salary:item:delete", "批量删除薪资项目")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalaryItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salaryItemService.DeleteSalaryItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪资项目状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>薪资项目DTO</returns>
    [TaktPermission("human:resource:compensation:salary:item:update", "更新薪资项目状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSalaryItemStatusAsync([FromBody] TaktSalaryItemStatusDto dto)
    {
        try
        {
            var result = await _salaryItemService.UpdateSalaryItemStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪资项目排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>薪资项目DTO</returns>
    [TaktPermission("human:resource:compensation:salary:item:update", "更新薪资项目排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSalaryItemSortAsync([FromBody] TaktSalaryItemSortDto dto)
    {
        try
        {
            var result = await _salaryItemService.UpdateSalaryItemSortAsync(dto);
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
    [TaktPermission("human:resource:compensation:salary:item:import", "获取薪资项目导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalaryItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salaryItemService.GetSalaryItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入薪资项目
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:compensation:salary:item:import", "导入薪资项目")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalaryItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salaryItemService.ImportSalaryItemAsync(stream, sheetName);
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
    /// 导出薪资项目
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:compensation:salary:item:export", "导出薪资项目")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalaryItemAsync([FromQuery] TaktSalaryItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salaryItemService.ExportSalaryItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
