// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderItemsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：进货检验单明细控制器
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
/// 进货检验单明细控制器
/// 提供进货检验单明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "进货检验单明细")]
public class TaktIqcOrderItemsController : TaktControllerBase
{
    private readonly ITaktIqcOrderItemService _iqcOrderItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="iqcOrderItemService">进货检验单明细服务</param>
    public TaktIqcOrderItemsController(ITaktIqcOrderItemService iqcOrderItemService)
    {
        _iqcOrderItemService = iqcOrderItemService;
    }

    /// <summary>
    /// 获取进货检验单明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:list", "进货检验单明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetIqcOrderItemListAsync([FromQuery] TaktIqcOrderItemQueryDto queryDto)
    {
        try
        {
            var result = await _iqcOrderItemService.GetIqcOrderItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取进货检验单明细
    /// </summary>
    /// <param name="id">进货检验单明细ID</param>
    /// <returns>进货检验单明细DTO</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:query", "进货检验单明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetIqcOrderItemByIdAsync(long id)
    {
        try
        {
            var result = await _iqcOrderItemService.GetIqcOrderItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("进货检验单明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取进货检验单明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:query", "进货检验单明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetIqcOrderItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _iqcOrderItemService.GetIqcOrderItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建进货检验单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>进货检验单明细DTO</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:create", "创建进货检验单明细")]
    [HttpPost]
    public async Task<IActionResult> CreateIqcOrderItemAsync([FromBody] TaktIqcOrderItemCreateDto dto)
    {
        try
        {
            var result = await _iqcOrderItemService.CreateIqcOrderItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新进货检验单明细
    /// </summary>
    /// <param name="id">进货检验单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>进货检验单明细DTO</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:update", "更新进货检验单明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIqcOrderItemAsync(long id, [FromBody] TaktIqcOrderItemUpdateDto dto)
    {
        try
        {
            var result = await _iqcOrderItemService.UpdateIqcOrderItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除进货检验单明细
    /// </summary>
    /// <param name="id">进货检验单明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:delete", "删除进货检验单明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIqcOrderItemByIdAsync(long id)
    {
        try
        {
            await _iqcOrderItemService.DeleteIqcOrderItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除进货检验单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:delete", "批量删除进货检验单明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteIqcOrderItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _iqcOrderItemService.DeleteIqcOrderItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新进货检验单明细状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>进货检验单明细DTO</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:update", "更新进货检验单明细状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateIqcOrderItemStatusAsync([FromBody] TaktIqcOrderItemStatusDto dto)
    {
        try
        {
            var result = await _iqcOrderItemService.UpdateIqcOrderItemStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新进货检验单明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>进货检验单明细DTO</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:update", "更新进货检验单明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateIqcOrderItemObsoleteAsync([FromBody] TaktIqcOrderItemObsoleteDto dto)
    {
        try
        {
            var result = await _iqcOrderItemService.UpdateIqcOrderItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:quality:operation:iqc:order:import", "获取进货检验单明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetIqcOrderItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _iqcOrderItemService.GetIqcOrderItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入进货检验单明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:import", "导入进货检验单明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportIqcOrderItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _iqcOrderItemService.ImportIqcOrderItemAsync(stream, sheetName);
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
    /// 导出进货检验单明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:iqc:order:export", "导出进货检验单明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportIqcOrderItemAsync([FromQuery] TaktIqcOrderItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _iqcOrderItemService.ExportIqcOrderItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
