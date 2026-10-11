// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services
// 文件名称：TaktServiceBase.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：应用服务基类，提供三层数据隔离上下文、日志和异常处理能力
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Takt.Domain.Interfaces;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;

namespace Takt.Application.Services;

/// <summary>
/// 应用服务基类
/// 核心职责：
/// 1. 提供三层数据隔离上下文（租户→公司→用户）
/// 2. 提供统一日志处理
/// 3. 提供异常处理和本地化
/// 
/// 注意：
/// - 具体的CRUD操作应该在各自的业务服务中实现
/// - 仓储操作通过依赖注入获取，不在基类中封装
/// - 数据过滤由仓储层自动处理（TenantCode + CompanyCode + IsDeleted）
/// </summary>
public abstract class TaktServiceBase
{
    #region 三层数据隔离上下文

    protected readonly ITaktUserContext? _userContext;
    protected readonly ITaktLocalizationService? _localizationService;

    /// <summary>
    /// 第一层：租户编码（物理隔离 - 多数据库）
    /// 从用户上下文中获取，仓储层自动过滤
    /// </summary>
    protected string CurrentTenantCode => _userContext?.TenantCode ?? string.Empty;

    /// <summary>
    /// 第二层：公司编码（逻辑隔离 - 字段过滤）
    /// 从用户上下文中获取，仓储层自动过滤
    /// </summary>
    protected string CurrentCompanyCode => _userContext?.CompanyCode ?? string.Empty;

    /// <summary>
    /// 第三层：用户上下文（权限控制）
    /// 用于审计字段自动填充和权限验证
    /// </summary>
    protected long? CurrentUserId => _userContext?.UserId;
    protected string? CurrentUserName => _userContext?.UserName;
    protected bool IsAuthenticated => _userContext?.IsAuthenticated ?? false;

    /// <summary>
    /// 验证三层上下文是否完整
    /// </summary>
    protected void EnsureThreeLayerContext()
    {
        if (string.IsNullOrWhiteSpace(CurrentTenantCode))
        {
            ThrowBusinessException("租户上下文缺失：无法确定当前租户");
        }

        if (string.IsNullOrWhiteSpace(CurrentCompanyCode))
        {
            ThrowBusinessException("公司上下文缺失：无法确定当前公司");
        }

        if (!IsAuthenticated)
        {
            ThrowBusinessException("用户未认证：请先登录");
        }
    }

    #endregion

    #region 构造函数

    /// <summary>
    /// 构造函数（可选依赖注入）
    /// </summary>
    /// <param name="userContext">用户上下文（提供租户、公司、用户信息）</param>
    /// <param name="localizationService">本地化服务（可选）</param>
    protected TaktServiceBase(
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
    {
        _userContext = userContext;
        _localizationService = localizationService;

        // 记录上下文初始化日志
        LogContextInitialized();
    }

    /// <summary>
    /// 记录上下文初始化日志
    /// </summary>
    private void LogContextInitialized()
    {
        try
        {
            var serviceType = GetType().Name;
            var contextInfo = _userContext != null && _userContext.IsAuthenticated
                ? $"TenantCode: {_userContext.TenantCode}, CompanyCode: {_userContext.CompanyCode}, UserId: {_userContext.UserId}"
                : "未认证用户";

            TaktLogger.Debug("[服务初始化] {ServiceType} - {ContextInfo}", serviceType, contextInfo);
        }
        catch
        {
            // 忽略日志异常，不影响服务初始化
        }
    }

    #endregion

    #region 统一日志处理

    /// <summary>
    /// 记录信息日志
    /// </summary>
    protected void LogInformation(string message)
    {
        TaktLogger.Information("[{ServiceType}] {Message}", GetType().Name, message);
    }

    /// <summary>
    /// 记录信息日志（带参数）
    /// </summary>
    protected void LogInformation(string messageTemplate, params object[] propertyValues)
    {
        TaktLogger.Information("[{ServiceType}] " + messageTemplate,
            new object[] { GetType().Name }.Concat(propertyValues).ToArray());
    }

    /// <summary>
    /// 记录警告日志
    /// </summary>
    protected void LogWarning(string message)
    {
        TaktLogger.Warning("[{ServiceType}] {Message}", GetType().Name, message);
    }

    /// <summary>
    /// 记录错误日志
    /// </summary>
    protected void LogError(string message)
    {
        TaktLogger.Error("[{ServiceType}] {Message}", GetType().Name, message);
    }

    /// <summary>
    /// 记录错误日志（带异常）
    /// </summary>
    protected void LogError(Exception exception, string message)
    {
        TaktLogger.Error(exception, "[{ServiceType}] {Message}", GetType().Name, message);
    }

    /// <summary>
    /// 记录调试日志
    /// </summary>
    protected void LogDebug(string message)
    {
        TaktLogger.Debug("[{ServiceType}] {Message}", GetType().Name, message);
    }

    #endregion

    #region 异常处理

    /// <summary>
    /// 抛出业务异常
    /// </summary>
    /// <remarks>业务异常由全局异常处理中间件统一记录日志</remarks>
    [DoesNotReturn]
    protected void ThrowBusinessException(string message)
    {
        throw new TaktBusinessException(message);
    }

    /// <summary>
    /// 抛出业务异常（本地化）
    /// </summary>
    /// <remarks>业务异常由全局异常处理中间件统一记录日志</remarks>
    protected void ThrowBusinessExceptionLocalized(string key, params object[] arguments)
    {
        var message = _localizationService != null
            ? _localizationService.Translate(key, args: arguments)
            : key;

        throw new TaktBusinessException(message);
    }

    /// <summary>
    /// 抛出本地化异常（TaktLocalizedException，由控制器 ResolveLocalizedException 解析）
    /// </summary>
    /// <param name="messageKey">翻译键</param>
    /// <param name="fieldKey">字段标签键（entity.* / common.field.*），可为 null</param>
    /// <param name="max">写入 {max} 占位</param>
    /// <param name="extraTokens">额外命名占位（如 fileName、index）</param>
    /// <param name="fieldExtras">拼在字段名后的附加值</param>
    [DoesNotReturn]
    protected void ThrowLocalizedException(
        string messageKey,
        string? fieldKey = null,
        int? max = null,
        IReadOnlyDictionary<string, string>? extraTokens = null,
        params object[] fieldExtras)
    {
        Dictionary<string, string>? tokens = null;
        if (max.HasValue)
        {
            tokens = extraTokens != null
                ? new Dictionary<string, string>(extraTokens, StringComparer.Ordinal)
                : new Dictionary<string, string>(StringComparer.Ordinal);
            tokens["max"] = max.Value.ToString(CultureInfo.InvariantCulture);
        }
        else if (extraTokens != null)
        {
            tokens = new Dictionary<string, string>(extraTokens, StringComparer.Ordinal);
        }

        throw new TaktLocalizedException(messageKey, "Backend", fieldKey, tokens, fieldExtras);
    }

    /// <summary>
    /// 获取本地化消息（非异常场景）
    /// </summary>
    /// <param name="key">国际化键</param>
    /// <param name="arguments">格式化参数</param>
    /// <returns>本地化字符串；服务未注入时返回 key</returns>
    protected string GetLocalizedMessage(string key, params object[] arguments)
    {
        if (_localizationService == null)
        {
            return key;
        }

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
    /// 组装抽象校验文案（<c>{field}</c> 占位，字段名走 <c>entity.*</c> 或 <c>common.field.*</c>）
    /// </summary>
    /// <param name="validationKey">校验键（如 common.validation.duplicate）</param>
    /// <param name="fieldI18nKey">字段标签键</param>
    /// <param name="fieldExtras">拼入字段后的附加值（如用户名、ID）</param>
    /// <returns>完整本地化校验提示</returns>
    protected string GetValidationMessage(string validationKey, string fieldI18nKey, params object[] fieldExtras)
    {
        if (_localizationService == null)
        {
            return validationKey;
        }

        return TaktValidationMessageHelper.Build(
            k => GetLocalizedMessage(k),
            validationKey,
            fieldI18nKey,
            fieldExtras: fieldExtras.Length == 0 ? null : fieldExtras);
    }

    /// <summary>
    /// 抛出抽象校验业务异常（<c>{field}</c> + entity.* / common.field.*）
    /// </summary>
    /// <param name="validationKey">校验键</param>
    /// <param name="fieldI18nKey">字段标签键</param>
    /// <param name="fieldExtras">字段附加值</param>
    [DoesNotReturn]
    protected void ThrowValidationLocalized(string validationKey, string fieldI18nKey, params object[] fieldExtras)
    {
        throw new TaktBusinessException(GetValidationMessage(validationKey, fieldI18nKey, fieldExtras));
    }

    /// <summary>
    /// 验证实体是否存在，不存在则抛出异常
    /// </summary>
    protected T EnsureExists<T>(T? entity, string errorMessage) where T : class
    {
        if (entity == null)
        {
            ThrowBusinessException(errorMessage);
        }
        return entity!;
    }

    /// <summary>
    /// 验证实体是否存在，不存在则抛出异常（本地化）
    /// </summary>
    protected T EnsureExistsLocalized<T>(T? entity, string key, params object[] arguments) where T : class
    {
        if (entity == null)
        {
            ThrowBusinessExceptionLocalized(key, arguments);
        }
        return entity!;
    }

    #endregion

    #region 数据权限验证（第三层隔离）

    /// <summary>
    /// 验证当前用户是否有权限访问指定公司
    /// 第三层隔离：业务数据权限控制
    /// </summary>
    /// <param name="targetCompanyCode">目标公司编码</param>
    /// <remarks>
    /// 注意：
    /// 1. 租户层和公司层的过滤由仓储层自动处理
    /// 2. 此方法用于验证用户是否有权限访问特定公司的数据（跨公司场景）
    /// 3. 具体实现需要注入 ITaktPermissionService
    /// </remarks>
    protected virtual Task<bool> HasCompanyAccessAsync(string targetCompanyCode)
    {
        // 默认实现：只能访问当前公司
        // 子类可以重写此方法，注入 ITaktPermissionService 实现复杂逻辑
        return Task.FromResult(targetCompanyCode == CurrentCompanyCode);
    }

    /// <summary>
    /// 验证当前用户是否有权限操作指定数据
    /// 第三层隔离：基于数据权限范围的控制
    /// </summary>
    /// <param name="dataOwnerUserId">数据所有者用户ID</param>
    /// <remarks>
    /// 数据权限范围：
    /// - All: 所有数据
    /// - Company: 本公司数据
    /// - Department: 本部门数据
    /// - Self: 仅本人数据
    /// </remarks>
    protected virtual Task<bool> HasDataAccessAsync(long dataOwnerUserId)
    {
        // 默认实现：只能访问自己的数据
        // 子类可以重写此方法，实现基于角色的数据权限控制
        return Task.FromResult(dataOwnerUserId == CurrentUserId);
    }

    #endregion

    #region 审批操作辅助方法

    /// <summary>
    /// 获取当前用户ID作为提交人/审批人
    /// 适用于需要审批流程的业务服务（如请假、报销、采购等）
    /// </summary>
    /// <exception cref="TaktBusinessException">用户未认证时抛出</exception>
    protected long GetCurrentUserIdForApproval()
    {
        if (!IsAuthenticated || CurrentUserId == null)
        {
            ThrowBusinessException("用户未认证，无法执行审批操作");
        }

        return CurrentUserId.Value;
    }

    /// <summary>
    /// 记录审批操作日志
    /// </summary>
    /// <param name="action">操作类型（提交/通过/驳回/撤销）</param>
    /// <param name="entityId">实体ID</param>
    /// <param name="opinion">审批意见</param>
    protected void LogApprovalAction(string action, long entityId, string? opinion = null)
    {
        var opinionText = string.IsNullOrWhiteSpace(opinion) ? "无" : opinion;
        LogInformation("审批操作: Action={Action}, EntityId={EntityId}, UserId={UserId}, Opinion={Opinion}",
            action, entityId, CurrentUserId ?? 0, opinionText);
    }

    #endregion
}
