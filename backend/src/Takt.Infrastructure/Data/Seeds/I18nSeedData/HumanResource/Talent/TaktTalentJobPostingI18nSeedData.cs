// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Talent
// 文件名称：TaktTalentJobPostingI18nSeedData.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTalentJobPosting 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Talent;

/// <summary>
/// TaktTalentJobPosting 实体国际化翻译种子（键前缀 entity.talentjobposting.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTalentJobPostingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTalentJobPosting 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 talentjobposting 实体翻译...", tenantCode);

        foreach (var item in GetTalentJobPostingTranslations())
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

        TaktLogger.Information("TaktTalentJobPosting 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTalentJobPosting 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.talentjobposting._self / entity.talentjobposting.{{field}}；ResourceGroup=Talent；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTalentJobPostingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.talentjobposting._self
            new TranslationSeedItem("entity.talentjobposting._self", "en-US", "Talent Job Posting Information_us", "实体名称"),
            // entity.talentjobposting._self
            new TranslationSeedItem("entity.talentjobposting._self", "ja-JP", "职位发布信息_jp", "实体名称"),
            // entity.talentjobposting._self
            new TranslationSeedItem("entity.talentjobposting._self", "zh-CN", "职位发布信息", "实体名称"),
            // entity.talentjobposting._self
            new TranslationSeedItem("entity.talentjobposting._self", "zh-HK", "职位发布信息_hk", "实体名称"),

            // entity.talentjobposting.staffingrequirementid
            new TranslationSeedItem("entity.talentjobposting.staffingrequirementid", "en-US", "用人需求ID_us", "用人需求（选项 TaktTalentStaffingRequirements/options；DictValue=Id）"),
            // entity.talentjobposting.staffingrequirementid
            new TranslationSeedItem("entity.talentjobposting.staffingrequirementid", "ja-JP", "用人需求ID_jp", "用人需求（选项 TaktTalentStaffingRequirements/options；DictValue=Id）"),
            // entity.talentjobposting.staffingrequirementid
            new TranslationSeedItem("entity.talentjobposting.staffingrequirementid", "zh-CN", "用人需求ID", "用人需求（选项 TaktTalentStaffingRequirements/options；DictValue=Id）"),
            // entity.talentjobposting.staffingrequirementid
            new TranslationSeedItem("entity.talentjobposting.staffingrequirementid", "zh-HK", "用人需求ID_hk", "用人需求（选项 TaktTalentStaffingRequirements/options；DictValue=Id）"),

            // entity.talentjobposting.postingcode
            new TranslationSeedItem("entity.talentjobposting.postingcode", "en-US", "发布编码_us", "发布编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 发布编码规则生成并展示，非手输；单据类型菜单：职位发布）"),
            // entity.talentjobposting.postingcode
            new TranslationSeedItem("entity.talentjobposting.postingcode", "ja-JP", "发布编码_jp", "发布编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 发布编码规则生成并展示，非手输；单据类型菜单：职位发布）"),
            // entity.talentjobposting.postingcode
            new TranslationSeedItem("entity.talentjobposting.postingcode", "zh-CN", "发布编码", "发布编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 发布编码规则生成并展示，非手输；单据类型菜单：职位发布）"),
            // entity.talentjobposting.postingcode
            new TranslationSeedItem("entity.talentjobposting.postingcode", "zh-HK", "发布编码_hk", "发布编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 发布编码规则生成并展示，非手输；单据类型菜单：职位发布）"),

            // entity.talentjobposting.title
            new TranslationSeedItem("entity.talentjobposting.title", "en-US", "职位标题_us", "职位标题"),
            // entity.talentjobposting.title
            new TranslationSeedItem("entity.talentjobposting.title", "ja-JP", "职位标题_jp", "职位标题"),
            // entity.talentjobposting.title
            new TranslationSeedItem("entity.talentjobposting.title", "zh-CN", "职位标题", "职位标题"),
            // entity.talentjobposting.title
            new TranslationSeedItem("entity.talentjobposting.title", "zh-HK", "职位标题_hk", "职位标题"),

            // entity.talentjobposting.publishdate
            new TranslationSeedItem("entity.talentjobposting.publishdate", "en-US", "职位发布日期_us", "职位发布日期"),
            // entity.talentjobposting.publishdate
            new TranslationSeedItem("entity.talentjobposting.publishdate", "ja-JP", "职位发布日期_jp", "职位发布日期"),
            // entity.talentjobposting.publishdate
            new TranslationSeedItem("entity.talentjobposting.publishdate", "zh-CN", "职位发布日期", "职位发布日期"),
            // entity.talentjobposting.publishdate
            new TranslationSeedItem("entity.talentjobposting.publishdate", "zh-HK", "职位发布日期_hk", "职位发布日期"),

            // entity.talentjobposting.opendate
            new TranslationSeedItem("entity.talentjobposting.opendate", "en-US", "招聘开放日期_us", "招聘开放日期"),
            // entity.talentjobposting.opendate
            new TranslationSeedItem("entity.talentjobposting.opendate", "ja-JP", "招聘开放日期_jp", "招聘开放日期"),
            // entity.talentjobposting.opendate
            new TranslationSeedItem("entity.talentjobposting.opendate", "zh-CN", "招聘开放日期", "招聘开放日期"),
            // entity.talentjobposting.opendate
            new TranslationSeedItem("entity.talentjobposting.opendate", "zh-HK", "招聘开放日期_hk", "招聘开放日期"),

            // entity.talentjobposting.closedate
            new TranslationSeedItem("entity.talentjobposting.closedate", "en-US", "招聘关闭日期_us", "招聘关闭日期"),
            // entity.talentjobposting.closedate
            new TranslationSeedItem("entity.talentjobposting.closedate", "ja-JP", "招聘关闭日期_jp", "招聘关闭日期"),
            // entity.talentjobposting.closedate
            new TranslationSeedItem("entity.talentjobposting.closedate", "zh-CN", "招聘关闭日期", "招聘关闭日期"),
            // entity.talentjobposting.closedate
            new TranslationSeedItem("entity.talentjobposting.closedate", "zh-HK", "招聘关闭日期_hk", "招聘关闭日期"),

            // entity.talentjobposting.publishchannel
            new TranslationSeedItem("entity.talentjobposting.publishchannel", "en-US", "发布渠道_us", "发布渠道（字典 humanresource_talent_publish_channel；0=官网 1=招聘网站 2=内推 3=校园 9=其他）"),
            // entity.talentjobposting.publishchannel
            new TranslationSeedItem("entity.talentjobposting.publishchannel", "ja-JP", "发布渠道_jp", "发布渠道（字典 humanresource_talent_publish_channel；0=官网 1=招聘网站 2=内推 3=校园 9=其他）"),
            // entity.talentjobposting.publishchannel
            new TranslationSeedItem("entity.talentjobposting.publishchannel", "zh-CN", "发布渠道", "发布渠道（字典 humanresource_talent_publish_channel；0=官网 1=招聘网站 2=内推 3=校园 9=其他）"),
            // entity.talentjobposting.publishchannel
            new TranslationSeedItem("entity.talentjobposting.publishchannel", "zh-HK", "发布渠道_hk", "发布渠道（字典 humanresource_talent_publish_channel；0=官网 1=招聘网站 2=内推 3=校园 9=其他）"),

            // entity.talentjobposting.reason
            new TranslationSeedItem("entity.talentjobposting.reason", "en-US", "发布说明_us", "发布说明"),
            // entity.talentjobposting.reason
            new TranslationSeedItem("entity.talentjobposting.reason", "ja-JP", "发布说明_jp", "发布说明"),
            // entity.talentjobposting.reason
            new TranslationSeedItem("entity.talentjobposting.reason", "zh-CN", "发布说明", "发布说明"),
            // entity.talentjobposting.reason
            new TranslationSeedItem("entity.talentjobposting.reason", "zh-HK", "发布说明_hk", "发布说明"),

            // entity.talentjobposting.postingstatus
            new TranslationSeedItem("entity.talentjobposting.postingstatus", "en-US", "发布状态_us", "发布状态（字典 humanresource_talent_job_posting_status；0=草稿 1=招聘中 2=已暂停 3=已关闭）"),
            // entity.talentjobposting.postingstatus
            new TranslationSeedItem("entity.talentjobposting.postingstatus", "ja-JP", "发布状态_jp", "发布状态（字典 humanresource_talent_job_posting_status；0=草稿 1=招聘中 2=已暂停 3=已关闭）"),
            // entity.talentjobposting.postingstatus
            new TranslationSeedItem("entity.talentjobposting.postingstatus", "zh-CN", "发布状态", "发布状态（字典 humanresource_talent_job_posting_status；0=草稿 1=招聘中 2=已暂停 3=已关闭）"),
            // entity.talentjobposting.postingstatus
            new TranslationSeedItem("entity.talentjobposting.postingstatus", "zh-HK", "发布状态_hk", "发布状态（字典 humanresource_talent_job_posting_status；0=草稿 1=招聘中 2=已暂停 3=已关闭）"),

            // entity.talentjobposting.staffingrequirement
            new TranslationSeedItem("entity.talentjobposting.staffingrequirement", "en-US", "用人需求_us", "用人需求"),
            // entity.talentjobposting.staffingrequirement
            new TranslationSeedItem("entity.talentjobposting.staffingrequirement", "ja-JP", "用人需求_jp", "用人需求"),
            // entity.talentjobposting.staffingrequirement
            new TranslationSeedItem("entity.talentjobposting.staffingrequirement", "zh-CN", "用人需求", "用人需求"),
            // entity.talentjobposting.staffingrequirement
            new TranslationSeedItem("entity.talentjobposting.staffingrequirement", "zh-HK", "用人需求_hk", "用人需求"),

            // entity.talentjobposting.talentoffers
            new TranslationSeedItem("entity.talentjobposting.talentoffers", "en-US", "录用信息_us", "录用信息"),
            // entity.talentjobposting.talentoffers
            new TranslationSeedItem("entity.talentjobposting.talentoffers", "ja-JP", "录用信息_jp", "录用信息"),
            // entity.talentjobposting.talentoffers
            new TranslationSeedItem("entity.talentjobposting.talentoffers", "zh-CN", "录用信息", "录用信息"),
            // entity.talentjobposting.talentoffers
            new TranslationSeedItem("entity.talentjobposting.talentoffers", "zh-HK", "录用信息_hk", "录用信息"),
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
        translation.ResourceGroup = "Talent";
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
