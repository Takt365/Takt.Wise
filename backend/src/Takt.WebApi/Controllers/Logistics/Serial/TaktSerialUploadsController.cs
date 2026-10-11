// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Serial
// 文件名称：TaktSerialUploadsController.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号上传控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Serial;
using Takt.Application.Services.Logistics.Serial;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Serial;

/// <summary>
/// 序列号上传控制器
/// 提供序列号上传的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "序列号上传")]
public class TaktSerialUploadsController : TaktControllerBase
{
    private readonly ITaktSerialUploadService _serialUploadService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serialUploadService">序列号上传服务</param>
    public TaktSerialUploadsController(ITaktSerialUploadService serialUploadService)
    {
        _serialUploadService = serialUploadService;
    }

    /// <summary>
    /// 获取序列号上传列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:serial:upload:list", "序列号上传列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSerialUploadListAsync([FromQuery] TaktSerialUploadQueryDto queryDto)
    {
        try
        {
            var result = await _serialUploadService.GetSerialUploadListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取序列号上传
    /// </summary>
    /// <param name="id">序列号上传ID</param>
    /// <returns>序列号上传DTO</returns>
    [TaktPermission("logistics:serial:upload:query", "序列号上传详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSerialUploadByIdAsync(long id)
    {
        try
        {
            var result = await _serialUploadService.GetSerialUploadByIdAsync(id);
            if (result == null)
            {
                return NotFound("序列号上传不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取序列号上传选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:serial:upload:query", "序列号上传选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSerialUploadOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _serialUploadService.GetSerialUploadOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建序列号上传
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>序列号上传DTO</returns>
    [TaktPermission("logistics:serial:upload:create", "创建序列号上传")]
    [HttpPost]
    public async Task<IActionResult> CreateSerialUploadAsync([FromBody] TaktSerialUploadCreateDto dto)
    {
        try
        {
            var result = await _serialUploadService.CreateSerialUploadAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新序列号上传
    /// </summary>
    /// <param name="id">序列号上传ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>序列号上传DTO</returns>
    [TaktPermission("logistics:serial:upload:update", "更新序列号上传")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSerialUploadAsync(long id, [FromBody] TaktSerialUploadUpdateDto dto)
    {
        try
        {
            var result = await _serialUploadService.UpdateSerialUploadAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除序列号上传
    /// </summary>
    /// <param name="id">序列号上传ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:serial:upload:delete", "删除序列号上传")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSerialUploadByIdAsync(long id)
    {
        try
        {
            await _serialUploadService.DeleteSerialUploadByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除序列号上传
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:serial:upload:delete", "批量删除序列号上传")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSerialUploadBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _serialUploadService.DeleteSerialUploadBatchAsync(ids);
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
    [TaktPermission("logistics:serial:upload:import", "获取序列号上传导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSerialUploadTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _serialUploadService.GetSerialUploadTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入序列号上传
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:serial:upload:import", "导入序列号上传")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSerialUploadAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _serialUploadService.ImportSerialUploadAsync(stream, sheetName);
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
    /// 导出序列号上传
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:serial:upload:export", "导出序列号上传")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSerialUploadAsync([FromQuery] TaktSerialUploadQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _serialUploadService.ExportSerialUploadAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
