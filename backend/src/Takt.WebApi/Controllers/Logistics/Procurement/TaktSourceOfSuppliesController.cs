// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktSourceOfSuppliesController.cs
// 创建时间：2026-07-21
// 创建人：Takt365(Cursor AI)
// 功能描述：货源清单清单控制器
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
/// 货源清单清单控制器
/// 提供货源清单清单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "货源清单清单")]
public class TaktSourceOfSuppliesController : TaktControllerBase
{
    private readonly ITaktSourceOfSupplyService _sourceOfSupplyService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sourceOfSupplyService">货源清单清单服务</param>
    public TaktSourceOfSuppliesController(ITaktSourceOfSupplyService sourceOfSupplyService)
    {
        _sourceOfSupplyService = sourceOfSupplyService;
    }

    /// <summary>
    /// 获取货源清单清单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:source:of:supply:list", "货源清单清单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSourceOfSupplyListAsync([FromQuery] TaktSourceOfSupplyQueryDto queryDto)
    {
        try
        {
            var result = await _sourceOfSupplyService.GetSourceOfSupplyListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取货源清单清单
    /// </summary>
    /// <param name="id">货源清单清单ID</param>
    /// <returns>货源清单清单DTO</returns>
    [TaktPermission("logistics:procurement:source:of:supply:query", "货源清单清单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSourceOfSupplyByIdAsync(long id)
    {
        try
        {
            var result = await _sourceOfSupplyService.GetSourceOfSupplyByIdAsync(id);
            if (result == null)
            {
                return NotFound("货源清单清单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取货源清单清单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:source:of:supply:query", "货源清单清单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSourceOfSupplyOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sourceOfSupplyService.GetSourceOfSupplyOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建货源清单清单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>货源清单清单DTO</returns>
    [TaktPermission("logistics:procurement:source:of:supply:create", "创建货源清单清单")]
    [HttpPost]
    public async Task<IActionResult> CreateSourceOfSupplyAsync([FromBody] TaktSourceOfSupplyCreateDto dto)
    {
        try
        {
            var result = await _sourceOfSupplyService.CreateSourceOfSupplyAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新货源清单清单
    /// </summary>
    /// <param name="id">货源清单清单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>货源清单清单DTO</returns>
    [TaktPermission("logistics:procurement:source:of:supply:update", "更新货源清单清单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSourceOfSupplyAsync(long id, [FromBody] TaktSourceOfSupplyUpdateDto dto)
    {
        try
        {
            var result = await _sourceOfSupplyService.UpdateSourceOfSupplyAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除货源清单清单
    /// </summary>
    /// <param name="id">货源清单清单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:source:of:supply:delete", "删除货源清单清单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSourceOfSupplyByIdAsync(long id)
    {
        try
        {
            await _sourceOfSupplyService.DeleteSourceOfSupplyByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除货源清单清单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:source:of:supply:delete", "批量删除货源清单清单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSourceOfSupplyBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sourceOfSupplyService.DeleteSourceOfSupplyBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新货源清单清单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>货源清单清单DTO</returns>
    [TaktPermission("logistics:procurement:source:of:supply:update", "更新货源清单清单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSourceOfSupplyStatusAsync([FromBody] TaktSourceOfSupplyStatusDto dto)
    {
        try
        {
            var result = await _sourceOfSupplyService.UpdateSourceOfSupplyStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新货源清单清单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>货源清单清单DTO</returns>
    [TaktPermission("logistics:procurement:source:of:supply:update", "更新货源清单清单排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSourceOfSupplySortAsync([FromBody] TaktSourceOfSupplySortDto dto)
    {
        try
        {
            var result = await _sourceOfSupplyService.UpdateSourceOfSupplySortAsync(dto);
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
    [TaktPermission("logistics:procurement:source:of:supply:import", "获取货源清单清单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSourceOfSupplyTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sourceOfSupplyService.GetSourceOfSupplyTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入货源清单清单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:source:of:supply:import", "导入货源清单清单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSourceOfSupplyAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sourceOfSupplyService.ImportSourceOfSupplyAsync(stream, sheetName);
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
    /// 导出货源清单清单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:source:of:supply:export", "导出货源清单清单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSourceOfSupplyAsync([FromQuery] TaktSourceOfSupplyQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sourceOfSupplyService.ExportSourceOfSupplyAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
