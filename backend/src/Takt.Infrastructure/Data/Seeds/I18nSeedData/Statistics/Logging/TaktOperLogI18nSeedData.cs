// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktOperLogI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktOperLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktOperLog 实体国际化翻译种子（键前缀 entity.operlog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktOperLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktOperLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 operlog 实体翻译...", tenantCode);

        foreach (var item in GetOperLogTranslations())
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

        TaktLogger.Information("TaktOperLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktOperLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.operlog._self / entity.operlog.{{field}}；ResourceGroup=Logging；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetOperLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.operlog._self
            new TranslationSeedItem("entity.operlog._self", "en-US", "Oper Log Information_us", "实体名称"),
            // entity.operlog._self
            new TranslationSeedItem("entity.operlog._self", "ja-JP", "操作日志信息_jp", "实体名称"),
            // entity.operlog._self
            new TranslationSeedItem("entity.operlog._self", "zh-CN", "操作日志信息", "实体名称"),
            // entity.operlog._self
            new TranslationSeedItem("entity.operlog._self", "zh-HK", "操作日志信息_hk", "实体名称"),

            // entity.operlog.username
            new TranslationSeedItem("entity.operlog.username", "en-US", "用户名_us", "用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）"),
            // entity.operlog.username
            new TranslationSeedItem("entity.operlog.username", "ja-JP", "用户名_jp", "用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）"),
            // entity.operlog.username
            new TranslationSeedItem("entity.operlog.username", "zh-CN", "用户名", "用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）"),
            // entity.operlog.username
            new TranslationSeedItem("entity.operlog.username", "zh-HK", "用户名_hk", "用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）"),

            // entity.operlog.opermodule
            new TranslationSeedItem("entity.operlog.opermodule", "en-US", "操作模块_us", "操作模块（如：用户管理、部门管理）"),
            // entity.operlog.opermodule
            new TranslationSeedItem("entity.operlog.opermodule", "ja-JP", "操作模块_jp", "操作模块（如：用户管理、部门管理）"),
            // entity.operlog.opermodule
            new TranslationSeedItem("entity.operlog.opermodule", "zh-CN", "操作模块", "操作模块（如：用户管理、部门管理）"),
            // entity.operlog.opermodule
            new TranslationSeedItem("entity.operlog.opermodule", "zh-HK", "操作模块_hk", "操作模块（如：用户管理、部门管理）"),

            // entity.operlog.opertype
            new TranslationSeedItem("entity.operlog.opertype", "en-US", "操作类型_us", "操作类型（TaktConstants.OperType；如 create=新增、update=修改、delete=删除、query=查询）"),
            // entity.operlog.opertype
            new TranslationSeedItem("entity.operlog.opertype", "ja-JP", "操作类型_jp", "操作类型（TaktConstants.OperType；如 create=新增、update=修改、delete=删除、query=查询）"),
            // entity.operlog.opertype
            new TranslationSeedItem("entity.operlog.opertype", "zh-CN", "操作类型", "操作类型（TaktConstants.OperType；如 create=新增、update=修改、delete=删除、query=查询）"),
            // entity.operlog.opertype
            new TranslationSeedItem("entity.operlog.opertype", "zh-HK", "操作类型_hk", "操作类型（TaktConstants.OperType；如 create=新增、update=修改、delete=删除、query=查询）"),

            // entity.operlog.opermethod
            new TranslationSeedItem("entity.operlog.opermethod", "en-US", "操作方法_us", "操作方法（如：TaktUserService.CreateUserAsync）"),
            // entity.operlog.opermethod
            new TranslationSeedItem("entity.operlog.opermethod", "ja-JP", "操作方法_jp", "操作方法（如：TaktUserService.CreateUserAsync）"),
            // entity.operlog.opermethod
            new TranslationSeedItem("entity.operlog.opermethod", "zh-CN", "操作方法", "操作方法（如：TaktUserService.CreateUserAsync）"),
            // entity.operlog.opermethod
            new TranslationSeedItem("entity.operlog.opermethod", "zh-HK", "操作方法_hk", "操作方法（如：TaktUserService.CreateUserAsync）"),

            // entity.operlog.requestmethod
            new TranslationSeedItem("entity.operlog.requestmethod", "en-US", "请求方式_us", "请求方式（GET、POST、PUT、DELETE 等）"),
            // entity.operlog.requestmethod
            new TranslationSeedItem("entity.operlog.requestmethod", "ja-JP", "请求方式_jp", "请求方式（GET、POST、PUT、DELETE 等）"),
            // entity.operlog.requestmethod
            new TranslationSeedItem("entity.operlog.requestmethod", "zh-CN", "请求方式", "请求方式（GET、POST、PUT、DELETE 等）"),
            // entity.operlog.requestmethod
            new TranslationSeedItem("entity.operlog.requestmethod", "zh-HK", "请求方式_hk", "请求方式（GET、POST、PUT、DELETE 等）"),

            // entity.operlog.operurl
            new TranslationSeedItem("entity.operlog.operurl", "en-US", "操作URL_us", "操作 URL（含查询字符串）"),
            // entity.operlog.operurl
            new TranslationSeedItem("entity.operlog.operurl", "ja-JP", "操作URL_jp", "操作 URL（含查询字符串）"),
            // entity.operlog.operurl
            new TranslationSeedItem("entity.operlog.operurl", "zh-CN", "操作URL", "操作 URL（含查询字符串）"),
            // entity.operlog.operurl
            new TranslationSeedItem("entity.operlog.operurl", "zh-HK", "操作URL_hk", "操作 URL（含查询字符串）"),

            // entity.operlog.requestparam
            new TranslationSeedItem("entity.operlog.requestparam", "en-US", "请求参数_us", "请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）"),
            // entity.operlog.requestparam
            new TranslationSeedItem("entity.operlog.requestparam", "ja-JP", "请求参数_jp", "请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）"),
            // entity.operlog.requestparam
            new TranslationSeedItem("entity.operlog.requestparam", "zh-CN", "请求参数", "请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）"),
            // entity.operlog.requestparam
            new TranslationSeedItem("entity.operlog.requestparam", "zh-HK", "请求参数_hk", "请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）"),

            // entity.operlog.jsonresult
            new TranslationSeedItem("entity.operlog.jsonresult", "en-US", "返回结果_us", "返回结果 JSON（当前操作出参/响应摘要）"),
            // entity.operlog.jsonresult
            new TranslationSeedItem("entity.operlog.jsonresult", "ja-JP", "返回结果_jp", "返回结果 JSON（当前操作出参/响应摘要）"),
            // entity.operlog.jsonresult
            new TranslationSeedItem("entity.operlog.jsonresult", "zh-CN", "返回结果", "返回结果 JSON（当前操作出参/响应摘要）"),
            // entity.operlog.jsonresult
            new TranslationSeedItem("entity.operlog.jsonresult", "zh-HK", "返回结果_hk", "返回结果 JSON（当前操作出参/响应摘要）"),

            // entity.operlog.errormsg
            new TranslationSeedItem("entity.operlog.errormsg", "en-US", "错误消息_us", "错误消息（失败时；成功为空串）"),
            // entity.operlog.errormsg
            new TranslationSeedItem("entity.operlog.errormsg", "ja-JP", "错误消息_jp", "错误消息（失败时；成功为空串）"),
            // entity.operlog.errormsg
            new TranslationSeedItem("entity.operlog.errormsg", "zh-CN", "错误消息", "错误消息（失败时；成功为空串）"),
            // entity.operlog.errormsg
            new TranslationSeedItem("entity.operlog.errormsg", "zh-HK", "错误消息_hk", "错误消息（失败时；成功为空串）"),

            // entity.operlog.operip
            new TranslationSeedItem("entity.operlog.operip", "en-US", "操作IP_us", "操作 IP"),
            // entity.operlog.operip
            new TranslationSeedItem("entity.operlog.operip", "ja-JP", "操作IP_jp", "操作 IP"),
            // entity.operlog.operip
            new TranslationSeedItem("entity.operlog.operip", "zh-CN", "操作IP", "操作 IP"),
            // entity.operlog.operip
            new TranslationSeedItem("entity.operlog.operip", "zh-HK", "操作IP_hk", "操作 IP"),

            // entity.operlog.operlocation
            new TranslationSeedItem("entity.operlog.operlocation", "en-US", "操作地点_us", "操作地点（由 OperIp 解析，如：中国-广东省-深圳市）"),
            // entity.operlog.operlocation
            new TranslationSeedItem("entity.operlog.operlocation", "ja-JP", "操作地点_jp", "操作地点（由 OperIp 解析，如：中国-广东省-深圳市）"),
            // entity.operlog.operlocation
            new TranslationSeedItem("entity.operlog.operlocation", "zh-CN", "操作地点", "操作地点（由 OperIp 解析，如：中国-广东省-深圳市）"),
            // entity.operlog.operlocation
            new TranslationSeedItem("entity.operlog.operlocation", "zh-HK", "操作地点_hk", "操作地点（由 OperIp 解析，如：中国-广东省-深圳市）"),

            // entity.operlog.useragent
            new TranslationSeedItem("entity.operlog.useragent", "en-US", "用户代理_us", "用户代理（User-Agent）"),
            // entity.operlog.useragent
            new TranslationSeedItem("entity.operlog.useragent", "ja-JP", "用户代理_jp", "用户代理（User-Agent）"),
            // entity.operlog.useragent
            new TranslationSeedItem("entity.operlog.useragent", "zh-CN", "用户代理", "用户代理（User-Agent）"),
            // entity.operlog.useragent
            new TranslationSeedItem("entity.operlog.useragent", "zh-HK", "用户代理_hk", "用户代理（User-Agent）"),

            // entity.operlog.browser
            new TranslationSeedItem("entity.operlog.browser", "en-US", "浏览器_us", "浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）"),
            // entity.operlog.browser
            new TranslationSeedItem("entity.operlog.browser", "ja-JP", "浏览器_jp", "浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）"),
            // entity.operlog.browser
            new TranslationSeedItem("entity.operlog.browser", "zh-CN", "浏览器", "浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）"),
            // entity.operlog.browser
            new TranslationSeedItem("entity.operlog.browser", "zh-HK", "浏览器_hk", "浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）"),

            // entity.operlog.os
            new TranslationSeedItem("entity.operlog.os", "en-US", "操作系统_us", "操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）"),
            // entity.operlog.os
            new TranslationSeedItem("entity.operlog.os", "ja-JP", "操作系统_jp", "操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）"),
            // entity.operlog.os
            new TranslationSeedItem("entity.operlog.os", "zh-CN", "操作系统", "操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）"),
            // entity.operlog.os
            new TranslationSeedItem("entity.operlog.os", "zh-HK", "操作系统_hk", "操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）"),

            // entity.operlog.devicetype
            new TranslationSeedItem("entity.operlog.devicetype", "en-US", "登录设备_us", "登录设备（TaktConstants.DeviceType；如 unknown、pc、mobile、tablet）"),
            // entity.operlog.devicetype
            new TranslationSeedItem("entity.operlog.devicetype", "ja-JP", "登录设备_jp", "登录设备（TaktConstants.DeviceType；如 unknown、pc、mobile、tablet）"),
            // entity.operlog.devicetype
            new TranslationSeedItem("entity.operlog.devicetype", "zh-CN", "登录设备", "登录设备（TaktConstants.DeviceType；如 unknown、pc、mobile、tablet）"),
            // entity.operlog.devicetype
            new TranslationSeedItem("entity.operlog.devicetype", "zh-HK", "登录设备_hk", "登录设备（TaktConstants.DeviceType；如 unknown、pc、mobile、tablet）"),

            // entity.operlog.opertime
            new TranslationSeedItem("entity.operlog.opertime", "en-US", "操作时间_us", "操作时间（业务操作发生时刻）"),
            // entity.operlog.opertime
            new TranslationSeedItem("entity.operlog.opertime", "ja-JP", "操作时间_jp", "操作时间（业务操作发生时刻）"),
            // entity.operlog.opertime
            new TranslationSeedItem("entity.operlog.opertime", "zh-CN", "操作时间", "操作时间（业务操作发生时刻）"),
            // entity.operlog.opertime
            new TranslationSeedItem("entity.operlog.opertime", "zh-HK", "操作时间_hk", "操作时间（业务操作发生时刻）"),

            // entity.operlog.elapsedtime
            new TranslationSeedItem("entity.operlog.elapsedtime", "en-US", "执行耗时毫秒_us", "执行耗时（毫秒）"),
            // entity.operlog.elapsedtime
            new TranslationSeedItem("entity.operlog.elapsedtime", "ja-JP", "执行耗时毫秒_jp", "执行耗时（毫秒）"),
            // entity.operlog.elapsedtime
            new TranslationSeedItem("entity.operlog.elapsedtime", "zh-CN", "执行耗时毫秒", "执行耗时（毫秒）"),
            // entity.operlog.elapsedtime
            new TranslationSeedItem("entity.operlog.elapsedtime", "zh-HK", "执行耗时毫秒_hk", "执行耗时（毫秒）"),

            // entity.operlog.operstatus
            new TranslationSeedItem("entity.operlog.operstatus", "en-US", "操作状态_us", "操作状态（字典 sys_success_fail_status；0=失败 1=成功）"),
            // entity.operlog.operstatus
            new TranslationSeedItem("entity.operlog.operstatus", "ja-JP", "操作状态_jp", "操作状态（字典 sys_success_fail_status；0=失败 1=成功）"),
            // entity.operlog.operstatus
            new TranslationSeedItem("entity.operlog.operstatus", "zh-CN", "操作状态", "操作状态（字典 sys_success_fail_status；0=失败 1=成功）"),
            // entity.operlog.operstatus
            new TranslationSeedItem("entity.operlog.operstatus", "zh-HK", "操作状态_hk", "操作状态（字典 sys_success_fail_status；0=失败 1=成功）"),
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
