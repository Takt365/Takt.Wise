// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesConditionRecordScaleQuantitiesController.cs
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
public class TaktSalesConditionRecordScaleQuantitiesController : TaktControllerBase
{
    private readonly ITaktSalesConditionRecordScaleQuantityService _salesConditionRecordScaleQuantityService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesConditionRecordScaleQuantityService">销售阶梯数量服务</param>
    public TaktSalesConditionRecordScaleQuantitiesController(ITaktSalesConditionRecordScaleQuantityService salesConditionRecordScaleQuantityService)
    {
        _salesConditionRecordScaleQuantityService = salesConditionRecordScaleQuantityService;
    }

    /// <summary>
    /// 获取销售阶梯数量列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:condition:record:list", "销售阶梯数量列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesConditionRecordScaleQuantityListAsync([FromQuery] TaktSalesConditionRecordScaleQuantityQueryDto queryDto)
    {
        try
        {
            var result = await _salesConditionRecordScaleQuantityService.GetSalesConditionRecordScaleQuantityListAsync(queryDto);
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
    [TaktPermission("logistics:sales:condition:record:query", "销售阶梯数量详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetSalesConditionRecordScaleQuantityByIdAsync(long id)
    {
        try
        {
            var result = await _salesConditionRecordScaleQuantityService.GetSalesConditionRecordScaleQuantityByIdAsync(id);
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
    [TaktPermission("logistics:sales:condition:record:query", "销售阶梯数量选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesConditionRecordScaleQuantityOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salesConditionRecordScaleQuantityService.GetSalesConditionRecordScaleQuantityOptionsAsync(plantCode, keyword);
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
    [TaktPermission("logistics:sales:condition:record:create", "创建销售阶梯数量")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesConditionRecordScaleQuantityAsync([FromBody] TaktSalesConditionRecordScaleQuantityCreateDto dto)
    {
        try
        {
            var result = await _salesConditionRecordScaleQuantityService.CreateSalesConditionRecordScaleQuantityAsync(dto);
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
    [TaktPermission("logistics:sales:condition:record:update", "更新销售阶梯数量")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesConditionRecordScaleQuantityAsync(long id, [FromBody] TaktSalesConditionRecordScaleQuantityUpdateDto dto)
    {
        try
        {
            var result = await _salesConditionRecordScaleQuantityService.UpdateSalesConditionRecordScaleQuantityAsync(id, dto);
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
    [TaktPermission("logistics:sales:condition:record:delete", "删除销售阶梯数量")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesConditionRecordScaleQuantityByIdAsync(long id)
    {
        try
        {
            await _salesConditionRecordScaleQuantityService.DeleteSalesConditionRecordScaleQuantityByIdAsync(id);
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
    [TaktPermission("logistics:sales:condition:record:delete", "批量删除销售阶梯数量")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesConditionRecordScaleQuantityBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesConditionRecordScaleQuantityService.DeleteSalesConditionRecordScaleQuantityBatchAsync(ids);
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
    [TaktPermission("logistics:sales:condition:record:update", "更新销售阶梯数量作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateSalesConditionRecordScaleQuantityObsoleteAsync([FromBody] TaktSalesConditionRecordScaleQuantityObsoleteDto dto)
    {
        try
        {
            var result = await _salesConditionRecordScaleQuantityService.UpdateSalesConditionRecordScaleQuantityObsoleteAsync(dto);
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
    [TaktPermission("logistics:sales:condition:record:import", "获取销售阶梯数量导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesConditionRecordScaleQuantityTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesConditionRecordScaleQuantityService.GetSalesConditionRecordScaleQuantityTemplateAsync(sheetName, templateName);
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
    [TaktPermission("logistics:sales:condition:record:import", "导入销售阶梯数量")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesConditionRecordScaleQuantityAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesConditionRecordScaleQuantityService.ImportSalesConditionRecordScaleQuantityAsync(stream, sheetName);
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
    [TaktPermission("logistics:sales:condition:record:export", "导出销售阶梯数量")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesConditionRecordScaleQuantityAsync([FromQuery] TaktSalesConditionRecordScaleQuantityQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesConditionRecordScaleQuantityService.ExportSalesConditionRecordScaleQuantityAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
