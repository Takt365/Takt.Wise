// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktWorkCenterSeedData.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：工作中心主数据种子（东莞 C100 制一/制二课；幂等创建或更新）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 工作中心主数据种子（按工厂 C100 写入标准工作中心；按 Database:CompanyCodes 映射公司）
/// </summary>
public class TaktWorkCenterSeedData : ITaktSeedDataCoordinator
{
    private const int StatusEnabled = 1;

    private static readonly HashSet<string> SeededPlantCodes = new(StringComparer.Ordinal)
    {
        "C100",
    };

    /// <summary>
    /// 执行顺序（生产班组种子之后）
    /// </summary>
    public int Order => 50;

    /// <summary>
    /// 初始化工作中心主数据种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化工作中心主数据种子...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过工作中心主数据种子初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktWorkCenter>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过工作中心主数据种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过工作中心主数据种子", tenantCode);
            return (0, 0);
        }
        var templates = GetStandardWorkCenters();
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化工作中心主数据...", tenantCode);
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
                    "公司 {CompanyCode} 未映射工厂，跳过工作中心主数据种子: {Message}",
                    company.CompanyCode,
                    ex.Message);
                continue;
            }
            if (!SeededPlantCodes.Contains(plantCode))
            {
                TaktLogger.Information(
                    "公司 {CompanyCode} 工厂 {PlantCode} 无预置工作中心目录，跳过",
                    company.CompanyCode,
                    plantCode);
                continue;
            }
            TaktLogger.Information(
                "正在为公司 {CompanyCode} 工厂 {PlantCode} 初始化工作中心主数据（{Count} 条）...",
                company.CompanyCode,
                plantCode,
                templates.Count);
            foreach (var template in templates)
            {
                var (_, inserted, updated) = await CreateOrUpdateWorkCenterAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode, company.CultureCode,
                    plantCode,
                    template);
                insertCount += inserted;
                updateCount += updated;
            }
        }
        TaktLogger.Information(
            "工作中心主数据种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 标准工作中心目录（C100 制一/制二课）
    /// </summary>
    /// <returns>工作中心种子项列表</returns>
    private static List<WorkCenterSeedItem> GetStandardWorkCenters()
    {
        return
        [
            new WorkCenterSeedItem("ZCPS", "制二课SMTCP"),
            new WorkCenterSeedItem("ZCPA", "制二课自插班CP"),
            new WorkCenterSeedItem("ZCPT", "制二课手插修正班CP"),
            new WorkCenterSeedItem("ZCPM", "制一课加工班CP"),
            new WorkCenterSeedItem("ZCPZ", "制一课组立班CP"),
            new WorkCenterSeedItem("ZPRS", "制二课SMTPRO"),
            new WorkCenterSeedItem("ZPRA", "制二课自插班PRO"),
            new WorkCenterSeedItem("ZPRT", "制二课手插修正班PRO"),
            new WorkCenterSeedItem("ZPRM", "制一课加工班PRO"),
            new WorkCenterSeedItem("ZPRZ", "制一课组立班PRO"),
            new WorkCenterSeedItem("ZMIS", "制二课SMTMI"),
            new WorkCenterSeedItem("ZMIA", "制二课自插MI"),
            new WorkCenterSeedItem("ZMIT", "制二课手插修正班MI"),
            new WorkCenterSeedItem("ZMIM", "制一课加工班MI"),
            new WorkCenterSeedItem("ZMIZ", "制一课组立班MI"),
            new WorkCenterSeedItem("ZBSS", "制二课SMTBS"),
            new WorkCenterSeedItem("ZBSA", "制二课自插班BS"),
            new WorkCenterSeedItem("ZBST", "制二课手插修正班BS"),
            new WorkCenterSeedItem("ZBSM", "制一课加工班BS"),
            new WorkCenterSeedItem("ZBSZ", "制一课组立班BS"),
            new WorkCenterSeedItem("ZEMS", "制二课SMTEMS"),
            new WorkCenterSeedItem("ZEMA", "制二课自插班EMS"),
            new WorkCenterSeedItem("ZEMT", "制二课手插修正班EMS"),
            new WorkCenterSeedItem("ZEMM", "制一课加工班EMS"),
            new WorkCenterSeedItem("ZEMZ", "制一课组立班EMS"),
            new WorkCenterSeedItem("ZODS", "制二课SMTOD"),
            new WorkCenterSeedItem("ZODA", "制二课自插班OD"),
            new WorkCenterSeedItem("ZODT", "制二课手插修正班OD"),
        ];
    }

    /// <summary>
    /// 创建或更新工作中心主数据
    /// </summary>
    /// <param name="repository">工作中心种子仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="plantCode">工厂编码</param>
    /// <param name="seed">种子项</param>
    /// <returns>实体与插入/更新计数</returns>
    private static async Task<(TaktWorkCenter WorkCenter, int InsertCount, int UpdateCount)> CreateOrUpdateWorkCenterAsync(
        ITaktCompanySeedRepository<TaktWorkCenter> repository,
        string tenantCode,
        string companyCode,
        string cultureCode,
        string plantCode,
        WorkCenterSeedItem seed)
    {
        var workCenter = await repository.FirstAsync(w =>
            w.TenantCode == tenantCode
            && w.CompanyCode == companyCode
            && w.PlantCode == plantCode
            && w.WorkCenterCode == seed.WorkCenterCode);
        if (workCenter == null)
        {
            workCenter = new TaktWorkCenter
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                PlantCode = plantCode,
                WorkCenterCode = seed.WorkCenterCode,
                WorkCenterDescription = seed.WorkCenterDescription,
                WorkCenterStatus = StatusEnabled,
                CultureCode = cultureCode
            };
            workCenter = await repository.CreateAsync(workCenter);
            return (workCenter, 1, 0);
        }
        var needUpdate = false;
        if (workCenter.WorkCenterDescription != seed.WorkCenterDescription)
        {
            workCenter.WorkCenterDescription = seed.WorkCenterDescription;
            needUpdate = true;
        }
        if (workCenter.WorkCenterStatus != StatusEnabled)
        {
            workCenter.WorkCenterStatus = StatusEnabled;
            needUpdate = true;
        }
        if (needUpdate)
        {
            await repository.UpdateAsync(workCenter);
            return (workCenter, 0, 1);
        }
        return (workCenter, 0, 0);
    }

    /// <summary>
    /// 工作中心种子项
    /// </summary>
    /// <param name="WorkCenterCode">工作中心编码</param>
    /// <param name="WorkCenterDescription">工作中心描述</param>
    private sealed record WorkCenterSeedItem(
        string WorkCenterCode,
        string WorkCenterDescription);
}
