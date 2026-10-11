// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityIncidentI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityIncident 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityIncident 实体国际化翻译种子（键前缀 entity.qualityincident.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityIncidentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityIncident 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityincident 实体翻译...", tenantCode);

        foreach (var item in GetQualityIncidentTranslations())
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

        TaktLogger.Information("TaktQualityIncident 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityIncident 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityincident._self / entity.qualityincident.{{field}}；ResourceGroup=Cost；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityIncidentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityincident._self
            new TranslationSeedItem("entity.qualityincident._self", "en-US", "Quality Incident Information_us", "实体名称"),
            // entity.qualityincident._self
            new TranslationSeedItem("entity.qualityincident._self", "ja-JP", "品质事故主表信息_jp", "实体名称"),
            // entity.qualityincident._self
            new TranslationSeedItem("entity.qualityincident._self", "zh-CN", "品质事故主表信息", "实体名称"),
            // entity.qualityincident._self
            new TranslationSeedItem("entity.qualityincident._self", "zh-HK", "品质事故主表信息_hk", "实体名称"),

            // entity.qualityincident.code
            new TranslationSeedItem("entity.qualityincident.code", "en-US", "品质事故编码_us", "品质事故编码(唯一,如:QI-2026-0001)"),
            // entity.qualityincident.code
            new TranslationSeedItem("entity.qualityincident.code", "ja-JP", "品质事故编码_jp", "品质事故编码(唯一,如:QI-2026-0001)"),
            // entity.qualityincident.code
            new TranslationSeedItem("entity.qualityincident.code", "zh-CN", "品质事故编码", "品质事故编码(唯一,如:QI-2026-0001)"),
            // entity.qualityincident.code
            new TranslationSeedItem("entity.qualityincident.code", "zh-HK", "品质事故编码_hk", "品质事故编码(唯一,如:QI-2026-0001)"),

            // entity.qualityincident.incidentdate
            new TranslationSeedItem("entity.qualityincident.incidentdate", "en-US", "事故日期_us", "事故日期"),
            // entity.qualityincident.incidentdate
            new TranslationSeedItem("entity.qualityincident.incidentdate", "ja-JP", "事故日期_jp", "事故日期"),
            // entity.qualityincident.incidentdate
            new TranslationSeedItem("entity.qualityincident.incidentdate", "zh-CN", "事故日期", "事故日期"),
            // entity.qualityincident.incidentdate
            new TranslationSeedItem("entity.qualityincident.incidentdate", "zh-HK", "事故日期_hk", "事故日期"),

            // entity.qualityincident.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityincident.indirectmanpowercostperminute", "en-US", "间接人员费率_us", "间接人员费率(元/分钟)"),
            // entity.qualityincident.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityincident.indirectmanpowercostperminute", "ja-JP", "间接人员费率_jp", "间接人员费率(元/分钟)"),
            // entity.qualityincident.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityincident.indirectmanpowercostperminute", "zh-CN", "间接人员费率", "间接人员费率(元/分钟)"),
            // entity.qualityincident.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityincident.indirectmanpowercostperminute", "zh-HK", "间接人员费率_hk", "间接人员费率(元/分钟)"),

            // entity.qualityincident.model
            new TranslationSeedItem("entity.qualityincident.model", "en-US", "机种_us", "机种/产品型号"),
            // entity.qualityincident.model
            new TranslationSeedItem("entity.qualityincident.model", "ja-JP", "机种_jp", "机种/产品型号"),
            // entity.qualityincident.model
            new TranslationSeedItem("entity.qualityincident.model", "zh-CN", "机种", "机种/产品型号"),
            // entity.qualityincident.model
            new TranslationSeedItem("entity.qualityincident.model", "zh-HK", "机种_hk", "机种/产品型号"),

            // entity.qualityincident.incidentreason
            new TranslationSeedItem("entity.qualityincident.incidentreason", "en-US", "事故内容_us", "事故内容(废弃原因)"),
            // entity.qualityincident.incidentreason
            new TranslationSeedItem("entity.qualityincident.incidentreason", "ja-JP", "事故内容_jp", "事故内容(废弃原因)"),
            // entity.qualityincident.incidentreason
            new TranslationSeedItem("entity.qualityincident.incidentreason", "zh-CN", "事故内容", "事故内容(废弃原因)"),
            // entity.qualityincident.incidentreason
            new TranslationSeedItem("entity.qualityincident.incidentreason", "zh-HK", "事故内容_hk", "事故内容(废弃原因)"),

            // entity.qualityincident.scrapgroupcode
            new TranslationSeedItem("entity.qualityincident.scrapgroupcode", "en-US", "废弃组编码_us", "废弃组编码（选项 TaktCostGroups/options?costGroupCategory=2；DictValue=CostGroupCode）"),
            // entity.qualityincident.scrapgroupcode
            new TranslationSeedItem("entity.qualityincident.scrapgroupcode", "ja-JP", "废弃组编码_jp", "废弃组编码（选项 TaktCostGroups/options?costGroupCategory=2；DictValue=CostGroupCode）"),
            // entity.qualityincident.scrapgroupcode
            new TranslationSeedItem("entity.qualityincident.scrapgroupcode", "zh-CN", "废弃组编码", "废弃组编码（选项 TaktCostGroups/options?costGroupCategory=2；DictValue=CostGroupCode）"),
            // entity.qualityincident.scrapgroupcode
            new TranslationSeedItem("entity.qualityincident.scrapgroupcode", "zh-HK", "废弃组编码_hk", "废弃组编码（选项 TaktCostGroups/options?costGroupCategory=2；DictValue=CostGroupCode）"),

            // entity.qualityincident.scrapgroupname
            new TranslationSeedItem("entity.qualityincident.scrapgroupname", "en-US", "废弃组名称_us", "废弃组名称（冗余：按 ScrapGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityincident.scrapgroupname
            new TranslationSeedItem("entity.qualityincident.scrapgroupname", "ja-JP", "废弃组名称_jp", "废弃组名称（冗余：按 ScrapGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityincident.scrapgroupname
            new TranslationSeedItem("entity.qualityincident.scrapgroupname", "zh-CN", "废弃组名称", "废弃组名称（冗余：按 ScrapGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityincident.scrapgroupname
            new TranslationSeedItem("entity.qualityincident.scrapgroupname", "zh-HK", "废弃组名称_hk", "废弃组名称（冗余：按 ScrapGroupCode 取 TaktCostGroups.CostGroupName 联动）"),

            // entity.qualityincident.totalscrapquantity
            new TranslationSeedItem("entity.qualityincident.totalscrapquantity", "en-US", "废弃总数_us", "废弃总数(自动计算 = 各子表废弃数量合计)"),
            // entity.qualityincident.totalscrapquantity
            new TranslationSeedItem("entity.qualityincident.totalscrapquantity", "ja-JP", "废弃总数_jp", "废弃总数(自动计算 = 各子表废弃数量合计)"),
            // entity.qualityincident.totalscrapquantity
            new TranslationSeedItem("entity.qualityincident.totalscrapquantity", "zh-CN", "废弃总数", "废弃总数(自动计算 = 各子表废弃数量合计)"),
            // entity.qualityincident.totalscrapquantity
            new TranslationSeedItem("entity.qualityincident.totalscrapquantity", "zh-HK", "废弃总数_hk", "废弃总数(自动计算 = 各子表废弃数量合计)"),

            // entity.qualityincident.totalscrapcost
            new TranslationSeedItem("entity.qualityincident.totalscrapcost", "en-US", "总废弃费用_us", "总废弃费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityincident.totalscrapcost
            new TranslationSeedItem("entity.qualityincident.totalscrapcost", "ja-JP", "总废弃费用_jp", "总废弃费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityincident.totalscrapcost
            new TranslationSeedItem("entity.qualityincident.totalscrapcost", "zh-CN", "总废弃费用", "总废弃费用(元,自动计算 = 各子表费用合计)"),
            // entity.qualityincident.totalscrapcost
            new TranslationSeedItem("entity.qualityincident.totalscrapcost", "zh-HK", "总废弃费用_hk", "总废弃费用(元,自动计算 = 各子表费用合计)"),

            // entity.qualityincident.currencycode
            new TranslationSeedItem("entity.qualityincident.currencycode", "en-US", "成本币种_us", "成本币种(CNY/USD/JPY等)"),
            // entity.qualityincident.currencycode
            new TranslationSeedItem("entity.qualityincident.currencycode", "ja-JP", "成本币种_jp", "成本币种(CNY/USD/JPY等)"),
            // entity.qualityincident.currencycode
            new TranslationSeedItem("entity.qualityincident.currencycode", "zh-CN", "成本币种", "成本币种(CNY/USD/JPY等)"),
            // entity.qualityincident.currencycode
            new TranslationSeedItem("entity.qualityincident.currencycode", "zh-HK", "成本币种_hk", "成本币种(CNY/USD/JPY等)"),

            // entity.qualityincident.incidentitems
            new TranslationSeedItem("entity.qualityincident.incidentitems", "en-US", "事故明细列表_us", "事故明细列表"),
            // entity.qualityincident.incidentitems
            new TranslationSeedItem("entity.qualityincident.incidentitems", "ja-JP", "事故明细列表_jp", "事故明细列表"),
            // entity.qualityincident.incidentitems
            new TranslationSeedItem("entity.qualityincident.incidentitems", "zh-CN", "事故明细列表", "事故明细列表"),
            // entity.qualityincident.incidentitems
            new TranslationSeedItem("entity.qualityincident.incidentitems", "zh-HK", "事故明细列表_hk", "事故明细列表"),
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
