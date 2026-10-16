// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepCheckItemI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopStepCheckItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop;

/// <summary>
/// TaktSopStepCheckItem 实体国际化翻译种子（键前缀 entity.sopstepcheckitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopStepCheckItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopStepCheckItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopstepcheckitem 实体翻译...", tenantCode);

        foreach (var item in GetSopStepCheckItemTranslations())
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

        TaktLogger.Information("TaktSopStepCheckItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopStepCheckItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopstepcheckitem._self / entity.sopstepcheckitem.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopStepCheckItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopstepcheckitem._self
            new TranslationSeedItem("entity.sopstepcheckitem._self", "en-US", "Sop Step Check Item Information_us", "实体名称"),
            // entity.sopstepcheckitem._self
            new TranslationSeedItem("entity.sopstepcheckitem._self", "ja-JP", "SOP 工步检验项目信息_jp", "实体名称"),
            // entity.sopstepcheckitem._self
            new TranslationSeedItem("entity.sopstepcheckitem._self", "zh-CN", "SOP 工步检验项目信息", "实体名称"),
            // entity.sopstepcheckitem._self
            new TranslationSeedItem("entity.sopstepcheckitem._self", "zh-HK", "SOP 工步检验项目信息_hk", "实体名称"),

            // entity.sopstepcheckitem.stepid
            new TranslationSeedItem("entity.sopstepcheckitem.stepid", "en-US", "工步ID_us", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopstepcheckitem.stepid
            new TranslationSeedItem("entity.sopstepcheckitem.stepid", "ja-JP", "工步ID_jp", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopstepcheckitem.stepid
            new TranslationSeedItem("entity.sopstepcheckitem.stepid", "zh-CN", "工步ID", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopstepcheckitem.stepid
            new TranslationSeedItem("entity.sopstepcheckitem.stepid", "zh-HK", "工步ID_hk", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),

            // entity.sopstepcheckitem.checkitemname
            new TranslationSeedItem("entity.sopstepcheckitem.checkitemname", "en-US", "检验项目名称_us", "检验项目名称"),
            // entity.sopstepcheckitem.checkitemname
            new TranslationSeedItem("entity.sopstepcheckitem.checkitemname", "ja-JP", "检验项目名称_jp", "检验项目名称"),
            // entity.sopstepcheckitem.checkitemname
            new TranslationSeedItem("entity.sopstepcheckitem.checkitemname", "zh-CN", "检验项目名称", "检验项目名称"),
            // entity.sopstepcheckitem.checkitemname
            new TranslationSeedItem("entity.sopstepcheckitem.checkitemname", "zh-HK", "检验项目名称_hk", "检验项目名称"),

            // entity.sopstepcheckitem.checkmethod
            new TranslationSeedItem("entity.sopstepcheckitem.checkmethod", "en-US", "检验方法_us", "检验方法"),
            // entity.sopstepcheckitem.checkmethod
            new TranslationSeedItem("entity.sopstepcheckitem.checkmethod", "ja-JP", "检验方法_jp", "检验方法"),
            // entity.sopstepcheckitem.checkmethod
            new TranslationSeedItem("entity.sopstepcheckitem.checkmethod", "zh-CN", "检验方法", "检验方法"),
            // entity.sopstepcheckitem.checkmethod
            new TranslationSeedItem("entity.sopstepcheckitem.checkmethod", "zh-HK", "检验方法_hk", "检验方法"),

            // entity.sopstepcheckitem.checkstandard
            new TranslationSeedItem("entity.sopstepcheckitem.checkstandard", "en-US", "检验标准_us", "检验标准"),
            // entity.sopstepcheckitem.checkstandard
            new TranslationSeedItem("entity.sopstepcheckitem.checkstandard", "ja-JP", "检验标准_jp", "检验标准"),
            // entity.sopstepcheckitem.checkstandard
            new TranslationSeedItem("entity.sopstepcheckitem.checkstandard", "zh-CN", "检验标准", "检验标准"),
            // entity.sopstepcheckitem.checkstandard
            new TranslationSeedItem("entity.sopstepcheckitem.checkstandard", "zh-HK", "检验标准_hk", "检验标准"),

            // entity.sopstepcheckitem.isrequired
            new TranslationSeedItem("entity.sopstepcheckitem.isrequired", "en-US", "是否必检_us", "是否必检（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopstepcheckitem.isrequired
            new TranslationSeedItem("entity.sopstepcheckitem.isrequired", "ja-JP", "是否必检_jp", "是否必检（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopstepcheckitem.isrequired
            new TranslationSeedItem("entity.sopstepcheckitem.isrequired", "zh-CN", "是否必检", "是否必检（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopstepcheckitem.isrequired
            new TranslationSeedItem("entity.sopstepcheckitem.isrequired", "zh-HK", "是否必检_hk", "是否必检（字典 sys_yes_no；0=否，1=是）"),

            // entity.sopstepcheckitem.sortorder
            new TranslationSeedItem("entity.sopstepcheckitem.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.sopstepcheckitem.sortorder
            new TranslationSeedItem("entity.sopstepcheckitem.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.sopstepcheckitem.sortorder
            new TranslationSeedItem("entity.sopstepcheckitem.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.sopstepcheckitem.sortorder
            new TranslationSeedItem("entity.sopstepcheckitem.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.sopstepcheckitem.step
            new TranslationSeedItem("entity.sopstepcheckitem.step", "en-US", "工步_us", "工步"),
            // entity.sopstepcheckitem.step
            new TranslationSeedItem("entity.sopstepcheckitem.step", "ja-JP", "工步_jp", "工步"),
            // entity.sopstepcheckitem.step
            new TranslationSeedItem("entity.sopstepcheckitem.step", "zh-CN", "工步", "工步"),
            // entity.sopstepcheckitem.step
            new TranslationSeedItem("entity.sopstepcheckitem.step", "zh-HK", "工步_hk", "工步"),
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
        translation.ResourceGroup = "Sop";
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
