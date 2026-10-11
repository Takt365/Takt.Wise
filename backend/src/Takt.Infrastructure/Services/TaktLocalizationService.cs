// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktLocalizationService.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：本地化服务实现（数据库翻译 + resx 兜底）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Services;

/// <summary>
/// ITaktLocalizationService 实现
/// 按 TaktLocalizationOptions.UseDatabaseLocalization 优先查库，未命中时回退 resx
/// </summary>
public class TaktLocalizationService : ITaktLocalizationService
{
    /// <summary>
    /// 本地化资源程序集定位（用于 IStringLocalizerFactory.Create）
    /// </summary>
    private static readonly string LocalizationResourceLocation =
        typeof(TaktLocalizationService).Assembly.FullName
        ?? typeof(TaktLocalizationService).Assembly.Location;

    /// <summary>
    /// 字符串本地化工厂
    /// </summary>
    private readonly IStringLocalizerFactory _localizerFactory;

    /// <summary>
    /// 本地化配置（默认语言、是否使用数据库等）
    /// </summary>
    private readonly TaktLocalizationOptions _options;

    /// <summary>
    /// 租户 SqlSugar 上下文（数据库翻译时注入；无 HTTP 场景可为 null）
    /// </summary>
    private readonly TaktSqlSugarContext? _dbContext;

    /// <summary>
    /// HTTP 上下文访问器（匿名登录页判断是否可连库翻译）
    /// </summary>
    private readonly IHttpContextAccessor? _httpContextAccessor;

    /// <summary>
    /// 租户请求头配置
    /// </summary>
    private readonly TaktTenantContextOptions _tenantContextOptions;

    /// <summary>
    /// 后端通用 resx 本地化器（懒加载）
    /// </summary>
    private IStringLocalizer? _backendLocalizer;

    /// <summary>
    /// 前端通用 resx 本地化器（懒加载）
    /// </summary>
    private IStringLocalizer? _frontendLocalizer;

    /// <summary>
    /// 验证消息 resx 本地化器（懒加载）
    /// </summary>
    private IStringLocalizer? _validationLocalizer;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="localizerFactory">字符串本地化工厂</param>
    /// <param name="options">本地化配置</param>
    /// <param name="tenantContextOptions">租户上下文配置</param>
    /// <param name="dbContext">数据库上下文（可选，启用数据库翻译时使用）</param>
    /// <param name="httpContextAccessor">HTTP 上下文（可选）</param>
    public TaktLocalizationService(
        IStringLocalizerFactory localizerFactory,
        IOptions<TaktLocalizationOptions> options,
        IOptions<TaktTenantContextOptions> tenantContextOptions,
        TaktSqlSugarContext? dbContext = null,
        IHttpContextAccessor? httpContextAccessor = null)
    {
        _localizerFactory = localizerFactory;
        _options = options.Value;
        _tenantContextOptions = tenantContextOptions.Value;
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 翻译文本；未匹配时直接返回资源键（无跨语言或 resx 兜底时记 Warning）
    /// </summary>
    /// <param name="key">翻译键</param>
    /// <param name="culture">语言代码（可选，默认当前语言）</param>
    /// <param name="args">格式化参数</param>
    /// <returns>翻译文本</returns>
    public string Translate(string key, string? culture = null, params object[] args)
    {
        var targetCulture = culture ?? GetCurrentCulture();

        if (CanUseDatabaseLocalization())
        {
            var translation = GetFromDatabase(key, targetCulture);
            if (!string.IsNullOrEmpty(translation))
            {
                return args.Length > 0 ? string.Format(translation, args) : translation;
            }

            var resxFallback = GetFromResx(key, targetCulture);
            if (!string.IsNullOrEmpty(resxFallback))
            {
                return args.Length > 0 ? string.Format(resxFallback, args) : resxFallback;
            }

            TaktLogger.Warning("翻译键未找到：{Key}，语言：{Culture}", key, targetCulture);
            return key;
        }

        var localizer = GetBackendLocalizer();
        var localizedString = localizer[key];

        if (localizedString.ResourceNotFound)
        {
            TaktLogger.Warning("翻译键未找到：{Key}，语言：{Culture}", key, targetCulture);
            return key;
        }

        return args.Length > 0 ? string.Format(localizedString.Value, args) : localizedString.Value;
    }

    /// <summary>
    /// 翻译异常消息
    /// </summary>
    /// <param name="messageKey">消息键</param>
    /// <param name="resourceType">资源类型（Backend / Frontend）</param>
    /// <param name="culture">语言代码</param>
    /// <param name="args">格式化参数</param>
    /// <returns>翻译文本；未匹配时返回消息键</returns>
    public string TranslateException(string messageKey, string resourceType = "Backend", string? culture = null, params object[] args)
    {
        var localizer = resourceType == "Frontend" ? GetFrontendLocalizer() : GetBackendLocalizer();
        var localizedString = localizer[messageKey];

        if (localizedString.ResourceNotFound)
        {
            TaktLogger.Error("异常翻译键未找到：{Key}，类型：{Type}", messageKey, resourceType);
            return messageKey;
        }

        return args.Length > 0 ? string.Format(localizedString.Value, args) : localizedString.Value;
    }

    /// <summary>
    /// 翻译验证消息
    /// </summary>
    /// <param name="messageKey">消息键</param>
    /// <param name="culture">语言代码</param>
    /// <param name="args">格式化参数</param>
    /// <returns>翻译文本；未匹配时返回消息键</returns>
    public string TranslateValidation(string messageKey, string? culture = null, params object[] args)
    {
        var localizer = GetValidationLocalizer();
        var localizedString = localizer[messageKey];

        if (localizedString.ResourceNotFound)
        {
            return messageKey;
        }

        return args.Length > 0 ? string.Format(localizedString.Value, args) : localizedString.Value;
    }

    /// <summary>
    /// 获取当前 UI 语言（BCP47）
    /// </summary>
    /// <returns>当前语言代码</returns>
    public string GetCurrentCulture()
    {
        return CultureInfo.CurrentUICulture.Name;
    }

    /// <summary>
    /// 当前请求是否允许走数据库翻译（已登录或请求已解析出租户）
    /// </summary>
    /// <returns>允许为 true</returns>
    private bool CanUseDatabaseLocalization()
    {
        if (!_options.UseDatabaseLocalization || _dbContext == null)
        {
            return false;
        }

        var httpContext = _httpContextAccessor?.HttpContext;
        if (httpContext == null)
        {
            return false;
        }

        if (httpContext.User?.Identity?.IsAuthenticated == true)
        {
            return true;
        }

        return !string.IsNullOrWhiteSpace(
            TaktUserContext.TryResolveTenantCode(httpContext, _tenantContextOptions.TenantHeaderName));
    }

    /// <summary>
    /// 从数据库获取翻译（按完整 BCP47 CultureCode 与有效 CultureId 匹配）
    /// </summary>
    /// <param name="key">国际化键</param>
    /// <param name="culture">语言代码</param>
    /// <returns>译文；未命中或异常时返回 null</returns>
    private string? GetFromDatabase(string key, string culture)
    {
        try
        {
            var cultureCode = string.IsNullOrWhiteSpace(culture) ? _options.DefaultCulture : culture.Trim();

            var translation = _dbContext!.Db.Queryable<Takt.Domain.Entities.Foundation.TaktTranslation>()
                .Where(x => x.I18nKey == key
                    && x.CultureCode == cultureCode
                    && x.CultureId > 0)
                .Select(x => x.TranslationText)
                .First();

            return translation;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "从数据库获取翻译失败：{Key}", key);
            return null;
        }
    }

    /// <summary>
    /// 从 resx 资源获取翻译（数据库未命中时的兜底）
    /// </summary>
    /// <param name="key">国际化键</param>
    /// <param name="culture">语言代码</param>
    /// <returns>译文；未命中或文化不存在时返回 null</returns>
    private string? GetFromResx(string key, string culture)
    {
        var previousCulture = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
            var localizedString = GetBackendLocalizer()[key];
            return localizedString.ResourceNotFound ? null : localizedString.Value;
        }
        catch (CultureNotFoundException)
        {
            return null;
        }
        finally
        {
            CultureInfo.CurrentUICulture = previousCulture;
        }
    }

    /// <summary>
    /// 获取后端本地化器
    /// </summary>
    /// <returns>后端 IStringLocalizer</returns>
    private IStringLocalizer GetBackendLocalizer()
    {
        _backendLocalizer ??= _localizerFactory.Create("TaktCommon", LocalizationResourceLocation);
        return _backendLocalizer;
    }

    /// <summary>
    /// 获取前端本地化器
    /// </summary>
    /// <returns>前端 IStringLocalizer</returns>
    private IStringLocalizer GetFrontendLocalizer()
    {
        _frontendLocalizer ??= _localizerFactory.Create("TaktCommon", LocalizationResourceLocation);
        return _frontendLocalizer;
    }

    /// <summary>
    /// 获取验证本地化器
    /// </summary>
    /// <returns>验证 IStringLocalizer</returns>
    private IStringLocalizer GetValidationLocalizer()
    {
        _validationLocalizer ??= _localizerFactory.Create("TaktCommon", LocalizationResourceLocation);
        return _validationLocalizer;
    }
}
