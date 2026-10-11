// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktPermissionService.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：权限服务实现，提供数据权限验证能力
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Enums;

namespace Takt.Infrastructure.Services;

/// <summary>
/// ITaktPermissionService 实现
/// 基于用户-公司、用户-角色-公司关联及角色数据范围判断访问权限
/// </summary>
public class TaktPermissionService : ITaktPermissionService
{
    /// <summary>
    /// 租户 SqlSugar 上下文
    /// </summary>
    private readonly TaktSqlSugarContext _dbContext;

    /// <summary>
    /// 当前请求的 SqlSugar 客户端
    /// </summary>
    private ISqlSugarClient Db => _dbContext.Db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    public TaktPermissionService(TaktSqlSugarContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 检查用户是否有权限访问指定公司（直接关联或角色关联）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>是否有权限</returns>
    public async Task<bool> HasCompanyAccessAsync(long userId, string tenantCode, string companyCode)
    {
        var hasDirectAccess = await Db.Queryable<TaktUserCompany>()
            .Where(x => x.UserId == userId)
            .Where(x => x.TenantCode == tenantCode)
            .Where(x => x.CompanyCode == companyCode)
            .Where(x => x.IsDeleted == 0)
            .AnyAsync();

        if (hasDirectAccess)
        {
            return true;
        }

        var hasRoleAccess = await Db.Queryable<TaktUserCompany>()
            .InnerJoin<TaktUserRole>((uc, ur) =>
                uc.UserId == ur.UserId
                && uc.TenantCode == ur.TenantCode
                && uc.CompanyCode == ur.CompanyCode)
            .InnerJoin<TaktRoleCompany>((uc, ur, rc) => ur.RoleId == rc.RoleId)
            .Where((uc, ur, rc) => uc.UserId == userId)
            .Where((uc, ur, rc) => uc.TenantCode == tenantCode)
            .Where((uc, ur, rc) => rc.CompanyCode == companyCode)
            .Where((uc, ur, rc) => uc.IsDeleted == 0 && ur.IsDeleted == 0 && rc.IsDeleted == 0)
            .AnyAsync();

        return hasRoleAccess;
    }

    /// <summary>
    /// 获取用户可访问的公司编码列表（直接关联与角色关联合并去重）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>公司编码列表</returns>
    public async Task<List<string>> GetAccessibleCompaniesAsync(long userId, string tenantCode)
    {
        var companies = new HashSet<string>();

        var directCompanies = await Db.Queryable<TaktUserCompany>()
            .Where(x => x.UserId == userId)
            .Where(x => x.TenantCode == tenantCode)
            .Where(x => x.IsDeleted == 0)
            .Select(x => x.CompanyCode)
            .ToListAsync();

        companies.UnionWith(directCompanies);

        var roleCompanies = await Db.Queryable<TaktUserCompany>()
            .InnerJoin<TaktUserRole>((uc, ur) =>
                uc.UserId == ur.UserId
                && uc.TenantCode == ur.TenantCode
                && uc.CompanyCode == ur.CompanyCode)
            .InnerJoin<TaktRoleCompany>((uc, ur, rc) => ur.RoleId == rc.RoleId)
            .Where((uc, ur, rc) => uc.UserId == userId)
            .Where((uc, ur, rc) => uc.TenantCode == tenantCode)
            .Where((uc, ur, rc) => uc.IsDeleted == 0 && ur.IsDeleted == 0 && rc.IsDeleted == 0)
            .Select((uc, ur, rc) => rc.CompanyCode)
            .ToListAsync();

        companies.UnionWith(roleCompanies);

        if (companies.Count == 0)
        {
            return [];
        }

        var ordered = await Db.Queryable<TaktCompany>()
            .Where(c => c.TenantCode == tenantCode && companies.Contains(c.CompanyCode))
            .Where(c => c.CompanyStatus == 1)
            .OrderBy(c => c.SortOrder)
            .OrderBy(c => c.CompanyCode)
            .Select(c => c.CompanyCode)
            .ToListAsync();

        foreach (var code in companies)
        {
            if (!ordered.Contains(code, StringComparer.OrdinalIgnoreCase))
            {
                ordered.Add(code);
            }
        }

        return ordered;
    }

    /// <summary>
    /// 检查用户是否有指定权限类型（结合数据权限范围判断）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="permissionType">权限类型</param>
    /// <returns>是否有权限</returns>
    public async Task<bool> HasPermissionAsync(long userId, string tenantCode, string companyCode, TaktPermissionType permissionType)
    {
        var hasAccess = await HasCompanyAccessAsync(userId, tenantCode, companyCode);
        if (!hasAccess)
        {
            return false;
        }

        var dataScope = await GetDataScopeAsync(userId, tenantCode, companyCode);

        if (dataScope == 1)
        {
            return true;
        }

        if (dataScope == 2)
        {
            return await HasCompanyAccessAsync(userId, tenantCode, companyCode);
        }

        if (dataScope == 4)
        {
            return permissionType == TaktPermissionType.Menu;
        }

        if (dataScope == 5)
        {
            var hasPermission = await Db.Queryable<TaktUserRole>()
                .InnerJoin<TaktRole>((ur, r) => ur.RoleId == r.Id)
                .InnerJoin<TaktRoleDept>((ur, r, rd) => r.Id == rd.RoleId)
                .Where((ur, r, rd) => ur.UserId == userId)
                .Where((ur, r, rd) => ur.TenantCode == tenantCode)
                .Where((ur, r, rd) => ur.CompanyCode == companyCode)
                .Where((ur, r, rd) => rd.CompanyCode == companyCode)
                .Where((ur, r, rd) => r.DataScope == (int)5)
                .Where((ur, r, rd) => ur.IsDeleted == 0 && r.IsDeleted == 0 && rd.IsDeleted == 0)
                .AnyAsync();

            return hasPermission;
        }

        return false;
    }

    /// <summary>
    /// 获取用户的数据权限范围（取当前公司下所有启用角色的最大 DataScope）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>数据权限范围；无角色时返回 Self</returns>
    public async Task<int> GetDataScopeAsync(long userId, string tenantCode, string companyCode)
    {
        var maxDataScope = await Db.Queryable<TaktUserRole>()
            .InnerJoin<TaktRole>((ur, r) => ur.RoleId == r.Id)
            .Where((ur, r) => ur.UserId == userId)
            .Where((ur, r) => ur.TenantCode == tenantCode)
            .Where((ur, r) => ur.CompanyCode == companyCode)
            .Where((ur, r) => ur.IsDeleted == 0 && r.IsDeleted == 0 && r.RoleStatus == 1)
            .MaxAsync((ur, r) => r.DataScope);

        return maxDataScope > 0 ? maxDataScope : 4;
    }
}
