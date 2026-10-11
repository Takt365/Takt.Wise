// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktEmployeePostSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt员工-岗位关联种子数据，初始化人事岗位关系
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 员工-岗位关联种子数据初始化
/// 幂等性操作：存在则跳过，不存在则创建
/// </summary>
public class TaktEmployeePostSeedData : ITaktSeedDataCoordinator
{
    public int Order => 64;

    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化员工-岗位关联种子数据...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过员工-岗位关联种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktEmployeePost>>();
        var employeeRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktEmployee>>();
        var postRepository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktPost>>();
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

        int insertCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化员工-岗位关联数据...", tenantCode);

        foreach (var company in orderedCompanies)
        {
            foreach (var (EmployeeCode, postCode) in GetEmployeePostTemplates(company))
            {
                insertCount += await CreateEmployeePostAsync(
                    repository,
                    employeeRepository,
                    postRepository,
                    tenantCode,
                    company.CompanyCode,
                    database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                    EmployeeCode,
                    postCode);
            }
        }

        TaktLogger.Information("员工-岗位关联种子数据初始化完成: 插入 {InsertCount} 条", insertCount);

        return (insertCount, 0);
    }

    /// <summary>
    /// 按公司默认文化匹配员工-岗位模板（不硬编码公司编码）
    /// </summary>
    /// <param name="company">公司实体</param>
    /// <returns>员工-岗位模板</returns>
    private static IEnumerable<(string EmployeeCode, string PostCode)> GetEmployeePostTemplates(TaktCompany company)
    {
        if (string.Equals(company.CultureCode, "zh-CN", StringComparison.OrdinalIgnoreCase)
            && string.Equals(company.IndustryAttribute, "C", StringComparison.OrdinalIgnoreCase))
        {
            yield return ("900001", "POST01");
            yield return ("900002", "POST02");
            yield return ("900003", "POST03");
            yield break;
        }
        if (string.Equals(company.CultureCode, "zh-HK", StringComparison.OrdinalIgnoreCase))
        {
            yield return ("900001", "POST04");
            yield return ("900002", "POST05");
            yield break;
        }
        if (string.Equals(company.CultureCode, "ja-JP", StringComparison.OrdinalIgnoreCase))
        {
            yield return ("900001", "POST06");
            yield return ("900002", "POST07");
        }
    }

    private static async Task<int> CreateEmployeePostAsync(
        ITaktCompanySeedRepository<TaktEmployeePost> repository,
        ITaktCompanySeedRepository<TaktEmployee> employeeRepository,
        ITaktCompanySeedRepository<TaktPost> postRepository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        string EmployeeCode,
        string postCode)
    {
        var employee = await employeeRepository.FirstAsync(e =>
            e.TenantCode == tenantCode &&
            e.CompanyCode == companyCode &&
            e.EmployeeCode == EmployeeCode);
        if (employee == null) return 0;

        var post = await postRepository.FirstAsync(p =>
            p.TenantCode == tenantCode &&
            p.CompanyCode == companyCode &&
            p.PostCode == postCode);
        if (post == null) return 0;

        var exists = await repository.FirstAsync(ep =>
            ep.TenantCode == tenantCode &&
            ep.CompanyCode == companyCode &&
            ep.EmployeeId == employee.Id &&
            ep.PostId == post.Id);
        if (exists == null)
        {
            await repository.CreateAsync(new TaktEmployeePost
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                PlantCode = plantCode,
                CultureCode = cultureCode,
                EmployeeId = employee.Id,
                PostId = post.Id
            });
            return 1;
        }
        return 0;
    }
}
