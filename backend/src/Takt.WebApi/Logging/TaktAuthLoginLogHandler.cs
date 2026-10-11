// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Logging
// 文件名称：TaktAuthLoginLogHandler.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：认证登录统一日志处理器（Serilog 结构化日志 + 登录日志表持久化）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Http;
using Takt.Application.Services.Foundation;
using Takt.Application.Services.Identity;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Services;
using Takt.Shared.Helpers;
using Takt.Shared.Constants;
using Takt.Shared.Models.Logging;

namespace Takt.WebApi.Logging;

/// <summary>
/// 认证登录日志写入请求
/// </summary>
public sealed class TaktAuthLoginLogWriteRequest
{
    /// <summary>
    /// 流程阶段（TaktAuthLoginPhases）
    /// </summary>
    public required string Phase { get; init; }

    /// <summary>
    /// 登录方式（TaktConstants.LoginType）
    /// </summary>
    public required string LoginType { get; init; }

    /// <summary>
    /// 登录结果（TaktConstants.LoginResult）
    /// </summary>
    public required string LoginResult { get; init; }

    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; init; } = string.Empty;

    /// <summary>
    /// 公司编码
    /// </summary>
    public string? CompanyCode { get; init; }

    /// <summary>
    /// 用户名或 client_id
    /// </summary>
    public string UserName { get; init; } = string.Empty;

    /// <summary>
    /// 结果说明
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// 用户 ID（可选）
    /// </summary>
    public long? UserId { get; init; }

    /// <summary>
    /// 耗时（毫秒，可选）
    /// </summary>
    public long? ElapsedMs { get; init; }

    /// <summary>
    /// 是否写入登录日志表（默认 true；RBAC 诊断步骤建议 false）
    /// </summary>
    public bool PersistToLoginLog { get; init; } = true;

    /// <summary>
    /// 结构化诊断字段（权限数、菜单采样等）
    /// </summary>
    public IReadOnlyDictionary<string, object?>? Detail { get; init; }
}

/// <summary>
/// 认证流程 RBAC 诊断步骤（仅 Serilog，不写登录日志表）
/// </summary>
public sealed class TaktAuthFlowStepRequest
{
    /// <summary>
    /// 流程阶段（TaktAuthLoginPhases）
    /// </summary>
    public required string Phase { get; init; }

    /// <summary>
    /// 说明
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// 用户 ID
    /// </summary>
    public long? UserId { get; init; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; init; }

    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; init; }

    /// <summary>
    /// 公司编码
    /// </summary>
    public string? CompanyCode { get; init; }

    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    public long? ElapsedMs { get; init; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess { get; init; } = true;

    /// <summary>
    /// 诊断明细
    /// </summary>
    public IReadOnlyDictionary<string, object?>? Detail { get; init; }
}

/// <summary>
/// 认证登录统一日志处理器
/// </summary>
public interface ITaktAuthLoginLogHandler
{
    /// <summary>
    /// 写入认证登录日志（控制台/文件 Serilog + 登录日志表）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="request">日志内容</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task WriteAsync(
        HttpContext httpContext,
        TaktAuthLoginLogWriteRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 写入认证流程 RBAC 诊断日志（仅 Serilog，不落登录日志表）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="request">步骤内容</param>
    void WriteFlowStep(HttpContext httpContext, TaktAuthFlowStepRequest request);
}

/// <summary>
/// ITaktAuthLoginLogHandler 实现
/// </summary>
public sealed class TaktAuthLoginLogHandler : ITaktAuthLoginLogHandler
{
    private const string LogModule = "identity/auth";

    private readonly ITaktLoginLogTenantWriter _loginLogTenantWriter;
    private readonly ITaktVisitLogTenantWriter _visitLogTenantWriter;
    private readonly ITaktAuthService _authService;
    private readonly ITaktOnlineService _onlineService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="loginLogTenantWriter">登录日志租户库写入器</param>
    /// <param name="visitLogTenantWriter">访问量租户库写入器</param>
    /// <param name="authService">身份认证服务</param>
    /// <param name="onlineService">在线用户服务</param>
    public TaktAuthLoginLogHandler(
        ITaktLoginLogTenantWriter loginLogTenantWriter,
        ITaktVisitLogTenantWriter visitLogTenantWriter,
        ITaktAuthService authService,
        ITaktOnlineService onlineService)
    {
        _loginLogTenantWriter = loginLogTenantWriter;
        _visitLogTenantWriter = visitLogTenantWriter;
        _authService = authService;
        _onlineService = onlineService;
    }

    /// <summary>
    /// 写入认证登录日志（控制台/文件 Serilog + 可选登录日志表）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="request">日志内容</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task WriteAsync(
        HttpContext httpContext,
        TaktAuthLoginLogWriteRequest request,
        CancellationToken cancellationToken = default)
    {
        TaktUserContext.StashAuditOperator(
            httpContext,
            request.UserId,
            request.UserName,
            BuildAuthAuditContextRemark(request));

        var client = TaktHttpAuditHelper.ResolveClientLogContext(httpContext);
        var requestId = httpContext.Items["RequestId"] as string;
        var path = httpContext.Request.Path.Value;
        var isSuccess = request.LoginResult == TaktConstants.LoginResult.Success;

        var logContext = new TaktLogContext
        {
            Module = LogModule,
            Action = request.Phase,
            UserId = request.UserId?.ToString(),
            UserName = ResolveLoginLogUserName(request.UserName),
            TenantCode = string.IsNullOrWhiteSpace(request.TenantCode) ? null : request.TenantCode,
            CompanyCode = request.CompanyCode,
            Route = path,
            RequestId = requestId,
            ClientIp = client.Ip,
            Extra = MergeExtra(
                new Dictionary<string, object?>
                {
                    ["LoginType"] = request.LoginType,
                    ["LoginResult"] = request.LoginResult,
                    ["Message"] = request.Message,
                    ["ElapsedMs"] = request.ElapsedMs,
                },
                request.Detail),
        };

        using (TaktLogger.BeginScope(logContext))
        {
            if (isSuccess)
            {
                TaktLogger.Information(
                    logContext,
                    "认证登录 [{Phase}] 成功: 用户={UserName}, 租户={TenantCode}, 方式={LoginType}, 说明={Message}, 耗时={ElapsedMs}ms",
                    request.Phase,
                    request.UserName,
                    request.TenantCode,
                    request.LoginType,
                    request.Message ?? string.Empty,
                    request.ElapsedMs);
            }
            else
            {
                TaktLogger.Warning(
                    logContext,
                    "认证登录 [{Phase}] 失败: 用户={UserName}, 租户={TenantCode}, 方式={LoginType}, 结果={LoginResult}, 说明={Message}, 耗时={ElapsedMs}ms",
                    request.Phase,
                    request.UserName,
                    request.TenantCode,
                    request.LoginType,
                    request.LoginResult,
                    request.Message ?? string.Empty,
                    request.ElapsedMs);
            }
        }

        if (request.PersistToLoginLog)
        {
            try
            {
                if (isSuccess && request.LoginType == TaktConstants.LoginType.SignOut)
                {
                    await TryFinalizeSignOutSideEffectsAsync(request, cancellationToken);
                }

                var (tenantCode, companyCode) = await ResolveLoginLogScopeAsync(request, cancellationToken);
                if (string.IsNullOrWhiteSpace(tenantCode) || string.IsNullOrWhiteSpace(companyCode))
                {
                    TaktLogger.Debug(
                        "[AuthLoginLog] 跳过登录日志表写入：租户或公司未解析 Phase={Phase}, Tenant={TenantCode}, Company={CompanyCode}, UserName={UserName}",
                        request.Phase,
                        tenantCode ?? string.Empty,
                        companyCode ?? string.Empty,
                        request.UserName);
                }
                else
                {
                    await _loginLogTenantWriter.CreateInTenantAsync(
                        tenantCode,
                        new TaktLoginLog
                        {
                            CompanyCode = companyCode,
                            UserName = ResolveLoginLogUserName(request.UserName),
                            LoginType = request.LoginType,
                            LoginResult = request.LoginResult,
                            LoginMessage = BuildPersistMessage(request),
                            Remark = ResolveLoginLogRemark(request),
                            LoginIp = client.Ip,
                            LoginLocation = client.Location,
                            UserAgent = client.UserAgent,
                            Browser = client.Browser,
                            Os = client.OperatingSystem,
                        },
                        request.UserId,
                        cancellationToken);
                }
            }
            catch (Exception ex)
            {
                TaktLogger.Warning(
                    ex,
                    "[AuthLoginLog] 持久化登录日志失败: Phase={Phase}, UserName={UserName}",
                    request.Phase,
                    request.UserName);
            }
        }

        if (isSuccess
            && request.UserId is > 0
            && ShouldRecordUserLastLogin(request.LoginType))
        {
            try
            {
                var (tenantCode, companyCode) = await ResolveLoginLogScopeAsync(request, cancellationToken);
                if (!string.IsNullOrWhiteSpace(tenantCode) && !string.IsNullOrWhiteSpace(companyCode))
                {
                    await _visitLogTenantWriter.IncrementDailyVisitCountAsync(
                        tenantCode,
                        companyCode,
                        request.UserId.Value,
                        request.UserName,
                        cancellationToken: cancellationToken);
                }
            }
            catch (Exception ex)
            {
                TaktLogger.Warning(
                    ex,
                    "[AuthLoginLog] 累加当日访问量失败: Phase={Phase}, UserId={UserId}",
                    request.Phase,
                    request.UserId);
            }
        }

        if (isSuccess
            && request.UserId is > 0
            && !string.IsNullOrWhiteSpace(request.TenantCode)
            && ShouldRecordUserLastLogin(request.LoginType))
        {
            try
            {
                await _authService.RecordUserLastLoginAsync(
                    request.UserId.Value,
                    request.TenantCode,
                    client.Ip,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                TaktLogger.Warning(
                    ex,
                    "[AuthLoginLog] 更新用户最后登录信息失败: Phase={Phase}, UserId={UserId}",
                    request.Phase,
                    request.UserId);
            }
        }
    }

    /// <summary>
    /// 解析登录日志落库的租户与公司（租户库硬隔离：仅使用请求显式租户，连接 Tenant_{code} 库；公司为公司级共享隔离）
    /// </summary>
    /// <param name="request">认证登录日志请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>租户与公司编码；任一项无法解析则对应值为 null</returns>
    private async Task<(string? TenantCode, string? CompanyCode)> ResolveLoginLogScopeAsync(
        TaktAuthLoginLogWriteRequest request,
        CancellationToken cancellationToken)
    {
        _ = cancellationToken;
        var tenantCode = string.IsNullOrWhiteSpace(request.TenantCode) ? null : request.TenantCode.Trim();
        var companyCode = string.IsNullOrWhiteSpace(request.CompanyCode) ? null : request.CompanyCode.Trim();

        if (!string.IsNullOrEmpty(tenantCode) && !_loginLogTenantWriter.IsTenantDatabaseConfigured(tenantCode))
        {
            TaktLogger.Debug(
                "[AuthLoginLog] 跳过登录日志表写入：未配置租户库 ConnectionStrings:Tenant_{TenantCode}",
                tenantCode);
            return (null, null);
        }

        if (string.IsNullOrEmpty(companyCode)
            && request.UserId is > 0
            && !string.IsNullOrEmpty(tenantCode))
        {
            companyCode = await _loginLogTenantWriter.ResolveUserDefaultCompanyCodeAsync(
                tenantCode,
                request.UserId.Value,
                cancellationToken);
        }

        return (tenantCode, companyCode);
    }

    /// <summary>
    /// 登出成功时回填 LoginLog.LogoutAt、关闭 Online 并刷新 LastActiveTime
    /// </summary>
    /// <param name="request">认证登录日志请求</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    private async Task TryFinalizeSignOutSideEffectsAsync(
        TaktAuthLoginLogWriteRequest request,
        CancellationToken cancellationToken)
    {
        var tenantCode = string.IsNullOrWhiteSpace(request.TenantCode) ? null : request.TenantCode.Trim();
        if (string.IsNullOrEmpty(tenantCode))
        {
            return;
        }

        if (request.UserId is not > 0 && IsUnknownOrEmptySignOutUserName(request.UserName))
        {
            return;
        }

        var companyCode = string.IsNullOrWhiteSpace(request.CompanyCode) ? null : request.CompanyCode.Trim();
        if (string.IsNullOrEmpty(companyCode)
            && request.UserId is > 0)
        {
            companyCode = await _loginLogTenantWriter.ResolveUserDefaultCompanyCodeAsync(
                tenantCode,
                request.UserId.Value,
                cancellationToken);
        }

        var logoutAt = DateTime.Now;
        var UserName = NormalizeSignOutUserName(request.UserName);
        if (string.IsNullOrEmpty(UserName)
            && request.UserId is > 0)
        {
            UserName = await _loginLogTenantWriter.ResolveUserNameByUserIdAsync(
                tenantCode,
                request.UserId.Value,
                cancellationToken);
        }

        if (!string.IsNullOrEmpty(UserName))
        {
            var updatedCount = await _loginLogTenantWriter.CloseOpenLoginSessionAsync(
                tenantCode,
                companyCode,
                UserName,
                logoutAt,
                request.UserId,
                cancellationToken);

            if (updatedCount == 0)
            {
                TaktLogger.Debug(
                    "[AuthLoginLog] 登出未匹配未关闭登录会话 Tenant={TenantCode}, Company={CompanyCode}, UserName={UserName}",
                    tenantCode,
                    companyCode,
                    UserName);
            }
        }

        if (request.UserId is > 0 && !string.IsNullOrEmpty(companyCode))
        {
            try
            {
                var closedCount = await _onlineService.CloseOnlineSessionsByUserIdAsync(
                    request.UserId.Value,
                    logoutAt);
                if (closedCount == 0)
                {
                    TaktLogger.Debug(
                        "[AuthLoginLog] 登出未匹配在线会话 Tenant={TenantCode}, Company={CompanyCode}, UserId={UserId}",
                        tenantCode,
                        companyCode,
                        request.UserId);
                }
            }
            catch (Exception ex)
            {
                TaktLogger.Warning(
                    ex,
                    "[AuthLoginLog] 登出关闭在线会话失败 Tenant={TenantCode}, Company={CompanyCode}, UserId={UserId}",
                    tenantCode,
                    companyCode,
                    request.UserId);
            }
        }
    }

    /// <summary>
    /// 登出用户名是否为空或 unknown
    /// </summary>
    /// <param name="UserName">原始用户名</param>
    /// <returns>无法用于匹配登录日志时为 true</returns>
    private static bool IsUnknownOrEmptySignOutUserName(string? UserName)
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            return true;
        }

        return string.Equals(
            UserName.Trim(),
            TaktConstants.AuditUserName.Unknown,
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 规范化登出匹配用登录名（小写）
    /// </summary>
    /// <param name="UserName">原始用户名</param>
    /// <returns>小写登录名；无效时返回 null</returns>
    private static string? NormalizeSignOutUserName(string? UserName)
    {
        if (IsUnknownOrEmptySignOutUserName(UserName))
        {
            return null;
        }

        return UserName!.Trim().ToLowerInvariant();
    }

    /// <summary>
    /// 是否应在成功认证后更新用户表最后登录字段
    /// </summary>
    /// <param name="loginType">登录方式</param>
    /// <returns>需要更新为 true</returns>
    private static bool ShouldRecordUserLastLogin(string loginType)
    {
        return loginType is not (
            TaktConstants.LoginType.SignOut
            or TaktConstants.LoginType.RefreshToken
            or TaktConstants.LoginType.VerifyPassword
            or TaktConstants.LoginType.ClientCredentials);
    }

    /// <summary>
    /// 写入认证流程 RBAC 诊断日志（仅 Serilog，不落登录日志表）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="request">步骤内容</param>
    public void WriteFlowStep(HttpContext httpContext, TaktAuthFlowStepRequest request)
    {
        var requestId = httpContext.Items["RequestId"] as string;
        var path = httpContext.Request.Path.Value;
        var client = TaktHttpAuditHelper.ResolveClientLogContext(httpContext);

        var logContext = new TaktLogContext
        {
            Module = LogModule,
            Action = request.Phase,
            UserId = request.UserId?.ToString(),
            UserName = request.UserName,
            TenantCode = string.IsNullOrWhiteSpace(request.TenantCode) ? null : request.TenantCode,
            CompanyCode = request.CompanyCode,
            Route = path,
            RequestId = requestId,
            ClientIp = client.Ip,
            Extra = MergeExtra(
                new Dictionary<string, object?>
                {
                    ["Message"] = request.Message,
                    ["ElapsedMs"] = request.ElapsedMs,
                    ["IsSuccess"] = request.IsSuccess,
                },
                request.Detail),
        };

        using (TaktLogger.BeginScope(logContext))
        {
            if (request.IsSuccess)
            {
                TaktLogger.Information(
                    logContext,
                    "认证流程 [{Phase}] 成功: 用户={UserName}, 租户={TenantCode}, 说明={Message}, 耗时={ElapsedMs}ms",
                    request.Phase,
                    request.UserName ?? string.Empty,
                    request.TenantCode ?? string.Empty,
                    request.Message ?? string.Empty,
                    request.ElapsedMs);
            }
            else
            {
                TaktLogger.Warning(
                    logContext,
                    "认证流程 [{Phase}] 失败: 用户={UserName}, 租户={TenantCode}, 说明={Message}, 耗时={ElapsedMs}ms",
                    request.Phase,
                    request.UserName ?? string.Empty,
                    request.TenantCode ?? string.Empty,
                    request.Message ?? string.Empty,
                    request.ElapsedMs);
            }
        }
    }

    /// <summary>
    /// 将诊断明细字段合并到 Serilog Extra 字典
    /// </summary>
    /// <param name="target">基础 Extra 字段</param>
    /// <param name="detail">可选诊断明细</param>
    /// <returns>合并后的字典</returns>
    private static Dictionary<string, object?> MergeExtra(
        Dictionary<string, object?> target,
        IReadOnlyDictionary<string, object?>? detail)
    {
        if (detail == null)
        {
            return target;
        }

        foreach (var pair in detail)
        {
            target[pair.Key] = pair.Value;
        }

        return target;
    }

    /// <summary>
    /// 合并阶段与说明，写入登录日志表 Message 字段
    /// </summary>
    private static string BuildPersistMessage(TaktAuthLoginLogWriteRequest request)
    {
        var message = request.Message?.Trim();
        if (string.IsNullOrEmpty(message))
        {
            return $"[{request.Phase}] {request.LoginResult}";
        }

        return $"[{request.Phase}] {message}";
    }

    /// <summary>
    /// 截断字符串至指定最大长度（用于 UserAgent 等落库字段）
    /// </summary>
    /// <param name="value">原始字符串</param>
    /// <param name="maxLength">最大长度</param>
    /// <returns>截断后的字符串；空值原样返回</returns>
    private static string? Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
        {
            return value;
        }

        return value[..maxLength];
    }

    /// <summary>
    /// 登录日志落库用户名（空则 unknown）
    /// </summary>
    /// <param name="UserName">原始用户名</param>
    /// <returns>小写登录名或 unknown</returns>
    private static string ResolveLoginLogUserName(string? UserName)
    {
        return string.IsNullOrWhiteSpace(UserName)
            ? TaktConstants.AuditUserName.Unknown
            : UserName.Trim().ToLowerInvariant();
    }

    /// <summary>
    /// 登录日志 Remark（登出且用户名未知时说明原因）
    /// </summary>
    /// <param name="request">认证登录日志请求</param>
    /// <returns>Remark 文本</returns>
    private static string ResolveLoginLogRemark(TaktAuthLoginLogWriteRequest request)
    {
        if (request.LoginType == TaktConstants.LoginType.SignOut
            && string.IsNullOrWhiteSpace(request.UserName))
        {
            return TaktAuditContextRemarks.BuildSignOutUnknownUser(request.Phase, request.Message);
        }

        return TaktAuthLoginPhases.BuildLoginStepRemark(request.Phase);
    }

    /// <summary>
    /// 暂存至 HttpContext 的审计 Remark（登出且用户名未知）
    /// </summary>
    /// <param name="request">认证登录日志请求</param>
    /// <returns>Remark；无需说明时为 null</returns>
    private static string? BuildAuthAuditContextRemark(TaktAuthLoginLogWriteRequest request)
    {
        if (request.LoginType != TaktConstants.LoginType.SignOut
            || !string.IsNullOrWhiteSpace(request.UserName))
        {
            return null;
        }

        return TaktAuditContextRemarks.BuildSignOutUnknownUser(request.Phase, request.Message);
    }
}
