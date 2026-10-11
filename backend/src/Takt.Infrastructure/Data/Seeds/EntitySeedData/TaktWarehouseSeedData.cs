// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktWarehouseSeedData.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：仓库主数据种子（东莞 C100 / 香港 H100 存货地点；幂等创建或更新）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 仓库主数据种子（按工厂 C100/H100 写入标准存货地点；按 Database:CompanyCodes 映射公司）
/// </summary>
public class TaktWarehouseSeedData : ITaktSeedDataCoordinator
{
    private const int StatusEnabled = 1;
    private const int IsBuiltInYes = 1;
    private const int IsVirtualNo = 0;
    private const int IsVirtualYes = 1;

    /// <summary>
    /// 执行顺序（工厂种子之后）
    /// </summary>
    public int Order => 7;

    /// <summary>
    /// 初始化仓库主数据种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化仓库主数据种子...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过仓库主数据种子初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktWarehouse>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过仓库主数据种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过仓库主数据种子", tenantCode);
            return (0, 0);
        }
        var seedByPlant = GetStandardWarehouses()
            .GroupBy(w => w.PlantCode, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.Ordinal);
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化仓库主数据...", tenantCode);
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
                    "公司 {CompanyCode} 未映射工厂，跳过仓库主数据种子: {Message}",
                    company.CompanyCode,
                    ex.Message);
                continue;
            }
            if (!seedByPlant.TryGetValue(plantCode, out var templates))
            {
                TaktLogger.Information(
                    "公司 {CompanyCode} 工厂 {PlantCode} 无预置仓库目录，跳过",
                    company.CompanyCode,
                    plantCode);
                continue;
            }
            TaktLogger.Information(
                "正在为公司 {CompanyCode} 工厂 {PlantCode} 初始化仓库主数据（{Count} 条）...",
                company.CompanyCode,
                plantCode,
                templates.Count);
            foreach (var template in templates)
            {
                var (_, inserted, updated) = await CreateOrUpdateWarehouseAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode, company.CultureCode,
                    template);
                insertCount += inserted;
                updateCount += updated;
            }
        }
        TaktLogger.Information(
            "仓库主数据种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 标准存货地点目录（PlantCode + WarehouseCode 唯一）
    /// </summary>
    /// <returns>仓库种子项列表</returns>
    private static List<WarehouseSeedItem> GetStandardWarehouses()
    {
        return
        [
            // C100 东莞蒂雅克
            new WarehouseSeedItem("C100", "C001", "原料保税仓", 0, IsVirtualNo, 1),
            new WarehouseSeedItem("C100", "C002", "原料非保税仓", 0, IsVirtualNo, 2),
            new WarehouseSeedItem("C100", "C003", "原料电子保税仓", 0, IsVirtualNo, 3),
            new WarehouseSeedItem("C100", "C004", "不良品仓", 3, IsVirtualNo, 4),
            new WarehouseSeedItem("C100", "C005", "辅料仓", 0, IsVirtualNo, 5),
            new WarehouseSeedItem("C100", "C006", "自制半成品仓", 1, IsVirtualNo, 6),
            new WarehouseSeedItem("C100", "C007", "保税退港仓", 5, IsVirtualNo, 7),
            new WarehouseSeedItem("C100", "C008", "海外成品仓", 2, IsVirtualNo, 8),
            new WarehouseSeedItem("C100", "C009", "制二课现场仓", 5, IsVirtualNo, 9),
            new WarehouseSeedItem("C100", "C010", "IQC仓", 5, IsVirtualNo, 10),
            new WarehouseSeedItem("C100", "C011", "制一课现场仓", 5, IsVirtualNo, 11),
            new WarehouseSeedItem("C100", "C012", "试验仓", 5, IsVirtualNo, 12),
            // H100 香港 TEAC AUDIO
            new WarehouseSeedItem("H100", "H001", "OFFICE for DTA", 5, IsVirtualYes, 1),
            new WarehouseSeedItem("H100", "H002", "OFFICE for Cust.", 5, IsVirtualYes, 2),
            new WarehouseSeedItem("H100", "H003", "ALPS HK", 5, IsVirtualNo, 3),
            new WarehouseSeedItem("H100", "H005", "报废仓", 3, IsVirtualNo, 4),
            new WarehouseSeedItem("H100", "H006", "退货", 5, IsVirtualNo, 5),
            new WarehouseSeedItem("H100", "C001", "转厂", 5, IsVirtualYes, 6),
            new WarehouseSeedItem("H100", "C002", "转厂", 5, IsVirtualYes, 7),
            new WarehouseSeedItem("H100", "C003", "转厂", 5, IsVirtualYes, 8),
        ];
    }

    /// <summary>
    /// 创建或更新品仓库主数据
    /// </summary>
    /// <param name="repository">仓库种子仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="seed">种子项</param>
    /// <returns>实体与插入/更新计数</returns>
    private static async Task<(TaktWarehouse Warehouse, int InsertCount, int UpdateCount)> CreateOrUpdateWarehouseAsync(
        ITaktCompanySeedRepository<TaktWarehouse> repository,
        string tenantCode,
        string companyCode,
        string cultureCode,
        WarehouseSeedItem seed)
    {
        var warehouse = await repository.FirstAsync(w =>
            w.TenantCode == tenantCode
            && w.CompanyCode == companyCode
            && w.PlantCode == seed.PlantCode
            && w.WarehouseCode == seed.WarehouseCode);
        if (warehouse == null)
        {
            warehouse = new TaktWarehouse
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                PlantCode = seed.PlantCode,
                WarehouseCode = seed.WarehouseCode,
                WarehouseName = seed.WarehouseName,
                WarehouseType = seed.WarehouseType,
                IsVirtual = seed.IsVirtual,
                IsBuiltIn = IsBuiltInYes,
                SortOrder = seed.SortOrder,
                WarehouseStatus = StatusEnabled,
                CultureCode = cultureCode
            };
            warehouse = await repository.CreateAsync(warehouse);
            return (warehouse, 1, 0);
        }
        var needUpdate = false;
        if (warehouse.WarehouseName != seed.WarehouseName)
        {
            warehouse.WarehouseName = seed.WarehouseName;
            needUpdate = true;
        }
        if (warehouse.WarehouseType != seed.WarehouseType)
        {
            warehouse.WarehouseType = seed.WarehouseType;
            needUpdate = true;
        }
        if (warehouse.IsVirtual != seed.IsVirtual)
        {
            warehouse.IsVirtual = seed.IsVirtual;
            needUpdate = true;
        }
        if (warehouse.SortOrder != seed.SortOrder)
        {
            warehouse.SortOrder = seed.SortOrder;
            needUpdate = true;
        }
        if (warehouse.IsBuiltIn != IsBuiltInYes)
        {
            warehouse.IsBuiltIn = IsBuiltInYes;
            needUpdate = true;
        }
        if (warehouse.WarehouseStatus != StatusEnabled)
        {
            warehouse.WarehouseStatus = StatusEnabled;
            needUpdate = true;
        }
        if (needUpdate)
        {
            await repository.UpdateAsync(warehouse);
            return (warehouse, 0, 1);
        }
        return (warehouse, 0, 0);
    }

    /// <summary>
    /// 仓库种子项
    /// </summary>
    /// <param name="PlantCode">工厂编码</param>
    /// <param name="WarehouseCode">存货地点编码</param>
    /// <param name="WarehouseName">仓库名称</param>
    /// <param name="WarehouseType">仓库类型（字典 logistics_materials_warehouse_type）</param>
    /// <param name="IsVirtual">是否虚拟仓（字典 sys_yes_no）</param>
    /// <param name="SortOrder">排序号</param>
    private sealed record WarehouseSeedItem(
        string PlantCode,
        string WarehouseCode,
        string WarehouseName,
        int WarehouseType,
        int IsVirtual,
        int SortOrder);
}
