// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomGroupsController.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM组主数据控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM组主数据控制器
/// 提供BOM组主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM组主数据")]
public class TaktBomGroupsController : TaktControllerBase
{
    private readonly ITaktBomGroupService _bomGroupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomGroupService">BOM组主数据服务</param>
    public TaktBomGroupsController(ITaktBomGroupService bomGroupService)
    {
        _bomGroupService = bomGroupService;
    }

    /// <summary>
    /// 获取BOM组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:group:list", "BOM组主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBomGroupListAsync([FromQuery] TaktBomGroupQueryDto queryDto)
    {
        try
        {
            var result = await _bomGroupService.GetBomGroupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取BOM组主数据
    /// </summary>
    /// <param name="id">BOM组主数据ID</param>
    /// <returns>BOM组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:group:query", "BOM组主数据详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetBomGroupByIdAsync(long id)
    {
        try
        {
            var result = await _bomGroupService.GetBomGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound("BOM组主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取BOM组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <param name="bomGroupCategory">BOM组类别（字典 logistics_manufacturing_bom_group_category；为空则返回全部启用组）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:group:query", "BOM组主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBomGroupOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] int? bomGroupCategory = null)
    {
        try
        {
            var result = await _bomGroupService.GetBomGroupOptionsAsync(plantCode, keyword, bomGroupCategory);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建BOM组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>BOM组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:group:create", "创建BOM组主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateBomGroupAsync([FromBody] TaktBomGroupCreateDto dto)
    {
        try
        {
            var result = await _bomGroupService.CreateBomGroupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新BOM组主数据
    /// </summary>
    /// <param name="id">BOM组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>BOM组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:group:update", "更新BOM组主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBomGroupAsync(long id, [FromBody] TaktBomGroupUpdateDto dto)
    {
        try
        {
            var result = await _bomGroupService.UpdateBomGroupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除BOM组主数据
    /// </summary>
    /// <param name="id">BOM组主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:group:delete", "删除BOM组主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBomGroupByIdAsync(long id)
    {
        try
        {
            await _bomGroupService.DeleteBomGroupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除BOM组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:group:delete", "批量删除BOM组主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBomGroupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _bomGroupService.DeleteBomGroupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新BOM组主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>BOM组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:group:update", "更新BOM组主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateBomGroupStatusAsync([FromBody] TaktBomGroupStatusDto dto)
    {
        try
        {
            var result = await _bomGroupService.UpdateBomGroupStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新BOM组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>BOM组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:group:update", "更新BOM组主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateBomGroupSortAsync([FromBody] TaktBomGroupSortDto dto)
    {
        try
        {
            var result = await _bomGroupService.UpdateBomGroupSortAsync(dto);
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
    [TaktPermission("logistics:manufacturing:bom:group:import", "获取BOM组主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetBomGroupTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _bomGroupService.GetBomGroupTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入BOM组主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:bom:group:import", "导入BOM组主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBomGroupAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _bomGroupService.ImportBomGroupAsync(stream, sheetName);
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
    /// 导出BOM组主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:group:export", "导出BOM组主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBomGroupAsync([FromQuery] TaktBomGroupQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomGroupService.ExportBomGroupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
