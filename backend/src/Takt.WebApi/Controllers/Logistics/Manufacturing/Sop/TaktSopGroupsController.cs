// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Sop
// 文件名称：TaktSopGroupsController.cs
// 创建时间：2026-10-08
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP组主数据控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Application.Services.Logistics.Manufacturing.Sop;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP组主数据控制器
/// 提供SOP组主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "SOP组主数据")]
public class TaktSopGroupsController : TaktControllerBase
{
    private readonly ITaktSopGroupService _sopGroupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopGroupService">SOP组主数据服务</param>
    public TaktSopGroupsController(ITaktSopGroupService sopGroupService)
    {
        _sopGroupService = sopGroupService;
    }

    /// <summary>
    /// 获取SOP组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:sop:group:list", "SOP组主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSopGroupListAsync([FromQuery] TaktSopGroupQueryDto queryDto)
    {
        try
        {
            var result = await _sopGroupService.GetSopGroupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取SOP组主数据
    /// </summary>
    /// <param name="id">SOP组主数据ID</param>
    /// <returns>SOP组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:group:query", "SOP组主数据详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetSopGroupByIdAsync(long id)
    {
        try
        {
            var result = await _sopGroupService.GetSopGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound("SOP组主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取SOP组主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:sop:group:query", "SOP组主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSopGroupOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sopGroupService.GetSopGroupOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建SOP组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>SOP组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:group:create", "创建SOP组主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateSopGroupAsync([FromBody] TaktSopGroupCreateDto dto)
    {
        try
        {
            var result = await _sopGroupService.CreateSopGroupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP组主数据
    /// </summary>
    /// <param name="id">SOP组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>SOP组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:group:update", "更新SOP组主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSopGroupAsync(long id, [FromBody] TaktSopGroupUpdateDto dto)
    {
        try
        {
            var result = await _sopGroupService.UpdateSopGroupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除SOP组主数据
    /// </summary>
    /// <param name="id">SOP组主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:group:delete", "删除SOP组主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSopGroupByIdAsync(long id)
    {
        try
        {
            await _sopGroupService.DeleteSopGroupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除SOP组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:sop:group:delete", "批量删除SOP组主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSopGroupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sopGroupService.DeleteSopGroupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP组主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>SOP组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:group:update", "更新SOP组主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSopGroupStatusAsync([FromBody] TaktSopGroupStatusDto dto)
    {
        try
        {
            var result = await _sopGroupService.UpdateSopGroupStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新SOP组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>SOP组主数据DTO</returns>
    [TaktPermission("logistics:manufacturing:sop:group:update", "更新SOP组主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSopGroupSortAsync([FromBody] TaktSopGroupSortDto dto)
    {
        try
        {
            var result = await _sopGroupService.UpdateSopGroupSortAsync(dto);
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
    [TaktPermission("logistics:manufacturing:sop:group:import", "获取SOP组主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSopGroupTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sopGroupService.GetSopGroupTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入SOP组主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:sop:group:import", "导入SOP组主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSopGroupAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sopGroupService.ImportSopGroupAsync(stream, sheetName);
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
    /// 导出SOP组主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:sop:group:export", "导出SOP组主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSopGroupAsync([FromQuery] TaktSopGroupQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sopGroupService.ExportSopGroupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
