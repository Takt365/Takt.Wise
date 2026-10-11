// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.Logging
// 文件名称：TaktOperLogsController.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：操作日志控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Statistics.Logging;
using Takt.Application.Services.Statistics.Logging;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Statistics.Logging;

/// <summary>
/// 操作日志控制器
/// 提供操作日志的 REST API
/// </summary>
[ApiModule(9, "统计日志")]
[Route("api/[controller]", Name = "操作日志")]
public class TaktOperLogsController : TaktControllerBase
{
    private readonly ITaktOperLogService _operLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="operLogService">操作日志服务</param>
    public TaktOperLogsController(ITaktOperLogService operLogService)
    {
        _operLogService = operLogService;
    }

    /// <summary>
    /// 获取当前登录用户的操作日志列表（分页；个人中心，无需管理端 list 权限）
    /// </summary>
    /// <param name="queryDto">查询DTO（高级条件可用；UserName 由服务端强制为当前用户）</param>
    /// <returns>分页结果</returns>
    [HttpGet("me/list")]
    public async Task<IActionResult> GetMyOperLogListAsync([FromQuery] TaktOperLogQueryDto queryDto)
    {
        try
        {
            var result = await _operLogService.GetMyOperLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取操作日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:logging:oper:log:list", "操作日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetOperLogListAsync([FromQuery] TaktOperLogQueryDto queryDto)
    {
        try
        {
            var result = await _operLogService.GetOperLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取操作日志
    /// </summary>
    /// <param name="id">操作日志ID</param>
    /// <returns>操作日志DTO</returns>
    [TaktPermission("statistics:logging:oper:log:query", "操作日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOperLogByIdAsync(long id)
    {
        try
        {
            var result = await _operLogService.GetOperLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("操作日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取操作日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:logging:oper:log:query", "操作日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetOperLogOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _operLogService.GetOperLogOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建操作日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>操作日志DTO</returns>
    [TaktPermission("statistics:logging:oper:log:create", "创建操作日志")]
    [HttpPost]
    public async Task<IActionResult> CreateOperLogAsync([FromBody] TaktOperLogCreateDto dto)
    {
        try
        {
            var result = await _operLogService.CreateOperLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新操作日志
    /// </summary>
    /// <param name="id">操作日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>操作日志DTO</returns>
    [TaktPermission("statistics:logging:oper:log:update", "更新操作日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOperLogAsync(long id, [FromBody] TaktOperLogUpdateDto dto)
    {
        try
        {
            var result = await _operLogService.UpdateOperLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除操作日志
    /// </summary>
    /// <param name="id">操作日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:oper:log:delete", "删除操作日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOperLogByIdAsync(long id)
    {
        try
        {
            await _operLogService.DeleteOperLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除操作日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:oper:log:delete", "批量删除操作日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteOperLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _operLogService.DeleteOperLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新操作日志状态
    /// </summary>
    /// <param name="dto">状态 DTO（TaktExecuteStatus 枚举）</param>
    /// <returns>操作日志DTO</returns>
    [TaktPermission("statistics:logging:oper:log:update", "更新操作日志状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateOperLogStatusAsync([FromBody] TaktOperLogStatusDto dto)
    {
        try
        {
            var result = await _operLogService.UpdateOperLogStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出操作日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:logging:oper:log:export", "导出操作日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportOperLogAsync([FromQuery] TaktOperLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _operLogService.ExportOperLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
