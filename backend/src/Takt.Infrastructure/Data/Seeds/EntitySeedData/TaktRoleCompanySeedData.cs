// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktRoleCompanySeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt角色-公司关联种子数据，定义角色可访问的公司范围（数据权限）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 角色-公司关联种子数据初始化
/// <para>所有启用角色均关联 <c>Database:CompanyCodes</c> 顺序下全部启用公司。</para>
/// 幂等性操作：存在则跳过，不存在则创建
/// </summary>
public class TaktRoleCompanySeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在公司种子之后、角色-部门关联之前）
    /// </summary>
    public int Order => 61;

    /// <summary>
    /// 初始化角色-公司关联种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数；本种子仅插入不更新）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化角色-公司关联种子数据...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过角色-公司关联种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktRoleCompany>>();
        var roleRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktRole>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var configuredCompanyCodes = database.CompanyCodes;
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            configuredCompanyCodes,
            companies,
            c => c.CompanyCode);

        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过角色-公司关联种子", tenantCode);
            return (0, 0);
        }

        int insertCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化角色-公司关联数据...", tenantCode);

        var roles = await roleRepository.GetListAsync(r => r.TenantCode == tenantCode && r.RoleStatus == 1);
        foreach (var role in roles)
        {
            foreach (var company in orderedCompanies)
            {
                insertCount += await CreateRoleCompanyAsync(
                    repository,
                    roleRepository,
                    companyRepository,
                    tenantCode,
                    role.RoleCode,
                    company.CompanyCode,
                    database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode);
            }
        }

        TaktLogger.Information("角色-公司关联种子数据初始化完成: 插入 {InsertCount} 条", insertCount);

        return (insertCount, 0);
    }

    /// <summary>
    /// 创建角色-公司关联（幂等：角色或公司不存在、或关联已存在时返回 0）
    /// </summary>
    /// <param name="repository">角色-公司关联仓储</param>
    /// <param name="roleRepository">角色仓储</param>
    /// <param name="companyRepository">公司仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="roleCode">角色编码（如 ROLE_SUPER_ADMIN）</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>新插入记录数为 1，否则为 0</returns>
    private static async Task<int> CreateRoleCompanyAsync(
        ITaktCompanySeedRepository<TaktRoleCompany> repository,
        ITaktTenantSeedRepository<TaktRole> roleRepository,
        ITaktTenantSeedRepository<TaktCompany> companyRepository,
        string tenantCode,
        string roleCode,
        string companyCode,
        string plantCode,
        string cultureCode)
    {
        var role = await roleRepository.FirstAsync(r => r.TenantCode == tenantCode && r.RoleCode == roleCode);
        if (role == null) return 0;

        var company = await companyRepository.FirstAsync(c =>
            c.TenantCode == tenantCode && c.CompanyCode == companyCode);
        if (company == null) return 0;

        var exists = await repository.FirstAsync(rc =>
            rc.TenantCode == tenantCode
            && rc.RoleId == role.Id
            && rc.CompanyCode == companyCode);
        if (exists != null) return 0;

        await repository.CreateAsync(new TaktRoleCompany
        {
            TenantCode = tenantCode,
            RoleId = role.Id,
            CompanyCode = companyCode,
            PlantCode = plantCode,
            CultureCode = cultureCode
        });
        return 1;
    }
}
