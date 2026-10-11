// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopStep 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop;

/// <summary>
/// TaktSopStep 实体国际化翻译种子（键前缀 entity.sopstep.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopStepI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopStep 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopstep 实体翻译...", tenantCode);

        foreach (var item in GetSopStepTranslations())
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

        TaktLogger.Information("TaktSopStep 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopStep 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopstep._self / entity.sopstep.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopStepTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopstep._self
            new TranslationSeedItem("entity.sopstep._self", "en-US", "Sop Step Information_us", "实体名称"),
            // entity.sopstep._self
            new TranslationSeedItem("entity.sopstep._self", "ja-JP", "SOP 工步信息_jp", "实体名称"),
            // entity.sopstep._self
            new TranslationSeedItem("entity.sopstep._self", "zh-CN", "SOP 工步信息", "实体名称"),
            // entity.sopstep._self
            new TranslationSeedItem("entity.sopstep._self", "zh-HK", "SOP 工步信息_hk", "实体名称"),

            // entity.sopstep.contentid
            new TranslationSeedItem("entity.sopstep.contentid", "en-US", "正文ID_us", "正文 ID（选项 TaktSopContents/options；DictValue=Id）"),
            // entity.sopstep.contentid
            new TranslationSeedItem("entity.sopstep.contentid", "ja-JP", "正文ID_jp", "正文 ID（选项 TaktSopContents/options；DictValue=Id）"),
            // entity.sopstep.contentid
            new TranslationSeedItem("entity.sopstep.contentid", "zh-CN", "正文ID", "正文 ID（选项 TaktSopContents/options；DictValue=Id）"),
            // entity.sopstep.contentid
            new TranslationSeedItem("entity.sopstep.contentid", "zh-HK", "正文ID_hk", "正文 ID（选项 TaktSopContents/options；DictValue=Id）"),

            // entity.sopstep.stepno
            new TranslationSeedItem("entity.sopstep.stepno", "en-US", "工步序号_us", "工步序号"),
            // entity.sopstep.stepno
            new TranslationSeedItem("entity.sopstep.stepno", "ja-JP", "工步序号_jp", "工步序号"),
            // entity.sopstep.stepno
            new TranslationSeedItem("entity.sopstep.stepno", "zh-CN", "工步序号", "工步序号"),
            // entity.sopstep.stepno
            new TranslationSeedItem("entity.sopstep.stepno", "zh-HK", "工步序号_hk", "工步序号"),

            // entity.sopstep.steptitle
            new TranslationSeedItem("entity.sopstep.steptitle", "en-US", "工步标题_us", "工步标题"),
            // entity.sopstep.steptitle
            new TranslationSeedItem("entity.sopstep.steptitle", "ja-JP", "工步标题_jp", "工步标题"),
            // entity.sopstep.steptitle
            new TranslationSeedItem("entity.sopstep.steptitle", "zh-CN", "工步标题", "工步标题"),
            // entity.sopstep.steptitle
            new TranslationSeedItem("entity.sopstep.steptitle", "zh-HK", "工步标题_hk", "工步标题"),

            // entity.sopstep.stepdescription
            new TranslationSeedItem("entity.sopstep.stepdescription", "en-US", "作业说明_us", "作业说明"),
            // entity.sopstep.stepdescription
            new TranslationSeedItem("entity.sopstep.stepdescription", "ja-JP", "作业说明_jp", "作业说明"),
            // entity.sopstep.stepdescription
            new TranslationSeedItem("entity.sopstep.stepdescription", "zh-CN", "作业说明", "作业说明"),
            // entity.sopstep.stepdescription
            new TranslationSeedItem("entity.sopstep.stepdescription", "zh-HK", "作业说明_hk", "作业说明"),

            // entity.sopstep.safetyalert
            new TranslationSeedItem("entity.sopstep.safetyalert", "en-US", "安全警示_us", "安全警示"),
            // entity.sopstep.safetyalert
            new TranslationSeedItem("entity.sopstep.safetyalert", "ja-JP", "安全警示_jp", "安全警示"),
            // entity.sopstep.safetyalert
            new TranslationSeedItem("entity.sopstep.safetyalert", "zh-CN", "安全警示", "安全警示"),
            // entity.sopstep.safetyalert
            new TranslationSeedItem("entity.sopstep.safetyalert", "zh-HK", "安全警示_hk", "安全警示"),

            // entity.sopstep.safetypopuprequired
            new TranslationSeedItem("entity.sopstep.safetypopuprequired", "en-US", "弹窗_us", "弹窗（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopstep.safetypopuprequired
            new TranslationSeedItem("entity.sopstep.safetypopuprequired", "ja-JP", "弹窗_jp", "弹窗（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopstep.safetypopuprequired
            new TranslationSeedItem("entity.sopstep.safetypopuprequired", "zh-CN", "弹窗", "弹窗（字典 sys_yes_no；0=否，1=是）"),
            // entity.sopstep.safetypopuprequired
            new TranslationSeedItem("entity.sopstep.safetypopuprequired", "zh-HK", "弹窗_hk", "弹窗（字典 sys_yes_no；0=否，1=是）"),

            // entity.sopstep.content
            new TranslationSeedItem("entity.sopstep.content", "en-US", "正文_us", "正文"),
            // entity.sopstep.content
            new TranslationSeedItem("entity.sopstep.content", "ja-JP", "正文_jp", "正文"),
            // entity.sopstep.content
            new TranslationSeedItem("entity.sopstep.content", "zh-CN", "正文", "正文"),
            // entity.sopstep.content
            new TranslationSeedItem("entity.sopstep.content", "zh-HK", "正文_hk", "正文"),

            // entity.sopstep.medialist
            new TranslationSeedItem("entity.sopstep.medialist", "en-US", "多媒体_us", "多媒体"),
            // entity.sopstep.medialist
            new TranslationSeedItem("entity.sopstep.medialist", "ja-JP", "多媒体_jp", "多媒体"),
            // entity.sopstep.medialist
            new TranslationSeedItem("entity.sopstep.medialist", "zh-CN", "多媒体", "多媒体"),
            // entity.sopstep.medialist
            new TranslationSeedItem("entity.sopstep.medialist", "zh-HK", "多媒体_hk", "多媒体"),

            // entity.sopstep.checkitems
            new TranslationSeedItem("entity.sopstep.checkitems", "en-US", "检验项目_us", "检验项目"),
            // entity.sopstep.checkitems
            new TranslationSeedItem("entity.sopstep.checkitems", "ja-JP", "检验项目_jp", "检验项目"),
            // entity.sopstep.checkitems
            new TranslationSeedItem("entity.sopstep.checkitems", "zh-CN", "检验项目", "检验项目"),
            // entity.sopstep.checkitems
            new TranslationSeedItem("entity.sopstep.checkitems", "zh-HK", "检验项目_hk", "检验项目"),
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
        translation.ResourceGroup = "Sop";
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
