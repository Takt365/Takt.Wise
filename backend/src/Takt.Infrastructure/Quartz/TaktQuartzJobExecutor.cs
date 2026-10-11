// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzJobExecutor.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 任务执行器（按 TaktQuartzTask 全字段执行、写日志并 SignalR 推送执行结果）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Quartz;
using SqlSugar;
using Takt.Application.Services.Foundation;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Infrastructure.Extensions;
using Takt.Infrastructure.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz 任务执行器（按任务类型分发并写执行日志）
/// </summary>
public sealed class TaktQuartzJobExecutor
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IEnumerable<ITaktQuartzJobHandler> _handlers;
    private readonly ITaktQuartzJobSignalRPushService _quartzJobSignalRPushService;
    private readonly TaktSqlSugarContext _sqlSugarContext;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="httpClientFactory">HTTP 客户端工厂</param>
    /// <param name="handlers">程序集任务处理器集合</param>
    /// <param name="quartzJobSignalRPushService">Quartz Job 执行 SignalR 推送</param>
    /// <param name="sqlSugarContext">Scoped SqlSugar（Assembly 按 targetDatabase 切库）</param>
    public TaktQuartzJobExecutor(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        IEnumerable<ITaktQuartzJobHandler> handlers,
        ITaktQuartzJobSignalRPushService quartzJobSignalRPushService,
        TaktSqlSugarContext sqlSugarContext)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(handlers);
        ArgumentNullException.ThrowIfNull(quartzJobSignalRPushService);
        ArgumentNullException.ThrowIfNull(sqlSugarContext);
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _handlers = handlers;
        _quartzJobSignalRPushService = quartzJobSignalRPushService;
        _sqlSugarContext = sqlSugarContext;
    }

    /// <summary>
    /// 执行 Quartz Job
    /// </summary>
    /// <param name="context">Quartz 执行上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    public async Task ExecuteAsync(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var data = context.MergedJobDataMap;
        var tenantCode = TaktQuartzJobDataMapHelper.GetStringOrNull(data, TaktQuartzJobDataKeys.TenantCode) ?? string.Empty;
        var companyCode = TaktQuartzJobDataMapHelper.GetStringOrNull(data, TaktQuartzJobDataKeys.CompanyCode) ?? string.Empty;
        var taskId = TaktQuartzJobDataMapHelper.GetLongOrDefault(data, TaktQuartzJobDataKeys.QuartzTaskId);
        var UserName = TaktQuartzJobDataMapHelper.GetStringOrNull(data, TaktQuartzJobDataKeys.UserName);
        var triggerExecuteParams = TaktQuartzJobDataMapHelper.GetStringOrNull(data, TaktQuartzJobDataKeys.ExecuteParams);
        var manualTrigger = TaktQuartzJobDataMapHelper.GetIntOrDefault(data, TaktQuartzJobDataKeys.ManualTrigger) == 1;
        if (string.IsNullOrWhiteSpace(tenantCode) || string.IsNullOrWhiteSpace(companyCode) || taskId <= 0)
        {
            var mapEx = new InvalidOperationException("Quartz JobDataMap 缺少 TenantCode/CompanyCode/QuartzTaskId");
            TaktQuartzJobExecutionLogger.LogInfrastructureFailure(mapEx, tenantCode, companyCode, taskId);
            throw mapEx;
        }
        using var seedContext = new TaktSeedContext(_configuration, tenantCode);
        var task = await seedContext.Db.Queryable<TaktQuartzTask>()
            .Where(x => x.Id == taskId
                && x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.IsDeleted == 0)
            .FirstAsync(cancellationToken);
        if (task == null)
        {
            var missingEx = new InvalidOperationException(
                $"定时任务不存在：Id={taskId}, Tenant={tenantCode}, Company={companyCode}");
            TaktQuartzJobExecutionLogger.LogInfrastructureFailure(missingEx, tenantCode, companyCode, taskId);
            throw missingEx;
        }
        using (TaktQuartzJobExecutionLogger.BeginExecutionScope(
            tenantCode,
            companyCode,
            task.Id,
            task.TaskCode,
            task.TaskType,
            UserName,
            manualTrigger))
        using (TaktSqlSugarAuditAop.BeginSuppressDiffLog())
        {
            var stopwatch = Stopwatch.StartNew();
            var executeTime = DateTime.Now;
            var executeMessage = string.Empty;
            var errorInfo = string.Empty;
            var executeStatus = TaktExecuteStatus.Success;
            var effectiveExecuteParams = TaktQuartzSchedulingHelper.ResolveEffectiveExecuteParams(task, triggerExecuteParams);
            Exception? executionException = null;
            TaktQuartzLog? executionLog = null;
            TaktQuartzJobExecutionLogger.LogStarted(task, manualTrigger, UserName);
            try
            {
                TaktQuartzSchedulingHelper.ValidateQuartzTaskForExecution(task, manualTrigger);
                executeMessage = await DispatchAsync(
                    task,
                    effectiveExecuteParams,
                    UserName,
                    seedContext,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                executeStatus = TaktExecuteStatus.Failed;
                errorInfo = TaktQuartzJobExecutionLogger.FormatErrorInfoForPersist(ex);
                executeMessage = string.IsNullOrWhiteSpace(executeMessage) ? "执行失败" : executeMessage;
                executionException = ex;
                TaktQuartzJobExecutionLogger.LogFailed(ex, task, stopwatch.ElapsedMilliseconds);
            }
            finally
            {
                stopwatch.Stop();
                try
                {
                    executionLog = await PersistExecutionResultAsync(
                        seedContext,
                        task,
                        executeTime,
                        stopwatch.ElapsedMilliseconds,
                        effectiveExecuteParams,
                        executeMessage,
                        errorInfo,
                        executeStatus,
                        context,
                        cancellationToken);
                }
                catch (Exception persistEx)
                {
                    TaktQuartzJobExecutionLogger.LogFailed(persistEx, task, stopwatch.ElapsedMilliseconds);
                    executionException ??= persistEx;
                }
            }
            if (executionLog != null)
            {
                TaktQuartzJobExecutionLogger.LogCompleted(
                    task,
                    executeStatus,
                    executionLog.ExecuteDuration,
                    executionLog.Id,
                    executeMessage,
                    errorInfo);
                await _quartzJobSignalRPushService.PushTaskExecutedAsync(task, executionLog, UserName);
            }
            if (executionException != null)
            {
                throw new JobExecutionException(executionException, refireImmediately: false);
            }
        }
    }

    /// <summary>
    /// 按任务类型分发执行逻辑
    /// </summary>
    /// <param name="task">定时任务实体</param>
    /// <param name="executeParams">有效执行参数</param>
    /// <param name="UserName">触发用户</param>
    /// <param name="seedContext">租户种子上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>执行摘要消息</returns>
    private async Task<string> DispatchAsync(
        TaktQuartzTask task,
        string? executeParams,
        string? UserName,
        TaktSeedContext seedContext,
        CancellationToken cancellationToken)
    {
        var taskType = (task.TaskType ?? string.Empty).Trim().ToLowerInvariant();
        return taskType switch
        {
            "assembly" => await ExecuteAssemblyAsync(
                task, executeParams, UserName, cancellationToken),
            "http" => await ExecuteHttpAsync(task, executeParams, cancellationToken),
            "sql" => await ExecuteSqlAsync(task, executeParams, seedContext, cancellationToken),
            _ => throw new InvalidOperationException($"不支持的任务类型：{task.TaskType}"),
        };
    }

    /// <summary>
    /// 执行程序集任务（HandlerKey = ClassName，校验 AssemblyName）
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="executeParams">执行参数</param>
    /// <param name="UserName">触发用户</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>执行摘要</returns>
    /// <summary>
    /// 执行程序集任务；若 ExecuteParams 含 targetDatabase，则本作用域仓储查询均落该库
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="executeParams">执行参数</param>
    /// <param name="UserName">触发用户</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>执行摘要</returns>
    private async Task<string> ExecuteAssemblyAsync(
        TaktQuartzTask task,
        string? executeParams,
        string? UserName,
        CancellationToken cancellationToken)
    {
        IDisposable? targetDbScope = null;
        try
        {
            var (_, targetDatabase) = TaktQuartzSyncExecuteParamsHelper.Parse(executeParams);
            if (!string.IsNullOrWhiteSpace(targetDatabase))
            {
                TaktQuartzSyncExecuteParamsHelper.ValidateDatabaseName(targetDatabase);
                var (resolvedTenant, connectionString) = ResolveTenantConnectionByDatabaseName(targetDatabase);
                targetDbScope = _sqlSugarContext.UseDatabaseConnection(resolvedTenant, connectionString);
                var liveDatabase = _sqlSugarContext.Db.Ado.Connection?.Database ?? string.Empty;
                TaktQuartzJobExecutionLogger.LogProgress(
                    $"Assembly 已切换目标库：TargetDb={targetDatabase}，LiveDb={liveDatabase}，ConfigTenant={resolvedTenant}",
                    task.Id);
            }
            var handler = _handlers.FirstOrDefault(x =>
                string.Equals(x.HandlerKey, task.ClassName, StringComparison.OrdinalIgnoreCase));
            if (handler == null)
            {
                throw new InvalidOperationException($"未找到 Quartz 任务处理器：{task.ClassName}");
            }
            TaktQuartzSchedulingHelper.EnsureAssemblyNameMatchesHandler(task, handler);
            var jobContext = new TaktQuartzJobContext
            {
                Task = task,
                ExecuteParams = executeParams,
                UserName = UserName,
            };
            await handler.ExecuteAsync(jobContext, cancellationToken);
            var assemblyHint = string.IsNullOrWhiteSpace(task.AssemblyName)
                ? handler.GetType().Assembly.GetName().Name
                : task.AssemblyName;
            var message = string.IsNullOrWhiteSpace(jobContext.ExecuteMessage)
                ? $"程序集任务已执行：{assemblyHint}/{handler.HandlerKey}"
                : jobContext.ExecuteMessage.Trim();
            if (!string.IsNullOrWhiteSpace(targetDatabase))
            {
                message = $"{message}，TargetDb={targetDatabase.Trim()}";
            }
            TaktQuartzJobExecutionLogger.LogProgress(message, task.Id);
            return message;
        }
        finally
        {
            targetDbScope?.Dispose();
        }
    }

    /// <summary>
    /// 执行 HTTP 任务（携带租户/公司请求头）
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="executeParams">执行参数（请求体）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>执行摘要</returns>
    private async Task<string> ExecuteHttpAsync(
        TaktQuartzTask task,
        string? executeParams,
        CancellationToken cancellationToken)
    {
        var method = string.IsNullOrWhiteSpace(task.RequestMethod) ? "GET" : task.RequestMethod.Trim().ToUpperInvariant();
        using var request = new HttpRequestMessage(new HttpMethod(method), task.ApiUrl);
        request.Headers.TryAddWithoutValidation("X-Tenant-Code", task.TenantCode);
        request.Headers.TryAddWithoutValidation("X-Company-Code", task.CompanyCode);
        if (!string.IsNullOrWhiteSpace(executeParams)
            && method is "POST" or "PUT" or "PATCH")
        {
            request.Content = new StringContent(executeParams);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        }
        var client = _httpClientFactory.CreateClient(TaktQuartzConstants.HttpClientName);
        using var response = await client.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"HTTP 任务失败：{(int)response.StatusCode} {response.ReasonPhrase}，URL={task.ApiUrl}");
        }
        var message =
            $"HTTP {method} 成功，状态码 {(int)response.StatusCode}，URL={task.ApiUrl}，响应长度 {body.Length}";
        TaktQuartzJobExecutionLogger.LogProgress(message, task.Id);
        return message;
    }

    /// <summary>
    /// 执行 SQL 任务：只读 SELECT → 查询；含 MERGE/DML 的脚本 → ExecuteCommand
    /// 同步脚本（Quartz/sync_*.sql）须在 ExecuteParams 提供 targetDatabase，并在该库连接上执行
    /// </summary>
    /// <param name="task">定时任务</param>
    /// <param name="executeParams">有效执行参数</param>
    /// <param name="seedContext">任务所属租户上下文（写日志仍用此库）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>执行摘要</returns>
    private async Task<string> ExecuteSqlAsync(
        TaktQuartzTask task,
        string? executeParams,
        TaktSeedContext seedContext,
        CancellationToken cancellationToken)
    {
        var (sourceDatabase, targetDatabase) = TaktQuartzSyncExecuteParamsHelper.Parse(executeParams);
        var costingPeriod = TaktBomQuartzExecuteParamsHelper.TryResolveCostingPeriodKey(
            executeParams,
            out var periodKey)
            ? periodKey
            : null;
        var isSyncScript = IsQuartzSyncSqlScript(task.SqlScript);
        if (isSyncScript)
        {
            if (string.IsNullOrWhiteSpace(targetDatabase))
            {
                throw new InvalidOperationException(
                    "同步 SQL 任务 ExecuteParams 须提供 targetDatabase（如 {\"targetDatabase\":\"zTakt_000_Dev\"}）");
            }
            TaktQuartzSyncExecuteParamsHelper.ValidateDatabaseName(targetDatabase);
        }
        var database = _configuration.RequireDatabase();
        // 优先任务自身 PlantCode；空则按 Database:CompanyCodes↔PlantCodes 映射（与种子一致）
        var plantCode = !string.IsNullOrWhiteSpace(task.PlantCode)
            ? task.PlantCode.Trim()
            : database.GetPlantCodeForCompanyCode(task.CompanyCode);
        var cultureCode = string.IsNullOrWhiteSpace(task.CultureCode)
            ? database.GetCultureCodeForCompanyCode(task.CompanyCode)
            : task.CultureCode.Trim();
        var sql = await TaktQuartzSqlScriptHelper.ResolveExecutableSqlAsync(
            task.SqlScript,
            task.TenantCode,
            task.CompanyCode,
            cultureCode,
            plantCode,
            sourceDatabase,
            targetDatabase,
            costingPeriod,
            cancellationToken);
        var dbHint = string.IsNullOrWhiteSpace(targetDatabase)
            ? string.Empty
            : $"，TargetDb={targetDatabase}";
        if (!string.IsNullOrWhiteSpace(sourceDatabase))
        {
            dbHint += $"，SourceDb={sourceDatabase}";
        }
        if (!string.IsNullOrWhiteSpace(costingPeriod))
        {
            dbHint += $"，CostingPeriod={costingPeriod}";
        }
        TaktQuartzJobExecutionLogger.LogProgress(
            $"SQL 已解析，路径={task.SqlScript}，SqlLength={sql.Length}{dbHint}",
            task.Id);
        ISqlSugarClient executeDb = seedContext.Db;
        SqlSugarClient? targetClient = null;
        try
        {
            if (isSyncScript && !string.IsNullOrWhiteSpace(targetDatabase))
            {
                var (tenantCode, connectionString) = ResolveTenantConnectionByDatabaseName(targetDatabase);
                targetClient = TaktSqlSugarConnectionHelper.CreateClient(
                    _configuration.GetSugarDbType(),
                    tenantCode,
                    connectionString);
                executeDb = targetClient;
            }
            if (IsReadOnlySelectScript(sql))
            {
                TaktSqlExecutorValidator.Validate(sql, TaktSqlExecuteOptions.ReadOnlyDefault);
                var rows = await TaktRepositoryReadOnlySql.QueryAsync(executeDb, sql, null, cancellationToken);
                var message = $"SQL 查询成功，路径={task.SqlScript}，返回 {rows.Count} 行{dbHint}";
                TaktQuartzJobExecutionLogger.LogProgress(message, task.Id);
                return message;
            }
            TaktSqlExecutorValidator.Validate(sql, TaktSqlExecuteOptions.NonQueryDefault);
            var previousTimeout = executeDb.Ado.CommandTimeOut;
            executeDb.Ado.CommandTimeOut = ResolveSqlCommandTimeoutSeconds();
            try
            {
                var message = await TaktQuartzSqlResultReader.ExecuteAndFormatSummaryAsync(
                    executeDb,
                    sql,
                    task.SqlScript,
                    progress => TaktQuartzJobExecutionLogger.LogProgress(progress, task.Id),
                    cancellationToken);
                message = string.IsNullOrWhiteSpace(dbHint) ? message : $"{message}{dbHint}";
                TaktQuartzJobExecutionLogger.LogProgress(message, task.Id);
                return message;
            }
            finally
            {
                executeDb.Ado.CommandTimeOut = previousTimeout;
            }
        }
        finally
        {
            targetClient?.Dispose();
        }
    }

    /// <summary>
    /// 是否为 Quartz 同步脚本（Quartz/sync_*.sql，不含 sync_data_create_tables）
    /// </summary>
    /// <param name="sqlScript">SqlScript 路径</param>
    /// <returns>是同步脚本则为 true</returns>
    private static bool IsQuartzSyncSqlScript(string? sqlScript)
    {
        if (string.IsNullOrWhiteSpace(sqlScript))
        {
            return false;
        }
        var normalized = sqlScript.Trim().Replace('\\', '/');
        if (!normalized.StartsWith("Quartz/sync_", StringComparison.OrdinalIgnoreCase)
            || !normalized.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        return !normalized.EndsWith("sync_data_create_tables.sql", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 读取 Quartz:SqlCommandTimeoutSeconds（缺省 7200；0=无限制；负值回退默认）
    /// </summary>
    /// <returns>命令超时秒数</returns>
    private int ResolveSqlCommandTimeoutSeconds()
    {
        var configured = _configuration.GetValue<int?>("Quartz:SqlCommandTimeoutSeconds");
        if (!configured.HasValue)
        {
            return TaktQuartzConstants.DefaultSqlCommandTimeoutSeconds;
        }
        return configured.Value < 0
            ? TaktQuartzConstants.DefaultSqlCommandTimeoutSeconds
            : configured.Value;
    }

    /// <summary>
    /// 按连接串 Database= 段匹配已配置 Tenant_* 连接
    /// </summary>
    /// <param name="databaseName">目标库展示名</param>
    /// <returns>租户编码与连接字符串</returns>
    private (string TenantCode, string ConnectionString) ResolveTenantConnectionByDatabaseName(string databaseName)
    {
        TaktQuartzSyncExecuteParamsHelper.ValidateDatabaseName(databaseName);
        var wanted = databaseName.Trim();
        foreach (var (tenantCode, connectionString) in _configuration.GetTenantConnections())
        {
            var displayName = ExtractDatabaseDisplayName(connectionString, tenantCode);
            if (string.Equals(displayName, wanted, StringComparison.OrdinalIgnoreCase))
            {
                return (tenantCode, connectionString);
            }
        }
        // TenantCodes 未含 900 等时，仍允许按 ConnectionStrings:Tenant_* 全量匹配
        foreach (var child in _configuration.GetSection("ConnectionStrings").GetChildren())
        {
            if (!child.Key.StartsWith("Tenant_", StringComparison.OrdinalIgnoreCase)
                || string.IsNullOrWhiteSpace(child.Value))
            {
                continue;
            }
            var tenantCode = child.Key["Tenant_".Length..].Trim();
            if (string.IsNullOrEmpty(tenantCode))
            {
                continue;
            }
            var displayName = ExtractDatabaseDisplayName(child.Value, tenantCode);
            if (string.Equals(displayName, wanted, StringComparison.OrdinalIgnoreCase))
            {
                return (tenantCode, child.Value);
            }
        }
        throw new InvalidOperationException(
            $"未找到 Database={wanted} 对应的 ConnectionStrings:Tenant_* 配置");
    }

    /// <summary>
    /// 从连接字符串提取 Database= 段
    /// </summary>
    /// <param name="connectionString">连接字符串</param>
    /// <param name="tenantCode">回退展示用租户编码</param>
    /// <returns>数据库名</returns>
    private static string ExtractDatabaseDisplayName(string connectionString, string tenantCode)
    {
        const string key = "Database=";
        var idx = connectionString.IndexOf(key, StringComparison.OrdinalIgnoreCase);
        if (idx < 0)
        {
            return $"Takt_{tenantCode}";
        }
        var start = idx + key.Length;
        var end = connectionString.IndexOf(';', start);
        var dbName = end > start
            ? connectionString[start..end]
            : connectionString[start..];
        return string.IsNullOrWhiteSpace(dbName) ? $"Takt_{tenantCode}" : dbName.Trim();
    }

    /// <summary>
    /// 是否为只读 SELECT/WITH 查询（无 DML/DDL 关键字）
    /// </summary>
    /// <param name="sql">SQL 文本</param>
    /// <returns>是则走查询路径</returns>
    private static bool IsReadOnlySelectScript(string sql)
    {
        var trimmed = sql.TrimStart();
        while (trimmed.StartsWith("/*", StringComparison.Ordinal))
        {
            var end = trimmed.IndexOf("*/", StringComparison.Ordinal);
            if (end < 0)
            {
                break;
            }
            trimmed = trimmed[(end + 2)..].TrimStart();
        }
        if (!trimmed.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase)
            && !trimmed.StartsWith("WITH", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        return !System.Text.RegularExpressions.Regex.IsMatch(
            sql,
            @"\b(INSERT|UPDATE|DELETE|DROP|ALTER|TRUNCATE|EXEC|EXECUTE|MERGE|CREATE|GRANT|REVOKE)\b",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
            | System.Text.RegularExpressions.RegexOptions.CultureInvariant);
    }

    /// <summary>
    /// 持久化执行日志并更新任务统计字段
    /// </summary>
    /// <param name="seedContext">租户上下文</param>
    /// <param name="task">定时任务</param>
    /// <param name="executeTime">执行时间</param>
    /// <param name="durationMs">耗时毫秒</param>
    /// <param name="executeParams">执行参数快照</param>
    /// <param name="executeMessage">执行消息</param>
    /// <param name="errorInfo">错误信息</param>
    /// <param name="executeStatus">执行状态</param>
    /// <param name="context">Quartz 上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>写入后的执行日志</returns>
    private async Task<TaktQuartzLog> PersistExecutionResultAsync(
        TaktSeedContext seedContext,
        TaktQuartzTask task,
        DateTime executeTime,
        long durationMs,
        string? executeParams,
        string executeMessage,
        string errorInfo,
        TaktExecuteStatus executeStatus,
        IJobExecutionContext context,
        CancellationToken cancellationToken)
    {
        var (executeIp, executeHost) = TaktServerHostHelper.ResolveQuartzExecuteEndpoint();
        var log = TaktQuartzSchedulingHelper.BuildQuartzLog(
            task,
            executeTime,
            durationMs,
            executeParams,
            executeMessage,
            string.IsNullOrWhiteSpace(errorInfo) ? null : errorInfo,
            executeStatus,
            executeIp,
            executeHost);
        TaktCompanyScopeFillHelper.ApplyCompanyScopeFromConfiguration(
            log,
            log.TenantCode,
            log.CompanyCode,
            _configuration);
        log.Id = await TaktPrimaryKeyInsertHelper.InsertEntityReturnInt64Async(
            seedContext.Db,
            log,
            TaktPrimaryKeyInsertHelper.RuntimeOptions,
            cancellationToken);
        task.ExecuteCount = checked(task.ExecuteCount + 1);
        task.LastRunAt = executeTime;
        task.NextRunAt = TaktQuartzSchedulingHelper.ResolveNextRunAt(context);
        await seedContext.Db.Updateable(task)
            .UpdateColumns(x => new { x.ExecuteCount, x.LastRunAt, x.NextRunAt })
            .ExecuteCommandAsync(cancellationToken);
        return log;
    }
}
