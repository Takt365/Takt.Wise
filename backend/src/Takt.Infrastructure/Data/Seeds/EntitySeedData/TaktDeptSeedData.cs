// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktDeptSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：部门种子；按公司各写一支：Company 1000=0000→1000→日本；2300=0000→2300→东莞；2400=0000→2400→香港
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 部门种子（按公司隔离，各写一支，禁止把三支整树复制到每个 CompanyCode）：
/// Company 1000：0000 TEAC → 1000 TCJ → 日本各部门；
/// Company 2300：0000 TEAC → 2300 DTA → 东莞各部门；
/// Company 2400：0000 TEAC → 2400 TAC → 香港各部门。
/// 幂等：存在则更新，不存在则创建。
/// </summary>
public class TaktDeptSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在菜单之后，岗位之前）
    /// </summary>
    public int Order => 30;

    /// <summary>
    /// 初始化部门种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化部门种子数据...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过部门种子数据初始化");
            return (0, 0);
        }
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktDept>>();
        var sqlSugarContext = serviceProvider.GetRequiredService<TaktSeedContext>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var configuredCompanyCodes = database.CompanyCodes;
        int insertCount = 0;
        int updateCount = 0;
        var companies = await companyRepository.GetListAsync(c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过部门种子数据初始化", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            configuredCompanyCodes,
            companies,
            c => c.CompanyCode);
        TaktLogger.Information(
            "正在为租户 {TenantCode} 初始化部门数据（顺序: {CompanyCodes}）...",
            tenantCode,
            string.Join(", ", configuredCompanyCodes));
        foreach (var company in orderedCompanies)
        {
            TaktLogger.Information("正在为公司 {CompanyCode} ({CompanyName1}) 初始化部门...", company.CompanyCode, company.CompanyName1);
            var plantCode = database.GetPlantCodeForCompanyCode(company.CompanyCode);
            try
            {
                var result = company.CompanyCode switch
                {
                    "1000" => await SeedJapanCompanyOrgAsync(repository, sqlSugarContext, tenantCode, company.CompanyCode, plantCode, company.CultureCode),
                    "2300" => await SeedDtaCompanyOrgAsync(repository, sqlSugarContext, tenantCode, company.CompanyCode, plantCode, company.CultureCode),
                    "2400" => await SeedTacCompanyOrgAsync(repository, sqlSugarContext, tenantCode, company.CompanyCode, plantCode, company.CultureCode),
                    _ => await SeedGenericRootOnlyAsync(repository, sqlSugarContext, tenantCode, company.CompanyCode, plantCode, company.CultureCode, company.CompanyName1),
                };
                insertCount += result.InsertCount;
                updateCount += result.UpdateCount;
            }
            catch (Exception ex)
            {
                TaktLogger.Error(
                    ex,
                    "公司 {CompanyCode} 部门种子失败，继续下一公司（租户 {TenantCode}）",
                    company.CompanyCode,
                    tenantCode);
            }
        }
        TaktLogger.Information("租户 {TenantCode} 部门种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            tenantCode, insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 公司 1000：0000 → 1000 TCJ → 日本各部门（不含 2300/2400 支）
    /// </summary>
    private static async Task<(int InsertCount, int UpdateCount)> SeedJapanCompanyOrgAsync(
        ITaktCompanySeedRepository<TaktDept> repository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode)
    {
        var (_, tcj, skeletonInsert, skeletonUpdate) = await SeedRootAndBranchNodeAsync(
            repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode,
            "1000", "TCJ", "TCJ", 1);
        var japan = await SeedJapanBranchAsync(repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode, tcj);
        return (skeletonInsert + japan.InsertCount, skeletonUpdate + japan.UpdateCount);
    }

    /// <summary>
    /// 公司 2300：0000 → 2300 DTA → 东莞各部门（不含 1000/2400 支）
    /// </summary>
    private static async Task<(int InsertCount, int UpdateCount)> SeedDtaCompanyOrgAsync(
        ITaktCompanySeedRepository<TaktDept> repository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode)
    {
        var (_, dta, skeletonInsert, skeletonUpdate) = await SeedRootAndBranchNodeAsync(
            repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode,
            "2300", "DTA", "DTA", 1);
        var branch = await SeedDtaBranchAsync(repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode, dta);
        return (skeletonInsert + branch.InsertCount, skeletonUpdate + branch.UpdateCount);
    }

    /// <summary>
    /// 公司 2400：0000 → 2400 TAC → 香港各部门（不含 1000/2300 支）
    /// </summary>
    private static async Task<(int InsertCount, int UpdateCount)> SeedTacCompanyOrgAsync(
        ITaktCompanySeedRepository<TaktDept> repository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode)
    {
        var (_, tac, skeletonInsert, skeletonUpdate) = await SeedRootAndBranchNodeAsync(
            repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode,
            "2400", "TAC", "TAC", 1);
        var branch = await SeedTacBranchAsync(repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode, tac);
        return (skeletonInsert + branch.InsertCount, skeletonUpdate + branch.UpdateCount);
    }

    /// <summary>
    /// 日本本社各部门（挂在 1000 TCJ 下）
    /// </summary>
    private static async Task<(int InsertCount, int UpdateCount)> SeedJapanBranchAsync(
        ITaktCompanySeedRepository<TaktDept> repository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        TaktDept tcj)
    {
        int insertCount = 0;
        int updateCount = 0;
        void Acc((TaktDept Dept, bool Inserted) r)
        {
            insertCount += r.Inserted ? 1 : 0;
            updateCount += r.Inserted ? 0 : 1;
        }
        async Task<(TaktDept Dept, bool Inserted)> SeedChild(
            string code, string nameJa, string nameEn, string? manager, long parentId, int sort, int costCategory = 2)
        {
            var shortName = code.Length <= 6 ? code : code[..6];
            var description = string.IsNullOrWhiteSpace(manager) || manager == "-"
                ? null
                : $"責任者: {manager.Trim()}";
            return await CreateOrUpdateDeptAsync(
                repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode,
                code, nameJa, shortName, costCategory, parentId, sort, description, nameEn, null);
        }
        // 総務人事部（挂在 TCJ 1000 下）
        var d1100 = await SeedChild("1100", "総務人事部", "General Affairs and Personnel Department", "秋野　浩隆", tcj.Id, 1);
        Acc(d1100);
        Acc(await SeedChild("1110", "人事労政課", "Human Resources and Employee Relations Sec., General Affairs and Personnel Department", "友井　寿美子", d1100.Dept.Id, 1));
        Acc(await SeedChild("1120", "総務課", "General Affairs Sec., General Affairs and Personnel Department", "永山　貴雄", d1100.Dept.Id, 2));
        Acc(await SeedChild("1170", "秘書課", "Secretarial Section, General Affairs and Personnel Department", "秋野　浩隆", d1100.Dept.Id, 3));
        Acc(await SeedChild("1510", "法務課", "Legal Affairs Section, General Affairs and Personnel Department", "青木　律子", d1100.Dept.Id, 4));
        Acc(await SeedChild("1520", "知的財産課", "Intellectual Property Section, General Affairs and Personnel Department", "江頭　潤", d1100.Dept.Id, 5));
        Acc(await SeedChild("1149", "部付ティアック労働組合専従", "TEAC WORKER'S UNION", "-", d1100.Dept.Id, 6));
        Acc(await SeedChild("1160", "部付その他", "General Affairs and Personnel Department (Others)", "-", d1100.Dept.Id, 7));
        // 財務企画部
        var d1200 = await SeedChild("1200", "財務企画部", "Corporate Finance and Planning Department", "福田　浩一", tcj.Id, 2);
        Acc(d1200);
        Acc(await SeedChild("1210", "財務課", "Credit Section, Corporate Finance and Planning Department", "福田　浩一", d1200.Dept.Id, 1));
        Acc(await SeedChild("1220", "経理課", "Accounting Section, Corporate Finance and Planning Department", "髙島　勇樹", d1200.Dept.Id, 2));
        Acc(await SeedChild("1430", "経営情報課", "Business Intelligence Section, Corporate Finance and Planning Department", "稲場　靖之", d1200.Dept.Id, 3));
        // 内部監査室
        Acc(await SeedChild("9800", "内部監査室", "Internal Audit Department", "-", tcj.Id, 3));
        // SCM本部
        var d9200 = await SeedChild("9200", "SCM本部", "Supply-Chain Management Headquarters", "山亀　浩康", tcj.Id, 4);
        Acc(d9200);
        var d1800 = await SeedChild("1800", "管理・購買部", "Administration and Purchasing Department, Supply-Chain Management Headquarters", "菊地　和子", d9200.Dept.Id, 1);
        Acc(d1800);
        Acc(await SeedChild("1820", "購買課", "Purchasing Section, Administration and Purchasing Department, Supply-Chain Management Headquarters", "小浦　龍司", d1800.Dept.Id, 1));
        Acc(await SeedChild("1810", "管理課", "Operations Section, Administration and Purchasing Department, Supply-Chain Management Headquarters", "濱田　高志", d1800.Dept.Id, 2));
        Acc(await SeedChild("1250", "貿易課", "Trade Operation Section, Administration and Purchasing Department, Supply-Chain Management Headquarters", "岡田　規子", d1800.Dept.Id, 3));
        var d1900 = await SeedChild("1900", "品質保証・技術部", "Quality Assurance and Engineering Department, Supply-Chain Management Headquarters", "細谷　文彦", d9200.Dept.Id, 2);
        Acc(d1900);
        Acc(await SeedChild("1920", "品質保証課", "Quality Assurance Section, Quality Assurance and Engineering Department, Supply-Chain Management Headquarters", "奥　朋則", d1900.Dept.Id, 1));
        Acc(await SeedChild("1940", "安全規格・環境管理課", "Product Safety Standards and Environment Control Section, Quality Assurance and Engineering Department, Supply-Chain Management Headquarters", "本間　聡", d1900.Dept.Id, 2));
        Acc(await SeedChild("1720", "生産技術課", "Engineering Section, Quality Assurance and Engineering Department, Supply-Chain Management Headquarters", "鴇田　雄飛", d1900.Dept.Id, 3));
        // プレミアムオーディオ事業部
        var d9701 = await SeedChild("9701", "プレミアムオーディオ事業部", "Premium Audio Division", "加藤　徹也", tcj.Id, 5);
        Acc(d9701);
        var d3100 = await SeedChild("3100", "国内営業部", "Domestic Sales Department, Premium Audio Division", "土井浦　良和", d9701.Dept.Id, 1, 1);
        Acc(d3100);
        Acc(await SeedChild("3110", "営業1課", "Sales Section 1, Domestic Sales Department, Premium Audio Division", "安井　健太郎", d3100.Dept.Id, 1, 1));
        Acc(await SeedChild("3120", "営業2課", "Sales Section 2, Domestic Sales Department, Premium Audio Division", "土井浦　良和", d3100.Dept.Id, 2, 1));
        var d3200 = await SeedChild("3200", "海外営業・販売促進部", "International Sales and Marketing Department, Premium Audio Division", "杉浦　烈", d9701.Dept.Id, 2, 1);
        Acc(d3200);
        Acc(await SeedChild("3530", "海外営業課", "International Sales Section, International Sales and Marketing Department, Premium Audio Division", "杉浦　烈", d3200.Dept.Id, 1, 1));
        Acc(await SeedChild("3540", "販売促進課", "Marketing and Communications Section, International Sales and Marketing Department, Premium Audio Division", "覚前　克彦", d3200.Dept.Id, 2, 1));
        Acc(await SeedChild("3560", "プロダクト課", "Product Section, International Sales and Marketing Department, Premium Audio Division", "吉田　穣", d3200.Dept.Id, 3, 1));
        var d2100 = await SeedChild("2100", "業務推進部", "Promotion Division Department, Premium Audio Division", "加藤　丈和", d9701.Dept.Id, 3);
        Acc(d2100);
        Acc(await SeedChild("2160", "流通管理課", "Distribution Management Section, Promotion Division Department, Premium Audio Division", "加藤　丈和", d2100.Dept.Id, 1));
        Acc(await SeedChild("2170", "イーコマース課", "Electronic Commerce Section, Promotion Division Department, Premium Audio Division", "加藤　丈和", d2100.Dept.Id, 2));
        var d2200 = await SeedChild("2200", "開発部", "Research and Development Department, Premium Audio Division", "新妻　知幸", d9701.Dept.Id, 4);
        Acc(d2200);
        Acc(await SeedChild("2230", "電気設計課", "Electrical Designing Section, Research and Development Department, Premium Audio Division", "仙土　和弘", d2200.Dept.Id, 1));
        Acc(await SeedChild("2240", "機構設計課", "Mechanical Designing Section, Research and Development Department, Premium Audio Division", "新妻　知幸", d2200.Dept.Id, 2));
        Acc(await SeedChild("2210", "コンテンツ制作課", "Content Production Section, Research and Development Department, Premium Audio Division", "東野　真哉", d2200.Dept.Id, 3));
        // タスカム事業部
        var d9703 = await SeedChild("9703", "タスカム事業部", "TASCAM Division", "松野　陽介", tcj.Id, 6);
        Acc(d9703);
        var d3400 = await SeedChild("3400", "国内営業部", "Domestic Sales Department, TASCAM Division", "山本　浩史", d9703.Dept.Id, 1, 1);
        Acc(d3400);
        Acc(await SeedChild("3460", "国内販売課", "Domestic Sales Section, Domestic Sales Department, TASCAM Division", "江見　達彦", d3400.Dept.Id, 1, 1));
        Acc(await SeedChild("3440", "カスタマーサポート課", "Customer Support Section, Domestic Sales Department, TASCAM Division", "加茂　尚広", d3400.Dept.Id, 2, 1));
        Acc(await SeedChild("3450", "放送営業課", "Broadcast Sales Section, Domestic Sales Department, TASCAM Division", "石田　祐一", d3400.Dept.Id, 3, 1));
        Acc(await SeedChild("3470", "PA/SR課", "PA/SR Section, Domestic Sales Department, TASCAM Division", "内田　哲", d3400.Dept.Id, 4, 1));
        var d3700 = await SeedChild("3700", "営業企画部", "Sales and Marketing Department, TASCAM Division", "松野　陽介", d9703.Dept.Id, 2, 1);
        Acc(d3700);
        Acc(await SeedChild("3490", "海外販売課", "International Sales Section, Sales and Marketing Department, TASCAM Division", "野沢　悠", d3700.Dept.Id, 1, 1));
        Acc(await SeedChild("3310", "販売促進課", "Sales Planning Section, Sales and Marketing Department, TASCAM Division", "花田　淳", d3700.Dept.Id, 2, 1));
        Acc(await SeedChild("3480", "流通管理課", "Distribution Management Section, Sales and Marketing Department, TASCAM Division", "中野　拓一", d3700.Dept.Id, 3, 1));
        // 開発統括本部
        var d9910 = await SeedChild("9910", "開発統括本部", "Research and Development Headquarters", "松浦　教夫", tcj.Id, 7);
        Acc(d9910);
        var d36ta = await SeedChild("36TA", "タスカム開発部", "TASCAM Research and Development Department, Research and Development Headquarters", "成田　博宣", d9910.Dept.Id, 1);
        Acc(d36ta);
        Acc(await SeedChild("3670", "ハードウェア開発課", "Hardware Designing Section, TASCAM Research and Development Department, Research and Development Headquarters", "岡　裕彦", d36ta.Dept.Id, 1));
        Acc(await SeedChild("3630", "ファームウェア開発課", "Firmware Designing Section, TASCAM Research and Development Department, Research and Development Headquarters", "千葉　克仁", d36ta.Dept.Id, 2));
        Acc(await SeedChild("3610", "企画開発課", "Product Planning Section, TASCAM Research and Development Department, Research and Development Headquarters", "島田　宏俊", d36ta.Dept.Id, 3));
        Acc(await SeedChild("3603", "アウトソーシング課", "Outsourcing Section, TASCAM Research and Development Department, Research and Development Headquarters", "坂口　充洋", d36ta.Dept.Id, 4));
        var d36ip = await SeedChild("36IP", "情報機器開発部", "Information Products Research and Development Department, Research and Development Headquarters", "亀田　一彦", d9910.Dept.Id, 2);
        Acc(d36ip);
        Acc(await SeedChild("4650", "ハードウェア開発課", "Hardware Designing Section, Information Products Research and Development Department, Research and Development Headquarters", "亀田　一彦", d36ip.Dept.Id, 1));
        Acc(await SeedChild("4660", "ファームウェア開発課", "Firmware Designing Section, Information Products Research and Development Department, Research and Development Headquarters", "亀田　一彦", d36ip.Dept.Id, 2));
        var d36pi = await SeedChild("36PI", "プロダクトインテグレーション部", "Product Integration Department, Research and Development Headquarters", "松浦　教夫", d9910.Dept.Id, 3);
        Acc(d36pi);
        Acc(await SeedChild("3690", "アプリ＆インテグレーション開発課", "Application and Integration Designing Section, Product Integration Department, Research and Development Headquarters", "早坂　要", d36pi.Dept.Id, 1));
        Acc(await SeedChild("3620", "機構開発課", "Mechanical Designing Section, Product Integration Department, Research and Development Headquarters", "松井　尚人", d36pi.Dept.Id, 2));
        Acc(await SeedChild("1710", "開発業務課", "Development Coordination Section, Product Integration Department, Research and Development Headquarters", "小林　誠希", d36pi.Dept.Id, 3));
        Acc(await SeedChild("3330", "デザイン課", "Design Work Section, Product Integration Department, Research and Development Headquarters", "天野　信義", d36pi.Dept.Id, 4));
        // 情報機器事業部
        var d9400 = await SeedChild("9400", "情報機器事業部", "Information Products Division", "小田原　路易", tcj.Id, 8);
        Acc(d9400);
        var d4500 = await SeedChild("4500", "事業推進部", "Business Operation Department, Information Products Division", "中平　繁克", d9400.Dept.Id, 1, 1);
        Acc(d4500);
        Acc(await SeedChild("4520", "販売促進課", "Sales Promotion Section, Business Operation Department, Information Products Division", "中平　繁克", d4500.Dept.Id, 1, 1));
        Acc(await SeedChild("4530", "事業計画課", "Business Planning Section, Business Operation Department, Information Products Division", "中平　繁克", d4500.Dept.Id, 2, 1));
        Acc(await SeedChild("4410", "カスタマーサポート課", "Customer Support Section, Business Operation Department, Information Products Division", "角　大輔", d4500.Dept.Id, 3, 1));
        Acc(await SeedChild("4430", "TRテクニカルソリューション課", "TR Technical Solution Section, Business Operation Department, Information Products Division", "中平　繁克", d4500.Dept.Id, 4, 1));
        var d4800 = await SeedChild("4800", "メジャメントプロダクト営業部", "Measurement Products Sales Department, Information Products Division", "依田　朋之", d9400.Dept.Id, 2, 1);
        Acc(d4800);
        Acc(await SeedChild("4810", "国内営業課", "Domestic Sales Section, Measurement Products Sales Department, Information Products Division", "坂井　利行", d4800.Dept.Id, 1, 1));
        Acc(await SeedChild("4830", "海外営業課", "International Sales Section, Measurement Products Sales Department, Information Products Division", "依田　朋之", d4800.Dept.Id, 2, 1));
        var d4900 = await SeedChild("4900", "イメージングシステムソリューション営業部", "Imaging System Solutions Sales Department, Information Products Division", "五味　健児", d9400.Dept.Id, 3, 1);
        Acc(d4900);
        Acc(await SeedChild("4910", "メディカルシステム国内営業課", "Medical System Domestic Sales Section, Imaging System Solutions Sales Department, Information Products Division", "近藤　純康", d4900.Dept.Id, 1, 1));
        Acc(await SeedChild("4920", "メディカルシステム海外営業課", "Medical System International Sales Section, Imaging System Solutions Sales Department, Information Products Division", "細井　陽一郎", d4900.Dept.Id, 2, 1));
        return (insertCount, updateCount);
    }


    /// <summary>
    /// 东莞 DTA 各部门（挂在 2300 下）
    /// </summary>
    private static async Task<(int InsertCount, int UpdateCount)> SeedDtaBranchAsync(
        ITaktCompanySeedRepository<TaktDept> repository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        TaktDept dta)
    {
        int insertCount = 0;
        int updateCount = 0;
        void Acc(bool inserted)
        {
            insertCount += inserted ? 1 : 0;
            updateCount += inserted ? 0 : 1;
        }
        async Task<(TaktDept Dept, bool IsInserted)> Seed(
            string code, string name, string shortName, int costCategory, long parentId, int sort)
        {
            return await CreateOrUpdateDeptAsync(
                repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode,
                code, name, shortName, costCategory, parentId, sort, null, null, null);
        }
        var (_, i1) = await Seed("D1000", "总经理室", "GM", 1, dta.Id, 1); Acc(i1);
        var (d0100, i2) = await Seed("D0100", "总务部", "GA", 1, dta.Id, 2); Acc(i2);
        var (d0200, i3) = await Seed("D0200", "财务部", "FIN", 1, dta.Id, 3); Acc(i3);
        var (d0300, i4) = await Seed("D0300", "IT部", "IT", 2, dta.Id, 4); Acc(i4);
        var (d0400, i5) = await Seed("D0400", "管理部", "ADM", 1, dta.Id, 5); Acc(i5);
        var (d0500, i6) = await Seed("D0500", "资材部", "MAT", 1, dta.Id, 6); Acc(i6);
        var (d0600, i7) = await Seed("D0600", "生产部", "PROD", 2, dta.Id, 7); Acc(i7);
        var (d0700, i8) = await Seed("D0700", "技术部", "ENG", 2, dta.Id, 8); Acc(i8);
        var (d0800, i9) = await Seed("D0800", "品保部", "QA", 2, dta.Id, 9); Acc(i9);
        var (d0900, i10) = await Seed("D0900", "OEM部", "OEM", 1, dta.Id, 10); Acc(i10);
        Acc((await Seed("D0110", "总务课", "GAS", 1, d0100.Id, 1)).IsInserted);
        Acc((await Seed("D0210", "财务课", "FAC", 1, d0200.Id, 1)).IsInserted);
        Acc((await Seed("D0310", "电脑课", "MIS", 2, d0300.Id, 1)).IsInserted);
        Acc((await Seed("D0410", "报关课", "CUS", 1, d0400.Id, 1)).IsInserted);
        Acc((await Seed("D0420", "生管课", "PC", 1, d0400.Id, 2)).IsInserted);
        Acc((await Seed("D0430", "部管课", "MC", 1, d0400.Id, 3)).IsInserted);
        Acc((await Seed("D0510", "采购课", "PUR", 1, d0500.Id, 1)).IsInserted);
        Acc((await Seed("D0610", "制造1课", "MFG1", 2, d0600.Id, 1)).IsInserted);
        var (d0620, i19) = await Seed("D0620", "制造2课", "MFG2", 2, d0600.Id, 2); Acc(i19);
        Acc((await Seed("D0630", "制造技术课", "ME", 2, d0600.Id, 3)).IsInserted);
        Acc((await Seed("D0621", "SMT", "SMT", 2, d0620.Id, 1)).IsInserted);
        Acc((await Seed("D0622", "自插", "AI", 2, d0620.Id, 2)).IsInserted);
        Acc((await Seed("D0623", "修正", "REW", 2, d0620.Id, 3)).IsInserted);
        Acc((await Seed("D0624", "手插", "MI", 2, d0620.Id, 4)).IsInserted);
        Acc((await Seed("D0625", "物料", "MTL", 1, d0620.Id, 5)).IsInserted);
        Acc((await Seed("D0626", "制造2课-间接", "MFG2I", 2, d0620.Id, 6)).IsInserted);
        Acc((await Seed("D0710", "技术课", "TE", 2, d0700.Id, 1)).IsInserted);
        Acc((await Seed("D0810", "受检课", "IQC", 2, d0800.Id, 1)).IsInserted);
        Acc((await Seed("D0820", "品管课", "QC", 2, d0800.Id, 2)).IsInserted);
        Acc((await Seed("D0910", "OEM QA课", "OEMQA", 2, d0900.Id, 1)).IsInserted);
        Acc((await Seed("D0920", "OEM管理课", "OEMADM", 1, d0900.Id, 2)).IsInserted);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 香港 TAC 各部门（挂在 2400 下）
    /// </summary>
    private static async Task<(int InsertCount, int UpdateCount)> SeedTacBranchAsync(
        ITaktCompanySeedRepository<TaktDept> repository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        TaktDept tac)
    {
        int insertCount = 0;
        int updateCount = 0;
        void Acc(bool inserted)
        {
            insertCount += inserted ? 1 : 0;
            updateCount += inserted ? 0 : 1;
        }
        async Task<(TaktDept Dept, bool IsInserted)> Seed(
            string code, string name, string nameEn, string? manager, int costCategory, long parentId, int sort)
        {
            var shortName = code.Length <= 6 ? code : code[..6];
            var description = string.IsNullOrWhiteSpace(manager) ? null : $"Manager: {manager.Trim()}";
            return await CreateOrUpdateDeptAsync(
                repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode,
                code, name, shortName, costCategory, parentId, sort, description, nameEn, null);
        }
        Acc((await Seed("T000", "General Manager Office", "General Manager Office", null, 2, tac.Id, 1)).IsInserted);
        Acc((await Seed("T100", "Finance Department", "Finance Department", "Kathy Lo", 1, tac.Id, 2)).IsInserted);
        Acc((await Seed("T200", "Materials & Logistics Department", "Materials & Logistics Department", "Joyce Li", 1, tac.Id, 3)).IsInserted);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 未单独建模的公司：0000 → 本公司节点
    /// </summary>
    private static async Task<(int InsertCount, int UpdateCount)> SeedGenericRootOnlyAsync(
        ITaktCompanySeedRepository<TaktDept> repository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        string companyName)
    {
        var rootName = string.IsNullOrWhiteSpace(companyName) ? companyCode : companyName.Trim();
        if (rootName.Length > 100)
        {
            rootName = rootName[..100];
        }
        var shortName = companyCode.Length <= 6 ? companyCode : companyCode[..6];
        var (group, i0) = await CreateOrUpdateDeptAsync(
            repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode,
            "0000", "TEAC", "TEAC", 2, 0, 0,
            "ティアック株式会社", "TEAC CORPORATION", null);
        var (_, i1) = await CreateOrUpdateDeptAsync(
            repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode,
            companyCode, rootName, shortName, 2, group.Id, 1,
            null, rootName, null);
        return ((i0 ? 1 : 0) + (i1 ? 1 : 0), (i0 ? 0 : 1) + (i1 ? 0 : 1));
    }

    /// <summary>
    /// 本公司组织骨架：0000 TEAC（根）→ 唯一分公司节点（1000 或 2300 或 2400）
    /// </summary>
    private static async Task<(TaktDept Group, TaktDept Branch, int InsertCount, int UpdateCount)> SeedRootAndBranchNodeAsync(
        ITaktCompanySeedRepository<TaktDept> repository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        string branchDeptCode,
        string branchName,
        string branchShortName,
        int branchSortOrder)
    {
        int insertCount = 0;
        int updateCount = 0;
        void Acc(bool inserted)
        {
            insertCount += inserted ? 1 : 0;
            updateCount += inserted ? 0 : 1;
        }
        var (group, i0) = await CreateOrUpdateDeptAsync(
            repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode,
            "0000", "TEAC", "TEAC", 2, 0, 0,
            "ティアック株式会社", "TEAC CORPORATION", null);
        Acc(i0);
        var (branch, i1) = await CreateOrUpdateDeptAsync(
            repository, sqlSugarContext, tenantCode, companyCode, plantCode, cultureCode,
            branchDeptCode, branchName, branchShortName, 2, group.Id, branchSortOrder, null, branchName, null);
        Acc(i1);
        return (group, branch, insertCount, updateCount);
    }

    /// <summary>
    /// 仅保留大写英文字母（A–Z）。
    /// </summary>
    /// <param name="value">源字符串</param>
    /// <returns>大写字母串；无字母时为空</returns>
    private static string LettersOnlyUpper(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }
        return new string(value.Where(char.IsAsciiLetter).Select(char.ToUpperInvariant).ToArray());
    }

    /// <summary>
    /// 截断为最多 6 位（部门简称 / ISO 编码列长）。
    /// </summary>
    /// <param name="letters">已为大写字母串</param>
    /// <returns>最多 6 位</returns>
    private static string TruncateEnglishAbbr(string letters)
    {
        if (string.IsNullOrEmpty(letters))
        {
            return string.Empty;
        }
        return letters.Length <= 6 ? letters : letters[..6];
    }

    /// <summary>
    /// 是否为纯数字（日本部门编码作简称时跳过，改从英文名取 ISO）。
    /// </summary>
    /// <param name="value">待判断字符串</param>
    /// <returns>全为数字则为 true</returns>
    private static bool IsAllDigits(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }
        return value.All(char.IsAsciiDigit);
    }

    /// <summary>
    /// 从英文部门名取首字母缩写（跳过虚词），最多 6 位大写。
    /// </summary>
    /// <param name="englishName">英文名或 Remark</param>
    /// <returns>ISO 候选</returns>
    private static string IsoFromEnglishInitials(string? englishName)
    {
        if (string.IsNullOrWhiteSpace(englishName))
        {
            return string.Empty;
        }
        var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "and", "of", "the", "a", "an", "section", "sec", "department", "division",
            "headquarters", "office", "others", "worker", "workers", "union", "for", "in", "to",
        };
        var parts = englishName.Split(
            [' ', ',', '/', '-', '&', '(', ')', '.', '·', '　'],
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var chars = new List<char>(6);
        foreach (var part in parts)
        {
            if (stopWords.Contains(part))
            {
                continue;
            }
            var letter = part.FirstOrDefault(char.IsAsciiLetter);
            if (letter == default)
            {
                continue;
            }
            chars.Add(char.ToUpperInvariant(letter));
            if (chars.Count >= 6)
            {
                break;
            }
        }
        return chars.Count == 0 ? string.Empty : new string(chars.ToArray());
    }

    /// <summary>
    /// 解析部门简称（与 ISO 编码一致，≤6）：优先简称参数，否则英文名首字母，再否则部门编码字母。
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <param name="deptShortName">部门简称候选</param>
    /// <param name="englishName">英文长名称（DeptName2）</param>
    /// <returns>最多 6 位大写英文字母</returns>
    private static string ResolveEnglishAbbr(string deptCode, string deptShortName, string? englishName)
    {
        var fromShort = LettersOnlyUpper(deptShortName);
        if (fromShort.Length >= 2 && !IsAllDigits(deptShortName))
        {
            return TruncateEnglishAbbr(fromShort);
        }
        var fromEn = IsoFromEnglishInitials(englishName);
        if (fromEn.Length > 0)
        {
            return TruncateEnglishAbbr(fromEn);
        }
        if (fromShort.Length > 0)
        {
            return TruncateEnglishAbbr(fromShort);
        }
        return TruncateEnglishAbbr(LettersOnlyUpper(deptCode));
    }

    /// <summary>
    /// 截断部门名称1（列长 40）。
    /// </summary>
    private static string TrimDeptName1(string? value)
    {
        var text = (value ?? string.Empty).Trim();
        return text.Length <= 40 ? text : text[..40];
    }

    /// <summary>
    /// 截断部门名称2（列长 70）；空则回退为名称1。
    /// </summary>
    private static string TrimDeptName2(string? value, string fallbackName1)
    {
        var text = string.IsNullOrWhiteSpace(value) ? fallbackName1.Trim() : value.Trim();
        return text.Length <= 70 ? text : text[..70];
    }

    /// <summary>
    /// 成本中心编码默认与部门编码一致（同长 6）。
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <returns>成本中心编码（与部门编码相同，最多 6 位）</returns>
    private static string DefaultCostCenterCode(string deptCode)
    {
        var code = (deptCode ?? string.Empty).Trim().ToUpperInvariant();
        if (code.Length == 0)
        {
            return string.Empty;
        }
        return code.Length <= 6 ? code : code[..6];
    }

    /// <summary>
    /// 创建或更新部门（对照 TaktMenuLevel1SeedData.CreateOrUpdateMenuAsync：
    /// 查用 sqlSugarContext.Db.Queryable；增用 repository.CreateAsync；Path/Level/父 IsLeaf 与更新用 Db.Updateable）
    /// </summary>
    private static async Task<(TaktDept Dept, bool IsInserted)> CreateOrUpdateDeptAsync(
        ITaktCompanySeedRepository<TaktDept> repository,
        TaktSeedContext sqlSugarContext,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        string deptCode,
        string deptName1,
        string deptShortName,
        int costCategory,
        long parentId,
        int sortOrder,
        string? deptDescription,
        string? deptName2,
        string? remark)
    {
        var name1 = TrimDeptName1(deptName1);
        var name2 = TrimDeptName2(deptName2, name1);
        var description = deptDescription ?? string.Empty;
        if (description.Length > 500)
        {
            description = description[..500];
        }
        if (deptCode.Length > 6)
        {
            deptCode = deptCode[..6];
        }
        var costCenterCode = DefaultCostCenterCode(deptCode);
        var englishAbbr = ResolveEnglishAbbr(deptCode, deptShortName, name2);
        deptShortName = englishAbbr;
        var isoCode = englishAbbr;
        // 注意：种子数据必须绕过仓储的租户过滤，直接使用 SqlSugar 原生查询（同菜单种子）
        var dept = await sqlSugarContext.Db.Queryable<TaktDept>()
            .Where(d =>
                d.TenantCode == tenantCode &&
                d.CompanyCode == companyCode &&
                d.DeptCode == deptCode &&
                d.IsDeleted == 0)
            .FirstAsync();
        if (dept == null)
        {
            dept = new TaktDept();
            // 必须先设置 TenantCode / CompanyCode（公司级实体）
            dept.TenantCode = tenantCode;
            dept.CompanyCode = companyCode;
            dept.DeptCode = deptCode;
            dept.DeptShortName = deptShortName;
            dept.DeptName1 = name1;
            dept.DeptName2 = name2;
            dept.IsoCode = isoCode;
            dept.CostCenterCode = costCenterCode;
            dept.CostCategory = costCategory;
            dept.ParentId = parentId;
            dept.Level = parentId > 0 ? 0 : 1; // 稍后根据父级计算
            dept.IsLeaf = 1; // 默认为叶子，后续创建子部门时会更新
            // 种子无用户主档映射：HeadUserId=0，冗余名同步为空（勿写描述里的責任者文案）
            dept.HeadUserId = 0;
            dept.HeadUserName = string.Empty;
            dept.Phone = string.Empty;
            dept.Email = string.Empty;
            dept.Location = string.Empty;
            dept.DeptPath = string.Empty;
            dept.DeptStatus = 1;
            dept.SortOrder = sortOrder;
            dept.IsBuiltIn = 1;
            dept.PlantCode = plantCode;
            dept.CultureCode = cultureCode;
            dept.DeptDescription = description;
            dept.Remark = remark ?? string.Empty;
            // CreatedBy / CreatedAt 由 ITaktCompanySeedRepository 自动填充
            dept = await repository.CreateAsync(dept);
            // 更新 DeptPath 和 Level
            if (dept.ParentId > 0)
            {
                // 注意：必须绕过仓储的租户过滤，直接查询
                var parentDept = await sqlSugarContext.Db.Queryable<TaktDept>()
                    .Where(d => d.Id == dept.ParentId && d.IsDeleted == 0)
                    .FirstAsync();
                if (parentDept != null)
                {
                    dept.DeptPath = $"{parentDept.DeptPath}{dept.Id}/";
                    dept.Level = parentDept.Level + 1;
                    // 更新父级 IsLeaf 为非叶子
                    if (parentDept.IsLeaf == 1)
                    {
                        parentDept.IsLeaf = 0;
                        parentDept.UpdatedBy = 900001;
                        parentDept.UpdatedAt = DateTime.Now;
                        await sqlSugarContext.Db.Updateable(parentDept).ExecuteCommandAsync();
                    }
                }
            }
            else
            {
                dept.DeptPath = $"/{dept.Id}/";
                dept.Level = 1;
            }
            // 更新 Level 和 DeptPath
            dept.UpdatedBy = 900001;
            dept.UpdatedAt = DateTime.Now;
            await sqlSugarContext.Db.Updateable(dept).ExecuteCommandAsync();
            return (dept, true);
        }
        else
        {
            var oldDeptName1 = dept.DeptName1;
            var oldDeptName2 = dept.DeptName2;
            var oldDeptShortName = dept.DeptShortName;
            var oldCostCategory = dept.CostCategory;
            var oldSortOrder = dept.SortOrder;
            var oldIsBuiltIn = dept.IsBuiltIn;
            var oldPlantCode = dept.PlantCode;
            var oldCultureCode = dept.CultureCode;
            var oldDeptDescription = dept.DeptDescription;
            var oldRemark = dept.Remark;
            var oldParentId = dept.ParentId;
            var oldDeptStatus = dept.DeptStatus;
            var oldIsoCode = dept.IsoCode ?? string.Empty;
            var oldCostCenterCode = dept.CostCenterCode ?? string.Empty;
            var oldHeadUserId = dept.HeadUserId;
            var oldHeadUserName = dept.HeadUserName ?? string.Empty;
            var oldPhone = dept.Phone ?? string.Empty;
            var oldEmail = dept.Email ?? string.Empty;
            var oldLocation = dept.Location ?? string.Empty;
            dept.DeptName1 = name1;
            dept.DeptName2 = name2;
            dept.DeptShortName = deptShortName;
            dept.IsoCode = isoCode;
            dept.CostCenterCode = costCenterCode;
            dept.CostCategory = costCategory;
            dept.SortOrder = sortOrder;
            dept.IsBuiltIn = 1;
            dept.PlantCode = plantCode;
            dept.CultureCode = cultureCode;
            dept.DeptDescription = description;
            dept.Remark = remark ?? string.Empty;
            dept.ParentId = parentId;
            dept.DeptStatus = 1;
            if (dept.HeadUserId <= 0)
            {
                dept.HeadUserId = 0;
                dept.HeadUserName = string.Empty;
            }
            if (string.IsNullOrEmpty(dept.Phone))
            {
                dept.Phone = string.Empty;
            }
            if (string.IsNullOrEmpty(dept.Email))
            {
                dept.Email = string.Empty;
            }
            if (string.IsNullOrEmpty(dept.Location))
            {
                dept.Location = string.Empty;
            }
            bool needUpdate =
                oldDeptName1 != dept.DeptName1 ||
                oldDeptName2 != dept.DeptName2 ||
                oldDeptShortName != dept.DeptShortName ||
                oldCostCategory != dept.CostCategory ||
                oldSortOrder != dept.SortOrder ||
                oldIsBuiltIn != dept.IsBuiltIn ||
                oldPlantCode != dept.PlantCode ||
                oldCultureCode != dept.CultureCode ||
                oldDeptDescription != dept.DeptDescription ||
                oldRemark != dept.Remark ||
                oldDeptStatus != dept.DeptStatus ||
                oldIsoCode != (dept.IsoCode ?? string.Empty) ||
                oldCostCenterCode != (dept.CostCenterCode ?? string.Empty) ||
                oldHeadUserId != dept.HeadUserId ||
                oldHeadUserName != (dept.HeadUserName ?? string.Empty) ||
                oldPhone != (dept.Phone ?? string.Empty) ||
                oldEmail != (dept.Email ?? string.Empty) ||
                oldLocation != (dept.Location ?? string.Empty);
            // 重新计算 Level 和 DeptPath（如果 ParentId 发生变化或 Path 为空）
            if (dept.ParentId != oldParentId || string.IsNullOrEmpty(dept.DeptPath))
            {
                needUpdate = true;
                if (dept.ParentId > 0)
                {
                    // 注意：必须绕过仓储的租户过滤，直接查询
                    var parentDept = await sqlSugarContext.Db.Queryable<TaktDept>()
                        .Where(d => d.Id == dept.ParentId && d.IsDeleted == 0)
                        .FirstAsync();
                    if (parentDept != null)
                    {
                        dept.DeptPath = $"{parentDept.DeptPath}{dept.Id}/";
                        dept.Level = parentDept.Level + 1;
                        if (parentDept.IsLeaf == 1)
                        {
                            parentDept.IsLeaf = 0;
                            parentDept.UpdatedBy = 900001;
                            parentDept.UpdatedAt = DateTime.Now;
                            await sqlSugarContext.Db.Updateable(parentDept).ExecuteCommandAsync();
                        }
                    }
                }
                else
                {
                    dept.DeptPath = $"/{dept.Id}/";
                    dept.Level = 1;
                }
            }
            if (needUpdate)
            {
                dept.UpdatedBy = 900001;
                dept.UpdatedAt = DateTime.Now;
                await sqlSugarContext.Db.Updateable(dept).ExecuteCommandAsync();
            }
            return (dept, false);
        }
    }
}
