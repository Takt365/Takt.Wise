// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktOnlinesController.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线用户控制器（查询/删除/导出 + SignalR 强退/统计推送；会话由 SignalR 自动注册，不提供新增/更新/导入/模板）
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Mapster;
using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Domain.Interfaces;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Foundation;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 在线用户控制器
/// 提供在线用户查询、删除、导出及 SignalR 扩展 API（不提供手动新增/更新/导入/模板）
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "在线用户")]
public class TaktOnlinesController : TaktControllerBase
{
    private readonly ITaktOnlineService _onlineService;
    private readonly ITaktSignalRDispatchService _signalRDispatchService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="onlineService">在线用户服务</param>
    /// <param name="signalRDispatchService">SignalR 推送调度服务</param>
    public TaktOnlinesController(
        ITaktOnlineService onlineService,
        ITaktSignalRDispatchService signalRDispatchService)
    {
        _onlineService = onlineService;
        _signalRDispatchService = signalRDispatchService;
    }

    /// <summary>
    /// 获取在线用户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:online:list", "在线用户列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetOnlineListAsync([FromQuery] TaktOnlineQueryDto queryDto)
    {
        try
        {
            var result = await _onlineService.GetOnlineListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <returns>在线用户DTO</returns>
    [TaktPermission("foundation:online:query", "在线用户详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOnlineByIdAsync(long id)
    {
        try
        {
            var result = await _onlineService.GetOnlineByIdAsync(id);
            if (result == null)
            {
                return NotFound("在线用户不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取在线用户选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:online:query", "在线用户选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetOnlineOptionsAsync([FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _onlineService.GetOnlineOptionsAsync(plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:online:delete", "删除在线用户")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOnlineByIdAsync(long id)
    {
        try
        {
            await _onlineService.DeleteOnlineByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除在线用户
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:online:delete", "批量删除在线用户")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteOnlineBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _onlineService.DeleteOnlineBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新在线用户状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>在线用户DTO</returns>
    [TaktPermission("foundation:online:update", "更新在线用户状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateOnlineStatusAsync([FromBody] TaktOnlineStatusDto dto)
    {
        try
        {
            var result = await _onlineService.UpdateOnlineStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取在线时长统计（唯一 API：当天/本周日均/本月日均，实现见 ITaktOnlineService.GetOnlineStatisticsAsync）
    /// </summary>
    /// <returns>统计结果</returns>
    [TaktPermission("foundation:online:query", "当前用户在线统计")]
    [HttpGet("statistics")]
    public async Task<IActionResult> GetOnlineStatisticsAsync([FromQuery] TaktOnlineStatisticsQueryDto? queryDto)
    {
        try
        {
            var result = await _onlineService.GetOnlineStatisticsAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取在线看板统计（公司维度：在线人数、当日总访问量、当前会话）
    /// </summary>
    /// <returns>看板统计结果</returns>
    [TaktPermission("foundation:online:list", "在线看板统计")]
    [HttpGet("statistics/dashboard")]
    public async Task<IActionResult> GetOnlineDashboardStatisticsAsync()
    {
        try
        {
            var result = await _onlineService.GetOnlineDashboardStatisticsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出在线用户
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:online:export", "导出在线用户")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportOnlineAsync([FromQuery] TaktOnlineQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _onlineService.ExportOnlineAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #region SignalR 推送调度

    /// <summary>
    /// 强制踢出在线用户（强退）
    /// </summary>
    /// <param name="onlineId">在线用户记录 ID</param>
    /// <param name="dto">强退参数</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:online:kick", "强退在线用户")]
    [HttpPost("{onlineId}/force-kick")]
    public async Task<IActionResult> ForceKickOnlineByIdAsync(string onlineId, [FromBody] TaktOnlineForceKickDto? dto = null)
    {
        try
        {
            if (!long.TryParse(onlineId, out var parsedOnlineId))
            {
                return BadRequest("无效的在线用户 ID");
            }

            var connectionId = dto?.ConnectionId?.Trim();
            if (parsedOnlineId <= 0 && string.IsNullOrEmpty(connectionId))
            {
                return BadRequest("无效的在线用户 ID");
            }

            await _signalRDispatchService.ForceKickOnlineAsync(
                parsedOnlineId,
                dto?.Reason,
                connectionId,
                dto?.DelaySeconds ?? 0);
            return Success("强退成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量强制踢出在线用户
    /// </summary>
    /// <param name="dto">批量强退参数</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:online:kick", "批量强退在线用户")]
    [HttpPost("force-kick/batch")]
    public async Task<IActionResult> ForceKickOnlineBatchAsync([FromBody] TaktOnlineForceKickBatchDto dto)
    {
        try
        {
            await _signalRDispatchService.ForceKickOnlineBatchAsync(dto.OnlineIds, dto.Reason, dto.DelaySeconds);
            return Success("批量强退完成");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推送广播通知到公司内在线客户端
    /// </summary>
    /// <param name="dto">广播内容</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:message:broadcast", "推送在线广播")]
    [HttpPost("messages/broadcast")]
    public async Task<IActionResult> PushBroadcastMessageAsync([FromBody] TaktMessageBroadcastDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.CompanyCode))
            {
                dto.CompanyCode = CurrentCompanyCode ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(dto.FromUserName))
            {
                dto.FromUserName = CurrentUserName ?? string.Empty;
            }

            await _signalRDispatchService.PushBroadcastMessageAsync(dto.Adapt<TaktSignalRBroadcastPush>());
            return Success("广播推送成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 向指定用户推送最新在线统计（多终端同步）
    /// </summary>
    /// <param name="dto">目标用户</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:online:stats", "推送在线统计")]
    [HttpPost("statistics/online/push")]
    public async Task<IActionResult> PushOnlineStatisticsAsync([FromBody] TaktSignalRPushStatisticsRequestDto dto)
    {
        try
        {
            var companyCode = CurrentCompanyCode;
            if (string.IsNullOrWhiteSpace(companyCode))
            {
                return BadRequest("缺少公司上下文");
            }

            await _signalRDispatchService.PushOnlineStatisticsToUserAsync(
                companyCode,
                dto.UserName.Trim(),
                ParseOptionalUserId(dto.UserId));
            return Success("在线统计推送成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 向指定用户推送最新消息统计（多终端同步）
    /// </summary>
    /// <param name="dto">目标用户</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:message:stats", "推送消息统计")]
    [HttpPost("statistics/message/push")]
    public async Task<IActionResult> PushMessageStatisticsAsync([FromBody] TaktSignalRPushStatisticsRequestDto dto)
    {
        try
        {
            var companyCode = CurrentCompanyCode;
            if (string.IsNullOrWhiteSpace(companyCode))
            {
                return BadRequest("缺少公司上下文");
            }

            await _signalRDispatchService.PushMessageStatisticsToUserAsync(
                companyCode,
                dto.UserName.Trim(),
                ParseOptionalUserId(dto.UserId));
            return Success("消息统计推送成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 解析可选用户 ID（前端 string ↔ 服务 long?）
    /// </summary>
    /// <param name="userId">用户 ID 字符串</param>
    /// <returns>解析成功返回 ID，否则 null</returns>
    private static long? ParseOptionalUserId(string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        return long.TryParse(userId.Trim(), out var parsedId) ? parsedId : null;
    }

    #endregion
}