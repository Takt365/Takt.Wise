// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecScansController.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP物料扫码记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Application.Services.Logistics.Manufacturing.Sop;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP物料扫码记录控制器
/// 提供SOP物料扫码记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SOP物料扫码记录")]
public class TaktSopExecScansController : TaktControllerBase
{
    private readonly ITaktSopExecScanService _sopExecScanService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopExecScanService">SOP物料扫码记录服务</param>
    public TaktSopExecScansController(ITaktSopExecScanService sopExecScanService)
    {
        _sopExecScanService = sopExecScanService;
    }

    /// <summary>
    /// 获取SOP物料扫码记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:list", "SOP物料扫码记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSopExecScanListAsync([FromQuery] TaktSopExecScanQueryDto queryDto)
    {
        try
        {
            var result = await _sopExecScanService.GetSopExecScanListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取SOP物料扫码记录
    /// </summary>
    /// <param name="id">SOP物料扫码记录ID</param>
    /// <returns>SOP物料扫码记录DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:query", "SOP物料扫码记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSopExecScanByIdAsync(long id)
    {
        try
        {
            var result = await _sopExecScanService.GetSopExecScanByIdAsync(id);
            if (result == null)
            {
                return NotFound("SOP物料扫码记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取SOP物料扫码记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:query", "SOP物料扫码记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSopExecScanOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sopExecScanService.GetSopExecScanOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建SOP物料扫码记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>SOP物料扫码记录DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:create", "创建SOP物料扫码记录")]
    [HttpPost]
    public async Task<IActionResult> CreateSopExecScanAsync([FromBody] TaktSopExecScanCreateDto dto)
    {
        try
        {
            var result = await _sopExecScanService.CreateSopExecScanAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP物料扫码记录
    /// </summary>
    /// <param name="id">SOP物料扫码记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>SOP物料扫码记录DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:update", "更新SOP物料扫码记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSopExecScanAsync(long id, [FromBody] TaktSopExecScanUpdateDto dto)
    {
        try
        {
            var result = await _sopExecScanService.UpdateSopExecScanAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除SOP物料扫码记录
    /// </summary>
    /// <param name="id">SOP物料扫码记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:delete", "删除SOP物料扫码记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSopExecScanByIdAsync(long id)
    {
        try
        {
            await _sopExecScanService.DeleteSopExecScanByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除SOP物料扫码记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:delete", "批量删除SOP物料扫码记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSopExecScanBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sopExecScanService.DeleteSopExecScanBatchAsync(ids);
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
    [TaktPermission("logistics:manufacturing:sop:exec:import", "获取SOP物料扫码记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSopExecScanTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sopExecScanService.GetSopExecScanTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入SOP物料扫码记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:import", "导入SOP物料扫码记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSopExecScanAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sopExecScanService.ImportSopExecScanAsync(stream, sheetName);
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
    /// 导出SOP物料扫码记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:sop:exec:export", "导出SOP物料扫码记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSopExecScanAsync([FromQuery] TaktSopExecScanQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sopExecScanService.ExportSopExecScanAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
