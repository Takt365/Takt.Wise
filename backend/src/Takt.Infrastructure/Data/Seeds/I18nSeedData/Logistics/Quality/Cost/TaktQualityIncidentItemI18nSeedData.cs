// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityIncidentItemI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityIncidentItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost;

/// <summary>
/// TaktQualityIncidentItem 实体国际化翻译种子（键前缀 entity.qualityincidentitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityIncidentItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityIncidentItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityincidentitem 实体翻译...", tenantCode);

        foreach (var item in GetQualityIncidentItemTranslations())
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

        TaktLogger.Information("TaktQualityIncidentItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityIncidentItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityincidentitem._self / entity.qualityincidentitem.{{field}}；ResourceGroup=Cost；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityIncidentItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityincidentitem._self
            new TranslationSeedItem("entity.qualityincidentitem._self", "en-US", "Quality Incident Item Information_us", "实体名称"),
            // entity.qualityincidentitem._self
            new TranslationSeedItem("entity.qualityincidentitem._self", "ja-JP", "品质事故明细 - 废弃零件明细行信息_jp", "实体名称"),
            // entity.qualityincidentitem._self
            new TranslationSeedItem("entity.qualityincidentitem._self", "zh-CN", "品质事故明细 - 废弃零件明细行信息", "实体名称"),
            // entity.qualityincidentitem._self
            new TranslationSeedItem("entity.qualityincidentitem._self", "zh-HK", "品质事故明细 - 废弃零件明细行信息_hk", "实体名称"),

            // entity.qualityincidentitem.qualityincidentid
            new TranslationSeedItem("entity.qualityincidentitem.qualityincidentid", "en-US", "品质事故主表ID_us", "品质事故主表 ID（选项 TaktQualityIncidents/options；DictValue=Id）"),
            // entity.qualityincidentitem.qualityincidentid
            new TranslationSeedItem("entity.qualityincidentitem.qualityincidentid", "ja-JP", "品质事故主表ID_jp", "品质事故主表 ID（选项 TaktQualityIncidents/options；DictValue=Id）"),
            // entity.qualityincidentitem.qualityincidentid
            new TranslationSeedItem("entity.qualityincidentitem.qualityincidentid", "zh-CN", "品质事故主表ID", "品质事故主表 ID（选项 TaktQualityIncidents/options；DictValue=Id）"),
            // entity.qualityincidentitem.qualityincidentid
            new TranslationSeedItem("entity.qualityincidentitem.qualityincidentid", "zh-HK", "品质事故主表ID_hk", "品质事故主表 ID（选项 TaktQualityIncidents/options；DictValue=Id）"),

            // entity.qualityincidentitem.qualityincidentcode
            new TranslationSeedItem("entity.qualityincidentitem.qualityincidentcode", "en-US", "品质事故编码_us", "品质事故编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.qualityincidentitem.qualityincidentcode
            new TranslationSeedItem("entity.qualityincidentitem.qualityincidentcode", "ja-JP", "品质事故编码_jp", "品质事故编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.qualityincidentitem.qualityincidentcode
            new TranslationSeedItem("entity.qualityincidentitem.qualityincidentcode", "zh-CN", "品质事故编码", "品质事故编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.qualityincidentitem.qualityincidentcode
            new TranslationSeedItem("entity.qualityincidentitem.qualityincidentcode", "zh-HK", "品质事故编码_hk", "品质事故编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.qualityincidentitem.linenumber
            new TranslationSeedItem("entity.qualityincidentitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.qualityincidentitem.linenumber
            new TranslationSeedItem("entity.qualityincidentitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.qualityincidentitem.linenumber
            new TranslationSeedItem("entity.qualityincidentitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityincidentitem.linenumber
            new TranslationSeedItem("entity.qualityincidentitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.qualityincidentitem.materialcode
            new TranslationSeedItem("entity.qualityincidentitem.materialcode", "en-US", "物料编码_us", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.qualityincidentitem.materialcode
            new TranslationSeedItem("entity.qualityincidentitem.materialcode", "ja-JP", "物料编码_jp", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.qualityincidentitem.materialcode
            new TranslationSeedItem("entity.qualityincidentitem.materialcode", "zh-CN", "物料编码", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),
            // entity.qualityincidentitem.materialcode
            new TranslationSeedItem("entity.qualityincidentitem.materialcode", "zh-HK", "物料编码_hk", "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）"),

            // entity.qualityincidentitem.materialdescription
            new TranslationSeedItem("entity.qualityincidentitem.materialdescription", "en-US", "物料描述_us", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.qualityincidentitem.materialdescription
            new TranslationSeedItem("entity.qualityincidentitem.materialdescription", "ja-JP", "物料描述_jp", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.qualityincidentitem.materialdescription
            new TranslationSeedItem("entity.qualityincidentitem.materialdescription", "zh-CN", "物料描述", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),
            // entity.qualityincidentitem.materialdescription
            new TranslationSeedItem("entity.qualityincidentitem.materialdescription", "zh-HK", "物料描述_hk", "物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）"),

            // entity.qualityincidentitem.scrapcost
            new TranslationSeedItem("entity.qualityincidentitem.scrapcost", "en-US", "废弃费用_us", "废弃费用(元)"),
            // entity.qualityincidentitem.scrapcost
            new TranslationSeedItem("entity.qualityincidentitem.scrapcost", "ja-JP", "废弃费用_jp", "废弃费用(元)"),
            // entity.qualityincidentitem.scrapcost
            new TranslationSeedItem("entity.qualityincidentitem.scrapcost", "zh-CN", "废弃费用", "废弃费用(元)"),
            // entity.qualityincidentitem.scrapcost
            new TranslationSeedItem("entity.qualityincidentitem.scrapcost", "zh-HK", "废弃费用_hk", "废弃费用(元)"),

            // entity.qualityincidentitem.scrapsize
            new TranslationSeedItem("entity.qualityincidentitem.scrapsize", "en-US", "废弃数量_us", "废弃数量"),
            // entity.qualityincidentitem.scrapsize
            new TranslationSeedItem("entity.qualityincidentitem.scrapsize", "ja-JP", "废弃数量_jp", "废弃数量"),
            // entity.qualityincidentitem.scrapsize
            new TranslationSeedItem("entity.qualityincidentitem.scrapsize", "zh-CN", "废弃数量", "废弃数量"),
            // entity.qualityincidentitem.scrapsize
            new TranslationSeedItem("entity.qualityincidentitem.scrapsize", "zh-HK", "废弃数量_hk", "废弃数量"),

            // entity.qualityincidentitem.partprice
            new TranslationSeedItem("entity.qualityincidentitem.partprice", "en-US", "零件单价_us", "零件单价(元)"),
            // entity.qualityincidentitem.partprice
            new TranslationSeedItem("entity.qualityincidentitem.partprice", "ja-JP", "零件单价_jp", "零件单价(元)"),
            // entity.qualityincidentitem.partprice
            new TranslationSeedItem("entity.qualityincidentitem.partprice", "zh-CN", "零件单价", "零件单价(元)"),
            // entity.qualityincidentitem.partprice
            new TranslationSeedItem("entity.qualityincidentitem.partprice", "zh-HK", "零件单价_hk", "零件单价(元)"),

            // entity.qualityincidentitem.scrapreasoncost
            new TranslationSeedItem("entity.qualityincidentitem.scrapreasoncost", "en-US", "废弃处理费用_us", "废弃处理费用(元)"),
            // entity.qualityincidentitem.scrapreasoncost
            new TranslationSeedItem("entity.qualityincidentitem.scrapreasoncost", "ja-JP", "废弃处理费用_jp", "废弃处理费用(元)"),
            // entity.qualityincidentitem.scrapreasoncost
            new TranslationSeedItem("entity.qualityincidentitem.scrapreasoncost", "zh-CN", "废弃处理费用", "废弃处理费用(元)"),
            // entity.qualityincidentitem.scrapreasoncost
            new TranslationSeedItem("entity.qualityincidentitem.scrapreasoncost", "zh-HK", "废弃处理费用_hk", "废弃处理费用(元)"),

            // entity.qualityincidentitem.freightcharges
            new TranslationSeedItem("entity.qualityincidentitem.freightcharges", "en-US", "运费_us", "运费(元)"),
            // entity.qualityincidentitem.freightcharges
            new TranslationSeedItem("entity.qualityincidentitem.freightcharges", "ja-JP", "运费_jp", "运费(元)"),
            // entity.qualityincidentitem.freightcharges
            new TranslationSeedItem("entity.qualityincidentitem.freightcharges", "zh-CN", "运费", "运费(元)"),
            // entity.qualityincidentitem.freightcharges
            new TranslationSeedItem("entity.qualityincidentitem.freightcharges", "zh-HK", "运费_hk", "运费(元)"),

            // entity.qualityincidentitem.otherexpenses
            new TranslationSeedItem("entity.qualityincidentitem.otherexpenses", "en-US", "其他费用_us", "其他费用(元)"),
            // entity.qualityincidentitem.otherexpenses
            new TranslationSeedItem("entity.qualityincidentitem.otherexpenses", "ja-JP", "其他费用_jp", "其他费用(元)"),
            // entity.qualityincidentitem.otherexpenses
            new TranslationSeedItem("entity.qualityincidentitem.otherexpenses", "zh-CN", "其他费用", "其他费用(元)"),
            // entity.qualityincidentitem.otherexpenses
            new TranslationSeedItem("entity.qualityincidentitem.otherexpenses", "zh-HK", "其他费用_hk", "其他费用(元)"),

            // entity.qualityincidentitem.reasonworktimeminutes
            new TranslationSeedItem("entity.qualityincidentitem.reasonworktimeminutes", "en-US", "处理作业时间_us", "处理作业时间(分钟)"),
            // entity.qualityincidentitem.reasonworktimeminutes
            new TranslationSeedItem("entity.qualityincidentitem.reasonworktimeminutes", "ja-JP", "处理作业时间_jp", "处理作业时间(分钟)"),
            // entity.qualityincidentitem.reasonworktimeminutes
            new TranslationSeedItem("entity.qualityincidentitem.reasonworktimeminutes", "zh-CN", "处理作业时间", "处理作业时间(分钟)"),
            // entity.qualityincidentitem.reasonworktimeminutes
            new TranslationSeedItem("entity.qualityincidentitem.reasonworktimeminutes", "zh-HK", "处理作业时间_hk", "处理作业时间(分钟)"),

            // entity.qualityincidentitem.tax
            new TranslationSeedItem("entity.qualityincidentitem.tax", "en-US", "关税_us", "关税(元)"),
            // entity.qualityincidentitem.tax
            new TranslationSeedItem("entity.qualityincidentitem.tax", "ja-JP", "关税_jp", "关税(元)"),
            // entity.qualityincidentitem.tax
            new TranslationSeedItem("entity.qualityincidentitem.tax", "zh-CN", "关税", "关税(元)"),
            // entity.qualityincidentitem.tax
            new TranslationSeedItem("entity.qualityincidentitem.tax", "zh-HK", "关税_hk", "关税(元)"),

            // entity.qualityincidentitem.reasonotherexpenses
            new TranslationSeedItem("entity.qualityincidentitem.reasonotherexpenses", "en-US", "处理发生其他费用_us", "处理发生其他费用(元)"),
            // entity.qualityincidentitem.reasonotherexpenses
            new TranslationSeedItem("entity.qualityincidentitem.reasonotherexpenses", "ja-JP", "处理发生其他费用_jp", "处理发生其他费用(元)"),
            // entity.qualityincidentitem.reasonotherexpenses
            new TranslationSeedItem("entity.qualityincidentitem.reasonotherexpenses", "zh-CN", "处理发生其他费用", "处理发生其他费用(元)"),
            // entity.qualityincidentitem.reasonotherexpenses
            new TranslationSeedItem("entity.qualityincidentitem.reasonotherexpenses", "zh-HK", "处理发生其他费用_hk", "处理发生其他费用(元)"),

            // entity.qualityincidentitem.scrapnote
            new TranslationSeedItem("entity.qualityincidentitem.scrapnote", "en-US", "废弃备注_us", "废弃备注"),
            // entity.qualityincidentitem.scrapnote
            new TranslationSeedItem("entity.qualityincidentitem.scrapnote", "ja-JP", "废弃备注_jp", "废弃备注"),
            // entity.qualityincidentitem.scrapnote
            new TranslationSeedItem("entity.qualityincidentitem.scrapnote", "zh-CN", "废弃备注", "废弃备注"),
            // entity.qualityincidentitem.scrapnote
            new TranslationSeedItem("entity.qualityincidentitem.scrapnote", "zh-HK", "废弃备注_hk", "废弃备注"),

            // entity.qualityincidentitem.isobsolete
            new TranslationSeedItem("entity.qualityincidentitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityincidentitem.isobsolete
            new TranslationSeedItem("entity.qualityincidentitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityincidentitem.isobsolete
            new TranslationSeedItem("entity.qualityincidentitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityincidentitem.isobsolete
            new TranslationSeedItem("entity.qualityincidentitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.qualityincidentitem.incident
            new TranslationSeedItem("entity.qualityincidentitem.incident", "en-US", "品质事故主表_us", "品质事故主表(导航属性)"),
            // entity.qualityincidentitem.incident
            new TranslationSeedItem("entity.qualityincidentitem.incident", "ja-JP", "品质事故主表_jp", "品质事故主表(导航属性)"),
            // entity.qualityincidentitem.incident
            new TranslationSeedItem("entity.qualityincidentitem.incident", "zh-CN", "品质事故主表", "品质事故主表(导航属性)"),
            // entity.qualityincidentitem.incident
            new TranslationSeedItem("entity.qualityincidentitem.incident", "zh-HK", "品质事故主表_hk", "品质事故主表(导航属性)"),
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
        translation.ResourceGroup = "Cost";
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
