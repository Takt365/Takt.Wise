// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktIpqcOrdersController.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：制程检验单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Application.Services.Logistics.Quality.Operation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Operation;

/// <summary>
/// 制程检验单控制器
/// 提供制程检验单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "制程检验单")]
public class TaktIpqcOrdersController : TaktControllerBase
{
    private readonly ITaktIpqcOrderService _ipqcOrderService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ipqcOrderService">制程检验单服务</param>
    public TaktIpqcOrdersController(ITaktIpqcOrderService ipqcOrderService)
    {
        _ipqcOrderService = ipqcOrderService;
    }

    /// <summary>
    /// 获取制程检验单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:list", "制程检验单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetIpqcOrderListAsync([FromQuery] TaktIpqcOrderQueryDto queryDto)
    {
        try
        {
            var result = await _ipqcOrderService.GetIpqcOrderListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 IPQC 检验统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>IPQC 检验统计</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:list", "IPQC 检验统计")]
    [HttpGet("inspection-stat")]
    public async Task<IActionResult> GetIpqcOrderStatAsync([FromQuery] TaktQualityStatQueryDto queryDto)
    {
        try
        {
            var result = await _ipqcOrderService.GetIpqcOrderStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取制程检验单
    /// </summary>
    /// <param name="id">制程检验单ID</param>
    /// <returns>制程检验单DTO</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:query", "制程检验单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetIpqcOrderByIdAsync(long id)
    {
        try
        {
            var result = await _ipqcOrderService.GetIpqcOrderByIdAsync(id);
            if (result == null)
            {
                return NotFound("制程检验单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取制程检验单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:query", "制程检验单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetIpqcOrderOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _ipqcOrderService.GetIpqcOrderOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建制程检验单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>制程检验单DTO</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:create", "创建制程检验单")]
    [HttpPost]
    public async Task<IActionResult> CreateIpqcOrderAsync([FromBody] TaktIpqcOrderCreateDto dto)
    {
        try
        {
            var result = await _ipqcOrderService.CreateIpqcOrderAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新制程检验单
    /// </summary>
    /// <param name="id">制程检验单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>制程检验单DTO</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:update", "更新制程检验单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIpqcOrderAsync(long id, [FromBody] TaktIpqcOrderUpdateDto dto)
    {
        try
        {
            var result = await _ipqcOrderService.UpdateIpqcOrderAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除制程检验单
    /// </summary>
    /// <param name="id">制程检验单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:delete", "删除制程检验单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIpqcOrderByIdAsync(long id)
    {
        try
        {
            await _ipqcOrderService.DeleteIpqcOrderByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除制程检验单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:delete", "批量删除制程检验单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteIpqcOrderBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ipqcOrderService.DeleteIpqcOrderBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新制程检验单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>制程检验单DTO</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:update", "更新制程检验单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateIpqcOrderStatusAsync([FromBody] TaktIpqcOrderStatusDto dto)
    {
        try
        {
            var result = await _ipqcOrderService.UpdateIpqcOrderStatusAsync(dto);
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
    [TaktPermission("logistics:quality:operation:ipqc:order:import", "获取制程检验单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetIpqcOrderTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ipqcOrderService.GetIpqcOrderTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入制程检验单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:import", "导入制程检验单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportIpqcOrderAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ipqcOrderService.ImportIpqcOrderAsync(stream, sheetName);
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
    /// 导出制程检验单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:ipqc:order:export", "导出制程检验单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportIpqcOrderAsync([FromQuery] TaktIpqcOrderQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ipqcOrderService.ExportIpqcOrderAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
