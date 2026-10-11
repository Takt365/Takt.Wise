// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Mrp
// 文件名称：TaktPurchasePlansController.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：采购计划控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Mrp;
using Takt.Application.Services.Logistics.Manufacturing.Mrp;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Mrp;

/// <summary>
/// 采购计划控制器
/// 提供采购计划的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购计划")]
public class TaktPurchasePlansController : TaktControllerBase
{
    private readonly ITaktPurchasePlanService _purchasePlanService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePlanService">采购计划服务</param>
    public TaktPurchasePlansController(ITaktPurchasePlanService purchasePlanService)
    {
        _purchasePlanService = purchasePlanService;
    }

    /// <summary>
    /// 获取采购计划列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:purchase:plan:list", "采购计划列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchasePlanListAsync([FromQuery] TaktPurchasePlanQueryDto queryDto)
    {
        try
        {
            var result = await _purchasePlanService.GetPurchasePlanListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购计划
    /// </summary>
    /// <param name="id">采购计划ID</param>
    /// <returns>采购计划DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:purchase:plan:query", "采购计划详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchasePlanByIdAsync(long id)
    {
        try
        {
            var result = await _purchasePlanService.GetPurchasePlanByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购计划不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购计划选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:mrp:purchase:plan:query", "采购计划选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchasePlanOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchasePlanService.GetPurchasePlanOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购计划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购计划DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:purchase:plan:create", "创建采购计划")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchasePlanAsync([FromBody] TaktPurchasePlanCreateDto dto)
    {
        try
        {
            var result = await _purchasePlanService.CreatePurchasePlanAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购计划
    /// </summary>
    /// <param name="id">采购计划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购计划DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:purchase:plan:update", "更新采购计划")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchasePlanAsync(long id, [FromBody] TaktPurchasePlanUpdateDto dto)
    {
        try
        {
            var result = await _purchasePlanService.UpdatePurchasePlanAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购计划
    /// </summary>
    /// <param name="id">采购计划ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:purchase:plan:delete", "删除采购计划")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchasePlanByIdAsync(long id)
    {
        try
        {
            await _purchasePlanService.DeletePurchasePlanByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购计划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:purchase:plan:delete", "批量删除采购计划")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchasePlanBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchasePlanService.DeletePurchasePlanBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购计划状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>采购计划DTO</returns>
    [TaktPermission("logistics:manufacturing:mrp:purchase:plan:update", "更新采购计划状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePurchasePlanStatusAsync([FromBody] TaktPurchasePlanStatusDto dto)
    {
        try
        {
            var result = await _purchasePlanService.UpdatePurchasePlanStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:mrp:purchase:plan:import", "获取采购计划导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchasePlanTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchasePlanService.GetPurchasePlanTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购计划
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:mrp:purchase:plan:import", "导入采购计划")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchasePlanAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchasePlanService.ImportPurchasePlanAsync(stream, sheetName);
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
    /// 导出采购计划
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:mrp:purchase:plan:export", "导出采购计划")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchasePlanAsync([FromQuery] TaktPurchasePlanQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchasePlanService.ExportPurchasePlanAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
