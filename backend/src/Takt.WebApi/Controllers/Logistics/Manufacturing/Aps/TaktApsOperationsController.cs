// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Aps
// 文件名称：TaktApsOperationsController.cs
// 创建时间：2026-07-24
// 创建人：Takt365(Cursor AI)
// 功能描述：APS工序排程控制器
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
/// APS工序排程控制器
/// 提供APS工序排程的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "APS工序排程")]
public class TaktApsOperationsController : TaktControllerBase
{
    private readonly ITaktApsOperationService _apsOperationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsOperationService">APS工序排程服务</param>
    public TaktApsOperationsController(ITaktApsOperationService apsOperationService)
    {
        _apsOperationService = apsOperationService;
    }

    /// <summary>
    /// 获取APS工序排程列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:list", "APS工序排程列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetApsOperationListAsync([FromQuery] TaktApsOperationQueryDto queryDto)
    {
        try
        {
            var result = await _apsOperationService.GetApsOperationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取APS工序排程
    /// </summary>
    /// <param name="id">APS工序排程ID</param>
    /// <returns>APS工序排程DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:query", "APS工序排程详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetApsOperationByIdAsync(long id)
    {
        try
        {
            var result = await _apsOperationService.GetApsOperationByIdAsync(id);
            if (result == null)
            {
                return NotFound("APS工序排程不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取APS工序排程选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:query", "APS工序排程选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetApsOperationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _apsOperationService.GetApsOperationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建APS工序排程
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>APS工序排程DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:create", "创建APS工序排程")]
    [HttpPost]
    public async Task<IActionResult> CreateApsOperationAsync([FromBody] TaktApsOperationCreateDto dto)
    {
        try
        {
            var result = await _apsOperationService.CreateApsOperationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS工序排程
    /// </summary>
    /// <param name="id">APS工序排程ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>APS工序排程DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:update", "更新APS工序排程")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApsOperationAsync(long id, [FromBody] TaktApsOperationUpdateDto dto)
    {
        try
        {
            var result = await _apsOperationService.UpdateApsOperationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除APS工序排程
    /// </summary>
    /// <param name="id">APS工序排程ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:delete", "删除APS工序排程")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApsOperationByIdAsync(long id)
    {
        try
        {
            await _apsOperationService.DeleteApsOperationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除APS工序排程
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:delete", "批量删除APS工序排程")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteApsOperationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _apsOperationService.DeleteApsOperationBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS工序排程状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>APS工序排程DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:update", "更新APS工序排程状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateApsOperationStatusAsync([FromBody] TaktApsOperationStatusDto dto)
    {
        try
        {
            var result = await _apsOperationService.UpdateApsOperationStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS工序排程作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>APS工序排程DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:update", "更新APS工序排程作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateApsOperationObsoleteAsync([FromBody] TaktApsOperationObsoleteDto dto)
    {
        try
        {
            var result = await _apsOperationService.UpdateApsOperationObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:aps:schedule:import", "获取APS工序排程导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetApsOperationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _apsOperationService.GetApsOperationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入APS工序排程
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:import", "导入APS工序排程")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportApsOperationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _apsOperationService.ImportApsOperationAsync(stream, sheetName);
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
    /// 导出APS工序排程
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:export", "导出APS工序排程")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportApsOperationAsync([FromQuery] TaktApsOperationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _apsOperationService.ExportApsOperationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
