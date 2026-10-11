// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktRoleMenuSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt角色-菜单关联种子数据，按公司隔离初始化 RBAC 菜单权限
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
/// 角色-菜单关联种子数据初始化（每个启用公司各一份）
/// 幂等性操作：存在则跳过，不存在则创建
/// </summary>
public class TaktRoleMenuSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在菜单种子之后，其他关联种子之前）
    /// </summary>
    public int Order => 50;

    /// <summary>
    /// 初始化角色-菜单关联种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化角色-菜单关联种子数据...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过角色-菜单关联种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktRoleMenu>>();
        var roleRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktRole>>();
        var menuRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktMenu>>();
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
            TaktLogger.Warning("租户 {TenantCode} 未找到启用公司，跳过角色-菜单关联种子", tenantCode);
            return (0, 0);
        }

        int insertCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化角色-菜单关联数据...", tenantCode);

        var roleMenus = GetStandardRoleMenus();
        foreach (var company in orderedCompanies)
        {
            var plantCode = database.GetPlantCodeForCompanyCode(company.CompanyCode);
            foreach (var roleMenuData in roleMenus)
            {
                if (roleMenuData.AssignMode == RoleMenuAssignMode.All)
                {
                    insertCount += await AssignAllMenusToRoleAsync(
                        repository, roleRepository, menuRepository,
                        tenantCode, company.CompanyCode, plantCode, company.CultureCode, roleMenuData.RoleCode);
                }
                else if (roleMenuData.AssignMode == RoleMenuAssignMode.ListAndQuery)
                {
                    insertCount += await AssignListAndQueryMenusToRoleAsync(
                        repository, roleRepository, menuRepository,
                        tenantCode, company.CompanyCode, plantCode, company.CultureCode, roleMenuData.RoleCode);
                }
                else
                {
                    insertCount += await AssignMenuByCodesToRoleAsync(
                        repository, roleRepository, menuRepository,
                        tenantCode, company.CompanyCode, plantCode, company.CultureCode,
                        roleMenuData.RoleCode, roleMenuData.MenuCodes!);
                }
            }
        }

        TaktLogger.Information("角色-菜单关联种子数据初始化完成: 插入 {InsertCount} 条", insertCount);
        return (insertCount, 0);
    }

    /// <summary>
    /// 角色菜单分配方式
    /// </summary>
    private enum RoleMenuAssignMode
    {
        /// <summary>按 MenuCode 列表</summary>
        ByCodes = 0,

        /// <summary>全部菜单</summary>
        All = 1,

        /// <summary>目录 + Permission 以 :list / :query 结尾的菜单/按钮</summary>
        ListAndQuery = 2
    }

    /// <summary>
    /// 获取标准角色-菜单关联列表。
    /// ROLE_SUPER_ADMIN：全部菜单。
    /// 各部门 ROLE_*_DEPT_ADMIN / ROLE_EMPLOYEE / ROLE_DEMO：目录 + :list / :query。
    /// </summary>
    private static List<(string RoleCode, RoleMenuAssignMode AssignMode, string[]? MenuCodes)> GetStandardRoleMenus()
    {
        var list = new List<(string, RoleMenuAssignMode, string[]?)>
        {
            ("ROLE_SUPER_ADMIN", RoleMenuAssignMode.All, null),
        };
        foreach (var d in TaktRoleSeedData.DeptAdminRoles)
        {
            list.Add((d.RoleCode, RoleMenuAssignMode.ListAndQuery, null));
        }
        list.Add(("ROLE_EMPLOYEE", RoleMenuAssignMode.ListAndQuery, null));
        list.Add(("ROLE_DEMO", RoleMenuAssignMode.ListAndQuery, null));
        return list;
    }

    /// <summary>
    /// 为角色分配所有菜单（超级管理员专用）
    /// </summary>
    private static async Task<int> AssignAllMenusToRoleAsync(
        ITaktCompanySeedRepository<TaktRoleMenu> repository,
        ITaktTenantSeedRepository<TaktRole> roleRepository,
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        string roleCode)
    {
        var role = await roleRepository.FirstAsync(r => r.TenantCode == tenantCode && r.RoleCode == roleCode);
        if (role == null) return 0;

        var allMenus = await menuRepository.GetListAsync(m => m.TenantCode == tenantCode);
        if (allMenus == null || allMenus.Count == 0) return 0;

        int insertCount = 0;
        foreach (var menu in allMenus)
        {
            insertCount += await CreateRoleMenuAsync(
                repository, tenantCode, companyCode, plantCode, cultureCode, role.Id, menu.Id);
        }
        return insertCount;
    }

    /// <summary>
    /// 为角色分配：目录（MenuType=0）+ Permission 以 :list / :query 结尾的页面与按钮。
    /// 用于各部门 ROLE_*_DEPT_ADMIN / ROLE_EMPLOYEE / ROLE_DEMO。
    /// </summary>
    private static async Task<int> AssignListAndQueryMenusToRoleAsync(
        ITaktCompanySeedRepository<TaktRoleMenu> repository,
        ITaktTenantSeedRepository<TaktRole> roleRepository,
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        string roleCode)
    {
        var role = await roleRepository.FirstAsync(r => r.TenantCode == tenantCode && r.RoleCode == roleCode);
        if (role == null) return 0;

        var menus = await menuRepository.GetListAsync(m => m.TenantCode == tenantCode && m.MenuStatus == 1);
        if (menus == null || menus.Count == 0) return 0;

        int insertCount = 0;
        foreach (var menu in menus)
        {
            var permission = menu.Permission?.Trim() ?? string.Empty;
            var isDirectory = menu.MenuType == 0;
            var isListOrQuery = permission.EndsWith(":list", StringComparison.OrdinalIgnoreCase)
                || permission.EndsWith(":query", StringComparison.OrdinalIgnoreCase);
            if (!isDirectory && !isListOrQuery)
            {
                continue;
            }

            insertCount += await CreateRoleMenuAsync(
                repository, tenantCode, companyCode, plantCode, cultureCode, role.Id, menu.Id);
        }
        return insertCount;
    }

    /// <summary>
    /// 为角色分配指定编码的菜单
    /// </summary>
    private static async Task<int> AssignMenuByCodesToRoleAsync(
        ITaktCompanySeedRepository<TaktRoleMenu> repository,
        ITaktTenantSeedRepository<TaktRole> roleRepository,
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        string roleCode,
        params string[] menuCodes)
    {
        var role = await roleRepository.FirstAsync(r => r.TenantCode == tenantCode && r.RoleCode == roleCode);
        if (role == null) return 0;

        int insertCount = 0;
        foreach (var menuCode in menuCodes)
        {
            var menu = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.MenuCode == menuCode);
            if (menu == null) continue;

            insertCount += await CreateRoleMenuAsync(
                repository, tenantCode, companyCode, plantCode, cultureCode, role.Id, menu.Id);
        }
        return insertCount;
    }

    /// <summary>
    /// 创建角色-菜单关联（幂等：存在则跳过）
    /// </summary>
    private static async Task<int> CreateRoleMenuAsync(
        ITaktCompanySeedRepository<TaktRoleMenu> repository,
        string tenantCode,
        string companyCode,
        string plantCode,
        string cultureCode,
        long roleId,
        long menuId)
    {
        var exists = await repository.FirstAsync(rm =>
            rm.TenantCode == tenantCode &&
            rm.CompanyCode == companyCode &&
            rm.RoleId == roleId &&
            rm.MenuId == menuId);
        if (exists != null) return 0;

        await repository.CreateAsync(new TaktRoleMenu
        {
            TenantCode = tenantCode,
            CompanyCode = companyCode,
            PlantCode = plantCode,
            CultureCode = cultureCode,
            RoleId = roleId,
            MenuId = menuId
        });
        return 1;
    }
}
