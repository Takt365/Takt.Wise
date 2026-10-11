// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktInspectionStandardsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：检验标准控制器
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
/// 检验标准控制器
/// 提供检验标准的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "检验标准")]
public class TaktInspectionStandardsController : TaktControllerBase
{
    private readonly ITaktInspectionStandardService _inspectionStandardService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="inspectionStandardService">检验标准服务</param>
    public TaktInspectionStandardsController(ITaktInspectionStandardService inspectionStandardService)
    {
        _inspectionStandardService = inspectionStandardService;
    }

    /// <summary>
    /// 获取检验标准列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:list", "检验标准列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetInspectionStandardListAsync([FromQuery] TaktInspectionStandardQueryDto queryDto)
    {
        try
        {
            var result = await _inspectionStandardService.GetInspectionStandardListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取检验标准
    /// </summary>
    /// <param name="id">检验标准ID</param>
    /// <returns>检验标准DTO</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:query", "检验标准详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInspectionStandardByIdAsync(long id)
    {
        try
        {
            var result = await _inspectionStandardService.GetInspectionStandardByIdAsync(id);
            if (result == null)
            {
                return NotFound("检验标准不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取检验标准选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:query", "检验标准选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetInspectionStandardOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _inspectionStandardService.GetInspectionStandardOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建检验标准
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>检验标准DTO</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:create", "创建检验标准")]
    [HttpPost]
    public async Task<IActionResult> CreateInspectionStandardAsync([FromBody] TaktInspectionStandardCreateDto dto)
    {
        try
        {
            var result = await _inspectionStandardService.CreateInspectionStandardAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新检验标准
    /// </summary>
    /// <param name="id">检验标准ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>检验标准DTO</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:update", "更新检验标准")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInspectionStandardAsync(long id, [FromBody] TaktInspectionStandardUpdateDto dto)
    {
        try
        {
            var result = await _inspectionStandardService.UpdateInspectionStandardAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除检验标准
    /// </summary>
    /// <param name="id">检验标准ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:delete", "删除检验标准")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInspectionStandardByIdAsync(long id)
    {
        try
        {
            await _inspectionStandardService.DeleteInspectionStandardByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除检验标准
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:delete", "批量删除检验标准")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteInspectionStandardBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _inspectionStandardService.DeleteInspectionStandardBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新检验标准状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>检验标准DTO</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:update", "更新检验标准状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateInspectionStandardStatusAsync([FromBody] TaktInspectionStandardStatusDto dto)
    {
        try
        {
            var result = await _inspectionStandardService.UpdateInspectionStandardStatusAsync(dto);
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
    [TaktPermission("logistics:quality:operation:inspection:standard:import", "获取检验标准导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetInspectionStandardTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _inspectionStandardService.GetInspectionStandardTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入检验标准
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:import", "导入检验标准")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportInspectionStandardAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _inspectionStandardService.ImportInspectionStandardAsync(stream, sheetName);
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
    /// 导出检验标准
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:inspection:standard:export", "导出检验标准")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportInspectionStandardAsync([FromQuery] TaktInspectionStandardQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _inspectionStandardService.ExportInspectionStandardAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
