// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Code.Database
// 文件名称：TaktDataCloneI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDataClone 实体字段国际化种子（已对齐前端 locales：src/locales/code/database/data-clone）
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
/// TaktDataClone 实体国际化翻译种子（键前缀 entity.dataclone.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDataCloneI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktDataClone 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 dataclone 实体翻译...", tenantCode);

        foreach (var item in GetDataCloneTranslations())
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

        TaktLogger.Information("TaktDataClone 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDataClone 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.dataclone._self / entity.dataclone.{{field}}；ResourceGroup=Database；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDataCloneTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.dataclone._self
            new TranslationSeedItem("entity.dataclone._self", "en-US", "Data Clone Information_us", "实体名称"),
            // entity.dataclone._self
            new TranslationSeedItem("entity.dataclone._self", "ja-JP", "公司级数据克隆信息_jp", "实体名称"),
            // entity.dataclone._self
            new TranslationSeedItem("entity.dataclone._self", "zh-CN", "公司级数据克隆信息", "实体名称"),
            // entity.dataclone._self
            new TranslationSeedItem("entity.dataclone._self", "zh-HK", "公司级数据克隆信息_hk", "实体名称"),

            // entity.dataclone.sourcetenantcode
            new TranslationSeedItem("entity.dataclone.sourcetenantcode", "en-US", "源租户编码_us", "源租户编码（3 位）"),
            // entity.dataclone.sourcetenantcode
            new TranslationSeedItem("entity.dataclone.sourcetenantcode", "ja-JP", "源租户编码_jp", "源租户编码（3 位）"),
            // entity.dataclone.sourcetenantcode
            new TranslationSeedItem("entity.dataclone.sourcetenantcode", "zh-CN", "源租户编码", "源租户编码（3 位）"),
            // entity.dataclone.sourcetenantcode
            new TranslationSeedItem("entity.dataclone.sourcetenantcode", "zh-HK", "源租户编码_hk", "源租户编码（3 位）"),

            // entity.dataclone.sourcedatabasename
            new TranslationSeedItem("entity.dataclone.sourcedatabasename", "en-US", "源数据库_us", "源数据库展示名"),
            // entity.dataclone.sourcedatabasename
            new TranslationSeedItem("entity.dataclone.sourcedatabasename", "ja-JP", "源数据库_jp", "源数据库展示名"),
            // entity.dataclone.sourcedatabasename
            new TranslationSeedItem("entity.dataclone.sourcedatabasename", "zh-CN", "源数据库", "源数据库展示名"),
            // entity.dataclone.sourcedatabasename
            new TranslationSeedItem("entity.dataclone.sourcedatabasename", "zh-HK", "源数据库_hk", "源数据库展示名"),

            // entity.dataclone.sourcetablename
            new TranslationSeedItem("entity.dataclone.sourcetablename", "en-US", "源数据表_us", "源物理表名"),
            // entity.dataclone.sourcetablename
            new TranslationSeedItem("entity.dataclone.sourcetablename", "ja-JP", "源数据表_jp", "源物理表名"),
            // entity.dataclone.sourcetablename
            new TranslationSeedItem("entity.dataclone.sourcetablename", "zh-CN", "源数据表", "源物理表名"),
            // entity.dataclone.sourcetablename
            new TranslationSeedItem("entity.dataclone.sourcetablename", "zh-HK", "源数据表_hk", "源物理表名"),

            // entity.dataclone.sourcecompanycode
            new TranslationSeedItem("entity.dataclone.sourcecompanycode", "en-US", "源公司编码_us", "源公司编码（4 位）"),
            // entity.dataclone.sourcecompanycode
            new TranslationSeedItem("entity.dataclone.sourcecompanycode", "ja-JP", "源公司编码_jp", "源公司编码（4 位）"),
            // entity.dataclone.sourcecompanycode
            new TranslationSeedItem("entity.dataclone.sourcecompanycode", "zh-CN", "源公司编码", "源公司编码（4 位）"),
            // entity.dataclone.sourcecompanycode
            new TranslationSeedItem("entity.dataclone.sourcecompanycode", "zh-HK", "源公司编码_hk", "源公司编码（4 位）"),

            // entity.dataclone.targettenantcode
            new TranslationSeedItem("entity.dataclone.targettenantcode", "en-US", "目标租户_us", "目标租户（3 位）"),
            // entity.dataclone.targettenantcode
            new TranslationSeedItem("entity.dataclone.targettenantcode", "ja-JP", "目标租户_jp", "目标租户（3 位）"),
            // entity.dataclone.targettenantcode
            new TranslationSeedItem("entity.dataclone.targettenantcode", "zh-CN", "目标租户", "目标租户（3 位）"),
            // entity.dataclone.targettenantcode
            new TranslationSeedItem("entity.dataclone.targettenantcode", "zh-HK", "目标租户_hk", "目标租户（3 位）"),

            // entity.dataclone.targetdatabasename
            new TranslationSeedItem("entity.dataclone.targetdatabasename", "en-US", "目标数据库_us", "目标数据库展示名"),
            // entity.dataclone.targetdatabasename
            new TranslationSeedItem("entity.dataclone.targetdatabasename", "ja-JP", "目标数据库_jp", "目标数据库展示名"),
            // entity.dataclone.targetdatabasename
            new TranslationSeedItem("entity.dataclone.targetdatabasename", "zh-CN", "目标数据库", "目标数据库展示名"),
            // entity.dataclone.targetdatabasename
            new TranslationSeedItem("entity.dataclone.targetdatabasename", "zh-HK", "目标数据库_hk", "目标数据库展示名"),

            // entity.dataclone.targettablename
            new TranslationSeedItem("entity.dataclone.targettablename", "en-US", "目标数据表_us", "目标物理表名"),
            // entity.dataclone.targettablename
            new TranslationSeedItem("entity.dataclone.targettablename", "ja-JP", "目标数据表_jp", "目标物理表名"),
            // entity.dataclone.targettablename
            new TranslationSeedItem("entity.dataclone.targettablename", "zh-CN", "目标数据表", "目标物理表名"),
            // entity.dataclone.targettablename
            new TranslationSeedItem("entity.dataclone.targettablename", "zh-HK", "目标数据表_hk", "目标物理表名"),

            // entity.dataclone.targetcompanycode
            new TranslationSeedItem("entity.dataclone.targetcompanycode", "en-US", "目标公司编码_us", "目标公司编码（4 位）"),
            // entity.dataclone.targetcompanycode
            new TranslationSeedItem("entity.dataclone.targetcompanycode", "ja-JP", "目标公司编码_jp", "目标公司编码（4 位）"),
            // entity.dataclone.targetcompanycode
            new TranslationSeedItem("entity.dataclone.targetcompanycode", "zh-CN", "目标公司编码", "目标公司编码（4 位）"),
            // entity.dataclone.targetcompanycode
            new TranslationSeedItem("entity.dataclone.targetcompanycode", "zh-HK", "目标公司编码_hk", "目标公司编码（4 位）"),

            // entity.dataclone.backuptablename
            new TranslationSeedItem("entity.dataclone.backuptablename", "en-US", "目标备份表名_us", "目标备份表名（克隆前 SELECT INTO 生成的物理表）"),
            // entity.dataclone.backuptablename
            new TranslationSeedItem("entity.dataclone.backuptablename", "ja-JP", "目标备份表名_jp", "目标备份表名（克隆前 SELECT INTO 生成的物理表）"),
            // entity.dataclone.backuptablename
            new TranslationSeedItem("entity.dataclone.backuptablename", "zh-CN", "目标备份表名", "目标备份表名（克隆前 SELECT INTO 生成的物理表）"),
            // entity.dataclone.backuptablename
            new TranslationSeedItem("entity.dataclone.backuptablename", "zh-HK", "目标备份表名_hk", "目标备份表名（克隆前 SELECT INTO 生成的物理表）"),

            // entity.dataclone.targetbackupdatabasename
            new TranslationSeedItem("entity.dataclone.targetbackupdatabasename", "en-US", "目标备份所在库_us", "目标备份所在数据库展示名（备份位置）"),
            // entity.dataclone.targetbackupdatabasename
            new TranslationSeedItem("entity.dataclone.targetbackupdatabasename", "ja-JP", "目标备份所在库_jp", "目标备份所在数据库展示名（备份位置）"),
            // entity.dataclone.targetbackupdatabasename
            new TranslationSeedItem("entity.dataclone.targetbackupdatabasename", "zh-CN", "目标备份所在库", "目标备份所在数据库展示名（备份位置）"),
            // entity.dataclone.targetbackupdatabasename
            new TranslationSeedItem("entity.dataclone.targetbackupdatabasename", "zh-HK", "目标备份所在库_hk", "目标备份所在数据库展示名（备份位置）"),

            // entity.dataclone.backeduprowcount
            new TranslationSeedItem("entity.dataclone.backeduprowcount", "en-US", "目标备份行数_us", "目标备份行数"),
            // entity.dataclone.backeduprowcount
            new TranslationSeedItem("entity.dataclone.backeduprowcount", "ja-JP", "目标备份行数_jp", "目标备份行数"),
            // entity.dataclone.backeduprowcount
            new TranslationSeedItem("entity.dataclone.backeduprowcount", "zh-CN", "目标备份行数", "目标备份行数"),
            // entity.dataclone.backeduprowcount
            new TranslationSeedItem("entity.dataclone.backeduprowcount", "zh-HK", "目标备份行数_hk", "目标备份行数"),

            // entity.dataclone.clearedrowcount
            new TranslationSeedItem("entity.dataclone.clearedrowcount", "en-US", "目标清空行数_us", "目标清空行数（备份后删除/TRUNCATE 的行数）"),
            // entity.dataclone.clearedrowcount
            new TranslationSeedItem("entity.dataclone.clearedrowcount", "ja-JP", "目标清空行数_jp", "目标清空行数（备份后删除/TRUNCATE 的行数）"),
            // entity.dataclone.clearedrowcount
            new TranslationSeedItem("entity.dataclone.clearedrowcount", "zh-CN", "目标清空行数", "目标清空行数（备份后删除/TRUNCATE 的行数）"),
            // entity.dataclone.clearedrowcount
            new TranslationSeedItem("entity.dataclone.clearedrowcount", "zh-HK", "目标清空行数_hk", "目标清空行数（备份后删除/TRUNCATE 的行数）"),

            // entity.dataclone.sourcerowcount
            new TranslationSeedItem("entity.dataclone.sourcerowcount", "en-US", "源公司匹配行数_us", "源公司匹配行数"),
            // entity.dataclone.sourcerowcount
            new TranslationSeedItem("entity.dataclone.sourcerowcount", "ja-JP", "源公司匹配行数_jp", "源公司匹配行数"),
            // entity.dataclone.sourcerowcount
            new TranslationSeedItem("entity.dataclone.sourcerowcount", "zh-CN", "源公司匹配行数", "源公司匹配行数"),
            // entity.dataclone.sourcerowcount
            new TranslationSeedItem("entity.dataclone.sourcerowcount", "zh-HK", "源公司匹配行数_hk", "源公司匹配行数"),

            // entity.dataclone.clonedrowcount
            new TranslationSeedItem("entity.dataclone.clonedrowcount", "en-US", "克隆行数_us", "实际写入目标表行数"),
            // entity.dataclone.clonedrowcount
            new TranslationSeedItem("entity.dataclone.clonedrowcount", "ja-JP", "克隆行数_jp", "实际写入目标表行数"),
            // entity.dataclone.clonedrowcount
            new TranslationSeedItem("entity.dataclone.clonedrowcount", "zh-CN", "克隆行数", "实际写入目标表行数"),
            // entity.dataclone.clonedrowcount
            new TranslationSeedItem("entity.dataclone.clonedrowcount", "zh-HK", "克隆行数_hk", "实际写入目标表行数"),
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
