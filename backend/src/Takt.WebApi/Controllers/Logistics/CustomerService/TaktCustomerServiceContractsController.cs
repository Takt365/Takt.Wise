// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.CustomerService
// 文件名称：TaktCustomerServiceContractsController.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：服务合同控制器
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
/// 服务合同控制器
/// 提供服务合同的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "服务合同")]
public class TaktCustomerServiceContractsController : TaktControllerBase
{
    private readonly ITaktCustomerServiceContractService _customerServiceContractService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceContractService">服务合同服务</param>
    public TaktCustomerServiceContractsController(ITaktCustomerServiceContractService customerServiceContractService)
    {
        _customerServiceContractService = customerServiceContractService;
    }

    /// <summary>
    /// 获取服务合同列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:service:customer:contract:list", "服务合同列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCustomerServiceContractListAsync([FromQuery] TaktCustomerServiceContractQueryDto queryDto)
    {
        try
        {
            var result = await _customerServiceContractService.GetCustomerServiceContractListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取服务合同
    /// </summary>
    /// <param name="id">服务合同ID</param>
    /// <returns>服务合同DTO</returns>
    [TaktPermission("logistics:service:customer:contract:query", "服务合同详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerServiceContractByIdAsync(long id)
    {
        try
        {
            var result = await _customerServiceContractService.GetCustomerServiceContractByIdAsync(id);
            if (result == null)
            {
                return NotFound("服务合同不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取服务合同选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:service:customer:contract:query", "服务合同选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCustomerServiceContractOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _customerServiceContractService.GetCustomerServiceContractOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建服务合同
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>服务合同DTO</returns>
    [TaktPermission("logistics:service:customer:contract:create", "创建服务合同")]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerServiceContractAsync([FromBody] TaktCustomerServiceContractCreateDto dto)
    {
        try
        {
            var result = await _customerServiceContractService.CreateCustomerServiceContractAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新服务合同
    /// </summary>
    /// <param name="id">服务合同ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>服务合同DTO</returns>
    [TaktPermission("logistics:service:customer:contract:update", "更新服务合同")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerServiceContractAsync(long id, [FromBody] TaktCustomerServiceContractUpdateDto dto)
    {
        try
        {
            var result = await _customerServiceContractService.UpdateCustomerServiceContractAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除服务合同
    /// </summary>
    /// <param name="id">服务合同ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:service:customer:contract:delete", "删除服务合同")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerServiceContractByIdAsync(long id)
    {
        try
        {
            await _customerServiceContractService.DeleteCustomerServiceContractByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除服务合同
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:service:customer:contract:delete", "批量删除服务合同")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCustomerServiceContractBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _customerServiceContractService.DeleteCustomerServiceContractBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新服务合同状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>服务合同DTO</returns>
    [TaktPermission("logistics:service:customer:contract:update", "更新服务合同状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCustomerServiceContractStatusAsync([FromBody] TaktCustomerServiceContractStatusDto dto)
    {
        try
        {
            var result = await _customerServiceContractService.UpdateCustomerServiceContractStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新服务合同排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>服务合同DTO</returns>
    [TaktPermission("logistics:service:customer:contract:update", "更新服务合同排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCustomerServiceContractSortAsync([FromBody] TaktCustomerServiceContractSortDto dto)
    {
        try
        {
            var result = await _customerServiceContractService.UpdateCustomerServiceContractSortAsync(dto);
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
    [TaktPermission("logistics:service:customer:contract:import", "获取服务合同导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCustomerServiceContractTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _customerServiceContractService.GetCustomerServiceContractTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入服务合同
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:service:customer:contract:import", "导入服务合同")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCustomerServiceContractAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _customerServiceContractService.ImportCustomerServiceContractAsync(stream, sheetName);
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
    /// 导出服务合同
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:service:customer:contract:export", "导出服务合同")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCustomerServiceContractAsync([FromQuery] TaktCustomerServiceContractQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _customerServiceContractService.ExportCustomerServiceContractAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
