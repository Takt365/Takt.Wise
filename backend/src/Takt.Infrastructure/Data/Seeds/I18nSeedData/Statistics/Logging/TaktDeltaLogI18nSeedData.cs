// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktDeltaLogI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDeltaLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktDeltaLog 实体国际化翻译种子（键前缀 entity.deltalog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDeltaLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktDeltaLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 deltalog 实体翻译...", tenantCode);

        foreach (var item in GetDeltaLogTranslations())
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

        TaktLogger.Information("TaktDeltaLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDeltaLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.deltalog._self / entity.deltalog.{{field}}；ResourceGroup=Logging；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDeltaLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.deltalog._self
            new TranslationSeedItem("entity.deltalog._self", "en-US", "Delta Log Information_us", "实体名称"),
            // entity.deltalog._self
            new TranslationSeedItem("entity.deltalog._self", "ja-JP", "差异日志信息_jp", "实体名称"),
            // entity.deltalog._self
            new TranslationSeedItem("entity.deltalog._self", "zh-CN", "差异日志信息", "实体名称"),
            // entity.deltalog._self
            new TranslationSeedItem("entity.deltalog._self", "zh-HK", "差异日志信息_hk", "实体名称"),

            // entity.deltalog.username
            new TranslationSeedItem("entity.deltalog.username", "en-US", "用户名_us", "用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）"),
            // entity.deltalog.username
            new TranslationSeedItem("entity.deltalog.username", "ja-JP", "用户名_jp", "用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）"),
            // entity.deltalog.username
            new TranslationSeedItem("entity.deltalog.username", "zh-CN", "用户名", "用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）"),
            // entity.deltalog.username
            new TranslationSeedItem("entity.deltalog.username", "zh-HK", "用户名_hk", "用户名（登录账号；无法解析时为 TaktConstants.AuditUserName.Unknown）"),

            // entity.deltalog.opertype
            new TranslationSeedItem("entity.deltalog.opertype", "en-US", "操作类型_us", "操作类型（TaktConstants.OperType；如 create=新增、update=修改、delete=删除、query=查询）"),
            // entity.deltalog.opertype
            new TranslationSeedItem("entity.deltalog.opertype", "ja-JP", "操作类型_jp", "操作类型（TaktConstants.OperType；如 create=新增、update=修改、delete=删除、query=查询）"),
            // entity.deltalog.opertype
            new TranslationSeedItem("entity.deltalog.opertype", "zh-CN", "操作类型", "操作类型（TaktConstants.OperType；如 create=新增、update=修改、delete=删除、query=查询）"),
            // entity.deltalog.opertype
            new TranslationSeedItem("entity.deltalog.opertype", "zh-HK", "操作类型_hk", "操作类型（TaktConstants.OperType；如 create=新增、update=修改、delete=删除、query=查询）"),

            // entity.deltalog.tablename
            new TranslationSeedItem("entity.deltalog.tablename", "en-US", "表名_us", "数据库表名（SugarTable 物理表名）"),
            // entity.deltalog.tablename
            new TranslationSeedItem("entity.deltalog.tablename", "ja-JP", "表名_jp", "数据库表名（SugarTable 物理表名）"),
            // entity.deltalog.tablename
            new TranslationSeedItem("entity.deltalog.tablename", "zh-CN", "表名", "数据库表名（SugarTable 物理表名）"),
            // entity.deltalog.tablename
            new TranslationSeedItem("entity.deltalog.tablename", "zh-HK", "表名_hk", "数据库表名（SugarTable 物理表名）"),

            // entity.deltalog.primarykeyid
            new TranslationSeedItem("entity.deltalog.primarykeyid", "en-US", "主键ID_us", "业务主键 ID"),
            // entity.deltalog.primarykeyid
            new TranslationSeedItem("entity.deltalog.primarykeyid", "ja-JP", "主键ID_jp", "业务主键 ID"),
            // entity.deltalog.primarykeyid
            new TranslationSeedItem("entity.deltalog.primarykeyid", "zh-CN", "主键ID", "业务主键 ID"),
            // entity.deltalog.primarykeyid
            new TranslationSeedItem("entity.deltalog.primarykeyid", "zh-HK", "主键ID_hk", "业务主键 ID"),

            // entity.deltalog.beforedata
            new TranslationSeedItem("entity.deltalog.beforedata", "en-US", "修改前数据_us", "修改前数据 JSON（旧值快照；无数据为空串）"),
            // entity.deltalog.beforedata
            new TranslationSeedItem("entity.deltalog.beforedata", "ja-JP", "修改前数据_jp", "修改前数据 JSON（旧值快照；无数据为空串）"),
            // entity.deltalog.beforedata
            new TranslationSeedItem("entity.deltalog.beforedata", "zh-CN", "修改前数据", "修改前数据 JSON（旧值快照；无数据为空串）"),
            // entity.deltalog.beforedata
            new TranslationSeedItem("entity.deltalog.beforedata", "zh-HK", "修改前数据_hk", "修改前数据 JSON（旧值快照；无数据为空串）"),

            // entity.deltalog.afterdata
            new TranslationSeedItem("entity.deltalog.afterdata", "en-US", "修改后数据_us", "修改后数据 JSON（新值快照；无数据为空串）"),
            // entity.deltalog.afterdata
            new TranslationSeedItem("entity.deltalog.afterdata", "ja-JP", "修改后数据_jp", "修改后数据 JSON（新值快照；无数据为空串）"),
            // entity.deltalog.afterdata
            new TranslationSeedItem("entity.deltalog.afterdata", "zh-CN", "修改后数据", "修改后数据 JSON（新值快照；无数据为空串）"),
            // entity.deltalog.afterdata
            new TranslationSeedItem("entity.deltalog.afterdata", "zh-HK", "修改后数据_hk", "修改后数据 JSON（新值快照；无数据为空串）"),

            // entity.deltalog.diffdata
            new TranslationSeedItem("entity.deltalog.diffdata", "en-US", "差异内容_us", "差异内容 JSON（变更字段及旧/新值明细；无差异为空串）"),
            // entity.deltalog.diffdata
            new TranslationSeedItem("entity.deltalog.diffdata", "ja-JP", "差异内容_jp", "差异内容 JSON（变更字段及旧/新值明细；无差异为空串）"),
            // entity.deltalog.diffdata
            new TranslationSeedItem("entity.deltalog.diffdata", "zh-CN", "差异内容", "差异内容 JSON（变更字段及旧/新值明细；无差异为空串）"),
            // entity.deltalog.diffdata
            new TranslationSeedItem("entity.deltalog.diffdata", "zh-HK", "差异内容_hk", "差异内容 JSON（变更字段及旧/新值明细；无差异为空串）"),

            // entity.deltalog.sqlstatement
            new TranslationSeedItem("entity.deltalog.sqlstatement", "en-US", "SQL语句_us", "执行的 SQL 语句（AOP 捕获；无 SQL 为空串）"),
            // entity.deltalog.sqlstatement
            new TranslationSeedItem("entity.deltalog.sqlstatement", "ja-JP", "SQL语句_jp", "执行的 SQL 语句（AOP 捕获；无 SQL 为空串）"),
            // entity.deltalog.sqlstatement
            new TranslationSeedItem("entity.deltalog.sqlstatement", "zh-CN", "SQL语句", "执行的 SQL 语句（AOP 捕获；无 SQL 为空串）"),
            // entity.deltalog.sqlstatement
            new TranslationSeedItem("entity.deltalog.sqlstatement", "zh-HK", "SQL语句_hk", "执行的 SQL 语句（AOP 捕获；无 SQL 为空串）"),

            // entity.deltalog.operip
            new TranslationSeedItem("entity.deltalog.operip", "en-US", "操作IP_us", "操作 IP"),
            // entity.deltalog.operip
            new TranslationSeedItem("entity.deltalog.operip", "ja-JP", "操作IP_jp", "操作 IP"),
            // entity.deltalog.operip
            new TranslationSeedItem("entity.deltalog.operip", "zh-CN", "操作IP", "操作 IP"),
            // entity.deltalog.operip
            new TranslationSeedItem("entity.deltalog.operip", "zh-HK", "操作IP_hk", "操作 IP"),

            // entity.deltalog.operlocation
            new TranslationSeedItem("entity.deltalog.operlocation", "en-US", "操作地点_us", "操作地点（由 OperIp 解析，如：中国-广东省-深圳市）"),
            // entity.deltalog.operlocation
            new TranslationSeedItem("entity.deltalog.operlocation", "ja-JP", "操作地点_jp", "操作地点（由 OperIp 解析，如：中国-广东省-深圳市）"),
            // entity.deltalog.operlocation
            new TranslationSeedItem("entity.deltalog.operlocation", "zh-CN", "操作地点", "操作地点（由 OperIp 解析，如：中国-广东省-深圳市）"),
            // entity.deltalog.operlocation
            new TranslationSeedItem("entity.deltalog.operlocation", "zh-HK", "操作地点_hk", "操作地点（由 OperIp 解析，如：中国-广东省-深圳市）"),

            // entity.deltalog.useragent
            new TranslationSeedItem("entity.deltalog.useragent", "en-US", "用户代理_us", "用户代理（User-Agent）"),
            // entity.deltalog.useragent
            new TranslationSeedItem("entity.deltalog.useragent", "ja-JP", "用户代理_jp", "用户代理（User-Agent）"),
            // entity.deltalog.useragent
            new TranslationSeedItem("entity.deltalog.useragent", "zh-CN", "用户代理", "用户代理（User-Agent）"),
            // entity.deltalog.useragent
            new TranslationSeedItem("entity.deltalog.useragent", "zh-HK", "用户代理_hk", "用户代理（User-Agent）"),

            // entity.deltalog.browser
            new TranslationSeedItem("entity.deltalog.browser", "en-US", "浏览器_us", "浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）"),
            // entity.deltalog.browser
            new TranslationSeedItem("entity.deltalog.browser", "ja-JP", "浏览器_jp", "浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）"),
            // entity.deltalog.browser
            new TranslationSeedItem("entity.deltalog.browser", "zh-CN", "浏览器", "浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）"),
            // entity.deltalog.browser
            new TranslationSeedItem("entity.deltalog.browser", "zh-HK", "浏览器_hk", "浏览器（TaktConstants.BrowserType；如 unknown、chrome、firefox、safari、edge）"),

            // entity.deltalog.os
            new TranslationSeedItem("entity.deltalog.os", "en-US", "操作系统_us", "操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）"),
            // entity.deltalog.os
            new TranslationSeedItem("entity.deltalog.os", "ja-JP", "操作系统_jp", "操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）"),
            // entity.deltalog.os
            new TranslationSeedItem("entity.deltalog.os", "zh-CN", "操作系统", "操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）"),
            // entity.deltalog.os
            new TranslationSeedItem("entity.deltalog.os", "zh-HK", "操作系统_hk", "操作系统（TaktConstants.OperatingSystem；如 unknown、windows、macos、linux、android、ios）"),

            // entity.deltalog.devicetype
            new TranslationSeedItem("entity.deltalog.devicetype", "en-US", "登录设备_us", "登录设备（TaktConstants.DeviceType；如 unknown、pc、mobile、tablet）"),
            // entity.deltalog.devicetype
            new TranslationSeedItem("entity.deltalog.devicetype", "ja-JP", "登录设备_jp", "登录设备（TaktConstants.DeviceType；如 unknown、pc、mobile、tablet）"),
            // entity.deltalog.devicetype
            new TranslationSeedItem("entity.deltalog.devicetype", "zh-CN", "登录设备", "登录设备（TaktConstants.DeviceType；如 unknown、pc、mobile、tablet）"),
            // entity.deltalog.devicetype
            new TranslationSeedItem("entity.deltalog.devicetype", "zh-HK", "登录设备_hk", "登录设备（TaktConstants.DeviceType；如 unknown、pc、mobile、tablet）"),

            // entity.deltalog.opertime
            new TranslationSeedItem("entity.deltalog.opertime", "en-US", "操作时间_us", "操作时间（数据变更发生时刻）"),
            // entity.deltalog.opertime
            new TranslationSeedItem("entity.deltalog.opertime", "ja-JP", "操作时间_jp", "操作时间（数据变更发生时刻）"),
            // entity.deltalog.opertime
            new TranslationSeedItem("entity.deltalog.opertime", "zh-CN", "操作时间", "操作时间（数据变更发生时刻）"),
            // entity.deltalog.opertime
            new TranslationSeedItem("entity.deltalog.opertime", "zh-HK", "操作时间_hk", "操作时间（数据变更发生时刻）"),

            // entity.deltalog.elapsedtime
            new TranslationSeedItem("entity.deltalog.elapsedtime", "en-US", "执行耗时毫秒_us", "执行耗时（毫秒）"),
            // entity.deltalog.elapsedtime
            new TranslationSeedItem("entity.deltalog.elapsedtime", "ja-JP", "执行耗时毫秒_jp", "执行耗时（毫秒）"),
            // entity.deltalog.elapsedtime
            new TranslationSeedItem("entity.deltalog.elapsedtime", "zh-CN", "执行耗时毫秒", "执行耗时（毫秒）"),
            // entity.deltalog.elapsedtime
            new TranslationSeedItem("entity.deltalog.elapsedtime", "zh-HK", "执行耗时毫秒_hk", "执行耗时（毫秒）"),
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
