// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseRequestsController.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：采购申请控制器
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
/// 采购申请控制器
/// 提供采购申请的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购申请")]
public class TaktPurchaseRequestsController : TaktControllerBase
{
    private readonly ITaktPurchaseRequestService _purchaseRequestService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseRequestService">采购申请服务</param>
    public TaktPurchaseRequestsController(ITaktPurchaseRequestService purchaseRequestService)
    {
        _purchaseRequestService = purchaseRequestService;
    }

    /// <summary>
    /// 获取采购申请列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:request:list", "采购申请列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseRequestListAsync([FromQuery] TaktPurchaseRequestQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseRequestService.GetPurchaseRequestListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购申请
    /// </summary>
    /// <param name="id">采购申请ID</param>
    /// <returns>采购申请DTO</returns>
    [TaktPermission("logistics:procurement:purchase:request:query", "采购申请详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetPurchaseRequestByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseRequestService.GetPurchaseRequestByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购申请不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购申请选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:request:query", "采购申请选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseRequestOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _purchaseRequestService.GetPurchaseRequestOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购申请
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购申请DTO</returns>
    [TaktPermission("logistics:procurement:purchase:request:create", "创建采购申请")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseRequestAsync([FromBody] TaktPurchaseRequestCreateDto dto)
    {
        try
        {
            var result = await _purchaseRequestService.CreatePurchaseRequestAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购申请
    /// </summary>
    /// <param name="id">采购申请ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购申请DTO</returns>
    [TaktPermission("logistics:procurement:purchase:request:update", "更新采购申请")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseRequestAsync(long id, [FromBody] TaktPurchaseRequestUpdateDto dto)
    {
        try
        {
            var result = await _purchaseRequestService.UpdatePurchaseRequestAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购申请
    /// </summary>
    /// <param name="id">采购申请ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:request:delete", "删除采购申请")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseRequestByIdAsync(long id)
    {
        try
        {
            await _purchaseRequestService.DeletePurchaseRequestByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购申请
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:request:delete", "批量删除采购申请")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseRequestBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseRequestService.DeletePurchaseRequestBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购申请状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>采购申请DTO</returns>
    [TaktPermission("logistics:procurement:purchase:request:update", "更新采购申请状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePurchaseRequestStatusAsync([FromBody] TaktPurchaseRequestStatusDto dto)
    {
        try
        {
            var result = await _purchaseRequestService.UpdatePurchaseRequestStatusAsync(dto);
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
    [TaktPermission("logistics:procurement:purchase:request:import", "获取采购申请导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseRequestTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseRequestService.GetPurchaseRequestTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购申请
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:request:import", "导入采购申请")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseRequestAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseRequestService.ImportPurchaseRequestAsync(stream, sheetName);
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
    /// 导出采购申请
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:request:export", "导出采购申请")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseRequestAsync([FromQuery] TaktPurchaseRequestQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseRequestService.ExportPurchaseRequestAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
