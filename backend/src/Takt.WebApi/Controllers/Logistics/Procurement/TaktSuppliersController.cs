// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktSuppliersController.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：供货商信息控制器
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
/// 供货商信息控制器
/// 提供供货商信息的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "供货商信息")]
public class TaktSuppliersController : TaktControllerBase
{
    private readonly ITaktSupplierService _supplierService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="supplierService">供货商信息服务</param>
    public TaktSuppliersController(ITaktSupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    /// <summary>
    /// 获取供货商信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:supplier:list", "供货商信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSupplierListAsync([FromQuery] TaktSupplierQueryDto queryDto)
    {
        try
        {
            var result = await _supplierService.GetSupplierListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取供货商信息
    /// </summary>
    /// <param name="id">供货商信息ID</param>
    /// <returns>供货商信息DTO</returns>
    [TaktPermission("logistics:procurement:supplier:query", "供货商信息详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplierByIdAsync(long id)
    {
        try
        {
            var result = await _supplierService.GetSupplierByIdAsync(id);
            if (result == null)
            {
                return NotFound("供货商信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取供货商信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:supplier:query", "供货商信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSupplierOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _supplierService.GetSupplierOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建供货商信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>供货商信息DTO</returns>
    [TaktPermission("logistics:procurement:supplier:create", "创建供货商信息")]
    [HttpPost]
    public async Task<IActionResult> CreateSupplierAsync([FromBody] TaktSupplierCreateDto dto)
    {
        try
        {
            var result = await _supplierService.CreateSupplierAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新供货商信息
    /// </summary>
    /// <param name="id">供货商信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>供货商信息DTO</returns>
    [TaktPermission("logistics:procurement:supplier:update", "更新供货商信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSupplierAsync(long id, [FromBody] TaktSupplierUpdateDto dto)
    {
        try
        {
            var result = await _supplierService.UpdateSupplierAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除供货商信息
    /// </summary>
    /// <param name="id">供货商信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:supplier:delete", "删除供货商信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplierByIdAsync(long id)
    {
        try
        {
            await _supplierService.DeleteSupplierByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除供货商信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:supplier:delete", "批量删除供货商信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSupplierBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _supplierService.DeleteSupplierBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新供货商信息状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>供货商信息DTO</returns>
    [TaktPermission("logistics:procurement:supplier:update", "更新供货商信息状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSupplierStatusAsync([FromBody] TaktSupplierStatusDto dto)
    {
        try
        {
            var result = await _supplierService.UpdateSupplierStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新供货商信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>供货商信息DTO</returns>
    [TaktPermission("logistics:procurement:supplier:update", "更新供货商信息排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSupplierSortAsync([FromBody] TaktSupplierSortDto dto)
    {
        try
        {
            var result = await _supplierService.UpdateSupplierSortAsync(dto);
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
    [TaktPermission("logistics:procurement:supplier:import", "获取供货商信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSupplierTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _supplierService.GetSupplierTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入供货商信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:supplier:import", "导入供货商信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSupplierAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _supplierService.ImportSupplierAsync(stream, sheetName);
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
    /// 导出供货商信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:supplier:export", "导出供货商信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSupplierAsync([FromQuery] TaktSupplierQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _supplierService.ExportSupplierAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
