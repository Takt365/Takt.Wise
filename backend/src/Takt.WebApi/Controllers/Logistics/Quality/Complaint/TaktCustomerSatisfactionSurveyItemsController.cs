// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyItemsController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：客户满意度调查项目明细控制器
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
/// 客户满意度调查项目明细控制器
/// 提供客户满意度调查项目明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "客户满意度调查项目明细")]
public class TaktCustomerSatisfactionSurveyItemsController : TaktControllerBase
{
    private readonly ITaktCustomerSatisfactionSurveyItemService _customerSatisfactionSurveyItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerSatisfactionSurveyItemService">客户满意度调查项目明细服务</param>
    public TaktCustomerSatisfactionSurveyItemsController(ITaktCustomerSatisfactionSurveyItemService customerSatisfactionSurveyItemService)
    {
        _customerSatisfactionSurveyItemService = customerSatisfactionSurveyItemService;
    }

    /// <summary>
    /// 获取客户满意度调查项目明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:list", "客户满意度调查项目明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCustomerSatisfactionSurveyItemListAsync([FromQuery] TaktCustomerSatisfactionSurveyItemQueryDto queryDto)
    {
        try
        {
            var result = await _customerSatisfactionSurveyItemService.GetCustomerSatisfactionSurveyItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取客户满意度调查项目明细
    /// </summary>
    /// <param name="id">客户满意度调查项目明细ID</param>
    /// <returns>客户满意度调查项目明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:query", "客户满意度调查项目明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerSatisfactionSurveyItemByIdAsync(long id)
    {
        try
        {
            var result = await _customerSatisfactionSurveyItemService.GetCustomerSatisfactionSurveyItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("客户满意度调查项目明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取客户满意度调查项目明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:query", "客户满意度调查项目明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCustomerSatisfactionSurveyItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _customerSatisfactionSurveyItemService.GetCustomerSatisfactionSurveyItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建客户满意度调查项目明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>客户满意度调查项目明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:create", "创建客户满意度调查项目明细")]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerSatisfactionSurveyItemAsync([FromBody] TaktCustomerSatisfactionSurveyItemCreateDto dto)
    {
        try
        {
            var result = await _customerSatisfactionSurveyItemService.CreateCustomerSatisfactionSurveyItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户满意度调查项目明细
    /// </summary>
    /// <param name="id">客户满意度调查项目明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>客户满意度调查项目明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:update", "更新客户满意度调查项目明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerSatisfactionSurveyItemAsync(long id, [FromBody] TaktCustomerSatisfactionSurveyItemUpdateDto dto)
    {
        try
        {
            var result = await _customerSatisfactionSurveyItemService.UpdateCustomerSatisfactionSurveyItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除客户满意度调查项目明细
    /// </summary>
    /// <param name="id">客户满意度调查项目明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:delete", "删除客户满意度调查项目明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerSatisfactionSurveyItemByIdAsync(long id)
    {
        try
        {
            await _customerSatisfactionSurveyItemService.DeleteCustomerSatisfactionSurveyItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除客户满意度调查项目明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:delete", "批量删除客户满意度调查项目明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCustomerSatisfactionSurveyItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _customerSatisfactionSurveyItemService.DeleteCustomerSatisfactionSurveyItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户满意度调查项目明细状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>客户满意度调查项目明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:update", "更新客户满意度调查项目明细状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCustomerSatisfactionSurveyItemStatusAsync([FromBody] TaktCustomerSatisfactionSurveyItemStatusDto dto)
    {
        try
        {
            var result = await _customerSatisfactionSurveyItemService.UpdateCustomerSatisfactionSurveyItemStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户满意度调查项目明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>客户满意度调查项目明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:update", "更新客户满意度调查项目明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateCustomerSatisfactionSurveyItemObsoleteAsync([FromBody] TaktCustomerSatisfactionSurveyItemObsoleteDto dto)
    {
        try
        {
            var result = await _customerSatisfactionSurveyItemService.UpdateCustomerSatisfactionSurveyItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:import", "获取客户满意度调查项目明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCustomerSatisfactionSurveyItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _customerSatisfactionSurveyItemService.GetCustomerSatisfactionSurveyItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入客户满意度调查项目明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:import", "导入客户满意度调查项目明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCustomerSatisfactionSurveyItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _customerSatisfactionSurveyItemService.ImportCustomerSatisfactionSurveyItemAsync(stream, sheetName);
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
    /// 导出客户满意度调查项目明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:export", "导出客户满意度调查项目明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCustomerSatisfactionSurveyItemAsync([FromQuery] TaktCustomerSatisfactionSurveyItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _customerSatisfactionSurveyItemService.ExportCustomerSatisfactionSurveyItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
