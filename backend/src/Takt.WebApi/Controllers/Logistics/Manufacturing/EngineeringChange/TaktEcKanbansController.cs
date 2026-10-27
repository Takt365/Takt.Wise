// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcKanbansController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变看板控制器
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
/// 设变看板控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "设变看板")]
public class TaktEcKanbansController : TaktControllerBase
{
    private readonly ITaktEcKanbanService _service;
    private readonly ITaktEcDeptMatrixService _deptMatrixService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcKanbansController(ITaktEcKanbanService service, ITaktEcDeptMatrixService deptMatrixService)
    {
        _service = service;
        _deptMatrixService = deptMatrixService;
    }

    /// <summary>
    /// 获取设变看板列表（分页）
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kanban:list", "设变看板列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcKanbanListAsync([FromQuery] TaktEcKanbanQueryDto queryDto)
    {
        try { var result = await _service.GetEcKanbanListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 获取设变看板详情
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kanban:query", "设变看板详情")]
    [HttpGet("{ecTeId}")]
    public async Task<IActionResult> GetEcKanbanByEcIdAsync(long ecTeId)
    {
        try { var result = await _service.GetEcKanbanByEcIdAsync(ecTeId); if (result == null) return NotFound("设变看板不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }


    /// <summary>
    /// 导出设变看板
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kanban:export", "导出设变看板")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcKanbanAsync([FromQuery] TaktEcKanbanQueryDto? query)
    {
        try { var (fileName, fileContent) = await _service.ExportEcKanbanAsync(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 获取设变看板统计（看板页签；实施状态 / 部门 / 按月）
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kanban:list", "设变看板统计")]
    [HttpGet("board-stat")]
    public async Task<IActionResult> GetEcKanbanBoardStatAsync([FromQuery] TaktEcKanbanQueryDto queryDto)
    {
        try { var result = await _service.GetEcKanbanBoardStatAsync(queryDto); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 统计部门执行行数（8 张部门表；看板/数据看板用）
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kanban:list", "设变部门执行行统计")]
    [HttpGet("dept-execution-count")]
    public async Task<IActionResult> CountDeptExecutionRowsAsync([FromQuery] int? isImplemented)
    {
        try
        {
            var count = await _deptMatrixService.CountDeptExecutionRowsAsync(isImplemented);
            return Success(new { count }, "查询成功");
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
