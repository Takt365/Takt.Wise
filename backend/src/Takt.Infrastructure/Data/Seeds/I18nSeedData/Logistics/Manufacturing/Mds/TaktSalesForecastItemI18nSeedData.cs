// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mds
// 文件名称：TaktSalesForecastItemI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesForecastItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Mds;

/// <summary>
/// TaktSalesForecastItem 实体国际化翻译种子（键前缀 entity.salesforecastitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesForecastItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesForecastItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesforecastitem 实体翻译...", tenantCode);

        foreach (var item in GetSalesForecastItemTranslations())
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

        TaktLogger.Information("TaktSalesForecastItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesForecastItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesforecastitem._self / entity.salesforecastitem.{{field}}；ResourceGroup=Mds；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesForecastItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesforecastitem._self
            new TranslationSeedItem("entity.salesforecastitem._self", "en-US", "Sales Forecast Item Information_us", "实体名称"),
            // entity.salesforecastitem._self
            new TranslationSeedItem("entity.salesforecastitem._self", "ja-JP", "Takt销售预测明细信息_jp", "实体名称"),
            // entity.salesforecastitem._self
            new TranslationSeedItem("entity.salesforecastitem._self", "zh-CN", "Takt销售预测明细信息", "实体名称"),
            // entity.salesforecastitem._self
            new TranslationSeedItem("entity.salesforecastitem._self", "zh-HK", "Takt销售预测明细信息_hk", "实体名称"),

            // entity.salesforecastitem.salesforecastid
            new TranslationSeedItem("entity.salesforecastitem.salesforecastid", "en-US", "销售预测ID_us", "销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.salesforecastitem.salesforecastid
            new TranslationSeedItem("entity.salesforecastitem.salesforecastid", "ja-JP", "销售预测ID_jp", "销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.salesforecastitem.salesforecastid
            new TranslationSeedItem("entity.salesforecastitem.salesforecastid", "zh-CN", "销售预测ID", "销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.salesforecastitem.salesforecastid
            new TranslationSeedItem("entity.salesforecastitem.salesforecastid", "zh-HK", "销售预测ID_hk", "销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.salesforecastitem.salesforecastcode
            new TranslationSeedItem("entity.salesforecastitem.salesforecastcode", "en-US", "销售预测编码_us", "销售预测编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesforecastitem.salesforecastcode
            new TranslationSeedItem("entity.salesforecastitem.salesforecastcode", "ja-JP", "销售预测编码_jp", "销售预测编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesforecastitem.salesforecastcode
            new TranslationSeedItem("entity.salesforecastitem.salesforecastcode", "zh-CN", "销售预测编码", "销售预测编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.salesforecastitem.salesforecastcode
            new TranslationSeedItem("entity.salesforecastitem.salesforecastcode", "zh-HK", "销售预测编码_hk", "销售预测编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.salesforecastitem.linenumber
            new TranslationSeedItem("entity.salesforecastitem.linenumber", "en-US", "行号_us", "行号（固定步长=10）"),
            // entity.salesforecastitem.linenumber
            new TranslationSeedItem("entity.salesforecastitem.linenumber", "ja-JP", "行号_jp", "行号（固定步长=10）"),
            // entity.salesforecastitem.linenumber
            new TranslationSeedItem("entity.salesforecastitem.linenumber", "zh-CN", "行号", "行号（固定步长=10）"),
            // entity.salesforecastitem.linenumber
            new TranslationSeedItem("entity.salesforecastitem.linenumber", "zh-HK", "行号_hk", "行号（固定步长=10）"),

            // entity.salesforecastitem.fiscalyear
            new TranslationSeedItem("entity.salesforecastitem.fiscalyear", "en-US", "财年_us", "财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）"),
            // entity.salesforecastitem.fiscalyear
            new TranslationSeedItem("entity.salesforecastitem.fiscalyear", "ja-JP", "财年_jp", "财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）"),
            // entity.salesforecastitem.fiscalyear
            new TranslationSeedItem("entity.salesforecastitem.fiscalyear", "zh-CN", "财年", "财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）"),
            // entity.salesforecastitem.fiscalyear
            new TranslationSeedItem("entity.salesforecastitem.fiscalyear", "zh-HK", "财年_hk", "财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）"),

            // entity.salesforecastitem.planmonth
            new TranslationSeedItem("entity.salesforecastitem.planmonth", "en-US", "计划月份_us", "计划月份（1～12）"),
            // entity.salesforecastitem.planmonth
            new TranslationSeedItem("entity.salesforecastitem.planmonth", "ja-JP", "计划月份_jp", "计划月份（1～12）"),
            // entity.salesforecastitem.planmonth
            new TranslationSeedItem("entity.salesforecastitem.planmonth", "zh-CN", "计划月份", "计划月份（1～12）"),
            // entity.salesforecastitem.planmonth
            new TranslationSeedItem("entity.salesforecastitem.planmonth", "zh-HK", "计划月份_hk", "计划月份（1～12）"),

            // entity.salesforecastitem.planquantity001
            new TranslationSeedItem("entity.salesforecastitem.planquantity001", "en-US", "计划数量版本001_us", "计划数量版本001"),
            // entity.salesforecastitem.planquantity001
            new TranslationSeedItem("entity.salesforecastitem.planquantity001", "ja-JP", "计划数量版本001_jp", "计划数量版本001"),
            // entity.salesforecastitem.planquantity001
            new TranslationSeedItem("entity.salesforecastitem.planquantity001", "zh-CN", "计划数量版本001", "计划数量版本001"),
            // entity.salesforecastitem.planquantity001
            new TranslationSeedItem("entity.salesforecastitem.planquantity001", "zh-HK", "计划数量版本001_hk", "计划数量版本001"),

            // entity.salesforecastitem.planquantity002
            new TranslationSeedItem("entity.salesforecastitem.planquantity002", "en-US", "计划数量版本002_us", "计划数量版本002"),
            // entity.salesforecastitem.planquantity002
            new TranslationSeedItem("entity.salesforecastitem.planquantity002", "ja-JP", "计划数量版本002_jp", "计划数量版本002"),
            // entity.salesforecastitem.planquantity002
            new TranslationSeedItem("entity.salesforecastitem.planquantity002", "zh-CN", "计划数量版本002", "计划数量版本002"),
            // entity.salesforecastitem.planquantity002
            new TranslationSeedItem("entity.salesforecastitem.planquantity002", "zh-HK", "计划数量版本002_hk", "计划数量版本002"),

            // entity.salesforecastitem.planquantitydelta
            new TranslationSeedItem("entity.salesforecastitem.planquantitydelta", "en-US", "计划增减_us", "计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）"),
            // entity.salesforecastitem.planquantitydelta
            new TranslationSeedItem("entity.salesforecastitem.planquantitydelta", "ja-JP", "计划增减_jp", "计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）"),
            // entity.salesforecastitem.planquantitydelta
            new TranslationSeedItem("entity.salesforecastitem.planquantitydelta", "zh-CN", "计划增减", "计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）"),
            // entity.salesforecastitem.planquantitydelta
            new TranslationSeedItem("entity.salesforecastitem.planquantitydelta", "zh-HK", "计划增减_hk", "计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）"),

            // entity.salesforecastitem.convertedquantity
            new TranslationSeedItem("entity.salesforecastitem.convertedquantity", "en-US", "已转生产销售数量_us", "已转生产/销售数量（基本单位数量）"),
            // entity.salesforecastitem.convertedquantity
            new TranslationSeedItem("entity.salesforecastitem.convertedquantity", "ja-JP", "已转生产销售数量_jp", "已转生产/销售数量（基本单位数量）"),
            // entity.salesforecastitem.convertedquantity
            new TranslationSeedItem("entity.salesforecastitem.convertedquantity", "zh-CN", "已转生产销售数量", "已转生产/销售数量（基本单位数量）"),
            // entity.salesforecastitem.convertedquantity
            new TranslationSeedItem("entity.salesforecastitem.convertedquantity", "zh-HK", "已转生产销售数量_hk", "已转生产/销售数量（基本单位数量）"),

            // entity.salesforecastitem.estimatedunitprice
            new TranslationSeedItem("entity.salesforecastitem.estimatedunitprice", "en-US", "预计单价_us", "预计单价"),
            // entity.salesforecastitem.estimatedunitprice
            new TranslationSeedItem("entity.salesforecastitem.estimatedunitprice", "ja-JP", "预计单价_jp", "预计单价"),
            // entity.salesforecastitem.estimatedunitprice
            new TranslationSeedItem("entity.salesforecastitem.estimatedunitprice", "zh-CN", "预计单价", "预计单价"),
            // entity.salesforecastitem.estimatedunitprice
            new TranslationSeedItem("entity.salesforecastitem.estimatedunitprice", "zh-HK", "预计单价_hk", "预计单价"),

            // entity.salesforecastitem.estimatedamount
            new TranslationSeedItem("entity.salesforecastitem.estimatedamount", "en-US", "预计金额_us", "预计金额"),
            // entity.salesforecastitem.estimatedamount
            new TranslationSeedItem("entity.salesforecastitem.estimatedamount", "ja-JP", "预计金额_jp", "预计金额"),
            // entity.salesforecastitem.estimatedamount
            new TranslationSeedItem("entity.salesforecastitem.estimatedamount", "zh-CN", "预计金额", "预计金额"),
            // entity.salesforecastitem.estimatedamount
            new TranslationSeedItem("entity.salesforecastitem.estimatedamount", "zh-HK", "预计金额_hk", "预计金额"),

            // entity.salesforecastitem.isobsolete
            new TranslationSeedItem("entity.salesforecastitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesforecastitem.isobsolete
            new TranslationSeedItem("entity.salesforecastitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesforecastitem.isobsolete
            new TranslationSeedItem("entity.salesforecastitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.salesforecastitem.isobsolete
            new TranslationSeedItem("entity.salesforecastitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "Mds";
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
