// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mrp
// 文件名称：TaktMaterialRequirementsPlanningItemsController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料需求计划MRP明细控制器
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
/// 物料需求计划MRP明细控制器
/// 提供物料需求计划MRP明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料需求计划MRP明细")]
public class TaktMaterialRequirementsPlanningItemsController : TaktControllerBase
{
    private readonly ITaktMaterialRequirementsPlanningItemService _materialRequirementsPlanningItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialRequirementsPlanningItemService">物料需求计划MRP明细服务</param>
    public TaktMaterialRequirementsPlanningItemsController(ITaktMaterialRequirementsPlanningItemService materialRequirementsPlanningItemService)
    {
        _materialRequirementsPlanningItemService = materialRequirementsPlanningItemService;
    }

    /// <summary>
    /// 获取物料需求计划MRP明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:planning:list", "物料需求计划MRP明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaterialRequirementsPlanningItemListAsync([FromQuery] TaktMaterialRequirementsPlanningItemQueryDto queryDto)
    {
        try
        {
            var result = await _materialRequirementsPlanningItemService.GetMaterialRequirementsPlanningItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取物料需求计划MRP明细
    /// </summary>
    /// <param name="id">物料需求计划MRP明细ID</param>
    /// <returns>物料需求计划MRP明细DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:planning:query", "物料需求计划MRP明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterialRequirementsPlanningItemByIdAsync(long id)
    {
        try
        {
            var result = await _materialRequirementsPlanningItemService.GetMaterialRequirementsPlanningItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("物料需求计划MRP明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料需求计划MRP明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:planning:query", "物料需求计划MRP明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaterialRequirementsPlanningItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _materialRequirementsPlanningItemService.GetMaterialRequirementsPlanningItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建物料需求计划MRP明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>物料需求计划MRP明细DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:planning:create", "创建物料需求计划MRP明细")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterialRequirementsPlanningItemAsync([FromBody] TaktMaterialRequirementsPlanningItemCreateDto dto)
    {
        try
        {
            var result = await _materialRequirementsPlanningItemService.CreateMaterialRequirementsPlanningItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料需求计划MRP明细
    /// </summary>
    /// <param name="id">物料需求计划MRP明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>物料需求计划MRP明细DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:planning:update", "更新物料需求计划MRP明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialRequirementsPlanningItemAsync(long id, [FromBody] TaktMaterialRequirementsPlanningItemUpdateDto dto)
    {
        try
        {
            var result = await _materialRequirementsPlanningItemService.UpdateMaterialRequirementsPlanningItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除物料需求计划MRP明细
    /// </summary>
    /// <param name="id">物料需求计划MRP明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:planning:delete", "删除物料需求计划MRP明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialRequirementsPlanningItemByIdAsync(long id)
    {
        try
        {
            await _materialRequirementsPlanningItemService.DeleteMaterialRequirementsPlanningItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除物料需求计划MRP明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:planning:delete", "批量删除物料需求计划MRP明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaterialRequirementsPlanningItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _materialRequirementsPlanningItemService.DeleteMaterialRequirementsPlanningItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料需求计划MRP明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>物料需求计划MRP明细DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:planning:update", "更新物料需求计划MRP明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateMaterialRequirementsPlanningItemObsoleteAsync([FromBody] TaktMaterialRequirementsPlanningItemObsoleteDto dto)
    {
        try
        {
            var result = await _materialRequirementsPlanningItemService.UpdateMaterialRequirementsPlanningItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:planning:import", "获取物料需求计划MRP明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaterialRequirementsPlanningItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _materialRequirementsPlanningItemService.GetMaterialRequirementsPlanningItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入物料需求计划MRP明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:planning:import", "导入物料需求计划MRP明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaterialRequirementsPlanningItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _materialRequirementsPlanningItemService.ImportMaterialRequirementsPlanningItemAsync(stream, sheetName);
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
    /// 导出物料需求计划MRP明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mrp:material:requirements:planning:export", "导出物料需求计划MRP明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaterialRequirementsPlanningItemAsync([FromQuery] TaktMaterialRequirementsPlanningItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialRequirementsPlanningItemService.ExportMaterialRequirementsPlanningItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
