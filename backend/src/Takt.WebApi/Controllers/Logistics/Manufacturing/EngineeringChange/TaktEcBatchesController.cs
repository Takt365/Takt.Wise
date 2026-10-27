// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcBatchesController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板批次页签 API（权限归看板 kanban）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变看板批次页签控制器（权限与看板一致）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变看板批次")]
public class TaktEcBatchesController : TaktControllerBase
{
    private readonly ITaktEcBatchService _service;
    private readonly ITaktEcDeptMatrixService _deptMatrixService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcBatchesController(ITaktEcBatchService service, ITaktEcDeptMatrixService deptMatrixService)
    {
        _service = service;
        _deptMatrixService = deptMatrixService;
    }

    /// <summary>
    /// 获取投入批次列表（分页）
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kanban:list", "设变看板批次列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcBatchListAsync([FromQuery] TaktEcBatchQueryDto queryDto)
    {
        try { var result = await _service.GetEcBatchListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 获取投入批次详情
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kanban:query", "设变看板批次详情")]
    [HttpGet("detail/{ecDetailId}")]
    public async Task<IActionResult> GetEcBatchByEcDetailIdAsync(long ecDetailId)
    {
        try { var result = await _service.GetEcBatchByEcDetailIdAsync(ecDetailId); if (result == null) return NotFound("投入批次不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 更新投入批次
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kanban:update", "更新设变看板批次")]
    [HttpPut("detail/{ecDetailId}")]
    public async Task<IActionResult> UpdateEcBatchAsync(long ecDetailId, [FromBody] TaktEcBatchUpdateDto dto)
    {
        try { var result = await _service.UpdateEcBatchAsync(ecDetailId, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 导出投入批次
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kanban:export", "导出设变看板批次")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcBatchAsync([FromQuery] TaktEcBatchQueryDto? query)
    {
        try { var (fileName, fileContent) = await _service.ExportEcBatchAsync(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 获取投入批次转置列表（分页；行=设变明细，列=各阶段日期+批次）
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kanban:list", "设变看板批次转置列表")]
    [HttpGet("transposed")]
    public async Task<IActionResult> GetEcBatchTransposedListAsync([FromQuery] TaktEcExecBatchTransposedQueryDto queryDto)
    {
        try { var result = await _deptMatrixService.GetEcBatchTransposedListAsync(queryDto); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }
}
