// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktProductionTeamEquipmentSeedData.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：生产班组设备组种子（C100/2300 PCBA 线 SMT1/SMT2/AI/手插；幂等创建或更新）
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
/// 生产班组设备组种子（工厂 C100 / 公司 2300：SMT1/SMT2/AI/手插线设备组）
/// </summary>
public class TaktProductionTeamEquipmentSeedData : ITaktSeedDataCoordinator
{
    private const int StatusEnabled = 1;
    private const int NotObsolete = 0;

    private static readonly HashSet<string> TargetPlantCodes = new(StringComparer.Ordinal) { "C100" };

    /// <summary>
    /// 执行顺序（生产设备主数据种子之后）
    /// </summary>
    public int Order => 493;

    /// <summary>
    /// 初始化生产班组设备组种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化生产班组设备组种子...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过生产班组设备组种子");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var teamEquipmentRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktProductionTeamEquipment>>();
        var teamRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktProductionTeam>>();
        var equipmentRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktProductionEquipment>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过生产班组设备组种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过生产班组设备组种子", tenantCode);
            return (0, 0);
        }
        var templates = GetStandardProductionTeamEquipments();
        var insertCount = 0;
        var updateCount = 0;
        foreach (var company in orderedCompanies)
        {
            string plantCode;
            try
            {
                plantCode = database.GetPlantCodeForCompanyCode(company.CompanyCode);
            }
            catch (InvalidOperationException ex)
            {
                TaktLogger.Warning("公司 {CompanyCode} 未映射工厂，跳过生产班组设备组种子: {Message}", company.CompanyCode, ex.Message);
                continue;
            }
            if (!TargetPlantCodes.Contains(plantCode))
            {
                continue;
            }
            foreach (var seed in templates)
            {
                var team = await teamRepository.FirstAsync(t =>
                    t.TenantCode == tenantCode
                    && t.CompanyCode == company.CompanyCode
                    && t.PlantCode == plantCode
                    && t.TeamCode == seed.TeamCode);
                if (team == null)
                {
                    TaktLogger.Warning(
                        "工厂 {PlantCode} 未找到班组 {TeamCode}，跳过设备组行 {EquipCode}",
                        plantCode,
                        seed.TeamCode,
                        seed.ProdEquipCode);
                    continue;
                }
                var equipment = await equipmentRepository.FirstAsync(e =>
                    e.TenantCode == tenantCode
                    && e.CompanyCode == company.CompanyCode
                    && e.PlantCode == plantCode
                    && e.ProdEquipCode == seed.ProdEquipCode);
                if (equipment == null)
                {
                    TaktLogger.Warning(
                        "工厂 {PlantCode} 未找到设备 {EquipCode}，跳过班组 {TeamCode} 设备组行",
                        plantCode,
                        seed.ProdEquipCode,
                        seed.TeamCode);
                    continue;
                }
                var (_, inserted, updated) = await CreateOrUpdateProductionTeamEquipmentAsync(
                    teamEquipmentRepository,
                    tenantCode,
                    company.CompanyCode, company.CultureCode,
                    plantCode,
                    team,
                    equipment,
                    seed);
                insertCount += inserted;
                updateCount += updated;
            }
        }
        TaktLogger.Information("生产班组设备组种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// C100（东莞/公司 2300）PCBA 线体设备组（SMT1/SMT2/AI/手插）
    /// </summary>
    private static List<TaktProductionTeamEquipment> GetStandardProductionTeamEquipments()
    {
        return
        [
            // SMT1 线
            new TaktProductionTeamEquipment
            {
                TeamCode = "SMT1",
                LineNumber = 10,
                ProdEquipCode = "PRI-SP18PL-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "SMT1",
                LineNumber = 20,
                ProdEquipCode = "SPI-ALDST3-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "SMT1",
                LineNumber = 30,
                ProdEquipCode = "SMT-CM602L-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "SMT1",
                LineNumber = 40,
                ProdEquipCode = "SMT-DT401F-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "SMT1",
                LineNumber = 50,
                ProdEquipCode = "REF-TNP50572-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "SMT1",
                LineNumber = 60,
                ProdEquipCode = "AOI-ALD8710S-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            // SMT2 线
            new TaktProductionTeamEquipment
            {
                TeamCode = "SMT2",
                LineNumber = 10,
                ProdEquipCode = "PRI-SP18PL-02",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "SMT2",
                LineNumber = 20,
                ProdEquipCode = "SMT-CM602L-02",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "SMT2",
                LineNumber = 30,
                ProdEquipCode = "REF-TAP30407-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "SMT2",
                LineNumber = 40,
                ProdEquipCode = "AOI-U22XHML-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            // AI 线
            new TaktProductionTeamEquipment
            {
                TeamCode = "AI1",
                LineNumber = 10,
                ProdEquipCode = "AI-AVB-01",
                EquipQuantity = 2,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "AI1",
                LineNumber = 20,
                ProdEquipCode = "AI-AVK3-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "AI1",
                LineNumber = 30,
                ProdEquipCode = "AI-VC7A-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "AI1",
                LineNumber = 40,
                ProdEquipCode = "AI-VC7AT-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            // 手插线
            new TaktProductionTeamEquipment
            {
                TeamCode = "MI1",
                LineNumber = 10,
                ProdEquipCode = "SEL-JT550-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "MI1",
                LineNumber = 20,
                ProdEquipCode = "REF-JN350BS-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
            new TaktProductionTeamEquipment
            {
                TeamCode = "MI1",
                LineNumber = 30,
                ProdEquipCode = "REF-FLADS300-01",
                EquipQuantity = 1,
                TeamEquipStatus = StatusEnabled,
                IsObsolete = NotObsolete,
            },
        ];
    }

    /// <summary>
    /// 创建或更新班组设备组行（按班组+设备幂等）
    /// </summary>
    private static async Task<(TaktProductionTeamEquipment Row, int InsertCount, int UpdateCount)> CreateOrUpdateProductionTeamEquipmentAsync(
        ITaktCompanySeedRepository<TaktProductionTeamEquipment> repository,
        string tenantCode,
        string companyCode,
        string cultureCode,
        string plantCode,
        TaktProductionTeam team,
        TaktProductionEquipment equipment,
        TaktProductionTeamEquipment seed)
    {
        var row = await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.PlantCode == plantCode
            && x.ProdTeamId == team.Id
            && x.ProdEquipId == equipment.Id);
        if (row == null)
        {
            row = new TaktProductionTeamEquipment
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                PlantCode = plantCode,
                ProdTeamId = team.Id,
                TeamCode = team.TeamCode,
                LineNumber = seed.LineNumber,
                ProdEquipId = equipment.Id,
                ProdEquipCode = equipment.ProdEquipCode,
                EquipQuantity = seed.EquipQuantity,
                TeamEquipStatus = seed.TeamEquipStatus,
                IsObsolete = seed.IsObsolete,
                CultureCode = cultureCode
            };
            row = await repository.CreateAsync(row);
            return (row, 1, 0);
        }
        var needUpdate = false;
        if (row.TeamCode != team.TeamCode)
        {
            row.TeamCode = team.TeamCode;
            needUpdate = true;
        }
        if (row.LineNumber != seed.LineNumber)
        {
            row.LineNumber = seed.LineNumber;
            needUpdate = true;
        }
        if (row.ProdEquipCode != equipment.ProdEquipCode)
        {
            row.ProdEquipCode = equipment.ProdEquipCode;
            needUpdate = true;
        }
        if (row.EquipQuantity != seed.EquipQuantity)
        {
            row.EquipQuantity = seed.EquipQuantity;
            needUpdate = true;
        }
        if (row.TeamEquipStatus != seed.TeamEquipStatus)
        {
            row.TeamEquipStatus = seed.TeamEquipStatus;
            needUpdate = true;
        }
        if (row.IsObsolete != seed.IsObsolete)
        {
            row.IsObsolete = seed.IsObsolete;
            needUpdate = true;
        }
        if (needUpdate)
        {
            await repository.UpdateAsync(row);
            return (row, 0, 1);
        }
        return (row, 0, 0);
    }
}
