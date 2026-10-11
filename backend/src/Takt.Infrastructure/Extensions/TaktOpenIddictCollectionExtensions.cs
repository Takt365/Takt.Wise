// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktOpenIddictCollectionExtensions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：OpenIddict 认证服务注册（Authorization Code + PKCE）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Validation.AspNetCore;
using Takt.Shared.Helpers;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Constants;
using Takt.Shared.Options;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// OpenIddict 认证服务注册扩展方法
/// </summary>
public static class TaktOpenIddictCollectionExtensions
{
    /// <summary>
    /// 添加 OpenIddict 认证服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTaktOpenIddict(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        var openIddictOptions = configuration.RequireOptions<TaktOpenIddictOptions>(TaktOpenIddictOptions.SectionName);
        openIddictOptions.Validate();
        var isDevelopment = environment.IsDevelopment();

        services.Configure<TaktOpenIddictOptions>(configuration.GetSection(TaktOpenIddictOptions.SectionName));

        services.AddDbContext<TaktOpenIddictContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("OpenIddict");
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsHistoryTable("__OpenIddictMigrationsHistory");
            });
        });

        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                       .UseDbContext<TaktOpenIddictContext>();
            })
            .AddServer(options =>
            {
                options.SetIssuer(new Uri(openIddictOptions.Issuer.TrimEnd('/') + "/"));

                options.SetAuthorizationEndpointUris("connect/authorize")
                       .SetTokenEndpointUris("connect/token")
                       .SetEndSessionEndpointUris("connect/logout");

                options.AllowAuthorizationCodeFlow()
                       .AllowRefreshTokenFlow()
                       .AllowClientCredentialsFlow();

                options.RequireProofKeyForCodeExchange();

                options.RegisterScopes(
                    Scopes.OpenId,
                    Scopes.Profile,
                    Scopes.Email,
                    Scopes.OfflineAccess);

                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                options.SetAccessTokenLifetime(openIddictOptions.AccessTokenLifetime)
                       .SetRefreshTokenLifetime(openIddictOptions.RefreshTokenLifetime);

                var aspNetCore = options.UseAspNetCore()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableTokenEndpointPassthrough()
                       .EnableEndSessionEndpointPassthrough();

                if (isDevelopment)
                {
                    aspNetCore.DisableTransportSecurityRequirement();
                }

                options.AddEventHandler<OpenIddictServerEvents.ProcessErrorContext>(builder =>
                {
                    builder.UseInlineHandler(context =>
                    {
                        var error = context.Error ?? "unknown";
                        var description = context.ErrorDescription ?? string.Empty;
                        var uri = context.RequestUri?.ToString() ?? string.Empty;
                        TaktLogger.Error(
                            "[OpenIddict] {Error}: {Description} ({Uri})",
                            error,
                            description,
                            uri);
                        return default;
                    });
                });
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.AddAudiences(openIddictOptions.ApiAudience);
                options.UseAspNetCore();
            });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = TaktAuthCookieDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = TaktAuthCookieDefaults.AuthenticationScheme;
        })
        .AddCookie(TaktAuthCookieDefaults.AuthenticationScheme, options =>
        {
            options.Cookie.Name = TaktAuthCookieDefaults.CookieName;
            options.Cookie.Path = "/";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = isDevelopment ? SameSiteMode.Lax : SameSiteMode.None;
            options.SlidingExpiration = true;
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.LoginPath = "/login";
        });

        services.AddAuthorization();

        return services;
    }
}
