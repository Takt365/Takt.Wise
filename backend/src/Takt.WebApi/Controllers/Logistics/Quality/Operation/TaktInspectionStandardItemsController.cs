// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktInspectionStandardItemsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：检验标准明细控制器
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
/// 检验标准明细控制器
/// 提供检验标准明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "检验标准明细")]
public class TaktInspectionStandardItemsController : TaktControllerBase
{
    private readonly ITaktInspectionStandardItemService _inspectionStandardItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="inspectionStandardItemService">检验标准明细服务</param>
    public TaktInspectionStandardItemsController(ITaktInspectionStandardItemService inspectionStandardItemService)
    {
        _inspectionStandardItemService = inspectionStandardItemService;
    }

    /// <summary>
    /// 获取检验标准明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:list", "检验标准明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetInspectionStandardItemListAsync([FromQuery] TaktInspectionStandardItemQueryDto queryDto)
    {
        try
        {
            var result = await _inspectionStandardItemService.GetInspectionStandardItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取检验标准明细
    /// </summary>
    /// <param name="id">检验标准明细ID</param>
    /// <returns>检验标准明细DTO</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:query", "检验标准明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInspectionStandardItemByIdAsync(long id)
    {
        try
        {
            var result = await _inspectionStandardItemService.GetInspectionStandardItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("检验标准明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取检验标准明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:query", "检验标准明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetInspectionStandardItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _inspectionStandardItemService.GetInspectionStandardItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建检验标准明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>检验标准明细DTO</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:create", "创建检验标准明细")]
    [HttpPost]
    public async Task<IActionResult> CreateInspectionStandardItemAsync([FromBody] TaktInspectionStandardItemCreateDto dto)
    {
        try
        {
            var result = await _inspectionStandardItemService.CreateInspectionStandardItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新检验标准明细
    /// </summary>
    /// <param name="id">检验标准明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>检验标准明细DTO</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:update", "更新检验标准明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInspectionStandardItemAsync(long id, [FromBody] TaktInspectionStandardItemUpdateDto dto)
    {
        try
        {
            var result = await _inspectionStandardItemService.UpdateInspectionStandardItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除检验标准明细
    /// </summary>
    /// <param name="id">检验标准明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:delete", "删除检验标准明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInspectionStandardItemByIdAsync(long id)
    {
        try
        {
            await _inspectionStandardItemService.DeleteInspectionStandardItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除检验标准明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:delete", "批量删除检验标准明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteInspectionStandardItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _inspectionStandardItemService.DeleteInspectionStandardItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新检验标准明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>检验标准明细DTO</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:update", "更新检验标准明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateInspectionStandardItemObsoleteAsync([FromBody] TaktInspectionStandardItemObsoleteDto dto)
    {
        try
        {
            var result = await _inspectionStandardItemService.UpdateInspectionStandardItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:quality:operation:inspection:standard:import", "获取检验标准明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetInspectionStandardItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _inspectionStandardItemService.GetInspectionStandardItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入检验标准明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:import", "导入检验标准明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportInspectionStandardItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _inspectionStandardItemService.ImportInspectionStandardItemAsync(stream, sheetName);
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
    /// 导出检验标准明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:export", "导出检验标准明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportInspectionStandardItemAsync([FromQuery] TaktInspectionStandardItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _inspectionStandardItemService.ExportInspectionStandardItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
