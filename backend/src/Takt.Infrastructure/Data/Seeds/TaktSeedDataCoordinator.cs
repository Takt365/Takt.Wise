// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktSeedDataCoordinator.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：种子数据协调器，通过 DI 统一管理所有种子数据初始化
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Seeds.EntitySeedData;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds;

/// <summary>
/// 种子数据协调器
/// 负责通过依赖注入按顺序执行所有 ITaktSeedDataCoordinator 实现类
/// </summary>
public static class TaktSeedDataCoordinator
{
    /// <summary>
    /// 初始化所有种子数据
    /// 阶段 1：全局租户主档；阶段 2：按 <c>TenantCodes</c>（如 000、100、500）依次切换租户库并执行全部种子，各租户内公司/工厂/假日/组织等由种子按 <c>CompanyCodes</c>/<c>PlantCodes</c> 顺序写入
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <returns>返回总的插入和更新记录数</returns>
    public static async Task<(int TotalInsertCount, int TotalUpdateCount)> InitializeAllAsync(
        IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var tenantCodes = database.TenantCodes;
        var companyCodes = database.CompanyCodes;
        var plantCodes = database.PlantCodes;
        var tenantCodeList = string.Join(", ", tenantCodes);
        var companyCodeList = string.Join(", ", companyCodes);
        var plantCodeList = string.Join(", ", plantCodes);
        TaktLogger.Information(
            "========== 开始初始化种子数据（TenantCodes: {TenantCodes}；CompanyCodes: {CompanyCodes}；PlantCodes: {PlantCodes}）==========",
            tenantCodeList,
            companyCodeList,
            plantCodeList);
        int totalInsertCount = 0;
        int totalUpdateCount = 0;
        TaktLogger.Information("[阶段 1] 执行租户种子数据（全局初始化，顺序: {TenantCodes}）...", tenantCodeList);
        var globalSeedTypes = new[] { typeof(TaktTenantSeedData) };
        var allSeedDataCoordinators = serviceProvider.GetServices<ITaktSeedDataCoordinator>().ToList();
        var globalSeeds = allSeedDataCoordinators
            .Where(s => globalSeedTypes.Contains(s.GetType()))
            .OrderBy(s => s.Order)
            .ToList();
        foreach (var seedData in globalSeeds)
        {
            try
            {
                TaktLogger.Information("[全局] [{Order}] 执行 {SeedName}...", seedData.Order, seedData.GetType().Name);
                var (insertCount, updateCount) = await seedData.SeedAsync(serviceProvider, null);
                totalInsertCount += insertCount;
                totalUpdateCount += updateCount;
                TaktLogger.Information(
                    "[全局] [{Order}] {SeedName} 完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
                    seedData.Order,
                    seedData.GetType().Name,
                    insertCount,
                    updateCount);
            }
            catch (Exception ex)
            {
                TaktLogger.Error(ex, "[全局] [{Order}] {SeedName} 执行失败", seedData.Order, seedData.GetType().Name);
            }
        }
        TaktLogger.Information("[阶段 1] 租户种子数据初始化完成");
        var seedContext = serviceProvider.GetRequiredService<Takt.Infrastructure.Data.Context.TaktSeedContext>();
        for (var index = 0; index < tenantCodes.Count; index++)
        {
            var tenantCode = tenantCodes[index];
            var tenantIndex = index + 1;
            var tenantTotal = tenantCodes.Count;
            TaktLogger.Information(
                "========== [租户 {TenantCode} {Index}/{Total}] 开始初始化（按 CompanyCodes 顺序: {CompanyCodes}） ==========",
                tenantCode,
                tenantIndex,
                tenantTotal,
                companyCodeList);
            seedContext.SwitchTenant(tenantCode);
            TaktLogger.Information(
                "已切换到租户 {TenantCode} 的数据库 (CurrentTenantCode={CurrentCode}, {Index}/{Total})",
                tenantCode,
                seedContext.CurrentTenantCode,
                tenantIndex,
                tenantTotal);
            var seedDataCoordinators = serviceProvider.GetServices<ITaktSeedDataCoordinator>()
                .Where(s => !globalSeedTypes.Contains(s.GetType()))
                .ToList();
            var orderedSeeds = seedDataCoordinators.OrderBy(s => s.Order).ToList();
            int tenantInsertCount = 0;
            int tenantUpdateCount = 0;
            foreach (var seedData in orderedSeeds)
            {
                try
                {
                    TaktLogger.Information(
                        "[租户 {TenantCode} {Index}/{Total}] [{Order}] 执行 {SeedName}...",
                        tenantCode,
                        tenantIndex,
                        tenantTotal,
                        seedData.Order,
                        seedData.GetType().Name);
                    var (insertCount, updateCount) = await seedData.SeedAsync(serviceProvider, tenantCode);
                    tenantInsertCount += insertCount;
                    tenantUpdateCount += updateCount;
                    TaktLogger.Information(
                        "[租户 {TenantCode} {Index}/{Total}] [{Order}] {SeedName} 完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
                        tenantCode,
                        tenantIndex,
                        tenantTotal,
                        seedData.Order,
                        seedData.GetType().Name,
                        insertCount,
                        updateCount);
                }
                catch (Exception ex)
                {
                    TaktLogger.Error(
                        ex,
                        "[租户 {TenantCode} {Index}/{Total}] [{Order}] {SeedName} 执行失败",
                        tenantCode,
                        tenantIndex,
                        tenantTotal,
                        seedData.Order,
                        seedData.GetType().Name);
                }
            }
            totalInsertCount += tenantInsertCount;
            totalUpdateCount += tenantUpdateCount;
            TaktLogger.Information(
                "========== [租户 {TenantCode} {Index}/{Total}] 种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条 ==========",
                tenantCode,
                tenantIndex,
                tenantTotal,
                tenantInsertCount,
                tenantUpdateCount);
        }
        TaktLogger.Information(
            "========== 种子数据初始化完成（TenantCodes: {TenantCodes}）: 总计插入 {TotalInsert} 条，更新 {TotalUpdate} 条 ==========",
            tenantCodeList,
            totalInsertCount,
            totalUpdateCount);
        return (totalInsertCount, totalUpdateCount);
    }
}
