// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemsController.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM物料成本明细控制器
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
/// BOM物料成本明细控制器
/// 提供BOM物料成本明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM物料成本明细")]
public class TaktBomMaterialCostItemsController : TaktControllerBase
{
    private readonly ITaktBomMaterialCostItemService _bomMaterialCostItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemService">BOM物料成本明细服务</param>
    public TaktBomMaterialCostItemsController(ITaktBomMaterialCostItemService bomMaterialCostItemService)
    {
        _bomMaterialCostItemService = bomMaterialCostItemService;
    }

    /// <summary>
    /// 获取BOM物料成本明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:item:list", "BOM物料成本明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBomMaterialCostItemListAsync([FromQuery] TaktBomMaterialCostItemQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <returns>BOM物料成本明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:item:query", "BOM物料成本明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBomMaterialCostItemByIdAsync(long id)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("BOM物料成本明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取BOM物料成本选项列表（按产品编码去重，可选按工厂过滤）
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:item:query", "BOM物料成本明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBomMaterialCostItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建BOM物料成本明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>BOM物料成本明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:item:create", "创建BOM物料成本明细")]
    [HttpPost]
    public async Task<IActionResult> CreateBomMaterialCostItemAsync([FromBody] TaktBomMaterialCostItemCreateDto dto)
    {
        try
        {
            var result = await _bomMaterialCostItemService.CreateBomMaterialCostItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>BOM物料成本明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:item:update", "更新BOM物料成本明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBomMaterialCostItemAsync(long id, [FromBody] TaktBomMaterialCostItemUpdateDto dto)
    {
        try
        {
            var result = await _bomMaterialCostItemService.UpdateBomMaterialCostItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:item:delete", "删除BOM物料成本明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBomMaterialCostItemByIdAsync(long id)
    {
        try
        {
            await _bomMaterialCostItemService.DeleteBomMaterialCostItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除BOM物料成本明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:item:delete", "批量删除BOM物料成本明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBomMaterialCostItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _bomMaterialCostItemService.DeleteBomMaterialCostItemBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:bom:material:cost:item:import", "获取BOM物料成本明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetBomMaterialCostItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _bomMaterialCostItemService.GetBomMaterialCostItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入BOM物料成本明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:item:import", "导入BOM物料成本明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBomMaterialCostItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _bomMaterialCostItemService.ImportBomMaterialCostItemAsync(stream, sheetName);
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
    /// 导出BOM物料成本明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:item:export", "导出BOM物料成本明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBomMaterialCostItemAsync([FromQuery] TaktBomMaterialCostItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostItemService.ExportBomMaterialCostItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
