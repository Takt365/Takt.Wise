// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktDefectGroupSeedData.cs
// 创建时间：2026-07-08
// 创建人：Takt365(Cursor AI)
// 功能描述：不良组主数据种子（Assy/Inspection/Repair 各类别各 01/02 演示目录；幂等创建或更新）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 不良组主数据种子（按不良类别 Assy/Inspection/Repair 分别写入；编码为工厂首字母+01/02）
/// </summary>
public class TaktDefectGroupSeedData : ITaktSeedDataCoordinator
{
    private const int StatusEnabled = 1;
    private const int IsBuiltInYes = 1;

    /// <summary>
    /// 不良类别：Assy
    /// </summary>
    private const int DefectGroupCategoryAssy = 0;

    /// <summary>
    /// 不良类别：Inspection
    /// </summary>
    private const int DefectGroupCategoryInspection = 1;

    /// <summary>
    /// 不良类别：Repair
    /// </summary>
    private const int DefectGroupCategoryRepair = 2;

    /// <summary>
    /// 执行顺序（质量组种子之后）
    /// </summary>
    public int Order => 12;

    /// <summary>
    /// 初始化不良组主数据种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化不良组主数据种子...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过不良组主数据种子初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktDefectGroup>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过不良组主数据种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过不良组主数据种子", tenantCode);
            return (0, 0);
        }
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化不良组主数据...", tenantCode);
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
                    "公司 {CompanyCode} 未映射工厂，跳过不良组主数据种子: {Message}",
                    company.CompanyCode,
                    ex.Message);
                continue;
            }
            var templates = GetStandardDefectGroups(plantCode);
            TaktLogger.Information(
                "正在为公司 {CompanyCode} 工厂 {PlantCode} 初始化不良组主数据（{Count} 条）...",
                company.CompanyCode,
                plantCode,
                templates.Count);
            foreach (var template in templates)
            {
                var (_, inserted, updated) = await CreateOrUpdateDefectGroupAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode, company.CultureCode,
                    template);
                insertCount += inserted;
                updateCount += updated;
            }
        }
        TaktLogger.Information(
            "不良组主数据种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 标准不良组目录（PlantCode + DefectGroupCategory + DefectGroupCode 唯一；每类别各 01/02）
    /// </summary>
    /// <param name="plantCode">工厂编码</param>
    /// <returns>不良组种子项列表</returns>
    private static List<DefectGroupSeedItem> GetStandardDefectGroups(string plantCode)
    {
        var codePrefix = plantCode[0];
        return
        [
            // Assy
            new DefectGroupSeedItem(plantCode, DefectGroupCategoryAssy, $"{codePrefix}01", "张三", null, 1),
            new DefectGroupSeedItem(plantCode, DefectGroupCategoryAssy, $"{codePrefix}02", "李四", null, 2),
            // Inspection
            new DefectGroupSeedItem(plantCode, DefectGroupCategoryInspection, $"{codePrefix}01", "张三", null, 1),
            new DefectGroupSeedItem(plantCode, DefectGroupCategoryInspection, $"{codePrefix}02", "李四", null, 2),
            // Repair
            new DefectGroupSeedItem(plantCode, DefectGroupCategoryRepair, $"{codePrefix}01", "张三", null, 1),
            new DefectGroupSeedItem(plantCode, DefectGroupCategoryRepair, $"{codePrefix}02", "李四", null, 2),
        ];
    }

    /// <summary>
    /// 创建或更新不良组主数据
    /// </summary>
    /// <param name="repository">不良组种子仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="seed">种子项</param>
    /// <returns>实体与插入/更新计数</returns>
    private static async Task<(TaktDefectGroup Group, int InsertCount, int UpdateCount)> CreateOrUpdateDefectGroupAsync(
        ITaktCompanySeedRepository<TaktDefectGroup> repository,
        string tenantCode,
        string companyCode,
        string cultureCode,
        DefectGroupSeedItem seed)
    {
        var group = await repository.FirstAsync(g =>
            g.TenantCode == tenantCode
            && g.CompanyCode == companyCode
            && g.PlantCode == seed.PlantCode
            && g.DefectGroupCategory == seed.DefectGroupCategory
            && g.DefectGroupCode == seed.DefectGroupCode);
        if (group == null)
        {
            group = new TaktDefectGroup
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                PlantCode = seed.PlantCode,
                DefectGroupCategory = seed.DefectGroupCategory,
                DefectGroupCode = seed.DefectGroupCode,
                DefectGroupName = seed.DefectGroupName,
                DefectGroupDescription = seed.DefectGroupDescription,
                IsBuiltIn = IsBuiltInYes,
                SortOrder = seed.SortOrder,
                GroupStatus = StatusEnabled,
                CultureCode = cultureCode
            };
            group = await repository.CreateAsync(group);
            return (group, 1, 0);
        }
        var needUpdate = false;
        if (group.DefectGroupName != seed.DefectGroupName)
        {
            group.DefectGroupName = seed.DefectGroupName;
            needUpdate = true;
        }
        if (group.DefectGroupDescription != seed.DefectGroupDescription)
        {
            group.DefectGroupDescription = seed.DefectGroupDescription;
            needUpdate = true;
        }
        if (group.SortOrder != seed.SortOrder)
        {
            group.SortOrder = seed.SortOrder;
            needUpdate = true;
        }
        if (group.IsBuiltIn != IsBuiltInYes)
        {
            group.IsBuiltIn = IsBuiltInYes;
            needUpdate = true;
        }
        if (group.GroupStatus != StatusEnabled)
        {
            group.GroupStatus = StatusEnabled;
            needUpdate = true;
        }
        if (needUpdate)
        {
            await repository.UpdateAsync(group);
            return (group, 0, 1);
        }
        return (group, 0, 0);
    }

    /// <summary>
    /// 不良组种子项
    /// </summary>
    /// <param name="PlantCode">工厂编码</param>
    /// <param name="DefectGroupCategory">不良组类别</param>
    /// <param name="DefectGroupCode">不良组编码</param>
    /// <param name="DefectGroupName">不良组名称</param>
    /// <param name="DefectGroupDescription">不良组描述</param>
    /// <param name="SortOrder">排序号</param>
    private sealed record DefectGroupSeedItem(
        string PlantCode,
        int DefectGroupCategory,
        string DefectGroupCode,
        string DefectGroupName,
        string? DefectGroupDescription,
        int SortOrder);
}
