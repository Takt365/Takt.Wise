// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktUserTenantSeedData.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：用户-租户关联种子数据，按公司隔离为每个启用用户初始化本租户访问权
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 用户-租户关联种子数据初始化（每个启用公司各一份）
/// 幂等性操作：存在则更新 IsDefault，不存在则创建
/// </summary>
public class TaktUserTenantSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在用户种子之后、用户公司之前）
    /// </summary>
    public int Order => 62;

    /// <summary>
    /// 初始化用户-租户关联种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>插入数与更新数</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化用户-租户关联种子数据...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过用户-租户关联种子数据初始化");
            return (0, 0);
        }

        var userRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktUser>>();
        var userTenantRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktUserTenant>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();

        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用公司，跳过用户-租户关联种子", tenantCode);
            return (0, 0);
        }

        int insertCount = 0;
        int updateCount = 0;

        var users = await userRepository.GetListAsync(
            u => u.TenantCode == tenantCode && u.UserStatus == 1);

        foreach (var company in orderedCompanies)
        {
            var plantCode = database.GetPlantCodeForCompanyCode(company.CompanyCode);
            foreach (var user in users)
            {
                var (inserted, updated) = await CreateOrUpdateUserTenantAsync(
                    userTenantRepository,
                    user.Id,
                    tenantCode,
                    company.CompanyCode,
                    plantCode,
                    company.CultureCode,
                    isDefault: true);
                insertCount += inserted;
                updateCount += updated;
            }
        }

        TaktLogger.Information(
            "用户-租户关联种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 创建或更新用户-租户关联
    /// </summary>
    private static async Task<(int Inserted, int Updated)> CreateOrUpdateUserTenantAsync(
        ITaktCompanySeedRepository<TaktUserTenant> userTenantRepository,
        long userId,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        bool isDefault)
    {
        var existing = await userTenantRepository.FirstAsync(ut =>
            ut.UserId == userId
            && ut.TenantCode == tenantCode
            && ut.CompanyCode == companyCode);

        if (existing == null)
        {
            await userTenantRepository.CreateAsync(new TaktUserTenant
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                PlantCode = plantCode,
                CultureCode = cultureCode,
                UserId = userId,
                IsDefault = isDefault ? 1 : 0,
            });
            return (1, 0);
        }

        if (existing.IsDefault != (isDefault ? 1 : 0))
        {
            existing.IsDefault = isDefault ? 1 : 0;
            await userTenantRepository.UpdateAsync(existing);
            return (0, 1);
        }

        return (0, 0);
    }
}
