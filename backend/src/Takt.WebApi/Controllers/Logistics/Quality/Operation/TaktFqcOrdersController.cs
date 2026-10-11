// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktFqcOrdersController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：出货检验单控制器
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
/// 出货检验单控制器
/// 提供出货检验单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "出货检验单")]
public class TaktFqcOrdersController : TaktControllerBase
{
    private readonly ITaktFqcOrderService _fqcOrderService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fqcOrderService">出货检验单服务</param>
    public TaktFqcOrdersController(ITaktFqcOrderService fqcOrderService)
    {
        _fqcOrderService = fqcOrderService;
    }

    /// <summary>
    /// 获取出货检验单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:list", "出货检验单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFqcOrderListAsync([FromQuery] TaktFqcOrderQueryDto queryDto)
    {
        try
        {
            var result = await _fqcOrderService.GetFqcOrderListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 FQC 检验统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>FQC 检验统计</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:list", "FQC 检验统计")]
    [HttpGet("inspection-stat")]
    public async Task<IActionResult> GetFqcOrderStatAsync([FromQuery] TaktQualityStatQueryDto queryDto)
    {
        try
        {
            var result = await _fqcOrderService.GetFqcOrderStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取出货检验单
    /// </summary>
    /// <param name="id">出货检验单ID</param>
    /// <returns>出货检验单DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:query", "出货检验单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFqcOrderByIdAsync(long id)
    {
        try
        {
            var result = await _fqcOrderService.GetFqcOrderByIdAsync(id);
            if (result == null)
            {
                return NotFound("出货检验单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取出货检验单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:query", "出货检验单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFqcOrderOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _fqcOrderService.GetFqcOrderOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建出货检验单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>出货检验单DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:create", "创建出货检验单")]
    [HttpPost]
    public async Task<IActionResult> CreateFqcOrderAsync([FromBody] TaktFqcOrderCreateDto dto)
    {
        try
        {
            var result = await _fqcOrderService.CreateFqcOrderAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新出货检验单
    /// </summary>
    /// <param name="id">出货检验单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>出货检验单DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:update", "更新出货检验单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFqcOrderAsync(long id, [FromBody] TaktFqcOrderUpdateDto dto)
    {
        try
        {
            var result = await _fqcOrderService.UpdateFqcOrderAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除出货检验单
    /// </summary>
    /// <param name="id">出货检验单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:delete", "删除出货检验单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFqcOrderByIdAsync(long id)
    {
        try
        {
            await _fqcOrderService.DeleteFqcOrderByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除出货检验单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:delete", "批量删除出货检验单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFqcOrderBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _fqcOrderService.DeleteFqcOrderBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新出货检验单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>出货检验单DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:update", "更新出货检验单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateFqcOrderStatusAsync([FromBody] TaktFqcOrderStatusDto dto)
    {
        try
        {
            var result = await _fqcOrderService.UpdateFqcOrderStatusAsync(dto);
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
    [TaktPermission("logistics:quality:operation:fqc:order:import", "获取出货检验单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFqcOrderTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _fqcOrderService.GetFqcOrderTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入出货检验单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:import", "导入出货检验单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFqcOrderAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _fqcOrderService.ImportFqcOrderAsync(stream, sheetName);
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
    /// 导出出货检验单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:export", "导出出货检验单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFqcOrderAsync([FromQuery] TaktFqcOrderQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _fqcOrderService.ExportFqcOrderAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
