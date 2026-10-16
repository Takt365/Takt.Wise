// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityAssuranceIncomingI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityAssuranceIncoming 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityAssuranceIncoming 实体国际化翻译种子（键前缀 entity.qualityassuranceincoming.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityAssuranceIncomingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityAssuranceIncoming 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityassuranceincoming 实体翻译...", tenantCode);

        foreach (var item in GetQualityAssuranceIncomingTranslations())
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

        TaktLogger.Information("TaktQualityAssuranceIncoming 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityAssuranceIncoming 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityassuranceincoming._self / entity.qualityassuranceincoming.{{field}}；ResourceGroup=Cost；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityAssuranceIncomingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityassuranceincoming._self
            new TranslationSeedItem("entity.qualityassuranceincoming._self", "en-US", "Quality Assurance Incoming Information_us", "实体名称"),
            // entity.qualityassuranceincoming._self
            new TranslationSeedItem("entity.qualityassuranceincoming._self", "ja-JP", "品质业务明细 - 来料检验费用信息_jp", "实体名称"),
            // entity.qualityassuranceincoming._self
            new TranslationSeedItem("entity.qualityassuranceincoming._self", "zh-CN", "品质业务明细 - 来料检验费用信息", "实体名称"),
            // entity.qualityassuranceincoming._self
            new TranslationSeedItem("entity.qualityassuranceincoming._self", "zh-HK", "品质业务明细 - 来料检验费用信息_hk", "实体名称"),

            // entity.qualityassuranceincoming.qualityassuranceid
            new TranslationSeedItem("entity.qualityassuranceincoming.qualityassuranceid", "en-US", "品质业务主表ID_us", "品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）"),
            // entity.qualityassuranceincoming.qualityassuranceid
            new TranslationSeedItem("entity.qualityassuranceincoming.qualityassuranceid", "ja-JP", "品质业务主表ID_jp", "品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）"),
            // entity.qualityassuranceincoming.qualityassuranceid
            new TranslationSeedItem("entity.qualityassuranceincoming.qualityassuranceid", "zh-CN", "品质业务主表ID", "品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）"),
            // entity.qualityassuranceincoming.qualityassuranceid
            new TranslationSeedItem("entity.qualityassuranceincoming.qualityassuranceid", "zh-HK", "品质业务主表ID_hk", "品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）"),

            // entity.qualityassuranceincoming.qualityassurancecode
            new TranslationSeedItem("entity.qualityassuranceincoming.qualityassurancecode", "en-US", "品质业务编码_us", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityassuranceincoming.qualityassurancecode
            new TranslationSeedItem("entity.qualityassuranceincoming.qualityassurancecode", "ja-JP", "品质业务编码_jp", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityassuranceincoming.qualityassurancecode
            new TranslationSeedItem("entity.qualityassuranceincoming.qualityassurancecode", "zh-CN", "品质业务编码", "品质业务编码（冗余字段,便于查询）"),
            // entity.qualityassuranceincoming.qualityassurancecode
            new TranslationSeedItem("entity.qualityassuranceincoming.qualityassurancecode", "zh-HK", "品质业务编码_hk", "品质业务编码（冗余字段,便于查询）"),

            // entity.qualityassuranceincoming.linenumber
            new TranslationSeedItem("entity.qualityassuranceincoming.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.qualityassuranceincoming.linenumber
            new TranslationSeedItem("entity.qualityassuranceincoming.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.qualityassuranceincoming.linenumber
            new TranslationSeedItem("entity.qualityassuranceincoming.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityassuranceincoming.linenumber
            new TranslationSeedItem("entity.qualityassuranceincoming.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.qualityassuranceincoming.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityassuranceincoming.directmanpowercostperminute", "en-US", "直接人员费率_us", "直接人员费率(元/分钟)"),
            // entity.qualityassuranceincoming.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityassuranceincoming.directmanpowercostperminute", "ja-JP", "直接人员费率_jp", "直接人员费率(元/分钟)"),
            // entity.qualityassuranceincoming.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityassuranceincoming.directmanpowercostperminute", "zh-CN", "直接人员费率", "直接人员费率(元/分钟)"),
            // entity.qualityassuranceincoming.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityassuranceincoming.directmanpowercostperminute", "zh-HK", "直接人员费率_hk", "直接人员费率(元/分钟)"),

            // entity.qualityassuranceincoming.incominginspectioncost
            new TranslationSeedItem("entity.qualityassuranceincoming.incominginspectioncost", "en-US", "来料检验业务费用_us", "来料检验业务费用(元)"),
            // entity.qualityassuranceincoming.incominginspectioncost
            new TranslationSeedItem("entity.qualityassuranceincoming.incominginspectioncost", "ja-JP", "来料检验业务费用_jp", "来料检验业务费用(元)"),
            // entity.qualityassuranceincoming.incominginspectioncost
            new TranslationSeedItem("entity.qualityassuranceincoming.incominginspectioncost", "zh-CN", "来料检验业务费用", "来料检验业务费用(元)"),
            // entity.qualityassuranceincoming.incominginspectioncost
            new TranslationSeedItem("entity.qualityassuranceincoming.incominginspectioncost", "zh-HK", "来料检验业务费用_hk", "来料检验业务费用(元)"),

            // entity.qualityassuranceincoming.inspectiontimeminutes
            new TranslationSeedItem("entity.qualityassuranceincoming.inspectiontimeminutes", "en-US", "检查时间_us", "检查时间(分钟)"),
            // entity.qualityassuranceincoming.inspectiontimeminutes
            new TranslationSeedItem("entity.qualityassuranceincoming.inspectiontimeminutes", "ja-JP", "检查时间_jp", "检查时间(分钟)"),
            // entity.qualityassuranceincoming.inspectiontimeminutes
            new TranslationSeedItem("entity.qualityassuranceincoming.inspectiontimeminutes", "zh-CN", "检查时间", "检查时间(分钟)"),
            // entity.qualityassuranceincoming.inspectiontimeminutes
            new TranslationSeedItem("entity.qualityassuranceincoming.inspectiontimeminutes", "zh-HK", "检查时间_hk", "检查时间(分钟)"),

            // entity.qualityassuranceincoming.travelcost
            new TranslationSeedItem("entity.qualityassuranceincoming.travelcost", "en-US", "交通费旅费_us", "交通费、旅费(元)"),
            // entity.qualityassuranceincoming.travelcost
            new TranslationSeedItem("entity.qualityassuranceincoming.travelcost", "ja-JP", "交通费旅费_jp", "交通费、旅费(元)"),
            // entity.qualityassuranceincoming.travelcost
            new TranslationSeedItem("entity.qualityassuranceincoming.travelcost", "zh-CN", "交通费旅费", "交通费、旅费(元)"),
            // entity.qualityassuranceincoming.travelcost
            new TranslationSeedItem("entity.qualityassuranceincoming.travelcost", "zh-HK", "交通费旅费_hk", "交通费、旅费(元)"),

            // entity.qualityassuranceincoming.otherexpenses
            new TranslationSeedItem("entity.qualityassuranceincoming.otherexpenses", "en-US", "检查其他费用_us", "检查其他费用(元)"),
            // entity.qualityassuranceincoming.otherexpenses
            new TranslationSeedItem("entity.qualityassuranceincoming.otherexpenses", "ja-JP", "检查其他费用_jp", "检查其他费用(元)"),
            // entity.qualityassuranceincoming.otherexpenses
            new TranslationSeedItem("entity.qualityassuranceincoming.otherexpenses", "zh-CN", "检查其他费用", "检查其他费用(元)"),
            // entity.qualityassuranceincoming.otherexpenses
            new TranslationSeedItem("entity.qualityassuranceincoming.otherexpenses", "zh-HK", "检查其他费用_hk", "检查其他费用(元)"),

            // entity.qualityassuranceincoming.incomingnote
            new TranslationSeedItem("entity.qualityassuranceincoming.incomingnote", "en-US", "来料检验备注_us", "来料检验备注"),
            // entity.qualityassuranceincoming.incomingnote
            new TranslationSeedItem("entity.qualityassuranceincoming.incomingnote", "ja-JP", "来料检验备注_jp", "来料检验备注"),
            // entity.qualityassuranceincoming.incomingnote
            new TranslationSeedItem("entity.qualityassuranceincoming.incomingnote", "zh-CN", "来料检验备注", "来料检验备注"),
            // entity.qualityassuranceincoming.incomingnote
            new TranslationSeedItem("entity.qualityassuranceincoming.incomingnote", "zh-HK", "来料检验备注_hk", "来料检验备注"),

            // entity.qualityassuranceincoming.isobsolete
            new TranslationSeedItem("entity.qualityassuranceincoming.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityassuranceincoming.isobsolete
            new TranslationSeedItem("entity.qualityassuranceincoming.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityassuranceincoming.isobsolete
            new TranslationSeedItem("entity.qualityassuranceincoming.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityassuranceincoming.isobsolete
            new TranslationSeedItem("entity.qualityassuranceincoming.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.qualityassuranceincoming.operation
            new TranslationSeedItem("entity.qualityassuranceincoming.operation", "en-US", "品质业务主表_us", "品质业务主表(导航属性)"),
            // entity.qualityassuranceincoming.operation
            new TranslationSeedItem("entity.qualityassuranceincoming.operation", "ja-JP", "品质业务主表_jp", "品质业务主表(导航属性)"),
            // entity.qualityassuranceincoming.operation
            new TranslationSeedItem("entity.qualityassuranceincoming.operation", "zh-CN", "品质业务主表", "品质业务主表(导航属性)"),
            // entity.qualityassuranceincoming.operation
            new TranslationSeedItem("entity.qualityassuranceincoming.operation", "zh-HK", "品质业务主表_hk", "品质业务主表(导航属性)"),
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
