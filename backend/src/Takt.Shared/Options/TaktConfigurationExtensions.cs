// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktConfigurationExtensions.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：配置节绑定扩展（Options 默认值 + appsettings 覆盖 + 启动校验）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using SqlSugar;

namespace Takt.Shared.Options;

/// <summary>
/// 配置节绑定扩展（标准三层：Options 默认值 → appsettings 覆盖 → Validate 校验）
/// </summary>
public static class TaktConfigurationExtensions
{
    /// <summary>
    /// 以 Options 类型默认值为底线，将 <paramref name="sectionName"/> 绑定为覆盖项
    /// </summary>
    /// <typeparam name="T">须有无参构造函数的配置类型</typeparam>
    /// <param name="configuration">应用配置</param>
    /// <param name="sectionName">配置节名称</param>
    /// <returns>合并后的配置实例</returns>
    public static T BindOptions<T>(this IConfiguration configuration, string sectionName)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }

    /// <summary>
    /// 绑定配置节（同 BindOptions{T}，保留历史调用名）
    /// </summary>
    /// <typeparam name="T">须有无参构造函数的配置类型</typeparam>
    /// <param name="configuration">应用配置</param>
    /// <param name="sectionName">配置节名称</param>
    /// <returns>合并后的配置实例</returns>
    public static T RequireOptions<T>(this IConfiguration configuration, string sectionName)
        where T : class, new()
        => configuration.BindOptions<T>(sectionName);

    /// <summary>
    /// 绑定并校验 <c>Database</c> 节（租户/公司/工厂范围与种子源）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>已规范化并校验的数据库配置</returns>
    public static TaktDatabaseOptions RequireDatabase(this IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktDatabaseOptions>(TaktDatabaseOptions.SectionName);
        options.NormalizeAndValidate();
        return options;
    }

    /// <summary>
    /// 绑定并校验 <c>Localization</c> 节（默认语言、多种语言内容通用码 mul、资源路径）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>已规范化并校验的本地化配置</returns>
    public static TaktLocalizationOptions RequireLocalization(this IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktLocalizationOptions>(TaktLocalizationOptions.SectionName);
        options.Validate();
        return options;
    }

    /// <summary>
    /// 获取已解析的 SqlSugar 数据库类型（经 Database 节一次映射并缓存）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>SqlSugar DbType</returns>
    public static DbType GetSugarDbType(this IConfiguration configuration) =>
        configuration.RequireDatabase().GetSugarDbType();

    /// <summary>
    /// 绑定 <c>Init</c> 节（建表/种子开关，与 TaktDatabaseOptions 数据范围分离）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>初始化开关配置</returns>
    public static TaktInitOptions RequireInit(this IConfiguration configuration) =>
        configuration.RequireOptions<TaktInitOptions>(TaktInitOptions.SectionName);

    /// <summary>
    /// 绑定并校验 <c>TenantContext</c> 节（请求头名称）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>租户上下文配置</returns>
    public static TaktTenantContextOptions RequireTenantContext(this IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktTenantContextOptions>(TaktTenantContextOptions.SectionName);
        options.Validate();
        return options;
    }

    /// <summary>
    /// 租户编码列表（优先 <c>Database:TenantCodes</c>；否则回退 <c>ConnectionStrings:Tenant_*</c>）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>租户编码只读列表</returns>
    public static IReadOnlyList<string> GetTenantCodes(this IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var raw = configuration.GetSection($"{TaktDatabaseOptions.SectionName}:TenantCodes").Get<List<string>>();
        if (raw != null && raw.Exists(code => !string.IsNullOrWhiteSpace(code)))
        {
            return configuration.RequireDatabase().TenantCodes;
        }
        var fallback = new List<string>();
        foreach (var child in configuration.GetSection("ConnectionStrings").GetChildren())
        {
            if (!child.Key.StartsWith("Tenant_", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            var tenantCode = child.Key["Tenant_".Length..].Trim();
            if (!string.IsNullOrEmpty(tenantCode) && !string.IsNullOrWhiteSpace(child.Value))
            {
                fallback.Add(tenantCode);
            }
        }
        if (fallback.Count == 0)
        {
            throw new InvalidOperationException("未找到任何租户配置（Database:TenantCodes 或 ConnectionStrings:Tenant_*）");
        }
        return fallback;
    }

    /// <summary>
    /// 租户编码与连接字符串（顺序与 GetTenantCodes 一致）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>租户编码与连接字符串列表</returns>
    public static List<(string TenantCode, string ConnectionString)> GetTenantConnections(this IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var result = new List<(string TenantCode, string ConnectionString)>();
        foreach (var tenantCode in configuration.GetTenantCodes())
        {
            var connectionString = configuration.GetConnectionString($"Tenant_{tenantCode}");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"未找到租户 {tenantCode} 的连接字符串配置 (ConnectionStrings:Tenant_{tenantCode})");
            }
            result.Add((tenantCode, connectionString));
        }
        return result;
    }

    /// <summary>
    /// 绑定并校验 <c>Ftp:{provider}</c> 节（提供商标识与 <c>sys_ftp_provider</c> 字典值一致）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="provider">FTP 提供商标识，如 <c>teac_cn</c></param>
    /// <returns>FTP 提供商配置</returns>
    public static TaktFtpOptions RequireFtpProvider(this IConfiguration configuration, string provider)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);

        var options = new TaktFtpOptions();
        configuration.GetSection($"{TaktFtpOptions.SectionName}:{provider}").Bind(options);
        options.Validate(provider);
        return options;
    }

    /// <summary>
    /// 绑定并校验 <c>Oss:{provider}</c> 节（提供商标识与 <c>sys_oss_provider</c> 字典值一致）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="provider">OSS 提供商标识，如 <c>aliyun</c></param>
    /// <returns>OSS 提供商配置</returns>
    public static TaktOssOptions RequireOssProvider(this IConfiguration configuration, string provider)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);

        var options = new TaktOssOptions();
        configuration.GetSection($"{TaktOssOptions.SectionName}:{provider}").Bind(options);
        options.Validate(provider);
        return options;
    }

    /// <summary>
    /// 绑定并校验 <c>Quartz</c> 节（定时任务调度开关与调度器标识）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>Quartz 配置</returns>
    public static TaktQuartzOptions RequireQuartz(this IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktQuartzOptions>(TaktQuartzOptions.SectionName);
        options.Validate();
        return options;
    }

    /// <summary>
    /// 绑定并校验 <c>Authentication</c> 节（Token 生命周期）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>认证配置</returns>
    public static TaktAuthenticationOptions RequireAuthentication(this IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktAuthenticationOptions>(TaktAuthenticationOptions.SectionName);
        options.Validate();
        return options;
    }

    /// <summary>
    /// 绑定并校验 <c>AccountLock</c> 节（登录失败锁定策略）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>账户锁定配置</returns>
    public static TaktAccountLockOptions RequireAccountLock(this IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktAccountLockOptions>(TaktAccountLockOptions.SectionName);
        options.Validate();
        return options;
    }

    /// <summary>
    /// 绑定并校验 <c>System</c> 节（运行环境与系统开关）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>系统配置</returns>
    public static TaktSystemOptions RequireSystem(this IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktSystemOptions>(TaktSystemOptions.SectionName);
        options.Validate();
        return options;
    }

    /// <summary>
    /// 绑定并校验 <c>Email</c> 节（SMTP 发信参数）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>邮件配置</returns>
    public static TaktEmailOptions RequireEmail(this IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktEmailOptions>(TaktEmailOptions.SectionName);
        options.Validate();
        return options;
    }

    /// <summary>
    /// 绑定并校验 <c>Paged</c> 节（列表默认页码/页大小/上限）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <returns>分页配置</returns>
    public static TaktPagedOptions RequirePaged(this IConfiguration configuration)
    {
        var options = configuration.RequireOptions<TaktPagedOptions>(TaktPagedOptions.SectionName);
        options.Validate();
        return options;
    }

    /// <summary>
    /// 校验 <c>appsettings:Ftp</c> 下所有已声明的提供商子节（跳过 <c>_</c> 前缀注释键）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    public static void ValidateConfiguredFtpProviders(this IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var section = configuration.GetSection(TaktFtpOptions.SectionName);
        var providers = section.GetChildren()
            .Where(c => !c.Key.StartsWith('_'))
            .ToList();
        if (providers.Count == 0)
        {
            throw new InvalidOperationException($"配置节 {TaktFtpOptions.SectionName} 缺失或无可校验的提供商子节");
        }

        foreach (var child in providers)
        {
            configuration.RequireFtpProvider(child.Key);
        }
    }

    /// <summary>
    /// 校验 <c>appsettings:Oss</c> 下所有已声明的提供商子节（跳过 <c>_</c> 前缀注释键）
    /// </summary>
    /// <param name="configuration">应用配置</param>
    public static void ValidateConfiguredOssProviders(this IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var section = configuration.GetSection(TaktOssOptions.SectionName);
        var providers = section.GetChildren()
            .Where(c => !c.Key.StartsWith('_'))
            .ToList();
        if (providers.Count == 0)
        {
            throw new InvalidOperationException($"配置节 {TaktOssOptions.SectionName} 缺失或无可校验的提供商子节");
        }

        foreach (var child in providers)
        {
            configuration.RequireOssProvider(child.Key);
        }
    }
}

