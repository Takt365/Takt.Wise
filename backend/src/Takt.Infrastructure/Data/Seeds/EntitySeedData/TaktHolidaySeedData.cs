// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktHolidaySeedData.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：2026 年公众假期种子（按 Database:CompanyCodes 各公司及 CultureCode 区域写入；不参与公司域克隆）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Attendance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 假日种子数据初始化（2026 年中国 / 美国 / 日本 / 香港公众假期）
/// 各租户库按 <c>CompanyCodes</c> 全部启用公司、按 <c>CultureCode</c> 写入区域假期；幂等：存在则更新，不存在则创建
/// </summary>
public class TaktHolidaySeedData : ITaktSeedDataCoordinator
{
    private static readonly int Statutory = 0;

    /// <summary>
    /// 执行顺序（在公司、区域文化、字典数据之后）
    /// </summary>
    public int Order => 47;

    /// <summary>
    /// 初始化假日种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 2026 年公众假期种子数据...");

        // 参数验证：必须使用协调器传入的租户编码
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过假日种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktHoliday>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var database = serviceProvider.GetRequiredService<IConfiguration>().RequireDatabase();

        int insertCount = 0;
        int updateCount = 0;

        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);

        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过假日种子数据初始化", tenantCode);
            return (0, 0);
        }

        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);

        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过假日种子数据初始化", tenantCode);
            return (0, 0);
        }

        TaktLogger.Information("正在为租户 {TenantCode} 初始化假日数据...", tenantCode);

        foreach (var company in orderedCompanies)
        {
            TaktLogger.Information(
                "正在为公司 {CompanyCode} ({CompanyName1}) 初始化假日...",
                company.CompanyCode,
                company.CompanyName1);

            var holidays = GetHolidaysForCompany(company);

            foreach (var holidayData in holidays)
            {
                var (_, i, u) = await CreateOrUpdateHolidayAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode,
                    database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                    holidayData);

                insertCount += i;
                updateCount += u;
            }
        }

        TaktLogger.Information("假日种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 获取指定公司应写入的假日种子（按公司 CultureCode 匹配地区模板）
    /// </summary>
    /// <param name="company">公司实体</param>
    /// <returns>该公司假日种子列表</returns>
    private static List<TaktHolidaySeedItem> GetHolidaysForCompany(TaktCompany company)
    {
        var items = company.CultureCode switch
        {
            "zh-CN" => GetChina2026Items().ToList(),
            "ja-JP" => GetJapan2026Items().ToList(),
            "zh-HK" => GetHongKong2026Items().ToList(),
            "en-US" => GetUnitedStates2026Items().ToList(),
            _ => [],
        };

        if (items.Count == 0)
        {
            TaktLogger.Warning("公司 {CompanyCode} 无匹配假日种子", company.CompanyCode);
        }

        return items;
    }

    /// <summary>
    /// 中国 2026 年法定节假日与调休上班日（国务院办公厅 2026 年部分节假日安排的通知）
    /// </summary>
    private static IEnumerable<TaktHolidaySeedItem> GetChina2026Items()
    {
        yield return Item("元旦", Statutory, D(2026, 1, 1), D(2026, 1, 3), "2026-01-04=>2026-01-02",
            "元旦快乐", "新年新气象，万事开头好。", "klein-blue");
        yield return Item("春节", Statutory, D(2026, 2, 15), D(2026, 2, 23), "2026-02-14=>2026-02-16;2026-02-28=>2026-02-23",
            "春节快乐", "辞旧迎新，阖家安康。", "chinese-red");
        yield return Item("清明节", Statutory, D(2026, 4, 4), D(2026, 4, 6), "",
            "清明安康", "慎终追远，踏青怀思。", "mars-green");
        yield return Item("劳动节", Statutory, D(2026, 5, 1), D(2026, 5, 5), "2026-05-09=>2026-05-05",
            "劳动节快乐", "致敬每一位劳动者。", "titian-red");
        yield return Item("端午节", Statutory, D(2026, 6, 19), D(2026, 6, 21), "",
            "端午安康", "粽叶飘香，平安顺遂。", "mars-green");
        yield return Item("中秋节", Statutory, D(2026, 9, 25), D(2026, 9, 27), "",
            "中秋快乐", "月圆人团圆，幸福常相伴。", "chinese-red");
        yield return Item("国庆节", Statutory, D(2026, 10, 1), D(2026, 10, 7), "2026-09-20=>2026-10-06;2026-10-10=>2026-10-07",
            "国庆节快乐", "祝福祖国繁荣昌盛。", "chinese-red");
    }

    /// <summary>
    /// 香港 2026 年公众假期（政府宪报 General Holidays，共 17 天；公司 2400）
    /// </summary>
    private static IEnumerable<TaktHolidaySeedItem> GetHongKong2026Items()
    {
        yield return Item("一月一日", Statutory, D(2026, 1, 1), D(2026, 1, 1), "",
            "元旦快樂", "新年新氣象，萬事開頭好。", "klein-blue");
        yield return Item("農曆年初一", Statutory, D(2026, 2, 17), D(2026, 2, 17), "",
            "新春快樂", "辭舊迎新，闔家安康。", "chinese-red");
        yield return Item("農曆年初二", Statutory, D(2026, 2, 18), D(2026, 2, 18), "",
            "新春快樂", "福氣滿門，順遂平安。", "chinese-red");
        yield return Item("農曆年初三", Statutory, D(2026, 2, 19), D(2026, 2, 19), "",
            "新春快樂", "吉祥如意，萬事勝意。", "chinese-red");
        yield return Item("耶穌受難節", Statutory, D(2026, 4, 3), D(2026, 4, 3), "",
            "受難節平安", "靜思默想，願您平安。", "memorial-gray");
        yield return Item("耶穌受難節翌日", Statutory, D(2026, 4, 4), D(2026, 4, 4), "",
            "受難節翌日", "延續靜思，願您安寧。", "memorial-gray");
        yield return Item("清明節翌日", Statutory, D(2026, 4, 6), D(2026, 4, 6), "",
            "清明安康", "慎終追遠，緬懷先人。", "mars-green");
        yield return Item("復活節星期一翌日", Statutory, D(2026, 4, 7), D(2026, 4, 7), "",
            "復活節快樂", "新生與希望，願您喜樂。", "klein-blue");
        yield return Item("勞動節", Statutory, D(2026, 5, 1), D(2026, 5, 1), "",
            "勞動節快樂", "致敬每一位勞動者。", "titian-red");
        yield return Item("佛誕翌日", Statutory, D(2026, 5, 25), D(2026, 5, 25), "",
            "佛誕吉祥", "願慈悲與智慧常伴左右。", "mars-green");
        yield return Item("端午節", Statutory, D(2026, 6, 19), D(2026, 6, 19), "",
            "端午安康", "粽葉飄香，平安順遂。", "mars-green");
        yield return Item("香港特別行政區成立紀念日", Statutory, D(2026, 7, 1), D(2026, 7, 1), "",
            "香港回歸紀念", "祝福香港繁榮穩定。", "klein-blue");
        yield return Item("中秋節翌日", Statutory, D(2026, 9, 26), D(2026, 9, 26), "",
            "中秋快樂", "月圓人團圓，幸福常相伴。", "chinese-red");
        yield return Item("國慶日", Statutory, D(2026, 10, 1), D(2026, 10, 1), "",
            "國慶快樂", "祝福祖國繁榮昌盛。", "chinese-red");
        yield return Item("重陽節翌日", Statutory, D(2026, 10, 19), D(2026, 10, 19), "",
            "重陽安康", "敬老尊賢，登高望遠。", "burgundy");
        yield return Item("聖誕節", Statutory, D(2026, 12, 25), D(2026, 12, 25), "",
            "聖誕快樂", "平安喜樂，與您同在。", "burgundy");
        yield return Item("聖誕節後第一個周日", Statutory, D(2026, 12, 26), D(2026, 12, 26), "",
            "聖誕假期", "延續節日喜悅，願您舒心。", "burgundy");
    }

    /// <summary>
    /// 美国 2026 年联邦公众假期（OPM 2026 Federal Holiday Schedule；公司 3000）
    /// </summary>
    private static IEnumerable<TaktHolidaySeedItem> GetUnitedStates2026Items()
    {
        yield return Item("New Year's Day", Statutory, D(2026, 1, 1), D(2026, 1, 1), "",
            "Happy New Year", "Cheers to a fresh start in 2026.", "klein-blue");
        yield return Item("Martin Luther King Jr. Day", Statutory, D(2026, 1, 19), D(2026, 1, 19), "",
            "Honoring Dr. King", "Celebrate service, dignity, and justice.", "prussian-blue");
        yield return Item("Washington's Birthday", Statutory, D(2026, 2, 16), D(2026, 2, 16), "",
            "Presidents Day", "Reflect on leadership and unity.", "prussian-blue");
        yield return Item("Memorial Day", Statutory, D(2026, 5, 25), D(2026, 5, 25), "",
            "Memorial Day", "Remember those who served.", "memorial-gray");
        yield return Item("Juneteenth National Independence Day", Statutory, D(2026, 6, 19), D(2026, 6, 19), "",
            "Happy Juneteenth", "Freedom, resilience, and progress.", "burgundy");
        yield return Item("Independence Day (Observed)", Statutory, D(2026, 7, 3), D(2026, 7, 3), "",
            "Happy Independence Day", "Liberty and unity for all.", "klein-blue");
        yield return Item("Labor Day", Statutory, D(2026, 9, 7), D(2026, 9, 7), "",
            "Happy Labor Day", "Honoring the workforce that builds the nation.", "titian-red");
        yield return Item("Columbus Day", Statutory, D(2026, 10, 12), D(2026, 10, 12), "",
            "Columbus Day", "A day of reflection and discovery.", "van-dyke-brown");
        yield return Item("Veterans Day", Statutory, D(2026, 11, 11), D(2026, 11, 11), "",
            "Veterans Day", "Thank you for your service.", "memorial-gray");
        yield return Item("Thanksgiving Day", Statutory, D(2026, 11, 26), D(2026, 11, 26), "",
            "Happy Thanksgiving", "Gratitude turns ordinary days into blessings.", "titian-red");
        yield return Item("Christmas Day", Statutory, D(2026, 12, 25), D(2026, 12, 25), "",
            "Merry Christmas", "Peace and joy to you and yours.", "burgundy");
    }

    /// <summary>
    /// 日本 2026 年国民祝日（内阁府祝日一覧・振替休日及び国民の休日）
    /// </summary>
    private static IEnumerable<TaktHolidaySeedItem> GetJapan2026Items()
    {
        yield return Item("元日", Statutory, D(2026, 1, 1), D(2026, 1, 1), "",
            "明けましておめでとうございます", "新しい年のご健勝をお祈りいたします。", "klein-blue");
        yield return Item("成人の日", Statutory, D(2026, 1, 12), D(2026, 1, 12), "",
            "成人の日おめでとうございます", "新たな一歩を応援します。", "tiffany-blue");
        yield return Item("建国記念の日", Statutory, D(2026, 2, 11), D(2026, 2, 11), "",
            "建国記念の日", "国の発展と平和を願います。", "prussian-blue");
        yield return Item("天皇誕生日", Statutory, D(2026, 2, 23), D(2026, 2, 23), "",
            "天皇誕生日", "国の繁栄と国民の幸福を祈念します。", "prussian-blue");
        yield return Item("春分の日", Statutory, D(2026, 3, 20), D(2026, 3, 20), "",
            "春分の日", "自然と調和した穏やかな一日を。", "mars-green");
        yield return Item("昭和の日", Statutory, D(2026, 4, 29), D(2026, 4, 29), "",
            "昭和の日", "激動の歴史を振り返り、未来へ。", "van-dyke-brown");
        yield return Item("憲法記念日", Statutory, D(2026, 5, 3), D(2026, 5, 3), "",
            "憲法記念日", "平和と民主主義を大切に。", "klein-blue");
        yield return Item("みどりの日", Statutory, D(2026, 5, 4), D(2026, 5, 4), "",
            "みどりの日", "自然に親しみ、環境を守りましょう。", "mars-green");
        yield return Item("こどもの日", Statutory, D(2026, 5, 5), D(2026, 5, 5), "",
            "こどもの日", "子どもたちの健やかな成長を願います。", "tiffany-blue");
        yield return Item("振替休日", Statutory, D(2026, 5, 6), D(2026, 5, 6), "",
            "振替休日", "憲法記念日の振替休日です。", "memorial-gray");
        yield return Item("海の日", Statutory, D(2026, 7, 20), D(2026, 7, 20), "",
            "海の日", "海の恩恵に感謝し、豊かな未来へ。", "klein-blue");
        yield return Item("山の日", Statutory, D(2026, 8, 11), D(2026, 8, 11), "",
            "山の日", "山岳の恵みと安全な登山を。", "mars-green");
        yield return Item("敬老の日", Statutory, D(2026, 9, 21), D(2026, 9, 21), "",
            "敬老の日", "長い年月に感謝し、敬意を表します。", "burgundy");
        yield return Item("国民の休日", Statutory, D(2026, 9, 22), D(2026, 9, 22), "",
            "国民の休日", "シルバーウィークの国民の休日です。", "chinese-red");
        yield return Item("秋分の日", Statutory, D(2026, 9, 23), D(2026, 9, 23), "",
            "秋分の日", "収穫と調和の季節を迎えます。", "titian-red");
        yield return Item("スポーツの日", Statutory, D(2026, 10, 12), D(2026, 10, 12), "",
            "スポーツの日", "スポーツで心身ともに健やかに。", "klein-blue");
        yield return Item("文化の日", Statutory, D(2026, 11, 3), D(2026, 11, 3), "",
            "文化の日", "自由と平和を愛し、文化を伸ばす。", "bordeaux");
        yield return Item("勤労感謝の日", Statutory, D(2026, 11, 23), D(2026, 11, 23), "",
            "勤労感謝の日", "勤労をたたえ、生産を祝い、相互に感謝します。", "titian-red");
    }

    /// <summary>
    /// 创建或更新假日
    /// </summary>
    /// <param name="repository">假日种子仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="seed">种子条目</param>
    /// <returns>假日实体与插入/更新计数</returns>
    private static async Task<(TaktHoliday Holiday, int InsertCount, int UpdateCount)> CreateOrUpdateHolidayAsync(
        ITaktCompanySeedRepository<TaktHoliday> repository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        TaktHolidaySeedItem seed)
    {
        var startDate = seed.StartDate.Date;
        var endDate = seed.EndDate.Date;
        var daysCount = ResolveDaysCount(startDate, endDate);

        var holiday = await repository.FirstAsync(h =>
            h.TenantCode == tenantCode
            && h.CompanyCode == companyCode
            && h.StartDate == startDate
            && h.HolidayName == seed.HolidayName);

        if (holiday == null)
        {
            holiday = new TaktHoliday
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                PlantCode = plantCode,
                CultureCode = cultureCode,
                HolidayName = seed.HolidayName,
                HolidayType = seed.HolidayType,
                StartDate = startDate,
                EndDate = endDate,
                DaysCount = daysCount,
                CompensatoryWorkDates = seed.CompensatoryWorkDates ?? string.Empty,
                IsPaid = 1,
                HolidayGreeting = seed.HolidayGreeting,
                HolidayQuote = seed.HolidayQuote,
                HolidayTheme = seed.HolidayTheme,
            };
            holiday = await repository.CreateAsync(holiday);
            return (holiday, 1, 0);
        }

        // 存在：更新记录（排除唯一索引字段）
        holiday.HolidayType = seed.HolidayType;
        holiday.EndDate = endDate;
        holiday.DaysCount = daysCount;
        holiday.CompensatoryWorkDates = seed.CompensatoryWorkDates ?? string.Empty;
        holiday.IsPaid = 1;
        holiday.HolidayGreeting = seed.HolidayGreeting;
        holiday.HolidayQuote = seed.HolidayQuote;
        holiday.HolidayTheme = seed.HolidayTheme;
        holiday.PlantCode = plantCode;
        holiday.CultureCode = cultureCode;

        await repository.UpdateAsync(holiday);

        return (holiday, 0, 1);
    }

    /// <summary>
    /// 计算假期天数（含起止日）
    /// </summary>
    /// <param name="startDate">开始日期</param>
    /// <param name="endDate">结束日期</param>
    /// <returns>天数；结束早于开始时返回 0</returns>
    private static int ResolveDaysCount(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
        {
            return 0;
        }
        return checked(endDate.Subtract(startDate).Days + 1);
    }

    private static DateTime D(int year, int month, int day) => new(year, month, day);

    /// <summary>
    /// 组装假日种子条目（CompensatoryWorkDates：上班日=&gt;所补放假日，与国务院上班日一致）
    /// </summary>
    private static TaktHolidaySeedItem Item(
        string holidayName,
        int holidayType,
        DateTime startDate,
        DateTime endDate,
        string compensatoryWorkDates,
        string greeting,
        string quote,
        string theme) =>
        new(holidayName, holidayType, startDate, endDate, compensatoryWorkDates, greeting, quote, theme);

    /// <summary>
    /// 假日种子条目
    /// </summary>
    /// <param name="HolidayName">假日名称</param>
    /// <param name="HolidayType">假日类型</param>
    /// <param name="StartDate">开始日期</param>
    /// <param name="EndDate">结束日期</param>
    /// <param name="CompensatoryWorkDates">调休对应（yyyy-MM-dd=&gt;yyyy-MM-dd;…；无可空串）</param>
    /// <param name="HolidayGreeting">问候语</param>
    /// <param name="HolidayQuote">引用文案</param>
    /// <param name="HolidayTheme">主题色 key</param>
    private sealed record TaktHolidaySeedItem(
        string HolidayName,
        int HolidayType,
        DateTime StartDate,
        DateTime EndDate,
        string CompensatoryWorkDates,
        string HolidayGreeting,
        string HolidayQuote,
        string HolidayTheme);
}
