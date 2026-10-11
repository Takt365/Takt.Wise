// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance
// 文件名称：TaktHolidayI18nSeedData.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktHoliday 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktHoliday 实体国际化翻译种子（键前缀 entity.holiday.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktHolidayI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktHoliday 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 holiday 实体翻译...", tenantCode);

        foreach (var item in GetHolidayTranslations())
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

        TaktLogger.Information("TaktHoliday 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktHoliday 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.holiday._self / entity.holiday.{{field}}；ResourceGroup=Attendance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetHolidayTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.holiday._self
            new TranslationSeedItem("entity.holiday._self", "en-US", "Holiday Information_us", "实体名称"),
            // entity.holiday._self
            new TranslationSeedItem("entity.holiday._self", "ja-JP", "假日信息_jp", "实体名称"),
            // entity.holiday._self
            new TranslationSeedItem("entity.holiday._self", "zh-CN", "假日信息", "实体名称"),
            // entity.holiday._self
            new TranslationSeedItem("entity.holiday._self", "zh-HK", "假日信息_hk", "实体名称"),

            // entity.holiday.name
            new TranslationSeedItem("entity.holiday.name", "en-US", "假日名称_us", "假日名称"),
            // entity.holiday.name
            new TranslationSeedItem("entity.holiday.name", "ja-JP", "假日名称_jp", "假日名称"),
            // entity.holiday.name
            new TranslationSeedItem("entity.holiday.name", "zh-CN", "假日名称", "假日名称"),
            // entity.holiday.name
            new TranslationSeedItem("entity.holiday.name", "zh-HK", "假日名称_hk", "假日名称"),

            // entity.holiday.type
            new TranslationSeedItem("entity.holiday.type", "en-US", "假日类型_us", "假日类型（字典 humanresource_attendance_holiday_category；0=法定 1=调休 2=公司）"),
            // entity.holiday.type
            new TranslationSeedItem("entity.holiday.type", "ja-JP", "假日类型_jp", "假日类型（字典 humanresource_attendance_holiday_category；0=法定 1=调休 2=公司）"),
            // entity.holiday.type
            new TranslationSeedItem("entity.holiday.type", "zh-CN", "假日类型", "假日类型（字典 humanresource_attendance_holiday_category；0=法定 1=调休 2=公司）"),
            // entity.holiday.type
            new TranslationSeedItem("entity.holiday.type", "zh-HK", "假日类型_hk", "假日类型（字典 humanresource_attendance_holiday_category；0=法定 1=调休 2=公司）"),

            // entity.holiday.startdate
            new TranslationSeedItem("entity.holiday.startdate", "en-US", "假日开始日期_us", "假日开始日期"),
            // entity.holiday.startdate
            new TranslationSeedItem("entity.holiday.startdate", "ja-JP", "假日开始日期_jp", "假日开始日期"),
            // entity.holiday.startdate
            new TranslationSeedItem("entity.holiday.startdate", "zh-CN", "假日开始日期", "假日开始日期"),
            // entity.holiday.startdate
            new TranslationSeedItem("entity.holiday.startdate", "zh-HK", "假日开始日期_hk", "假日开始日期"),

            // entity.holiday.enddate
            new TranslationSeedItem("entity.holiday.enddate", "en-US", "假日结束日期_us", "假日结束日期"),
            // entity.holiday.enddate
            new TranslationSeedItem("entity.holiday.enddate", "ja-JP", "假日结束日期_jp", "假日结束日期"),
            // entity.holiday.enddate
            new TranslationSeedItem("entity.holiday.enddate", "zh-CN", "假日结束日期", "假日结束日期"),
            // entity.holiday.enddate
            new TranslationSeedItem("entity.holiday.enddate", "zh-HK", "假日结束日期_hk", "假日结束日期"),

            // entity.holiday.dayscount
            new TranslationSeedItem("entity.holiday.dayscount", "en-US", "假期天数_us", "假期天数（含起止日；由 StartDate～EndDate 派生）"),
            new TranslationSeedItem("entity.holiday.dayscount", "ja-JP", "假期天数_jp", "假期天数（含起止日；由 StartDate～EndDate 派生）"),
            new TranslationSeedItem("entity.holiday.dayscount", "zh-CN", "假期天数", "假期天数（含起止日；由 StartDate～EndDate 派生）"),
            new TranslationSeedItem("entity.holiday.dayscount", "zh-HK", "假期天数_hk", "假期天数（含起止日；由 StartDate～EndDate 派生）"),

            // entity.holiday.compensatoryworkdates
            new TranslationSeedItem("entity.holiday.compensatoryworkdates", "en-US", "调休对应_us", "调休对应（上班日=>所补放假日；多对分号分隔）"),
            new TranslationSeedItem("entity.holiday.compensatoryworkdates", "ja-JP", "调休对应_jp", "调休对应（上班日=>所补放假日；多对分号分隔）"),
            new TranslationSeedItem("entity.holiday.compensatoryworkdates", "zh-CN", "调休对应", "调休对应（上班日=>所补放假日；多对分号分隔，如 2026-09-20=>2026-10-06;2026-10-10=>2026-10-07）"),
            new TranslationSeedItem("entity.holiday.compensatoryworkdates", "zh-HK", "调休对应_hk", "调休对应（上班日=>所补放假日；多对分号分隔）"),

            // entity.holiday.ispaid
            new TranslationSeedItem("entity.holiday.ispaid", "en-US", "是否带薪假_us", "是否带薪假（字典 sys_yes_no；0=否 1=是）"),
            new TranslationSeedItem("entity.holiday.ispaid", "ja-JP", "是否带薪假_jp", "是否带薪假（字典 sys_yes_no；0=否 1=是）"),
            new TranslationSeedItem("entity.holiday.ispaid", "zh-CN", "是否带薪假", "是否带薪假（字典 sys_yes_no；0=否 1=是）"),
            new TranslationSeedItem("entity.holiday.ispaid", "zh-HK", "是否带薪假_hk", "是否带薪假（字典 sys_yes_no；0=否 1=是）"),

            // entity.holiday.greeting
            new TranslationSeedItem("entity.holiday.greeting", "en-US", "假日问候语_us", "假日问候语（简短，用于界面问候展示）"),
            // entity.holiday.greeting
            new TranslationSeedItem("entity.holiday.greeting", "ja-JP", "假日问候语_jp", "假日问候语（简短，用于界面问候展示）"),
            // entity.holiday.greeting
            new TranslationSeedItem("entity.holiday.greeting", "zh-CN", "假日问候语", "假日问候语（简短，用于界面问候展示）"),
            // entity.holiday.greeting
            new TranslationSeedItem("entity.holiday.greeting", "zh-HK", "假日问候语_hk", "假日问候语（简短，用于界面问候展示）"),

            // entity.holiday.quote
            new TranslationSeedItem("entity.holiday.quote", "en-US", "假日引用_us", "假日引用/诗句（用于引用区展示）"),
            // entity.holiday.quote
            new TranslationSeedItem("entity.holiday.quote", "ja-JP", "假日引用_jp", "假日引用/诗句（用于引用区展示）"),
            // entity.holiday.quote
            new TranslationSeedItem("entity.holiday.quote", "zh-CN", "假日引用", "假日引用/诗句（用于引用区展示）"),
            // entity.holiday.quote
            new TranslationSeedItem("entity.holiday.quote", "zh-HK", "假日引用_hk", "假日引用/诗句（用于引用区展示）"),

            // entity.holiday.theme
            new TranslationSeedItem("entity.holiday.theme", "en-US", "假日主题_us", "假日主题（对应前端主题色 key，用于日历等非工作日展示）"),
            // entity.holiday.theme
            new TranslationSeedItem("entity.holiday.theme", "ja-JP", "假日主题_jp", "假日主题（对应前端主题色 key，用于日历等非工作日展示）"),
            // entity.holiday.theme
            new TranslationSeedItem("entity.holiday.theme", "zh-CN", "假日主题", "假日主题（对应前端主题色 key，用于日历等非工作日展示）"),
            // entity.holiday.theme
            new TranslationSeedItem("entity.holiday.theme", "zh-HK", "假日主题_hk", "假日主题（对应前端主题色 key，用于日历等非工作日展示）"),
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
