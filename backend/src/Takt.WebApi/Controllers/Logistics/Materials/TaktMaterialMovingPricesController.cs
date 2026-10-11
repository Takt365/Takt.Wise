// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialMovingPricesController.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：移动价格控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 移动价格控制器
/// 提供移动价格的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "移动价格")]
public class TaktMaterialMovingPricesController : TaktControllerBase
{
    private readonly ITaktMaterialMovingPriceService _materialMovingPriceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialMovingPriceService">移动价格服务</param>
    public TaktMaterialMovingPricesController(ITaktMaterialMovingPriceService materialMovingPriceService)
    {
        _materialMovingPriceService = materialMovingPriceService;
    }

    /// <summary>
    /// 获取移动价格列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:material:moving:price:list", "移动价格列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaterialMovingPriceListAsync([FromQuery] TaktMaterialMovingPriceQueryDto queryDto)
    {
        try
        {
            var result = await _materialMovingPriceService.GetMaterialMovingPriceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取在库金额统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>在库金额统计</returns>
    [TaktPermission("logistics:materials:material:moving:price:list", "在库金额统计")]
    [HttpGet("stock-stat")]
    public async Task<IActionResult> GetMaterialMovingPriceStatAsync([FromQuery] TaktMaterialMovingPriceStatQueryDto queryDto)
    {
        try
        {
            var result = await _materialMovingPriceService.GetMaterialMovingPriceStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取移动价格
    /// </summary>
    /// <param name="id">移动价格ID</param>
    /// <returns>移动价格DTO</returns>
    [TaktPermission("logistics:materials:material:moving:price:query", "移动价格详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterialMovingPriceByIdAsync(long id)
    {
        try
        {
            var result = await _materialMovingPriceService.GetMaterialMovingPriceByIdAsync(id);
            if (result == null)
            {
                return NotFound("移动价格不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料移动价格选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:moving:price:query", "移动价格选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaterialMovingPriceOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _materialMovingPriceService.GetMaterialMovingPriceOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建移动价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>移动价格DTO</returns>
    [TaktPermission("logistics:materials:material:moving:price:create", "创建移动价格")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterialMovingPriceAsync([FromBody] TaktMaterialMovingPriceCreateDto dto)
    {
        try
        {
            var result = await _materialMovingPriceService.CreateMaterialMovingPriceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新移动价格
    /// </summary>
    /// <param name="id">移动价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>移动价格DTO</returns>
    [TaktPermission("logistics:materials:material:moving:price:update", "更新移动价格")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialMovingPriceAsync(long id, [FromBody] TaktMaterialMovingPriceUpdateDto dto)
    {
        try
        {
            var result = await _materialMovingPriceService.UpdateMaterialMovingPriceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除移动价格
    /// </summary>
    /// <param name="id">移动价格ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:moving:price:delete", "删除移动价格")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialMovingPriceByIdAsync(long id)
    {
        try
        {
            await _materialMovingPriceService.DeleteMaterialMovingPriceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除移动价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:moving:price:delete", "批量删除移动价格")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaterialMovingPriceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _materialMovingPriceService.DeleteMaterialMovingPriceBatchAsync(ids);
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
    [TaktPermission("logistics:materials:material:moving:price:import", "获取移动价格导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaterialMovingPriceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _materialMovingPriceService.GetMaterialMovingPriceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入移动价格
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:material:moving:price:import", "导入移动价格")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaterialMovingPriceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _materialMovingPriceService.ImportMaterialMovingPriceAsync(stream, sheetName);
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
    /// 导出移动价格
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:material:moving:price:export", "导出移动价格")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaterialMovingPriceAsync([FromQuery] TaktMaterialMovingPriceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialMovingPriceService.ExportMaterialMovingPriceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
