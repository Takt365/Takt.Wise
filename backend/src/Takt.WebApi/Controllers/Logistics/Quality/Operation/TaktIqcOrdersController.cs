// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktIqcOrdersController.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：进货检验单控制器
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
/// 进货检验单控制器
/// 提供进货检验单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "进货检验单")]
public class TaktIqcOrdersController : TaktControllerBase
{
    private readonly ITaktIqcOrderService _iqcOrderService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="iqcOrderService">进货检验单服务</param>
    public TaktIqcOrdersController(ITaktIqcOrderService iqcOrderService)
    {
        _iqcOrderService = iqcOrderService;
    }

    /// <summary>
    /// 获取进货检验单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:list", "进货检验单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetIqcOrderListAsync([FromQuery] TaktIqcOrderQueryDto queryDto)
    {
        try
        {
            var result = await _iqcOrderService.GetIqcOrderListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 IQC 检验统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>IQC 检验统计</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:list", "IQC 检验统计")]
    [HttpGet("inspection-stat")]
    public async Task<IActionResult> GetIqcOrderStatAsync([FromQuery] TaktQualityStatQueryDto queryDto)
    {
        try
        {
            var result = await _iqcOrderService.GetIqcOrderStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取进货检验单
    /// </summary>
    /// <param name="id">进货检验单ID</param>
    /// <returns>进货检验单DTO</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:query", "进货检验单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetIqcOrderByIdAsync(long id)
    {
        try
        {
            var result = await _iqcOrderService.GetIqcOrderByIdAsync(id);
            if (result == null)
            {
                return NotFound("进货检验单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取进货检验单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:query", "进货检验单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetIqcOrderOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _iqcOrderService.GetIqcOrderOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建进货检验单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>进货检验单DTO</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:create", "创建进货检验单")]
    [HttpPost]
    public async Task<IActionResult> CreateIqcOrderAsync([FromBody] TaktIqcOrderCreateDto dto)
    {
        try
        {
            var result = await _iqcOrderService.CreateIqcOrderAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新进货检验单
    /// </summary>
    /// <param name="id">进货检验单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>进货检验单DTO</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:update", "更新进货检验单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIqcOrderAsync(long id, [FromBody] TaktIqcOrderUpdateDto dto)
    {
        try
        {
            var result = await _iqcOrderService.UpdateIqcOrderAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除进货检验单
    /// </summary>
    /// <param name="id">进货检验单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:delete", "删除进货检验单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIqcOrderByIdAsync(long id)
    {
        try
        {
            await _iqcOrderService.DeleteIqcOrderByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除进货检验单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:delete", "批量删除进货检验单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteIqcOrderBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _iqcOrderService.DeleteIqcOrderBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新进货检验单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>进货检验单DTO</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:update", "更新进货检验单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateIqcOrderStatusAsync([FromBody] TaktIqcOrderStatusDto dto)
    {
        try
        {
            var result = await _iqcOrderService.UpdateIqcOrderStatusAsync(dto);
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
    [TaktPermission("logistics:quality:operation:iqc:order:import", "获取进货检验单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetIqcOrderTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _iqcOrderService.GetIqcOrderTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入进货检验单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:import", "导入进货检验单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportIqcOrderAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _iqcOrderService.ImportIqcOrderAsync(stream, sheetName);
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
    /// 导出进货检验单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:export", "导出进货检验单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportIqcOrderAsync([FromQuery] TaktIqcOrderQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _iqcOrderService.ExportIqcOrderAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
