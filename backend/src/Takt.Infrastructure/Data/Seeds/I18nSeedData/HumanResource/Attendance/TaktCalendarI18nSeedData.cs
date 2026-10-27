// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Attendance
// 文件名称：TaktCalendarI18nSeedData.cs
// 创建时间：2026-10-27
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCalendar 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktCalendar 实体国际化翻译种子（键前缀 entity.calendar.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCalendarI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCalendar 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 calendar 实体翻译...", tenantCode);

        foreach (var item in GetCalendarTranslations())
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

        TaktLogger.Information("TaktCalendar 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCalendar 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.calendar._self / entity.calendar.{{field}}；ResourceGroup=Attendance；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCalendarTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.calendar._self
            new TranslationSeedItem("entity.calendar._self", "en-US", "Calendar Information_us", "实体名称"),
            // entity.calendar._self
            new TranslationSeedItem("entity.calendar._self", "ja-JP", "工厂日历信息_jp", "实体名称"),
            // entity.calendar._self
            new TranslationSeedItem("entity.calendar._self", "zh-CN", "工厂日历信息", "实体名称"),
            // entity.calendar._self
            new TranslationSeedItem("entity.calendar._self", "zh-HK", "工厂日历信息_hk", "实体名称"),

            // entity.calendar.date
            new TranslationSeedItem("entity.calendar.date", "en-US", "日历日期_us", "日历日期"),
            // entity.calendar.date
            new TranslationSeedItem("entity.calendar.date", "ja-JP", "日历日期_jp", "日历日期"),
            // entity.calendar.date
            new TranslationSeedItem("entity.calendar.date", "zh-CN", "日历日期", "日历日期"),
            // entity.calendar.date
            new TranslationSeedItem("entity.calendar.date", "zh-HK", "日历日期_hk", "日历日期"),

            // entity.calendar.dayofmonth
            new TranslationSeedItem("entity.calendar.dayofmonth", "en-US", "月内第几天_us", "月内第几天（1～31；由 CalendarDate 派生）"),
            // entity.calendar.dayofmonth
            new TranslationSeedItem("entity.calendar.dayofmonth", "ja-JP", "月内第几天_jp", "月内第几天（1～31；由 CalendarDate 派生）"),
            // entity.calendar.dayofmonth
            new TranslationSeedItem("entity.calendar.dayofmonth", "zh-CN", "月内第几天", "月内第几天（1～31；由 CalendarDate 派生）"),
            // entity.calendar.dayofmonth
            new TranslationSeedItem("entity.calendar.dayofmonth", "zh-HK", "月内第几天_hk", "月内第几天（1～31；由 CalendarDate 派生）"),

            // entity.calendar.weekday
            new TranslationSeedItem("entity.calendar.weekday", "en-US", "星期_us", "星期（1=周一 2=周二 3=周三 4=周四 5=周五 6=周六 7=周日；由 CalendarDate 派生）"),
            // entity.calendar.weekday
            new TranslationSeedItem("entity.calendar.weekday", "ja-JP", "星期_jp", "星期（1=周一 2=周二 3=周三 4=周四 5=周五 6=周六 7=周日；由 CalendarDate 派生）"),
            // entity.calendar.weekday
            new TranslationSeedItem("entity.calendar.weekday", "zh-CN", "星期", "星期（1=周一 2=周二 3=周三 4=周四 5=周五 6=周六 7=周日；由 CalendarDate 派生）"),
            // entity.calendar.weekday
            new TranslationSeedItem("entity.calendar.weekday", "zh-HK", "星期_hk", "星期（1=周一 2=周二 3=周三 4=周四 5=周五 6=周六 7=周日；由 CalendarDate 派生）"),

            // entity.calendar.weekofyear
            new TranslationSeedItem("entity.calendar.weekofyear", "en-US", "年内第几周_us", "年内第几周（ISO 8601；1～53；由 CalendarDate 派生）"),
            // entity.calendar.weekofyear
            new TranslationSeedItem("entity.calendar.weekofyear", "ja-JP", "年内第几周_jp", "年内第几周（ISO 8601；1～53；由 CalendarDate 派生）"),
            // entity.calendar.weekofyear
            new TranslationSeedItem("entity.calendar.weekofyear", "zh-CN", "年内第几周", "年内第几周（ISO 8601；1～53；由 CalendarDate 派生）"),
            // entity.calendar.weekofyear
            new TranslationSeedItem("entity.calendar.weekofyear", "zh-HK", "年内第几周_hk", "年内第几周（ISO 8601；1～53；由 CalendarDate 派生）"),

            // entity.calendar.quarter
            new TranslationSeedItem("entity.calendar.quarter", "en-US", "季度_us", "季度（1～4；自然年 Q1=1～3 月；由 CalendarDate 派生）"),
            // entity.calendar.quarter
            new TranslationSeedItem("entity.calendar.quarter", "ja-JP", "季度_jp", "季度（1～4；自然年 Q1=1～3 月；由 CalendarDate 派生）"),
            // entity.calendar.quarter
            new TranslationSeedItem("entity.calendar.quarter", "zh-CN", "季度", "季度（1～4；自然年 Q1=1～3 月；由 CalendarDate 派生）"),
            // entity.calendar.quarter
            new TranslationSeedItem("entity.calendar.quarter", "zh-HK", "季度_hk", "季度（1～4；自然年 Q1=1～3 月；由 CalendarDate 派生）"),

            // entity.calendar.dayofquarter
            new TranslationSeedItem("entity.calendar.dayofquarter", "en-US", "季内第几天_us", "季内第几天（1～92；自然年季度；由 CalendarDate 派生）"),
            // entity.calendar.dayofquarter
            new TranslationSeedItem("entity.calendar.dayofquarter", "ja-JP", "季内第几天_jp", "季内第几天（1～92；自然年季度；由 CalendarDate 派生）"),
            // entity.calendar.dayofquarter
            new TranslationSeedItem("entity.calendar.dayofquarter", "zh-CN", "季内第几天", "季内第几天（1～92；自然年季度；由 CalendarDate 派生）"),
            // entity.calendar.dayofquarter
            new TranslationSeedItem("entity.calendar.dayofquarter", "zh-HK", "季内第几天_hk", "季内第几天（1～92；自然年季度；由 CalendarDate 派生）"),

            // entity.calendar.dayofyear
            new TranslationSeedItem("entity.calendar.dayofyear", "en-US", "年内第几天_us", "年内第几天（1～366；由 CalendarDate 派生）"),
            // entity.calendar.dayofyear
            new TranslationSeedItem("entity.calendar.dayofyear", "ja-JP", "年内第几天_jp", "年内第几天（1～366；由 CalendarDate 派生）"),
            // entity.calendar.dayofyear
            new TranslationSeedItem("entity.calendar.dayofyear", "zh-CN", "年内第几天", "年内第几天（1～366；由 CalendarDate 派生）"),
            // entity.calendar.dayofyear
            new TranslationSeedItem("entity.calendar.dayofyear", "zh-HK", "年内第几天_hk", "年内第几天（1～366；由 CalendarDate 派生）"),

            // entity.calendar.isworkingday
            new TranslationSeedItem("entity.calendar.isworkingday", "en-US", "是否工作日_us", "是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）"),
            // entity.calendar.isworkingday
            new TranslationSeedItem("entity.calendar.isworkingday", "ja-JP", "是否工作日_jp", "是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）"),
            // entity.calendar.isworkingday
            new TranslationSeedItem("entity.calendar.isworkingday", "zh-CN", "是否工作日", "是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）"),
            // entity.calendar.isworkingday
            new TranslationSeedItem("entity.calendar.isworkingday", "zh-HK", "是否工作日_hk", "是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）"),

            // entity.calendar.holidayid
            new TranslationSeedItem("entity.calendar.holidayid", "en-US", "关联假日ID_us", "关联假日（选项 TaktHolidays/options；DictValue=Id）"),
            // entity.calendar.holidayid
            new TranslationSeedItem("entity.calendar.holidayid", "ja-JP", "关联假日ID_jp", "关联假日（选项 TaktHolidays/options；DictValue=Id）"),
            // entity.calendar.holidayid
            new TranslationSeedItem("entity.calendar.holidayid", "zh-CN", "关联假日ID", "关联假日（选项 TaktHolidays/options；DictValue=Id）"),
            // entity.calendar.holidayid
            new TranslationSeedItem("entity.calendar.holidayid", "zh-HK", "关联假日ID_hk", "关联假日（选项 TaktHolidays/options；DictValue=Id）"),

            // entity.calendar.shiftid
            new TranslationSeedItem("entity.calendar.shiftid", "en-US", "关联班次ID_us", "关联班次（选项 TaktWorkShifts/options；DictValue=Id）"),
            // entity.calendar.shiftid
            new TranslationSeedItem("entity.calendar.shiftid", "ja-JP", "关联班次ID_jp", "关联班次（选项 TaktWorkShifts/options；DictValue=Id）"),
            // entity.calendar.shiftid
            new TranslationSeedItem("entity.calendar.shiftid", "zh-CN", "关联班次ID", "关联班次（选项 TaktWorkShifts/options；DictValue=Id）"),
            // entity.calendar.shiftid
            new TranslationSeedItem("entity.calendar.shiftid", "zh-HK", "关联班次ID_hk", "关联班次（选项 TaktWorkShifts/options；DictValue=Id）"),
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
