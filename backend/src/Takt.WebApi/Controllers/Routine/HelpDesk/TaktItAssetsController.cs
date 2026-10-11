// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.HelpDesk
// 文件名称：TaktItAssetsController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：IT设备保修扩展控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Application.Services.Routine.HelpDesk;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.HelpDesk;

/// <summary>
/// IT设备保修扩展控制器
/// 提供IT设备保修扩展的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "IT设备保修扩展")]
public class TaktItAssetsController : TaktControllerBase
{
    private readonly ITaktItAssetService _itAssetService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="itAssetService">IT设备保修扩展服务</param>
    public TaktItAssetsController(ITaktItAssetService itAssetService)
    {
        _itAssetService = itAssetService;
    }

    /// <summary>
    /// 获取IT设备保修扩展列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:help:desk:it:asset:list", "IT设备保修扩展列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetItAssetListAsync([FromQuery] TaktItAssetQueryDto queryDto)
    {
        try
        {
            var result = await _itAssetService.GetItAssetListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取IT设备保修扩展
    /// </summary>
    /// <param name="id">IT设备保修扩展ID</param>
    /// <returns>IT设备保修扩展DTO</returns>
    [TaktPermission("routine:help:desk:it:asset:query", "IT设备保修扩展详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetItAssetByIdAsync(long id)
    {
        try
        {
            var result = await _itAssetService.GetItAssetByIdAsync(id);
            if (result == null)
            {
                return NotFound("IT设备保修扩展不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取IT设备保修扩展选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:help:desk:it:asset:query", "IT设备保修扩展选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetItAssetOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _itAssetService.GetItAssetOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建IT设备保修扩展
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>IT设备保修扩展DTO</returns>
    [TaktPermission("routine:help:desk:it:asset:create", "创建IT设备保修扩展")]
    [HttpPost]
    public async Task<IActionResult> CreateItAssetAsync([FromBody] TaktItAssetCreateDto dto)
    {
        try
        {
            var result = await _itAssetService.CreateItAssetAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新IT设备保修扩展
    /// </summary>
    /// <param name="id">IT设备保修扩展ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>IT设备保修扩展DTO</returns>
    [TaktPermission("routine:help:desk:it:asset:update", "更新IT设备保修扩展")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItAssetAsync(long id, [FromBody] TaktItAssetUpdateDto dto)
    {
        try
        {
            var result = await _itAssetService.UpdateItAssetAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除IT设备保修扩展
    /// </summary>
    /// <param name="id">IT设备保修扩展ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:it:asset:delete", "删除IT设备保修扩展")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItAssetByIdAsync(long id)
    {
        try
        {
            await _itAssetService.DeleteItAssetByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除IT设备保修扩展
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:it:asset:delete", "批量删除IT设备保修扩展")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteItAssetBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _itAssetService.DeleteItAssetBatchAsync(ids);
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
    [TaktPermission("routine:help:desk:it:asset:import", "获取IT设备保修扩展导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetItAssetTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _itAssetService.GetItAssetTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入IT设备保修扩展
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:help:desk:it:asset:import", "导入IT设备保修扩展")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportItAssetAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _itAssetService.ImportItAssetAsync(stream, sheetName);
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
    /// 导出IT设备保修扩展
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:help:desk:it:asset:export", "导出IT设备保修扩展")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportItAssetAsync([FromQuery] TaktItAssetQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _itAssetService.ExportItAssetAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
