// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Context
// 文件名称：TaktSeedContext.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：种子数据专用数据库上下文，支持动态切换租户连接
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using Takt.Domain.Entities;
using Takt.Infrastructure.Data.Seeds;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Context;

/// <summary>
/// 种子数据专用数据库上下文（启动步骤 3：仅负责种子写入，不负责建库建表）
/// </summary>
public class TaktSeedContext : IDisposable
{
    private ISqlSugarClient _db;
    private readonly IConfiguration _configuration;
    private readonly DbType _sugarDbType;
    private string _currentTenantCode;
    private bool _disposed = false;

    /// <summary>
    /// SqlSugar客户端实例
    /// </summary>
    public ISqlSugarClient Db => _db;
    
    /// <summary>
    /// 当前租户编码
    /// </summary>
    public string CurrentTenantCode => _currentTenantCode;

    /// <summary>
    /// 所有实体类型集合（通过反射自动扫描）
    /// </summary>
    private static readonly Type[] _entityTypes;

    /// <summary>
    /// 静态构造函数：自动扫描所有实体
    /// </summary>
    static TaktSeedContext()
    {
        var domainAssembly = typeof(TaktTenantEntityBase).Assembly;
        _entityTypes = domainAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract &&
                       (typeof(TaktTenantCoreEntityScopeBase).IsAssignableFrom(t) ||
                        typeof(TaktCompanyEntityScopeBase).IsAssignableFrom(t) ||
                        typeof(TaktApprovalEntityScopeBase).IsAssignableFrom(t)))
            .ToArray();
    }

    /// <summary>
    /// 构造函数（必须指定租户编码）
    /// </summary>
    /// <param name="configuration">配置信息</param>
    /// <param name="tenantCode">租户编码（必须提供，不允许默认值）</param>
    /// <exception cref="ArgumentException">租户编码为空时抛出</exception>
    public TaktSeedContext(IConfiguration configuration, string tenantCode)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            throw new ArgumentException("租户编码不能为空，必须显式指定租户", nameof(tenantCode));
        }
        
        _configuration = configuration;
        _sugarDbType = configuration.GetSugarDbType();
        _currentTenantCode = tenantCode;
        _db = CreateSqlSugarClient(tenantCode);
    }
    
    /// <summary>
    /// 切换到指定租户的数据库连接
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <exception cref="ArgumentException">租户编码为空时抛出</exception>
    /// <exception cref="InvalidOperationException">未找到租户连接字符串时抛出</exception>
    public void SwitchTenant(string tenantCode)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            throw new ArgumentException("租户编码不能为空", nameof(tenantCode));
        }
        
        if (_currentTenantCode == tenantCode)
        {
            return; // 已经是当前租户，无需切换
        }
        
        _db.Close();
        _db.Dispose();

        _currentTenantCode = tenantCode;
        _db = CreateSqlSugarClient(tenantCode);
    }

    /// <summary>
    /// 创建 SqlSugar 客户端（种子上下文 / 切换租户共用）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>SqlSugar 客户端</returns>
    private ISqlSugarClient CreateSqlSugarClient(string tenantCode)
    {
        var connectionString = _configuration.GetConnectionString($"Tenant_{tenantCode}");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"未找到租户 {tenantCode} 的连接字符串配置。请在 appsettings.json 中配置 'ConnectionStrings:Tenant_{tenantCode}'。");
        }
        var db = TaktSqlSugarConnectionHelper.CreateClient(_sugarDbType, tenantCode, connectionString);
#if DEBUG
        db.Aop.OnLogExecuting = (sql, pars) =>
        {
            TaktLogger.Debug("SQL: {Sql}", sql);
            TaktLogger.Debug("Parameters: {Parameters}", string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value) ?? Array.Empty<string>()));
        };
#endif
        return db;
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
    /// 获取所有实体类型（与 TaktSqlSugarContext 扫描规则一致）
    /// </summary>
    public static IReadOnlyList<Type> EntityTypes => _entityTypes;

    /// <summary>
    /// 初始化业务种子数据（步骤 3）
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <returns>插入与更新条数</returns>
    public static async Task<(int TotalInsertCount, int TotalUpdateCount)> InitializeSeedDataAsync(
        IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        TaktSqlSugarContext.EnsureAllTenantDatabasesReady(configuration);
        return await TaktSeedDataCoordinator.InitializeAllAsync(serviceProvider);
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _db?.Close();
                _db?.Dispose();
            }
            _disposed = true;
        }
    }
}
