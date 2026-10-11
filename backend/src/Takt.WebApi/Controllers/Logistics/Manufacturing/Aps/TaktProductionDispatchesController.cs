// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Aps
// 文件名称：TaktProductionDispatchesController.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：生产派工单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Application.Services.Logistics.Manufacturing.Aps;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Aps;

/// <summary>
/// 生产派工单控制器
/// 提供生产派工单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "生产派工单")]
public class TaktProductionDispatchesController : TaktControllerBase
{
    private readonly ITaktProductionDispatchService _productionDispatchService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionDispatchService">生产派工单服务</param>
    public TaktProductionDispatchesController(ITaktProductionDispatchService productionDispatchService)
    {
        _productionDispatchService = productionDispatchService;
    }

    /// <summary>
    /// 获取生产派工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:aps:production:dispatch:list", "生产派工单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetProductionDispatchListAsync([FromQuery] TaktProductionDispatchQueryDto queryDto)
    {
        try
        {
            var result = await _productionDispatchService.GetProductionDispatchListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取生产派工单
    /// </summary>
    /// <param name="id">生产派工单ID</param>
    /// <returns>生产派工单DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:production:dispatch:query", "生产派工单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductionDispatchByIdAsync(long id)
    {
        try
        {
            var result = await _productionDispatchService.GetProductionDispatchByIdAsync(id);
            if (result == null)
            {
                return NotFound("生产派工单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取生产派工单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:aps:production:dispatch:query", "生产派工单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetProductionDispatchOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _productionDispatchService.GetProductionDispatchOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建生产派工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>生产派工单DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:production:dispatch:create", "创建生产派工单")]
    [HttpPost]
    public async Task<IActionResult> CreateProductionDispatchAsync([FromBody] TaktProductionDispatchCreateDto dto)
    {
        try
        {
            var result = await _productionDispatchService.CreateProductionDispatchAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产派工单
    /// </summary>
    /// <param name="id">生产派工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>生产派工单DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:production:dispatch:update", "更新生产派工单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductionDispatchAsync(long id, [FromBody] TaktProductionDispatchUpdateDto dto)
    {
        try
        {
            var result = await _productionDispatchService.UpdateProductionDispatchAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除生产派工单
    /// </summary>
    /// <param name="id">生产派工单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:production:dispatch:delete", "删除生产派工单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductionDispatchByIdAsync(long id)
    {
        try
        {
            await _productionDispatchService.DeleteProductionDispatchByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除生产派工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:production:dispatch:delete", "批量删除生产派工单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteProductionDispatchBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _productionDispatchService.DeleteProductionDispatchBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产派工单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>生产派工单DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:production:dispatch:update", "更新生产派工单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateProductionDispatchStatusAsync([FromBody] TaktProductionDispatchStatusDto dto)
    {
        try
        {
            var result = await _productionDispatchService.UpdateProductionDispatchStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:aps:production:dispatch:import", "获取生产派工单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetProductionDispatchTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _productionDispatchService.GetProductionDispatchTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入生产派工单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:aps:production:dispatch:import", "导入生产派工单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportProductionDispatchAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _productionDispatchService.ImportProductionDispatchAsync(stream, sheetName);
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
    /// 导出生产派工单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:aps:production:dispatch:export", "导出生产派工单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportProductionDispatchAsync([FromQuery] TaktProductionDispatchQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _productionDispatchService.ExportProductionDispatchAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
