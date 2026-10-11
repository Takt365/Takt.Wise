// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputsController.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services.Logistics.Manufacturing.Output;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA日报控制器
/// 提供PCBA日报的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "PCBA日报")]
public class TaktPcbaOutputsController : TaktControllerBase
{
    private readonly ITaktPcbaOutputService _pcbaOutputService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaOutputService">PCBA日报服务</param>
    public TaktPcbaOutputsController(ITaktPcbaOutputService pcbaOutputService)
    {
        _pcbaOutputService = pcbaOutputService;
    }

    /// <summary>
    /// 获取PCBA日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:output:pcba:list", "PCBA日报列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPcbaOutputListAsync([FromQuery] TaktPcbaOutputQueryDto queryDto)
    {
        try
        {
            var result = await _pcbaOutputService.GetPcbaOutputListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 PCBA 生产统计（数据看板 production-stat）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>PCBA 生产统计</returns>
    [TaktPermission("logistics:manufacturing:output:pcba:list", "PCBA生产统计")]
    [HttpGet("production-stat")]
    public async Task<IActionResult> GetPcbaOutputProductionStatAsync([FromQuery] TaktOutputProductionStatQueryDto queryDto)
    {
        try
        {
            var result = await _pcbaOutputService.GetPcbaOutputProductionStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取PCBA日报
    /// </summary>
    /// <param name="id">PCBA日报ID</param>
    /// <returns>PCBA日报DTO</returns>
    [TaktPermission("logistics:manufacturing:output:pcba:query", "PCBA日报详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPcbaOutputByIdAsync(long id)
    {
        try
        {
            var result = await _pcbaOutputService.GetPcbaOutputByIdAsync(id);
            if (result == null)
            {
                return NotFound("PCBA日报不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取PCBA日报选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:output:pcba:query", "PCBA日报选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPcbaOutputOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _pcbaOutputService.GetPcbaOutputOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建PCBA日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>PCBA日报DTO</returns>
    [TaktPermission("logistics:manufacturing:output:pcba:create", "创建PCBA日报")]
    [HttpPost]
    public async Task<IActionResult> CreatePcbaOutputAsync([FromBody] TaktPcbaOutputCreateDto dto)
    {
        try
        {
            var result = await _pcbaOutputService.CreatePcbaOutputAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新PCBA日报
    /// </summary>
    /// <param name="id">PCBA日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>PCBA日报DTO</returns>
    [TaktPermission("logistics:manufacturing:output:pcba:update", "更新PCBA日报")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePcbaOutputAsync(long id, [FromBody] TaktPcbaOutputUpdateDto dto)
    {
        try
        {
            var result = await _pcbaOutputService.UpdatePcbaOutputAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除PCBA日报
    /// </summary>
    /// <param name="id">PCBA日报ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:pcba:delete", "删除PCBA日报")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePcbaOutputByIdAsync(long id)
    {
        try
        {
            await _pcbaOutputService.DeletePcbaOutputByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除PCBA日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:pcba:delete", "批量删除PCBA日报")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePcbaOutputBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _pcbaOutputService.DeletePcbaOutputBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:output:pcba:import", "获取PCBA日报导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPcbaOutputTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _pcbaOutputService.GetPcbaOutputTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入PCBA日报
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:output:pcba:import", "导入PCBA日报")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPcbaOutputAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _pcbaOutputService.ImportPcbaOutputAsync(stream, sheetName);
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
    /// 导出PCBA日报
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:output:pcba:export", "导出PCBA日报")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPcbaOutputAsync([FromQuery] TaktPcbaOutputQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _pcbaOutputService.ExportPcbaOutputAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
