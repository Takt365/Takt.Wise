// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Aps
// 文件名称：TaktProductionOrdersController.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：生产工单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Application.Services.Logistics.Manufacturing.Aps;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Aps;

/// <summary>
/// 生产工单控制器
/// 提供生产工单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "生产工单")]
public class TaktProductionOrdersController : TaktControllerBase
{
    private readonly ITaktProductionOrderService _productionOrderService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionOrderService">生产工单服务</param>
    public TaktProductionOrdersController(ITaktProductionOrderService productionOrderService)
    {
        _productionOrderService = productionOrderService;
    }

    /// <summary>
    /// 获取生产工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:aps:production:order:list", "生产工单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetProductionOrderListAsync([FromQuery] TaktProductionOrderQueryDto queryDto)
    {
        try
        {
            var result = await _productionOrderService.GetProductionOrderListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取生产工单
    /// </summary>
    /// <param name="id">生产工单ID</param>
    /// <returns>生产工单DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:production:order:query", "生产工单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductionOrderByIdAsync(long id)
    {
        try
        {
            var result = await _productionOrderService.GetProductionOrderByIdAsync(id);
            if (result == null)
            {
                return NotFound("生产工单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取生产工单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（工单号/物料号模糊匹配，可选）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:aps:production:order:query", "生产工单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetProductionOrderOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _productionOrderService.GetProductionOrderOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建生产工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>生产工单DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:production:order:create", "创建生产工单")]
    [HttpPost]
    public async Task<IActionResult> CreateProductionOrderAsync([FromBody] TaktProductionOrderCreateDto dto)
    {
        try
        {
            var result = await _productionOrderService.CreateProductionOrderAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产工单
    /// </summary>
    /// <param name="id">生产工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>生产工单DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:production:order:update", "更新生产工单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductionOrderAsync(long id, [FromBody] TaktProductionOrderUpdateDto dto)
    {
        try
        {
            var result = await _productionOrderService.UpdateProductionOrderAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除生产工单
    /// </summary>
    /// <param name="id">生产工单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:production:order:delete", "删除生产工单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductionOrderByIdAsync(long id)
    {
        try
        {
            await _productionOrderService.DeleteProductionOrderByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除生产工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:production:order:delete", "批量删除生产工单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteProductionOrderBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _productionOrderService.DeleteProductionOrderBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产工单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>生产工单DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:production:order:update", "更新生产工单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateProductionOrderStatusAsync([FromBody] TaktProductionOrderStatusDto dto)
    {
        try
        {
            var result = await _productionOrderService.UpdateProductionOrderStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:aps:production:order:import", "获取生产工单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetProductionOrderTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _productionOrderService.GetProductionOrderTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入生产工单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:aps:production:order:import", "导入生产工单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportProductionOrderAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _productionOrderService.ImportProductionOrderAsync(stream, sheetName);
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
    /// 导出生产工单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:aps:production:order:export", "导出生产工单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportProductionOrderAsync([FromQuery] TaktProductionOrderQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _productionOrderService.ExportProductionOrderAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
