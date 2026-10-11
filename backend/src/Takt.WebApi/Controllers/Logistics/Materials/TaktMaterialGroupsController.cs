// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialGroupsController.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：物料组主数据控制器
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
/// 物料组主数据控制器
/// 提供物料组主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料组主数据")]
public class TaktMaterialGroupsController : TaktControllerBase
{
    private readonly ITaktMaterialGroupService _materialGroupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialGroupService">物料组主数据服务</param>
    public TaktMaterialGroupsController(ITaktMaterialGroupService materialGroupService)
    {
        _materialGroupService = materialGroupService;
    }

    /// <summary>
    /// 获取物料组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:material:group:list", "物料组主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaterialGroupListAsync([FromQuery] TaktMaterialGroupQueryDto queryDto)
    {
        try
        {
            var result = await _materialGroupService.GetMaterialGroupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取物料组主数据
    /// </summary>
    /// <param name="id">物料组主数据ID</param>
    /// <returns>物料组主数据DTO</returns>
    [TaktPermission("logistics:materials:material:group:query", "物料组主数据详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterialGroupByIdAsync(long id)
    {
        try
        {
            var result = await _materialGroupService.GetMaterialGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound("物料组主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料组主数据选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:group:query", "物料组主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaterialGroupOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _materialGroupService.GetMaterialGroupOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建物料组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>物料组主数据DTO</returns>
    [TaktPermission("logistics:materials:material:group:create", "创建物料组主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterialGroupAsync([FromBody] TaktMaterialGroupCreateDto dto)
    {
        try
        {
            var result = await _materialGroupService.CreateMaterialGroupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料组主数据
    /// </summary>
    /// <param name="id">物料组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>物料组主数据DTO</returns>
    [TaktPermission("logistics:materials:material:group:update", "更新物料组主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialGroupAsync(long id, [FromBody] TaktMaterialGroupUpdateDto dto)
    {
        try
        {
            var result = await _materialGroupService.UpdateMaterialGroupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除物料组主数据
    /// </summary>
    /// <param name="id">物料组主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:group:delete", "删除物料组主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialGroupByIdAsync(long id)
    {
        try
        {
            await _materialGroupService.DeleteMaterialGroupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除物料组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:group:delete", "批量删除物料组主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaterialGroupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _materialGroupService.DeleteMaterialGroupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>物料组主数据DTO</returns>
    [TaktPermission("logistics:materials:material:group:update", "更新物料组主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateMaterialGroupSortAsync([FromBody] TaktMaterialGroupSortDto dto)
    {
        try
        {
            var result = await _materialGroupService.UpdateMaterialGroupSortAsync(dto);
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
    [TaktPermission("logistics:materials:material:group:import", "获取物料组主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaterialGroupTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _materialGroupService.GetMaterialGroupTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入物料组主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:material:group:import", "导入物料组主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaterialGroupAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _materialGroupService.ImportMaterialGroupAsync(stream, sheetName);
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
    /// 导出物料组主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:material:group:export", "导出物料组主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaterialGroupAsync([FromQuery] TaktMaterialGroupQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialGroupService.ExportMaterialGroupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
