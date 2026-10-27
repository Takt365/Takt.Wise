// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueMeetingI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQualityIssueMeeting 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktQualityIssueMeeting 实体国际化翻译种子（键前缀 entity.qualityissuemeeting.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQualityIssueMeetingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQualityIssueMeeting 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 qualityissuemeeting 实体翻译...", tenantCode);

        foreach (var item in GetQualityIssueMeetingTranslations())
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

        TaktLogger.Information("TaktQualityIssueMeeting 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQualityIssueMeeting 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.qualityissuemeeting._self / entity.qualityissuemeeting.{{field}}；ResourceGroup=Cost；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQualityIssueMeetingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.qualityissuemeeting._self
            new TranslationSeedItem("entity.qualityissuemeeting._self", "en-US", "Quality Issue Meeting Information_us", "实体名称"),
            // entity.qualityissuemeeting._self
            new TranslationSeedItem("entity.qualityissuemeeting._self", "ja-JP", "品质问题应对明细 - 会议/调查/试验费用信息_jp", "实体名称"),
            // entity.qualityissuemeeting._self
            new TranslationSeedItem("entity.qualityissuemeeting._self", "zh-CN", "品质问题应对明细 - 会议/调查/试验费用信息", "实体名称"),
            // entity.qualityissuemeeting._self
            new TranslationSeedItem("entity.qualityissuemeeting._self", "zh-HK", "品质问题应对明细 - 会议/调查/试验费用信息_hk", "实体名称"),

            // entity.qualityissuemeeting.qualityissueid
            new TranslationSeedItem("entity.qualityissuemeeting.qualityissueid", "en-US", "品质问题主表ID_us", "品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）"),
            // entity.qualityissuemeeting.qualityissueid
            new TranslationSeedItem("entity.qualityissuemeeting.qualityissueid", "ja-JP", "品质问题主表ID_jp", "品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）"),
            // entity.qualityissuemeeting.qualityissueid
            new TranslationSeedItem("entity.qualityissuemeeting.qualityissueid", "zh-CN", "品质问题主表ID", "品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）"),
            // entity.qualityissuemeeting.qualityissueid
            new TranslationSeedItem("entity.qualityissuemeeting.qualityissueid", "zh-HK", "品质问题主表ID_hk", "品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）"),

            // entity.qualityissuemeeting.qualityissuecode
            new TranslationSeedItem("entity.qualityissuemeeting.qualityissuecode", "en-US", "品质问题编码_us", "品质问题编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.qualityissuemeeting.qualityissuecode
            new TranslationSeedItem("entity.qualityissuemeeting.qualityissuecode", "ja-JP", "品质问题编码_jp", "品质问题编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.qualityissuemeeting.qualityissuecode
            new TranslationSeedItem("entity.qualityissuemeeting.qualityissuecode", "zh-CN", "品质问题编码", "品质问题编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.qualityissuemeeting.qualityissuecode
            new TranslationSeedItem("entity.qualityissuemeeting.qualityissuecode", "zh-HK", "品质问题编码_hk", "品质问题编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.qualityissuemeeting.linenumber
            new TranslationSeedItem("entity.qualityissuemeeting.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.qualityissuemeeting.linenumber
            new TranslationSeedItem("entity.qualityissuemeeting.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.qualityissuemeeting.linenumber
            new TranslationSeedItem("entity.qualityissuemeeting.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.qualityissuemeeting.linenumber
            new TranslationSeedItem("entity.qualityissuemeeting.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.qualityissuemeeting.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityissuemeeting.directmanpowercostperminute", "en-US", "直接人员费率_us", "直接人员费率（元/分钟）"),
            // entity.qualityissuemeeting.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityissuemeeting.directmanpowercostperminute", "ja-JP", "直接人员费率_jp", "直接人员费率（元/分钟）"),
            // entity.qualityissuemeeting.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityissuemeeting.directmanpowercostperminute", "zh-CN", "直接人员费率", "直接人员费率（元/分钟）"),
            // entity.qualityissuemeeting.directmanpowercostperminute
            new TranslationSeedItem("entity.qualityissuemeeting.directmanpowercostperminute", "zh-HK", "直接人员费率_hk", "直接人员费率（元/分钟）"),

            // entity.qualityissuemeeting.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityissuemeeting.indirectmanpowercostperminute", "en-US", "间接人员费率_us", "间接人员费率（元/分钟）"),
            // entity.qualityissuemeeting.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityissuemeeting.indirectmanpowercostperminute", "ja-JP", "间接人员费率_jp", "间接人员费率（元/分钟）"),
            // entity.qualityissuemeeting.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityissuemeeting.indirectmanpowercostperminute", "zh-CN", "间接人员费率", "间接人员费率（元/分钟）"),
            // entity.qualityissuemeeting.indirectmanpowercostperminute
            new TranslationSeedItem("entity.qualityissuemeeting.indirectmanpowercostperminute", "zh-HK", "间接人员费率_hk", "间接人员费率（元/分钟）"),

            // entity.qualityissuemeeting.meetinginvestigationcontent
            new TranslationSeedItem("entity.qualityissuemeeting.meetinginvestigationcontent", "en-US", "讨论调查试验内容_us", "讨论调查试验内容(会议记录)"),
            // entity.qualityissuemeeting.meetinginvestigationcontent
            new TranslationSeedItem("entity.qualityissuemeeting.meetinginvestigationcontent", "ja-JP", "讨论调查试验内容_jp", "讨论调查试验内容(会议记录)"),
            // entity.qualityissuemeeting.meetinginvestigationcontent
            new TranslationSeedItem("entity.qualityissuemeeting.meetinginvestigationcontent", "zh-CN", "讨论调查试验内容", "讨论调查试验内容(会议记录)"),
            // entity.qualityissuemeeting.meetinginvestigationcontent
            new TranslationSeedItem("entity.qualityissuemeeting.meetinginvestigationcontent", "zh-HK", "讨论调查试验内容_hk", "讨论调查试验内容(会议记录)"),

            // entity.qualityissuemeeting.meetinginvestigationcost
            new TranslationSeedItem("entity.qualityissuemeeting.meetinginvestigationcost", "en-US", "讨论调查试验费用_us", "讨论调查试验费用(元)"),
            // entity.qualityissuemeeting.meetinginvestigationcost
            new TranslationSeedItem("entity.qualityissuemeeting.meetinginvestigationcost", "ja-JP", "讨论调查试验费用_jp", "讨论调查试验费用(元)"),
            // entity.qualityissuemeeting.meetinginvestigationcost
            new TranslationSeedItem("entity.qualityissuemeeting.meetinginvestigationcost", "zh-CN", "讨论调查试验费用", "讨论调查试验费用(元)"),
            // entity.qualityissuemeeting.meetinginvestigationcost
            new TranslationSeedItem("entity.qualityissuemeeting.meetinginvestigationcost", "zh-HK", "讨论调查试验费用_hk", "讨论调查试验费用(元)"),

            // entity.qualityissuemeeting.meetingtimeminutes
            new TranslationSeedItem("entity.qualityissuemeeting.meetingtimeminutes", "en-US", "检讨会使用时间_us", "讨论会使用时间(分钟)"),
            // entity.qualityissuemeeting.meetingtimeminutes
            new TranslationSeedItem("entity.qualityissuemeeting.meetingtimeminutes", "ja-JP", "检讨会使用时间_jp", "讨论会使用时间(分钟)"),
            // entity.qualityissuemeeting.meetingtimeminutes
            new TranslationSeedItem("entity.qualityissuemeeting.meetingtimeminutes", "zh-CN", "检讨会使用时间", "讨论会使用时间(分钟)"),
            // entity.qualityissuemeeting.meetingtimeminutes
            new TranslationSeedItem("entity.qualityissuemeeting.meetingtimeminutes", "zh-HK", "检讨会使用时间_hk", "讨论会使用时间(分钟)"),

            // entity.qualityissuemeeting.directparticipantcount
            new TranslationSeedItem("entity.qualityissuemeeting.directparticipantcount", "en-US", "直接人员参加人数_us", "直接人员参加人数"),
            // entity.qualityissuemeeting.directparticipantcount
            new TranslationSeedItem("entity.qualityissuemeeting.directparticipantcount", "ja-JP", "直接人员参加人数_jp", "直接人员参加人数"),
            // entity.qualityissuemeeting.directparticipantcount
            new TranslationSeedItem("entity.qualityissuemeeting.directparticipantcount", "zh-CN", "直接人员参加人数", "直接人员参加人数"),
            // entity.qualityissuemeeting.directparticipantcount
            new TranslationSeedItem("entity.qualityissuemeeting.directparticipantcount", "zh-HK", "直接人员参加人数_hk", "直接人员参加人数"),

            // entity.qualityissuemeeting.indirectparticipantcount
            new TranslationSeedItem("entity.qualityissuemeeting.indirectparticipantcount", "en-US", "间接人员参加人数_us", "间接人员参加人数"),
            // entity.qualityissuemeeting.indirectparticipantcount
            new TranslationSeedItem("entity.qualityissuemeeting.indirectparticipantcount", "ja-JP", "间接人员参加人数_jp", "间接人员参加人数"),
            // entity.qualityissuemeeting.indirectparticipantcount
            new TranslationSeedItem("entity.qualityissuemeeting.indirectparticipantcount", "zh-CN", "间接人员参加人数", "间接人员参加人数"),
            // entity.qualityissuemeeting.indirectparticipantcount
            new TranslationSeedItem("entity.qualityissuemeeting.indirectparticipantcount", "zh-HK", "间接人员参加人数_hk", "间接人员参加人数"),

            // entity.qualityissuemeeting.investigationworktimeminutes
            new TranslationSeedItem("entity.qualityissuemeeting.investigationworktimeminutes", "en-US", "调查评价试验工作时间_us", "调查评价试验工作时间（分钟）"),
            // entity.qualityissuemeeting.investigationworktimeminutes
            new TranslationSeedItem("entity.qualityissuemeeting.investigationworktimeminutes", "ja-JP", "调查评价试验工作时间_jp", "调查评价试验工作时间（分钟）"),
            // entity.qualityissuemeeting.investigationworktimeminutes
            new TranslationSeedItem("entity.qualityissuemeeting.investigationworktimeminutes", "zh-CN", "调查评价试验工作时间", "调查评价试验工作时间（分钟）"),
            // entity.qualityissuemeeting.investigationworktimeminutes
            new TranslationSeedItem("entity.qualityissuemeeting.investigationworktimeminutes", "zh-HK", "调查评价试验工作时间_hk", "调查评价试验工作时间（分钟）"),

            // entity.qualityissuemeeting.travelcost
            new TranslationSeedItem("entity.qualityissuemeeting.travelcost", "en-US", "交通费旅费_us", "交通费、旅费（元）"),
            // entity.qualityissuemeeting.travelcost
            new TranslationSeedItem("entity.qualityissuemeeting.travelcost", "ja-JP", "交通费旅费_jp", "交通费、旅费（元）"),
            // entity.qualityissuemeeting.travelcost
            new TranslationSeedItem("entity.qualityissuemeeting.travelcost", "zh-CN", "交通费旅费", "交通费、旅费（元）"),
            // entity.qualityissuemeeting.travelcost
            new TranslationSeedItem("entity.qualityissuemeeting.travelcost", "zh-HK", "交通费旅费_hk", "交通费、旅费（元）"),

            // entity.qualityissuemeeting.otherexpenses
            new TranslationSeedItem("entity.qualityissuemeeting.otherexpenses", "en-US", "其他费用_us", "其他费用（元）"),
            // entity.qualityissuemeeting.otherexpenses
            new TranslationSeedItem("entity.qualityissuemeeting.otherexpenses", "ja-JP", "其他费用_jp", "其他费用（元）"),
            // entity.qualityissuemeeting.otherexpenses
            new TranslationSeedItem("entity.qualityissuemeeting.otherexpenses", "zh-CN", "其他费用", "其他费用（元）"),
            // entity.qualityissuemeeting.otherexpenses
            new TranslationSeedItem("entity.qualityissuemeeting.otherexpenses", "zh-HK", "其他费用_hk", "其他费用（元）"),

            // entity.qualityissuemeeting.otherworktimeminutes
            new TranslationSeedItem("entity.qualityissuemeeting.otherworktimeminutes", "en-US", "其他作业时间_us", "其他作业時間（分钟）"),
            // entity.qualityissuemeeting.otherworktimeminutes
            new TranslationSeedItem("entity.qualityissuemeeting.otherworktimeminutes", "ja-JP", "其他作业时间_jp", "其他作业時間（分钟）"),
            // entity.qualityissuemeeting.otherworktimeminutes
            new TranslationSeedItem("entity.qualityissuemeeting.otherworktimeminutes", "zh-CN", "其他作业时间", "其他作业時間（分钟）"),
            // entity.qualityissuemeeting.otherworktimeminutes
            new TranslationSeedItem("entity.qualityissuemeeting.otherworktimeminutes", "zh-HK", "其他作业时间_hk", "其他作业時間（分钟）"),

            // entity.qualityissuemeeting.otherapparatuscost
            new TranslationSeedItem("entity.qualityissuemeeting.otherapparatuscost", "en-US", "其他设备工程搬运费_us", "其他设备购入费、工程费、搬运费等（元）"),
            // entity.qualityissuemeeting.otherapparatuscost
            new TranslationSeedItem("entity.qualityissuemeeting.otherapparatuscost", "ja-JP", "其他设备工程搬运费_jp", "其他设备购入费、工程费、搬运费等（元）"),
            // entity.qualityissuemeeting.otherapparatuscost
            new TranslationSeedItem("entity.qualityissuemeeting.otherapparatuscost", "zh-CN", "其他设备工程搬运费", "其他设备购入费、工程费、搬运费等（元）"),
            // entity.qualityissuemeeting.otherapparatuscost
            new TranslationSeedItem("entity.qualityissuemeeting.otherapparatuscost", "zh-HK", "其他设备工程搬运费_hk", "其他设备购入费、工程费、搬运费等（元）"),

            // entity.qualityissuemeeting.responsegroupcode
            new TranslationSeedItem("entity.qualityissuemeeting.responsegroupcode", "en-US", "对应组编码_us", "对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）"),
            // entity.qualityissuemeeting.responsegroupcode
            new TranslationSeedItem("entity.qualityissuemeeting.responsegroupcode", "ja-JP", "对应组编码_jp", "对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）"),
            // entity.qualityissuemeeting.responsegroupcode
            new TranslationSeedItem("entity.qualityissuemeeting.responsegroupcode", "zh-CN", "对应组编码", "对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）"),
            // entity.qualityissuemeeting.responsegroupcode
            new TranslationSeedItem("entity.qualityissuemeeting.responsegroupcode", "zh-HK", "对应组编码_hk", "对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）"),

            // entity.qualityissuemeeting.responsegroupname
            new TranslationSeedItem("entity.qualityissuemeeting.responsegroupname", "en-US", "对应组名称_us", "对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityissuemeeting.responsegroupname
            new TranslationSeedItem("entity.qualityissuemeeting.responsegroupname", "ja-JP", "对应组名称_jp", "对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityissuemeeting.responsegroupname
            new TranslationSeedItem("entity.qualityissuemeeting.responsegroupname", "zh-CN", "对应组名称", "对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）"),
            // entity.qualityissuemeeting.responsegroupname
            new TranslationSeedItem("entity.qualityissuemeeting.responsegroupname", "zh-HK", "对应组名称_hk", "对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）"),

            // entity.qualityissuemeeting.isobsolete
            new TranslationSeedItem("entity.qualityissuemeeting.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityissuemeeting.isobsolete
            new TranslationSeedItem("entity.qualityissuemeeting.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityissuemeeting.isobsolete
            new TranslationSeedItem("entity.qualityissuemeeting.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.qualityissuemeeting.isobsolete
            new TranslationSeedItem("entity.qualityissuemeeting.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.qualityissuemeeting.issue
            new TranslationSeedItem("entity.qualityissuemeeting.issue", "en-US", "质量问题主表_us", "质量问题主表（导航属性）"),
            // entity.qualityissuemeeting.issue
            new TranslationSeedItem("entity.qualityissuemeeting.issue", "ja-JP", "质量问题主表_jp", "质量问题主表（导航属性）"),
            // entity.qualityissuemeeting.issue
            new TranslationSeedItem("entity.qualityissuemeeting.issue", "zh-CN", "质量问题主表", "质量问题主表（导航属性）"),
            // entity.qualityissuemeeting.issue
            new TranslationSeedItem("entity.qualityissuemeeting.issue", "zh-HK", "质量问题主表_hk", "质量问题主表（导航属性）"),
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
