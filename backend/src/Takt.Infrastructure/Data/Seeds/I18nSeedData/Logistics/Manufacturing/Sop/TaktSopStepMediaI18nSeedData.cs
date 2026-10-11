// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepMediaI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSopStepMedia 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktSopStepMedia 实体国际化翻译种子（键前缀 entity.sopstepmedia.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSopStepMediaI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSopStepMedia 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 sopstepmedia 实体翻译...", tenantCode);

        foreach (var item in GetSopStepMediaTranslations())
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

        TaktLogger.Information("TaktSopStepMedia 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSopStepMedia 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.sopstepmedia._self / entity.sopstepmedia.{{field}}；ResourceGroup=Sop；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSopStepMediaTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.sopstepmedia._self
            new TranslationSeedItem("entity.sopstepmedia._self", "en-US", "Sop Step Media Information_us", "实体名称"),
            // entity.sopstepmedia._self
            new TranslationSeedItem("entity.sopstepmedia._self", "ja-JP", "SOP 工步多媒体信息_jp", "实体名称"),
            // entity.sopstepmedia._self
            new TranslationSeedItem("entity.sopstepmedia._self", "zh-CN", "SOP 工步多媒体信息", "实体名称"),
            // entity.sopstepmedia._self
            new TranslationSeedItem("entity.sopstepmedia._self", "zh-HK", "SOP 工步多媒体信息_hk", "实体名称"),

            // entity.sopstepmedia.stepid
            new TranslationSeedItem("entity.sopstepmedia.stepid", "en-US", "工步ID_us", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopstepmedia.stepid
            new TranslationSeedItem("entity.sopstepmedia.stepid", "ja-JP", "工步ID_jp", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopstepmedia.stepid
            new TranslationSeedItem("entity.sopstepmedia.stepid", "zh-CN", "工步ID", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),
            // entity.sopstepmedia.stepid
            new TranslationSeedItem("entity.sopstepmedia.stepid", "zh-HK", "工步ID_hk", "工步 ID（选项 TaktSopSteps/options；DictValue=Id）"),

            // entity.sopstepmedia.mediatype
            new TranslationSeedItem("entity.sopstepmedia.mediatype", "en-US", "媒体类型_us", "媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）"),
            // entity.sopstepmedia.mediatype
            new TranslationSeedItem("entity.sopstepmedia.mediatype", "ja-JP", "媒体类型_jp", "媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）"),
            // entity.sopstepmedia.mediatype
            new TranslationSeedItem("entity.sopstepmedia.mediatype", "zh-CN", "媒体类型", "媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）"),
            // entity.sopstepmedia.mediatype
            new TranslationSeedItem("entity.sopstepmedia.mediatype", "zh-HK", "媒体类型_hk", "媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）"),

            // entity.sopstepmedia.fileurl
            new TranslationSeedItem("entity.sopstepmedia.fileurl", "en-US", "文件URL_us", "文件 URL"),
            // entity.sopstepmedia.fileurl
            new TranslationSeedItem("entity.sopstepmedia.fileurl", "ja-JP", "文件URL_jp", "文件 URL"),
            // entity.sopstepmedia.fileurl
            new TranslationSeedItem("entity.sopstepmedia.fileurl", "zh-CN", "文件URL", "文件 URL"),
            // entity.sopstepmedia.fileurl
            new TranslationSeedItem("entity.sopstepmedia.fileurl", "zh-HK", "文件URL_hk", "文件 URL"),

            // entity.sopstepmedia.fileext
            new TranslationSeedItem("entity.sopstepmedia.fileext", "en-US", "文件扩展名_us", "文件扩展名（jpg/png/mp4/pdf/glb 等）"),
            // entity.sopstepmedia.fileext
            new TranslationSeedItem("entity.sopstepmedia.fileext", "ja-JP", "文件扩展名_jp", "文件扩展名（jpg/png/mp4/pdf/glb 等）"),
            // entity.sopstepmedia.fileext
            new TranslationSeedItem("entity.sopstepmedia.fileext", "zh-CN", "文件扩展名", "文件扩展名（jpg/png/mp4/pdf/glb 等）"),
            // entity.sopstepmedia.fileext
            new TranslationSeedItem("entity.sopstepmedia.fileext", "zh-HK", "文件扩展名_hk", "文件扩展名（jpg/png/mp4/pdf/glb 等）"),

            // entity.sopstepmedia.sortorder
            new TranslationSeedItem("entity.sopstepmedia.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.sopstepmedia.sortorder
            new TranslationSeedItem("entity.sopstepmedia.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.sopstepmedia.sortorder
            new TranslationSeedItem("entity.sopstepmedia.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.sopstepmedia.sortorder
            new TranslationSeedItem("entity.sopstepmedia.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),

            // entity.sopstepmedia.step
            new TranslationSeedItem("entity.sopstepmedia.step", "en-US", "工步_us", "工步"),
            // entity.sopstepmedia.step
            new TranslationSeedItem("entity.sopstepmedia.step", "ja-JP", "工步_jp", "工步"),
            // entity.sopstepmedia.step
            new TranslationSeedItem("entity.sopstepmedia.step", "zh-CN", "工步", "工步"),
            // entity.sopstepmedia.step
            new TranslationSeedItem("entity.sopstepmedia.step", "zh-HK", "工步_hk", "工步"),
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
