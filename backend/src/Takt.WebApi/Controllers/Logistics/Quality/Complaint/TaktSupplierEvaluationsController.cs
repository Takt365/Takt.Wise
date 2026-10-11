// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationsController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：供应商评价考核控制器
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
/// 供应商评价考核控制器
/// 提供供应商评价考核的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "供应商评价考核")]
public class TaktSupplierEvaluationsController : TaktControllerBase
{
    private readonly ITaktSupplierEvaluationService _supplierEvaluationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="supplierEvaluationService">供应商评价考核服务</param>
    public TaktSupplierEvaluationsController(ITaktSupplierEvaluationService supplierEvaluationService)
    {
        _supplierEvaluationService = supplierEvaluationService;
    }

    /// <summary>
    /// 获取供应商评价考核列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:list", "供应商评价考核列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSupplierEvaluationListAsync([FromQuery] TaktSupplierEvaluationQueryDto queryDto)
    {
        try
        {
            var result = await _supplierEvaluationService.GetSupplierEvaluationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取供应商评价考核
    /// </summary>
    /// <param name="id">供应商评价考核ID</param>
    /// <returns>供应商评价考核DTO</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:query", "供应商评价考核详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplierEvaluationByIdAsync(long id)
    {
        try
        {
            var result = await _supplierEvaluationService.GetSupplierEvaluationByIdAsync(id);
            if (result == null)
            {
                return NotFound("供应商评价考核不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取供应商评价考核选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:query", "供应商评价考核选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSupplierEvaluationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _supplierEvaluationService.GetSupplierEvaluationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建供应商评价考核
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>供应商评价考核DTO</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:create", "创建供应商评价考核")]
    [HttpPost]
    public async Task<IActionResult> CreateSupplierEvaluationAsync([FromBody] TaktSupplierEvaluationCreateDto dto)
    {
        try
        {
            var result = await _supplierEvaluationService.CreateSupplierEvaluationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新供应商评价考核
    /// </summary>
    /// <param name="id">供应商评价考核ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>供应商评价考核DTO</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:update", "更新供应商评价考核")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSupplierEvaluationAsync(long id, [FromBody] TaktSupplierEvaluationUpdateDto dto)
    {
        try
        {
            var result = await _supplierEvaluationService.UpdateSupplierEvaluationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除供应商评价考核
    /// </summary>
    /// <param name="id">供应商评价考核ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:delete", "删除供应商评价考核")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplierEvaluationByIdAsync(long id)
    {
        try
        {
            await _supplierEvaluationService.DeleteSupplierEvaluationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除供应商评价考核
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:delete", "批量删除供应商评价考核")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSupplierEvaluationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _supplierEvaluationService.DeleteSupplierEvaluationBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新供应商评价考核状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>供应商评价考核DTO</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:update", "更新供应商评价考核状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSupplierEvaluationStatusAsync([FromBody] TaktSupplierEvaluationStatusDto dto)
    {
        try
        {
            var result = await _supplierEvaluationService.UpdateSupplierEvaluationStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新供应商评价考核排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>供应商评价考核DTO</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:update", "更新供应商评价考核排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSupplierEvaluationSortAsync([FromBody] TaktSupplierEvaluationSortDto dto)
    {
        try
        {
            var result = await _supplierEvaluationService.UpdateSupplierEvaluationSortAsync(dto);
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
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:import", "获取供应商评价考核导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSupplierEvaluationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _supplierEvaluationService.GetSupplierEvaluationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入供应商评价考核
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:import", "导入供应商评价考核")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSupplierEvaluationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _supplierEvaluationService.ImportSupplierEvaluationAsync(stream, sheetName);
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
    /// 导出供应商评价考核
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:complaint:supplier:evaluation:export", "导出供应商评价考核")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSupplierEvaluationAsync([FromQuery] TaktSupplierEvaluationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _supplierEvaluationService.ExportSupplierEvaluationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
