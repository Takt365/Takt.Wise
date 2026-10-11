// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialSubstitutesController.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM替代料控制器
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
/// BOM替代料控制器
/// 提供BOM替代料的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM替代料")]
public class TaktBillOfMaterialSubstitutesController : TaktControllerBase
{
    private readonly ITaktBillOfMaterialSubstituteService _billOfMaterialSubstituteService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialSubstituteService">BOM替代料服务</param>
    public TaktBillOfMaterialSubstitutesController(ITaktBillOfMaterialSubstituteService billOfMaterialSubstituteService)
    {
        _billOfMaterialSubstituteService = billOfMaterialSubstituteService;
    }

    /// <summary>
    /// 获取BOM替代料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:list", "BOM替代料列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBillOfMaterialSubstituteListAsync([FromQuery] TaktBillOfMaterialSubstituteQueryDto queryDto)
    {
        try
        {
            var result = await _billOfMaterialSubstituteService.GetBillOfMaterialSubstituteListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取BOM替代料
    /// </summary>
    /// <param name="id">BOM替代料ID</param>
    /// <returns>BOM替代料DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:query", "BOM替代料详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBillOfMaterialSubstituteByIdAsync(long id)
    {
        try
        {
            var result = await _billOfMaterialSubstituteService.GetBillOfMaterialSubstituteByIdAsync(id);
            if (result == null)
            {
                return NotFound("BOM替代料不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取BOM替代料选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:query", "BOM替代料选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBillOfMaterialSubstituteOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _billOfMaterialSubstituteService.GetBillOfMaterialSubstituteOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建BOM替代料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>BOM替代料DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:create", "创建BOM替代料")]
    [HttpPost]
    public async Task<IActionResult> CreateBillOfMaterialSubstituteAsync([FromBody] TaktBillOfMaterialSubstituteCreateDto dto)
    {
        try
        {
            var result = await _billOfMaterialSubstituteService.CreateBillOfMaterialSubstituteAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新BOM替代料
    /// </summary>
    /// <param name="id">BOM替代料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>BOM替代料DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:update", "更新BOM替代料")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBillOfMaterialSubstituteAsync(long id, [FromBody] TaktBillOfMaterialSubstituteUpdateDto dto)
    {
        try
        {
            var result = await _billOfMaterialSubstituteService.UpdateBillOfMaterialSubstituteAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除BOM替代料
    /// </summary>
    /// <param name="id">BOM替代料ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:delete", "删除BOM替代料")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBillOfMaterialSubstituteByIdAsync(long id)
    {
        try
        {
            await _billOfMaterialSubstituteService.DeleteBillOfMaterialSubstituteByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除BOM替代料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:delete", "批量删除BOM替代料")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBillOfMaterialSubstituteBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _billOfMaterialSubstituteService.DeleteBillOfMaterialSubstituteBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新BOM替代料作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>BOM替代料DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:update", "更新BOM替代料作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateBillOfMaterialSubstituteObsoleteAsync([FromBody] TaktBillOfMaterialSubstituteObsoleteDto dto)
    {
        try
        {
            var result = await _billOfMaterialSubstituteService.UpdateBillOfMaterialSubstituteObsoleteAsync(dto);
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
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:import", "获取BOM替代料导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetBillOfMaterialSubstituteTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _billOfMaterialSubstituteService.GetBillOfMaterialSubstituteTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入BOM替代料
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:import", "导入BOM替代料")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBillOfMaterialSubstituteAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _billOfMaterialSubstituteService.ImportBillOfMaterialSubstituteAsync(stream, sheetName);
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
    /// 导出BOM替代料
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:bill:of:material:export", "导出BOM替代料")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBillOfMaterialSubstituteAsync([FromQuery] TaktBillOfMaterialSubstituteQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _billOfMaterialSubstituteService.ExportBillOfMaterialSubstituteAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
