// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mrp
// 文件名称：TaktProductionPlanItemsController.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：生产计划明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;
using Takt.Application.Services.Logistics.Manufacturing.Mrp;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Mrp;

/// <summary>
/// 生产计划明细控制器
/// 提供生产计划明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "生产计划明细")]
public class TaktProductionPlanItemsController : TaktControllerBase
{
    private readonly ITaktProductionPlanItemService _productionPlanItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionPlanItemService">生产计划明细服务</param>
    public TaktProductionPlanItemsController(ITaktProductionPlanItemService productionPlanItemService)
    {
        _productionPlanItemService = productionPlanItemService;
    }

    /// <summary>
    /// 获取生产计划明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:production:plan:list", "生产计划明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetProductionPlanItemListAsync([FromQuery] TaktProductionPlanItemQueryDto queryDto)
    {
        try
        {
            var result = await _productionPlanItemService.GetProductionPlanItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取生产计划明细
    /// </summary>
    /// <param name="id">生产计划明细ID</param>
    /// <returns>生产计划明细DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:production:plan:query", "生产计划明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductionPlanItemByIdAsync(long id)
    {
        try
        {
            var result = await _productionPlanItemService.GetProductionPlanItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("生产计划明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取生产计划明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mrp:production:plan:query", "生产计划明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetProductionPlanItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _productionPlanItemService.GetProductionPlanItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建生产计划明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>生产计划明细DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:production:plan:create", "创建生产计划明细")]
    [HttpPost]
    public async Task<IActionResult> CreateProductionPlanItemAsync([FromBody] TaktProductionPlanItemCreateDto dto)
    {
        try
        {
            var result = await _productionPlanItemService.CreateProductionPlanItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产计划明细
    /// </summary>
    /// <param name="id">生产计划明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>生产计划明细DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:production:plan:update", "更新生产计划明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductionPlanItemAsync(long id, [FromBody] TaktProductionPlanItemUpdateDto dto)
    {
        try
        {
            var result = await _productionPlanItemService.UpdateProductionPlanItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除生产计划明细
    /// </summary>
    /// <param name="id">生产计划明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:production:plan:delete", "删除生产计划明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductionPlanItemByIdAsync(long id)
    {
        try
        {
            await _productionPlanItemService.DeleteProductionPlanItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除生产计划明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:production:plan:delete", "批量删除生产计划明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteProductionPlanItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _productionPlanItemService.DeleteProductionPlanItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产计划明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>生产计划明细DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:production:plan:update", "更新生产计划明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateProductionPlanItemObsoleteAsync([FromBody] TaktProductionPlanItemObsoleteDto dto)
    {
        try
        {
            var result = await _productionPlanItemService.UpdateProductionPlanItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:mrp:production:plan:import", "获取生产计划明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetProductionPlanItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _productionPlanItemService.GetProductionPlanItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入生产计划明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:production:plan:import", "导入生产计划明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportProductionPlanItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _productionPlanItemService.ImportProductionPlanItemAsync(stream, sheetName);
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
    /// 导出生产计划明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mrp:production:plan:export", "导出生产计划明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportProductionPlanItemAsync([FromQuery] TaktProductionPlanItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _productionPlanItemService.ExportProductionPlanItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
