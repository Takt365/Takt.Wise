// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktServiceCollectionExtensions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：业务服务注册扩展方法（用户上下文、本地化、CORS 等）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Services;
using Takt.Shared.Options;
using CorsSettings = Takt.Shared.Options.CorsSettings;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// 业务服务注册扩展方法
/// </summary>
public static class TaktServiceCollectionExtensions
{
    /// <summary>
    /// 添加 Takt 业务服务（用户上下文、本地化、CORS 等）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTaktServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 注册 HTTP 上下文访问器（用于获取当前用户信息）
        services.AddHttpContextAccessor();

        // 始终 TaktUserContext（动态读 HttpContext）。Quartz 须在解析 Scoped 服务前注入租户/公司头。
        services.AddScoped<ITaktUserContext>(sp =>
            ActivatorUtilities.CreateInstance<TaktUserContext>(sp));

        // 注册本地化服务（默认 en-US；请求头 Accept-Language 与前端 locale 同步）
        var localizationOptions = configuration.RequireLocalization();

        services.Configure<TaktLocalizationOptions>(configuration.GetSection(TaktLocalizationOptions.SectionName));

        services.AddLocalization(options =>
        {
            options.ResourcesPath = localizationOptions.ResourcesPath;
        });

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(localizationOptions.DefaultCulture);
            options.RequestCultureProviders =
            [
                new AcceptLanguageHeaderRequestCultureProvider(),
            ];
        });

        services.AddScoped<ITaktLocalizationService, TaktLocalizationService>();

        var corsSettings = configuration.RequireOptions<CorsSettings>(CorsSettings.SectionName);
        corsSettings.Validate();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(corsSettings.AllowedOrigins)
                      .WithMethods(corsSettings.AllowedMethods)
                      .WithHeaders(corsSettings.AllowedHeaders);

                if (corsSettings.AllowCredentials)
                {
                    policy.AllowCredentials();
                }
            });
        });

        return services;
    }
}
