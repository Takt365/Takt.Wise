// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktManufacturerMaterialsController.cs
// 创建时间：2026-08-06
// 创建人：Takt365(Cursor AI)
// 功能描述：制造商物料控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Procurement;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Procurement;

/// <summary>
/// 制造商物料控制器
/// 提供制造商物料的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "制造商物料")]
public class TaktManufacturerMaterialsController : TaktControllerBase
{
    private readonly ITaktManufacturerMaterialService _manufacturerMaterialService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="manufacturerMaterialService">制造商物料服务</param>
    public TaktManufacturerMaterialsController(ITaktManufacturerMaterialService manufacturerMaterialService)
    {
        _manufacturerMaterialService = manufacturerMaterialService;
    }

    /// <summary>
    /// 获取制造商物料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:manufacturer:material:list", "制造商物料列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetManufacturerMaterialListAsync([FromQuery] TaktManufacturerMaterialQueryDto queryDto)
    {
        try
        {
            var result = await _manufacturerMaterialService.GetManufacturerMaterialListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取制造商物料
    /// </summary>
    /// <param name="id">制造商物料ID</param>
    /// <returns>制造商物料DTO</returns>
    [TaktPermission("logistics:procurement:manufacturer:material:query", "制造商物料详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetManufacturerMaterialByIdAsync(long id)
    {
        try
        {
            var result = await _manufacturerMaterialService.GetManufacturerMaterialByIdAsync(id);
            if (result == null)
            {
                return NotFound("制造商物料不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取制造商物料选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:manufacturer:material:query", "制造商物料选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetManufacturerMaterialOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _manufacturerMaterialService.GetManufacturerMaterialOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建制造商物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>制造商物料DTO</returns>
    [TaktPermission("logistics:procurement:manufacturer:material:create", "创建制造商物料")]
    [HttpPost]
    public async Task<IActionResult> CreateManufacturerMaterialAsync([FromBody] TaktManufacturerMaterialCreateDto dto)
    {
        try
        {
            var result = await _manufacturerMaterialService.CreateManufacturerMaterialAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新制造商物料
    /// </summary>
    /// <param name="id">制造商物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>制造商物料DTO</returns>
    [TaktPermission("logistics:procurement:manufacturer:material:update", "更新制造商物料")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateManufacturerMaterialAsync(long id, [FromBody] TaktManufacturerMaterialUpdateDto dto)
    {
        try
        {
            var result = await _manufacturerMaterialService.UpdateManufacturerMaterialAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除制造商物料
    /// </summary>
    /// <param name="id">制造商物料ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:manufacturer:material:delete", "删除制造商物料")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteManufacturerMaterialByIdAsync(long id)
    {
        try
        {
            await _manufacturerMaterialService.DeleteManufacturerMaterialByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除制造商物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:manufacturer:material:delete", "批量删除制造商物料")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteManufacturerMaterialBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _manufacturerMaterialService.DeleteManufacturerMaterialBatchAsync(ids);
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
    [TaktPermission("logistics:procurement:manufacturer:material:import", "获取制造商物料导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetManufacturerMaterialTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _manufacturerMaterialService.GetManufacturerMaterialTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入制造商物料
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:manufacturer:material:import", "导入制造商物料")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportManufacturerMaterialAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _manufacturerMaterialService.ImportManufacturerMaterialAsync(stream, sheetName);
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
    /// 导出制造商物料
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:manufacturer:material:export", "导出制造商物料")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportManufacturerMaterialAsync([FromQuery] TaktManufacturerMaterialQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _manufacturerMaterialService.ExportManufacturerMaterialAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
