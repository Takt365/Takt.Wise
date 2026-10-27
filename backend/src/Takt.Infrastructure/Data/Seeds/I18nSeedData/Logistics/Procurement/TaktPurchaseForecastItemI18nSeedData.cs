// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement
// 文件名称：TaktPurchaseForecastItemI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktPurchaseForecastItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Procurement;

/// <summary>
/// TaktPurchaseForecastItem 实体国际化翻译种子（键前缀 entity.purchaseforecastitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktPurchaseForecastItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktPurchaseForecastItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 purchaseforecastitem 实体翻译...", tenantCode);

        foreach (var item in GetPurchaseForecastItemTranslations())
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

        TaktLogger.Information("TaktPurchaseForecastItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktPurchaseForecastItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.purchaseforecastitem._self / entity.purchaseforecastitem.{{field}}；ResourceGroup=Procurement；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetPurchaseForecastItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.purchaseforecastitem._self
            new TranslationSeedItem("entity.purchaseforecastitem._self", "en-US", "Purchase Forecast Item Information_us", "实体名称"),
            // entity.purchaseforecastitem._self
            new TranslationSeedItem("entity.purchaseforecastitem._self", "ja-JP", "Takt采购预测明细信息_jp", "实体名称"),
            // entity.purchaseforecastitem._self
            new TranslationSeedItem("entity.purchaseforecastitem._self", "zh-CN", "Takt采购预测明细信息", "实体名称"),
            // entity.purchaseforecastitem._self
            new TranslationSeedItem("entity.purchaseforecastitem._self", "zh-HK", "Takt采购预测明细信息_hk", "实体名称"),

            // entity.purchaseforecastitem.purchaseforecastid
            new TranslationSeedItem("entity.purchaseforecastitem.purchaseforecastid", "en-US", "采购预测ID_us", "采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseforecastitem.purchaseforecastid
            new TranslationSeedItem("entity.purchaseforecastitem.purchaseforecastid", "ja-JP", "采购预测ID_jp", "采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseforecastitem.purchaseforecastid
            new TranslationSeedItem("entity.purchaseforecastitem.purchaseforecastid", "zh-CN", "采购预测ID", "采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.purchaseforecastitem.purchaseforecastid
            new TranslationSeedItem("entity.purchaseforecastitem.purchaseforecastid", "zh-HK", "采购预测ID_hk", "采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.purchaseforecastitem.purchaseforecastcode
            new TranslationSeedItem("entity.purchaseforecastitem.purchaseforecastcode", "en-US", "采购预测编码_us", "采购预测编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseforecastitem.purchaseforecastcode
            new TranslationSeedItem("entity.purchaseforecastitem.purchaseforecastcode", "ja-JP", "采购预测编码_jp", "采购预测编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseforecastitem.purchaseforecastcode
            new TranslationSeedItem("entity.purchaseforecastitem.purchaseforecastcode", "zh-CN", "采购预测编码", "采购预测编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.purchaseforecastitem.purchaseforecastcode
            new TranslationSeedItem("entity.purchaseforecastitem.purchaseforecastcode", "zh-HK", "采购预测编码_hk", "采购预测编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.purchaseforecastitem.linenumber
            new TranslationSeedItem("entity.purchaseforecastitem.linenumber", "en-US", "行号_us", "行号（固定步长=10）"),
            // entity.purchaseforecastitem.linenumber
            new TranslationSeedItem("entity.purchaseforecastitem.linenumber", "ja-JP", "行号_jp", "行号（固定步长=10）"),
            // entity.purchaseforecastitem.linenumber
            new TranslationSeedItem("entity.purchaseforecastitem.linenumber", "zh-CN", "行号", "行号（固定步长=10）"),
            // entity.purchaseforecastitem.linenumber
            new TranslationSeedItem("entity.purchaseforecastitem.linenumber", "zh-HK", "行号_hk", "行号（固定步长=10）"),

            // entity.purchaseforecastitem.fiscalyear
            new TranslationSeedItem("entity.purchaseforecastitem.fiscalyear", "en-US", "财年_us", "财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）"),
            // entity.purchaseforecastitem.fiscalyear
            new TranslationSeedItem("entity.purchaseforecastitem.fiscalyear", "ja-JP", "财年_jp", "财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）"),
            // entity.purchaseforecastitem.fiscalyear
            new TranslationSeedItem("entity.purchaseforecastitem.fiscalyear", "zh-CN", "财年", "财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）"),
            // entity.purchaseforecastitem.fiscalyear
            new TranslationSeedItem("entity.purchaseforecastitem.fiscalyear", "zh-HK", "财年_hk", "财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）"),

            // entity.purchaseforecastitem.planmonth
            new TranslationSeedItem("entity.purchaseforecastitem.planmonth", "en-US", "计划月份_us", "计划月份（1～12）"),
            // entity.purchaseforecastitem.planmonth
            new TranslationSeedItem("entity.purchaseforecastitem.planmonth", "ja-JP", "计划月份_jp", "计划月份（1～12）"),
            // entity.purchaseforecastitem.planmonth
            new TranslationSeedItem("entity.purchaseforecastitem.planmonth", "zh-CN", "计划月份", "计划月份（1～12）"),
            // entity.purchaseforecastitem.planmonth
            new TranslationSeedItem("entity.purchaseforecastitem.planmonth", "zh-HK", "计划月份_hk", "计划月份（1～12）"),

            // entity.purchaseforecastitem.planquantity001
            new TranslationSeedItem("entity.purchaseforecastitem.planquantity001", "en-US", "计划数量版本001_us", "计划数量版本001"),
            // entity.purchaseforecastitem.planquantity001
            new TranslationSeedItem("entity.purchaseforecastitem.planquantity001", "ja-JP", "计划数量版本001_jp", "计划数量版本001"),
            // entity.purchaseforecastitem.planquantity001
            new TranslationSeedItem("entity.purchaseforecastitem.planquantity001", "zh-CN", "计划数量版本001", "计划数量版本001"),
            // entity.purchaseforecastitem.planquantity001
            new TranslationSeedItem("entity.purchaseforecastitem.planquantity001", "zh-HK", "计划数量版本001_hk", "计划数量版本001"),

            // entity.purchaseforecastitem.planquantity002
            new TranslationSeedItem("entity.purchaseforecastitem.planquantity002", "en-US", "计划数量版本002_us", "计划数量版本002"),
            // entity.purchaseforecastitem.planquantity002
            new TranslationSeedItem("entity.purchaseforecastitem.planquantity002", "ja-JP", "计划数量版本002_jp", "计划数量版本002"),
            // entity.purchaseforecastitem.planquantity002
            new TranslationSeedItem("entity.purchaseforecastitem.planquantity002", "zh-CN", "计划数量版本002", "计划数量版本002"),
            // entity.purchaseforecastitem.planquantity002
            new TranslationSeedItem("entity.purchaseforecastitem.planquantity002", "zh-HK", "计划数量版本002_hk", "计划数量版本002"),

            // entity.purchaseforecastitem.planquantitydelta
            new TranslationSeedItem("entity.purchaseforecastitem.planquantitydelta", "en-US", "计划增减_us", "计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）"),
            // entity.purchaseforecastitem.planquantitydelta
            new TranslationSeedItem("entity.purchaseforecastitem.planquantitydelta", "ja-JP", "计划增减_jp", "计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）"),
            // entity.purchaseforecastitem.planquantitydelta
            new TranslationSeedItem("entity.purchaseforecastitem.planquantitydelta", "zh-CN", "计划增减", "计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）"),
            // entity.purchaseforecastitem.planquantitydelta
            new TranslationSeedItem("entity.purchaseforecastitem.planquantitydelta", "zh-HK", "计划增减_hk", "计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）"),

            // entity.purchaseforecastitem.convertedquantity
            new TranslationSeedItem("entity.purchaseforecastitem.convertedquantity", "en-US", "已转采购数量_us", "已转采购数量（基本单位数量）"),
            // entity.purchaseforecastitem.convertedquantity
            new TranslationSeedItem("entity.purchaseforecastitem.convertedquantity", "ja-JP", "已转采购数量_jp", "已转采购数量（基本单位数量）"),
            // entity.purchaseforecastitem.convertedquantity
            new TranslationSeedItem("entity.purchaseforecastitem.convertedquantity", "zh-CN", "已转采购数量", "已转采购数量（基本单位数量）"),
            // entity.purchaseforecastitem.convertedquantity
            new TranslationSeedItem("entity.purchaseforecastitem.convertedquantity", "zh-HK", "已转采购数量_hk", "已转采购数量（基本单位数量）"),

            // entity.purchaseforecastitem.estimatedunitprice
            new TranslationSeedItem("entity.purchaseforecastitem.estimatedunitprice", "en-US", "预计单价_us", "预计单价"),
            // entity.purchaseforecastitem.estimatedunitprice
            new TranslationSeedItem("entity.purchaseforecastitem.estimatedunitprice", "ja-JP", "预计单价_jp", "预计单价"),
            // entity.purchaseforecastitem.estimatedunitprice
            new TranslationSeedItem("entity.purchaseforecastitem.estimatedunitprice", "zh-CN", "预计单价", "预计单价"),
            // entity.purchaseforecastitem.estimatedunitprice
            new TranslationSeedItem("entity.purchaseforecastitem.estimatedunitprice", "zh-HK", "预计单价_hk", "预计单价"),

            // entity.purchaseforecastitem.estimatedamount
            new TranslationSeedItem("entity.purchaseforecastitem.estimatedamount", "en-US", "预计金额_us", "预计金额"),
            // entity.purchaseforecastitem.estimatedamount
            new TranslationSeedItem("entity.purchaseforecastitem.estimatedamount", "ja-JP", "预计金额_jp", "预计金额"),
            // entity.purchaseforecastitem.estimatedamount
            new TranslationSeedItem("entity.purchaseforecastitem.estimatedamount", "zh-CN", "预计金额", "预计金额"),
            // entity.purchaseforecastitem.estimatedamount
            new TranslationSeedItem("entity.purchaseforecastitem.estimatedamount", "zh-HK", "预计金额_hk", "预计金额"),

            // entity.purchaseforecastitem.isobsolete
            new TranslationSeedItem("entity.purchaseforecastitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseforecastitem.isobsolete
            new TranslationSeedItem("entity.purchaseforecastitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseforecastitem.isobsolete
            new TranslationSeedItem("entity.purchaseforecastitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.purchaseforecastitem.isobsolete
            new TranslationSeedItem("entity.purchaseforecastitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Procurement";
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
