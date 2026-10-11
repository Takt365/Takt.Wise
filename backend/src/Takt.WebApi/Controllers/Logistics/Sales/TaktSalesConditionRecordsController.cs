// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesConditionRecordsController.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：销售条件记录控制器
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
/// 销售条件记录控制器
/// 提供销售条件记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售条件记录")]
public class TaktSalesConditionRecordsController : TaktControllerBase
{
    private readonly ITaktSalesConditionRecordService _salesConditionRecordService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesConditionRecordService">销售条件记录服务</param>
    public TaktSalesConditionRecordsController(ITaktSalesConditionRecordService salesConditionRecordService)
    {
        _salesConditionRecordService = salesConditionRecordService;
    }

    /// <summary>
    /// 获取销售条件记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:condition:record:list", "销售条件记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesConditionRecordListAsync([FromQuery] TaktSalesConditionRecordQueryDto queryDto)
    {
        try
        {
            var result = await _salesConditionRecordService.GetSalesConditionRecordListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售条件记录
    /// </summary>
    /// <param name="id">销售条件记录ID</param>
    /// <returns>销售条件记录DTO</returns>
    [TaktPermission("logistics:sales:condition:record:query", "销售条件记录详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetSalesConditionRecordByIdAsync(long id)
    {
        try
        {
            var result = await _salesConditionRecordService.GetSalesConditionRecordByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售条件记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:condition:record:query", "销售条件记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesConditionRecordOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salesConditionRecordService.GetSalesConditionRecordOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售条件记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售条件记录DTO</returns>
    [TaktPermission("logistics:sales:condition:record:create", "创建销售条件记录")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesConditionRecordAsync([FromBody] TaktSalesConditionRecordCreateDto dto)
    {
        try
        {
            var result = await _salesConditionRecordService.CreateSalesConditionRecordAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售条件记录
    /// </summary>
    /// <param name="id">销售条件记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售条件记录DTO</returns>
    [TaktPermission("logistics:sales:condition:record:update", "更新销售条件记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesConditionRecordAsync(long id, [FromBody] TaktSalesConditionRecordUpdateDto dto)
    {
        try
        {
            var result = await _salesConditionRecordService.UpdateSalesConditionRecordAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售条件记录
    /// </summary>
    /// <param name="id">销售条件记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:condition:record:delete", "删除销售条件记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesConditionRecordByIdAsync(long id)
    {
        try
        {
            await _salesConditionRecordService.DeleteSalesConditionRecordByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售条件记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:condition:record:delete", "批量删除销售条件记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesConditionRecordBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesConditionRecordService.DeleteSalesConditionRecordBatchAsync(ids);
            return Success("删除成功");
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
    [TaktPermission("logistics:sales:condition:record:import", "获取销售条件记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesConditionRecordTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesConditionRecordService.GetSalesConditionRecordTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售条件记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:condition:record:import", "导入销售条件记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesConditionRecordAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesConditionRecordService.ImportSalesConditionRecordAsync(stream, sheetName);
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
    /// 导出销售条件记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:condition:record:export", "导出销售条件记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesConditionRecordAsync([FromQuery] TaktSalesConditionRecordQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesConditionRecordService.ExportSalesConditionRecordAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
