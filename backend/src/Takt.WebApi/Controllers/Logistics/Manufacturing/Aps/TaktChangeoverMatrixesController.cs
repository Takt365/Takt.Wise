// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Aps
// 文件名称：TaktChangeoverMatrixesController.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：换型矩阵控制器
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
/// 换型矩阵控制器
/// 提供换型矩阵的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "换型矩阵")]
public class TaktChangeoverMatrixesController : TaktControllerBase
{
    private readonly ITaktChangeoverMatrixService _changeoverMatrixService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="changeoverMatrixService">换型矩阵服务</param>
    public TaktChangeoverMatrixesController(ITaktChangeoverMatrixService changeoverMatrixService)
    {
        _changeoverMatrixService = changeoverMatrixService;
    }

    /// <summary>
    /// 获取换型矩阵列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:aps:changeover:matrix:list", "换型矩阵列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetChangeoverMatrixListAsync([FromQuery] TaktChangeoverMatrixQueryDto queryDto)
    {
        try
        {
            var result = await _changeoverMatrixService.GetChangeoverMatrixListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取换型矩阵
    /// </summary>
    /// <param name="id">换型矩阵ID</param>
    /// <returns>换型矩阵DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:changeover:matrix:query", "换型矩阵详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetChangeoverMatrixByIdAsync(long id)
    {
        try
        {
            var result = await _changeoverMatrixService.GetChangeoverMatrixByIdAsync(id);
            if (result == null)
            {
                return NotFound("换型矩阵不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取换型矩阵选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:aps:changeover:matrix:query", "换型矩阵选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetChangeoverMatrixOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _changeoverMatrixService.GetChangeoverMatrixOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建换型矩阵
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>换型矩阵DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:changeover:matrix:create", "创建换型矩阵")]
    [HttpPost]
    public async Task<IActionResult> CreateChangeoverMatrixAsync([FromBody] TaktChangeoverMatrixCreateDto dto)
    {
        try
        {
            var result = await _changeoverMatrixService.CreateChangeoverMatrixAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新换型矩阵
    /// </summary>
    /// <param name="id">换型矩阵ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>换型矩阵DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:changeover:matrix:update", "更新换型矩阵")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChangeoverMatrixAsync(long id, [FromBody] TaktChangeoverMatrixUpdateDto dto)
    {
        try
        {
            var result = await _changeoverMatrixService.UpdateChangeoverMatrixAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除换型矩阵
    /// </summary>
    /// <param name="id">换型矩阵ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:changeover:matrix:delete", "删除换型矩阵")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChangeoverMatrixByIdAsync(long id)
    {
        try
        {
            await _changeoverMatrixService.DeleteChangeoverMatrixByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除换型矩阵
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:changeover:matrix:delete", "批量删除换型矩阵")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteChangeoverMatrixBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _changeoverMatrixService.DeleteChangeoverMatrixBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新换型矩阵状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>换型矩阵DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:changeover:matrix:update", "更新换型矩阵状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateChangeoverMatrixStatusAsync([FromBody] TaktChangeoverMatrixStatusDto dto)
    {
        try
        {
            var result = await _changeoverMatrixService.UpdateChangeoverMatrixStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:aps:changeover:matrix:import", "获取换型矩阵导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetChangeoverMatrixTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _changeoverMatrixService.GetChangeoverMatrixTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入换型矩阵
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:aps:changeover:matrix:import", "导入换型矩阵")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportChangeoverMatrixAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _changeoverMatrixService.ImportChangeoverMatrixAsync(stream, sheetName);
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
    /// 导出换型矩阵
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:aps:changeover:matrix:export", "导出换型矩阵")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportChangeoverMatrixAsync([FromQuery] TaktChangeoverMatrixQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _changeoverMatrixService.ExportChangeoverMatrixAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
