// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktCalendarSeedData.cs
// 创建时间：2026-07-16
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂日历种子（公司 2300 / 工厂 C100 / 2026；周一至周五基线 + China2026 假日覆盖；幂等）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Attendance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 工厂日历种子（仅公司 2300 / 工厂 C100；覆盖 2026 全年；依赖假日种子 China2026）
/// </summary>
public class TaktCalendarSeedData : ITaktSeedDataCoordinator
{
    private const int CalendarYear = 2026;

    private const int Off = 0;

    private const int Work = 1;

    private static readonly HashSet<string> TargetPlantCodes = new(StringComparer.Ordinal) { "C100" };

    /// <summary>
    /// 执行顺序（假日种子 Order 47 之后）
    /// </summary>
    public int Order => 470;

    /// <summary>
    /// 初始化工厂日历种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化工厂日历种子数据（{Year} / C100）...", CalendarYear);
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过工厂日历种子数据初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var calendarRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktCalendar>>();
        var holidayRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktHoliday>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过工厂日历种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过工厂日历种子", tenantCode);
            return (0, 0);
        }
        var yearStart = new DateTime(CalendarYear, 1, 1);
        var yearEnd = new DateTime(CalendarYear, 12, 31);
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化工厂日历...", tenantCode);
        foreach (var company in orderedCompanies)
        {
            string plantCode;
            try
            {
                plantCode = database.GetPlantCodeForCompanyCode(company.CompanyCode);
            }
            catch (InvalidOperationException ex)
            {
                TaktLogger.Warning(
                    "公司 {CompanyCode} 未映射工厂，跳过工厂日历种子: {Message}",
                    company.CompanyCode,
                    ex.Message);
                continue;
            }
            if (!TargetPlantCodes.Contains(plantCode))
            {
                TaktLogger.Information(
                    "公司 {CompanyCode} 工厂 {PlantCode} 非目标工厂 C100，跳过工厂日历种子",
                    company.CompanyCode,
                    plantCode);
                continue;
            }
            TaktLogger.Information(
                "正在为公司 {CompanyCode} 工厂 {PlantCode} 生成 {Year} 年日历...",
                company.CompanyCode,
                plantCode,
                CalendarYear);
            var holidays = await holidayRepository.GetListAsync(h =>
                h.TenantCode == tenantCode
                && h.CompanyCode == company.CompanyCode
                && h.StartDate <= yearEnd
                && h.EndDate >= yearStart);
            if (holidays == null || holidays.Count == 0)
            {
                TaktLogger.Warning(
                    "公司 {CompanyCode} 无 {Year} 年假日数据，日历仅按周一至周五基线生成",
                    company.CompanyCode,
                    CalendarYear);
            }
            var holidayByDate = BuildHolidayOverlay(holidays ?? []);
            var existing = await calendarRepository.GetListAsync(c =>
                c.TenantCode == tenantCode
                && c.CompanyCode == company.CompanyCode
                && c.PlantCode == plantCode
                && c.CalendarDate >= yearStart
                && c.CalendarDate <= yearEnd);
            var existingByDate = (existing ?? [])
                .GroupBy(c => c.CalendarDate.Date)
                .ToDictionary(g => g.Key, g => g.First());
            var toInsert = new List<TaktCalendar>();
            var toUpdate = new List<TaktCalendar>();
            for (var day = yearStart; day <= yearEnd; day = day.AddDays(1))
            {
                var date = day.Date;
                var (isWorkingDay, holidayId) = ResolveDay(date, holidayByDate);
                var derived = ResolveDateDerived(date);
                if (!existingByDate.TryGetValue(date, out var row))
                {
                    var entity = new TaktCalendar
                    {
                        TenantCode = tenantCode,
                        CompanyCode = company.CompanyCode,
                        CalendarDate = date,
                        IsWorkingDay = isWorkingDay,
                        HolidayId = holidayId,
                        ShiftId = null,
                        PlantCode = plantCode,
                        CultureCode = company.CultureCode
                    };
                    ApplyDateDerived(entity, derived);
                    toInsert.Add(entity);
                    continue;
                }
                var needUpdate = row.IsWorkingDay != isWorkingDay
                    || row.HolidayId != holidayId
                    || row.CultureCode != company.CultureCode
                    || row.DayOfMonth != derived.DayOfMonth
                    || row.Weekday != derived.Weekday
                    || row.WeekOfYear != derived.WeekOfYear
                    || row.Quarter != derived.Quarter
                    || row.DayOfQuarter != derived.DayOfQuarter
                    || row.DayOfYear != derived.DayOfYear;
                if (!needUpdate)
                {
                    continue;
                }
                row.IsWorkingDay = isWorkingDay;
                row.HolidayId = holidayId;
                row.CultureCode = company.CultureCode;
                ApplyDateDerived(row, derived);
                toUpdate.Add(row);
            }
            if (toInsert.Count > 0)
            {
                insertCount += await calendarRepository.CreateRangeBulkAsync(toInsert);
            }
            if (toUpdate.Count > 0)
            {
                updateCount += await calendarRepository.UpdateRangeAsync(toUpdate);
            }
            TaktLogger.Information(
                "公司 {CompanyCode} 工厂 {PlantCode} 日历完成: 插入 {InsertCount}，更新 {UpdateCount}（假日覆盖 {HolidayDays} 天）",
                company.CompanyCode,
                plantCode,
                toInsert.Count,
                toUpdate.Count,
                holidayByDate.Count);
        }
        TaktLogger.Information(
            "工厂日历种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 将假日区间标为休息、将 CompensatoryWorkDates 标为上班；同日后写覆盖（调休上班优先于假日区间）
    /// </summary>
    /// <param name="holidays">公司假日列表</param>
    /// <returns>日期覆盖字典</returns>
    private static Dictionary<DateTime, (int IsWorkingDay, long HolidayId)> BuildHolidayOverlay(
        List<TaktHoliday> holidays)
    {
        var map = new Dictionary<DateTime, (int IsWorkingDay, long HolidayId)>();
        foreach (var holiday in holidays.OrderBy(h => h.HolidayType).ThenBy(h => h.StartDate))
        {
            var start = holiday.StartDate.Date;
            var end = holiday.EndDate.Date;
            if (end >= start)
            {
                for (var d = start; d <= end; d = d.AddDays(1))
                {
                    if (d.Year != CalendarYear)
                    {
                        continue;
                    }
                    map[d] = (Off, holiday.Id);
                }
            }
            foreach (var workDate in ParseCompensatoryWorkDates(holiday.CompensatoryWorkDates))
            {
                if (workDate.Year != CalendarYear)
                {
                    continue;
                }
                map[workDate] = (Work, holiday.Id);
            }
        }
        return map;
    }

    /// <summary>
    /// 解析调休上班日（取「上班日=&gt;放假日」左侧；兼容仅上班日）
    /// </summary>
    /// <param name="raw">CompensatoryWorkDates 原文</param>
    /// <returns>调休上班日序列</returns>
    private static IEnumerable<DateTime> ParseCompensatoryWorkDates(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            yield break;
        }
        foreach (var part in raw.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var workRaw = part;
            var arrow = part.IndexOf("=>", StringComparison.Ordinal);
            if (arrow >= 0)
            {
                workRaw = part[..arrow].Trim();
            }
            if (DateTime.TryParseExact(workRaw, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
            {
                yield return d.Date;
            }
        }
    }

    /// <summary>
    /// 解析单日工作日：假日覆盖优先，否则周一至周五为工作日、周末为休息日
    /// </summary>
    /// <param name="date">自然日</param>
    /// <param name="holidayByDate">假日覆盖字典</param>
    /// <returns>是否工作日与关联假日 Id</returns>
    private static (int IsWorkingDay, long? HolidayId) ResolveDay(
        DateTime date,
        IReadOnlyDictionary<DateTime, (int IsWorkingDay, long HolidayId)> holidayByDate)
    {
        if (holidayByDate.TryGetValue(date, out var overlay))
        {
            return (overlay.IsWorkingDay, overlay.HolidayId);
        }
        var isWeekend = date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        return (isWeekend ? Off : Work, null);
    }

    /// <summary>
    /// 由日历日期派生：月内日、星期（周一=1…周日=7）、ISO 周次、自然年季度、季内日、年内日
    /// </summary>
    /// <param name="date">自然日</param>
    /// <returns>日期派生值</returns>
    private static (
        int DayOfMonth,
        int Weekday,
        int WeekOfYear,
        int Quarter,
        int DayOfQuarter,
        int DayOfYear) ResolveDateDerived(DateTime date)
    {
        date = date.Date;
        var dow = (int)date.DayOfWeek;
        var weekday = dow == 0 ? 7 : dow;
        var quarter = (date.Month - 1) / 3 + 1;
        var quarterStart = new DateTime(date.Year, (quarter - 1) * 3 + 1, 1);
        var dayOfQuarter = (date - quarterStart).Days + 1;
        return (
            date.Day,
            weekday,
            ISOWeek.GetWeekOfYear(date),
            quarter,
            dayOfQuarter,
            date.DayOfYear);
    }

    /// <summary>
    /// 将日期派生字段写入日历实体
    /// </summary>
    /// <param name="entity">日历实体</param>
    /// <param name="derived">派生值</param>
    private static void ApplyDateDerived(
        TaktCalendar entity,
        (
            int DayOfMonth,
            int Weekday,
            int WeekOfYear,
            int Quarter,
            int DayOfQuarter,
            int DayOfYear) derived)
    {
        entity.DayOfMonth = derived.DayOfMonth;
        entity.Weekday = derived.Weekday;
        entity.WeekOfYear = derived.WeekOfYear;
        entity.Quarter = derived.Quarter;
        entity.DayOfQuarter = derived.DayOfQuarter;
        entity.DayOfYear = derived.DayOfYear;
    }
}
