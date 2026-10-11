// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Filters
// 文件名称：TaktPermissionFilter.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：API 功能权限过滤器（读取 TaktPermission 特性并校验 RBAC 权限码）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Takt.Application.Services.Identity;
using Takt.Domain.Interfaces;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.WebApi.Filters;

/// <summary>
/// 功能权限授权过滤器
/// </summary>
public sealed class TaktPermissionFilter : IAsyncAuthorizationFilter
{
    private readonly ITaktAuthService _authService;
    private readonly ITaktUserContext _userContext;
    private readonly ITaktLocalizationService _localizationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="authService">身份认证服务</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPermissionFilter(
        ITaktAuthService authService,
        ITaktUserContext userContext,
        ITaktLocalizationService localizationService)
    {
        _authService = authService;
        _userContext = userContext;
        _localizationService = localizationService;
    }

    /// <summary>
    /// 授权过滤器入口：校验 TaktPermission 特性与 RBAC 权限码
    /// </summary>
    /// <param name="context">授权过滤器上下文</param>
    /// <returns>任务</returns>
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor actionDescriptor)
        {
            return;
        }

        if (HasAllowAnonymous(context))
        {
            return;
        }

        var permissionAttribute = actionDescriptor.MethodInfo
            .GetCustomAttributes(typeof(TaktPermissionAttribute), inherit: true)
            .OfType<TaktPermissionAttribute>()
            .FirstOrDefault();

        if (permissionAttribute == null)
        {
            permissionAttribute = actionDescriptor.ControllerTypeInfo
                .GetCustomAttributes(typeof(TaktPermissionAttribute), inherit: true)
                .OfType<TaktPermissionAttribute>()
                .FirstOrDefault();
        }

        if (permissionAttribute == null)
        {
            return;
        }

        if (!_userContext.IsAuthenticated || _userContext.UserId == null)
        {
            context.Result = new ObjectResult(
                TaktApiResult.Fail(_localizationService.Translate("common.permission.unauthorized"), TaktResultCode.Unauthorized))
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }

        var tenantCode = _userContext.TenantCode;
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            context.Result = new ObjectResult(
                TaktApiResult.Fail(TaktValidationMessageHelper.Build(
                    k => _localizationService.Translate(k),
                    TaktValidationI18nKeys.Required,
                    TaktValidationI18nKeys.FieldTenantContext), TaktResultCode.BadRequest))
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            return;
        }

        var hasPermission = await _authService.HasUserPermissionAsync(
            _userContext.UserId.Value,
            tenantCode,
            permissionAttribute.PermissionCode);

        if (!hasPermission)
        {
            var requiredCode = permissionAttribute.PermissionCode?.Trim() ?? string.Empty;
            TaktLogger.Warning(
                "功能权限校验失败: UserId={UserId}, Tenant={TenantCode}, RequiredPermission={PermissionCode}, DisplayName={DisplayName}, Path={Path}",
                _userContext.UserId.Value,
                tenantCode,
                requiredCode,
                permissionAttribute.DisplayName,
                context.HttpContext.Request.Path.Value);

            // 错误文案同时带 DisplayName 与权限码，便于对照菜单/角色是否真有该码
            var deniedMessage = _localizationService.Translate(
                TaktValidationI18nKeys.PermissionDeniedWithAction,
                culture: null,
                $"{permissionAttribute.DisplayName} [{requiredCode}]");

            context.Result = new ObjectResult(
                TaktApiResult.Fail(deniedMessage, TaktResultCode.Forbidden))
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }

    /// <summary>
    /// 当前终结点是否标记 IAllowAnonymous（跳过功能权限校验）
    /// </summary>
    /// <param name="context">授权过滤器上下文</param>
    /// <returns>允许匿名访问为 true</returns>
    private static bool HasAllowAnonymous(AuthorizationFilterContext context)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
        {
            return true;
        }

        return false;
    }
}
