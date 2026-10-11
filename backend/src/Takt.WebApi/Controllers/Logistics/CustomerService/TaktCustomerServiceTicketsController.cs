// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.CustomerService
// 文件名称：TaktCustomerServiceTicketsController.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：服务工单控制器
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
/// 服务工单控制器
/// 提供服务工单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "服务工单")]
public class TaktCustomerServiceTicketsController : TaktControllerBase
{
    private readonly ITaktCustomerServiceTicketService _customerServiceTicketService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceTicketService">服务工单服务</param>
    public TaktCustomerServiceTicketsController(ITaktCustomerServiceTicketService customerServiceTicketService)
    {
        _customerServiceTicketService = customerServiceTicketService;
    }

    /// <summary>
    /// 获取服务工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:service:customer:ticket:list", "服务工单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCustomerServiceTicketListAsync([FromQuery] TaktCustomerServiceTicketQueryDto queryDto)
    {
        try
        {
            var result = await _customerServiceTicketService.GetCustomerServiceTicketListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取服务工单
    /// </summary>
    /// <param name="id">服务工单ID</param>
    /// <returns>服务工单DTO</returns>
    [TaktPermission("logistics:service:customer:ticket:query", "服务工单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerServiceTicketByIdAsync(long id)
    {
        try
        {
            var result = await _customerServiceTicketService.GetCustomerServiceTicketByIdAsync(id);
            if (result == null)
            {
                return NotFound("服务工单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取服务工单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:service:customer:ticket:query", "服务工单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCustomerServiceTicketOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _customerServiceTicketService.GetCustomerServiceTicketOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建服务工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>服务工单DTO</returns>
    [TaktPermission("logistics:service:customer:ticket:create", "创建服务工单")]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerServiceTicketAsync([FromBody] TaktCustomerServiceTicketCreateDto dto)
    {
        try
        {
            var result = await _customerServiceTicketService.CreateCustomerServiceTicketAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新服务工单
    /// </summary>
    /// <param name="id">服务工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>服务工单DTO</returns>
    [TaktPermission("logistics:service:customer:ticket:update", "更新服务工单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerServiceTicketAsync(long id, [FromBody] TaktCustomerServiceTicketUpdateDto dto)
    {
        try
        {
            var result = await _customerServiceTicketService.UpdateCustomerServiceTicketAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除服务工单
    /// </summary>
    /// <param name="id">服务工单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:service:customer:ticket:delete", "删除服务工单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerServiceTicketByIdAsync(long id)
    {
        try
        {
            await _customerServiceTicketService.DeleteCustomerServiceTicketByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除服务工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:service:customer:ticket:delete", "批量删除服务工单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCustomerServiceTicketBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _customerServiceTicketService.DeleteCustomerServiceTicketBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新服务工单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>服务工单DTO</returns>
    [TaktPermission("logistics:service:customer:ticket:update", "更新服务工单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCustomerServiceTicketStatusAsync([FromBody] TaktCustomerServiceTicketStatusDto dto)
    {
        try
        {
            var result = await _customerServiceTicketService.UpdateCustomerServiceTicketStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新服务工单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>服务工单DTO</returns>
    [TaktPermission("logistics:service:customer:ticket:update", "更新服务工单排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCustomerServiceTicketSortAsync([FromBody] TaktCustomerServiceTicketSortDto dto)
    {
        try
        {
            var result = await _customerServiceTicketService.UpdateCustomerServiceTicketSortAsync(dto);
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
    [TaktPermission("logistics:service:customer:ticket:import", "获取服务工单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCustomerServiceTicketTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _customerServiceTicketService.GetCustomerServiceTicketTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入服务工单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:service:customer:ticket:import", "导入服务工单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCustomerServiceTicketAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _customerServiceTicketService.ImportCustomerServiceTicketAsync(stream, sheetName);
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
    /// 导出服务工单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:service:customer:ticket:export", "导出服务工单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCustomerServiceTicketAsync([FromQuery] TaktCustomerServiceTicketQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _customerServiceTicketService.ExportCustomerServiceTicketAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
