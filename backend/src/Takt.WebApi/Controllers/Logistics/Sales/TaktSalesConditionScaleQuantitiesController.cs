// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesConditionScaleQuantitiesController.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：销售阶梯数量控制器
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
/// 销售阶梯数量控制器
/// 提供销售阶梯数量的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售阶梯数量")]
public class TaktSalesConditionScaleQuantitiesController : TaktControllerBase
{
    private readonly ITaktSalesConditionScaleQuantityService _salesConditionScaleQuantityService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesConditionScaleQuantityService">销售阶梯数量服务</param>
    public TaktSalesConditionScaleQuantitiesController(ITaktSalesConditionScaleQuantityService salesConditionScaleQuantityService)
    {
        _salesConditionScaleQuantityService = salesConditionScaleQuantityService;
    }

    /// <summary>
    /// 获取销售阶梯数量列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:condition:list", "销售阶梯数量列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesConditionScaleQuantityListAsync([FromQuery] TaktSalesConditionScaleQuantityQueryDto queryDto)
    {
        try
        {
            var result = await _salesConditionScaleQuantityService.GetSalesConditionScaleQuantityListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售阶梯数量
    /// </summary>
    /// <param name="id">销售阶梯数量ID</param>
    /// <returns>销售阶梯数量DTO</returns>
    [TaktPermission("logistics:sales:condition:query", "销售阶梯数量详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetSalesConditionScaleQuantityByIdAsync(long id)
    {
        try
        {
            var result = await _salesConditionScaleQuantityService.GetSalesConditionScaleQuantityByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售阶梯数量不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售阶梯数量选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:condition:query", "销售阶梯数量选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesConditionScaleQuantityOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salesConditionScaleQuantityService.GetSalesConditionScaleQuantityOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售阶梯数量
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售阶梯数量DTO</returns>
    [TaktPermission("logistics:sales:condition:create", "创建销售阶梯数量")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesConditionScaleQuantityAsync([FromBody] TaktSalesConditionScaleQuantityCreateDto dto)
    {
        try
        {
            var result = await _salesConditionScaleQuantityService.CreateSalesConditionScaleQuantityAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售阶梯数量
    /// </summary>
    /// <param name="id">销售阶梯数量ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售阶梯数量DTO</returns>
    [TaktPermission("logistics:sales:condition:update", "更新销售阶梯数量")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesConditionScaleQuantityAsync(long id, [FromBody] TaktSalesConditionScaleQuantityUpdateDto dto)
    {
        try
        {
            var result = await _salesConditionScaleQuantityService.UpdateSalesConditionScaleQuantityAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售阶梯数量
    /// </summary>
    /// <param name="id">销售阶梯数量ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:condition:delete", "删除销售阶梯数量")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesConditionScaleQuantityByIdAsync(long id)
    {
        try
        {
            await _salesConditionScaleQuantityService.DeleteSalesConditionScaleQuantityByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售阶梯数量
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:condition:delete", "批量删除销售阶梯数量")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesConditionScaleQuantityBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesConditionScaleQuantityService.DeleteSalesConditionScaleQuantityBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售阶梯数量作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>销售阶梯数量DTO</returns>
    [TaktPermission("logistics:sales:condition:update", "更新销售阶梯数量作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateSalesConditionScaleQuantityObsoleteAsync([FromBody] TaktSalesConditionScaleQuantityObsoleteDto dto)
    {
        try
        {
            var result = await _salesConditionScaleQuantityService.UpdateSalesConditionScaleQuantityObsoleteAsync(dto);
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
    [TaktPermission("logistics:sales:condition:import", "获取销售阶梯数量导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesConditionScaleQuantityTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesConditionScaleQuantityService.GetSalesConditionScaleQuantityTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售阶梯数量
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:condition:import", "导入销售阶梯数量")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesConditionScaleQuantityAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesConditionScaleQuantityService.ImportSalesConditionScaleQuantityAsync(stream, sheetName);
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
    /// 导出销售阶梯数量
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:condition:export", "导出销售阶梯数量")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesConditionScaleQuantityAsync([FromQuery] TaktSalesConditionScaleQuantityQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesConditionScaleQuantityService.ExportSalesConditionScaleQuantityAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
