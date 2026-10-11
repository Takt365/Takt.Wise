// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveysController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：客户满意度调查控制器
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
/// 客户满意度调查控制器
/// 提供客户满意度调查的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "客户满意度调查")]
public class TaktCustomerSatisfactionSurveysController : TaktControllerBase
{
    private readonly ITaktCustomerSatisfactionSurveyService _customerSatisfactionSurveyService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerSatisfactionSurveyService">客户满意度调查服务</param>
    public TaktCustomerSatisfactionSurveysController(ITaktCustomerSatisfactionSurveyService customerSatisfactionSurveyService)
    {
        _customerSatisfactionSurveyService = customerSatisfactionSurveyService;
    }

    /// <summary>
    /// 获取客户满意度调查列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:list", "客户满意度调查列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCustomerSatisfactionSurveyListAsync([FromQuery] TaktCustomerSatisfactionSurveyQueryDto queryDto)
    {
        try
        {
            var result = await _customerSatisfactionSurveyService.GetCustomerSatisfactionSurveyListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取客户满意度调查
    /// </summary>
    /// <param name="id">客户满意度调查ID</param>
    /// <returns>客户满意度调查DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:query", "客户满意度调查详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerSatisfactionSurveyByIdAsync(long id)
    {
        try
        {
            var result = await _customerSatisfactionSurveyService.GetCustomerSatisfactionSurveyByIdAsync(id);
            if (result == null)
            {
                return NotFound("客户满意度调查不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取客户满意度调查选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:query", "客户满意度调查选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCustomerSatisfactionSurveyOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _customerSatisfactionSurveyService.GetCustomerSatisfactionSurveyOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建客户满意度调查
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>客户满意度调查DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:create", "创建客户满意度调查")]
    [HttpPost]
    public async Task<IActionResult> CreateCustomerSatisfactionSurveyAsync([FromBody] TaktCustomerSatisfactionSurveyCreateDto dto)
    {
        try
        {
            var result = await _customerSatisfactionSurveyService.CreateCustomerSatisfactionSurveyAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户满意度调查
    /// </summary>
    /// <param name="id">客户满意度调查ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>客户满意度调查DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:update", "更新客户满意度调查")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomerSatisfactionSurveyAsync(long id, [FromBody] TaktCustomerSatisfactionSurveyUpdateDto dto)
    {
        try
        {
            var result = await _customerSatisfactionSurveyService.UpdateCustomerSatisfactionSurveyAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除客户满意度调查
    /// </summary>
    /// <param name="id">客户满意度调查ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:delete", "删除客户满意度调查")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerSatisfactionSurveyByIdAsync(long id)
    {
        try
        {
            await _customerSatisfactionSurveyService.DeleteCustomerSatisfactionSurveyByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除客户满意度调查
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:delete", "批量删除客户满意度调查")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCustomerSatisfactionSurveyBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _customerSatisfactionSurveyService.DeleteCustomerSatisfactionSurveyBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户满意度调查状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>客户满意度调查DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:update", "更新客户满意度调查状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCustomerSatisfactionSurveyStatusAsync([FromBody] TaktCustomerSatisfactionSurveyStatusDto dto)
    {
        try
        {
            var result = await _customerSatisfactionSurveyService.UpdateCustomerSatisfactionSurveyStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新客户满意度调查排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>客户满意度调查DTO</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:update", "更新客户满意度调查排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateCustomerSatisfactionSurveySortAsync([FromBody] TaktCustomerSatisfactionSurveySortDto dto)
    {
        try
        {
            var result = await _customerSatisfactionSurveyService.UpdateCustomerSatisfactionSurveySortAsync(dto);
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
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:import", "获取客户满意度调查导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCustomerSatisfactionSurveyTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _customerSatisfactionSurveyService.GetCustomerSatisfactionSurveyTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入客户满意度调查
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:import", "导入客户满意度调查")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCustomerSatisfactionSurveyAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _customerSatisfactionSurveyService.ImportCustomerSatisfactionSurveyAsync(stream, sheetName);
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
    /// 导出客户满意度调查
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:complaint:customer:satisfaction:survey:export", "导出客户满意度调查")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCustomerSatisfactionSurveyAsync([FromQuery] TaktCustomerSatisfactionSurveyQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _customerSatisfactionSurveyService.ExportCustomerSatisfactionSurveyAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
