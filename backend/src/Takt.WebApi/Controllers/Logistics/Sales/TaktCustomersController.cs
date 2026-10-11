// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktCustomersController.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：客户信息控制器
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
/// 客户信息控制器
/// 提供客户信息的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "客户信息")]
public class TaktCustomersController : TaktControllerBase
{
    private readonly ITaktCustomerService _customerService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerService">客户信息服务</param>
    public TaktCustomersController(ITaktCustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// 获取客户信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:customer:list", "客户信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCustomerListAsync([FromQuery] TaktCustomerQueryDto queryDto)
    {
        try
        {
            var result = await _customerService.GetCustomerListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取客户信息
    /// </summary>
    /// <param name="id">客户信息ID</param>
    /// <returns>客户信息DTO</returns>
    [TaktPermission("logistics:sales:customer:query", "客户信息详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerByIdAsync(long id)
    {
        try
        {
            var result = await _customerService.GetCustomerByIdAsync(id);
            if (result == null)
            {
                return NotFound("客户信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取客户信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:customer:query", "客户信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCustomerOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _customerService.GetCustomerOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建客户信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>客户信息DTO</returns>
    [TaktPermission("logistics:sales:customer:create", "创建客户信息")]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] TaktCustomerCreateDto dto)
    {
        try
        {
            var result = await _customerService.CreateCustomerAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户信息
    /// </summary>
    /// <param name="id">客户信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>客户信息DTO</returns>
    [TaktPermission("logistics:sales:customer:update", "更新客户信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerAsync(long id, [FromBody] TaktCustomerUpdateDto dto)
    {
        try
        {
            var result = await _customerService.UpdateCustomerAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除客户信息
    /// </summary>
    /// <param name="id">客户信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:customer:delete", "删除客户信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerByIdAsync(long id)
    {
        try
        {
            await _customerService.DeleteCustomerByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除客户信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:customer:delete", "批量删除客户信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCustomerBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _customerService.DeleteCustomerBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户信息状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>客户信息DTO</returns>
    [TaktPermission("logistics:sales:customer:update", "更新客户信息状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCustomerStatusAsync([FromBody] TaktCustomerStatusDto dto)
    {
        try
        {
            var result = await _customerService.UpdateCustomerStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>客户信息DTO</returns>
    [TaktPermission("logistics:sales:customer:update", "更新客户信息排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCustomerSortAsync([FromBody] TaktCustomerSortDto dto)
    {
        try
        {
            var result = await _customerService.UpdateCustomerSortAsync(dto);
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
    [TaktPermission("logistics:sales:customer:import", "获取客户信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCustomerTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _customerService.GetCustomerTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入客户信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:customer:import", "导入客户信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCustomerAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _customerService.ImportCustomerAsync(stream, sheetName);
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
    /// 导出客户信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:customer:export", "导出客户信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCustomerAsync([FromQuery] TaktCustomerQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _customerService.ExportCustomerAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
