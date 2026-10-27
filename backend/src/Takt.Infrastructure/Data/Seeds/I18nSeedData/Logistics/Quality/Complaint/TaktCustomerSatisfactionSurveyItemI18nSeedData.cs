// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyItemI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerSatisfactionSurveyItem 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint;

/// <summary>
/// TaktCustomerSatisfactionSurveyItem 实体国际化翻译种子（键前缀 entity.customersatisfactionsurveyitem.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerSatisfactionSurveyItemI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerSatisfactionSurveyItem 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customersatisfactionsurveyitem 实体翻译...", tenantCode);

        foreach (var item in GetCustomerSatisfactionSurveyItemTranslations())
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

        TaktLogger.Information("TaktCustomerSatisfactionSurveyItem 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerSatisfactionSurveyItem 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customersatisfactionsurveyitem._self / entity.customersatisfactionsurveyitem.{{field}}；ResourceGroup=Complaint；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerSatisfactionSurveyItemTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customersatisfactionsurveyitem._self
            new TranslationSeedItem("entity.customersatisfactionsurveyitem._self", "en-US", "Customer Satisfaction Survey Item Information_us", "实体名称"),
            // entity.customersatisfactionsurveyitem._self
            new TranslationSeedItem("entity.customersatisfactionsurveyitem._self", "ja-JP", "客户满意度调查项目明细信息_jp", "实体名称"),
            // entity.customersatisfactionsurveyitem._self
            new TranslationSeedItem("entity.customersatisfactionsurveyitem._self", "zh-CN", "客户满意度调查项目明细信息", "实体名称"),
            // entity.customersatisfactionsurveyitem._self
            new TranslationSeedItem("entity.customersatisfactionsurveyitem._self", "zh-HK", "客户满意度调查项目明细信息_hk", "实体名称"),

            // entity.customersatisfactionsurveyitem.surveyid
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.surveyid", "en-US", "调查表ID_us", "调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）"),
            // entity.customersatisfactionsurveyitem.surveyid
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.surveyid", "ja-JP", "调查表ID_jp", "调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）"),
            // entity.customersatisfactionsurveyitem.surveyid
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.surveyid", "zh-CN", "调查表ID", "调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）"),
            // entity.customersatisfactionsurveyitem.surveyid
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.surveyid", "zh-HK", "调查表ID_hk", "调查表 ID（选项 TaktCustomerSatisfactionSurveys/options；DictValue=Id）"),

            // entity.customersatisfactionsurveyitem.customersatisfactionsurveycode
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.customersatisfactionsurveycode", "en-US", "调查表编码_us", "调查表编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customersatisfactionsurveyitem.customersatisfactionsurveycode
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.customersatisfactionsurveycode", "ja-JP", "调查表编码_jp", "调查表编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customersatisfactionsurveyitem.customersatisfactionsurveycode
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.customersatisfactionsurveycode", "zh-CN", "调查表编码", "调查表编码（冗余：按对应 Id 取主数据名称联动）"),
            // entity.customersatisfactionsurveyitem.customersatisfactionsurveycode
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.customersatisfactionsurveycode", "zh-HK", "调查表编码_hk", "调查表编码（冗余：按对应 Id 取主数据名称联动）"),

            // entity.customersatisfactionsurveyitem.linenumber
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.customersatisfactionsurveyitem.linenumber
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.customersatisfactionsurveyitem.linenumber
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.customersatisfactionsurveyitem.linenumber
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.customersatisfactionsurveyitem.categorytype
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.categorytype", "en-US", "调查类别_us", "调查类别类型（字典 logistics_quality_satisfaction_category）"),
            // entity.customersatisfactionsurveyitem.categorytype
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.categorytype", "ja-JP", "调查类别_jp", "调查类别类型（字典 logistics_quality_satisfaction_category）"),
            // entity.customersatisfactionsurveyitem.categorytype
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.categorytype", "zh-CN", "调查类别", "调查类别类型（字典 logistics_quality_satisfaction_category）"),
            // entity.customersatisfactionsurveyitem.categorytype
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.categorytype", "zh-HK", "调查类别_hk", "调查类别类型（字典 logistics_quality_satisfaction_category）"),

            // entity.customersatisfactionsurveyitem.itemname
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.itemname", "en-US", "调查项目_us", "调查项目名称"),
            // entity.customersatisfactionsurveyitem.itemname
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.itemname", "ja-JP", "调查项目_jp", "调查项目名称"),
            // entity.customersatisfactionsurveyitem.itemname
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.itemname", "zh-CN", "调查项目", "调查项目名称"),
            // entity.customersatisfactionsurveyitem.itemname
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.itemname", "zh-HK", "调查项目_hk", "调查项目名称"),

            // entity.customersatisfactionsurveyitem.itemdescription
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.itemdescription", "en-US", "项目说明_us", "调查项目说明"),
            // entity.customersatisfactionsurveyitem.itemdescription
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.itemdescription", "ja-JP", "项目说明_jp", "调查项目说明"),
            // entity.customersatisfactionsurveyitem.itemdescription
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.itemdescription", "zh-CN", "项目说明", "调查项目说明"),
            // entity.customersatisfactionsurveyitem.itemdescription
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.itemdescription", "zh-HK", "项目说明_hk", "调查项目说明"),

            // entity.customersatisfactionsurveyitem.weight
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.weight", "en-US", "权重_us", "权重（%）"),
            // entity.customersatisfactionsurveyitem.weight
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.weight", "ja-JP", "权重_jp", "权重（%）"),
            // entity.customersatisfactionsurveyitem.weight
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.weight", "zh-CN", "权重", "权重（%）"),
            // entity.customersatisfactionsurveyitem.weight
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.weight", "zh-HK", "权重_hk", "权重（%）"),

            // entity.customersatisfactionsurveyitem.score
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.score", "en-US", "评分_us", "评分（0-100分）"),
            // entity.customersatisfactionsurveyitem.score
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.score", "ja-JP", "评分_jp", "评分（0-100分）"),
            // entity.customersatisfactionsurveyitem.score
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.score", "zh-CN", "评分", "评分（0-100分）"),
            // entity.customersatisfactionsurveyitem.score
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.score", "zh-HK", "评分_hk", "评分（0-100分）"),

            // entity.customersatisfactionsurveyitem.satisfactionlevel
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.satisfactionlevel", "en-US", "满意度等级_us", "满意度等级（字典 logistics_quality_satisfaction_level）"),
            // entity.customersatisfactionsurveyitem.satisfactionlevel
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.satisfactionlevel", "ja-JP", "满意度等级_jp", "满意度等级（字典 logistics_quality_satisfaction_level）"),
            // entity.customersatisfactionsurveyitem.satisfactionlevel
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.satisfactionlevel", "zh-CN", "满意度等级", "满意度等级（字典 logistics_quality_satisfaction_level）"),
            // entity.customersatisfactionsurveyitem.satisfactionlevel
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.satisfactionlevel", "zh-HK", "满意度等级_hk", "满意度等级（字典 logistics_quality_satisfaction_level）"),

            // entity.customersatisfactionsurveyitem.customerfeedback
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.customerfeedback", "en-US", "客户反馈_us", "客户反馈/意见"),
            // entity.customersatisfactionsurveyitem.customerfeedback
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.customerfeedback", "ja-JP", "客户反馈_jp", "客户反馈/意见"),
            // entity.customersatisfactionsurveyitem.customerfeedback
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.customerfeedback", "zh-CN", "客户反馈", "客户反馈/意见"),
            // entity.customersatisfactionsurveyitem.customerfeedback
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.customerfeedback", "zh-HK", "客户反馈_hk", "客户反馈/意见"),

            // entity.customersatisfactionsurveyitem.improvementsuggestion
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.improvementsuggestion", "en-US", "改进建议_us", "改进建议"),
            // entity.customersatisfactionsurveyitem.improvementsuggestion
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.improvementsuggestion", "ja-JP", "改进建议_jp", "改进建议"),
            // entity.customersatisfactionsurveyitem.improvementsuggestion
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.improvementsuggestion", "zh-CN", "改进建议", "改进建议"),
            // entity.customersatisfactionsurveyitem.improvementsuggestion
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.improvementsuggestion", "zh-HK", "改进建议_hk", "改进建议"),

            // entity.customersatisfactionsurveyitem.followupaction
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.followupaction", "en-US", "跟进措施_us", "跟进措施"),
            // entity.customersatisfactionsurveyitem.followupaction
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.followupaction", "ja-JP", "跟进措施_jp", "跟进措施"),
            // entity.customersatisfactionsurveyitem.followupaction
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.followupaction", "zh-CN", "跟进措施", "跟进措施"),
            // entity.customersatisfactionsurveyitem.followupaction
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.followupaction", "zh-HK", "跟进措施_hk", "跟进措施"),

            // entity.customersatisfactionsurveyitem.followupstatus
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.followupstatus", "en-US", "跟进状态_us", "跟进状态（字典 logistics_quality_follow_up_status）"),
            // entity.customersatisfactionsurveyitem.followupstatus
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.followupstatus", "ja-JP", "跟进状态_jp", "跟进状态（字典 logistics_quality_follow_up_status）"),
            // entity.customersatisfactionsurveyitem.followupstatus
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.followupstatus", "zh-CN", "跟进状态", "跟进状态（字典 logistics_quality_follow_up_status）"),
            // entity.customersatisfactionsurveyitem.followupstatus
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.followupstatus", "zh-HK", "跟进状态_hk", "跟进状态（字典 logistics_quality_follow_up_status）"),

            // entity.customersatisfactionsurveyitem.isobsolete
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.customersatisfactionsurveyitem.isobsolete
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.customersatisfactionsurveyitem.isobsolete
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.customersatisfactionsurveyitem.isobsolete
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.customersatisfactionsurveyitem.survey
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.survey", "en-US", "调查表主表_us", "调查表主表"),
            // entity.customersatisfactionsurveyitem.survey
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.survey", "ja-JP", "调查表主表_jp", "调查表主表"),
            // entity.customersatisfactionsurveyitem.survey
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.survey", "zh-CN", "调查表主表", "调查表主表"),
            // entity.customersatisfactionsurveyitem.survey
            new TranslationSeedItem("entity.customersatisfactionsurveyitem.survey", "zh-HK", "调查表主表_hk", "调查表主表"),
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
        translation.ResourceGroup = "Complaint";
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
