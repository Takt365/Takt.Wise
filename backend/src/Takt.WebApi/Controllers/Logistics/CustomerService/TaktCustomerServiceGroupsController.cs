// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.CustomerService
// 文件名称：TaktCustomerServiceGroupsController.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：客户服务组主数据控制器
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
/// 客户服务组主数据控制器
/// 提供客户服务组主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "客户服务组主数据")]
public class TaktCustomerServiceGroupsController : TaktControllerBase
{
    private readonly ITaktCustomerServiceGroupService _customerServiceGroupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceGroupService">客户服务组主数据服务</param>
    public TaktCustomerServiceGroupsController(ITaktCustomerServiceGroupService customerServiceGroupService)
    {
        _customerServiceGroupService = customerServiceGroupService;
    }

    /// <summary>
    /// 获取客户服务组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:customer:service:group:list", "客户服务组主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCustomerServiceGroupListAsync([FromQuery] TaktCustomerServiceGroupQueryDto queryDto)
    {
        try
        {
            var result = await _customerServiceGroupService.GetCustomerServiceGroupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取客户服务组主数据
    /// </summary>
    /// <param name="id">客户服务组主数据ID</param>
    /// <returns>客户服务组主数据DTO</returns>
    [TaktPermission("logistics:customer:service:group:query", "客户服务组主数据详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCustomerServiceGroupByIdAsync(long id)
    {
        try
        {
            var result = await _customerServiceGroupService.GetCustomerServiceGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound("客户服务组主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取客户服务组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:customer:service:group:query", "客户服务组主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCustomerServiceGroupOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _customerServiceGroupService.GetCustomerServiceGroupOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建客户服务组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>客户服务组主数据DTO</returns>
    [TaktPermission("logistics:customer:service:group:create", "创建客户服务组主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerServiceGroupAsync([FromBody] TaktCustomerServiceGroupCreateDto dto)
    {
        try
        {
            var result = await _customerServiceGroupService.CreateCustomerServiceGroupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户服务组主数据
    /// </summary>
    /// <param name="id">客户服务组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>客户服务组主数据DTO</returns>
    [TaktPermission("logistics:customer:service:group:update", "更新客户服务组主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerServiceGroupAsync(long id, [FromBody] TaktCustomerServiceGroupUpdateDto dto)
    {
        try
        {
            var result = await _customerServiceGroupService.UpdateCustomerServiceGroupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除客户服务组主数据
    /// </summary>
    /// <param name="id">客户服务组主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:customer:service:group:delete", "删除客户服务组主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerServiceGroupByIdAsync(long id)
    {
        try
        {
            await _customerServiceGroupService.DeleteCustomerServiceGroupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除客户服务组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:customer:service:group:delete", "批量删除客户服务组主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCustomerServiceGroupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _customerServiceGroupService.DeleteCustomerServiceGroupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户服务组主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>客户服务组主数据DTO</returns>
    [TaktPermission("logistics:customer:service:group:update", "更新客户服务组主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCustomerServiceGroupStatusAsync([FromBody] TaktCustomerServiceGroupStatusDto dto)
    {
        try
        {
            var result = await _customerServiceGroupService.UpdateCustomerServiceGroupStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户服务组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>客户服务组主数据DTO</returns>
    [TaktPermission("logistics:customer:service:group:update", "更新客户服务组主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCustomerServiceGroupSortAsync([FromBody] TaktCustomerServiceGroupSortDto dto)
    {
        try
        {
            var result = await _customerServiceGroupService.UpdateCustomerServiceGroupSortAsync(dto);
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
    [TaktPermission("logistics:customer:service:group:import", "获取客户服务组主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCustomerServiceGroupTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _customerServiceGroupService.GetCustomerServiceGroupTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入客户服务组主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:customer:service:group:import", "导入客户服务组主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCustomerServiceGroupAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _customerServiceGroupService.ImportCustomerServiceGroupAsync(stream, sheetName);
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
    /// 导出客户服务组主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:customer:service:group:export", "导出客户服务组主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCustomerServiceGroupAsync([FromQuery] TaktCustomerServiceGroupQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _customerServiceGroupService.ExportCustomerServiceGroupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
