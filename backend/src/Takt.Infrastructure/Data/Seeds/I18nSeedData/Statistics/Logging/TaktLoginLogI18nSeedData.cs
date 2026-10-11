// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktLoginLogI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktLoginLog 实体字段国际化种子（已对齐前端 locales：src/locales/statistics/logging/login-log）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging;

/// <summary>
/// TaktLoginLog 实体国际化翻译种子（键前缀 entity.loginlog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktLoginLogI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（实体翻译种子，位于部门翻译之后）
    /// </summary>
    public int Order => 52;

    /// <summary>
    /// 初始化实体字段翻译种子
    /// </summary>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 TaktLoginLog 实体国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过实体国际化翻译种子初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 loginlog 实体翻译...", tenantCode);

        foreach (var item in GetLoginLogTranslations())
        {
            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);
                continue;
            }

            var (translation, i, u) = await CreateOrUpdateTranslationAsync(
                repository,
                tenantCode,
                cultureId,
                item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("TaktLoginLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktLoginLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.loginlog._self / entity.loginlog.{{field}}；ResourceGroup=Logging；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetLoginLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.loginlog._self
            new TranslationSeedItem("entity.loginlog._self", "en-US", "Login Log Information_us", "实体名称"),
            // entity.loginlog._self
            new TranslationSeedItem("entity.loginlog._self", "ja-JP", "登录日志信息_jp", "实体名称"),
            // entity.loginlog._self
            new TranslationSeedItem("entity.loginlog._self", "zh-CN", "登录日志信息", "实体名称"),
            // entity.loginlog._self
            new TranslationSeedItem("entity.loginlog._self", "zh-HK", "登录日志信息_hk", "实体名称"),

            // entity.loginlog.username
            new TranslationSeedItem("entity.loginlog.username", "en-US", "用户名_us", "用户名（登录账号）"),
            // entity.loginlog.username
            new TranslationSeedItem("entity.loginlog.username", "ja-JP", "用户名_jp", "用户名（登录账号）"),
            // entity.loginlog.username
            new TranslationSeedItem("entity.loginlog.username", "zh-CN", "用户名", "用户名（登录账号）"),
            // entity.loginlog.username
            new TranslationSeedItem("entity.loginlog.username", "zh-HK", "用户名_hk", "用户名（登录账号）"),

            // entity.loginlog.logintype
            new TranslationSeedItem("entity.loginlog.logintype", "en-US", "登录方式_us", "登录方式（TaktConstants.LoginType；如 password=账号密码、refreshtoken=刷新令牌）"),
            // entity.loginlog.logintype
            new TranslationSeedItem("entity.loginlog.logintype", "ja-JP", "登录方式_jp", "登录方式（TaktConstants.LoginType；如 password=账号密码、refreshtoken=刷新令牌）"),
            // entity.loginlog.logintype
            new TranslationSeedItem("entity.loginlog.logintype", "zh-CN", "登录方式", "登录方式（TaktConstants.LoginType；如 password=账号密码、refreshtoken=刷新令牌）"),
            // entity.loginlog.logintype
            new TranslationSeedItem("entity.loginlog.logintype", "zh-HK", "登录方式_hk", "登录方式（TaktConstants.LoginType；如 password=账号密码、refreshtoken=刷新令牌）"),

            // entity.loginlog.browser
            new TranslationSeedItem("entity.loginlog.browser", "en-US", "浏览器_us", "浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）"),
            // entity.loginlog.browser
            new TranslationSeedItem("entity.loginlog.browser", "ja-JP", "浏览器_jp", "浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）"),
            // entity.loginlog.browser
            new TranslationSeedItem("entity.loginlog.browser", "zh-CN", "浏览器", "浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）"),
            // entity.loginlog.browser
            new TranslationSeedItem("entity.loginlog.browser", "zh-HK", "浏览器_hk", "浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）"),

            // entity.loginlog.os
            new TranslationSeedItem("entity.loginlog.os", "en-US", "操作系统_us", "操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）"),
            // entity.loginlog.os
            new TranslationSeedItem("entity.loginlog.os", "ja-JP", "操作系统_jp", "操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）"),
            // entity.loginlog.os
            new TranslationSeedItem("entity.loginlog.os", "zh-CN", "操作系统", "操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）"),
            // entity.loginlog.os
            new TranslationSeedItem("entity.loginlog.os", "zh-HK", "操作系统_hk", "操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）"),

            // entity.loginlog.useragent
            new TranslationSeedItem("entity.loginlog.useragent", "en-US", "用户代理_us", "用户代理（User-Agent）"),
            // entity.loginlog.useragent
            new TranslationSeedItem("entity.loginlog.useragent", "ja-JP", "用户代理_jp", "用户代理（User-Agent）"),
            // entity.loginlog.useragent
            new TranslationSeedItem("entity.loginlog.useragent", "zh-CN", "用户代理", "用户代理（User-Agent）"),
            // entity.loginlog.useragent
            new TranslationSeedItem("entity.loginlog.useragent", "zh-HK", "用户代理_hk", "用户代理（User-Agent）"),

            // entity.loginlog.loginresult
            new TranslationSeedItem("entity.loginlog.loginresult", "en-US", "登录结果_us", "登录结果（TaktConstants.LoginResult；如 success=成功、passworderror=密码错误）"),
            // entity.loginlog.loginresult
            new TranslationSeedItem("entity.loginlog.loginresult", "ja-JP", "登录结果_jp", "登录结果（TaktConstants.LoginResult；如 success=成功、passworderror=密码错误）"),
            // entity.loginlog.loginresult
            new TranslationSeedItem("entity.loginlog.loginresult", "zh-CN", "登录结果", "登录结果（TaktConstants.LoginResult；如 success=成功、passworderror=密码错误）"),
            // entity.loginlog.loginresult
            new TranslationSeedItem("entity.loginlog.loginresult", "zh-HK", "登录结果_hk", "登录结果（TaktConstants.LoginResult；如 success=成功、passworderror=密码错误）"),

            // entity.loginlog.loginmessage
            new TranslationSeedItem("entity.loginlog.loginmessage", "en-US", "登录结果消息_us", "登录结果消息"),
            // entity.loginlog.loginmessage
            new TranslationSeedItem("entity.loginlog.loginmessage", "ja-JP", "登录结果消息_jp", "登录结果消息"),
            // entity.loginlog.loginmessage
            new TranslationSeedItem("entity.loginlog.loginmessage", "zh-CN", "登录结果消息", "登录结果消息"),
            // entity.loginlog.loginmessage
            new TranslationSeedItem("entity.loginlog.loginmessage", "zh-HK", "登录结果消息_hk", "登录结果消息"),

            // entity.loginlog.loginip
            new TranslationSeedItem("entity.loginlog.loginip", "en-US", "登录IP地址_us", "登录IP地址"),
            // entity.loginlog.loginip
            new TranslationSeedItem("entity.loginlog.loginip", "ja-JP", "登录IP地址_jp", "登录IP地址"),
            // entity.loginlog.loginip
            new TranslationSeedItem("entity.loginlog.loginip", "zh-CN", "登录IP地址", "登录IP地址"),
            // entity.loginlog.loginip
            new TranslationSeedItem("entity.loginlog.loginip", "zh-HK", "登录IP地址_hk", "登录IP地址"),

            // entity.loginlog.loginlocation
            new TranslationSeedItem("entity.loginlog.loginlocation", "en-US", "登录地点_us", "登录地点（IP解析，如：中国-广东省-深圳市）"),
            // entity.loginlog.loginlocation
            new TranslationSeedItem("entity.loginlog.loginlocation", "ja-JP", "登录地点_jp", "登录地点（IP解析，如：中国-广东省-深圳市）"),
            // entity.loginlog.loginlocation
            new TranslationSeedItem("entity.loginlog.loginlocation", "zh-CN", "登录地点", "登录地点（IP解析，如：中国-广东省-深圳市）"),
            // entity.loginlog.loginlocation
            new TranslationSeedItem("entity.loginlog.loginlocation", "zh-HK", "登录地点_hk", "登录地点（IP解析，如：中国-广东省-深圳市）"),

            // entity.loginlog.logoutat
            new TranslationSeedItem("entity.loginlog.logoutat", "en-US", "登出时间_us", "登出时间（未登出时为 null；登出成功时由 CloseOpenLoginSessionAsync 回填，对齐 TaktOnline.DisconnectTime）"),
            // entity.loginlog.logoutat
            new TranslationSeedItem("entity.loginlog.logoutat", "ja-JP", "登出时间_jp", "登出时间（未登出时为 null；登出成功时由 CloseOpenLoginSessionAsync 回填，对齐 TaktOnline.DisconnectTime）"),
            // entity.loginlog.logoutat
            new TranslationSeedItem("entity.loginlog.logoutat", "zh-CN", "登出时间", "登出时间（未登出时为 null；登出成功时由 CloseOpenLoginSessionAsync 回填，对齐 TaktOnline.DisconnectTime）"),
            // entity.loginlog.logoutat
            new TranslationSeedItem("entity.loginlog.logoutat", "zh-HK", "登出时间_hk", "登出时间（未登出时为 null；登出成功时由 CloseOpenLoginSessionAsync 回填，对齐 TaktOnline.DisconnectTime）"),
        };
    }

    /// <summary>
    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）
    /// </summary>
    private static void ApplyTranslationFields(
        TaktTranslation translation,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        translation.TenantCode = tenantCode;
        translation.CultureId = cultureId;
        translation.CultureCode = item.CultureCode;
        translation.I18nKey = item.I18nKey;
        translation.TranslationText = item.TranslationText;
        translation.ResourceGroup = "Logging";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
        translation.Remark = null;
        translation.IsDeleted = 0;
        translation.DeletedBy = null;
        translation.DeletedAt = null;
    }

    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(
        ITaktTenantSeedRepository<TaktTranslation> repository,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        var translation = await repository.FirstAsync(t =>
            t.TenantCode == tenantCode &&
            t.I18nKey == item.I18nKey &&
            t.CultureCode == item.CultureCode);

        if (translation == null)
        {
            translation = new TaktTranslation();
            ApplyTranslationFields(translation, tenantCode, cultureId, item);
            translation = await repository.CreateAsync(translation);
            return (translation, 1, 0);
        }

        ApplyTranslationFields(translation, tenantCode, cultureId, item);
        await repository.UpdateAsync(translation);
        return (translation, 0, 1);
    }

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
