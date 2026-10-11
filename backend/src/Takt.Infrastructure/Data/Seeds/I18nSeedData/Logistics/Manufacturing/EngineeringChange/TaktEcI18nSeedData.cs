// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcI18nSeedData.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEc 实体字段国际化种子（已对齐前端 locales：src/locales/logistics/manufacturing/engineering-change/ec）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktEc 实体国际化翻译种子（键前缀 entity.ec.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEc 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ec 实体翻译...", tenantCode);

        foreach (var item in GetEcTranslations())
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

        TaktLogger.Information("TaktEc 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEc 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ec._self / entity.ec.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ec._self
            new TranslationSeedItem("entity.ec._self", "en-US", "Ec Information_us", "实体名称"),
            // entity.ec._self
            new TranslationSeedItem("entity.ec._self", "ja-JP", "设变信息_jp", "实体名称"),
            // entity.ec._self
            new TranslationSeedItem("entity.ec._self", "zh-CN", "设变信息", "实体名称"),
            // entity.ec._self
            new TranslationSeedItem("entity.ec._self", "zh-HK", "设变信息_hk", "实体名称"),

            // entity.ec.plantcode
            new TranslationSeedItem("entity.ec.plantcode", "en-US", "工厂代码_us", "工厂代码"),
            // entity.ec.plantcode
            new TranslationSeedItem("entity.ec.plantcode", "ja-JP", "工厂代码_jp", "工厂代码"),
            // entity.ec.plantcode
            new TranslationSeedItem("entity.ec.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.ec.plantcode
            new TranslationSeedItem("entity.ec.plantcode", "zh-HK", "工厂代码_hk", "工厂代码"),

            // entity.ec.no
            new TranslationSeedItem("entity.ec.no", "en-US", "设变单号_us", "设变单号（唯一）"),
            // entity.ec.no
            new TranslationSeedItem("entity.ec.no", "ja-JP", "设变单号_jp", "设变单号（唯一）"),
            // entity.ec.no
            new TranslationSeedItem("entity.ec.no", "zh-CN", "设变单号", "设变单号（唯一）"),
            // entity.ec.no
            new TranslationSeedItem("entity.ec.no", "zh-HK", "设变单号_hk", "设变单号（唯一）"),

            // entity.ec.issuedate
            new TranslationSeedItem("entity.ec.issuedate", "en-US", "发行日期_us", "发行日期"),
            // entity.ec.issuedate
            new TranslationSeedItem("entity.ec.issuedate", "ja-JP", "发行日期_jp", "发行日期"),
            // entity.ec.issuedate
            new TranslationSeedItem("entity.ec.issuedate", "zh-CN", "发行日期", "发行日期"),
            // entity.ec.issuedate
            new TranslationSeedItem("entity.ec.issuedate", "zh-HK", "发行日期_hk", "发行日期"),

            // entity.ec.title
            new TranslationSeedItem("entity.ec.title", "en-US", "EC Title", "设变标题"),
            // entity.ec.title
            new TranslationSeedItem("entity.ec.title", "ja-JP", "設変タイトル", "设变标题"),
            // entity.ec.title
            new TranslationSeedItem("entity.ec.title", "zh-CN", "设变标题", "设变标题"),
            // entity.ec.title
            new TranslationSeedItem("entity.ec.title", "zh-HK", "設變標題", "设变标题"),

            // entity.ec.content
            new TranslationSeedItem("entity.ec.content", "en-US", "设变内容_us", "设变内容"),
            // entity.ec.content
            new TranslationSeedItem("entity.ec.content", "ja-JP", "设变内容_jp", "设变内容"),
            // entity.ec.content
            new TranslationSeedItem("entity.ec.content", "zh-CN", "设变内容", "设变内容"),
            // entity.ec.content
            new TranslationSeedItem("entity.ec.content", "zh-HK", "设变内容_hk", "设变内容"),

            // entity.ec.leader
            new TranslationSeedItem("entity.ec.leader", "en-US", "负责人_us", "负责人（关联 TaktEmployee.Id，选项 TaktEmployees/options）"),
            // entity.ec.leader
            new TranslationSeedItem("entity.ec.leader", "ja-JP", "负责人_jp", "负责人（关联 TaktEmployee.Id，选项 TaktEmployees/options）"),
            // entity.ec.leader
            new TranslationSeedItem("entity.ec.leader", "zh-CN", "负责人", "负责人（关联 TaktEmployee.Id，选项 TaktEmployees/options）"),
            // entity.ec.leader
            new TranslationSeedItem("entity.ec.leader", "zh-HK", "负责人_hk", "负责人（关联 TaktEmployee.Id，选项 TaktEmployees/options）"),

            // entity.ec.lossamount
            new TranslationSeedItem("entity.ec.lossamount", "en-US", "损失金额_us", "损失金额"),
            // entity.ec.lossamount
            new TranslationSeedItem("entity.ec.lossamount", "ja-JP", "损失金额_jp", "损失金额"),
            // entity.ec.lossamount
            new TranslationSeedItem("entity.ec.lossamount", "zh-CN", "损失金额", "损失金额"),
            // entity.ec.lossamount
            new TranslationSeedItem("entity.ec.lossamount", "zh-HK", "损失金额_hk", "损失金额"),

            // entity.ec.scope
            new TranslationSeedItem("entity.ec.scope", "en-US", "Scope", "实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）"),
            // entity.ec.scope
            new TranslationSeedItem("entity.ec.scope", "ja-JP", "実施範囲", "实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）"),
            // entity.ec.scope
            new TranslationSeedItem("entity.ec.scope", "zh-CN", "实施范围", "实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）"),
            // entity.ec.scope
            new TranslationSeedItem("entity.ec.scope", "zh-HK", "實施範圍", "实施范围（字典 logistics_manufacturing_ec_scope_category；1=全仕向，2=部管，3=内部，4=技术）"),

            // entity.ec.effectivedate
            new TranslationSeedItem("entity.ec.effectivedate", "en-US", "生效日期_us", "生效日期"),
            // entity.ec.effectivedate
            new TranslationSeedItem("entity.ec.effectivedate", "ja-JP", "生效日期_jp", "生效日期"),
            // entity.ec.effectivedate
            new TranslationSeedItem("entity.ec.effectivedate", "zh-CN", "生效日期", "生效日期"),
            // entity.ec.effectivedate
            new TranslationSeedItem("entity.ec.effectivedate", "zh-HK", "生效日期_hk", "生效日期"),

            // entity.ec.entrydate
            new TranslationSeedItem("entity.ec.entrydate", "en-US", "录入日期_us", "录入日期"),
            // entity.ec.entrydate
            new TranslationSeedItem("entity.ec.entrydate", "ja-JP", "录入日期_jp", "录入日期"),
            // entity.ec.entrydate
            new TranslationSeedItem("entity.ec.entrydate", "zh-CN", "录入日期", "录入日期"),
            // entity.ec.entrydate
            new TranslationSeedItem("entity.ec.entrydate", "zh-HK", "录入日期_hk", "录入日期"),

            // entity.ec.changestatus
            new TranslationSeedItem("entity.ec.changestatus", "en-US", "变更状态_us", "变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)"),
            // entity.ec.changestatus
            new TranslationSeedItem("entity.ec.changestatus", "ja-JP", "变更状态_jp", "变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)"),
            // entity.ec.changestatus
            new TranslationSeedItem("entity.ec.changestatus", "zh-CN", "变更状态", "变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)"),
            // entity.ec.changestatus
            new TranslationSeedItem("entity.ec.changestatus", "zh-HK", "变更状态_hk", "变更状态(1=工作的 2=取消的 3=发行的 4=P.P中变更的 5=固定的 6=挂起的 7=拒绝的)"),

            // entity.ec.status
            new TranslationSeedItem("entity.ec.status", "en-US", "设变状态_us", "设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）"),
            // entity.ec.status
            new TranslationSeedItem("entity.ec.status", "ja-JP", "设变状态_jp", "设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）"),
            // entity.ec.status
            new TranslationSeedItem("entity.ec.status", "zh-CN", "设变状态", "设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）"),
            // entity.ec.status
            new TranslationSeedItem("entity.ec.status", "zh-HK", "设变状态_hk", "设变状态（字典 logistics_manufacturing_ec_gijutsu_status；1=发行，2=执行中，3=完成）"),

            // entity.ec.details
            new TranslationSeedItem("entity.ec.details", "en-US", "设变明细列表_us", "设变明细列表"),
            // entity.ec.details
            new TranslationSeedItem("entity.ec.details", "ja-JP", "设变明细列表_jp", "设变明细列表"),
            // entity.ec.details
            new TranslationSeedItem("entity.ec.details", "zh-CN", "设变明细列表", "设变明细列表"),
            // entity.ec.details
            new TranslationSeedItem("entity.ec.details", "zh-HK", "设变明细列表_hk", "设变明细列表"),

            // entity.ec.attachments
            new TranslationSeedItem("entity.ec.attachments", "en-US", "设变附件列表_us", "设变附件列表（一个设变可对应多个附件）"),
            // entity.ec.attachments
            new TranslationSeedItem("entity.ec.attachments", "ja-JP", "设变附件列表_jp", "设变附件列表（一个设变可对应多个附件）"),
            // entity.ec.attachments
            new TranslationSeedItem("entity.ec.attachments", "zh-CN", "设变附件列表", "设变附件列表（一个设变可对应多个附件）"),
            // entity.ec.attachments
            new TranslationSeedItem("entity.ec.attachments", "zh-HK", "设变附件列表_hk", "设变附件列表（一个设变可对应多个附件）"),
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
        translation.ResourceGroup = "EngineeringChange";
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
