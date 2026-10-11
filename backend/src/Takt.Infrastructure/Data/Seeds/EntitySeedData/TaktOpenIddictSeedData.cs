// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktOpenIddictSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：OpenIddict种子数据（Authorization Code + PKCE 公共客户端）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Takt.Shared.Options;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// OpenIddict 种子数据初始化
/// 幂等性操作：范围不存在则创建；SPA 客户端存在则更新、不存在则创建；API 机密客户端不存在则创建
/// 由 TaktOpenIddictContext.InitializeAsync 在认证库建表后调用
/// 配置来源：TaktOpenIddictOptions（appsettings OpenIddict 节）
/// </summary>
public class TaktOpenIddictSeedData
{
    /// <summary>
    /// 初始化 OpenIddict 数据（OAuth2/OIDC 范围与客户端应用）
    /// </summary>
    /// <param name="serviceProvider">服务提供者（解析 OpenIddict 管理器与配置选项）</param>
    /// <returns>表示异步初始化完成的任务</returns>
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var applicationManager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var scopeManager = serviceProvider.GetRequiredService<IOpenIddictScopeManager>();
        var openIddictOptions = serviceProvider.GetRequiredService<IOptions<TaktOpenIddictOptions>>().Value;

        await CreateScopesAsync(scopeManager, openIddictOptions.ApiAudience);
        await CreateApplicationsAsync(applicationManager, openIddictOptions);
    }

    /// <summary>
    /// 创建标准 OIDC 范围（openid、profile、email、offline_access）
    /// </summary>
    /// <param name="scopeManager">OpenIddict 范围管理器</param>
    /// <param name="apiAudience">API 资源标识（写入各范围的 Resources）</param>
    /// <returns>表示异步创建完成的任务</returns>
    private static async Task CreateScopesAsync(IOpenIddictScopeManager scopeManager, string apiAudience)
    {
        await CreateScopeIfMissingAsync(scopeManager, Scopes.OpenId, "OpenID Connect", "OpenID Connect 标准范围", apiAudience);
        await CreateScopeIfMissingAsync(scopeManager, Scopes.Profile, "用户资料", "访问用户基本资料", apiAudience);
        await CreateScopeIfMissingAsync(scopeManager, Scopes.Email, "电子邮箱", "访问用户电子邮箱", apiAudience);
        await CreateScopeIfMissingAsync(scopeManager, Scopes.OfflineAccess, "离线访问", "允许刷新令牌", apiAudience);
    }

    /// <summary>
    /// 按名称创建 OpenIddict 范围（已存在则跳过）
    /// </summary>
    /// <param name="scopeManager">OpenIddict 范围管理器</param>
    /// <param name="name">范围名称（如 openid、profile）</param>
    /// <param name="displayName">显示名称</param>
    /// <param name="description">范围说明</param>
    /// <param name="resource">关联 API 资源（audience）</param>
    /// <returns>表示异步创建或跳过的任务</returns>
    private static async Task CreateScopeIfMissingAsync(
        IOpenIddictScopeManager scopeManager,
        string name,
        string displayName,
        string description,
        string resource)
    {
        if (await scopeManager.FindByNameAsync(name) != null)
        {
            return;
        }

        await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
        {
            Name = name,
            DisplayName = displayName,
            Description = description,
            Resources = { resource }
        });
    }

    /// <summary>
    /// 创建或更新 OpenIddict 客户端应用（SPA 公共客户端与 API 机密客户端）
    /// </summary>
    /// <param name="applicationManager">OpenIddict 应用管理器</param>
    /// <param name="options">OpenIddict 配置（客户端 ID、回调地址等）</param>
    /// <returns>表示异步应用种子完成的任务</returns>
    private static async Task CreateApplicationsAsync(
        IOpenIddictApplicationManager applicationManager,
        TaktOpenIddictOptions options)
    {
        await CreateOrUpdateSpaClientAsync(applicationManager, options);
        await CreateApiClientIfMissingAsync(applicationManager, options.ApiAudience);
    }

    /// <summary>
    /// SPA 公共客户端：Authorization Code + PKCE（强制）
    /// 已存在则按当前配置更新重定向地址与权限；不存在则创建
    /// </summary>
    /// <param name="applicationManager">OpenIddict 应用管理器</param>
    /// <param name="options">OpenIddict 配置（SpaClientId、SpaRedirectUris、SpaPostLogoutRedirectUris）</param>
    /// <returns>表示异步创建或更新 SPA 客户端的任务</returns>
    private static async Task CreateOrUpdateSpaClientAsync(
        IOpenIddictApplicationManager applicationManager,
        TaktOpenIddictOptions options)
    {
        var clientId = options.SpaClientId;
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            ClientType = ClientTypes.Public,
            ConsentType = ConsentTypes.Implicit,
            DisplayName = "Takt SPA",
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.Endpoints.EndSession,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Prefixes.Scope + Scopes.OpenId,
                Permissions.Prefixes.Scope + Scopes.Profile,
                Permissions.Prefixes.Scope + Scopes.Email,
                Permissions.Prefixes.Scope + Scopes.OfflineAccess
            },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        };

        foreach (var uri in options.SpaRedirectUris)
        {
            descriptor.RedirectUris.Add(new Uri(uri));
        }

        foreach (var uri in options.SpaPostLogoutRedirectUris)
        {
            descriptor.PostLogoutRedirectUris.Add(new Uri(uri));
        }

        var existing = await applicationManager.FindByClientIdAsync(clientId);
        if (existing == null)
        {
            await applicationManager.CreateAsync(descriptor);
            return;
        }

        await applicationManager.UpdateAsync(existing, descriptor);
    }

    /// <summary>
    /// 机密客户端：Client Credentials（服务间调用）
    /// 客户端 ID 固定为 takt-api-client；已存在则跳过
    /// </summary>
    /// <param name="applicationManager">OpenIddict 应用管理器</param>
    /// <param name="apiAudience">API 资源标识（audience）</param>
    /// <returns>表示异步创建或跳过 API 客户端的任务</returns>
    private static async Task CreateApiClientIfMissingAsync(
        IOpenIddictApplicationManager applicationManager,
        string apiAudience)
    {
        const string clientId = "takt-api-client";
        if (await applicationManager.FindByClientIdAsync(clientId) != null)
        {
            return;
        }

        await applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
            ClientType = ClientTypes.Confidential,
            DisplayName = "Takt API Client",
            Permissions =
            {
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.ClientCredentials,
                Permissions.Prefixes.Scope + Scopes.OpenId,
                Permissions.Prefixes.Scope + Scopes.Profile
            }
        });
    }
}
