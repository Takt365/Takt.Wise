// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSellerMaterialsController.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：销售商物料控制器
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
/// 销售商物料控制器
/// 提供销售商物料的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售商物料")]
public class TaktSellerMaterialsController : TaktControllerBase
{
    private readonly ITaktSellerMaterialService _sellerMaterialService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sellerMaterialService">销售商物料服务</param>
    public TaktSellerMaterialsController(ITaktSellerMaterialService sellerMaterialService)
    {
        _sellerMaterialService = sellerMaterialService;
    }

    /// <summary>
    /// 获取销售商物料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:seller:material:list", "销售商物料列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSellerMaterialListAsync([FromQuery] TaktSellerMaterialQueryDto queryDto)
    {
        try
        {
            var result = await _sellerMaterialService.GetSellerMaterialListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售商物料
    /// </summary>
    /// <param name="id">销售商物料ID</param>
    /// <returns>销售商物料DTO</returns>
    [TaktPermission("logistics:sales:seller:material:query", "销售商物料详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSellerMaterialByIdAsync(long id)
    {
        try
        {
            var result = await _sellerMaterialService.GetSellerMaterialByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售商物料不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售商物料选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:seller:material:query", "销售商物料选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSellerMaterialOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _sellerMaterialService.GetSellerMaterialOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售商物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售商物料DTO</returns>
    [TaktPermission("logistics:sales:seller:material:create", "创建销售商物料")]
    [HttpPost]
    public async Task<IActionResult> CreateSellerMaterialAsync([FromBody] TaktSellerMaterialCreateDto dto)
    {
        try
        {
            var result = await _sellerMaterialService.CreateSellerMaterialAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售商物料
    /// </summary>
    /// <param name="id">销售商物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售商物料DTO</returns>
    [TaktPermission("logistics:sales:seller:material:update", "更新销售商物料")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSellerMaterialAsync(long id, [FromBody] TaktSellerMaterialUpdateDto dto)
    {
        try
        {
            var result = await _sellerMaterialService.UpdateSellerMaterialAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售商物料
    /// </summary>
    /// <param name="id">销售商物料ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:seller:material:delete", "删除销售商物料")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSellerMaterialByIdAsync(long id)
    {
        try
        {
            await _sellerMaterialService.DeleteSellerMaterialByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售商物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:seller:material:delete", "批量删除销售商物料")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSellerMaterialBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _sellerMaterialService.DeleteSellerMaterialBatchAsync(ids);
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
    [TaktPermission("logistics:sales:seller:material:import", "获取销售商物料导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSellerMaterialTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _sellerMaterialService.GetSellerMaterialTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售商物料
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:seller:material:import", "导入销售商物料")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSellerMaterialAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _sellerMaterialService.ImportSellerMaterialAsync(stream, sheetName);
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
    /// 导出销售商物料
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:seller:material:export", "导出销售商物料")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSellerMaterialAsync([FromQuery] TaktSellerMaterialQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _sellerMaterialService.ExportSellerMaterialAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
