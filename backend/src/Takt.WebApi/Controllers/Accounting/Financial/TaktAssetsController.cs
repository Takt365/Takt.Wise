// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Accounting.Financial
// 文件名称：TaktAssetsController.cs
// 创建时间：2026-08-30
// 创建人：Takt365(Cursor AI)
// 功能描述：资产控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Application.Services.Accounting.Financial;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Accounting.Financial;

/// <summary>
/// 资产控制器
/// 提供资产的 REST API
/// </summary>
[ApiModule(3, "财务核算")]
[Route("api/[controller]", Name = "资产")]
public class TaktAssetsController : TaktControllerBase
{
    private readonly ITaktAssetService _assetService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assetService">资产服务</param>
    public TaktAssetsController(ITaktAssetService assetService)
    {
        _assetService = assetService;
    }

    /// <summary>
    /// 获取资产列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:financial:asset:list", "资产列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAssetListAsync([FromQuery] TaktAssetQueryDto queryDto)
    {
        try
        {
            var result = await _assetService.GetAssetListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取资产
    /// </summary>
    /// <param name="id">资产ID</param>
    /// <returns>资产DTO</returns>
    [TaktPermission("accounting:financial:asset:query", "资产详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssetByIdAsync(long id)
    {
        try
        {
            var result = await _assetService.GetAssetByIdAsync(id);
            if (result == null)
            {
                return NotFound("资产不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取资产选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:financial:asset:query", "资产选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetAssetOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _assetService.GetAssetOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建资产
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>资产DTO</returns>
    [TaktPermission("accounting:financial:asset:create", "创建资产")]
    [HttpPost]
    public async Task<IActionResult> CreateAssetAsync([FromBody] TaktAssetCreateDto dto)
    {
        try
        {
            var result = await _assetService.CreateAssetAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新资产
    /// </summary>
    /// <param name="id">资产ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>资产DTO</returns>
    [TaktPermission("accounting:financial:asset:update", "更新资产")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssetAsync(long id, [FromBody] TaktAssetUpdateDto dto)
    {
        try
        {
            var result = await _assetService.UpdateAssetAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除资产
    /// </summary>
    /// <param name="id">资产ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:asset:delete", "删除资产")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssetByIdAsync(long id)
    {
        try
        {
            await _assetService.DeleteAssetByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除资产
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:financial:asset:delete", "批量删除资产")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteAssetBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _assetService.DeleteAssetBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新资产状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>资产DTO</returns>
    [TaktPermission("accounting:financial:asset:update", "更新资产状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateAssetStatusAsync([FromBody] TaktAssetStatusDto dto)
    {
        try
        {
            var result = await _assetService.UpdateAssetStatusAsync(dto);
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
    [TaktPermission("accounting:financial:asset:import", "获取资产导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetAssetTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _assetService.GetAssetTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入资产
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("accounting:financial:asset:import", "导入资产")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportAssetAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _assetService.ImportAssetAsync(stream, sheetName);
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
    /// 导出资产
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:financial:asset:export", "导出资产")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportAssetAsync([FromQuery] TaktAssetQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _assetService.ExportAssetAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
