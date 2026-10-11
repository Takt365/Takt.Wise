// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcGroupsController.cs
// 创建时间：2026-07-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变组主数据控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变组主数据控制器
/// 提供设变组主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变组主数据")]
public class TaktEcGroupsController : TaktControllerBase
{
    private readonly ITaktEcGroupService _ecGroupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecGroupService">设变组主数据服务</param>
    public TaktEcGroupsController(ITaktEcGroupService ecGroupService)
    {
        _ecGroupService = ecGroupService;
    }

    /// <summary>
    /// 获取设变组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:ec:group:list", "设变组主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcGroupListAsync([FromQuery] TaktEcGroupQueryDto queryDto)
    {
        try
        {
            var result = await _ecGroupService.GetEcGroupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设变组主数据
    /// </summary>
    /// <param name="id">设变组主数据ID</param>
    /// <returns>设变组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:ec:group:query", "设变组主数据详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEcGroupByIdAsync(long id)
    {
        try
        {
            var result = await _ecGroupService.GetEcGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound("设变组主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设变组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="keyword">搜索关键字（可选）</param>
    /// <param name="ecGroupCategory">设变组类别（可选；部门编码，多个逗号分隔）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:ec:group:query", "设变组主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEcGroupOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null, [FromQuery] string? ecGroupCategory = null)
    {
        try
        {
            var result = await _ecGroupService.GetEcGroupOptionsAsync(plantCode, keyword, ecGroupCategory);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设变组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设变组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:ec:group:create", "创建设变组主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateEcGroupAsync([FromBody] TaktEcGroupCreateDto dto)
    {
        try
        {
            var result = await _ecGroupService.CreateEcGroupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变组主数据
    /// </summary>
    /// <param name="id">设变组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设变组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:ec:group:update", "更新设变组主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEcGroupAsync(long id, [FromBody] TaktEcGroupUpdateDto dto)
    {
        try
        {
            var result = await _ecGroupService.UpdateEcGroupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设变组主数据
    /// </summary>
    /// <param name="id">设变组主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:ec:group:delete", "删除设变组主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEcGroupByIdAsync(long id)
    {
        try
        {
            await _ecGroupService.DeleteEcGroupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设变组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:ec:group:delete", "批量删除设变组主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEcGroupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ecGroupService.DeleteEcGroupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变组主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>设变组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:ec:group:update", "更新设变组主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEcGroupStatusAsync([FromBody] TaktEcGroupStatusDto dto)
    {
        try
        {
            var result = await _ecGroupService.UpdateEcGroupStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设变组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>设变组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:ec:group:update", "更新设变组主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateEcGroupSortAsync([FromBody] TaktEcGroupSortDto dto)
    {
        try
        {
            var result = await _ecGroupService.UpdateEcGroupSortAsync(dto);
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
    [TaktPermission("logistics:manufacturing:engineering:change:ec:group:import", "获取设变组主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEcGroupTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ecGroupService.GetEcGroupTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设变组主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:ec:group:import", "导入设变组主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEcGroupAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ecGroupService.ImportEcGroupAsync(stream, sheetName);
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
    /// 导出设变组主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:engineering:change:ec:group:export", "导出设变组主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcGroupAsync([FromQuery] TaktEcGroupQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ecGroupService.ExportEcGroupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
