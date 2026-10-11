// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Aps
// 文件名称：TaktApsOrdersController.cs
// 创建时间：2026-07-24
// 创建人：Takt365(Cursor AI)
// 功能描述：APS排程订单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Application.Services.Logistics.Manufacturing.Aps;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Aps;

/// <summary>
/// APS排程订单控制器
/// 提供APS排程订单的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "APS排程订单")]
public class TaktApsOrdersController : TaktControllerBase
{
    private readonly ITaktApsOrderService _apsOrderService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsOrderService">APS排程订单服务</param>
    public TaktApsOrdersController(ITaktApsOrderService apsOrderService)
    {
        _apsOrderService = apsOrderService;
    }

    /// <summary>
    /// 获取APS排程订单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:list", "APS排程订单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetApsOrderListAsync([FromQuery] TaktApsOrderQueryDto queryDto)
    {
        try
        {
            var result = await _apsOrderService.GetApsOrderListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取APS排程订单
    /// </summary>
    /// <param name="id">APS排程订单ID</param>
    /// <returns>APS排程订单DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:query", "APS排程订单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetApsOrderByIdAsync(long id)
    {
        try
        {
            var result = await _apsOrderService.GetApsOrderByIdAsync(id);
            if (result == null)
            {
                return NotFound("APS排程订单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取APS排程订单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:query", "APS排程订单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetApsOrderOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _apsOrderService.GetApsOrderOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建APS排程订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>APS排程订单DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:create", "创建APS排程订单")]
    [HttpPost]
    public async Task<IActionResult> CreateApsOrderAsync([FromBody] TaktApsOrderCreateDto dto)
    {
        try
        {
            var result = await _apsOrderService.CreateApsOrderAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS排程订单
    /// </summary>
    /// <param name="id">APS排程订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>APS排程订单DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:update", "更新APS排程订单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApsOrderAsync(long id, [FromBody] TaktApsOrderUpdateDto dto)
    {
        try
        {
            var result = await _apsOrderService.UpdateApsOrderAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除APS排程订单
    /// </summary>
    /// <param name="id">APS排程订单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:delete", "删除APS排程订单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApsOrderByIdAsync(long id)
    {
        try
        {
            await _apsOrderService.DeleteApsOrderByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除APS排程订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:delete", "批量删除APS排程订单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteApsOrderBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _apsOrderService.DeleteApsOrderBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS排程订单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>APS排程订单DTO</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:update", "更新APS排程订单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateApsOrderStatusAsync([FromBody] TaktApsOrderStatusDto dto)
    {
        try
        {
            var result = await _apsOrderService.UpdateApsOrderStatusAsync(dto);
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
    [TaktPermission("logistics:manufacturing:aps:schedule:import", "获取APS排程订单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetApsOrderTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _apsOrderService.GetApsOrderTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入APS排程订单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:import", "导入APS排程订单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportApsOrderAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _apsOrderService.ImportApsOrderAsync(stream, sheetName);
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
    /// 导出APS排程订单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:aps:schedule:export", "导出APS排程订单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportApsOrderAsync([FromQuery] TaktApsOrderQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _apsOrderService.ExportApsOrderAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
