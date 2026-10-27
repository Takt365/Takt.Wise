// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.VisitorCenter
// 文件名称：TaktVisitorCompanionI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktVisitorCompanion 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.VisitorCenter;

/// <summary>
/// TaktVisitorCompanion 实体国际化翻译种子（键前缀 entity.visitorcompanion.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktVisitorCompanionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktVisitorCompanion 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 visitorcompanion 实体翻译...", tenantCode);

        foreach (var item in GetVisitorCompanionTranslations())
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

        TaktLogger.Information("TaktVisitorCompanion 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktVisitorCompanion 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.visitorcompanion._self / entity.visitorcompanion.{{field}}；ResourceGroup=VisitorCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetVisitorCompanionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.visitorcompanion._self
            new TranslationSeedItem("entity.visitorcompanion._self", "en-US", "Visitor Companion Information_us", "实体名称"),
            // entity.visitorcompanion._self
            new TranslationSeedItem("entity.visitorcompanion._self", "ja-JP", "来访人员子信息_jp", "实体名称"),
            // entity.visitorcompanion._self
            new TranslationSeedItem("entity.visitorcompanion._self", "zh-CN", "来访人员子信息", "实体名称"),
            // entity.visitorcompanion._self
            new TranslationSeedItem("entity.visitorcompanion._self", "zh-HK", "来访人员子信息_hk", "实体名称"),

            // entity.visitorcompanion.visitorid
            new TranslationSeedItem("entity.visitorcompanion.visitorid", "en-US", "来访记录ID_us", "来访记录 ID（选项 TaktVisitors/options；DictValue=Id）"),
            // entity.visitorcompanion.visitorid
            new TranslationSeedItem("entity.visitorcompanion.visitorid", "ja-JP", "来访记录ID_jp", "来访记录 ID（选项 TaktVisitors/options；DictValue=Id）"),
            // entity.visitorcompanion.visitorid
            new TranslationSeedItem("entity.visitorcompanion.visitorid", "zh-CN", "来访记录ID", "来访记录 ID（选项 TaktVisitors/options；DictValue=Id）"),
            // entity.visitorcompanion.visitorid
            new TranslationSeedItem("entity.visitorcompanion.visitorid", "zh-HK", "来访记录ID_hk", "来访记录 ID（选项 TaktVisitors/options；DictValue=Id）"),

            // entity.visitorcompanion.linenumber
            new TranslationSeedItem("entity.visitorcompanion.linenumber", "en-US", "行号_us", "行号（固定步长=10）"),
            // entity.visitorcompanion.linenumber
            new TranslationSeedItem("entity.visitorcompanion.linenumber", "ja-JP", "行号_jp", "行号（固定步长=10）"),
            // entity.visitorcompanion.linenumber
            new TranslationSeedItem("entity.visitorcompanion.linenumber", "zh-CN", "行号", "行号（固定步长=10）"),
            // entity.visitorcompanion.linenumber
            new TranslationSeedItem("entity.visitorcompanion.linenumber", "zh-HK", "行号_hk", "行号（固定步长=10）"),

            // entity.visitorcompanion.department
            new TranslationSeedItem("entity.visitorcompanion.department", "en-US", "部门_us", "部门"),
            // entity.visitorcompanion.department
            new TranslationSeedItem("entity.visitorcompanion.department", "ja-JP", "部门_jp", "部门"),
            // entity.visitorcompanion.department
            new TranslationSeedItem("entity.visitorcompanion.department", "zh-CN", "部门", "部门"),
            // entity.visitorcompanion.department
            new TranslationSeedItem("entity.visitorcompanion.department", "zh-HK", "部门_hk", "部门"),

            // entity.visitorcompanion.jobtitle
            new TranslationSeedItem("entity.visitorcompanion.jobtitle", "en-US", "职称_us", "职称"),
            // entity.visitorcompanion.jobtitle
            new TranslationSeedItem("entity.visitorcompanion.jobtitle", "ja-JP", "职称_jp", "职称"),
            // entity.visitorcompanion.jobtitle
            new TranslationSeedItem("entity.visitorcompanion.jobtitle", "zh-CN", "职称", "职称"),
            // entity.visitorcompanion.jobtitle
            new TranslationSeedItem("entity.visitorcompanion.jobtitle", "zh-HK", "职称_hk", "职称"),

            // entity.visitorcompanion.companionname
            new TranslationSeedItem("entity.visitorcompanion.companionname", "en-US", "来访人员姓名_us", "来访人员姓名"),
            // entity.visitorcompanion.companionname
            new TranslationSeedItem("entity.visitorcompanion.companionname", "ja-JP", "来访人员姓名_jp", "来访人员姓名"),
            // entity.visitorcompanion.companionname
            new TranslationSeedItem("entity.visitorcompanion.companionname", "zh-CN", "来访人员姓名", "来访人员姓名"),
            // entity.visitorcompanion.companionname
            new TranslationSeedItem("entity.visitorcompanion.companionname", "zh-HK", "来访人员姓名_hk", "来访人员姓名"),

            // entity.visitorcompanion.isobsolete
            new TranslationSeedItem("entity.visitorcompanion.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.visitorcompanion.isobsolete
            new TranslationSeedItem("entity.visitorcompanion.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.visitorcompanion.isobsolete
            new TranslationSeedItem("entity.visitorcompanion.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.visitorcompanion.isobsolete
            new TranslationSeedItem("entity.visitorcompanion.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.visitorcompanion.visitor
            new TranslationSeedItem("entity.visitorcompanion.visitor", "en-US", "来访记录_us", "来访记录（主表）"),
            // entity.visitorcompanion.visitor
            new TranslationSeedItem("entity.visitorcompanion.visitor", "ja-JP", "来访记录_jp", "来访记录（主表）"),
            // entity.visitorcompanion.visitor
            new TranslationSeedItem("entity.visitorcompanion.visitor", "zh-CN", "来访记录", "来访记录（主表）"),
            // entity.visitorcompanion.visitor
            new TranslationSeedItem("entity.visitorcompanion.visitor", "zh-HK", "来访记录_hk", "来访记录（主表）"),
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
        translation.ResourceGroup = "VisitorCenter";
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
