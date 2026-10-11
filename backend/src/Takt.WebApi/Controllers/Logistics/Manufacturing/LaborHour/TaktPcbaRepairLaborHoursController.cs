// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.LaborHour
// 文件名称：TaktPcbaRepairLaborHoursController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA改修工数统计控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.LaborHour;
using Takt.Application.Services.Logistics.Manufacturing.LaborHour;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.LaborHour;

/// <summary>
/// PCBA改修工数统计控制器
/// 提供PCBA改修工数统计的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "PCBA改修工数统计")]
public class TaktPcbaRepairLaborHoursController : TaktControllerBase
{
    private readonly ITaktPcbaRepairLaborHourService _pcbaRepairLaborHourService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaRepairLaborHourService">PCBA改修工数统计服务</param>
    public TaktPcbaRepairLaborHoursController(ITaktPcbaRepairLaborHourService pcbaRepairLaborHourService)
    {
        _pcbaRepairLaborHourService = pcbaRepairLaborHourService;
    }

    /// <summary>
    /// 获取PCBA改修工数统计列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:labor:hour:pcba:repair:list", "PCBA改修工数统计列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPcbaRepairLaborHourListAsync([FromQuery] TaktPcbaRepairLaborHourQueryDto queryDto)
    {
        try
        {
            var result = await _pcbaRepairLaborHourService.GetPcbaRepairLaborHourListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取PCBA改修工数统计
    /// </summary>
    /// <param name="id">PCBA改修工数统计ID</param>
    /// <returns>PCBA改修工数统计DTO</returns>
    [TaktPermission("logistics:manufacturing:labor:hour:pcba:repair:query", "PCBA改修工数统计详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPcbaRepairLaborHourByIdAsync(long id)
    {
        try
        {
            var result = await _pcbaRepairLaborHourService.GetPcbaRepairLaborHourByIdAsync(id);
            if (result == null)
            {
                return NotFound("PCBA改修工数统计不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取PCBA改修工数统计选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:labor:hour:pcba:repair:query", "PCBA改修工数统计选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPcbaRepairLaborHourOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _pcbaRepairLaborHourService.GetPcbaRepairLaborHourOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建PCBA改修工数统计
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>PCBA改修工数统计DTO</returns>
    [TaktPermission("logistics:manufacturing:labor:hour:pcba:repair:create", "创建PCBA改修工数统计")]
    [HttpPost]
    public async Task<IActionResult> CreatePcbaRepairLaborHourAsync([FromBody] TaktPcbaRepairLaborHourCreateDto dto)
    {
        try
        {
            var result = await _pcbaRepairLaborHourService.CreatePcbaRepairLaborHourAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新PCBA改修工数统计
    /// </summary>
    /// <param name="id">PCBA改修工数统计ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>PCBA改修工数统计DTO</returns>
    [TaktPermission("logistics:manufacturing:labor:hour:pcba:repair:update", "更新PCBA改修工数统计")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePcbaRepairLaborHourAsync(long id, [FromBody] TaktPcbaRepairLaborHourUpdateDto dto)
    {
        try
        {
            var result = await _pcbaRepairLaborHourService.UpdatePcbaRepairLaborHourAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除PCBA改修工数统计
    /// </summary>
    /// <param name="id">PCBA改修工数统计ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:labor:hour:pcba:repair:delete", "删除PCBA改修工数统计")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePcbaRepairLaborHourByIdAsync(long id)
    {
        try
        {
            await _pcbaRepairLaborHourService.DeletePcbaRepairLaborHourByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除PCBA改修工数统计
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:labor:hour:pcba:repair:delete", "批量删除PCBA改修工数统计")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePcbaRepairLaborHourBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _pcbaRepairLaborHourService.DeletePcbaRepairLaborHourBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:labor:hour:pcba:repair:import", "获取PCBA改修工数统计导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPcbaRepairLaborHourTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _pcbaRepairLaborHourService.GetPcbaRepairLaborHourTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入PCBA改修工数统计
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:labor:hour:pcba:repair:import", "导入PCBA改修工数统计")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPcbaRepairLaborHourAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _pcbaRepairLaborHourService.ImportPcbaRepairLaborHourAsync(stream, sheetName);
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
    /// 导出PCBA改修工数统计
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:labor:hour:pcba:repair:export", "导出PCBA改修工数统计")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPcbaRepairLaborHourAsync([FromQuery] TaktPcbaRepairLaborHourQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _pcbaRepairLaborHourService.ExportPcbaRepairLaborHourAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
