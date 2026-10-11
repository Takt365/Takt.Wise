// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Aps
// 文件名称：TaktApsGroupsController.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：APS组主数据控制器
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
/// APS组主数据控制器
/// 提供APS组主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "APS组主数据")]
public class TaktApsGroupsController : TaktControllerBase
{
    private readonly ITaktApsGroupService _apsGroupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsGroupService">APS组主数据服务</param>
    public TaktApsGroupsController(ITaktApsGroupService apsGroupService)
    {
        _apsGroupService = apsGroupService;
    }

    /// <summary>
    /// 获取APS组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:aps:group:list", "APS组主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetApsGroupListAsync([FromQuery] TaktApsGroupQueryDto queryDto)
    {
        try
        {
            var result = await _apsGroupService.GetApsGroupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取APS组主数据
    /// </summary>
    /// <param name="id">APS组主数据ID</param>
    /// <returns>APS组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:group:query", "APS组主数据详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetApsGroupByIdAsync(long id)
    {
        try
        {
            var result = await _apsGroupService.GetApsGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound("APS组主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取APS组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <param name="apsGroupCategory">APS组类别（可选；1=计划员 2=发布员 3=设备操作员 4=设备维护员）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:aps:group:query", "APS组主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetApsGroupOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] int? apsGroupCategory = null)
    {
        try
        {
            var result = await _apsGroupService.GetApsGroupOptionsAsync(plantCode, keyword, apsGroupCategory);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建APS组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>APS组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:group:create", "创建APS组主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateApsGroupAsync([FromBody] TaktApsGroupCreateDto dto)
    {
        try
        {
            var result = await _apsGroupService.CreateApsGroupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS组主数据
    /// </summary>
    /// <param name="id">APS组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>APS组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:group:update", "更新APS组主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApsGroupAsync(long id, [FromBody] TaktApsGroupUpdateDto dto)
    {
        try
        {
            var result = await _apsGroupService.UpdateApsGroupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除APS组主数据
    /// </summary>
    /// <param name="id">APS组主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:group:delete", "删除APS组主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApsGroupByIdAsync(long id)
    {
        try
        {
            await _apsGroupService.DeleteApsGroupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除APS组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:group:delete", "批量删除APS组主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteApsGroupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _apsGroupService.DeleteApsGroupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS组主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>APS组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:group:update", "更新APS组主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateApsGroupStatusAsync([FromBody] TaktApsGroupStatusDto dto)
    {
        try
        {
            var result = await _apsGroupService.UpdateApsGroupStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>APS组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:group:update", "更新APS组主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateApsGroupSortAsync([FromBody] TaktApsGroupSortDto dto)
    {
        try
        {
            var result = await _apsGroupService.UpdateApsGroupSortAsync(dto);
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
    [TaktPermission("logistics:manufacturing:aps:group:import", "获取APS组主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetApsGroupTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _apsGroupService.GetApsGroupTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入APS组主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:aps:group:import", "导入APS组主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportApsGroupAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _apsGroupService.ImportApsGroupAsync(stream, sheetName);
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
    /// 导出APS组主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:aps:group:export", "导出APS组主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportApsGroupAsync([FromQuery] TaktApsGroupQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _apsGroupService.ExportApsGroupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
