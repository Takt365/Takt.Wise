// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktOrganizationManagerSeedData.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：公司/工厂负责人回填（admin 用户 Id + UserName 成对写入，幂等）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 组织负责人种子：在用户种子之后，将内置 admin 用户成对写入公司/工厂负责人外键与冗余名称
/// </summary>
public class TaktOrganizationManagerSeedData : ITaktSeedDataCoordinator
{
    private const string DefaultManagerUserName = "admin";

    /// <summary>
    /// 执行顺序（用户种子 Order=15 之后）
    /// </summary>
    public int Order => 66;

    /// <summary>
    /// 回填公司/工厂负责人（CompanyManagerUserId + CompanyManagerUserName / PlantManagerUserId + PlantManagerUserName）
    /// </summary>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化组织负责人种子...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过组织负责人种子");
            return (0, 0);
        }
        var userRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktUser>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var plantRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktPlant>>();
        var managerUser = await userRepository.FirstAsync(u =>
            u.TenantCode == tenantCode && u.UserName == DefaultManagerUserName && u.UserStatus == 1);
        if (managerUser == null)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到用户 {UserName}，跳过组织负责人回填", tenantCode, DefaultManagerUserName);
            return (0, 0);
        }
        var updateCount = 0;
        var companies = await companyRepository.GetListAsync(c => c.TenantCode == tenantCode);
        foreach (var company in companies)
        {
            if (company.CompanyManagerUserId == managerUser.Id
                && string.Equals(company.CompanyManagerUserName, managerUser.UserName, StringComparison.Ordinal))
            {
                continue;
            }
            company.CompanyManagerUserId = managerUser.Id;
            company.CompanyManagerUserName = managerUser.UserName;
            await companyRepository.UpdateAsync(company);
            updateCount++;
        }
        var plants = await plantRepository.GetListAsync(p => p.TenantCode == tenantCode);
        foreach (var plant in plants)
        {
            if (plant.PlantManagerUserId == managerUser.Id
                && string.Equals(plant.PlantManagerUserName, managerUser.UserName, StringComparison.Ordinal))
            {
                continue;
            }
            plant.PlantManagerUserId = managerUser.Id;
            plant.PlantManagerUserName = managerUser.UserName;
            await plantRepository.UpdateAsync(plant);
            updateCount++;
        }
        TaktLogger.Information("组织负责人种子完成: 更新 {UpdateCount} 条", updateCount);
        return (0, updateCount);
    }
}
