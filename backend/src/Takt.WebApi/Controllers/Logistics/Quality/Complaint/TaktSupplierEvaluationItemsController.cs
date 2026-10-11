// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationItemsController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：供应商评价考核项目明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Application.Services.Logistics.Quality.Complaint;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Complaint;

/// <summary>
/// 供应商评价考核项目明细控制器
/// 提供供应商评价考核项目明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "供应商评价考核项目明细")]
public class TaktSupplierEvaluationItemsController : TaktControllerBase
{
    private readonly ITaktSupplierEvaluationItemService _supplierEvaluationItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="supplierEvaluationItemService">供应商评价考核项目明细服务</param>
    public TaktSupplierEvaluationItemsController(ITaktSupplierEvaluationItemService supplierEvaluationItemService)
    {
        _supplierEvaluationItemService = supplierEvaluationItemService;
    }

    /// <summary>
    /// 获取供应商评价考核项目明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:list", "供应商评价考核项目明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSupplierEvaluationItemListAsync([FromQuery] TaktSupplierEvaluationItemQueryDto queryDto)
    {
        try
        {
            var result = await _supplierEvaluationItemService.GetSupplierEvaluationItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取供应商评价考核项目明细
    /// </summary>
    /// <param name="id">供应商评价考核项目明细ID</param>
    /// <returns>供应商评价考核项目明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:query", "供应商评价考核项目明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplierEvaluationItemByIdAsync(long id)
    {
        try
        {
            var result = await _supplierEvaluationItemService.GetSupplierEvaluationItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("供应商评价考核项目明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取供应商评价考核项目明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:query", "供应商评价考核项目明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSupplierEvaluationItemOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _supplierEvaluationItemService.GetSupplierEvaluationItemOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建供应商评价考核项目明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>供应商评价考核项目明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:create", "创建供应商评价考核项目明细")]
    [HttpPost]
    public async Task<IActionResult> CreateSupplierEvaluationItemAsync([FromBody] TaktSupplierEvaluationItemCreateDto dto)
    {
        try
        {
            var result = await _supplierEvaluationItemService.CreateSupplierEvaluationItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新供应商评价考核项目明细
    /// </summary>
    /// <param name="id">供应商评价考核项目明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>供应商评价考核项目明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:update", "更新供应商评价考核项目明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSupplierEvaluationItemAsync(long id, [FromBody] TaktSupplierEvaluationItemUpdateDto dto)
    {
        try
        {
            var result = await _supplierEvaluationItemService.UpdateSupplierEvaluationItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除供应商评价考核项目明细
    /// </summary>
    /// <param name="id">供应商评价考核项目明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:delete", "删除供应商评价考核项目明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplierEvaluationItemByIdAsync(long id)
    {
        try
        {
            await _supplierEvaluationItemService.DeleteSupplierEvaluationItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除供应商评价考核项目明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:delete", "批量删除供应商评价考核项目明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSupplierEvaluationItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _supplierEvaluationItemService.DeleteSupplierEvaluationItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新供应商评价考核项目明细状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>供应商评价考核项目明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:update", "更新供应商评价考核项目明细状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSupplierEvaluationItemStatusAsync([FromBody] TaktSupplierEvaluationItemStatusDto dto)
    {
        try
        {
            var result = await _supplierEvaluationItemService.UpdateSupplierEvaluationItemStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新供应商评价考核项目明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>供应商评价考核项目明细DTO</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:update", "更新供应商评价考核项目明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateSupplierEvaluationItemObsoleteAsync([FromBody] TaktSupplierEvaluationItemObsoleteDto dto)
    {
        try
        {
            var result = await _supplierEvaluationItemService.UpdateSupplierEvaluationItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:import", "获取供应商评价考核项目明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSupplierEvaluationItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _supplierEvaluationItemService.GetSupplierEvaluationItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入供应商评价考核项目明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:import", "导入供应商评价考核项目明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSupplierEvaluationItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _supplierEvaluationItemService.ImportSupplierEvaluationItemAsync(stream, sheetName);
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
    /// 导出供应商评价考核项目明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:export", "导出供应商评价考核项目明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSupplierEvaluationItemAsync([FromQuery] TaktSupplierEvaluationItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _supplierEvaluationItemService.ExportSupplierEvaluationItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
