// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesGroupsController.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售组主数据控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Application.Services.Logistics.Sales;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Sales;

/// <summary>
/// 销售组主数据控制器
/// 提供销售组主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售组主数据")]
public class TaktSalesGroupsController : TaktControllerBase
{
    private readonly ITaktSalesGroupService _salesGroupService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesGroupService">销售组主数据服务</param>
    public TaktSalesGroupsController(ITaktSalesGroupService salesGroupService)
    {
        _salesGroupService = salesGroupService;
    }

    /// <summary>
    /// 获取销售组主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:group:list", "销售组主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesGroupListAsync([FromQuery] TaktSalesGroupQueryDto queryDto)
    {
        try
        {
            var result = await _salesGroupService.GetSalesGroupListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售组主数据
    /// </summary>
    /// <param name="id">销售组主数据ID</param>
    /// <returns>销售组主数据DTO</returns>
    [TaktPermission("logistics:sales:group:query", "销售组主数据详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalesGroupByIdAsync(long id)
    {
        try
        {
            var result = await _salesGroupService.GetSalesGroupByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售组主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售组主数据选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:group:query", "销售组主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesGroupOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _salesGroupService.GetSalesGroupOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售组主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售组主数据DTO</returns>
    [TaktPermission("logistics:sales:group:create", "创建销售组主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesGroupAsync([FromBody] TaktSalesGroupCreateDto dto)
    {
        try
        {
            var result = await _salesGroupService.CreateSalesGroupAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售组主数据
    /// </summary>
    /// <param name="id">销售组主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售组主数据DTO</returns>
    [TaktPermission("logistics:sales:group:update", "更新销售组主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesGroupAsync(long id, [FromBody] TaktSalesGroupUpdateDto dto)
    {
        try
        {
            var result = await _salesGroupService.UpdateSalesGroupAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售组主数据
    /// </summary>
    /// <param name="id">销售组主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:group:delete", "删除销售组主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesGroupByIdAsync(long id)
    {
        try
        {
            await _salesGroupService.DeleteSalesGroupByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售组主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:group:delete", "批量删除销售组主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesGroupBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesGroupService.DeleteSalesGroupBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售组主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>销售组主数据DTO</returns>
    [TaktPermission("logistics:sales:group:update", "更新销售组主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSalesGroupStatusAsync([FromBody] TaktSalesGroupStatusDto dto)
    {
        try
        {
            var result = await _salesGroupService.UpdateSalesGroupStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售组主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>销售组主数据DTO</returns>
    [TaktPermission("logistics:sales:group:update", "更新销售组主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSalesGroupSortAsync([FromBody] TaktSalesGroupSortDto dto)
    {
        try
        {
            var result = await _salesGroupService.UpdateSalesGroupSortAsync(dto);
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
    [TaktPermission("logistics:sales:group:import", "获取销售组主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesGroupTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesGroupService.GetSalesGroupTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售组主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:group:import", "导入销售组主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesGroupAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesGroupService.ImportSalesGroupAsync(stream, sheetName);
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
    /// 导出销售组主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:group:export", "导出销售组主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesGroupAsync([FromQuery] TaktSalesGroupQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesGroupService.ExportSalesGroupAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
