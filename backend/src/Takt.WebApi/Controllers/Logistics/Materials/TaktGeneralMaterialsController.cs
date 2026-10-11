// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktGeneralMaterialsController.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：全局物料控制器
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
/// 全局物料控制器
/// 提供全局物料的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "全局物料")]
public class TaktGeneralMaterialsController : TaktControllerBase
{
    private readonly ITaktGeneralMaterialService _generalMaterialService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="generalMaterialService">全局物料服务</param>
    public TaktGeneralMaterialsController(ITaktGeneralMaterialService generalMaterialService)
    {
        _generalMaterialService = generalMaterialService;
    }

    /// <summary>
    /// 获取全局物料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:general:material:list", "全局物料列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetGeneralMaterialListAsync([FromQuery] TaktGeneralMaterialQueryDto queryDto)
    {
        try
        {
            var result = await _generalMaterialService.GetGeneralMaterialListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取全局物料
    /// </summary>
    /// <param name="id">全局物料ID</param>
    /// <returns>全局物料DTO</returns>
    [TaktPermission("logistics:materials:general:material:query", "全局物料详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGeneralMaterialByIdAsync(long id)
    {
        try
        {
            var result = await _generalMaterialService.GetGeneralMaterialByIdAsync(id);
            if (result == null)
            {
                return NotFound("全局物料不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取全局物料选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:general:material:query", "全局物料选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetGeneralMaterialOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _generalMaterialService.GetGeneralMaterialOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建全局物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>全局物料DTO</returns>
    [TaktPermission("logistics:materials:general:material:create", "创建全局物料")]
    [HttpPost]
    public async Task<IActionResult> CreateGeneralMaterialAsync([FromBody] TaktGeneralMaterialCreateDto dto)
    {
        try
        {
            var result = await _generalMaterialService.CreateGeneralMaterialAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新全局物料
    /// </summary>
    /// <param name="id">全局物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>全局物料DTO</returns>
    [TaktPermission("logistics:materials:general:material:update", "更新全局物料")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGeneralMaterialAsync(long id, [FromBody] TaktGeneralMaterialUpdateDto dto)
    {
        try
        {
            var result = await _generalMaterialService.UpdateGeneralMaterialAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除全局物料
    /// </summary>
    /// <param name="id">全局物料ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:general:material:delete", "删除全局物料")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGeneralMaterialByIdAsync(long id)
    {
        try
        {
            await _generalMaterialService.DeleteGeneralMaterialByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除全局物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:general:material:delete", "批量删除全局物料")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteGeneralMaterialBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _generalMaterialService.DeleteGeneralMaterialBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新全局物料状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>全局物料DTO</returns>
    [TaktPermission("logistics:materials:general:material:update", "更新全局物料状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateGeneralMaterialStatusAsync([FromBody] TaktGeneralMaterialStatusDto dto)
    {
        try
        {
            var result = await _generalMaterialService.UpdateGeneralMaterialStatusAsync(dto);
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
    [TaktPermission("logistics:materials:general:material:import", "获取全局物料导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetGeneralMaterialTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _generalMaterialService.GetGeneralMaterialTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入全局物料
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:general:material:import", "导入全局物料")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportGeneralMaterialAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _generalMaterialService.ImportGeneralMaterialAsync(stream, sheetName);
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
    /// 导出全局物料
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:general:material:export", "导出全局物料")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportGeneralMaterialAsync([FromQuery] TaktGeneralMaterialQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _generalMaterialService.ExportGeneralMaterialAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
