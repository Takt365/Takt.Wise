// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostsController.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：材料成本控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// 材料成本控制器
/// 提供材料成本的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "材料成本")]
public class TaktBomMaterialCostsController : TaktControllerBase
{
    private readonly ITaktBomMaterialCostService _bomMaterialCostService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostService">材料成本服务</param>
    public TaktBomMaterialCostsController(ITaktBomMaterialCostService bomMaterialCostService)
    {
        _bomMaterialCostService = bomMaterialCostService;
    }

    /// <summary>
    /// 获取材料成本列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:list", "材料成本列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBomMaterialCostListAsync([FromQuery] TaktBomMaterialCostQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostService.GetBomMaterialCostListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取材料成本
    /// </summary>
    /// <param name="id">材料成本ID</param>
    /// <returns>材料成本DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:query", "材料成本详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBomMaterialCostByIdAsync(long id)
    {
        try
        {
            var result = await _bomMaterialCostService.GetBomMaterialCostByIdAsync(id);
            if (result == null)
            {
                return NotFound("材料成本不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取材料成本选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:query", "材料成本选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBomMaterialCostOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _bomMaterialCostService.GetBomMaterialCostOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建材料成本
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>材料成本DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:create", "创建材料成本")]
    [HttpPost]
    public async Task<IActionResult> CreateBomMaterialCostAsync([FromBody] TaktBomMaterialCostCreateDto dto)
    {
        try
        {
            var result = await _bomMaterialCostService.CreateBomMaterialCostAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新材料成本
    /// </summary>
    /// <param name="id">材料成本ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>材料成本DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:update", "更新材料成本")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBomMaterialCostAsync(long id, [FromBody] TaktBomMaterialCostUpdateDto dto)
    {
        try
        {
            var result = await _bomMaterialCostService.UpdateBomMaterialCostAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除材料成本
    /// </summary>
    /// <param name="id">材料成本ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:delete", "删除材料成本")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBomMaterialCostByIdAsync(long id)
    {
        try
        {
            await _bomMaterialCostService.DeleteBomMaterialCostByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除材料成本
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:delete", "批量删除材料成本")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBomMaterialCostBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _bomMaterialCostService.DeleteBomMaterialCostBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:bom:material:cost:import", "获取材料成本导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetBomMaterialCostTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _bomMaterialCostService.GetBomMaterialCostTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入材料成本
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:import", "导入材料成本")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBomMaterialCostAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _bomMaterialCostService.ImportBomMaterialCostAsync(stream, sheetName);
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
    /// 导出材料成本
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:export", "导出材料成本")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBomMaterialCostAsync([FromQuery] TaktBomMaterialCostQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostService.ExportBomMaterialCostAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
