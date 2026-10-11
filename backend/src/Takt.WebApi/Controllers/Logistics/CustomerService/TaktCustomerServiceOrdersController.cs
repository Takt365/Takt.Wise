// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.CustomerService
// 文件名称：TaktCustomerServiceOrdersController.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：服务订单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.CustomerService;
using Takt.Application.Services.Logistics.CustomerService;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.CustomerService;

/// <summary>
/// 服务订单控制器
/// 提供服务订单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "服务订单")]
public class TaktCustomerServiceOrdersController : TaktControllerBase
{
    private readonly ITaktCustomerServiceOrderService _customerServiceOrderService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceOrderService">服务订单服务</param>
    public TaktCustomerServiceOrdersController(ITaktCustomerServiceOrderService customerServiceOrderService)
    {
        _customerServiceOrderService = customerServiceOrderService;
    }

    /// <summary>
    /// 获取服务订单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:service:customer:order:list", "服务订单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCustomerServiceOrderListAsync([FromQuery] TaktCustomerServiceOrderQueryDto queryDto)
    {
        try
        {
            var result = await _customerServiceOrderService.GetCustomerServiceOrderListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取服务订单
    /// </summary>
    /// <param name="id">服务订单ID</param>
    /// <returns>服务订单DTO</returns>
    [TaktPermission("logistics:service:customer:order:query", "服务订单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerServiceOrderByIdAsync(long id)
    {
        try
        {
            var result = await _customerServiceOrderService.GetCustomerServiceOrderByIdAsync(id);
            if (result == null)
            {
                return NotFound("服务订单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取服务订单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:service:customer:order:query", "服务订单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCustomerServiceOrderOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _customerServiceOrderService.GetCustomerServiceOrderOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建服务订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>服务订单DTO</returns>
    [TaktPermission("logistics:service:customer:order:create", "创建服务订单")]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerServiceOrderAsync([FromBody] TaktCustomerServiceOrderCreateDto dto)
    {
        try
        {
            var result = await _customerServiceOrderService.CreateCustomerServiceOrderAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新服务订单
    /// </summary>
    /// <param name="id">服务订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>服务订单DTO</returns>
    [TaktPermission("logistics:service:customer:order:update", "更新服务订单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerServiceOrderAsync(long id, [FromBody] TaktCustomerServiceOrderUpdateDto dto)
    {
        try
        {
            var result = await _customerServiceOrderService.UpdateCustomerServiceOrderAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除服务订单
    /// </summary>
    /// <param name="id">服务订单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:service:customer:order:delete", "删除服务订单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerServiceOrderByIdAsync(long id)
    {
        try
        {
            await _customerServiceOrderService.DeleteCustomerServiceOrderByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除服务订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:service:customer:order:delete", "批量删除服务订单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCustomerServiceOrderBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _customerServiceOrderService.DeleteCustomerServiceOrderBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新服务订单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>服务订单DTO</returns>
    [TaktPermission("logistics:service:customer:order:update", "更新服务订单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCustomerServiceOrderStatusAsync([FromBody] TaktCustomerServiceOrderStatusDto dto)
    {
        try
        {
            var result = await _customerServiceOrderService.UpdateCustomerServiceOrderStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新服务订单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>服务订单DTO</returns>
    [TaktPermission("logistics:service:customer:order:update", "更新服务订单排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCustomerServiceOrderSortAsync([FromBody] TaktCustomerServiceOrderSortDto dto)
    {
        try
        {
            var result = await _customerServiceOrderService.UpdateCustomerServiceOrderSortAsync(dto);
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
    [TaktPermission("logistics:service:customer:order:import", "获取服务订单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCustomerServiceOrderTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _customerServiceOrderService.GetCustomerServiceOrderTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入服务订单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:service:customer:order:import", "导入服务订单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCustomerServiceOrderAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _customerServiceOrderService.ImportCustomerServiceOrderAsync(stream, sheetName);
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
    /// 导出服务订单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:service:customer:order:export", "导出服务订单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCustomerServiceOrderAsync([FromQuery] TaktCustomerServiceOrderQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _customerServiceOrderService.ExportCustomerServiceOrderAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
