// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktVendorsController.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Cursor AI)
// 功能描述：经销商信息控制器
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
/// 经销商信息控制器
/// 提供经销商信息的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "经销商信息")]
public class TaktVendorsController : TaktControllerBase
{
    private readonly ITaktVendorService _vendorService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="vendorService">经销商信息服务</param>
    public TaktVendorsController(ITaktVendorService vendorService)
    {
        _vendorService = vendorService;
    }

    /// <summary>
    /// 获取经销商信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:vendor:list", "经销商信息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetVendorListAsync([FromQuery] TaktVendorQueryDto queryDto)
    {
        try
        {
            var result = await _vendorService.GetVendorListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取经销商信息
    /// </summary>
    /// <param name="id">经销商信息ID</param>
    /// <returns>经销商信息DTO</returns>
    [TaktPermission("logistics:procurement:vendor:query", "经销商信息详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetVendorByIdAsync(long id)
    {
        try
        {
            var result = await _vendorService.GetVendorByIdAsync(id);
            if (result == null)
            {
                return NotFound("经销商信息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取经销商信息选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:vendor:query", "经销商信息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetVendorOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _vendorService.GetVendorOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建经销商信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>经销商信息DTO</returns>
    [TaktPermission("logistics:procurement:vendor:create", "创建经销商信息")]
    [HttpPost]
    public async Task<IActionResult> CreateVendorAsync([FromBody] TaktVendorCreateDto dto)
    {
        try
        {
            var result = await _vendorService.CreateVendorAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新经销商信息
    /// </summary>
    /// <param name="id">经销商信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>经销商信息DTO</returns>
    [TaktPermission("logistics:procurement:vendor:update", "更新经销商信息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVendorAsync(long id, [FromBody] TaktVendorUpdateDto dto)
    {
        try
        {
            var result = await _vendorService.UpdateVendorAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除经销商信息
    /// </summary>
    /// <param name="id">经销商信息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:vendor:delete", "删除经销商信息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVendorByIdAsync(long id)
    {
        try
        {
            await _vendorService.DeleteVendorByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除经销商信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:vendor:delete", "批量删除经销商信息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteVendorBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _vendorService.DeleteVendorBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新经销商信息状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>经销商信息DTO</returns>
    [TaktPermission("logistics:procurement:vendor:update", "更新经销商信息状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateVendorStatusAsync([FromBody] TaktVendorStatusDto dto)
    {
        try
        {
            var result = await _vendorService.UpdateVendorStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新经销商信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>经销商信息DTO</returns>
    [TaktPermission("logistics:procurement:vendor:update", "更新经销商信息排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateVendorSortAsync([FromBody] TaktVendorSortDto dto)
    {
        try
        {
            var result = await _vendorService.UpdateVendorSortAsync(dto);
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
    [TaktPermission("logistics:procurement:vendor:import", "获取经销商信息导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetVendorTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _vendorService.GetVendorTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入经销商信息
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:vendor:import", "导入经销商信息")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportVendorAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _vendorService.ImportVendorAsync(stream, sheetName);
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
    /// 导出经销商信息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:vendor:export", "导出经销商信息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportVendorAsync([FromQuery] TaktVendorQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _vendorService.ExportVendorAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
