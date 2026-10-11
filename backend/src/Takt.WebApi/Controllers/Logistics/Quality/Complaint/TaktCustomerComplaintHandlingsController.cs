// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintHandlingsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉处理记录控制器
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
/// 客诉处理记录控制器
/// 提供客诉处理记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "客诉处理记录")]
public class TaktCustomerComplaintHandlingsController : TaktControllerBase
{
    private readonly ITaktCustomerComplaintHandlingService _customerComplaintHandlingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerComplaintHandlingService">客诉处理记录服务</param>
    public TaktCustomerComplaintHandlingsController(ITaktCustomerComplaintHandlingService customerComplaintHandlingService)
    {
        _customerComplaintHandlingService = customerComplaintHandlingService;
    }

    /// <summary>
    /// 获取客诉处理记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:handling:list", "客诉处理记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCustomerComplaintHandlingListAsync([FromQuery] TaktCustomerComplaintHandlingQueryDto queryDto)
    {
        try
        {
            var result = await _customerComplaintHandlingService.GetCustomerComplaintHandlingListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取客诉处理记录
    /// </summary>
    /// <param name="id">客诉处理记录ID</param>
    /// <returns>客诉处理记录DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:handling:query", "客诉处理记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerComplaintHandlingByIdAsync(long id)
    {
        try
        {
            var result = await _customerComplaintHandlingService.GetCustomerComplaintHandlingByIdAsync(id);
            if (result == null)
            {
                return NotFound("客诉处理记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取客诉处理记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:complaint:customer:handling:query", "客诉处理记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCustomerComplaintHandlingOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _customerComplaintHandlingService.GetCustomerComplaintHandlingOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建客诉处理记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>客诉处理记录DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:handling:create", "创建客诉处理记录")]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerComplaintHandlingAsync([FromBody] TaktCustomerComplaintHandlingCreateDto dto)
    {
        try
        {
            var result = await _customerComplaintHandlingService.CreateCustomerComplaintHandlingAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客诉处理记录
    /// </summary>
    /// <param name="id">客诉处理记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>客诉处理记录DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:handling:update", "更新客诉处理记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerComplaintHandlingAsync(long id, [FromBody] TaktCustomerComplaintHandlingUpdateDto dto)
    {
        try
        {
            var result = await _customerComplaintHandlingService.UpdateCustomerComplaintHandlingAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除客诉处理记录
    /// </summary>
    /// <param name="id">客诉处理记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:handling:delete", "删除客诉处理记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerComplaintHandlingByIdAsync(long id)
    {
        try
        {
            await _customerComplaintHandlingService.DeleteCustomerComplaintHandlingByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除客诉处理记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:handling:delete", "批量删除客诉处理记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCustomerComplaintHandlingBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _customerComplaintHandlingService.DeleteCustomerComplaintHandlingBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客诉处理记录状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>客诉处理记录DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:handling:update", "更新客诉处理记录状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCustomerComplaintHandlingStatusAsync([FromBody] TaktCustomerComplaintHandlingStatusDto dto)
    {
        try
        {
            var result = await _customerComplaintHandlingService.UpdateCustomerComplaintHandlingStatusAsync(dto);
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
    [TaktPermission("logistics:quality:complaint:customer:handling:import", "获取客诉处理记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCustomerComplaintHandlingTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _customerComplaintHandlingService.GetCustomerComplaintHandlingTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入客诉处理记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:handling:import", "导入客诉处理记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCustomerComplaintHandlingAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _customerComplaintHandlingService.ImportCustomerComplaintHandlingAsync(stream, sheetName);
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
    /// 导出客诉处理记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:complaint:customer:handling:export", "导出客诉处理记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCustomerComplaintHandlingAsync([FromQuery] TaktCustomerComplaintHandlingQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _customerComplaintHandlingService.ExportCustomerComplaintHandlingAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
