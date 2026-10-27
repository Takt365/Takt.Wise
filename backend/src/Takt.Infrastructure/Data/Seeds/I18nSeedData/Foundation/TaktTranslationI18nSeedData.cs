// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktTranslationI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTranslation 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation;

/// <summary>
/// TaktTranslation 实体国际化翻译种子（键前缀 entity.translation.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTranslationI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTranslation 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 translation 实体翻译...", tenantCode);

        foreach (var item in GetTranslationTranslations())
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

        TaktLogger.Information("TaktTranslation 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTranslation 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.translation._self / entity.translation.{{field}}；ResourceGroup=Foundation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTranslationTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.translation._self
            new TranslationSeedItem("entity.translation._self", "en-US", "Translation Information_us", "实体名称"),
            // entity.translation._self
            new TranslationSeedItem("entity.translation._self", "ja-JP", "翻译信息_jp", "实体名称"),
            // entity.translation._self
            new TranslationSeedItem("entity.translation._self", "zh-CN", "翻译信息", "实体名称"),
            // entity.translation._self
            new TranslationSeedItem("entity.translation._self", "zh-HK", "翻译信息_hk", "实体名称"),

            // entity.translation.cultureid
            new TranslationSeedItem("entity.translation.cultureid", "en-US", "文化ID_us", "区域文化（选项 TaktCultures/options；DictValue=Id）"),
            // entity.translation.cultureid
            new TranslationSeedItem("entity.translation.cultureid", "ja-JP", "文化ID_jp", "区域文化（选项 TaktCultures/options；DictValue=Id）"),
            // entity.translation.cultureid
            new TranslationSeedItem("entity.translation.cultureid", "zh-CN", "文化ID", "区域文化（选项 TaktCultures/options；DictValue=Id）"),
            // entity.translation.cultureid
            new TranslationSeedItem("entity.translation.cultureid", "zh-HK", "文化ID_hk", "区域文化（选项 TaktCultures/options；DictValue=Id）"),

            // entity.translation.culturecode
            new TranslationSeedItem("entity.translation.culturecode", "en-US", "区域文化_us", "区域文化编码（选项 TaktCultures/options，DictValue=CultureCode）"),
            // entity.translation.culturecode
            new TranslationSeedItem("entity.translation.culturecode", "ja-JP", "区域文化_jp", "区域文化编码（选项 TaktCultures/options，DictValue=CultureCode）"),
            // entity.translation.culturecode
            new TranslationSeedItem("entity.translation.culturecode", "zh-CN", "区域文化", "区域文化编码（选项 TaktCultures/options，DictValue=CultureCode）"),
            // entity.translation.culturecode
            new TranslationSeedItem("entity.translation.culturecode", "zh-HK", "区域文化_hk", "区域文化编码（选项 TaktCultures/options，DictValue=CultureCode）"),

            // entity.translation.i18nkey
            new TranslationSeedItem("entity.translation.i18nkey", "en-US", "翻译键_us", "翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；varchar Length=140；如 common.confirm）"),
            // entity.translation.i18nkey
            new TranslationSeedItem("entity.translation.i18nkey", "ja-JP", "翻译键_jp", "翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；varchar Length=140；如 common.confirm）"),
            // entity.translation.i18nkey
            new TranslationSeedItem("entity.translation.i18nkey", "zh-CN", "翻译键", "翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；varchar Length=140；如 common.confirm）"),
            // entity.translation.i18nkey
            new TranslationSeedItem("entity.translation.i18nkey", "zh-HK", "翻译键_hk", "翻译键（唯一索引：租户内键+文化唯一，见 ix_translation_key_culture_unique；varchar Length=140；如 common.confirm）"),

            // entity.translation.text
            new TranslationSeedItem("entity.translation.text", "en-US", "翻译文本_us", "翻译文本（该语言下的显示文本）"),
            // entity.translation.text
            new TranslationSeedItem("entity.translation.text", "ja-JP", "翻译文本_jp", "翻译文本（该语言下的显示文本）"),
            // entity.translation.text
            new TranslationSeedItem("entity.translation.text", "zh-CN", "翻译文本", "翻译文本（该语言下的显示文本）"),
            // entity.translation.text
            new TranslationSeedItem("entity.translation.text", "zh-HK", "翻译文本_hk", "翻译文本（该语言下的显示文本）"),

            // entity.translation.resourcegroup
            new TranslationSeedItem("entity.translation.resourcegroup", "en-US", "资源分组_us", "资源分组（选项 TaktMenus/tree-options,DictValue=Id）"),
            // entity.translation.resourcegroup
            new TranslationSeedItem("entity.translation.resourcegroup", "ja-JP", "资源分组_jp", "资源分组（选项 TaktMenus/tree-options,DictValue=Id）"),
            // entity.translation.resourcegroup
            new TranslationSeedItem("entity.translation.resourcegroup", "zh-CN", "资源分组", "资源分组（选项 TaktMenus/tree-options,DictValue=Id）"),
            // entity.translation.resourcegroup
            new TranslationSeedItem("entity.translation.resourcegroup", "zh-HK", "资源分组_hk", "资源分组（选项 TaktMenus/tree-options,DictValue=Id）"),

            // entity.translation.resourcetype
            new TranslationSeedItem("entity.translation.resourcetype", "en-US", "资源类别_us", "资源类别（字典 sys_resource_type；frontend=前端 backend=后端）"),
            // entity.translation.resourcetype
            new TranslationSeedItem("entity.translation.resourcetype", "ja-JP", "资源类别_jp", "资源类别（字典 sys_resource_type；frontend=前端 backend=后端）"),
            // entity.translation.resourcetype
            new TranslationSeedItem("entity.translation.resourcetype", "zh-CN", "资源类别", "资源类别（字典 sys_resource_type；frontend=前端 backend=后端）"),
            // entity.translation.resourcetype
            new TranslationSeedItem("entity.translation.resourcetype", "zh-HK", "资源类别_hk", "资源类别（字典 sys_resource_type；frontend=前端 backend=后端）"),

            // entity.translation.contextnote
            new TranslationSeedItem("entity.translation.contextnote", "en-US", "上下文注释_us", "上下文注释（帮助翻译人员理解使用场景）"),
            // entity.translation.contextnote
            new TranslationSeedItem("entity.translation.contextnote", "ja-JP", "上下文注释_jp", "上下文注释（帮助翻译人员理解使用场景）"),
            // entity.translation.contextnote
            new TranslationSeedItem("entity.translation.contextnote", "zh-CN", "上下文注释", "上下文注释（帮助翻译人员理解使用场景）"),
            // entity.translation.contextnote
            new TranslationSeedItem("entity.translation.contextnote", "zh-HK", "上下文注释_hk", "上下文注释（帮助翻译人员理解使用场景）"),

            // entity.translation.culture
            new TranslationSeedItem("entity.translation.culture", "en-US", "文化_us", "文化（多对一关联）"),
            // entity.translation.culture
            new TranslationSeedItem("entity.translation.culture", "ja-JP", "文化_jp", "文化（多对一关联）"),
            // entity.translation.culture
            new TranslationSeedItem("entity.translation.culture", "zh-CN", "文化", "文化（多对一关联）"),
            // entity.translation.culture
            new TranslationSeedItem("entity.translation.culture", "zh-HK", "文化_hk", "文化（多对一关联）"),
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
        translation.ResourceGroup = "Foundation";
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
