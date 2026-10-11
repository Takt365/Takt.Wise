// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepairsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA改修日报控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Application.Services.Logistics.Manufacturing.Defect;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA改修日报控制器
/// 提供PCBA改修日报的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "PCBA改修日报")]
public class TaktPcbaRepairsController : TaktControllerBase
{
    private readonly ITaktPcbaRepairService _pcbaRepairService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaRepairService">PCBA改修日报服务</param>
    public TaktPcbaRepairsController(ITaktPcbaRepairService pcbaRepairService)
    {
        _pcbaRepairService = pcbaRepairService;
    }

    /// <summary>
    /// 获取PCBA改修日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:list", "PCBA改修日报列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPcbaRepairListAsync([FromQuery] TaktPcbaRepairQueryDto queryDto)
    {
        try
        {
            var result = await _pcbaRepairService.GetPcbaRepairListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 PCBA 改修不良统计（数据看板 defect-stat）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>PCBA 改修统计</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:list", "PCBA改修不良统计")]
    [HttpGet("defect-stat")]
    public async Task<IActionResult> GetPcbaRepairStatAsync([FromQuery] TaktDefectStatQueryDto queryDto)
    {
        try
        {
            var result = await _pcbaRepairService.GetPcbaRepairStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取PCBA改修日报
    /// </summary>
    /// <param name="id">PCBA改修日报ID</param>
    /// <returns>PCBA改修日报DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:query", "PCBA改修日报详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPcbaRepairByIdAsync(long id)
    {
        try
        {
            var result = await _pcbaRepairService.GetPcbaRepairByIdAsync(id);
            if (result == null)
            {
                return NotFound("PCBA改修日报不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取PCBA改修日报选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:query", "PCBA改修日报选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPcbaRepairOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _pcbaRepairService.GetPcbaRepairOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建PCBA改修日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>PCBA改修日报DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:create", "创建PCBA改修日报")]
    [HttpPost]
    public async Task<IActionResult> CreatePcbaRepairAsync([FromBody] TaktPcbaRepairCreateDto dto)
    {
        try
        {
            var result = await _pcbaRepairService.CreatePcbaRepairAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新PCBA改修日报
    /// </summary>
    /// <param name="id">PCBA改修日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>PCBA改修日报DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:update", "更新PCBA改修日报")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePcbaRepairAsync(long id, [FromBody] TaktPcbaRepairUpdateDto dto)
    {
        try
        {
            var result = await _pcbaRepairService.UpdatePcbaRepairAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除PCBA改修日报
    /// </summary>
    /// <param name="id">PCBA改修日报ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:delete", "删除PCBA改修日报")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePcbaRepairByIdAsync(long id)
    {
        try
        {
            await _pcbaRepairService.DeletePcbaRepairByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除PCBA改修日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:delete", "批量删除PCBA改修日报")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePcbaRepairBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _pcbaRepairService.DeletePcbaRepairBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:import", "获取PCBA改修日报导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPcbaRepairTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _pcbaRepairService.GetPcbaRepairTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入PCBA改修日报
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:import", "导入PCBA改修日报")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPcbaRepairAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _pcbaRepairService.ImportPcbaRepairAsync(stream, sheetName);
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
    /// 导出PCBA改修日报
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:export", "导出PCBA改修日报")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPcbaRepairAsync([FromQuery] TaktPcbaRepairQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _pcbaRepairService.ExportPcbaRepairAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
