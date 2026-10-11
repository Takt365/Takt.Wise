// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Aps
// 文件名称：TaktApsScheduleItemsController.cs
// 创建时间：2026-07-24
// 创建人：Takt365(Cursor AI)
// 功能描述：APS排程明细控制器
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
/// APS排程明细控制器
/// 提供APS排程明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "APS排程明细")]
public class TaktApsScheduleItemsController : TaktControllerBase
{
    private readonly ITaktApsScheduleItemService _apsScheduleItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsScheduleItemService">APS排程明细服务</param>
    public TaktApsScheduleItemsController(ITaktApsScheduleItemService apsScheduleItemService)
    {
        _apsScheduleItemService = apsScheduleItemService;
    }

    /// <summary>
    /// 获取APS排程明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:list", "APS排程明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetApsScheduleItemListAsync([FromQuery] TaktApsScheduleItemQueryDto queryDto)
    {
        try
        {
            var result = await _apsScheduleItemService.GetApsScheduleItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取APS排程明细
    /// </summary>
    /// <param name="id">APS排程明细ID</param>
    /// <returns>APS排程明细DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:query", "APS排程明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetApsScheduleItemByIdAsync(long id)
    {
        try
        {
            var result = await _apsScheduleItemService.GetApsScheduleItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("APS排程明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取APS排程明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:query", "APS排程明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetApsScheduleItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _apsScheduleItemService.GetApsScheduleItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建APS排程明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>APS排程明细DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:create", "创建APS排程明细")]
    [HttpPost]
    public async Task<IActionResult> CreateApsScheduleItemAsync([FromBody] TaktApsScheduleItemCreateDto dto)
    {
        try
        {
            var result = await _apsScheduleItemService.CreateApsScheduleItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS排程明细
    /// </summary>
    /// <param name="id">APS排程明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>APS排程明细DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:update", "更新APS排程明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApsScheduleItemAsync(long id, [FromBody] TaktApsScheduleItemUpdateDto dto)
    {
        try
        {
            var result = await _apsScheduleItemService.UpdateApsScheduleItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除APS排程明细
    /// </summary>
    /// <param name="id">APS排程明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:delete", "删除APS排程明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApsScheduleItemByIdAsync(long id)
    {
        try
        {
            await _apsScheduleItemService.DeleteApsScheduleItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除APS排程明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:delete", "批量删除APS排程明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteApsScheduleItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _apsScheduleItemService.DeleteApsScheduleItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS排程明细状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>APS排程明细DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:update", "更新APS排程明细状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateApsScheduleItemStatusAsync([FromBody] TaktApsScheduleItemStatusDto dto)
    {
        try
        {
            var result = await _apsScheduleItemService.UpdateApsScheduleItemStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS排程明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>APS排程明细DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:update", "更新APS排程明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateApsScheduleItemObsoleteAsync([FromBody] TaktApsScheduleItemObsoleteDto dto)
    {
        try
        {
            var result = await _apsScheduleItemService.UpdateApsScheduleItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:aps:schedule:import", "获取APS排程明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetApsScheduleItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _apsScheduleItemService.GetApsScheduleItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入APS排程明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:import", "导入APS排程明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportApsScheduleItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _apsScheduleItemService.ImportApsScheduleItemAsync(stream, sheetName);
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
    /// 导出APS排程明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:export", "导出APS排程明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportApsScheduleItemAsync([FromQuery] TaktApsScheduleItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _apsScheduleItemService.ExportApsScheduleItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
