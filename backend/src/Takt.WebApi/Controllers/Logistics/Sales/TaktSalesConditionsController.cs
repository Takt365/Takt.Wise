// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesConditionsController.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：销售条件控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Application.Services.Logistics.Sales;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Sales;

/// <summary>
/// 销售条件控制器
/// 提供销售条件的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售条件")]
public class TaktSalesConditionsController : TaktControllerBase
{
    private readonly ITaktSalesConditionService _salesConditionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesConditionService">销售条件服务</param>
    public TaktSalesConditionsController(ITaktSalesConditionService salesConditionService)
    {
        _salesConditionService = salesConditionService;
    }

    /// <summary>
    /// 获取销售条件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:condition:list", "销售条件列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesConditionListAsync([FromQuery] TaktSalesConditionQueryDto queryDto)
    {
        try
        {
            var result = await _salesConditionService.GetSalesConditionListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售条件
    /// </summary>
    /// <param name="id">销售条件ID</param>
    /// <returns>销售条件DTO</returns>
    [TaktPermission("logistics:sales:condition:query", "销售条件详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetSalesConditionByIdAsync(long id)
    {
        try
        {
            var result = await _salesConditionService.GetSalesConditionByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售条件不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:condition:query", "销售条件选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesConditionOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salesConditionService.GetSalesConditionOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售条件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售条件DTO</returns>
    [TaktPermission("logistics:sales:condition:create", "创建销售条件")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesConditionAsync([FromBody] TaktSalesConditionCreateDto dto)
    {
        try
        {
            var result = await _salesConditionService.CreateSalesConditionAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售条件
    /// </summary>
    /// <param name="id">销售条件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售条件DTO</returns>
    [TaktPermission("logistics:sales:condition:update", "更新销售条件")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesConditionAsync(long id, [FromBody] TaktSalesConditionUpdateDto dto)
    {
        try
        {
            var result = await _salesConditionService.UpdateSalesConditionAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售条件
    /// </summary>
    /// <param name="id">销售条件ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:condition:delete", "删除销售条件")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesConditionByIdAsync(long id)
    {
        try
        {
            await _salesConditionService.DeleteSalesConditionByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售条件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:condition:delete", "批量删除销售条件")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesConditionBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesConditionService.DeleteSalesConditionBatchAsync(ids);
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
    [TaktPermission("logistics:sales:condition:import", "获取销售条件导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesConditionTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesConditionService.GetSalesConditionTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售条件
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:condition:import", "导入销售条件")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesConditionAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesConditionService.ImportSalesConditionAsync(stream, sheetName);
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
    /// 导出销售条件
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:condition:export", "导出销售条件")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesConditionAsync([FromQuery] TaktSalesConditionQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesConditionService.ExportSalesConditionAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
