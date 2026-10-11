// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintItemsController.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Application.Services.Logistics.Quality.Complaint;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Complaint;

/// <summary>
/// 客诉明细控制器
/// 提供客诉明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "客诉明细")]
public class TaktCustomerComplaintItemsController : TaktControllerBase
{
    private readonly ITaktCustomerComplaintItemService _customerComplaintItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerComplaintItemService">客诉明细服务</param>
    public TaktCustomerComplaintItemsController(ITaktCustomerComplaintItemService customerComplaintItemService)
    {
        _customerComplaintItemService = customerComplaintItemService;
    }

    /// <summary>
    /// 获取客诉明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:list", "客诉明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCustomerComplaintItemListAsync([FromQuery] TaktCustomerComplaintItemQueryDto queryDto)
    {
        try
        {
            var result = await _customerComplaintItemService.GetCustomerComplaintItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取客诉明细
    /// </summary>
    /// <param name="id">客诉明细ID</param>
    /// <returns>客诉明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:query", "客诉明细详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCustomerComplaintItemByIdAsync(long id)
    {
        try
        {
            var result = await _customerComplaintItemService.GetCustomerComplaintItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("客诉明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取客诉明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:complaint:customer:query", "客诉明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCustomerComplaintItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _customerComplaintItemService.GetCustomerComplaintItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建客诉明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>客诉明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:create", "创建客诉明细")]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerComplaintItemAsync([FromBody] TaktCustomerComplaintItemCreateDto dto)
    {
        try
        {
            var result = await _customerComplaintItemService.CreateCustomerComplaintItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客诉明细
    /// </summary>
    /// <param name="id">客诉明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>客诉明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:update", "更新客诉明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerComplaintItemAsync(long id, [FromBody] TaktCustomerComplaintItemUpdateDto dto)
    {
        try
        {
            var result = await _customerComplaintItemService.UpdateCustomerComplaintItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除客诉明细
    /// </summary>
    /// <param name="id">客诉明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:delete", "删除客诉明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerComplaintItemByIdAsync(long id)
    {
        try
        {
            await _customerComplaintItemService.DeleteCustomerComplaintItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除客诉明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:delete", "批量删除客诉明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCustomerComplaintItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _customerComplaintItemService.DeleteCustomerComplaintItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客诉明细状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>客诉明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:update", "更新客诉明细状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCustomerComplaintItemStatusAsync([FromBody] TaktCustomerComplaintItemStatusDto dto)
    {
        try
        {
            var result = await _customerComplaintItemService.UpdateCustomerComplaintItemStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客诉明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>客诉明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:update", "更新客诉明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateCustomerComplaintItemObsoleteAsync([FromBody] TaktCustomerComplaintItemObsoleteDto dto)
    {
        try
        {
            var result = await _customerComplaintItemService.UpdateCustomerComplaintItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:quality:complaint:customer:import", "获取客诉明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCustomerComplaintItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _customerComplaintItemService.GetCustomerComplaintItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入客诉明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:import", "导入客诉明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCustomerComplaintItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _customerComplaintItemService.ImportCustomerComplaintItemAsync(stream, sheetName);
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
    /// 导出客诉明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:complaint:customer:export", "导出客诉明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCustomerComplaintItemAsync([FromQuery] TaktCustomerComplaintItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _customerComplaintItemService.ExportCustomerComplaintItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
