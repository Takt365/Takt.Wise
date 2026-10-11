// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktPackagingMaterialsController.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：包装物料控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 包装物料控制器
/// 提供包装物料的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "包装物料")]
public class TaktPackagingMaterialsController : TaktControllerBase
{
    private readonly ITaktPackagingMaterialService _packagingMaterialService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="packagingMaterialService">包装物料服务</param>
    public TaktPackagingMaterialsController(ITaktPackagingMaterialService packagingMaterialService)
    {
        _packagingMaterialService = packagingMaterialService;
    }

    /// <summary>
    /// 获取包装物料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:packaging:material:list", "包装物料列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPackagingMaterialListAsync([FromQuery] TaktPackagingMaterialQueryDto queryDto)
    {
        try
        {
            var result = await _packagingMaterialService.GetPackagingMaterialListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取包装物料
    /// </summary>
    /// <param name="id">包装物料ID</param>
    /// <returns>包装物料DTO</returns>
    [TaktPermission("logistics:materials:packaging:material:query", "包装物料详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPackagingMaterialByIdAsync(long id)
    {
        try
        {
            var result = await _packagingMaterialService.GetPackagingMaterialByIdAsync(id);
            if (result == null)
            {
                return NotFound("包装物料不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取包装物料选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:packaging:material:query", "包装物料选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPackagingMaterialOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _packagingMaterialService.GetPackagingMaterialOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建包装物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>包装物料DTO</returns>
    [TaktPermission("logistics:materials:packaging:material:create", "创建包装物料")]
    [HttpPost]
    public async Task<IActionResult> CreatePackagingMaterialAsync([FromBody] TaktPackagingMaterialCreateDto dto)
    {
        try
        {
            var result = await _packagingMaterialService.CreatePackagingMaterialAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新包装物料
    /// </summary>
    /// <param name="id">包装物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>包装物料DTO</returns>
    [TaktPermission("logistics:materials:packaging:material:update", "更新包装物料")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePackagingMaterialAsync(long id, [FromBody] TaktPackagingMaterialUpdateDto dto)
    {
        try
        {
            var result = await _packagingMaterialService.UpdatePackagingMaterialAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除包装物料
    /// </summary>
    /// <param name="id">包装物料ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:packaging:material:delete", "删除包装物料")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePackagingMaterialByIdAsync(long id)
    {
        try
        {
            await _packagingMaterialService.DeletePackagingMaterialByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除包装物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:packaging:material:delete", "批量删除包装物料")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePackagingMaterialBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _packagingMaterialService.DeletePackagingMaterialBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新包装物料排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>包装物料DTO</returns>
    [TaktPermission("logistics:materials:packaging:material:update", "更新包装物料排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdatePackagingMaterialSortAsync([FromBody] TaktPackagingMaterialSortDto dto)
    {
        try
        {
            var result = await _packagingMaterialService.UpdatePackagingMaterialSortAsync(dto);
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
    [TaktPermission("logistics:materials:packaging:material:import", "获取包装物料导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPackagingMaterialTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _packagingMaterialService.GetPackagingMaterialTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入包装物料
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:packaging:material:import", "导入包装物料")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPackagingMaterialAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _packagingMaterialService.ImportPackagingMaterialAsync(stream, sheetName);
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
    /// 导出包装物料
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:packaging:material:export", "导出包装物料")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPackagingMaterialAsync([FromQuery] TaktPackagingMaterialQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _packagingMaterialService.ExportPackagingMaterialAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
