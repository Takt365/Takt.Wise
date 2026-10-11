// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers
// 文件名称：TaktControllerBase.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：API控制器基类，提供统一响应格式、日志、本地化、用户和租户上下文
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Services;
using Takt.WebApi.Filters;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.WebApi.Controllers;

/// <summary>
/// API 控制器基类
/// 提供统一的响应格式、日志、本地化、用户和租户上下文
/// </summary>
[ApiController]
[Authorize]
[ServiceFilter(typeof(TaktPermissionFilter))]
[Route("api/[controller]")]
public abstract class TaktControllerBase : ControllerBase
{
    protected readonly ITaktUserContext? _userContext;
    protected readonly ITaktLocalizationService? _localizationService;

    /// <summary>
    /// 构造函数（可选依赖注入）
    /// </summary>
    /// <param name="userContext">用户上下文（可选）</param>
    /// <param name="localizationService">本地化服务（可选）</param>
    protected TaktControllerBase(
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
    {
        _userContext = userContext;
        _localizationService = localizationService;
        
        // 输出用户和租户上下文信息日志
        LogContextInfo();
    }

    /// <summary>
    /// 记录用户和租户上下文信息
    /// </summary>
    private void LogContextInfo()
    {
        try
        {
            var controllerTypeName = GetType().Name;
            var requestPath = HttpContext?.Request?.Path.Value ?? "unknown";
            var requestMethod = HttpContext?.Request?.Method ?? "unknown";
            
            // 获取用户上下文信息
            var userInfo = _userContext != null && _userContext.IsAuthenticated
                ? $"UserId: {_userContext.UserId}, UserName: {_userContext.UserName}, TenantCode: {_userContext.TenantCode}, CompanyCode: {_userContext.CompanyCode}"
                : "User: 未登录";
            
            TaktLogger.Debug("控制器上下文信息: ControllerType: {ControllerType}, RequestPath: {RequestPath}, RequestMethod: {RequestMethod}, {UserInfo}",
                controllerTypeName, requestPath, requestMethod, userInfo);
        }
        catch
        {
            // 忽略日志输出异常，避免影响控制器初始化
        }
    }

    #region 上下文属性

    /// <summary>
    /// 获取当前租户编码（从用户上下文或请求头）
    /// </summary>
    protected string? CurrentTenantCode =>
        _userContext?.TenantCode ?? TaktUserContext.TryResolveTenantCode(HttpContext);

    /// <summary>
    /// 获取当前公司编码（从用户上下文或请求头）
    /// </summary>
    protected string? CurrentCompanyCode =>
        _userContext?.CompanyCode ?? TaktUserContext.TryResolveCompanyCode(HttpContext);

    /// <summary>
    /// 获取当前用户ID（从用户上下文或 JWT Token）
    /// </summary>
    protected long? CurrentUserId
    {
        get
        {
            // 优先从用户上下文获取
            if (_userContext?.UserId.HasValue == true)
            {
                return _userContext.UserId;
            }

            return TaktUserContext.TryResolveUserId(User);
        }
    }

    /// <summary>
    /// 获取当前用户名（从用户上下文或 JWT Token）
    /// </summary>
    protected string? CurrentUserName
    {
        get
        {
            // 优先从用户上下文获取
            if (!string.IsNullOrEmpty(_userContext?.UserName))
            {
                return _userContext.UserName;
            }

            return TaktUserContext.TryResolveUserName(User);
        }
    }

    /// <summary>
    /// 获取当前用户是否已认证
    /// </summary>
    protected bool IsAuthenticated => _userContext?.IsAuthenticated == true || User.Identity?.IsAuthenticated == true;

    #endregion

    #region 统一日志处理

    /// <summary>
    /// 记录信息级别日志
    /// </summary>
    /// <param name="message">日志消息</param>
    protected void LogInformation(string message)
    {
        TaktLogger.Information("[{ControllerType}] {Message}", GetType().Name, message);
    }

    /// <summary>
    /// 记录信息级别日志（带参数）
    /// </summary>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    protected void LogInformation(string messageTemplate, params object[] propertyValues)
    {
        TaktLogger.Information("[{ControllerType}] " + messageTemplate, 
            new object[] { GetType().Name }.Concat(propertyValues).ToArray());
    }

    /// <summary>
    /// 记录警告级别日志
    /// </summary>
    /// <param name="message">日志消息</param>
    protected void LogWarning(string message)
    {
        TaktLogger.Warning("[{ControllerType}] {Message}", GetType().Name, message);
    }

    /// <summary>
    /// 记录警告级别日志（带参数）
    /// </summary>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    protected void LogWarning(string messageTemplate, params object[] propertyValues)
    {
        TaktLogger.Warning("[{ControllerType}] " + messageTemplate, 
            new object[] { GetType().Name }.Concat(propertyValues).ToArray());
    }

    /// <summary>
    /// 记录错误级别日志
    /// </summary>
    /// <param name="message">日志消息</param>
    protected void LogError(string message)
    {
        TaktLogger.Error("[{ControllerType}] {Message}", GetType().Name, message);
    }

    /// <summary>
    /// 记录错误级别日志（带异常）
    /// </summary>
    /// <param name="exception">异常对象</param>
    /// <param name="message">日志消息</param>
    protected void LogError(Exception exception, string message)
    {
        TaktLogger.Error(exception, "[{ControllerType}] {Message}", GetType().Name, message);
    }

    /// <summary>
    /// 记录错误级别日志（带参数）
    /// </summary>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    protected void LogError(string messageTemplate, params object[] propertyValues)
    {
        TaktLogger.Error("[{ControllerType}] " + messageTemplate, 
            new object[] { GetType().Name }.Concat(propertyValues).ToArray());
    }

    /// <summary>
    /// 记录调试级别日志
    /// </summary>
    /// <param name="message">日志消息</param>
    protected void LogDebug(string message)
    {
        TaktLogger.Debug("[{ControllerType}] {Message}", GetType().Name, message);
    }

    /// <summary>
    /// 记录调试级别日志（带参数）
    /// </summary>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    protected void LogDebug(string messageTemplate, params object[] propertyValues)
    {
        TaktLogger.Debug("[{ControllerType}] " + messageTemplate, 
            new object[] { GetType().Name }.Concat(propertyValues).ToArray());
    }

    /// <summary>
    /// 记录详细级别日志
    /// </summary>
    /// <param name="message">日志消息</param>
    protected void LogVerbose(string message)
    {
        TaktLogger.Verbose("[{ControllerType}] {Message}", GetType().Name, message);
    }

    /// <summary>
    /// 记录详细级别日志（带参数）
    /// </summary>
    /// <param name="messageTemplate">消息模板</param>
    /// <param name="propertyValues">属性值</param>
    protected void LogVerbose(string messageTemplate, params object[] propertyValues)
    {
        TaktLogger.Verbose("[{ControllerType}] " + messageTemplate, 
            new object[] { GetType().Name }.Concat(propertyValues).ToArray());
    }

    #endregion

    #region 统一响应格式

    /// <summary>
    /// 返回成功响应（无数据）
    /// </summary>
    /// <param name="message">成功消息</param>
    /// <returns>统一响应对象</returns>
    protected IActionResult Success(string? message = null)
    {
        return Ok(TaktApiResult.Ok(message ?? GetLocalizedString("common.feedback.success")));
    }

    /// <summary>
    /// 返回成功响应（带数据）
    /// </summary>
    /// <param name="data">响应数据</param>
    /// <param name="message">成功消息</param>
    /// <returns>统一响应对象</returns>
    protected IActionResult Success<T>(T data, string? message = null)
    {
        return Ok(TaktApiResult<T>.Ok(data, message ?? GetLocalizedString("common.feedback.success")));
    }

    /// <summary>
    /// 返回成功响应（带分页数据）
    /// </summary>
    /// <param name="items">数据列表</param>
    /// <param name="total">总记录数</param>
    /// <param name="message">成功消息</param>
    /// <returns>统一响应对象</returns>
    protected IActionResult Success<T>(List<T> items, int total, int pageIndex = 1, int pageSize = 10, string? message = null)
    {
        return Ok(TaktApiResult<TaktPagedResult<T>>.Ok(TaktPagedResult<T>.Create(items, total, pageIndex, pageSize), message ?? GetLocalizedString("common.feedback.query.success")));
    }

    /// <summary>
    /// 返回错误响应
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="code">错误代码</param>
    /// <returns>统一响应对象</returns>
    protected IActionResult Error(string? message = null, TaktResultCode code = TaktResultCode.BadRequest)
    {
        var resolvedMessage = message ?? GetLocalizedString("common.feedback.failed");
        var httpStatusCode = code switch
        {
            TaktResultCode.BadRequest => 400,
            TaktResultCode.Unauthorized => 401,
            TaktResultCode.Forbidden => 403,
            TaktResultCode.NotFound => 404,
            TaktResultCode.InternalServerError => 500,
            _ => 500
        };
        
        return StatusCode(httpStatusCode, TaktApiResult.Fail(resolvedMessage, code));
    }

    /// <summary>
    /// 校验登录前预览接口：查询参数 tenantCode 须与请求头 X-Tenant-Code 一致（禁止回落配置默认租户）
    /// </summary>
    /// <param name="tenantCode">查询参数租户编码</param>
    /// <returns>校验失败时返回 400；通过时返回 null</returns>
    protected IActionResult? ValidateLoginPreviewTenantHeader(string tenantCode)
    {
        var effectiveTenant = tenantCode.Trim();
        if (string.IsNullOrWhiteSpace(effectiveTenant))
        {
            return BadRequest("租户编码不能为空");
        }

        var headerTenant = TaktUserContext.TryResolveTenantCode(HttpContext);
        if (string.IsNullOrWhiteSpace(headerTenant))
        {
            return BadRequest("缺少 X-Tenant-Code 请求头");
        }

        if (!string.Equals(headerTenant, effectiveTenant, StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("X-Tenant-Code 与 tenantCode 不一致");
        }

        return null;
    }

    /// <summary>
    /// 返回参数错误响应（400）
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>统一响应对象</returns>
    protected IActionResult BadRequest(string? message = null)
    {
        return Error(message ?? GetLocalizedString("common.system.bad.request"), TaktResultCode.BadRequest);
    }

    /// <summary>
    /// 返回未授权响应（401）
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>统一响应对象</returns>
    protected IActionResult Unauthorized(string? message = null)
    {
        return Error(message ?? GetLocalizedString("common.permission.unauthorized"), TaktResultCode.Unauthorized);
    }

    /// <summary>
    /// 返回禁止访问响应（403）
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>统一响应对象</returns>
    protected IActionResult Forbidden(string? message = null)
    {
        return Error(message ?? GetLocalizedString("common.permission.forbidden"), TaktResultCode.Forbidden);
    }

    /// <summary>
    /// 返回未找到响应（404）
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <returns>统一响应对象</returns>
    protected IActionResult NotFound(string? message = null)
    {
        return Error(message ?? GetLocalizedString("common.system.resource.not.found"), TaktResultCode.NotFound);
    }

    #endregion

    #region 异常处理

    /// <summary>
    /// 处理异常并返回错误响应
    /// </summary>
    /// <param name="ex">异常</param>
    /// <returns>错误响应</returns>
    protected IActionResult HandleException(Exception ex)
    {
        // 本地化异常（使用 ITaktLocalizationService）
        if (ex is TaktLocalizedException lex)
        {
            TaktLogger.Error(ex, "[{ControllerType}] 控制器操作发生错误", GetType().Name);
            var localized = ResolveLocalizedException(lex);
            return Error(localized, TaktResultCode.BadRequest);
        }

        // 业务异常（使用 TaktBusinessException）
        if (ex is TaktBusinessException businessEx)
        {
            if (businessEx.ErrorCode?.StartsWith("error.tenant.database", StringComparison.Ordinal) == true)
            {
                TaktLogger.Warning(ex, "[{ControllerType}] 租户业务库未就绪: {Message}", GetType().Name, businessEx.Message);
            }
            else
            {
                TaktLogger.Error(ex, "[{ControllerType}] 控制器操作发生错误", GetType().Name);
            }

            return Error(
                businessEx.ErrorCode?.StartsWith("error.tenant.database", StringComparison.Ordinal) == true
                    ? ResolveTenantDatabaseMessage(businessEx)
                    : GetLocalizedExceptionMessage(businessEx),
                TaktResultCode.BadRequest);
        }

        if (TaktTenantDatabaseHelper.IsInfrastructureFailure(ex))
        {
            var configuration = HttpContext.RequestServices.GetService<IConfiguration>();
            var tenantCode = CurrentTenantCode ?? "000";
            var tenantDbEx = TaktTenantDatabaseHelper.CreateBusinessException(ex, configuration, tenantCode);
            TaktLogger.Warning(
                ex,
                "[{ControllerType}] 租户业务库不可用: Tenant={TenantCode}, Message={Message}",
                GetType().Name,
                tenantCode,
                tenantDbEx.Message);
            return Error(ResolveTenantDatabaseMessage(tenantDbEx), TaktResultCode.BadRequest);
        }

        TaktLogger.Error(ex, "[{ControllerType}] 控制器操作发生错误", GetType().Name);

        // 默认返回500错误（使用 TaktResultCode.InternalServerError）
        var errorMessage = GetLocalizedString("common.system.internal.error", "Frontend");
        return Error(errorMessage, TaktResultCode.InternalServerError);
    }

    #endregion

    #region 本地化方法

    /// <summary>
    /// 组装抽象校验文案（<c>{field}</c>、<c>{min}</c>、<c>{max}</c> 等占位）
    /// </summary>
    /// <param name="validationKey">校验键</param>
    /// <param name="fieldKey">字段标签键</param>
    /// <param name="min">最小值占位</param>
    /// <param name="max">最大值占位</param>
    /// <param name="extraTokens">额外占位符</param>
    /// <param name="fieldExtras">字段附加值</param>
    /// <returns>完整本地化校验提示</returns>
    protected string GetValidationMessage(
        string validationKey,
        string? fieldKey = null,
        int? min = null,
        int? max = null,
        IReadOnlyDictionary<string, string>? extraTokens = null,
        params object[] fieldExtras)
    {
        if (_localizationService == null) return validationKey;
        return TaktValidationMessageHelper.Build(
            k => _localizationService.Translate(k),
            validationKey,
            fieldKey,
            min,
            max,
            extraTokens,
            fieldExtras.Length == 0 ? null : fieldExtras);
    }

    /// <summary>
    /// 解析 TaktLocalizedException 为完整本地化文案。
    /// </summary>
    protected string ResolveLocalizedException(TaktLocalizedException lex)
    {
        if (_localizationService == null) return lex.MessageKey;
        if (!string.IsNullOrEmpty(lex.FieldKey) || lex.NamedTokens != null)
        {
            return TaktValidationMessageHelper.Build(
                k => _localizationService.Translate(k),
                lex.MessageKey,
                lex.FieldKey,
                extraTokens: lex.NamedTokens,
                fieldExtras: lex.FieldExtras.Length == 0 ? null : lex.FieldExtras);
        }

        try
        {
            return lex.Arguments.Length > 0
                ? _localizationService.Translate(lex.MessageKey, args: lex.Arguments)
                : _localizationService.Translate(lex.MessageKey);
        }
        catch
        {
            return lex.MessageKey;
        }
    }

    /// <summary>
    /// 获取本地化字符串
    /// </summary>
    /// <param name="key">国际化键</param>
    /// <param name="resourceType">资源类型（Frontend=前端，Backend=后端），默认为Backend</param>
    /// <returns>本地化字符串；本地化服务未注入时返回 key</returns>
    protected string GetLocalizedString(string key, string resourceType = "Backend")
    {
        if (_localizationService == null) return key;
        
        try
        {
            return _localizationService.Translate(key);
        }
        catch
        {
            return key;
        }
    }

    /// <summary>
    /// 获取本地化字符串（带参数）
    /// </summary>
    /// <param name="key">国际化键</param>
    /// <param name="resourceType">资源类型（Frontend=前端，Backend=后端），默认为Backend</param>
    /// <param name="arguments">参数数组</param>
    /// <returns>本地化字符串；本地化服务未注入时返回 key</returns>
    protected string GetLocalizedString(string key, string resourceType, params object[] arguments)
    {
        if (_localizationService == null) return key;
        
        try
        {
            return _localizationService.Translate(key, args: arguments);
        }
        catch
        {
            return key;
        }
    }

    /// <summary>
    /// 将异常转换为本地化文案；若非本地化异常则返回原始消息。
    /// </summary>
    protected string GetLocalizedExceptionMessage(Exception ex)
    {
        if (ex is TaktLocalizedException lex)
            return ResolveLocalizedException(lex);
        
        if (ex is TaktBusinessException bex)
        {
            if (!string.IsNullOrWhiteSpace(bex.ErrorCode))
            {
                var byCode = GetLocalizedString(bex.ErrorCode, "Backend");
                if (!string.Equals(byCode, bex.ErrorCode, StringComparison.Ordinal))
                {
                    return byCode;
                }
            }

            var backendText = GetLocalizedString(bex.Message, "Backend");
            if (!string.Equals(backendText, bex.Message, StringComparison.Ordinal))
            {
                return backendText;
            }

            var frontendText = GetLocalizedString(bex.Message, "Frontend");
            if (!string.Equals(frontendText, bex.Message, StringComparison.Ordinal))
            {
                return frontendText;
            }

            return bex.Message;
        }
        
        return ex.Message;
    }

    /// <summary>
    /// 解析租户业务库不可用文案（优先 i18n 键 error.tenant.database.*）
    /// </summary>
    /// <param name="businessEx">租户库业务异常</param>
    /// <returns>面向用户的说明</returns>
    protected string ResolveTenantDatabaseMessage(TaktBusinessException businessEx)
    {
        if (string.IsNullOrWhiteSpace(businessEx.ErrorCode) || _localizationService == null)
        {
            return businessEx.Message;
        }

        var tenantCode = CurrentTenantCode ?? string.Empty;
        var configuration = HttpContext.RequestServices.GetService<IConfiguration>();
        var databaseName = TaktTenantDatabaseHelper.ResolveDatabaseName(configuration, tenantCode);
        try
        {
            var localized = _localizationService.Translate(
                businessEx.ErrorCode,
                args: new object[] { tenantCode, databaseName });
            if (!string.IsNullOrWhiteSpace(localized)
                && !string.Equals(localized, businessEx.ErrorCode, StringComparison.Ordinal))
            {
                return localized;
            }
        }
        catch
        {
            // 回退默认中文说明
        }

        return businessEx.Message;
    }

    #endregion

    #region 验证方法

    /// <summary>
    /// 验证租户信息是否存在
    /// </summary>
    /// <returns>是否有效</returns>
    protected bool ValidateTenant()
    {
        if (string.IsNullOrWhiteSpace(CurrentTenantCode))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 验证公司信息是否存在
    /// </summary>
    /// <returns>是否有效</returns>
    protected bool ValidateCompany()
    {
        if (string.IsNullOrWhiteSpace(CurrentCompanyCode))
        {
            return false;
        }
        return true;
    }

    #endregion
}

