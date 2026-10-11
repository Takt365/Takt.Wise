// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.SignalR
// 文件名称：TaktSignalRLogging.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 专用日志（Hub 连接、统计推送、协商请求）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Helpers;
using Takt.Shared.Models.Logging;

namespace Takt.Infrastructure.SignalR;

/// <summary>
/// SignalR 专用日志辅助类（统一 Module=signalr，控制台与文件均可检索）
/// </summary>
internal static class TaktSignalRLogging
{
    private const string ModuleName = "signalr";

    /// <summary>
    /// 记录 Hub 客户端连接成功
    /// </summary>
    /// <param name="hubName">Hub 名称</param>
    /// <param name="connectionId">连接 ID</param>
    /// <param name="UserName">用户名</param>
    /// <param name="userId">用户 ID</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="tenantCode">租户编码</param>
    public static void LogHubConnected(
        string hubName,
        string? connectionId,
        string UserName,
        long? userId,
        string companyCode,
        string? tenantCode = null,
        string? clientIp = null,
        string? connectLocation = null)
    {
        var context = BuildContext(
            "connect",
            hubName,
            connectionId,
            UserName,
            userId,
            companyCode,
            tenantCode,
            clientIp,
            connectLocation);
        TaktLogger.Information(
            context,
            "SignalR Hub 已连接: {HubName}, ConnectionId={ConnectionId}, User={UserName}",
            hubName,
            connectionId,
            UserName);
    }

    /// <summary>
    /// 记录 Hub 客户端断开连接
    /// </summary>
    /// <param name="hubName">Hub 名称</param>
    /// <param name="connectionId">连接 ID</param>
    /// <param name="UserName">用户名</param>
    /// <param name="userId">用户 ID</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="exception">断开异常</param>
    public static void LogHubDisconnected(
        string hubName,
        string? connectionId,
        string UserName,
        long? userId,
        string companyCode,
        string? tenantCode = null,
        Exception? exception = null,
        string? clientIp = null,
        string? connectLocation = null)
    {
        var context = BuildContext(
            "disconnect",
            hubName,
            connectionId,
            UserName,
            userId,
            companyCode,
            tenantCode,
            clientIp,
            connectLocation);
        if (exception != null)
        {
            TaktLogger.Error(
                exception,
                context,
                "SignalR Hub 异常断开: {HubName}, ConnectionId={ConnectionId}, User={UserName}",
                hubName,
                connectionId,
                UserName);
            return;
        }

        TaktLogger.Information(
            context,
            "SignalR Hub 已断开: {HubName}, ConnectionId={ConnectionId}, User={UserName}",
            hubName,
            connectionId,
            UserName);
    }

    /// <summary>
    /// 记录统计推送
    /// </summary>
    /// <param name="statisticsType">统计类型（online / message）</param>
    /// <param name="UserName">目标用户名</param>
    /// <param name="companyCode">公司编码</param>
    public static void LogStatisticsPushed(string statisticsType, string UserName, string companyCode)
    {
        var context = new TaktLogContext
        {
            Module = ModuleName,
            Action = "statistics-push",
            UserName = UserName,
            CompanyCode = companyCode,
            Extra = new Dictionary<string, object?> { ["StatisticsType"] = statisticsType },
        };
        TaktLogger.Information(
            context,
            "SignalR 统计已推送: Type={StatisticsType}, User={UserName}, Company={CompanyCode}",
            statisticsType,
            UserName,
            companyCode);
    }

    /// <summary>
    /// 记录私信推送
    /// </summary>
    /// <param name="toUserName">接收者用户名</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="messageId">消息 ID</param>
    /// <param name="userGroup">用户组名</param>
    /// <param name="recipientUserId">接收者用户 ID（Clients.User 定向推送，可为 null）</param>
    public static void LogPrivateMessagePushed(
        string toUserName,
        string companyCode,
        long messageId,
        string userGroup,
        string? recipientUserId = null)
    {
        var context = new TaktLogContext
        {
            Module = ModuleName,
            Action = "private-message-push",
            UserName = toUserName,
            CompanyCode = companyCode,
            Extra = new Dictionary<string, object?>
            {
                ["MessageId"] = messageId.ToString(),
                ["UserGroup"] = userGroup,
                ["RecipientUserId"] = recipientUserId,
            },
        };
        TaktLogger.Information(
            context,
            "SignalR 私信已推送: MessageId={MessageId}, ToUser={ToUserName}, Group={UserGroup}, RecipientUserId={RecipientUserId}",
            messageId,
            toUserName,
            userGroup,
            recipientUserId ?? "(group)");
    }

    /// <summary>
    /// 记录工作流推送
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="detail">详情</param>
    /// <param name="UserName">目标用户名</param>
    public static void LogWorkflowPushed(string eventType, string companyCode, string detail, string? UserName = null)
    {
        var context = new TaktLogContext
        {
            Module = ModuleName,
            Action = "workflow-push",
            UserName = UserName,
            CompanyCode = companyCode,
            Extra = new Dictionary<string, object?>
            {
                ["EventType"] = eventType,
                ["Detail"] = detail,
            },
        };
        TaktLogger.Information(
            context,
            "SignalR 工作流已推送: Type={EventType}, Company={CompanyCode}, Detail={Detail}, User={UserName}",
            eventType,
            companyCode,
            detail,
            UserName ?? "(company)");
    }

    /// <summary>
    /// 记录 Quartz 定时任务推送
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="detail">详情</param>
    public static void LogQuartzPushed(string eventType, string companyCode, string detail)
    {
        var context = new TaktLogContext
        {
            Module = ModuleName,
            Action = "quartz-push",
            CompanyCode = companyCode,
            Extra = new Dictionary<string, object?>
            {
                ["EventType"] = eventType,
                ["Detail"] = detail,
            },
        };
        TaktLogger.Information(
            context,
            "SignalR Quartz 已推送: Type={EventType}, Company={CompanyCode}, Detail={Detail}",
            eventType,
            companyCode,
            detail);
    }

    /// <summary>
    /// 记录 Hub 相关 HTTP 请求完成（协商、WebSocket 升级等）
    /// </summary>
    /// <param name="logContext">请求级日志上下文（含 rid/tenant 等）</param>
    /// <param name="method">HTTP 方法</param>
    /// <param name="path">请求路径</param>
    /// <param name="statusCode">状态码</param>
    /// <param name="elapsedMs">耗时（毫秒）</param>
    public static void LogHubHttpCompleted(
        TaktLogContext logContext,
        string method,
        string path,
        int statusCode,
        long elapsedMs)
    {
        logContext.Module = ModuleName;
        logContext.Action = "http";
        logContext.Route = path;
        logContext.Extra = new Dictionary<string, object?>
        {
            ["StatusCode"] = statusCode,
            ["ElapsedMs"] = elapsedMs,
        };
        var httpPrefix = TaktLogFormatter.FormatHttpRequestPrefix(logContext);

        if (statusCode >= 400)
        {
            TaktLogger.Warning(
                logContext,
                "{HttpPrefix}SignalR HTTP 请求: {Method} {Path}, StatusCode={StatusCode}, Elapsed={ElapsedMs}ms",
                httpPrefix,
                method,
                path,
                statusCode,
                elapsedMs);
            return;
        }

        TaktLogger.Information(
            logContext,
            "{HttpPrefix}SignalR HTTP 请求: {Method} {Path}, StatusCode={StatusCode}, Elapsed={ElapsedMs}ms",
            httpPrefix,
            method,
            path,
            statusCode,
            elapsedMs);
    }

    /// <summary>
    /// 构建 SignalR 日志上下文
    /// </summary>
    private static TaktLogContext BuildContext(
        string action,
        string hubName,
        string? connectionId,
        string UserName,
        long? userId,
        string companyCode,
        string? tenantCode,
        string? clientIp = null,
        string? connectLocation = null)
    {
        return new TaktLogContext
        {
            Module = ModuleName,
            Action = action,
            Route = hubName,
            UserName = UserName,
            UserId = userId?.ToString(),
            CompanyCode = companyCode,
            TenantCode = tenantCode,
            ClientIp = clientIp,
            Extra = new Dictionary<string, object?>
            {
                ["ConnectionId"] = connectionId,
                ["ConnectLocation"] = connectLocation,
            },
        };
    }
}
