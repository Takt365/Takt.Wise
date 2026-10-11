// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktFqcOrderItemsController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：出货检验单明细控制器
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
/// 出货检验单明细控制器
/// 提供出货检验单明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "出货检验单明细")]
public class TaktFqcOrderItemsController : TaktControllerBase
{
    private readonly ITaktFqcOrderItemService _fqcOrderItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fqcOrderItemService">出货检验单明细服务</param>
    public TaktFqcOrderItemsController(ITaktFqcOrderItemService fqcOrderItemService)
    {
        _fqcOrderItemService = fqcOrderItemService;
    }

    /// <summary>
    /// 获取出货检验单明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:list", "出货检验单明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFqcOrderItemListAsync([FromQuery] TaktFqcOrderItemQueryDto queryDto)
    {
        try
        {
            var result = await _fqcOrderItemService.GetFqcOrderItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取出货检验单明细
    /// </summary>
    /// <param name="id">出货检验单明细ID</param>
    /// <returns>出货检验单明细DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:query", "出货检验单明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFqcOrderItemByIdAsync(long id)
    {
        try
        {
            var result = await _fqcOrderItemService.GetFqcOrderItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("出货检验单明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取出货检验单明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:query", "出货检验单明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFqcOrderItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _fqcOrderItemService.GetFqcOrderItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建出货检验单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>出货检验单明细DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:create", "创建出货检验单明细")]
    [HttpPost]
    public async Task<IActionResult> CreateFqcOrderItemAsync([FromBody] TaktFqcOrderItemCreateDto dto)
    {
        try
        {
            var result = await _fqcOrderItemService.CreateFqcOrderItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新出货检验单明细
    /// </summary>
    /// <param name="id">出货检验单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>出货检验单明细DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:update", "更新出货检验单明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFqcOrderItemAsync(long id, [FromBody] TaktFqcOrderItemUpdateDto dto)
    {
        try
        {
            var result = await _fqcOrderItemService.UpdateFqcOrderItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除出货检验单明细
    /// </summary>
    /// <param name="id">出货检验单明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:delete", "删除出货检验单明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFqcOrderItemByIdAsync(long id)
    {
        try
        {
            await _fqcOrderItemService.DeleteFqcOrderItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除出货检验单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:delete", "批量删除出货检验单明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFqcOrderItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _fqcOrderItemService.DeleteFqcOrderItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新出货检验单明细状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>出货检验单明细DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:update", "更新出货检验单明细状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateFqcOrderItemStatusAsync([FromBody] TaktFqcOrderItemStatusDto dto)
    {
        try
        {
            var result = await _fqcOrderItemService.UpdateFqcOrderItemStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新出货检验单明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>出货检验单明细DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:update", "更新出货检验单明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateFqcOrderItemObsoleteAsync([FromBody] TaktFqcOrderItemObsoleteDto dto)
    {
        try
        {
            var result = await _fqcOrderItemService.UpdateFqcOrderItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:quality:operation:fqc:order:import", "获取出货检验单明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetFqcOrderItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _fqcOrderItemService.GetFqcOrderItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入出货检验单明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:import", "导入出货检验单明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportFqcOrderItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _fqcOrderItemService.ImportFqcOrderItemAsync(stream, sheetName);
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
    /// 导出出货检验单明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:export", "导出出货检验单明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFqcOrderItemAsync([FromQuery] TaktFqcOrderItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _fqcOrderItemService.ExportFqcOrderItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
