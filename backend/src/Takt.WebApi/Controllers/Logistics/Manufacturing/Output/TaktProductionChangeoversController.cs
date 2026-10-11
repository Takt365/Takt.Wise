// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktProductionChangeoversController.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：生产切换记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services.Logistics.Manufacturing.Output;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Output;

/// <summary>
/// 生产切换记录控制器
/// 提供生产切换记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "生产切换记录")]
public class TaktProductionChangeoversController : TaktControllerBase
{
    private readonly ITaktProductionChangeoverService _productionChangeoverService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionChangeoverService">生产切换记录服务</param>
    public TaktProductionChangeoversController(ITaktProductionChangeoverService productionChangeoverService)
    {
        _productionChangeoverService = productionChangeoverService;
    }

    /// <summary>
    /// 获取生产切换记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:output:production:changeover:list", "生产切换记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetProductionChangeoverListAsync([FromQuery] TaktProductionChangeoverQueryDto queryDto)
    {
        try
        {
            var result = await _productionChangeoverService.GetProductionChangeoverListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取生产切换记录
    /// </summary>
    /// <param name="id">生产切换记录ID</param>
    /// <returns>生产切换记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:production:changeover:query", "生产切换记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductionChangeoverByIdAsync(long id)
    {
        try
        {
            var result = await _productionChangeoverService.GetProductionChangeoverByIdAsync(id);
            if (result == null)
            {
                return NotFound("生产切换记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取生产切换记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:output:production:changeover:query", "生产切换记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetProductionChangeoverOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _productionChangeoverService.GetProductionChangeoverOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建生产切换记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>生产切换记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:production:changeover:create", "创建生产切换记录")]
    [HttpPost]
    public async Task<IActionResult> CreateProductionChangeoverAsync([FromBody] TaktProductionChangeoverCreateDto dto)
    {
        try
        {
            var result = await _productionChangeoverService.CreateProductionChangeoverAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产切换记录
    /// </summary>
    /// <param name="id">生产切换记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>生产切换记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:production:changeover:update", "更新生产切换记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductionChangeoverAsync(long id, [FromBody] TaktProductionChangeoverUpdateDto dto)
    {
        try
        {
            var result = await _productionChangeoverService.UpdateProductionChangeoverAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除生产切换记录
    /// </summary>
    /// <param name="id">生产切换记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:production:changeover:delete", "删除生产切换记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductionChangeoverByIdAsync(long id)
    {
        try
        {
            await _productionChangeoverService.DeleteProductionChangeoverByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除生产切换记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:production:changeover:delete", "批量删除生产切换记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteProductionChangeoverBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _productionChangeoverService.DeleteProductionChangeoverBatchAsync(ids);
            return Success("删除成功");
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
    [TaktPermission("logistics:manufacturing:output:production:changeover:import", "获取生产切换记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetProductionChangeoverTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _productionChangeoverService.GetProductionChangeoverTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入生产切换记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:output:production:changeover:import", "导入生产切换记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportProductionChangeoverAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _productionChangeoverService.ImportProductionChangeoverAsync(stream, sheetName);
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
    /// 导出生产切换记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:output:production:changeover:export", "导出生产切换记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportProductionChangeoverAsync([FromQuery] TaktProductionChangeoverQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _productionChangeoverService.ExportProductionChangeoverAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
