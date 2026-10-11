// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.HumanResource.Benefits
// 文件名称：TaktBenefitItemsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：福利项目控制器
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
/// 福利项目控制器
/// 提供福利项目的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "福利项目")]
public class TaktBenefitItemsController : TaktControllerBase
{
    private readonly ITaktBenefitItemService _benefitItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="benefitItemService">福利项目服务</param>
    public TaktBenefitItemsController(ITaktBenefitItemService benefitItemService)
    {
        _benefitItemService = benefitItemService;
    }

    /// <summary>
    /// 获取福利项目列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("human:resource:benefits:benefit:item:list", "福利项目列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBenefitItemListAsync([FromQuery] TaktBenefitItemQueryDto queryDto)
    {
        try
        {
            var result = await _benefitItemService.GetBenefitItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取福利项目
    /// </summary>
    /// <param name="id">福利项目ID</param>
    /// <returns>福利项目DTO</returns>
    [TaktPermission("human:resource:benefits:benefit:item:query", "福利项目详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBenefitItemByIdAsync(long id)
    {
        try
        {
            var result = await _benefitItemService.GetBenefitItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("福利项目不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取福利项目选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("human:resource:benefits:benefit:item:query", "福利项目选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBenefitItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _benefitItemService.GetBenefitItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建福利项目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>福利项目DTO</returns>
    [TaktPermission("human:resource:benefits:benefit:item:create", "创建福利项目")]
    [HttpPost]
    public async Task<IActionResult> CreateBenefitItemAsync([FromBody] TaktBenefitItemCreateDto dto)
    {
        try
        {
            var result = await _benefitItemService.CreateBenefitItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新福利项目
    /// </summary>
    /// <param name="id">福利项目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>福利项目DTO</returns>
    [TaktPermission("human:resource:benefits:benefit:item:update", "更新福利项目")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBenefitItemAsync(long id, [FromBody] TaktBenefitItemUpdateDto dto)
    {
        try
        {
            var result = await _benefitItemService.UpdateBenefitItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除福利项目
    /// </summary>
    /// <param name="id">福利项目ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:benefits:benefit:item:delete", "删除福利项目")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBenefitItemByIdAsync(long id)
    {
        try
        {
            await _benefitItemService.DeleteBenefitItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除福利项目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("human:resource:benefits:benefit:item:delete", "批量删除福利项目")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBenefitItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _benefitItemService.DeleteBenefitItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新福利项目状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>福利项目DTO</returns>
    [TaktPermission("human:resource:benefits:benefit:item:update", "更新福利项目状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateBenefitItemStatusAsync([FromBody] TaktBenefitItemStatusDto dto)
    {
        try
        {
            var result = await _benefitItemService.UpdateBenefitItemStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新福利项目排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>福利项目DTO</returns>
    [TaktPermission("human:resource:benefits:benefit:item:update", "更新福利项目排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateBenefitItemSortAsync([FromBody] TaktBenefitItemSortDto dto)
    {
        try
        {
            var result = await _benefitItemService.UpdateBenefitItemSortAsync(dto);
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
    [TaktPermission("human:resource:benefits:benefit:item:import", "获取福利项目导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetBenefitItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _benefitItemService.GetBenefitItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入福利项目
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("human:resource:benefits:benefit:item:import", "导入福利项目")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBenefitItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _benefitItemService.ImportBenefitItemAsync(stream, sheetName);
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
    /// 导出福利项目
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("human:resource:benefits:benefit:item:export", "导出福利项目")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBenefitItemAsync([FromQuery] TaktBenefitItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _benefitItemService.ExportBenefitItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
