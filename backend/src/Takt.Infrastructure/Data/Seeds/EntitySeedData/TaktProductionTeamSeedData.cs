// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktProductionTeamSeedData.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：生产班组种子（按 logistics_manufacturing_team_category 分类；仅制造工厂 C100/公司 2300；幂等创建或更新）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Manufacturing.Mps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 生产班组种子数据（组立/PCBA/SMT/质检；仅工厂 C100 / 公司 2300）
/// </summary>
public class TaktProductionTeamSeedData : ITaktSeedDataCoordinator
{
    private const int StatusEnabled = 1;
    private const int DefaultShiftNo = 1;

    private static readonly HashSet<string> TargetPlantCodes = new(StringComparer.Ordinal) { "C100" };

    /// <summary>
    /// 执行顺序（工厂、编码规则之后）
    /// </summary>
    public int Order => 49;

    /// <summary>
    /// 初始化生产班组种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化生产班组种子数据...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过生产班组种子数据初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktProductionTeam>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过生产班组种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过生产班组种子", tenantCode);
            return (0, 0);
        }
        var templates = GetStandardProductionTeams();
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化生产班组...", tenantCode);
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
                    "公司 {CompanyCode} 未映射工厂，跳过生产班组种子: {Message}",
                    company.CompanyCode,
                    ex.Message);
                continue;
            }
            if (!TargetPlantCodes.Contains(plantCode))
            {
                TaktLogger.Information(
                    "公司 {CompanyCode} 工厂 {PlantCode} 非目标工厂 C100，跳过生产班组种子",
                    company.CompanyCode,
                    plantCode);
                continue;
            }
            TaktLogger.Information(
                "正在为公司 {CompanyCode} 工厂 {PlantCode} 初始化生产班组...",
                company.CompanyCode,
                plantCode);
            foreach (var template in templates)
            {
                var (_, inserted, updated) = await CreateOrUpdateProductionTeamAsync(
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
            "生产班组种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 标准生产班组目录（A=组立 P=PCBA Q=质检；RWC/SUB/SMT/MI/AI 及质检均为 1～2）
    /// </summary>
    /// <returns>班组种子项列表</returns>
    private static List<ProductionTeamSeedItem> GetStandardProductionTeams()
    {
        var items = new List<ProductionTeamSeedItem>();
        for (var i = 1; i <= 15; i++)
        {
            // 组立班组
            items.Add(new ProductionTeamSeedItem(i.ToString(), $"{i}班", "A"));
        }
        // PCBA 工艺统计排序：1=SMT → 2=AI → 3=MI → 4=RWC（修正）
        // RWC=Rework/Correction；SUB=Sub-assembly；SMT=Surface Mount Technology；
        // MI=Manual Insertion（人工插件，亦称 DIP）；AI=Auto Insertion（自插）
        for (var i = 1; i <= 2; i++)
        {
            items.Add(new ProductionTeamSeedItem($"SUB{i}", $"加工{i}班", "A"));
            items.Add(new ProductionTeamSeedItem($"SMT{i}", $"SMT{i}线", "P"));
            items.Add(new ProductionTeamSeedItem($"AI{i}", $"AI{i}线", "P"));
            items.Add(new ProductionTeamSeedItem($"MI{i}", $"手插{i}线", "P"));
            items.Add(new ProductionTeamSeedItem($"RWC{i}", $"修正{i}班", "P"));
            items.Add(new ProductionTeamSeedItem($"IQC{i}", $"IQC{i}班", "Q"));
            items.Add(new ProductionTeamSeedItem($"QA{i}", $"QA{i}班", "Q"));
        }
        return items;
    }

    /// <summary>
    /// 创建或更新生产班组
    /// </summary>
    /// <param name="repository">生产班组种子仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="plantCode">工厂编码</param>
    /// <param name="seed">种子项</param>
    /// <returns>实体与插入/更新计数</returns>
    private static async Task<(TaktProductionTeam Team, int InsertCount, int UpdateCount)> CreateOrUpdateProductionTeamAsync(
        ITaktCompanySeedRepository<TaktProductionTeam> repository,
        string tenantCode,
        string companyCode,
        string cultureCode,
        string plantCode,
        ProductionTeamSeedItem seed)
    {
        var team = await repository.FirstAsync(t =>
            t.TenantCode == tenantCode
            && t.CompanyCode == companyCode
            && t.PlantCode == plantCode
            && t.TeamCode == seed.TeamCode
            && t.TeamCategory == seed.TeamCategory);
        if (team == null)
        {
            team = new TaktProductionTeam
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                PlantCode = plantCode,
                TeamCode = seed.TeamCode,
                TeamName = seed.TeamName,
                TeamCategory = seed.TeamCategory,
                ShiftNo = DefaultShiftNo,
                TeamStatus = StatusEnabled,
                CultureCode = cultureCode
            };
            team = await repository.CreateAsync(team);
            return (team, 1, 0);
        }
        var needUpdate = false;
        if (team.TeamName != seed.TeamName)
        {
            team.TeamName = seed.TeamName;
            needUpdate = true;
        }
        if (team.TeamStatus != StatusEnabled)
        {
            team.TeamStatus = StatusEnabled;
            needUpdate = true;
        }
        if (needUpdate)
        {
            await repository.UpdateAsync(team);
            return (team, 0, 1);
        }
        return (team, 0, 0);
    }

    /// <summary>
    /// 生产班组种子项
    /// </summary>
    /// <param name="TeamCode">班组编码</param>
    /// <param name="TeamName">班组名称</param>
    /// <param name="TeamCategory">班组分类 DictValue</param>
    private sealed record ProductionTeamSeedItem(
        string TeamCode,
        string TeamName,
        string TeamCategory);
}
