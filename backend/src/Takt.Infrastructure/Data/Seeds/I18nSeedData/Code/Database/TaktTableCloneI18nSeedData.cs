// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Code.Database
// 文件名称：TaktTableCloneI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTableClone 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Code.Database;

/// <summary>
/// TaktTableClone 实体国际化翻译种子（键前缀 entity.tableclone.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTableCloneI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTableClone 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 tableclone 实体翻译...", tenantCode);

        foreach (var item in GetTableCloneTranslations())
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

        TaktLogger.Information("TaktTableClone 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTableClone 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.tableclone._self / entity.tableclone.{{field}}；ResourceGroup=Database；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTableCloneTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.tableclone._self
            new TranslationSeedItem("entity.tableclone._self", "en-US", "Table Clone Information_us", "实体名称"),
            // entity.tableclone._self
            new TranslationSeedItem("entity.tableclone._self", "ja-JP", "整表克隆信息_jp", "实体名称"),
            // entity.tableclone._self
            new TranslationSeedItem("entity.tableclone._self", "zh-CN", "整表克隆信息", "实体名称"),
            // entity.tableclone._self
            new TranslationSeedItem("entity.tableclone._self", "zh-HK", "整表克隆信息_hk", "实体名称"),

            // entity.tableclone.sourcetenantcode
            new TranslationSeedItem("entity.tableclone.sourcetenantcode", "en-US", "源租户编码_us", "源租户编码（3 位）"),
            // entity.tableclone.sourcetenantcode
            new TranslationSeedItem("entity.tableclone.sourcetenantcode", "ja-JP", "源租户编码_jp", "源租户编码（3 位）"),
            // entity.tableclone.sourcetenantcode
            new TranslationSeedItem("entity.tableclone.sourcetenantcode", "zh-CN", "源租户编码", "源租户编码（3 位）"),
            // entity.tableclone.sourcetenantcode
            new TranslationSeedItem("entity.tableclone.sourcetenantcode", "zh-HK", "源租户编码_hk", "源租户编码（3 位）"),

            // entity.tableclone.sourcedatabasename
            new TranslationSeedItem("entity.tableclone.sourcedatabasename", "en-US", "源数据库_us", "源数据库展示名"),
            // entity.tableclone.sourcedatabasename
            new TranslationSeedItem("entity.tableclone.sourcedatabasename", "ja-JP", "源数据库_jp", "源数据库展示名"),
            // entity.tableclone.sourcedatabasename
            new TranslationSeedItem("entity.tableclone.sourcedatabasename", "zh-CN", "源数据库", "源数据库展示名"),
            // entity.tableclone.sourcedatabasename
            new TranslationSeedItem("entity.tableclone.sourcedatabasename", "zh-HK", "源数据库_hk", "源数据库展示名"),

            // entity.tableclone.sourcetablename
            new TranslationSeedItem("entity.tableclone.sourcetablename", "en-US", "源数据表_us", "源物理表名"),
            // entity.tableclone.sourcetablename
            new TranslationSeedItem("entity.tableclone.sourcetablename", "ja-JP", "源数据表_jp", "源物理表名"),
            // entity.tableclone.sourcetablename
            new TranslationSeedItem("entity.tableclone.sourcetablename", "zh-CN", "源数据表", "源物理表名"),
            // entity.tableclone.sourcetablename
            new TranslationSeedItem("entity.tableclone.sourcetablename", "zh-HK", "源数据表_hk", "源物理表名"),

            // entity.tableclone.targettenantcode
            new TranslationSeedItem("entity.tableclone.targettenantcode", "en-US", "目标租户_us", "目标租户（3 位）"),
            // entity.tableclone.targettenantcode
            new TranslationSeedItem("entity.tableclone.targettenantcode", "ja-JP", "目标租户_jp", "目标租户（3 位）"),
            // entity.tableclone.targettenantcode
            new TranslationSeedItem("entity.tableclone.targettenantcode", "zh-CN", "目标租户", "目标租户（3 位）"),
            // entity.tableclone.targettenantcode
            new TranslationSeedItem("entity.tableclone.targettenantcode", "zh-HK", "目标租户_hk", "目标租户（3 位）"),

            // entity.tableclone.targetdatabasename
            new TranslationSeedItem("entity.tableclone.targetdatabasename", "en-US", "目标数据库_us", "目标数据库展示名"),
            // entity.tableclone.targetdatabasename
            new TranslationSeedItem("entity.tableclone.targetdatabasename", "ja-JP", "目标数据库_jp", "目标数据库展示名"),
            // entity.tableclone.targetdatabasename
            new TranslationSeedItem("entity.tableclone.targetdatabasename", "zh-CN", "目标数据库", "目标数据库展示名"),
            // entity.tableclone.targetdatabasename
            new TranslationSeedItem("entity.tableclone.targetdatabasename", "zh-HK", "目标数据库_hk", "目标数据库展示名"),

            // entity.tableclone.targettablename
            new TranslationSeedItem("entity.tableclone.targettablename", "en-US", "目标数据表_us", "目标物理表名"),
            // entity.tableclone.targettablename
            new TranslationSeedItem("entity.tableclone.targettablename", "ja-JP", "目标数据表_jp", "目标物理表名"),
            // entity.tableclone.targettablename
            new TranslationSeedItem("entity.tableclone.targettablename", "zh-CN", "目标数据表", "目标物理表名"),
            // entity.tableclone.targettablename
            new TranslationSeedItem("entity.tableclone.targettablename", "zh-HK", "目标数据表_hk", "目标物理表名"),

            // entity.tableclone.backuptablename
            new TranslationSeedItem("entity.tableclone.backuptablename", "en-US", "目标备份表名_us", "目标备份表名（克隆前 SELECT INTO 生成的物理表）"),
            // entity.tableclone.backuptablename
            new TranslationSeedItem("entity.tableclone.backuptablename", "ja-JP", "目标备份表名_jp", "目标备份表名（克隆前 SELECT INTO 生成的物理表）"),
            // entity.tableclone.backuptablename
            new TranslationSeedItem("entity.tableclone.backuptablename", "zh-CN", "目标备份表名", "目标备份表名（克隆前 SELECT INTO 生成的物理表）"),
            // entity.tableclone.backuptablename
            new TranslationSeedItem("entity.tableclone.backuptablename", "zh-HK", "目标备份表名_hk", "目标备份表名（克隆前 SELECT INTO 生成的物理表）"),

            // entity.tableclone.targetbackupdatabasename
            new TranslationSeedItem("entity.tableclone.targetbackupdatabasename", "en-US", "目标备份所在库_us", "目标备份所在数据库展示名（备份位置）"),
            // entity.tableclone.targetbackupdatabasename
            new TranslationSeedItem("entity.tableclone.targetbackupdatabasename", "ja-JP", "目标备份所在库_jp", "目标备份所在数据库展示名（备份位置）"),
            // entity.tableclone.targetbackupdatabasename
            new TranslationSeedItem("entity.tableclone.targetbackupdatabasename", "zh-CN", "目标备份所在库", "目标备份所在数据库展示名（备份位置）"),
            // entity.tableclone.targetbackupdatabasename
            new TranslationSeedItem("entity.tableclone.targetbackupdatabasename", "zh-HK", "目标备份所在库_hk", "目标备份所在数据库展示名（备份位置）"),

            // entity.tableclone.backeduprowcount
            new TranslationSeedItem("entity.tableclone.backeduprowcount", "en-US", "目标备份行数_us", "目标备份行数"),
            // entity.tableclone.backeduprowcount
            new TranslationSeedItem("entity.tableclone.backeduprowcount", "ja-JP", "目标备份行数_jp", "目标备份行数"),
            // entity.tableclone.backeduprowcount
            new TranslationSeedItem("entity.tableclone.backeduprowcount", "zh-CN", "目标备份行数", "目标备份行数"),
            // entity.tableclone.backeduprowcount
            new TranslationSeedItem("entity.tableclone.backeduprowcount", "zh-HK", "目标备份行数_hk", "目标备份行数"),

            // entity.tableclone.clearedrowcount
            new TranslationSeedItem("entity.tableclone.clearedrowcount", "en-US", "目标清空行数_us", "目标清空行数（备份后 TRUNCATE 的行数）"),
            // entity.tableclone.clearedrowcount
            new TranslationSeedItem("entity.tableclone.clearedrowcount", "ja-JP", "目标清空行数_jp", "目标清空行数（备份后 TRUNCATE 的行数）"),
            // entity.tableclone.clearedrowcount
            new TranslationSeedItem("entity.tableclone.clearedrowcount", "zh-CN", "目标清空行数", "目标清空行数（备份后 TRUNCATE 的行数）"),
            // entity.tableclone.clearedrowcount
            new TranslationSeedItem("entity.tableclone.clearedrowcount", "zh-HK", "目标清空行数_hk", "目标清空行数（备份后 TRUNCATE 的行数）"),

            // entity.tableclone.sourcerowcount
            new TranslationSeedItem("entity.tableclone.sourcerowcount", "en-US", "源表行数_us", "源表行数"),
            // entity.tableclone.sourcerowcount
            new TranslationSeedItem("entity.tableclone.sourcerowcount", "ja-JP", "源表行数_jp", "源表行数"),
            // entity.tableclone.sourcerowcount
            new TranslationSeedItem("entity.tableclone.sourcerowcount", "zh-CN", "源表行数", "源表行数"),
            // entity.tableclone.sourcerowcount
            new TranslationSeedItem("entity.tableclone.sourcerowcount", "zh-HK", "源表行数_hk", "源表行数"),

            // entity.tableclone.clonedrowcount
            new TranslationSeedItem("entity.tableclone.clonedrowcount", "en-US", "克隆行数_us", "实际写入目标表行数"),
            // entity.tableclone.clonedrowcount
            new TranslationSeedItem("entity.tableclone.clonedrowcount", "ja-JP", "克隆行数_jp", "实际写入目标表行数"),
            // entity.tableclone.clonedrowcount
            new TranslationSeedItem("entity.tableclone.clonedrowcount", "zh-CN", "克隆行数", "实际写入目标表行数"),
            // entity.tableclone.clonedrowcount
            new TranslationSeedItem("entity.tableclone.clonedrowcount", "zh-HK", "克隆行数_hk", "实际写入目标表行数"),
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
        translation.ResourceGroup = "Database";
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
