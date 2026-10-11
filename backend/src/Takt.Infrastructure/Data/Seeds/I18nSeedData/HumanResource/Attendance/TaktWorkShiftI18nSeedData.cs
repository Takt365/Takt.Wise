// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance
// 文件名称：TaktWorkShiftI18nSeedData.cs
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktWorkShift 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance;

/// <summary>
/// TaktWorkShift 实体国际化翻译种子（键前缀 entity.workshift.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktWorkShiftI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktWorkShift 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 workshift 实体翻译...", tenantCode);

        foreach (var item in GetWorkShiftTranslations())
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

        TaktLogger.Information("TaktWorkShift 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktWorkShift 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.workshift._self / entity.workshift.{{field}}；ResourceGroup=Attendance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetWorkShiftTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.workshift._self
            new TranslationSeedItem("entity.workshift._self", "en-US", "Work Shift Information_us", "实体名称"),
            // entity.workshift._self
            new TranslationSeedItem("entity.workshift._self", "ja-JP", "班次定义信息_jp", "实体名称"),
            // entity.workshift._self
            new TranslationSeedItem("entity.workshift._self", "zh-CN", "班次定义信息", "实体名称"),
            // entity.workshift._self
            new TranslationSeedItem("entity.workshift._self", "zh-HK", "班次定义信息_hk", "实体名称"),

            // entity.workshift.shiftcode
            new TranslationSeedItem("entity.workshift.shiftcode", "en-US", "班次编码_us", "班次编码（租户+公司内唯一）"),
            // entity.workshift.shiftcode
            new TranslationSeedItem("entity.workshift.shiftcode", "ja-JP", "班次编码_jp", "班次编码（租户+公司内唯一）"),
            // entity.workshift.shiftcode
            new TranslationSeedItem("entity.workshift.shiftcode", "zh-CN", "班次编码", "班次编码（租户+公司内唯一）"),
            // entity.workshift.shiftcode
            new TranslationSeedItem("entity.workshift.shiftcode", "zh-HK", "班次编码_hk", "班次编码（租户+公司内唯一）"),

            // entity.workshift.shiftname
            new TranslationSeedItem("entity.workshift.shiftname", "en-US", "班次名称_us", "班次名称"),
            // entity.workshift.shiftname
            new TranslationSeedItem("entity.workshift.shiftname", "ja-JP", "班次名称_jp", "班次名称"),
            // entity.workshift.shiftname
            new TranslationSeedItem("entity.workshift.shiftname", "zh-CN", "班次名称", "班次名称"),
            // entity.workshift.shiftname
            new TranslationSeedItem("entity.workshift.shiftname", "zh-HK", "班次名称_hk", "班次名称"),

            // entity.workshift.starttime
            new TranslationSeedItem("entity.workshift.starttime", "en-US", "开始时间_us", "当班开始时间（HH:mm）"),
            // entity.workshift.starttime
            new TranslationSeedItem("entity.workshift.starttime", "ja-JP", "开始时间_jp", "当班开始时间（HH:mm）"),
            // entity.workshift.starttime
            new TranslationSeedItem("entity.workshift.starttime", "zh-CN", "开始时间", "当班开始时间（HH:mm）"),
            // entity.workshift.starttime
            new TranslationSeedItem("entity.workshift.starttime", "zh-HK", "开始时间_hk", "当班开始时间（HH:mm）"),

            // entity.workshift.endtime
            new TranslationSeedItem("entity.workshift.endtime", "en-US", "结束时间_us", "当班结束时间（HH:mm）"),
            // entity.workshift.endtime
            new TranslationSeedItem("entity.workshift.endtime", "ja-JP", "结束时间_jp", "当班结束时间（HH:mm）"),
            // entity.workshift.endtime
            new TranslationSeedItem("entity.workshift.endtime", "zh-CN", "结束时间", "当班结束时间（HH:mm）"),
            // entity.workshift.endtime
            new TranslationSeedItem("entity.workshift.endtime", "zh-HK", "结束时间_hk", "当班结束时间（HH:mm）"),

            // entity.workshift.crossmidnight
            new TranslationSeedItem("entity.workshift.crossmidnight", "en-US", "是否跨日_us", "是否跨自然日（字典 sys_yes_no；0=否 1=是）"),
            // entity.workshift.crossmidnight
            new TranslationSeedItem("entity.workshift.crossmidnight", "ja-JP", "是否跨日_jp", "是否跨自然日（字典 sys_yes_no；0=否 1=是）"),
            // entity.workshift.crossmidnight
            new TranslationSeedItem("entity.workshift.crossmidnight", "zh-CN", "是否跨日", "是否跨自然日（字典 sys_yes_no；0=否 1=是）"),
            // entity.workshift.crossmidnight
            new TranslationSeedItem("entity.workshift.crossmidnight", "zh-HK", "是否跨日_hk", "是否跨自然日（字典 sys_yes_no；0=否 1=是）"),

            // entity.workshift.sortorder
            new TranslationSeedItem("entity.workshift.sortorder", "en-US", "排序号_us", "排序号（回填）"),
            // entity.workshift.sortorder
            new TranslationSeedItem("entity.workshift.sortorder", "ja-JP", "排序号_jp", "排序号（回填）"),
            // entity.workshift.sortorder
            new TranslationSeedItem("entity.workshift.sortorder", "zh-CN", "排序号", "排序号（回填）"),
            // entity.workshift.sortorder
            new TranslationSeedItem("entity.workshift.sortorder", "zh-HK", "排序号_hk", "排序号（回填）"),
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
        translation.ResourceGroup = "Attendance";
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
