// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialsController.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：物料清单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// 物料清单控制器
/// 提供物料清单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料清单")]
public class TaktBillOfMaterialsController : TaktControllerBase
{
    private readonly ITaktBillOfMaterialService _billOfMaterialService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialService">物料清单服务</param>
    public TaktBillOfMaterialsController(ITaktBillOfMaterialService billOfMaterialService)
    {
        _billOfMaterialService = billOfMaterialService;
    }

    /// <summary>
    /// 获取物料清单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:list", "物料清单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBillOfMaterialListAsync([FromQuery] TaktBillOfMaterialQueryDto queryDto)
    {
        try
        {
            var result = await _billOfMaterialService.GetBillOfMaterialListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取物料清单
    /// </summary>
    /// <param name="id">物料清单ID</param>
    /// <returns>物料清单DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:query", "物料清单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBillOfMaterialByIdAsync(long id)
    {
        try
        {
            var result = await _billOfMaterialService.GetBillOfMaterialByIdAsync(id);
            if (result == null)
            {
                return NotFound("物料清单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料清单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:query", "物料清单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBillOfMaterialOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _billOfMaterialService.GetBillOfMaterialOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建物料清单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>物料清单DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:create", "创建物料清单")]
    [HttpPost]
    public async Task<IActionResult> CreateBillOfMaterialAsync([FromBody] TaktBillOfMaterialCreateDto dto)
    {
        try
        {
            var result = await _billOfMaterialService.CreateBillOfMaterialAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料清单
    /// </summary>
    /// <param name="id">物料清单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>物料清单DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:update", "更新物料清单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBillOfMaterialAsync(long id, [FromBody] TaktBillOfMaterialUpdateDto dto)
    {
        try
        {
            var result = await _billOfMaterialService.UpdateBillOfMaterialAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除物料清单
    /// </summary>
    /// <param name="id">物料清单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:delete", "删除物料清单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBillOfMaterialByIdAsync(long id)
    {
        try
        {
            await _billOfMaterialService.DeleteBillOfMaterialByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除物料清单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:delete", "批量删除物料清单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBillOfMaterialBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _billOfMaterialService.DeleteBillOfMaterialBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料清单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>物料清单DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:update", "更新物料清单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateBillOfMaterialStatusAsync([FromBody] TaktBillOfMaterialStatusDto dto)
    {
        try
        {
            var result = await _billOfMaterialService.UpdateBillOfMaterialStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料清单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>物料清单DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:update", "更新物料清单排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateBillOfMaterialSortAsync([FromBody] TaktBillOfMaterialSortDto dto)
    {
        try
        {
            var result = await _billOfMaterialService.UpdateBillOfMaterialSortAsync(dto);
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
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:import", "获取物料清单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetBillOfMaterialTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _billOfMaterialService.GetBillOfMaterialTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入物料清单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:import", "导入物料清单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBillOfMaterialAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _billOfMaterialService.ImportBillOfMaterialAsync(stream, sheetName);
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
    /// 导出物料清单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:export", "导出物料清单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBillOfMaterialAsync([FromQuery] TaktBillOfMaterialQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _billOfMaterialService.ExportBillOfMaterialAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
