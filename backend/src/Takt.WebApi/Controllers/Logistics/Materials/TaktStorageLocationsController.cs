// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktStorageLocationsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：库位主数据控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 库位主数据控制器
/// 提供库位主数据的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "库位主数据")]
public class TaktStorageLocationsController : TaktControllerBase
{
    private readonly ITaktStorageLocationService _storageLocationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="storageLocationService">库位主数据服务</param>
    public TaktStorageLocationsController(ITaktStorageLocationService storageLocationService)
    {
        _storageLocationService = storageLocationService;
    }

    /// <summary>
    /// 获取库位主数据列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:warehouse:list", "库位主数据列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetStorageLocationListAsync([FromQuery] TaktStorageLocationQueryDto queryDto)
    {
        try
        {
            var result = await _storageLocationService.GetStorageLocationListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取库位主数据
    /// </summary>
    /// <param name="id">库位主数据ID</param>
    /// <returns>库位主数据DTO</returns>
    [TaktPermission("logistics:materials:warehouse:query", "库位主数据详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStorageLocationByIdAsync(long id)
    {
        try
        {
            var result = await _storageLocationService.GetStorageLocationByIdAsync(id);
            if (result == null)
            {
                return NotFound("库位主数据不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取库位主数据选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:warehouse:query", "库位主数据选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetStorageLocationOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _storageLocationService.GetStorageLocationOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建库位主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>库位主数据DTO</returns>
    [TaktPermission("logistics:materials:warehouse:create", "创建库位主数据")]
    [HttpPost]
    public async Task<IActionResult> CreateStorageLocationAsync([FromBody] TaktStorageLocationCreateDto dto)
    {
        try
        {
            var result = await _storageLocationService.CreateStorageLocationAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新库位主数据
    /// </summary>
    /// <param name="id">库位主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>库位主数据DTO</returns>
    [TaktPermission("logistics:materials:warehouse:update", "更新库位主数据")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStorageLocationAsync(long id, [FromBody] TaktStorageLocationUpdateDto dto)
    {
        try
        {
            var result = await _storageLocationService.UpdateStorageLocationAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除库位主数据
    /// </summary>
    /// <param name="id">库位主数据ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:warehouse:delete", "删除库位主数据")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStorageLocationByIdAsync(long id)
    {
        try
        {
            await _storageLocationService.DeleteStorageLocationByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除库位主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:warehouse:delete", "批量删除库位主数据")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteStorageLocationBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _storageLocationService.DeleteStorageLocationBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新库位主数据状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>库位主数据DTO</returns>
    [TaktPermission("logistics:materials:warehouse:update", "更新库位主数据状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateStorageLocationStatusAsync([FromBody] TaktStorageLocationStatusDto dto)
    {
        try
        {
            var result = await _storageLocationService.UpdateStorageLocationStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新库位主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>库位主数据DTO</returns>
    [TaktPermission("logistics:materials:warehouse:update", "更新库位主数据排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateStorageLocationSortAsync([FromBody] TaktStorageLocationSortDto dto)
    {
        try
        {
            var result = await _storageLocationService.UpdateStorageLocationSortAsync(dto);
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
    [TaktPermission("logistics:materials:warehouse:import", "获取库位主数据导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetStorageLocationTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _storageLocationService.GetStorageLocationTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入库位主数据
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:warehouse:import", "导入库位主数据")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportStorageLocationAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _storageLocationService.ImportStorageLocationAsync(stream, sheetName);
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
    /// 导出库位主数据
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:warehouse:export", "导出库位主数据")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportStorageLocationAsync([FromQuery] TaktStorageLocationQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _storageLocationService.ExportStorageLocationAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
