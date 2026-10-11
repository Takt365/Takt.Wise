// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialDescriptionsController.cs
// 创建时间：2026-08-05
// 创建人：Takt365(Cursor AI)
// 功能描述：物料描述控制器
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
/// 物料描述控制器
/// 提供物料描述的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料描述")]
public class TaktMaterialDescriptionsController : TaktControllerBase
{
    private readonly ITaktMaterialDescriptionService _materialDescriptionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialDescriptionService">物料描述服务</param>
    public TaktMaterialDescriptionsController(ITaktMaterialDescriptionService materialDescriptionService)
    {
        _materialDescriptionService = materialDescriptionService;
    }

    /// <summary>
    /// 获取物料描述列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:material:description:list", "物料描述列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaterialDescriptionListAsync([FromQuery] TaktMaterialDescriptionQueryDto queryDto)
    {
        try
        {
            var result = await _materialDescriptionService.GetMaterialDescriptionListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取物料描述
    /// </summary>
    /// <param name="id">物料描述ID</param>
    /// <returns>物料描述DTO</returns>
    [TaktPermission("logistics:materials:material:description:query", "物料描述详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterialDescriptionByIdAsync(long id)
    {
        try
        {
            var result = await _materialDescriptionService.GetMaterialDescriptionByIdAsync(id);
            if (result == null)
            {
                return NotFound("物料描述不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料描述选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:description:query", "物料描述选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaterialDescriptionOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _materialDescriptionService.GetMaterialDescriptionOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建物料描述
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>物料描述DTO</returns>
    [TaktPermission("logistics:materials:material:description:create", "创建物料描述")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterialDescriptionAsync([FromBody] TaktMaterialDescriptionCreateDto dto)
    {
        try
        {
            var result = await _materialDescriptionService.CreateMaterialDescriptionAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料描述
    /// </summary>
    /// <param name="id">物料描述ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>物料描述DTO</returns>
    [TaktPermission("logistics:materials:material:description:update", "更新物料描述")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialDescriptionAsync(long id, [FromBody] TaktMaterialDescriptionUpdateDto dto)
    {
        try
        {
            var result = await _materialDescriptionService.UpdateMaterialDescriptionAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除物料描述
    /// </summary>
    /// <param name="id">物料描述ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:description:delete", "删除物料描述")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialDescriptionByIdAsync(long id)
    {
        try
        {
            await _materialDescriptionService.DeleteMaterialDescriptionByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除物料描述
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:description:delete", "批量删除物料描述")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaterialDescriptionBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _materialDescriptionService.DeleteMaterialDescriptionBatchAsync(ids);
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
    [TaktPermission("logistics:materials:material:description:import", "获取物料描述导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaterialDescriptionTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _materialDescriptionService.GetMaterialDescriptionTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入物料描述
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:material:description:import", "导入物料描述")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaterialDescriptionAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _materialDescriptionService.ImportMaterialDescriptionAsync(stream, sheetName);
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
    /// 导出物料描述
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:material:description:export", "导出物料描述")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaterialDescriptionAsync([FromQuery] TaktMaterialDescriptionQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialDescriptionService.ExportMaterialDescriptionAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
