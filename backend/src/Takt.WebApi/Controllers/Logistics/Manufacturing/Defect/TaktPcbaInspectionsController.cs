// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionsController.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA检查日报控制器
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
/// PCBA检查日报控制器
/// 提供PCBA检查日报的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "PCBA检查日报")]
public class TaktPcbaInspectionsController : TaktControllerBase
{
    private readonly ITaktPcbaInspectionService _pcbaInspectionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaInspectionService">PCBA检查日报服务</param>
    public TaktPcbaInspectionsController(ITaktPcbaInspectionService pcbaInspectionService)
    {
        _pcbaInspectionService = pcbaInspectionService;
    }

    /// <summary>
    /// 获取PCBA检查日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:list", "PCBA检查日报列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPcbaInspectionListAsync([FromQuery] TaktPcbaInspectionQueryDto queryDto)
    {
        try
        {
            var result = await _pcbaInspectionService.GetPcbaInspectionListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 PCBA 检查不良统计（数据看板 defect-stat）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>PCBA 检查统计</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:list", "PCBA检查不良统计")]
    [HttpGet("defect-stat")]
    public async Task<IActionResult> GetPcbaInspectionStatAsync([FromQuery] TaktDefectStatQueryDto queryDto)
    {
        try
        {
            var result = await _pcbaInspectionService.GetPcbaInspectionStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取PCBA检查日报
    /// </summary>
    /// <param name="id">PCBA检查日报ID</param>
    /// <returns>PCBA检查日报DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:query", "PCBA检查日报详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPcbaInspectionByIdAsync(long id)
    {
        try
        {
            var result = await _pcbaInspectionService.GetPcbaInspectionByIdAsync(id);
            if (result == null)
            {
                return NotFound("PCBA检查日报不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取PCBA检查日报选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:query", "PCBA检查日报选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPcbaInspectionOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _pcbaInspectionService.GetPcbaInspectionOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建PCBA检查日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>PCBA检查日报DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:create", "创建PCBA检查日报")]
    [HttpPost]
    public async Task<IActionResult> CreatePcbaInspectionAsync([FromBody] TaktPcbaInspectionCreateDto dto)
    {
        try
        {
            var result = await _pcbaInspectionService.CreatePcbaInspectionAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新PCBA检查日报
    /// </summary>
    /// <param name="id">PCBA检查日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>PCBA检查日报DTO</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:update", "更新PCBA检查日报")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePcbaInspectionAsync(long id, [FromBody] TaktPcbaInspectionUpdateDto dto)
    {
        try
        {
            var result = await _pcbaInspectionService.UpdatePcbaInspectionAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除PCBA检查日报
    /// </summary>
    /// <param name="id">PCBA检查日报ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:delete", "删除PCBA检查日报")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePcbaInspectionByIdAsync(long id)
    {
        try
        {
            await _pcbaInspectionService.DeletePcbaInspectionByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除PCBA检查日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:delete", "批量删除PCBA检查日报")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePcbaInspectionBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _pcbaInspectionService.DeletePcbaInspectionBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:import", "获取PCBA检查日报导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPcbaInspectionTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _pcbaInspectionService.GetPcbaInspectionTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入PCBA检查日报
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:import", "导入PCBA检查日报")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPcbaInspectionAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _pcbaInspectionService.ImportPcbaInspectionAsync(stream, sheetName);
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
    /// 导出PCBA检查日报
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:export", "导出PCBA检查日报")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPcbaInspectionAsync([FromQuery] TaktPcbaInspectionQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _pcbaInspectionService.ExportPcbaInspectionAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
