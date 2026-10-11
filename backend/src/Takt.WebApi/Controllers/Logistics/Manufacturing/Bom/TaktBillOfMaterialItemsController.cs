// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialItemsController.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：物料清单明细控制器
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
/// 物料清单明细控制器
/// 提供物料清单明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料清单明细")]
public class TaktBillOfMaterialItemsController : TaktControllerBase
{
    private readonly ITaktBillOfMaterialItemService _billOfMaterialItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialItemService">物料清单明细服务</param>
    public TaktBillOfMaterialItemsController(ITaktBillOfMaterialItemService billOfMaterialItemService)
    {
        _billOfMaterialItemService = billOfMaterialItemService;
    }

    /// <summary>
    /// 获取物料清单明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:list", "物料清单明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBillOfMaterialItemListAsync([FromQuery] TaktBillOfMaterialItemQueryDto queryDto)
    {
        try
        {
            var result = await _billOfMaterialItemService.GetBillOfMaterialItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取物料清单明细
    /// </summary>
    /// <param name="id">物料清单明细ID</param>
    /// <returns>物料清单明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:query", "物料清单明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBillOfMaterialItemByIdAsync(long id)
    {
        try
        {
            var result = await _billOfMaterialItemService.GetBillOfMaterialItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("物料清单明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料清单明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:query", "物料清单明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBillOfMaterialItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _billOfMaterialItemService.GetBillOfMaterialItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建物料清单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>物料清单明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:create", "创建物料清单明细")]
    [HttpPost]
    public async Task<IActionResult> CreateBillOfMaterialItemAsync([FromBody] TaktBillOfMaterialItemCreateDto dto)
    {
        try
        {
            var result = await _billOfMaterialItemService.CreateBillOfMaterialItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料清单明细
    /// </summary>
    /// <param name="id">物料清单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>物料清单明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:update", "更新物料清单明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBillOfMaterialItemAsync(long id, [FromBody] TaktBillOfMaterialItemUpdateDto dto)
    {
        try
        {
            var result = await _billOfMaterialItemService.UpdateBillOfMaterialItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除物料清单明细
    /// </summary>
    /// <param name="id">物料清单明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:delete", "删除物料清单明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBillOfMaterialItemByIdAsync(long id)
    {
        try
        {
            await _billOfMaterialItemService.DeleteBillOfMaterialItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除物料清单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:delete", "批量删除物料清单明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBillOfMaterialItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _billOfMaterialItemService.DeleteBillOfMaterialItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料清单明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>物料清单明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:update", "更新物料清单明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateBillOfMaterialItemObsoleteAsync([FromBody] TaktBillOfMaterialItemObsoleteDto dto)
    {
        try
        {
            var result = await _billOfMaterialItemService.UpdateBillOfMaterialItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:import", "获取物料清单明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetBillOfMaterialItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _billOfMaterialItemService.GetBillOfMaterialItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入物料清单明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:import", "导入物料清单明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBillOfMaterialItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _billOfMaterialItemService.ImportBillOfMaterialItemAsync(stream, sheetName);
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
    /// 导出物料清单明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:export", "导出物料清单明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBillOfMaterialItemAsync([FromQuery] TaktBillOfMaterialItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _billOfMaterialItemService.ExportBillOfMaterialItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
