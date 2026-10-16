// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesConditionScaleValuesController.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：销售阶梯价格控制器
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
/// 销售阶梯价格控制器
/// 提供销售阶梯价格的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售阶梯价格")]
public class TaktSalesConditionScaleValuesController : TaktControllerBase
{
    private readonly ITaktSalesConditionScaleValueService _salesConditionScaleValueService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesConditionScaleValueService">销售阶梯价格服务</param>
    public TaktSalesConditionScaleValuesController(ITaktSalesConditionScaleValueService salesConditionScaleValueService)
    {
        _salesConditionScaleValueService = salesConditionScaleValueService;
    }

    /// <summary>
    /// 获取销售阶梯价格列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:condition:list", "销售阶梯价格列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesConditionScaleValueListAsync([FromQuery] TaktSalesConditionScaleValueQueryDto queryDto)
    {
        try
        {
            var result = await _salesConditionScaleValueService.GetSalesConditionScaleValueListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售阶梯价格
    /// </summary>
    /// <param name="id">销售阶梯价格ID</param>
    /// <returns>销售阶梯价格DTO</returns>
    [TaktPermission("logistics:sales:condition:query", "销售阶梯价格详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetSalesConditionScaleValueByIdAsync(long id)
    {
        try
        {
            var result = await _salesConditionScaleValueService.GetSalesConditionScaleValueByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售阶梯价格不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售阶梯价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:condition:query", "销售阶梯价格选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesConditionScaleValueOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salesConditionScaleValueService.GetSalesConditionScaleValueOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售阶梯价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售阶梯价格DTO</returns>
    [TaktPermission("logistics:sales:condition:create", "创建销售阶梯价格")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesConditionScaleValueAsync([FromBody] TaktSalesConditionScaleValueCreateDto dto)
    {
        try
        {
            var result = await _salesConditionScaleValueService.CreateSalesConditionScaleValueAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售阶梯价格
    /// </summary>
    /// <param name="id">销售阶梯价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售阶梯价格DTO</returns>
    [TaktPermission("logistics:sales:condition:update", "更新销售阶梯价格")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesConditionScaleValueAsync(long id, [FromBody] TaktSalesConditionScaleValueUpdateDto dto)
    {
        try
        {
            var result = await _salesConditionScaleValueService.UpdateSalesConditionScaleValueAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售阶梯价格
    /// </summary>
    /// <param name="id">销售阶梯价格ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:condition:delete", "删除销售阶梯价格")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesConditionScaleValueByIdAsync(long id)
    {
        try
        {
            await _salesConditionScaleValueService.DeleteSalesConditionScaleValueByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售阶梯价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:condition:delete", "批量删除销售阶梯价格")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesConditionScaleValueBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesConditionScaleValueService.DeleteSalesConditionScaleValueBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售阶梯价格作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>销售阶梯价格DTO</returns>
    [TaktPermission("logistics:sales:condition:update", "更新销售阶梯价格作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateSalesConditionScaleValueObsoleteAsync([FromBody] TaktSalesConditionScaleValueObsoleteDto dto)
    {
        try
        {
            var result = await _salesConditionScaleValueService.UpdateSalesConditionScaleValueObsoleteAsync(dto);
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
    [TaktPermission("logistics:sales:condition:import", "获取销售阶梯价格导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesConditionScaleValueTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesConditionScaleValueService.GetSalesConditionScaleValueTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售阶梯价格
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:condition:import", "导入销售阶梯价格")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesConditionScaleValueAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesConditionScaleValueService.ImportSalesConditionScaleValueAsync(stream, sheetName);
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
    /// 导出销售阶梯价格
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:condition:export", "导出销售阶梯价格")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesConditionScaleValueAsync([FromQuery] TaktSalesConditionScaleValueQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesConditionScaleValueService.ExportSalesConditionScaleValueAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
