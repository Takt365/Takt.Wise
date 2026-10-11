// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.CustomerService
// 文件名称：TaktCustomerServiceRequestsController.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：服务请求控制器
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
/// 服务请求控制器
/// 提供服务请求的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "服务请求")]
public class TaktCustomerServiceRequestsController : TaktControllerBase
{
    private readonly ITaktCustomerServiceRequestService _customerServiceRequestService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceRequestService">服务请求服务</param>
    public TaktCustomerServiceRequestsController(ITaktCustomerServiceRequestService customerServiceRequestService)
    {
        _customerServiceRequestService = customerServiceRequestService;
    }

    /// <summary>
    /// 获取服务请求列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:service:customer:request:list", "服务请求列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCustomerServiceRequestListAsync([FromQuery] TaktCustomerServiceRequestQueryDto queryDto)
    {
        try
        {
            var result = await _customerServiceRequestService.GetCustomerServiceRequestListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取服务请求
    /// </summary>
    /// <param name="id">服务请求ID</param>
    /// <returns>服务请求DTO</returns>
    [TaktPermission("logistics:service:customer:request:query", "服务请求详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerServiceRequestByIdAsync(long id)
    {
        try
        {
            var result = await _customerServiceRequestService.GetCustomerServiceRequestByIdAsync(id);
            if (result == null)
            {
                return NotFound("服务请求不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取服务请求选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:service:customer:request:query", "服务请求选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCustomerServiceRequestOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _customerServiceRequestService.GetCustomerServiceRequestOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建服务请求
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>服务请求DTO</returns>
    [TaktPermission("logistics:service:customer:request:create", "创建服务请求")]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerServiceRequestAsync([FromBody] TaktCustomerServiceRequestCreateDto dto)
    {
        try
        {
            var result = await _customerServiceRequestService.CreateCustomerServiceRequestAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新服务请求
    /// </summary>
    /// <param name="id">服务请求ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>服务请求DTO</returns>
    [TaktPermission("logistics:service:customer:request:update", "更新服务请求")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerServiceRequestAsync(long id, [FromBody] TaktCustomerServiceRequestUpdateDto dto)
    {
        try
        {
            var result = await _customerServiceRequestService.UpdateCustomerServiceRequestAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除服务请求
    /// </summary>
    /// <param name="id">服务请求ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:service:customer:request:delete", "删除服务请求")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerServiceRequestByIdAsync(long id)
    {
        try
        {
            await _customerServiceRequestService.DeleteCustomerServiceRequestByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除服务请求
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:service:customer:request:delete", "批量删除服务请求")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCustomerServiceRequestBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _customerServiceRequestService.DeleteCustomerServiceRequestBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新服务请求状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>服务请求DTO</returns>
    [TaktPermission("logistics:service:customer:request:update", "更新服务请求状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCustomerServiceRequestStatusAsync([FromBody] TaktCustomerServiceRequestStatusDto dto)
    {
        try
        {
            var result = await _customerServiceRequestService.UpdateCustomerServiceRequestStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新服务请求排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>服务请求DTO</returns>
    [TaktPermission("logistics:service:customer:request:update", "更新服务请求排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCustomerServiceRequestSortAsync([FromBody] TaktCustomerServiceRequestSortDto dto)
    {
        try
        {
            var result = await _customerServiceRequestService.UpdateCustomerServiceRequestSortAsync(dto);
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
    [TaktPermission("logistics:service:customer:request:import", "获取服务请求导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCustomerServiceRequestTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _customerServiceRequestService.GetCustomerServiceRequestTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入服务请求
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:service:customer:request:import", "导入服务请求")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCustomerServiceRequestAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _customerServiceRequestService.ImportCustomerServiceRequestAsync(stream, sheetName);
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
    /// 导出服务请求
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:service:customer:request:export", "导出服务请求")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCustomerServiceRequestAsync([FromQuery] TaktCustomerServiceRequestQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _customerServiceRequestService.ExportCustomerServiceRequestAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
