// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Extensions
// 文件名称：TaktSwaggerCollectionExtensions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：OpenAPI / Scalar 文档注册（扫描 ApiModule 特性自动分组）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Takt.Shared.Constants;
using Takt.Shared.Enums;

namespace Takt.Infrastructure.Extensions;

/// <summary>
/// OpenAPI / Scalar 文档扩展（按控制器 ApiModuleAttribute 自动分组）
/// </summary>
/// <remarks>
/// <para>路由约定：</para>
/// <list type="bullet">
///   <item>OpenAPI JSON：<c>/openapi/{documentName}.json</c></item>
///   <item>Scalar 主页（模块下拉）：<c>/scalar</c> 或 <c>/scalar/</c></item>
///   <item>Scalar 单模块直达：<c>/scalar/{documentName}</c>（无模块切换下拉）</item>
///   <item><c>{documentName}</c> 与 ApiModuleAttribute.DocumentName 一致</item>
/// </list>
/// </remarks>
public static class TaktSwaggerCollectionExtensions
{
    private const string TenantHeaderName = "X-Tenant-Code";
    private const string CompanyHeaderName = "X-Company-Code";
    private const string OpenApiRoutePattern = "/openapi/{documentName}.json";
    private const string ScalarEndpointPrefix = "/scalar";

    /// <summary>
    /// 注册 OpenAPI 文档（每个 ApiModuleAttribute 模块一组）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTaktOpenApi(this IServiceCollection services)
    {
        foreach (var group in DiscoverApiModuleGroups())
        {
            services.AddOpenApi(group.DocumentName, options => ConfigureModuleDocument(options, group));
        }

        return services;
    }

    /// <summary>
    /// 启用 OpenAPI JSON 与 Scalar UI（仅开发环境）
    /// </summary>
    /// <param name="app">应用程序</param>
    /// <returns>应用程序</returns>
    public static WebApplication UseTaktOpenApi(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return app;
        }

        var groups = DiscoverApiModuleGroups();
        if (groups.Count == 0)
        {
            return app;
        }

        app.MapOpenApi(OpenApiRoutePattern);

        var defaultGroup = ResolveDefaultGroup(groups);
        var scalarDocuments = groups
            .Select(group => new ScalarDocument(
                group.DocumentName,
                group.DisplayName,
                OpenApiRoutePattern,
                group.Module == defaultGroup.Module))
            .ToList();

        app.MapScalarApiReference(ScalarEndpointPrefix, options =>
        {
            options.WithTitle("TaktWise API");
            options.WithFavicon("/logo/takt.svg");
            options.WithOpenApiRoutePattern(OpenApiRoutePattern);
            options.AddDocuments(scalarDocuments);
            options.ExpandAllTags();
        });

        return app;
    }

    /// <summary>
    /// 扫描入口程序集控制器上的 ApiModuleAttribute，按模块去重分组
    /// </summary>
    private static IReadOnlyList<ApiModuleGroup> DiscoverApiModuleGroups()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly == null)
        {
            return Array.Empty<ApiModuleGroup>();
        }

        return entryAssembly
            .GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && typeof(ControllerBase).IsAssignableFrom(type))
            .Select(type => type.GetCustomAttribute<ApiModuleAttribute>(inherit: true))
            .Where(attr => attr != null)
            .Select(attr => attr!.Module)
            .Distinct()
            .OrderBy(module => module)
            .Select(module => new ApiModuleGroup(
                module,
                GetDocumentName(module),
                GetModuleDisplayName(module)))
            .ToList();
    }

    /// <summary>
    /// 默认文档组（优先 Identity，否则第一个已发现模块）
    /// </summary>
    private static ApiModuleGroup ResolveDefaultGroup(IReadOnlyList<ApiModuleGroup> groups) =>
        groups.FirstOrDefault(g => g.Module == TaktModule.Identity) ?? groups[0];

    /// <summary>
    /// 配置单个模块 OpenAPI 文档
    /// </summary>
    private static void ConfigureModuleDocument(OpenApiOptions options, ApiModuleGroup group)
    {
        options.ShouldInclude = apiDescription =>
        {
            if (apiDescription.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
            {
                return false;
            }

            var moduleAttr = actionDescriptor.ControllerTypeInfo.GetCustomAttribute<ApiModuleAttribute>(inherit: true);
            return moduleAttr != null && moduleAttr.Module == group.Module;
        };

        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Info = new OpenApiInfo
            {
                Title = $"TaktWise - {group.DisplayName}",
                Version = group.DocumentName,
                Description = $"业务模块：{group.DisplayName}（{group.DocumentName}）",
            };
            return Task.CompletedTask;
        });

        options.AddOperationTransformer((operation, context, cancellationToken) =>
        {
            AppendOptionalHeaderParameter(operation, TenantHeaderName, "租户编码（3位，可选；业务接口建议传入）");
            AppendOptionalHeaderParameter(operation, CompanyHeaderName, "公司编码（4位，可选；公司级接口建议传入）");
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// 追加可选请求头参数（已存在则跳过）
    /// </summary>
    private static void AppendOptionalHeaderParameter(OpenApiOperation operation, string headerName, string description)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        if (operation.Parameters.Any(p =>
                p.In == ParameterLocation.Header &&
                string.Equals(p.Name, headerName, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = headerName,
            In = ParameterLocation.Header,
            Required = false,
            Description = description,
            Schema = new OpenApiSchema { Type = "string" },
        });
    }

    /// <summary>
    /// 文档名（与 ApiModuleAttribute.DocumentName 规则一致）
    /// </summary>
    private static string GetDocumentName(TaktModule module) =>
        module.ToString().ToLowerInvariant();

    /// <summary>
    /// 模块显示名（取自 TaktModule 的 DisplayAttribute）
    /// </summary>
    private static string GetModuleDisplayName(TaktModule module)
    {
        var field = typeof(TaktModule).GetField(module.ToString());
        var display = field?.GetCustomAttribute<DisplayAttribute>();
        return display?.Name ?? module.ToString();
    }

    /// <summary>
    /// ApiModule 分组描述（OpenAPI 文档 + Scalar 下拉项）
    /// </summary>
    private sealed record ApiModuleGroup(TaktModule Module, string DocumentName, string DisplayName);
}
