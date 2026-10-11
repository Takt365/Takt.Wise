// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktPlantSeedData.cs
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂种子数据初始化
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 工厂种子数据初始化
/// 每个租户按 <c>Database:PlantCodes</c> 顺序初始化工厂主档；
/// <c>RelatedCompany</c>/<c>CultureCode</c> 仅由 CompanyCodes↔PlantCodes↔CultureCodes 同序映射解析，禁止手写对照
/// 幂等性操作：存在则更新，不存在则创建
/// </summary>
public class TaktPlantSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在公司种子之后）
    /// </summary>
    public int Order => 6;

    /// <summary>
    /// 初始化工厂种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化工厂种子数据...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过工厂种子数据初始化");
            return (0, 0);
        }
        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktPlant>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var configuredPlantCodes = database.PlantCodes;
        var seedByCode = GetStandardPlants().ToDictionary(s => s.PlantCode, StringComparer.Ordinal);
        int insertCount = 0;
        int updateCount = 0;
        TaktLogger.Information(
            "正在为租户 {TenantCode} 初始化工厂数据（顺序: {PlantCodes}）...",
            tenantCode,
            string.Join(", ", configuredPlantCodes));
        for (var index = 0; index < configuredPlantCodes.Count; index++)
        {
            var plantCode = configuredPlantCodes[index];
            if (!seedByCode.TryGetValue(plantCode, out var seedDefinition))
            {
                throw new InvalidOperationException(
                    $"Database:PlantCodes 中的工厂编码 {plantCode} 未定义种子数据");
            }
            var seed = seedDefinition with
            {
                SortOrder = index + 1,
                RelatedCompany = database.GetCompanyCodeForPlantCode(plantCode),
                CultureCode = database.GetCultureCodeForPlantCode(plantCode),
            };
            var (_, inserted, updated) = await CreateOrUpdatePlantAsync(repository, tenantCode, seed);
            insertCount += inserted;
            updateCount += updated;
        }
        TaktLogger.Information("工厂种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 标准工厂种子目录（实际初始化范围由 Database:PlantCodes 决定）
    /// </summary>
    private static List<TaktPlantSeedItem> GetStandardPlants()
    {
        return
        [
            new TaktPlantSeedItem(
                "C001", "Takt Technologies Co., Ltd.", "TKC", "TKC", "zh-CN", "0001",
                "150", "I", "XS",
                "软件开发；软件销售；信息系统集成服务；智能控制系统集成；信息技术咨询服务；技术服务、技术开发、技术咨询、技术交流、技术转让、技术推广；电子产品销售；电力电子元器件销售；计算机软硬件及辅助设备零售；办公设备销售；办公设备耗材销售；电子专用设备销售；数字视频监控系统制造；机械零件、零部件销售；机械设备销售。（除依法须经批准的项目外，凭营业执照依法自主开展经营活动）",
                "CN", "北京市", "海淀区", "北京市海淀区中关村", "中关村软件园二期",
                "CN", "北京市", "海淀区", "北京市海淀区中关村", "中关村软件园二期",
                "北京市海淀区中关村", "中关村软件园二期",
                "+86-10-62600001", "info@takt365.com", "+86-10-62600002", "https://www.takt365.cn",
                "91110108MA01234567", "91110108MA01234567", "张三",
                1000000m, new DateTime(2010, 1, 1), 1),
            new TaktPlantSeedItem(
                "J100", "ティアック株式会社", "TCJ", "TCJ", "ja-JP", "1000",
                "160", "C", "S",
                "音響機器事業：プレミアムオーディオ機器（ESOTERICブランド）、ハイエンドオーディオ製品（TEACブランド）、業務用・音楽制作用音響機器（TASCAMブランド）の開発・製造・販売、情報機器事業：計測機器（データレコーダー、センサー・トランスデューサー）、医用画像記録再生機器、航空機搭載用記録再生機器（IFE）、産業用光ドライブ等の開発・製造・販売、ならびにソリューションビジネス・EMS事業",
                "JP", "东京都", "多摩市", "〒206-8530　東京都多摩市落合1丁目47番地", "TEAC本社",
                "JP", "东京都", "多摩市", "〒206-8530　東京都多摩市落合1丁目47番地", "TEAC本社",
                "〒206-8530　東京都多摩市落合1丁目47番地", "TEAC本社",
                "+81-42-374-1311", "info@teac.co.jp", "+81-42-374-1312", "https://www.teac.co.jp",
                "T4010001234567", "T4010001234567", "山下 太郎",
                2000000m, new DateTime(1953, 8, 1), 2),
            new TaktPlantSeedItem(
                "C100", "东莞蒂雅克电子有限公司", "DTA", "DTA", "zh-CN", "2300",
                "330", "C", "S",
                "电子元器件制造；电子元器件批发；电子元器件零售；音响设备制造；音响设备销售；影视录放设备制造；家用视听设备销售；电气信号设备装置制造；电气信号设备装置销售；日用电器修理。",
                "CN", "广东省", "东莞市", "广东省东莞市长安镇上角社区上兴路1号", "上角社区",
                "CN", "广东省", "东莞市", "广东省东莞市长安镇上角社区上兴路1号", "上角社区",
                "广东省东莞市长安镇上角社区上兴路1号", "上角社区",
                "+86-769-85331234", "info@teac.com.cn", "+86-769-85331235", "https://www.teac.com.cn",
                "91441900712345678X", "91441900712345678X", "王二",
                1000000m, new DateTime(1996, 3, 16), 3),
            new TaktPlantSeedItem(
                "H100", "TEAC AUDIO (CHINA) CO., LTD.", "TAC", "TAC", "zh-HK", "2400",
                "330", "C", "S",
                "Wholesale of miscellaneous durable goods / Import and Export Trading",
                "HK", "香港特别行政区", "沙田", "沙田小瀝源安心街19號匯貿中心8樓9室", "匯貿中心",
                "HK", "香港特别行政区", "沙田", "沙田小瀝源安心街19號匯貿中心8樓9室", "匯貿中心",
                "沙田小瀝源安心街19號匯貿中心8樓9室", "匯貿中心",
                "+852-26461234", "tac@teac.com.hk", "+852-26461235", "https://www.teac.com.hk",
                "12345678-000-08-23-A", "12345678-000-08-23-A", "赵六",
                500000m, new DateTime(1995, 12, 1), 4),
            new TaktPlantSeedItem(
                "C270", "蒂雅克商贸(深圳)有限公司", "TSZ", "TSZ", "zh-CN", "2700",
                "330", "C", "S",
                "音响设备及其零配件、电子产品及其零配件、电子元器件的研发、批发、佣金代理（拍卖除外）、进出口及相关配套服务。",
                "CN", "广东省", "深圳市", "深圳市福田区深南大道南泰然九路西喜年中心A座817房", "喜年中心A座",
                "CN", "广东省", "深圳市", "深圳市福田区深南大道南泰然九路西喜年中心A座817房", "喜年中心A座",
                "深圳市福田区深南大道南泰然九路西喜年中心A座817房", "喜年中心A座",
                "+86-755-82851234", "tsz@teac.com.cn", "+86-755-82851235", "https://www.teac.com.cn",
                "91440300765432109X", "91440300765432109X", "孙八",
                500000m, new DateTime(2012, 1, 1), 5),
            new TaktPlantSeedItem(
                "A300", "TEAC AMERICA, INC.", "TCA", "TCA", "en-US", "3000",
                "330", "C", "S",
                "Engaged in the distribution, wholesale, import/export, and after-sales service of high-fidelity audio equipment (Hi-Fi / Esoteric brand), consumer audio-video electronics, professional audio recording equipment (TASCAM brand), and computer data storage and peripheral devices (including CD/DVD drives and disc publishing systems) throughout the United States and North America.",
                "US", "California", "Santa Fe Springs", "14525 Valley View Ave., Suite I, Santa Fe Springs, California 90670, U.S.A", "Suite I",
                "US", "California", "Santa Fe Springs", "14525 Valley View Ave., Suite I, Santa Fe Springs, California 90670, U.S.A", "Suite I",
                "14525 Valley View Ave., Suite I, Santa Fe Springs, California 90670, U.S.A", "Suite I",
                "+1-562-903-9600", "info@teac-audio.com", "+1-562-903-9601", "https://www.teac-audio.com",
                "95-1234567", "95-1234567", "John Smith",
                3000000m, new DateTime(1967, 5, 1), 6),
        ];
    }

    /// <summary>
    /// 创建或更新工厂
    /// </summary>
    private static async Task<(TaktPlant Plant, int InsertCount, int UpdateCount)> CreateOrUpdatePlantAsync(
        ITaktTenantSeedRepository<TaktPlant> repository,
        string tenantCode,
        TaktPlantSeedItem seed)
    {
        var plant = await repository.FirstAsync(p => p.TenantCode == tenantCode && p.PlantCode == seed.PlantCode);
        if (plant == null)
        {
            plant = new TaktPlant { TenantCode = tenantCode };
            ApplySeed(plant, seed);
            await repository.CreateAsync(plant);
            return (plant, 1, 0);
        }
        ApplySeed(plant, seed);
        await repository.UpdateAsync(plant);
        return (plant, 0, 1);
    }

    /// <summary>
    /// 将种子项写入实体（与 TaktPlant 字段一一对应；仅 ClosingDate 可为 null）
    /// </summary>
    private static void ApplySeed(TaktPlant plant, TaktPlantSeedItem seed)
    {
        plant.PlantCode = seed.PlantCode;
        plant.PlantName1 = seed.PlantName1;
        plant.PlantShortName = seed.PlantShortName;
        plant.CodeAlias = seed.CodeAlias;
        plant.CultureCode = seed.CultureCode;
        plant.EnterpriseNature = seed.EnterpriseNature;
        plant.IndustryAttribute = seed.IndustryAttribute;
        plant.EnterpriseScale = seed.EnterpriseScale;
        plant.BusinessScope = seed.BusinessScope;
        plant.RegistrationRegion = seed.RegistrationRegion;
        plant.RegistrationProvince = seed.RegistrationProvince;
        plant.RegistrationCity = seed.RegistrationCity;
        plant.RegistrationAddress1 = seed.RegistrationAddress1;
        plant.RegistrationAddress2 = seed.RegistrationAddress2;
        plant.BusinessRegion = seed.BusinessRegion;
        plant.BusinessProvince = seed.BusinessProvince;
        plant.BusinessCity = seed.BusinessCity;
        plant.BusinessAddress1 = seed.BusinessAddress1;
        plant.BusinessAddress2 = seed.BusinessAddress2;
        plant.PlantAddress1 = seed.PlantAddress1;
        plant.PlantAddress2 = seed.PlantAddress2;
        plant.PlantPhone = seed.PlantPhone;
        plant.PlantEmail = seed.PlantEmail;
        plant.PlantFax = seed.PlantFax;
        plant.PlantWebsite = seed.PlantWebsite;
        plant.UnifiedSocialCreditCode = seed.UnifiedSocialCreditCode;
        plant.TaxRegistrationNumber = seed.TaxRegistrationNumber;
        plant.LegalRepresentative = seed.LegalRepresentative;
        plant.RegisteredCapital = seed.RegisteredCapital;
        plant.EstablishmentDate = seed.EstablishmentDate;
        plant.ClosingDate = null;
        plant.PlantExistence = 1;
        plant.RelatedCompany = seed.RelatedCompany;
        plant.SalesOrganization = seed.RelatedCompany;
        plant.PurchasingOrganization = seed.PlantCode;
        plant.ValuationArea = seed.PlantCode;
        plant.PlantStatus = 1;
        plant.SortOrder = seed.SortOrder;
        plant.Remark = TaktZeroCharHelper.InterleaveDisplayName(seed.PlantName1);
    }

    /// <summary>
    /// 工厂种子项（字段与 TaktPlant 对齐，IsNullable=false 列均须非空字符串或有效值）
    /// </summary>
    private sealed record TaktPlantSeedItem(
        string PlantCode,
        string PlantName1,
        string PlantShortName,
        string CodeAlias,
        string CultureCode,
        string RelatedCompany,
        string EnterpriseNature,
        string IndustryAttribute,
        string EnterpriseScale,
        string BusinessScope,
        string RegistrationRegion,
        string RegistrationProvince,
        string RegistrationCity,
        string RegistrationAddress1,
        string RegistrationAddress2,
        string BusinessRegion,
        string BusinessProvince,
        string BusinessCity,
        string BusinessAddress1,
        string BusinessAddress2,
        string PlantAddress1,
        string PlantAddress2,
        string PlantPhone,
        string PlantEmail,
        string PlantFax,
        string PlantWebsite,
        string UnifiedSocialCreditCode,
        string TaxRegistrationNumber,
        string LegalRepresentative,
        decimal RegisteredCapital,
        DateTime EstablishmentDate,
        int SortOrder);
}
