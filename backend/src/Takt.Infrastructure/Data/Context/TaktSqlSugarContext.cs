// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Context
// 文件名称：TaktSqlSugarContext.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SqlSugar数据库上下文，管理所有实体集合和数据库连接
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

// Takt 内部引用 - ORM 框架
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Infrastructure.Extensions;
using Takt.Infrastructure.Services;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Context;

/// <summary>
/// SqlSugar数据库上下文（运行时 ORM；启动步骤 2 负责租户库与表结构初始化）
/// </summary>
public class TaktSqlSugarContext : IDisposable
{
    /// <summary>
    /// 构造时绑定的默认租户连接（UseDatabaseConnection 结束后恢复）
    /// </summary>
    private readonly ISqlSugarClient _defaultClient;
    /// <summary>
    /// 当前生效客户端（默认或临时切库）
    /// </summary>
    private ISqlSugarClient _db;
    /// <summary>
    /// UseDatabaseConnection 创建的临时客户端（Dispose 切回时释放）
    /// </summary>
    private SqlSugarClient? _temporaryClient;
    /// <summary>
    /// 是否已释放
    /// </summary>
    private bool _disposed;
    /// <summary>
    /// 应用程序配置
    /// </summary>
    private readonly IConfiguration _configuration;
    /// <summary>
    /// 当前上下文绑定的租户编码
    /// </summary>
    private readonly string _tenantCode;
    /// <summary>
    /// 已解析的 SqlSugar 数据库类型（构造时从配置映射一次）
    /// </summary>
    private readonly DbType _sugarDbType;
    /// <summary>
    /// HTTP 上下文（审计 AOP；临时切库重建客户端时复用）
    /// </summary>
    private readonly IHttpContextAccessor? _httpContextAccessor;

    /// <summary>
    /// SqlSugar客户端实例
    /// </summary>
    public ISqlSugarClient Db => _db;
    
    /// <summary>
    /// 当前租户编码
    /// </summary>
    public string TenantCode => _tenantCode;

    /// <summary>
    /// 所有实体类型集合（通过反射自动扫描）
    /// </summary>
    private static readonly Type[] _entityTypes;

    /// <summary>
    /// 静态构造函数:自动扫描所有实体
    /// </summary>
    static TaktSqlSugarContext()
    {
        // 从 Domain 程序集自动扫描所有继承自实体基类的类型
        var domainAssembly = typeof(TaktTenantEntityBase).Assembly;
        _entityTypes = domainAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract &&
                       (typeof(TaktTenantCoreEntityScopeBase).IsAssignableFrom(t) ||
                        typeof(TaktCompanyEntityScopeBase).IsAssignableFrom(t) ||
                        typeof(TaktApprovalEntityScopeBase).IsAssignableFrom(t)))
            .ToArray();

        TaktSqlSugarAuditAop.RegisterGlobalDiffLogSwitch();
    }

    /// <summary>
    /// 构造函数（必须指定租户编码）
    /// </summary>
    /// <param name="configuration">配置信息</param>
    /// <param name="tenantCode">租户编码（必须提供，不允许默认值）</param>
    /// <param name="httpContextAccessor">HTTP 上下文（差异/操作日志审计可选）</param>
    /// <exception cref="ArgumentException">租户编码为空时抛出</exception>
    public TaktSqlSugarContext(
        IConfiguration configuration,
        string tenantCode,
        IHttpContextAccessor? httpContextAccessor = null)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            throw new ArgumentException("租户编码不能为空，必须显式指定租户", nameof(tenantCode));
        }
        
        _configuration = configuration;
        _tenantCode = tenantCode;
        _httpContextAccessor = httpContextAccessor;
        _sugarDbType = configuration.GetSugarDbType();
        var connectionString = _configuration.GetConnectionString($"Tenant_{tenantCode}");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"未找到租户 {tenantCode} 的连接字符串配置。请在 appsettings.json 中配置 'ConnectionStrings:Tenant_{tenantCode}'。");
        }
        _db = TaktSqlSugarConnectionHelper.CreateClient(_sugarDbType, tenantCode, connectionString);
        _defaultClient = _db;
        
        // SqlSugar 内置雪花ID：当实体主键为 long 类型时，插入时自动生成
        // 无需手动配置，SqlSugar 会自动处理

        TaktSqlSugarAuditAop.ConfigureClient(_db, _httpContextAccessor, _configuration);

        // 开启日志（开发环境）
#if DEBUG
        _db.Aop.OnLogExecuting = (sql, pars) =>
        {
            TaktLogger.Debug("SQL: {Sql}, Parameters: {Parameters}", sql, string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value) ?? new string[0]));
        };
#endif
    }

    /// <summary>
    /// 临时切换到指定连接（Quartz 选中 targetDatabase 后，本作用域内仓储查询均落该库）
    /// </summary>
    /// <param name="configTenantCode">ConnectionConfig.ConfigId（Tenant_* 编码）</param>
    /// <param name="connectionString">目标库连接串</param>
    /// <returns>Dispose 时切回默认连接并释放临时客户端</returns>
    /// <exception cref="InvalidOperationException">已存在未结束的临时切库</exception>
    public IDisposable UseDatabaseConnection(string configTenantCode, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configTenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        if (_temporaryClient != null)
        {
            throw new InvalidOperationException("已存在未结束的临时数据库连接切换");
        }
        // 独立 ConfigId：选中库的连接不能复用请求租户客户端的静态连接，否则查询仍落在原库
        var next = TaktSqlSugarConnectionHelper.CreateClient(
            _sugarDbType,
            configTenantCode.Trim(),
            connectionString,
            configId: "quartz_" + configTenantCode.Trim());
        TaktSqlSugarAuditAop.ConfigureClient(next, _httpContextAccessor, _configuration);
        _temporaryClient = next;
        _db = next;
        return new TemporaryDatabaseConnectionScope(this);
    }

    /// <summary>
    /// 结束临时切库：释放临时客户端并恢复默认连接
    /// </summary>
    private void EndTemporaryDatabaseConnection()
    {
        if (_temporaryClient != null)
        {
            _temporaryClient.Close();
            _temporaryClient.Dispose();
            _temporaryClient = null;
        }
        _db = _defaultClient;
    }

    /// <summary>
    /// 获取实体的Queryable对象
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <returns>Queryable对象</returns>
    public ISugarQueryable<T> Query<T>() where T : class, new()
    {
        return _db.Queryable<T>();
    }

    /// <summary>
    /// UseDatabaseConnection 的 Dispose 作用域
    /// </summary>
    private sealed class TemporaryDatabaseConnectionScope : IDisposable
    {
        private readonly TaktSqlSugarContext _owner;
        private bool _disposed;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="owner">上下文</param>
        public TemporaryDatabaseConnectionScope(TaktSqlSugarContext owner)
        {
            _owner = owner;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            _owner.EndTemporaryDatabaseConnection();
        }
    }

    /// <summary>
    /// 获取所有实体类型（用于初始化数据库）
    /// </summary>
    /// <returns>Domain 程序集扫描得到的实体类型只读列表</returns>
    public static IReadOnlyList<Type> EntityTypes => _entityTypes;

    // ========================================
    // 数据库初始化方法
    // ========================================

    /// <summary>
    /// 初始化所有租户业务数据库（创建库 + 表结构，步骤 2）
    /// </summary>
    /// <param name="configuration">应用程序配置</param>
    /// <exception cref="InvalidOperationException">未配置任何租户连接字符串时抛出</exception>
    public static void InitializeAllTenantDatabases(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        try
        {
            var tenantConnections = GetTenantConnectionStrings(configuration);
            if (tenantConnections.Count == 0)
            {
                throw new InvalidOperationException("未找到任何租户连接字符串配置");
            }

            TaktLogger.Information("发现 {Count} 个租户数据库，开始初始化...", tenantConnections.Count);
            foreach (var (tenantCode, _) in tenantConnections)
            {
                TaktLogger.Information("[租户 {TenantCode}] 开始初始化数据库...", tenantCode);
                InitializeSingleDatabase(configuration, tenantCode);
            }

            TaktLogger.Information("✓ 所有租户数据库初始化完成！共 {Count} 个数据库。", tenantConnections.Count);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "数据库初始化失败");
            throw;
        }
    }

    /// <summary>
    /// 初始化数据库（创建表结构）
    /// 注意：生产环境应使用迁移脚本，不应自动建表
    /// </summary>
    /// <exception cref="InvalidOperationException">未配置任何租户连接字符串时抛出</exception>
    public void InitializeDatabase()
    {
        InitializeAllTenantDatabases(_configuration);
    }

    /// <summary>
    /// 校验所有租户业务库与实体表已就绪（步骤 3 写种子前调用；<c>InitDb=false</c> 且 <c>SeedData=true</c> 时缺失即抛错）
    /// </summary>
    /// <param name="configuration">应用程序配置</param>
    /// <exception cref="InvalidOperationException">库不存在、无法连接或缺少业务表时抛出</exception>
    public static void EnsureAllTenantDatabasesReady(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var tenantConnections = GetTenantConnectionStrings(configuration);
        if (tenantConnections.Count == 0)
        {
            throw new InvalidOperationException("未找到任何租户连接字符串配置，无法执行种子初始化");
        }

        foreach (var (tenantCode, _) in tenantConnections)
        {
            EnsureSingleTenantDatabaseReady(configuration, tenantCode);
        }
    }
        
    /// <summary>
    /// 初始化单个租户数据库
    /// </summary>
    /// <param name="configuration">应用程序配置</param>
    /// <param name="tenantCode">租户编码</param>
    private static void InitializeSingleDatabase(IConfiguration configuration, string tenantCode)
    {
        try
        {
            using var tempDb = CreateBootstrapClient(configuration, tenantCode);
                
            // 创建数据库（如果不存在）
            tempDb.DbMaintenance.CreateDatabase();
            TaktLogger.Information("  ✓ 数据库创建成功");
                            
            // 自动初始化所有实体表
            TaktLogger.Information("  ℹ 开始初始化 {Count} 个数据表...", _entityTypes.Length);
            foreach (var entityType in _entityTypes)
            {
                try
                {
                    tempDb.CodeFirst.InitTables(entityType);
                    TaktLogger.Information("    ✓ {EntityName} -> {TableName}", entityType.Name, tempDb.EntityMaintenance.GetEntityInfo(entityType).DbTableName);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"CodeFirst 建表失败：实体 {entityType.FullName}（{entityType.Name}）",
                        ex);
                }
            }
                            
            TaktLogger.Information("  ✓ [租户 {TenantCode}] 数据库表初始化成功！共 {Count} 个表。", tenantCode, _entityTypes.Length);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "  ✗ [租户 {TenantCode}] 数据库初始化失败", tenantCode);
            throw;
        }
    }

    /// <summary>
    /// 校验单个租户业务库可连接且全部实体表已存在
    /// </summary>
    /// <param name="configuration">应用程序配置</param>
    /// <param name="tenantCode">租户编码</param>
    /// <exception cref="InvalidOperationException">库或表缺失时抛出</exception>
    private static void EnsureSingleTenantDatabaseReady(IConfiguration configuration, string tenantCode)
    {
        using var tempDb = CreateBootstrapClient(configuration, tenantCode);
        try
        {
            tempDb.Ado.GetInt("SELECT 1");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"租户 {tenantCode} 的业务数据库不存在或无法连接（InitDb=false 且 SeedData=true 时须先建库建表）。请将 Init.InitDb 设为 true 后重启，或手动创建数据库。连接：Tenant_{tenantCode}",
                ex);
        }

        var missingTables = new List<string>(_entityTypes.Length);
        foreach (var entityType in _entityTypes)
        {
            var tableName = tempDb.EntityMaintenance.GetEntityInfo(entityType).DbTableName;
            if (string.IsNullOrWhiteSpace(tableName) || !tempDb.DbMaintenance.IsAnyTable(tableName, false))
            {
                missingTables.Add(string.IsNullOrWhiteSpace(tableName) ? entityType.Name : tableName);
            }
        }

        if (missingTables.Count == 0)
        {
            return;
        }

        const int previewCount = 15;
        var preview = string.Join(", ", missingTables.Take(previewCount));
        var suffix = missingTables.Count > previewCount ? $" 等共 {missingTables.Count} 张" : string.Empty;
        throw new InvalidOperationException(
            $"租户 {tenantCode} 缺少业务数据表（InitDb=false 且 SeedData=true 时须先有完整表结构）。缺失：{preview}{suffix}。请将 Init.InitDb 设为 true 后重启。");
    }

    /// <summary>
    /// 创建启动/bootstrap 阶段使用的临时 SqlSugar 客户端
    /// </summary>
    /// <param name="configuration">应用程序配置</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>SqlSugar 客户端</returns>
    private static SqlSugarClient CreateBootstrapClient(IConfiguration configuration, string tenantCode)
    {
        var connectionString = configuration.GetConnectionString($"Tenant_{tenantCode}");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"未找到租户 {tenantCode} 的连接字符串配置。请在 appsettings.json 中配置 'ConnectionStrings:Tenant_{tenantCode}'。");
        }
        return TaktSqlSugarConnectionHelper.CreateClient(
            configuration.GetSugarDbType(),
            tenantCode,
            connectionString);
    }
        
    /// <summary>
    /// 获取所有租户连接字符串
    /// </summary>
    /// <param name="configuration">应用程序配置</param>
    /// <returns>租户编码和连接字符串的列表</returns>
    private static List<(string TenantCode, string ConnectionString)> GetTenantConnectionStrings(IConfiguration configuration)
    {
        return configuration.GetTenantConnections();
    }

    /// <summary>
    /// 获取单条插入操作对象
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="insertObj">待插入实体</param>
    /// <returns>Insertable 对象</returns>
    public IInsertable<T> Insertable<T>(T insertObj) where T : class, new()
    {
        return _db.Insertable(insertObj);
    }

    /// <summary>
    /// 获取批量插入对象
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="insertObjs">实体列表</param>
    /// <returns>Insertable对象</returns>
    public IInsertable<T> Insertable<T>(List<T> insertObjs) where T : class, new()
    {
        return _db.Insertable(insertObjs);
    }

    /// <summary>
    /// 获取可更新对象
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="updateObj">实体对象</param>
    /// <returns>Updateable对象</returns>
    public IUpdateable<T> Updateable<T>(T updateObj) where T : class, new()
    {
        return _db.Updateable(updateObj);
    }

    /// <summary>
    /// 获取批量更新对象
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="updateObjs">实体列表</param>
    /// <returns>Updateable对象</returns>
    public IUpdateable<T> Updateable<T>(List<T> updateObjs) where T : class, new()
    {
        return _db.Updateable(updateObjs);
    }

    /// <summary>
    /// 获取可删除对象
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="deleteObj">实体对象</param>
    /// <returns>Deleteable对象</returns>
    public IDeleteable<T> Deleteable<T>(T deleteObj) where T : class, new()
    {
        return _db.Deleteable(deleteObj);
    }

    /// <summary>
    /// 根据ID删除
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="id">主键ID</param>
    /// <returns>Deleteable对象</returns>
    public IDeleteable<T> Deleteable<T>(long id) where T : class, new()
    {
        return _db.Deleteable<T>(id);
    }

    /// <summary>
    /// 批量根据ID删除
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="ids">主键ID列表</param>
    /// <returns>Deleteable对象</returns>
    public IDeleteable<T> Deleteable<T>(List<long> ids) where T : class, new()
    {
        return _db.Deleteable<T>().In(ids);
    }

    /// <summary>
    /// 开启事务
    /// </summary>
    public void BeginTran()
    {
        _db.Ado.BeginTran();
    }

    /// <summary>
    /// 提交事务
    /// </summary>
    public void CommitTran()
    {
        _db.Ado.CommitTran();
    }

    /// <summary>
    /// 回滚事务
    /// </summary>
    public void RollbackTran()
    {
        _db.Ado.RollbackTran();
    }

    /// <summary>
    /// 释放 SqlSugar 客户端与连接资源（请求作用域结束时由 DI 调用）
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 释放托管资源
    /// </summary>
    /// <param name="disposing">是否释放托管对象</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            if (_temporaryClient != null)
            {
                _temporaryClient.Close();
                _temporaryClient.Dispose();
                _temporaryClient = null;
            }
            if (_defaultClient is SqlSugarClient defaultClient)
            {
                defaultClient.Close();
                defaultClient.Dispose();
            }
            _db = _defaultClient;
        }

        _disposed = true;
    }
}

/// <summary>
/// SqlSugar 差异日志（AOP）与操作日志持久化辅助（挂接于 TaktSqlSugarContext，供三级仓储共用同一客户端）。
/// </summary>
/// <remarks>
/// <para><strong>差异审计仅使用 SqlSugar 标准能力</strong>（与 EF Core ChangeTracker 同类，见官方文档「差异日志功能」）：</para>
/// <list type="number">
/// <item><description>3.2 批量：RegisterGlobalDiffLogSwitch 注册 <c>StaticConfig.CompleteInsertableFunc/CompleteUpdateableFunc/CompleteDeleteableFunc</c>，自动对 Insertable/Updateable/Deleteable 调用 <c>EnableDiffLogEvent</c>。</description></item>
/// <item><description>3.1 事件：每个租户 ConfigureClient 挂接 <c>db.Aop.OnDiffLogEvent</c>，在回调中读取 DiffLogModel 的 BeforeData/AfterData/Sql/BusinessData/Time/DiffType 并写入 TaktDeltaLog。</description></item>
/// <item><description>差异字段：BuildDiffJson 在事件内对比 Before/After 的 Columns（官方「只获取差异部分」推荐做法）。</description></item>
/// </list>
/// <para>StatisticsLoggingEntityNamespace 下实体（含 TaktDeltaLog）的 CUD 及 Statistics.Logging API 请求排除，避免审计表自写递归。</para>
/// </remarks>
internal static class TaktSqlSugarAuditAop
{
    /// <summary>
    /// 统计日志实体命名空间（对应 Domain <c>Entities/Statistics/Logging</c> 目录）
    /// </summary>
    private static readonly string StatisticsLoggingEntityNamespace = typeof(TaktDeltaLog).Namespace
        ?? "Takt.Domain.Entities.Statistics.Logging";

    /// <summary>
    /// 统计日志控制器命名空间（对应 WebApi <c>Controllers/Statistics/Logging</c> 目录）
    /// </summary>
    private const string StatisticsLoggingControllerNamespace = "Takt.WebApi.Controllers.Statistics.Logging";

    /// <summary>
    /// 统计日志实体物理表名缓存（懒加载，避免重复反射）
    /// </summary>
    private static readonly Lazy<HashSet<string>> ExcludedStatisticsLoggingTableNames =
        new(BuildExcludedStatisticsLoggingTableNames);

    /// <summary>
    /// 统计日志 API 路由段缓存（懒加载，与控制器复数命名对齐）
    /// </summary>
    private static readonly Lazy<HashSet<string>> ExcludedStatisticsLoggingApiRouteSegments =
        new(BuildExcludedStatisticsLoggingApiRouteSegments);

    /// <summary>
    /// 抑制审计写入标志（写入操作/AOP 日志时避免递归触发）
    /// </summary>
    private static readonly AsyncLocal<bool> SuppressAuditWrite = new();

    /// <summary>
    /// 抑制差异日志 AOP（Quartz 批量同步等：脚本已写摘要，勿再为 Message 等实体写逐行差异）
    /// </summary>
    private static readonly AsyncLocal<bool> SuppressDiffLog = new();

    /// <summary>
    /// 进入差异日志 AOP 抑制作用域（Dispose 后恢复）
    /// </summary>
    /// <returns>作用域</returns>
    public static IDisposable BeginSuppressDiffLog()
    {
        var previous = SuppressDiffLog.Value;
        SuppressDiffLog.Value = true;
        return new SuppressDiffLogScope(previous);
    }

    /// <summary>
    /// 差异日志抑制作用域
    /// </summary>
    private sealed class SuppressDiffLogScope : IDisposable
    {
        private readonly bool _previous;
        private bool _disposed;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="previous">进入前标志</param>
        public SuppressDiffLogScope(bool previous)
        {
            _previous = previous;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            SuppressDiffLog.Value = _previous;
            _disposed = true;
        }
    }

    /// <summary>
    /// 程序启动时注册 Insertable/Updateable/Deleteable 自动启用差异日志（SqlSugar 官方 StaticConfig 批量配置）
    /// </summary>
    public static void RegisterGlobalDiffLogSwitch()
    {
        if (StaticConfig.CompleteInsertableFunc != null
            && StaticConfig.CompleteUpdateableFunc != null
            && StaticConfig.CompleteDeleteableFunc != null)
        {
            return;
        }

        StaticConfig.CompleteInsertableFunc = TryEnableDiffLogEvent;
        StaticConfig.CompleteUpdateableFunc = TryEnableDiffLogEvent;
        StaticConfig.CompleteDeleteableFunc = TryEnableDiffLogEvent;
    }

    /// <summary>
    /// 为当前 SqlSugar 客户端挂接 OnDiffLogEvent（须在执行业务 SQL 前、同一客户端实例上配置）
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="httpContextAccessor">HTTP 上下文访问器</param>
    /// <param name="configuration">应用配置（Quartz/后台无 RequestServices 时回退解析 Company↔Plant）</param>
    public static void ConfigureClient(
        ISqlSugarClient db,
        IHttpContextAccessor? httpContextAccessor,
        IConfiguration? configuration = null)
    {
        db.Aop.OnDiffLogEvent = diff => PersistDiffLog(db, diff, httpContextAccessor, configuration);
    }

    /// <summary>
    /// 将 HTTP 操作写入操作日志表（由请求日志中间件调用）
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="elapsedMs">耗时毫秒</param>
    /// <param name="statusCode">HTTP 状态码</param>
    /// <param name="requestBody">请求体快照（可选）</param>
    public static void TryPersistOperLog(
        ISqlSugarClient db,
        HttpContext httpContext,
        long elapsedMs,
        int statusCode,
        string? requestBody)
    {
        if (SuppressAuditWrite.Value || !ShouldPersistOperLog(httpContext))
        {
            return;
        }

        try
        {
            var client = TaktHttpAuditHelper.ResolveClientLogContext(httpContext);
            var operLog = new TaktOperLog
            {
                TenantCode = TaktUserContext.TryResolveTenantCode(httpContext) ?? string.Empty,
                CompanyCode = TaktUserContext.TryResolveCompanyCode(httpContext) ?? string.Empty,
                UserName = TaktUserContext.ResolveAuditUserName(httpContext),
                Remark = TaktUserContext.ResolveAuditContextRemark(httpContext),
                OperModule = TaktHttpAuditHelper.ResolveOperModule(httpContext.Request.Path.Value),
                OperType = TaktHttpAuditHelper.ResolveOperType(httpContext),
                OperMethod = httpContext.GetEndpoint()?.DisplayName ?? string.Empty,
                RequestMethod = httpContext.Request.Method ?? string.Empty,
                OperUrl = BuildOperUrl(httpContext) ?? string.Empty,
                RequestParam = MaskSensitiveJson(requestBody) ?? string.Empty,
                OperStatus = statusCode >= 400 ? (int)TaktExecuteStatus.Failed : (int)TaktExecuteStatus.Success,
                OperIp = client.Ip,
                OperLocation = client.Location,
                UserAgent = client.UserAgent,
                Browser = client.Browser,
                Os = client.OperatingSystem,
                DeviceType = client.DeviceType,
                OperTime = DateTime.Now,
                ElapsedTime = (int)Math.Min(int.MaxValue, elapsedMs),
            };
            operLog.ApplyCreate(
                TaktUserContext.TryResolveUserId(httpContext));

            if (string.IsNullOrWhiteSpace(operLog.TenantCode) || string.IsNullOrWhiteSpace(operLog.CompanyCode))
            {
                return;
            }

            TryApplyCompanyScope(
                httpContext,
                operLog,
                operLog.TenantCode,
                operLog.CompanyCode,
                fallbackConfiguration: null);

            SuppressAuditWrite.Value = true;
            TaktPrimaryKeyInsertHelper.InsertEntitySync(db, operLog, TaktPrimaryKeyInsertHelper.RuntimeOptions);
        }
        catch (Exception ex)
        {
            TaktLogger.Warning(ex, "[SqlSugarAudit] 写入操作日志失败");
        }
        finally
        {
            SuppressAuditWrite.Value = false;
        }
    }

    /// <summary>
    /// 将 SqlSugar 差异日志持久化到 TaktDeltaLog 表
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="diff">差异日志模型</param>
    /// <param name="httpContextAccessor">HTTP 上下文访问器</param>
    /// <param name="fallbackConfiguration">无 RequestServices 时用于 Company↔Plant 映射的配置</param>
    private static void PersistDiffLog(
        ISqlSugarClient db,
        DiffLogModel diff,
        IHttpContextAccessor? httpContextAccessor,
        IConfiguration? fallbackConfiguration)
    {
        if (SuppressAuditWrite.Value || SuppressDiffLog.Value)
        {
            return;
        }

        var httpContext = httpContextAccessor?.HttpContext;
        if (httpContext == null)
        {
            return;
        }

        var tableName = ResolveTableName(diff);
        if (string.IsNullOrWhiteSpace(tableName) || IsExcludedStatisticsLoggingTableName(tableName))
        {
            return;
        }

        try
        {
            var client = TaktHttpAuditHelper.ResolveClientLogContext(httpContext);
            var beforeJson = SerializeDiffTables(diff.BeforeData);
            var afterJson = SerializeDiffTables(diff.AfterData);
            var deltaLog = new TaktDeltaLog
            {
                TenantCode = TaktUserContext.TryResolveTenantCode(httpContext) ?? string.Empty,
                CompanyCode = TaktUserContext.TryResolveCompanyCode(httpContext) ?? string.Empty,
                UserName = TaktUserContext.ResolveAuditUserName(httpContext),
                Remark = TaktUserContext.ResolveAuditContextRemark(httpContext),
                OperType = TaktHttpAuditHelper.ResolveOperTypeFromEntityChange(
                    diff.DiffType == DiffType.insert,
                    diff.DiffType == DiffType.delete),
                TableName = tableName,
                PrimaryKeyId = ResolvePrimaryKeyId(diff),
                BeforeData = beforeJson,
                AfterData = afterJson,
                DiffData = BuildDiffJson(diff),
                ExtField = SerializeBusinessData(diff.BusinessData) ?? string.Empty,
                SqlStatement = diff.Sql ?? string.Empty,
                OperIp = client.Ip,
                OperLocation = client.Location,
                UserAgent = client.UserAgent,
                Browser = client.Browser,
                Os = client.OperatingSystem,
                DeviceType = client.DeviceType,
                OperTime = DateTime.Now,
                ElapsedTime = (int)Math.Min(
                    int.MaxValue,
                    diff.Time?.TotalMilliseconds ?? db.Ado.SqlExecutionTime.TotalMilliseconds),
            };
            deltaLog.ApplyCreate(
                TaktUserContext.TryResolveUserId(httpContext));

            if (string.IsNullOrWhiteSpace(deltaLog.TenantCode) || string.IsNullOrWhiteSpace(deltaLog.CompanyCode))
            {
                return;
            }

            TryApplyCompanyScope(
                httpContext,
                deltaLog,
                deltaLog.TenantCode,
                deltaLog.CompanyCode,
                fallbackConfiguration);

            SuppressAuditWrite.Value = true;
            TaktPrimaryKeyInsertHelper.InsertEntitySync(db, deltaLog, TaktPrimaryKeyInsertHelper.RuntimeOptions);
        }
        catch (Exception ex)
        {
            TaktLogger.Warning(ex, "[SqlSugarAudit] 写入差异日志失败: {TableName}", tableName);
        }
        finally
        {
            SuppressAuditWrite.Value = false;
        }
    }

    /// <summary>
    /// 按 Database:CompanyCodes↔PlantCodes↔CultureCodes 写入公司级日志实体的 CultureCode / PlantCode。
    /// 优先 HttpContext.RequestServices；Quartz/后台 RequestServices 已释放时回退 fallbackConfiguration。
    /// </summary>
    /// <param name="httpContext">HTTP 上下文（可为已结束请求）</param>
    /// <param name="entity">公司级实体（操作日志 / 差异日志）</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="fallbackConfiguration">无 RequestServices 时的配置回退</param>
    private static void TryApplyCompanyScope(
        HttpContext? httpContext,
        object entity,
        string tenantCode,
        string companyCode,
        IConfiguration? fallbackConfiguration)
    {
        IConfiguration? configuration = null;
        var services = httpContext?.RequestServices;
        if (services != null)
        {
            configuration = services.GetService<IConfiguration>();
        }

        configuration ??= fallbackConfiguration;
        if (configuration == null)
        {
            return;
        }

        TaktCompanyScopeFillHelper.ApplyCompanyScopeFromConfiguration(
            entity,
            tenantCode,
            companyCode,
            configuration);
    }

    /// <summary>
    /// 为 Insertable/Updateable/Deleteable 启用差异日志（统计日志实体除外；与官方 StaticConfig 示例一致）
    /// </summary>
    /// <param name="builder">SqlSugar 可写构建器实例</param>
    private static void TryEnableDiffLogEvent(object builder)
    {
        var entityType = ResolveEntityTypeFromBuilder(builder);
        if (entityType == null || IsStatisticsLoggingEntityType(entityType))
        {
            return;
        }

        var builderType = builder.GetType();
        var method = builderType.GetMethod("EnableDiffLogEvent", new[] { typeof(object) })
            ?? builderType.GetMethod("EnableDiffLogEvent", Type.EmptyTypes);
        if (method == null)
        {
            return;
        }

        if (method.GetParameters().Length == 0)
        {
            method.Invoke(builder, null);
        }
        else
        {
            method.Invoke(builder, new object?[] { null });
        }
    }

    /// <summary>
    /// 序列化 DiffLogModel.BusinessData（EnableDiffLogEvent 传入的业务对象或字符串）
    /// </summary>
    /// <param name="businessData">SqlSugar 回调中的 BusinessData</param>
    /// <returns>JSON 或原字符串；无数据时为 null</returns>
    private static string? SerializeBusinessData(object? businessData)
    {
        if (businessData == null)
        {
            return null;
        }

        return businessData is string text ? text : JsonConvert.SerializeObject(businessData);
    }

    /// <summary>
    /// 从 SqlSugar 构建器泛型参数解析实体类型
    /// </summary>
    /// <param name="builder">SqlSugar 可写构建器实例</param>
    /// <returns>实体类型；无法解析时为 null</returns>
    private static Type? ResolveEntityTypeFromBuilder(object builder)
    {
        var builderType = builder.GetType();
        var genericArgs = builderType.GetGenericArguments();
        if (genericArgs.Length > 0)
        {
            return genericArgs[0];
        }

        foreach (var iface in builderType.GetInterfaces())
        {
            if (!iface.IsGenericType)
            {
                continue;
            }

            var args = iface.GetGenericArguments();
            if (args.Length > 0)
            {
                return args[0];
            }
        }

        return null;
    }

    /// <summary>
    /// 从差异日志中解析物理表名
    /// </summary>
    /// <param name="diff">差异日志模型</param>
    /// <returns>表名；无变更数据时为 null</returns>
    private static string? ResolveTableName(DiffLogModel diff)
    {
        var table = diff.AfterData?.FirstOrDefault() ?? diff.BeforeData?.FirstOrDefault();
        return table?.TableName;
    }

    /// <summary>
    /// 从差异日志列信息中解析主键 ID
    /// </summary>
    /// <param name="diff">差异日志模型</param>
    /// <returns>主键 long 值；无主键或无法解析时为 null</returns>
    private static long? ResolvePrimaryKeyId(DiffLogModel diff)
    {
        var columns = diff.AfterData?.FirstOrDefault()?.Columns
            ?? diff.BeforeData?.FirstOrDefault()?.Columns;
        if (columns == null)
        {
            return null;
        }

        var pk = columns.FirstOrDefault(c => c.IsPrimaryKey);
        if (pk?.Value == null)
        {
            return null;
        }

        return long.TryParse(pk.Value.ToString(), out var id) ? id : null;
    }

    /// <summary>
    /// 将差异表数据序列化为 JSON 字符串
    /// </summary>
    /// <param name="tables">差异表列表</param>
    /// <returns>JSON 字符串；无数据时为空串</returns>
    private static string SerializeDiffTables(List<DiffLogTableInfo>? tables)
    {
        if (tables == null || tables.Count == 0)
        {
            return string.Empty;
        }

        return JsonConvert.SerializeObject(tables);
    }

    /// <summary>
    /// 构建列级前后值差异 JSON（仅包含有变更的列）
    /// </summary>
    /// <param name="diff">差异日志模型</param>
    /// <returns>差异 JSON；无有效对比时为空串</returns>
    private static string BuildDiffJson(DiffLogModel diff)
    {
        if (diff.BeforeData == null || diff.AfterData == null)
        {
            return string.Empty;
        }

        var beforeColumns = diff.BeforeData.FirstOrDefault()?.Columns;
        var afterColumns = diff.AfterData.FirstOrDefault()?.Columns;
        if (beforeColumns == null || afterColumns == null)
        {
            return string.Empty;
        }

        var changes = new List<object>();
        foreach (var afterCol in afterColumns)
        {
            var beforeCol = beforeColumns.FirstOrDefault(c =>
                string.Equals(c.ColumnName, afterCol.ColumnName, StringComparison.OrdinalIgnoreCase));
            var beforeValue = beforeCol?.Value?.ToString();
            var afterValue = afterCol.Value?.ToString();
            if (string.Equals(beforeValue, afterValue, StringComparison.Ordinal))
            {
                continue;
            }

            changes.Add(new
            {
                column = afterCol.ColumnName,
                before = beforeValue,
                after = afterValue,
            });
        }

        return changes.Count == 0 ? string.Empty : JsonConvert.SerializeObject(changes);
    }

    /// <summary>
    /// 判断当前 HTTP 请求是否应写入操作日志
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>应持久化时为 true</returns>
    private static bool ShouldPersistOperLog(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        if (!path.StartsWith("/api/", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (path.Contains("/hubs/", StringComparison.OrdinalIgnoreCase)
            || IsStatisticsLoggingApiRequest(context))
        {
            return false;
        }

        return context.Request.Method is "POST" or "PUT" or "PATCH" or "DELETE";
    }

    /// <summary>
    /// 拼接请求路径与查询字符串作为操作 URL
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>完整操作 URL</returns>
    private static string? BuildOperUrl(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        var query = context.Request.QueryString.Value;
        return string.IsNullOrEmpty(query) ? path : path + query;
    }

    /// <summary>
    /// 是否为 StatisticsLoggingEntityNamespace 下实体（<c>Entities/Statistics/Logging</c> 目录）
    /// </summary>
    /// <param name="entityType">实体类型</param>
    /// <returns>是统计日志实体为 true</returns>
    private static bool IsStatisticsLoggingEntityType(Type entityType)
    {
        return string.Equals(
            entityType.Namespace,
            StatisticsLoggingEntityNamespace,
            StringComparison.Ordinal);
    }

    /// <summary>
    /// 从 TaktSqlSugarContext.EntityTypes 扫描 Statistics.Logging 下全部实体物理表名
    /// </summary>
    /// <returns>统计日志表名集合</returns>
    private static HashSet<string> BuildExcludedStatisticsLoggingTableNames()
    {
        var tableNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var entityType in TaktSqlSugarContext.EntityTypes)
        {
            if (!IsStatisticsLoggingEntityType(entityType))
            {
                continue;
            }

            var sugarTable = entityType.GetCustomAttributes(typeof(SugarTable), inherit: false)
                .OfType<SugarTable>()
                .FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(sugarTable?.TableName))
            {
                tableNames.Add(sugarTable.TableName.Trim());
            }
        }

        return tableNames;
    }

    /// <summary>
    /// 由 Statistics.Logging 实体推导 API 路由段（如 TaktDeltaLog → TaktDeltaLogs）
    /// </summary>
    /// <returns>需排除的操作日志 API 路由段集合</returns>
    private static HashSet<string> BuildExcludedStatisticsLoggingApiRouteSegments()
    {
        var routeSegments = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var entityType in TaktSqlSugarContext.EntityTypes)
        {
            if (!IsStatisticsLoggingEntityType(entityType))
            {
                continue;
            }

            routeSegments.Add(GetControllerRouteSegmentForEntity(entityType.Name));
        }

        return routeSegments;
    }

    /// <summary>
    /// 物理表名是否属于 Statistics.Logging 实体表
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <returns>是统计日志表为 true</returns>
    private static bool IsExcludedStatisticsLoggingTableName(string tableName)
    {
        return ExcludedStatisticsLoggingTableNames.Value.Contains(tableName.Trim());
    }

    /// <summary>
    /// 是否为 Statistics.Logging 模块 HTTP 请求（优先控制器命名空间，回退路由段）
    /// </summary>
    /// <param name="context">HTTP 上下文</param>
    /// <returns>是统计日志 API 为 true</returns>
    private static bool IsStatisticsLoggingApiRequest(HttpContext context)
    {
        var descriptor = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (descriptor?.ControllerTypeInfo.Namespace != null
            && string.Equals(
                descriptor.ControllerTypeInfo.Namespace,
                StatisticsLoggingControllerNamespace,
                StringComparison.Ordinal))
        {
            return true;
        }

        return IsStatisticsLoggingApiPath(context.Request.Path.Value);
    }

    /// <summary>
    /// 按路径判断是否为 Statistics.Logging API（<c>/api/Takt*Logs</c> 段由实体动态生成）
    /// </summary>
    /// <param name="path">请求路径</param>
    /// <returns>是统计日志 API 为 true</returns>
    private static bool IsStatisticsLoggingApiPath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 2
            || !segments[0].Equals("api", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return ExcludedStatisticsLoggingApiRouteSegments.Value.Contains(segments[1]);
    }

    /// <summary>
    /// 由实体类名生成控制器路由段（与 <c>scripts/generate-script-common.cjs</c> 复数规则对齐）
    /// </summary>
    /// <param name="entityName">实体类名，如 TaktDeltaLog</param>
    /// <returns>路由段，如 TaktDeltaLogs</returns>
    private static string GetControllerRouteSegmentForEntity(string entityName)
    {
        if (!entityName.StartsWith("Takt", StringComparison.Ordinal))
        {
            return entityName;
        }

        var entityShort = entityName["Takt".Length..];
        return $"Takt{PluralizeEntityShort(entityShort)}";
    }

    /// <summary>
    /// 实体短名复数化（用于 API 路由段推导）
    /// </summary>
    /// <param name="entityShort">去掉 Takt 前缀后的短名</param>
    /// <returns>复数短名</returns>
    private static string PluralizeEntityShort(string entityShort)
    {
        if (string.IsNullOrEmpty(entityShort))
        {
            return entityShort;
        }

        if (entityShort.EndsWith("y", StringComparison.Ordinal)
            && entityShort.Length > 1
            && "aeiou".IndexOf(char.ToLowerInvariant(entityShort[^2])) < 0)
        {
            return entityShort[..^1] + "ies";
        }

        if (entityShort.EndsWith("Company", StringComparison.Ordinal))
        {
            return entityShort[..^7] + "Companies";
        }

        if (entityShort.EndsWith("s", StringComparison.OrdinalIgnoreCase)
            || entityShort.EndsWith("x", StringComparison.OrdinalIgnoreCase)
            || entityShort.EndsWith("z", StringComparison.OrdinalIgnoreCase)
            || entityShort.EndsWith("ch", StringComparison.OrdinalIgnoreCase)
            || entityShort.EndsWith("sh", StringComparison.OrdinalIgnoreCase))
        {
            return entityShort + "es";
        }

        return entityShort + "s";
    }

    /// <summary>
    /// 对请求/差异 JSON 脱敏并截断过长内容
    /// </summary>
    /// <param name="json">原始 JSON 文本</param>
    /// <returns>脱敏或截断后的文本；空输入原样返回</returns>
    private static string? MaskSensitiveJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return json;
        }

        if (json.Contains("password", StringComparison.OrdinalIgnoreCase)
            || json.Contains("token", StringComparison.OrdinalIgnoreCase))
        {
            return "[已脱敏]";
        }

        return json.Length > 8000 ? json[..8000] : json;
    }
}
